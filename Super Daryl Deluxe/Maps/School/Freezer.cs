using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Freezer:MapClass
    {
        static Portal toCafeteria;

        public static Portal ToCafeteria { get { return toCafeteria; } }

        public Freezer(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Freezer";

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

            toCafeteria = new Portal(50, platforms[0], "Freezer");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCafeteria, Cafeteria.ToFreezer);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
