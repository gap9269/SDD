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
    public class ShockingStatementsCh3 : Skill
    {
        List<SkillProjectile> shots;
        int ammo = 1;
        int maxAmmo = 1;
        Boolean reloading = false;
        int phase = 0;
        Rectangle shot1;
        int poseType = 0;
        int chanceToStun = 3;
        int stunTime = 120;
        public ShockingStatementsCh3(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .35f;
            experience = 0;
            experienceUntilLevel = 800;
            skillRank = 1;
            levelToUse = 10;
            name = "Shocking Statements CH.3";
            canUse = true;
            description = "Use your body as a lightning rod to deflect electricity in front of you and onto your enemies! /nChance to stun enemies: " + chanceToStun + "%";
            fullCooldown = 90;
            //--Animation and skill attributes
            animationLength = 0;
            hitPauseTime = 1;
            shots = new List<SkillProjectile>();
            skillBarColor = new Color(52, 248, 244);

            costToBuy = 3;

            transformLevels = new int[3] { 3, 6, 9 };

            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(13);

            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(14);
            playerLevelRequiredToLevel.Add(16);
            playerLevelRequiredToLevel.Add(18);
            playerLevelRequiredToLevel.Add(20);

            playerLevelRequiredToLevel.Add(24);
            playerLevelRequiredToLevel.Add(29);
            playerLevelRequiredToLevel.Add(36);
            playerLevelRequiredToLevel.Add(40);
            playerLevelRequiredToLevel.Add(43);

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Ranged;
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .35f;
                    experience = 0;
                    chanceToStun = 3;
                    experienceUntilLevel = 1200;
                    break;
                case 3:
                    damage = .36f;
                    experience = 0;
                    chanceToStun = 3;
                    experienceUntilLevel = 1600;
                    fullCooldown = 90;
                    break;
                case 4:
                    damage = .36f;
                    experience = 0;
                    chanceToStun = 4;
                    experienceUntilLevel = 2500;
                    fullCooldown = 90;
                    break;
                case 5:
                    damage = .37f;
                    experience = 0;
                    chanceToStun = 4;
                    experienceUntilLevel = 3100;
                    fullCooldown = 90;
                    break;
                case 6:
                    damage = .38f;
                    experience = 0;
                    chanceToStun = 4;
                    experienceUntilLevel = 4100;
                    fullCooldown = 90;
                    break;
                case 7:
                    damage = .38f;
                    chanceToStun = 5;
                    experienceUntilLevel = 6200;
                    fullCooldown = 90;
                    experience = 0;
                    break;
                case 8:
                    damage = .39f;
                    chanceToStun = 5;
                    experienceUntilLevel = 8100;
                    fullCooldown = 90;
                    experience = 0;
                    break;
                case 9:
                    damage = .41f;
                    chanceToStun = 5;
                    experienceUntilLevel = 10000;
                    fullCooldown = 90;
                    experience = 0;
                    break;
                case 10:
                    damage = .36f;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 90;
                    experience = 0;
                    break;
                case 11:
                    damage = .37f;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 90;
                    experience = 0;
                    break;
                case 12:
                    damage = .38f;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 80;
                    break;
                case 13:
                    damage = .39f;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 80;
                    break;
                case 14:
                    damage = .4f;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 80;
                    break;
                case 15:
                    damage = .43f;
                    experience = 0;
                    chanceToStun = 5;
                    experienceUntilLevel = 4200;
                    fullCooldown = 80;
                    break;
            }

            description = "Use your body as a lightning rod to deflect electricity in front of you and onto your enemies! /nChance to stun enemies: " + chanceToStun + "%";
        }

        public override Rectangle GetSourceRec()
        {
            int tempFrame = moveFrame;

            if (skillRank < 7)
            {
                if (moveFrame > 2)
                    tempFrame = 2;
            }
            else if (skillRank >= 12)
            {
                if (moveFrame > 5)
                    tempFrame = 5;
            }

            switch (skillRank)
            {
                case 1:
                case 2:
                    if (poseType == 0)
                        return new Rectangle(530 * tempFrame, 0, 530, 398);
                    else
                        return new Rectangle((530 * tempFrame) + 1590, 0, 530, 398);
                case 3:
                case 4:
                case 5:
                case 6:
                    if (poseType == 0)
                        return new Rectangle(530 * tempFrame, 796, 530, 398);
                    else
                        return new Rectangle((530 * tempFrame) + 1590, 796, 530, 398);
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (poseType == 0)
                        return new Rectangle(530 * tempFrame, 1592, 530, 398);
                    else
                        return new Rectangle((530 * tempFrame), 1990, 530, 398);
                case 12:
                case 13:
                case 14:
                case 15:
                    return new Rectangle(530 * tempFrame, 2786, 530, 398);
            }

            return new Rectangle();
        }

        public Rectangle GetLightningSourceRec()
        {
            int tempFrame = moveFrame;

            if (skillRank < 3)
            {
                if (moveFrame > 3)
                    tempFrame = 3;
            }

            switch (skillRank)
            {
                case 1:
                case 2:
                    return new Rectangle(981 * tempFrame, 398, 981, 398);
                case 3:
                case 4:
                case 5:
                case 6:
                    return new Rectangle(981 * tempFrame, 1194, 981, 398);
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (tempFrame == 4)
                        return new Rectangle(2943, 1990, 981, 398);
                    else
                        return new Rectangle(981 * tempFrame, 2388, 981, 398);
                case 12:
                case 13:
                case 14:
                case 15:
                    if (tempFrame < 4)
                        return new Rectangle(tempFrame * 981, 3184, 981, 398);
                    else
                        return new Rectangle(981 * (tempFrame - 4), 3582, 981, 398);
            }

            return new Rectangle();
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (ammo > 0)
            {
                if (justPressed == false)
                {
                    justPressed = true;
                    ammo--;
                    phase++;
                    //--Set cooldown and other base stuff
                    currentCooldown = animationLength;

                    if (skillRank < transformLevels[0])
                        PlayRandomUseSound(0, 2);
                    else if (skillRank < transformLevels[1])
                        PlayRandomUseSound(3, 5);
                    else if (skillRank < transformLevels[2])
                        PlayRandomUseSound(6, 8);
                    else
                        PlayRandomUseSound(9, 11);

                    if (ammo <= 0)
                    {
                        currentCooldown = fullCooldown;
                        reloading = true;
                    }

                    animationLength = 12;


                    if (skillRank >= 12)
                        animationLength = 20;
                    else if (skillRank >= 7)
                        animationLength = 16;


                    useKey = key;
                    SetRectangles();


                    if (skillRank >= 7)
                        game.Camera.ShakeCamera(10, (skillRank / 4) - 1);

                    poseType = 0;// Game1.randomNumberGen.Next(2);
                }
            }
        }

        public void SetRectangles()
        {
            #region Set rectangles
            if (player.FacingRight == true)
            {
                if(skillRank >= 12)
                    shot1 = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY + 30, 400 + (3 * 100), 80);
                else if(skillRank >= 7)
                    shot1 = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY + 30, 400 + (2 * 100), 80);
                else if (skillRank >= 3)
                    shot1 = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY + 30, 400 + (1 * 100), 80);
                else
                    shot1 = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY + 30, 400, 80);
            }
            else
            {
                if (skillRank >= 12)
                    shot1 = new Rectangle(player.VitalRecX - (400 + (3* 100)), player.VitalRecY + 30, 400 + (3 * 100), 80);
                else if (skillRank >= 7)
                    shot1 = new Rectangle(player.VitalRecX - (400 + (2 * 100)), player.VitalRecY + 30, 400 + (2 * 100), 80);
                else if (skillRank >= 3)
                    shot1 = new Rectangle(player.VitalRecX - (400 + (1 * 100)), player.VitalRecY + 30, 400 + (1 * 100), 80);
                else
                    shot1 = new Rectangle(player.VitalRecX - (400 + ( 100)), player.VitalRecY + 30, 400, 80);
            }
            #endregion
        }

        public override void Update()
        {
            base.Update();

            if (animationLength > 0)
            {
                //StayInAir();
                if (skillRank < 7)
                {
                    if (animationLength > 10)
                        moveFrame = 0;
                    else if (animationLength > 7)
                        moveFrame = 1;
                    else if (animationLength > 5)
                        moveFrame = 2;
                    else if (animationLength > 3)
                        moveFrame = 3;
                    else
                        moveFrame = 4;

                }
                else if (skillRank >= 7 && skillRank < 12)
                {
                    if (animationLength > 13)
                        moveFrame = 0;
                    else if (animationLength > 10)
                        moveFrame = 1;
                    else if (animationLength > 7)
                        moveFrame = 2;
                    else if (animationLength > 4)
                        moveFrame = 3;
                    else
                        moveFrame = 4;
                }
                else
                {
                    if (animationLength > 18)
                        moveFrame = 0;
                    else if (animationLength > 16)
                        moveFrame = 1;
                    else if (animationLength > 14)
                        moveFrame = 2;
                    else if (animationLength > 10)
                        moveFrame = 3;
                    else if (animationLength > 7)
                        moveFrame = 4;
                    else if (animationLength > 5)
                        moveFrame = 5;
                    else if (animationLength > 3)
                        moveFrame = 6;
                    else if (animationLength > 1)
                        moveFrame = 7;
                    else
                        moveFrame = 8;
                }
            }

            if (skillRank >= 12)
            {
                if (animationLength == 17 || animationLength == 14 || animationLength == 11 || animationLength == 8)
                {
                    CheckCollisions(shot1, damage, new Vector2(3, -3), 3, 3);

                    int stun = Game1.randomNumberGen.Next(1, 101);

                    if (stun <= chanceToStun)
                    {
                        StunEnemy(shot1, stunTime);
                    }
                }
            }
            else if (skillRank >= 7)
            {
                if (animationLength == 15 || animationLength == 12 || animationLength == 8 || animationLength == 5)
                {
                    if (player.FacingRight)
                        CheckCollisions(shot1, damage, new Vector2(3, -3), 2, 2);
                    else
                        CheckCollisions(shot1, damage, new Vector2(-3, -3), 2, 2);

                    int stun = Game1.randomNumberGen.Next(1, 101);

                    if (stun <= chanceToStun)
                    {
                        StunEnemy(shot1, stunTime);
                    }
                }
            }
            else if (skillRank >= 3)
            {
                if (animationLength == 8 || animationLength == 5 || animationLength == 2)
                {
                    if (player.FacingRight)
                        CheckCollisions(shot1, damage, new Vector2(3, -3), 1, 1);
                    else
                        CheckCollisions(shot1, damage, new Vector2(-3, -3), 1, 1);

                    int stun = Game1.randomNumberGen.Next(1, 101);

                    if (stun <= chanceToStun)
                    {
                        StunEnemy(shot1, stunTime);
                    }
                }
            }
            else
            {
                if (animationLength == 8 || animationLength == 5)
                {
                    if (player.FacingRight)
                        CheckCollisions(shot1, damage, new Vector2(3, -3), 1, 1);
                    else
                        CheckCollisions(shot1, damage, new Vector2(-3, -3), 1, 1);

                    int stun = Game1.randomNumberGen.Next(1, 101);

                    if (stun <= chanceToStun)
                    {
                        StunEnemy(shot1, stunTime);
                    }
                }
            }

            if (animationLength > 0)
            {
                SetRectangles();

                if (current.IsKeyDown(Keys.Right))
                {
                    player.Position += new Vector2(4, 0);
                }

                if (current.IsKeyDown(Keys.Left))
                {
                    player.Position += new Vector2(-4, 0);
                }
            }

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                useNext[0] = false;
                useNext[1] = false;
                useNext[2] = false;
            }

            #region Use next shot
            //if (animationLength == 0 && useNext[1] == true && ammo > 0)
            //{
            //    currentCooldown = animationLength;
            //    ammo--;
            //    phase++;

            //    if (ammo <= 0)
            //    {
            //        currentCooldown = fullCooldown;
            //        reloading = true;
            //    }

            //    animationLength = 18;

            //}

            //else if (animationLength == 0 && useNext[0] == true && ammo > 0)
            //{
            //    currentCooldown = animationLength;
            //    ammo--;
            //    phase++;
            //    if (ammo <= 0)
            //    {
            //        currentCooldown = fullCooldown;
            //        reloading = true;
            //    }

            //    animationLength = 15;
            //}
            #endregion

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            if (reloading == true && currentCooldown <= 0)
            {
                reloading = false;
                phase = 0;
                ammo = maxAmmo;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                if (player.FacingRight)
                {
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.Rec.X, player.Rec.Y, 981, 398), GetLightningSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.Rec.X - 431, player.Rec.Y, 981, 398), GetLightningSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }

            ////Draw shots
            //for (int i = 0; i < shots.Count; i++)
            //{
            //    shots[i].Draw(s);
            //}
        }


    }
}
