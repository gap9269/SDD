using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
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
            damage = .55f;
            experience = 0;
            experienceUntilLevel = 100;// 400;
            skillRank = 1;
            levelToUse = 3;
            name = "Quick Retort";
            canUse = true;
            description = "Daryl charges forward and strikes the first enemy he sees. \nDistance: " + maxAnimationLength * 10;
            fullCooldown = 120;
            //--Animation and skill attributes
            animationLength = 0;
            hitPauseTime = 2;
            costToBuy = 1;

            skillBarColor = new Color(255, 0, 83);

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            transformLevels = new int[0] { };

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

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .56f;
                    experience = 0;
                    experienceUntilLevel = 250;
                    maxAnimationLength = 7;
                    break;
                case 3:
                    damage = .57f;
                    experience = 0;
                    experienceUntilLevel = 350;
                    maxAnimationLength = 8;
                    break;
                case 4:
                    damage = .59f;
                    experience = 0;
                    experienceUntilLevel = 600;
                    maxAnimationLength = 8;
                    break;
                case 5:
                    damage = .6f;
                    experience = 0;
                    experienceUntilLevel = 700;
                    maxAnimationLength = 9;
                    break;
                case 6:
                    damage = .61f;
                    experience = 0;
                    experienceUntilLevel = 2200;
                    maxAnimationLength = 9;
                    break;
                case 7:
                    damage = .62f;
                    experience = 0;
                    experienceUntilLevel = 2900;
                    maxAnimationLength = 10;
                    break;
                case 8:
                    damage = .63f;
                    experience = 0;
                    experienceUntilLevel = 3900;
                    maxAnimationLength = 10;
                    break;
                case 9:
                    damage = .64f;
                    experience = 0;
                    experienceUntilLevel = 4500;
                    maxAnimationLength = 11;
                    break;
                case 10:
                    damage = .65f;
                    experienceUntilLevel = 6000;
                    experience = 0;
                    maxAnimationLength = 12;
                    fullCooldown = 100;
                    break;
                case 11:
                    damage = .67f;
                    experienceUntilLevel = 8000;
                    experience = 0;
                    maxAnimationLength = 13;
                    fullCooldown = 100;
                    break;
                case 12:
                    damage = .69f;
                    experienceUntilLevel = 11000;
                    experience = 0;
                    maxAnimationLength = 14;
                    fullCooldown = 100;
                    break;
                case 13:
                    damage = .71f;
                    experienceUntilLevel = 14000;
                    experience = 0;
                    maxAnimationLength = 15;
                    fullCooldown = 100;
                    break;
                case 14:
                    damage = .73f;
                    experienceUntilLevel = 18000;
                    experience = 0;
                    maxAnimationLength = 16;
                    fullCooldown = 100;
                    break;
                case 15:
                    damage = .75f;
                    experience = 0;
                    maxAnimationLength = 17;
                    fullCooldown = 100;
                    break;
            }

            description = "Daryl charges forward and strikes the first enemy he sees. \nDistance: " + maxAnimationLength * 10;

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
                PlayRandomUseSound();
            }
        }



        public override void Update()
        {
            base.Update();

            switch (skillRank)
            {
                case 1:
                case 2:
                case 3:
                case 4:
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
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
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
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
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
                case 15:
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
                    CheckCollisions(checkPlatRec, damage, new Vector2(15, -6), 3 + (skillRank / 3), 3 +(skillRank / 3));
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
                    player.VitalRecHeight + 35);
            }
            else
            {
                hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 200, player.VitalRecY - 30,
                    200, player.VitalRecHeight);

                checkPlatRec = new Rectangle(player.VitalRecX - 75, player.VitalRecY, 75,
                    player.VitalRecHeight + 35);
            }
            #endregion


            //Check to see if you run into a platform or enemy or object
            if (animationLength > 0 && !attacking)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (checkPlatRec.Intersects(platforms[i].Rec) && platforms[i].Passable == false)
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
                    if (checkPlatRec.Intersects(interactiveObjectsInMap[i].VitalRec) && interactiveObjectsInMap[i].Finished == false && interactiveObjectsInMap[i].canBeHit && !interactiveObjectsInMap[i].IsHidden)
                    {
                        animationLength = 5;
                        player.VelocityX = 0;
                        attacking = true;
                        impactRec = player.Rec;
                        impactTime = 8;
                        break;
                    }
                }

                if (game.CurrentChapter.BossFight && checkPlatRec.Intersects(currentBoss.VitalRec) && currentBoss.CanBeHurt)
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

    }
}

