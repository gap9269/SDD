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
    class ButterflyChamber : MapClass
    {
        static Portal toChamberOfCorruption;
        public static Portal ToChamberOfCorruption { get { return toChamberOfCorruption; } }

        static Portal toOrganStorageRoomThree;
        public static Portal ToOrganStorageRoomThree { get { return toOrganStorageRoomThree; } }
        Texture2D foreground, bars;

        public ButterflyChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1500;
            mapName = "Butterfly Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 25;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ButterflyChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ButterflyChamber\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\ButterflyChamber\bars");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.LocustEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            Locust en = new Locust(pos, "Locust", game, ref player, this, mapRec);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && game.ChapterTwo.ChapterTwoBooleans["butterflyChamberCleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"])
            {
                RespawnFlyingEnemies(new Rectangle(300, 100, 900, 550));
                spawnEnemies = false;
                toChamberOfCorruption.IsUseable = false;
                toOrganStorageRoomThree.IsUseable = false;
            }
            else if (enemiesInMap.Count == enemyAmount && game.ChapterTwo.ChapterTwoBooleans["butterflyChamberCleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"] = true;
                game.Camera.ShakeCamera(5, 15);
            }
            else if (enemiesInMap.Count == 0 && game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"] = false;
                game.ChapterTwo.ChapterTwoBooleans["butterflyChamberCleared"] = true;
                game.Camera.ShakeCamera(5, 15);

                toChamberOfCorruption.IsUseable = true;
                toOrganStorageRoomThree.IsUseable = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["butterflyChamberCleared"] && enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                enemyAmount = 10;
                RespawnFlyingEnemies(new Rectangle(300, 100, 900, 550));

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toChamberOfCorruption = new Portal(50, platforms[0], "Butterfly Chamber");
            toOrganStorageRoomThree = new Portal(1250, platforms[0], "Butterfly Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["butterflyChamberLocked"])
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toChamberOfCorruption, ChamberOfCorruption.ToButterflyChamber);
            portals.Add(toOrganStorageRoomThree, OrgranStorageRoomThree.ToButterflyChamber);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
