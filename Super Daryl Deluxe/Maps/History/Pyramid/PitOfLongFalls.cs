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
    class PitOfLongFalls : MapClass
    {
        static Portal toInnerChamber;

        public static Portal ToInnerChamber { get { return toInnerChamber; } }

        Texture2D foreground;

        public PitOfLongFalls(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 2300;
            mapName = "The Pit of Long Falls";

            mapRec = new Rectangle(0, -410, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            treasureChests.Add(new TreasureChest(Game1.treasureChestSheet, 2075, mapRec.Y + 340 + 180, player, 4.50f, new Textbook(), this));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PitOfLongFalls\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PitOfLongFalls\foreground");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (player.VitalRecY > 800)
            {
                ForceToNewMap(new Portal(0, 0, "The Pit of Long Falls"), PyramidChute.fromPitOfLongFalls);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toInnerChamber = new Portal(100, platforms[0], "The Pit of Long Falls");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toInnerChamber, InnerChamber.ToPitOfLongFalls);
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
