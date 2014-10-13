using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class PrincessLockerRoom : MapClass
    {
        static Portal toUpperVentsVI;
        static Portal toUpperVentsI;

        public static Portal ToUpperVentsVI { get { return toUpperVentsVI; } }
        public static Portal ToUpperVentsI { get { return toUpperVentsI; } }

        public PrincessLockerRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 2600;
            mapName = "Princess's Locker Room";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVentsVI = new Portal(50, -420, "PrincessLockerRoom");
            toUpperVentsI = new Portal(100, 630, "PrincessLockerRoom");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVentsVI, UpperVentsVI.ToPrincess);
            portals.Add(toUpperVentsI, UpperVents1.ToPrincess);
        }
    }
}