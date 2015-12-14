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
    public class BlindingLogic : Skill
    {

        Rectangle hitBox;
        int skillRange = 430;
        int skillHeight = 300;
        int stunTime = 120;
        public static Dictionary<String, Texture2D> flashTextures;

        public BlindingLogic(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .3f;
            experience = 0;
            experienceUntilLevel = 10;
            skillRank = 1;
            name = "Blinding Logic";
            canUse = true;
            description = "Daryl removes his glasses, stunning enemies in front of him. \nMax Enemies Hit: 2";
            fullCooldown = 300;
            levelToUse = 3;

            //--Animation and skill attributes
            animationLength = 0;
            canUseInAir = true;
            maxHit = 2;
            costToBuy = 1;
            skillBarColor = new Color(196, 196, 196);

            transformLevels = new int[3] { 3, 7, 12 };

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(5);
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

            rangedOrMelee = AttackType.RangedOrMelee.Ranged;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override Rectangle GetSourceRec()
        {
            if (moveFrame < 7)
                return new Rectangle(moveFrame * 530, 0, 530, 398);
            else
                return new Rectangle((530 * (moveFrame - 7)), 398, 530, 398);
        }

        public Rectangle GetShadowSourceRec()
        {
            if (skillRank > 9 && skillRank < 15)
                return new Rectangle(moveFrame * 530, 1194, 530, 398);
            else
                return new Rectangle(moveFrame * 530, 796, 530, 398);
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 27;
                useKey = key;
                hitThisTime = 0;
                stunnedThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                PlayRandomUseSound();
            }
        }

        public override void Update()
        {
            base.Update();

            if (animationLength > 24)
                moveFrame = 0;
            if (animationLength > 21)
                moveFrame = 1;
            else if (animationLength > 18)
                moveFrame = 2;
            else if (animationLength > 15)
                moveFrame = 3;
            else if (animationLength > 12)
                moveFrame = 4;
            else if (animationLength > 9)
                moveFrame = 5;
            else if (animationLength > 6)
                moveFrame = 6;
            else if (animationLength > 3)
                moveFrame = 7;
            else
                moveFrame = 8;

            if (animationLength > 0)
                player.MoveDuringAttackJump();

            //--If the animation is completed
            if (animationLength < 0)
            {
                justPressed = false;
            }
            if (animationLength == 20)
            {
                //--Check to see if an enemy is getting hit, then stun them
                CheckFiniteCollisions(hitBox, damage, Vector2.Zero, 0, 0);
                StunEnemy(hitBox, stunTime);
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            #region Set rectangles
            if (player.FacingRight == true)
            {
                hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY - ((skillHeight - 200) / 2),
                    skillRange, skillHeight);
            }
            else
            {
                hitBox = new Rectangle(player.VitalRecX - skillRange + 25, player.VitalRecY - ((skillHeight - 200) / 2),
                    skillRange, skillHeight);
            }
            #endregion

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                if (player.FacingRight)
                {
                    if ((skillRank >= 10) && moveFrame < 5)
                        s.Draw(game.SkillAnimations[name], player.Rec, GetShadowSourceRec(), Color.White);

                    s.Draw(game.SkillAnimations[name], player.Rec, GetSourceRec(), Color.White);

                    if (moveFrame < 6)
                    {
                        switch (skillRank)
                        {
                            case 1:
                            case 2:
                                s.Draw(flashTextures["Level1" + moveFrame], new Rectangle(player.RecX - 148, player.RecY - 317, 1266, 1031), Color.White);
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                                s.Draw(flashTextures["Level2" + moveFrame], new Rectangle(player.RecX - 148, player.RecY - 317, 1266, 1031), Color.White);
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                s.Draw(flashTextures["Level3" + moveFrame], new Rectangle(player.RecX - 148, player.RecY - 317, 1266, 1031), Color.White);
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                s.Draw(flashTextures["Level4" + moveFrame], new Rectangle(player.RecX - 148, player.RecY - 317, 1266, 1031), Color.White);
                                break;
                        }
                    }
                }
                else
                {

                    if ((skillRank >= 10) && moveFrame < 5)
                        s.Draw(game.SkillAnimations[name], player.Rec, GetShadowSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    s.Draw(game.SkillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (moveFrame < 6)
                    {
                        switch (skillRank)
                        {
                            case 1:
                            case 2:
                                s.Draw(flashTextures["Level1" + moveFrame], new Rectangle(player.RecX - 148 - 455, player.RecY - 317, 1266, 1031), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                                s.Draw(flashTextures["Level2" + moveFrame], new Rectangle(player.RecX - 148 - 455, player.RecY - 317, 1266, 1031), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                s.Draw(flashTextures["Level3" + moveFrame], new Rectangle(player.RecX - 148 - 455, player.RecY - 317, 1266, 1031), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                s.Draw(flashTextures["Level4" + moveFrame], new Rectangle(player.RecX - 148 - 455, player.RecY - 317, 1266, 1031), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                        }
                    }
                }
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    fullCooldown = 295;
                    damage = .31f;
                    experience = 0;
                    experienceUntilLevel = 300;
                    maxHit = 2;
                    skillRange = 430;
                    skillHeight = 300;
                    break;
                case 3:
                    fullCooldown = 285;
                    damage = .32f;
                    experience = 0;
                    experienceUntilLevel = 400;
                    maxHit = 3;
                    skillRange = 475;
                    skillHeight = 300;
                    break;
                case 4:
                    fullCooldown = 280;
                    damage = .33f;
                    experience = 0;
                    experienceUntilLevel = 700;
                    maxHit = 3;
                    skillRange = 475;
                    skillHeight = 300;
                    break;
                case 5:
                    fullCooldown = 275;
                    damage = .35f;
                    experience = 0;
                    experienceUntilLevel = 1500;
                    maxHit = 3;
                    skillRange = 475;
                    skillHeight = 350;
                    break;
                case 6:
                    fullCooldown = 275;
                    damage = .36f;
                    experienceUntilLevel = 1700;
                    maxHit = 3;
                    skillRange = 475;
                    skillHeight = 350;
                    experience = 0;
                    break;
                case 7:
                    fullCooldown = 275;
                    damage = .37f;
                    experienceUntilLevel = 1900;
                    maxHit = 4;
                    skillRange = 550;
                    skillHeight = 350;
                    experience = 0;
                    stunTime = 155;
                    break;
                case 8:
                    fullCooldown = 275;
                    damage = .38f;
                    experienceUntilLevel = 2500;
                    maxHit = 4;
                    skillRange = 550;
                    skillHeight = 350;
                    experience = 0;
                    stunTime = 155;
                    break;
                case 9:
                    fullCooldown = 275;
                    damage = .39f;
                    experienceUntilLevel = 2500;
                    maxHit = 4;
                    skillRange = 550;
                    skillHeight = 350;
                    experience = 0;
                    stunTime = 155;
                    break;
                case 10:
                    fullCooldown = 275;
                    damage = .4f;
                    experienceUntilLevel = 2500;
                    maxHit = 4;
                    skillRange = 550;
                    skillHeight = 425;
                    experience = 0;
                    stunTime = 155;
                    break;
                case 11:
                    fullCooldown = 240;
                    damage = .43f;
                    experience = 0;
                    maxHit = 4;
                    skillRange = 550;
                    skillHeight = 425;
                    stunTime = 155;
                    break;
                case 12:
                    fullCooldown = 240;
                    damage = .46f;
                    experience = 0;
                    maxHit = 5;
                    skillRange = 730;
                    skillHeight = 425;
                    stunTime = 200;
                    break;
                case 13:
                    fullCooldown = 240;
                    damage = .51f;
                    experience = 0;
                    maxHit = 5;
                    skillRange = 730;
                    skillHeight = 425;
                    stunTime = 200;
                    break;
                case 14:
                    fullCooldown = 240;
                    damage = .56f;
                    experience = 0;
                    maxHit = 5;
                    skillRange = 730;
                    skillHeight = 425;
                    stunTime = 200;
                    break;
                case 15:
                    fullCooldown = 240;
                    damage = .6f;
                    experience = 0;
                    maxHit = 6;
                    skillRange = 730;
                    skillHeight = 730;
                    stunTime = 200;
                    break;
            }

            description = "Daryl removes his glasses, stunning enemies in front of him. \nMax Enemies Hit: " + maxHit + " | Stun Time: " + stunTime;
        }
    }
}

