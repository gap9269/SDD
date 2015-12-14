using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    public class SouthHall : MapClass
    {
        static Portal toMainLobby;
        static Portal toDwarvesAndDruids;
        static Portal toGymLobby;
        static Portal toMusicAndArt;


        public static Portal ToMainLobby { get { return toMainLobby; } }
        public static Portal ToDwarvesAndDruids { get { return toDwarvesAndDruids; } }
        public static Portal ToGymLobby { get { return toGymLobby; } }
        public static Portal ToMusicAndArt { get { return toMusicAndArt; } }

        Texture2D laserSprite, DDPoster, DDLabel, musicLabel; 
        int laserDelay = 5;
        int laserFrame = 0;

        public SouthHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 5000;
            mapName = "South Hall";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            enemyAmount = 0;

            AddPlatforms();
            AddBounds();
            SetPortals();

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;

        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_school_empty");
        }


        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\South Hall\background"));
            background.Add(content.Load<Texture2D>(@"Maps\School\South Hall\background2"));

            //titsLabel = content.Load<Texture2D>(@"Maps\RoomLabels\TITS Banner");
            //titsStickers = content.Load<Texture2D>(@"Maps\RoomLabels\TITS Stickers");
            DDLabel = content.Load<Texture2D>(@"Maps\RoomLabels\D&D Banner");
            DDPoster = content.Load<Texture2D>(@"Maps\RoomLabels\D&D Poster");
            laserSprite = content.Load<Texture2D>(@"Maps\School\South Hall\laserSprite");

            game.NPCSprites["Journal Enthusiast"] = content.Load<Texture2D>(@"NPC\Main\Journal Kid");
            Game1.npcFaces["Journal Enthusiast"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\JournalKid");

            if (Chapter.lastMap != "Main Lobby" && Chapter.lastMap != "Gym Lobby" && Chapter.lastMap != "Dwarves & Druids Club")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("NoirHalls", backgroundMusic);
            }

            if (Chapter.lastMap != "Main Lobby" && Chapter.lastMap != "Gym Lobby" && Chapter.lastMap != "Dwarves & Druids Club")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_school_empty", amb);
            }
            if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["Karma Shaman"] = content.Load<Texture2D>(@"NPC\DD\karma");
                Game1.npcFaces["Karma Shaman"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Karma");

                game.CurrentChapter.NPCs["Karma Shaman"].Dialogue.Clear();
                game.CurrentChapter.NPCs["Karma Shaman"].Dialogue.Add("Welcome almighty \"" + player.SocialRank + "\", to the the High Guild Hall, home of all things \"Dwarves and Druids\"! Perhaps you are seeking new quests or a beautiful wench to satisfy your boredom?");
                game.CurrentChapter.NPCs["Karma Shaman"].Dialogue.Add("Step inside, and let my brothers whisk away your aches and worries with endless ale and roasted boar! And may good fortune follow you.");
            }

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Journal Enthusiast"] = Game1.whiteFilter;
            Game1.npcFaces["Journal Enthusiast"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "Main Lobby" && Chapter.theNextMap != "Gym Lobby" && Chapter.theNextMap != "Dwarves & Druids Club")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }

            if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["Karma Shaman"] = Game1.whiteFilter;
                Game1.npcFaces["Karma Shaman"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void Update()
        {
            base.Update();
            laserDelay--;

            if (laserDelay < 0)
            {
                laserFrame++;
                laserDelay = 5;

                if (laserFrame > 2)
                    laserFrame = 0;
            }
            PlayBackgroundMusic();
            PlayAmbience();

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMainLobby = new Portal(25, platforms[0], "South Hall", Portal.DoorType.movement_door_open);
            toGymLobby = new Portal(4800, platforms[0], "South Hall", Portal.DoorType.movement_door_open);
            toMusicAndArt = new Portal(2090, platforms[0], "South Hall");

            toMusicAndArt.FButtonYOffset = -30;
            toMusicAndArt.PortalNameYOffset = -30;

            toMainLobby.FButtonYOffset = -20;
            toMainLobby.PortalNameYOffset = -20;

            toGymLobby.FButtonYOffset = -20;
            toGymLobby.PortalNameYOffset = -20;
            toDwarvesAndDruids = new Portal(1225, platforms[0], "South Hall");

            toDwarvesAndDruids.FButtonYOffset = -30;
            toDwarvesAndDruids.PortalNameYOffset = -30;
            //toUpstairs = new Portal(6700, platforms[0], "South Hall");
            //toPopularBathroom = new Portal(5500, platforms[0], "South Hall", "Popular Bathroom Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainLobby, MainLobby.ToSouthHall);
           // portals.Add(toUpstairs, Upstairs.ToSouthHall);
            portals.Add(toDwarvesAndDruids, DwarvesAndDruidsRoom.ToSouthHall);
           // portals.Add(toPopularBathroom, Bathroom.ToLastMap);
            portals.Add(toGymLobby, GymLobby.ToSouthHall);
            portals.Add(toMusicAndArt, MusicIntroRoom.ToSouthHall);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(DDLabel, new Vector2(1195, 280), Color.White);
            s.Draw(DDPoster, new Vector2(1315, 405), Color.White);

            //s.Draw(titsLabel, new Vector2(1248, 285), Color.White);
            //s.Draw(titsStickers, new Rectangle(1290, 405, titsStickers.Width, titsStickers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            s.Draw(laserSprite, new Rectangle(3637, 276, 209, 330), new Rectangle(209 * laserFrame, 0, 209, 330), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.End();
        }
    }
}
