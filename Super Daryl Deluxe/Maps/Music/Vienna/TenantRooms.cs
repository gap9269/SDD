using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    //Hall West
    class MozartsRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public MozartsRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Mozart's Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));

            game.NPCSprites["Mozart"] = content.Load<Texture2D>(@"NPC\Music\Mozart");
            Game1.npcFaces["Mozart"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\MozartNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Mozart"] = Game1.whiteFilter;
            Game1.npcFaces["Mozart"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Mozart's Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayWest.ToMozartsRoom);
        }
    }

    class TchaikovskysRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public TchaikovskysRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tchaikovsky's Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));

            game.NPCSprites["Tchaikovsky"] = content.Load<Texture2D>(@"NPC\Music\Tchaikovsky");
            Game1.npcFaces["Tchaikovsky"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\TchaikovskyNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Tchaikovsky"] = Game1.whiteFilter;
            Game1.npcFaces["Tchaikovsky"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Tchaikovsky's Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayWest.ToTchaikovskysRoom);
        }
    }

    class VacantRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        LivingLocker locker;

        public VacantRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Vacant Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            locker = new LivingLocker(game, new Rectangle(350, 200, 750, 400));
            interactiveObjects.Add(locker);

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Vacant Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayWest.ToVacantRoom);
        }
    }

    class BeethovensRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }


        BeethovensEarHorn earHorn;
        Sparkles sparkles;

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
            earHorn.ShowFButton = false;
            storyItems.Add(earHorn);
            sparkles = new Sparkles(900, 400);

        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Beethoven's Room\background"));
        }

        public override void Update()
        {
            base.Update();

            if (!earHorn.PickedUp)
            {
                sparkles.Update();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Beethoven's Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayWest.ToBeethovensRoom);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!earHorn.PickedUp)
            {
                sparkles.Draw(s);
            }
        }
    }



    //Hall East

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
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Tenant Room #3");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayEast.ToRoomThree);
        }
    }

    class WarholsRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public WarholsRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Warhol's Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Warhol's Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayEast.ToWarholsRoom);
        }
    }

    //Trenchcoat
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
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Tenant Room #4");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayEast.ToRoomFour);
        }
    }

    class RoomFive : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public RoomFive(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Tenant Room #5";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Tenant Room #5");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayEast.ToRoomFive);
        }
    }

    class StorageRoom : MapClass
    {
        static Portal toTenantHallway;

        public static Portal ToTenantHallway { get { return toTenantHallway; } }

        public StorageRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Storage Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Bedroom\background"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTenantHallway = new Portal(50, 630, "Storage Room");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTenantHallway, TenantHallwayEast.ToStorageRoom);
        }
    }
}
