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
    class ForestPath : MapClass
    {
        static Portal toBehindTheGreatWall;
        static Portal toForestOfEnts;
        public static Portal toBathroom;

        public static Portal ToBehindTheGreatWall { get { return toBehindTheGreatWall; } }
        public static Portal ToForestOfEnts { get { return toForestOfEnts; } }

        Texture2D sky, parallax, parallaxFar, outhouseTexture;
        Dictionary<String, Texture2D> portal;

        int portalFrame;
        int portalFrameDelay = 5;
        public ForestPath(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Forest Path";

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
            background.Add(content.Load<Texture2D>(@"Maps\Literature\Forest Path\background"));
            sky = content.Load<Texture2D>(@"Maps\Literature\Forest Path\sky");
            parallax = content.Load<Texture2D>(@"Maps\Literature\Forest Path\parallax");
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");
            parallaxFar = content.Load<Texture2D>(@"Maps\Literature\Forest Path\parallaxFar");

            portal = ContentLoader.LoadContent(content, @"Maps\History\InsideGreatWall\Portal");

        }

        public override void Update()
        {
            base.Update();

            portalFrameDelay--;

            if (portalFrameDelay < 0)
            {
                portalFrame++;
                portalFrameDelay = 3;

                if (portalFrame > portal.Count - 1)
                    portalFrame = 0;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBehindTheGreatWall = new Portal(50, platforms[0], "Forest Path");
            toForestOfEnts = new Portal(2750, platforms[0], "Forest Path");
            toBathroom = new Portal(636, platforms[0], "Forest Path");

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(portal["portal" + portalFrame], new Vector2(-12, 0), Color.White);

            s.Draw(outhouseTexture, new Vector2(526, platforms[0].RecY - outhouseTexture.Height), null, Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBehindTheGreatWall, BehindTheGreatWall.toForestPath);
            portals.Add(toForestOfEnts, ForestOfEnts.ToForestPath);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
           // s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.18f, this, game));
            s.Draw(parallaxFar, new Rectangle(0, 0, parallaxFar.Width, parallaxFar.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(parallax, new Rectangle(0, 0, parallax.Width, parallax.Height), Color.White);
            s.End();
        }
    }
}
