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
    class UndergroundTunnelII : MapClass
    {
        static Portal toUndergroundTunnelI;
        static Portal toThePit;
        public static Portal ToThePit { get { return toThePit; } }
        public static Portal ToUndergroundTunnelI { get { return toUndergroundTunnelI; } }

        Texture2D foreground, rocks, foregroundRocks;
        Platform rock1, rock2;
        public UndergroundTunnelII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1200;
            mapWidth = 1800;
            mapName = "Underground Tunnel II";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 2;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            rock1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(570, 433, 100, 50), true, false, false);
            rock2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(386, 503, 100, 50), true, false, false);

            collectibles.Add(new SilverKey(900, 80));

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel2\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel2\foreground");
            rocks = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel2\rocks");
            foregroundRocks = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel2\foregroundRocks");
        }

        public override void Update()
        {
            base.Update();

            if (game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"] && !platforms.Contains(rock1))
            {
                platforms.Add(rock1);
                platforms.Add(rock2);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUndergroundTunnelI = new Portal(85, platforms[0], "Underground Tunnel II");
            toThePit = new Portal(1548, platforms[0], "Underground Tunnel II");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"])
                s.Draw(rocks, new Vector2(312, mapRec.Y + 536), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUndergroundTunnelI, UndergroundTunnelI.ToUndergroundTunnelII);
            portals.Add(toThePit, ThePit.ToUndergroundTunnelII);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
              if (game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"])
                s.Draw(foregroundRocks, new Vector2(93, mapRec.Y + 727), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
