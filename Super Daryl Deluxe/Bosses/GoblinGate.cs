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
    class GoblinGate : Boss
    {

        // CONSTRUCTOR \\
        public GoblinGate(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 80;
            maxHealth = 80;
            level = 5;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, 100, 600);
            vitalRec = new Rectangle((int)position.X, (int)position.Y, 100, 600);

            addToHealthWidth = game.EnemySpriteSheets["BossHealthBar"].Width;
            drawHUDName = true;
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
        }

        public override Rectangle GetHealthSourceRectangle()
        {
            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision)
        {
            if (canBeHurt)
            {
                hasBeenHit = true;

                ShakeHealthBar();

                damage = 1;

                health -= damage;

                AddDamageNum(damage, collision);
            }
        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }

            return false;
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            Move(currentMap.MapWidth);

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            vitalRec.X = rec.X;
            vitalRec.Y = rec.Y;

            if (IsDead())
            {
                game.CurrentChapter.BossFight = false;
                game.CurrentChapter.CurrentBoss = null;
            }
        }

        public override void ImplementGravity()
        {
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.whiteFilter, rec, Color.White);
        }


    }
}