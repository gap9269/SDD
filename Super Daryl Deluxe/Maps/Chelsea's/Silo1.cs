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
    class Silo1 : MapClass
    {
        static Portal toOokySpookyBarn;
        static Portal toSilo2;

        public static Portal ToSilo2 { get { return toSilo2; } }
        public static Portal ToOokySpookyBarn { get { return toOokySpookyBarn; } }

        public Silo1(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4700;
            mapWidth = 4500;
            mapName = "Silo 1";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();


            //Diagonals
            MovingPlatform p1 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1030, -780, 200, 50), false, false, false, new List<Vector2> { new Vector2(1560, -1020), new Vector2(1030, -780) }, 3, 75);
            platforms.Add(p1);

            MovingPlatform p2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(840, -1620, 200, 50), true, false, false, new List<Vector2> { new Vector2(1620,-1220), new Vector2(840, -1620) }, 3, 75);
            platforms.Add(p2);

            //Elevators
            MovingPlatform p3 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(620,-1940, 200, 50), true, false, false, new List<Vector2> { new Vector2(620, -2560), new Vector2(620, -1940) }, 3, 100);
            platforms.Add(p3);

            MovingPlatform p4 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(220, -2380, 200, 50), true, false, false, new List<Vector2> { new Vector2(220,-2900), new Vector2(220, -2380) }, 4, 100);
            platforms.Add(p4);

            //Horizontal
            MovingPlatform p5 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1060, -2900, 200, 50), false, false, false, new List<Vector2> { new Vector2(1900, -2900), new Vector2(1060, -2900) }, 3, 75);
            platforms.Add(p5);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toOokySpookyBarn = new Portal(300, platforms[0], "Silo1");
            toSilo2 = new Portal(4300, -3223, "Silo1");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOokySpookyBarn, OokySpookyBarn.ToSilo1);
            portals.Add(toSilo2, Silo2.ToSilo1);
        }
    }
}