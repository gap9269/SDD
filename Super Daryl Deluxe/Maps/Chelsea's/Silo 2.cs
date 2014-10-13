using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class Silo2 : MapClass
    {
        static Portal toSilo1;
        static Portal toSilo3;

        public static Portal ToSilo3 { get { return toSilo3; } }
        public static Portal ToSilo1 { get { return toSilo1; } }

        public Silo2(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4700;
            mapWidth = 8000;
            mapName = "Silo 2";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //Beginning steps
            SinkingPlatform step1 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(860, 380, 300, 50), false, false, false, 1, 250, 2);
            platforms.Add(step1);

            SinkingPlatform step2 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1350, 240, 300, 50), false, false, false, 2, 250, 2);
            platforms.Add(step2);

            SinkingPlatform step3 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1890, 80, 300, 50), false, false, false, 3, 250, 3);
            platforms.Add(step3);

            //First 'puzzle' sinking plat
            SinkingPlatform longSink = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4220, -120, 1125, 50), false, false, false, 4, 400, 2);
            platforms.Add(longSink);

            //Second 'puzzle' sinking plat and assisting plats
            SinkingPlatform secondLongSink = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5800, -900, 600, 50), false, false, false, 4, 400, 2);
            platforms.Add(secondLongSink);
            
            SinkingPlatform step4 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(6680, -640, 200, 50), false, false, false, 3, 250, 3);
            platforms.Add(step4);

            SinkingPlatform step5 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(7380, -750, 200, 50), false, false, false, 3, 250, 3);
            platforms.Add(step5);

            SinkingPlatform step6 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(7140, -960, 200, 50), true, false, false, 3, 250, 3);
            platforms.Add(step6);

            //Disappearing plat before exit
            DisappearingPlatform lastPlat = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4620, -1740, 150, 50), false, false, false, 30, 30);
            platforms.Add(lastPlat);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSilo1 = new Portal(100, platforms[0], "Silo2");
            toSilo3 = new Portal(3950, -1748, "Silo2");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSilo1, Silo1.ToSilo2);
            portals.Add(toSilo3, Silo3.ToSilo2);
        }
    }
}