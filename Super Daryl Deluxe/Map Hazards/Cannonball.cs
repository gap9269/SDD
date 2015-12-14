using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Cannonball
    {

        public Boolean finished = false;
        public Rectangle rec, damageRec;
        int damage, moveframe;
        int frameDelay = 5;
        Texture2D spritesheet;
        Boolean facingRight;
        public Boolean foreground;
        public Cannonball(int x, int y, Texture2D sprite, int damage, Boolean facingRight, Boolean foreground = false)
        {
            rec = new Rectangle(x, y, 921, 720);
            spritesheet = sprite;
            this.damage = damage;
            this.facingRight = facingRight;
            this.foreground = foreground;

            damageRec = new Rectangle(x + 277, y + 407, 421, 288);
        }

        public Rectangle GetSourceRec()
        {
            if (moveframe < 4)
                return new Rectangle(moveframe * 921, 0, 921, 720);
            else if (moveframe < 8)
                return new Rectangle((moveframe - 4) * 921, 720, 921, 720);
            else if (moveframe < 12)
                return new Rectangle((moveframe - 8) * 921, 1440, 921, 720);
            else
                return new Rectangle((moveframe - 12) * 921, 2160, 921, 720);
        }

        public void Update()
        {
            frameDelay--;

            if (frameDelay <= 0)
            {
                moveframe++;
                frameDelay = 5;

                if (moveframe == 8 || moveframe == 9)
                {
                    if (moveframe == 8)
                    {
                        //Shake the screen based on how close the troll is to you
                        float stepShakeMag = 1000 / Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(Game1.Player.Rec.Center.X, Game1.Player.Rec.Center.Y));
                        if (stepShakeMag < 1)
                            stepShakeMag = 0f;
                        if (stepShakeMag > 4)
                            stepShakeMag = 4f;

                        stepShakeMag *= 3;

                        Game1.camera.ShakeCamera(10, stepShakeMag);
                    }
                    if (Game1.Player.CheckIfHit(damageRec) && Game1.Player.InvincibleTime <= 0)
                    {
                        Vector2 kb;

                        if (damageRec.Center.X < Game1.Player.VitalRec.Center.X)
                            kb = new Vector2(25, -8);
                        else
                            kb = new Vector2(-25, -8);

                        Game1.Player.TakeDamage(damage, 10);
                        Game1.Player.KnockPlayerBack(kb);
                        Game1.Player.HitPauseTimer = 3;
                        Game1.camera.ShakeCamera(2, 2);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(damageRec, Game1.Player.VitalRec));
                    }
                }
                else if (moveframe == 15)
                {
                    finished = true;
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(spritesheet, rec, GetSourceRec(), Color.White);
            else
                s.Draw(spritesheet, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

        }
    }
}
