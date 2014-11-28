using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    public class ShockingStatement : Skill
    {
        Rectangle lightningRec;
        Rectangle lightningRec2;
        Rectangle lightningRec3;
        int lightningTime = 0;
        int lightningTime2 = 0;
        int lightningTime3 = 0;

        int lightningFrame = 0;
        int lightningFrame2 = 0;
        int lightningFrame3 = 0;

        int lightningAmount;

        Random randomLightningIndex;

        int dischargeTime, dischargeFrame;

        public ShockingStatement(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .25f;
            experience = 0;
            experienceUntilLevel = 500;
            skillRank = 1;
            levelToUse = 3;
            name = "Shocking Statement";
            canUse = true;
            description = "Daryl summons lightning a short distance in front of him";
            fullCooldown = 60;
            //--Animation and skill attributes
            animationLength = 0;
            lightningAmount = 1;
            costToBuy = 1;
            hitPauseTime = 1;

            skillBarColor = new Color(0, 255, 255);

            skillType = AttackType.AttackTypes.Lightning;
            rangedOrMelee = AttackType.RangedOrMelee.Ranged;

            playerLevelRequiredToLevel.Add(14);
            playerLevelRequiredToLevel.Add(14);
            playerLevelRequiredToLevel.Add(15);

            randomLightningIndex = new Random();
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

            //--Check to see if an enemy is getting hit
            if (lightningTime == 7 || lightningTime == 3)
            {
                CheckCollisions(lightningRec, damage, new Vector2(5, -5), 0, 0);
            }
            //--Check to see if an enemy is getting hit
            if ((lightningTime2 == 7 || lightningTime2 == 3) && lightningAmount > 1)
            {
                CheckCollisions(lightningRec2, damage, new Vector2(5, -5), 0, 0);
            }
            //--Check to see if an enemy is getting hit
            if ((lightningTime3 == 7 || lightningTime3 == 3) && lightningAmount > 2)
            {
                CheckCollisions(lightningRec3, damage, new Vector2(5, -5), 0, 0);
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
        }

        public override void ApplyLevelUp()
        {
            base.ApplyLevelUp();

            switch (skillRank)
            {
                case 2:
                    damage = .3f;
                    lightningAmount = 2;
                    experience = 0;
                    experienceUntilLevel = 700;
                    fullCooldown = 50;
                    break;
                case 3:
                    damage = .3f;
                    lightningAmount = 3;
                    fullCooldown = 50;
                    experienceUntilLevel = 1200;
                    experience = 0;
                    break;
                case 4:
                    damage = .35f;
                    lightningAmount = 3;
                    experience = 0;
                    //name = "Lightning pun";
                    fullCooldown = 60;
                    break;
            }
        }
    }
}
