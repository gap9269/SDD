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
    class SideHall : MapClass
    {

        static Portal toMainLobby;
        static Portal toSouthHall;
        static Portal toBasement;

        public static Portal ToMainLobby { get { return toMainLobby; } }
        public static Portal ToSouthHall { get { return toSouthHall; } }
        public static Portal ToBasement { get { return toBasement; } }

        public SideHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1900;
            mapName = "Side Hall";

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

            toMainLobby = new Portal(150, platforms[0], "SideHall");
            toSouthHall = new Portal(1700, platforms[0], "SideHall");
            toBasement = new Portal(1000, platforms[0], "SideHall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainLobby, MainLobby.ToSouthHall);
            portals.Add(toSouthHall, SouthHall.ToMainLobby);
            portals.Add(toBasement, Basement.ToSideHall);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
