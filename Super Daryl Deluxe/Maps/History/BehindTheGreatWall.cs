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
    class BehindTheGreatWall : MapClass
    {
        public static Portal toForestPath;
        public static Portal toTheGreatWall;

        Texture2D foreground, trojanHorse, soldiers, blockedGate;
        Dictionary<String, Texture2D> portal;

        int portalFrame;
        int portalFrameDelay = 5;

        public BehindTheGreatWall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Behind the Great Wall";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;
            zoomLevel = 1;
            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new LivingLocker(game, new Rectangle(1330, 100, 1000, 500)));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\InsideGreatWall\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\InsideGreatWall\foreground");
            soldiers = content.Load<Texture2D>(@"Maps\History\InsideGreatWall\soldiers");
            portal = ContentLoader.LoadContent(content, @"Maps\History\InsideGreatWall\Portal");
            blockedGate = content.Load<Texture2D>(@"Maps\History\InsideGreatWall\blockedGate");

            game.NPCSprites["Julius Caesar"] = content.Load<Texture2D>(@"NPC\Party\Julius");
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusHelmet");
            trojanHorse = content.Load<Texture2D>(@"Maps\History\TrojanHorseNormal\horse00");

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Julius Caesar"] = Game1.whiteFilter;
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] && toTheGreatWall.IsUseable)
                toTheGreatWall.IsUseable = false;
            else if (!toTheGreatWall.IsUseable && game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
                toTheGreatWall.IsUseable = true;

            portalFrameDelay--;

            if (portalFrameDelay < 0)
            {
                portalFrame++;
                portalFrameDelay = 3;

                if (portalFrame > portal.Count - 1)
                    portalFrame = 0;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toForestPath = new Portal(40, platforms[0], "Behind the Great Wall");
            toTheGreatWall = new Portal(3600, platforms[0], "Behind the Great Wall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toForestPath, ForestPath.ToBehindTheGreatWall);
            portals.Add(toTheGreatWall, TheGreatWall.toBehindGreatWall);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(soldiers, new Vector2(0, 0), Color.White);

            s.Draw(portal["portal" + portalFrame], new Vector2(-12, 0), Color.White);

            if (game.ChapterTwo.ChapterTwoBooleans["invadeChinaPartThreePlayed"] && game.CurrentChapter.state != Chapter.GameState.Cutscene && !game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"])
                s.Draw(trojanHorse, new Rectangle(700, -78, trojanHorse.Width, trojanHorse.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            if (game.ChapterTwo.ChapterTwoBooleans["invadeChinaPartThreePlayed"] && !game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
                s.Draw(blockedGate, new Vector2(3373, 348), Color.White);

        }
        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);
        }
    }
}
