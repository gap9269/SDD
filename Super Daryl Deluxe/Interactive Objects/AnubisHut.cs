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
    class AnubisHut : BreakableObject
    {
        int frameDelay = 5;
        int frame;

        int poofFrame;
        int poofDelay = 5;

        Player player;

        float redDamageAlpha = 0;

        Boolean smokePoofShowing = false;

        public AnubisHut(Game1 g, int x, int y, Texture2D s, int hlthDrop, float mon, Boolean facingRight)
            : base(g, x, y, s, true, 10, hlthDrop, mon, false)
        {
            rec = new Rectangle(x, y - 610, 549, 466);
            this.facingRight = facingRight;

            if (facingRight)
                vitalRec = new Rectangle(rec.X + 70, rec.Y, 375, 440);
            else
                vitalRec = new Rectangle(rec.X + 100, rec.Y, 375, 440);

            player = Game1.Player;

        }

        public override Rectangle GetSourceRec()
        {
            if (frameState == 0)
                return new Rectangle(549 * frame, 0, 549, 466);
            else if (frameState == 1)
                return new Rectangle(549 * frame, 932, 549, 466);
            else
                return new Rectangle(0, 932 + 466, 549, 466);
        }

        public override void Update()
        {
            if (smokePoofShowing && poofFrame < 6)
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
                frameDelay--;

                if (redDamageAlpha > 0)
                    redDamageAlpha -= .04f;

                if (frameDelay == 0)
                {
                    frame++;
                    frameDelay = 6;

                    if (frame > 2 && frameState == 0)
                        frame = 0;
                    else if (frameState == 1 && frame > 1)
                        frame = 0;
                }

                if (health > maxHealth / 2)
                    frameState = 0;
                else if (health > 0)
                    frameState = 1;

                if (health <= 0 && finished == false)
                {
                    if (facingRight)
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + 150, rec.Y + 350, 248, 248), 3);
                    else
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + 250, rec.Y + 350, 248, 248), 3);
                    finished = true;

                  //  DropHealth();
                   //DropMoney();

                    Sound.PlaySoundInstance(Sound.SoundNames.object_goblin_hut_destroy);

                    int spawnAnubisNumber = Game1.randomNumberGen.Next(0, 3);

                    if (game.chapterState == Game1.ChapterState.demo)
                    {
                        AnubisWarriorDemo ben = new AnubisWarriorDemo(new Vector2(vitalRec.Center.X, vitalRec.Center.Y - 130), "Anubis Warrior", game, ref player, game.CurrentChapter.CurrentMap,
    new Rectangle(game.CurrentChapter.CurrentMap.mapRec.X + 200, game.CurrentChapter.CurrentMap.mapRec.Y + 300, game.CurrentChapter.CurrentMap.MapWidth - 400, 500));
                        ben.Hostile = true;
                        ben.SpawnWithPoof = false;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);
                    }
                    else
                    {
                        AnubisWarrior ben = new AnubisWarrior(new Vector2(vitalRec.Center.X, vitalRec.Center.Y - 130), "Anubis Warrior", game, ref player, game.CurrentChapter.CurrentMap,
                            new Rectangle(game.CurrentChapter.CurrentMap.mapRec.X + 200, game.CurrentChapter.CurrentMap.mapRec.Y + 300, game.CurrentChapter.CurrentMap.MapWidth - 400, 500));
                        ben.Hostile = true;
                        ben.SpawnWithPoof = false;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);
                    }
                    
                }
            }
            else
            {
                frameState = 2;
            }

            base.Update();
        }

        public override void TakeHit(int damage = 1)
        {
            if (health > 0)
                health-=damage;

            redDamageAlpha = 1f;

            if (health == maxHealth / 2)
            {
                Sound.PlaySoundInstance(Sound.SoundNames.object_goblin_hut_damage);
                smokePoofShowing = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden)
            {
                base.Draw(s);

                if (facingRight)
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);

                    if (frameState == 0)
                        s.Draw(sprite, rec, new Rectangle(549, 1398, 549, 466), Color.White * redDamageAlpha);
                    else if (frameState == 1)
                        s.Draw(sprite, rec, new Rectangle(1098, 1398, 549, 466), Color.White * redDamageAlpha);


                    if (smokePoofShowing && poofFrame < 6)
                    {
                        s.Draw(sprite, rec, new Rectangle(549 * poofFrame, 466, 549, 466), Color.White);
                    }

                }
                else
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (frameState == 0)
                        s.Draw(sprite, rec, new Rectangle(549, 1398, 549, 466), Color.White * redDamageAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (frameState == 1)
                        s.Draw(sprite, rec, new Rectangle(1098, 1398, 549, 466), Color.White * redDamageAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (smokePoofShowing && poofFrame < 6)
                    {
                        s.Draw(sprite, rec, new Rectangle(549 * poofFrame, 466, 549, 466), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                }
            }
        }
    }
}
