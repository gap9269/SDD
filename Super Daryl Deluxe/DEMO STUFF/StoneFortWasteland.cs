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
    public class StoneFortWastelandDemo : MapClass
    {
        static Portal toWest;
        static Portal toBathroom;
        static Portal toAxisOfHistoricalReality;

        public static Portal ToWest { get { return toWest; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToAxisOfHistoricalReality { get { return toAxisOfHistoricalReality; } }

        Texture2D foreground, foreground2, sky, sky2, parallax, outhouse;

        public static Texture2D demoEndTexture;

        LivingLocker locker;

        public StoneFortWastelandDemo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            zoomLevel = 1f;

            mapWidth = 5500;
            mapHeight = 1160;
            mapName = "Stone Fort Wasteland";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            locker = new LivingLocker(game, new Rectangle(1000, 100, 1600, 500));
            interactiveObjects.Add(locker);

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Fort Wasteland\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Fort Wasteland\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\Fort Wasteland\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\Fort Wasteland\foreground2");
            sky = content.Load<Texture2D>(@"Maps\History\Fort Wasteland\sky");
            sky2 = content.Load<Texture2D>(@"Maps\History\Fort Wasteland\sky2");
            parallax = content.Load<Texture2D>(@"Maps\History\Fort Wasteland\parallax");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            demoEndTexture = content.Load<Texture2D>(@"Demo Stuff\DemoEnd");

            if (Chapter.lastMap != "Stone Fort - West")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_wasteland");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_wasteland", amb);
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (Chapter.theNextMap != "Stone Fort - West")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("North Hall");
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_wasteland");
        }

        public override void Update()
        {
            base.Update();

            // PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toAxisOfHistoricalReality = new Portal(5230, platforms[0], "Stone Fort Wasteland");
            toAxisOfHistoricalReality.FButtonYOffset = -40;
            toAxisOfHistoricalReality.PortalNameYOffset = -40;

            toWest = new Portal(100, platforms[0], "Stone Fort Wasteland");
            toWest.FButtonYOffset = -100;
            toWest.PortalNameYOffset = -100;

            toBathroom = new Portal(2980, platforms[0], "Stone Fort Wasteland");
            toBathroom.FButtonYOffset = -40;
            toBathroom.PortalNameYOffset = -40;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            //portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toWest, StoneFortWestDemo.ToWasteland);
            portals.Add(toAxisOfHistoricalReality, AxisOfHistoricalRealityDemo.ToWasteland);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Rectangle(2870,355, outhouse.Width, outhouse.Height), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(sky2, new Vector2(sky.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.96f, this, game));
            s.Draw(parallax, new Vector2(0, 0), Color.White);
            s.End();
        }
    }
}
