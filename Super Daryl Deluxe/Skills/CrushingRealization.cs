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
    public class CrushingRealization : Skill
    {

        Rectangle hitBox;
        int skillRange;
        int timeOnHangFrame;
        public CrushingRealization(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = 1.75f;
            experience = 0;
            experienceUntilLevel = 100;
            skillRank = 1;
            name = "Crushing Realization";
            canUse = true;
            description = "Nothing says \"Oh shit!\" like an enemy about to be pummeled by a giant pointy thing on a stick.";
            fullCooldown = 150;
            levelToUse = 3;
            hitPauseTime = 5;

            //--Animation and skill attributes
            animationLength = 0;
            canUseInAir = true;
            costToBuy = 2;
            skillBarColor = new Color(204, 106, 0);

            transformLevels = new int[3] { 3, 7, 12 };

            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(7);
            playerLevelRequiredToLevel.Add(9);

            playerLevelRequiredToLevel.Add(11);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(16);
            playerLevelRequiredToLevel.Add(20);

            playerLevelRequiredToLevel.Add(24);
            playerLevelRequiredToLevel.Add(29);
            playerLevelRequiredToLevel.Add(36);
            playerLevelRequiredToLevel.Add(40);
            playerLevelRequiredToLevel.Add(43);

            rangedOrMelee = AttackType.RangedOrMelee.Melee;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override Rectangle GetSourceRec()
        {
            switch(skillRank)
            {
                case 1:
                case 2:
                if (moveFrame < 7)
                    return new Rectangle(moveFrame * 530, 0, 530, 463);
                else
                    return new Rectangle((530 * (moveFrame - 7)), 463, 530, 463);
                case 3:
                case 4:
                case 5:
                case 6:
                if (moveFrame < 7)
                    return new Rectangle(moveFrame * 563, 926, 563, 460);
                else
                    return new Rectangle((563 * (moveFrame - 7)), 1386, 563, 460);
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                if (moveFrame < 6)
                    return new Rectangle(moveFrame * 714, 1846, 714, 517);
                else
                    return new Rectangle((714 * (moveFrame - 6)), 2363, 714, 517);
                case 12:
                case 13:
                case 14:
                case 15:
                if (moveFrame < 4)
                    return new Rectangle(moveFrame * 844, 2880, 844, 560);
                else if (moveFrame == 4)
                    return new Rectangle(3454, 2925, 844, 560);
                else if(moveFrame != 9)
                    return new Rectangle((844 * (moveFrame - 5)), 3440, 844, 560);
                else
                    return new Rectangle(3153, 2367, 844, 560);


            }

            return new Rectangle();
            
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 90;
                useKey = key;

                if (skillRank < transformLevels[0])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_hammer_lvl1"], "weapon_hammer_lvl1", false);
                else if (skillRank < transformLevels[1])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_hammer_lvl2"], "weapon_hammer_lvl2", false);
                else if (skillRank < transformLevels[2])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_hammer_lvl3"], "weapon_hammer_lvl3", false);
                else
                    Sound.PlaySoundInstance(skillUseSounds["weapon_hammer_lvl4"], "weapon_hammer_lvl4", false);

                moveFrame = 0;
                frameDelay = 5;
                timeOnHangFrame = 0;
            }
        }

        public override void Update()
        {
            base.Update();

            StayInAir();

            if (animationLength > 0)
            {
                frameDelay--;

                if (frameDelay == 0)
                {
                    frameDelay = 5;
                    moveFrame++;

                    if (moveFrame == 6 && timeOnHangFrame < 36)
                    {
                        moveFrame = 4;
                    }

                    if (moveFrame == 9)
                        frameDelay = 15;

                    if (moveFrame == 6)
                    {
                        game.Camera.ShakeCamera(10 + ((skillRank / 4) * 3), 5 + (skillRank / 4));
                    }
                }
            }

            if (moveFrame == 4 || moveFrame == 5)
                timeOnHangFrame++;
            //--If the animation is completed
            if (animationLength < 0)
            {
                justPressed = false;
            }
            if (animationLength == 20)
            {
                //--Check to see if an enemy is getting hit, then stun them
                CheckCollisions(hitBox, damage, new Vector2(10, -5), 0, 0);
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            #region Set rectangles
            if (player.FacingRight == true)
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:
                        hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.RecY - 50,
    180, player.Rec.Height);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.RecY - 50,
    210, player.Rec.Height); 
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.RecY - 50,
    280, player.Rec.Height);
                        break;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.RecY - 50,
    350, player.Rec.Height);
                        break;
                }

            }
            else
            {

                switch (skillRank)
                {
                    case 1:
                    case 2:

                        hitBox = new Rectangle(player.VitalRecX - 180, player.RecY - 50,
                            180, player.Rec.Height);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:

                        hitBox = new Rectangle(player.VitalRecX - 210, player.RecY - 50,
                            210, player.Rec.Height);
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        hitBox = new Rectangle(player.VitalRecX - 280, player.RecY - 50,
                            280, player.Rec.Height);
                        break;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        hitBox = new Rectangle(player.VitalRecX - 350, player.RecY - 50,
                            350, player.Rec.Height);
                        break;
                }
            }
            #endregion

            if (animationLength > 0 && player.KnockedBack)
            {
                //player.StopSkills();
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                Rectangle sourceRec = GetSourceRec();
                if (player.FacingRight)
                {
                    if(skillRank < 12)
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White);
                    else if(skillRank >= 12)
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX - 72, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White);
                    else
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX - 153, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White);
                }
                else
                {
                    if (skillRank < 13)
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX - sourceRec.Width + 530, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if(skillRank >= 13)
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX - sourceRec.Width + 530 + 72, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else
                        s.Draw(game.SkillAnimations[name], new Rectangle(player.RecX - sourceRec.Width + 530 + 153, player.RecY - sourceRec.Height + 398, sourceRec.Width, sourceRec.Height), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    fullCooldown = 150;
                    damage = 1.8f;
                    hitPauseTime = 7;
                    experience = 0;
                    experienceUntilLevel = 350;
                    break;
                case 3:
                    fullCooldown = 150;
                    damage = 1.85f;
                    hitPauseTime = 7;
                    experience = 0;
                    experienceUntilLevel = 425;
                    break;
                case 4:
                    fullCooldown = 150;
                    damage = 1.92f;
                    hitPauseTime = 7;
                    experience = 0;
                    experienceUntilLevel = 950;
                    break;
                case 5:
                    fullCooldown = 150;
                    damage = 2f;
                    hitPauseTime = 7;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    break;
                case 6:
                    fullCooldown = 150;
                    damage = 2.05f;
                    hitPauseTime = 7;
                    experience = 0;
                    experienceUntilLevel = 1800;
                    break;
                case 7:
                    fullCooldown = 125;
                    damage = 2.1f;
                    hitPauseTime = 9;
                    experience = 0;
                    experienceUntilLevel = 2300;
                    break;
                case 8:
                    fullCooldown = 125;
                    damage = 2.15f;
                    hitPauseTime = 9;
                    experience = 0;
                    experienceUntilLevel = 2300;
                    break;
                case 9:
                    fullCooldown = 125;
                    damage = 2.2f;
                    hitPauseTime = 9;
                    experience = 0;
                    experienceUntilLevel = 2300;
                    break;
                case 10:
                    fullCooldown = 125;
                    damage = 2.25f;
                    experienceUntilLevel = 1200;
                    hitPauseTime = 9;
                    experience = 0;
                    break;
                case 11:
                    fullCooldown = 125;
                    damage = 2.3f;
                    experienceUntilLevel = 1200;
                    hitPauseTime = 9;
                    experience = 0;
                    break;
                case 12:
                    fullCooldown = 120;
                    damage = 2.35f;
                    experienceUntilLevel = 1200;
                    hitPauseTime = 15;
                    experience = 0;
                    break;
                case 13:
                    fullCooldown = 115;
                    damage = 2.55f;
                    experienceUntilLevel = 1200;
                    hitPauseTime = 15;
                    experience = 0;
                    break;
                case 14:
                    fullCooldown = 110;
                    damage = 2.8f;
                    experienceUntilLevel = 1200;
                    hitPauseTime = 15;
                    experience = 0;
                    break;
                case 15:
                    fullCooldown = 100;
                    hitPauseTime = 15;
                    damage = 3f;
                    experience = 0;
                    break;
            }

            description = "Nothing says \"Oh shit!\" like an enemy about to be pummeled by a giant pointy thing on a stick.";

        }
    }
}

