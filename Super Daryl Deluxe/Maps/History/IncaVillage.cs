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
    class IncaVillage : MapClass
    {
        static Portal toHistoryIntro;
        static Portal toCamp;

        public static Portal ToCamp { get { return toCamp; } }
        public static Portal ToHistoryIntro { get { return toHistoryIntro; } }

        Texture2D foreground, sky, foreground2, houses, parallax1, parallax2, foreHouseLeft, foreHouseRight;

        float foregroundAlpha = 1f;

        public IncaVillage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 6000;
            mapName = "Inca Empire";

            zoomLevel = .85f;

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\IncaVillage\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\IncaVillage\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\IncaVillage\sky");
            houses = content.Load<Texture2D>(@"Maps\History\IncaVillage\houses");
            parallax1 = content.Load<Texture2D>(@"Maps\History\IncaVillage\parallax1");
            parallax2 = content.Load<Texture2D>(@"Maps\History\IncaVillage\parallax2");
            foreground = content.Load<Texture2D>(@"Maps\History\IncaVillage\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\IncaVillage\foreground2");
            foreHouseLeft = content.Load<Texture2D>(@"Maps\History\IncaVillage\foregroundHouseLeft");
            foreHouseRight = content.Load<Texture2D>(@"Maps\History\IncaVillage\foregroundHouseRight");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }


        public override void Update()
        {
            base.Update();
            zoomLevel = .7f;
            if (game.Camera.center.Y > 120)
            {
                game.Camera.center.Y = 119;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toHistoryIntro = new Portal(50, platforms[0], "Inca Empire");
            toCamp = new Portal(5800, platforms[0], "Inca Empire");
            toCamp.PortalRecY = -840;
            toHistoryIntro.PortalRecY = -400;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHistoryIntro, HistoryEntrance.ToIncaVillage);
            portals.Add(toCamp, NapoleonsCamp.ToIncaEmpire);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (player.PositionY > -456)
                s.Draw(foreHouseLeft, new Vector2(0, mapRec.Y), Color.White);

            if (player.PositionY > -923)
                s.Draw(foreHouseRight, new Vector2(foreHouseLeft.Width + 2, mapRec.Y), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.PositionY <= -456)
                s.Draw(foreHouseLeft, new Vector2(0, mapRec.Y), Color.White);

            if (player.PositionY <= -923)
                s.Draw(foreHouseRight, new Vector2(foreHouseLeft.Width + 2, mapRec.Y), Color.White);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
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
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(parallax1, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.4f, this, game));
            s.Draw(parallax2, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));
            s.Draw(houses, new Vector2(2300, mapRec.Y), Color.White);
            s.End();
        }
    }
}
