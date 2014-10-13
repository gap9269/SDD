using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Furnace : MapClass
    {
        static Portal toUpperVentsVI;

        public static Portal ToUpperVentsVI { get { return toUpperVentsVI; } }

        public Furnace(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Furnace Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            TreasureChest chest = new TreasureChest(Game1.treasureChestSheet, 1300, 630, player, 0, new ChallengeRoomKey(true), this);
            treasureChests.Add(chest);
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVentsVI = new Portal(50, platforms[0], "FurnaceRoom");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVentsVI, UpperVentsVI.ToFurnace);
        }
    }
}