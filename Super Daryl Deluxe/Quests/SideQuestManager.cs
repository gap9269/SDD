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
    public class SideQuestManager
    {

        Game1 game;

        //Prologue +
        NPC gardener;
        public RatQuest ratQuest;
        NPC farSideDeath;

        //Chapter One +
        NPC drew;
        NPC furnaceNPC;
        CountRoger roger;
        NPC colin;
        NPC mozart;
        TrenchcoatKid trenchcoatUpstairs;
        NPC townClaysmith;
        NPC daVinci;
        NPC warhol;
        NPC employeeTaskList;
        NPC keeperOfQuests;

        AGiftForTrenchcoatKid aGiftForTrenchcoat;
        BrokenGlassCollecting sideQuestBrokenGlass;
        CoalQuest feedingTheWorkers;
        SousChefFirstCourse sousChefOne;
        SavingCountRoger savingCountRoger;
        StolenPaintings stolenPaintings;
        public static AStarIsBorn aStarIsBorn;
        TheClaysmithsHome theClaysmithsHome;
        TheClaysmithsWork theClaysmithsWork;
        ANewArtisticMedium newArtMedium;
        PaulAndTheBeanstalk paulAndTheBeanstalk;

        //Chapter Two +
        NPC thePyramidGardener, henry, kingHasbended, mikrov, santa, poole, egyptianGuard, riftRepairmanDesert, frenchSoldier, riftRepairmanForest;
        DrDominique jeanLarrey;

        SupplyRun supplyRun;
        public SoldierRepairs soldierRepairs;
        public DesertMemorial desertMemorial;
        FieryFlora fieryFlora;
        MikrovsToll mikrovsToll;
        SavingChristmasI savingChristmasI;
        SavingChristmasII savingChristmasII;
        public PoolesBoy poolesBoy;
        AGuardsRevenge pharaohsRevenge;
        public DesertDimensions desertDimensions;
        public AnotherMiddleEarth anotherMiddleEarth;
        SousChefSecondCourse sousChefTwo;
        DominiqueTransform dominiqueTransformScene;
        BlowingUpLove blowingUpLove;
        CurseOfTheMummy curseOfTheMummy; 
        public Dictionary<String, NPC> nPCs;

        public enum SideQuestScenes
        {
            none, dominiqueTransform
        }
        public SideQuestScenes sideQuestScenes;


        public SideQuestManager(Game1 g)
        {
            game = g;

            nPCs = new Dictionary<string, NPC>();

            //Prologue
            ratQuest = new RatQuest(false);
            game.AllQuests.Add(ratQuest.QuestName, ratQuest);

            
            //Chapter One
            sideQuestBrokenGlass = new BrokenGlassCollecting(false);
            feedingTheWorkers = new CoalQuest(false);
            sousChefOne = new SousChefFirstCourse(false);
            savingCountRoger = new SavingCountRoger(false);
            aStarIsBorn = new AStarIsBorn(false);
            aGiftForTrenchcoat = new AGiftForTrenchcoatKid(false);
            stolenPaintings = new StolenPaintings(false);
            theClaysmithsHome = new TheClaysmithsHome(false);
            theClaysmithsWork = new TheClaysmithsWork(false);
            newArtMedium = new ANewArtisticMedium(false);
            paulAndTheBeanstalk = new PaulAndTheBeanstalk(false);

            game.AllQuests.Add(sideQuestBrokenGlass.QuestName, sideQuestBrokenGlass);
            game.AllQuests.Add(feedingTheWorkers.QuestName, feedingTheWorkers);
            game.AllQuests.Add(sousChefOne.QuestName, sousChefOne);
            game.AllQuests.Add(savingCountRoger.QuestName, savingCountRoger);
            game.AllQuests.Add(aStarIsBorn.QuestName, aStarIsBorn);
            game.AllQuests.Add(aGiftForTrenchcoat.QuestName, aGiftForTrenchcoat);
            game.AllQuests.Add(stolenPaintings.QuestName, stolenPaintings);
            game.AllQuests.Add(theClaysmithsHome.QuestName, theClaysmithsHome);
            game.AllQuests.Add(theClaysmithsWork.QuestName, theClaysmithsWork);
            game.AllQuests.Add(newArtMedium.QuestName, newArtMedium);
            game.AllQuests.Add(paulAndTheBeanstalk.QuestName, paulAndTheBeanstalk);

            //Chapter Two
            supplyRun = new SupplyRun(false);
            soldierRepairs = new SoldierRepairs(false);
            desertMemorial = new DesertMemorial(false);
            fieryFlora = new FieryFlora(false);
            mikrovsToll = new MikrovsToll(false);
            savingChristmasI = new SavingChristmasI(false);
            savingChristmasII = new SavingChristmasII(false);
            poolesBoy = new PoolesBoy(false);
            pharaohsRevenge = new AGuardsRevenge(false);
            desertDimensions = new DesertDimensions(false);
            sousChefTwo = new SousChefSecondCourse(false);
            blowingUpLove = new BlowingUpLove(false);
            anotherMiddleEarth = new AnotherMiddleEarth(false);
            curseOfTheMummy = new CurseOfTheMummy(false);

            game.AllQuests.Add(supplyRun.QuestName, supplyRun);
            game.AllQuests.Add(soldierRepairs.QuestName, soldierRepairs);
            game.AllQuests.Add(desertMemorial.QuestName, desertMemorial);
            game.AllQuests.Add(fieryFlora.QuestName, fieryFlora);
            game.AllQuests.Add(mikrovsToll.QuestName, mikrovsToll);
            game.AllQuests.Add(savingChristmasI.QuestName, savingChristmasI);
            game.AllQuests.Add(savingChristmasII.QuestName, savingChristmasII);
            game.AllQuests.Add(poolesBoy.QuestName, poolesBoy);
            game.AllQuests.Add(pharaohsRevenge.QuestName, pharaohsRevenge);
            game.AllQuests.Add(desertDimensions.QuestName, desertDimensions);
            game.AllQuests.Add(sousChefTwo.QuestName, sousChefTwo);
            game.AllQuests.Add(blowingUpLove.QuestName, blowingUpLove);
            game.AllQuests.Add(anotherMiddleEarth.QuestName, anotherMiddleEarth);
            game.AllQuests.Add(curseOfTheMummy.QuestName, curseOfTheMummy);

            dominiqueTransformScene = new DominiqueTransform(game, game.Camera, Game1.Player);
        }

        public void Update()
        {
            #region Prologue and Up

            if (nPCs.ContainsKey("The Gardener") && nPCs["The Gardener"].MapName == "East Hall")
            {
                Game1.schoolMaps.maps["East Hall"].currentBackgroundMusic = Sound.MusicNames.FurryFuneral;

            }
            else
                Game1.schoolMaps.maps["East Hall"].currentBackgroundMusic = Sound.MusicNames.NoirHalls;

            if (nPCs.ContainsKey("Far Side Death") && farSideDeath.Talking && game.Prologue.PrologueBooleans["buriedRiley"] == false)
            {
                game.Prologue.PrologueBooleans["buriedRiley"] = true;
            }

            if (nPCs.ContainsKey("Far Side Death"))
            {
                if (game.CurrentSideQuests.Contains(ratQuest) || game.Prologue.PrologueBooleans["buriedRiley"])
                    farSideDeath.canTalk = true;
                else
                    farSideDeath.canTalk = false;
            }
            if (!game.CurrentChapter.TalkingToNPC && game.Prologue.PrologueBooleans["buriedRiley"] == true && ratQuest.CompletedQuest == false)
            {
                ratQuest.CompletedQuest = true;
                farSideDeath.ClearDialogue();
                farSideDeath.Dialogue.Add("Careful on the way down. I'm not much for saving lives.");
                Chapter.effectsManager.AddSmokePoof(new Rectangle(357, -2920 + 524, 95, 95), 2);
            }

            if (ratQuest.CompletedQuest && Game1.Player.AllCharacterBios["Gardener"] == false && !game.CurrentSideQuests.Contains(ratQuest))
                Game1.Player.UnlockCharacterBio("Gardener");
            #endregion


            //Chapter One and Up - Add some NPCs and remove previous ones if their quests are done
            if (game.chapterState >= Game1.ChapterState.chapterOne)
            {
                //if (sideQuestBrokenGlass.CompletedQuest == true && killFlasks.CompletedQuest == false && drew.Quest == null)
                //{
                //    drew.AddQuest(killFlasks);
                //}
                //--Add the second furnace NPC quest

                #region Task List Stuff
                employeeTaskList.FacingRight = true;
                if (employeeTaskList.Quest == null || employeeTaskList.Quest.CompletedQuest || employeeTaskList.DialogueState == 0)
                    employeeTaskList.CurrentDialogueFace = "Normal";
                else if (employeeTaskList.Quest == aGiftForTrenchcoat)
                    employeeTaskList.CurrentDialogueFace = "Alan";
                else if (employeeTaskList.Quest == paulAndTheBeanstalk)
                    employeeTaskList.CurrentDialogueFace = "Paul";

                if (employeeTaskList.Quest == null && employeeTaskList.Dialogue[0] != "There are no tasks currently posted.")
                {
                    employeeTaskList.ClearDialogue(); 
                    employeeTaskList.Dialogue.Add("There are no tasks currently posted.");
                }

                if (game.ChapterOne.ChapterOneBooleans["questOneSceneStarted"] && employeeTaskList.Quest == null && game.AllQuests[aGiftForTrenchcoat.QuestName].CompletedQuest == false)
                {
                    employeeTaskList.AddQuest(aGiftForTrenchcoat);
                }
                if (game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"] && employeeTaskList.Quest == null && game.AllQuests["Paul and the Beanstalk"].CompletedQuest == false)
                {
                    employeeTaskList.AddQuest(paulAndTheBeanstalk);
                }

                #endregion

                //--Start the da vinci scene
                if (game.ChapterOne.ChapterOneBooleans["daVinciSceneStarted"] == false && (daVinci.Talking) && game.CurrentQuests.Contains(ChapterOne.fundRaising))
                {
                    game.ChapterOne.ChapterOneBooleans["daVinciSceneStarted"] = true;
                    daVinci.Talking = false;
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.ChapterOne.ChapterOneBooleans["spawnedArtMerchant"] = true;
                }

                if ((game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"] || game.ChapterOne.ChapterOneBooleans["soldPaintingToArtDealer"]) && daVinci.Quest == null && game.AllQuests["Stolen Paintings"].CompletedQuest == false)
                {
                    daVinci.AddQuest(stolenPaintings);
                }

                if (Game1.Player.Level >= 8 && trenchcoatUpstairs.ItemsOnSale[0].name == "Super Weenie Sword")
                {
                    trenchcoatUpstairs.ItemsOnSale.Clear();
                    trenchcoatUpstairs.ItemsOnSale.Add(new WeaponForSale(new AverageWeenieProtractor(), 20.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new OutfitForSale(new AverageWeenieShirt(), 20.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new HatForSale(new AverageWeenieHat(), 20.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new AccessoryForSale(new AverageWeenieCalculator(), 20.00f));
                    trenchcoatUpstairs.ClearDialogue();

                    trenchcoatUpstairs.Dialogue.Add("Great news. Looks like you got yourself a whole bunch of those \"experience points\" that I hear all of you dorks talking about. Good for you. I bet you found a whole bunch of lunch money too, right?");
                    trenchcoatUpstairs.Dialogue.Add("To honor this momentous occasion I've got some new stock in for you that really encapsulates your personality. I hand selected it myself. Yep, should be just what you're looking for. Take a look.");

                    trenchcoatUpstairs.RestockItemsForSale();
                }


                if (Game1.Player.Level >= 12 && trenchcoatUpstairs.ItemsOnSale[0].name == "Average Weenie Protractor")
                {
                    trenchcoatUpstairs.ItemsOnSale.Clear();
                    trenchcoatUpstairs.ItemsOnSale.Add(new WeaponForSale(new CardboardSword(), 60.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new WeaponForSale(new CardboardSword(), 60.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new OutfitForSale(new CardboardShirt(), 60.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new HatForSale(new CardboardHat(), 60.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new AccessoryForSale(new Cardboard(), 60.00f));
                    trenchcoatUpstairs.ItemsOnSale.Add(new AccessoryForSale(new Cardboard(), 60.00f));
                    trenchcoatUpstairs.ClearDialogue();

                    trenchcoatUpstairs.Dialogue.Add("Well well well, looks like you're really gettin' those \"levels\", aren't ya? A guy just stopped by here earlier and told me, \"Hey that kid in the orange pants sure has a lot of levels now.\".");
                    trenchcoatUpstairs.Dialogue.Add("I've decided to bring out something a bit better for you. Had these lying around for a while, waiting for the right person. Take a look.");

                    trenchcoatUpstairs.RestockItemsForSale();
                }

                if (game.AllQuests["Flowers for Riley"].CompletedQuest && nPCs.ContainsKey("The Gardener") && game.CurrentChapter.CurrentMap.MapName != gardener.MapName && gardener.Quest == null)
                {

                    nPCs.Remove("The Gardener");
                    nPCs.Remove("Far Side Death");
                    game.Prologue.PrologueBooleans["finishedRatQuest"] = true;
                }

                if (game.ChapterOne.ChapterOneBooleans["ghostLockGoneScenePlayed"] && mozart.Quest == null && game.AllQuests["A Star is Born"].CompletedQuest == false)
                {
                    mozart.AddQuest(aStarIsBorn);
                }

                if (game.ChapterOne.ChapterOneBooleans["destroyedVases"] && townClaysmith.Quest == null && game.AllQuests["The Claysmith's Work"].CompletedQuest == false)
                {
                    townClaysmith.AddQuest(theClaysmithsWork);
                }
            }

            if (game.chapterState >= Game1.ChapterState.chapterTwo)
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["dominiqueTransformScenePlayed"] && jeanLarrey.Quest == soldierRepairs && game.AllQuests[soldierRepairs.QuestName].CompletedQuest == true && jeanLarrey.Talking)
                {
                    game.CurrentChapter.TalkingToNPC = false;
                    jeanLarrey.Talking = false;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
                    game.ChapterTwo.ChapterTwoBooleans["dominiqueTransformScenePlayed"] = true;
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    sideQuestScenes = SideQuestScenes.dominiqueTransform;
                }

                if (game.ChapterTwo.ChapterTwoBooleans["dominiqueTransformScenePlayed"] && jeanLarrey.goblinified == false)
                    jeanLarrey.goblinified = true;
                if (game.AllQuests["Sous Chef: First Course"].CompletedQuest && colin.Quest == null && game.AllQuests["Sous Chef: Second Course"].CompletedQuest == false)
                {
                    colin.AddQuest(sousChefTwo);
                }

                if (jeanLarrey.Quest == null && game.AllQuests[soldierRepairs.QuestName].CompletedQuest == false)
                {
                    jeanLarrey.AddQuest(soldierRepairs);
                }

                if (santa.Quest == null && game.AllQuests[savingChristmasII.QuestName].CompletedQuest == false)
                {
                    santa.AddQuest(savingChristmasII);
                }
            }
        }

        public void PlaySideQuestScene()
        {
            switch (sideQuestScenes)
            {
                case SideQuestScenes.dominiqueTransform:
                    if (!dominiqueTransformScene.skippingCutscene)
                        dominiqueTransformScene.Play();
                    break;
            }
        }

        public void DrawSideQuestScene(SpriteBatch s)
        {
            switch (sideQuestScenes)
            {
                case SideQuestScenes.dominiqueTransform:
                    dominiqueTransformScene.Draw(s);
                    if (dominiqueTransformScene.skippingCutscene)
                        dominiqueTransformScene.DrawSkipCutscene(s);
                    break;
            }
        }
        public void AddNPC(String name, NPC npc)
        {
            nPCs.Add(name, npc);

            if (game.saveData.sideQuestNPCs != null)
            {
                for (int i = 0; i < game.saveData.sideQuestNPCs.Count; i++)
                {
                    if (name == game.saveData.sideQuestNPCs[i].npcName)
                    {
                        if (game.saveData.sideQuestNPCs[i].questName != null)
                        {
                            npc.Dialogue = game.saveData.sideQuestNPCs[i].dialogue;
                            npc.QuestDialogue = game.saveData.sideQuestNPCs[i].questDialogue;
                            npc.DialogueState = game.saveData.sideQuestNPCs[i].dialogueState;
                            npc.FacingRight = game.saveData.sideQuestNPCs[i].facingRight;
                            npc.Quest = game.AllQuests[game.saveData.sideQuestNPCs[i].questName];
                            npc.AcceptedQuest = game.saveData.sideQuestNPCs[i].acceptedQuest;
                            npc.MapName = game.saveData.sideQuestNPCs[i].mapName;
                        }

                        else if (game.saveData.sideQuestNPCs[i].trenchCoat == false)
                        {
                            npc.Dialogue = game.saveData.sideQuestNPCs[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.sideQuestNPCs[i].dialogueState;
                            npc.FacingRight = game.saveData.sideQuestNPCs[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            npc.MapName = game.saveData.sideQuestNPCs[i].mapName;
                        }
                        else
                        {
                            npc.Dialogue = game.saveData.sideQuestNPCs[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.sideQuestNPCs[i].dialogueState;
                            npc.FacingRight = game.saveData.sideQuestNPCs[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            (npc as TrenchcoatKid).SoldOut = game.saveData.sideQuestNPCs[i].trenchcoatSoldOut;
                            npc.MapName = game.saveData.sideQuestNPCs[i].mapName;
                        }
                    }
                }
            }
        }

        //--List NPCs in the order they are added to the game (Prologue to End)
        public void AddNPCs()
        {

            if (!nPCs.ContainsKey("The Gardener") && game.Prologue.PrologueBooleans["addedGardener"] == true && game.Prologue.PrologueBooleans["finishedRatQuest"] == false)
            {
                List<String> gardenDialogue = new List<string>();
                gardenDialogue.Add("I should find a new pet...");
                gardener = new NPC(game.NPCSprites["The Gardener"], gardenDialogue, ratQuest, new Rectangle(700, 680 - 395, 516, 388),
                    Game1.Player, game.Font, game, "East Hall", "The Gardener", false);
                nPCs.Add("The Gardener", gardener);

                List<String> dialogue = new List<string>();
                dialogue.Add("Quite the view up here, eh?");
                dialogue.Add("Waddaya got here, a smashed rat? Looks like little Riley...well that's a shame. Riley had a gene that could've reversed aging in humans. Too bad she lived with that batty gardener...");
                dialogue.Add("Coulda put me out of a job! BAHAHA! Ah...yeah. Oh well. I'll bury her next to the other poor souls.");
                farSideDeath = new NPC(game.NPCSprites["Death"], dialogue,
                    new Rectangle(50, -2920 + 303, 516, 388), Game1.Player, game.Font, game, "The Far Side", "Death", false);
                farSideDeath.FacingRight = true;
                AddNPC("Far Side Death", farSideDeath);
            }
            
            if (game.chapterState >= Game1.ChapterState.chapterOne)
            {

                if (!nPCs.ContainsKey("TrenchcoatCronyUpstairs"))
                {
                    //Trenchcoat crony
                    List<String> cronydialogue = new List<string>();
                    cronydialogue.Add("You're that new kid, right? Plays D&D with those other nerds? Let me introduce myself. I'm the guy that's gonna be selling you special equipment made specifically for your \"level\". Or whatever.");
                    cronydialogue.Add("You just come around whenever you gained enough of those \"experience points\" and feel like you need some more shit to wear. I'll always have new stuff that I feel suits you best.");
                    cronydialogue.Add("You're pretty new though, so I think I have the perfect stuff for ya. Take a look.");
                    List<ItemForSale> items = new List<ItemForSale>();
                    items.Add(new WeaponForSale(new SuperWeenieSword(), 2.00f));
                    items.Add(new OutfitForSale(new SuperWeenieShirt(), 2.00f));
                    items.Add(new HatForSale(new SuperWeenieHat(), 2.00f));
                    items.Add(new AccessoryForSale(new SuperWeeniePurse(), 2.00f));
                    trenchcoatUpstairs = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 1500, 630, Game1.Player, game, "Upstairs", items);
                    AddNPC("TrenchcoatCronyUpstairs", trenchcoatUpstairs);
                }

                if (!nPCs.ContainsKey("Employee Task List"))
                {
                    List<String> d = new List<string>();
                    d.Add("There are no tasks currently posted.");
                    employeeTaskList = new NPC(game.NPCSprites["Employee Task List"], d,
                        new Rectangle(3360, 263, 516, 388), Game1.Player, game.Font, game, "North Hall", "Employee Task List", false);
                    AddNPC("Employee Task List", employeeTaskList);
                    employeeTaskList.FacingRight = true;
                }

                if (!nPCs.ContainsKey("Furnace Man"))
                {
                    //--Science Room NPC
                    List<String> furnaceDialogue = new List<string>();
                    furnaceDialogue.Add("Nnnnh. The warmth is back.");
                    furnaceNPC = new NPC(game.NPCSprites["Furnace Man"], furnaceDialogue, feedingTheWorkers,
                        new Rectangle(550, 320, 516, 388), Game1.Player, game.Font, game, "Furnace Room", "Furnace Man", false);
                    AddNPC("Furnace Man", furnaceNPC);
                }

                //--Colin
                if (!nPCs.ContainsKey("Chef Flex"))
                {
                    List<String> dialogue1 = new List<string>();
                    dialogue1.Add("You come on back here tomorrah an' we'll do business again.");
                    colin = new NPC(Game1.whiteFilter, dialogue1, sousChefOne, new Rectangle(820, 290, 516, 388),
                        Game1.Player, game.Font, game, "Kitchen", "Chef Flex", false);

                    AddNPC("Chef Flex", colin);
                }

                if (!nPCs.ContainsKey("Andy Warhol"))
                {
                    List<String> dialogue1 = new List<string>();
                    dialogue1.Add("This is the medium of the television age!");
                    warhol = new NPC(Game1.whiteFilter, dialogue1, newArtMedium, new Rectangle(4190, -100, 516, 388),
                        Game1.Player, game.Font, game, "Second Floor", "Andy Warhol", false);

                    AddNPC("Andy Warhol", warhol);
                }

                if (!nPCs.ContainsKey("Count Roger"))
                {
                    //--Science Room NPC
                    List<String> rogerDia = new List<string>();
                    rogerDia.Add("Leave me to my solitude, lowly slave. Count Roger calls upon his underlings when they are needed!");
                    roger = new CountRoger(game.NPCSprites["Count Roger"], rogerDia, savingCountRoger,
                        new Rectangle(3032, 330 - 388, 516, 388), Game1.Player, game.Font, game, "Upper Vents VI", "Count Roger", false);
                    AddNPC("Count Roger", roger);
                }

                if (!nPCs.ContainsKey("Mozart"))
                {
                    //--Science Room NPC
                    List<String> mozartDia = new List<string>();
                    mozartDia.Add("Hi there, mister! My name's Wolfgang. My parents said I can't talk to weird strangers, and I'm busy composing my greatest ever masterpiece, anyway.");
                    mozartDia.Add("Bye, bye!");
                    mozart = new NPC(game.NPCSprites["Mozart"], mozartDia,
                        new Rectangle(500, 660 - 388, 516, 388), Game1.Player, game.Font, game, "Mozart's Room", "Mozart", false);
                    AddNPC("Mozart", mozart);
                }
                if (!nPCs.ContainsKey("Leonardo Da Vinci"))
                {
                    List<String> d = new List<string>();
                    d.Add("This region is exquisite!");
                    daVinci = new NPC(game.NPCSprites["Leonardo Da Vinci"], d,
                        new Rectangle(1894, -965, 516, 388), Game1.Player, game.Font, game, "The Grand Canal", "Leonardo Da Vinci", false);
                    daVinci.FacingRight = false;
                    AddNPC("Leonardo Da Vinci", daVinci);
                }

                if (!nPCs.ContainsKey("Town Claysmith"))
                {
                    List<String> d = new List<string>();
                    d.Add("Ooohhh..I hope those awful thieves don't destroy my life's work.");
                    townClaysmith = new NPC(game.NPCSprites["Town Claysmith"], d, theClaysmithsHome,
                        new Rectangle(2900, 110 - 388, 516, 388), Game1.Player, game.Font, game, "Town Square", "Town Claysmith", false);
                    townClaysmith.FacingRight = false;
                    AddNPC("Town Claysmith", townClaysmith);
                }

                if (!nPCs.ContainsKey("Keeper of the Quests"))
                {
                    //--Journal Instructor
                    List<String> dialogueJournal = new List<string>();
                    dialogueJournal.Add("Yes, *ahem*, welcome to the High Guild Hall. Let's see here...it appears I don't have any quests for the likes of you.");
                    dialogueJournal.Add("How mortifyingly tragic.");
                    keeperOfQuests = new NPC(game.NPCSprites["Keeper of the Quests"], dialogueJournal,
                        new Rectangle(571, 301, 516, 388), Game1.Player, game.Font, game, "Dwarves & Druids Club", "Keeper of the Quests", false);

                    AddNPC("Keeper of the Quests", keeperOfQuests);
                }
                if (!nPCs.ContainsKey("Drew") && game.ChapterOne.ChapterOneBooleans["addedMainLobbyNPCs"] == true)
                {
                    //--Science Room NPC
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("I sure do love glass!");
                    drew = new NPC(game.NPCSprites["Drew"], dialogueSideQuestNPC, sideQuestBrokenGlass,
                        new Rectangle(800, 270, 516, 388), Game1.Player, game.Font, game, "Upstairs", "Drew", false);
                    AddNPC("Drew", drew);
                }
            }

            if (game.chapterState >= Game1.ChapterState.chapterTwo)
            {
                if (!nPCs.ContainsKey("Dr. Dominique Jean Larrey"))
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("Zis iz most disturbing.");
                    jeanLarrey = new DrDominique(game.NPCSprites["Dr. Dominique Jean Larrey"], dialogueSideQuestNPC, supplyRun,
                        new Rectangle(100, 300, 516, 388), Game1.Player, game.Font, game, "Medical Tent", "Dr. Dominique Jean Larrey", false);
                    jeanLarrey.FacingRight = true;
                    AddNPC("Dr. Dominique Jean Larrey", jeanLarrey);

                    List<String> dialogue3 = new List<string>();
                    dialogue3.Add("Nothing quite like blowing up things that make you sad.");
                    frenchSoldier = new NPC(game.NPCSprites["French Soldier"], dialogue3, blowingUpLove, new Rectangle(2800, 271, 516, 388), Game1.Player, game.Font, game, "Napoleon's Camp", "French Soldier", false);
                    frenchSoldier.FacingRight = false;
                    AddNPC("French Soldier", frenchSoldier);
                }

                if (!nPCs.ContainsKey("Mikrov"))
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("Mikrov bridge last long years. High quality rock only.");
                    mikrov = new DrDominique(game.NPCSprites["Mikrov"], dialogueSideQuestNPC, mikrovsToll,
                        new Rectangle(-51, -3010 + 2976, 516, 388), Game1.Player, game.Font, game, "Mikrov's Hill", "Mikrov", false);
                    mikrov.FacingRight = true;
                    AddNPC("Mikrov", mikrov);
                }

                if (!nPCs.ContainsKey("The Pyramid Gardener"))
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("Ooohhh...I do wish my garden was more friendly...");
                    thePyramidGardener = new NPC(game.NPCSprites["The Pyramid Gardener"], dialogueSideQuestNPC, desertMemorial,
                        new Rectangle(600, 290, 516, 388), Game1.Player, game.Font, game, "Eastern Chamber", "The Pyramid Gardener", false);
                    thePyramidGardener.FacingRight = true;
                    AddNPC("The Pyramid Gardener", thePyramidGardener);
                }

                if (!nPCs.ContainsKey("Henry Horus"))
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("Careful around here. These things are everywhere.");
                    henry = new NPC(game.NPCSprites["Henry Horus"], dialogueSideQuestNPC, fieryFlora,
                        new Rectangle(430, 290, 516, 388), Game1.Player, game.Font, game, "Flower Sanctuary", "Henry Horus", false);
                    henry.FacingRight = true;
                    AddNPC("Henry Horus", henry);
                }

                if (!nPCs.ContainsKey("Santa Claus"))
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("The spirit of Christmas is dead. And now... I will have my revenge.");
                    santa = new NPC(game.NPCSprites["Santa Claus"], dialogueSideQuestNPC, savingChristmasI,
                        new Rectangle(687, 272, 516, 388), Game1.Player, game.Font, game, "Abandoned Safe Room", "Santa Claus", false);
                    santa.FacingRight = false;
                    AddNPC("Santa Claus", santa);
                }

                if (!nPCs.ContainsKey("Poole") && game.ChapterTwo.ChapterTwoBooleans["bedroomThreeCleared"])
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("I do hope Master Scrooge leaves the fireplaces alone this time.");
                    poole = new NPC(game.NPCSprites["Poole"], dialogueSideQuestNPC, poolesBoy,
                        new Rectangle(1505, 256, 516, 388), Game1.Player, game.Font, game, "Ebenezer's Mansion", "Poole", false);
                    AddNPC("Poole", poole);
                }

                if (!nPCs.ContainsKey("Chained Pharaoh Guard"))
                {
                    List<String> dialogue2 = new List<string>();
                    dialogue2.Add("It's actually kind of nice to sit down for once.");
                    egyptianGuard = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue2, pharaohsRevenge, new Rectangle(1670, 281, 516, 388), Game1.Player, game.Font, game, "Pyramid Entrance", "Chained Pharaoh Guard", false);
                    egyptianGuard.FacingRight = false;
                    AddNPC("Chained Pharaoh Guard", egyptianGuard);
                }

                if (!nPCs.ContainsKey("Portal Repair Specialist Desert"))
                {
                    List<String> dialogue2 = new List<string>();
                    dialogue2.Add("If I could reverse engineer these giant magical band-aids and reproduce them commercially, I could quit this awful job and be the most popular lab crony in existence!");
                    riftRepairmanDesert = new NPC(game.NPCSprites["Portal Repair Specialist"], dialogue2, desertDimensions, new Rectangle(3190, -2100, 516, 388), Game1.Player, game.Font, game, "Central Sands", "Portal Repair Specialist", false);
                    riftRepairmanDesert.FacingRight = true;
                    AddNPC("Portal Repair Specialist Desert", riftRepairmanDesert);

                    List<String> dialogue3 = new List<string>();
                    dialogue3.Add("This partnership is going somewhere, I think.");
                    riftRepairmanForest = new NPC(game.NPCSprites["Portal Repair Specialist"], dialogue3, anotherMiddleEarth, new Rectangle(1923, -720 + 47, 516, 388), Game1.Player, game.Font, game, "Forest of Ents", "Portal Repair Specialist", false);
                    riftRepairmanForest.FacingRight = false;
                    AddNPC("Portal Repair Specialist Forest", riftRepairmanForest);
                }

                if (!nPCs.ContainsKey("King Hasbended") && game.ChapterTwo.ChapterTwoBooleans["mummySummoned"])
                {
                    List<String> dialogueSideQuestNPC = new List<string>();
                    dialogueSideQuestNPC.Add("I will use these \"toy-lets\" to destroy the world!");
                    kingHasbended = new NPC(game.NPCSprites["King Hasbended"], dialogueSideQuestNPC, curseOfTheMummy,
                        new Rectangle(893, 253, 516, 388), Game1.Player, game.Font, game, "Burial Chamber", "King Hasbended", false);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(kingHasbended.Rec.Center.X - 125, kingHasbended.Rec.Center.Y - 100, 250, 250), 2);
                    AddNPC("King Hasbended", kingHasbended);
                }
            }
        }
    }
}
