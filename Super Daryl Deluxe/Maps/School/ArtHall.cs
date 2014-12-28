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


    class ArtHall : MapClass
    {
        static Portal toMainLobby;
        static Portal toNorthHall;
        static Portal toJanitorsCloset;

        public static Portal ToMainLobby { get { return toMainLobby; } }
        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToJanitorsCloset { get { return toJanitorsCloset; } }

        public ArtHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1900;
            mapName = "East Hall";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            backgroundMusicName = "Noir Halls";

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\JanitorHall"));

            if (game.SideQuestManager.nPCs.ContainsKey("The Gardener"))
            {
                game.NPCSprites["The Gardener"] = content.Load<Texture2D>(@"NPC\Main\gardener");

                Game1.npcFaces["The Gardener"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Groundskeeper");
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (game.SideQuestManager.nPCs.ContainsKey("The Gardener"))
            {
                game.NPCSprites["The Gardener"] = Game1.whiteFilter;

                Game1.npcFaces["The Gardener"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }


        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic("North Hall");
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("North Hall");
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMainLobby = new Portal(1610, platforms[0], "EastHall");
            toNorthHall = new Portal(50, platforms[0], "EastHall");
            toJanitorsCloset = new Portal(607, platforms[0], "EastHall", "Closet Key");

            toNorthHall.FButtonYOffset = -10;
            toNorthHall.PortalNameYOffset = -10;

            toJanitorsCloset.FButtonYOffset = -20;
            toJanitorsCloset.PortalNameYOffset = -20;

            toMainLobby.FButtonYOffset = -20;
            toMainLobby.PortalNameYOffset = -20;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainLobby, MainLobby.ToArtHall);
            portals.Add(toNorthHall, NorthHall.ToArtHall);
            portals.Add(toJanitorsCloset, JanitorCloset.ToArtHall);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
