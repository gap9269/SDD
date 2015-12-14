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
    class ArtistsPlayground : MapClass
    {
        static Portal toGrandCanal;
        static Portal toMarket;

        public static Portal ToGrandCanal { get { return toGrandCanal; } }
        public static Portal ToMarket { get { return toMarket; } }

        Texture2D parallax1, parallax2, dots;

        public ArtistsPlayground(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 8150;
            mapName = "Artist's Playground";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 8;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Slay Dough", 0);
            enemyNamesAndNumberInMap.Add("Eatball", 0);
        }

        public override void RespawnGroundEnemies()
        {
            if (enemyNamesAndNumberInMap["Slay Dough"] < 4)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                SlayDough en = new SlayDough(pos, "Slay Dough", game, ref player, this);
                monsterY = platforms[0].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);
                en.TimeBeforeSpawn = 60;

                en.UpdateRectangles();

                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Slay Dough"]++;
                }
            }

            if (enemyNamesAndNumberInMap["Eatball"] < 4)
            {
                monsterX = rand.Next(platforms[1].Rec.X, platforms[1].Rec.X + platforms[1].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                Eatball en = new Eatball(pos, "Eatball", game, ref player, this);
                monsterY = platforms[1].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);
                en.TimeBeforeSpawn = 60;
                en.UpdateRectangles();

                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Eatball"]++;
                }
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SlayDoughEnemy(content);
            EnemyContentLoader.EatballEnemy(content);
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Artist's Playground\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\Artist's Playground\background2"));
            parallax1 = content.Load<Texture2D>(@"Maps\Music\Artist's Playground\parallax1");
            parallax2 = content.Load<Texture2D>(@"Maps\Music\Artist's Playground\parallax2");
            dots = content.Load<Texture2D>(@"Maps\Music\Artist's Playground\dots");
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();

            toGrandCanal.PortalRecX = 120;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toGrandCanal = new Portal(400, platforms[0], "Artist's Playground");
            toMarket = new Portal(7850, 186, "Artist's Playground");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toGrandCanal, GrandCanal.ToPlayground);
            portals.Add(toMarket, Market.ToPlayground);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(Game1.whiteFilter, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), new Color(108, 71, 118));
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.05f, .05f, this, game));
            s.Draw(dots, new Rectangle(-250 , -500, dots.Width, dots.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.45f, this, game));
            s.Draw(parallax1, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(parallax2, new Vector2(parallax1.Width, mapRec.Y), Color.White);
            s.End();
        }

    }
}
