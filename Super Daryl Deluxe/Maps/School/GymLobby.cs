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
        static Portal toBathroom;
        static Portal toSouthHall;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToNorthHall { get { return toNorthHall; } }
        public static Portal ToSouthHall { get { return toSouthHall; } }

        Texture2D foreground;

        public static Rectangle sunRec;

        public GymLobby(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3200;
            mapName = "Gym Lobby";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            sunRec = new Rectangle(2720, 295, 466, 404);

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\Gym Lobby\gym lobby"));
            foreground = content.Load<Texture2D>(@"Maps\School\Gym Lobby\fore");

            if (Chapter.lastMap != "South Hall" && Chapter.lastMap != "North Hall")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("NoirHalls", backgroundMusic);
            }

            if (Chapter.lastMap != "South Hall" && Chapter.lastMap != "North Hall")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_school_empty", amb);
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

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (Chapter.theNextMap != "South Hall" && Chapter.theNextMap != "North Hall")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNorthHall = new Portal(2420, platforms[0], "Gym Lobby", Portal.DoorType.movement_door_open);
            toSouthHall = new Portal(460, platforms[0], "Gym Lobby", Portal.DoorType.movement_door_open);
            toBathroom = new Portal(1025, platforms[0], "Gym Lobby", Portal.DoorType.movement_door_open);

            ToNorthHall.FButtonYOffset = -30;
            toNorthHall.PortalNameYOffset = -30;

            toSouthHall.FButtonYOffset = -30;
            toSouthHall.PortalNameYOffset = -30;

            toBathroom.FButtonYOffset = -30;
            toBathroom.PortalNameYOffset = -30;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNorthHall, NorthHall.ToGymLobby);
            portals.Add(toSouthHall, SouthHall.ToGymLobby);
            portals.Add(toBathroom, Bathroom.ToLastMap);
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
    }
}
