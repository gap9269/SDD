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
    class TheStage : MapClass
    {
        static Portal toBackstage;
        static Portal toEntranceHall;
        static Portal toSecondFloor;

        public static Portal ToBackstage { get { return toBackstage; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToSecondFloor { get { return toSecondFloor; } }


        Platform trapDoor;
        public TheStage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 3600;
            mapWidth = 3600;
            mapName = "The Stage";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .9f;
            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            trapDoor = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1100, -650, 500, 50), false, false, false);
            platforms.Add(trapDoor);
        }

        public override void Update()
        {
            base.Update();

            if (game.MapBooleans.chapterOneMapBooleans["TrapDoorOpened"] && platforms.Contains(trapDoor))
            {
                platforms.Remove(trapDoor);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBackstage = new Portal(2000, -650, "TheStage");
            toEntranceHall = new Portal(50, -650, "TheStage");
            toSecondFloor = new Portal(500, -1820, "TheStage");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntranceHall, EntranceHall.ToTheStage);
            portals.Add(toSecondFloor, SecondFloor.ToStage);
            portals.Add(ToBackstage, Backstage.ToTheStage);
        }
    }
}
