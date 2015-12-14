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
    class AxisOfHistoricalReality : MapClass
    {
        static Portal toWasteland;
        public static Portal ToWasteland { get { return toWasteland; } }

        Texture2D foreground;

        public AxisOfHistoricalReality(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1420;
            mapWidth = 2500;
            mapName = "Axis of Historical Reality";

            mapRec = new Rectangle(0, -410, mapWidth, 1100);
            enemyAmount = 1;
            zoomLevel = .95f;
            yScroll = true;

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
            background.Add(content.Load<Texture2D>(@"Maps\History\AxisOfHistoricalReality\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\AxisOfHistoricalReality\foreground");
            game.NPCSprites["Mr. Robatto"] = content.Load<Texture2D>(@"NPC\Main\robatto");
            Game1.npcFaces["Mr. Robatto"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Robatto");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            game.NPCSprites["Mr. Robatto"] = Game1.whiteFilter;
            Game1.npcFaces["Mr. Robatto"].faces["Normal"] = Game1.whiteFilter;
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

            portals.Add(toWasteland, StoneFortWasteland.ToAxisOfHistoricalReality);
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

            s.Draw(foreground, new Vector2(mapWidth - foreground.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}
