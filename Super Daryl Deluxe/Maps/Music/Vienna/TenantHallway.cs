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
    class TenantHallway : MapClass
    {
        static Portal toThirdFloor;
        static Portal toRoomOne;
        static Portal toRoomTwo;
        static Portal toRoomThree;
        static Portal toRoomFour;
        static Portal toBeethovensRoom;

        public static Portal ToThirdFloor { get { return toThirdFloor; } }
        public static Portal ToRoomOne { get { return toRoomOne; } }
        public static Portal ToRoomTwo { get { return toRoomTwo; } }
        public static Portal ToRoomThree { get { return toRoomThree; } }
        public static Portal ToRoomFour { get { return toRoomFour; } }
        public static Portal ToBeethovensRoom { get { return toBeethovensRoom; } }

        public TenantHallway(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Tenant Hallway";

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

            toThirdFloor = new Portal(50, 630, "TenantHallway");
            toRoomOne = new Portal(500, 630, "TenantHallway");
            toRoomTwo = new Portal(1000, 630, "TenantHallway");
            toRoomThree = new Portal(1500, 630, "TenantHallway", "Silver Key");
            toRoomFour = new Portal(2000, 630, "TenantHallway");
            toBeethovensRoom = new Portal(2500, 630, "TenantHallway");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toThirdFloor, ThirdFloor.ToTenantHallway);

            portals.Add(toRoomOne, RoomOne.ToTenantHallway);
            portals.Add(toRoomTwo, RoomTwo.ToTenantHallway);
            portals.Add(toRoomThree, RoomThree.ToTenantHallway);
            portals.Add(toRoomFour, RoomFour.ToTenantHallway);
            portals.Add(toBeethovensRoom, BeethovensRoom.ToTenantHallway);
        }
    }
}
