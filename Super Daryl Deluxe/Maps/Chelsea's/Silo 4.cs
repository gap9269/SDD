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
    class Silo4 : MapClass
    {
        static Portal toSilo3;

        public static Portal ToSilo3 { get { return toSilo3; } }

        public Silo4(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 5100;
            mapWidth = 5700;
            mapName = "Silo 4";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();


            //Beginning
            DisappearingPlatform d1 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2440, 200, 450, 50), false, false, false, 60, 60);
            platforms.Add(d1);

            DisappearingPlatform d2 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3160, 80, 350, 50), false, false, false, 45, 60);
            platforms.Add(d2);

            SinkingDisappearingPlatform s1 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3800, -100, 300, 50), false, false, false, 3, 400, 2, 60, 60 );
            platforms.Add(s1);

            DisappearingPlatform d3 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4400, 40, 350, 50), false, false, false, 40, 60);
            platforms.Add(d3);

            //Sinking plat before moving plat
            SinkingPlatform s2 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5000, -140, 250, 50), false, false, false, 3, 400, 2);
            platforms.Add(s2);

            //Small steps
            DisappearingPlatform d4 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5380, -200, 100, 50), false, false, false, 20, 75);
            platforms.Add(d4);

            DisappearingPlatform d5 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5380, -480, 100, 50), true, false, false, 20, 75);
            platforms.Add(d5);

            //moving platform
            MovingPlatform m1 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4620, -300, 250, 50), true, false, false, new List<Vector2> { new Vector2(3700, -640), new Vector2(4620, -300) }, 2, 100);
            platforms.Add(m1);

            //Straight line before elevator
            DisappearingPlatform d6 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3220, -700, 200, 50), false, false, false, 25, 60);
            platforms.Add(d6);

            DisappearingPlatform d7 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2780, -700, 200, 50), false, false, false, 25, 60);
            platforms.Add(d7);

            DisappearingPlatform d8 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2160, -700, 350, 50), false, false, false, 35, 60);
            platforms.Add(d8);

            //Two sinkDisps and one disappearing to juggle before elevator
            SinkingDisappearingPlatform s3 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1580, -700, 300, 50), false, false, false, 5, 300, 1, 30, 100);
            platforms.Add(s3);

            SinkingDisappearingPlatform s4 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1120, -700, 300, 50), false, false, false, 5, 300, 1, 30, 100);
            platforms.Add(s4);

            DisappearingPlatform d9 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1420, -760, 150, 50), true, false, false, 60, 220);
            platforms.Add(d9);

            //elevator
            MovingPlatform m2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(580, -780, 300, 50), true, false, false, new List<Vector2> { new Vector2(580, -1460), new Vector2(580, -780) }, 2, 100);
            platforms.Add(m2);


            //loopy part 
            SinkingDisappearingPlatform s5 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1137, -1460, 100, 50), false, false, false, 3, 150, 2, 45, 60);
            platforms.Add(s5);

            SinkingDisappearingPlatform s6 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1520, -1340, 100, 50), false, false, false, 3, 150, 2, 45, 60);
            platforms.Add(s6);

            DisappearingPlatform d10 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1800, -1460, 100, 50), true, false, false, 30, 80);
            platforms.Add(d10);

            DisappearingPlatform d11 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1990, -1680, 100, 50), true, false, false, 30, 80);
            platforms.Add(d11);

            SinkingPlatform s7 = new SinkingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1860, -1940, 100, 50), true, false, false, 3, 250, 2);
            platforms.Add(s7);

            DisappearingPlatform d12 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1300, -2030, 300, 50), true, false, false, 40, 80);
            platforms.Add(d12);


            DisappearingPlatform d13 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(870, -2130, 200, 50), true, false, false, 30, 80);
            platforms.Add(d13);

            //side by side sinky things
            SinkingDisappearingPlatform s8 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(520, -2280, 100, 50), false, false, false, 4, 200, 2, 30, 80);
            platforms.Add(s8);

            SinkingDisappearingPlatform s9 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(800, -2450, 100, 50), true, false, false, 4, 200, 2, 30, 80);
            platforms.Add(s9);

            SinkingDisappearingPlatform s10 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(540, -2640, 100, 50), true, false, false, 4, 200, 2, 30, 80);
            platforms.Add(s10);

            SinkingDisappearingPlatform s11 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(900, -2820, 100, 50), true, false, false, 4, 200, 2, 30, 80);
            platforms.Add(s11);

            DisappearingPlatform d14 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(620, -3020, 100, 50), true, false, false, 30, 80);
            platforms.Add(d14);

            //Straight line of platforms
            for (int i = 980; i < 1980; i += 200)
            {
                DisappearingPlatform t = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(i, -3140, 200, 50), true, false, false, 20, 60);
                platforms.Add(t);
            }

            //long one where you have to jump between two platforms
            DisappearingPlatform d15 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1980, -3140, 600, 50), true, false, false, 35, 60);
            platforms.Add(d15);

            DisappearingPlatform d16 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2580, -3140, 200, 50), true, false, false, 20, 60);
            platforms.Add(d16);

            DisappearingPlatform d17 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2900, -3120, 150, 50), false, false, false, 20, 60);
            platforms.Add(d17);


            //Straight line of platforms
            for (int i = 3220; i < 3620; i += 200)
            {
                DisappearingPlatform t = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(i, -2960, 200, 50), true, false, false, 20, 60);
                platforms.Add(t);
            }

            DisappearingPlatform d18 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3620, -2960, 400, 50), false, false, false, 25, 60);
            platforms.Add(d18);

            for (int i = 4020; i < 4420; i += 200)
            {
                DisappearingPlatform t = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(i, -2960, 200, 50), true, false, false, 20, 60);
                platforms.Add(t);
            }

            //Last few steps
            SinkingDisappearingPlatform s12 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4660, -3040, 100, 50), false, false, false, 5, 200, 2, 30, 80);
            platforms.Add(s12);

            DisappearingPlatform d19 = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4970, -3120, 100, 50), true, false, false, 10, 60);
            platforms.Add(d19);

            SinkingDisappearingPlatform s13 = new SinkingDisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5300, -3220, 150, 50), true, false, false, 5, 200, 2, 30, 80);
            platforms.Add(s13);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSilo3 = new Portal(100, platforms[0], "Silo4");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSilo3, Silo3.ToSilo4);
        }
    }
}