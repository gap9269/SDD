using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Cafeteria : MapClass
    {
        static Portal toSouthHall;
        static Portal toFreezer;

        public static Portal ToFreezer { get { return toFreezer; } }
        public static Portal ToSouthHall { get { return toSouthHall; } }

        public Cafeteria(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1900;
            mapName = "Cafeteria";

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

            toSouthHall = new Portal(50, platforms[0], "Cafeteria");
            toFreezer = new Portal(1700, platforms[0], "Cafeteria", "Freezer Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSouthHall, SouthHall.ToCafeteria);
            portals.Add(toFreezer, Freezer.ToCafeteria);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
