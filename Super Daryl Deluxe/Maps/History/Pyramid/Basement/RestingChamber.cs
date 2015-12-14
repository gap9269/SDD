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
    class RestingChamber : MapClass
    {
        static Portal toDarkenedChamber;
        static Portal toTheSummoningCrypt;
        public static Portal toBathroom;

        public static Portal ToTheSummoningCrypt { get { return toTheSummoningCrypt; } }
        public static Portal ToDarkenedChamber { get { return toDarkenedChamber; } }

        Texture2D foreground, outhouse;

        public RestingChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1900;
            mapName = "Resting Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new LivingLocker(game, new Rectangle(300, 100, 550, 500)));

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\RestingChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\RestingChamber\foreground");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

        }

        public override void Update()
        {
            base.Update();

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toDarkenedChamber = new Portal(1620, platforms[0], "Resting Chamber");
            toTheSummoningCrypt = new Portal(40, platforms[0], "Resting Chamber", "Pyramid Key");
            toBathroom = new Portal(1200, platforms[0], "Resting Chamber");

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Vector2(1100, 370), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toDarkenedChamber, DarkenedChamber.ToTheRestingChamber);
            portals.Add(toTheSummoningCrypt, TheSummoningCrypt.ToRestingChamber);
            portals.Add(toBathroom, Bathroom.ToLastMap);
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
