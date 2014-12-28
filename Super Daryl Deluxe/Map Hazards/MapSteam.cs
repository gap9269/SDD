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
        Rectangle damageRec;
        int frame;
        Vector2 knockBack;
        Boolean passable;
        Boolean horizontal;
        Boolean facingUpOrRight;

        public MapSteam(Rectangle rec, Game1 g, int d, Vector2 knock, bool pass, bool hori, bool upOrRight)
            : base(rec.X, rec.Y, g)
        {
            //rec = new Rectangle(x, y, 88, 500);
            //damageRec = new Rectangle(x, y, 88, 450);
            this.rec = rec;
            damageRec = new Rectangle(rec.X + 15, rec.Y, rec.Width - 30, rec.Height) ;
            damage = d;
            knockBack =  knock;
            active = true;
            frameTimer = 1;
            passable = pass;
            horizontal = hori;
            facingUpOrRight = upOrRight;

            if(!horizontal)
                texture = game.MapHazards["Fire"];
            else
                texture = game.MapHazards["HorizontalFire"];
        }

        public Rectangle GetSourceRec()
        {
            if (horizontal == false)
            {
                switch (frame)
                {
                    case 0: return new Rectangle(0, 0, 88, 500);

                    case 1: return new Rectangle(88, 0, 88, 500);

                    case 2: return new Rectangle(176, 0, 88, 500);

                    case 3: return new Rectangle(264, 0, 88, 500);

                    case 4: return new Rectangle(352, 0, 88, 500);

                }
            }
            else
            {
                switch (frame)
                {
                    case 0: return new Rectangle(0, 0, 500, 88);

                    case 1: return new Rectangle(0, 88, 500, 88);

                    case 2: return new Rectangle(0, 176, 500, 88);

                    case 3: return new Rectangle(0, 264, 500, 88);

                    case 4: return new Rectangle(0, 352, 500, 88);

                }
            }

            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (!active)
            {
                frame = 0;
            }

            if (active)
            {
                frameTimer--;

                if (frameTimer == 0)
                {
                    frame++;

                    if (frame == 1)
                        frameTimer = 15;
                    else
                        frameTimer = 5;
                }

                if (frame > 1)
                    DamagePlayer();

                if (frame == 5)
                    frame = 3;
            }

            //--Don't let the player pass through if it isn't passable
            if (!passable)
            {
                //--If the hazard is vertial, you can't pass through the sides
                if (!horizontal)
                {
                    Rectangle rightPlay = new Rectangle((int)Game1.Player.VitalRec.X + Game1.Player.VitalRec.Width, (int)Game1.Player.VitalRec.Y + 5, 15, Game1.Player.VitalRec.Height);
                    Rectangle leftPlay = new Rectangle((int)Game1.Player.VitalRec.X, (int)Game1.Player.VitalRec.Y + 5, 15, Game1.Player.VitalRec.Height);


                    if (rightPlay.Intersects(damageRec) && active)
                    {
                        Game1.Player.PositionX -= Game1.Player.MoveSpeed;
                    }
                    else if (leftPlay.Intersects(damageRec) && active)
                    {
                        Game1.Player.PositionX += Game1.Player.MoveSpeed;
                    }
                }
                //--If it is horizontal, you can't jump up through it
                else
                {
                    Rectangle topPlay = new Rectangle((int)Game1.Player.VitalRec.X + 5, (int)Game1.Player.VitalRec.Y, Game1.Player.VitalRec.Width - 5, 10);

                    if (topPlay.Intersects(damageRec) && active && Game1.Player.VelocityY < 0)
                    {
                        Game1.Player.VelocityY = 0;
                        Game1.Player.VelocityY = GameConstants.GRAVITY;
                        Game1.Player.playerState = Player.PlayerState.jumping;
                    }
                }
            }
        }

        public override void DamagePlayer()
        {
            base.DamagePlayer();

            if (rec.Intersects(Game1.Player.VitalRec))
            {
                Game1.Player.TakeDamage(damage);
                //Vector2 knockback = new Vector2(20, -5);

                if (!horizontal)
                {
                    if (Game1.Player.VitalRec.Center.X < damageRec.Center.X)
                        knockBack.X = -(knockBack.X);
                }

                Game1.Player.KnockPlayerBack(knockBack);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if (facingUpOrRight)
                s.Draw(texture, rec, GetSourceRec(), Color.White);
            else
            {
                if(!horizontal)
                    s.Draw(texture, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
                else
                    s.Draw(texture, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}