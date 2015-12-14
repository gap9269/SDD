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
    public class Boombox : Projectile
    {
        public enum State
        {
            inAir,
            sliding,
            resting,
            disappearing
        }
        public State state;
        int health;
        int tolerance;

        int collisionDamage;

        int frame;
        int frameDelay = 5;

        public Rectangle vitalRec;

        public Boombox(int x, int y, Vector2 vel, int dam, Vector2 kb, int collisionDamage, int level, int health, int tolerance, Boolean facingRight)
            : base(x, y, -1, vel, 0, dam, kb, 1, 1, -1, ProjType.boombox, level)
        {
            this.collisionDamage = collisionDamage;
            this.health = health;
            this.tolerance = tolerance;
            this.facingRight = facingRight;
            vitalRec = new Rectangle(0, 0, 97, 55);
        }

        public Rectangle GetSourceRectangle()
        {
            switch (state)
            {
                case State.inAir:
                case State.sliding:
                    return new Rectangle(0, 900, 172, 99);
                case State.resting:
                    return new Rectangle(172 * frame, 999, 172, 99);
                case State.disappearing:
                    return new Rectangle(1032+(172 * frame), 999, 172, 99);
            }

            return new Rectangle();
        }

        public void StartDisappearing()
        {
            frame = 0;
            frameDelay = 5;
            state = State.disappearing;
            
        }

        public override void Update()
        {
           
            switch (state)
            {
                case State.inAir:
                    timeInAir++;
                    velocity.Y += GameConstants.GRAVITY * .75f;
                    CheckPlatforms();

                    if (Game1.Player.CheckIfHit(vitalRec) && Game1.Player.InvincibleTime <= 0)
                    {
                        Game1.Player.TakeDamage(collisionDamage, level);

                        Game1.Player.KnockPlayerBack(new Vector2(velocity.X, -5));

                        Game1.Player.HitPauseTimer = 3;
                        Game1.camera.ShakeCamera(3, 1);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                    }
                    break;
                case State.sliding:
                    if (velocity.X > 0)
                        velocity.X -= .15f;
                    else if(velocity.X < 0)
                        velocity.X += .15f;

                    CheckPlatforms();

                    if (Math.Abs(velocity.X) <= 1)
                    {
                        velocity.X = 0;
                        state = State.resting;
                    }
                    break;
                case State.resting:
                    
                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        frame++;
                        frameDelay = 5;

                        if (frame > 5)
                            frame = 0;
                    }

                    if (Game1.Player.CheckIfHit(rec) && Game1.Player.InvincibleTime <= 0)
                    {
                        Game1.Player.TakeDamage((int)(collisionDamage * .75f), level);

                        if(Game1.Player.VitalRec.Center.X < rec.Center.X)
                            Game1.Player.KnockPlayerBack(new Vector2(-10, -5));
                        else
                            Game1.Player.KnockPlayerBack(new Vector2(10, -5));

                        Game1.Player.HitPauseTimer = 1;
                        Game1.camera.ShakeCamera(3, 1);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                    }
                    break;
                case State.disappearing:

                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        frame++;
                        frameDelay = 5;

                        if (frame > 6)
                            frame = 0;
                    }
                    break;
            }

            rec.X += (int)velocity.X;
            rec.Y += (int)velocity.Y;

            position.X = rec.X;
            position.Y = rec.Y;

            vitalRec.X = rec.X + 37;
            vitalRec.Y = rec.Y + 32;
        }

        public void CheckPlatforms()
        {
            //--Check to see if it is colliding with a platform
            for (int i = 0; i < Game1.currentChapter.CurrentMap.Platforms.Count; i++)
            {

                Platform plat = Game1.currentChapter.CurrentMap.Platforms[i];

                //Sides of the current plat it is checking
                Rectangle top = new Rectangle(plat.Rec.X + 5, plat.Rec.Y, plat.Rec.Width - 5, 20);
                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle bombBottom = new Rectangle(rec.X, rec.Y + rec.Height - 20, rec.Width, 20);

                if (bombBottom.Intersects(top))
                {
                    if (state == State.inAir)
                    {
                        state = State.sliding;
                        velocity.X = velocity.X / 4f;
                        velocity.Y = 0;
                        rec.Y = top.Y - rec.Height;
                    }
                }

                if (rec.Intersects(left) || rec.Intersects(right))
                {
                    velocity.X = -(velocity.X / 4);
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(Game1.g.EnemySpriteSheets["Captain Sax"], rec, GetSourceRectangle(), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            else
                s.Draw(Game1.g.EnemySpriteSheets["Captain Sax"], rec, GetSourceRectangle(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

        }
    }
}
