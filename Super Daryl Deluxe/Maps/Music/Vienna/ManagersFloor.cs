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
    class ManagersFloor : MapClass
    {
        static Portal toManagersOffice;
        static Portal toBackstage;
        static Portal toFourthFloor;
        static Portal toRoomThree;
        static Portal toRoomFour;
        static Portal toBeethovensRoom;

        public static Portal ToManagersOffice { get { return toManagersOffice; } }
        public static Portal ToBackstage { get { return toBackstage; } }
        public static Portal ToFourthFloor { get { return toFourthFloor; } }

        public ManagersFloor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Manager's Floor";

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

            toManagersOffice = new Portal(2500, 630, "Manager'sFloor");
            toFourthFloor = new Portal(300, 630, "Manager'sFloor");
            toBackstage = new Portal(1200, 630, "Manager'sFloor");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBackstage, Backstage.ToManagerFloor);
           // portals.Add(toRoomOne, RoomOne.ToTenantHallway);
           // portals.Add(toRoomTwo, RoomTwo.ToTenantHallway);
        }
    }
}
