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
    public class CoffinExplosion
    {
        int frame;
        int frameDelay = 5;
        int delayBeforeStart;

        Rectangle rec;

        public Boolean finished = false;
        public CoffinExplosion(int x, int y)
        {
            rec = new Rectangle(x, y, 398, 981);
            delayBeforeStart = Game1.randomNumberGen.Next(45, 150);
        }

        public void Update()
        {
            if (delayBeforeStart <= 0)
            {
                frameDelay--;


                if (Game1.Player.CheckIfHit(rec) && Game1.Player.InvincibleTime <= 0)
                {
                    Game1.Player.TakeDamage(50, 18);

                    if (Game1.Player.VitalRecX < rec.Center.X)
                        Game1.Player.KnockPlayerBack(new Vector2(-10, -8));
                    else
                        Game1.Player.KnockPlayerBack(new Vector2(10, -8));

                    Game1.Player.HitPauseTimer = 3;
                    Game1.g.Camera.ShakeCamera(2, 2);
                }


                if (frameDelay <= 0)
                {
                    frame++;

                    frameDelay = 5;

                    if (frame > 4)
                        finished = true;
                }
            }
            else
                delayBeforeStart--;
        }
        public Rectangle GetSource()
        {
            switch (frame)
            {
                case 0:
                    return new Rectangle(796, 0, 398, 981);
                case 1:
                    return new Rectangle(398, 0, 398, 981);
                case 2:
                    return new Rectangle(0, 0, 398, 981);
                case 3:
                    return new Rectangle(398, 0, 398, 981);
                case 4:
                    return new Rectangle(796, 0, 398, 981);
            }

            return new Rectangle();
        }
        public void Draw(SpriteBatch s)
        {
            if (delayBeforeStart <= 0)
                s.Draw(Game1.g.EnemySpriteSheets["Corrupted Coffin"], rec, GetSource(), Color.White);
            else if (delayBeforeStart <= 45)
            {
                s.Draw(Game1.whiteFilter, new Rectangle(rec.Center.X - 50, rec.Y, 100, rec.Height), Color.Red * .5f);
            }
        }
    }

    class CorruptedCoffin : Boss
    {
        int summonCooldown;
        int maxSummonCooldown = 450;

        int explosionCooldown;
        int maxExplosionCooldown = 250;

        List<CoffinExplosion> explosions;

        // CONSTRUCTOR \\
        public CorruptedCoffin(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 100;
            maxHealth = 100;
            level = 15;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, 100, 600);
            vitalRec = new Rectangle((int)position.X + 1870, (int)position.Y + 940, 380, 550);

            explosions = new List<CoffinExplosion>();
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

        public void SummonMummies()
        {
            summonCooldown--;

            if (summonCooldown <= 0)
            {
                int enemyNum = currentMap.EnemiesInMap.Count;

                if (enemyNum < 15)
                {
                    int numToSpawn = health < maxHealth / 2 ? 5 : 3;

                    for (int i = 0; i < numToSpawn; i++)
                    {
                        Enemy en;

                        if(Game1.randomNumberGen.Next(0, 4) < 3)
                            en = new Mummy(Vector2.Zero, "Mummy", game, ref player, currentMap);
                        else
                            en = new VileMummy(Vector2.Zero, "Vile Mummy", game, ref player, currentMap);

                        float monsterY = 577 - en.Rec.Height - 10;
                        en.Position = new Vector2(Game1.randomNumberGen.Next(vitalRec.X - 300, vitalRec.X + vitalRec.Width + 300), monsterY);

                        en.SpawnWithPoof = true;

                        en.UpdateRectangles();
                        currentMap.AddEnemyToEnemyList(en);
                    }
                }

                summonCooldown = maxSummonCooldown;
            }
        }

        public void SummonExplosions()
        {
            explosionCooldown--;

            if (explosionCooldown <= 0)
            {
                int explosionNum;

                if (health < maxHealth / 2)
                {
                    explosionNum = Game1.randomNumberGen.Next(1, 4);
                    explosionCooldown = (int)(maxExplosionCooldown * .75f);
                }
                else
                {
                    explosionNum = Game1.randomNumberGen.Next(2, 5);
                    explosionCooldown = maxExplosionCooldown;
                }

                for (int i = 0; i < explosionNum; i++)
                {
                    explosions.Add(new CoffinExplosion(Game1.randomNumberGen.Next(vitalRec.X - 300, vitalRec.X + vitalRec.Width + 300), currentMap.mapRec.Y + 625));
                }
            }
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

            if (health < maxHealth)
            {
                SummonExplosions();
                SummonMummies();
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                if (explosions[i].finished)
                {
                    explosions.RemoveAt(i);
                    i--;
                    continue;
                }
                explosions[i].Update();
            }

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            vitalRec.X = rec.X + 1870;
            vitalRec.Y = rec.Y + 940;

            if (IsDead())
            {
                explosions.Clear() ;
                game.CurrentChapter.BossFight = false;
                game.CurrentChapter.CurrentBoss = null;

                Chapter.effectsManager.AddSmokePoof(vitalRec, 3);
                game.ChapterTwo.ChapterTwoBooleans["corruptedCoffinDestroyed"] = true;
            }
        }

        public override void ImplementGravity()
        {
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.whiteFilter, vitalRec, Color.White);

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(s);
            }
        }
    }
}