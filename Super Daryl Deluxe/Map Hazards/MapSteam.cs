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
    class MapSteam : MapHazard
    {
        Rectangle damageRec1;
        int frame;
        Boolean alwaysOn = false;
        Boolean ending = false;
        Boolean starting = false;
        Boolean currentlyEnding = false;
        Boolean horizontal = false;
        Boolean facingRight = false;
        Boolean isPassable = true;
        public SoundEffectInstance object_steam_vent_loop;

        public Boolean CurrentlyEnding { get { return currentlyEnding; } set { currentlyEnding = value; } }

        float rotation;
        public MapSteam(int active, int not, int x, int y, Game1 g, int d, Boolean alwaysOn = false, Boolean horizontal = false, Boolean facingRight = true, Boolean isPassable = true)
            : base(x, y, g)
        {
            texture = game.MapHazards["Map Steam"];
            rec = new Rectangle(x, y, 401, 444);

            if (!horizontal)
            {
                damageRec1 = new Rectangle(rec.X + 107, Rec.Y + 60, 188, 300);
            }
            else
            {
                if (facingRight)
                {
                    rotation = 90;
                    damageRec1 = new Rectangle(x, y, 150, 188);
                }
                else
                {
                    rotation = 270;
                    damageRec1 = new Rectangle(rec.X - 160, rec.Y - 140 / 2, 290, 140);
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
        /// <summary>
        /// For the always on fire only
        /// </summary>
        public void TurnOff()
        {
            ending = true;
            currentlyEnding = true;
            Sound.PlaySoundInstance(Sound.SoundNames.object_steam_vent_stop, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
            object_steam_vent_loop.Stop();
        }

        /// <summary>
        /// For the always on fire only
        /// </summary>
        public void TurnOn()
        {
            starting = true;
            active = true;
            timer = 0;
            frameTimer = 5;
            frame = 0;
            int temp = Game1.randomNumberGen.Next(1, 3);
            Sound.PlaySoundInstance(Sound.SoundNames.object_steam_vent_start, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);

        }
        public Rectangle GetSourceRec()
        {
            return new Rectangle(frame * 401, 0, 401, 444);
        }

        public override void StopSounds()
        {
            object_steam_vent_loop.Stop();
        }

        public override void Update()
        {
            base.Update();
            float disToPlayer = Vector2.Distance(Game1.Player.Rec.Center.ToVector2(), rec.Center.ToVector2());
            if (active && !ending && !currentlyEnding && object_steam_vent_loop.State != SoundState.Playing && disToPlayer < 1280)
                Sound.PlaySoundInstance(object_steam_vent_loop, Game1.GetFileName(() => object_steam_vent_loop), true, rec.Center.X, rec.Center.Y, 600, 500, 1280, false, 2);
            if (object_steam_vent_loop.State == SoundState.Playing)
                Sound.UpdatePanAndVolume(object_steam_vent_loop, "object_steam_vent_loop", rec.Center.X, rec.Center.Y, 600, 500, 1280);
            if (disToPlayer >= 1280 && object_steam_vent_loop.State == SoundState.Playing)
                object_steam_vent_loop.Stop();

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
                    }
                }
                if (active)
                {
                    timer++;
                    frameTimer--;

                    if (timer == timeActive - 15)
                        frame = 5;

                    if (frameTimer == 0)
                    {
                        frame++;
                        frameTimer = 5;
                    }

                    if (frame < 5)
                        DamagePlayer();

                    if (frame > 4 && timer < timeActive - 15)
                        frame = 3;

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

                        if (topPlay.Intersects(damageRec1) && active && Game1.Player.VelocityY < 0)
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
                        Rectangle left = new Rectangle(rec.X + 140, rec.Y + 50, 20, rec.Height - 50);
                        Rectangle right = new Rectangle(rec.X + rec.Width - 170, rec.Y + 50, 20, rec.Height - 50);

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
                        frame = 5;
                    }

                    if (frameTimer <= 0)
                    {
                        frame++;
                        frameTimer = 5;
                    }

                    if ( frame < 4)
                        DamagePlayer();

                    if (frame > 4 && !currentlyEnding)
                    {
                        if (starting)
                            starting = false;
                        frame = 0;
                    }
                    if (frame > 7)
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

            if (frame < 5 && damageRec1.Intersects(Game1.Player.VitalRec))
            {
                Game1.Player.TakeDamage(damage, 50);
                Vector2 knockback = new Vector2(20, -5);

                if (horizontal)
                {
                    if (facingRight)
                        knockback.X = 40;
                    else
                        knockback.X = -40;

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

            if (horizontal)
                vec = new Vector2(rec.Width / 2, rec.Height / 2);

            s.Draw(texture, rec, new Rectangle(1203, 444, 401, 444), Color.White, (float)(rotation * (Math.PI / 180)), vec, SpriteEffects.None, 0);

            if (active)
            {

                if(frame < 3 && starting)
                    s.Draw(texture, rec, new Rectangle(frame * 401, 444, 401, 444), Color.White, (float)(rotation * (Math.PI / 180)), vec, SpriteEffects.None, 0);

                s.Draw(texture, rec, GetSourceRec(), Color.White, (float)(rotation * (Math.PI / 180)), vec, SpriteEffects.None, 0);
            }
        }
    }
}
