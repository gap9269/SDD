using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    public class SouthHall : MapClass
    {
        static Portal toSideHall;
        static Portal toUpstairs;
        static Portal toCafeteria;
        static Portal toPopularBathroom;
        static Portal toGymLobby;
        static Portal toMusic;


        public static Portal ToSideHall { get { return toSideHall; } }
        public static Portal ToUpstairs { get { return toUpstairs; } }
        public static Portal ToCafeteria { get { return toCafeteria; } }
        public static Portal ToGymLobby { get { return toGymLobby; } }
        public static Portal ToPopularBathroom { get { return toPopularBathroom; } }
        public static Portal ToMusic { get { return toMusic; } }

        public SouthHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 7352;
            mapName = "South Hall";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            enemyAmount = 0;

            AddPlatforms();
            AddBounds();
            SetPortals();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideHall = new Portal(200, platforms[0], "SouthHall");
            toUpstairs = new Portal(6700, platforms[0], "SouthHall");
            toCafeteria = new Portal(2000, platforms[0], "SouthHall");
            toPopularBathroom = new Portal(5500, platforms[0], "SouthHall", "Popular Bathroom Key");
            toGymLobby = new Portal(7000, platforms[0], "SouthHall");
            toMusic = new Portal(3000, platforms[0], "SouthHall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideHall, SideHall.ToSouthHall);
            portals.Add(toUpstairs, Upstairs.ToSouthHall);
            portals.Add(toCafeteria, Cafeteria.ToSouthHall);
            portals.Add(toPopularBathroom, Bathroom.ToLastMap);
            portals.Add(toGymLobby, GymLobby.ToSouthHall);
            portals.Add(toMusic, MusicIntroRoom.ToSouthHall);
        }
    }
}
