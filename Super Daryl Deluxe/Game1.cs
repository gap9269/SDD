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
        public static Texture2D sideQuestReceived, storyQuestReceived, sideQuestComplete, storyQuestComplete;
        public static Texture2D overLockerButton;
        public static Texture2D bioTexture;
        public static Texture2D skillLevelUpTexture;
        public static Texture2D phoneTexture;
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
        public static Texture2D fOuter, fInner, spaceInner, spaceOuter;
        public static Texture2D equipDescriptionBox, otherDescriptionBox, dualWieldIcon;
        public static Texture2D cutsceneDialogueBox;
        Texture2D playerSheet;

        //Constant Monster Textures
        Texture2D EHealthBox, EHealthBar, BossHUD, BossHealthBar, ExtraBossHealthBar, scientistSprite, BossLine;

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

        KeyboardState current;
        KeyboardState last;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            SetResolution(1280, 720);
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

            if (currentChapter != null)
            {
                currentChapter.HUD.UpdateResolution();
                notebook.UpdateGameResolution();
                options.UpdateResolution();
                yourLocker.UpdateResolution();
                shop.UpdateResolution();
                //craftingMenu.UpdateResolution();
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

        public void MakeFull()
        {
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        public void MakeNotFull()
        {
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        public void PopulateNPCWalkingFrameDictionary()
        {
            numberOfNPCWalkingFrames.Add("Paul", 10);
            numberOfNPCWalkingFrames.Add("Alan", 9);
            numberOfNPCWalkingFrames.Add("Mr. Robatto", 10);
            numberOfNPCWalkingFrames.Add("Tim", 8);
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

            //MAP ZONE WRAPPERS
            schoolZoneWrapper = new MapZoneWrapper(true);

            //MAP ZONES
            schoolMaps = new SchoolMaps(this, player);

            loadingTips = new List<string>();
            loadingTips.Add("Press or hold 'SHIFT' to pick up enemy drops!");
            loadingTips.Add("Hover over skills on the HUD to view their information!");
            loadingTips.Add("Remember to equip anything that you buy, and make sure that \nyou equip something once you have met its level requirement.");
            loadingTips.Add("You can sell Loot/Enemy Drops to the Trenchcoat Employees!");
            loadingTips.Add("Dying a lot? Level up your skills or try \na new combination. Use your best equipment, too!");
            loadingTips.Add("Press ESC to view the game's controls!");

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

        //This is called when a new save file is made, and only once. For the normal game, unload all maps except school. For the demo, all but chelseas
        public void CreateNewSaveFileMapData()
        {
            schoolMaps.LoadSchoolZone();
            schoolMaps.PopulateSaveDataOnNewSaveSlot();
            schoolMaps.SaveMapDataToWrapper();
        }

        public void TempPlayHitSound(float volume = 1)
        {
            SoundEffectInstance sound = tempHitSounds.ElementAt(randomNumberGen.Next(3)).Value.CreateInstance();
            if (volume > 1)
                volume = 1;
            sound.Volume = volume;
            sound.Play();
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
            #endregion

            #region Hats
            allEquipment.Add("Dunce Cap", new DunceCap());
            allEquipment.Add("Band Hat", new BandHat());
            allEquipment.Add("Fez", new Fez());
            allEquipment.Add("Powdered Wig", new PowderedWig());
            allEquipment.Add("Gardening Hat", new GardeningHat());
            allEquipment.Add("Party Hat", new PartyHat());
            allEquipment.Add("Scarecrow Hat", new ScarecrowHat());
            allEquipment.Add("Pelt Kid's Hat", new PeltKidsHat());
            #endregion

            #region Hoodies
            allEquipment.Add("Lab Coat", new LabCoat());
            allEquipment.Add("Band Uniform", new BandUniform());
            allEquipment.Add("'I Love Melons' Band Tee", new ILoveMelons());
            allEquipment.Add("'I Hate Melons' Band Tee", new IHateMelons());
            allEquipment.Add("Scarecrow Vest", new ScarecrowVest());
            allEquipment.Add("Toga", new Toga());

            #endregion

            #region Accessories
            allEquipment.Add("Riley's Bow", new RileysBow());
            allEquipment.Add("Uranium Rod", new UraniumRod());
            allEquipment.Add("Lab Goggles", new LabGoggles());
            allEquipment.Add("Used Tissue", new UsedTissue());
            allEquipment.Add("Fang Necklace", new FangNecklace());
            allEquipment.Add("Jar of Dirt", new JarOfDirt());
            allEquipment.Add("Beer Goggles", new BeerGoggles());
            allEquipment.Add("Solo Cup", new SoloCup());
            #endregion

            #endregion

            sideQuestManager = new SideQuestManager(this);

            #region Fonts
            font = this.Content.Load<SpriteFont>("Fonts\\font");
            descriptionFont = this.Content.Load<SpriteFont>("Fonts\\DescriptionBoxFont");

            skillLevelMoireFont = Content.Load<SpriteFont>("Fonts\\SkillLevelMoireFont");
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

            HUDFont = Content.Load<SpriteFont>("Fonts\\HUDFont");
            HUDFont.Spacing = -24;

            xpHUDFont = Content.Load<SpriteFont>("Fonts\\XPHUDFont");
            xpHUDFont.Spacing = -29;

            moneyFont = Content.Load<SpriteFont>("Fonts\\MoneyFont1");
            moneyFont.Spacing = -22; //-5?

            playerFont = Content.Load<SpriteFont>("Fonts\\PlayerFont");
            playerFont.Spacing = -16;

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
            socialRankUpTexture = ContentLoader.LoadContent(Content, "Notifications\\Social Rank Up");
            newBioNotification = Content.Load<Texture2D>(@"Notifications\newBio");
            fOuter = Content.Load<Texture2D>(@"Notifications\fOuter");
            fInner = Content.Load<Texture2D>(@"Notifications\fInner");
            spaceOuter = Content.Load<Texture2D>(@"Notifications\spaceOuter");
            spaceInner = Content.Load<Texture2D>(@"Notifications\spaceInner");
            cutsceneDialogueBox = Content.Load<Texture2D>(@"Notifications\cutsceneDialogueBox");

            equipDescriptionBox = Content.Load<Texture2D>(@"Notifications\equipDescriptionBox");
            otherDescriptionBox = Content.Load<Texture2D>(@"Notifications\otherDescriptionBox");
            dualWieldIcon = Content.Load<Texture2D>(@"Notifications\DualWieldIcon");
            #endregion

            #region Effects
            bangFX1 = Content.Load<Texture2D>(@"FX\bangFX1");
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

            //--D&D Dorks

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

            //HISTORY
            NPCFace cleoFace = new NPCFace();
            cleoFace.faces = new Dictionary<string, Texture2D>();
            cleoFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Cleopatra", cleoFace);

            NPCFace napoleonFace = new NPCFace();
            napoleonFace.faces = new Dictionary<string, Texture2D>();
            napoleonFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Napoleon", napoleonFace);


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


            //Party
            NPCFace peltKidFace = new NPCFace();
            peltKidFace.faces = new Dictionary<string, Texture2D>();
            peltKidFace.faces.Add("Normal", whiteFilter);
            npcFaces.Add("Pelt Kid", peltKidFace);

            NPCFace juliusFace = new NPCFace();
            juliusFace.faces = new Dictionary<string, Texture2D>();
            juliusFace.faces.Add("Normal", whiteFilter);
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



            npcFaces.Add("Callyn", robattoFace);
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

            
            //Main characters
            npcSprites.Add("Alan", whiteFilter);
            npcHeightFromRecTop.Add("Alan", 125);
            npcSprites.Add("Paul", whiteFilter);
            npcHeightFromRecTop.Add("Paul", 135);
            npcSprites.Add("Mark", whiteFilter);
            npcHeightFromRecTop.Add("Mark", 50);
            
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

            //History
            npcSprites.Add("Cleopatra", whiteFilter);
            npcHeightFromRecTop.Add("Cleopatra", 88);
            npcSprites.Add("Napoleon", whiteFilter);
            npcHeightFromRecTop.Add("Napoleon", 116);

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

            #region Drops
            
            EnemyDrop.allDrops.Add("Broken Glass", new DropType("Broken Glass", "A shard of broken glass.", Content.Load<Texture2D>(@"DropTextures\BrokenGlassDrop"), .05f, .05f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Bat Fangs", new DropType("Bat Fangs", "A pair of sharp fangs dropped by a bat.", Content.Load<Texture2D>(@"DropTextures\BatFangs"), .07f, .07f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Coal", new DropType("Coal", "A lump of stolen coal.", Content.Load<Texture2D>(@"DropTextures\Coal"), .1f, .1f, 0, "Loot"));
            EnemyDrop.allDrops.Add("Ruby (Rough)", new DropType("Ruby (Rough)", "A rough cut ruby. Somewhat rare.", Content.Load<Texture2D>(@"DropTextures\RoughCutRuby"), 2f, 10.00f, 0, "Craft"));
            EnemyDrop.allDrops.Add("Emerald (Rough)", new DropType("Emerald (Rough)", "A rough cut emerald. Somewhat rare.", Content.Load<Texture2D>(@"DropTextures\RoughCutEmerald"), 2f, 10.00f, 0, "Craft"));
            EnemyDrop.allDrops.Add("Sapphire (Rough)", new DropType("Sapphire (Rough)", "A rough cut sapphire. Somewhat rare.", Content.Load<Texture2D>(@"DropTextures\RoughCutSapphire"), 2f, 10.00f, 0, "Craft"));

            EnemyDrop.allDrops.Add("Unfinished Sheet Music", new DropType("Unfinished Sheet Music", "Sheet music that was tragically left unfinished by its deceased composer.", Content.Load<Texture2D>(@"DropTextures\SheetMusic"), .50f, .50f, 0, "Loot"));

            EnemyDrop.allDrops.Add("Ectoplasm", new DropType("Ectoplasm", "Spooky ghost goo.", Content.Load<Texture2D>(@"DropTextures\Ectoplasm"), .20f, .20f, 0, "Loot"));
            EnemyDrop.allDrops.Add("ID Card", new DropType("ID Card", "A guard in Vienna dropped this.", Content.Load<Texture2D>(@"DropTextures\IDCard"), .25f, .25f, 0, "Loot"));
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
            qBar = Content.Load<Texture2D>(@"HUD\qBar");
            wBar = Content.Load<Texture2D>(@"HUD\wBar");
            eBar = Content.Load<Texture2D>(@"HUD\eBar");
            rBar = Content.Load<Texture2D>(@"HUD\rBar");
            Texture2D min = Content.Load<Texture2D>(@"HUD\Minimize");
            Texture2D max = Content.Load<Texture2D>(@"HUD\Maximize");
            hud = new HUD(hudBack, hudHealth, skillBar, hudExp,qBar, wBar, eBar, rBar,
                player, font, this, descriptionBoxManager, min, max, Content.Load<Texture2D>(@"HUD\HUDTop"), Content.Load<Texture2D>(@"HUD\skillTop"), Content.Load<Texture2D>(@"HUD\skillTopPad"), Content.Load<Texture2D>(@"HUD\HUDPiggy"), Content.Load<Texture2D>(@"HUD\skillLevel"), Content.Load<Texture2D>(@"HUD\hudgradient"), Content.Load<Texture2D>(@"HUD\skillTip"), Content.Load<Texture2D>(@"HUD\lootBox"));

            phoneTexture = Content.Load<Texture2D>(@"HUD\Phone");

            questHUD = new QuestHud(this);
            questHUD.LoadContent();
            #endregion

            #region Skills
            skillAnimations = new Dictionary<string,Texture2D>();
            skillIcons = new Dictionary<string,Texture2D>();

            skillLevelUpTexture = this.Content.Load<Texture2D>(@"SpriteSheets\Skills\SkillLevelUp");

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

            skillAnimations.Add("Shocking Statement", Game1.whiteFilter);
            skillIcons.Add("Shocking Statement", Game1.whiteFilter);

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

            skillManager = new SkillManager(skillAnimations, skillIcons);

            SkillManager.skillImpactEffects.Add("Discuss Differences", bangFX1);
            SkillManager.skillImpactEffects.Add("Blinding Logic", bangFX1);
            SkillManager.skillImpactEffects.Add("Mopping Up", bangFX1);
            SkillManager.skillImpactEffects.Add("Fowl Mouth", bangFX1);
            SkillManager.skillImpactEffects.Add("Shocking Statement", this.Content.Load<Texture2D>(@"SkillImpacts\LightningImpact"));
            SkillManager.skillImpactEffects.Add("Quick Retort", bangFX1);
            SkillManager.skillImpactEffects.Add("Sharp Comments", bangFX1);
            #endregion

            #region Monsters

            //HUD Stuff
            
            EHealthBox = Content.Load<Texture2D>(@"EnemySprites\EnemyHealthBack");
            EHealthBar = Content.Load<Texture2D>(@"EnemySprites\EnemyHealthFore");
            BossHealthBar = Content.Load<Texture2D>(@"HUD\BossHealth");
            BossHUD = Content.Load<Texture2D>(@"HUD\BossOutline");
            BossLine = Content.Load<Texture2D>(@"HUD\BossBlackLine");
            ExtraBossHealthBar = Content.Load<Texture2D>(@"HUD\extraBossBar");
            scientistSprite = this.Content.Load<Texture2D>(@"EnemySprites\scientist");

            
            enemySpriteSheets.Add("HealthBox", EHealthBox);
            enemySpriteSheets.Add("HealthBar", EHealthBar);
            enemySpriteSheets.Add("BossHUD", BossHUD);
            enemySpriteSheets.Add("BossLine", BossLine);
            enemySpriteSheets.Add("BossHealthBar", BossHealthBar);
            enemySpriteSheets.Add("ExtraBossHealthBar", ExtraBossHealthBar);
            enemySpriteSheets.Add("Scientist", scientistSprite);
            #endregion

            #region Interactive Objects
            interactiveObjects.Add("StoneStatue", Content.Load<Texture2D>(@"InteractiveObjects\statue"));
            interactiveObjects.Add("BadBarrel", Content.Load<Texture2D>(@"Tutorial\barrel"));
            interactiveObjects.Add("Barrel", Content.Load<Texture2D>(@"InteractiveObjects\barrelSprite"));
            interactiveObjects.Add("Scarecrow", Content.Load<Texture2D>(@"InteractiveObjects\Scarecrow"));
            interactiveObjects.Add("Pitfall", Content.Load<Texture2D>(@"InteractiveObjects\Leaves"));
            interactiveObjects.Add("KidCage", Content.Load<Texture2D>(@"InteractiveObjects\KidCage"));
            interactiveObjects.Add("Nest", Content.Load<Texture2D>(@"Maps\Chelseas\AnotherSpookyFieldNest"));
            interactiveObjects.Add("Sparkles", Content.Load<Texture2D>(@"InteractiveObjects\Sparkles"));
            flyingLockerSprite = Content.Load<Texture2D>(@"InteractiveObjects\LockerSheet");
            #endregion

            #region Map Hazards
            mapHazards.Add("Fire", Content.Load<Texture2D>(@"MapAssets\Fire"));
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
            allStoryItems = new AllStoryItems();

            deathScreen = new DeathScreen(this);
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
            enemySpriteSheets.Add("ExtraBossHealthBar", ExtraBossHealthBar);
            enemySpriteSheets.Add("Scientist", scientistSprite);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Sound.poofSounds != null)
            {
                for (int i = 0; i < Sound.poofSounds.Count; i++)
                {
                    if (Sound.poofSounds[i].State == SoundState.Stopped)
                    {
                        Sound.poofSounds.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

            //Update the game pad if it is connected
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                myGamePad.UpdateGamePad();
                gamePadConnected = true;
            }
            else
                gamePadConnected = false;

            last = current;
            current = Keyboard.GetState();

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
            Sound.ResetSound();
            player.ResetPlayer();
            currentChapter.CurrentMap.UnloadContent();
            Chapter.effectsManager.ResetEffects();
            allQuests = new Dictionary<string, Quest>();
            questHUD.questHelperQuests.Clear();
            notebook.ComboPage.LockerCombos.Clear();
            yourLocker.SkillsOnSale.Clear();
            yourLocker.ResetInventoryBoxes();


            sideQuestManager = new SideQuestManager(this);
            prologue = new Prologue(this, ref player, hud, cursor, camera, prologueTextures);
            chapterOne = new ChapterOne(this, ref player, hud, cursor, camera, chOneTextures);
            chapterTwo = new ChapterTwo(this, ref player, hud, cursor, camera, chTwoTextures);
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
    }
}
