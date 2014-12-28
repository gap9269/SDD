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
    class StackOfBoxes : BreakableObject
    {
        public StackOfBoxes(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, Object content, float mon, Boolean fore)
            : base(g, x, y, s, pass, hlth, content, mon,fore)
        {
            rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }

        public StackOfBoxes(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, int hlthDrop, float mon, Boolean fore)
            : base(g, x, y, s, pass, hlth, hlthDrop, mon, fore)
        {
            rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }

        public StackOfBoxes(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, String content, float mon, Boolean fore)
            : base(g, x, y, s, pass, hlth, content, mon, fore)
        {
            rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }

        public StackOfBoxes(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, StoryItem story, float mon, Boolean fore)
            : base(g, x, y, s, pass, hlth, story, mon, fore)
        {
            rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }


        public override Rectangle GetSourceRec()
        {
            switch (frameState)
            {
                case 0:
                    return new Rectangle(0, 0, sprite.Width / 3, sprite.Height);
                case 1:
                    return new Rectangle(sprite.Width / 3, 0, sprite.Width / 3, sprite.Height);
                case 2:
                    return new Rectangle((sprite.Width / 3) * 2, 0, sprite.Width / 3, sprite.Height);
            }

            return new Rectangle();
        }

        public override void Update()
        {
            if (!finished)
            {

                if (health < 2)
                    frameState = 2;
                else if (health < 5)
                    frameState = 1;
                else
                    frameState = 0;

                if (health <= 0 && finished == false)
                {
                    Chapter.effectsManager.AddSmokePoof(rec, 2);
                    finished = true;

                    DropHealth();
                    DropMoney();

                    if (drop != null)
                    {
                        EnemyDrop tempDrop = new EnemyDrop(drop as Equipment, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                        game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    }
                    else if (enemyDrop != "" && enemyDrop != null)
                    {
                        EnemyDrop tempDrop = new EnemyDrop(enemyDrop, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                        game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    }
                    else if (storyItem != null)
                    {
                        EnemyDrop tempDrop = new EnemyDrop(storyItem, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                        game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                    }
                }
            }

            base.Update();
        }
    }
}
