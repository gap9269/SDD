using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class UpperVents1 : MapClass
    {

        static Portal toUpstairs;
        static Portal toSideVents;
        static Portal toSpiderVents;
        static Portal toUpperVents2;
        static Portal toPrincess;

        public static Portal ToUpstairs { get { return toUpstairs; } }
        public static Portal ToSideVents { get { return toSideVents; } }
        public static Portal ToSpiderVents { get { return toSpiderVents; } }
        public static Portal ToUpperVents2 { get { return toUpperVents2; } }
        public static Portal ToPrincess { get { return toPrincess; } }

        WallSwitch steamSwitch;
        MapSteam steam;

        public UpperVents1(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 2900;
            mapHeight = 3000;
            mapName = "Upper Vents I";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            steamSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(1900, 240, 42, 83));
            switches.Add(steamSwitch);

            steam = new MapSteam(new Rectangle(1680, 100, 100, 300), game, 1, new Vector2(0, 0), false, false, true);
            mapHazards.Add(steam);
        }

        public override void Update()
        {
            base.Update();

            if (steamSwitch.Active)
            {
                steam.Active = false;
            }

            CheckSwitch(steamSwitch);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpstairs = new Portal(50, 630, "UpperVentsI");
            toSideVents = new Portal(50, 40, "UpperVentsI");
            toSpiderVents = new Portal(1200, -460, "UpperVentsI");
            toUpperVents2 = new Portal(2600, 40, "UpperVentsI");
            toPrincess = new Portal(2000, 380, "UpperVentsI");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideVents, SideVentsI.ToUpperVentsI);
            portals.Add(toUpstairs, Upstairs.ToUpperVents);
            portals.Add(toUpperVents2, UpperVentsII.ToUpperVents1);
            portals.Add(toPrincess, PrincessLockerRoom.ToUpperVentsI);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
    }
}
