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

namespace ISurvived
{
    public class Portal
    {
        // ATTRIBUTES \\
        Rectangle portalRec, lockRec;
        Texture2D portalTexture;
        Platform platform;
        String mapName;
        String itemNameToUnlock;
        Boolean isUseable = true;
        public Texture2D lockTexture;
        public static Texture2D gold, silver, bronze;
        int moveFrame, frameTimer;
        float floatCycle, lockPosY;
        static Random ranNum, ranTime;

        public int FButtonYOffset; //Use this to move the F Button around for the door. It gets added to the normal location
        public int PortalNameYOffset; //Use this to move the F Button around for the door. It gets added to the normal location

        public static ContentManager content;

        Boolean increaseGlow;
        public Boolean openingLock = false;
        public Boolean lockFinished = false;
        public Boolean startedToOpen = false;
        float glowAlpha;

        // PROPERTIES \\
        public Texture2D PortalTexture { get { return portalTexture; } set { portalTexture = value; } }
        public Rectangle PortalRec { get { return portalRec; } set { portalRec = value; } }
        public int PortalRecY { get { return portalRec.Y; } set { portalRec.Y = value; } }
        public int PortalRecX { get { return portalRec.X; } set { portalRec.X = value; } }
        public Platform Platform { get { return platform; } set { platform = value; } }
        public String MapName { get { return mapName; } set { mapName = value; } }
        public String ItemNameToUnlock { get { return itemNameToUnlock; } set { itemNameToUnlock = value; } }
        public Boolean IsUseable { get { return isUseable; } set { isUseable = value; } }

        // CONSTRUCTOR \\
        public Portal(int x, Platform plat, String name)
        {
            mapName = name;
            platform = plat;
            portalTexture = Game1.portalTexture;
            portalRec = new Rectangle(x, platform.Rec.Y - portalTexture.Height, portalTexture.Width, portalTexture.Height);

            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";

            ranNum = new Random();
            ranTime = new Random();
        }

        public Portal(int x, int platY, String name)
        {
            mapName = name;
            portalTexture = Game1.portalTexture;
            portalRec = new Rectangle(x, platY - portalTexture.Height, portalTexture.Width, portalTexture.Height);
            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";
            ranNum = new Random();
            ranTime = new Random();
        }

        public Portal(int x, Platform plat, Texture2D tex, String name)
        {
            mapName = name;
            platform = plat;
            portalTexture = tex;
            portalRec = new Rectangle(x, platform.Rec.Y - portalTexture.Height, tex.Width, tex.Height);
            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";
            ranNum = new Random();
            ranTime = new Random();
        }

        public Portal(int x, Platform plat, String name, String itemToUnlock)
        {
            mapName = name;
            platform = plat;
            portalTexture = Game1.lockedPortalTexture;
            portalRec = new Rectangle(x, platform.Rec.Y - portalTexture.Height, portalTexture.Width, portalTexture.Height);
            itemNameToUnlock = itemToUnlock;
            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";
            ranNum = new Random();
            ranTime = new Random();

            lockRec = new Rectangle(portalRec.X + 40, portalRec.Y, 95, 172);
        }

        public Portal(int x, int platY, String name, String itemToUnlock)
        {
            mapName = name;
            portalTexture = Game1.lockedPortalTexture;
            portalRec = new Rectangle(x, platY - portalTexture.Height, portalTexture.Width, portalTexture.Height);
            itemNameToUnlock = itemToUnlock;
            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";
            ranNum = new Random();
            ranTime = new Random();

            lockRec = new Rectangle(portalRec.X + 40, portalRec.Y, 95, 172);
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(95 * moveFrame, 0, 95, 172);
        }

        public void UpdateLock()
        {
            if (itemNameToUnlock != "" && itemNameToUnlock != null)
            {
                #region Lock glow

                if (increaseGlow)
                {
                    glowAlpha += .02f;

                    if (glowAlpha > 1f)
                        increaseGlow = false;
                }
                else
                {
                    glowAlpha -= .02f;

                    if (glowAlpha < 0f)
                        increaseGlow = true;
                }

                #endregion

                //Animate the chest
                if (!openingLock)
                {
                    frameTimer--;

                    if (frameTimer <= 0)
                    {
                        moveFrame = ranNum.Next(0, 4);

                        //Blink frame
                        if (moveFrame == 1)
                            frameTimer = 5;
                        else
                            frameTimer = ranTime.Next(20, 120);
                    }
                }
            }
        }

        public void KillLock()
        {
            //Set the frame timer and current frame
            if (openingLock == false)
            {
                openingLock = true;
                moveFrame = 4;
                frameTimer = 20;

            }

            frameTimer--;

            //Make a smoke poof and set the timer high
            if (frameTimer == 0)
            {
                Chapter.effectsManager.AddSmokePoof(new Rectangle(lockRec.X - 25, lockRec.Y + 35, 150, 150), 2);
                frameTimer = 60;
            }

            //After a single frame, make the lock disappear. This can only happen once the smoke poof is made, due to the number being 59
            if (frameTimer == 59)
            {
                lockFinished = true;
                itemNameToUnlock = null;
            }
        }

        public void DrawLock(SpriteBatch s)
        {

            if (!lockFinished)
            {

                #region FLOAT UP AND DOWN
                //--Once it hits the ground, make it float up and down
                    //--Every 20 frames it changes direction
                    //--It floats at 1 pixel per frame, every 2 frames

                floatCycle++;
                    if (floatCycle < 50)
                    {
                         if (floatCycle % 5 == 0)
                        {
                            lockPosY -= 1f;
                        }

                    }
                    else
                    {
                        if (floatCycle % 5 == 0)
                        {
                            lockPosY += 1f;
                        }

                        if (floatCycle > 94)
                        {
                            floatCycle = -1;
                        }
                    }
                
                #endregion

                s.Draw(lockTexture, new Rectangle(lockRec.X, lockRec.Y + (int)lockPosY, lockRec.Width, lockRec.Height), new Rectangle(0, 172, 219, 219), Color.White * glowAlpha);

                s.Draw(lockTexture, new Rectangle(lockRec.X, lockRec.Y + (int)lockPosY, lockRec.Width, lockRec.Height), GetSourceRectangle(), Color.White);
            }

        }

        /// <summary>
        /// Takes in "Gold", "Silver", "Bronze", or "Special"
        /// </summary>
        /// <param name="type"></param>
        public static void LoadLockContent(String type)
        {
            switch (type)
            {
                case "Gold":
                    gold = content.Load<Texture2D>(@"SpriteSheets\GoldLockSheet");
                    break;
                case "Silver":
                    silver = content.Load<Texture2D>(@"SpriteSheets\SilverLockSheet");
                    break;
                case "Bronze":
                    bronze = content.Load<Texture2D>(@"SpriteSheets\BronzeLockSheet");
                    break;
            }
        }

        public void UnloadContent()
        {
            gold = null;
            silver = null;
            bronze = null;
            content.Unload();
            lockPosY = 0;
        }
    }


    public class MapClass
    {
        // ATTRIBUTES \\
        protected Cutscene fadeInOut;
        protected Dictionary<Portal, Portal> portals;
        protected List<Texture2D> background;
        protected List<Enemy> enemiesInMap;
        protected int mapHeight;
        protected int mapWidth;
        public Rectangle mapRec;
        protected int enemyAmount;
        protected static Random rand = new Random();
        protected Game1 game;
        protected Player player;
        protected KeyboardState current;
        protected KeyboardState last;
        protected List<Platform> platforms;
        protected Platform mapEdgePlat;
        protected List<EnemyDrop> drops;
        protected List<StoryItem> storyItems;
        protected List<Collectible> collectibles;
        protected List<StudentLocker> lockers;
        protected List<TreasureChest> treasureChests;
        protected List<Ladder> ladders;
        protected StoryItem currentPickedItem;
        protected Boolean discovered = false;
        protected List<HealthDrop> healthDrops;
        protected List<MoneyDrop> moneyDrops;
        protected List<Switch> switches;
        protected List<MapQuestSign> mapQuestSigns;
        protected List<MapHazard> mapHazards;
        protected List<MoveBlock> moveBlocks;
        protected List<InteractiveObject> interactiveObjects;
        protected List<Projectile> projectiles;
        protected ContentManager content;

        public static int currentLoadingTipNum;

        public String backgroundMusicName = "";

        protected float zoomLevel = 1f;
        protected int overlayType = 0; //0 is normal, 1 is dropshadow

        //Enemy stuff
        protected Boolean spawnEnemies = true;
        protected Dictionary<String, int> enemyNamesAndNumberInMap;

        public Boolean firstFrameInMap = true;

        //--Map quest - Enemy kills
        protected List<String> enemyNames;
        protected List<int> enemiesKilledForQuest;
        protected List<int> enemiesToKill;
        protected Boolean completedMapQuest = false;
        protected Boolean mapWithMapQuest = false;

        public bool yScroll;
        protected String mapName;
        protected int mapNameTimer;
        protected float mapNameAlpha;
        public int leavingMapTimer ;
        public bool enteringMap = false;
        protected String nextMapName = "null";

        /*
        public enum chapterState
        {
            prologue,
            chapterOne,
            chapterTwo
        }
        public chapterState state;*/

        //--Respawning enemies
        protected int platformNum;
        protected int monsterX;
        protected int monsterY;
        protected Vector2 pos;
        // PROPERTIES \\
        public int OverlayType { get { return overlayType; } set { overlayType = value; } }
        public Dictionary<String, int> EnemyNamesAndNumberInMap { get { return enemyNamesAndNumberInMap; } set { enemyNamesAndNumberInMap = value; } }
        public List<Collectible> Collectibles { get { return collectibles; } set { collectibles = value; } }
        public List<HealthDrop> HealthDrops { get { return healthDrops; } set { healthDrops = value; } }
        public List<MoneyDrop> MoneyDrops { get { return moneyDrops; } set { moneyDrops = value; } }
        public List<Enemy> EnemiesInMap { get { return enemiesInMap; } set { enemiesInMap = value; } }
        public List<Platform> Platforms { get { return platforms; } set { platforms = value; } }
        public int MapWidth { get { return mapWidth; } }
        public int MapHeight { get { return mapHeight; } }
        public int MapY { get { return mapRec.Y; } }
        public int Mapx { get { return mapRec.X; } }
        public List<EnemyDrop> Drops { get { return drops; } set { drops = value; } }
        public List<StoryItem> StoryItems { get { return storyItems; } set { storyItems = value; } }
        public String MapName { get { return mapName; } set { mapName = value; } }
        public int MapNameTimer { get { return mapNameTimer; } set { mapNameTimer = value; } }
        public List<TreasureChest> TreasureChests { get { return treasureChests; } set { treasureChests = value; } }
        public StoryItem CurrentPickedItem { get { return currentPickedItem; } set { currentPickedItem = value; } }
        public Player Player { get { return player; } set { player = value; } }
        public Boolean Discovered { get { return discovered; } set { discovered = value; } }
        public List<Ladder> Ladders { get { return ladders; } set { ladders = value; } }
        public List<Switch> Switches { get { return switches; } set { switches = value; } }
        public List<MapQuestSign> MapQuestSigns { get { return mapQuestSigns; } set { mapQuestSigns = value; } }
        public float ZoomLevel { get { return zoomLevel; } set { zoomLevel = value; } }
        public Dictionary<Portal, Portal> Portals { get { return portals; } set { portals = value; } }
        public List<MoveBlock> MoveBlocks { get { return moveBlocks; } set { moveBlocks = value; } }
        public List<int> EnemiesKilledForQuest { get { return enemiesKilledForQuest; } set { enemiesKilledForQuest = value; } }
        public List<String> EnemyNames { get { return enemyNames; } }
        public List<StudentLocker> Lockers { get { return lockers; } set { lockers = value; } }
        public List<InteractiveObject> InteractiveObjects { get { return interactiveObjects; } set { interactiveObjects = value; } }
        public List<Projectile> Projectiles { get { return projectiles; } set { projectiles = value; } }
        // CONSTRUCTOR \\

        public MapClass(List<Texture2D> bg, Game1 g, ref Player play)
        {
            portals = new Dictionary<Portal, Portal>();
            enemiesInMap = new List<Enemy>();
            background = bg;

            game = g;
            player = Game1.Player;
            platforms = new List<Platform>();
            drops = new List<EnemyDrop>();
            storyItems = new List<StoryItem>();
            lockers = new List<StudentLocker>();
            treasureChests = new List<TreasureChest>();
            ladders = new List<Ladder>();
            yScroll = false;
            mapNameTimer = 0;
            mapNameAlpha = 1f;
            fadeInOut = new Cutscene(game, game.Camera);

            healthDrops = new List<HealthDrop>();
            switches = new List<Switch>();
            mapQuestSigns = new List<MapQuestSign>();
            mapHazards = new List<MapHazard>();
            moveBlocks = new List<MoveBlock>();
            collectibles = new List<Collectible>();
            interactiveObjects = new List<InteractiveObject>();
            projectiles = new List<Projectile>();
            moneyDrops = new List<MoneyDrop>();

            enemyNamesAndNumberInMap = new Dictionary<string, int>();

            //--Map quest
            enemiesKilledForQuest = new List<int>();
            enemiesToKill = new List<int>();
            enemyNames = new List<string>();

            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";
        }

        // METHODS \\


        public virtual void LoadContent()
        {
        }

        public virtual void UnloadContent()
        {
            UnloadNPCContent();
            Sound.enemySoundEffects.Clear();
            content.Unload();
            background.Clear();
        }

        public virtual void UnloadNPCContent()
        {

        }

        public virtual void LoadEnemyData()
        {
            game.ResetEnemySpriteList();
        }

        public virtual void PlayBackgroundMusic()
        {

        }

        public virtual void PlayAmbience()
        {

        }
        //-- Update method
        public virtual void Update()
        {
            last = current;
            current = Keyboard.GetState();

            mapNameTimer++;
            

            #region Enemies update, death, and quest kill incrementing
            //--For every enemy on screen, call update
            for (int i = 0; i < enemiesInMap.Count; i++)
            {
                enemiesInMap[i].Update(mapWidth);

                //Check to see if any passives interact with the enemy
                for (int e = 0; e < player.OwnedPassives.Count; e++)
                {
                    player.OwnedPassives[e].CheckEnemyCollisions(enemiesInMap[i]);
                }

                //--If the enemy is dead, remove it
                if (enemiesInMap[i].IsDead())
                {
       
                    player.HitPauseTimer = 6;
                    game.Camera.ShakeCamera(5, 7);

                    //--Remove it from the enemies in map dictionary if it is a map with a set number of enemy types
                    if (enemyNamesAndNumberInMap.ContainsKey(enemiesInMap[i].EnemyType))
                    {
                        enemyNamesAndNumberInMap[enemiesInMap[i].EnemyType]--;
                    }

                    //-- REGULAR QUEST
                    if (game.CurrentQuests != null)
                    {
                        for (int j = 0; j < game.CurrentQuests.Count; j++)
                        {
                            //If the enemy is the type needed for the current quest, update the enemies killed number for the quest
                            for (int k = 0; k < game.CurrentQuests[j].EnemyNames.Count; k++)
                            {
                                Enemy currentEnemy = enemiesInMap[i];
                                if (currentEnemy.EnemyType == game.CurrentQuests[j].EnemyNames[k] &&
                                    game.CurrentQuests[j].EnemiesKilledForQuest[k] < game.CurrentQuests[j].EnemiesToKill[k])
                                {

                                    game.CurrentQuests[j].EnemiesKilledForQuest[k]++;
                                    break;
                                }
                            }
                        }
                    }

                    //-- MAP QUEST
                    if (mapWithMapQuest)
                    {
                        //If the enemy is the type needed for the current map quest, update the enemies killed number for the quest
                        for (int k = 0; k < enemyNames.Count; k++)
                        {
                            Enemy currentEnemy = enemiesInMap[i];
                            if (currentEnemy.EnemyType == enemyNames[k] &&
                                enemiesKilledForQuest[k] < enemiesToKill[k])
                            {
                                enemiesKilledForQuest[k]++;
                                break;
                            }
                        }

                    }

                    enemiesInMap.Remove(enemiesInMap[i]);
                }
            }
            #endregion

            //Portal locks
            for (int i = 0; i < portals.Count; i++)
            {

                if (portals.ElementAt(i).Key.ItemNameToUnlock != "" && portals.ElementAt(i).Key.ItemNameToUnlock != null)
                {
                    portals.ElementAt(i).Key.UpdateLock();

                    if (portals.ElementAt(i).Key.startedToOpen)
                    {
                        portals.ElementAt(i).Key.KillLock();
                    }
                }
            }

            //--Update all of the switches
            for (int i = 0; i < switches.Count; i++)
            {
                switches[i].Update();
            }

            for (int i = 0; i < collectibles.Count; i++)
            {
                collectibles[i].Update();
            }

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                interactiveObjects[i].Update();
            }

            for (int i = 0; i < mapQuestSigns.Count; i++)
            {
                mapQuestSigns[i].Update();

                Rectangle frec = new Rectangle(mapQuestSigns[i].Rec.X + mapQuestSigns[i].Rec.Width / 2 - 43 / 2,
                    mapQuestSigns[i].Rec.Y + 100, 43, 65);

                if (player.VitalRec.Intersects(mapQuestSigns[i].Rec) && !mapQuestSigns[i].Active)
                {
                    if ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.RightBumperPressed())
                        mapQuestSigns[i].ActivateSign();

                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }

            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].Update();

                if (platforms[i] is MoveBlock)
                {
                    (platforms[i] as MoveBlock).Update(player, this);
                }
            }

            for (int i = 0; i < lockers.Count; i++)
            {
                //if (((game.CurrentChapter is Prologue) && (game.CurrentChapter as Prologue).CanBreakIntoLockers) || !(game.CurrentChapter is Prologue))
              //  {
                    if (game.CurrentChapter.BossFight == false)
                        lockers[i].CheckIfOpening();
             //   }
            }

            for (int i = 0; i < treasureChests.Count; i++)
            {
                treasureChests[i].Update();
            }

            //Enemy projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update();

                if (projectiles[i].Dead == true)
                {
                    projectiles.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            for (int i = 0; i < storyItems.Count; i++)
            {
                storyItems[i].Update();
            }

            for (int i = 0; i < mapHazards.Count; i++)
            {
                mapHazards[i].Update();
            }

            //Health
            for (int i = 0; i < healthDrops.Count; i++)
            {
                healthDrops[i].Update(platforms, player);

                if (healthDrops[i].TimeOnScreen >= 1800)
                {
                    healthDrops.RemoveAt(i);
                    i--;
                    continue;
                }

                if (healthDrops[i].PickedUp)
                {
                    healthDrops.RemoveAt(i);
                    i--;
                    continue;
                }
                
            }

            //Money
            for (int i = 0; i < moneyDrops.Count; i++)
            {
                moneyDrops[i].Update(platforms, player);

                if (moneyDrops[i].TimeOnScreen >= 1800)
                {
                    moneyDrops.RemoveAt(i);
                    i--;
                    continue;
                }

                if (moneyDrops[i].PickedUp)
                {
                    moneyDrops.RemoveAt(i);
                    i--;
                    continue;
                }

            }

            #region ENEMY DROPS
            for (int i = 0; i < drops.Count; i++)
            {
                drops[i].Update(platforms);

                if (drops[i].MaxTimeOnScreen())
                {
                    drops.RemoveAt(i);
                    i--;
                }

                //--Once the timer reaches 0, remove it from the list of drops
                if (drops[i].PickedUpTimer <= 0)
                {
                    drops.RemoveAt(i);
                    i--;
                }
            }
            #endregion
            
        }

        /// <summary>
        /// Calls the UseSwitch method on the parameter switch. The UseSwitch method simply changes the status of the switch from active to 
        /// nonactive, or vice versa. Returns true if the player is using the switch
        /// </summary>
        /// <param name="sw"></param>
        /// <returns></returns>
        public Boolean CheckSwitch(Switch sw)
        {
            //--If the player is touching it and hits F, turn it on or off
            if (player.VitalRec.Intersects(sw.Rec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.RightBumperPressed()) && player.playerState != ISurvived.Player.PlayerState.jumping && player.playerState != ISurvived.Player.PlayerState.attackJumping)
            {
                sw.UseSwitch();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks a switch to see if it is being used. Pass in a random int that does nothing, simply there to overload the function. Returns nothing
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="randomIntSoICanOverloadThisFunction"></param>
        public void CheckSwitch(Switch sw, int randomIntSoICanOverloadThisFunction)
        {
            //--If the player is touching it and hits F, turn it on or off
            if (player.VitalRec.Intersects(sw.Rec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.RightBumperPressed()))
            {
                sw.UseSwitch();
            }
        }

        //--This method will be used to set the location of portals
        public virtual void SetPortals()
        {
        }

        public virtual void SetDestinationPortals()
        {

        }

        //--This method respawns certain enemies at random locations
        public virtual void RespawnGroundEnemies()
        {
            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }

            monsterX = rand.Next(platforms[platformNum].Rec.X, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);
        }

        //Call this inside of RespawnGroundEnemies before c
        public void GetRandomRespawnPoint(int monsterRecWidth)
        {

        }

        //Spawns enemies in the air within a boundary
        public virtual void RespawnFlyingEnemies(Rectangle bounds)
        {
            monsterX = rand.Next(bounds.X, bounds.X + bounds.Width - 50);
            monsterY = rand.Next(bounds.Y, bounds.Y + bounds.Height - 50);

            pos = new Vector2(monsterX, monsterY);
        }

        //--Draw each enemy on the screen, and the map itself. Including portals
        public virtual void Draw(SpriteBatch s)
        {
            for (int i = 0; i < background.Count; i++)
            {
                if (background.Count != 1)
                {
                    if (i != 0)
                    {
                        s.Draw(background[i], new Rectangle(mapRec.X + background[i - 1].Width, mapRec.Y,
                            background[i].Width, background[i].Height), Color.White);
                    }
                    else
                    {
                        s.Draw(background[i], new Rectangle(mapRec.X, mapRec.Y,
                            background[i].Width, background[i].Height), Color.White);
                    }
                }
                else
                    s.Draw(background[i], new Rectangle(mapRec.X, mapRec.Y,
                        mapRec.Width, mapRec.Height), Color.White);
            }


            for (int i = 0; i < platforms.Count; i++)
            {
                if(platforms[i].DrawPlatform || game.CurrentChapter.state == Chapter.GameState.mapEdit)
                    platforms[i].Draw(s);
            }

            for (int i = 0; i < collectibles.Count; i++)
            {
                collectibles[i].Draw(s);
            }

            for (int i = 0; i < switches.Count; i++)
            {
                switches[i].Draw(s);
                                    
                Rectangle frec = new Rectangle(switches[i].Rec.X + switches[i].Rec.Width / 2 - 43 / 2,
                        switches[i].Rec.Y + 30, 43, 65);

                if (player.VitalRec.Intersects(switches[i].Rec))
                {

                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);

                    /*
                    s.Draw(Game1.fButton, new Rectangle(switches[i].Rec.X + switches[i].Rec.Width / 2 - Game1.fButton.Width / 2,
                        switches[i].Rec.Y - 100, Game1.fButton.Width, Game1.fButton.Height), Color.White * .7f);*/
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }

            for (int i = 0; i < mapQuestSigns.Count; i++)
            {
                mapQuestSigns[i].Draw(s);
            }

            for (int i = 0; i < portals.Count; i++)
            {

                if (portals.ElementAt(i).Key.ItemNameToUnlock != "" && portals.ElementAt(i).Key.ItemNameToUnlock != null)
                {
                    portals.ElementAt(i).Key.DrawLock(s);
                }

                if (portals.ElementAt(i).Key.IsUseable)
                {

                    Rectangle frec = new Rectangle(portals.ElementAt(i).Key.PortalRec.X + portals.ElementAt(i).Key.PortalRec.Width / 2 -
                       43 / 2, portals.ElementAt(i).Key.PortalRec.Y - 100 + portals.ElementAt(i).Key.FButtonYOffset, 43,
                        65);

                    if (player.VitalRec.Intersects(portals.ElementAt(i).Key.PortalRec) &&
                        !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game)
                    {

                        if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(frec))
                            Chapter.effectsManager.AddForeroundFButton(frec);
                    }
                    else
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(frec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(frec);
                    }
                }
            }

            #region DARYL'S LOCKERS (North Hall and The Party)

            if (this is NorthHall)
            {
                Point distanceFromPortal = new Point(Math.Abs(player.VitalRec.Center.X - NorthHall.ToYourLockerButton.ButtonRec.Center.X),
                Math.Abs(player.VitalRec.Center.Y - NorthHall.ToYourLockerButton.ButtonRec.Center.Y));

                //--If it is less than 250 pixels
                if (distanceFromPortal.X < 250 && distanceFromPortal.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game && player.LearnedSkills.Count > 0 && game.CurrentChapter.BossFight == false)
                {
                    s.DrawString(game.Font, "Daryl's Locker & Skill Shop",
   new Vector2(NorthHall.ToYourLockerButton.ButtonRec.X - 30 - 2, NorthHall.ToYourLockerButton.ButtonRec.Y - 40 - 2), Color.Black);

                    s.DrawString(game.Font, "Daryl's Locker & Skill Shop",
                       new Vector2(NorthHall.ToYourLockerButton.ButtonRec.X - 30, NorthHall.ToYourLockerButton.ButtonRec.Y - 40), Color.White);
                }


                Rectangle frec = new Rectangle(NorthHall.ToYourLockerButton.ButtonRecX + NorthHall.ToYourLockerButton.ButtonRecWidth / 2 -
               43 / 2 - 4, NorthHall.ToYourLockerButton.ButtonRecY - 100, 43,
               65);

                if (player.VitalRec.Intersects(NorthHall.ToYourLockerButton.ButtonRec) &&
                    !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game && player.LearnedSkills.Count > 0 && game.CurrentChapter.BossFight == false)
                {

                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }

                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }

                //NorthHall.ToYourLockerButton.Draw(s);
            }

            if (this is TheParty || this is TutorialMapSeven)
            {

                Button lockerButton;

                if (this is TheParty)
                    lockerButton = TheParty.ToYourLockerButton;
                else
                    lockerButton = TutorialMapSeven.ToYourLockerButton;

                Point distanceFromPortal = new Point(Math.Abs(player.VitalRec.Center.X - lockerButton.ButtonRec.Center.X),
                Math.Abs(player.VitalRec.Center.Y - lockerButton.ButtonRec.Center.Y));

                //--If it is less than 250 pixels
                if (distanceFromPortal.X < 250 && distanceFromPortal.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game && (player.LearnedSkills.Count > 0 || this is TutorialMapSeven))
                {
                    s.DrawString(game.Font, "Daryl's Locker / Skill Shop",
   new Vector2(lockerButton.ButtonRec.X + lockerButton.ButtonRecWidth / 2 - (game.Font.MeasureString("Daryl's Locker / Skill Shop").X / 2) - 2, lockerButton.ButtonRec.Y - 40 - 2), Color.Black);

                    s.DrawString(game.Font, "Daryl's Locker / Skill Shop",
                       new Vector2(lockerButton.ButtonRec.X + lockerButton.ButtonRecWidth / 2 - (game.Font.MeasureString("Daryl's Locker / Skill Shop").X / 2), lockerButton.ButtonRec.Y - 40), Color.White);
                }


                Rectangle frec = new Rectangle(lockerButton.ButtonRecX + lockerButton.ButtonRecWidth / 2 -
                43 / 2, lockerButton.ButtonRecY - 100, 43,
                65);

                if (player.VitalRec.Intersects(lockerButton.ButtonRec) &&
                    !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game && (player.LearnedSkills.Count > 0 || this is TutorialMapSeven) && game.CurrentChapter.BossFight == false)
                {

                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }

                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }

            #endregion

            //--STUDENT LOCKERS
            for (int i = 0; i < lockers.Count; i++)
            {
                s.Draw(lockers[i].Tex, new Vector2(lockers[i].Rec.X - 10, lockers[i].Rec.Y - 25), Color.White);

                Point distanceFromLocker = new Point(Math.Abs(player.VitalRec.Center.X - lockers[i].Rec.Center.X),
Math.Abs(player.VitalRec.Center.Y - lockers[i].Rec.Center.Y));

                //--If it is less than 250 pixels, draw the name above the locker
                if (distanceFromLocker.X < 250 && distanceFromLocker.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game)
                {
                    float x = lockers[i].Rec.X + lockers[i].Rec.Width / 2 - (game.Font.MeasureString(lockers[i].LockerName).X / 2);
                    s.DrawString(game.Font, lockers[i].LockerName, new Vector2(x, lockers[i].Rec.Y - 40), Color.Black);
                }

                Rectangle frec = new Rectangle(lockers[i].Rec.X + lockers[i].Rec.Width / 2 - 43 / 2, lockers[i].Rec.Y - 100, 43, 65);

                if (player.VitalRec.Intersects(lockers[i].Rec) && !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                    //s.Draw(Game1.fButton, frec, Color.White * .7f);
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }

            //--LADDERS
            for (int i = 0; i < ladders.Count; i++)
            {
                ladders[i].Draw(s);
            }

            for (int i = 0; i < treasureChests.Count; i++)
            {
                treasureChests[i].Draw(s);
            }

            for (int i = 0; i < storyItems.Count; i++)
            {
                storyItems[i].Draw(s);
            }

            for (int i = 0; i < mapHazards.Count; i++)
            {
                mapHazards[i].Draw(s);
            }

            for (int i = 0; i < moveBlocks.Count; i++)
            {
                moveBlocks[i].Draw(s);
            }

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground == false && !(interactiveObjects[i] is LivingLocker))
                    interactiveObjects[i].Draw(s);
            }

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(s);
            }
        }

        public void DrawLivingLocker(SpriteBatch s)
        {
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i] is LivingLocker)
                    interactiveObjects[i].Draw(s);
            }
        }

        public void DrawPortalInfo(SpriteBatch s)
        {
            for (int i = 0; i < portals.Count; i++)
            {

                if (portals.ElementAt(i).Key.IsUseable)
                {
                    #region Draw map names above doors

                    //--Get the distance from daryl to the door
                    Point distanceFromPortal = new Point(Math.Abs(player.VitalRec.Center.X - portals.ElementAt(i).Key.PortalRec.Center.X),
                    Math.Abs(player.VitalRec.Center.Y - portals.ElementAt(i).Key.PortalRec.Center.Y));

                    //--If it is less than 250 pixels
                    if (distanceFromPortal.X < 250 && distanceFromPortal.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game)
                    {
                        //--Draw the map name if the map at the other end has been discovered
                        if (Game1.schoolMaps.maps[portals.ElementAt(i).Value.MapName].discovered == true)
                        {
                            float meas = game.Font.MeasureString(Game1.schoolMaps.maps[portals.ElementAt(i).Value.MapName].MapName).X;

                            s.DrawString(game.Font, Game1.schoolMaps.maps[portals.ElementAt(i).Value.MapName].MapName,
new Vector2(portals.ElementAt(i).Key.PortalRec.X + portals.ElementAt(i).Key.PortalRec.Width / 2 - meas / 2 - 2, portals.ElementAt(i).Key.PortalRec.Y - 40 - 2 + portals.ElementAt(i).Key.PortalNameYOffset), Color.Black);

                            s.DrawString(game.Font, Game1.schoolMaps.maps[portals.ElementAt(i).Value.MapName].MapName,
                                new Vector2(portals.ElementAt(i).Key.PortalRec.X + portals.ElementAt(i).Key.PortalRec.Width / 2 - meas / 2, portals.ElementAt(i).Key.PortalRec.Y - 40 + portals.ElementAt(i).Key.PortalNameYOffset), Color.White);//new Color(241, 107, 79));
                        }
                        //--Otherwise draw a question mark
                        else
                        {
                            float meas = game.Font.MeasureString("???").X;
                            s.DrawString(game.Font, "???",
new Vector2(portals.ElementAt(i).Key.PortalRec.X + portals.ElementAt(i).Key.PortalRec.Width / 2 - meas / 2 - 1, portals.ElementAt(i).Key.PortalRec.Y - 40 - 1 + portals.ElementAt(i).Key.PortalNameYOffset), Color.Black);
                            s.DrawString(game.Font, "???",
                                new Vector2(portals.ElementAt(i).Key.PortalRec.X + portals.ElementAt(i).Key.PortalRec.Width / 2 - meas / 2, portals.ElementAt(i).Key.PortalRec.Y - 40 + portals.ElementAt(i).Key.PortalNameYOffset), Color.White);//new Color(241, 107, 79));

                        }
                    }
                    #endregion
                }
            }
        }

        public void DrawDrops(SpriteBatch s)
        {
            for (int i = 0; i < drops.Count; i++)
            {
                drops[i].Draw(s);
            }

            for (int i = 0; i < healthDrops.Count; i++)
            {
                healthDrops[i].Draw(s);
            }

            for (int i = 0; i < moneyDrops.Count; i++)
            {
                moneyDrops[i].Draw(s);
            }
        }

        public void DrawEnemies(SpriteBatch s)
        {
            for (int i = 0; i < enemiesInMap.Count; i++)
            {
                if(!(enemiesInMap[i] is SteeringEnemy))
                    enemiesInMap[i].Draw(s);
            }
        }

        public void DrawEnemyForegroundEffects(SpriteBatch s)
        {
            for (int i = 0; i < enemiesInMap.Count; i++)
            {
                enemiesInMap[i].DrawForegroundEffect(s);
            }
        }
        
        //They need to be drawn in the foreground
        public void DrawFlyingEnemies(SpriteBatch s)
        {
            for (int i = 0; i < enemiesInMap.Count; i++)
            {
                if ((enemiesInMap[i] is SteeringEnemy))
                    enemiesInMap[i].Draw(s);
            }
        }

        public void DrawEnemyDamage(SpriteBatch s)
        {
            for (int i = 0; i < enemiesInMap.Count; i++)
            {
                enemiesInMap[i].DrawDamage(s);
            }

            if (game.CurrentChapter.BossFight)
                game.CurrentChapter.CurrentBoss.DrawDamage(s);
        }

        /// <summary>
        /// If a player uses a portal, it returns the destination map's name. Else, returns null
        /// </summary>
        /// <returns></returns>
        public virtual String CheckPortals()
        {
            for (int i = 0; i < portals.Count; i++)
            {
                if (portals.ElementAt(i).Key.IsUseable)
                {
                    //If the player uses a portal
                    if (player.VitalRec.Intersects(portals.ElementAt(i).Key.PortalRec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.RightBumperPressed())  && CheckPortalLock(portals.ElementAt(i).Key) && player.playerState != ISurvived.Player.PlayerState.jumping && player.playerState != ISurvived.Player.PlayerState.attackJumping && player.Ducking == false)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.DoorOpen);
                        MapNameTimer = 0;
                        player.nextMapPos = new Vector2(portals.ElementAt(i).Value.PortalRec.X - 200, portals.ElementAt(i).Value.PortalRec.Y + portals.ElementAt(i).Value.PortalRec.Height - 170 - 135 - 37);
                        drops.Clear();
                        nextMapName = portals.ElementAt(i).Value.MapName;
                        player.Landing = false;

                        currentLoadingTipNum = rand.Next(game.loadingTips.Count);

                        //This sets the startingPortal for the player equal to the portal you will be entering from
                        //Every portal has a sister portal that is hooked up in the nextMap. The startingPortal is set to this
                        game.CurrentChapter.StartingPortal = portals.ElementAt(i).Value;

                        if (nextMapName == "Bathroom")
                        {
                            Bathroom.LastMapPortal = portals.ElementAt(i).Key;
                        }

                        Chapter.effectsManager.ClearDialogue();

                        Game1.schoolMaps.maps[nextMapName].firstFrameInMap = true;

                        Game1.schoolMaps.maps[nextMapName].ResetMapAssetsOnEnter();

                        if (Game1.schoolMaps.maps[nextMapName].yScroll)
                            player.YScroll = true;
                        else
                            player.YScroll = false;

                        return nextMapName;
                    }
                }
            }

            return "null";
        }

        //--Unlocking doors
        public bool CheckPortalLock(Portal portal)
        {
            //--If there is an item required to open
            if (portal.ItemNameToUnlock != null)
            {
                //--If it requires the standard key and the player has one
                if ((portal.ItemNameToUnlock == "Bronze Key" && player.BronzeKeys > 0) ||
                    (portal.ItemNameToUnlock == "Silver Key" && player.SilverKeys > 0) ||
                    (portal.ItemNameToUnlock == "Gold Key" && player.GoldKeys > 0))
                {
                    //--Take one and open the door
                    if (portal.ItemNameToUnlock == "Bronze Key")
                        player.BronzeKeys--;
                    else if (portal.ItemNameToUnlock == "Silver Key")
                        player.SilverKeys--;
                    else if (portal.ItemNameToUnlock == "Gold Key")
                        player.GoldKeys--;

                    if (portal.openingLock == false)
                    {
                        portal.startedToOpen = true;
                    }
                    return false;
                }
                //--If it requires a special item, don't take it but open the door
                else if (player.StoryItems.ContainsKey(portal.ItemNameToUnlock))
                {
                    if (portal.openingLock == false)
                    {
                        portal.startedToOpen = true;
                    }
                    return false;
                }
                else
                {
                    Chapter.effectsManager.AddLockedDoorMessage();
                    return false;
                }

            }

            //--If it doesn't need a key, open the door
            else
                return true;
        }

        //Use this method to create a binary map file of every map file in the game
        //Only needs to be called if there are binary map files missing or some are outdated
        //When using the map edit mode a binary file is created of every map. The issue with this is it adds in those extra barriers on the side
        //of the map. This method is much more powerful to use, as long as the normal map text files are correct
        public void CreateBinaryMapFiles()
        {

            String file = "Maps\\" + mapName + ".txt";

            //Only do this if a regular map file exists for the map
            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    String[] lines = line.Split(',');

                    Rectangle rec = new Rectangle(int.Parse(lines[3]), int.Parse(lines[4]), int.Parse(lines[1]), int.Parse(lines[2]));
                    bool pass = Boolean.Parse(lines[5]);
                    bool spawn = Boolean.Parse(lines[6]);
                    bool invis = Boolean.Parse(lines[7]);
                    Platform plat = new Platform(Game1.platformTextures[lines[0]], rec, pass, spawn, invis);
                    platforms.Add(plat);
                }
                sr.Close();


                BinaryWriter write = new BinaryWriter(File.Open("Maps\\" + mapName + "Binary.txt", FileMode.Create));

                for (int i = 0; i < Platforms.Count; i++)
                {
                    write.Write("RockFloor2");
                    write.Write(Platforms[i].Rec.Width);
                    write.Write(Platforms[i].Rec.Height);
                    write.Write(Platforms[i].Rec.X);
                    write.Write(Platforms[i].Rec.Y);
                    write.Write(Platforms[i].Passable);
                    write.Write(Platforms[i].SpawnOnTop);
                    write.Write(false);
                }
                write.Close();

                platforms.Clear();
            }
        }

        public virtual void AddPlatforms()
        {
            //ENABLE THIS IF YOU WANT TO MAKE A BINARY COPY OF EVERY MAP THAT HAS A REGULAR TEXT FILE. GOOD FOR UPDATING BINARY FILES WHEN A MAP FILE HAS BEEN CHANGED
            CreateBinaryMapFiles();

            //This section reads the binary map file and creates a text file called "ReadBinary" that has the information of every platform in the
            //binary file. That way we'll have just a regular text file that is easy to create platforms out of
            //*************************************************************************************
            String file2 = "Maps\\" + mapName + "ReadBinary.txt";
            File.Delete(file2);
            StreamWriter sw2 = File.AppendText(file2);

            BinaryReader reader = new BinaryReader(File.Open("Maps\\" + mapName + "Binary.txt", FileMode.Open));

            //While the reader's stream position is not the end of the file, add a new line to the text file (Basically a new platform)
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                sw2.WriteLine(reader.ReadString() + "," + reader.ReadInt32() + "," +
    reader.ReadInt32() + "," + reader.ReadInt32() + "," + reader.ReadInt32() + "," + reader.ReadBoolean() + "," + reader.ReadBoolean() + "," + reader.ReadBoolean());
            }


            sw2.Close();
            reader.Close();
            //*************************************************************************************

            //This part opens up a streamreader with the "ReadBinary" file we just created.
            //It then reads one line at a time and parses it at the commas, effectively splitting the line (a platform's attributes) into individual 
            //sections (individual attributes such as width, height, etc)
            //It then creates a platform out of said attributes, adds it to the list, and reads the next line as long as it isn't the end of the file
            //*************************************************************************************
            String file = "Maps\\" + mapName + "ReadBinary.txt";
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                String[] lines = line.Split(',');

                Rectangle rec = new Rectangle(int.Parse(lines[3]), int.Parse(lines[4]), int.Parse(lines[1]), int.Parse(lines[2]));
                bool pass = Boolean.Parse(lines[5]);
                bool spawn = Boolean.Parse(lines[6]);
                bool invis = Boolean.Parse(lines[7]);
                Platform plat = new Platform(Game1.platformTextures[lines[0]], rec, pass, spawn, invis);
                platforms.Add(plat);
            }
            sr.Close();
            //**************************************************************************************

            //Delete the temporary "ReadBinary" text file we created, because we don't need it anymore and we don't want the player to see it
            File.Delete("Maps\\" + mapName + "ReadBinary.txt");
        }

        public virtual void AddNPCs()
        {

        }

        /// <summary>
        /// Use this method to draw parallax items and anything in the foreground. Things in the foreground are drawn normally, while parallax has a special float passed in for depth
        /// </summary>
        /// <param name="s"></param>
        public virtual void DrawParallaxAndForeground(SpriteBatch s)
        {
        }

        public virtual void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }

        public virtual void DrawMapOverlay(SpriteBatch s)
        {
        }

        /// <summary>
        /// Use this to reset positions of things if they should be reset when the player enters it
        /// </summary>
        public virtual void ResetMapAssetsOnEnter()
        {

        }

        //-Adds invisible walls to the left and right sides of the map
        public virtual void AddBounds()
        {
            mapEdgePlat = new Platform(Game1.emptyBox, new Rectangle(-50, mapRec.Y - 500, 50, MapHeight + 500), false, false, true);
            platforms.Add(mapEdgePlat);

            mapEdgePlat = new Platform(Game1.emptyBox, new Rectangle(mapWidth, mapRec.Y - 500, 50, MapHeight + 500), false, false, true);
            platforms.Add(mapEdgePlat);
        }

        /// <summary>
        /// Draws the map name at the top of the screen, which fades out. Call it inside the specific chapter's
        /// gameState under static drawing
        /// </summary>
        /// <param name="s"></param>
        public void DrawMapName(SpriteBatch s)
        {

            //--Measures the map name in pixels to center it on the screen
            Vector2 stringMeasure = Game1.youFoundFont.MeasureString(mapName);
            float vecX = (float)(1280 / 2) - (float)(stringMeasure.X / 2); 

            if (mapNameTimer <= 100)
            {
                s.DrawString(Game1.youFoundFont, mapName, new Vector2(vecX - 2, 50 - 2), Color.Black);
                s.DrawString(Game1.youFoundFont, mapName, new Vector2(vecX, 50), Color.White);
            }
            if (mapNameTimer > 100 && mapNameTimer < 300f)
            {
                mapNameAlpha -= .010f;
                s.DrawString(Game1.youFoundFont, mapName, new Vector2(vecX - 2, 50 - 2), Color.Black * mapNameAlpha);
                s.DrawString(Game1.youFoundFont, mapName, new Vector2(vecX, 50), Color.White * mapNameAlpha);
            }
            else
                mapNameAlpha = 1f;
        }
    }
}
