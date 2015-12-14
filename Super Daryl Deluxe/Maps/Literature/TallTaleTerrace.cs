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
    class TallTaleTerrace : MapClass
    {
        static Portal toWelcomeToMiddleEarth;
        static Portal toSnowyStreets;

        public static Portal ToWelcomeToMiddleEarth { get { return toWelcomeToMiddleEarth; } }
        public static Portal ToSnowyStreets { get { return toSnowyStreets; } }

        Texture2D foreground, boat, parallax, sky, door, lamp;

        float doorAlpha = 0f;
        public TallTaleTerrace(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 9000;
            mapName = "Tall Tale Terrace";

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
            background.Add(content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\foreground");
            parallax = content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\parallax");
            boat = content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\boat");
            sky = content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\sky");
            door = content.Load<Texture2D>(@"Maps\Music\Entrance\door");
            lamp = content.Load<Texture2D>(@"Maps\Literature\TallTaleTerrace\lamp");

        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWelcomeToMiddleEarth = new Portal(300, platforms[0], "Tall Tale Terrace");
            toSnowyStreets = new Portal(2270, platforms[0], "Tall Tale Terrace");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSnowyStreets, SnowyStreets.ToTallTaleTerrace);
            portals.Add(toWelcomeToMiddleEarth, WelcomeToMiddleEarth.ToTallTaleTerrace);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
                s.Draw(foreground, new Vector2(0, 0), Color.White);

                //if (player.VitalRec.X < 3500 && player.VitalRecX > 2790)
                //{
                //    if (doorAlpha < .7f)
                //        doorAlpha += .05f;
                //}
                //else
                //{
                //    if (doorAlpha > 0)
                //        doorAlpha -= .05f;
                //}

                //s.Draw(door, new Vector2(3119, 390), Color.White * doorAlpha);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.4f, this, game));
            s.Draw(lamp, new Vector2(2300, 0), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallax, new Vector2(0, 0), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(boat, new Vector2(6640, 0), Color.White);


            s.End();
        }
    }
}
