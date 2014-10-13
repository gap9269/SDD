using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVentsVI : MapClass
    {
        static Portal toUpperVents5;
        static Portal toPrincess;
        static Portal toFurance;

        public static Portal ToFurnace { get { return toFurance; } }
        public static Portal ToUpperVents5 { get { return toUpperVents5; } }
        public static Portal ToPrincess { get { return toPrincess; } }

        MoveBlock buddyBlock;

        public UpperVentsVI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;

            mapWidth = 3540;
            mapHeight = 2820;
            mapName = "Upper Vents VI";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            overlayType = 1;
            AddPlatforms();
            AddBounds();
            SetPortals();

            buddyBlock = new MoveBlock(Game1.platformTextures.ElementAt(1).Value, new Rectangle(2180, -1560, 200, 150), 4);
            moveBlocks.Add(buddyBlock);
            platforms.Add(buddyBlock);
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents5 = new Portal(50, 630, "UpperVentsVI");
            toFurance = new Portal(3300, -660, "UpperVentsVI");
            toPrincess = new Portal(2920, 220, "UpperVentsVI");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents5, UpperVentsV.ToUpperVents6);
            portals.Add(toFurance, Furnace.ToUpperVentsVI);
            portals.Add(toPrincess, PrincessLockerRoom.ToUpperVentsVI);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
