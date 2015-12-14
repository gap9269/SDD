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
    class MusicIntroRoom : MapClass
    {
        static Portal toSouthHall;
        static Portal toEntranceHall;
        static Portal toCityStreets;
        static Portal toBridge;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToBridge { get { return toBridge; } }
        public static Portal ToCityStreets { get { return toCityStreets; } }
        public static Portal ToSouthHall { get { return toSouthHall; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }

        Texture2D foreground, foreground2, sky, door, theaterParallax, streetsParallax, outhouseTexture;

        float doorAlpha;

        public MusicIntroRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 5000;
            mapName = "Intro To Music";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Entrance\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Music\Entrance\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Entrance\foreground");
            //foreground2 = content.Load<Texture2D>(@"Maps\Music\Entrance\foreground2");
            sky = content.Load<Texture2D>(@"Maps\Music\Entrance\sky");
            streetsParallax = content.Load<Texture2D>(@"Maps\Music\Entrance\streetsParallax");
            theaterParallax = content.Load<Texture2D>(@"Maps\Music\Entrance\theaterParallax");
            door = content.Load<Texture2D>(@"Maps\Music\Entrance\door");
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");
        
        
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSouthHall = new Portal(3065, platforms[0], "Intro To Music");
            toBridge = new Portal(50, platforms[0], "Intro To Music");
            toEntranceHall = new Portal(3943, platforms[0], "Intro To Music");
            toCityStreets = new Portal(4750, platforms[0], "Intro To Music");
            toBathroom = new Portal(4450, platforms[0], "Intro To Music");

            toBathroom.FButtonYOffset = -8;
            toBathroom.PortalNameYOffset = -8;

            toSouthHall.FButtonYOffset = -25;
            toSouthHall.PortalNameYOffset = -25;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouseTexture, new Vector2(4340, platforms[0].RecY - outhouseTexture.Height - 15), null, Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toSouthHall, SouthHall.ToMusicAndArt);
            portals.Add(toEntranceHall, EntranceHall.ToIntroRoom);
            portals.Add(toCityStreets, CityStreets.ToIntroRoom);
            portals.Add(toBridge, BridgeOfArmanhand.ToEntrance);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.VitalRec.X < 3500 && player.VitalRecX > 2790)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(2790, 371), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(mapRec.Width - foreground2.Width, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(streetsParallax, new Vector2(4804, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(theaterParallax, new Vector2(3821, mapRec.Y), Color.White);
            s.End();
        }
    }
}
