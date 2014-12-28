using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class TheaterAnDerWien : MapClass
    {
        static Portal toEntranceHall;
        static Portal toMusicRoom;

        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToMusicRoom { get { return toMusicRoom; } }

        public TheaterAnDerWien(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Theater An Der Wien";

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

            toEntranceHall = new Portal(1050, 478, "TheaterAnDerWien");
            toMusicRoom = new Portal(50, platforms[0], "TheaterAnDerWien");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMusicRoom, MusicIntroRoom.ToVienna);
            portals.Add(toEntranceHall, EntranceHall.ToTheaterAnDerWien);
        }
    }
}
