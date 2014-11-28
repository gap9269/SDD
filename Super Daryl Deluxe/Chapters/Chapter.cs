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
using System.IO;
using System.Threading;

namespace ISurvived
{
    public class Chapter
    {
        public static Boolean LoadTrollTexturesByDrawing = false;
        public static Boolean loadingContentInBackground = false;
        int loadingFrame;
        int loadingFrameDelay = 5;

        protected MapClass currentMap;
        protected String nextMap;
        protected String synopsis;
        protected StudentLocker currentLocker;
        protected List<Cutscene> chapterScenes;
        protected Camera camera;
        protected SkillManager skillManager;
        public Player player;
        protected Game1 game;
        protected HUD hud;
        protected Cursor cursor;
        protected Boolean talkingToNPC;
        protected Boolean makingDecision = false;
        protected int decisionNum; //Used to make sure decisions aren't repeated. Probably better to just use a list of booleans in the chapterBools
        protected Dictionary<String, NPC> nPCs;

        protected int cutsceneState;
        protected Dictionary<String, Texture2D> chapterTextures;
        protected KeyboardState current;
        protected KeyboardState last;
        protected bool justPaused;
        protected Portal startingPortal;
        public static EffectsManager effectsManager;
        public static Cursor staticCursor;

        public static String lastMap;
        public static String theNextMap;

        public float tintAlpha = 0f;
        Boolean tintAlphaRising = true;

        protected Boolean bossFight = false;
        protected Boss currentBoss;

        Random rand = new Random();

        Dictionary<String, Quest> completedSideQuests;
        Dictionary<String, Quest> completedStoryQuests;

        public Boolean fallingOffMap = false;

        #region MAP EDIT STUFF
        Boolean pass = false;
        Boolean spawn = false;
        Button passButton, spawnButton, platformButton, saveButton;
        Platform selectedPlat;
        Vector2 currentMousePos, lastMousePos;

        #endregion

        //--Map fade attributes
        protected bool notFirstFrame;
        protected bool fade;
        protected int fadeTimer;
        protected float fadeAlpha;
        public enum GameState
        {
            MainMenu,
            Game,
            Pause,
            YourLocker,
            BreakingLocker,
            Cutscene,
            ChangingMaps,
            loading,
            noteBook,
            shop, 
            crafting,
            dead,
            mapEdit
        }
        public GameState state;

        public enum CurrentMapZone
        {
            school,
            science,
            vienna,
            upperVents,
            chelseas,
            tutorial
        }
        public Dictionary<String, NPC> NPCs { get { return nPCs; } set { nPCs = value; } }
        public MapClass CurrentMap { get { return currentMap; } set { currentMap = value; } }
        public StudentLocker CurrentLocker { get { return currentLocker; } set { currentLocker = value; } }
        public Boolean TalkingToNPC { get { return talkingToNPC; } 
            set { talkingToNPC = value; } }

        public int CutsceneState { get { return cutsceneState; } set { cutsceneState = value; } }
        public List<Cutscene> ChapterScenes { get { return chapterScenes; } }
        public Portal StartingPortal { get { return startingPortal; } set { startingPortal = value; } }
        public String NextMap { get { return nextMap; } set { nextMap = value; } }
        public String Synopsis { get { return synopsis; } set { synopsis = value; } }
        public Dictionary<String, Quest> CompletedSideQuests { get { return completedSideQuests; } set { completedSideQuests = value; } }
        public Dictionary<String, Quest> CompletedStoryQuests { get { return completedStoryQuests; } set { completedStoryQuests = value; } }
        public HUD HUD { get { return hud; } set { hud = value; } }
        public Boss CurrentBoss { get { return currentBoss; } set { currentBoss = value; } }
        public Boolean BossFight { get { return bossFight; } set { bossFight = value; } }
        public Boolean MakingDecision { get { return makingDecision; } set { makingDecision = value; } }

        public Chapter(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> tex)
        {
            player = p;
            game = g;
            //allMaps = maps;
            hud = nHud;
            cursor = cur;
            camera = cam;
            state = GameState.Game;
            skillManager = game.SkillManager;
            nPCs = new Dictionary<String, NPC>();
            cutsceneState = 0;
            chapterTextures = tex;

            //currentMap = Game1.alwaysLoadedMaps.maps["Bathroom"];
            fade = false;
            fadeTimer = 0;
            fadeAlpha = 0;
            notFirstFrame = false;
            

            chapterScenes = new List<Cutscene>();
            effectsManager = new EffectsManager();
            completedStoryQuests = new Dictionary<string, Quest>();
            completedSideQuests = new Dictionary<string, Quest>();

            spawnButton = new Button(new Rectangle(1000, 0, 50, 50));
            passButton = new Button(new Rectangle(1075, 0, 50, 50));
            platformButton = new Button(new Rectangle(1150, 0, 50, 50));
            saveButton= new Button(new Rectangle(1225, 0, 50, 50));


            //Map edit shit
            lastMousePos = new Vector2();
            currentMousePos = new Vector2();
        }


        public void LoadMapContentInBackground()
        {
            currentMap.LoadContent();
            currentMap.LoadEnemyData();

            loadingContentInBackground = false;
            EnterMap();
        }

        public void LoadCurrentMapLocks()
        {
            for (int i = 0; i < currentMap.Portals.Count; i++)
            {
                //If it has a lock
                if (currentMap.Portals.ElementAt(i).Key.ItemNameToUnlock != null && currentMap.Portals.ElementAt(i).Key.ItemNameToUnlock != "")
                {
                    if (currentMap.Portals.ElementAt(i).Key.ItemNameToUnlock == "Gold Key")
                    {
                        if (Portal.gold == null)
                        {
                            Portal.LoadLockContent("Gold");
                        }
                        currentMap.Portals.ElementAt(i).Key.lockTexture = Portal.gold;

                    }
                    else if (currentMap.Portals.ElementAt(i).Key.ItemNameToUnlock == "Silver Key")
                    {
                        if (Portal.silver == null)
                        {
                            Portal.LoadLockContent("Silver");
                        }
                        currentMap.Portals.ElementAt(i).Key.lockTexture = Portal.silver;

                    }
                    else if (currentMap.Portals.ElementAt(i).Key.ItemNameToUnlock == "Bronze Key")
                    {
                        if (Portal.bronze == null)
                        {
                            Portal.LoadLockContent("Bronze");
                        }
                        currentMap.Portals.ElementAt(i).Key.lockTexture = Portal.bronze;

                    }
                    else
                    {
                        if (Portal.gold == null)
                        {
                            Portal.LoadLockContent("Gold");
                        }

                        currentMap.Portals.ElementAt(i).Key.lockTexture = Portal.gold;
                    }
                }
            }
        }

        public void EnterMap()
        {
            currentMap.enteringMap = true;
            player.Position = player.nextMapPos;

            //This updates the camera to the player's new coordinates
            //This is done here because updating the camera's position before the game resumed was causing issues with parallax and foreground items
            //Basically I would update the camera every frame to allow them to be drawn correctly, but it would make the camera move to the new player
            //before changing maps, since this code used to be run as soon as a portal was used
            game.Camera.centerTarget = new Vector2(player.nextMapPos.X + (player.Rec.Width / 2), 0);

            if (currentMap.yScroll)
                game.Camera.centerTarget += new Vector2(0, player.nextMapPos.Y + (player.Rec.Height / 2));

            game.Camera.center = game.Camera.centerTarget;

            LoadCurrentMapLocks();

            if (fallingOffMap)
                fallingOffMap = false;

            //while (Sound.backgroundVolume < .5f)
            //{
            //    Sound.IncrementBackgroundVolume((float)(1f / 10000f));
            //    currentMap.PlayBackgroundMusic();
            //}
            //while (Sound.ambienceVolume < .5f)
            //{
            //    Sound.IncrementAmbienceVolume((float)(1f / 10000f));
            //    currentMap.PlayAmbience();
            //}
            state = GameState.Game;
        }

        public virtual void Update()
        {
            
            last = current;
            current = Keyboard.GetState();

            //This needs to be ran every frame so differing resolutions always have the static screens transform matrix
            camera.UpdateStaticTransform(game);

            //Console.WriteLine(player.CurrentPlat.Rec.ToString());

            switch (state)
            {
                #region NOTEBOOK
                case GameState.noteBook:
                    camera.Update(player, game, currentMap);
                    game.Notebook.Update();
                    break;
                #endregion

                #region BREAKING INTO LOCKER
                case GameState.BreakingLocker:
                    camera.Update(player, game, currentMap);
                    currentLocker.Update();
                    game.Notebook.Update();
                    break;
                #endregion

                #region CUTSCENE
                case GameState.Cutscene:
                    chapterScenes[cutsceneState].Play();
                    break;
                #endregion

                #region YOUR LOCKER
                case GameState.YourLocker:
                    game.YourLocker.Update();
                    break;
                #endregion

                #region PAUSE
                case GameState.Pause:
                    camera.Update(player, game, currentMap);

                    game.Options.Update();

                    if (current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back) && justPaused == false)
                        state = GameState.Game;

                    justPaused = false;
                break;
                #endregion

                #region GAME
                case GameState.Game:

                //Set the last map value
                if (lastMap == "" || lastMap == null)
                {
                    lastMap = currentMap.MapName;
                }

                if (player.playerState != Player.PlayerState.dead)
                {

#if DEBUG
                    if (current.IsKeyUp(Keys.F2) && last.IsKeyDown(Keys.F2))
                        state = GameState.mapEdit;
#endif

                    //--Update the boss if you are in his map
                    if (BossFight && currentMap == CurrentBoss.CurrentMap && !player.LevelingUp)
                    {
                        currentBoss.Update(currentMap.MapWidth);
                    }

                    //--If the player has a quest, update the current quest
                    if (game.CurrentQuests != null)
                    {
                        for (int i = 0; i < game.CurrentQuests.Count; i++)
                        {
                            game.CurrentQuests[i].UpdateQuest();
                        }
                    }

                    //--Pause the game
                    if (((current.IsKeyUp(Keys.Escape) && last.IsKeyDown(Keys.Escape)) || MyGamePad.StartPressed()) && !talkingToNPC && !makingDecision)
                    {
                        state = GameState.Pause;
                        justPaused = true;
                    }

                    //--Open your journal once you can
                    if (!(game.CurrentChapter is Prologue) || (game.CurrentChapter as Prologue).QuestOne.CompletedQuest)
                     {
                    if (!talkingToNPC && !makingDecision && !player.LevelingUp)
                    {
                        //Decrement time until you get another text
                        if(Game1.Player.HasCellPhone)
                            effectsManager.timeUntilNextMessage--;

                        if (MyGamePad.SelectPressed())
                        {
                            game.Notebook.LoadContent();
                            state = GameState.noteBook;
                            game.Notebook.Inventory.ResetInventoryBoxes();
                            game.Notebook.Inventory.ResetStoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }

                        if (current.IsKeyUp(Keys.I) && last.IsKeyDown(Keys.I))
                        {
                            game.Notebook.LoadContent();
                            state = GameState.noteBook;
                            game.Notebook.Inventory.ResetInventoryBoxes();
                            game.Notebook.Inventory.ResetStoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }

                        if (current.IsKeyUp(Keys.J) && last.IsKeyDown(Keys.J))
                        {
                            state = GameState.noteBook;
                            game.Notebook.LoadContent();
                            game.Notebook.state = DarylsNotebook.State.journal;

                            //If there is a notification up about the journal just being updated, make it go to the page that was just updated
                            if (effectsManager.secondNotificationQueue.Count > 0 && effectsManager.secondNotificationQueue.ElementAt(0) is JournalUpdateNotification)
                            {
                                //Set the journal to the current chapter, as that's the only chapter that could be updated
                                switch (game.chapterState)
                                {
                                    case Game1.ChapterState.prologue:
                                        game.Notebook.Journal.chapterState = Journal.ChapterState.Prologue;
                                        break;

                                    case Game1.ChapterState.chapterOne:
                                        game.Notebook.Journal.chapterState = Journal.ChapterState.ChapterOne;
                                        break;

                                    case Game1.ChapterState.chapterTwo:
                                        game.Notebook.Journal.chapterState = Journal.ChapterState.ChapterTwo;
                                        break;
                                }

                                //Set it to the state it needs to be at, depending on the type of notification

                                // 1 is synopsis
                                if ((effectsManager.secondNotificationQueue.ElementAt(0) as JournalUpdateNotification).typeOfEntryAdded == 1)
                                {
                                    game.Notebook.Journal.ViewingSpecificQuest = false;

                                    switch (game.chapterState)
                                    {
                                        case Game1.ChapterState.prologue:
                                            game.Notebook.Journal.prologueSynopsisRead = true;
                                            break;

                                        case Game1.ChapterState.chapterOne:
                                            game.Notebook.Journal.chOneSynopsisRead = true;
                                            break;

                                        case Game1.ChapterState.chapterTwo:
                                            game.Notebook.Journal.chTwoSynopsisRead = true;
                                            break;
                                    }

                                }

                                //2 is story quests
                                else if ((effectsManager.secondNotificationQueue.ElementAt(0) as JournalUpdateNotification).typeOfEntryAdded == 2)
                                {
                                    game.Notebook.Journal.insideChapterState = Journal.InsideChapterState.story;
                                    game.Notebook.Journal.selectedQuest = (effectsManager.secondNotificationQueue.ElementAt(0) as JournalUpdateNotification).quest;
                                    game.Notebook.Journal.ViewingSpecificQuest = true;
                                    game.Notebook.Journal.openedToSpecificQuest = true;
                                }

                                //3 is side quests
                                else if ((effectsManager.secondNotificationQueue.ElementAt(0) as JournalUpdateNotification).typeOfEntryAdded == 3)
                                {
                                    game.Notebook.Journal.insideChapterState = Journal.InsideChapterState.side;
                                    game.Notebook.Journal.selectedQuest = (effectsManager.secondNotificationQueue.ElementAt(0) as JournalUpdateNotification).quest;
                                    game.Notebook.Journal.ViewingSpecificQuest = true;
                                    game.Notebook.Journal.openedToSpecificQuest = true;
                                }
                            }

                           // game.Notebook.Inventory.ResetInventoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }

                        if (current.IsKeyUp(Keys.L) && last.IsKeyDown(Keys.L))
                        {
                            state = GameState.noteBook;
                            game.Notebook.LoadContent();
                            game.Notebook.state = DarylsNotebook.State.combos;
                            //game.Notebook.Inventory.ResetInventoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }

                        if (current.IsKeyUp(Keys.K) && last.IsKeyDown(Keys.K))
                        {
                            state = GameState.noteBook;
                            game.Notebook.LoadContent();
                            game.Notebook.state = DarylsNotebook.State.quests;
                            //game.Notebook.Inventory.ResetInventoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }

                        if (current.IsKeyUp(Keys.B) && last.IsKeyDown(Keys.B))
                        {
                            state = GameState.noteBook;
                            game.Notebook.LoadContent();
                            game.Notebook.state = DarylsNotebook.State.bios;
                            //game.Notebook.Inventory.ResetInventoryBoxes();
                            Chapter.effectsManager.RemoveToolTip();
                        }
                    }
                    }

                    if (talkingToNPC)
                        effectsManager.ClearDialogue();
                    else
                    {
                        //Makes it so you cannot speak to more than one NPC at a time
                        //Essentially says that if you spoke to an NPC this frame, you must wait two frames before the next "F" press will count
                        if (Game1.spokeThisFrame == true)
                        {
                            Game1.spokeThisFrameNum++;

                            if (Game1.spokeThisFrameNum >= 2)
                            {
                                Game1.spokeThisFrame = false;
                                Game1.spokeThisFrameNum = 0;
                            }
                        }
                    }
                }

                effectsManager.Update();
                break;
                #endregion

                #region CHANGING MAPS
                //--When the player changes maps
                case GameState.ChangingMaps:

                    if (loadingContentInBackground == false)
                    {
                        // camera.Update(player, game, currentMap); UNCOMMENT THIS IF CAMERA HAS ISSUES DURING CHANGING MAP WITH RESOLUTION OR SOMETHING
                        //THIS IS UNCOMMENTED TO MAKE IT SO CHANGING MAPS DOES NOT MAKE THE CAMERA SCROLL TO THE PLAYER, BUT SNAPS INSTEAD
                        effectsManager.RemoveToolTip();
                        //--Fade out for 40 frames
                        if (currentMap.leavingMapTimer <= 40)
                        {
                            MapFade(40);
                        }

                        theNextMap = nextMap;

                        //--Increase the fadeTimer and the mapTimer
                        fadeTimer++;
                        currentMap.leavingMapTimer++;

                        //Get the track name for the next map
                        String nextMapMusicName = Game1.schoolMaps.maps[nextMap].backgroundMusicName;

                        //--Once it has been 60 frames, reset the timer and change the map
                        //--But before going back to the game, change the next map's enteringMap attribute to true
                        if (Sound.backgroundVolume > 0) // && currentMap.backgroundMusicName != nextMapMusicName This makes it so music doesn't fade if the maps have the same music. I like it with the subtle fade a lot more
                        {
                            currentMap.PlayBackgroundMusic();
                            if(Sound.backgroundVolume > .2f)
                                Sound.IncrementBackgroundVolume(-(float)(1f / 70f));
                        }

                        if (Sound.ambienceVolume > 0)
                        {
                            currentMap.PlayAmbience();
                            if (Sound.ambienceVolume > .2f)
                            Sound.IncrementAmbienceVolume(-(float)(1f / 70f));
                        }

                        if (currentMap.leavingMapTimer >= 60)
                        {
                            if (!currentMap.Discovered)
                                currentMap.Discovered = true;

                            currentMap.leavingMapTimer = 0;

                            //Don't reset NPCs if you are reloading the map due to falling off
                            if (!fallingOffMap)
                            {
                                //Reset the NPC sprites
                                for (int i = 0; i < nPCs.Count; i++)
                                {
                                    if (nPCs.ElementAt(i).Value.MapName == currentMap.MapName)
                                    {
                                        nPCs.ElementAt(i).Value.Spritesheet = Game1.whiteFilter;
                                    }
                                }

                                //Reset the Side Quest Mananger NPC sprites
                                for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
                                {
                                    if (game.SideQuestManager.nPCs.ElementAt(i).Value.MapName == currentMap.MapName)
                                    {
                                        game.SideQuestManager.nPCs.ElementAt(i).Value.Spritesheet = Game1.whiteFilter;
                                    }
                                }

                                //Reset the lock textures
                                for (int i = 0; i < currentMap.Portals.Count; i++)
                                {
                                    currentMap.Portals.ElementAt(i).Key.lockTexture = null;
                                }

                                lastMap = currentMap.MapName;


                                //Unload the last map
                                currentMap.UnloadContent();
                            }

                            currentMap = Game1.schoolMaps.maps[nextMap];

                            //Don't reload assets if you are reloading the map due to falling off
                            if (!fallingOffMap)
                            {
                                Thread loadingThread = new Thread(new ThreadStart(LoadMapContentInBackground));
                                loadingThread.Start();

                                loadingContentInBackground = true;
                                state = GameState.loading;
                            }
                            else
                                EnterMap();
                        }
                    }
                    break;
                #endregion

                case GameState.shop:
                    game.Shop.Update();
                    break;
                case GameState.crafting:
                    //game.CraftingMenu.Update();
                    break;
                case GameState.dead:
                    cursor.Update();
                    Game1.deathScreen.Update();
                    break;
                case GameState.mapEdit:
                    if (current.IsKeyUp(Keys.F2) && last.IsKeyDown(Keys.F2))
                        state = GameState.Game;

                    player.Update();
                    hud.Update();
                    currentMap.Update();

                    lastMousePos = currentMousePos;

                    currentMousePos = new Vector2(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y - 600);

                    camera.Update(player, game, currentMap);
                    player.Enemies = currentMap.EnemiesInMap;

                    if (spawnButton.Clicked())
                        spawn = !spawn;

                    if (passButton.Clicked())
                        pass = !pass;


                    if (platformButton.Clicked() && selectedPlat == null)
                    {
                        if (currentMap.yScroll)
                        {
                            selectedPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360, 300, 50), pass, spawn, false);
                        }
                        else
                        {
                            selectedPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y, 300, 50), pass, spawn, false);
                        }
                    }

                    if (selectedPlat != null)
                    {

                        Console.WriteLine(selectedPlat.Rec.ToString());

                        int x = (int)(currentMousePos.X - lastMousePos.X);
                        int y = (int)(currentMousePos.Y - lastMousePos.Y);

                        selectedPlat.Position = new Vector2(selectedPlat.Position.X + x, selectedPlat.Position.Y + y);
                        selectedPlat.RecX = (int)selectedPlat.Position.X;
                        selectedPlat.RecY = (int)selectedPlat.Position.Y;

                        if (current.IsKeyUp(Keys.V) && last.IsKeyDown(Keys.V))
                        {
                            selectedPlat.mapEditButton.ButtonRec = selectedPlat.Rec;

                            if(!currentMap.Platforms.Contains(selectedPlat))
                                currentMap.Platforms.Add(selectedPlat);

                            selectedPlat = null;
                            pass = false;
                            spawn = false;
                        }

                        if (current.IsKeyUp(Keys.U) && last.IsKeyDown(Keys.U))
                        {
                            selectedPlat.RecWidth -= 50;
                        }

                        if (current.IsKeyUp(Keys.I) && last.IsKeyDown(Keys.I))
                        {
                            selectedPlat.RecWidth += 50;
                        }

                        if (current.IsKeyUp(Keys.O) && last.IsKeyDown(Keys.O))
                        {
                            selectedPlat.RecHeight -= 50;
                        }

                        if (current.IsKeyUp(Keys.L) && last.IsKeyDown(Keys.L))
                        {
                            selectedPlat.RecHeight += 50;
                        }

                        if (current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back))
                        {

                            if (currentMap.Platforms.Contains(selectedPlat))
                                currentMap.Platforms.Remove(selectedPlat);
                            selectedPlat = null;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < currentMap.Platforms.Count; i++)
                        {
                            if (currentMap.Platforms[i].Clicked())
                            {
                                selectedPlat = currentMap.Platforms[i];
                                pass = selectedPlat.Passable;
                                spawn = selectedPlat.SpawnOnTop;
                            }
                        }
                    }

                    if (saveButton.Clicked())
                    {

                        //Create a text file with all of the platform's attributes
                        //Make sure to delete this text file for the final relase, as we don't want the player to see them
                        //They are very useful for the method in MapClass that creates a binary file out of every mapfile, however.
                        //----------------------------------------------------------
                        String file = "Maps\\" + currentMap.MapName + ".txt";
                        File.Delete(file);
                        StreamWriter sw = File.AppendText(file);

                        
                        for (int i = 0; i < currentMap.Platforms.Count; i++)
                        {
                            sw.WriteLine("RockFloor2" + "," + currentMap.Platforms[i].Rec.Width + "," +
                                currentMap.Platforms[i].Rec.Height + "," + (currentMap.Platforms[i].Rec.X) + "," + (currentMap.Platforms[i].Rec.Y) + "," + currentMap.Platforms[i].Passable + "," + currentMap.Platforms[i].SpawnOnTop + "," + "False");
                        }

                        sw.Close();
                        //-----------------------------------------------------------

                        //Writes a binary file of the platform's information. This file is necessary, as it is used to create the platforms
                        //on map load
                        //---------------------------------------------------
                        BinaryWriter write = new BinaryWriter(File.Open("Maps\\" + currentMap.MapName + "Binary.txt", FileMode.Create));

                        for (int i = 0; i < currentMap.Platforms.Count; i++)
                        {
                            write.Write("RockFloor2");
                            write.Write(currentMap.Platforms[i].Rec.Width);
                            write.Write(currentMap.Platforms[i].Rec.Height);
                            write.Write(currentMap.Platforms[i].Rec.X);
                            write.Write(currentMap.Platforms[i].Rec.Y);
                            write.Write(currentMap.Platforms[i].Passable);
                            write.Write(currentMap.Platforms[i].SpawnOnTop);
                            write.Write(false);
                        }
                        write.Close();
                        //----------------------------------------------------------
                    }

                    break;
            }

        }

        public void UpdateNPCs()
        {
            for (int i = 0; i < nPCs.Count; i++)
            {
                nPCs.ElementAt(i).Value.Update();
            }

            for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
            {
                game.SideQuestManager.nPCs.ElementAt(i).Value.Update();
            }
        }

        public virtual void Draw(SpriteBatch s)
        {
            switch (state)
            {

                #region GAME
                case GameState.Game:
                    currentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    currentMap.Draw(s);
                    DrawNPC(s);
                    currentMap.DrawLivingLocker(s);
                    currentMap.DrawEnemies(s);

                    //--If you are fighting a boss and are in the same map as him, draw him
                    if (BossFight && currentBoss.CurrentMap == currentMap)
                    {
                        currentBoss.Draw(s);
                    }

                    effectsManager.DrawSkillLevelUpEffect(s);

                    if (!talkingToNPC)
                    {
                        effectsManager.DrawFButtons(s);
                        effectsManager.DrawSpaceButtons(s);

                    }
                    player.Draw(s);

                    if (player.LevelingUp)
                        effectsManager.DrawLevelUp(s);

                    currentMap.DrawDrops(s);
                    s.End();

                    //--Map Parallax
                    currentMap.DrawParallaxAndForeground(s);

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                        currentMap.DrawEnemyForegroundEffects(s);
                        currentMap.DrawFlyingEnemies(s);
                        effectsManager.DrawSkillEffects(s);
                        currentMap.DrawEnemyDamage(s);
                        player.DrawDamage(s);
                        currentMap.DrawMapOverlay(s);
                        currentMap.DrawPortalInfo(s);
                    s.End();

                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                    if (!talkingToNPC)
                        effectsManager.DrawForegroundFButtons(s);
                    s.End();

                    //--Static stuff
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);


                    currentMap.DrawMapName(s);

                    if (talkingToNPC == true)
                    {
                        DrawDialogue(s);
                    }
                    else
                    {
                        //--Draw the boss's HUD if you are in his map
                        if (bossFight && currentMap == currentBoss.CurrentMap)
                            currentBoss.DrawHud(s);

                        //HUD
                        hud.Draw(s);

                        //DECISIONS
                        if (makingDecision)
                            effectsManager.DrawDecision(s);
                        else
                        {
                            //NOTIFICATIONS AND OTHER POP UPS
                            effectsManager.DrawDialogue(s);

                            if (!player.LevelingUp)
                            {
                                effectsManager.DrawAnnouncement(s);
                                effectsManager.DrawLockedDoorMessage(s);
                                effectsManager.DrawNotification(s);
                                effectsManager.DrawSkillLevelUpBox(s);
                                effectsManager.DrawFoundItem(s);
                            }

                                effectsManager.DrawLevelUpInfo(s);

                            //TIMERS IN MAPS
                            effectsManager.DrawTimer(s);

                            //TOOLTIPS
                            if (!player.LevelingUp)
                            {
                                effectsManager.DrawToolTip(s);
                            }
                            effectsManager.DrawTextMessage(s);
                        }
                    }

                    cursor.Draw(s);

                    //--This only happens for a single frame
                    //--It happens at the end of a fadeout to the next map, this extra frame is here to prevent the previous
                    //--map from being drawn for a single frame
                    if (currentMap.enteringMap == true)
                    {
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                        currentMap.enteringMap = false;

                        //Draws the troll's textures during the load screen for a single frame, because the initial drawing of these
                        //Causes a slight pause for the first time. Doing it here will basically just increase the loading time for the map,
                        //But will eliminate any gameplay pauses
                        if (LoadTrollTexturesByDrawing)
                        {
                            s.Draw(game.EnemySpriteSheets["Field Troll"], new Rectangle(0, 0, 4096, 4096), Color.White * 0);
                            s.Draw(game.EnemySpriteSheets["TrollFall"], new Rectangle(0, 0, 4096, 4096), Color.White * 0);
                            s.Draw(game.EnemySpriteSheets["TrollAttack"], new Rectangle(0, 0, 4096, 4096), Color.White * 0);
                            s.Draw(game.EnemySpriteSheets["TrollClubGone"], new Rectangle(0, 0, 4096, 4096), Color.White * 0);

                            LoadTrollTexturesByDrawing = false;

                            s.DrawString(Game1.font, "Loading...", new Vector2(1100, 690), Color.White);
                        }

                    }

                    //Screen tint based on Daryl's low health
                    if (player.Health < (player.MaxHealth / 4))
                    {
                        #region Update screen tint
                        if (tintAlphaRising)
                        {
                            tintAlpha += .01f;

                            if (tintAlpha >= .6f)
                            {
                                tintAlpha = .6f;
                                tintAlphaRising = false;
                            }
                        }
                        else
                        {
                            tintAlpha -= .01f;

                            if (tintAlpha <= 0f)
                            {
                                tintAlpha = 0f;
                                tintAlphaRising = true;
                            }
                        }
                        #endregion

                         s.Draw(Game1.lowHealthTint, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * tintAlpha);
                    }

                    s.End();
                    break;
                #endregion

                #region NOTEBOOK
                case GameState.noteBook:

                    currentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    currentMap.Draw(s);
                    DrawNPC(s);
                    currentMap.DrawLivingLocker(s);
                    player.Draw(s);
                    s.End();

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                    currentMap.DrawEnemyForegroundEffects(s);
                    currentMap.DrawFlyingEnemies(s);
                    currentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    currentMap.DrawMapOverlay(s);
                    s.End();

                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                        currentMap.DrawMapOverlay(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Gray * .7f);
                    game.Notebook.Draw(s);

                    //TOOLTIPS
                    effectsManager.DrawToolTip(s);

                    cursor.Draw(s);
                    s.End();
                    break;
                #endregion

                case GameState.loading:
                    //Static stuff
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    //--Then keep the screen black for 20 frames
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);

                    s.DrawString(Game1.font, "Loading...", new Vector2(1100, 690), Color.White);

                    loadingFrameDelay--;

                    if (loadingFrameDelay == 0)
                    {
                        loadingFrame++;
                        loadingFrameDelay = 8;

                        if (loadingFrame == 5)
                            loadingFrame = 0;
                    }

                    s.Draw(Game1.Player.PlayerSheet, new Rectangle(395, (int)220, 530, 398), new Rectangle(1060 + (530 * loadingFrame), 3184, 530, 398), Color.White);

                    s.DrawString(Game1.pickUpFont, game.loadingTips[MapClass.currentLoadingTipNum], new Vector2(640 - Game1.pickUpFont.MeasureString(game.loadingTips[MapClass.currentLoadingTipNum]).X / 2, 200), Color.White);

                    s.End();
                    break;

                #region CHANGING MAPS
                //--When the player is changing maps
                case GameState.ChangingMaps:
                    currentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    //--Draw all the map stuff, player, enemies, etc
                    currentMap.Draw(s);
                    DrawNPC(s);
                    currentMap.DrawLivingLocker(s);
                    currentMap.DrawEnemies(s);
                    player.Draw(s);
                    s.End();

                    //--Map Parallax
                    currentMap.DrawParallaxAndForeground(s);

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                    currentMap.DrawEnemyForegroundEffects(s);
                    currentMap.DrawFlyingEnemies(s);
                    currentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    currentMap.DrawMapOverlay(s);
                    s.End();

                    //Static stuff
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    hud.Draw(s);

                    //--Fade out for 40 frames
                    if (currentMap.leavingMapTimer <= 40)
                        DrawMapFade(s);

                    //--Then keep the screen black for 20 frames
                    if (currentMap.leavingMapTimer > 40 && currentMap.leavingMapTimer <= 60)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);

                    loadingFrameDelay--;

                    if (loadingFrameDelay == 0)
                    {
                        loadingFrame++;
                        loadingFrameDelay = 8;

                        if (loadingFrame == 5)
                            loadingFrame = 0;
                    }

                    float loadingAnimationAlpha = fadeAlpha;

                    if (currentMap.leavingMapTimer > 40)
                        loadingAnimationAlpha = 1f;

                    s.Draw(Game1.Player.PlayerSheet, new Rectangle(395, (int)220, 530, 398), new Rectangle(1060 + (530 * loadingFrame), 3184, 530, 398), Color.White * loadingAnimationAlpha);

                    cursor.Draw(s);
                    if (talkingToNPC == true)
                    {
                        DrawDialogue(s);
                    }

                    //Screen tint based on Daryl's low health
                    if (player.Health < (player.MaxHealth / 4))
                    {
                        s.Draw(Game1.lowHealthTint, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * tintAlpha);
                    }

                    s.DrawString(Game1.font, "Loading...", new Vector2(1100, 690), Color.White);

                    s.DrawString(Game1.pickUpFont, game.loadingTips[MapClass.currentLoadingTipNum], new Vector2(640 - Game1.pickUpFont.MeasureString(game.loadingTips[MapClass.currentLoadingTipNum]).X / 2, 200), Color.White);

                    s.End();
                    break;
                #endregion

                #region PAUSE
                case GameState.Pause:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.StaticTransform);
                    game.Options.Draw(s);
                    cursor.Draw(s);
                    s.End();
                    break;
                #endregion

                #region YOUR LOCKER
                case GameState.YourLocker:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    null, null, null, null, game.Camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawLivingLocker(s);
                    game.CurrentChapter.CurrentMap.DrawEnemies(s);

                    //--If you are fighting a boss and are in the same map as him, draw him
                    if (game.CurrentChapter.BossFight && game.CurrentChapter.CurrentBoss.CurrentMap == game.CurrentChapter.CurrentMap)
                    {
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    }

                    player.Draw(s);

                    game.CurrentChapter.CurrentMap.DrawDrops(s);
                    s.End();

                    //--Map Parallax
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
        null, null, null, null, game.Camera.Transform);
                    game.CurrentChapter.CurrentMap.DrawEnemyForegroundEffects(s);
                    game.CurrentChapter.CurrentMap.DrawFlyingEnemies(s);
                    Chapter.effectsManager.DrawSkillEffects(s);
                    game.CurrentChapter.CurrentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    game.CurrentChapter.CurrentMap.DrawMapOverlay(s);
                    game.CurrentChapter.CurrentMap.DrawPortalInfo(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    game.YourLocker.Draw(s);
                    //TOOLTIPS
                    effectsManager.DrawToolTip(s);
                    cursor.Draw(s);
                    s.End();
                    break;
                #endregion

                #region BREAKING INTO LOCKER
                case GameState.BreakingLocker:
                                        game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    null, null, null, null, game.Camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawLivingLocker(s);
                    game.CurrentChapter.CurrentMap.DrawEnemies(s);

                    //--If you are fighting a boss and are in the same map as him, draw him
                    if (game.CurrentChapter.BossFight && game.CurrentChapter.CurrentBoss.CurrentMap == game.CurrentChapter.CurrentMap)
                    {
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    }

                    player.Draw(s);

                    game.CurrentChapter.CurrentMap.DrawDrops(s);
                    s.End();

                    //--Map Parallax
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
        null, null, null, null, game.Camera.Transform);
                    game.CurrentChapter.CurrentMap.DrawEnemyForegroundEffects(s);
                    game.CurrentChapter.CurrentMap.DrawFlyingEnemies(s);
                    Chapter.effectsManager.DrawSkillEffects(s);
                    game.CurrentChapter.CurrentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    game.CurrentChapter.CurrentMap.DrawMapOverlay(s);
                    game.CurrentChapter.CurrentMap.DrawPortalInfo(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    currentLocker.Draw(s);
                    currentLocker.DrawDescriptions();
                    //TOOLTIPS
                    effectsManager.DrawToolTip(s);
                    cursor.Draw(s);
                    s.End();
                    break;
                #endregion

                #region MAP EDIT
                case GameState.mapEdit:

                    currentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    currentMap.Draw(s);
                    if(selectedPlat != null)
                         selectedPlat.Draw(s);
                    DrawNPC(s);
                    currentMap.DrawEnemies(s);

                    //--If you are fighting a boss and are in the same map as him, draw him
                    if (BossFight && currentBoss.CurrentMap == currentMap)
                    {
                        currentBoss.Draw(s);
                    }

                    player.Draw(s);

                    if (player.LevelingUp)
                        effectsManager.DrawLevelUp(s);

                    currentMap.DrawDrops(s);

                    currentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    s.End();

                    //--Map Parallax
                    currentMap.DrawParallaxAndForeground(s);

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                    currentMap.DrawFlyingEnemies(s);
                    currentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    currentMap.DrawMapOverlay(s);
                    s.End();

                    //--Static stuff
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);


                    if (talkingToNPC == true)
                    {
                        DrawDialogue(s);
                    }
                    else
                    {

                        //HUD
                        hud.Draw(s);

                        //DECISIONS
                        if (makingDecision)
                            effectsManager.DrawDecision(s);
                        else
                        {
                            //NOTIFICATIONS AND OTHER POP UPS
                            effectsManager.DrawDialogue(s);
                            effectsManager.DrawAnnouncement(s);
                            effectsManager.DrawLockedDoorMessage(s);
                            effectsManager.DrawNotification(s);

                            //TIMERS IN MAPS
                            effectsManager.DrawTimer(s);

                            //TOOLTIPS
                            if (!player.LevelingUp)
                            {
                                effectsManager.DrawToolTip(s);
                                effectsManager.DrawTextMessage(s);
                            }
                        }
                    }

                    s.DrawString(Game1.HUDFont, "Map Edit Mode", new Vector2(500, 10), Color.Black);


                    if(spawn)
                        s.Draw(Game1.whiteFilter, spawnButton.ButtonRec, Color.White);
                    else
                        s.Draw(Game1.whiteFilter, spawnButton.ButtonRec, Color.Gray);

                    if(pass)
                        s.Draw(Game1.whiteFilter, passButton.ButtonRec, Color.White);
                    else
                        s.Draw(Game1.whiteFilter, passButton.ButtonRec, Color.Gray);

                    s.Draw(Game1.whiteFilter, platformButton.ButtonRec, Color.White);

                    s.Draw(Game1.whiteFilter, saveButton.ButtonRec, Color.White);

                    s.DrawString(Game1.expMoneyFloatingNumFont, "Spawn", new Vector2(spawnButton.ButtonRecX, 0), Color.Black);
                    s.DrawString(Game1.expMoneyFloatingNumFont, "Passable", new Vector2(passButton.ButtonRecX, 0), Color.Black);
                    s.DrawString(Game1.expMoneyFloatingNumFont, "Plat", new Vector2(platformButton.ButtonRecX, 0), Color.Black);

                    cursor.Draw(s);

                    s.End();
                    break;
                #endregion

                case GameState.dead:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    Game1.deathScreen.Draw(s);

                    //Only draw the cursor before selecting an option
                    if(!Game1.deathScreen.main && !Game1.deathScreen.load)
                        cursor.Draw(s);

                    s.End();
                    break;

                case GameState.shop:

                    currentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    currentMap.Draw(s);
                    DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    //DRAW FLYING ENEMIES AND OVERLAY IN FOREGROUND
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                    currentMap.DrawEnemyForegroundEffects(s);
                    currentMap.DrawFlyingEnemies(s);
                    currentMap.DrawEnemyDamage(s);
                    player.DrawDamage(s);
                    currentMap.DrawMapOverlay(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Gray * .7f);
                    game.Shop.Draw(s);
                    //TOOLTIPS
                    effectsManager.DrawToolTip(s);
                    cursor.Draw(s);
                    s.End();
                    break;

                case GameState.crafting:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                   // game.CraftingMenu.Draw(s);
                    cursor.Draw(s);
                    s.End();
                    break;
            }
            
        }

        public void DrawDialogue(SpriteBatch s)
        {
            for (int i = 0; i < nPCs.Count; i++)
            {
                nPCs.ElementAt(i).Value.DrawDialogue(s);
            }

            for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
            {
                game.SideQuestManager.nPCs.ElementAt(i).Value.DrawDialogue(s);
            }
        }

        public void DrawNPC(SpriteBatch s)
        {
            for (int i = 0; i < nPCs.Count; i++)
            {
                nPCs.ElementAt(i).Value.Draw(s);
            }

            for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
            {
                game.SideQuestManager.nPCs.ElementAt(i).Value.Draw(s);
            }

        }

        public virtual void AddNPCs()
        {

        }

        public void DrawMapFade(SpriteBatch s)
        {
            s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black * fadeAlpha);
        }

        public void MapFade(int length)
        {
            //--First frame, start the fade out. Alpha is 0 for the black square
            if (notFirstFrame == false)
            {
                fadeAlpha = 0;
                fadeTimer = 0;
                fade = true;
            }

            notFirstFrame = true;

            //--Increase alpha as the timer increases
            if (fadeTimer < length)
            {
                fadeAlpha += (float)(1f / length);
            }
            //--At the end, reset the attributes
            else
            {
                notFirstFrame = false;
                fadeTimer = 0;
                fadeAlpha = 0f;
                fade = false;
            }
        }
    }
}
