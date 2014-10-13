using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class MapFire :MapHazard
    {
        Rectangle damageRec1, damageRec2, damageRec3;
        int frame;

        public MapFire(int active, int not, int x, int y, Game1 g, int d)
            : base(x, y, g)
        {
            texture = game.MapHazards["Fire"];
            rec = new Rectangle(x, y, 188, 832);
            damageRec1 = new Rectangle(x, y + rec.Height - 150, 188, 150);
            damageRec2 = new Rectangle(x, y + rec.Height - 240, 188, 240);
            damageRec3 = new Rectangle(x, y + rec.Height - 450, 188, 450);
            timeActive = active;
            timeNotActive = not;
            damage = d;
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

        public override void DamagePlayer()
        {
            base.DamagePlayer();

            if (((frame == 6 || frame == 7) && damageRec1.Intersects(Game1.Player.VitalRec)) || ((frame == 8 || frame == 9) && damageRec2.Intersects(Game1.Player.VitalRec)) || (frame > 9 && damageRec3.Intersects(Game1.Player.VitalRec)))
            {
                Game1.Player.TakeDamage(damage);
                Vector2 knockback = new Vector2(20, -5);

                if (Game1.Player.VitalRec.Center.X < damageRec1.Center.X)
                    knockback.X = -(knockback.X);

                Game1.Player.KnockPlayerBack(knockback);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, new Rectangle(3008, 0, 188, 832), Color.White);

            if(active)
                s.Draw(texture, rec, GetSourceRec(), Color.White);
        }
    }
}
