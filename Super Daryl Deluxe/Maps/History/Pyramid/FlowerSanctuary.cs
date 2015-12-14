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
    class FlowerSanctuary : MapClass
    {
        static Portal toFalseRoom;
        static Portal toInnerChamber;

        public static Portal ToInnerChamber { get { return toInnerChamber; } }
        public static Portal ToFalseRoom { get { return toFalseRoom; } }

        Texture2D foreground, hole;

        ExplodingFlower flower;

        public FlowerSanctuary(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Flower Sanctuary";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            flower = new ExplodingFlower(game, 1275, 500, false, 300);
            interactiveObjects.Add(flower);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\FlowerSanctuary\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\FlowerSanctuary\foreground");
            hole = content.Load<Texture2D>(@"Maps\History\Pyramid\FlowerSanctuary\backgroundHole");

            game.NPCSprites["Henry Horus"] = content.Load<Texture2D>(@"NPC\History\Henry Horus");
            Game1.npcFaces["Henry Horus"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Henry Horus Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Henry Horus"] = Game1.whiteFilter;
            Game1.npcFaces["Henry Horus"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (toInnerChamber.IsUseable && game.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"] == false)
            {
                toInnerChamber.IsUseable = false;
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"] && toInnerChamber.IsUseable == false)
                toInnerChamber.IsUseable = true;

            if (game.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"] == false)
            {
                if (flower.flowerState == ExplodingFlower.FlowerState.dead)
                {
                    game.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"] = true;
                    game.Camera.ShakeCamera(10, 25);
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toFalseRoom = new Portal(120, platforms[0], "Flower Sanctuary");
            toInnerChamber = new Portal(1330, platforms[0], "Flower Sanctuary");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"] && background[0] != hole)
            {
                flower.RecX = 1050;
                background[0] = hole;
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toInnerChamber, InnerChamber.ToFlowerSanctuary);
            portals.Add(toFalseRoom, FalseRoom.ToFlowerSanctuary);
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
