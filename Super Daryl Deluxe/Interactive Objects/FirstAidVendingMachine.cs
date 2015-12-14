using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class FirstAidVendingMachine : BreakableObject
    {
        float redDamageAlpha = 0;

        Player player;
        int kitsDropped;

        public FirstAidVendingMachine(Game1 g, int x, int y, Texture2D s, int hlthDrop, float mon, int numKits)
            : base(g, x, y, s, true, 6, hlthDrop, mon, false)
        {
            rec = new Rectangle(x, y, 149, 253);
            this.facingRight = facingRight;

            vitalRec = new Rectangle(rec.X, rec.Y, 149, 253);

            player = Game1.Player;
            kitsDropped = numKits;
        }

        public override void Update()
        {
            if (!finished)
            {
                if (redDamageAlpha > 0)
                    redDamageAlpha -= .04f;

                if (health <= 0 && finished == false)
                {
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.Center.X - 125, rec.Center.Y - 125, 248, 248), 3);
                    finished = true;

                    DropHealth();
                    DropMoney();

                    for(int i = 0; i < kitsDropped; i++)
                        game.CurrentChapter.CurrentMap.Drops.Add(new EnemyDrop("First Aid Kit", new Rectangle(rec.Center.X, rec.Center.Y, 70, 70)));
                }
            }

            base.Update();
        }
        public override void StopSound()
        {
            base.StopSound();
        }
        public override void TakeHit(int damage = 1)
        {
            if (health > 0)
                health-=damage;
            redDamageAlpha = 1f;
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden && !finished)
            {
                base.Draw(s);

                s.Draw(sprite, rec, new Rectangle(0,0, 149, 253), Color.White);

                s.Draw(sprite, rec, new Rectangle(149, 0, 149, 253), Color.White * redDamageAlpha);

            }
        }
    }
}
