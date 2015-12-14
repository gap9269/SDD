using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using Newtonsoft.Json;
namespace ISurvived
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SaveLoadManager saveLoadManager;
        MyGamePad myGamePad;
        public static Boolean gamePadConnected = false;
        PassiveManager passiveManager;
        
        public static Game1 g;
        public static float dt;

        public static Random randomNumberGen;

        //Resolution stuff
        List<Vector2> supportedResolutions;
        Vector2 currentResolutionInWindowed;
        int currentResIndex;
        public Boolean IsFullScreen { get { return graphics.IsFullScreen; } }
        List<Vector2> lowResolutions;
        public static Dictionary<String, float> loadedGameSettings;
        public static int screenHeightAspect;

        public static GameTime gameTimeData;
        public SaveLoadManager.SaveGame saveData;

        #region Fonts

        public static SpriteFont twConMedium;
        public static SpriteFont font;
        public static SpriteFont descriptionFont;
        public static SpriteFont HUDFont;
        public static SpriteFont xpHUDFont;
        public static SpriteFont VerySmallTwCondensedFont;
        public static SpriteFont expMoneyFloatingNumFont;
        public static SpriteFont pickUpFont;
        public static SpriteFont questNameFont;
        public static SpriteFont youFoundFont;
        public static SpriteFont smallFont;
        public static SpriteFont smallHUDFont;
        public static SpriteFont dialogueFont;
        public static SpriteFont arcadeFont;
        public static SpriteFont moneyFont;
        public static SpriteFont playerFont;
        public static SpriteFont enemyFont;
        public static SpriteFont enemyFontStrong;
        public static SpriteFont enemyFontWeak;
        public static SpriteFont xpFont;
        public static SpriteFont TwCondensedSmallFont;
        public static SpriteFont skillNameMoireFont;
        public static SpriteFont skillLevelMoireFont;
        public static SpriteFont skillInfoImpactFont;
        public static SpriteFont lockerTextbookFont;
        public static SpriteFont lockerCostFont;
        public static SpriteFont twConLarge;
        public static SpriteFont phoneTextFont;
        public static SpriteFont twConQuestHudInfo;
        public static SpriteFont twConQuestHudName;
        public static SpriteFont twConRegularSmall;
        public static SpriteFont bioPageNameFont;
        public static SpriteFont skillNameSmallerMoireFont, skillLevelSmallerMoireFont, hudStatsFont, hudLevelFont;
        #endregion

        //For NPC face textures
        public struct NPCFace
        {
            public Dictionary<String, Texture2D> faces;
        }

        #region Textures
        public static Texture2D questionMarkFace;
        public static Texture2D newBioNotification;
        public static Texture2D skillDescriptionBox;
        public static Texture2D backspaceTexture;
        public static Texture2D portalTexture;
        public static Texture2D lockedPortalTexture;
        public static Texture2D portalLocker;
        public static Texture2D studentLockerTex;
        public static Texture2D whiteFilter;
        public static Texture2D emptyBox;
        public static Texture2D singleSkillBarBack;
        public static Texture2D questWantedTexture;
        public static Texture2D treasureChestSheet;
        public static Texture2D bangFX1;
        public static Texture2D lockerComboButton;
        public static Texture2D healthDrop;
        public static Texture2D ladderTexture;
        public static Texture2D switchTexture;
        public static Texture2D mapSign;
        public static Texture2D textbookTextures;
        public static Texture2D youFoundItemTexture;
        public static Texture2D petRatSprite;
        public static Texture2D levelUpAnimation;
        public static Texture2D flyingLockerSprite;
        public static Texture2D danceSprite;
        public static Texture2D moneySprite;
        public static Texture2D dustSprite;
        public static Texture2D textbookRay;
        public static Texture2D pitFallTexture;
        public static Texture2D rightBumperTexture;
        public static Texture2D lowHealthTint;
        public static Texture2D jumpPoofSprite;
        public static Texture2D squigglesIcon;
        public static Texture2D sideQuestReceived, storyQuestReceived, sideQuestComplete, storyQuestComplete, storyQuestUpdated, sideQuestUpdated, unlockedNewSkillsTexture;
        public static Texture2D overLockerButton;
        public static Texture2D bioTexture;
        public static Dictionary<string, Texture2D> skillLevelUpTexture;
        public static Texture2D phoneTexture, phoneTextureController;
        public static Texture2D levelUpStatBox;
        public static Texture2D toolTipTexture;
        public static Texture2D journalUpdatedTexture;
        public static Texture2D skillLevelUpBox;
        public static Dictionary<String, Texture2D> smallTypeIcons;
        public static Texture2D lockedDoorMessageTexture;
        public static Texture2D decisionBox;
        public static Texture2D treasureChestFlash;
        public static Texture2D treasureChestBox;
        public static Dictionary<String, Texture2D> socialRankUpTexture;
        public static Texture2D fOuter, fInner, spaceInner, spaceOuter, lbOutline, lbBack, rtOutline, rtBack;
        public static Texture2D equipDescriptionBox, otherDescriptionBox, dualWieldIcon;
        public static Texture2D cutsceneDialogueBox;
        public static Texture2D loadingScreenLogo, loadingScreenRays, loadingScreenText, loadingScreenGradient;
        public static Texture2D skillStar;
        public static Texture2D AButton;
        Texture2D playerSheet;

        //Constant Monster Textures
        Texture2D EHealthBox, EHealthBar, BossHUD, BossHealthBar, FriendHealthBar, FriendHealthBox, ExtraBossHealthBar, scientistSprite, BossLine;

        #endregion

        static Player player;
        DarylsNotebook notebook;
        Shop shop;

        //CraftingMenu craftingMenu;
        YourLocker yourLocker;
        public static Camera camera;
        SkillManager skillManager;
        public static CharacterMonsterBioDictionary characterBioDictionary;
        public static DescriptionBoxManager descriptionBoxManager;
        public static LoadingScreen loadingScreen;

        Dictionary<String, Texture2D> enemySpriteSheets;
        Dictionary<String, Texture2D> npcSprites;
        //static Dictionary<String, MapClass> allMaps;

        public static Dictionary<String, Texture2D> skillIcons;
        Dictionary<String, Texture2D> mapHazards;
        Dictionary<String, Quest> allQuests;
        Dictionary<String, Equipment> allEquipment;
        public static Dictionary<String, SoundEffect> tempHitSounds;

        public static Dictionary<String, int> numberOfNPCWalkingFrames;


        public List<String> loadingTips;

        public static Dictionary<String, Texture2D> skillAnimations;
        public static Dictionary<String, NPCFace> npcFaces;
        public static Dictionary<String, Texture2D> equipmentTextures;
        public static Dictionary<String, Texture2D> platformTextures;
        public static Dictionary<String, Texture2D> storyItems;
        public static Dictionary<String, Texture2D> storyItemIcons;
        public static Dictionary<String, Texture2D> interactiveObjects;

        public static Texture2D projectileTextures;
        public static Texture2D notificationTextures;
        public static Dictionary<String, int> npcHeightFromRecTop;
        Dictionary<String, Texture2D> prologueTextures;
        Dictionary<String, Texture2D> chOneTextures;
        Dictionary<String, Texture2D> chTwoTextures;

        HUD hud;
        public static QuestHud questHUD;

        Cursor cursor;
        public static DeathScreen deathScreen;

        public static Chapter currentChapter;

        public static Dictionary<String, SoundEffectInstance> allSounds;
        public static AllStoryItems allStoryItems;

        public static Boolean spokeThisFrame = false;
        public static int spokeThisFrameNum = 0;

        MainMenu mainMenu;
        OptionsMenu options;
        public static MapBooleans mapBooleans;
        SideQuestManager sideQuestManager;

        List<Quest> currentQuests;
        List<Quest> currentSideQuests;

        Prologue prologue;
        ChapterOne chapterOne;
        ChapterTwo chapterTwo;
        ChapterTwoDEMO chapterTwoDemo;

        //MAP ZONES
        public static SchoolMaps schoolMaps;

        //MAP ZONE WRAPPERS FOR SAVING
        public MapZoneWrapper schoolZoneWrapper;

        public Vector2 res;
        public static float aspectRatio;

        public enum ChapterState
        {
            mainMenu,
            prologue,
            chapterOne,
            chapterTwo,
            demo,
        }
        public ChapterState chapterState;

        #region Properties
        public SideQuestManager SideQuestManager { get { return sideQuestManager; } set { sideQuestManager = value; } }
        public YourLocker YourLocker { get { return yourLocker; } set { yourLocker = value; } }
        public Dictionary<String, Quest> AllQuests { get { return allQuests; } set { allQuests = value; } }
        public Dictionary<String, Texture2D> EnemySpriteSheets { get { return enemySpriteSheets; } set { enemySpriteSheets = value; } }
        public Dictionary<String, Texture2D> EquipmentTextures { get { return equipmentTextures; } }
        public Dictionary<String, Texture2D> NPCSprites { get { return npcSprites; } }
       // public static Dictionary<String, MapClass> AllMaps { get { return allMaps; } set { allMaps = value; } }
        public Dictionary<String, Texture2D> SkillIcons { get { return skillIcons; } set { skillIcons = value; } }
        public Dictionary<String, Texture2D> SkillAnimations { get { return skillAnimations; } set { skillAnimations = value; } }
        public Dictionary<String, Texture2D> MapHazards { get { return mapHazards; } set { mapHazards = value; } }
        public List<Quest> CurrentQuests { get { return currentQuests; } set { currentQuests = value; } }
        public List<Quest> CurrentSideQuests { get { return currentSideQuests; } set { currentSideQuests = value; } }
        public Dictionary<String, Equipment> AllEquipment { get { return allEquipment; } set { allEquipment = value; } }
        public Chapter CurrentChapter { get { return currentChapter; } set { currentChapter = value; } }
        public Prologue Prologue { get { return prologue; } set { prologue = value; } }
        public ChapterOne ChapterOne { get { return chapterOne; } set { chapterOne = value; } }
        public ChapterTwo ChapterTwo { get { return chapterTwo; } set { chapterTwo = value; } }
        public ChapterTwoDEMO ChapterTwoDemo { get { return chapterTwoDemo; } set { chapterTwoDemo = value; } }
        public SkillManager SkillManager { get { return skillManager; } set { skillManager = value; } }
        public DescriptionBoxManager DescriptionBoxManager { get { return descriptionBoxManager; } }
        public Camera Camera { get { return camera; } }
        public static Player Player { get { return player; } }
        public SpriteFont Font { get { return font; } }
        public OptionsMenu Options { get { return options; } }
        public SaveLoadManager SaveLoadManager { get { return saveLoadManager; } set { saveLoadManager = value; } }
        public MapBooleans MapBooleans { get { return mapBooleans; } set { mapBooleans = value; } }
        public DarylsNotebook Notebook { get { return notebook; } set { notebook = value; } }
        public Shop Shop { get { return shop; } set { shop = value; } }

        #endregion

        public KeyboardState current;
        public KeyboardState last;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.Components.Add(new GamerServicesComponent(this));
            myGamePad = new MyGamePad();
            randomNumberGen = new Random();
            //Update the game pad if it is connected
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                gamePadConnected = true;
            }
            else
                gamePadConnected = false;

            g = this;
        }

        public static int screenWidth, screenHeight;

        public void SetResolution(int width, int height)
        {
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;

            screenWidth = width;
            screenHeight = height;

            res = new Vector2(width, height);

            graphics.ApplyChanges();

            aspectRatio = (float)height / (float)width;

            screenHeightAspect = (int)(1280 * aspectRatio);
            //Save in settings file
            SaveSettingsFile();

            Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - screenWidth / 2, (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - screenHeight / 2);

            if (currentChapter != null)
            {
                currentChapter.HUD.UpdateResolution();
                notebook.UpdateGameResolution();
                options.UpdateResolution();
                yourLocker.UpdateResolution();
                shop.UpdateResolution();
            }
            if (chapterState == ChapterState.mainMenu)
            {
                try
                {
                    options.UpdateResolution();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Changes the current game's resolution by moving it up or down one in the list of supported resolutions
        /// </summary>
        public void ChangeCurrentResolution(int indexChange)
        {
            currentResIndex += indexChange;

            if (currentResIndex > supportedResolutions.Count - 1)
                currentResIndex = 0;
            else if (currentResIndex < 0)
                currentResIndex = supportedResolutions.Count - 1;

            //Set the resolution to the new one, and save it in a variable in case they go full screen and back
            SetResolution((int)(supportedResolutions[currentResIndex].X), (int)(supportedResolutions[currentResIndex].Y));
            currentResolutionInWindowed = new Vector2((int)(supportedResolutions[currentResIndex].X), (int)(supportedResolutions[currentResIndex].Y));

        }

        public void ToggleBorderless()
        {
            Window.IsBorderless = !Window.IsBorderless;

            //Save to settings file
            SaveSettingsFile();

        }

        public void MakeFull()
        {
            if (graphics.IsFullScreen == false)
            {
                SetResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();

                //Save to settings file
                SaveSettingsFile();

            }
        }

        public void MakeNotFull()
        {
            if (graphics.IsFullScreen)
            {
                SetResolution((int)currentResolutionInWindowed.X, (int)currentResolutionInWindowed.Y);
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();

                //Save to settings file
                SaveSettingsFile();
            }
        }

        public void PopulateNPCWalkingFrameDictionary()
        {
            numberOfNPCWalkingFrames.Add("Paul", 10);
            numberOfNPCWalkingFrames.Add("Alan", 9);
            numberOfNPCWalkingFrames.Add("Theater Manager", 8);
            numberOfNPCWalkingFrames.Add("Mr. Robatto", 10);
            numberOfNPCWalkingFrames.Add("Tim", 8);
            numberOfNPCWalkingFrames.Add("Balto", 9);
        }

        protected override void Initialize()
        {
            numberOfNPCWalkingFrames = new Dictionary<string, int>();
            PopulateNPCWalkingFrameDictionary();
            characterBioDictionary = new CharacterMonsterBioDictionary();
            camera = new Camera(GraphicsDevice.Viewport);
            enemySpriteSheets = new Dictionary<string, Texture2D>();
            equipmentTextures = new Dictionary<string, Texture2D>();
            chapterState = ChapterState.mainMenu;
            npcFaces = new Dictionary<string, NPCFace>();
            platformTextures = new Dictionary<string, Texture2D>();
            npcSprites = new Dictionary<string, Texture2D>();
            storyItems = new Dictionary<string, Texture2D>();
            storyItemIcons = new Dictionary<string, Texture2D>();
            allSounds = new Dictionary<string, SoundEffectInstance>();
            mapHazards = new Dictionary<string, Texture2D>();
            currentQuests = new List<Quest>();
            currentSideQuests = new List<Quest>();
            allQuests = new Dictionary<string, Quest>();
            allEquipment = new Dictionary<string, Equipment>();
            mapBooleans = new MapBooleans();
            interactiveObjects = new Dictionary<string, Texture2D>();
            npcHeightFromRecTop = new Dictionary<string, int>();

            supportedResolutions = new List<Vector2>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if(mode.Width >= 800)
                    supportedResolutions.Add(new Vector2(mode.Width, mode.Height));
            }
            //Load the game settings
            loadedGameSettings = LoadSettingsFile();

            supportedResolutions = supportedResolutions.Distinct().ToList();

            lowResolutions = new List<Vector2>() { new Vector2(1280, 720), new Vector2(1024, 576), new Vector2(960, 540), new Vector2(864, 486) };

            Vector2 currentComputerResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            //Read resolution in from the settings file
            int savedWidth = (int)loadedGameSettings["width"];
            int savedHeight = (int)loadedGameSettings["height"];

            //Check to see if their computer's resolution is smaller than their saved. If it is, adjust the resolution down to the closest 16:9
            if (currentComputerResolution.X < savedWidth || currentComputerResolution.Y < savedHeight)
            {
                //Loop through the resolutions and see which one is the closest to their computer's, but isn't full screen
                for (int i = 0; i < lowResolutions.Count; i++)
                {
                    savedWidth = (int)lowResolutions[i].X;
                    savedHeight = (int)lowResolutions[i].Y;

                    if (!(currentComputerResolution.X < savedWidth || currentComputerResolution.Y < savedHeight))
                    {
                        currentResolutionInWindowed = new Vector2(savedWidth, savedHeight);
                        break;
                    }
                    //If their resolution is so low that I didn't bother supporting it, just make the game screen their computer's resolution
                    else if (i == lowResolutions.Count - 1)
                    {
                        currentResolutionInWindowed = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                    }
                }
            }
            else
                currentResolutionInWindowed = new Vector2(savedWidth, savedHeight);

            if (loadedGameSettings["fullscreen"] == 1)
            {
                MakeFull();
            }
            else
            {
                SetResolution((int)currentResolutionInWindowed.X, (int)currentResolutionInWindowed.Y);
            }

            if (loadedGameSettings["borderless"] == 1)
            {
                Window.IsBorderless = true;
            }

            //MAP ZONE WRAPPERS
            schoolZoneWrapper = new MapZoneWrapper(true);

            //MAP ZONES
            schoolMaps = new SchoolMaps(this, player);

            loadingTips = new List<string>();
            loadingTips.Add("Press or hold 'SPACE'/'RIGHT TRIGGER' to pick up enemy drops!");
            loadingTips.Add("Hover over skills on the HUD to view their information!");
            loadingTips.Add("Remember to equip items!");
            loadingTips.Add("You can sell Loot/Enemy Drops to the Trenchcoat Employees!");
            loadingTips.Add("Dying a lot? Level up your skills or try a new combination. Use your best equipment, too!");
           // loadingTips.Add("Press ESC to view the game's controls!");

            GenerateLockerCombinations.GenerateLockerCombinationsForSaveFile(); //Generate random locker combos for the student lockers
            base.Initialize();
        }

        public void SetMapBooleans()
        {
            //TUTORIAL
            MapBooleans.tutorialMapBooleans.Add("DestroyedSomeFruit", false);
            MapBooleans.tutorialMapBooleans.Add("DestroyedAllFruit", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipElevenUsed", false);
            MapBooleans.tutorialMapBooleans.Add("BrokeIntoTutorialLocker", false);
            MapBooleans.tutorialMapBooleans.Add("FinishedTutorialLocker", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipThirteenUsed", false);
            MapBooleans.tutorialMapBooleans.Add("AddedDiscussDifferencesToShop", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipOneUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipTwoUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipThreeUsed", false);
            MapBooleans.tutorialMapBooleans.Add("MonsterOneKilled", false);
            MapBooleans.tutorialMapBooleans.Add("MonsterTwoKilled", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialMonstersSpawned", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialSaved", false);
            MapBooleans.tutorialMapBooleans.Add("lawyerTip", false);
            MapBooleans.tutorialMapBooleans.Add("targetsAdded", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipNineUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipTenUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TreeFell", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipFourUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipFiveUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipSixUsed", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipSevenUsed", false);
            MapBooleans.tutorialMapBooleans.Add("JumpedGap", false);
            MapBooleans.tutorialMapBooleans.Add("FellBackInGap", false);
            MapBooleans.tutorialMapBooleans.Add("EquipRemind", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipEightUsed", false);
            MapBooleans.tutorialMapBooleans.Add("YouShouldSave", false);
            MapBooleans.tutorialMapBooleans.Add("ClearedGarden", false);
            MapBooleans.tutorialMapBooleans.Add("TutorialTipTwelveUsed", false);
            MapBooleans.tutorialMapBooleans.Add("FoundTextbook", false);
            MapBooleans.tutorialMapBooleans.Add("LeftWithoutChest", false);
            MapBooleans.tutorialMapBooleans.Add("AddedShop", false);
            MapBooleans.tutorialMapBooleans.Add("TakenClown", false);
            MapBooleans.tutorialMapBooleans.Add("BalloonFloated", false);
            MapBooleans.tutorialMapBooleans.Add("BalloonGone", false);

            //PROLOGUE
            MapBooleans.prologueMapBooleans.Add("spawnedBennys", false);
            MapBooleans.prologueMapBooleans.Add("spawnEnemies", false);
            MapBooleans.prologueMapBooleans.Add("doorsAdded", false);
            MapBooleans.prologueMapBooleans.Add("enemyKilled", false);
            MapBooleans.prologueMapBooleans.Add("enemyAdded", false);
            MapBooleans.prologueMapBooleans.Add("targetsAdded", false);
            MapBooleans.prologueMapBooleans.Add("targets2Added", false);

            //CHAPTER ONE
            MapBooleans.chapterOneMapBooleans.Add("scienceTargetsAdded", false);
            MapBooleans.chapterOneMapBooleans.Add("scienceChallengeStarted", false);
            MapBooleans.chapterOneMapBooleans.Add("scienceChallengeCompleted", false); 
            MapBooleans.chapterOneMapBooleans.Add("TrapDoorOpened", false);
            MapBooleans.chapterOneMapBooleans.Add("lockedUpperVentIVDoors", false);
            MapBooleans.chapterOneMapBooleans.Add("upperVentIVClear", false);
            MapBooleans.chapterOneMapBooleans.Add("upperVentIVSpawned", false);
            MapBooleans.chapterOneMapBooleans.Add("VentLowered", false);
            MapBooleans.chapterOneMapBooleans.Add("enteredUpperVentChallengeRoom", false);
            MapBooleans.chapterOneMapBooleans.Add("upperVentChallengeClear", false);
            MapBooleans.chapterOneMapBooleans.Add("upperVentChallengeSpawned", false);
            MapBooleans.chapterOneMapBooleans.Add("sideVentDoorsLocked", false);
            MapBooleans.chapterOneMapBooleans.Add("sideVentDoorsUnlocked", false);

            //CHAPTER TWO
            MapBooleans.chapterTwoMapBooleans.Add("FoundHandsaw", false);
            MapBooleans.chapterTwoMapBooleans.Add("FoundPeltHat", false);
            MapBooleans.chapterTwoMapBooleans.Add("SpawnedScarecrows", false);
            MapBooleans.chapterTwoMapBooleans.Add("KilledFirstScare", false);
            MapBooleans.chapterTwoMapBooleans.Add("KilledSecondScare", false);
            MapBooleans.chapterTwoMapBooleans.Add("ClearedSpookyField", false);
            MapBooleans.chapterTwoMapBooleans.Add("ClearedWorkersField", false);
            MapBooleans.chapterTwoMapBooleans.Add("TurnedLaserOneOff", false);
            MapBooleans.chapterTwoMapBooleans.Add("TurnedLaserTwoOff", false);
            MapBooleans.chapterTwoMapBooleans.Add("TurnedLaserThreeOff", false);
            MapBooleans.chapterTwoMapBooleans.Add("HoleCovered", false);
            MapBooleans.chapterTwoMapBooleans.Add("explosionHitFrame10", false);
            MapBooleans.chapterTwoMapBooleans.Add("gateHitHalfHealth", false);
            
        }

        //This is called when a new save file is made, and only once
        public void CreateNewSaveFileMapData()
        {
            schoolMaps.LoadSchoolZone();
            schoolMaps.PopulateSaveDataOnNewSaveSlot();
            schoolMaps.SaveMapDataToWrapper();
        }

        public void TempPlayHitSound(int originx = int.MaxValue, int originy = int.MaxValue)
        {
            int num = randomNumberGen.Next(3);
            SoundEffect sound = tempHitSounds.ElementAt(num).Value;
            Sound.PlaySoundInstance(sound, tempHitSounds.ElementAt(num).Key, false, originx, originy, 600, 500, 2000);
        }

        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SetMapBooleans();
            passiveManager = new PassiveManager(this);
            SocialRankManager socialRankManager = new SocialRankManager();

            tempHitSounds = ContentLoader.LoadSoundContent(Content, "Sound\\Skills\\Discuss Differences\\HitSounds");

            whiteFilter = Content.Load<Texture2D>("whiteFilter");
            emptyBox = whiteFilter;
            lowHealthTint = Content.Load<Texture2D>(@"FX\LowHealthTint");
            AButton = Content.Load<Texture2D>(@"Dialogue\AButton");

            #region Equipment
            equipmentTextures = ContentLoader.LoadContent(Content, "Equipment");
            smallTypeIcons = ContentLoader.LoadContent(Content, "Equipment\\SmallTypeIcons");

            #region Weapons
            allEquipment.Add("Dried Out Marker", new Marker());
            allEquipment.Add("Coal Shovel", new CoalShovel());
            allEquipment.Add("Conductor's Wand", new ComposersWand());
            allEquipment.Add("Melon-Mashing Mallet", new MelonMallet());
            allEquipment.Add("Dirty Broken Hoe", new DirtyBrokenHoe());
            allEquipment.Add("Hand Saw", new HandSaw());
            allEquipment.Add("Paintbrush of Destruction", new Paintbrush());
            allEquipment.Add("Average Weenie Protractor", new AverageWeenieProtractor());
            allEquipment.Add("Super Weenie Sword", new SuperWeenieSword());
            allEquipment.Add("Chisel of Forgotten Love", new ChiselOfForgottenLove());
            allEquipment.Add("Cardboard Sword", new CardboardSword());
            allEquipment.Add("Tree Branch", new TreeBranch());
            #endregion

            #region Hats
            allEquipment.Add("Dunce Cap", new DunceCap());
            allEquipment.Add("Band Hat", new BandHat());
            allEquipment.Add("Fez", new Fez());
            allEquipment.Add("Powdered Wig", new PowderedWig());
            allEquipment.Add("Gardening Hat", new GardeningHat());
            allEquipment.Add("Party Hat", new PartyHat());
            allEquipment.Add("Scarecrow Hat", new ScarecrowHat());
            allEquipment.Add("Super Weenie Hat", new SuperWeenieHat());
            allEquipment.Add("Average Weenie Hat", new AverageWeenieHat());
            allEquipment.Add("Lucky Beret", new Beret());
            allEquipment.Add("Pelt Kid's Hat", new PeltKidsHat());
            allEquipment.Add("Nurse Hat", new NurseHat());
            allEquipment.Add("Cardboard Hat", new CardboardHat());
            allEquipment.Add("Mikrov's Eye Patch", new MikrovsEyePatch());
            allEquipment.Add("Tree Top", new TreeTop());

            #endregion

            #region Outfits
            allEquipment.Add("Lab Coat", new LabCoat());
            allEquipment.Add("Band Uniform", new BandUniform());
            allEquipment.Add("'I Love Melons' Band Tee", new ILoveMelons());
            allEquipment.Add("'I Hate Melons' Band Tee", new IHateMelons());
            allEquipment.Add("Scarecrow Vest", new ScarecrowVest());
            allEquipment.Add("Toga", new Toga());
            allEquipment.Add("Smock of Desperation", new ArtSmock());
            allEquipment.Add("Super Weenie Shirt", new SuperWeenieShirt());
            allEquipment.Add("Average Weenie Shirt", new AverageWeenieShirt());
            allEquipment.Add("Cape of Count Roger", new CapeOfCountRoger());
            allEquipment.Add("Cardboard Shirt", new CardboardShirt());
            allEquipment.Add("Loincloth", new LoinCloth());
            allEquipment.Add("Tree Trunk", new TreeTrunk());

            #endregion

            #region Accessories
            allEquipment.Add("Riley's Bow", new RileysBow());
            allEquipment.Add("Uranium Rod", new UraniumRod());
            allEquipment.Add("Lab Goggles", new LabGoggles());
            allEquipment.Add("Used Tissue", new UsedTissue());
            allEquipment.Add("Yin-Yang Necklace", new YinYangNecklace());
            allEquipment.Add("Garlic Necklace", new GarlicNecklace());
            allEquipment.Add("Jar of Dirt", new JarOfDirt());
            allEquipment.Add("Beer Goggles", new BeerGoggles());
            allEquipment.Add("Solo Cup", new SoloCup());
            allEquipment.Add("Average Weenie Calculator", new AverageWeenieCalculator());
            allEquipment.Add("Super Weenie Purse", new SuperWeeniePurse());
            allEquipment.Add("Artist's Palette", new ArtistsPalette());
            allEquipment.Add("Venus of Willendorf", new VenusOfWillendorf());
            allEquipment.Add("Eighth of Luck Butter", new EighthOfButter());
            allEquipment.Add("Fourth of Luck Butter", new QuarterOfButter());
            allEquipment.Add("Goblin Toe", new GoblinToe());
            allEquipment.Add("Automatic First-Aid", new AutomaticFirstAid());
            allEquipment.Add("Monkey's Paw", new MonkeysPaw());
            allEquipment.Add("Cardboard", new Cardboard());
            #endregion

            #endregion

            #region Fonts
            font = this.Content.Load<SpriteFont>("Fonts\\font");
            descriptionFont = this.Content.Load<SpriteFont>("Fonts\\DescriptionBoxFont");

            skillLevelMoireFont = Content.Load<SpriteFont>("Fonts\\SkillLevelMoireFont");
            skillLevelSmallerMoireFont = Content.Load<SpriteFont>("Fonts\\SkillLevelSmallerMoireFont");
            skillNameMoireFont = Content.Load<SpriteFont>("Fonts\\SkillNameMoireFont");
            skillInfoImpactFont = Content.Load<SpriteFont>("Fonts\\SkillInfoImpactFont");

            expMoneyFloatingNumFont = Content.Load<SpriteFont>("Fonts\\ExpMoneyFloatingNums");
            pickUpFont = Content.Load<SpriteFont>("Fonts\\PickUpItem");
            questNameFont = Content.Load<SpriteFont>("Fonts\\QuestName");
            youFoundFont = Content.Load<SpriteFont>("Fonts\\YouFoundFont");
            smallFont = Content.Load<SpriteFont>("Fonts\\SmallerHUDFont");
            smallHUDFont = Content.Load<SpriteFont>("Fonts\\SmallHUDFont");
            smallHUDFont.Spacing = -27;
            dialogueFont = Content.Load<SpriteFont>("Fonts\\DialogueFont");
            arcadeFont = Content.Load<SpriteFont>("Fonts\\ArcadeFont");
            skillNameSmallerMoireFont = Content.Load<SpriteFont>("Fonts\\SkillNameSmallerMoireFont");

            hudStatsFont = Content.Load<SpriteFont>("Fonts\\HUDStatsFont");
            hudLevelFont = Content.Load<SpriteFont>("Fonts\\HUDLevelFont");

            HUDFont = Content.Load<SpriteFont>("Fonts\\HUDFont");
            HUDFont.Spacing = -24;

            xpHUDFont = Content.Load<SpriteFont>("Fonts\\XPHUDFont");
            xpHUDFont.Spacing = -29;

            moneyFont = Content.Load<SpriteFont>("Fonts\\MoneyFont1");
            moneyFont.Spacing = -18; //-5?

            playerFont = Content.Load<SpriteFont>("Fonts\\PlayerFont");
            playerFont.Spacing = -13;

            enemyFont = Content.Load<SpriteFont>("Fonts\\EnemyFont");
            enemyFont.Spacing = -16;

            enemyFontStrong = Content.Load<SpriteFont>("Fonts\\EnemyFontStrong");
            enemyFontStrong.Spacing = -16;

            enemyFontWeak = Content.Load<SpriteFont>("Fonts\\EnemyFontWeak");
            enemyFontWeak.Spacing = -16;

            xpFont = Content.Load<SpriteFont>("Fonts\\XPFont");
            xpFont.Spacing = -16;

            TwCondensedSmallFont = Content.Load<SpriteFont>("Fonts\\SmallerHUDfont");
            VerySmallTwCondensedFont = Content.Load<SpriteFont>("Fonts\\TwCondensedVerySmall");
            lockerTextbookFont = Content.Load<SpriteFont>("Fonts\\LockerTextbookFont");
            lockerCostFont = Content.Load<SpriteFont>("Fonts\\LockerCostFont");
            twConLarge = Content.Load<SpriteFont>("Fonts\\TWConBig");
            twConMedium = Content.Load<SpriteFont>("Fonts\\TWConMedium");

            phoneTextFont = Content.Load<SpriteFont>("Fonts\\PhoneTextFont");

            twConQuestHudInfo = Content.Load<SpriteFont>("Fonts\\TwCondensedQuestHudInfo");
            twConQuestHudName = Content.Load<SpriteFont>("Fonts\\TwCondensedQuestHudName");

            twConRegularSmall = Content.Load<SpriteFont>("Fonts\\TwConRegularSmall");
            bioPageNameFont = Content.Load<SpriteFont>("Fonts\\BioPageName");
            #endregion

            #region Sounds
            //Background music content manager
            Sound.backgroundMusicContent = new ContentManager(g.Services);
            Sound.backgroundMusicContent.RootDirectory = "Content";

            //Permanent sound content manager
            Sound.permanentContent = new ContentManager(g.Services);
            Sound.permanentContent.RootDirectory = "Content";


            //Notebook content manager
            Sound.menuContent = new ContentManager(g.Services);
            Sound.menuContent.RootDirectory = "Content";

            //Ambience
            Sound.ambienceContent = new ContentManager(g.Services);
            Sound.ambienceContent.RootDirectory = "Content";

            //Map Zones
            Sound.mapZoneContent = new ContentManager(g.Services);
            Sound.mapZoneContent.RootDirectory = "Content";

            Sound.LoadPermanentContent();

            #endregion

            #region Notifications & Pop Ups
            skillDescriptionBox = Content.Load<Texture2D>(@"Notifications\skillDescription");
            notificationTextures = Content.Load<Texture2D>(@"Notifications\NotificationSprites");
            journalUpdatedTexture = Content.Load<Texture2D>(@"Notifications\NewJournalEntry");
            toolTipTexture = Content.Load<Texture2D>(@"Notifications\ToolTip");
            skillLevelUpBox = Content.Load<Texture2D>(@"Notifications\SkillLevelUp");
            youFoundItemTexture = Content.Load<Texture2D>(@"Notifications\ItemPickedUp");
            lockedDoorMessageTexture = Content.Load<Texture2D>(@"Notifications\LockedDoor");
            decisionBox = Content.Load<Texture2D>(@"Notifications\decisionBox");
            storyQuestComplete = Content.Load<Texture2D>(@"Notifications\StoryQuestComplete");
            sideQuestComplete = Content.Load<Texture2D>(@"Notifications\SideQuestComplete");
            sideQuestUpdated = Content.Load<Texture2D>(@"Notifications\SideQuestUpdated");
            storyQuestUpdated = Content.Load<Texture2D>(@"Notifications\StoryQuestUpdated");
            socialRankUpTexture = ContentLoader.LoadContent(Content, "Notifications\\Social Rank Up");
            newBioNotification = Content.Load<Texture2D>(@"Notifications\newBio");
            fOuter = Content.Load<Texture2D>(@"Notifications\fOuter");
            fInner = Content.Load<Texture2D>(@"Notifications\fInner");
            lbOutline = Content.Load<Texture2D>(@"Notifications\LBOutline");
            lbBack = Content.Load<Texture2D>(@"Notifications\LBBack");
            rtBack = Content.Load<Texture2D>(@"Notifications\RTOutline");
            rtOutline = Content.Load<Texture2D>(@"Notifications\RTBack");
            spaceOuter = Content.Load<Texture2D>(@"Notifications\spaceOuter");
            spaceInner = Content.Load<Texture2D>(@"Notifications\spaceInner");
            cutsceneDialogueBox = Content.Load<Texture2D>(@"Notifications\cutsceneDialogueBox");
            unlockedNewSkillsTexture = Content.Load<Texture2D>(@"Notifications\unlockedNewSkillsTexture");
            equipDescriptionBox = Content.Load<Texture2D>(@"Notifications\equipDescriptionBox");
            otherDescriptionBox = Content.Load<Texture2D>(@"Notifications\otherDescriptionBox");
            dualWieldIcon = Content.Load<Texture2D>(@"Notifications\DualWieldIcon");
            #endregion

            #region Effects
            EffectsManager.deathSpriteSheet = Content.Load<Texture2D>(@"FX\DeathSheet");
            #endregion

            #region NPC Faces
            questionMarkFace = Content.Load<Texture2D>(@"NPCFaces\question mark");

            NPCFace dandelionFace = new NPCFace();
            dandelionFace.faces = new Dictionary<string, Texture2D>();
            dandelionFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Dandelion", dandelionFace);

            NPCFace tricerFace = new NPCFace();
            tricerFace.faces = new Dictionary<string, Texture2D>();
            tricerFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Triceratops", tricerFace);

            NPCFace flowerFace = new NPCFace();
            flowerFace.faces = new Dictionary<string, Texture2D>();
            flowerFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Flower God", flowerFace);

            NPCFace robattoFace = new NPCFace();
            robattoFace.faces = new Dictionary<string, Texture2D>();
            robattoFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Mr. Robatto", robattoFace);

            #region --MAIN
            NPCFace alanFace = new NPCFace();
            alanFace.faces = new Dictionary<string, Texture2D>();
            alanFace.faces.Add("Normal", whiteFilter);
            alanFace.faces.Add("Arrogant", whiteFilter);
            alanFace.faces.Add("Tutorial", whiteFilter);
            npcFaces.Add("Alan", alanFace);

            NPCFace paulFace = new NPCFace();
            paulFace.faces = new Dictionary<string, Texture2D>();
            paulFace.faces.Add("Normal", whiteFilter);
            paulFace.faces.Add("Arrogant", whiteFilter);
            paulFace.faces.Add("Tutorial", whiteFilter);
            paulFace.faces.Add("Fonz", whiteFilter);
            npcFaces.Add("Paul", paulFace);

            NPCFace princessFace = new NPCFace();
            princessFace.faces = new Dictionary<string, Texture2D>();
            princessFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("The Princess", princessFace);

            NPCFace janitorFace = new NPCFace();
            janitorFace.faces = new Dictionary<string, Texture2D>();
            janitorFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("The Janitor", janitorFace);

            NPCFace taskFace = new NPCFace();
            taskFace.faces = new Dictionary<string, Texture2D>();
            taskFace.faces.Add("Alan", whiteFilter);
            taskFace.faces.Add("Paul", whiteFilter);
            taskFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Employee Task List", taskFace);

            NPCFace timFace = new NPCFace();
            timFace.faces = new Dictionary<string, Texture2D>();
            timFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Tim", timFace);

            NPCFace markFace = new NPCFace();
            markFace.faces = new Dictionary<string, Texture2D>();
            markFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Mark", markFace);

            npcFaces.Add("Balto", timFace);

            NPCFace gardenerFace = new NPCFace();
            gardenerFace.faces = new Dictionary<string, Texture2D>();
            gardenerFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("The Gardener", gardenerFace);

            NPCFace bobFace = new NPCFace();
            bobFace.faces = new Dictionary<string, Texture2D>();
            bobFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Bob the Construction Guy", bobFace);

            NPCFace trenchFace = new NPCFace();
            trenchFace.faces = new Dictionary<string, Texture2D>();
            trenchFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Trenchcoat Employee", trenchFace);

            NPCFace chelseaface = new NPCFace();
            chelseaface.faces = new Dictionary<string, Texture2D>();
            chelseaface.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Chelsea", chelseaface);

            NPCFace deathFace = new NPCFace();
            deathFace.faces = new Dictionary<string, Texture2D>();
            deathFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Death", chelseaface);

            NPCFace journalEnthusiastFace = new NPCFace();
            journalEnthusiastFace.faces = new Dictionary<string, Texture2D>();
            journalEnthusiastFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Journal Enthusiast", journalEnthusiastFace);

            NPCFace honorsCronyFace = new NPCFace();
            honorsCronyFace.faces = new Dictionary<string, Texture2D>();
            honorsCronyFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Portal Repair Specialist", honorsCronyFace);

            #endregion

            #region --D&D Dorks

            NPCFace karmaFace = new NPCFace();
            karmaFace.faces = new Dictionary<string, Texture2D>();
            karmaFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Karma Shaman", karmaFace);
            

            NPCFace skillFace = new NPCFace();
            skillFace.faces = new Dictionary<string, Texture2D>();
            skillFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Skill Sorceress", skillFace);


            NPCFace equipFace = new NPCFace();
            equipFace.faces = new Dictionary<string, Texture2D>();
            equipFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Weapons Master", equipFace);


            NPCFace saveFace = new NPCFace();
            saveFace.faces = new Dictionary<string, Texture2D>();
            saveFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Saving Instructor", saveFace);


            NPCFace journalFace = new NPCFace();
            journalFace.faces = new Dictionary<string, Texture2D>();
            journalFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Keeper of the Quests", journalFace);

            NPCFace mapsFace = new NPCFace();
            mapsFace.faces = new Dictionary<string, Texture2D>();
            mapsFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("The Magister of Maps", mapsFace);
            #endregion

            #region TITS
            NPCFace jasonFace = new NPCFace();
            jasonFace.faces = new Dictionary<string, Texture2D>();
            jasonFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Jason Mysterio", jasonFace);

            NPCFace claireFace = new NPCFace();
            claireFace.faces = new Dictionary<string, Texture2D>();
            claireFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Claire Voyant", claireFace);

            NPCFace pantskiFace = new NPCFace();
            pantskiFace.faces = new Dictionary<string, Texture2D>();
            pantskiFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Steve Pantski", pantskiFace);

            NPCFace kenface = new NPCFace();
            kenface.faces = new Dictionary<string, Texture2D>();
            kenface.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Ken Speercy", kenface);
            #endregion

            #region KICKSTARTER
            NPCFace drewFace = new NPCFace();
            drewFace.faces = new Dictionary<string, Texture2D>();
            drewFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Drew", drewFace);

            NPCFace colinFace = new NPCFace();
            colinFace.faces = new Dictionary<string, Texture2D>();
            colinFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Chef Flex", colinFace);

            NPCFace roger = new NPCFace();
            roger.faces = new Dictionary<string, Texture2D>();
            roger.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Count Roger", roger);

            //Side Quest
            NPCFace furnaceFace = new NPCFace();
            furnaceFace.faces = new Dictionary<string, Texture2D>();
            furnaceFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Furnace Man", furnaceFace);
            #endregion

            #region Music
            NPCFace beethovenFace = new NPCFace();
            beethovenFace.faces = new Dictionary<string, Texture2D>();
            beethovenFace.faces.Add("Normal", whiteFilter);
            beethovenFace.faces.Add("Horn", whiteFilter);
            npcFaces.Add("Beethoven", beethovenFace);

            NPCFace theaterManagerFace = new NPCFace();
            theaterManagerFace.faces = new Dictionary<string, Texture2D>();
            theaterManagerFace.faces.Add("Normal", whiteFilter);
            theaterManagerFace.faces.Add("Sneer", whiteFilter);
            npcFaces.Add("Theater Manager", theaterManagerFace);

            NPCFace mozartFace = new NPCFace();
            mozartFace.faces = new Dictionary<string, Texture2D>();
            mozartFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Mozart", mozartFace);

            NPCFace tchaikovskyFace = new NPCFace();
            tchaikovskyFace.faces = new Dictionary<string, Texture2D>();
            tchaikovskyFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Tchaikovsky", tchaikovskyFace);

            NPCFace daVinciFace = new NPCFace();
            daVinciFace.faces = new Dictionary<string, Texture2D>();
            daVinciFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Leonardo Da Vinci", daVinciFace);

            NPCFace claysmithFace = new NPCFace();
            claysmithFace.faces = new Dictionary<string, Texture2D>();
            claysmithFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Town Claysmith", claysmithFace);

            NPCFace artDealerFace = new NPCFace();
            artDealerFace.faces = new Dictionary<string, Texture2D>();
            artDealerFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Percy von Lugsworth", artDealerFace);

            NPCFace warholFace = new NPCFace();
            warholFace.faces = new Dictionary<string, Texture2D>();
            warholFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Andy Warhol", warholFace);
            #endregion 

            #region HISTORY
            NPCFace cleoFace = new NPCFace();
            cleoFace.faces = new Dictionary<string, Texture2D>();
            cleoFace.faces.Add("Normal", whiteFilter);
            cleoFace.faces.Add("Chained", whiteFilter);
            npcFaces.Add("Cleopatra", cleoFace);

            NPCFace napoleonFace = new NPCFace();
            napoleonFace.faces = new Dictionary<string, Texture2D>();
            napoleonFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Napoleon", napoleonFace);

            NPCFace fSoldierFace = new NPCFace();
            fSoldierFace.faces = new Dictionary<string, Texture2D>();
            fSoldierFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("French Soldier", fSoldierFace);
            npcFaces.Add("Private Brian", fSoldierFace);

            NPCFace genghisFace = new NPCFace();
            genghisFace.faces = new Dictionary<string, Texture2D>();
            genghisFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Genghis", genghisFace);

            npcFaces.Add("Messenger Boy", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Mikrov", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Pharaoh Guard", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Chained Pharaoh Guard", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("The Pyramid Gardener", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Henry Horus", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("King Hasbended", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Time Lord", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Pastor Goblin", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });

            NPCFace larreyFace = new NPCFace();
            larreyFace.faces = new Dictionary<string, Texture2D>();
            larreyFace.faces.Add("Normal", whiteFilter);
            larreyFace.faces.Add("Goblin", whiteFilter);
            npcFaces.Add("Dr. Dominique Jean Larrey", larreyFace);
            #endregion

            #region Literature
            npcFaces.Add("Santa Claus", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Gary R. Pigeon", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Poole", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Jacob Marley", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Literature Guardian", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter } } });
            npcFaces.Add("Ebenezer Scrooge", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter }, { "Scared", whiteFilter } } });
            npcFaces.Add("Bell Man", new NPCFace() { faces = new Dictionary<string, Texture2D>() { { "Normal", whiteFilter }} });

            #endregion

            #region Party
            NPCFace peltKidFace = new NPCFace();
            peltKidFace.faces = new Dictionary<string, Texture2D>();
            peltKidFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Pelt Kid", peltKidFace);

            NPCFace juliusFace = new NPCFace();
            juliusFace.faces = new Dictionary<string, Texture2D>();
            juliusFace.faces.Add("Normal", whiteFilter);
            juliusFace.faces.Add("Naked", whiteFilter);
            juliusFace.faces.Add("Helmet", whiteFilter);
            npcFaces.Add("Julius Caesar", juliusFace);


            NPCFace squirrelFace = new NPCFace();
            squirrelFace.faces = new Dictionary<string, Texture2D>();
            squirrelFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Squirrel Boy", squirrelFace);

            NPCFace jesseface = new NPCFace();
            jesseface.faces = new Dictionary<string, Texture2D>();
            jesseface.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Jesse", jesseface);

            NPCFace blursoFace = new NPCFace();
            blursoFace.faces = new Dictionary<string, Texture2D>();
            blursoFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Blurso", blursoFace);

            NPCFace steveFace = new NPCFace();
            steveFace.faces = new Dictionary<string, Texture2D>();
            steveFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Steve", steveFace);

            #endregion

            #region MISC
            NPCFace chadFace = new NPCFace();
            chadFace.faces = new Dictionary<string, Texture2D>();
            chadFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Chad Champson", chadFace);

            //TUTORIAL
            NPCFace demoDannyFace = new NPCFace();
            demoDannyFace.faces = new Dictionary<string, Texture2D>();
            demoDannyFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Demo Danny", demoDannyFace);


            NPCFace friendFace = new NPCFace();
            friendFace.faces = new Dictionary<string, Texture2D>();
            friendFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Your Friend", friendFace);


            NPCFace lawyerFace = new NPCFace();
            lawyerFace.faces = new Dictionary<string, Texture2D>();
            lawyerFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Copyright Lawyer", lawyerFace);

            npcFaces.Add("Callyn", robattoFace);
            #endregion

            #endregion

            #region NPCs
            //DEMO/TUTORIAL
            npcSprites.Add("Your Friend", whiteFilter);
            npcHeightFromRecTop.Add("Your Friend", 117);

            npcSprites.Add("PaulBad", whiteFilter);
            npcHeightFromRecTop.Add("PaulBad", 135);

            npcSprites.Add("AlanBad", whiteFilter);
            npcHeightFromRecTop.Add("AlanBad", 125);

            npcSprites.Add("TrenchcoatBad", whiteFilter);
            npcHeightFromRecTop.Add("TrenchcoatBad", 152);

            npcSprites.Add("Employee Task List", whiteFilter);
            npcHeightFromRecTop.Add("Employee Task List", 147);

            //Main characters
            npcSprites.Add("Alan", whiteFilter);
            npcHeightFromRecTop.Add("Alan", 125);
            npcSprites.Add("Paul", whiteFilter);
            npcHeightFromRecTop.Add("Paul", 135);
            npcSprites.Add("Mark", whiteFilter);
            npcHeightFromRecTop.Add("Mark", 50);
            npcSprites.Add("The Princess", whiteFilter);
            npcHeightFromRecTop.Add("The Princess", 163);
            npcSprites.Add("The Janitor", whiteFilter);
            npcHeightFromRecTop.Add("The Janitor", 104);
            npcSprites.Add("Mr. Robatto",whiteFilter);
            npcHeightFromRecTop.Add("Mr. Robatto", 80);
            npcSprites.Add("The Gardener", whiteFilter);
            npcHeightFromRecTop.Add("The Gardener", 185);
            npcSprites.Add("Trenchcoat Employee", whiteFilter);
            npcHeightFromRecTop.Add("Trenchcoat Employee", 152);
            npcSprites.Add("Tim", whiteFilter);
            npcHeightFromRecTop.Add("Tim", 152);

            npcSprites.Add("Bob the Construction Guy", whiteFilter);
            npcHeightFromRecTop.Add("Bob the Construction Guy", 127);

            npcSprites.Add("Balto", whiteFilter);
            npcHeightFromRecTop.Add("Balto", 152);

            npcSprites.Add("Death", whiteFilter);
            npcHeightFromRecTop.Add("Death", 44);

            npcSprites.Add("Journal Enthusiast", whiteFilter);
            npcHeightFromRecTop.Add("Journal Enthusiast", 167);

            npcSprites.Add("Portal Repair Specialist", whiteFilter);
            npcHeightFromRecTop.Add("Portal Repair Specialist", 119);

            //--D&D Characters
            npcSprites.Add("Karma Shaman", whiteFilter);
            npcHeightFromRecTop.Add("Karma Shaman", 145);
            npcSprites.Add("Weapons Master", whiteFilter);
            npcHeightFromRecTop.Add("Weapons Master", 120);
            npcSprites.Add("Keeper of the Quests", whiteFilter);
            npcHeightFromRecTop.Add("Keeper of the Quests", 140);
            npcSprites.Add("Saving Instructor", whiteFilter);
            npcHeightFromRecTop.Add("Saving Instructor", 167);
            npcSprites.Add("Skill Sorceress", whiteFilter);
            npcHeightFromRecTop.Add("Skill Sorceress", 125);
            npcSprites.Add("The Magister of Maps", whiteFilter);
            npcHeightFromRecTop.Add("The Magister of Maps", 182);

            //TITS
            npcSprites.Add("Jason Mysterio", whiteFilter);
            npcHeightFromRecTop.Add("Jason Mysterio", 130);
            npcSprites.Add("Claire Voyant", whiteFilter);
            npcHeightFromRecTop.Add("Claire Voyant", 170);
            npcSprites.Add("Ken Speercy", whiteFilter);
            npcHeightFromRecTop.Add("Ken Speercy", 118);
            npcSprites.Add("Steve Pantski", whiteFilter);
            npcHeightFromRecTop.Add("Steve Pantski", 160);

            //Kickstarter
            npcSprites.Add("Drew", whiteFilter);
            npcHeightFromRecTop.Add("Drew", 117);
            npcSprites.Add("Chef Flex", whiteFilter);
            npcHeightFromRecTop.Add("Chef Flex", 102);
            npcSprites.Add("Count Roger", whiteFilter);
            npcHeightFromRecTop.Add("Count Roger", 127);

            //Side Quest
            npcSprites.Add("Furnace Man", whiteFilter);
            npcHeightFromRecTop.Add("Furnace Man", 100);

            //Music
            npcSprites.Add("Beethoven", whiteFilter);
            npcHeightFromRecTop.Add("Beethoven", 142);
            npcSprites.Add("Mozart", whiteFilter);
            npcHeightFromRecTop.Add("Mozart", 192);
            npcSprites.Add("Tchaikovsky", whiteFilter);
            npcHeightFromRecTop.Add("Tchaikovsky", 130);
            npcSprites.Add("Theater Manager", whiteFilter);
            npcHeightFromRecTop.Add("Theater Manager", 163);
            npcSprites.Add("Leonardo Da Vinci", whiteFilter);
            npcHeightFromRecTop.Add("Leonardo Da Vinci", 116);
            npcSprites.Add("Town Claysmith", whiteFilter);
            npcHeightFromRecTop.Add("Town Claysmith", 158);
            npcSprites.Add("Percy von Lugsworth", whiteFilter);
            npcHeightFromRecTop.Add("Percy von Lugsworth", 128);
            npcSprites.Add("Andy Warhol", whiteFilter);
            npcHeightFromRecTop.Add("Andy Warhol", 124);
            //History
            npcSprites.Add("Cleopatra", whiteFilter);
            npcHeightFromRecTop.Add("Cleopatra", 88);
            npcSprites.Add("Napoleon", whiteFilter);
            npcHeightFromRecTop.Add("Napoleon", 116);
            npcSprites.Add("French Soldier", whiteFilter);
            npcHeightFromRecTop.Add("French Soldier", 92);
            npcSprites.Add("Genghis", whiteFilter);
            npcHeightFromRecTop.Add("Genghis", 40);
            npcSprites.Add("Private Brian", whiteFilter);
            npcHeightFromRecTop.Add("Private Brian", 92);
            npcSprites.Add("Mikrov", whiteFilter);
            npcHeightFromRecTop.Add("Mikrov", 176);
            npcSprites.Add("Messenger Boy", whiteFilter);
            npcHeightFromRecTop.Add("Messenger Boy", 176);
            npcSprites.Add("Pharaoh Guard", whiteFilter);
            npcHeightFromRecTop.Add("Pharaoh Guard", 136);
            npcSprites.Add("Chained Pharaoh Guard", whiteFilter);
            npcHeightFromRecTop.Add("Chained Pharaoh Guard", 218);
            npcSprites.Add("Dr. Dominique Jean Larrey", whiteFilter);
            npcHeightFromRecTop.Add("Dr. Dominique Jean Larrey", 121);
            npcSprites.Add("The Pyramid Gardener", whiteFilter);
            npcHeightFromRecTop.Add("The Pyramid Gardener", 197);
            npcSprites.Add("Henry Horus", whiteFilter);
            npcHeightFromRecTop.Add("Henry Horus", 153);
            npcSprites.Add("King Hasbended", whiteFilter);
            npcHeightFromRecTop.Add("King Hasbended", 90);
            npcSprites.Add("Time Lord", whiteFilter);
            npcHeightFromRecTop.Add("Time Lord", 107);


            //Literature
            npcSprites.Add("Santa Claus", whiteFilter);
            npcHeightFromRecTop.Add("Santa Claus", 161);
            npcSprites.Add("Poole", whiteFilter);
            npcHeightFromRecTop.Add("Poole", 116);
            npcSprites.Add("Gary R. Pigeon", whiteFilter);
            npcHeightFromRecTop.Add("Gary R. Pigeon", 219);
            npcSprites.Add("Ebenezer Scrooge", whiteFilter);
            npcHeightFromRecTop.Add("Ebenezer Scrooge", 156);
            npcSprites.Add("Jacob Marley", whiteFilter);
            npcHeightFromRecTop.Add("Jacob Marley", 0);
            npcSprites.Add("Bell Man", whiteFilter);
            npcHeightFromRecTop.Add("Bell Man", 50);
            //Party
            npcSprites.Add("Julius Caesar", whiteFilter);
            npcHeightFromRecTop.Add("Julius Caesar", 70);
            npcSprites.Add("Pelt Kid", whiteFilter);
            npcHeightFromRecTop.Add("Pelt Kid", 122);
            npcSprites.Add("Squirrel Boy", whiteFilter);
            npcHeightFromRecTop.Add("Squirrel Boy", 180);
            npcSprites.Add("Callyn", whiteFilter);
            npcHeightFromRecTop.Add("Callyn", 145);
            npcSprites.Add("Jesse", whiteFilter);
            npcHeightFromRecTop.Add("Jesse", 114);
            npcSprites.Add("Chelsea", whiteFilter);
            npcHeightFromRecTop.Add("Chelsea", 121);
            npcSprites.Add("Steve", whiteFilter);
            npcHeightFromRecTop.Add("Steve", 123);
            npcSprites.Add("Blurso", whiteFilter);
            npcHeightFromRecTop.Add("Blurso", 1);

            //MISC
            npcSprites.Add("Chad Champson", whiteFilter);
            npcHeightFromRecTop.Add("Chad Champson", 158);
            #endregion

            #region Projectiles
            projectileTextures = Content.Load<Texture2D>(@"Projectiles\ProjectileSprite");
            #endregion

            #region Random
            questWantedTexture = Content.Load<Texture2D>(@"Notifications\questInfo");
            rightBumperTexture = Content.Load<Texture2D>(@"HUD\RBumper");
            descriptionBoxManager = new DescriptionBoxManager(spriteBatch, descriptionFont);
            treasureChestSheet = Content.Load<Texture2D>(@"SpriteSheets\Chest\Chester Chest Sheet");
            treasureChestFlash = Content.Load<Texture2D>(@"SpriteSheets\Chest\treasureChestFlash");
            treasureChestBox = Content.Load<Texture2D>(@"SpriteSheets\Chest\treasureChestPopUp");
            lockerComboButton = Content.Load<Texture2D>(@"Menus\StudentLocker\lockerComboButton");
            cursor = new Cursor(this.Content.Load<Texture2D>("CursorSheet"));

            storyQuestReceived = Content.Load<Texture2D>(@"Notifications\StoryQuest");
            sideQuestReceived = Content.Load<Texture2D>(@"Notifications\SideQuest");
            #endregion

            #region Platforms
            platformTextures.Add("RockFloor", Content.Load<Texture2D>(@"Floor Textures\TileFloor"));
            platformTextures.Add("RockFloor2", platformTextures["RockFloor"]);
            platformTextures.Add("TutorialMoving", Content.Load<Texture2D>(@"Tutorial\MovingPlatMapFour"));
            #endregion

            #region Enemy Drops
            
            EnemyDrop.allDrops.Add("Broken Glass", new DropType("Broken Glass", "A shard of broken science.", Content.Load<Texture2D>(@"DropTextures\BrokenGlassDrop"), .03f, .03f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Spinach", new DropType("Spinach", "It gives biceps to glass.", Content.Load<Texture2D>(@"DropTextures\Spinach"), .05f, .05f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Half-Eaten Cheese", new DropType("Half-Eaten Cheese", "It has rabies built right in!", Content.Load<Texture2D>(@"DropTextures\Cheese"), .07f, .07f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Guano", new DropType("Guano", "Shiny, valuable bat poop.", Content.Load<Texture2D>(@"DropTextures\Guano"), .08f, .08f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Coal", new DropType("Coal", "A lump of stolen coal.", Content.Load<Texture2D>(@"DropTextures\Coal"), .1f, .1f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Topaz", new DropType("Topaz", "Your average, every-day topaz. Not worth much.", Content.Load<Texture2D>(@"DropTextures\Topaz"), 40.00f, 40.00f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Ruby", new DropType("Ruby", "A fiery ruby. Buy yourself somethin' nice.", Content.Load<Texture2D>(@"DropTextures\Ruby"), 100.00f, 100.00f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Sapphire", new DropType("Sapphire", "Uncommon, and worth a pretty penny. Good find!", Content.Load<Texture2D>(@"DropTextures\Sapphire"), 500.00f, 500.00f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Emerald", new DropType("Emerald", "What's green and rectangular? Money! Which this is worth a lot of.", Content.Load<Texture2D>(@"DropTextures\Emerald"), 1000.00f, 1000.00f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Diamond", new DropType("Diamond", "This would pay for college, if you had a chance of going.", Content.Load<Texture2D>(@"DropTextures\Diamond"), 10000.00f, 10000.00f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Sheet Music", new DropType("Sheet Music", "Beautiful musical prose written in crayon.", Content.Load<Texture2D>(@"DropTextures\Sheet Music"), .15f, .15f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Ectoplasm", new DropType("Ectoplasm", "Spooky ghost goo.", Content.Load<Texture2D>(@"DropTextures\Ectoplasm"), .1f, .2f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Cymbal Polish", new DropType("Cymbal Polish", "Brass-colored paint.", Content.Load<Texture2D>(@"DropTextures\Cymbal Polish"), .12f, .12f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Mexican Jumping Beans", new DropType("Mexican Jumping Beans", "Made in China", Content.Load<Texture2D>(@"DropTextures\Mexican Jumping Beans"), .11f, .11f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Mix Tape", new DropType("Mix Tape", "The greatest of all romantic pre-teen gifts.", Content.Load<Texture2D>(@"DropTextures\Mixed Tape"), .13f, .13f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Fuzzy Meat Chunk", new DropType("Fuzzy Meat Chunk", "FDA Approved.", Content.Load<Texture2D>(@"DropTextures\Fuzzy Meat Chunk"), .17f, .17f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Stolen Painting", new DropType("Stolen Painting", "There's an artist somewhere missing hundreds of these.", Content.Load<Texture2D>(@"DropTextures\Stolen Painting"), .15f, .15f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Clay Dough", new DropType("Clay Dough", "Comes in a variety of browns!", Content.Load<Texture2D>(@"DropTextures\Clay Dough"), .17f, .17f, 0, "Loot"));
            EnemyDrop.allDrops.Add("First Aid Kit", new DropType("First Aid Kit", "For goblin use only.", Content.Load<Texture2D>(@"DropTextures\First Aid Kit"), .20f, .20f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Goblin Gold", new DropType("Goblin Gold", "A pouch of painted rocks.", Content.Load<Texture2D>(@"DropTextures\Goblin Gold"), .36f, .36f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Goblin Rocks", new DropType("Goblin Rocks", "A pouch of rocks.", Content.Load<Texture2D>(@"DropTextures\Goblin Rocks"), .10f, .10f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Cactus Flower", new DropType("Cactus Flower", "For goblin use only.", Content.Load<Texture2D>(@"DropTextures\Cactus Flower"), 1f, 1f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Insta-Bomb!", new DropType("Insta-Bomb!", "For goblin use only.", Content.Load<Texture2D>(@"DropTextures\Insta-Bomb!"), .30f, .30f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Peyote", new DropType("Peyote", "For goblin use only.", Content.Load<Texture2D>(@"DropTextures\Peyote"), .42f, .42f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Grilled Buzzard Leg", new DropType("Grilled Buzzard Leg", "For goblin use only.", Content.Load<Texture2D>(@"DropTextures\Grilled Buzzard Leg"), .55f, .55f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Lumber", new DropType("Lumber", "Still breathing.", Content.Load<Texture2D>(@"DropTextures\Lumber"), .4f, .4f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Haunted Present", new DropType("Haunted Present", "May contain murder.", Content.Load<Texture2D>(@"DropTextures\Haunted Present"), .39f, .39f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Haunted Walnuts", new DropType("Haunted Walnuts", "They can't be cracked, and they'll laugh at you for trying.", Content.Load<Texture2D>(@"DropTextures\Haunted Walnuts"), .39f, .39f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Mummy's Curse", new DropType("Mummy's Curse", "The afflicted will forever be cursed to have all toilet seats they sit on to be uncomfortably warm.", Content.Load<Texture2D>(@"DropTextures\Mummy's Curse"), .5f, .5f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Corn Stalk", new DropType("Corn Stalk", "A large and study corn stick.", Content.Load<Texture2D>(@"DropTextures\CornStalk"), .05f, .05f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Acorn Sack", new DropType("Acorn Sack", "A small pouch full of acorns", Content.Load<Texture2D>(@"DropTextures\Ectoplasm"), .05f, .05f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Crow Feather", new DropType("Crow Feather", "A sleek, black feather.", Content.Load<Texture2D>(@"DropTextures\CrowFeather"), .05f, .05f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Haunted Lumber", new DropType("Haunted Lumber", "Some spooky wood", Content.Load<Texture2D>(@"DropTextures\Ectoplasm"), .05f, .05f, 0, "Loot"));
            healthDrop = Content.Load<Texture2D>(@"DropTextures\healthDrop");
            moneySprite = Content.Load<Texture2D>(@"DropTextures\moneySprite");
            #endregion

            #region Story Items
            storyItems = ContentLoader.LoadContent(Content, "StoryItems\\Sprites");
            storyItemIcons = ContentLoader.LoadContent(Content, "StoryItems\\Icons");
            #endregion

            #region Collectibles
            textbookTextures = (Content.Load<Texture2D>(@"Collectibles\TextBooks"));
            textbookRay = (Content.Load<Texture2D>(@"Collectibles\textBookRay"));
            bioTexture = Content.Load<Texture2D>(@"Collectibles\CharacterBio");
            #endregion

            #region Player
            dustSprite = this.Content.Load<Texture2D>(@"SpriteSheets\dustSprite");
            jumpPoofSprite = this.Content.Load<Texture2D>(@"SpriteSheets\JumpPoofSheet");
            danceSprite = this.Content.Load<Texture2D>(@"SpriteSheets\CombatSheetFixed");
            playerSheet = this.Content.Load<Texture2D>(@"SpriteSheets\Player");
            levelUpAnimation = this.Content.Load<Texture2D>(@"SpriteSheets\LevelUpSheet");
            levelUpStatBox = this.Content.Load<Texture2D>(@"SpriteSheets\LevelUpBox");
            player = new Player(playerSheet, font, this);

            #endregion
            
            #region Hud
            Texture2D hudBack, hudHealth, skillBar, hudExp, qBar, wBar, eBar, rBar;
            hudBack = Content.Load<Texture2D>(@"HUD\HUDOutlines");
            hudHealth = Content.Load<Texture2D>(@"HUD\motivBar");
            hudExp = Content.Load<Texture2D>(@"HUD\expBar");
            skillBar = Content.Load<Texture2D>(@"HUD\skillBack");
            singleSkillBarBack = Content.Load<Texture2D>(@"HUD\singleSkillBarBack");
            qBar = Content.Load<Texture2D>(@"HUD\qBar");
            wBar = Content.Load<Texture2D>(@"HUD\wBar");
            eBar = Content.Load<Texture2D>(@"HUD\eBar");
            rBar = Content.Load<Texture2D>(@"HUD\rBar");
            hud = new HUD(hudBack, hudHealth, skillBar, hudExp,qBar, wBar, eBar, rBar,
                player, font, this, descriptionBoxManager, Content.Load<Texture2D>(@"HUD\HUDTop"), Content.Load<Texture2D>(@"HUD\skillTop"), Content.Load<Texture2D>(@"HUD\skillTopPad"), Content.Load<Texture2D>(@"HUD\HUDPiggy"), Content.Load<Texture2D>(@"HUD\skillLevel"), Content.Load<Texture2D>(@"HUD\hudgradient"), Content.Load<Texture2D>(@"HUD\skillTip"), Content.Load<Texture2D>(@"HUD\lootBox"));

            phoneTexture = Content.Load<Texture2D>(@"HUD\Phone");
            phoneTextureController = Content.Load<Texture2D>(@"HUD\PhoneController");

            questHUD = new QuestHud(this);
            questHUD.LoadContent();
            #endregion

            #region Skills
            skillAnimations = new Dictionary<string,Texture2D>();
            skillIcons = new Dictionary<string,Texture2D>();

            skillStar = Content.Load<Texture2D>(@"HUD\skillStar");

            skillLevelUpTexture = ContentLoader.LoadContent(Content, @"SpriteSheets\Skills\Skill Level Up");

            Texture2D skillIconSheet = whiteFilter;

            skillAnimations.Add("Discuss Differences", Game1.whiteFilter);
            skillIcons.Add("Discuss Differences", Game1.whiteFilter);

            skillAnimations.Add("Blinding Logic", Game1.whiteFilter);
            skillIcons.Add("Blinding Logic", Game1.whiteFilter);
            BlindingLogic.flashTextures = ContentLoader.LoadContent(Content, "SpriteSheets\\Skills\\BlindingLogicFlash");

            skillAnimations.Add("Quick Retort", Game1.whiteFilter);
            skillIcons.Add("Quick Retort", Game1.whiteFilter);

            skillAnimations.Add("Twisted Thinking", Game1.whiteFilter);
            skillIcons.Add("Twisted Thinking", this.Content.Load<Texture2D>(@"SkillIcons\TwistedThinkingIcon"));

            skillAnimations.Add("Shocking Statements CH.1", Game1.whiteFilter);
            skillIcons.Add("Shocking Statements CH.1", Game1.whiteFilter);

            skillAnimations.Add("Lightning Dash", Game1.whiteFilter);
            skillIcons.Add("Lightning Dash", this.Content.Load<Texture2D>(@"SkillIcons\LightningDash"));

            skillAnimations.Add("Distance Yourself", this.Content.Load<Texture2D>(@"SpriteSheets\Skills\distanceYourselfSheet"));
            skillIcons.Add("Distance Yourself", this.Content.Load<Texture2D>(@"SkillIcons\DistanceYourself"));

            skillAnimations.Add("Sharp Comment", Game1.whiteFilter);
            skillIcons.Add("Sharp Comment", this.Content.Load<Texture2D>(@"SkillIcons\SharpComment"));

            skillAnimations.Add("Pointed Jabs", Game1.whiteFilter);
            skillIcons.Add("Pointed Jabs", this.Content.Load<Texture2D>(@"SkillIcons\PointedJabs"));

            skillAnimations.Add("Sharp Comments", Game1.whiteFilter);
            skillIcons.Add("Sharp Comments", Game1.whiteFilter);

            skillAnimations.Add("Fowl Mouth", Game1.whiteFilter);
            skillIcons.Add("Fowl Mouth", Game1.whiteFilter);

            skillAnimations.Add("Mopping Up", Game1.whiteFilter);
            skillIcons.Add("Mopping Up", Game1.whiteFilter);

            skillAnimations.Add("Shocking Statements CH.3", Game1.whiteFilter);
            skillIcons.Add("Shocking Statements CH.3", Game1.whiteFilter);

            skillAnimations.Add("Crushing Realization", Game1.whiteFilter);
            skillIcons.Add("Crushing Realization", Game1.whiteFilter);

            skillAnimations.Add("Combustible Confutation CH.1", Game1.whiteFilter);
            skillIcons.Add("Combustible Confutation CH.1", Game1.whiteFilter);

            skillAnimations.Add("Cutting Corners", Game1.whiteFilter);
            skillIcons.Add("Cutting Corners", Game1.whiteFilter);

            skillAnimations.Add("Shocking Statements CH.2", Game1.whiteFilter);
            skillIcons.Add("Shocking Statements CH.2", Game1.whiteFilter);

            skillAnimations.Add("Combustible Confutation CH.2", Game1.whiteFilter);
            skillIcons.Add("Combustible Confutation CH.2", Game1.whiteFilter);

            skillAnimations.Add("Combustible Confutation CH.3", Game1.whiteFilter);
            skillIcons.Add("Combustible Confutation CH.3", Game1.whiteFilter);

            skillManager = new SkillManager(skillAnimations, skillIcons);

            bangFX1 = this.Content.Load<Texture2D>(@"SkillImpacts\red smack");
            Texture2D lightningEffect = this.Content.Load<Texture2D>(@"SkillImpacts\LightningImpact");
            Texture2D lightningEffect2 = this.Content.Load<Texture2D>(@"SkillImpacts\LightningImpact2");
            Texture2D whiteSmack = this.Content.Load<Texture2D>(@"SkillImpacts\smack");
            Texture2D orangeSlash = this.Content.Load<Texture2D>(@"SkillImpacts\orange slash");
            Texture2D tripleOrangeSlash = this.Content.Load<Texture2D>(@"SkillImpacts\triple orange slash");
            Texture2D fireEffect = this.Content.Load<Texture2D>(@"SkillImpacts\Fire");

            SkillManager.skillImpactEffects.Add("Discuss Differences", new List<Texture2D> () { bangFX1 });
            SkillManager.skillImpactEffects.Add("Blinding Logic", new List<Texture2D> () {whiteSmack});
            SkillManager.skillImpactEffects.Add("Mopping Up", new List<Texture2D> () {whiteSmack});
            SkillManager.skillImpactEffects.Add("Fowl Mouth", new List<Texture2D> () {whiteSmack});
            SkillManager.skillImpactEffects.Add("Shocking Statements CH.1", new List<Texture2D>() { lightningEffect });
            SkillManager.skillImpactEffects.Add("Quick Retort",new List<Texture2D> () { bangFX1});
            SkillManager.skillImpactEffects.Add("Sharp Comments",new List<Texture2D> () { orangeSlash});
            SkillManager.skillImpactEffects.Add("Shocking Statements CH.3", new List<Texture2D>() { lightningEffect2 });
            SkillManager.skillImpactEffects.Add("Crushing Realization", new List<Texture2D> () {bangFX1});
            SkillManager.skillImpactEffects.Add("Combustible Confutation CH.1", new List<Texture2D> () {fireEffect});
            SkillManager.skillImpactEffects.Add("Cutting Corners", new List<Texture2D> () {orangeSlash});
            SkillManager.skillImpactEffects.Add("Shocking Statements CH.2", new List<Texture2D>() { lightningEffect });
            SkillManager.skillImpactEffects.Add("Combustible Confutation CH.2", new List<Texture2D> () {fireEffect});
            SkillManager.skillImpactEffects.Add("Combustible Confutation CH.3", new List<Texture2D>() { fireEffect });
            #endregion

            #region Monsters

            //HUD Stuff
            
            EHealthBox = Content.Load<Texture2D>(@"EnemySprites\EnemyHealthBack");
            EHealthBar = Content.Load<Texture2D>(@"EnemySprites\EnemyHealthFore");
            BossHealthBar = Content.Load<Texture2D>(@"HUD\BossHealth");
            FriendHealthBar = Content.Load<Texture2D>(@"HUD\AllyHealthBar");
            FriendHealthBox = Content.Load<Texture2D>(@"HUD\AllyHealthBox");
            BossHUD = Content.Load<Texture2D>(@"HUD\BossOutline");
            BossLine = Content.Load<Texture2D>(@"HUD\BossBlackLine");
            ExtraBossHealthBar = Content.Load<Texture2D>(@"HUD\extraBossBar");
            scientistSprite = this.Content.Load<Texture2D>(@"EnemySprites\scientist");

            
            enemySpriteSheets.Add("HealthBox", EHealthBox);
            enemySpriteSheets.Add("HealthBar", EHealthBar);
            enemySpriteSheets.Add("BossHUD", BossHUD);
            enemySpriteSheets.Add("BossLine", BossLine);
            enemySpriteSheets.Add("BossHealthBar", BossHealthBar);
            enemySpriteSheets.Add("FriendHealthBar", FriendHealthBar);
            enemySpriteSheets.Add("FriendHealthBox", FriendHealthBox);
            enemySpriteSheets.Add("ExtraBossHealthBar", ExtraBossHealthBar);
            enemySpriteSheets.Add("Scientist", scientistSprite);
            #endregion

            #region Interactive Objects
          //  interactiveObjects.Add("StoneStatue", Content.Load<Texture2D>(@"InteractiveObjects\statue"));
            interactiveObjects.Add("BadBarrel", Content.Load<Texture2D>(@"Tutorial\barrel"));
            interactiveObjects.Add("Barrel", Content.Load<Texture2D>(@"InteractiveObjects\barrelSprite"));
            interactiveObjects.Add("Vase", Content.Load<Texture2D>(@"InteractiveObjects\vaseSprite"));
            interactiveObjects.Add("Scarecrow", Content.Load<Texture2D>(@"InteractiveObjects\Scarecrow"));
            interactiveObjects.Add("ExplodingFlower", Content.Load<Texture2D>(@"InteractiveObjects\ExplodingFlower"));
            interactiveObjects.Add("Ghost Light", Content.Load<Texture2D>(@"InteractiveObjects\GhostLight"));
           // interactiveObjects.Add("Pitfall", Content.Load<Texture2D>(@"InteractiveObjects\Leaves"));
            //interactiveObjects.Add("KidCage", Content.Load<Texture2D>(@"InteractiveObjects\KidCage"));
            interactiveObjects.Add("Nest", Content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldNest"));
            interactiveObjects.Add("Sparkles", Content.Load<Texture2D>(@"InteractiveObjects\Sparkles"));
            flyingLockerSprite = Content.Load<Texture2D>(@"InteractiveObjects\LockerSheet");
            #endregion

            #region Map Hazards
            mapHazards.Add("Fire", Content.Load<Texture2D>(@"MapAssets\Fire"));
            mapHazards.Add("Spike Trap", Content.Load<Texture2D>(@"MapAssets\SpikeTrapSprite"));
            mapHazards.Add("Map Steam", Content.Load<Texture2D>(@"MapAssets\SteamSprite"));
            mapHazards.Add("HorizontalFire", Content.Load<Texture2D>(@"MapAssets\MapFireHori"));
            #endregion

            #region Switches, buttons, ladders
            ladderTexture = Content.Load<Texture2D>(@"MapAssets\ladder");
            switchTexture = Content.Load<Texture2D>(@"MapAssets\WallSwitch");
            mapSign = Content.Load<Texture2D>(@"MapAssets\QuestSign");
            #endregion

            #region Maps
            portalLocker = this.Content.Load<Texture2D>(@"Maps\locker");
            portalTexture = this.Content.Load<Texture2D>(@"Maps\Door");
            lockedPortalTexture = this.Content.Load<Texture2D>(@"Maps\LockedDoor");
            studentLockerTex = Content.Load<Texture2D>(@"MapAssets\studentLocker");

            List<Texture2D> tempBacks = new List<Texture2D>() { whiteFilter };

            #endregion

            backspaceTexture = Content.Load<Texture2D>(@"Menus\Backspace");

            #region Locker Screen
            //--Your Locker Screen
           
            yourLocker = new YourLocker(this, player);
            #endregion

            overLockerButton = Content.Load<Texture2D>(@"Menus\StudentLocker\lockerComboButtonOver");

            notebook = new DarylsNotebook(this);

            loadingScreenGradient = Content.Load<Texture2D>(@"Menus\Loading\textBackdrop");
            loadingScreenLogo = Content.Load<Texture2D>(@"Menus\Loading\logo");
            loadingScreenRays = Content.Load<Texture2D>(@"Menus\Loading\rays");
            loadingScreenText = Content.Load<Texture2D>(@"Menus\Loading\loading");

            loadingScreen = new LoadingScreen(this);

            #region Chapters
            
            prologueTextures = new Dictionary<string, Texture2D>();
            prologueTextures.Add("OutsideSchool", Content.Load<Texture2D>(@"Prologue\OutsideSchool"));
            prologueTextures.Add("Player", playerSheet);
            prologueTextures.Add("Schedule", Content.Load<Texture2D>(@"Prologue\Schedule"));
            prologueTextures.Add("MainOffice", Content.Load<Texture2D>(@"Prologue\MainOffice"));
            prologueTextures.Add("PrologueComplete", Content.Load<Texture2D>(@"Prologue\prologueComplete"));
            prologueTextures.Add("Flashback", Content.Load<Texture2D>(@"Prologue\flashback"));
            prologueTextures.Add("OutsideSchoolWords", Content.Load<Texture2D>(@"Prologue\OutsideSchoolText"));
            prologueTextures.Add("Clouds1", Content.Load<Texture2D>(@"Prologue\clouds1"));
            prologueTextures.Add("Clouds2", Content.Load<Texture2D>(@"Prologue\clouds2"));
            petRatSprite = Content.Load<Texture2D>(@"SpriteSheets\ratSprite");


            prologue = new Prologue(this, ref player, hud, cursor, camera, prologueTextures);

            chOneTextures = new Dictionary<string, Texture2D>();
            chOneTextures.Add("FlashBack", Content.Load<Texture2D>(@"ChapterOne\ch1Flash"));
            chOneTextures.Add("GymScene", Content.Load<Texture2D>(@"ChapterOne\ch1Opening"));

            chapterOne = new ChapterOne(this, ref player, hud, cursor, camera, chOneTextures);

            
            chTwoTextures = new Dictionary<string, Texture2D>();
            chTwoTextures.Add("TutorialPopUps", Content.Load<Texture2D>(@"Tutorial\TutorialPopUps"));
            chTwoTextures.Add("Text1", Content.Load<Texture2D>(@"Tutorial\TutorialLevelOne"));
            chTwoTextures.Add("Text2", Content.Load<Texture2D>(@"Tutorial\TutorialLevelTwo"));
            chTwoTextures.Add("enterButton", Content.Load<Texture2D>(@"Dialogue\enterButton"));
            chTwoTextures.Add("associateOne", Content.Load<Texture2D>(@"Tutorial\ToolTip1"));
            chapterTwo = new ChapterTwo(this, ref player, hud, cursor, camera, chTwoTextures);

            chapterTwoDemo = new ChapterTwoDEMO(this, ref player, hud, cursor, camera, new Dictionary<string,Texture2D>());


            #endregion

            #region Main Menu

            mainMenu = new MainMenu(this, cursor);
            mainMenu.LoadContent();
            #endregion

            #region Options Menu

            //options = new OptionsMenu(Content.Load<Texture2D>(@"Menus\Options"), this);
            options = new OptionsMenu(Content.Load<Texture2D>(@"Menus\Pause"), this); //DEMO ONLY
            #endregion

            shop = new Shop(this, player);

            saveLoadManager = new SaveLoadManager(player, this);
            sideQuestManager = new SideQuestManager(this);

            allStoryItems = new AllStoryItems();

            deathScreen = new DeathScreen(this);

            //Getting the variable name example
            ////Console.Writeline(GetFileName(() => deathScreen));
        }

        //Clears the list and readds the essentials, so every zone can load their own monsters
        public void ResetEnemySpriteList()
        {
            enemySpriteSheets.Clear();

            enemySpriteSheets.Add("HealthBox", EHealthBox);
            enemySpriteSheets.Add("HealthBar", EHealthBar);
            enemySpriteSheets.Add("BossHUD", BossHUD);
            enemySpriteSheets.Add("BossLine", BossLine);
            enemySpriteSheets.Add("BossHealthBar", BossHealthBar);
            enemySpriteSheets.Add("FriendHealthBar", FriendHealthBar);
            enemySpriteSheets.Add("FriendHealthBox", FriendHealthBox);
            enemySpriteSheets.Add("ExtraBossHealthBar", ExtraBossHealthBar);
            enemySpriteSheets.Add("Scientist", scientistSprite);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            gameTimeData = gameTime;
            if (Sound.poofSounds != null)
            {
                for (int i = 0; i < Sound.poofSounds.Count; i++)
                {
                    if (Sound.poofSounds[i].State == SoundState.Stopped)
                    {
                        Sound.poofSounds[i].Dispose();
                        Sound.poofSounds.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

            Sound.UpdateCurrentPlayingSounds();

            //Update the game pad if it is connected
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                myGamePad.UpdateGamePad();
                gamePadConnected = true;
                MyGamePad.gamePad = new SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One);
            }
            else
            {
                gamePadConnected = false;
                MyGamePad.gamePad = null;
            }

            last = current;
            current = Keyboard.GetState();

            MouseManager.Update();

            switch (chapterState)
            {
                case ChapterState.mainMenu:
                    mainMenu.Update();
                    break;
                case ChapterState.prologue:
                    prologue.Update();
                    break;
                case ChapterState.chapterOne:
                    chapterOne.Update();
                    break;
                case ChapterState.chapterTwo:
                    chapterTwo.Update();
                    break;
                case ChapterState.demo:
                    chapterTwoDemo.Update();
                    break;
            }


            base.Update(gameTime);

        }

        //Call on player death or 'return to main menu'
        public void ResetGameAndGoToMain()
        {
            ResetWithoutGoingToMain();
            mainMenu.LoadContent();

            chapterState = ChapterState.mainMenu;
        }

        public void ResetWithoutGoingToMain()
        {
            saveData = new SaveLoadManager.SaveGame();
            Sound.ResetSound();
            player.ResetPlayer();
            currentChapter.CurrentMap.LeaveMap();
            currentChapter.CurrentMap.UnloadContent();
            Chapter.effectsManager.ResetEffects();
            Chapter.lastMap = null;
            Chapter.theNextMap = null;
            allQuests = new Dictionary<string, Quest>();
            questHUD.questHelperQuests.Clear();
            notebook.ComboPage.LockerCombos.Clear();
            yourLocker.SkillsOnSale.Clear();
            yourLocker.ResetInventoryBoxes();


            sideQuestManager = new SideQuestManager(this);
            prologue = new Prologue(this, ref player, hud, cursor, camera, prologueTextures);
            chapterOne = new ChapterOne(this, ref player, hud, cursor, camera, chOneTextures);
            chapterTwo = new ChapterTwo(this, ref player, hud, cursor, camera, chTwoTextures);
            chapterTwoDemo = new ChapterTwoDEMO(this, ref player, hud, cursor, camera, chTwoTextures);
            currentQuests = new List<Quest>();
            currentSideQuests = new List<Quest>();
            mapBooleans = new MapBooleans();

            //MAP ZONE WRAPPERS
            schoolZoneWrapper = new MapZoneWrapper(true);

            //MAP ZONES
            schoolMaps = new SchoolMaps(this, player);

            mainMenu.ResetMainMenu();

            //reset inventory stuff
            notebook.Inventory.ResetInventoryForGameOver();

            ResetEnemySpriteList();

            //Reset map booleans

            mapBooleans.tutorialMapBooleans = new Dictionary<string, bool>();
            mapBooleans.prologueMapBooleans = new Dictionary<string, bool>();
            mapBooleans.chapterOneMapBooleans = new Dictionary<string, bool>();
            mapBooleans.chapterTwoMapBooleans = new Dictionary<string, bool>();
            SetMapBooleans();

            notebook.Journal.ResetJournal();

            skillManager.Reset();

        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (chapterState)
            {
                case ChapterState.mainMenu:
                    //camera.Update(player, this, schoolMaps.maps["Bathroom"]);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    mainMenu.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case ChapterState.prologue:
                    prologue.Draw(spriteBatch);
                    break;
                case ChapterState.chapterOne:
                    chapterOne.Draw(spriteBatch);
                    break;
                case ChapterState.chapterTwo:
                    chapterTwo.Draw(spriteBatch);
                    break;
                case ChapterState.demo:
                    chapterTwoDemo.Draw(spriteBatch);
                    break;
            }
          
            base.Draw(gameTime);
        }

        //Takes in a string and adds line breaks according to the linewidth passed in
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if(word.Contains("\n"))
                {
                    lineWidth = 0;
                }
                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            //This part removes any extra character spaces at the end of the dialogue. This is necessary for NPC dialogue and anything that scrolls
            String dialogue = sb.ToString();
            while(dialogue[dialogue.Length - 1].Equals(' '))
            {
                dialogue = dialogue.Remove(dialogue.Length - 1, 1);
            }

            return dialogue;
        }

        // http://stackoverflow.com/questions/716399/c-sharp-how-do-you-get-a-variables-name-as-it-was-physically-typed-in-its-dec
        public static String GetFileName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }

        public static void OutlineFont(SpriteFont font, SpriteBatch s, String str, int outlineWidth, int x, int y, Color outlineColor, Color fillColor)
        {
            s.DrawString(font, str, new Vector2(x + outlineWidth, y + outlineWidth), outlineColor);
            s.DrawString(font, str, new Vector2(x - outlineWidth, y - outlineWidth), outlineColor);
            s.DrawString(font, str, new Vector2(x + outlineWidth, y - outlineWidth), outlineColor);
            s.DrawString(font, str, new Vector2(x - outlineWidth, y + outlineWidth), outlineColor);

            s.DrawString(font, str, new Vector2(x + outlineWidth, y), outlineColor);
            s.DrawString(font, str, new Vector2(x, y + outlineWidth), outlineColor);
            s.DrawString(font, str, new Vector2(x, y - outlineWidth), outlineColor);
            s.DrawString(font, str, new Vector2(x - outlineWidth, y), outlineColor);


            s.DrawString(font, str, new Vector2(x, y), fillColor);
        }

        public void SaveSettingsFile()
        {
            //-----Read the binary file and turn it into a regular text file----//
            String file2 = "settings.txt";
            File.Delete(file2);
            StreamWriter sw2 = File.AppendText(file2);

            sw2.WriteLine("width, " + (int)currentResolutionInWindowed.X);
            sw2.WriteLine("height, " + (int)currentResolutionInWindowed.Y);
            sw2.WriteLine("music, " + (int)Sound.setBackgroundVolume);
            sw2.WriteLine("sound, " + (int)Sound.setSoundVolume);

            if(Window.IsBorderless)
                sw2.WriteLine("borderless, 1");
            else
                sw2.WriteLine("borderless, 0");

            if(graphics.IsFullScreen)
                sw2.WriteLine("fullscreen, 1");
            else
                sw2.WriteLine("fullscreen, 0");
           
            sw2.Close();
        }

        public Dictionary<String, float> LoadSettingsFile()
        {
            String file = "settings.txt";

            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                List<String> lines = new List<string>();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    lines.Add(line);
                }
                sr.Close();


                BinaryWriter write = new BinaryWriter(File.Open("settingsBinary.txt", FileMode.Create));

                for (int i = 0; i < lines.Count; i++)
                {
                    write.Write(lines[i]);
                }
                write.Close();
            }

            //-----Read the binary file and turn it into a regular text file----//
            String file2 = "settingsReadBinary.txt";
            File.Delete(file2);
            StreamWriter sw2 = File.AppendText(file2);

            BinaryReader reader = new BinaryReader(File.Open("settingsBinary.txt", FileMode.Open));

            //While the reader's stream position is not the end of the file, add a new line to the text file (Basically a new platform)
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                sw2.WriteLine(reader.ReadString());
            }

            sw2.Close();
            reader.Close();
            //-----------------------------------------------------------------//

            String file3 = "settingsReadBinary.txt";
            StreamReader sr2 = new StreamReader(file3);


            Dictionary<String, float> lines2 = new Dictionary<String, float>();

            while (!sr2.EndOfStream)
            {
                String line = sr2.ReadLine();

                if (line != "" && !line[0].Equals('-'))
                {
                    String[] lines = line.Split(',');
                    lines2.Add(lines[0], float.Parse(lines[1]));
                }
            }
            sr2.Close();

            File.Delete("settingsReadBinary.txt");
            return lines2;
        }

        public static int GetYValueForDifferentAspectRatios(int normalYPos)
        {
            return (int)(((float)normalYPos / screenHeightAspect) * screenHeightAspect);
        }

        public static void DrawFlipHorizontal(SpriteBatch s, Texture2D t, Rectangle r)
        {
            s.Draw(t, r, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }

        public static void DrawFlipHorizontal(SpriteBatch s, Texture2D t, Vector2 v)
        {
            s.Draw(t, new Rectangle((int)v.X, (int)v.Y, t.Width, t.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
