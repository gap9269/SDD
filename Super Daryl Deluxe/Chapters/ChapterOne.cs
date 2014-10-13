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
    public class ChapterOne : Chapter
    {
        NPC alan;
        NPC paul;
        NPC trenchcoatVents;
        NPC inventoryInstructor;
        NPC karmaInstructor;
        NPC princess;
        NPC scientistOne;
        NPC beethoven;

        //--Story quests
        ReturningKeys returningKeys;
        DaddysLittlePrincess daddysLittlePrincess;
        CommunicatingWithBeethoven communicatingWithBeethoven;
        DealingWithManagement dealingWithManagement;

        //Cutscenes
        ChapterOneOpening chOneOpening;
        ChapterOneGymScene gymScene;
        GettingQuestOne gettingQuestOne;
        PrincessCutscene princessCutscene;
        FirstScientistScene firstScientistScene;

        //--Story Quest attributes
        Dictionary<String, Boolean> chapterOneBooleans;

        //Switch state to decision making, then pick the decision
        enum Decisions
        {
            none,
            test
        }
        Decisions decisions;

        static Random ran = new Random();

        public Dictionary<String, Boolean> ChapterOneBooleans { get { return chapterOneBooleans; } set { chapterOneBooleans = value; } }
        public ReturningKeys ReturningKeys { get { return returningKeys; } }
        public DaddysLittlePrincess DaddysLittlePrincess { get { return daddysLittlePrincess; } }

        public ChapterOne(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            //Quests
            returningKeys = new ReturningKeys(true);
            daddysLittlePrincess = new DaddysLittlePrincess(true);
            communicatingWithBeethoven = new CommunicatingWithBeethoven(true);
            dealingWithManagement = new DealingWithManagement(true, game);

            chapterOneBooleans = new Dictionary<String, bool>();

            game.AllQuests.Add(returningKeys.QuestName, returningKeys);
            game.AllQuests.Add(daddysLittlePrincess.QuestName, daddysLittlePrincess);
            game.AllQuests.Add(communicatingWithBeethoven.QuestName, communicatingWithBeethoven);

            chapterOneBooleans.Add("announcementUsed", false);
            chapterOneBooleans.Add("alanApproached", false);
            chapterOneBooleans.Add("questOneSceneStarted", false);
            chapterOneBooleans.Add("checkedArtHall", false);
            chapterOneBooleans.Add("overheardInventoryInstructor", false);
            chapterOneBooleans.Add("addedMainLobbyNPCs", false);
            chapterOneBooleans.Add("playedPrincessScene", false);
            chapterOneBooleans.Add("playedMusicRoomScene", false);
            chapterOneBooleans.Add("givenBackstageKey", false);

            //Cutscenes
            chOneOpening = new ChapterOneOpening(game, camera, player, textures["FlashBack"]);
            gymScene = new ChapterOneGymScene(game, camera, player, textures["GymScene"]);
            gettingQuestOne = new GettingQuestOne(game, camera, player);
            princessCutscene = new PrincessCutscene(game, camera, player, textures["FlashBack"]);
            firstScientistScene = new FirstScientistScene(game, camera, player);

            chapterScenes.Add(chOneOpening);
            chapterScenes.Add(gymScene);
            chapterScenes.Add(gettingQuestOne);
            chapterScenes.Add(princessCutscene);
            chapterScenes.Add(firstScientistScene);

            AddNPCs();

            //Change to cutscene to play scene
            state = GameState.Game;
            cutsceneState = 2;
            synopsis = "";
        }

        public override void Update()
        {
            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }
            if (!player.LevelingUp)
            {
                base.Update();
                
                cursor.Update();
                AddNPCs();
                game.SideQuestManager.AddNPCs();

                //--Remove the janitor's closet portal if it is there
                if (ArtHall.ToJanitorsCloset.IsUseable)
                {
                    ArtHall.ToJanitorsCloset.IsUseable = false;
                }

                switch (state)
                {

                    case GameState.Game:
                        UpdateNPCs();
                        game.SideQuestManager.Update();

                        #region In Game Dialogue
                        //--Announcement at the start of the chapter
                        if (chapterOneBooleans["announcementUsed"] == false)
                        {
                            Chapter.effectsManager.AddAnnouncement("Stolen item. Report by end of day.", 180);

                            Chapter.effectsManager.AddAnnouncement("Have a nice day!", 180);

                            chapterOneBooleans["announcementUsed"] = true;
                        }

                        //--Alan when you approach them for the first quest
                        if (chapterOneBooleans["alanApproached"] == false && CurrentMap == Game1.schoolMaps.maps["NorthHall"] && Vector2.Distance(player.Position, alan.Position) < 2500)
                        {
                            Chapter.effectsManager.AddInGameDialogue("What the hell are we going to do?!", "Alan", "Normal", 200);
                            chapterOneBooleans["alanApproached"] = true;
                        }

                        //--Inventory guy talking about the upper vents
                        if (chapterOneBooleans["overheardInventoryInstructor"] == false && CurrentMap == Game1.schoolMaps.maps["MainLobby"])
                        {
                            Chapter.effectsManager.AddInGameDialogue("They took the grate off upstairs, I swear! I bet it's going to be a sick new area to explore. \nRumor has it they lead to a whole bunch of places around school.", "Equipment Instructor", "Normal", 300);
                            chapterOneBooleans["overheardInventoryInstructor"] = true;
                        }
                        
                        #endregion

                        #region Story Quests
                        if (communicatingWithBeethoven.CompletedQuest == true && dealingWithManagement.CompletedQuest == false && beethoven.Quest == null)
                        {
                            beethoven.AddQuest(dealingWithManagement);
                        }
                        #endregion

                        //--Start the Quest One Cutscene when you talk to paul or alan at the start of the chapter
                        if (chapterOneBooleans["questOneSceneStarted"] == false && (paul.Talking || alan.Talking))
                        {
                            chapterOneBooleans["questOneSceneStarted"] = true;
                            paul.Talking = false;
                            alan.Talking = false;
                            state = GameState.Cutscene;
                        }

                        //--Checked the art hall
                        if (CurrentMap == Game1.schoolMaps.maps["ArtHall"] && chapterOneBooleans["checkedArtHall"] == false)
                            chapterOneBooleans["checkedArtHall"] = true;
                        

                        //--Add the NPCs in the main lobby
                        if (chapterOneBooleans["checkedArtHall"] == true && chapterOneBooleans["addedMainLobbyNPCs"] == false)
                        {
                            chapterOneBooleans["addedMainLobbyNPCs"] = true;
                        }


                        //--TEST FOR HOW DECISIONS WORK
                        #region Decisions
                        if (current.IsKeyUp(Keys.M) && last.IsKeyDown(Keys.M) && decisionNum == 0)
                        {
                            makingDecision = true;
                            decisions = Decisions.test;
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

                        if (TalkingToNPC == false && makingDecision == false)
                        {
                            #region Starting Cutscenes


                            if (chapterOneBooleans["playedPrincessScene"] == false && currentMap == Game1.schoolMaps.maps["PrincessLockerRoom"])
                            {
                                state = GameState.Cutscene;
                                chapterOneBooleans["playedPrincessScene"] = true;
                            }


                            if (chapterOneBooleans["playedMusicRoomScene"] == false && currentMap == Game1.schoolMaps.maps["IntroToMusic"])
                            {
                                state = GameState.Cutscene;
                                chapterOneBooleans["playedMusicRoomScene"] = true;
                                MusicIntroRoom.ToSouthHall.ItemNameToUnlock = "boss";
                                MusicIntroRoom.ToSouthHall.PortalTexture = Game1.lockedPortalTexture;
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
                alan = new NPC(Game1.whiteFilter, dialogue1, new Rectangle(4190, 680 - 388, 516, 388),
                    player, game.Font, game, "North Hall", "Alan", false);

                nPCs.Add("Alan", alan);
            }



            if (!nPCs.ContainsKey("TrenchcoatCrony"))
            {
                //Trenchcoat crony
                List<String> cronydialogue = new List<string>();
                cronydialogue.Add("Sup, bitchtits");
                List<ItemForSale> items = new List<ItemForSale>();
                items.Add(new TextbookForSale(0f, 2));
                items.Add(new WeaponForSale(new Marker(), 0f));
                trenchcoatVents = new TrenchcoatKid(game.NPCSprites["Trenchcoat Employee"], cronydialogue, 1000, 630, player, game,  "Main Lobby" , items);
                nPCs.Add("TrenchcoatCrony", trenchcoatVents);
            }

            if (!nPCs.ContainsKey("Paul"))
            {
                //--Paul
                List<String> dialogue2 = new List<string>();
                dialogue2.Add(" ");
                paul = new NPC(game.NPCSprites["Paul"], dialogue2, new Rectangle(4370, 680 - 388, 516, 388), player,
                    game.Font, game,  "North Hall" , "Paul", false);
                nPCs.Add("Paul", paul);
            }

            if (!nPCs.ContainsKey("Daddy's Little Princess"))
            {
                //--Princess
                List<String> dialogue = new List<string>();
                princess = new NPC(game.NPCSprites["Paul"], dialogue, new Rectangle(0, 0, 0, 0), player,
                    game.Font, game,  "Princess's Locker Room" , "Alan", true);
                princess.RecY = 630 - princess.Rec.Height;
                nPCs.Add("Daddy's Little Princess", princess);

                NPCs["Daddy's Little Princess"].RecX = 1800;
                NPCs["Daddy's Little Princess"].PositionX = princess.RecX;
                NPCs["Daddy's Little Princess"].PositionY = princess.RecY;
            }

            if (!nPCs.ContainsKey("ScientistOne"))
            {
                //--Science Room NPC
                List<String> d = new List<string>();
                scientistOne = new NPC(game.EnemySpriteSheets["Scientist"], d,
                    new Rectangle(0, 0, 0, 0), player, game.Font, game, "Intro To Music", "Alan", true);
                scientistOne.RecY = 630 - scientistOne.Rec.Height;
                scientistOne.RecX = -1000;
                nPCs.Add("ScientistOne", scientistOne);
            }

            if (!nPCs.ContainsKey("Beethoven"))
            {
                //--Science Room NPC
                List<String> d = new List<string>();
                d.Add("temporary dialogue");
                beethoven = new NPC(game.NPCSprites["Paul"], d, communicatingWithBeethoven,
                    new Rectangle(0, 0, 0, 0), player, game.Font, game, "The Stage", "Alan", true);
                beethoven.RecY = -650 - beethoven.Rec.Height;
                beethoven.RecX = 2100;
                nPCs.Add("Beethoven", beethoven);
            }

            if (!nPCs.ContainsKey("InventoryInstructor") && chapterOneBooleans["addedMainLobbyNPCs"] == true)
            {
                #region Add two NPCs
                //--Inventory Instructor
                List<String> dialogueEquipment = new List<string>();
                dialogueEquipment.Add("Oh hey dude. Cool seeing you again. I was just telling my friend here about that wicked cool dungeon upstairs.");
                inventoryInstructor = new NPC(game.NPCSprites["Equipment Instructor"], dialogueEquipment,
                    new Rectangle(0, 0, 0, 0), player, game.Font, game,  "Main Lobby" , "Equipment Instructor", true);

                inventoryInstructor.RecY = 630 - inventoryInstructor.Rec.Height;
                inventoryInstructor.RecX = 1050;
                inventoryInstructor.FacingRight = false;
                nPCs.Add("InventoryInstructor", inventoryInstructor);

                //--Karma Instructor
                List<String> dialogueKarma = new List<string>();
                dialogueKarma.Add("I bet there are a ton of epic monsters in those vents!");
                dialogueKarma.Add("I doubt that they actually lead to anywhere cool though. Probably just to the janitor's closet or something.");
                karmaInstructor = new NPC(game.NPCSprites["Karma Instructor"], dialogueKarma,
                    new Rectangle(0, 0, 0, 0), player, game.Font, game,  "Main Lobby", "Karma Instructor", true);

                karmaInstructor.RecY = 630 - karmaInstructor.Rec.Height;
                karmaInstructor.RecX = 800;
                nPCs.Add("KarmaInstructor", karmaInstructor);
                #endregion
            }

        }
    }
}
