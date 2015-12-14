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
    public class ChapterOne : Chapter
    {
        NPC alan;
        NPC paul;
        NPC trenchcoatVents;
        NPC inventoryInstructor, karmaShaman, skillSorceress;
        NPC mapsKid;
        NPC princess;
        NPC beethoven;
        NPC journalEnthusiast;
        NPC jason, claire, ken, steve;
        NPC tchaikovsky;
        NPC theaterManager;
        NPC portalRepairBridge;
        NPC artMerchant;
        TrenchcoatKid trenchcoatMarket, trenchcoatTheater;
        NPC napoleon, frenchSoldier;
        NPC janitor;

        //Homecoming section
        NPC quarterback, death, saveInstructor, alan2, paul2;

        //--Story quests
        BookSprayQuest returningKeys;
        DaddysLittlePrincess daddysLittlePrincess;
        CommunicatingWithBeethoven communicatingWithBeethoven;
        public DealingWithManagement dealingWithManagement;
        public static FundRaising fundRaising;
        public static ProtectTITS protectTITS;
        public PortalRepairman portalRepairman;
        public MusicForAPrincess musicForAPrincess;
        public OccupationOfVienna wheredTheCrowdGo;
        public ProtectTheCamp protectTheCamp;

        //Side Quests
        public LearningAboutMaps learningAboutMaps;

        //Cutscenes
        ChapterOneOpening chOneOpening;
        GettingQuestOne gettingQuestOne;
        PrincessCutscene princessCutscene;
        GhostHuntersInMusic ghostHuntersScene;
        GhostSuckerDestroyed ghostSuckerDestroyed;
        GhostLockGone ghostLockGone;
        BeethovenCanHear beethovenCanHearScene;
        IntroducingTheManager introducingTheManager;
        IntroducingXylophone introducingXylophone;
        BeethovenCanStay beethovenCanStay;
        DaVinciPaintingScene daVinciPaintingScene;
        EnteringTheMarket enteringTheMarket;
        MeetingNapoleon meetingNapoleon;
        CommencePhaseOne commencePhaseOne;
        EmergencyFootballPractice emergencyFootballPractice;
        PrincessAndTheFlute princessAndTheFlute;
        JanitorCutscene janitorCutscene;
        ChapterOneEnd chapterOneEnd;

        //--Story Quest attributes
        Dictionary<String, Boolean> chapterOneBooleans;

        //Switch state to decision making, then pick the decision
        enum Decisions
        {
            none,
            test,
            trenchcoatMarket,
            artDealerMarket
        }
        Decisions decisions;

        static Random ran = new Random();

        public Dictionary<String, Boolean> ChapterOneBooleans { get { return chapterOneBooleans; } set { chapterOneBooleans = value; } }
        public BookSprayQuest ReturningKeys { get { return returningKeys; } }
        public DaddysLittlePrincess DaddysLittlePrincess { get { return daddysLittlePrincess; } }

        public ChapterOne(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            //Quests
            returningKeys = new BookSprayQuest(true);
            learningAboutMaps = new LearningAboutMaps(false);
            daddysLittlePrincess = new DaddysLittlePrincess(true);
            communicatingWithBeethoven = new CommunicatingWithBeethoven(true);
            protectTITS = new ProtectTITS(true);
            dealingWithManagement = new DealingWithManagement(true, game);
            portalRepairman = new PortalRepairman(false);
            fundRaising = new FundRaising(true, game);
            musicForAPrincess = new MusicForAPrincess(true);
            wheredTheCrowdGo = new OccupationOfVienna(true);
            protectTheCamp = new ProtectTheCamp(true);

            chapterOneBooleans = new Dictionary<String, bool>();

            game.AllQuests.Add(returningKeys.QuestName, returningKeys);
            game.AllQuests.Add(learningAboutMaps.QuestName, learningAboutMaps);
            game.AllQuests.Add(daddysLittlePrincess.QuestName, daddysLittlePrincess);
            game.AllQuests.Add(communicatingWithBeethoven.QuestName, communicatingWithBeethoven);
            game.AllQuests.Add(protectTITS.QuestName, protectTITS);
            game.AllQuests.Add(dealingWithManagement.QuestName, dealingWithManagement);
            game.AllQuests.Add(portalRepairman.QuestName, portalRepairman);
            game.AllQuests.Add(fundRaising.QuestName, fundRaising);
            game.AllQuests.Add(musicForAPrincess.QuestName, musicForAPrincess);
            game.AllQuests.Add(wheredTheCrowdGo.QuestName, wheredTheCrowdGo);
            game.AllQuests.Add(protectTheCamp.QuestName, protectTheCamp);


            chapterOneBooleans.Add("alanApproached", false);
            chapterOneBooleans.Add("questOneSceneStarted", false);
            chapterOneBooleans.Add("checkedArtHall", false);
            chapterOneBooleans.Add("completedMapsQuest", false);
            chapterOneBooleans.Add("addedMainLobbyNPCs", false);
            chapterOneBooleans.Add("playedPrincessScene", false);
            chapterOneBooleans.Add("soldGuanoToTrenchcoat", false);
            chapterOneBooleans.Add("openedMaps", false);
            chapterOneBooleans.Add("quickRetortObtained", false);

            chapterOneBooleans.Add("beethovenCanHearSceneStarted", false);
            chapterOneBooleans.Add("spawnedGhostHunters", false);
            chapterOneBooleans.Add("ghostHuntersMet", false);
            chapterOneBooleans.Add("savedGhostHunters", false);
            chapterOneBooleans.Add("ghostLockGoneScenePlayed", false);

            chapterOneBooleans.Add("givenBackstageKey", false);
            chapterOneBooleans.Add("introducingManagerPlayed", false);
            chapterOneBooleans.Add("clearedBackstage", false);
            chapterOneBooleans.Add("clearedRestrictedHall", false);
            chapterOneBooleans.Add("summonedXylophone", false);
            chapterOneBooleans.Add("introducingXylophonePlayed", false);
            chapterOneBooleans.Add("chasingTheManager", false);
            chapterOneBooleans.Add("xylophoneDestroyed", false);
            chapterOneBooleans.Add("finishedManagerQuest", false);
            chapterOneBooleans.Add("beethovenCanStayedPlayed", false);

            chapterOneBooleans.Add("bridgeOfArmanhandRiftStarted", false);
            chapterOneBooleans.Add("bridgeOfArmanhandRiftCompleted", false);
            chapterOneBooleans.Add("daVinciSceneStarted", false);
            chapterOneBooleans.Add("spawnedArtMerchant", false);
            chapterOneBooleans.Add("marketScenePlayed", false);
            chapterOneBooleans.Add("passedTrenchcoat", false);
            chapterOneBooleans.Add("forcedTrenchcoatDecision", false);
            chapterOneBooleans.Add("talkedToArtDealer", false);
            chapterOneBooleans.Add("forcedArtDealerDecision", false);
            chapterOneBooleans.Add("soldPaintingToTrenchcoat", false);
            chapterOneBooleans.Add("soldPaintingToArtDealer", false);
            chapterOneBooleans.Add("completedFundingQuest", false);

            chapterOneBooleans.Add("addedNapoleon", false);
            chapterOneBooleans.Add("meetingNapleonSceneStarted", false);
            chapterOneBooleans.Add("commencePhaseOneSceneStarted", false);
            chapterOneBooleans.Add("homecomingHypeStarted", false);
            chapterOneBooleans.Add("homecomingHypeEnded", false);
            chapterOneBooleans.Add("paulTalkedTo", false);
            chapterOneBooleans.Add("alanTalkedTo", false);
            chapterOneBooleans.Add("deathTalkedTo", false);
            chapterOneBooleans.Add("chadTalkedTo", false);
            chapterOneBooleans.Add("footballSceneStarted", false);
            chapterOneBooleans.Add("destroyedVases", false);

            chapterOneBooleans.Add("battlefieldCleared", false);
            chapterOneBooleans.Add("protectCampQuestComplete", false);

            chapterOneBooleans.Add("princessFluteCutscenePlayed", false);
            chapterOneBooleans.Add("janitorCutscenePlayed", false);
            chapterOneBooleans.Add("chandelierAdded", false);

            //Cutscenes
            chOneOpening = new ChapterOneOpening(game, camera, player, textures["FlashBack"]);
            gettingQuestOne = new GettingQuestOne(game, camera, player);
            princessCutscene = new PrincessCutscene(game, camera, player);
            ghostHuntersScene = new GhostHuntersInMusic(game, camera, player);
            beethovenCanHearScene = new BeethovenCanHear(game, camera, player);
            ghostSuckerDestroyed = new GhostSuckerDestroyed(game, camera, player);
            ghostLockGone = new GhostLockGone(game, camera, player);
            introducingTheManager = new IntroducingTheManager(game, camera, player);
            introducingXylophone = new IntroducingXylophone(game, camera, player);
            beethovenCanStay = new BeethovenCanStay(game, camera, player);
            daVinciPaintingScene = new DaVinciPaintingScene(game, camera, player);
            enteringTheMarket = new EnteringTheMarket(game, camera, player);
            meetingNapoleon = new MeetingNapoleon(game, camera, player);
            commencePhaseOne = new CommencePhaseOne(game, camera, player);
            emergencyFootballPractice = new EmergencyFootballPractice(game, camera, player);
            princessAndTheFlute = new PrincessAndTheFlute(game, camera, player);
            janitorCutscene = new JanitorCutscene(game, camera, player);
            chapterOneEnd = new ChapterOneEnd(game, camera, player);
             
            chapterScenes.Add(chOneOpening);
            chapterScenes.Add(gettingQuestOne);
            chapterScenes.Add(princessCutscene);
            chapterScenes.Add(ghostHuntersScene);
            chapterScenes.Add(ghostLockGone);
            chapterScenes.Add(beethovenCanHearScene);
            chapterScenes.Add(introducingTheManager);
            chapterScenes.Add(introducingXylophone);
            chapterScenes.Add(beethovenCanStay);
            chapterScenes.Add(daVinciPaintingScene);
            chapterScenes.Add(enteringTheMarket);
            chapterScenes.Add(meetingNapoleon);
            chapterScenes.Add(commencePhaseOne);
            chapterScenes.Add(emergencyFootballPractice);
            chapterScenes.Add(princessAndTheFlute);
            chapterScenes.Add(janitorCutscene);
            chapterScenes.Add(chapterOneEnd);

            //Change to cutscene to play scene
            state = GameState.Cutscene;
            cutsceneState = 0;
            synopsis = "";
        }

        public override void Update()
        {
            //TODO TEMPORARY STUFF DELETE
            NorthHall.ToScienceIntroRoom.ItemNameToUnlock = null;
            game.MapBooleans.prologueMapBooleans["targets2Added"] = true;
#if DEBUG
            if (current.IsKeyUp(Keys.P) && last.IsKeyDown(Keys.P))
                player.Experience = player.ExperienceUntilLevel;

            if (current.IsKeyUp(Keys.M) && last.IsKeyDown(Keys.M))
            {
                //player.EquippedSkills[1].Experience = player.EquippedSkills[1].ExperienceUntilLevel;
            }
#endif
            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }
            if (!player.LevelingUp && player.playerState != Player.PlayerState.dead)
            {
                base.Update();

                cursor.Update();
                AddNPCs();
                game.SideQuestManager.AddNPCs();

                //--Remove the janitor's closet portal if it is there
                if (EastHall.ToJanitorsCloset.IsUseable)
                {
                    EastHall.ToJanitorsCloset.IsUseable = false;
                    NorthHall.ToGymLobby.IsUseable = true;
                    NorthHall.ToUpstairsRight.IsUseable = true;
                    MainLobby.ToSouthHall.IsUseable = true;
                    NorthHall.ToUpstairsLeft.IsUseable = true;
                    EastHall.ToKitchen.IsUseable = true;
                }
                switch (state)
                {

                    case GameState.Game:
                        UpdateNPCs();
                        game.SideQuestManager.Update();

                        #region In Game Dialogue
                        //--Alan when you approach them for the first quest
                        if (chapterOneBooleans["alanApproached"] == false && CurrentMap == Game1.schoolMaps.maps["North Hall"] && Vector2.Distance(player.Position, alan.Position) < 2000)
                        {
                           // Chapter.effectsManager.AddInGameDialogue("--in a mud puddle or something.", "Alan", "Normal", 200);
                            chapterOneBooleans["alanApproached"] = true;
                        }
                        #endregion

                        #region Story Quests
                        if (communicatingWithBeethoven.CompletedQuest && beethoven.CurrentDialogueFace != "Horn")
                            beethoven.CurrentDialogueFace = "Horn";

                        if (beethoven.Quest == null && chapterOneBooleans["playedPrincessScene"] && chapterOneBooleans["spawnedGhostHunters"] == false)
                        {
                            beethoven.AddQuest(communicatingWithBeethoven);
                        }

                        if (communicatingWithBeethoven.CompletedQuest == true && dealingWithManagement.CompletedQuest == false && beethoven.Quest == null)
                        {
                            beethoven.AddQuest(dealingWithManagement);
                        }

                        if (dealingWithManagement.CompletedQuest == true && fundRaising.CompletedQuest == false && beethoven.Quest == null)
                        {
                            beethoven.AddQuest(fundRaising);
                        }

                        if (daddysLittlePrincess.CompletedQuest && princess.Talking && princess.Quest == daddysLittlePrincess)
                        {
                            daddysLittlePrincess.RewardPlayer();
                            game.CurrentQuests.Remove(daddysLittlePrincess);
                            Game1.questHUD.RemoveQuestFromHelper(daddysLittlePrincess);
                            princess.RemoveQuest(daddysLittlePrincess);
                            princess.ClearDialogue();
                            princess.AddQuest(musicForAPrincess);
                            princess.Talking = true;
                            talkingToNPC = true;
                        }
                        if (chapterOneBooleans["addedNapoleon"] && wheredTheCrowdGo.CompletedQuest == false && beethoven.Quest == null)
                        {
                            beethoven.AddQuest(wheredTheCrowdGo);
                        }
                        #endregion

                        //Opens up the vents upstairs
                        if (learningAboutMaps.CompletedQuest && chapterOneBooleans["completedMapsQuest"] == false)
                        {
                            chapterOneBooleans["completedMapsQuest"] = true;
                        }

                        #region Starting cutscenes when an NPC begins talking
                        //--Start the Quest One Cutscene when you talk to paul or alan at the start of the chapter
                        if (chapterOneBooleans["questOneSceneStarted"] == false && (paul.Talking || alan.Talking))
                        {
                            chapterOneBooleans["questOneSceneStarted"] = true;
                            paul.Talking = false;
                            alan.Talking = false;
                            state = GameState.Cutscene;
                        }

                        //--Start the Quest One Cutscene when you talk to paul or alan at the start of the chapter
                        if (chapterOneBooleans["spawnedGhostHunters"] && chapterOneBooleans["ghostHuntersMet"] == false && (jason.Talking || claire.Talking))
                        {
                            chapterOneBooleans["ghostHuntersMet"] = true;
                            jason.Talking = false;
                            claire.Talking = false;
                            state = GameState.Cutscene;
                        }

                        //--Start the beethoven can hear scene after you talk to him with the ear horn
                        if (chapterOneBooleans["beethovenCanHearSceneStarted"] == false && (beethoven.Talking) && beethoven.Quest == dealingWithManagement)
                        {
                            chapterOneBooleans["beethovenCanHearSceneStarted"] = true;
                            beethoven.Talking = false;
                            state = GameState.Cutscene;
                        }

                        if (game.ChapterOne.ChapterOneBooleans["passedTrenchcoat"] && !game.ChapterOne.ChapterOneBooleans["forcedTrenchcoatDecision"] && trenchcoatMarket.Talking && currentMap.MapName.Equals("Art Gallery"))
                        {
                            player.VelocityX = 0;
                            player.ImplementGravity();
                            player.UpdatePosition();
                            player.Landing = false;
                            player.standingBackUp = false;
                            player.drawStandingBackUpLines = false;
                        }
                        if (game.ChapterOne.ChapterOneBooleans["passedTrenchcoat"] && game.ChapterOne.ChapterOneBooleans["forcedTrenchcoatDecision"] && trenchcoatMarket.Talking && !game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"] && !game.ChapterOne.ChapterOneBooleans["soldPaintingToArtDealer"] && currentMap.MapName.Equals("Art Gallery"))
                        {
                            talkingToNPC = false;
                            trenchcoatMarket.Talking = false;
                            effectsManager.foregroundFButtonRecs.Clear();
                            makingDecision = true;
                            decisions = Decisions.trenchcoatMarket;
                        }

                        //--Start the meeting napoleon scene
                        if (chapterOneBooleans["meetingNapleonSceneStarted"] == false && napoleon != null && (napoleon.Talking))
                        {
                            chapterOneBooleans["meetingNapleonSceneStarted"] = true;
                            napoleon.Talking = false;
                            state = GameState.Cutscene;
                        }

                        //--Start the meeting napoleon scene
                        if (chapterOneBooleans["meetingNapleonSceneStarted"] && chapterOneBooleans["commencePhaseOneSceneStarted"] == false && currentMap.MapName == "South Hall")
                        {
                            chapterOneBooleans["commencePhaseOneSceneStarted"] = true;
                            chapterOneBooleans["homecomingHypeStarted"] = true;
                            state = GameState.Cutscene;

                            NorthHall.ToHistoryIntroRoom.IsUseable = false;
                        }

                        #endregion
                        //--Checked the art hall
                        if (CurrentMap == Game1.schoolMaps.maps["East Hall"] && chapterOneBooleans["checkedArtHall"] == false && chapterOneBooleans["questOneSceneStarted"] == true)
                            chapterOneBooleans["checkedArtHall"] = true;
                        
                        //--Add the NPCs in the main lobby
                        if (chapterOneBooleans["checkedArtHall"] == true && chapterOneBooleans["addedMainLobbyNPCs"] == false)
                        {
                            chapterOneBooleans["addedMainLobbyNPCs"] = true;
                        }

                        //Get paul and alan out of the North Hall for this section. We use paul2 and alan2 instead
                        if (chapterOneBooleans["homecomingHypeStarted"] && !chapterOneBooleans["homecomingHypeEnded"] && paul2 != null)
                        {
                            if (paul.MapName != "")
                            {
                                paul.MapName = "";
                                alan.MapName = "";
                            }

                            if (paul2.Talking && !chapterOneBooleans["paulTalkedTo"])
                                chapterOneBooleans["paulTalkedTo"] = true;
                            if (alan2.Talking && !chapterOneBooleans["alanTalkedTo"])
                                chapterOneBooleans["alanTalkedTo"] = true;
                            if (death.Talking && !chapterOneBooleans["deathTalkedTo"])
                                chapterOneBooleans["deathTalkedTo"] = true;
                            if (quarterback.Talking && !chapterOneBooleans["chadTalkedTo"])
                                chapterOneBooleans["chadTalkedTo"] = true;
                            if (saveInstructor.PositionX != 2768)
                            {
                                saveInstructor.PositionX = 2768;
                                saveInstructor.PositionY = 274;
                                saveInstructor.RecX = 2768;
                                saveInstructor.RecY = 274;
                                saveInstructor.UpdateRecAndPosition();
                            }
                            if (saveInstructor.MapName != "North Hall")
                            {
                                saveInstructor.MapName = "North Hall";
                                saveInstructor.ClearDialogue();
                                saveInstructor.Dialogue.Add("Wow! That's him! It's Chad Champson, the quarterback of the Water Falls Salmon!");
                                saveInstructor.Dialogue.Add("You know, 'Dwarves and Druids' might take a lot of talent and effort to play, but being the quarterback of a football team is a whole different boss to tackle!");
                                saveInstructor.Dialogue.Add("You can't just start over if you make a bad throw! I wish I could be that confident in my abilities.");
                                saveInstructor.PositionX = 2768;
                                saveInstructor.PositionY = 274;
                                saveInstructor.RecX = 2768;
                                saveInstructor.RecY = 274;
                                saveInstructor.UpdateRecAndPosition();
                            }
                        }
                        else if (chapterOneBooleans["homecomingHypeStarted"] && chapterOneBooleans["homecomingHypeEnded"] && paul.MapName != "North Hall")
                        {
                            SwitchOutOfHomecomingHypeState();
                        }

                        //--Decision making
                        #region Decisions
                        if (makingDecision)
                        {
                            switch (decisions)
                            {
                                case Decisions.trenchcoatMarket:
                                    int num = effectsManager.UpdateDecision("So how about it? Ten bucks for the cheap art. Ain't gonna find a better deal. I'll even give you a discount on my own stock.", "Trenchcoat Employee");

                                    if (num == 1)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("Pleasure doin' business. Now that we got that outta the way, why not check out what I'm sellin'? I'll even give you a discount.", "Trenchcoat Employee", "Normal", 250);
                                        makingDecision = false;
                                        decisions = Decisions.none;
                                        chapterOneBooleans["soldPaintingToTrenchcoat"] = true;
                                        trenchcoatMarket.canShop = true;
                                        trenchcoatMarket.ClearDialogue();
                                        trenchcoatMarket.Dialogue.Add("That painting is gonna look real nice above my firepit. You earned yourself a discount.");
                                        player.RemoveStoryItem("Old Painting", 1);
                                        player.AddStoryItem("Ten Bucks", "ten American dollars", 1);
                                        artMerchant.ClearDialogue();

                                        trenchcoatMarket.ItemsOnSale.Clear();
                                        trenchcoatMarket.ItemsOnSale.Add(new AccessoryForSale(new VenusOfWillendorf(), 5.50f));

                                        trenchcoatMarket.RestockItemsForSale();

                                        if (chapterOneBooleans["talkedToArtDealer"])
                                            artMerchant.Dialogue.Add("I certainly hope you didn't sell that wonderful painting to that scoundrel over there. He has been scamming artists and reselling their work all day.");
                                        else
                                        {
                                            artMerchant.Dialogue.Add("Mmmm yes, yes, hello. I am very sorry, but I do not have time to speak with you. I'm currently looking for additions to my Da Vinci collection.");
                                            artMerchant.Dialogue.Add("If you know of anyone in possession of a Da Vinci work, please do send him my way. I'd be willing to purchase it for $1,000,000. Negotiable, of course...mmhm, quite.");
                                        }
                                    }
                                    else if (num == 2)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("Suit yourself. I'll be here once you realize no one else is gonna buy that waste of paper from you.", "Trenchcoat Employee", "Normal", 250);
                                        decisions = Decisions.none;
                                        makingDecision = false;
                                    }
                                    break;

                                case Decisions.artDealerMarket:
                                    int num2 = effectsManager.UpdateDecision("Mmhmmm... Is 250 Ducats for this fabulous painting satisfactory? Do we have a deal?", "Percy von Lugsworth");

                                    if (num2 == 1)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("Oooh, splendid! This will make an excellent addition to my collection. As a token of good faith, I will also throw in this small trinket that I purchased earlier today. Aravoir!", "Percy von Lugsworth", "Normal", 250);
                                        makingDecision = false;
                                        decisions = Decisions.none;
                                        chapterOneBooleans["soldPaintingToArtDealer"] = true;
                                        artMerchant.ClearDialogue();
                                        artMerchant.Dialogue.Add("I do believe that this painting will make a fabulous addition to the collection above my fireplace.");
                                        player.RemoveStoryItem("Old Painting", 1);
                                        player.AddStoryItem("250 Ducats", "250 ducats", 1);

                                        trenchcoatMarket.ClearDialogue();
                                        trenchcoatMarket.Dialogue.Add("Not the brightest star, huh? Could have made ten bucks. What'd that fruit give you anyway, useless history money?");
                                        trenchcoatMarket.Dialogue.Add("Well, a customer's a customer. Take a look at what I got. Just remember: you could have had a serious discount.");
                                        trenchcoatMarket.canShop = true;

                                        trenchcoatMarket.ItemsOnSale.Clear();
                                        trenchcoatMarket.ItemsOnSale.Add(new WeaponForSale(new SwordOfOverpoweredSwords(), 10000.00f));
                                        trenchcoatMarket.ItemsOnSale.Add(new HatForSale(new HatOfBrownieSummoning(), 10000.00f));
                                        trenchcoatMarket.ItemsOnSale.Add(new OutfitForSale(new RobeOfChampions(), 10000.00f));
                                        trenchcoatMarket.ItemsOnSale.Add(new AccessoryForSale(new VoiceBox(), 10000.00f));
                                        trenchcoatMarket.ItemsOnSale.Add(new AccessoryForSale(new InfiniteLives(), 10000.00f));

                                        trenchcoatMarket.RestockItemsForSale();

                                        player.AddAccessoryToInventory(new VenusOfWillendorf());
                                        effectsManager.AddFoundItem("Venus of Willendorf", game.EquipmentTextures["Venus of Willendorf"]);
                                    }
                                    else if (num2 == 2)
                                    {
                                        effectsManager.ClearDialogue();
                                        effectsManager.AddInGameDialogue("Ah! You may be talented young man, but don't be arrogant. You will not find many offers greater than my own!", "Percy von Lugsworth", "Normal", 250);
                                        chapterOneBooleans["talkedToArtDealer"] = true;
                                        decisions = Decisions.none;
                                        makingDecision = false;
                                    }
                                    break;
                            }
                        }
                        #endregion

                        if (game.ChapterOne.ChapterOneBooleans["spawnedArtMerchant"] && artMerchant!= null && artMerchant.Talking && !game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"] && !game.ChapterOne.ChapterOneBooleans["soldPaintingToArtDealer"] && currentMap.MapName.Equals("Art Gallery") && artMerchant.DialogueState == artMerchant.Dialogue.Count - 1)
                        {
                            talkingToNPC = false;
                            artMerchant.Talking = false;
                            effectsManager.foregroundFButtonRecs.Clear();
                            makingDecision = true;
                            decisions = Decisions.artDealerMarket;
                            artMerchant.DialogueState = 0;
                        }

                        if (TalkingToNPC == false && makingDecision == false)
                        {
                            #region Starting Cutscenes

                            if (chapterOneBooleans["playedPrincessScene"] == false && currentMap == Game1.schoolMaps.maps["Princess' Room"])
                            {
                                state = GameState.Cutscene;
                                chapterOneBooleans["playedPrincessScene"] = true;
                            }

                            if (chapterOneBooleans["playedPrincessScene"] && inventoryInstructor != null && inventoryInstructor.MapName != "Dwarves and Druids Club")
                            {
                                #region Add two NPCs
                                //--Maps Instructor
                                mapsKid.MapName = "Dwarves & Druids Club";
                                mapsKid.RecX = 398;
                                mapsKid.RecY = 201;
                                mapsKid.PositionX = 398;
                                mapsKid.PositionY = 201;
                                mapsKid.canTalk = false;
                                mapsKid.FacingRight = true;

                                inventoryInstructor.ClearDialogue();
                                inventoryInstructor.MapName = "Dwarves & Druids Club";
                                inventoryInstructor.RecX = 323;
                                inventoryInstructor.RecY = 270;
                                inventoryInstructor.PositionX = 323;
                                inventoryInstructor.PositionY = 270;
                                inventoryInstructor.FacingRight = true;

                                inventoryInstructor.Dialogue.Add("Greetings, weary traveler. Let us find a seat so you may unburden yourself from your travels and stuff.");
                                #endregion
                            }

                            if (game.ChapterOne.ChapterOneBooleans["savedGhostHunters"] && !game.ChapterOne.ChapterOneBooleans["ghostLockGoneScenePlayed"])
                            {
                                game.ChapterOne.ChapterOneBooleans["ghostLockGoneScenePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!game.ChapterOne.ChapterOneBooleans["introducingManagerPlayed"] && currentMap.MapName.Equals("Manager's Office"))
                            {
                                game.ChapterOne.ChapterOneBooleans["introducingManagerPlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!game.ChapterOne.ChapterOneBooleans["introducingXylophonePlayed"] && currentMap.MapName.Equals("Axis of Musical Reality"))
                            {
                                game.ChapterOne.ChapterOneBooleans["introducingXylophonePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!game.ChapterOne.ChapterOneBooleans["beethovenCanStayedPlayed"] && game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"] && currentMap.MapName.Equals("Manager's Office"))
                            {
                                game.ChapterOne.ChapterOneBooleans["beethovenCanStayedPlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (!game.ChapterOne.ChapterOneBooleans["marketScenePlayed"] && currentMap.MapName.Equals("Art Gallery") && game.ChapterOne.ChapterOneBooleans["spawnedArtMerchant"])
                            {
                                game.ChapterOne.ChapterOneBooleans["marketScenePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (game.ChapterOne.ChapterOneBooleans["spawnedArtMerchant"] && trenchcoatMarket != null && player.VitalRec.Center.X > trenchcoatMarket.Rec.Center.X + 50 && !game.ChapterOne.ChapterOneBooleans["passedTrenchcoat"] && currentMap.MapName.Equals("Art Gallery") && state == GameState.Game)
                            {
                                game.ChapterOne.ChapterOneBooleans["passedTrenchcoat"] = true;
                                talkingToNPC = true;
                                trenchcoatMarket.canTalk = true;
                                trenchcoatMarket.Talking = true;
                            }

                            if (!game.ChapterOne.ChapterOneBooleans["forcedTrenchcoatDecision"] && game.ChapterOne.ChapterOneBooleans["passedTrenchcoat"] && trenchcoatMarket.Talking == false && currentMap.MapName.Equals("Art Gallery"))
                            {
                                game.ChapterOne.ChapterOneBooleans["forcedTrenchcoatDecision"] = true;
                                trenchcoatMarket.ClearDialogue();
                                trenchcoatMarket.Dialogue.Add("So how about it? Ten bucks for the cheap art. Ain't gonna find a better deal.");
                            }

                            if (chapterOneBooleans["paulTalkedTo"] && chapterOneBooleans["alanTalkedTo"] && chapterOneBooleans["deathTalkedTo"] && chapterOneBooleans["chadTalkedTo"] && !chapterOneBooleans["footballSceneStarted"])
                            {
                                chapterOneBooleans["footballSceneStarted"] = true;
                                state = GameState.Cutscene;
                            }

                            if (napoleon != null && napoleon.MapName == "Napoleon's Tent" && napoleon.PositionX != 680)
                            {
                                napoleon.RecX = 680;
                                napoleon.RecY = 310;
                                napoleon.PositionX = 680;
                                napoleon.PositionY = 310;
                            }

                            if (chapterOneBooleans["protectCampQuestComplete"] && !chapterOneBooleans["princessFluteCutscenePlayed"] && currentMap == Game1.schoolMaps.maps["Princess' Room"])
                            {
                                chapterOneBooleans["princessFluteCutscenePlayed"] = true;
                                state = GameState.Cutscene;
                            }

                            if (chapterOneBooleans["princessFluteCutscenePlayed"] && !chapterOneBooleans["janitorCutscenePlayed"] && currentMap == Game1.schoolMaps.maps["Janitor's Closet"])
                            {
                                chapterOneBooleans["janitorCutscenePlayed"] = true;
                                state = GameState.Cutscene;
                            }
                            #endregion

                            player.Update();
                            hud.Update();
                            currentMap.Update();

                            camera.Update(player, game, currentMap);
                            player.Enemies = currentMap.EnemiesInMap;

                            #region NPCs Wander
                            if ((nPCs["Alan"].Quest == returningKeys && game.CurrentQuests.Contains(returningKeys) && !nPCs["Alan"].Quest.CompletedQuest))
                            {
                                nPCs["Paul"].Wander(2400, 2850);
                                nPCs["Alan"].Wander(2250, 2750);
                            }
                            else
                            {
                                nPCs["Paul"].moveState = NPC.MoveState.standing;
                                nPCs["Alan"].moveState = NPC.MoveState.standing;
                            }

                            #endregion

                            #region Map change stuff
                            String checkPortal = currentMap.CheckPortals();
                            //--Change states to start the fade out
                            if (checkPortal != "null")
                            {
                                nextMap = checkPortal;
                                state = GameState.ChangingMaps;
                            }
                            #endregion

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

            if (game.saveData.chapterOneNPCWrappers != null)
            {
                for (int i = 0; i < game.saveData.chapterOneNPCWrappers.Count; i++)
                {
                    if (name == game.saveData.chapterOneNPCWrappers[i].npcName)
                    {
                        if (game.saveData.chapterOneNPCWrappers[i].questName != null)
                        {
                            npc.Dialogue = game.saveData.chapterOneNPCWrappers[i].dialogue;
                            npc.QuestDialogue = game.saveData.chapterOneNPCWrappers[i].questDialogue;
                            npc.DialogueState = game.saveData.chapterOneNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterOneNPCWrappers[i].facingRight;
                            npc.Quest = game.AllQuests[game.saveData.chapterOneNPCWrappers[i].questName];
                            npc.AcceptedQuest = game.saveData.chapterOneNPCWrappers[i].acceptedQuest;
                            npc.MapName = game.saveData.chapterOneNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterOneNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterOneNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterOneNPCWrappers[i].positionY;
                        }

                        else if (game.saveData.chapterOneNPCWrappers[i].trenchCoat == false)
                        {
                            npc.Dialogue = game.saveData.chapterOneNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.chapterOneNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterOneNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            npc.MapName = game.saveData.chapterOneNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterOneNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterOneNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterOneNPCWrappers[i].positionY;
                        }
                        else
                        {
                            npc.Dialogue = game.saveData.chapterOneNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.chapterOneNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.chapterOneNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            (npc as TrenchcoatKid).SoldOut = game.saveData.chapterOneNPCWrappers[i].trenchcoatSoldOut;
                            npc.MapName = game.saveData.chapterOneNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.chapterOneNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.chapterOneNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.chapterOneNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.chapterOneNPCWrappers[i].positionY;
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
                alan = new NPC(Game1.whiteFilter, dialogue1, returningKeys, new Rectangle(2700, 270, 516, 388),
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

            if (!nPCs.ContainsKey("TrenchcoatCronyVents"))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("You're the first customer I've seen in ages.");
                List<ItemForSale> items = new List<ItemForSale>();
                items.Add(new KeyForSale(4.00f, KeyForSale.KeyType.Silver));
                items.Add(new AccessoryForSale(new GarlicNecklace(), 3.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                items.Add(new StoryItemForSale(new Coal(false), 1.00f));
                trenchcoatVents = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 100, 270, player, game, "Upper Vents V", items);
                trenchcoatVents.FacingRight = true;
                AddNPC("TrenchcoatCronyVents", trenchcoatVents);
            }



            if (!nPCs.ContainsKey("Karma Shaman"))
            {
                //--Karma Instructor
                List<String> dialogueKarma = new List<string>();
                dialogueKarma.Add("Welcome almighty \"" + player.SocialRank + "\", to the the High Guild Hall, home of all things \"Dwarves and Druids\"! Perhaps you are seeking new quests or a beautiful wench to satisfy your boredom?");
                dialogueKarma.Add("Step inside, and let my brothers whisk away your aches and worries with endless ale and roasted boar! And may good fortune follow you.");
                karmaShaman = new NPC(game.NPCSprites["Karma Shaman"], dialogueKarma,
                    new Rectangle(1255, 270, 516, 388), player, game.Font, game, "South Hall", "Karma Shaman", false);
                karmaShaman.FacingRight = true;
                AddNPC("Karma Shaman", karmaShaman);


                #region Add skill instructor and box enemy
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogueSkill = new List<string>();
                dialogueSkill.Add("Are you here to hone your skills? If you'd like I can conjure up a new hat for you to fight, but they've been extra mean lately. And some of them have bombs.");
                skillSorceress = new NPC(game.NPCSprites["Skill Sorceress"], dialogueSkill,
                    new Rectangle(1011, 287, 516, 388), player, game.Font, game, "Dwarves & Druids Club", "Skill Sorceress", false);

                AddNPC("Skill Sorceress", skillSorceress);
                #endregion

                List<String> gokuDia = new List<string>();
                gokuDia.Add("I wish I could get upstairs to that new vent dungeon. The elevator has been broken for weeks now, and last time the Weapons Master carried me up there everyone forgot about me.");
                gokuDia.Add("I heard an evil vampire spirit lives in there! I'm probably missing out on great loot.");
                saveInstructor = new NPC(game.NPCSprites["Saving Instructor"], gokuDia, new Rectangle(1246, 249, 516, 388), player, game.Font, game, "Dwarves & Druids Club", "Saving Instructor", false);
                saveInstructor.FacingRight = true;
                AddNPC("Saving Instructor", saveInstructor);
            }
            if (!nPCs.ContainsKey("The Princess"))
            {
                //--Princess
                List<String> dialogue = new List<string>();
                dialogue.Add("UGH. Why are you still here??");
                princess = new NPC(game.NPCSprites["The Princess"], dialogue, new Rectangle(420, -60, 516, 388),
                    player, game.Font, game, "Princess' Room", "The Princess", false);
                AddNPC("The Princess", princess);
            }

            if (!nPCs.ContainsKey("The Janitor"))
            {
                //--Princess
                List<String> dialogue = new List<string>();
                dialogue.Add("I have a cutscene");
                janitor = new NPC(game.NPCSprites["The Janitor"], dialogue, new Rectangle(670, 270, 516, 388),
                    player, game.Font, game, "Janitor's Closet", "The Janitor", false);
                janitor.FacingRight = false;
                AddNPC("The Janitor", janitor);
            }

            if (!nPCs.ContainsKey("Beethoven"))
            {
                //--Princess
                List<String> dialogue = new List<string>();
                dialogue.Add("WHAT?");
                beethoven = new NPC(game.NPCSprites["Beethoven"], dialogue, new Rectangle(700, 290, 516, 388),
                    player, game.Font, game, "The Stage", "Beethoven", false);
                AddNPC("Beethoven", beethoven);
            }

            if (!nPCs.ContainsKey("Journal Enthusiast"))
            {
                //--Maps Instructor
                List<String> journalDialogue = new List<string>();
                journalDialogue.Add("Wowee, this here journal sure is exciting. Not only does it keep track of everything I do, but it's darn-right funny to boot!");
                journalDialogue.Add("I sure do feel sorry for those nice people who aren't reading their journal.");
                journalEnthusiast = new NPC(game.NPCSprites["Journal Enthusiast"], journalDialogue,
                    new Rectangle(2300, 660 - 388, 516, 388), player, game.Font, game, "South Hall", "Journal Enthusiast", false);

                AddNPC("Journal Enthusiast", journalEnthusiast);
            }

            if (!nPCs.ContainsKey("Tchaikovsky"))
            {
                //--Maps Instructor
                List<String> tchaikovskyDialogue = new List<string>();

                tchaikovskyDialogue.Add("*sigh* You look absolutely ridiculous. Do you need something? If not, please, I'd rather be alone.");
                tchaikovskyDialogue.Add("Oh...wait. Look, I'm sorry you had to catch me like this. See, I just read in an encycopedia that I don't have much longer to live...");
                tchaikovskyDialogue.Add("I only have until I'm 53 to turn my life around until cholera does me in. Possibly self-inflicted! Oh what a garbage life I'll have led... I'll be remembered for Christmas jingles. And video game cameos.");
                tchaikovskyDialogue.Add("Kind of hard to turn one's life around when they're locked in a room. I'm not allowed to leave until I write that punk downstairs a \"Top 40 hit\". I don't even know what that means.");
                tchaikovskyDialogue.Add("Anyway, I don't feel like looking through any more encyclopedias.");

                tchaikovsky = new NPC(game.NPCSprites["Tchaikovsky"], tchaikovskyDialogue,
                    new Rectangle(600, 660 - 388, 516, 388), player, game.Font, game, "Tchaikovsky's Room", "Tchaikovsky", false);
                tchaikovsky.FacingRight = false;
                AddNPC("Tchaikovsky", tchaikovsky);
            }

            if (!nPCs.ContainsKey("TrenchcoatTheater"))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Lotta band nerds around here. Looks like you might wanna join the club.");
                List<ItemForSale> items = new List<ItemForSale>();
                items.Add(new WeaponForSale(new ComposersWand(), 8.00f));
                items.Add(new HatForSale(new BandHat(), 8));
                items.Add(new OutfitForSale(new BandUniform(), 8));
                trenchcoatTheater = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 700, 630, player, game, "Tenant Room #4", items);
                trenchcoatTheater.FacingRight = false;
                AddNPC("TrenchcoatTheater", trenchcoatTheater);
            }

            if (!nPCs.ContainsKey("Theater Manager"))
            {
                //--Maps Instructor
                List<String> managerDialogue = new List<string>();
                managerDialogue.Add("");
                theaterManager = new NPC(game.NPCSprites["Theater Manager"], managerDialogue,
                    new Rectangle(904, 294, 516, 388), player, game.Font, game, "Manager's Office", "Theater Manager", false);
                theaterManager.FacingRight = false;
                AddNPC("Theater Manager", theaterManager);
            }

            if (!nPCs.ContainsKey("Portal Repair Specialist Bridge"))
            {
                List<String> d = new List<string>();
                d.Add("Hmmm...another one. Most peculiar.");
                portalRepairBridge = new NPC(game.NPCSprites["Portal Repair Specialist"], d, portalRepairman,
                    new Rectangle(1435, 264, 516, 388), player, game.Font, game, "Bridge of Armanhand", "Portal Repair Specialist", false);
                AddNPC("Portal Repair Specialist Bridge", portalRepairBridge);
            }

            if (!nPCs.ContainsKey("InventoryInstructor") && chapterOneBooleans["addedMainLobbyNPCs"] == true)
            {
                #region Add two NPCs
                //--Maps Instructor
                List<String> dialogueKarma = new List<string>();
                dialogueKarma.Add("I bet there are a ton of epic monsters in those vents!");
                dialogueKarma.Add("I doubt that they actually lead to anywhere cool though. Probably just to the janitor's closet or something.");
                mapsKid = new NPC(game.NPCSprites["The Magister of Maps"], dialogueKarma, learningAboutMaps,
                    new Rectangle(800, 630 - 388, 516, 388), player, game.Font, game, "Main Lobby", "The Magister of Maps", false);

                AddNPC("The Magister of Maps", mapsKid);

                //--Inventory Instructor
                List<String> dialogueEquipment = new List<string>();
                dialogueEquipment.Add("Only the most powerful of my weapons can aid me through the terrible trials that lurk in the vents upstairs.");
                dialogueEquipment.Add("I will be sure to bring ample Weapon Potion to grow better weapons.");
                inventoryInstructor = new NPC(game.NPCSprites["Weapons Master"], dialogueEquipment,
                    new Rectangle(1050, 630 - 388, 516, 388), player, game.Font, game, "Main Lobby", "Weapons Master", false);
                inventoryInstructor.FacingRight = false;
                AddNPC("InventoryInstructor", inventoryInstructor);
                #endregion
            }

            if (!nPCs.ContainsKey("Jason Mysterio") && chapterOneBooleans["spawnedGhostHunters"] == true)
            {
                List<String> d = new List<string>();
                d.Add("");
                jason = new NPC(game.NPCSprites["Jason Mysterio"], d,
                    new Rectangle(2253, 293, 516, 388), player, game.Font, game, "Tenant Hallway West", "Jason Mysterio", false);
                jason.FacingRight = false;
                AddNPC("Jason Mysterio", jason);

                List<String> d2 = new List<string>();
                d2.Add("");
                claire = new NPC(game.NPCSprites["Claire Voyant"], d2,
                    new Rectangle(1691, 293, 516, 388), player, game.Font, game, "Tenant Hallway West", "Claire Voyant", false);
                claire.FacingRight = false;
                AddNPC("Claire Voyant", claire);
            }

            if (chapterOneBooleans["finishedManagerQuest"] && jason != null && jason.MapName != "Paranormal Club")
            {
                jason.ClearDialogue();
                jason.Dialogue.Add("Sorry kiddo, we don't have time for any puny ghost that might be haunting you. This Lock Ghost is going to make us famous!");
                jason.MapName = "Paranormal Club";
                jason.RecX = 1322;
                jason.RecY = 278;
                jason.PositionX = 1322;
                jason.PositionY = 278;
                jason.FacingRight = true;

                claire.ClearDialogue();
                claire.Dialogue.Add("I hope your brain is feeling better now that it has had ample time to recover from my psychic touch. It appears that I don't know my own strength; controlling your mind was much easier than mind control used to be for me.");
                claire.MapName = "Paranormal Club";
                claire.RecX = 565;
                claire.RecY = 258;
                claire.PositionX = 565;
                claire.PositionY = 258;
                claire.FacingRight = true;

                List<String> d = new List<string>();
                d.Add("Did you like seeing the Ghost Sucker in action? I built it myself. Jason doesn't really appreciate the genius that goes into my inventions, but I can tell you saw the merit in it.");
                ken = new NPC(game.NPCSprites["Ken Speercy"], d,
                    new Rectangle(965, 218, 516, 388), player, game.Font, game, "Paranormal Club", "Ken Speercy", false);
                ken.FacingRight = true;
                AddNPC("Ken Speercy", ken);

                List<String> d2 = new List<string>();
                d2.Add("I can't believe they're keeping that ghost sealed up in that machine! In here! With -US-!");
                d2.Add("I think I can hear it from over here...");
                steve = new NPC(game.NPCSprites["Steve Pantski"], d2,
                    new Rectangle(238, 240, 516, 388), player, game.Font, game, "Paranormal Club", "Steve Pantski", false);
                steve.FacingRight = true;
                AddNPC("Steve Pantski", steve);
            }

            if (!nPCs.ContainsKey("Art Merchant") && chapterOneBooleans["spawnedArtMerchant"] == true)
            {
                List<String> d = new List<string>();
                d.Add("");
                artMerchant = new NPC(game.NPCSprites["Percy von Lugsworth"], d,
                    new Rectangle(2000, 293, 516, 388), player, game.Font, game, "Art Gallery", "Percy von Lugsworth", false);
                artMerchant.FacingRight = false;
                AddNPC("Art Merchant", artMerchant);

                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Pssssst...");
                cronydialogue.Add("Hey... yeah you.");
                cronydialogue.Add("I'll give you ten bucks for that painting you got there.");
                List<ItemForSale> items = new List<ItemForSale>();

                trenchcoatMarket = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 1180, 630, player, game, "Art Gallery", items);
                trenchcoatMarket.FacingRight = true;
                trenchcoatMarket.canTalk = false;
                trenchcoatMarket.canShop = false;
                trenchcoatMarket.PositionY = 270;
                trenchcoatMarket.UpdateRecAndPosition();
                AddNPC("TrenchcoatMarket", trenchcoatMarket);
            }

            if (chapterOneBooleans["addedNapoleon"] && !nPCs.ContainsKey("Napoleon"))
            {
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("This dialogue does not matter because I have a have a cutscene");
                napoleon = new NPC(game.NPCSprites["Napoleon"], dialogue2, new Rectangle(1240, 660 - 388, 516, 388), player, game.Font, game, "Town Square", "Napoleon", false);
                napoleon.FacingRight = true;
                AddNPC("Napoleon", napoleon);

                List<String> d = new List<string>();
                d.Add("You should not be outside. Zis area is under lock down!");
                frenchSoldier = new NPC(game.NPCSprites["French Soldier"], d, new Rectangle(1440, 630 - 388, 516, 388), player, game.Font, game, "Town Square", "French Soldier", false);
                frenchSoldier.FacingRight = false;
                AddNPC("French Soldier", frenchSoldier);
            }

            if (chapterOneBooleans["homecomingHypeStarted"] && !nPCs.ContainsKey("Chad Champson"))
            {
                List<String> qbDialogue = new List<string>();
                qbDialogue.Add("Ha, hey there. You must be another fan. What's up? Name's Chad Champson. I'm the quarterback of the Water Falls Salmon. It's a pleasure to meet me.");
                qbDialogue.Add("I'm sure you have a lot of questions for me like, \"How do you plan on besting the Stony Brook Bears tomorrow at Homecoming?\" or, \"Have the New England Patriots recruited you yet?\" or, \"Can I have your autograph?\".");
                qbDialogue.Add("Well, I'll tell you. We may have had a pretty rough season so far, I won't deny it. But we've been practicing pretty hard and I really feel the team's energy out on the field. I think as long as we play better than the Bears we'll be able to score some touchdowns, get some points, and win. You dig?");
                qbDialogue.Add("As for the Patriots, well, I haven't heard back after I sent them my try-out video, but I imagine that once they need a new quarterback they'll be getting back to me. They're probably just shifting some money around to make room for my salary.");
                qbDialogue.Add("And no, you can't have my autograph. After I end our losing streak tomorrow those things are going to skyrocket in price, and I can't just hand them out willy-nilly.");
                qbDialogue.Add("Now if you don't mind, there are a lot of other fans here that want to talk to me.");
                quarterback = new NPC(game.NPCSprites["Chad Champson"], qbDialogue, new Rectangle(1907, 254, 516, 388), player, game.Font, game, "North Hall", "Chad Champson", false);
                quarterback.FacingRight = true;
                AddNPC("Chad Champson", quarterback);

                List<String> paul2Dialogue = new List<string>();
                paul2Dialogue.Add("Where the hell have you been? It's almost the end of the day and we haven't seen a drop of Book Spray. Homecoming is tomorrow, and we need to be ready to open shop.");
                paul2Dialogue.Add("We're playing our rivals, the Stony Brook Bears, and it's going to be packed. You don't have time to gawk at the football team, no matter how amazing their losing streak is.");
                paul2Dialogue.Add("We're better than these losers, Daniel. You get us that Book Spray and I guarantee we'll be the ones pulling crowds around here tomorrow.");
                paul2 = new NPC(game.NPCSprites["Paul"], paul2Dialogue, new Rectangle(1482, 660 - 388, 516, 388), player, game.Font, game, "North Hall", "Paul", false);
                paul2.FacingRight = true;
                AddNPC("Paul2", paul2);

                List<String> alan2Dialogue = new List<string>();
                alan2Dialogue.Add("Hey Derek, check this out. It's our football team, the Water Falls Salmon. Well, three of them at least. That guy in the middle there is our quarterback, Chad Champson.");
                alan2Dialogue.Add("He's famously terrible. They featured him on the news last year for his world-record completion rate of -5.3%. They say our team hasn't won a game in decades.");
                alan2Dialogue.Add("There's actually a pretty big gambling scene here that revolves around him. Like the number of times he'll accidentally hand the ball off to the opposing team during a game, or how many minutes in it will be before he pretends to break his ankle so he can go home.");
                alan2Dialogue.Add("Yeah, it's pretty sad. I imagine you want to be on the other end of the social spectrum here, so I suggest you go get us our goddamn Book Spray before we sign you up to be the next quarterback. ");
                alan2 = new NPC(game.NPCSprites["Alan"], alan2Dialogue, new Rectangle(2160, 660 - 388, 516, 388), player, game.Font, game, "North Hall", "Alan", false);
                alan2.FacingRight = false;
                AddNPC("Alan2", alan2);

                List<String> deathDia = new List<string>();
                deathDia.Add("Oh, hey there champ. You getting in on the betting here, too? Homecoming's tomorrow, and it sees the highest bets of the year.");
                deathDia.Add("I make a fortune off these kids. Gambling's pretty easy when you can see through time. BAHAHA! Yep, they never learn.");
                deathDia.Add("You folks have quite the team here, I gotta admit. Each game almost keeps me too busy to deal with whatever new pets the gardener has found and subsequently lost. I swear the helmets these kids use are made of cardboard.");
                deathDia.Add("Hey, tell ya what, go ahead and throw some money down on the quarterback being spun around, becoming confused, and running for a touchdown against his own team. Should happen in the second quarter.");
                deathDia.Add("Ha...yeah. I love this school.");
                death = new NPC(game.NPCSprites["Death"], deathDia, new Rectangle(2474, 280, 516, 388), player, game.Font, game, "North Hall", "Death", false);
                death.FacingRight = false;
                AddNPC("Death2", death);
            }
        }

        public void PlayGhostSuckedDestroyed()
        {
            chapterScenes.Add(ghostSuckerDestroyed);
            cutsceneState = chapterScenes.Count - 1;
            state = GameState.Cutscene;
        }

        public void SwitchOutOfHomecomingHypeState()
        {
            paul.MapName = "North Hall";
            alan.MapName = "North Hall";

            if (nPCs["Paul"].RecX != 2880)
            {
                nPCs["Paul"].RecX = 2880;
                nPCs["Paul"].PositionX = 2880;
                nPCs["Paul"].FacingRight = false;
            }
            if (nPCs["Alan"].RecX != 2700)
            {
                nPCs["Alan"].RecX = 2700;
                nPCs["Alan"].PositionX = 2700;
                nPCs["Alan"].FacingRight = true;
            }

            if (saveInstructor.MapName != "Dwarves & Druids Club")
            {
                saveInstructor.MapName = "Dwarves & Druids Club";
                saveInstructor.ClearDialogue();
                saveInstructor.Dialogue.Add("I wish I could get upstairs to that new vent dungeon. The elevator has been broken for weeks now, and last time the Weapons Master carried me up there everyone forgot about me.");
                saveInstructor.Dialogue.Add("I heard an evil vampire spirit lives in there! I'm probably missing out on great loot.");
                saveInstructor.PositionX = 1246;
                saveInstructor.PositionY = 249;
                saveInstructor.RecX = 1246;
                saveInstructor.RecY = 249;
            }

            paul.UpdateRecAndPosition();
            alan.UpdateRecAndPosition();

            paul.Dialogue.Clear();
            paul.Dialogue.Add("It's almost the end of the day. If you don't get us that Book Spray soon we're going to have to reconsider your employment.");
            paul2.MapName = "";
            alan2.MapName = "";
            death.MapName = "";
            quarterback.MapName = "";

            NorthHall.ToHistoryIntroRoom.IsUseable = true;
        }
    }
}
