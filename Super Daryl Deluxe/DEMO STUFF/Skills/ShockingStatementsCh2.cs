using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
namespace ISurvived
{
    public class ShockingStatementCh2Demo : Skill
    {
        int chanceToStun = 5;
        int stunTime = 120;
        SoundEffectInstance weapon_flash_bomb_loop;

        struct FlashBomb
        {
            public Rectangle rec, explosionRec;
            public Boolean exploding;
            public Boolean dead;
            public int frame, frameDelay;
            public Vector2 velocity;
            public float posX, posY;
            public int timeAlive, timeBeforeExplode;
            public Boolean facingRight;
        }

        FlashBomb flashbomb;

        public ShockingStatementCh2Demo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .5f;
            experience = 0;
            experienceUntilLevel = 350;
            skillRank = 1;
            levelToUse = 3;
            name = "Shocking Statements CH.2";
            canUse = true;
            description = "Launch an orb of electric murder out of your chest like a real man. Press again to detonate early, if you're into that sort of thing.";
            fullCooldown = 160;
            //--Animation and skill attributes
            animationLength = 0;
            costToBuy = 1;
            hitPauseTime = 1;

            skillBarColor = new Color(0, 255, 255);

            skillType = AttackType.AttackTypes.Lightning;
            rangedOrMelee = AttackType.RangedOrMelee.Ranged;

            transformLevels = new int[3] { 3, 7, 12 };

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

            flashbomb.dead = true;
        }

        public override Rectangle GetSourceRec()
        {
            if (skillRank >= 12)
            {
                if(moveFrame == 3)
                    return new Rectangle(3470, 0, 530, 398);

                return new Rectangle(2106 + (530 * moveFrame), 668, 530, 398);
            }
            return new Rectangle(530 * moveFrame, 0, 530, 398);
        }

        public Rectangle GetLightningSourceRec()
        {
            switch (skillRank)
            {
                case 1:
                case 2:
                    if(flashbomb.exploding)
                        return new Rectangle(flashbomb.frame * 480, 1066, 480, 480);

                    return new Rectangle(2120 + (flashbomb.frame * 186), 0, 186, 186);
                case 3:
                case 4:
                case 5:
                case 6:
                    if (flashbomb.exploding)
                        return new Rectangle((flashbomb.frame * 576), 1546, 576, 576);

                    return new Rectangle((flashbomb.frame * 223), 398, 223, 223);
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (flashbomb.exploding)
                        return new Rectangle((flashbomb.frame * 600), 2122, 600, 612);

                    return new Rectangle(1338 + (flashbomb.frame * 270), 398, 270, 270);
                case 12:
                case 13:
                case 14:
                case 15:
                    if (flashbomb.exploding)
                    {
                        if(flashbomb.frame < 3)
                            return new Rectangle((flashbomb.frame * 1264), 2734, 1264, 594);

                        return new Rectangle(((flashbomb.frame - 3) * 1264), 3328, 1264, 594);
                    }
                    return new Rectangle(flashbomb.frame * 351, 668, 351, 351);

            }
            return new Rectangle();
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {

                if (weapon_flash_bomb_loop == null)
                {
                    weapon_flash_bomb_loop = skillUseSounds["weapon_flash_bomb_loop"].CreateInstance();
                }

                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 16;
                useKey = key;
                PlayRandomUseSound();
                moveFrame = 0;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();

                if (skillRank < transformLevels[0])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_cast_lvl1"], "weapon_flash_bomb_cast_lvl1", false);
                else if (skillRank < transformLevels[1])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_cast_lvl2"], "weapon_flash_bomb_cast_lvl2", false);
                else if (skillRank < transformLevels[2])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_cast_lvl3"], "weapon_flash_bomb_cast_lvl3", false);
                else
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_cast_lvl4"], "weapon_flash_bomb_cast_lvl4", false);


            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            weapon_flash_bomb_loop = null;
            flashbomb.dead = true;
            flashbomb.exploding = false;
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();

            if (animationLength > 13)
                moveFrame = 0;
            else if (animationLength > 9)
                moveFrame = 1;
            else if (animationLength > 6)
                moveFrame = 2;
            else
                moveFrame = 3;

            if (
                (
                    (current.IsKeyUp(useKey) && last.IsKeyDown(useKey)) ||
                    (useKey == Keys.Q && MyGamePad.APressed()) ||
                    (useKey == Keys.W && MyGamePad.BPressed()) ||
                    (useKey == Keys.E && MyGamePad.XPressed()) ||
                    (useKey == Keys.R && MyGamePad.YPressed())
                ) && flashbomb.exploding == false && flashbomb.dead == false && flashbomb.timeAlive >= 10
               )
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:
                        flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 240, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 240, 480, 480);
                        flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 280, 280);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 576/2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 576 / 2, 576, 576);
                        flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 376, 376);
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 600 / 2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 612 / 2, 600, 612);
                        flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 400, 412);
                        break;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 1264 / 2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 594 / 2, 1264, 594);
                        flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 1064, 594);
                        break;
                }
                int camShake = ((skillRank / 4) * 2) - 3;
                if(camShake > 0)
                    game.Camera.ShakeCamera(10, camShake);
                flashbomb.exploding = true;
                weapon_flash_bomb_loop.Stop();

                if (skillRank < transformLevels[0])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl1"], "weapon_flash_bomb_explode_lvl1", false);
                else if (skillRank < transformLevels[1])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl2"], "weapon_flash_bomb_explode_lvl2", false);
                else if (skillRank < transformLevels[2])
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl3"], "weapon_flash_bomb_explode_lvl3", false);
                else
                    Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl4"], "weapon_flash_bomb_explode_lvl4", false);

                flashbomb.frame = 0;
                flashbomb.frameDelay = 0;
            }

            if (moveFrame == 1 && flashbomb.dead)
            {
                flashbomb = new FlashBomb();
                flashbomb.dead = false;
                flashbomb.frame = 0;
                flashbomb.frameDelay = 5;
                flashbomb.exploding = false;
                flashbomb.timeAlive = 0;
                flashbomb.timeBeforeExplode = 30;
                
                if (player.FacingRight)
                {
                    flashbomb.velocity = new Vector2(13 + ((skillRank / 4) - 1), -15);
                    flashbomb.facingRight = true;
                    flashbomb.posX = player.VitalRecX + player.VitalRecWidth / 2;
                    flashbomb.posY = player.VitalRecY + player.VitalRecHeight / 2 - 60;
                }
                else
                {
                    flashbomb.velocity = new Vector2(-(13 + ((skillRank / 4) - 1)), -15);
                    flashbomb.facingRight = true;
                    flashbomb.posX = player.VitalRecX - player.VitalRecWidth / 2 - 40;
                    flashbomb.posY = player.VitalRecY + player.VitalRecHeight / 2 - 60;
                }
                    switch (skillRank)
                    {
                        case 1:
                        case 2:
                            flashbomb.rec = new Rectangle((int)flashbomb.posX, (int)flashbomb.posY, 186, 186);
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            flashbomb.posX -= 40;
                            flashbomb.posY -= 20;
                            flashbomb.rec = new Rectangle((int)flashbomb.posX, (int)flashbomb.posY, 223,223);
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            flashbomb.posX -= 60;
                            flashbomb.posY -= 60;
                            flashbomb.rec = new Rectangle((int)flashbomb.posX, (int)flashbomb.posY, 270, 270);
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            flashbomb.posX -= 100;
                            flashbomb.posY -= 130;
                            if (player.FacingRight)
                                flashbomb.posX -= 50;
                            flashbomb.rec = new Rectangle((int)flashbomb.posX, (int)flashbomb.posY, 351, 351);
                            break;
                    }
                
            }

            if (!flashbomb.dead)
            {
                if (!flashbomb.exploding)
                {
                    if (weapon_flash_bomb_loop.State != SoundState.Playing)
                        Sound.PlaySoundInstance(weapon_flash_bomb_loop, Game1.GetFileName(() => weapon_flash_bomb_loop), true, flashbomb.rec.Center.X, flashbomb.rec.Center.Y, 600, 500, 1500);

                    flashbomb.velocity.Y += GameConstants.GRAVITY;
                    flashbomb.posX += flashbomb.velocity.X;
                    flashbomb.posY += flashbomb.velocity.Y;

                    flashbomb.rec.X = (int)flashbomb.posX;
                    flashbomb.rec.Y = (int)flashbomb.posY;
                }

                flashbomb.frameDelay--;

                if (flashbomb.frameDelay <= 0)
                {
                    flashbomb.frameDelay = 5;
                    flashbomb.frame++;

                    //Bomb is dead
                    if (flashbomb.exploding)
                    {
                        if (flashbomb.frame > 3)
                            flashbomb.dead = true;
                    }

                    else if (flashbomb.frame > 5)
                        flashbomb.frame = 0;
                }

                if (!flashbomb.exploding)
                {
                    flashbomb.timeAlive++;

                    if (flashbomb.timeAlive >= flashbomb.timeBeforeExplode)
                    {
                        switch (skillRank)
                        {
                            case 1:
                            case 2:
                                flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 240, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 240, 480, 480);
                                flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 280, 280);
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                                flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 576 / 2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 576 / 2, 576, 576);
                                flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 376, 376);
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 600 / 2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 612 / 2, 600, 612);
                                flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 400, 412);
                                break;
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                                flashbomb.rec = new Rectangle(flashbomb.rec.X + flashbomb.rec.Width / 2 - 1264 / 2, flashbomb.rec.Y + flashbomb.rec.Height / 2 - 594 / 2, 1264, 594);
                                flashbomb.explosionRec = new Rectangle(flashbomb.rec.X + 100, flashbomb.rec.Y + 100, 1064, 594);
                                break;
                        }

                        int camShake = ((skillRank / 4) * 2) - 3;
                        if (camShake > 0)
                            game.Camera.ShakeCamera(10, camShake);

                        weapon_flash_bomb_loop.Stop();
                        flashbomb.exploding = true;
                        flashbomb.frame = 0;
                        flashbomb.frameDelay = 0;

                        if (skillRank < transformLevels[0])
                            Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl1"], "weapon_flash_bomb_explode_lvl1", false);
                        else if (skillRank < transformLevels[1])
                            Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl2"], "weapon_flash_bomb_explode_lvl2", false);
                        else if (skillRank < transformLevels[2])
                            Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl3"], "weapon_flash_bomb_explode_lvl3", false);
                        else
                            Sound.PlaySoundInstance(skillUseSounds["weapon_flash_bomb_explode_lvl4"], "weapon_flash_bomb_explode_lvl4", false);

                    }
                }
            }


            //--Check to see if an enemy is getting hit
            if (flashbomb.exploding && !flashbomb.dead && flashbomb.frame < 3)
            {
                if (flashbomb.frame == 1 && flashbomb.frameDelay == 3)
                {
                    hitThisTime = 0;
                    enemiesHitThisAttack.Clear();
                    bossesHitThisAttack.Clear();
                    interactiveObjectsThisAttack.Clear();
                }

                CheckFiniteCollisions(flashbomb.explosionRec, damage, new Vector2(5, -5), 0, 0);

                if (flashbomb.frame == 1 && flashbomb.frameDelay == 4)
                {
                    int stun = Game1.randomNumberGen.Next(1, 101);

                    if (stun <= chanceToStun)
                    {
                        StunEnemy(flashbomb.explosionRec, stunTime);
                    }
                }
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            if (animationLength < 0)
            {
                justPressed = false;
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

            if (flashbomb.facingRight && !flashbomb.dead)
                s.Draw(Game1.skillAnimations[name], flashbomb.rec, GetLightningSourceRec(), Color.White);
            else if(!flashbomb.dead)
                s.Draw(Game1.skillAnimations[name], flashbomb.rec, GetLightningSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
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
                    damage = .42f;
                    experience = 0;
                    chanceToStun = 6;
                    experienceUntilLevel = 800;
                    fullCooldown = 140;
                    break;
                case 7:
                    damage = .45f;
                    chanceToStun = 7;
                    experienceUntilLevel = 1900;
                    fullCooldown = 130;
                    experience = 0;
                    break;
                case 8:
                    damage = .49f;
                    chanceToStun = 7;
                    experienceUntilLevel = 1600;
                    fullCooldown = 130;
                    experience = 0;
                    break;
                case 9:
                    damage = .50f;
                    chanceToStun = 7;
                    experienceUntilLevel = 1600;
                    fullCooldown = 130;
                    experience = 0;
                    break;
                case 10:
                    damage = .51f;
                    chanceToStun = 7;
                    experienceUntilLevel = 1600;
                    fullCooldown = 130;
                    experience = 0;
                    break;
                case 11:
                    damage = .52f;
                    chanceToStun = 7;
                    experienceUntilLevel = 1600;
                    fullCooldown = 130;
                    experience = 0;
                    break;
                case 12:
                    damage = .55f;
                    experience = 0;
                    chanceToStun = 8;
                    fullCooldown = 120;
                    break;
                case 13:
                    damage = .56f;
                    experience = 0;
                    chanceToStun = 8;
                    fullCooldown = 120;
                    break;
                case 14:
                    damage = .58f;
                    experience = 0;
                    chanceToStun = 8;
                    fullCooldown = 120;
                    break;
                case 15:
                    damage = .6f;
                    experience = 0;
                    chanceToStun = 8;
                    fullCooldown = 120;
                    break;
            }

            description = "Launch an orb of electric murder out of your chest like a man. Press again to detonate early, if you're into that sort of thing. Chance to stun: " + chanceToStun +"%";

        }
    }
}
