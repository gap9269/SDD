using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class RoomOne : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public RoomOne(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tenant Room #1";

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

            toTenantHallway = new Portal(50, 630, "TenantRoom#1");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallway.ToRoomOne);
        }
    }

    class RoomTwo : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public RoomTwo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tenant Room #2";

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

            toTenantHallway = new Portal(50, 630, "TenantRoom#2");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallway.ToRoomTwo);
        }
    }

    class RoomThree : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public RoomThree(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tenant Room #3";

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

            toTenantHallway = new Portal(50, 630, "TenantRoom#3");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallway.ToRoomThree);
        }
    }

    class RoomFour : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public RoomFour(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tenant Room #4";

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

            toTenantHallway = new Portal(50, 630, "TenantRoom#4");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallway.ToRoomFour);
        }
    }

    class BeethovensRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }


        BeethovensEarHorn earHorn;
        public BeethovensRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Beethoven's Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            earHorn = new BeethovensEarHorn(900, 400);
            storyItems.Add(earHorn);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Beethoven'sRoom");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallway.ToBeethovensRoom);
        }
    }
}
