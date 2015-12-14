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
    class WindyDesert : MapClass
    {
        static Portal toDryDesert;
        static Portal toCentralSands;
        static Portal toTomb;

        public static Portal ToTomb { get { return toTomb; } }
        public static Portal ToCentralSands { get { return toCentralSands; } }
        public static Portal ToDryDesert { get { return toDryDesert; } }

        Texture2D parallax, parallaxFar, sun, sky;

        public WindyDesert(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 7000;
            mapName = "Windy Desert";

            zoomLevel = .9f;

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 12;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);

            //--Map Quest
            mapWithMapQuest = true;

            enemiesToKill.Add(15);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Burnie Buzzard");

            MapQuestSign sign = new MapQuestSign(1658, mapRec.Y + 91, "Kill 15 Burnie Buzzards", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(250), new BandHat(), new Karma(2) });
            mapQuestSigns.Add(sign);
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\WindyDesert\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\WindyDesert\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\WindyDesert\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\WindyDesert\parallax");
            parallaxFar = content.Load<Texture2D>(@"Maps\History\WindyDesert\parallax2");
            sun = content.Load<Texture2D>(@"Maps\History\WindyDesert\sun");
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

            monsterX = rand.Next(platforms[platformNum].Rec.X, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width - 500);
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);

            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 5)
            {
                SexySaguaro en = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
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
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 7)
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
                RespawnFlyingEnemies(new Rectangle(400, mapRec.Y + 150, mapWidth - 400, mapHeight - 800));
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toDryDesert = new Portal(250, platforms[0], "Windy Desert");
            toTomb = new Portal(5440, platforms[0], "Windy Desert");
            toCentralSands = new Portal(6800, -100, "Windy Desert");
            toTomb.PortalRecY = -80;
            toCentralSands.PortalRecY = -50;

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toDryDesert, DryDesert.ToWindyDesert);
            portals.Add(toTomb, ForgottenTomb.ToWindyDesert);
            portals.Add(toCentralSands, CentralSands.ToWindyDesert);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
           // s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
             s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(sun, new Vector2(0, mapRec.Y), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallaxFar, new Vector2(0, mapRec.Y), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.4f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }
    }
}
