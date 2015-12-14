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
    public class TrojanHorse : Boss
    {
        public int health, maxHealth;
        int frame;
        int frameDelay = 5;
        Rectangle frec;
        Boolean facingRight = true;
        public Boolean hasLocker, drawFButton;

        public Rectangle lockerRec;
        Dictionary<String, SoundEffect> hitSounds;
        Dictionary<String, Texture2D> textures;

        public static Texture2D hudFaceTexture;
        public static Texture2D explosionTexture;

        KeyboardState current, last;

        SoundEffectInstance object_bomb_horse_move_loop;

        int lastRecXPos;

        public TrojanHorse(int x, int y, Player p, MapClass cur) 
            : base(new Vector2(x, y), "Gift Horse", Game1.g, ref p, cur)
        {
            health = maxHealth = 100;
            position = new Vector2(x, y);
            rec = new Rectangle(x, y, 638, 745);
            vitalRec = new Rectangle(rec.X, rec.Y, 250, 300);
            lockerRec = new Rectangle(rec.X + 150, rec.Y, 130, rec.Height);

            drawHUDName = true;

            addToHealthWidth = game.EnemySpriteSheets["BossHealthBar"].Width;
            canBeHurt = false;
        }

        public void LoadContent(ContentManager content, Boolean locker, Boolean normal)
        {
            if(locker)
                 textures = ContentLoader.LoadContent(content, "Maps\\History\\TrojanHorseLocker");
            else if(normal)
                textures = ContentLoader.LoadContent(content, "Maps\\History\\TrojanHorseNormal");
            else
                textures = ContentLoader.LoadContent(content, "Maps\\History\\TrojanHorse");


            explosionTexture = content.Load<Texture2D>(@"Maps\\History\\Explosion");

            hitSounds = new Dictionary<String, SoundEffect>();
            hitSounds.Add("object_horse_damage_01", content.Load<SoundEffect>(@"Sound\Objects\object_horse_damage_01"));
            hitSounds.Add("object_horse_damage_02", content.Load<SoundEffect>(@"Sound\Objects\object_horse_damage_02"));
            hitSounds.Add("object_horse_damage_03", content.Load<SoundEffect>(@"Sound\Objects\object_horse_damage_03"));


            object_bomb_horse_move_loop = content.Load<SoundEffect>(@"Sound\Objects\object_bomb_horse_move_loop").CreateInstance();
        }

        public override Rectangle GetHealthSourceRectangle()
        {
            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override void Update(int mapwidth)
        {
            UpdateShakeHealthBar();
            canBeHurt = false;

            if (hasBeenHit)
            {
                targetHealthWidth = (int)((float)originalHealthWidth * ((float)health / (float)maxHealth));
                addToHealthWidth += ((targetHealthWidth - healthBarRec.Width) * .07f); //Must add it to this because converting it to a int cuts off numbers
                healthBarRec.Width = (int)addToHealthWidth;
            }

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            lastRecXPos = rec.X;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;
            vitalRec.X = rec.X + 175;
            vitalRec.Y = rec.Y + 400;
            lockerRec.X = rec.X + 150;

            if (lastRecXPos == rec.X)
            {
                if (object_bomb_horse_move_loop.State == SoundState.Playing)
                    object_bomb_horse_move_loop.Stop();
            }

            if (hasLocker)
            {
                last = current;
                current = Keyboard.GetState();

                if (facingRight)
                    frec = new Rectangle((rec.X + 210), rec.Y + 285, 43, 65);
                else
                    frec = new Rectangle((rec.X + rec.Width / 2 - 43 / 2), rec.Y + 265, 43, 65);

                #region Draw the F Button if you are intersecting with him
                if (Game1.Player.VitalRec.Intersects(lockerRec) && !Game1.g.CurrentChapter.TalkingToNPC && Game1.g.CurrentChapter.state == Chapter.GameState.Game && Game1.g.CurrentChapter.BossFight == false)
                    drawFButton = true;
                else
                    drawFButton = false;

                if (drawFButton)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }

                #endregion

                //If you press F, go to your locker
                if (Game1.Player.VitalRec.Intersects(lockerRec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed()) && Game1.Player.LearnedSkills.Count > 0 && Game1.Player.playerState != Player.PlayerState.attackJumping && Game1.Player.playerState != Player.PlayerState.attacking)
                {
                    Game1.g.YourLocker.LoadContent();
                    Game1.g.CurrentChapter.state = Chapter.GameState.YourLocker;
                }
            }

            if (IsDead())
            {
                StopSound();
                game.ChapterTwo.PlayHorseExplosion();
                game.CurrentChapter.BossFight = false;
                game.CurrentChapter.CurrentBoss = null;
            }
        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }

            return false;
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision)
        {

            hasBeenHit = true;

            ShakeHealthBar();

            damage = 1;

            health -= damage;

            AddDamageNum(damage, collision);

            Chapter.effectsManager.AddDamageFX(10, collision);

            int randomSound = Game1.randomNumberGen.Next(3);
            Sound.PlaySoundInstance(hitSounds.ElementAt(randomSound).Value, hitSounds.ElementAt(randomSound).Key, false, rec.Center.X, rec.Center.Y, 600, 300, 2000);
        }
        public override void StopSound()
        {
            base.StopSound();

            object_bomb_horse_move_loop.Stop();
        }
        public void Move(float speed)
        {
            if (object_bomb_horse_move_loop.State != SoundState.Playing && game.CurrentChapter.state != Chapter.GameState.Cutscene)
            {
                Sound.PlaySoundInstance(object_bomb_horse_move_loop, Game1.GetFileName(() => object_bomb_horse_move_loop), true, rec.Center.X, rec.Center.Y, 600, 500, 3000);
            }

            if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                Chapter.effectsManager.fButtonRecs.Remove(frec);

            frameDelay--;

            if (frameDelay <= 0)
            {
                frameDelay = 5;
                frame++;

                if (frame > 18)
                    frame = 0;
            }

            position.X += speed;

            if (speed > 0)
                facingRight = true;
            else
                facingRight = false;
        }

        public override void DrawHud(SpriteBatch s)
        {
            s.Draw(game.EnemySpriteSheets["FriendHealthBox"], bossHUDRec, Color.White);
            s.Draw(game.EnemySpriteSheets["FriendHealthBar"], healthBarRec, GetHealthSourceRectangle(), Color.White);
            s.Draw(game.EnemySpriteSheets["BossLine"], bossHUDRec, Color.White);

            if (faceTexture != null)
                s.Draw(faceTexture, new Rectangle(1280 - faceTexture.Width + 31, (int)(Game1.aspectRatio * 1280) - faceTexture.Height + 29, faceTexture.Width, faceTexture.Height), Color.White);

            if (drawHUDName)
            {
                Game1.OutlineFont(Game1.questNameFont, s, name, 1, (int)(1070 - (Game1.questNameFont.MeasureString(name).X) + 80), (int)(Game1.aspectRatio * 1280 * .9f) - 26 + 29, Color.Black, Color.White);

            }
        }

        public void DrawHorse(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(textures.ElementAt(frame).Value, rec, Color.White);
            else
                s.Draw(textures.ElementAt(frame).Value, rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}