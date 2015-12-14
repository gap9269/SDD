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
    public class MainLobby : MapClass
    {
        static Portal toArtHall;
        static Portal toTheQuad;
        static Portal toBathroom;
        static Portal toSouthHall;
        public static Portal toMainOffice;

        public static Portal ToSouthHall { get { return toSouthHall; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToArtHall { get { return toArtHall; } }
        public static Portal ToTheQuad { get { return toTheQuad; } }

        Texture2D foreground, parallax;

        public static Rectangle sunRec;

        public MainLobby(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3425;
            mapName = "Main Lobby";

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            sunRec = new Rectangle(12, 256, 410, 456);

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            currentBackgroundMusic = Sound.MusicNames.NoirHalls;
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\mainlobby"));
            foreground = content.Load<Texture2D>(@"Maps\School\mainLobbyFore");
            parallax = content.Load<Texture2D>(@"Maps\School\mainLobbyParallax");

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Karma Shaman"] = content.Load<Texture2D>(@"NPC\DD\karma");
                Game1.npcFaces["Karma Shaman"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Karma");
            }
            else if(game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["The Magister of Maps"] = content.Load<Texture2D>(@"NPC\DD\maps");
                Game1.npcFaces["The Magister of Maps"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\MapsKid");

                game.NPCSprites["Weapons Master"] = content.Load<Texture2D>(@"NPC\DD\inventory");
                Game1.npcFaces["Weapons Master"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Equipment");
            }

            if (Chapter.lastMap != "East Hall" && Chapter.lastMap != "South Hall")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("NoirHalls", backgroundMusic);
            }

            if (Chapter.lastMap != "East Hall" && Chapter.lastMap != "South Hall")
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

            if (Chapter.theNextMap != "East Hall" && Chapter.theNextMap != "South Hall")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Karma Shaman"] = Game1.whiteFilter;
                Game1.npcFaces["Karma Shaman"].faces["Normal"] = Game1.whiteFilter;
            }
            else if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["The Magister of Maps"] = Game1.whiteFilter;
                Game1.npcFaces["The Magister of Maps"].faces["Normal"] = Game1.whiteFilter;

                game.NPCSprites["Weapons Master"] = Game1.whiteFilter;
                Game1.npcFaces["Weapons Master"].faces["Normal"] = Game1.whiteFilter;
            }
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

            toSouthHall = new Portal(640, platforms[0], "Main Lobby", Portal.DoorType.movement_door_open);
            toSouthHall.FButtonYOffset = -90;
            toSouthHall.PortalNameYOffset = -90;

            toArtHall = new Portal(2730, platforms[0], "Main Lobby", Portal.DoorType.movement_door_open);
            toArtHall.FButtonYOffset = -100;
            toArtHall.PortalNameYOffset = -100;

            toTheQuad = new Portal(2145, platforms[0], "Main Lobby", Portal.DoorType.movement_door_open);
            toTheQuad.FButtonYOffset = -105;
            toTheQuad.PortalNameYOffset = -105;

            toMainOffice = new Portal(3190, platforms[0], "Main Lobby", Portal.DoorType.movement_door_open);

            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toArtHall, EastHall.ToMainLobby);
            portals.Add(toSouthHall, SouthHall.ToMainLobby);
            portals.Add(toTheQuad, TheQuad.ToMainLobby);
           // portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Rectangle(0, 0, foreground.Width, 720), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.93f, this, game));
            s.Draw(parallax, new Vector2(-100, 0), Color.White);
            s.End();
        }
    }
}
