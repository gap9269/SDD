using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class WoodsyRiver : MapClass
    {
        static Portal toCrossroads;

        public static Portal ToCrossroads { get { return toCrossroads; } }

        Texture2D bridge;

        Platform barrier;

        KidCage kidCage;

        public WoodsyRiver(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Woodsy River";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            barrier = new Platform(Game1.whiteFilter, new Rectangle(1060, 0, 100, 720), false, false, false);
            platforms.Add(barrier);

            kidCage = new KidCage(Game1.interactiveObjects["KidCage"], 2700, 440, player);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\WoodsyRiver"));
            bridge = content.Load<Texture2D>(@"Maps\Chelseas\WoodsyRiverBridge");
            game.NPCSprites["Tim"] = content.Load<Texture2D>(@"NPC\Main\tim");
            game.NPCSprites["Alan"] = content.Load<Texture2D>(@"NPC\Main\alan");
        }

        public override void Update()
        {
            base.Update();

            //Remove the barrier once the quest to build the bridge is complete
            if (platforms.Contains(barrier) && game.ChapterTwo.buildBridgeTwo.CompletedQuest && !game.CurrentQuests.Contains(game.ChapterTwo.buildBridgeTwo))
                platforms.Remove(barrier);

            if (player.VitalRecX > 1700 && game.ChapterTwo.ChapterTwoBooleans["ApproachedTim"] == false)
            {
                game.ChapterTwo.ChapterTwoBooleans["ApproachedTim"] = true;
                Chapter.effectsManager.AddInGameDialogue("Oh for fuck's sake, not you.", "Tim", "Normal", 120);

            }

            kidCage.Update();

            //If the cage is gone but the boolean hasn't be activated, activate it
            if (!game.ChapterTwo.ChapterTwoBooleans["kidTwoSaved"] && kidCage.Finished)
            {
                game.ChapterTwo.ChapterTwoBooleans["kidTwoSaved"] = true;

                game.ChapterTwo.NPCs["Tim"].RecX = 2588;
                game.ChapterTwo.NPCs["Tim"].PositionX = 2588;

                game.ChapterTwo.NPCs["CrossroadsKid"].RecX = 346;
                game.ChapterTwo.NPCs["CrossroadsKid"].PositionX = 346;

                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Clear();
                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Add("When you were gone I thought I heard some shouting from back here.");
            }

            //If the game has loaded and the boolean is activated, but the cage isn't finished, set it to finished
            if (game.ChapterTwo.ChapterTwoBooleans["kidTwoSaved"] && !kidCage.Finished)
                kidCage.Finished = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCrossroads = new Portal(100, platforms[0], "WoodsyRiver");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.buildBridgeTwo.CompletedQuest && !game.CurrentQuests.Contains(game.ChapterTwo.buildBridgeTwo))
                s.Draw(bridge, new Rectangle(735, 160, 1102, 561) , Color.White);

            //Only draw the cage if it hasn't been opened
            if (!kidCage.Finished)
            {
                kidCage.Draw(s);
            }
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCrossroads, Crossroads.ToBridge);
        }
    }
}
