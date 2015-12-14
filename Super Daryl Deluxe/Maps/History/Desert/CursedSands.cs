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
    class CursedSands : MapClass
    {
        static Portal toEgypt;
        static Portal toPyramid;

        public static Portal ToPyramid { get { return toPyramid; } }
        public static Portal ToEgypt { get { return toEgypt; } }

        Texture2D foreground, sky, parallax, foregroundCave;
        float wallAlpha = 1f;

        LockerCombo lockerSheet;

        public CursedSands(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 8000;
            mapName = "Cursed Sands";

            zoomLevel = .9f;

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 14;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lockerSheet = new LockerCombo(60, mapRec.Y + 930, "Chelsea", game);
            collectibles.Add(lockerSheet);

            collectibles.Add(new Textbook(61, 130, 1));

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Cursed Sands\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Cursed Sands\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\Cursed Sands\foreground");
            sky = content.Load<Texture2D>(@"Maps\History\CentralSands\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\Cursed Sands\parallax");
            foregroundCave = content.Load<Texture2D>(@"Maps\History\Cursed Sands\foregroundCave");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }

            monsterX = 0;
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);

            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 5)
            {
                SexySaguaro en = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                monsterX = rand.Next(platforms[platformNum].Rec.X - 300, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width - en.Rec.Width + 300);
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 30;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Sexy Saguaro"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 9)
            {
                BurnieBuzzard en = new BurnieBuzzard(pos, "Burnie Buzzard", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Burnie Buzzard"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnFlyingEnemies(new Rectangle(1000, mapRec.Y + 150, mapWidth - 1800, mapHeight - 800));
                RespawnGroundEnemies();
            }
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toEgypt = new Portal(3590, 308, "Cursed Sands");
            toPyramid = new Portal(7800, platforms[0], "Cursed Sands");
            ToPyramid.PortalRecY = 200;

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEgypt, Egypt.ToCursedSands);
            portals.Add(toPyramid, TheGreatPyramid.ToCursedSands);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(mapWidth-foreground.Width,mapRec.Y), Color.White);

            if (player.VitalRec.X < 465)
            {

                if (wallAlpha > 0)
                    wallAlpha -= .05f;
            }
            else
            {
                if (wallAlpha < 1f)
                    wallAlpha += .05f;
            }

            s.Draw(foregroundCave, new Vector2(0, mapRec.Y), Color.White * wallAlpha);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(parallax, new Vector2(-200, 0), Color.White);
            s.End();
        }
    }
}
