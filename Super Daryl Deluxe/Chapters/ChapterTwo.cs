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
    public class ChapterTwo : Chapter
    {
        //NPCs
        NPC alan;
        NPC paul;
        Chelsea chelsea;
        Mark mark;
        Julius julius;
        Jesse jesse;
        NPC balto, princess, messengerBoy, robatto;

        NPC jason, steve, claire, ken;
        TheJanitor janitor;
        BridgeKid outsideCampBob;
        BridgeKid cliffBob;

        //History
        Cleopatra cleopatra;
        HistoryHologram hologram;
        NPC napoleon, genghis, privateBrian, frenchSoldier, pharaohGuardOne, pharaohGuardTwo, pharaohGuardThree;

        //Pyramid guards
        NPC pyramidGuardOne, pyramidGuardTwo, pyramidGuardThree, pyramidGuard4, pyramidGuard5, pyramidGuard6, pyramidGuard7, pyramidGuard8, pyramidGuard9;

        //Literature
        NPC pigeon;
        Scrooge scrooge;
        BellMan bellman;
        Marley marley;

        TrenchcoatKid trenchcoatCamp, trenchcoatSnowyStreets;

        //--Story quests
        public FindBaltosPhone findBaltosPhone;
        public FortRaid fortRaid;
        public BehindGoblinyLinesPartOne behindGoblinyLinesPartOne;
        public BehindGoblinyLinesPartTwo behindGoblinyLinesPartTwo;
        JoiningForcesPartOne joiningForcesPartOne;
        public LivingLumber livingLumber;
        public ForeignDebt foreignDebt;
        public PackageForMrScrooge packageForScrooge;
        public DeliveringSupplies deliveringSupplies;
        public JoiningForcesPartTwo joiningForcesPartTwo;
        public AnubisInvasion anubisInvasion;
        public TutoringThePrincess tutoringThePrincess;

        //--Cutscenes
        ChapterTwoOpening opening;
        BaltosPhone baltosPhone;
        SoldiersLeavingValley soldiersLeavingValley;
        TreeEntsAppear treeEntsAppear;
        AfterLumberQuestScene afterLumberScene;
        InvadeChinaSceneP1 invadeChinaPartOne;
        InvadeChinaSceneP2 invadeChinaPartTwo;
        InvadeChinaSceneP3 invadeChinaPartThree;
        ScroogeRoomOne scroogeRoomOne;
        ScroogeRoomTwo scroogeRoomTwo;
        ScroogeRoomThree scroogeRoomThree;
        LitGuardianDefeated litGuardianDefeated;
        ScroogeReward scroogeReward;
        FirstMessageScene firstMessageScene;
        DeliveringSuppliesScene deliveringSuppliesScene;
        CaesarArrivesAtCamp caesarArrivesAtCamp;
        WeddingCrasher weddingCrasher;
        SecondMessageScene secondMessageScene;
        PrincessSceneOne princessSceneOne;
        BombArrives bombArrives;

        //Fort raid
        CampGateOpenScene campGateOpenScene;
        TrollAppear trollAppearScene;
        BombExplode bombExplodeScene;
        EnteringAxisScene enteringAxisScene;
        HorseDestroyed horseDestroyedScene;
        RobattoArrivesInHistory robattoArrivesInHistory;
        ChapterTwoEndOne chapterEndOne;
        ChapterTwoEndTwo chapterEndTwo;
        ChapterTwoEndThree chapterEndThree;

        //Switch state to decision making, then pick the decision
        public enum Decisions
        {
            none,
            invadeChina, invadeFort
        }
        public Decisions decisions;

        //--Story Quest attributes
        Dictionary<String, Boolean> chapterTwoBooleans;

        static Random ran = new Random();

        public Dictionary<String, Boolean> ChapterTwoBooleans { get { return chapterTwoBooleans; } set { chapterTwoBooleans = value; } }

        public ChapterTwo(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            //Quests
            findBaltosPhone = new FindBaltosPhone(true);
            behindGoblinyLinesPartOne = new BehindGoblinyLinesPartOne(true);
            behindGoblinyLinesPartTwo = new BehindGoblinyLinesPartTwo(true);
            joiningForcesPartOne = new JoiningForcesPartOne(true);
            livingLumber = new LivingLumber(true);
            foreignDebt = new ForeignDebt(true);
            anubisInvasion = new AnubisInvasion(true);
            fortRaid = new FortRaid(true);
            packageForScrooge = new PackageForMrScrooge(false);
            deliveringSupplies = new DeliveringSupplies(true);
            joiningForcesPartTwo = new JoiningForcesPartTwo(true);
            tutoringThePrincess = new TutoringThePrincess(true);

            game.AllQuests.Add(findBaltosPhone.QuestName, findBaltosPhone);
            game.AllQuests.Add(behindGoblinyLinesPartOne.QuestName, behindGoblinyLinesPartOne);
            game.AllQuests.Add(behindGoblinyLinesPartTwo.QuestName, behindGoblinyLinesPartTwo);
            game.AllQuests.Add(joiningForcesPartOne.QuestName, joiningForcesPartOne);
            game.AllQuests.Add(livingLumber.QuestName, livingLumber);
            game.AllQuests.Add(foreignDebt.QuestName, foreignDebt);
            game.AllQuests.Add(anubisInvasion.QuestName, anubisInvasion);
            game.AllQuests.Add(fortRaid.QuestName, fortRaid);
            game.AllQuests.Add(packageForScrooge.QuestName, packageForScrooge);
            game.AllQuests.Add(deliveringSupplies.QuestName, deliveringSupplies);
            game.AllQuests.Add(joiningForcesPartTwo.QuestName, joiningForcesPartTwo);
            game.AllQuests.Add(tutoringThePrincess.QuestName, tutoringThePrincess);

            //Booleans
            chapterTwoBooleans = new Dictionary<String, bool>();

            //General
            chapterTwoBooleans.Add("firstMessageScenePlayed", false);
            chapterTwoBooleans.Add("secondMessageScenePlayed", false);
            chapterTwoBooleans.Add("hangermanOfficeScenePlayed", false);
            chapterTwoBooleans.Add("princessSceneOnePlayed", false);
            chapterTwoBooleans.Add("enteringAxisScenePlayed", false);
            chapterTwoBooleans.Add("robattoArrivesInHistoryPlayed", false);
            chapterTwoBooleans.Add("chapterEndPlayed", false);
            chapterTwoBooleans.Add("chapterEndPlayedTwo", false);
            chapterTwoBooleans.Add("chapterEndPlayedThree", false);

            //Side quest
            chapterTwoBooleans.Add("serumGiven", false);
            chapterTwoBooleans.Add("dominiqueTransformScenePlayed", false);
            chapterTwoBooleans.Add("bridgeUsable", false);

            //History room
            chapterTwoBooleans.Add("canEnterBattlefield", false);
            chapterTwoBooleans.Add("behindGoblinyLinesOneCompleted", false);
            chapterTwoBooleans.Add("valleyMortarsDestroyed", false);
            chapterTwoBooleans.Add("behindGoblinyLinesTwoCompleted", false);
            chapterTwoBooleans.Add("soldiersLeavingValleyPlayed", false);
            chapterTwoBooleans.Add("centralSandsRiftStarted", false);
            chapterTwoBooleans.Add("centralSandsRiftCompleted", false);
            chapterTwoBooleans.Add("larreyTransformedToGoblin", false);
            chapterTwoBooleans.Add("suppliesDelivered", false);
            chapterTwoBooleans.Add("caesarArrivesScenePlayed", false);
            chapterTwoBooleans.Add("movedToOutskirts", false);
            chapterTwoBooleans.Add("completedJoiningForcesTwo", false);
            chapterTwoBooleans.Add("bombArriveScenePlayed", false);
            chapterTwoBooleans.Add("movedToFort", false);

            //Caesar Arc
            chapterTwoBooleans.Add("entsReleasedScenePlayed", false);
            chapterTwoBooleans.Add("mongolsRetreated", false);
            chapterTwoBooleans.Add("lumberQuestFinished", false);
            chapterTwoBooleans.Add("lumberQuestScenePlayed", false);
            chapterTwoBooleans.Add("choseToUseHorse", false);
            chapterTwoBooleans.Add("invadeChinaPartOnePlayed", false);
            chapterTwoBooleans.Add("invadeChinaPartTwoPlayed", false);
            chapterTwoBooleans.Add("invadeChinaPartThreePlayed", false);
            chapterTwoBooleans.Add("finishedCaesarArc", false);

            //Literature
            chapterTwoBooleans.Add("forestOfEntsRiftStarted", false);
            chapterTwoBooleans.Add("forestOfEntsRiftCompleted", false);
            chapterTwoBooleans.Add("lightsTurnedOn", false);
            chapterTwoBooleans.Add("bedroomOneCleared", false);
            chapterTwoBooleans.Add("bedroomTwoCleared", false);
            chapterTwoBooleans.Add("bedroomThreeCleared", false);
            chapterTwoBooleans.Add("livingRoomChestSpawned", false);
            chapterTwoBooleans.Add("batteryPlaced", false);
            chapterTwoBooleans.Add("santaReleased", false);
            chapterTwoBooleans.Add("keyGhostDisappeared", false);
            chapterTwoBooleans.Add("keyGhostKilled", false);
            chapterTwoBooleans.Add("summoningDeathDialoguePlayed", false);
            chapterTwoBooleans.Add("literatureGuardianDefeated", false);
            chapterTwoBooleans.Add("moneyReceived", false);

            //Pyramid
            chapterTwoBooleans.Add("pharaohGuardsChained", false);
            chapterTwoBooleans.Add("enteredEgyptDialoguePlayed", false);
            chapterTwoBooleans.Add("savedPharaohGuards", false);
            chapterTwoBooleans.Add("addedSaveCleoQuest", false);

            //Pyramid
            chapterTwoBooleans.Add("outerChamberLocked", false);
            chapterTwoBooleans.Add("outerChamberCleared", false);
            chapterTwoBooleans.Add("sideChamberIIILocked", false);
            chapterTwoBooleans.Add("sideChamberIIICleared", false);
            chapterTwoBooleans.Add("centralHallILocked", false);
            chapterTwoBooleans.Add("centralHallICleared", false);
            chapterTwoBooleans.Add("forgottenChamberILocked", false);
            chapterTwoBooleans.Add("forgottenChamberICleared", false);
            chapterTwoBooleans.Add("forgottenChamberISpawn", false);
            chapterTwoBooleans.Add("undergroundTunnelPlatformOpen", false);
            chapterTwoBooleans.Add("innerChamberPlatformOpen", false);
            chapterTwoBooleans.Add("jarFound", false);
            chapterTwoBooleans.Add("flowerWallBlown", false);
            chapterTwoBooleans.Add("sideChamberIIIWallBlown", false);
            chapterTwoBooleans.Add("floorBlownOut", false);
            chapterTwoBooleans.Add("corruptedCoffinDestroyed", false);
            chapterTwoBooleans.Add("chamberOfCorruptionRockDestroyed", false);
            chapterTwoBooleans.Add("butterflyChamberLocked", false);
            chapterTwoBooleans.Add("butterflyChamberCleared", false);
            chapterTwoBooleans.Add("organChamberThreeWallDestroyed", false);
            chapterTwoBooleans.Add("organChamberThreeJarObtained", false);
            chapterTwoBooleans.Add("organChamberTwoJarObtained", false);
            chapterTwoBooleans.Add("organChamberOneJarObtained", false);
            chapterTwoBooleans.Add("jarThreePlaced", false);
            chapterTwoBooleans.Add("jarTwoPlaced", false);
            chapterTwoBooleans.Add("jarOnePlaced", false);
            chapterTwoBooleans.Add("mummySummoned", false);
            chapterTwoBooleans.Add("tortureChamberOpened", false);
            chapterTwoBooleans.Add("pharaohsTrapStarted", false);
            chapterTwoBooleans.Add("pharaohsTrapFloorBroken", false);
            chapterTwoBooleans.Add("pharaohsTrapCleared", false);
            chapterTwoBooleans.Add("hiddenPassageLocked", false);
            chapterTwoBooleans.Add("hiddenPassageCleared", false);
            chapterTwoBooleans.Add("aspRoomUnlocked", false);
            chapterTwoBooleans.Add("secretPassageOpen", false);
            chapterTwoBooleans.Add("weddingCrashed", false);
            chapterTwoBooleans.Add("finishedCleopatraArc", false);

            //Fort raid
            chapterTwoBooleans.Add("campDoorFallen", false);
            chapterTwoBooleans.Add("clearedCentralFortFirstTime", false);
            chapterTwoBooleans.Add("justEnteredFort", false);
            chapterTwoBooleans.Add("enemyReinforcementsSpawning", false);
            chapterTwoBooleans.Add("clearedEastHuts", false);
            chapterTwoBooleans.Add("clearedWestHuts", false);
            chapterTwoBooleans.Add("spawnedOutsideCampGuards", false);
            chapterTwoBooleans.Add("eastCommanderSpawned", false);
            chapterTwoBooleans.Add("westCommanderSpawned", false);
            chapterTwoBooleans.Add("westCommanderKilled", false);
            chapterTwoBooleans.Add("eastCommanderKilled", false);
            chapterTwoBooleans.Add("centralCommanderSpawned", false);
            chapterTwoBooleans.Add("centralCommanderKilled", false);
            chapterTwoBooleans.Add("horseInCentral", true);
            chapterTwoBooleans.Add("horseInWest", false);
            chapterTwoBooleans.Add("horseInEast", false);
            chapterTwoBooleans.Add("trollSpawnedInWest", false);
            chapterTwoBooleans.Add("westTrollKilled", false);
            chapterTwoBooleans.Add("bombExploded", false);

            //Cutscenes
            opening = new ChapterTwoOpening(game, camera, player);
            chapterScenes.Add(opening);

            baltosPhone = new BaltosPhone(game, camera, player);
            chapterScenes.Add(baltosPhone);

            soldiersLeavingValley = new SoldiersLeavingValley(game, camera, player);
            chapterScenes.Add(soldiersLeavingValley);

            treeEntsAppear = new TreeEntsAppear(game, camera, player);
            chapterScenes.Add(treeEntsAppear);

            afterLumberScene = new AfterLumberQuestScene(game, camera, player);
            chapterScenes.Add(afterLumberScene);

            invadeChinaPartOne = new InvadeChinaSceneP1(game, camera, player);
            chapterScenes.Add(invadeChinaPartOne);

            invadeChinaPartTwo = new InvadeChinaSceneP2(game, camera, player);
            chapterScenes.Add(invadeChinaPartTwo);

            invadeChinaPartThree = new InvadeChinaSceneP3(game, camera, player);
            chapterScenes.Add(invadeChinaPartThree);

            scroogeRoomOne = new ScroogeRoomOne(game, camera, player);
            chapterScenes.Add(scroogeRoomOne);

            scroogeRoomTwo = new ScroogeRoomTwo(game, camera, player);
            chapterScenes.Add(scroogeRoomTwo);

            scroogeRoomThree = new ScroogeRoomThree(game, camera, player);
            chapterScenes.Add(scroogeRoomThree);

            litGuardianDefeated = new LitGuardianDefeated(game, camera, player);
            chapterScenes.Add(litGuardianDefeated);

            scroogeReward = new ScroogeReward(game, camera, player);
            chapterScenes.Add(scroogeReward);

            firstMessageScene = new FirstMessageScene(game, camera, player);
            chapterScenes.Add(firstMessageScene);

            deliveringSuppliesScene = new DeliveringSuppliesScene(game, camera, player);
            chapterScenes.Add(deliveringSuppliesScene);

            caesarArrivesAtCamp = new CaesarArrivesAtCamp(game, camera, player);
            chapterScenes.Add(caesarArrivesAtCamp);

            weddingCrasher = new WeddingCrasher(game, camera, player);
            chapterScenes.Add(weddingCrasher);

            secondMessageScene = new SecondMessageScene(game, camera, player);
            chapterScenes.Add(secondMessageScene);

            princessSceneOne = new PrincessSceneOne(game, camera, player);
            chapterScenes.Add(princessSceneOne);

            bombArrives = new BombArrives(game, camera, player);
            chapterScenes.Add(bombArrives);

            //Fort raid
            campGateOpenScene = new CampGateOpenScene(game, camera, player);
            chapterScenes.Add(campGateOpenScene);

            trollAppearScene = new TrollAppear(game, camera, player);
            chapterScenes.Add(trollAppearScene);

            bombExplodeScene = new BombExplode(game, camera, player);
            chapterScenes.Add(bombExplodeScene);

            enteringAxisScene = new EnteringAxisScene(game, camera, player);
            chapterScenes.Add(enteringAxisScene);

            robattoArrivesInHistory = new RobattoArrivesInHistory(game, camera, player);
            chapterScenes.Add(robattoArrivesInHistory);

            chapterEndOne = new ChapterTwoEndOne(game, camera, player);
            chapterScenes.Add(chapterEndOne);

            chapterEndTwo = new ChapterTwoEndTwo(game, camera, player);
            chapterScenes.Add(chapterEndTwo);

            chapterEndThree = new ChapterTwoEndThree(game, camera, player);
            chapterScenes.Add(chapterEndThree);

            horseDestroyedScene = new HorseDestroyed(game, camera, player);
            //Add it when the horse is actually destroyed


            //Change to cutscene to play scene
            state = GameState.Game;
            cutsceneState = 0;
            synopsis = "";
        }

        public override void Update()
        {
            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }

            #if DEBUG
            NorthHall.ToScienceIntroRoom.ItemNameToUnlock = null;
            BridgeOfArmanhand.ToRiver.ItemNameToUnlock = null;
            TheStage.ToBackstage.ItemNameToUnlock = null;

            if (current.IsKeyUp(Keys.P) && last.IsKeyDown(Keys.P) && !chapterTwoBooleans["chapterEndPlayed"])
            {
                //player.Experience = player.ExperienceUntilLevel;
                //chapterTwoBooleans["chapterEndPlayed"] = true;
                //state = GameState.Cutscene;

                player.Health = player.realMaxHealth;

                //napoleon.RemoveQuest(behindGoblinyLinesPartOne);
                //napoleon.AddQuest(fortRaid);
                //chapterTwoBooleans["completedJoiningForcesTwo"] = true;
            }
            
            #endif

            if (!player.LevelingUp && player.playerState != Player.PlayerState.dead)
            {
                base.Update();
                cursor.Update();
                AddNPCs();
                game.SideQuestManager.AddNPCs();

                switch (state)
                {
                    case GameState.Game:
                        UpdateNPCs();
                        game.SideQuestManager.Update();
                        if(!chapterTwoBooleans["invadeChinaPartOnePlayed"] && !genghis.canTalk && player.StoryItems.ContainsKey("Letter to Caesar"))
                        {
                            genghis.canTalk = true;
                        }
                        if (!chapterTwoBooleans["invadeChinaPartThreePlayed"] && chapterTwoBooleans["invadeChinaPartTwoPlayed"] && currentMap.MapName == "Behind the Great Wall")
                        {
                            state = GameState.Cutscene;
                            chapterTwoBooleans["invadeChinaPartThreePlayed"] = true;
                        }

                        if (!chapterTwoBooleans["invadeChinaPartTwoPlayed"] && chapterTwoBooleans["invadeChinaPartOnePlayed"] && currentMap.MapName == "The Great Wall")
                        {
                            state = GameState.Cutscene;
                            chapterTwoBooleans["invadeChinaPartTwoPlayed"] = true;
                        }

                        //Start tree ents released scene
                        if (!chapterTwoBooleans["entsReleasedScenePlayed"] && player.StoryItems.ContainsKey("Letter to Caesar") && currentMap.MapName == "The Great Wall" && genghis.Talking)
                        {
                            Chapter.effectsManager.ClearDialogue();
                            chapterTwoBooleans["entsReleasedScenePlayed"] = true;
                            genghis.Talking = false;
                            state = GameState.Cutscene;
                        }

                        //Decision to invade caesar's china with the horse
                        if (chapterTwoBooleans["lumberQuestScenePlayed"] && currentMap.MapName == "Mongolian Camp" && genghis.Talking && !chapterTwoBooleans["choseToUseHorse"])
                        {
                            talkingToNPC = false;
                            genghis.Talking = false;
                            effectsManager.foregroundFButtonRecs.Clear();
                            effectsManager.fButtonRecs.Clear();
                            makingDecision = true;
                            decisions = Decisions.invadeChina;
                        }

                        //Receive scrooge reward
                        if (!chapterTwoBooleans["moneyReceived"] && chapterTwoBooleans["literatureGuardianDefeated"] && currentMap.MapName == "Haunted Bedroom" && scrooge.Talking)
                        {
                            Chapter.effectsManager.ClearDialogue();
                            chapterTwoBooleans["moneyReceived"] = true;
                            scrooge.Talking = false;
                            state = GameState.Cutscene;
                        }
                        if (chapterTwoBooleans["moneyReceived"] && scrooge.isScared)
                            scrooge.isScared = false;
                        if (!chapterTwoBooleans["princessSceneOnePlayed"] && chapterTwoBooleans["hangermanOfficeScenePlayed"] && princess.Talking)
                        {
                            Chapter.effectsManager.ClearDialogue();
                            chapterTwoBooleans["princessSceneOnePlayed"] = true;
                            princess.Talking = false;
                            state = GameState.Cutscene;
                        }

                        if (chapterTwoBooleans["moneyReceived"] && jason.MapName != "Snowy Streets" && !chapterTwoBooleans["finishedCleopatraArc"])
                        {
                            game.ChapterTwo.NPCs["Jason Mysterio"].PositionX = 2172;
                            game.ChapterTwo.NPCs["Jason Mysterio"].PositionY = -470 + 673;
                            game.ChapterTwo.NPCs["Jason Mysterio"].ClearDialogue();
                            game.ChapterTwo.NPCs["Jason Mysterio"].Dialogue.Add("Did you just come out of that mansion?");
                            game.ChapterTwo.NPCs["Jason Mysterio"].Dialogue.Add("It's full of ghosts, I hear. I also hear there's a big cash reward to be earned. Our first bounty is basically already in the bag!");
                            game.ChapterTwo.NPCs["Jason Mysterio"].UpdateRecAndPosition();
                            game.ChapterTwo.NPCs["Jason Mysterio"].FacingRight = true;
                            game.ChapterTwo.NPCs["Jason Mysterio"].MapName = "Snowy Streets";

                            game.ChapterTwo.NPCs["Claire Voyant"].PositionX = 2422;
                            game.ChapterTwo.NPCs["Claire Voyant"].PositionY = -470 + 673;
                            game.ChapterTwo.NPCs["Claire Voyant"].ClearDialogue();
                            game.ChapterTwo.NPCs["Claire Voyant"].Dialogue.Add("There are great riches to be had in this residence.");
                            game.ChapterTwo.NPCs["Claire Voyant"].Dialogue.Add("And I'm sensing that there is no one more powerful than our Team Squad to take it!");
                            game.ChapterTwo.NPCs["Claire Voyant"].FacingRight = true;
                            game.ChapterTwo.NPCs["Claire Voyant"].UpdateRecAndPosition();
                            game.ChapterTwo.NPCs["Claire Voyant"].MapName = "Snowy Streets";

                            game.ChapterTwo.NPCs["Steve Pantski"].PositionX = 2506;
                            game.ChapterTwo.NPCs["Steve Pantski"].PositionY = -470 + 600;
                            game.ChapterTwo.NPCs["Steve Pantski"].canTalk = false;
                            game.ChapterTwo.NPCs["Steve Pantski"].ClearDialogue();
                            game.ChapterTwo.NPCs["Steve Pantski"].UpdateRecAndPosition();
                            game.ChapterTwo.NPCs["Steve Pantski"].MapName = "Snowy Streets";

                            game.ChapterTwo.NPCs["Ken Speercy"].MapName = "No real map";
                        }
                        else if (chapterTwoBooleans["finishedCleopatraArc"] && jason.MapName == "Snowy Streets")
                        {
                            game.ChapterTwo.NPCs["Jason Mysterio"].MapName = "No real map";

                            game.ChapterTwo.NPCs["Claire Voyant"].MapName = "No real map";

                            game.ChapterTwo.NPCs["Steve Pantski"].MapName = "No real map";
                        }

                        if (game.AllQuests[joiningForcesPartOne.QuestName].CompletedQuest && napoleon.Quest == null && game.AllQuests[deliveringSupplies.QuestName].CompletedQuest == false)
                        {
                            napoleon.AddQuest(deliveringSupplies);
                        }

                        if (!chapterTwoBooleans["suppliesDelivered"] && game.CurrentQuests.Contains(deliveringSupplies) && currentMap.MapName == "Battlefield Outskirts" && privateBrian.Talking)
                        {
                            Chapter.effectsManager.ClearDialogue();
                            chapterTwoBooleans["suppliesDelivered"] = true;
                            privateBrian.Talking = false;
                            state = GameState.Cutscene;

                            genghis.MapName = "The Yurt of Khan";
                            genghis.RecX = 680;
                            genghis.RecY = 310;
                            genghis.PositionX = 680;
                            genghis.PositionY = 310;
                            genghis.ClearDialogue();
                            genghis.Dialogue.Add("Well done, Kublai. Caesar has paid his debt in full, with sufficient interest to keep me from parading around with his head on a pike.");
                            genghis.Dialogue.Add("He mentioned he was going to a place of riches, power, and beautiful women far across the History Room. Perhaps I should consider joining this alliance as well...");
                        }

                        if (!chapterTwoBooleans["caesarArrivesScenePlayed"] && chapterTwoBooleans["suppliesDelivered"] && currentMap.MapName == "Napoleon's Camp" && napoleon.Talking)
                        {
                            Chapter.effectsManager.ClearDialogue();
                            chapterTwoBooleans["caesarArrivesScenePlayed"] = true;
                            napoleon.Talking = false;
                            state = GameState.Cutscene;
                            chapterTwoBooleans["pharaohGuardsChained"] = true;
                        }

                        if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && !chapterTwoBooleans["movedToFort"] && napoleon.MapName != "Battlefield Outskirts")
                        {
                            genghis.MapName = "Battlefield Outskirts";
                            genghis.PositionX = 1273;
                            genghis.PositionY = Game1.schoolMaps.maps["Battlefield Outskirts"].mapRec.Y + 681 + 160;
                            genghis.UpdateRecAndPosition();
                            genghis.ClearDialogue();
                            genghis.Dialogue.Add("Kublai! I am glad to see you have also joined this alliance!");
                            genghis.Dialogue.Add("We will bathe in the blood of these disgusting creatures and win glory for our tribe!");
                            genghis.FacingRight = true;
                            genghis.canTalk = true;

                            napoleon.MapName = "Battlefield Outskirts";
                            napoleon.PositionX = 925;
                            napoleon.PositionY = Game1.schoolMaps.maps["Battlefield Outskirts"].mapRec.Y + 685 + 160;
                            napoleon.UpdateRecAndPosition();
                            napoleon.FacingRight = false;

                            julius.MapName = "Battlefield Outskirts";
                            julius.PositionX = 1752;
                            julius.PositionY = Game1.schoolMaps.maps["Battlefield Outskirts"].mapRec.Y + 678 + 160;
                            julius.UpdateRecAndPosition();
                            julius.ClearDialogue();
                            julius.Dialogue.Add("Now that we are all here, I can finally show my dear Cleopatra how powerful my armies are.");
                            julius.Dialogue.Add("A woman should never have to dirty herself in a fight such as the one that is about to begin.");
                            julius.FacingRight = false;

                            cleopatra.MapName = "Battlefield Outskirts";
                            cleopatra.PositionX = 2059;
                            cleopatra.PositionY = Game1.schoolMaps.maps["Battlefield Outskirts"].mapRec.Y + 698 + 160;
                            cleopatra.UpdateRecAndPosition();
                            cleopatra.ClearDialogue();
                            cleopatra.chained = false;
                            cleopatra.FacingRight = true;
                            cleopatra.Dialogue.Add("I must thank you for saving me earlier, as I never got the chance. It was most courageous, although I wish you had not brought that drooling dog along with you.");
                        }

                        if (chapterTwoBooleans["completedJoiningForcesTwo"] && napoleon.Quest == null && game.AllQuests[fortRaid.QuestName].CompletedQuest == false)
                        {
                            napoleon.AddQuest(fortRaid);
                        }

                        //Decision to invade caesar's china with the horse
                        if (chapterTwoBooleans["completedJoiningForcesTwo"] && currentMap.MapName == "Battlefield Outskirts" && napoleon.Talking && napoleon.Quest == fortRaid)
                        {
                            talkingToNPC = false;
                            napoleon.Talking = false;
                            effectsManager.foregroundFButtonRecs.Clear();
                            effectsManager.fButtonRecs.Clear();
                            makingDecision = true;
                            decisions = Decisions.invadeFort;
                        }



                        //--Decision making
                        #region Decisions
                        if (makingDecision)
                        {
                            switch (decisions)
                            {
                                case Decisions.invadeChina:
                                    int num = effectsManager.UpdateDecision("Here it is, Kublai! Caesar will never suspect a thing. Are you ready to do your duty to our tribe?", "Genghis");

                                    if (num == 1)
                                    {
                                        makingDecision = false;
                                        decisions = Decisions.none;
                                        chapterTwoBooleans["choseToUseHorse"] = true;
                                        chapterTwoBooleans["invadeChinaPartOnePlayed"] = true;
                                        state = GameState.Cutscene;

                                    }
                                    else if (num == 2)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("What are you waiting for? Caesar must pay, Kublai!", "Genghis", "Normal", 250);
                                        decisions = Decisions.none;
                                        makingDecision = false;
                                    }
                                    break;

                                case Decisions.invadeFort:
                                    int num2 = effectsManager.UpdateDecision("We are ready to attack ze enemy's fort now. Once we go will cannot turn back until it is over, you must know. Are you ready?", "Napoleon");

                                    if (num2 == 1)
                                    {
                                        makingDecision = false;
                                        decisions = Decisions.none;
                                        state = GameState.Cutscene;
                                        chapterTwoBooleans["movedToFort"] = true;
                                    }
                                    else if (num2 == 2)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("We will wait for you to be prepared, of course. Perhaps you need to use ze bathroom zat we brought with us?", "Napoleon", "Normal", 250);
                                        decisions = Decisions.none;
                                        makingDecision = false;
                                    }
                                    break;
                            }
                        }
                        #endregion

                        if (TalkingToNPC == false && makingDecision == false)
                        {

                            #region Starting Cutscenes / Story related changes

                            if (ChapterTwoBooleans["behindGoblinyLinesTwoCompleted"] && chapterTwoBooleans["soldiersLeavingValleyPlayed"] == false)
                            {
                                chapterTwoBooleans["soldiersLeavingValleyPlayed"] = true;
                                state = GameState.Cutscene;

                                napoleon.AddQuest(joiningForcesPartOne);
                            }

                            if (chapterTwoBooleans["pharaohGuardsChained"] && pharaohGuardOne.Name != "Chained Pharaoh Guard")
                            {
                                pharaohGuardOne.Name = "Chained Pharaoh Guard";
                                pharaohGuardTwo.Name = "Chained Pharaoh Guard";
                                pharaohGuardThree.Name = "Chained Pharaoh Guard";

                                pharaohGuardOne.canTalk = false;
                                pharaohGuardTwo.canTalk = false;
                                pharaohGuardThree.canTalk = false;

                                pharaohGuardThree.PositionX = 3600;
                                pharaohGuardThree.UpdateRecAndPosition();
                            }

                            if (chapterTwoBooleans["savedPharaohGuards"] && !chapterTwoBooleans["addedSaveCleoQuest"])
                            {
                                chapterTwoBooleans["addedSaveCleoQuest"] = true;

                                pharaohGuardOne.ClearDialogue();
                                pharaohGuardOne.Dialogue.Add("Merciless Aten, chained up and boiling in the sun. I guess it could be worse, we could be fighting through Hell.");

                                pharaohGuardTwo.ClearDialogue();
                                pharaohGuardTwo.Dialogue.Add("Thank Wadjet, finally, a chance to sit down. I've been standing for hours.");

                                pharaohGuardThree.ClearDialogue();
                                pharaohGuardThree.AddQuest(anubisInvasion);

                                pharaohGuardOne.canTalk = true;
                                pharaohGuardTwo.canTalk = true;
                                pharaohGuardThree.canTalk = true;
                            }

                            if (chapterTwoBooleans["lumberQuestFinished"] && !chapterTwoBooleans["lumberQuestScenePlayed"])
                            {

                                chapterTwoBooleans["lumberQuestScenePlayed"] = true;
                                state = GameState.Cutscene;

                            }

                            if (!chapterTwoBooleans["firstMessageScenePlayed"] && chapterTwoBooleans["finishedCaesarArc"] && currentMap.MapName == "Mongolian Camp" && player.VitalRecX > 800 && currentMap.enteringMap == false)
                            {
                                chapterTwoBooleans["firstMessageScenePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!chapterTwoBooleans["secondMessageScenePlayed"] && currentMap.MapName == "Pyramid Entrance" && player.VitalRecX < 1000 && currentMap.enteringMap == false)
                            {
                                chapterTwoBooleans["secondMessageScenePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!chapterTwoBooleans["weddingCrashed"] && currentMap.MapName == "Ancient Altar")
                            {

                                chapterTwoBooleans["weddingCrashed"] = true;
                                state = GameState.Cutscene;

                            }

                            if (!chapterTwoBooleans["enteringAxisScenePlayed"] && currentMap.MapName == "Axis of Historical Reality")
                            {
                                state = GameState.Cutscene;
                            }

                            if (!chapterTwoBooleans["robattoArrivesInHistoryPlayed"] && chapterTwoBooleans["enteringAxisScenePlayed"] && currentMap.MapName == "Axis of Historical Reality" && player.PositionX < 1000)
                            {
                                chapterTwoBooleans["robattoArrivesInHistoryPlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!chapterTwoBooleans["chapterEndPlayedTwo"] && chapterTwoBooleans["chapterEndPlayed"] && currentMap.MapName == "Princess' Room")
                            {
                                chapterTwoBooleans["chapterEndPlayedTwo"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!chapterTwoBooleans["chapterEndPlayedThree"] && chapterTwoBooleans["chapterEndPlayedTwo"] && currentMap.MapName == "North Hall")
                            {
                                chapterTwoBooleans["chapterEndPlayedThree"] = true;
                                state = GameState.Cutscene;
                            }
                            #endregion

                            player.Update();
                            hud.Update();
                            currentMap.Update();

                            camera.Update(player, game, currentMap);
                            player.Enemies = currentMap.EnemiesInMap;

                            #region NPCs Wander
                            if (paul.AcceptedQuest)
                            {
                                paul.RecX = 2880;
                                //nPCs["Paul"].Wander(2400, 2850);
                                //nPCs["Alan"].Wander(2350, 2750);
                                nPCs["Balto"].Wander(2350, 2650);
                            }
                            else
                            {
                                nPCs["Paul"].moveState = NPC.MoveState.standing;
                                nPCs["Alan"].moveState = NPC.MoveState.standing;
                            }
                            #endregion


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

                    if (game.SideQuestManager.sideQuestScenes == SideQuestManager.SideQuestScenes.none)
                    {
                        chapterScenes[cutsceneState].Draw(s);
                        if (chapterScenes[cutsceneState].skippingCutscene)
                            chapterScenes[cutsceneState].DrawSkipCutscene(s);
                    }
                    else
                        game.SideQuestManager.DrawSideQuestScene(s);
                    break;
            }
        }

        public void AddNPC(String name, NPC npc)
        {
            nPCs.Add(name, npc);

            if (game.saveData.chapterTwoNPCWrappers != null)
            {
                for (int i = 0; i < game.saveData.chapterTwoNPCWrappers.Count; i++)
                {
                    if (name == game.saveData.chapterTwoNPCWrappers[i].npcName)
                    {
                        if (game.saveData.chapterTwoNPCWrappers[i].questName != null)
                        {
                            npc.Dialogue = game.saveData.chapterTwoNPCWrappers[i].dialogue;
                            npc.QuestDialogue = game.saveData.chapterTwoNPCWrappers[i].questDialogue;
                            npc.DialogueState = game.saveData.chapterTwoNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterTwoNPCWrappers[i].facingRight;
                            npc.Quest = game.AllQuests[game.saveData.chapterTwoNPCWrappers[i].questName];
                            npc.AcceptedQuest = game.saveData.chapterTwoNPCWrappers[i].acceptedQuest;
                            npc.MapName = game.saveData.chapterTwoNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterTwoNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                        }

                        else if (game.saveData.chapterTwoNPCWrappers[i].trenchCoat == false)
                        {
                            npc.Dialogue = game.saveData.chapterTwoNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.chapterTwoNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterTwoNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            npc.MapName = game.saveData.chapterTwoNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterTwoNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                        }
                        else
                        {
                            npc.Dialogue = game.saveData.chapterTwoNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.chapterTwoNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterTwoNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            (npc as TrenchcoatKid).SoldOut = game.saveData.chapterTwoNPCWrappers[i].trenchcoatSoldOut;
                            npc.MapName = game.saveData.chapterTwoNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterTwoNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterTwoNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterTwoNPCWrappers[i].positionY;
                        }
                    }
                }
            }
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
            //--Alan
            if (!nPCs.ContainsKey("Alan"))
            {
                List<String> dialogue1 = new List<string>();
                dialogue1.Add(" ");
                alan = new NPC(Game1.whiteFilter, dialogue1, new Rectangle(2750, 270, 516, 388),
                    player, game.Font, game, "North Hall", "Alan", false);

                AddNPC("Alan", alan);
            }

            if (!nPCs.ContainsKey("Paul"))
            {
                //--Paul
                List<String> dialogue2 = new List<string>();
                dialogue2.Add(" ");
                paul = new NPC(game.NPCSprites["Paul"], dialogue2, new Rectangle(2880, 680 - 388, 516, 388), player,
                    game.Font, game, "North Hall", "Paul", false);
                AddNPC("Paul", paul);
            }

            if (!nPCs.ContainsKey("Balto"))
            {
                List<String> dialogue2 = new List<string>();
                dialogue2.Add(" ");
                balto = new NPC(game.NPCSprites["Balto"], dialogue2, new Rectangle(1500, 277, 516, 388), player,
                    game.Font, game, "North Hall", "Balto", false);
                AddNPC("Balto", balto);
            }

            if (!nPCs.ContainsKey("Mr. Robatto"))
            {
                List<String> dialogue1 = new List<string>();
                robatto = new NPC(Game1.whiteFilter, dialogue1, new Rectangle(-200, 330, 516, 388),
                    player, game.Font, game, "Not a real map", "Mr. Robatto", false);

                AddNPC("Mr. Robatto", robatto);
            }

            if (!nPCs.ContainsKey("Jason Mysterio"))
            {
                jason = new NPC(game.NPCSprites["Jason Mysterio"], new List<String>() {"We've decided that Reality TV shows about ghosts are way out of style. Our talent can be put to better use by hunting ghosts.", "That's why, starting today, the Transparanormal Investigation Team Squad is focusing 100% on ghost bounties.", "Tell your friends." }, new Rectangle(1322, 278, 516, 388), player,
                    game.Font, game, "Paranormal Club", "Jason Mysterio", false);
                AddNPC("Jason Mysterio", jason);

                claire = new NPC(game.NPCSprites["Claire Voyant"], new List<String>() {"I foresee in our future, great riches and fame.", "...In yours I see unemployment." }, new Rectangle(565, 258, 516, 388), player,
                    game.Font, game, "Paranormal Club", "Claire Voyant", false);
                claire.FacingRight = false;
                AddNPC("Claire Voyant", claire);

                ken = new NPC(game.NPCSprites["Ken Speercy"], new List<String>() {"Jason thinks we can become ghost bounty hunters now that my Ghost Lock Catcher works. I'm not sure how to tell him that it's barely portable, and that the battery is about to die." }, new Rectangle(965, 218, 516, 388), player,
                    game.Font, game, "Paranormal Club", "Ken Speercy", false);
                AddNPC("Ken Speercy", ken);

                steve = new NPC(game.NPCSprites["Steve Pantski"], new List<String>() { "Have you heard the story about how we got our name? It's a good one.", "You see, whenever you start a new club around here you have to submit your Articles of Organization to the Vice Principal's office so he can approve whatever it is you're trying to start.", "The problem is, Claire and Jason both filled out and submitted the forms individually. Claire was always calling us the \"Paranormal Investigation Team\", but Jason is the original founder and thought that \"Squad\" sounded way cooler.", "Anyway, they put down different names for our club on each form and it must have confused Mr. Robatto a bit, because he just ended up merging the names together. Ken tried going to him and clarifying, but he got kicked out of the office for not having a Hall Pass.", "The rest is history!" }, new Rectangle(238, 240, 516, 388), player, 
                    game.Font, game, "Paranormal Club", "Steve Pantski", false);
                steve.FacingRight = true;
                AddNPC("Steve Pantski", steve);
            }

            if (!nPCs.ContainsKey("The Princess"))
            {
                //--Princess
                List<String> dialogue = new List<string>();
                dialogue.Add("I'd like to thank you for completely shattering my sense of privacy. I don't know how I lived before nerds began dropping down from the ceiling every day.");
                princess = new NPC(game.NPCSprites["The Princess"], dialogue, new Rectangle(420, -60, 516, 388),
                    player, game.Font, game, "Princess' Room", "The Princess", false);
                AddNPC("The Princess", princess);
            }

            if (!nPCs.ContainsKey("Messenger Boy"))
            {
                List<String> dialogue = new List<string>();
                messengerBoy = new NPC(game.NPCSprites["Messenger Boy"], dialogue, new Rectangle(420, -60, 516, 388),
                    player, game.Font, game, "No Map", "Messenger Boy", false);
                AddNPC("Messenger Boy", messengerBoy);
            }

            if (!nPCs.ContainsKey("Julius"))
            {
                List<String> dialogue2 = new List<string>();
                //dialogue2.Add("Have a gander at that one over there! I sure would like to conquer -her- lands. I do think I'll go ask for her numerals.");
                //julius = new Julius(game.NPCSprites["Julius Caesar"], dialogue2, beerForToga, new Rectangle(1920, 290, 516,388), player, game.Font, game, "The Party", "Julius Caesar", false);
                //julius.FacingRight = false;

                //dialogue2.Add("Have a gander at that one over there! I sure would like to conquer -her- lands. I do think I'll go ask for her numerals.");
                //julius = new Julius(game.NPCSprites["Julius Caesar"], dialogue2, new Rectangle(2350, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Julius Caesar", false);
                //julius.FacingRight = false;
                //julius.BattleReady = true;
                //AddNPC("Julius", julius);

                dialogue2.Add("Have a gander at that one over there! I sure would like to conquer -her- lands. I do think I'll go ask for her numerals.");
                julius = new Julius(game.NPCSprites["Julius Caesar"], dialogue2, new Rectangle(461, 282, 516, 388), player, game.Font, game, "Behind the Great Wall", "Julius Caesar", false);
                julius.FacingRight = true;
                julius.BattleReady = true;
                AddNPC("Julius", julius);
            }

            if (!nPCs.ContainsKey("BobTheConstructionGuyOne"))
            {
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogue = new List<string>();

                dialogue.Add("Whoa there, pal! Watch where you step, alright?");
                dialogue.Add("My name's Bob and I fix a whole bunch of stuff, like this big ol' scary cliff.");
                dialogue.Add("The problem is that I ran out of bricks and I just can't seem to find my way out of this dang pyramid.");
                dialogue.Add("Haha, yep. Oh well. Unless you have wings or wear shoes with springs in them, I suggest you don't go jumping down there.");
                dialogue.Add("Of course if you do have one of those things, go right ahead. There's treasure all over this place.");
                outsideCampBob = new BridgeKid(game.NPCSprites["Bob the Construction Guy"], dialogue,
                    new Rectangle(600, 100, 516, 388), player, game.Font, game, "The Cliff of Ile", "Bob the Construction Guy", false);
                nPCs.Add("BobTheConstructionGuyOne", outsideCampBob);
            }

            if (!nPCs.ContainsKey("BobTheConstructionGuyTwo"))
            {
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogue = new List<string>();

                dialogue.Add("Whoa there, pal! Watch where you step, alright?");
                dialogue.Add("You're free to drown in this moat all you want, but don't expect anything realistic. There have been a lot of budget cuts lately, we can't afford a splash.");
                outsideCampBob = new BridgeKid(game.NPCSprites["Bob the Construction Guy"], dialogue,
                    new Rectangle(3150, 385, 516, 388), player, game.Font, game, "Stone Fort Gate", "Bob the Construction Guy", false);
                nPCs.Add("BobTheConstructionGuyTwo", outsideCampBob);
            }


            if (!nPCs.ContainsKey("Napoleon"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Be careful in ze deserts between here and Egypt. I doubt even ze Warlord has his forces there. Ze place is dangerous, and odd things lurk there.");

                //This is where he should stand for the fortraid quest
                //napoleon = new NPC(game.NPCSprites["Napoleon"], dialogue2, fortRaid, new Rectangle(1840, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Napoleon", false);

                napoleon = new NPC(game.NPCSprites["Napoleon"], dialogue2, behindGoblinyLinesPartOne, new Rectangle(680, 310, 516, 388), player, game.Font, game, "Napoleon's Tent", "Napoleon", false);
                napoleon.FacingRight = true;
                AddNPC("Napoleon", napoleon);
            }

            if (!nPCs.ContainsKey("Private Brian"))
            {
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("I have a quest");
                privateBrian = new NPC(game.NPCSprites["Private Brian"], dialogue2, behindGoblinyLinesPartTwo, new Rectangle(5125, 350, 516, 388), player, game.Font, game, "No Man's Valley", "Private Brian", false);
                privateBrian.FacingRight = true;
                AddNPC("Private Brian", privateBrian);


                List<String> dialogue3 = new List<string>();
                dialogue3.Add("I knew I should have stayed back and helped Dr. Dominique collect medical supplies...");
                frenchSoldier = new NPC(game.NPCSprites["French Soldier"], dialogue3, new Rectangle(4861, 330, 516, 388), player, game.Font, game, "No Man's Valley", "French Soldier", false);
                frenchSoldier.FacingRight = false;
                AddNPC("French Soldier", frenchSoldier);
            }

            if (!nPCs.ContainsKey("Pharaoh Guard 1"))
            {
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Holy Ra, it's hot out. I suppose it could be worse though, at least we aren't wrapped in chains under this merciless sun. I hate being wrapped in chains.");
                pharaohGuardOne = new NPC(game.NPCSprites["Pharaoh Guard"], dialogue2, new Rectangle(2440, 273, 516, 388), player, game.Font, game, "Egypt", "Pharaoh Guard", false);
                pharaohGuardOne.FacingRight = false;
                AddNPC("Pharaoh Guard 1", pharaohGuardOne);

                List<String> dialogue3 = new List<string>();
                dialogue3.Add("Criminy, it's hot today. I need a break. I haven't sat down in hours.");
                pharaohGuardTwo = new NPC(game.NPCSprites["Pharaoh Guard"], dialogue3, new Rectangle(2947, 287, 516, 388), player, game.Font, game, "Egypt", "Pharaoh Guard", false);
                pharaohGuardTwo.FacingRight = true;
                AddNPC("Pharaoh Guard 2", pharaohGuardTwo);

                List<String> dialogue4 = new List<string>();
                dialogue4.Add("Sorry, pale one. Only the Pharaoh is allowed beyond these gates.");
                pharaohGuardThree = new NPC(game.NPCSprites["Pharaoh Guard"], dialogue4, new Rectangle(3303, 286, 516, 388), player, game.Font, game, "Egypt", "Pharaoh Guard", false);
                pharaohGuardThree.FacingRight = true;
                AddNPC("Pharaoh Guard 3", pharaohGuardThree);
            }

            if (!nPCs.ContainsKey("Cleopatra"))
            {

                //Stone fort cleo
                //List<String> dialogue2 = new List<string>();
                //dialogue2.Add("I am starting to believe that my soldiers may be underdressed for the occasion.");
                //cleopatra = new Cleopatra(game.NPCSprites["Cleopatra"], dialogue2, new Rectangle(1690, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Cleopatra", false);
                //cleopatra.FacingRight = true;
                //AddNPC("Cleopatra", cleopatra);

                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Take these chains off of me at once!");
                cleopatra = new Cleopatra(game.NPCSprites["Cleopatra"], dialogue2, new Rectangle(1792, 301, 516, 388), player, game.Font, game, "Ancient Altar", "Cleopatra", false);
                cleopatra.FacingRight = true;
                cleopatra.chained = true;
                AddNPC("Cleopatra", cleopatra);

                hologram = new HistoryHologram(game.NPCSprites["Time Lord"], new List<String>(), new Rectangle(2121, 269, 516, 388), player, game.Font, game, "Ancient Altar", "Time Lord", false);
                hologram.FacingRight = false;
                AddNPC("Time Lord", hologram);
            }

            if (!nPCs.ContainsKey("Genghis"))
            {
                //List<String> dialogue2 = new List<string>();
                //dialogue2.Add("Kublai! Where have you been?");
                //genghis = new NPC(game.NPCSprites["Genghis"], dialogue2, new Rectangle(1470, 350, 516, 388), player, game.Font, game, "Stone Fort Gate", "Genghis", false);
                //genghis.FacingRight = true;
                //AddNPC("Genghis", genghis);

                List<String> dialogue2 = new List<string>();
                dialogue2.Add("You will be mine, Caesar!");
                genghis = new NPC(game.NPCSprites["Genghis"], dialogue2, new Rectangle(1623, 650, 516, 388), player, game.Font, game, "The Great Wall", "Genghis", false);
                genghis.FacingRight = false;
                genghis.canTalk = false;
                AddNPC("Genghis", genghis);
            }

            if (!nPCs.ContainsKey("Chelsea"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                chelsea = new Chelsea(game.NPCSprites["Chelsea"], dialogue2, new Rectangle(3504- 516, 286, 516, 388), player,
                    game.Font, game, "Not a real map", "Chelsea", false);
                chelsea.FacingRight = false;
                AddNPC("Chelsea", chelsea);
            }

            if (!nPCs.ContainsKey("Jesse"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Maybe I'll go ask that guy for his recipe.");
                jesse = new Jesse(game.NPCSprites["Jesse"], dialogue2, new Rectangle(1500, 660 - 388, 516, 388), player,
                    game.Font, game, "Outside the Party", "Jesse", false);
                jesse.FacingRight = true;
                AddNPC("Jesse", jesse);
            }

            if (!nPCs.ContainsKey("Mark"))
            {
                //--Markbear
                List<String> dialogue = new List<string>();
                dialogue.Add("...GGHHHHRRRAAAARRR.");
                mark = new Mark(game.NPCSprites["Mark"], dialogue,
                    new Rectangle(400, 680 - 388, 516, 388), player, game.Font, game, "Outside the Party", "Mark", false);
                mark.FacingRight = true;
                AddNPC("Mark", mark);
            }

            if (!nPCs.ContainsKey("Balto"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add(". . .");
                balto = new Balto(game.NPCSprites["Balto"], dialogue,
                    new Rectangle(2400, -388, 516, 388), player, game.Font, game, "Outside the Party", "Balto", false);
                balto.FacingRight = true;
                
                AddNPC("Balto", balto);
            }

            if (!nPCs.ContainsKey("Bell Man"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add("Wanted! Wanted! Ghost slayer for hire!");
                dialogue.Add("Will you be the one to rid Master Scrooge's residence of those foul demons?");
                bellman = new BellMan(game.NPCSprites["Bell Man"], dialogue,
                    new Rectangle(883, -470 + 742, 516, 388), player, game.Font, game, "Snowy Streets", "Bell Man", false);
                AddNPC("Bell Man", bellman);
            }

            if (!nPCs.ContainsKey("Ebenezer Scrooge"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add(". . .");
                scrooge = new Scrooge(game.NPCSprites["Ebenezer Scrooge"], dialogue,
                    new Rectangle(370 + 516, 165, 516, 388), player, game.Font, game, "Scrooge's Bedroom", "Ebenezer Scrooge", false);
                scrooge.FacingRight = false;
                scrooge.isScared = true;
                scrooge.canTalk = false;
                AddNPC("Ebenezer Scrooge", scrooge);
            }

            if (!nPCs.ContainsKey("Marley"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add(". . .");
                marley = new Marley(game.NPCSprites["Jacob Marley"], dialogue,
                    new Rectangle(433, 45, 516, 388), player, game.Font, game, "Scrooge's Bedroom", "Jacob Marley", false);
                marley.FacingRight = true;
                marley.canTalk = false;
                AddNPC("Marley", marley);
            }

            if (!nPCs.ContainsKey("Gary R. Pigeon") && chapterTwoBooleans["bedroomTwoCleared"])
            {
                List<String> dialogue = new List<string>();
                dialogue.Add("Pleasure doin' bidness widya. Coo.");
                pigeon = new NPC(game.NPCSprites["Gary R. Pigeon"], dialogue, packageForScrooge,
                    new Rectangle(3562, -605, 516, 388), player, game.Font, game, "The Grand Corridor", "Gary R. Pigeon", false);
                pigeon.FacingRight = false;

                AddNPC("Gary R. Pigeon", pigeon);
            }

            if (!nPCs.ContainsKey("The Janitor"))
            {
                //--Paul
                List<String> dialogue2 = new List<string>();
                dialogue2.Add(" ");
                janitor = new TheJanitor(game.NPCSprites["The Janitor"], dialogue2, new Rectangle(2104, 349, 516, 388), player,
                    game.Font, game, "Not a real map", "The Janitor", false);
                AddNPC("The Janitor", janitor);
            }

            #region Pyramid Guards
            if (!nPCs.ContainsKey("Pyramid Guard One"))
            {
                List<String> dialogue = new List<string>();
                dialogue.Add("Watch your step. I was just repairing the floor when the bastards got me.");
                dialogue.Add("They didn't have our Pharaoh with them. Perhaps they haven't taken her?");
                pyramidGuardOne = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue,
                    new Rectangle(331, 253, 516, 388), player, game.Font, game, "Side Chamber IV", "Chained Pharaoh Guard", false);
                pyramidGuardOne.FacingRight = false;
                AddNPC("Pyramid Guard One", pyramidGuardOne);

                List<String> dialogue2 = new List<string>();
                dialogue2.Add("They went through here! They have Pharaoh Cleopatra!");
                dialogue2.Add("Oh gods, they're headed for the Pyramid's center. Legend says the greatest treasure is stored there, and only the Pharaoh can open the door!");
                pyramidGuardTwo = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue2,
                    new Rectangle(1513, -720 + 483, 516, 388), player, game.Font, game, "Main Chamber", "Chained Pharaoh Guard", false);
                AddNPC("Pyramid Guard Two", pyramidGuardTwo);


                List<String> dialogue3 = new List<string>();
                dialogue3.Add("And here I thought I was immune to the powers of Hell.");
                pyramidGuardThree = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue3,
                    new Rectangle(1819, -720 + 483, 516, 388), player, game.Font, game, "Main Chamber", "Chained Pharaoh Guard", false);
                AddNPC("Pyramid Guard Three", pyramidGuardThree);

                List<String> dialogue4 = new List<string>();
                dialogue4.Add("There used to be a tunnel here leading to the Pyramid's Center, and the wonderous treasure that lies there.");
                dialogue4.Add("Luckily I covered it up nice and good before those hellspawn tied me up. Yep, nothing's getting through there. Nothing at all.");
                pyramidGuard4 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue4,
                    new Rectangle(641, -300 + 146, 516, 388), player, game.Font, game, "Central Hall III", "Chained Pharaoh Guard", false);
                AddNPC("Pyramid Guard 4", pyramidGuard4);

                List<String> dialogue5 = new List<string>();
                dialogue5.Add("The demons came through here not very long ago. They're using the Pharaoh to get into the room at the Pyramid's Center.");
                dialogue5.Add("Our poor Cleopatra...she was chained and screaming. I stopped hearing her a few minutes ago, I hope they haven't done anything to harm her.");
                pyramidGuard5 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue5,
                    new Rectangle(2319, 267, 516, 388), player, game.Font, game, "Inner Chamber", "Chained Pharaoh Guard", false);
                AddNPC("Pyramid Guard 5", pyramidGuard5);

                List<String> dialogue6 = new List<string>();
                dialogue6.Add("You! You have to save our Pharaoh! Ra knows what those bastard demons plan to do to her once she's opened the Pyramid's Center for them!");
                dialogue6.Add("It's not too far up ahead, but be careful...you're following a very dangerous path now, meant to make intruders very dead.");
                pyramidGuard6 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue6,
                    new Rectangle(435, 257, 516, 388), player, game.Font, game, "Pharaoh's Road", "Chained Pharaoh Guard", false);
                pyramidGuard6.FacingRight = false;
                AddNPC("Pyramid Guard 6", pyramidGuard6);

                List<String> dialogue7 = new List<string>();
                dialogue7.Add("It's said that a wonderful treasure sits at the Pyramid's Center. That's what they're after. None of us mortals know what it could possibly be.");
                dialogue7.Add("Only the Pharaoh knows, and only she can unlock it...");
                pyramidGuard7 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue7,
                    new Rectangle(670, 255, 516, 388), player, game.Font, game, "Pharaoh's Road", "Chained Pharaoh Guard", false);
                pyramidGuard7.FacingRight = true;
                AddNPC("Pyramid Guard 7", pyramidGuard7);

                List<String> dialogue8 = new List<string>();
                dialogue8.Add("They just came through her with the Pharaoh and forced her to unlock the gate! Only a couple of them went inside...what could they be doing?");
                dialogue8.Add("I hope they spare our poor Pharaoh...you may be too late.");
                pyramidGuard8 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue8,
                    new Rectangle(3129, 262, 516, 388), player, game.Font, game, "Pharaoh's Gate", "Chained Pharaoh Guard", false);
                pyramidGuard8.FacingRight = false;
                pyramidGuard8.canTalk = false;
                AddNPC("Pyramid Guard 8", pyramidGuard8);

                List<String> dialogue9 = new List<string>();
                dialogue9.Add("Did you ever consider untying us so we could help you?");
                pyramidGuard9 = new NPC(game.NPCSprites["Chained Pharaoh Guard"], dialogue9,
                    new Rectangle(3702, 262, 516, 388), player, game.Font, game, "Pharaoh's Gate", "Chained Pharaoh Guard", false);
                pyramidGuard9.FacingRight = false;
                pyramidGuard9.canTalk = false;

                AddNPC("Pyramid Guard 9", pyramidGuard9);
            }
            #endregion

            if (!nPCs.ContainsKey("Trenchcoat Camp"))
            {
                //Napoleons Camp
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Hey kid, any idea what Robatto is doing here? Thought he was coming to bust me, but he walked right on by.");
                cronydialogue.Add("Anyway, looking for some textbooks? Boss raised the prices recently.");
                List<ItemForSale> items = new List<ItemForSale>();
                items.Add(new TextbookForSale(45, 2));
                items.Add(new TextbookForSale(45, 1));
                items.Add(new TextbookForSale(45, 0));
                trenchcoatCamp = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 3026, 638, player, game, "Napoleon's Camp", items);
                trenchcoatCamp.FacingRight = true;
                AddNPC("Trenchcoat Camp", trenchcoatCamp);

                //Snowy Streets
                List<String> cronydialogue2 = new List<string>();
                cronydialogue2.Add("You're looking pretty cold. Can I interest you in some textbooks?");
                List<ItemForSale> items2 = new List<ItemForSale>();
                items2.Add(new TextbookForSale(65, 3));
                items2.Add(new TextbookForSale(65, 0));
                items2.Add(new KeyForSale(150, KeyForSale.KeyType.Silver));
                trenchcoatSnowyStreets = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue2, 1417, -470 + 710 + 388, player, game, "Snowy Streets", items2);
                trenchcoatSnowyStreets.FacingRight = true;
                AddNPC("Trenchcoat Snowy Streets", trenchcoatSnowyStreets);
            }
        }

        public void PlayHorseExplosion()
        {
            chapterScenes.Add(horseDestroyedScene);
            cutsceneState = chapterScenes.Count - 1;
            state = GameState.Cutscene;
        }
    }
}
