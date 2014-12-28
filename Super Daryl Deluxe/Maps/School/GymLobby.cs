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
    class GymLobby : MapClass
    {

        static Portal toNorthHall;
        static Portal toSouthHall;

        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToSouthHall { get { return toSouthHall; } }

        public GymLobby(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2400;
            mapName = "Gym Lobby";

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

            toNorthHall = new Portal(2100, platforms[0], "GymLobby");
            toSouthHall = new Portal(100, platforms[0], "GymLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNorthHall, NorthHall.ToGymLobby);
            portals.Add(toSouthHall, SouthHall.ToGymLobby);
        }
    }
}
