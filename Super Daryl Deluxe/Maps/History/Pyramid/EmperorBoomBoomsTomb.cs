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
    class EmperorBoomBoomsTomb : MapClass
    {
        static Portal toForgottenChamberII;

        public static Portal ToForgottenChamberII { get { return toForgottenChamberII; } }

        Texture2D foreground, hole;
        ExplodingFlower flower, flower2, flower3;
        public EmperorBoomBoomsTomb(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Emperor Boom-Boom's Tomb";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            flower = new ExplodingFlower(game, 600, 425, false, 300);
            interactiveObjects.Add(flower);

            flower2 = new ExplodingFlower(game, 850, 425, false, 300);
            interactiveObjects.Add(flower2);

            flower3 = new ExplodingFlower(game, 1100, 425, false, 300);
            interactiveObjects.Add(flower3);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\BoomBoom\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\BoomBoom\foreground");
            hole = content.Load<Texture2D>(@"Maps\History\Pyramid\BoomBoom\hole");
        }

        public override void Update()
        {
            base.Update();

            if (flower.flowerState == ExplodingFlower.FlowerState.dead)
            {
                if (Math.Abs(flower.deathTime - flower.maxDeathTime) < 2 && flower3.flowerState == ExplodingFlower.FlowerState.dead && Math.Abs(flower3.deathTime - flower3.maxDeathTime) < 2 && flower2.flowerState == ExplodingFlower.FlowerState.dead && game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"] == false)
                {
                    game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"] = true;
                    game.Camera.ShakeCamera(15, 35);
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toForgottenChamberII = new Portal(50, platforms[0], "Emperor Boom-Boom's Tomb");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["floorBlownOut"])
                s.Draw(hole, new Vector2(0, 720 - hole.Height), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toForgottenChamberII, ForgottenChamberII.ToEmperorBoomBoomsTomb);
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
