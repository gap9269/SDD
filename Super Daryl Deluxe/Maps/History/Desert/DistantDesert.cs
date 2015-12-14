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
    class DistantDesert : MapClass
    {
        static Portal toTombOfCactusKing;
        static Portal toOasis;

        public static Portal ToOasis { get { return toOasis; } }
        public static Portal ToTombOfCactusKing { get { return toTombOfCactusKing; } }

        Texture2D foreground, foreground2, sky, sun, parallax, parallaxFar;

        public DistantDesert(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1300;
            mapWidth = 6000;
            mapName = "Distant Desert";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 7;

            zoomLevel = .95f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\DistantDesert\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\DistantDesert\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\DistantDesert\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\DistantDesert\foreground2");
            sky = content.Load<Texture2D>(@"Maps\History\DistantDesert\sky");
            sun = content.Load<Texture2D>(@"Maps\History\DistantDesert\sun");
            parallax = content.Load<Texture2D>(@"Maps\History\DistantDesert\parallax");
            parallaxFar = content.Load<Texture2D>(@"Maps\History\DistantDesert\parallaxFar");
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
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 4)
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
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 3)
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
                RespawnFlyingEnemies(new Rectangle(400, mapRec.Y + 150, mapWidth - 800, mapHeight - 800));
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            //toTombOfCactusKing = new Portal(350, platforms[0], "Distant Desert");
            toOasis = new Portal(5800, 551, "Distant Desert");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

          //  portals.Add(toTombOfCactusKing, MongolCamp.ToDesert);
           portals.Add(toOasis, Oasis.ToDistantDesert);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);

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
            s.Draw(sun, new Vector2(-1000, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.18f, this, game));
            s.Draw(parallaxFar, new Rectangle(0, mapRec.Y, parallaxFar.Width, parallaxFar.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(parallax, new Rectangle(0, mapRec.Y, parallax.Width, parallax.Height), Color.White);
            s.End();
        }
    }
}
