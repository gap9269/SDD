using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class BreakableWall : BreakableObject
    {
        public BreakableWall(Game1 g, int x, int y, int health, Texture2D s, Boolean facingRight)
            : base(g, x, y, s, false, health, 0, 0, false)
        {
            rec = new Rectangle(x, y, 174, 356);
            this.facingRight = facingRight;

            vitalRec = rec;
            frameState = 0;
        }

        public override Rectangle GetSourceRec()
        {
            if (frameState == 0)
                return new Rectangle(0, 0, 174, 356);
            else if(frameState == 1)
                return new Rectangle(174, 0, 174, 356);

            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (frameState == 1 && !passable)
                passable = true;
        }

        public override void TakeHit(int damage = 1)
        {
            if (frameState != 1)
            {
                base.TakeHit(damage);

                if (health == 0)
                {
                    finished = true;
                    frameState = 1;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden)
            {
                if (facingRight)
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }

    class UpperVentsII : MapClass
    {
        static Portal toUpperVents1;
        static Portal toUpperVents3;
        static Portal toUpperVents5;

        public static Portal ToUpperVents5 { get { return toUpperVents5; } }
        public static Portal ToUpperVents1 { get { return toUpperVents1; } }
        public static Portal ToUpperVents3 { get { return toUpperVents3; } }

        Texture2D foreground, wallSprite, fan;
        float fanRotation;

        BreakableWall breakableWall;

        LockerCombo lockerSheet;

        public UpperVentsII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 3100;
            mapHeight = 2680;
            mapName = "Upper Vents II";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 10;

            //--Map Quest
            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(2500, mapRec.Y + 1850 - Game1.mapSign.Height, "Clear the area of enemies!", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Marker(), new Karma(2), new Money(2.50f)});
            mapQuestSigns.Add(sign);

            lockerSheet = new LockerCombo(795, -105, "Drew", game);
            collectibles.Add(lockerSheet);

            breakableWall = new BreakableWall(game, 2157,mapRec.Y + 1907, 5, wallSprite, true);
            interactiveObjects.Add(breakableWall);

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BatEnemy(content);
            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Fluffles the Rat"] < 6)
            {
                FlufflesTheRat en = new FlufflesTheRat(pos, "Fluffles the Rat", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Fluffles the Rat"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            if (enemyNamesAndNumberInMap["Vent Bat"] < 4)
            {
                Bat en = new Bat(pos, "Vent Bat", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Vent Bat"]++;
                    AddEnemyToEnemyList(en);
                }
            }

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            enemiesInMap.Clear();
            ResetEnemyNamesAndNumberInMap();
            spawnEnemies = true;
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 2\background"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 2\foreground");
            wallSprite = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 2\wallSprite");
            fan = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 2\fan");
            Sound.LoadVentZoneSounds();

            breakableWall.Sprite = wallSprite;
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            fanRotation+=8;

            if (fanRotation == 360)
                fanRotation = 0;

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnFlyingEnemies(new Rectangle(100, -700, 2900, 300));
                RespawnGroundEnemies();
            }

            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;

            if (enemiesInMap.Count == 0 && !spawnEnemies)
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents1 = new Portal(50, -350, "Upper Vents II");
            toUpperVents3 = new Portal(2700, 640, "Upper Vents II");
            toUpperVents5 = new Portal(2755, -350, "Upper Vents II", "Bronze Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents1, UpperVents1.ToUpperVents2);
            portals.Add(toUpperVents3, UpperVentsIII.ToUpperVents2);
            portals.Add(toUpperVents5, UpperVentsV.ToUpperVents2);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.End();

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(fan, new Rectangle(739 + fan.Width / 2, mapRec.Y + 1398 + fan.Height / 2, fan.Width, fan.Height), null, Color.White, (float)(fanRotation * (Math.PI / 180)), new Vector2(fan.Width / 2, fan.Height / 2), SpriteEffects.None, 0f);
            s.End();
        }
    }
}
