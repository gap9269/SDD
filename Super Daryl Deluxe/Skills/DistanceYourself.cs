using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class DistanceYourself : Skill
    {

        Rectangle attackRec;

        public DistanceYourself(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .5f;
            experience = 0;
            experienceUntilLevel = 15;
            skillRank = 1;
            levelToUse = 1;
            name = "Distance Yourself";
            canUse = true;
            description = "Daryl slashes in front of him, \nknocking three enemies away.";
            fullCooldown = 100;
            //--Animation and skill attributes
            maxHit = 3;
            hitPauseTime = 2;
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(moveFrame * 530, 0, 530, 398);
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 20;
                useKey = key;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            if (animationLength > 12)
                moveFrame = 0;
            else if (animationLength > 8)
                moveFrame = 1;
            else if (animationLength > 6)
                moveFrame = 2;
            else if (animationLength > 4)
                moveFrame = 3;
            else
                moveFrame = 4;

                if (animationLength > 6 && animationLength < 15)
                {
                    //--Check to see if an enemy is getting hit on every 5th frame
                    CheckFiniteCollisions(attackRec, damage, new Vector2(35, -5), 2, 2);
                }

            #region Set rectangles
            if (player.FacingRight)
            {
                attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY,
                    350, player.VitalRecHeight);
            }
            else
            {
                attackRec = new Rectangle(player.VitalRecX - 350, player.VitalRecY,
                    350, player.VitalRecHeight);
            }
            #endregion

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }
        }

        

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                if (player.FacingRight)
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .6f;
                    experience = 0;
                    experienceUntilLevel = 300;
                    description = "Daryl slashes in front of him, \nknocking four enemies away.";
                    maxHit = 4;
                    break;
                case 3:
                    damage = .7f;
                    experienceUntilLevel = 7000;
                    experience = 0;
                    fullCooldown = 90;
                    break;
                case 4:
                    damage = .8f;
                    experience = 0;
                    //name = "Lightning pun";
                    description = "Daryl slashes in front of him, \nknocking six enemies away.";
                    maxHit = 6;
                    break;
            }
        }
    }
}
