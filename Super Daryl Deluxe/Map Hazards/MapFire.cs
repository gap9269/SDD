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
    class MapFire :MapHazard
    {
        Rectangle damageRec1, damageRec2, damageRec3;
        int frame;
        public static SoundEffect object_fire_trap_01, object_fire_trap_02;
        Boolean alwaysOn = false;
        Boolean ending = false;
        Boolean starting = false;
        Boolean currentlyEnding = false;
        Boolean horizontal = false;
        Boolean facingRight = false;
        Boolean isPassable = true;

        public Boolean CurrentlyEnding { get { return currentlyEnding; } set { currentlyEnding = value; } }

        float rotation;
        public MapFire(int active, int not, int x, int y, Game1 g, int d, Boolean alwaysOn = false, Boolean horizontal = false, Boolean facingRight = true, Boolean isPassable = true)
            : base(x, y, g)
        {
            texture = game.MapHazards["Fire"];
            rec = new Rectangle(x, y, 188, 832);

            if (!horizontal)
            {
                damageRec1 = new Rectangle(x, y + rec.Height - 150, 188, 150);
                damageRec2 = new Rectangle(x, y + rec.Height - 240, 188, 240);
                damageRec3 = new Rectangle(x, y + rec.Height - 450, 188, 450);
            }
            else
            {
                if (facingRight)
                {
                    rotation = 90;
                    damageRec1 = new Rectangle(x, y, 150, 188);
                    damageRec2 = new Rectangle(x, y, 240, 188);
                    damageRec3 = new Rectangle(x, y, 450, 188);
                }
                else
                {
                    rotation = 270;
                    damageRec1 = new Rectangle(rec.X - rec.Width / 2 + 300, rec.Y - 140 / 2, 150, 140);
                    damageRec2 = new Rectangle(rec.X - rec.Width / 2 + (450 - 240), rec.Y - 140 / 2, 240, 140);
                    damageRec3 = new Rectangle(rec.X - rec.Width / 2, rec.Y - 140 / 2, 450, 140);
                }
            }

            timeActive = active;
            timeNotActive = not;
            damage = d;
            this.alwaysOn = alwaysOn;
            this.active = alwaysOn;
            this.horizontal = horizontal;
            this.facingRight = facingRight;
            this.isPassable = isPassable;
        }

        public MapFire(int active, int not, Rectangle r, Game1 g, int d)
            : base(r.X, r.Y, g)
        {
            texture = game.MapHazards["Fire"];
            rec = r;

            damageRec1 = rec;
            damageRec1.Y = rec.Y + rec.Height - 150;
            damageRec1.Height = 150;

            damageRec2 = rec;
            damageRec2.Y = rec.Y + rec.Height - 240;
            damageRec2.Height = 240;

            damageRec3 = rec;
            damageRec3.Y = rec.Y + rec.Height - 450;
            damageRec3.Height = 450;

            timeActive = active;
            timeNotActive = not;
            damage = d;

        }

        /// <summary>
        /// For the always on fire only
        /// </summary>
        public void TurnOff()
        {
            ending = true;
            currentlyEnding = true;
        }

        /// <summary>
        /// For the always on fire only
        /// </summary>
        public void TurnOn()
        {
            active = true;
            timer = 0;
            frameTimer = 5;
            frame = 6;
            int temp = Game1.randomNumberGen.Next(1, 3);

            if (temp == 0)
            {
                Sound.PlaySoundInstance(object_fire_trap_01, Game1.GetFileName(() => object_fire_trap_01), false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
            }
            else
            {
                Sound.PlaySoundInstance(object_fire_trap_02, Game1.GetFileName(() => object_fire_trap_02), false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
            }
        }
        public Rectangle GetSourceRec()
        {
            if (frame < 5)
                return new Rectangle(frame * 188, 0, 188, 832);
            else
                return new Rectangle((frame - 5) * 188, 832, 188, 832);
        }

        public override void Update()
        {
            base.Update();

            if (!alwaysOn)
            {
                if (!active)
                {
                    timer++;

                    if (timer == timeNotActive)
                    {
                        active = true;
                        timer = 0;
                        frameTimer = 5;
                        int temp = Game1.randomNumberGen.Next(1, 3);

                        if (temp == 0)
                        {
                            Sound.PlaySoundInstance(object_fire_trap_01, Game1.GetFileName(() => object_fire_trap_01), false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                        }
                        else
                        {
                            Sound.PlaySoundInstance(object_fire_trap_02, Game1.GetFileName(() => object_fire_trap_02), false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                        }
                    }
                }
                if (active)
                {
                    timer++;
                    frameTimer--;

                    if (timer == timeActive - 15)
                        frame = 19;

                    if (frameTimer == 0)
                    {
                        frame++;
                        frameTimer = 5;
                    }

                    if (frame > 5 && frame < 19)
                        DamagePlayer();

                    if (frame > 18 && timer < timeActive - 15)
                        frame = 14;

                    if (timer == timeActive)
                    {
                        active = false;
                        timer = 0;
                        frame = 0;
                    }
                }
            }
            else
            {
                if (active)
                {

                    if (horizontal && !isPassable)
                    {
                        Rectangle topPlay = new Rectangle((int)Game1.Player.VitalRec.X + 5, (int)Game1.Player.VitalRec.Y, Game1.Player.VitalRec.Width - 5, 10);

                        if (topPlay.Intersects(damageRec3) && active && Game1.Player.VelocityY < 0)
                        {
                            Game1.Player.VelocityY = 0;
                            Game1.Player.VelocityY = GameConstants.GRAVITY;
                            Game1.Player.playerState = Player.PlayerState.jumping;
                        }
                    }

                    else if (!horizontal)
                    {
                        #region Don't pass through it
                        Rectangle rightPlay = new Rectangle((int)Game1.Player.VitalRec.X + Game1.Player.VitalRec.Width, (int)Game1.Player.VitalRec.Y + 5, 25, Game1.Player.VitalRec.Height + 35);
                        Rectangle leftPlay = new Rectangle((int)Game1.Player.VitalRec.X - 25, (int)Game1.Player.VitalRec.Y + 5, 25, Game1.Player.VitalRec.Height + 35);
                        Rectangle left = new Rectangle(rec.X + 50, rec.Y + 300, 20, 450);
                        Rectangle right = new Rectangle(rec.X + rec.Width - 70, rec.Y + 300, 20, 450);

                        if (Game1.Player.KnockedBack)
                        {
                            Rectangle checkPlatRec;

                            if (Game1.Player.VelocityX >= 0)
                            {
                                checkPlatRec = new Rectangle(rightPlay.X, rightPlay.Y, (int)Game1.Player.VelocityX, rightPlay.Height);

                                if (checkPlatRec.Intersects(left))
                                {
                                    //playerState = PlayerState.standing;
                                    Game1.Player.PositionX -= Game1.Player.VelocityX;
                                    Game1.Player.KnockedBack = false;
                                    Game1.Player.VelocityX = 0;
                                    // playerState = PlayerState.standing;
                                }
                            }
                            else
                            {
                                checkPlatRec = new Rectangle(leftPlay.X - Math.Abs((int)Game1.Player.VelocityX), leftPlay.Y, Math.Abs((int)Game1.Player.VelocityX), leftPlay.Height);

                                if (checkPlatRec.Intersects(right))
                                {
                                    // playerState = PlayerState.standing;
                                    Game1.Player.PositionX += Math.Abs(Game1.Player.VelocityX);
                                    Game1.Player.KnockedBack = false;
                                    Game1.Player.VelocityX = 0;
                                    //playerState = PlayerState.standing;
                                }
                            }
                        }

                        if ((rightPlay.Intersects(left) || leftPlay.Intersects(right)))
                        {
                            if (rightPlay.Intersects(left))
                            {

                                if (Game1.Player.playerState != Player.PlayerState.jumping)
                                {
                                    Game1.Player.PositionX -= Game1.Player.MoveSpeed;
                                }
                                else
                                {
                                    Game1.Player.PositionX -= Game1.Player.AirMoveSpeed;
                                }

                                Game1.Player.VelocityX = 0;

                            }

                            if (leftPlay.Intersects(right))
                            {
                                if (Game1.Player.playerState != Player.PlayerState.jumping)
                                {
                                    Game1.Player.PositionX += Game1.Player.MoveSpeed;
                                }
                                else
                                {
                                    Game1.Player.PositionX += Game1.Player.AirMoveSpeed;
                                }
                                Game1.Player.VelocityX = 0;

                            }
                        }


                        #endregion
                    }

                    frameTimer--;

                    if (ending)
                    {
                        ending = false;
                        frame = 19;
                    }

                    if (frameTimer <= 0)
                    {
                        frame++;
                        frameTimer = 5;
                    }

                    if (frame > 5 && frame < 19)
                        DamagePlayer();

                    if (frame > 18 && !currentlyEnding)
                        frame = 14;

                    if (frame > 23)
                    {
                        currentlyEnding = false;
                        active = false;
                        timer = 0;
                        frame = 0;
                    }
                }
            }
        }

        public override void DamagePlayer()
        {
            base.DamagePlayer();

            if (((frame == 6 || frame == 7) && damageRec1.Intersects(Game1.Player.VitalRec)) || ((frame == 8 || frame == 9) && damageRec2.Intersects(Game1.Player.VitalRec)) || (frame > 9 && damageRec3.Intersects(Game1.Player.VitalRec)))
            {
                damage = 20;
                Game1.Player.TakeDamage(damage, 50);
                Vector2 knockback = new Vector2(20, -5);

                if (horizontal)
                {
                    if (facingRight)
                        knockback.X = 30;
                    else
                        knockback.X = -30;

                }
                else
                {

                    if (Game1.Player.VitalRec.Center.X < damageRec1.Center.X)
                        knockback.X = -(knockback.X);
                }

                Game1.Player.KnockPlayerBack(knockback);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 vec = Vector2.Zero;

            if(horizontal)
                vec = new Vector2(rec.Width / 2, rec.Height / 2);

            s.Draw(texture, rec, new Rectangle(3008, 0, 188, 832), Color.White, (float)(rotation * (Math.PI / 180)), vec, SpriteEffects.None, 0);

           if(active)
               s.Draw(texture, rec, GetSourceRec(), Color.White, (float)(rotation * (Math.PI / 180)), vec, SpriteEffects.None, 0);
        }
    }
}
