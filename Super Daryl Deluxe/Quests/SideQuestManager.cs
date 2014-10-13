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
    public class SideQuestManager
    {

        Game1 game;

        //Prologue +
        NPC gardener;
        RatQuest ratQuest;

        //Chapter One +
        NPC scienceQuestNPC;
        NPC furnaceNPC;

        BrokenGlassCollecting sideQuestBrokenGlass;
        KillingFlasks killFlasks;
        FeedingTheWorkers feedingTheWorkers;
        FindingCoal findingCoal;

        public Dictionary<String, NPC> nPCs;

        public SideQuestManager(Game1 g)
        {
            game = g;

            nPCs = new Dictionary<string, NPC>();

            //Prologue
            ratQuest = new RatQuest(false);
            game.AllQuests.Add(ratQuest.QuestName, ratQuest);

            
            //Chapter One
            sideQuestBrokenGlass = new BrokenGlassCollecting(false);
            killFlasks = new KillingFlasks(false);
            feedingTheWorkers = new FeedingTheWorkers(false);
            findingCoal = new FindingCoal(false);

            game.AllQuests.Add(sideQuestBrokenGlass.QuestName, sideQuestBrokenGlass);
            game.AllQuests.Add(killFlasks.QuestName, killFlasks);
            game.AllQuests.Add(feedingTheWorkers.QuestName, feedingTheWorkers);
            game.AllQuests.Add(findingCoal.QuestName, findingCoal);
        }


        public void Update()
        {
            if (game.chapterState >= Game1.ChapterState.chapterOne)
            {
                if (sideQuestBrokenGlass.CompletedQuest == true && killFlasks.CompletedQuest == false && scienceQuestNPC.Quest == null)
                {
                    scienceQuestNPC.AddQuest(killFlasks);
                }
                //--Add the second furnace NPC quest
                if (feedingTheWorkers.CompletedQuest == true && findingCoal.CompletedQuest == false && furnaceNPC.Quest == null)
                {
                    furnaceNPC.AddQuest(findingCoal);
                }

                if (ratQuest.CompletedQuest && nPCs.ContainsKey("The Gardener") && game.CurrentChapter.CurrentMap.MapName != gardener.MapName && gardener.Quest == null)
                {

                    nPCs.Remove("The Gardener");
                    game.Prologue.PrologueBooleans["finishedRatQuest"] = true;
                }
            }
        }

        //--List NPCs in the order they are added to the game (Prologue to End)
        public void AddNPCs()
        {

            if (!nPCs.ContainsKey("The Gardener") && game.Prologue.PrologueBooleans["addedGardener"] == true && game.Prologue.PrologueBooleans["finishedRatQuest"] == false)
            {
                List<String> gardenDialogue = new List<string>();
                gardenDialogue.Add("I...*sniff*...I don't know how I'll live without my darling Riley.");
                gardener = new NPC(game.NPCSprites["The Gardener"], gardenDialogue, ratQuest, new Rectangle(700, 680 - 395, 516, 388),
                    Game1.Player, game.Font, game, "East Hall", "The Gardener", false);
                nPCs.Add("The Gardener", gardener);
            }

            
            if (game.chapterState >= Game1.ChapterState.chapterOne)
            {
                if (!nPCs.ContainsKey("ScienceQuestNPC"))
                {
                    //--Science Room NPC
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("I hate detention. And science.");
                    scienceQuestNPC = new NPC(game.NPCSprites["Alan"], dialogueSideQuestNPC, sideQuestBrokenGlass,
                        new Rectangle(5770, 680 - 388, 516, 388), Game1.Player, game.Font, game, "North Hall", "Alan", false);
                    nPCs.Add("ScienceQuestNPC", scienceQuestNPC);
                }

                if (!nPCs.ContainsKey("FurnaceNPC"))
                {
                    //--Science Room NPC
                    List<String> furnaceDialogue = new List<string>();
                    furnaceDialogue.Add("With all this food and coal, we'll be set for weeks! Thanks again, sonny!");
                    furnaceNPC = new NPC(game.NPCSprites["Alan"], furnaceDialogue, feedingTheWorkers,
                        new Rectangle(0, 0, 0, 0), Game1.Player, game.Font, game, "Furnace Room", "Alan", true);
                    furnaceNPC.RecY = 630 - furnaceNPC.Rec.Height;
                    furnaceNPC.RecX = 1000;
                    nPCs.Add("FurnaceNPC", furnaceNPC);
                }
            }
        }
    }
}
