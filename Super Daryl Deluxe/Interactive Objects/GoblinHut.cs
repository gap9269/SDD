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
    class GoblinHut : BreakableObject
    {
        int frameDelay = 5;
        int frame;

        int poofFrame;
        int poofDelay = 5;

        static Random rng;

        Player player;

        Boolean smokePoofShowing = false;


        public GoblinHut(Game1 g, int x, int y, Texture2D s, int hlthDrop, float mon, Boolean facingRight)
            : base(g, x, y, s, true, 2, hlthDrop, mon, false)
        {
            rng = new Random();
            rec = new Rectangle(x, y - 610, 655, 610);
            this.facingRight = facingRight;

            if(facingRight)
                vitalRec = new Rectangle(rec.X + 100, rec.Y + 260, 360, 300);
            else
                vitalRec = new Rectangle(rec.X + 200, rec.Y + 260, 360, 300);

            player = Game1.Player;

        }

        public override Rectangle GetSourceRec()
        {
            if(frameState == 0)
                return new Rectangle(0, 0, 655, 610);
            else if(frameState == 1)
                return new Rectangle(655, 0, 655, 610);
            else
                return new Rectangle(1310, 0, 655, 610);
        }

        public override void Update()
        {
            if (smokePoofShowing && poofFrame < 6 )
            {
                poofDelay--;

                if (poofDelay == 0)
                {
                    poofDelay = 5;
                    poofFrame++;
                }
            }

            if (!finished)
            {
                if (health > maxHealth / 2)
                    frameState = 0;
                else if (health > 0)
                    frameState = 1;

                if (health <= 0 && finished == false)
                {
                    if(facingRight)
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + 150, rec.Y + 350, 248, 248), 3);
                    else
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + 250, rec.Y + 350, 248, 248), 3);
                    finished = true;

                    DropHealth();
                    DropMoney();

                    int spawnGoblinNumber = rng.Next(0, 7);

                    switch (spawnGoblinNumber)
                    {
                        case 0:
                            break;
                        case 1: //One goblin
                            Goblin ben = new Goblin(new Vector2(vitalRec.Center.X, vitalRec.Center.Y + 30), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben.Hostile = false;
                            ben.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben);
                            break;
                        case 2: //Two regular goblins
                            Goblin ben2 = new Goblin(new Vector2(vitalRec.Center.X - 75, vitalRec.Center.Y + 30), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben2.Hostile = false;
                            ben2.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben2);

                            Goblin ben3 = new Goblin(new Vector2(vitalRec.Center.X + 75, vitalRec.Center.Y + 30), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben3.Hostile = false;
                            ben3.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben3);
                            break;
                        case 3: //One  bomb goblin
                            Bomblin ben4 = new Bomblin(new Vector2(vitalRec.Center.X, vitalRec.Center.Y - 250), "Bomblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben4.Hostile = false;
                            ben4.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben4);
                            break;
                        case 4: //Two bomb goblins
                            Bomblin ben5 = new Bomblin(new Vector2(vitalRec.Center.X - 75, vitalRec.Center.Y - 250), "Bomblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben5.Hostile = false;
                            ben5.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben5);

                            Bomblin ben6 = new Bomblin(new Vector2(vitalRec.Center.X + 75, vitalRec.Center.Y - 250), "Bomblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben6.Hostile = false;
                            ben6.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben6);
                            break;
                        case 5: //One each
                            Goblin ben7 = new Goblin(new Vector2(vitalRec.Center.X - 75, vitalRec.Center.Y + 30), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben7.Hostile = false;
                            ben7.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben7);

                            Bomblin ben8 = new Bomblin(new Vector2(vitalRec.Center.X + 75, vitalRec.Center.Y - 250), "Bomblin", game, ref player, game.CurrentChapter.CurrentMap);
                            ben8.Hostile = false;
                            ben8.SpawnWithPoof = false;
                            game.CurrentChapter.CurrentMap.EnemiesInMap.Add(ben8);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                frameState = 2;

                frameDelay--;

                if (frameDelay == 0)
                {
                    frame++;
                    frameDelay = 6;

                    if (frame > 5)
                        frame = 0;
                }
            }

            base.Update();
        }

        public override void TakeHit()
        {
            if (health > 0)
                health--;

            if (health == maxHealth / 2)
            {
                smokePoofShowing = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (facingRight)
            {
                if (frameState != 2)
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);
                else
                    s.Draw(sprite, rec, new Rectangle(655 * frame, 1220, 655, 610), Color.White);

                if (smokePoofShowing && poofFrame < 6)
                {
                    s.Draw(sprite, rec, new Rectangle(655 * poofFrame, 610, 655, 610), Color.White);
                }
            }
            else
            {
                if (frameState != 2)
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                else
                    s.Draw(sprite, rec, new Rectangle(655 * frame, 1220, 655, 610), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                if (smokePoofShowing && poofFrame < 6)
                {
                    s.Draw(sprite, rec, new Rectangle(655 * poofFrame, 610, 655, 610), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }
}
