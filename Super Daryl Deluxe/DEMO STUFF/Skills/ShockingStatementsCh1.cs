using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    public class ShockingStatementDemo : Skill
    {
        Rectangle lightningRec;
        Rectangle lightningRec2;
        Rectangle lightningRec3;
        Rectangle lightningRec4;

        int lightningTime = 0;
        int lightningTime2 = 0;
        int lightningTime3 = 0;
        int lightningTime4 = 0;

        int lightningFrame = 0;
        int lightningFrame2 = 0;
        int lightningFrame3 = 0;
        int lightningFrame4 = 0;

        int lightningAmount;

        int chanceToStun = 5;
        int stunTime = 120;

        Random randomLightningIndex;

        int dischargeTime, dischargeFrame;

        public ShockingStatementDemo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .35f;
            experience = 0;
            experienceUntilLevel = 300;
            skillRank = 1;
            levelToUse = 3;
            name = "Shocking Statements CH.1";
            canUse = true;
            description = "Call death down from above, in the form of puny little lightning bolts. \nChance to stun enemies: 5%";
            fullCooldown = 50;
            //--Animation and skill attributes
            animationLength = 0;
            lightningAmount = 1;
            costToBuy = 1;
            hitPauseTime = 1;

            skillBarColor = new Color(0, 255, 255);

            skillType = AttackType.AttackTypes.Lightning;
            rangedOrMelee = AttackType.RangedOrMelee.Ranged;

            transformLevels = new int[3] { 3, 7, 12};

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(6);

            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(11);
            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(19);

            playerLevelRequiredToLevel.Add(23);
            playerLevelRequiredToLevel.Add(27);
            playerLevelRequiredToLevel.Add(34);
            playerLevelRequiredToLevel.Add(38);
            playerLevelRequiredToLevel.Add(41);

            randomLightningIndex = new Random();
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
                    damage = .36f;
                    lightningAmount = 2;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 860;
                    fullCooldown = 50;
                    break;
                case 7:
                    damage = .37f;
                    lightningAmount = 3;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 2300;
                    fullCooldown = 50;
                    break;
                case 12:
                    damage = .38f;
                    lightningAmount = 4;
                    fullCooldown = 50;
                    chanceToStun = 5;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    stunTime = 150;
                    break;
                case 13:
                    damage = .39f;
                    lightningAmount = 4;
                    fullCooldown = 50;
                    chanceToStun = 5;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    stunTime = 150;
                    break;
                case 14:
                    damage = .4f;
                    lightningAmount = 4;
                    fullCooldown = 50;
                    chanceToStun = 5;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    stunTime = 150;
                    break;
                case 15:
                    damage = .41f;
                    lightningAmount = 4;
                    experience = 0;
                    chanceToStun = 5;
                    fullCooldown = 60;
                    stunTime = 180;
                    break;
            }

            description = "Call death down from above, in the form of puny little lightning bolts. \nChance to stun enemies: " + chanceToStun + "% | Stun Time: " + stunTime;

        }


        public override Rectangle GetSourceRec()
        {
            return new Rectangle(530 * moveFrame, 0, 530, 398);
        }

        public Rectangle GetDischargeSourceRec()
        {
            return new Rectangle(530 * dischargeFrame, 398, 530, 398);
        }

        public Rectangle GetLightningSourceRec(int lightningIndex, int lightningNum)
        {
            if(lightningNum == 1)
                return new Rectangle(240 * lightningFrame, 796, 240, 323);
            if (lightningNum == 2)
                return new Rectangle(240 * lightningFrame2, 1120, 240, 323);
            if (lightningNum == 4)
                return new Rectangle(960 + (240 * lightningFrame4), 796, 240, 323);
            else
                return new Rectangle(240 * lightningFrame3, 1444, 240, 323);
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
                lightningTime = 15;
                PlayRandomUseSound();
            }
        }

        public override void Update()
        {
            base.Update();

            if (animationLength > 18)
                moveFrame = 0;
            else if (animationLength > 16)
                moveFrame = 1;
            else if (animationLength > 14)
                moveFrame = 2;
            else if (animationLength > 11)
                moveFrame = 3;
            else if (animationLength > 6)
                moveFrame = 4;
            else
                moveFrame = 5;

            //Decrease lightning times
            if (lightningTime > 0)
                lightningTime--;
            if (lightningTime2 > 0)
                lightningTime2--;
            if (lightningTime3 > 0)
                lightningTime3--;
            if (lightningTime4 > 0)
                lightningTime4--;

            //lightning 1
            if (lightningTime > 9)
                lightningFrame = 0;
            else if (lightningTime > 6)
                lightningFrame = 1;
            else if (lightningTime > 3)
                lightningFrame = 2;
            else
                lightningFrame = 3;

            //lightning 2
            if (lightningTime2 > 9)
                lightningFrame2 = 0;
            else if (lightningTime2 > 6)
                lightningFrame2 = 1;
            else if (lightningTime2 > 3)
                lightningFrame2 = 2;
            else
                lightningFrame2 = 3;

            //lightning 3
            if (lightningTime3 > 9)
                lightningFrame3 = 0;
            else if (lightningTime3 > 6)
                lightningFrame3 = 1;
            else if (lightningTime3 > 3)
                lightningFrame3 = 2;
            else
                lightningFrame3 = 3;

            //lightning 4
            if (lightningTime4 > 9)
                lightningFrame4 = 0;
            else if (lightningFrame4 > 6)
                lightningFrame4 = 1;
            else if (lightningFrame4 > 3)
                lightningFrame4 = 2;
            else
                lightningFrame4 = 3;


            //discharge of electricity after the skill
            if (dischargeTime > 0)
                dischargeTime--;

            if (dischargeTime > 6)
                dischargeFrame = 0;
            else if (dischargeTime > 3)
                dischargeFrame = 1;
            else
                dischargeFrame = 2;

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                dischargeTime = 9;
            }

            if (lightningTime == 5)
            {
                lightningTime2 = 15;

                if(lightningAmount > 1)
                    PlayRandomUseSound();
            }
            if (lightningTime2 == 5)
            {
                lightningTime3 = 15;

                if (lightningAmount > 2)
                    PlayRandomUseSound();

            }
            if (lightningTime3 == 5)
            {
                lightningTime4 = 15;

                if (lightningAmount > 3)
                    PlayRandomUseSound();
            }

            //--Check to see if an enemy is getting hit
            if (lightningTime == 7 || lightningTime == 3)
            {
                CheckCollisions(lightningRec, damage, new Vector2(5, -5), 0, 0);

                int stun = Game1.randomNumberGen.Next(1, 101);

                if (stun <= chanceToStun)
                {
                    StunEnemy(lightningRec, stunTime);
                }
            }
            //--Check to see if an enemy is getting hit
            if ((lightningTime2 == 7 || lightningTime2 == 3) && lightningAmount > 1)
            {
                CheckCollisions(lightningRec2, damage, new Vector2(5, -5), 0, 0);

                int stun = Game1.randomNumberGen.Next(1, 101);

                if (stun <= chanceToStun)
                {
                    StunEnemy(lightningRec2, stunTime);
                }
            }
            //--Check to see if an enemy is getting hit
            if ((lightningTime3 == 7 || lightningTime3 == 3) && lightningAmount > 2)
            {
                CheckCollisions(lightningRec3, damage, new Vector2(5, -5), 0, 0);

                int stun = Game1.randomNumberGen.Next(1, 101);

                if (stun <= chanceToStun)
                {
                    StunEnemy(lightningRec3, stunTime);
                }
            }

            //--Check to see if an enemy is getting hit
            if ((lightningTime4 == 7 || lightningTime4 == 3) && lightningAmount > 3)
            {
                CheckCollisions(lightningRec4, damage, new Vector2(5, -5), 0, 0);

                int stun = Game1.randomNumberGen.Next(1, 101);

                if (stun <= chanceToStun)
                {
                    StunEnemy(lightningRec3, stunTime);
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
                //--Only move the rectangles if they aren't in use
                if (lightningTime <= 0)
                {
                    lightningRec = new Rectangle(player.VitalRecX + player.VitalRecWidth + 250, player.VitalRecY - 100,
                        100, 323);
                }

                if (lightningTime2 <= 0)
                {
                    lightningRec2 = new Rectangle(player.VitalRecX + player.VitalRecWidth + 100, player.VitalRecY - 100,
        100, 323);
                }

                if (lightningTime3 <= 0)
                {
                    lightningRec3 = new Rectangle(player.VitalRecX + player.VitalRecWidth + 310, player.VitalRecY - 100,
    100, 323);
                }

                if (lightningTime4 <= 0)
                {
                    lightningRec4 = new Rectangle(player.VitalRecX + player.VitalRecWidth + 160, player.VitalRecY - 100,
    100, 323);
                }
            }
            else
            {
                if (lightningTime <= 0)
                {
                    lightningRec = new Rectangle(player.VitalRecX - 350, player.VitalRecY - 100,
                        100, player.VitalRecHeight + 100);
                }

                if (lightningTime2 <= 0)
                {
                    lightningRec2 = new Rectangle(player.VitalRecX - 200, player.VitalRecY - 100,
        100, player.VitalRecHeight + 100);
                }

                if (lightningTime3 <= 0)
                {
                    lightningRec3 = new Rectangle(player.VitalRecX - 410, player.VitalRecY - 100,
        100, player.VitalRecHeight + 100);
                }

                if (lightningTime4 <= 0)
                {
                    lightningRec4 = new Rectangle(player.VitalRecX - 260, player.VitalRecY - 100,
        100, player.VitalRecHeight + 100);
                }
            }
            #endregion
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

            if (dischargeTime > 0)
            {
                if (player.FacingRight)
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetDischargeSourceRec(), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetDischargeSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }

            //--Always draw the lightning if it is in use
            if (lightningTime > 0)
                s.Draw(Game1.skillAnimations[name], new Rectangle(lightningRec.X - 60, lightningRec.Y, 240, 323) , GetLightningSourceRec(1, 1), Color.White);
            if (lightningTime2 > 0 && lightningAmount > 1)
                s.Draw(Game1.skillAnimations[name], new Rectangle(lightningRec2.X - 60, lightningRec2.Y, 240, 323), GetLightningSourceRec(2, 2), Color.White);
            if (lightningTime3 > 0 && lightningAmount > 2)
                s.Draw(Game1.skillAnimations[name], new Rectangle(lightningRec3.X - 60, lightningRec3.Y, 240, 323), GetLightningSourceRec(3, 3), Color.White);
            if (lightningTime4 > 0 && lightningAmount > 3)
                s.Draw(Game1.skillAnimations[name], new Rectangle(lightningRec4.X - 60, lightningRec4.Y, 240, 323), GetLightningSourceRec(4, 4), Color.White);
        }
    }
}
