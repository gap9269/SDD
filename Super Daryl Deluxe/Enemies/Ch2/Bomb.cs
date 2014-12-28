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
    public class Bomb : Projectile
    {
        int timeUntilExplode = 80;
        Boolean inAir = true;
        Boolean resting = false;
        int collisionDamage;

        int frame;
        int frameDelay = 3;

        public Bomb(int x, int y, Vector2 vel, int dam, Vector2 kb, int collisionDamage) 
            : base (x, y, -1, vel, 0, dam, kb, 1, 1, -1, ProjType.bomb)
        {
            this.collisionDamage = collisionDamage;
        }

        public override void Update()
        {
            rotation += velocity.X * .75f;
            if (rotation == 360)
                rotation = 0;

            frameDelay--;

            if (frameDelay <= 0)
            {
                frame++;
                frameDelay = 5;

                if (frame > 4)
                    frame = 0;
            }

            if (inAir)
            {
                timeInAir++;
            }
            position.X = rec.X;
            position.Y = rec.Y;

            rec.X += (int)velocity.X;
            rec.Y += (int)velocity.Y;

            velocity.Y += GameConstants.GRAVITY;
         
            //--Check to see if it is colliding with a platform
            for (int i = 0; i < Game1.currentChapter.CurrentMap.Platforms.Count; i++)
            {

                Platform plat = Game1.currentChapter.CurrentMap.Platforms[i];

                //Sides of the current plat it is checking
                Rectangle top = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + 15, plat.Rec.Width - 5, 10);
                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle bombBottom = new Rectangle(rec.X, rec.Y + rec.Height -25, rec.Width, 10);
                Rectangle bombTop = new Rectangle(rec.X + 5, rec.Y, rec.Width, 10);

                if (bombTop.Intersects(top) || bombBottom.Intersects(bottom))
                {
                    if (inAir)
                    {
                        inAir = false;
                    }

                    rec.Y -= (int)(velocity.Y * .5f);
                    velocity.Y = -velocity.Y *.5f;
                    velocity.X = velocity.X * .5f;


                    Vector2 temp = velocity;
                    if (Math.Abs(temp.X) < 1.5f)
                    {
                        velocity.X = 0;
                    }

                    if (Math.Abs(temp.Y) < 1.5f)
                    {
                        Console.WriteLine(rec.X - 2037);
                        resting = true;
                        velocity.Y = 0;
                    }
                }

                if (rec.Intersects(left) || rec.Intersects(right))
                {
                    velocity.X = -velocity.X;
                }
            }

            if (resting)
            {
                timeUntilExplode--;

                if (timeUntilExplode <= 0)
                {
                    if (Game1.Player.CheckIfHit(new Rectangle(rec.X + rec.Width / 2 - 105, rec.Y + rec.Height / 2 - 105, 210, 210)) && Game1.Player.InvincibleTime <= 0)
                    {
                        Game1.Player.TakeDamage(damage);

                        if (rec.Center.X < Game1.Player.VitalRec.Center.X)
                            Game1.Player.KnockPlayerBack(new Vector2(25, -8));
                        else
                            Game1.Player.KnockPlayerBack(new Vector2(-25, -8));

                        Game1.Player.HitPauseTimer = 3;
                        Game1.camera.ShakeCamera(3, 5);
                        MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                    }

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + rec.Width / 2 - 75, rec.Y + rec.Height / 2 - 45, 150, 150), 3);
                    dead = true;
                }
            }

            if (Game1.Player.CheckIfHit(rec) && Game1.Player.InvincibleTime <= 0 && velocity.X > 7)
            {
                Game1.Player.TakeDamage(collisionDamage);
                
                if(rec.Center.X < Game1.Player.VitalRec.Center.X)
                    Game1.Player.KnockPlayerBack(new Vector2(3, -3));
                else
                    Game1.Player.KnockPlayerBack(new Vector2(-3, -3));

                Game1.Player.HitPauseTimer = 1;
                Game1.camera.ShakeCamera(3, 1);
                MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, new Rectangle(46 * frame, 48, 46, 67), Color.White, (float)(rotation * (Math.PI / 180)), new Vector2(rec.Width / 2, rec.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
