using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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

        Texture2D foreground;

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

            steamSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(1780, 100, (int)(333 * .8f), (int)(335 * .8f)));
            switches.Add(steamSwitch);

            steam = new MapSteam(120, 100, 1570, 15, game, 1, true);

            mapHazards.Add(steam);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 1\background"));
            Sound.LoadVentZoneSounds();
            foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 1\foreground");

            foreach (MapSteam s in mapHazards)
            {
                s.object_steam_vent_loop = Sound.mapZoneSoundEffects["object_steam_vent_loop"].CreateInstance();
            }

            if (Chapter.lastMap != "Upper Vents II" && Chapter.lastMap != "Side Vents I" && Chapter.lastMap != "Princess' Room")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_vents");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_vents", amb);
            }
        }
        public override void UnloadContent()
        {
            base.UnloadContent();

            if (Chapter.theNextMap != "Upper Vents II" && Chapter.theNextMap != "Side Vents I" && Chapter.theNextMap != "Princess' Room")
            {
                Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
                Sound.UnloadMapZoneSounds();
            }
        }
        public override void StopSounds()
        {
            base.StopSounds();

            foreach (MapSteam s in mapHazards)
            {
                
            }
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();
            PlayAmbience();

            
            if (CheckSwitch(steamSwitch))
            {
                if (steamSwitch.Active)
                {
                    steam.TurnOff();
                }
            }

            if (steamSwitch.Active && steam.Active && steam.CurrentlyEnding == false)
            {
                steam.TurnOff();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpstairs = new Portal(50, 630, "Upper Vents I");
            toSideVents = new Portal(50, 40, "Upper Vents I");
            toSpiderVents = new Portal(1200, -460, "Upper Vents I");
            toUpperVents2 = new Portal(2600, 40, "Upper Vents I");
            toPrincess = new Portal(2000, 380, "Upper Vents I");

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

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
          
            s.End();

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }
    }
}
