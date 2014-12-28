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
    public class Projectile
    {
        protected float rotation;
        protected Vector2 position;
        protected Rectangle rec;
        protected int timeInAir;
        protected Vector2 velocity;
        protected Texture2D texture;
        protected int maxTimeInAir;
        protected Boolean dead = false;
        protected int damage;
        protected Vector2 knockback;
        protected int cameraShake;
        protected int hitPauseTime;
        protected int speed;

        public Boolean Dead { get { return dead; } set { dead = value; } }

        public enum ProjType
        {
            arrow,
            goblinSpit,
            bomb
        }
        public ProjType projectileType;

        public Projectile(int x, int y, int time, Vector2 vel, float rot, int dam, Vector2 kb, int hitPause, int shake, int spd, ProjType type)
        {
            texture = Game1.projectileTextures;
            projectileType = type;

            switch (projectileType)
            {
                case ProjType.goblinSpit:
                    rec = new Rectangle(x, y, 48, 25);
                    break;

                case ProjType.arrow:
                    rec = new Rectangle(x, y, 169, 48);
                    break;

                case ProjType.bomb:
                    rec = new Rectangle(x, y, 47, 63);
                    break;
            }
            position = new Vector2(x, y);
            knockback = kb;
            damage = dam;
            maxTimeInAir = time;
            velocity = vel;
            speed = spd;
            rotation = rot;

        }

        public Rectangle GetSourceRectangle()
        {
            switch (projectileType)
            {
                case ProjType.goblinSpit:
                    return new Rectangle(0, 0, 48, 25);

                case ProjType.arrow:
                    return new Rectangle(53, 0, 169, 48);

                case ProjType.bomb:
                    return new Rectangle(0, 48, 47, 63);
            }

            return new Rectangle();
        }

        public virtual void Update()
        {

            timeInAir++;
            position += velocity * speed;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            if (timeInAir >= maxTimeInAir)
            {
                dead = true;
            }

            if (Game1.Player.CheckIfHit(rec) && Game1.Player.InvincibleTime <= 0)
            {
                dead = true;
                Game1.Player.TakeDamage(damage);
                Game1.Player.KnockPlayerBack(knockback);
                Game1.Player.HitPauseTimer = hitPauseTime;
                Game1.camera.ShakeCamera(3, cameraShake);
                MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
            }
        }

        public virtual void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRectangle(), Color.White, rotation, new Vector2(0, 12), SpriteEffects.None, 0f);
        }
    }
}
