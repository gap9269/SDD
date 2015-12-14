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
    class SecretPassage : MapClass
    {
        public static Portal toCenterOfPyramid;
        public static Portal toCentralHallIII;

        Texture2D foreground, foreground2;
        public SecretPassage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5592;
            mapHeight = 1460;
            mapName = "Secret Passage";
            mapRec = new Rectangle(0, -410, mapWidth, 1100);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SecretPassage\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SecretPassage\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SecretPassage\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\Pyramid\SecretPassage\foreground2");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
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

            toCenterOfPyramid = new Portal(5200, platforms[1], "Secret Passage");
            toCentralHallIII = new Portal(90, platforms[0], "Secret Passage");

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCenterOfPyramid, CenterOfThePyramid.toSecretPassage);
            portals.Add(toCentralHallIII, CentralHallIII.toSecretPassage);
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

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
