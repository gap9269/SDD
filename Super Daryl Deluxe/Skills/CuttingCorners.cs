using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class CuttingCorners : Skill
    {
        Rectangle attackRec, checkPlatRec;
        int phase = 1;
        int maxAnimationLength;
        Boolean lastHit;
        int maxPhase = 2;

        public CuttingCorners(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .3f;
            experience = 0;
            experienceUntilLevel = 1000;
            skillRank = 1;
            levelToUse = 12;
            name = "Cutting Corners";
            canUse = true;
            description = "Test out your sword collection as your dash passed your foes. \nNumber of attacks: 2";
            fullCooldown = 135;
            //--Animation and skill attributes
            hitPauseTime = 1;

            costToBuy = 5;
            skillBarColor = new Color(255, 156, 108);

            transformLevels = new int[2] { 5, 10 };

            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(15);

            playerLevelRequiredToLevel.Add(16);
            playerLevelRequiredToLevel.Add(18);
            playerLevelRequiredToLevel.Add(18);
            playerLevelRequiredToLevel.Add(19);
            playerLevelRequiredToLevel.Add(20);

            playerLevelRequiredToLevel.Add(21);
            playerLevelRequiredToLevel.Add(29);
            playerLevelRequiredToLevel.Add(36);
            playerLevelRequiredToLevel.Add(40);
            playerLevelRequiredToLevel.Add(43);

            rangedOrMelee = AttackType.RangedOrMelee.Melee;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .32f;
                    experience = 0;
                    experienceUntilLevel = 750;
                    break;
                case 3:
                    damage = .35f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    fullCooldown = 125;
                    break;
                case 4:
                    damage = .37f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    fullCooldown = 125;
                    break;
                case 5:
                    damage = .39f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    maxPhase = 3;
                    fullCooldown = 125;
                    break;
                case 6:
                    damage = .42f;
                    experienceUntilLevel = 1600;
                    experience = 0;
                    maxPhase = 3;
                    fullCooldown = 125;
                    break;
                case 7:
                    damage = .43f;
                    experience = 0;
                    maxPhase = 3;
                    fullCooldown = 125;
                    break;
                case 8:
                    damage = .45f;
                    experience = 0;
                    maxPhase = 3;
                    fullCooldown = 125;
                    break;
                case 9:
                    damage = .46f;
                    experience = 0;
                    maxPhase = 3;
                    fullCooldown = 125;
                    break;
                case 10:
                    damage = .48f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
                case 11:
                    damage = .50f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
                case 12:
                    damage = .52f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
                case 13:
                    damage = .53f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
                case 14:
                    damage = .54f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
                case 15:
                    damage = .55f;
                    experience = 0;
                    maxPhase = 4;
                    fullCooldown = 125;
                    break;
            }

            description = "Test out your sword collection as your dash passed your foes. \nNumber of attacks: " + maxPhase;
        }


        public override Rectangle GetSourceRec()
        {
            if (moveFrame == 3 && phase != 4)
                moveFrame = 2;
            switch (phase)
            {
                case 1:
                    return new Rectangle(1124 * moveFrame, 0, 1124, 590);
                case 2:
                    return new Rectangle(1124 * moveFrame, 590, 1124, 590);
                case 3:
                    return new Rectangle(1124 * moveFrame, 1180, 1124, 590);
                case 4:
                    if(moveFrame < 3)
                        return new Rectangle(1124 * moveFrame, 1770, 1124, 590);

                    return new Rectangle(0, 2360, 1124, 590);
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
                animationLength = 13;
                maxAnimationLength = animationLength;
                useKey = key;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();

                if(skillRank >= 10)
                    Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + 4 + "_a_01"], "weapon_sword_barrage_lvl" + 4 + "_a_01", false);
                else if (skillRank >= 5)
                    Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + 3 + "_a_01"], "weapon_sword_barrage_lvl" + 3 + "_a_01", false);
                else
                    Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + 1 + "_a_01"], "weapon_sword_barrage_lvl" + 1 + "_a_01", false);

            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();
            StayInAir();

            if (animationLength >= 0)
            {
                if (animationLength == maxAnimationLength - 4)
                {
                    if (player.FacingRight)
                        player.VelocityX = 250;
                    else
                        player.VelocityX = -250;
                }
                else if (animationLength == maxAnimationLength - 5)
                {
                    player.VelocityX = 0;
                }

                if (!lastHit)
                {
                    if (animationLength > 9)
                        moveFrame = 0;
                    else if (animationLength > 5)
                        moveFrame = 1;
                    else
                        moveFrame = 2;
                }
                else
                {
                    if (animationLength > 16)
                        moveFrame = 0;
                    else if (animationLength > 12)
                        moveFrame = 1;
                    else if (animationLength > 8)
                        moveFrame = 2;
                    else
                        moveFrame = 3;
                }
                if (moveFrame < 2)
                {
                    float tempDamage = (phase / 15f) + (.075f + (skillRank / 5) * .025f);
                    CheckFiniteCollisions(attackRec, tempDamage, new Vector2(10, -5), 2, 2);
                }
                if (animationLength == maxAnimationLength - 4)
                {
                    hitThisTime = 0;
                    enemiesHitThisAttack.Clear();
                    bossesHitThisAttack.Clear();
                    interactiveObjectsThisAttack.Clear();

                }
            }

            #region Set rectangles
            if (player.FacingRight)
            {
                switch (phase)
                {
                    case 1:
                        attackRec = new Rectangle(player.VitalRecX - 285, player.VitalRecY - 80,
    580, player.VitalRecHeight + 130);

                        break;
                    case 2:
                        attackRec = new Rectangle(player.VitalRecX - 275, player.VitalRecY - 90,
670, player.VitalRecHeight + 130);
                        break;
                    case 3:
                        attackRec = new Rectangle(player.VitalRecX - 255, player.VitalRecY - 120,
    630, player.VitalRecHeight + 200);
                        break;
                    case 4:
                        attackRec = new Rectangle(player.VitalRecX - 305, player.VitalRecY - 180,
    780, player.VitalRecHeight + 240);
                        break;
                }

                checkPlatRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY, (int)300,
    player.VitalRecHeight + 35);
            }
            else
            {
                switch (phase)
                {
                    case 1:
                        attackRec = new Rectangle(player.VitalRecX - 235, player.VitalRecY - 80,
    580, player.VitalRecHeight + 130);
                        break;
                    case 2:
                        attackRec = new Rectangle(player.VitalRecX - 335, player.VitalRecY - 90,
670, player.VitalRecHeight + 130);
                        break;
                    case 3:
                        attackRec = new Rectangle(player.VitalRecX - 305, player.VitalRecY - 120,
630, player.VitalRecHeight + 200);
                        break;
                    case 4:
                        attackRec = new Rectangle(player.VitalRecX - 415, player.VitalRecY - 180,
    780, player.VitalRecHeight + 240);
                        break;
                }

                checkPlatRec = new Rectangle(player.VitalRecX - (int)300, player.VitalRecY, (int)300,
    player.VitalRecHeight + 35);
            }
            #endregion

            if (animationLength >= 0)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (checkPlatRec.Intersects(platforms[i].Rec) && platforms[i].Passable == false)
                    {
                        player.VelocityX = 0;
                    }
                }
            }

            if (animationLength == 0)
            {
                if (phase < maxPhase)
                {
                    hitThisTime = 0;
                    enemiesHitThisAttack.Clear();
                    bossesHitThisAttack.Clear();
                    interactiveObjectsThisAttack.Clear();

                    int skillRankSoundNum; 
                    if (skillRank >= 10)
                    {
                        skillRankSoundNum = 4;
                    }
                    else if (skillRank >= 5)
                    {
                        skillRankSoundNum = 3;

                    }
                    else
                    {
                        skillRankSoundNum = 1;

                    }

                    if (phase == 1)
                        Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + skillRankSoundNum + "_b_01"], "weapon_sword_barrage_lvl" + skillRankSoundNum + "_b_01", false);
                    if (phase == 2)
                        Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + skillRankSoundNum + "_c_01"], "weapon_sword_barrage_lvl" + skillRankSoundNum + "_c_01", false);
                    if (phase == 3)
                        Sound.PlaySoundInstance(skillUseSounds["weapon_sword_barrage_lvl" + skillRankSoundNum + "_d_01"], "weapon_sword_barrage_lvl" + skillRankSoundNum + "_d_01", false);

                    phase++;
                    moveFrame = 0;
                    animationLength = 13;

                    if (phase == 4)
                        lastHit = true;

                    if (lastHit)
                        animationLength = 20;
                    maxAnimationLength = animationLength;
                }
            }

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                    moveFrame = 0;
                    justPressed = false;
                    lastHit = false;
                    phase = 1;
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

            if (animationLength >= 0)
            {
                if (player.FacingRight)
                {
                    s.Draw(Game1.skillAnimations[name], new Vector2(player.RecX - 399, player.RecY - 191), GetSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.RecX - GetSourceRec().Width + 932, player.RecY - 191, GetSourceRec().Width, GetSourceRec().Height), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }

            }
        }

    }
}
