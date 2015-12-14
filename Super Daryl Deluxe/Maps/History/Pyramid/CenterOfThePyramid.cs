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
    class CenterOfThePyramid : MapClass
    {
        public static Portal toChamberOfRedundancy;
        public static Portal toAncientAltar;
        public static Portal toSecretPassage;

        Texture2D foreground, door;
        float doorAlpha;
        public CenterOfThePyramid(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1460;
            mapWidth = 1920;
            mapName = "Center of the Pyramid";
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
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CenterOfThePyramid\background"));
            //foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CenterOfThePyramid\foreground");
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\CenterOfThePyramid\door");
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

            toChamberOfRedundancy.PortalRecX = 1650;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toChamberOfRedundancy = new Portal(1700, platforms[0], "Center of the Pyramid");
            toAncientAltar = new Portal(1000, platforms[0], "Center of the Pyramid");
            toSecretPassage = new Portal(700, platforms[0], "Center of the Pyramid");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toChamberOfRedundancy, PharaohsGate.ToCenterOfPyramid);
            portals.Add(toAncientAltar, AncientAltar.toCenterOfThePyramid);
            portals.Add(toSecretPassage, SecretPassage.toCenterOfPyramid);
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

            //s.Draw(foreground, new Vector2(0, 0), Color.White);

            if (player.VitalRec.X > 1450)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            if(!game.CurrentChapter.BossFight)
                s.Draw(door, new Vector2(1502, mapRec.Y + 617), Color.White * doorAlpha);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
