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
    class AxisOfHistoricalRealityDemo : MapClass
    {
        static Portal toWasteland;
        public static Portal ToWasteland { get { return toWasteland; } }

        public AxisOfHistoricalRealityDemo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Axis of Historical Reality";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }


        public override void LoadEnemyData()
        {
            base.LoadEnemyData();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Axis of Musical Reality\background"));
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWasteland = new Portal(50, platforms[0], "Axis of Historical Reality");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWasteland, StoneFortWastelandDemo.ToAxisOfHistoricalReality);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
            s.End();
        }
    }
}
