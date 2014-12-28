using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class OutsideEnemyCamp : MapClass
    {
        static Portal toCampWest;

        public static Portal ToCampWest { get { return toCampWest; } }

        Texture2D foreground, parallax;

        public OutsideEnemyCamp(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            zoomLevel = .8f;

            mapWidth = 5000;
            mapHeight = 1750;
            mapName = "Outside Enemy Camp";

            //backgroundMusicName = "Noir Halls";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(Game1.whiteFilter);
          //  foreground = content.Load<Texture2D>(@"Maps\School\mainLobbyFore");
           // parallax = content.Load<Texture2D>(@"Maps\School\mainLobbyParallax");

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("North Hall", backgroundMusic);
            //}

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
            //    SoundEffectInstance amb = am.CreateInstance();
            //    amb.IsLooped = true;
            //    Sound.ambience.Add("North Hall", amb);
            //}
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (Chapter.theNextMap != "EastHall")
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
            //Sound.PlayAmbience("North Hall");
        }

        public override void Update()
        {
            base.Update();
            zoomLevel = .8f;
           // PlayBackgroundMusic();
           // PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCampWest = new Portal(50, platforms[0], "OutsideEnemyCamp");
            toCampWest.FButtonYOffset = -100;
            toCampWest.PortalNameYOffset = -100;

            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

             portals.Add(toCampWest, Bathroom.ToLastMap);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            yScroll = true;
            
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            //s.Draw(foreground, new Rectangle(0, 0, foreground.Width, 720), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.93f, this, game));
            //s.Draw(parallax, new Vector2(-100, 0), Color.White);
            s.End();
        }
    }
}
