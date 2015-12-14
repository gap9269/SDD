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


    class EastHall : MapClass
    {
        static Portal toMainLobby;
        static Portal toNorthHall;
        static Portal toJanitorsCloset;
        static Portal toKitchen;

        public static Portal ToKitchen { get { return toKitchen; } }
        public static Portal ToMainLobby { get { return toMainLobby; } }
        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToJanitorsCloset { get { return toJanitorsCloset; } }

        Texture2D kitchenDoor;
        float doorAlpha = 0f;

        public EastHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1900;
            mapName = "East Hall";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                toJanitorsCloset.ItemNameToUnlock = null;
                background.Add(content.Load<Texture2D>(@"Maps\School\EastHall\\backgroundasbestos"));
            }
            else
                background.Add(content.Load<Texture2D>(@"Maps\School\EastHall\\JanitorHall"));

            kitchenDoor = content.Load<Texture2D>(@"Maps\School\EastHall\\kitchenDoor");

            if (game.SideQuestManager.nPCs.ContainsKey("The Gardener"))
            {
                game.NPCSprites["The Gardener"] = content.Load<Texture2D>(@"NPC\Main\gardener");

                Game1.npcFaces["The Gardener"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Groundskeeper");

                SoundEffect bg = content.Load<SoundEffect>(@"Sound\Music\FurryFuneral");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("FurryFuneral", backgroundMusic);
            }

            if (game.chapterState == Game1.ChapterState.prologue && Chapter.lastMap != "North Hall" && Chapter.lastMap != "Main Lobby" && Chapter.lastMap != "Kitchen")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_school_empty", amb);
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (game.SideQuestManager.nPCs.ContainsKey("The Gardener"))
            {
                game.NPCSprites["The Gardener"] = Game1.whiteFilter;

                Game1.npcFaces["The Gardener"].faces["Normal"] = Game1.whiteFilter;

                Sound.music.Remove("FurryFuneral");
            }

            if (game.chapterState == Game1.ChapterState.prologue && Chapter.theNextMap != "North Hall" && Chapter.theNextMap != "Main Lobby" && Chapter.theNextMap != "Kitchen")
                Sound.UnloadAmbience();

        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }


        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_school_empty");
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

            toMainLobby = new Portal(1610, platforms[0], "East Hall", Portal.DoorType.movement_door_open);
            toNorthHall = new Portal(50, platforms[0], "East Hall", Portal.DoorType.movement_door_open);
            toKitchen = new Portal(1110, platforms[0], "East Hall", Portal.DoorType.movement_door_open);
            toJanitorsCloset = new Portal(607, platforms[0], "East Hall", "Closet Key", Portal.DoorType.movement_door_open);


            toNorthHall.FButtonYOffset = -10;
            toNorthHall.PortalNameYOffset = -10;

            toJanitorsCloset.FButtonYOffset = -20;
            toJanitorsCloset.PortalNameYOffset = -20;

            toKitchen.FButtonYOffset = -10;
            toKitchen.PortalNameYOffset = -10;

            toMainLobby.FButtonYOffset = -20;
            toMainLobby.PortalNameYOffset = -20;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainLobby, MainLobby.ToArtHall);
            portals.Add(toNorthHall, NorthHall.ToArtHall);
            portals.Add(toKitchen, Kitchen.ToEastHall);
            portals.Add(toJanitorsCloset, JanitorCloset.ToArtHall);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            int disToDoor = Math.Abs(player.VitalRec.Center.X - (834 + kitchenDoor.Width / 2));

            if (disToDoor < 300)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if(EastHall.ToKitchen.IsUseable)
                s.Draw(kitchenDoor, new Vector2(834, 371), Color.White * doorAlpha);

            s.End();
        }
    }
}
