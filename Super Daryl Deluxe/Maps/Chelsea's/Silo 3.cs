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
    class Silo3 : MapClass
    {
        static Portal toSilo2;
        static Portal toSilo4;

        public static Portal ToSilo2 { get { return toSilo2; } }
        public static Portal ToSilo4 { get { return toSilo4; } }

        public Silo3(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight =5100;
            mapWidth = 5700;
            mapName = "Silo 3";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //Moving plats at the end
            MovingPlatform m1 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2830, -920, 300, 50), false, false, false, new List<Vector2> { new Vector2(2100,-920), new Vector2(2830, -920) }, 3, 100);
            platforms.Add(m1);

            MovingPlatform m2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1680, -840, 300, 50), false, false, false, new List<Vector2> { new Vector2(1680,-1540), new Vector2(1680, -840) }, 4, 100);
            platforms.Add(m2);

            MovingPlatform m3 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(700, -1620, 300, 50), true, false, false, new List<Vector2> { new Vector2(1640, -1830), new Vector2(2560, -1620), new Vector2(1640, -1830), new Vector2(700, -1620)}, 3, 100);
            platforms.Add(m3);

            MovingPlatform m4 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2890, -1760, 300, 50), false, false, false, new List<Vector2> { new Vector2(3360, -1460), new Vector2(4840, -1760), new Vector2(3360, -2180), new Vector2(2890, -1760) }, 6, 50);
            platforms.Add(m4);

            MovingPlatform m5 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3040, -2360, 300, 50), true, false, false, new List<Vector2> { new Vector2(2000,-2880), new Vector2(3040, -2360) }, 4, 100);
            platforms.Add(m5);

            //Steps
            DisappearingPlatform d1 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(680, 260, 200, 50), true, false, false, 30, 60);
            platforms.Add(d1);

            DisappearingPlatform d2 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(480, 0, 200, 50), true, false, false, 30, 60);
            platforms.Add(d2);

            DisappearingPlatform d3 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(900, -200, 200, 50), false, false, false, 30, 60);
            platforms.Add(d3);

            DisappearingPlatform d4 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1300, -110, 200, 50), false, false, false, 30, 60);
            platforms.Add(d4);

            //Long line of platforms
            DisappearingPlatform d11 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1600, -110, 300, 50), false, false, false, 30, 60);
            platforms.Add(d11);

            for (int i = 1900; i < 2500; i += 200)
            {
                DisappearingPlatform t = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(i, -110, 200, 50), false, false, false, 20, 60);
                platforms.Add(t);
            }

            DisappearingPlatform d7 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2500, -110, 400, 50), false, false, false, 40, 60);
            platforms.Add(d7);

            DisappearingPlatform d8 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2900, -110, 500, 50), false, false, false, 50, 60);
            platforms.Add(d8);

            for (int i = 3400; i < 3900; i += 200)
            {
                DisappearingPlatform t = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(i, -110, 200, 50), false, false, false, 20, 60);
                platforms.Add(t);
            }

            //Sinking plat at the end
            SinkingPlatform sink = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4640, -290, 300, 50), false, false, false, 4, 400, 2);
            platforms.Add(sink);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSilo2 = new Portal(100, platforms[0], "Silo3");
            toSilo4 = new Portal(3460, -3520, "Silo3");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSilo2, Silo2.ToSilo3);
            portals.Add(toSilo4, Silo4.ToSilo3);
        }
    }
}