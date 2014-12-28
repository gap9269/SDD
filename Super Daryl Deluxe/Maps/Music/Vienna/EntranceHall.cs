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
    class EntranceHall : MapClass
    {
        static Portal toTheaterAnDerWien;
        static Portal toTheStage;
        static Portal toSecondFloor;

        public static Portal ToTheaterAnDerWien { get { return toTheaterAnDerWien; } }
        public static Portal ToTheStage { get { return toTheStage; } }
        public static Portal ToSecondFloor { get { return toSecondFloor; } }

        public EntranceHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3200;
            mapName = "Entrance Hall";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheaterAnDerWien = new Portal(1500, platforms[0], "EntranceHall");
            toTheStage = new Portal(3000, platforms[0], "EntranceHall");
            toSecondFloor = new Portal(500, platforms[0], "EntranceHall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheaterAnDerWien, TheaterAnDerWien.ToEntranceHall);
            portals.Add(ToSecondFloor, SecondFloor.ToEntranceHall);
            portals.Add(toTheStage, TheStage.ToEntranceHall);
        }
    }
}
