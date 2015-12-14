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
    class CityStreets : MapClass
    {
        static Portal toIntroRoom;
        static Portal toTownSquare;

        public static Portal ToIntroRoom { get { return toIntroRoom; } }
        public static Portal ToTownSquare { get { return toTownSquare; } }

        Texture2D foreground, sky, parallax1, parallax2, wall;

        float wallAlpha;

        public CityStreets(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 9500;
            mapName = "City Streets";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 20;

            yScroll = true;

            Vase bar = new Vase(game, 9220, mapRec.Y + 2203 + 155, Game1.interactiveObjects["Vase"], true, 3, 3, 2.38f, false, Vase.VaseType.clayLarge);
            interactiveObjects.Add(bar);
            Vase bar1 = new Vase(game, 8432, mapRec.Y + 1666 + 155, Game1.interactiveObjects["Vase"], true, 1, 1, .11f, false, Vase.VaseType.claySmall);
            interactiveObjects.Add(bar1);
            Vase bar2 = new Vase(game, 8345, mapRec.Y + 1666 + 155, Game1.interactiveObjects["Vase"], true, 2, 2, .24f, false, Vase.VaseType.clayMedium);
            interactiveObjects.Add(bar2);

            interactiveObjects.Add(new Vase(game, 8253, mapRec.Y + 1666 + 155, Game1.interactiveObjects["Vase"], true, 3, 3, .38f, false, Vase.VaseType.clayLarge));
            interactiveObjects.Add(new Vase(game, 9321, mapRec.Y + 2173 + 155, Game1.interactiveObjects["Vase"], true, 1, 1, .11f, false, Vase.VaseType.claySmall));
            interactiveObjects.Add(new Vase(game, 8941, mapRec.Y + 1662 + 155, Game1.interactiveObjects["Vase"], true, 1, 1, .11f, false, Vase.VaseType.claySmall));
            interactiveObjects.Add(new Vase(game, 8051, mapRec.Y + 2504 + 155, Game1.interactiveObjects["Vase"], true, 2, 2, .24f, false, Vase.VaseType.clayMedium));
            interactiveObjects.Add(new Vase(game, 7968, mapRec.Y + 2468 + 155, Game1.interactiveObjects["Vase"], true, 2, 2, 1.76f, false, Vase.VaseType.clayMedium));
            interactiveObjects.Add(new Vase(game, 9075, mapRec.Y + 1956 + 155, Game1.interactiveObjects["Vase"], true, 1, 1, .11f, false, Vase.VaseType.claySmall));
            interactiveObjects.Add(new Vase(game, 9018, mapRec.Y + 2172 + 155, Game1.interactiveObjects["Vase"], true, 3, 3, .38f, false, Vase.VaseType.clayLarge));
            interactiveObjects.Add(new Vase(game, 9193, mapRec.Y + 1695 + 155, Game1.interactiveObjects["Vase"], true, 2, 2, .24f, true, Vase.VaseType.clayMedium));
            interactiveObjects.Add(new Vase(game, 9296, mapRec.Y + 1665 + 155, Game1.interactiveObjects["Vase"], true, 3, 3, .38f, false, Vase.VaseType.clayLarge));

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\City Streets\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\City Streets\background2"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\City Streets\background3"));
            sky = content.Load<Texture2D>(@"Maps\Music\City Streets\sky");
            parallax1 = content.Load<Texture2D>(@"Maps\Music\City Streets\parallax1");
            parallax2 = content.Load<Texture2D>(@"Maps\Music\City Streets\parallax2");
            wall = content.Load<Texture2D>(@"Maps\Music\City Streets\wall");
            foreground = content.Load<Texture2D>(@"Maps\Music\City Streets\foreground");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.FlufflesBandit(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();


            FlufflesTheBandit en = new FlufflesTheBandit(pos, "Fluffles the Bandit", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 120;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }

        }


        public override void Update()
        {
            base.Update();

            if (!game.ChapterOne.ChapterOneBooleans["destroyedVases"])
            {
                for (int i = 0; i < interactiveObjects.Count; i++)
                {
                    if (interactiveObjects[i] is Vase && interactiveObjects[i].Finished == false)
                        break;

                    if (i == interactiveObjects.Count - 1)
                        game.ChapterOne.ChapterOneBooleans["destroyedVases"] = true;
                }
            }

            if (enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIntroRoom = new Portal(1295, platforms[0], "City Streets");
            toTownSquare = new Portal(9100, platforms[0], "City Streets");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIntroRoom, MusicIntroRoom.ToCityStreets);
            portals.Add(toTownSquare, TownSquare.ToCityStreets);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(8000, mapRec.Y), Color.White);

            //Front door
            if (player.VitalRec.X > 8219 && player.VitalRecY < -300)
            {

                if (wallAlpha > 0)
                    wallAlpha -= .05f;
            }
            else
            {
                if (wallAlpha < 1f)
                    wallAlpha += .05f;
            }

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(wall, new Vector2(8000, mapRec.Y), Color.White * wallAlpha);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.96f, .92f, this, game));
            s.Draw(parallax1, new Rectangle(0, mapRec.Y, parallax1.Width, parallax1.Height), Color.White);
            s.Draw(parallax2, new Rectangle(parallax1.Width, mapRec.Y, parallax2.Width, parallax2.Height), Color.White);
            s.End();
        }
    }
}
