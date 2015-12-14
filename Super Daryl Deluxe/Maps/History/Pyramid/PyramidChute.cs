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
    class PyramidChute : MapClass
    {
        static Portal toBasementEntrance;

        public static Portal fromCliffOfIle;
        public static Portal fromPitOfLongFalls;

        public static Portal ToBasementEntrance { get { return toBasementEntrance; } }

        Texture2D foreground;

        public PyramidChute(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 4000;
            mapName = "Pyramid Chute";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 5;
            yScroll = true;
            zoomLevel = .8f;
            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            treasureChests.Add(new TreasureChest(Game1.treasureChestSheet, 1610, -1315 + 180, player, 0, new EnemyDrop("Ruby", new Rectangle()), this));

            PyramidKey key = new PyramidKey(2127, mapRec.Y +  1637);
            collectibles.Add(key);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PyramidChute\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PyramidChute\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

       
        public override void Update()
        {
            base.Update();

            if (player.VelocityY > 23)
                game.Camera.Center = game.Camera.centerTarget;

            if (player.VelocityY > 24)
                player.VelocityY = 24;

            if (player.VitalRecY > 800)
            {
                ForceToNewMap(new Portal(600, 800, "Pyramid Chute"), ThePit.fromChute);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBasementEntrance = new Portal(3800, platforms[0], "Pyramid Chute");

            fromCliffOfIle = new Portal(600, mapRec.Y - 100, mapName);
            fromPitOfLongFalls = new Portal(3040, mapRec.Y - 100, mapName);
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();
            portals.Add(toBasementEntrance, BasementEntrance.ToPyramidChute);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
