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
    public class ShooterDemo : Skill
    {
        List<SkillProjectile> shots;
        int ammo = 1;
        int maxAmmo = 1;
        Boolean reloading = false;
        int shotSpeed = 25;
        int timeOneScreen = 60;
        int phase = 0;

        public ShooterDemo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .5f;
            experience = 0;
            experienceUntilLevel = 250;
            skillRank = 1;
            levelToUse = 3;
            name = "Fowl Mouth";
            canUse = true;
            description = "Display your finesse in archery! \nArrows: 1";
            fullCooldown = 120;
            //--Animation and skill attributes
            animationLength = 0;
            hitPauseTime = 1;
            shots = new List<SkillProjectile>();
            skillBarColor = new Color(255, 237, 5);

            transformLevels = new int[3] { 3, 6, 11 };

            costToBuy = 1;

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(6);

            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(9);
            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(14);

            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(18);
            playerLevelRequiredToLevel.Add(22);
            playerLevelRequiredToLevel.Add(25);

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Ranged;
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            if (skillRank == 2)
                skillRank = 3;
            else if (skillRank == 4)
                skillRank = 6;
            else if (skillRank == 7)
                skillRank = 11;

            switch (skillRank)
            {
                case 3:
                    damage = .54f;
                    experience = 0;
                    experienceUntilLevel = 700;
                    maxAmmo = 2;
                    break;
                case 6:
                    damage = .62f;
                    experience = 0;
                    experienceUntilLevel = 1800;
                    maxAmmo = 3;
                    break;
                case 11:
                    damage = .72f;
                    experienceUntilLevel = 1200;
                    experience = 0;
                    fullCooldown = 100;
                    maxAmmo = 4;
                    break;
                case 12:
                    damage = .74f;
                    experienceUntilLevel = 1200;
                    experience = 0;
                    fullCooldown = 100;
                    maxAmmo = 4;
                    break;
                case 13:
                    damage = .76f;
                    experienceUntilLevel = 1200;
                    experience = 0;
                    fullCooldown = 100;
                    maxAmmo = 4;
                    break;
                case 14:
                    damage = .78f;
                    experienceUntilLevel = 1200;
                    experience = 0;
                    fullCooldown = 100;
                    maxAmmo = 4;
                    break;
                case 15:
                    damage = .8f;
                    experience = 0;
                    //name = "Lightning pun";
                    maxAmmo = 4;
                    break;
            }

            description = "Display your finesse in archery! \nArrows: " + maxAmmo;

        }


        public override Rectangle GetSourceRec()
        {
            switch (phase)
            {
                case 1:
                case 4:
                    return new Rectangle(530 * moveFrame, 0, 530, 398);
                case 2:
                    return new Rectangle(1590 + (530 * moveFrame), 0, 530, 398);
                case 3:
                    if(moveFrame == 0)
                        return new Rectangle(3180, 0, 530, 398);
                    else
                        return new Rectangle(530 * (moveFrame - 1), 398, 530, 398);
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

                    if (ammo <= 0)
                    {
                        currentCooldown = fullCooldown;
                        reloading = true;
                    }

                    animationLength = 15;

                    if (phase == 3)
                        animationLength = 18;
                    useKey = key;

                    PlayRandomUseSound();
                }
            }
        }

        public void SetRectangles()
        {
            #region Set rectangles
            if (player.FacingRight == true)
            {
                shots.Add(new SkillProjectile(Game1.skillAnimations[name], new Rectangle(player.VitalRecX + player.VitalRecWidth + 80, player.VitalRecY + 50, 217, 50), timeOneScreen, new Vector2(1, 0), 0f, damage, new Vector2(5, -5), 1, 0, 0, shotSpeed, game, false, 0, new Rectangle(3710, 0, 217, 50), true, skillType, rangedOrMelee, skillHitSounds));
            }
            else
            {
                shots.Add(new SkillProjectile(Game1.skillAnimations[name], new Rectangle(player.VitalRecX - 275, player.VitalRecY + 50, 217, 50), timeOneScreen, new Vector2(-1, 0), 0f, damage, new Vector2(5, -5), 1, 0, 0, shotSpeed, game, false, 0, new Rectangle(3710, 0, 217, 50), false, skillType, rangedOrMelee, skillHitSounds));
            }
            #endregion
        }

        public override void Update()
        {
            base.Update();

            StayInAir();

            if (phase == 3)
            {
                if (animationLength > 14)
                    moveFrame = 0;
                else if (animationLength > 10)
                    moveFrame = 1;
                else
                    moveFrame = 2;

                if(animationLength == 10)
                    SetRectangles();
            }
            else
            {
                if (animationLength > 10)
                    moveFrame = 0;
                else if (animationLength > 5)
                    moveFrame = 1;
                else
                    moveFrame = 2;

                if(animationLength == 5)
                    SetRectangles();
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

            if (animationLength == 0 && useNext[2] == true && ammo > 0)
            {
                currentCooldown = animationLength;
                ammo--;
                phase++;

                if (ammo <= 0)
                {
                    currentCooldown = fullCooldown;
                    reloading = true;
                }
                PlayRandomUseSound();

                animationLength = 15;

            }

            else if (animationLength == 0 && useNext[1] == true && ammo > 0)
            {
                currentCooldown = animationLength;
                ammo--;
                phase++;

                if (ammo <= 0)
                {
                    currentCooldown = fullCooldown;
                    reloading = true;
                }
                PlayRandomUseSound();

                animationLength = 18;

            }

            else if (animationLength == 0 && useNext[0] == true && ammo > 0)
            {
                currentCooldown = animationLength;
                ammo--;
                phase++;
                if (ammo <= 0)
                {
                    currentCooldown = fullCooldown;
                    reloading = true;
                }
                PlayRandomUseSound();

                animationLength = 15;
            }
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

            //Update shots
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update();

                if (shots[i].Dead == true)
                {
                    shots.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength >= 0)
            {
                //s.Draw(Game1.emptyBox, punch, Color.Black);
                if (player.FacingRight)
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }

            //Draw shots
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Draw(s);
            }
        }

    }
}
