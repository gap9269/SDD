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
    class Backstage : MapClass
    {
        static Portal toTheStage;
        static Portal toStorageRoom;
        static Portal toManagerFloor;
        static Portal toRoomThree;
        static Portal toRoomFour;
        static Portal toBeethovensRoom;

        public static Portal ToTheStage { get { return toTheStage; } }
        public static Portal ToStorageRoom { get { return toStorageRoom; } }
        public static Portal ToManagerFloor { get { return toManagerFloor; } }

        public Backstage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2700;
            mapName = "Backstage";

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

            toTheStage = new Portal(50, 630, "Backstage");
            toStorageRoom = new Portal(2400, 630, "Backstage");
            toManagerFloor = new Portal(1000, 630, "Backstage");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheStage, TheStage.ToBackstage);

            portals.Add(toManagerFloor, ManagersFloor.ToBackstage);
        }
    }
}
