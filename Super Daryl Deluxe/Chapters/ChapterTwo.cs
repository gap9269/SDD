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
    public class ChapterTwo : Chapter
    {
        //NPCs
        NPC alan;
        NPC paul;

        //PARTY NPCS
        Callyn callyn;
        PeltKid peltKidOne;
        NPC squirrelBoy;
        Chelsea chelsea;
        Mark mark;
        Julius julius;
        Jesse jesse;
        BridgeKid bridgeKidOne;
        Balto balto;
        NPC skillInstructor;
        NPC saveInstructor;
        NPC alaina;
        NPC crossroadsKid;
        NPC tim;
        NPC inventoryInstructor;
        NPC blurso;

        //Trenchcoats
        TrenchcoatKid tutorialShop;
        TrenchcoatKid poolShop;
        TrenchcoatKid fieldShop;

        //TUTORIAL NPCS
        List<List<String>> associateDialogue; //This is gonna get confusing real quick
        int selectedAssociate; //1 2 or 3
        NPC friendOne;

        //TUTORIAL/DEMO
        public Texture2D associateOneTex;

        //--Story quests
        DemoQuestOne demoQuestOne;
        DemoQuestTwo demoQuestTwo;
        public FindingBalto findingBalto;
        public BuildingACornBridge buildBridgeOne;
        public BuildingBridgeTwo buildBridgeTwo;

        //Side quests
        BeerForToga beerForToga;
        BeerForGoggles beerForGoggles;
        BeerForHat beerForHat;
        SquirrelQuest squirrelQuest;
        ScissorsQuest scissorsQuest;

        //--Cutscenes
        //Demo scenes
        DemoOpening demoOpening;
        TreeFall treeFall;
        SteveAustinScene steveAustinScene;
        TutorialEnd tutorialEnd;
        DemoEnd demoEnd;

        //Party Scenes
        CrossroadsScene crossroadsScene;
        SavedFirstKidScene savedFirstKidScene;
        GateCollapseScene gateCollapseScene;
        DarylCaught darylCaughtScene;

        //Timers
        int firstTextTimer = 120;

        //Switch state to decision making, then pick the decision
        public enum Decisions
        {
            none,
            test,
            tutorialResolution
        }
        public Decisions decisions;

        //--Story Quest attributes
        Dictionary<String, Boolean> chapterTwoBooleans;

        static Random ran = new Random();

        public int SelectedAssociate { get { return selectedAssociate; } set { selectedAssociate = value; } }
        public List<List<string>> AssociateDialogue { get { return associateDialogue; } }
        public Dictionary<String, Boolean> ChapterTwoBooleans { get { return chapterTwoBooleans; } set { chapterTwoBooleans = value; } }

        public ChapterTwo(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            #region ASSOCIATE DIALOGUE
            //Tutorial Associate Dialogue
            List<String> associateOneDialogue = new List<string>();
            if (!Game1.gamePadConnected)
            {
                associateOneDialogue.Add("Hey there! Let's get started! \nPress the Left and Right Arrow keys to \nmake Daryl run!");
                associateOneDialogue.Add("Press the Up Arrow key to jump!");
                associateOneDialogue.Add("Press 'F' to use doors! I'm so excited! What \ncould be in the next map?!");
            }
            else
            {
                associateOneDialogue.Add("Hey there! Let's get started! \nUse the left analog stick to make Daryl run!");
                associateOneDialogue.Add("Press 'A' to jump!");
                associateOneDialogue.Add("Press the 'Right Bumper' to use doors! I'm so \nexcited! What could be in the next map?!");
            }
            associateOneDialogue.Add("Wow! What a beautiful valley! Look at the \nfabulous textures, player!");

            if (!Game1.gamePadConnected)
                associateOneDialogue.Add("The tree is blocking your path! Hold [SPACE] \nand press the Down arrow to drop through \ncertain platforms!");
            else
                associateOneDialogue.Add("The tree is blocking your path! Hold the Left \n Trigger and press down to drop through \ncertain platforms!");


            associateOneDialogue.Add("Good job player! You're a natural!");

            if (Game1.gamePadConnected)
            {
                associateOneDialogue.Add("Some jumps are too far for Daryl! \n\nTry holding the Left Trigger while running to \nsprint, then pressing 'A' to jump extra far!");
                associateOneDialogue.Add("It's okay, player! It takes some practice! \n\nRemember: Hold the Left Trigger and press 'A'.");
                associateOneDialogue.Add("Everyone makes mistakes! Try again!");
                associateOneDialogue.Add("...You'll get it!");
                associateOneDialogue.Add("Hold the Left Trigger while running to sprint. \nJumping while sprinting gives you extra distance.");
            }
            else
            {
                associateOneDialogue.Add("Some jumps are too far for Daryl! \n\nTry holding [SPACE] while running to sprint, \nthen pressing the Up arrow to jump extra far!");
                associateOneDialogue.Add("It's okay, player! It takes some practice! \n\nRemember: Hold [SPACE] and press UP.");
                associateOneDialogue.Add("Everyone makes mistakes! Try again!");
                associateOneDialogue.Add("...You'll get it!");
                associateOneDialogue.Add("Hold [SPACE] while running to sprint. Jumping \nwhile sprinting gives you extra distance.");
            }



            associateOneDialogue.Add("Wait, I get it. You got me! You're just joking \naround! You're missing the jump on purpose!");
            associateOneDialogue.Add("You're a plain jokester.");
            associateOneDialogue.Add("Player, I know you like that hole in the \nground and the cute branch popping out of the \nwall, but we have other things to do!");
            associateOneDialogue.Add("I'm just going to stop talking until you \nfigure this out.");
            associateOneDialogue.Add("I knew you could do it!");
            associateOneDialogue.Add("Wha...why? Why did you do that? You were \nout!");
            associateOneDialogue.Add("..Oh. Okay, then. Back into the hole, I \nguess.");
            
            associateOneDialogue.Add("Oh boy! It's your friend! \n\nHe looks sad. Go talk to him!");
            associateOneDialogue.Add("A quest! You can see what quests you have up \nhere. Story quests have red names, side quests \nare purple. Pressing 'TAB' would cycle through \nthem...if you had more!");
            associateOneDialogue.Add("Look! A switch appeared! How convenient!");
            associateOneDialogue.Add("Switches do a lot of things. This one made the \nplatform move up to the next room.");
            associateOneDialogue.Add("You can get into that locker with the \ncombination you found!");
            associateOneDialogue.Add("See that lock? That's a bronze lock. It can \nbe opened with the Bronze Key you just stole!");

            if (Game1.gamePadConnected)
                associateOneDialogue.Add("Click the dials to enter the combination. You \ncan see the combinations you know by pressing \nthe button up on the left! (Right stick controls \nthe Cursor. Press it in to click.)");
            else
                associateOneDialogue.Add("Click the dials to enter the combination. You \ncan see the combinations you know by pressing \nthe button up on the left!");

            associateOneDialogue.Add("A key! Double click it to take it!");
            associateOneDialogue.Add("Cool, a treasure chest! Hold 'F' to open it!");
            associateOneDialogue.Add("Back to the chest room! You should probably \nopen it.");
            associateOneDialogue.Add("A textbook! How lucky! You can use those to \nbuy skills.");
            associateOneDialogue.Add("It's Paul and Alan, your friends! \nAnd your locker! You can use that for skill \nstuff! Quick, buy a skill from them! Open your \nlocker!");

            if (Game1.gamePadConnected)
                associateOneDialogue.Add("Your equipped skills! Use them with RT, X, Y, \nand B. Skills level up by killing monsters, and \nyou can view their experience and level on this \nbar. Press Up on the DPad to open and close it.");
            else
                associateOneDialogue.Add("Your equipped skills! Use them with Q, W, E, \nand R. Skills level up by killing monsters, and \nyou can view their experience and level on this \nbar. Press 'T' to open and close it.");

            associateOneDialogue.Add("A bathroom! You can save in those!");
            associateOneDialogue.Add("No, no, no! Go back and save! It's too \ndangerous to continue on without saving!");
            associateOneDialogue.Add("A monster! Oh no! Quick player, destroy it \nwith your new skill! It will drop money and \nhealth when it dies!");


            associateOneDialogue.Add("Keep track of your health and experience up \nhere while you take out the next two monsters!");
            associateOneDialogue.Add("You can see a skill's experience bar on the \nside of each skill.");

            associateOneDialogue.Add("Great job, player! And look, your skill leveled \nup! You can keep track of it's experience and \nlevel here. \nTry using it twice quickly to see the change. ");
            associateOneDialogue.Add("That's an Area Quest Sign. Some areas have \nquests that can completed for rewards, or \nsimply passage through the area! Press 'F' near \nit to view the quest.");
            associateOneDialogue.Add("You leveled up! Sweet! Your stats increase when \nyou do that.");
            associateOneDialogue.Add("You did it! You can continue now!");

            if (Game1.gamePadConnected)
            {
                associateOneDialogue.Add("Look! An item! Press the Left Bumper to pick \nit up!");
                associateOneDialogue.Add("Lucky! A jar of dirt! Press 'OPTIONS' to open \nyour inventory and equip it!");
            }
            else
            {
                associateOneDialogue.Add("Look! An item! Press 'SHIFT' to pick it up!");
                associateOneDialogue.Add("Lucky! A jar of dirt! Press 'i' to open your \ninventory and equip it!");
            }

            associateOneDialogue.Add("Look, a Trenchcoat Crony! These shady \ncharacters sell you all sorts of great \nknick-knacks.");
            associateOneDialogue.Add("Remember to equip anything you bought!");
            associateOneDialogue.Add("You should save now. We're almost at the end, \nand you wouldn't want to die and start from \nthe last save!");

            if (Game1.gamePadConnected)
                associateOneDialogue.Add("Hold down to duck and avoid his \npunches when he gets close!");
            else
                associateOneDialogue.Add("Hold the Down Arrow to duck and avoid his \npunches when he gets close!");

            associateOneDialogue.Add("He's summoning his lawyers! Take them out!");
            associateOneDialogue.Add("You did it! Great job, player!\n\nLet's get out of here!");

            List<String> associateTwoDialogue = new List<string>();
            associateTwoDialogue.Add("Can we get this over with? I \nhave places to be, you know. Just\npress the arrow keys to move.");
            associateTwoDialogue.Add("Press up to jump. Is that really\na surprise?");
            associateTwoDialogue.Add("See that? That's a door. Good job. Press 'F' to\nuse it.");
            associateTwoDialogue.Add("What, did they hire a kindergartener to make\n this freaking place?");
            associateTwoDialogue.Add("Look at that. It just missed you. How unfortuate.\nHold [SPACE] and press the Down Arrow to \ndrop through certain platforms.");
            associateTwoDialogue.Add("Color me surprised. You actually did it.");
            associateTwoDialogue.Add("Ha, nice try, but I don't think so. That's a big \ngap. Hold [SPACE] to sprint. You jump farther \nwhen you're sprinting.");
            associateTwoDialogue.Add("I said, \"Hold [SPACE] and press UP.\"");
            associateTwoDialogue.Add("You're just full of mistakes, aren't you?");
            associateTwoDialogue.Add("*Yaaaaaawn*");
            associateTwoDialogue.Add("...Like I said, Hold [SPACE] while running to \nsprint. Jumping while sprinting gives \nyou extra distance.");
            associateTwoDialogue.Add("You're just wasting my time on purpose, aren't \nyou?");
            associateTwoDialogue.Add("I don't get paid enough for this.");
            associateTwoDialogue.Add("In fact, I don't get paid at all. I don't even \nknow why I'm here.");
            associateTwoDialogue.Add("I'm gonna put you on hold until you figure \nthis out.");
            associateTwoDialogue.Add("I truly believed you could do it.");
            associateTwoDialogue.Add("Now I know you're fucking with me.");
            associateTwoDialogue.Add("Great. Right back in.");

            associateTwoDialogue.Add("Did they seriously use stock images for this \nentire tutorial? Go talk to your \"friend\".");
            associateTwoDialogue.Add("You have a quest. You can see your accepted \nquests here. Story quests have red names, side quests \nare purple. Pressing 'TAB' would cycle through \nthem...but you only have one.");
            associateTwoDialogue.Add("Did that switch just appear out of thin air? \nSeriously?");
            associateTwoDialogue.Add("I'm instructed to tell you that switches do \"a\nlot of things\". This one moved the platform over \nthere.");
            associateTwoDialogue.Add("You can break into some random guy's locker with \nthat combination you found. How ethically sound of you.");
            associateTwoDialogue.Add("If you haven't figured it out yet, keys open locks.\nBronze keys (like the one you just stole) open \nbronze locks (like that one)");
            associateTwoDialogue.Add("Click that button in the upper left to see what \ncombinations you own. Then click those dials to open \nthe locker.");
            associateTwoDialogue.Add("Double click to steal that key. See if I care.");
            associateTwoDialogue.Add("Oh wow, a stock image chest. How immersive. \nHold 'F' to open it.");


            associateTwoDialogue.Add("Great idea. Actually open the chest.");
            associateTwoDialogue.Add("A physics textbook. Kind of weird. But \nI'm being told you can use those to buy skills.");
            associateTwoDialogue.Add("Two dorks and a locker. You can buy and \nequip skills in there. Go do it.");
            associateTwoDialogue.Add("Your equipped skills. Use them with Q, W, E, \nand R. Skills level up by killing monsters, and \nyou can view their experience and level on this \nbar. Press 'T' to open and close it.");
            associateTwoDialogue.Add("A dirty, disgusting bathroom. I guess you can \nsave your progress in them.");
            associateTwoDialogue.Add("What are you doing? Go back and save first.");
            associateTwoDialogue.Add("A...garden gnome. *sigh* Alright, kill it with \nyour skill. It'll drop money and health, \nor something.");


            associateTwoDialogue.Add("Your health and experience are up here.");
            associateTwoDialogue.Add("Remember to watch your skill's experience, too.");
            associateTwoDialogue.Add("Your skill leveled up. Now you can use it twice \nin a row.");
            associateTwoDialogue.Add("That's a Area Quest Sign. Some you have to do, \nothers are optional for rewards. Press 'F' near \nit to view the quest.");
            associateTwoDialogue.Add("I guess that was you leveling up. This game is \nso weird.");
            associateTwoDialogue.Add("Aren't you just an expert?");
            associateTwoDialogue.Add("Who would have thought that breaking those poorly \ndrawn barrels would give you an item. Press \n[SHIFT] to pick it up.");
            associateTwoDialogue.Add("A jar of dirt...? Okay...press 'i' to open your \ninventory and equip it.");
            associateTwoDialogue.Add("That's one shady looking creeper. I guess they're \ncalled \"Trenchcoat Cronies\", and they sell you \nthings.");
            associateTwoDialogue.Add("Remember to equip anything you bought.");
            associateTwoDialogue.Add("You should save now. I really don't want to go \nthrough this entire thing again.");
            associateTwoDialogue.Add("This is absolutely ridiculous...hold down the Down \nArrow to duck and not get punched in the face.");
            associateTwoDialogue.Add("Is he calling his lawyers? What the hell?");
            associateTwoDialogue.Add("Queue the confetti. Grab that Health Boost and \nlet's get this done with.");

            List<String> associateThreeDialogue = new List<string>();
            #endregion

            associateDialogue = new List<List<string>>() { associateOneDialogue, associateTwoDialogue, associateThreeDialogue };
            //Tutorial textures
            associateOneTex = textures["associateOne"];

            //Quests
            demoQuestOne = new DemoQuestOne(true);
            demoQuestTwo = new DemoQuestTwo(true);
            findingBalto = new FindingBalto(true);

            //Side
            beerForToga = new BeerForToga(false);
            beerForGoggles = new BeerForGoggles(false);
            beerForHat = new BeerForHat(false);
            squirrelQuest = new SquirrelQuest(false);
            scissorsQuest = new ScissorsQuest(false);
            buildBridgeOne = new BuildingACornBridge(false);
            buildBridgeTwo = new BuildingBridgeTwo(false);

            game.AllQuests.Add(demoQuestOne.QuestName, demoQuestOne);
            game.AllQuests.Add(demoQuestTwo.QuestName, demoQuestTwo);
            game.AllQuests.Add(findingBalto.QuestName, findingBalto);
            game.AllQuests.Add(buildBridgeOne.QuestName, buildBridgeOne);
            game.AllQuests.Add(buildBridgeTwo.QuestName, buildBridgeTwo);

            //Side
            game.AllQuests.Add(beerForToga.QuestName, beerForToga);
            game.AllQuests.Add(beerForGoggles.QuestName, beerForGoggles);
            game.AllQuests.Add(beerForHat.QuestName, beerForHat);
            game.AllQuests.Add(squirrelQuest.QuestName, squirrelQuest);
            game.AllQuests.Add(scissorsQuest.QuestName, scissorsQuest);

            //Booleans
            chapterTwoBooleans = new Dictionary<String, bool>();
            chapterTwoBooleans.Add("firstTextUsed", false);
            chapterTwoBooleans.Add("secondTextUsed", false);
            chapterTwoBooleans.Add("ThirdTextUsed", false);
            chapterTwoBooleans.Add("checkedPartyAfterBarn", false);
            chapterTwoBooleans.Add("baltoFallen", false);
            chapterTwoBooleans.Add("baltoOnGround", false);
            chapterTwoBooleans.Add("movedBalto", false);
            chapterTwoBooleans.Add("woodsUnlocked", false);
            chapterTwoBooleans.Add("goblinGateDestroyed", false);
            chapterTwoBooleans.Add("crossroadsScenePlayed", false);
            chapterTwoBooleans.Add("kidOneSaved", false);
            chapterTwoBooleans.Add("kidTwoSaved", false);
            chapterTwoBooleans.Add("kidThreeSaved", false);
            chapterTwoBooleans.Add("kidFourSaved", false);
            chapterTwoBooleans.Add("builtBridgeOne", false);
            chapterTwoBooleans.Add("builtBridgeTwo", false);
            chapterTwoBooleans.Add("ApproachedTim", false);
            chapterTwoBooleans.Add("gateFinished", false);
            chapterTwoBooleans.Add("ApproachedTimAgain", false);
            chapterTwoBooleans.Add("SavedTim", false);
            chapterTwoBooleans.Add("DarylCaughtScenePlayed", false);
            chapterTwoBooleans.Add("GateCollapseScenePlayed", false);
            chapterTwoBooleans.Add("trollAdded", false);
            chapterTwoBooleans.Add("demoEndPlayed", false);

            //TUTORIAL DEMO
            chapterTwoBooleans.Add("lowResTutorial", false);
            chapterTwoBooleans.Add("treeFallPlayed", false);
            chapterTwoBooleans.Add("demoFriendSeen", false);
            chapterTwoBooleans.Add("equippedDirt", false);
            chapterTwoBooleans.Add("steveAustinScenePlayed", false);
            chapterTwoBooleans.Add("tutorialEndScenePlayed", false);
            chapterTwoBooleans.Add("spawnTutorialEnemies", false);
            

            AddNPCs();

            //Cutscenes
            demoOpening = new DemoOpening(game, camera, player, textures["TutorialPopUps"], textures["Text1"], textures["Text2"], textures["enterButton"]);
            chapterScenes.Add(demoOpening);

            treeFall = new TreeFall(game, camera, player);
            chapterScenes.Add(treeFall);

            steveAustinScene = new SteveAustinScene(game, camera, player);
            chapterScenes.Add(steveAustinScene);

            tutorialEnd = new TutorialEnd(game, camera, player);
            chapterScenes.Add(tutorialEnd);

            gateCollapseScene = new GateCollapseScene(game, camera, player);
            chapterScenes.Add(gateCollapseScene);

            demoEnd = new DemoEnd(game, camera, player);
            chapterScenes.Add(demoEnd);

            crossroadsScene = new CrossroadsScene(game, camera, player);
            chapterScenes.Add(crossroadsScene);

            savedFirstKidScene = new SavedFirstKidScene(game, camera, player);
            chapterScenes.Add(savedFirstKidScene);

            darylCaughtScene = new DarylCaught(game, camera, player);
            chapterScenes.Add(darylCaughtScene);

            //Change to cutscene to play scene
            state = GameState.Game;
            cutsceneState = 0;
            synopsis = "";
        }

        public override void Update()
        {
            //player.StoryItems.Add("Key Ring", 1);

            //player.OwnedAccessories.Add(new SoloCup());
            //player.OwnedHats.Add(new PartyHat());
            //player.OwnedHoodies.Add(new Toga());
            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }

#if DEBUG
            if(current.IsKeyUp(Keys.P) && last.IsKeyDown(Keys.P))
            {
                player.Experience = player.ExperienceUntilLevel;
            }

            #region Decisions
            if (current.IsKeyUp(Keys.M) && last.IsKeyDown(Keys.M) && decisionNum == 0)
            {
                player.Karma++;
               // makingDecision = true;
                //decisions = Decisions.test;

                //player.EquippedSkills[0].Experience = player.EquippedSkills[0].ExperienceUntilLevel;
            }

            if (makingDecision)
            {
                switch (decisions)
                {
                    case Decisions.test:
                        int num = effectsManager.UpdateDecision("Test decision!");

                        if (num == 1)
                        {
                            effectsManager.AddAnnouncement("It worked!", 60);
                            makingDecision = false;
                            decisions = Decisions.none;
                            decisionNum = 1;
                        }
                        else if (num == 2)
                        {
                            decisions = Decisions.none;
                            makingDecision = false;
                        }
                        break;
                }
            }
            #endregion

#endif

            if (!player.LevelingUp && player.playerState != Player.PlayerState.dead)
            {
                base.Update();
                cursor.Update();
                AddNPCs();
                game.SideQuestManager.AddNPCs();

                switch (state)
                {
                    case GameState.BreakingLocker:

                        #region TUTORIAL STUFF
                        if (currentMap.MapName == "Tutorial Map Five")
                        {
                            if (game.MapBooleans.tutorialMapBooleans["BrokeIntoTutorialLocker"] == false)
                            {
                                game.MapBooleans.tutorialMapBooleans["BrokeIntoTutorialLocker"] = true;
                                game.MapBooleans.tutorialMapBooleans["TutorialTipElevenUsed"] = true;
                            }
                            if (game.MapBooleans.tutorialMapBooleans["FinishedTutorialLocker"] == false)
                            {
                                if(currentLocker != null && currentLocker.state == StudentLocker.LockerState.closed)
                                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][24], 600, 170, game.ChapterTwo.associateOneTex);
                                else
                                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][25], 100, 30, game.ChapterTwo.associateOneTex);

                            }

                                if (player.StoryItems.ContainsKey("Bronze Key"))
                                {
                                    game.MapBooleans.tutorialMapBooleans["FinishedTutorialLocker"] = true;
                                    Chapter.effectsManager.RemoveToolTip();
                                }
                        }
                        #endregion

                        break;

                    case GameState.ChangingMaps:

                        //DEMO END
                        if (nextMap == "TrollsHut" && chapterTwoBooleans["demoEndPlayed"] == false)
                        {
                            state = GameState.Cutscene;
                            chapterTwoBooleans["demoEndPlayed"] = true;
                        }

                        break;

                    case GameState.Game:
                        UpdateNPCs();

                        #region Unlocking Character Bios
                        if (chelsea.Talking && player.AllCharacterBios["Chelsea"] == false)
                            player.UnlockCharacterBio("Chelsea");

                        if (beerForHat.CompletedQuest && player.AllCharacterBios["Jesse"] == false && !game.CurrentSideQuests.Contains(beerForHat))
                            player.UnlockCharacterBio("Jesse");

                        if (beerForToga.CompletedQuest && player.AllCharacterBios["Julius Caesar"] == false && !game.CurrentSideQuests.Contains(beerForToga))
                            player.UnlockCharacterBio("Julius Caesar");

                        if (scissorsQuest.CompletedQuest && player.AllCharacterBios["Pelt Kid"] == false && !game.CurrentSideQuests.Contains(scissorsQuest))
                            player.UnlockCharacterBio("Pelt Kid");

                        if (mark.Talking && player.AllCharacterBios["Mark"] == false)
                            player.UnlockCharacterBio("Mark");
                        #endregion

                        #region Decisions
                        if (makingDecision)
                        {
                            switch (decisions)
                            {
                                //Change the tutorial resolution or not
                                case Decisions.tutorialResolution:
                                    int num = effectsManager.UpdateDecision("WARNING! We notice your computer may be running this at a lower FPS \nthan you may like. \n\nWould you like to decrease the quality of the tutorial level to ease the \nstress on your machine?");

                                    if (num == 1)
                                    {
                                        //effectsManager.AddAnnouncement("Tutorial Level resolution decreased.", 60);
                                        makingDecision = false;
                                        chapterTwoBooleans["lowResTutorial"] = true;
                                        decisions = Decisions.none;
                                    }
                                    else if (num == 2)
                                    {
                                        decisions = Decisions.none;
                                        makingDecision = false;
                                    }
                                    break;
                            }
                        }
                        #endregion

                        //Add a smoke poof when the bridge is built
                        if (buildBridgeOne.CompletedQuest && bridgeKidOne.Quest == null && !game.CurrentQuests.Contains(buildBridgeOne) && !talkingToNPC && !chapterTwoBooleans["builtBridgeOne"])
                        {
                            chapterTwoBooleans["builtBridgeOne"] = true;
                            effectsManager.AddSmokePoof(new Rectangle(1000, 300, 300, 300), 2);
                        }

                        //Add a smoke poof when the second bridge is built
                        if (buildBridgeTwo.CompletedQuest && bridgeKidOne.Quest == null && !game.CurrentQuests.Contains(buildBridgeTwo) && !talkingToNPC && !chapterTwoBooleans["builtBridgeTwo"])
                        {
                            chapterTwoBooleans["builtBridgeTwo"] = true;
                            effectsManager.AddSmokePoof(new Rectangle(850, 48, 870, 652), 2);
                        }

                        //Make balto disappear when paul is talking inside
                        if (paul.Talking && chapterTwoBooleans["baltoOnGround"] && !chapterTwoBooleans["movedBalto"])
                        {
                            chapterTwoBooleans["movedBalto"] = true;
                        }

                        if (chapterTwoBooleans["movedBalto"] && balto.PositionX != 10000)
                        {
                            balto.PositionX = 10000;
                            balto.UpdateRecAndPosition();
                        }

                        if (chapterTwoBooleans["movedBalto"] && currentMap.MapName == "Outside the Party" && player.PositionX > 1600 && !chapterTwoBooleans["woodsUnlocked"])
                        {
                            chapterTwoBooleans["woodsUnlocked"] = true;
                            findingBalto.ConditionsToComplete = "Find Balto again.";
                            findingBalto.SpecialConditions.Clear();
                            findingBalto.SpecialConditions.Add("Find Balto again.", false);

                            TheParty.ToBehindTheParty.IsUseable = true;
                            mark.MapName = "Behind the Party";
                            mark.Dialogue.Clear();
                            mark.Dialogue.Add("I don't feel so good.");
                        }

                        //--Add the second tutorial quest
                        if (demoQuestOne.CompletedQuest == true && demoQuestTwo.CompletedQuest == false && friendOne.Quest == null)
                        {
                            friendOne.AddQuest(demoQuestTwo);
                        }

                        if (TalkingToNPC == false && makingDecision == false)
                        {

                            //Show first text as soon as party starts to demonstrate you having the phone
                            if (!chapterTwoBooleans["firstTextUsed"] && currentMap.MapName == "Chelsea's Field" && game.CurrentQuests.Contains(findingBalto))
                            {
                                firstTextTimer--;

                                if (firstTextTimer == 0)
                                {
                                    chapterTwoBooleans["firstTextUsed"] = true;
                                    effectsManager.AddTextMessage("Greg", "Yo Balto, we still meeting at the barn?");
                                    //Don't allow a new message for a while
                                    effectsManager.timeUntilNextMessage = 12000;
                                }
                            }

                            /*
                            //Third text. Lock the door to the roof and put mark in front of it
                            if (!chapterTwoBooleans["ThirdTextUsed"] && currentMap.MapName == "Spooky Field" && alaina.AcceptedQuest)
                            {
                                chapterTwoBooleans["ThirdTextUsed"] = true;
                                effectsManager.AddTextMessage("Chelsea", "DAMN IT BALTO GET DOWN OFF MY ROOF, DON'T ACT \nLIKE I DIDN'T JUST WATCH YOU GO UP THERE!!!!");
                                findingBalto.ConditionsToComplete = "Go back to the party and get Balto off the roof";
                                findingBalto.SpecialConditions.Clear();
                                findingBalto.SpecialConditions.Add("Go back to the party and get Balto off the roof", false);

                                mark.MapName = "The Party";
                                mark.PositionX = 1665;
                                mark.UpdateRecAndPosition();
                                mark.Dialogue[0] = "bleeegh *moan*";

                                TheParty.ToBehindTheParty.IsUseable = false;
                            }*/

                            //When the player checks the party after the barn
                            if (!chapterTwoBooleans["checkedPartyAfterBarn"] && chapterTwoBooleans["ThirdTextUsed"] && currentMap.MapName == "The Party")
                            {
                                chapterTwoBooleans["checkedPartyAfterBarn"] = true;
                            }

                            if (chapterTwoBooleans["checkedPartyAfterBarn"] && !chapterTwoBooleans["baltoFallen"] && !chapterTwoBooleans["baltoOnGround"] && currentMap.MapName == "Outside the Party" && player.PositionX > 2000)
                            {
                                chapterTwoBooleans["baltoFallen"] = true;
                            }

                            if (chapterTwoBooleans["baltoFallen"] && !chapterTwoBooleans["baltoOnGround"])
                            {
                                if (balto.PositionY < 680 - 388)
                                    balto.VelocityY += GameConstants.GRAVITY;
                                else
                                {
                                    balto.PositionY = 680 - 388;
                                    game.Camera.ShakeCamera(5, 5);
                                    chapterTwoBooleans["baltoOnGround"] = true;

                                    findingBalto.ConditionsToComplete = "Find help for Balto";
                                    findingBalto.SpecialConditions.Clear();
                                    findingBalto.SpecialConditions.Add("Find help for Balto", false);

                                    balto.state = Balto.NPCState.ground;
                                    balto.VelocityY = 0;
                                }
                                
                                balto.UpdateRecAndPosition();
                            }

                            #region Starting Cutscenes


                            if (currentMap == Game1.schoolMaps.maps["TutorialMapTwo"] && player.PositionX >= 1500 && chapterTwoBooleans["treeFallPlayed"] == false)
                            {
                                state = GameState.Cutscene;
                                chapterTwoBooleans["treeFallPlayed"] = true;
                            }
                            if (currentMap == Game1.schoolMaps.maps["TutorialMapFourteen"] && chapterTwoBooleans["steveAustinScenePlayed"] == false)
                            {
                                state = GameState.Cutscene;
                                chapterTwoBooleans["steveAustinScenePlayed"] = true;
                            }
                            

                            //CROSSROADS SCENE ON ENTER
                            if (currentMap.MapName == "Crossroads" && chapterTwoBooleans["crossroadsScenePlayed"] == false)
                            {
                                //state = GameState.Cutscene;
                                //chapterTwoBooleans["crossroadsScenePlayed"] = true;
                            }

                            //DARYL GETS CAUGHT
                            if (chapterTwoBooleans["SavedTim"] == true && chapterTwoBooleans["DarylCaughtScenePlayed"] == false)
                            {
                                state = GameState.Cutscene;
                                chapterTwoBooleans["DarylCaughtScenePlayed"] = true;
                            }
                            

                            #endregion

                            #region Quest stuff
                            if (julius.Quest == null && !julius.GaveToga)
                            {
                                julius.GaveToga = true;
                            }

                            if (callyn.Quest == null && !callyn.GaveGoggles)
                            {
                                callyn.GaveGoggles = true;
                            }

                            if (peltKidOne.Quest == null && !peltKidOne.Dead && currentMap.MapName != "The Goats")
                            {
                                peltKidOne.Dead = true;
                                peltKidOne.Dialogue[0] = "*sizzle*";
                            }
                            #endregion

                            player.Update();
                            hud.Update();
                            currentMap.Update();

                            camera.Update(player, game, currentMap);
                            player.Enemies = currentMap.EnemiesInMap;

                            String checkPortal = currentMap.CheckPortals();

                            //--Change states to start the fade out
                            if (checkPortal != "null")
                            {
                                nextMap = checkPortal;
                                state = GameState.ChangingMaps;
                            }
                        }
                        break;
                }
            }

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case GameState.Cutscene:

                    chapterScenes[cutsceneState].Draw(s);

                    break;
                case GameState.Game:

                    //This draws a frame of black screen before the cutscene plays so the camera doesn't show a frame of the map before it starts
                    if (!chapterTwoBooleans["crossroadsScenePlayed"] && currentMap.MapName == "Crossroads")
                    {
                        s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, camera.StaticTransform);
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 2000, 2000), Color.Black);
                        s.End();
                    }

                    break;
            }
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
            //--Alan
            if (!nPCs.ContainsKey("Alan"))
            {
                List<String> dialogue1 = new List<string>();
                //dialogue1.Add("Chelsea's parties are always rockin'!");
                dialogue1.Add("Got any textbooks for us?");
                alan = new NPC(Game1.whiteFilter, dialogue1, new Rectangle(497, 680 - 388, 516, 388),
                    player, game.Font, game, "The Party", "Alan", false);
                alan.FacingRight = true;
                //alan.CurrentDialogueFace = Game1.npcFaces["Alan"]["Tutorial"];
                nPCs.Add("Alan", alan);
            }

            if (!nPCs.ContainsKey("Paul"))
            {
                //--Paul
                List<String> dialogue2 = new List<string>();
                //dialogue2.Add("Why do you still have Balto's phone? He's been bitching about it all night.");
                dialogue2.Add("Buy a skill. Go ahead.");
                paul = new NPC(game.NPCSprites["Paul"], dialogue2, new Rectangle(825, 280, 516, 388), player,
                    game.Font, game, "The Party", "Paul", false);
                //paul.CurrentDialogueFace = Game1.npcFaces["Paul"]["Tutorial"];
                nPCs.Add("Paul", paul);
            }

            if (!nPCs.ContainsKey("YourFriend"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Melons might taste really bad, but mine can fly!");
                friendOne = new NPC(game.NPCSprites["Your Friend"], dialogue2, demoQuestOne, new Rectangle(907, 625 - 388, (int)(516 * 1.2f), (int)(388 * 1.2f)), player,
                    game.Font, game, "Tutorial Map Three", "Your Friend", false);
                friendOne.FacingRight = true;
                nPCs.Add("YourFriend", friendOne);
            }

            if (!nPCs.ContainsValue(tutorialShop))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Fresh stock in today");
                List<ItemForSale> items = new List<ItemForSale>() { new WeaponForSale(new MelonMallet(), .50f), new HatForSale(new GardeningHat(), .30f), new HoodieForSale(new IHateMelons(), .50f) };
                tutorialShop = new TrenchcoatKid(game.NPCSprites["TrenchcoatBad"], cronydialogue, -10000, 0, player, game, "Tutorial Map Twelve", items);
                nPCs.Add("TutorialCrony", tutorialShop);
            }

            if (!nPCs.ContainsValue(poolShop))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Hey. You look like someone who could use a stolen Textbook or two.");
                List<ItemForSale> items = new List<ItemForSale>() {new WeaponForSale(new Marker(), 1.00f), new AccessoryForSale(new SoloCup(), 2.00f), new TextbookForSale(5.00f, 2), new HoodieForSale(new ScarecrowVest(), 15.00f) };
                 poolShop = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 1000, 650, player, game, "Chelsea's Pool", items);
                nPCs.Add("PoolCrony", poolShop);
            }

            if (!nPCs.ContainsValue(fieldShop))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Nine out of ten doctors recommend buying Textbooks from me.");
                List<ItemForSale> items = new List<ItemForSale>() { new StoryItemForSale(new Scissors(0, 0), 5.99f), new TextbookForSale(7.99f, 3) };
                fieldShop = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 1100, 680, player, game, "InBetween Field", items);
                nPCs.Add("FieldShop", fieldShop);
            }



            if (!nPCs.ContainsKey("Callyn"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("There are so many babes here, bro!");
                callyn = new Callyn(game.NPCSprites["Callyn"], dialogue2, beerForGoggles, new Rectangle(907, 650 - 388, 516, 388), player,
                    game.Font, game, "Ooky Spooky Barn", "Callyn", false);
                callyn.FacingRight = true;
                nPCs.Add("Callyn", callyn);
            }

            if (!nPCs.ContainsKey("PeltKidOne"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Come back once I've harvested this beautiful pelt.");
                peltKidOne = new PeltKid(game.NPCSprites["Pelt Kid"], dialogue2, scissorsQuest, new Rectangle(800, 260, 516, 388), player, game.Font, game, "The Goats", "Pelt Kid", false);
                peltKidOne.FacingRight = true;
                nPCs.Add("PeltKidOne", peltKidOne);
            }

            if (!nPCs.ContainsKey("Julius"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Have a gander at that one over there! I sure would like to conquer -her- lands. I do think I'll go ask for her numerals.");
                julius = new Julius(game.NPCSprites["Julius Caesar"], dialogue2, beerForToga, new Rectangle(1920, 290, 516,388), player, game.Font, game, "The Party", "Julius Caesar", false);
                julius.FacingRight = false;
                nPCs.Add("Julius", julius);
            }

            if (!nPCs.ContainsKey("Squirrel Boy"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Is...is he g-gone?");
                squirrelBoy = new NPC(game.NPCSprites["Squirrel Boy"], dialogue2, new Rectangle(515, 235, 516, 388), player,
                    game.Font, game, "Tree House", "Squirrel Boy", false);
                squirrelBoy.FacingRight = false;
                nPCs.Add("Squirrel Boy", squirrelBoy);
            }

            if (!nPCs.ContainsKey("Chelsea"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("...What do you want, Daryl?");
                chelsea = new Chelsea(game.NPCSprites["Chelsea"], dialogue2, new Rectangle(1200, 650 - 388, 516, 388), player,
                    game.Font, game, "Outside the Party", "Chelsea", false);
                chelsea.FacingRight = false;
                nPCs.Add("Chelsea", chelsea);
            }

            if (!nPCs.ContainsKey("Alaina"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Ooga booga");
                alaina = new NPC(game.NPCSprites["Paul"], dialogue2, new Rectangle(800, -106 - 388, 516, 388), player,
                    game.Font, game, "Ooky Spooky Barn", "Paul", false);
                alaina.FacingRight = true;
                nPCs.Add("Alaina", alaina);
            }

            if (!nPCs.ContainsKey("Blurso"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Duuuuuuuude. Like, what?");
                blurso = new NPC(game.NPCSprites["Blurso"], dialogue2, findingBalto, new Rectangle(2900, 630 - 388, 516, 388), player,
                    game.Font, game, "Outside the Party", "Blurso", false);
                blurso.FacingRight = true;
                nPCs.Add("Blurso", blurso);
            }

            if (!nPCs.ContainsKey("Jesse"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Maybe I'll go ask that guy for his recipe.");
                jesse = new Jesse(game.NPCSprites["Jesse"], dialogue2, beerForHat, new Rectangle(1500, 660 - 388, 516, 388), player,
                    game.Font, game, "Outside the Party", "Jesse", false);
                jesse.FacingRight = true;
                nPCs.Add("Jesse", jesse);
            }

            if (!nPCs.ContainsKey("SaveInstructor"))
            {
                //--Level up NPC
                List<String> dialogueSave = new List<string>();

                dialogueSave.Add("AAAAAAAAHHHHHHHHHHHHHHHH...");
                dialogueSave.Add("Oh, sorry. I'm powering up before we raid the fields over there.");
                dialogueSave.Add("I should be ready in a few more days. I always get annoyed when they try to rush me.");
                saveInstructor = new NPC(game.NPCSprites["Save Instructor"], dialogueSave,
                    new Rectangle(950, 630 - 388, 516, 388), player, game.Font, game, "Chelsea's Field", "Save Instructor", false);
                saveInstructor.FacingRight = false;
                nPCs.Add("SaveInstructor", saveInstructor);
                
            }

            if (!nPCs.ContainsKey("SkillInstructor"))
            {
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogueSkill = new List<string>();
                dialogueSkill.Add("One is wise not to proceed any further without first mastering the ancient art of...uh, skilling. With skills.");
                skillInstructor = new NPC(game.NPCSprites["Skill Instructor"], dialogueSkill,
                    new Rectangle(700, 630 - 388, 516, 388), player, game.Font, game, "Chelsea's Field", "Skill Instructor", false);
                skillInstructor.FacingRight = true;
                nPCs.Add("SkillInstructor", skillInstructor);
            }

            
            if (!nPCs.ContainsKey("InventoryInstructor"))
            {
                //--Inventory Instructor
                List<String> dialogueEquipment = new List<string>();
                dialogueEquipment.Add("Greetings weary traveler.");
                dialogueEquipment.Add("I must warn you that beyond this great wall of grass a powerful evil is present.");
                dialogueEquipment.Add("...");
                dialogueEquipment.Add("I would, like, not suggest going in unless you're at least level three.");
                dialogueEquipment.Add("Farewell fellow warrior, and good luck in your, like, goals.");

                inventoryInstructor = new NPC(game.NPCSprites["Equipment Instructor"], dialogueEquipment,
                    new Rectangle(1605, 630 - 388, 516, 388), player, game.Font, game, "Chelsea's Field", "Equipment Instructor", false);

                nPCs.Add("InventoryInstructor", inventoryInstructor);
            }

            if (!nPCs.ContainsKey("BobTheConstructionGuyOne"))
            {
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogue = new List<string>();

                dialogue.Add("Whoa there, stranger! Hard hats are required beyond this point. This here map is under construction.");
                dialogue.Add("So far we've been making maps free of charge, but the boys and I haven't eaten in weeks!");
                dialogue.Add("Ha ha! Yep, and now I'm the only one left. Human meat ain't so bad.");
                dialogue.Add("But this map could take a while with just one guy, so feel free to stick around and lend a hand if you want. I've got some leftover chili if you're interested.");
                bridgeKidOne = new BridgeKid(game.NPCSprites["Bob the Construction Guy"], dialogue,
                    new Rectangle(500, 680 - 388, 516, 388), player, game.Font, game, "Behind the Party", "Bob the Construction Guy", false);
                bridgeKidOne.FacingRight = false;
                nPCs.Add("BobTheConstructionGuyOne", bridgeKidOne);
            }

            if (!nPCs.ContainsKey("Mark"))
            {
                //--Markbear
                List<String> dialogue = new List<string>();
                dialogue.Add("...GGHHHHRRRAAAARRR.");
                mark = new Mark(game.NPCSprites["Mark"], dialogue,
                    new Rectangle(400, 680 - 388, 516, 388), player, game.Font, game, "Outside the Party", "Mark", false);
                mark.FacingRight = true;
                nPCs.Add("Mark", mark);
            }

            if (!nPCs.ContainsKey("Balto"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add(". . .");
                balto = new Balto(game.NPCSprites["Balto"], dialogue,
                    new Rectangle(2400, -388, 516, 388), player, game.Font, game, "Outside the Party", "Balto", false);
                balto.FacingRight = true;
                nPCs.Add("Balto", balto);
            }

            if (!nPCs.ContainsKey("CrossroadsKid"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add("You go on ahead! I'll keep watch here.");
                crossroadsKid = new NPC(game.NPCSprites["Paul"], dialogue,
                    new Rectangle(-1508, 322, 516, 388), player, game.Font, game, "Crossroads", "Paul", false);
                crossroadsKid.FacingRight = false;
                nPCs.Add("CrossroadsKid", crossroadsKid);
            }

            if (!nPCs.ContainsKey("Tim"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add("Nope. There's no fuckin' way I'm going with you.");
                dialogue.Add("Just get your goofy lookin' ass out of here and leave me alone. I'll take my chances with the deer.");
                tim = new NPC(game.NPCSprites["Tim"], dialogue,
                    new Rectangle(-1508, 322, 516, 388), player, game.Font, game, "Woodsy River", "Tim", false);
                crossroadsKid.FacingRight = false;
                nPCs.Add("Tim", tim);
            }
        }
    }
}
