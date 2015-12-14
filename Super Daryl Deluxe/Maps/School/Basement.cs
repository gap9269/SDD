using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Basement : MapClass
    {
        static Portal toSideHall;
        static Portal toDDRoom;
        static Portal toGeneratorRoom;
        static Portal toPrison;

        public static Portal ToSideHall { get { return toSideHall; } }
        public static Portal ToDDRoom { get { return toDDRoom; } }
        public static Portal ToGeneratorRoom { get { return toGeneratorRoom; } }
        public static Portal ToPrison { get { return toPrison; } }

        public Basement(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Basement";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideHall = new Portal(300, platforms[0], "Basement");
            toDDRoom = new Portal(800, platforms[0], "Basement");
            toGeneratorRoom = new Portal(1500, platforms[0], "Basement");
            toPrison = new Portal(2100, platforms[0], "Basement", "Dungeon Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideHall, SideHall.ToBasement);
            portals.Add(toGeneratorRoom, GeneratorRoom.ToBasement);
            portals.Add(toPrison, Dungeon.ToBasement);
        }
    }
}
