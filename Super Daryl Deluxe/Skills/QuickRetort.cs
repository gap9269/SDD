using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class QuickRetort : Skill
    {

        Rectangle checkPlatRec;
        Rectangle hitBox;
        Boolean attacking = false;

        int maxAnimationLength = 6;
        int impactFrame;
        int impactTime;

        Rectangle impactRec;
        Boolean impactFacingRight;

        public QuickRetort(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .4f;
            experience = 0;
            experienceUntilLevel = 75;
            skillRank = 1;
            levelToUse = 3;
            name = "Quick Retort";
            canUse = true;
            description = "Daryl charges forward and strikes the first enemy he sees.";
            fullCooldown = 120;
            //--Animation and skill attributes
            animationLength = 0;
            hitPauseTime = 2;
            costToBuy = 1;

            skillBarColor = new Color(255, 0, 83);

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);

        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(530 * moveFrame, 0, 530, 398);
        }

        public Rectangle GetImpactSourceRec()
        {
            return new Rectangle(530 * impactFrame, 398, 530, 398);
        }


        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = maxAnimationLength;
                useKey = key;
                Sound.skillSoundEffects["QuickRetortUse1"].CreateInstance().Play();
            }
        }



        public override void Update()
        {
            base.Update();

            switch (skillRank)
            {
                case 1:
                    if (animationLength == 5)
                        moveFrame = 0;
                    else if (animationLength == 4)
                        moveFrame = 1;
                    else if (animationLength == 3)
                        moveFrame = 2;
                    else if (animationLength == 2)
                        moveFrame = 3;
                    else if (animationLength == 1)
                        moveFrame = 4;
                    else if (animationLength == 0)
                        moveFrame = 5;
                    break;
                case 2:
                    if (animationLength >= 5)
                        moveFrame = 0;
                    else if (animationLength == 4)
                        moveFrame = 1;
                    else if (animationLength == 3)
                        moveFrame = 2;
                    else if (animationLength == 2)
                        moveFrame = 3;
                    else if (animationLength == 1)
                        moveFrame = 4;
                    else if (animationLength == 0)
                        moveFrame = 5;
                    break;
                case 3:
                    if (animationLength > 9)
                        moveFrame = 0;
                    else if (animationLength > 7)
                        moveFrame = 1;
                    else if (animationLength > 5)
                        moveFrame = 2;
                    else if (animationLength > 3)
                        moveFrame = 3;
                    else if (animationLength == 2)
                        moveFrame = 4;
                    else
                        moveFrame = 5;
                    break;
                case 4:
                    if (animationLength > 13)
                        moveFrame = 0;
                    else if (animationLength > 10)
                        moveFrame = 1;
                    else if (animationLength > 7)
                        moveFrame = 2;
                    else if (animationLength > 5)
                        moveFrame = 3;
                    else if (animationLength > 3)
                        moveFrame = 4;
                    else
                        moveFrame = 5;
                    break;
            }


            if (animationLength > 0 && !attacking)
            {
                if (player.FacingRight)
                {
                    player.VelocityX = 40;
                }
                else
                    player.VelocityX = -40;
            }

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                player.VelocityX = 0;
                attacking = false;
            }
            

            if(impactTime > 0)
                impactTime--;

            if (impactTime > 6)
                impactFrame = 0;
            else if (impactTime > 5)
                impactFrame = 1;
            else if (impactTime > 3)
                impactFrame = 2;
            else if (impactTime > 1)
                impactFrame = 3;
            else
                impactFrame = 4;

            if (attacking)
            {
                //--Check to see if an enemy is getting hit, then stun them
                if (animationLength == 4)
                {
                    CheckCollisions(hitBox, damage, new Vector2(15, -6), 3, 3);
                }

            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            #region Set rectangles
            if (player.FacingRight == true)
            {
                hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY - 30,
                    200, player.VitalRecHeight);

                checkPlatRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY, 75,
                    player.VitalRecHeight - 40);
            }
            else
            {
                hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 200, player.VitalRecY - 30,
                    200, player.VitalRecHeight);

                checkPlatRec = new Rectangle(player.VitalRecX - 75, player.VitalRecY, 75,
                    player.VitalRecHeight - 40);
            }
            #endregion


            //Check to see if you run into a platform or enemy or object
            if (animationLength > 0 && !attacking)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (checkPlatRec.Intersects(platforms[i].Rec))
                    {
                        player.VelocityX = 0;
                        animationLength = 0;
                    }
                }

                for (int i = 0; i < enemies.Count; i++)
                {
                    if (checkPlatRec.Intersects(enemies[i].VitalRec))
                    {
                        animationLength = 5;
                        player.VelocityX = 0;
                        attacking = true;
                        impactTime = 8;
                        impactRec = player.Rec;
                        impactFacingRight = player.FacingRight;
                        break;
                    }
                }

                for (int i = 0; i < interactiveObjectsInMap.Count; i++)
                {
                    if (checkPlatRec.Intersects(interactiveObjectsInMap[i].Rec) && interactiveObjectsInMap[i].Finished == false)
                    {
                        animationLength = 5;
                        player.VelocityX = 0;
                        attacking = true;
                        impactRec = player.Rec;
                        impactTime = 8;
                        break;
                    }
                }

                if (game.CurrentChapter.BossFight && checkPlatRec.Intersects(currentBoss.VitalRec))
                {
                    animationLength = 5;
                    player.VelocityX = 0;
                    attacking = true;
                }
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

            if ( impactTime > 0)
            {
                if (impactFacingRight)
                    s.Draw(Game1.skillAnimations[name], impactRec, GetImpactSourceRec(), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], impactRec, GetImpactSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public override void ApplyLevelUp()
        {
            base.ApplyLevelUp();

            switch (skillRank)
            {
                case 2:
                    damage = .45f;
                    experience = 0;
                    experienceUntilLevel = 300;
                    maxAnimationLength = 9;
                    break;
                case 3:
                    damage = .5f;
                    experienceUntilLevel = 400;
                    experience = 0;
                    maxAnimationLength = 12;
                    fullCooldown = 100;
                    break;
                case 4:
                    damage = .6f;
                    experience = 0;
                    maxAnimationLength = 16;
                    break;
            }
        }
    }
}

