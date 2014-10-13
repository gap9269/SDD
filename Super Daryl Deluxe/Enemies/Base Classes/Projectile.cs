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
    public class Projectile
    {
        float rotation;
        Vector2 position;
        Rectangle rec;
        int timeInAir;
        Vector2 velocity;
        Texture2D texture;
        int maxTimeInAir;
        Boolean dead = false;
        int damage;
        Vector2 knockback;
        int cameraShake;
        int hitPauseTime;
        int speed;

        public Boolean Dead { get { return dead; } set { dead = value; } }

        public enum ProjType
        {
            arrow,
            goblinSpit
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
            }

            return new Rectangle();
        }

        public void Update()
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

        public void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRectangle(), Color.White, rotation, new Vector2(0, 12), SpriteEffects.None, 0f);
        }
    }
}
