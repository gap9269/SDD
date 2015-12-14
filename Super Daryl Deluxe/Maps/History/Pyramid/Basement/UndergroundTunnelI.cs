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
    class UndergroundTunnelI : MapClass
    {
        static Portal toBasementStairs;
        static Portal toUndergroundTunnelII;
        static Portal toChamber44;
        public static Portal ToChamber44 { get { return toChamber44; } }
        public static Portal ToUndergroundTunnelII { get { return toUndergroundTunnelII; } }
        public static Portal ToBasementStairs { get { return toBasementStairs; } }

        Texture2D foreground, platform, farback;
        Boolean opening = false;
        int doorPosX = 600;
        Platform door;
        WallSwitch doorSwitch;
        public UndergroundTunnelI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Underground Tunnel I";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            if (!game.ChapterTwo.ChapterTwoBooleans["undergroundTunnelPlatformOpen"])
            {
                door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(639, 413, 250, 50), false, false, false);
                platforms.Add(door);
            }

            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(1050, 380, 333, 335));
            switches.Add(doorSwitch);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel1\middle"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel1\foreground");
            platform = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel1\platform");
            farback = content.Load<Texture2D>(@"Maps\History\Pyramid\UndergroundTunnel1\background");

            if (game.ChapterTwo.ChapterTwoBooleans["undergroundTunnelPlatformOpen"])
                doorPosX = 300;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);

            while (enemiesInMap.Count < 1)
            {
                RespawnGroundEnemies();
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Mummy en = new Mummy(pos, "Mummy", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.SpawnWithPoof = false;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }

            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (doorSwitch.Active)
            {
                game.ChapterTwo.ChapterTwoBooleans["undergroundTunnelPlatformOpen"] = true;
                opening = true;
            }
            if (opening && doorPosX > 300)
            {
                doorPosX -= 5;
                Game1.camera.ShakeCamera(1, 2);
            }
            if (game.ChapterTwo.ChapterTwoBooleans["undergroundTunnelPlatformOpen"])
            {
                if (platforms.Contains(door))
                    platforms.Remove(door);
            }

            if (!doorSwitch.Active)
            {
                CheckSwitch(doorSwitch);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBasementStairs = new Portal(200, platforms[1], "Underground Tunnel I");
            toUndergroundTunnelII = new Portal(1160, platforms[1], "Underground Tunnel I");
            toChamber44 = new Portal(100, platforms[0], "Underground Tunnel I");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBasementStairs, BasementStairs.ToUndergroundTunnelI);
            portals.Add(toUndergroundTunnelII, UndergroundTunnelII.ToUndergroundTunnelI);
            portals.Add(toChamber44, Chamber44.ToUndergroundTunnelI);
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
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(farback, new Vector2(0, 0), Color.White);
            s.Draw(platform, new Vector2(doorPosX, mapRec.Y + 403), Color.White);
            s.End();
        }
    }
}
