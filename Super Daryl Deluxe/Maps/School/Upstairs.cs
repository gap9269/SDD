using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Upstairs : MapClass
    {

        static Portal toSouthHall;
        static Portal toTheRoof;
        static Portal toUpperVents;
        static Portal toNorthHall;

        public static Portal ToSouthHall { get { return toSouthHall; } }
        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToTheRoof { get { return toTheRoof; } }
        public static Portal ToUpperVents { get { return toUpperVents; } }

        public Upstairs(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 7352;
            mapHeight = 720;
            mapName = "Upstairs";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNorthHall = new Portal(7000, platforms[0], "Upstairs");
            toSouthHall = new Portal(50, platforms[0], "Upstairs");
            toTheRoof = new Portal(3100, platforms[0], "Upstairs", "Roof Key");
            toUpperVents = new Portal(5100, platforms[0], "Upstairs");

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\hall"));

        }


        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toNorthHall, NorthHall.ToUpstairs);
            portals.Add(toSouthHall, SouthHall.ToUpstairs);
            portals.Add(toTheRoof, Roof.ToUpstairs);
            portals.Add(toUpperVents, UpperVents1.ToUpstairs);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
