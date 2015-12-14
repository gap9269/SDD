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
    public class GhostSucker : Boss
    {
        public int health, maxHealth;
        int frame;
        int frameDelay = 5;
        Rectangle frec;
        Boolean facingRight = true;
        Boolean activated = false;

        Dictionary<String, SoundEffect> hitSounds;

        SoundEffectInstance object_ghost_sucker_loop;

        int flinchTimer = 0;

        public static Texture2D hudFaceTexture;

        public GhostSucker(int x, int y, Player p, MapClass cur) 
            : base(new Vector2(x, y), "Ghost Sucker", Game1.g, ref p, cur)
        {
            health = maxHealth = 50;
            position = new Vector2(x, y);
            rec = new Rectangle(x, y, 363, 166);
            vitalRec = new Rectangle(rec.X, rec.Y, 100, 166);

            drawHUDName = true;

            addToHealthWidth = game.EnemySpriteSheets["BossHealthBar"].Width;
            canBeHurt = false;
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>(@"InteractiveObjects\Ghost Sucker");
            hitSounds = new Dictionary<String, SoundEffect>();
            hitSounds.Add("object_ghost_sucker_damage_01", content.Load<SoundEffect>(@"Sound\Objects\object_ghost_sucker_damage_01"));
            hitSounds.Add("object_ghost_sucker_damage_02", content.Load<SoundEffect>(@"Sound\Objects\object_ghost_sucker_damage_02"));

            object_ghost_sucker_loop = content.Load<SoundEffect>(@"Sound\Objects\object_ghost_sucker_loop").CreateInstance();

        }

        public override Rectangle GetHealthSourceRectangle()
        {
            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override void StopSound()
        {
            base.StopSound();

            object_ghost_sucker_loop.Stop();
        }

        public override void Update(int mapwidth)
        {
            UpdateShakeHealthBar();
            canBeHurt = false;

            if (flinchTimer > 0)
                flinchTimer--;

            if (object_ghost_sucker_loop.State != SoundState.Playing)
            {
                Sound.PlaySoundInstance(object_ghost_sucker_loop, Game1.GetFileName(() => object_ghost_sucker_loop), true, rec.Center.X, rec.Center.Y, 600, 500, 2500, false);
            }

            frameDelay--;

            if (frameDelay <= 0)
            {
                frame++;
                frameDelay = 3;

                if (frame > 2)
                    frame = 0;
            }

            if (hasBeenHit)
            {
                targetHealthWidth = (int)((float)originalHealthWidth * ((float)health / (float)maxHealth));
                addToHealthWidth += ((targetHealthWidth - healthBarRec.Width) * .07f); //Must add it to this because converting it to a int cuts off numbers
                healthBarRec.Width = (int)addToHealthWidth;
            }

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            rec.X = (int)position.X;
            rec.Y = (int)position.Y;
            vitalRec = new Rectangle(rec.X, rec.Y, 100, 166);

            if (IsDead())
            {
                StopSound();
                game.ChapterOne.PlayGhostSuckedDestroyed();
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
            flinchTimer = 15;
            damage = 1;

            health -= damage;

            AddDamageNum(damage, collision);

            Chapter.effectsManager.AddDamageFX(10, collision);

            int randomSound = Game1.randomNumberGen.Next(3);
            Sound.PlaySoundInstance(hitSounds.ElementAt(randomSound).Value, hitSounds.ElementAt(randomSound).Key, false, rec.Center.X, rec.Center.Y, 600, 300, 2000);
        }

        public void Move(float speed)
        {
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
                s.Draw(faceTexture, new Rectangle(1280 - faceTexture.Width, (int)(Game1.aspectRatio * 1280) - faceTexture.Height, faceTexture.Width, faceTexture.Height), Color.White);

            if (drawHUDName)
            {
                s.DrawString(Game1.questNameFont, name, new Vector2(1045 - (Game1.font.MeasureString(name).X), (int)(Game1.aspectRatio * 1280 * .9f) - 26), Color.Black);
                s.DrawString(Game1.questNameFont, name, new Vector2(1046 - (Game1.font.MeasureString(name).X), (int)(Game1.aspectRatio * 1280 * .9f) - 28), Color.White);

            }
        }

        public void DrawGhostSucker(SpriteBatch s)
        {
            if (flinchTimer <= 0)
            {
                if (facingRight)
                    s.Draw(spriteSheet, rec, new Rectangle(363 * frame, 0, 363, 166), Color.White);
                else
                    s.Draw(spriteSheet, rec, new Rectangle(363 * frame, 0, 363, 166), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                if (facingRight)
                    s.Draw(spriteSheet, rec, new Rectangle(1089, 0, 363, 166), Color.White);
                else
                    s.Draw(spriteSheet, rec, new Rectangle(1089, 0, 363, 166), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}