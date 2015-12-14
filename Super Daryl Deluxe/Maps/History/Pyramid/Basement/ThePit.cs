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
    class ThePit : MapClass
    {
        static Portal toUndergroundTunnelII;
        public static Portal ToUndergroundTunnelII { get { return toUndergroundTunnelII; } }

        public static Portal fromChute;
        Sparkles sparkles;
        Texture2D foreground, skeletonWithJar;

        public ThePit(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "The Pit";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            sparkles = new Sparkles(1830, 545);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ThePit\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ThePit\foreground");
            skeletonWithJar = content.Load<Texture2D>(@"Maps\History\Pyramid\ThePit\skeletonWithJar");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (player.VitalRec.Intersects(sparkles.rec) && ((last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space)) || MyGamePad.RightTriggerPressed()) && game.ChapterTwo.ChapterTwoBooleans["jarFound"] == false)
            {
                player.AddStoryItem("Empty Bottle", "an Empty Bottle", 1);
                game.ChapterTwo.ChapterTwoBooleans["jarFound"] = true;
            }
            if (!game.ChapterTwo.ChapterTwoBooleans["jarFound"])
            {
                sparkles.Update();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toUndergroundTunnelII = new Portal(80, platforms[0], "The Pit");
            fromChute = new Portal(1600, -100, mapName);
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["jarFound"] == false)
                s.Draw(skeletonWithJar, new Vector2(1729, 484), Color.White);

            if (!game.ChapterTwo.ChapterTwoBooleans["jarFound"])
                sparkles.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUndergroundTunnelII, UndergroundTunnelII.ToThePit);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
