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
    class AncientAltar : MapClass
    {
        public static Portal toCenterOfThePyramid;

        Texture2D foreground, pastorGoblin;

        public AncientAltar(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Ancient Altar";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\Altar\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\Altar\foreground");
            pastorGoblin = content.Load<Texture2D>(@"NPC\History\Pastor Goblin");

            game.NPCSprites["Cleopatra"] = content.Load<Texture2D>(@"NPC\History\Cleopatra");
            Game1.npcFaces["Cleopatra"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\CleopatraNormal");
            Game1.npcFaces["Cleopatra"].faces["Chained"] = content.Load<Texture2D>(@"NPCFaces\History\Cleopatra Chained");

            game.NPCSprites["Time Lord"] = content.Load<Texture2D>(@"NPC\History\Hologram");
            Game1.npcFaces["Time Lord"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\WarlordNormal");

            game.NPCSprites["Julius Caesar"] = content.Load<Texture2D>(@"NPC\Party\Julius");
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusHelmet");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Cleopatra"] = Game1.whiteFilter;
            Game1.npcFaces["Cleopatra"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Cleopatra"].faces["Chained"] = Game1.whiteFilter;

            game.NPCSprites["Time Lord"] = Game1.whiteFilter;
            Game1.npcFaces["Time Lord"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Julius Caesar"] = Game1.whiteFilter;
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCenterOfThePyramid = new Portal(50, platforms[0], "Ancient Altar");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["finishedCleopatraArc"])
                s.Draw(pastorGoblin, new Vector2(1948, 211), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCenterOfThePyramid, CenterOfThePyramid.toAncientAltar);
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

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
