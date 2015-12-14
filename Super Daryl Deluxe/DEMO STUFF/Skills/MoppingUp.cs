using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class MoppingUpDemo : Skill
    {

        Rectangle attackRec;
        Rectangle launchRec;
        Rectangle topRec;

        Rectangle groundRec;

        public MoppingUpDemo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .4f;
            experience = 0;
            experienceUntilLevel = 250;
            skillRank = 1;
            levelToUse = 3;
            name = "Mopping Up";
            canUse = true;
            description = "Harness the essence of your inner janitor and launch your problems into the sky.";
            fullCooldown = 150;
            //--Animation and skill attributes
            hitPauseTime = 2;
            maxHit = 3;
            canUseInAir = false;

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            skillBarColor = new Color(150, 224, 0);

            transformLevels = new int[0] { };

            costToBuy = 1;

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(5);
            playerLevelRequiredToLevel.Add(6);

            playerLevelRequiredToLevel.Add(7);
            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(9);
            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(10);

            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(15);
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 12;
                useKey = key;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();
                player.VelocityY = -33;
                groundRec = player.Rec;
                skillBarColor = new Color(150, 224, 0);
                PlayRandomUseSound();
            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();

            
            if (animationLength > 10)
                moveFrame = 0;
            else if (animationLength > 8)
                moveFrame = 1;
            else if (animationLength > 5)
                moveFrame = 2;
            else if (animationLength > 2)
                moveFrame = 3;
            else
                moveFrame = 4;

            
            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.VelocityY += 1.5f;
            }
            hitPauseTime = 2;

            if (animationLength == 9 || animationLength == 8)//|| animationLength == 5)
            {
                CheckFiniteCollisions(launchRec, damage, new Vector2(0, -23), 2, 2);
                CheckFiniteCollisions(attackRec, damage, new Vector2(0, -10), 2, 2);
            }
            else if (animationLength < 8 && animationLength > 2)
            {
                CheckFiniteCollisions(attackRec, damage, new Vector2(0, -10), 2, 2);
            }

            if (animationLength == 1)
            {
                //--Reset amount hit for the next rectangle
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();
            }


            #region Set rectangles
            if (player.FacingRight)
            {
                attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY - 150,
                    200, player.VitalRecHeight + 50);
                launchRec = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, attackRec.Y + attackRec.Height,
                    200, 100);
                topRec = new Rectangle(player.Rec.X, player.RecY - 103, 530, 104);
            }
            else
            {
                attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 300, player.VitalRecY - 150,
    200, player.VitalRecHeight + 50);
                launchRec = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 300, attackRec.Y + attackRec.Height,
                    200, 100);
                topRec = new Rectangle(player.Rec.X, player.RecY - 103, 530, 104);
            }

            #endregion

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                player.AttackFalling = true;
                player.playerState = Player.PlayerState.jumping;
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }
        }

        public override Rectangle GetSourceRec()
        {
            if(moveFrame == 0)
                return new Rectangle(1060, 398, 530, 398);
            else if (moveFrame < 4)
                return new Rectangle((530 * moveFrame) + 1060, 398, 530, 398);
            else
                return new Rectangle(2650, 398, 530, 398);
        }

        public Rectangle GetGroundSourceRec()
        {
            return new Rectangle(530 * moveFrame, 796, 530, 398);
        }

        public Rectangle GetTopSourceRec()
        {
            if (moveFrame == 1)
                return new Rectangle(3180, 398, 530, 103);
            else if (moveFrame == 2)
                return new Rectangle(2650, 796, 530, 103);
            else if (moveFrame == 3 || moveFrame == 4)
                return new Rectangle(3180, 796, 530, 103);

            return new Rectangle();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (animationLength > 0)
            {
                if (player.FacingRight)
                {
                    s.Draw(game.SkillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                    s.Draw(game.SkillAnimations[name], groundRec, GetGroundSourceRec(), Color.White);

                    if(moveFrame > 0 && moveFrame < 5)
                    s.Draw(game.SkillAnimations[name], topRec, GetTopSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(game.SkillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    s.Draw(game.SkillAnimations[name], groundRec, GetGroundSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (moveFrame > 0 && moveFrame < 5)
                    s.Draw(game.SkillAnimations[name], topRec, GetTopSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                }
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            if (skillRank == 2)
                skillRank = 3;
            else if (skillRank == 4)
                skillRank = 7;
            else if (skillRank == 8)
                skillRank = 12;

            switch (skillRank)
            {

                case 3:
                    damage = .57f;
                    experience = 0;
                    maxHit = 3;
                    experienceUntilLevel = 750;
                    break;

                case 7:
                    damage = .62f;
                    experience = 0;
                    maxHit = 4;
                    fullCooldown = 120;
                    experienceUntilLevel = 1700;
                    break;
                
                case 12:
                    damage = .69f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    fullCooldown = 110;
                    maxHit = 5;
                    break;
                case 13:
                    damage = .71f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    fullCooldown = 100;
                    maxHit = 5;
                    break;
                case 14:
                    damage = .73f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    maxHit = 6;
                    fullCooldown = 100;
                    break;
                case 15:
                    damage = .75f;
                    experience = 0;
                    fullCooldown = 100;
                    maxHit = 6;
                    break;
            }

            description = "Harness the essence of your inner janitor and launch your problems into the sky. \nEnemies hit: " + maxHit;

        }
    }
}