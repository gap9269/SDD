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
    class Coffin : BreakableObject
    {
        int frameDelay = 5;
        int frame;
        Boolean spawnVileMummies, spawnHostileMummies;
        int mummySpawnAmount, maxTimeToSpawn, spawnTimer;

        Player player;

        float redDamageAlpha = 0;

        public Coffin(Game1 g, int x, int y, Texture2D s, Boolean facingRight, int mummyAmount, int timeToSpawn = 600, Boolean spawnVile = false, Boolean hostileMummies = false)
            : base(g, x, y, s, true, 15, 0, 0, false)
        {
            rec = new Rectangle(x, y, 314, 293);
            this.facingRight = facingRight;

            if (facingRight)
                vitalRec = new Rectangle(rec.X + 124, rec.Y + 58, 72, 227);
            else
                vitalRec = new Rectangle(rec.X + 118, rec.Y + 58, 72, 227);

            player = Game1.Player;

            mummySpawnAmount = mummyAmount;
            spawnVileMummies = spawnVile;
            spawnHostileMummies = hostileMummies;
            maxTimeToSpawn = timeToSpawn;
            spawnTimer = maxTimeToSpawn;
        }

        public override Rectangle GetSourceRec()
        {
            if (frameState == 0)
                return new Rectangle(314 * frame, 0, 314, 293);
            else if (frameState == 1)
                return new Rectangle(314 * frame, 293, 314, 293);
            else
                return new Rectangle(628, 586, 314, 293);
        }

        public override void Update()
        {

            if (!finished)
            {
                frameDelay--;

                if (redDamageAlpha > 0)
                    redDamageAlpha -= .04f;

                if (frameDelay == 0)
                {
                    frame++;
                    frameDelay = 5;

                    if (frame > 12)
                        frame = 0;
                }

                if (health > maxHealth / 2)
                    frameState = 0;
                else if (health > 0)
                    frameState = 1;

                spawnTimer--;

                if (spawnTimer <= 0)
                {
                    for (int i = 0; i < mummySpawnAmount; i++)
                    {
                        if (spawnVileMummies && Game1.randomNumberGen.Next(0, 3) == 0)
                        {
                            VileMummy ben = new VileMummy(new Vector2(vitalRec.Center.X + Game1.randomNumberGen.Next(-150, 150), vitalRec.Center.Y - 130), "Vile Mummy", game, ref player, game.CurrentChapter.CurrentMap);
                            ben.Hostile = spawnHostileMummies;
                            ben.SpawnWithPoof = true;
                            if(game.CurrentChapter.CurrentMap.EnemiesInMap.Count < 25)
                                game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);
                        }
                        else
                        {
                            Mummy ben = new Mummy(new Vector2(vitalRec.Center.X + Game1.randomNumberGen.Next(-150, 150), vitalRec.Center.Y - 130), "Mummy", game, ref player, game.CurrentChapter.CurrentMap);
                            ben.Hostile = spawnHostileMummies;
                            ben.SpawnWithPoof = true;
                            if (game.CurrentChapter.CurrentMap.EnemiesInMap.Count < 25)
                                game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);
                        }
                    }

                    spawnTimer = maxTimeToSpawn;

                    if (frameState == 1)
                        spawnTimer += (int)(maxTimeToSpawn * .35f);
                }

                if (health <= 0 && finished == false)
                {
                    if (facingRight)
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.Center.X - 125, rec.Center.Y - 100, 250, 250), 3);
                    else
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.Center.X - 125, rec.Center.Y - 100, 250, 250), 3);
                    finished = true;

                    Sound.PlaySoundInstance(Sound.SoundNames.object_goblin_hut_destroy);
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
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden)
            {
                if (!facingRight)
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);

                    if (frameState == 0)
                        s.Draw(sprite, rec, new Rectangle(0, 586, 314, 293), Color.White * redDamageAlpha);
                    else if (frameState == 1)
                        s.Draw(sprite, rec, new Rectangle(314, 586, 314, 293), Color.White * redDamageAlpha);
                }
                else
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (frameState == 0)
                        s.Draw(sprite, rec, new Rectangle(0, 586, 314, 293), Color.White * redDamageAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    else if (frameState == 1)
                        s.Draw(sprite, rec, new Rectangle(314, 586, 314, 293), Color.White * redDamageAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }
}
