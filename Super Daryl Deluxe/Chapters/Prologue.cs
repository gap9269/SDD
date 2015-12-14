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
    public class Prologue : Chapter
    {
        NPC alan;
        NPC paul;
        NPC tim;
        TrenchcoatKid trenchcoatEmployee;

        //--Tutorial NPCs
        NPC karmaInstructor;
        NPC inventoryInstructor;
        NPC journalInstructor;
        NPC skillInstructor;
        NPC saveInstructor;

        //--Story quests
        TutorialQuestOne questOne;
        TutorialQuestTwo questTwo;
        TutorialQuestThree questThree;
        TutorialQuestFour questFour;
        TutorialQuestFive questFive;

        //--Side quests
        KarmaQuest karmaQuest;
        InventoryQuest inventoryQuest;
        JournalQuest journalQuest;
        SkillQuest skillQuest;
        SaveQuest saveQuest;

        //--Cutscenes
        PrologueIntroFlashBack introFlashback;
        OpeningScene openingScene;
        Science103Scene science103Scene;
        GettingQuestTwoScene getQuestTwoScene;
        BusinessCutscene businessScene;
        TrenchcoatCutscene trenchcoatScene;
        TimScene timScene;
        PrologueEnd prologueEnd;

        //--Story Quest attributes
        Dictionary<String, Boolean> prologueBooleans;

        int placeFlowersTimer;

        static Random ran = new Random();

        public Boolean CanBreakIntoLockers { get { return prologueBooleans["canBreakIntoLockers"]; } set { prologueBooleans["canBreakIntoLockers"] = value; } }
        public TutorialQuestOne QuestOne { get { return questOne; } }
        public TutorialQuestTwo QuestTwo { get { return questTwo; } }
        public TutorialQuestThree QuestThree { get { return questThree; } }
        public JournalQuest JournalQuest { get { return journalQuest; } set { journalQuest = value; } }
        public Dictionary<String, Boolean> PrologueBooleans { get { return prologueBooleans; } set { prologueBooleans = value; } }

        public Boolean BrokeIntoLocker { get { return prologueBooleans["brokeIntoLocker"]; } }
        public Prologue(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            questOne = new TutorialQuestOne(true);
            questTwo = new TutorialQuestTwo(true);
            questThree = new TutorialQuestThree(true);
            questFour = new TutorialQuestFour(true);
            questFive = new TutorialQuestFive(true);

            karmaQuest = new KarmaQuest(false);
            inventoryQuest = new InventoryQuest(false, game);
            journalQuest = new JournalQuest(false);
            skillQuest = new SkillQuest(false);
            saveQuest = new SaveQuest(false);

            prologueBooleans = new Dictionary<String, bool>();

            game.AllQuests.Add(questOne.QuestName, questOne);
            game.AllQuests.Add(questTwo.QuestName, questTwo);
            game.AllQuests.Add(questThree.QuestName, questThree);
            game.AllQuests.Add(questFour.QuestName, questFour);
            game.AllQuests.Add(questFive.QuestName, questFive);

            game.AllQuests.Add(karmaQuest.QuestName, karmaQuest);
            game.AllQuests.Add(inventoryQuest.QuestName, inventoryQuest);
            game.AllQuests.Add(journalQuest.QuestName, journalQuest);
            game.AllQuests.Add(skillQuest.QuestName, skillQuest);
            game.AllQuests.Add(saveQuest.QuestName, saveQuest);

            prologueBooleans.Add("ratSpawned", false);
            prologueBooleans.Add("brokeIntoLocker", false);
            prologueBooleans.Add("canBreakIntoLockers", false);
            prologueBooleans.Add("checkedPhysicsDoor", false);
            prologueBooleans.Add("updatedKeyDialogue", false);
            prologueBooleans.Add("addedQuestFour", false);
            prologueBooleans.Add("addedKarmaAndInventory", false);
            prologueBooleans.Add("addedSkillInstructor", false);
            prologueBooleans.Add("acceptedQuestFour", false);
            prologueBooleans.Add("addedJournalInstructor", false);
            prologueBooleans.Add("addedGardener", false);
            prologueBooleans.Add("sawGardener", false);
            prologueBooleans.Add("addedTim", false);
            prologueBooleans.Add("addBox", false);
            prologueBooleans.Add("addedBox", false);
            prologueBooleans.Add("ratDead", false);
            prologueBooleans.Add("markerGiven", false);
            prologueBooleans.Add("giveKey", false);
            prologueBooleans.Add("gotKey", false);
            prologueBooleans.Add("gotKey2", false);
            prologueBooleans.Add("gotTextbook", false);
            prologueBooleans.Add("equippedSecondSkill", false);
            prologueBooleans.Add("removeNPCs", false);
            prologueBooleans.Add("finishedRatQuest", false);
            prologueBooleans.Add("buriedRiley", false);
            prologueBooleans.Add("FoundGoggles", false);
            prologueBooleans.Add("PickedUpDrop", false);
            prologueBooleans.Add("skillShopTutorialCompleted", false);

            prologueBooleans.Add("secondSceneNotPlayed", true);
            prologueBooleans.Add("thirdSceneNotPlayed", true);
            prologueBooleans.Add("fourthSceneNotPlayed", true);
            prologueBooleans.Add("fifthSceneNotPlayed", true);
            prologueBooleans.Add("sixthSceneNotPlayed", true);

            //TOOLTIPS FOR NOTEBOOK
            prologueBooleans.Add("firstInventory", true);
            prologueBooleans.Add("firstEquipped", true);
            prologueBooleans.Add("firstCombo", true);
            prologueBooleans.Add("firstJournal", true);
            prologueBooleans.Add("firstJournalChapter", true);
            prologueBooleans.Add("firstJournalSynopsis", true);
            prologueBooleans.Add("firstJournalStoryQuest", true);
            prologueBooleans.Add("firstJournalSideQuest", true);
            prologueBooleans.Add("firstQuestPageQuestCheck", true);

            //TOOLTIPS FOR SHOP
            prologueBooleans.Add("firstTrench", true);
            prologueBooleans.Add("firstTrenchSell", true);
            prologueBooleans.Add("firstTrenchSelectedItem", true);

            //LOCKER TOOLTIP
            prologueBooleans.Add("firstSkillLocker", true);
            prologueBooleans.Add("firstSkillLockerWithSkill", true);
            prologueBooleans.Add("firstShop", true);

            introFlashback = new PrologueIntroFlashBack(game, game.Camera, p, chapterTextures["Flashback"]);
            openingScene = new OpeningScene(game, game.Camera, p, chapterTextures);
            science103Scene = new Science103Scene(game, game.Camera, p);
            getQuestTwoScene = new GettingQuestTwoScene(game, game.Camera, p);
            businessScene = new BusinessCutscene(game, game.Camera, p);
            trenchcoatScene = new TrenchcoatCutscene(game, game.Camera, p);
            timScene = new TimScene(game, game.Camera, p);
            prologueEnd = new PrologueEnd(game, game.Camera, p, chapterTextures["PrologueComplete"]);

            chapterScenes.Add(introFlashback);
            chapterScenes.Add(openingScene);
            chapterScenes.Add(getQuestTwoScene);
            chapterScenes.Add(businessScene);
            chapterScenes.Add(science103Scene);
            chapterScenes.Add(trenchcoatScene);
            chapterScenes.Add(timScene);
            chapterScenes.Add(prologueEnd);

            //Change to cutscene to play scene
            state = GameState.Cutscene;
            cutsceneState = 1;


            synopsis = "Your name is Daryl Whitelaw, and today was your first day at Water Falls High School. You were warmly welcomed by the vice principal, Mr. Robatto, who, while directing you to your new locker, introduced you to two of your peers, Paul and Alan. To allow the seeds of friendship to grow, Robatto left you kids alone. Paul and Alan were reluctant to accept you into their social circle, but after you recovered for them a piece of paper containing their good friend Tim's locker combination, along with a bouquet of dandelions, they decided you were of use after all.";

            game.Notebook.Journal.prologueSynopsisRead = false;

            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Blinding Logic"]);

            //player.EquippedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
            //player.EquippedSkills[0].SkillRank = 3;
            //player.EquippedSkills[0].ApplyLevelUp();
            //player.EquippedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
            //player.EquippedSkills[1].SkillRank = 2;
            //player.EquippedSkills[1].ApplyLevelUp();

           // player.CanJump = true;
        }

        public override void Update()
        {
            //player.AddStoryItemWithoutPopup("Piece of Paper", 1);
            //player.AddStoryItemWithoutPopup("Dandelion", 3);

           // player.Health = 3;
            //player.Health = 0;
            //if (player.LearnedSkills.Count == 2)
            //{
            //    player.Strength = 100;
            //    player.LearnedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
            //}

           // player.EquippedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);

            if (NorthHall.ToGymLobby.IsUseable)
            {
                NorthHall.ToGymLobby.IsUseable = false;
                NorthHall.ToHistoryIntroRoom.IsUseable = false;
                NorthHall.ToUpstairsRight.IsUseable = false;
                MainLobby.ToSouthHall.IsUseable = false;
                NorthHall.ToUpstairsLeft.IsUseable = false;
                EastHall.ToKitchen.IsUseable = false;
            }

            if (questOne.CompletedQuest == false)
            {
                NorthHall.ToBathroom.IsUseable = false;
            }
            else if(!NorthHall.ToBathroom.IsUseable)
            {
                NorthHall.ToBathroom.IsUseable = true;
            }

            cursor.Update();

            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }

            if (!player.LevelingUp && player.playerState != Player.PlayerState.dead)
            {
                base.Update();

                if (current.IsKeyUp(Keys.P) && last.IsKeyDown(Keys.P))
                {
                    //player.EquippedSkills[0].SkillRank++;
                     //player.Experience = player.ExperienceUntilLevel;
                    // player.Karma++;
                }

                AddNPCs();
                game.SideQuestManager.AddNPCs();

                if (skillQuest.CompletedQuest == true)
                {
                    prologueBooleans["addedBox"] = true;
                }

                switch (state)
                {

                    case GameState.Game:

                        #region Unlocking Character Bios
                        if (questOne.CompletedQuest && player.AllCharacterBios["Paul"] == false && !game.CurrentQuests.Contains(questOne))
                        {
                            player.UnlockCharacterBio("Paul");
                            player.UnlockCharacterBio("Alan");
                        }

                        if (questFive.CompletedQuest && player.AllCharacterBios["Trenchcoat Employee"] == false && !game.CurrentQuests.Contains(questFive))
                        {
                            player.UnlockCharacterBio("Trenchcoat Employee");
                        }

                        if (skillQuest.CompletedQuest && player.AllCharacterBios["Skill Instructor"] == false && !game.CurrentSideQuests.Contains(skillQuest))
                            player.UnlockCharacterBio("Skill Instructor");

                        if (saveQuest.CompletedQuest && player.AllCharacterBios["Save Instructor"] == false && !game.CurrentSideQuests.Contains(saveQuest))
                            player.UnlockCharacterBio("Save Instructor");

                        if (inventoryQuest.CompletedQuest && player.AllCharacterBios["Equipment Instructor"] == false && !game.CurrentSideQuests.Contains(inventoryQuest))
                            player.UnlockCharacterBio("Equipment Instructor");

                        if (journalQuest.CompletedQuest && player.AllCharacterBios["Journal Instructor"] == false && !game.CurrentSideQuests.Contains(journalQuest))
                            player.UnlockCharacterBio("Journal Instructor");

                        if (karmaQuest.CompletedQuest && player.AllCharacterBios["Karma Instructor"] == false && !game.CurrentSideQuests.Contains(karmaQuest))
                            player.UnlockCharacterBio("Karma Instructor");
                        #endregion

                        //--Gardener talking to herself
                        if (prologueBooleans["sawGardener"] == false && prologueBooleans["addedGardener"] && CurrentMap == Game1.schoolMaps.maps["East Hall"])
                        {
                            Chapter.effectsManager.AddInGameDialogue("Oh my poor sweet Riley! *sob* Why did this have to happen? *sniff*", "The Gardener", "Normal", 180);

                            prologueBooleans["sawGardener"] = true;
                        }

                        
                        game.SideQuestManager.Update();

                        #region Quests

                        //--If you try to open the locked science door, set the boolean to true
                        if (player.VitalRec.Intersects(NorthHall.ToScienceIntroRoom.PortalRec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                            && questTwo.CompletedQuest && prologueBooleans["checkedPhysicsDoor"] == false)
                        {
                            prologueBooleans["checkedPhysicsDoor"] = true;

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
                        }

                        //--Start the business cutscene when you talk to Paul or Alan
                        if (prologueBooleans["fourthSceneNotPlayed"] && questTwo.CompletedQuest && (paul.Talking || alan.Talking))
                        {
                            synopsis += "\n\nAt the suggestion of Paul and Alan, you placed the beautiful bouquet in Tim's locker to strengthen the bond of friendship between all parties involved. To reward you for the favor, they gave you an old self-help book titled Interacting With Others, which they claimed would help you interact with other students and make more friends. Like good friends, they saved you the trouble of ripping out all the pages. To ensure your continued comradery, you were officially employed by them to collect contraband textbooks for them to sell on the highly competitive textbook market. As payment, you would receive chapters of the self-help book to expand your social interaction horizons, and as Alan claimed, \"become the Fonz in no time.\". To get you started, they graciously gave you a social skill for free and sent you off to the Science Room to clean out the textbook collection they alleged remained from the massive school-wide renovation and peculiar classroom lockdown a year prior.";


                            prologueBooleans["fourthSceneNotPlayed"] = false;
                            paul.Talking = false;
                            alan.Talking = false;
                            state = GameState.Cutscene;
                            game.CurrentChapter.CurrentMap.PlayBackgroundMusic();
                        }

                        if (prologueBooleans["fourthSceneNotPlayed"] == false && state == GameState.Game && currentMap.MapName == "North Hall" && (currentMap.currentBackgroundMusic != Sound.MusicNames.NoirHalls || Sound.currentBackgroundVolume < Sound.setBackgroundVolume))
                        {
                            Sound.ChangeBackgroundMusicWithFade(Sound.MusicNames.NoirHalls, 25);
                        }

                        //--Add Quest 4 to Paul
                        if (prologueBooleans["checkedPhysicsDoor"] && player.EquippedSkills.Count > 0 && prologueBooleans["addedQuestFour"] == false)
                        {
                            prologueBooleans["addedQuestFour"] = true;

                            alan.Dialogue.Clear();
                            alan.Dialogue.Add("It's locked? Talk to Paul.");
                            paul.RemoveQuest(questThree);
                            //Re-add it so it stays in your Quest Page for now.
                            Game1.g.CurrentQuests.Add(Game1.g.Prologue.QuestThree);
                            paul.AddQuest(questFour);
                        }

                        if (prologueBooleans["addedQuestFour"] && prologueBooleans["addedKarmaAndInventory"] == false)
                        {
                            prologueBooleans["addedKarmaAndInventory"] = true;
                        }

                        //--Once you accept the fourth quest, change Alan's dialogue
                        if (game.CurrentQuests.Contains(questFour) && prologueBooleans["acceptedQuestFour"] == false)
                        {
                            prologueBooleans["acceptedQuestFour"] = true;
                            alan.Dialogue.Clear();
                            alan.Dialogue.Add("We'll find it, alright! Go talk to someone else!");
                        }

                        //--Once you complete the fourth quest, add it to the journal and add Quest 5
                        if (questFour.CompletedQuest && prologueBooleans["giveKey"] == false && prologueBooleans["gotKey"] == false)
                        {
                            alan.Dialogue.Clear();
                            alan.Dialogue.Add("Paul has the key.");

                            paul.RemoveQuest(questFour);
                            paul.AddQuest(questFive);
                            prologueBooleans["giveKey"] = true;

                        }

                        //--Give the player the closet key once you talk to Paul after finishing quest four
                        if (prologueBooleans["giveKey"] == true && paul.Talking && prologueBooleans["gotKey"] == false)
                        {

                            alan.Dialogue.Clear();
                            alan.Dialogue.Add("The Janitor's Closet is in the East Hall.");
                            questFour.RewardPlayer();
                            Game1.questHUD.RemoveQuestFromHelper(questFour);
                            player.StoryItems.Add("Closet Key", 1);
                            prologueBooleans["gotKey"] = true;
                            prologueBooleans["addedGardener"] = true;
                        }

                        if (prologueBooleans["gotKey"] == true && paul.Talking == false && prologueBooleans["gotKey2"] == false)
                        {
                            Chapter.effectsManager.AddFoundItem("the Closet Key", Game1.storyItemIcons["Closet Key"]);
                            prologueBooleans["gotKey2"] = true;
                        }

                        //--Change the NPC's dialogue when you obtain the key ring
                        if (player.StoryItems.ContainsKey("Key Ring") && prologueBooleans["updatedKeyDialogue"] == false)
                        {
                            alan.Dialogue.Clear();
                            alan.Dialogue.Add("Science is scary. Be careful not to damage those textbooks once you get them.");
                            prologueBooleans["updatedKeyDialogue"] = true;
                        }

                        if (prologueBooleans["brokeIntoLocker"] == true)
                            placeFlowersTimer = 101;


                        if (player.LearnedSkills.Count == 2 && prologueBooleans["equippedSecondSkill"] == false)
                        {
                            alan.Dialogue.Clear();
                            paul.DialogueState++;
                            paul.QuestDialogue.Add("Glad we could do business. Go ahead and give that new page a try.");
                            alan.Dialogue.Add("Try it out!");

                            prologueBooleans["equippedSecondSkill"] = true;
                        }
                        #endregion

                        UpdateNPCs();

                        if (alan.Quest == questOne)
                            alan.RemoveQuest(questOne);

                        if (TalkingToNPC == false)
                        {

                            player.Update();
                            hud.Update();
                            currentMap.Update();

                            camera.Update(player, game, currentMap);

                            player.Enemies = currentMap.EnemiesInMap;

                            #region NPCs Wander
                            if (!(nPCs["Paul"].Quest == questTwo && nPCs["Paul"].Quest.CompletedQuest) && !(nPCs["Paul"].Quest == questThree && karmaQuest.CompletedQuest && inventoryQuest.CompletedQuest) && !(prologueBooleans["checkedPhysicsDoor"] == true && questFour.CompletedQuest == false) && !(prologueBooleans["gotTextbook"]))
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

                            #region Starting Cutscenes

                            //-----
                            //--Quad Paper Scene is handled in The Quad map
                            //-----

                            if (game.CurrentQuests.Contains(questOne) && questOne.CompletedQuest)
                            {
                                Game1.schoolMaps.maps["North Hall"].currentBackgroundMusic = Sound.MusicNames.PaulAndAlanTheme;
                            }

                            //--Getting quest two
                            if (prologueBooleans["thirdSceneNotPlayed"] && game.CurrentQuests.Contains(questOne) && questOne.CompletedQuest && game.CurrentChapter.CurrentMap.MapName == "North Hall")
                            {
                                prologueBooleans["canBreakIntoLockers"] = true;
                                state = GameState.Cutscene;
                                prologueBooleans["thirdSceneNotPlayed"] = false;

                                prologueBooleans["addedJournalInstructor"] = true;
                            }

                            //--Trenchcoat in science 5
                            if (prologueBooleans["fifthSceneNotPlayed"] && game.CurrentChapter.CurrentMap == Game1.schoolMaps.maps["Science 105"] && player.PositionX > 1900)
                            {
                                prologueBooleans["fifthSceneNotPlayed"] = false;
                                state = GameState.Cutscene;

                            }
                            

                            if (prologueBooleans["removeNPCs"] == true && nPCs.ContainsKey("JournalInstructor"))
                            {
                                nPCs.Remove("SkillInstructor");
                                nPCs.Remove("JournalInstructor");
                                nPCs.Remove("SaveInstructor");
                            }

                            if (player.Textbooks == 1 && prologueBooleans["gotTextbook"] == false)
                            {
                                prologueBooleans["gotTextbook"] = true;
                                //questFive.RewardPlayer();

                                alan.Dialogue.Clear();
                                paul.DialogueState++;
                                paul.QuestDialogue.Add("Only one textbook? Well, it's not impressive but it's a start. Step into our shop and we'll make a deal.");
                                alan.Dialogue.Add("Come on into the shop. You're our first customer!");

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
                            }

                            //--Start the tim boss fight
                            if (!prologueBooleans["fifthSceneNotPlayed"] && player.EquippedSkills.Count == 2 && prologueBooleans["sixthSceneNotPlayed"])
                            {
                                paul.RemoveQuest(questFive);
                                Game1.questHUD.RemoveQuestFromHelper(questFive);
                                questFive.RewardPlayer();
                                alan.FacingRight = true;
                                paul.FacingRight = true;
                                prologueBooleans["addedTim"] = true;
                                AddNPCs();
                                prologueBooleans["sixthSceneNotPlayed"] = false;
                                state = GameState.Cutscene;

                                synopsis += "\n\nUnsurprisingly, the science room was locked. Luckily for you, Paul had a key to the Janitor's Closet. In order to locate the exact whereabouts of this key, they sent you away so they could find it in peace. In your quest to give your new friends some space, you encountered a scattered group of role players on a quest of their own, quests from the realm of Dwarves & Druids: the coolest game ever. One by one they taught you the basics of role playing while simultaneously teaching you some tips for properly playing Super Daryl Deluxe, only in a way that broke the fourth wall much more subtly than your journal just did. Soon you returned to Paul and Alan and were given the closet keys. After a rather quick and painless trespass, you found the Science Room keys and finally began your mission.  You ventured through the completely normal, if not a bit dusty science room and met a kid in a trenchcoat who had already cleared the room of all textbooks. All was not in vain, however, as he was more than happy to sell you one for a small price. Luckily, you had stolen Tim's lunch money from his locker, like a real jerk, and were more than able to pay.";
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

                    case GameState.BreakingLocker:
                        if (prologueBooleans["brokeIntoLocker"] == true && placeFlowersTimer <= 100)
                            placeFlowersTimer++;
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
                case GameState.BreakingLocker:
                    if (prologueBooleans["brokeIntoLocker"] == false)
                    {
                        if ((Game1.schoolMaps.maps["North Hall"] as NorthHall).TimsLocker.state == StudentLocker.LockerState.open 
                            && player.StoryItems.ContainsKey("Dandelion") && player.StoryItems["Dandelion"] == 3)
                        {
                            prologueBooleans["brokeIntoLocker"] = true;
                            player.RemoveStoryItem("Dandelion", 3);

                            prologueBooleans["addedSkillInstructor"] = true;
                        }
                    }

                    if (placeFlowersTimer < 100 && prologueBooleans["brokeIntoLocker"] == true)
                    {
                        s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                        s.Draw(Game1.youFoundItemTexture, new Vector2(450, 300), Color.White);
                        s.DrawString(Game1.twConQuestHudName, "You put the Dandelions in \n           Tim's Locker!", new Vector2(505, 340), Color.Black);

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

                        s.End();
                    }
                    break;
            }
        }

        public void AddNPC(String name, NPC npc)
        {
            nPCs.Add(name, npc);

            if (game.saveData.prologueNPCWrappers != null)
            {
                for (int i = 0; i < game.saveData.prologueNPCWrappers.Count; i++)
                {
                    if (name == game.saveData.prologueNPCWrappers[i].npcName)
                    {
                        if (game.saveData.prologueNPCWrappers[i].questName != null)
                        {
                            npc.Dialogue = game.saveData.prologueNPCWrappers[i].dialogue;
                            npc.QuestDialogue = game.saveData.prologueNPCWrappers[i].questDialogue;
                            npc.DialogueState = game.saveData.prologueNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.prologueNPCWrappers[i].facingRight;
                            npc.Quest = game.AllQuests[game.saveData.prologueNPCWrappers[i].questName];
                            npc.AcceptedQuest = game.saveData.prologueNPCWrappers[i].acceptedQuest;
                            npc.MapName = game.saveData.prologueNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.prologueNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.prologueNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.prologueNPCWrappers[i].positionY;
                        }

                        else if (game.saveData.prologueNPCWrappers[i].trenchCoat == false)
                        {
                            npc.Dialogue = game.saveData.prologueNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.prologueNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.prologueNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            npc.MapName = game.saveData.prologueNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.prologueNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.prologueNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.prologueNPCWrappers[i].positionY;
                        }
                        else
                        {
                            npc.Dialogue = game.saveData.prologueNPCWrappers[i].dialogue;
                            npc.QuestDialogue = null;
                            npc.DialogueState = game.saveData.prologueNPCWrappers[i].dialogueState;
                            npc.FacingRight = game.saveData.prologueNPCWrappers[i].facingRight;
                            npc.Quest = null;
                            npc.AcceptedQuest = false;
                            (npc as TrenchcoatKid).SoldOut = game.saveData.prologueNPCWrappers[i].trenchcoatSoldOut;
                            npc.MapName = game.saveData.prologueNPCWrappers[i].mapName;
                            npc.canTalk = game.saveData.prologueNPCWrappers[i].canTalk;
                            npc.PositionX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.PositionY = game.saveData.prologueNPCWrappers[i].positionY;
                            npc.RecX = game.saveData.prologueNPCWrappers[i].positionX;
                            npc.RecY = game.saveData.prologueNPCWrappers[i].positionY;
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
                alan = new NPC(Game1.whiteFilter, dialogue1, new Rectangle(2700, 270, 516, 388),
                    player, game.Font, game,  "North Hall" , "Alan", false);
                AddNPC("Alan", alan);
            }

            if (!nPCs.ContainsKey("Paul"))
            {
                //--Paul
                List<String> dialogue2 = new List<string>();
                paul = new NPC(game.NPCSprites["Paul"], dialogue2, questOne, new Rectangle(2880, 680 - 388, 516, 388), player,
                    game.Font, game,  "North Hall" , "Paul", false);
                AddNPC("Paul", paul);
            }


            if (!nPCs.ContainsKey("TrenchcoatCrony"))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                List<ItemForSale> items = new List<ItemForSale>();
                items.Add(new TextbookForSale(5.00f, 1));
                trenchcoatEmployee = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, -10000, 0, player, game, "Science 105", items);
                AddNPC("TrenchcoatCrony", trenchcoatEmployee);
            }

            if (!nPCs.ContainsKey("SkillInstructor") && prologueBooleans["addedSkillInstructor"] == true && prologueBooleans["removeNPCs"] == false)
            {
                #region Add skill instructor and box enemy
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogueSkill = new List<string>();
                dialogueSkill.Add("Only through knowledge of skills will one obtain true power.");
                skillInstructor = new NPC(game.NPCSprites["Skill Sorceress"], dialogueSkill, skillQuest,
                    new Rectangle(3350, 680 - 395, 516, 388), player, game.Font, game, "North Hall", "Skill Sorceress", false);

                AddNPC("SkillInstructor", skillInstructor);
                #endregion
            }

            if (!nPCs.ContainsKey("SaveInstructor") && prologueBooleans["addedJournalInstructor"] == true && prologueBooleans["removeNPCs"] == false)
            {
                //--Level up NPC
                List<String> dialogueSave = new List<string>();
                dialogueSave.Add("I save every time I come in here!");
                saveInstructor = new NPC(game.NPCSprites["Saving Instructor"], dialogueSave, saveQuest,
                    new Rectangle(470, 680 - 388, 516, 388), player, game.Font, game, "Bathroom", "Saving Instructor", false);
                AddNPC("SaveInstructor", saveInstructor);
            }

            if (!nPCs.ContainsKey("JournalInstructor") && prologueBooleans["addedJournalInstructor"] == true && prologueBooleans["removeNPCs"] == false)
            {
                //--Journal Instructor
                List<String> dialogueJournal = new List<string>();
                dialogueJournal.Add("Organization is power!");
                journalInstructor = new NPC(game.NPCSprites["Keeper of the Quests"], dialogueJournal, journalQuest,
                    new Rectangle(1630, 670 - 388, 516, 388), player, game.Font, game, "North Hall", "Keeper of the Quests", false);

                AddNPC("JournalInstructor", journalInstructor);
            }

            if (!nPCs.ContainsKey("InventoryInstructor") && prologueBooleans["addedKarmaAndInventory"] == true)
            {

                #region Add two NPCs
                //--Inventory Instructor
                List<String> dialogueEquipment = new List<string>();
                dialogueEquipment.Add("I scored this sweet helmet of +2 Dwarf Slaying from a raid yesterday.");
                inventoryInstructor = new NPC(game.NPCSprites["Weapons Master"], dialogueEquipment, inventoryQuest,
                    new Rectangle(500, 680 - 388, 516, 388), player, game.Font, game, "The Quad", "Weapons Master", false);

                AddNPC("InventoryInstructor", inventoryInstructor);

                //--Karma Instructor
                List<String> dialogueKarma = new List<string>();
                dialogueKarma.Add("I'm almost a Level 5 'Wizard'!");
                karmaInstructor = new NPC(game.NPCSprites["Karma Shaman"], dialogueKarma, karmaQuest,
                    new Rectangle(1500, 255, 516, 388), player, game.Font, game,  "Main Lobby", "Karma Shaman", false);
                karmaInstructor.FacingRight = true;
                AddNPC("KarmaInstructor", karmaInstructor);
                #endregion
            }

            if (!nPCs.ContainsKey("Tim") && prologueBooleans["addedTim"] == true)
            {
                //--Tim
                List<String> timDialogue = new List<string>();
                tim = new NPC(game.NPCSprites["Tim"], timDialogue, new Rectangle(0, 680 - 388, 516, 388),
                    player, game.Font, game, "North Hall", "Tim", false);
                AddNPC("Tim", tim);
            }
        }
    }
}
