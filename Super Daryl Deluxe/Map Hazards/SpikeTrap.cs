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
    class SpikeTrap :MapHazard
    {
        Rectangle damageRec1;
        int frame;
        public static SoundEffect object_fire_trap_01, object_fire_trap_02;
        Boolean ending = false;
        Boolean starting = false;
        Boolean currentlyEnding = false;
        Boolean horizontal = false;

        public Boolean CurrentlyEnding { get { return currentlyEnding; } set { currentlyEnding = value; } }

        float rotation;

        public SpikeTrap(int not, int x, int y, Game1 g, int d)
            : base(x, y, g)
        {
            texture = game.MapHazards["Spike Trap"];
            rec = new Rectangle(x, y, 308, 162);

            damageRec1 = rec;

            timeNotActive = not;
            damage = d;
        }

        public Rectangle GetSourceRec()
        {
            return new Rectangle(frame * 308, 0, 308, 162);
        }

        public override void Update()
        {
            base.Update();

            if (!active)
            {
                if (frame != 0)
                {
                    frameTimer--;

                    if (frameTimer <= 0)
                    {
                        frame--;
                        frameTimer = 2;
                    }
                }

                timer++;
                if (timer == timeNotActive)
                {
                    frame = 1;
                    active = true;
                    timer = 0;
                    frameTimer = 60;
                }
            }
            if (active)
            {
                frameTimer--;

                if (frameTimer == 0 && frame < 3)
                {
                    frame++;
                    frameTimer = 2;
                }

                if (frame > 1)
                    DamagePlayer();

                if (frame == 3)
                    timer++;

                if (timer == 45)
                {
                    active = false;
                    timer = 0;
                    frameTimer = 3;
                }
            }
        }
        

        public override void DamagePlayer()
        {
            base.DamagePlayer();

            if (frame > 1 && damageRec1.Intersects(Game1.Player.VitalRec))
            {
                Game1.Player.TakeDamage(damage, 50);
                Vector2 knockback = new Vector2(20, -5);

                if (Game1.Player.VitalRec.Center.X < damageRec1.Center.X)
                    knockback.X = -(knockback.X);


                Game1.Player.KnockPlayerBack(knockback);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRec(), Color.White, (float)(rotation * (Math.PI / 180)), Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
