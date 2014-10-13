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
    public class Player : GameObject
    {
        // ATTRIBUTES \\
        int maxHealth;
        int health; //health
        int defense; //defense
        int strength;
        int moveSpeed;
        int pushBlockSpeed;
        int jumpHeight;
        int airMoveSpeed;
        int skillUsed;
        int frameDelay = 5;
        int moveFrame;
        int invincibleTime;
        int level;
        int experience;
        int experienceUntilLevel;
        int levelUpTextTimer;
        int statPoints;
        int strengthPoints;
        int tolerancePoints;
        int motivationPoints;
        double money;
        int karma;
        int socialRankIndex;
        int textbooks;
        int bronzeKeys, silverKeys, goldKeys;
        bool falling;
        bool landing = false;
        bool attackFalling = false;
        bool facingRight;
        bool knockedBack;
        bool invincible;
        bool attackFloating = false;
        bool dead = false;
        bool hasCellPhone = false;
        Boolean dancing = false;
        Boolean cutsceneMoving = false;
        Boolean canJump = false;
        Boolean jumpingDown = false;
        Boolean sprinting = false;
        Boolean sprintJump = false;
        Boolean levelingUp = false;
        Boolean danceAnimationForward = false;
        int danceState;
        int jumpDowntimer = 0;

        List<Passive> ownedPassives;

        public int healthAddedDuringLevel, defenseAddedDuringLevel, strengthAddedDuringLevel;

        public Vector2 nextMapPos; //This is used when changing maps so the game knows where to place the player

        int pickUpCooldown = 0; //Goes to 10
        bool cantPickUp = false;

        String socialRank;
        float moneyJustPickedUp = 0f;
        int moneyJustPickedUpTimer = 0;
        Game1 game;
        Texture2D playerSheet;
        KeyboardState current;
        KeyboardState last;
        Rectangle mouseRec;

        Rectangle platGrabRec;
        Rectangle jumpingVitalRec;
        Rectangle duckingVitalRec;

        MapClass currentMap;
        Platform currentPlat;
        Ladder currentLadder;

        List<Skill> equippedSkills;
        List<Skill> learnedSkills;

        List<Enemy> enemies;

        List<Weapon> ownedWeapons;
        List<Hat> ownedHats;
        List<Hoodie> ownedHoodies;
        List<Accessory> ownedAccessories;
        Dictionary<String, Boolean> allMonsterBios;
        Dictionary<String, Boolean> allCharacterBios;

        Weapon equippedWeapon = null;
        Weapon secondWeapon = null;
        Hat equippedHat = null;
        Hoodie equippedHoodie = null;
        Accessory equippedAccessory = null;
        Accessory secondAccessory= null;

        Dictionary<String, int> enemyDrops;
        Dictionary<String, int> craftingAndCatalysts;
        Dictionary<String, int> storyItems;

        List<String> pickUps; //This holds 5 drops to be drawn on the HUD as you pick them up
        List<int> pickUpTimers; //Holds ints relating to how long the text for picking an item up stays on the screen

        Boolean ducking = false;
        Boolean standingBackUp = false;
        Boolean drawStandingBackUpLines = false;

        int deathTimer; //Increases after death to tell when to go to the game over screen

        int flinchType; //Randomly determines which flinch to use
        int flinchTimer;
        Random selectFlinch;

        int starFrame, starTimer;

        // STATE MACHINE \\
        public enum PlayerState
        {
            standing,
            running,
            walking,
            attacking,
            jumping,
            attackJumping,
            relaxedStanding,
            climbingLadder,
            grabbingLedge,
            pushingBlock, 
            dead
        }
        public PlayerState playerState;

        Boolean climbingUp = false;
        int timeHanging;

        Boolean platBelowPlayerForStairs = false;

        // PROPERTIES \\
        public int BronzeKeys { get; set; }
        public int SilverKeys { get; set; }
        public int GoldKeys { get; set; }
        public List<Skill> LearnedSkills { get { return learnedSkills; } set { learnedSkills = value; } }
        public List<Skill> EquippedSkills { get { return equippedSkills; } set { equippedSkills = value; } }
        public List<Enemy> Enemies { get { return enemies; } set { enemies = value; } }
        public List<String> PickUps { get { return pickUps; } set { pickUps = value; } }
        public Boolean CanJump { get { return canJump; } set { canJump = value; } }
        public Boolean Ducking { get { return ducking; } set { ducking = value; } }
        public Boolean LevelingUp { get { return levelingUp; } set { levelingUp = value; } }
        public int InvincibleTime { get { return invincibleTime; } set { invincibleTime = value; } }
        public int SocialRankIndex { get { return socialRankIndex; } set { socialRankIndex = value; } }
        public Texture2D PlayerSheet { get { return playerSheet; } }
        public Rectangle PlayerRec { get { return rec; } set { rec = value; } }
        public Rectangle VitalRec { get { return vitalRec; } set { vitalRec = value; } }
        public Rectangle JumpingVitalRec { get { return jumpingVitalRec; } set { jumpingVitalRec = value; } }
        public Rectangle DuckingVitalRec { get { return duckingVitalRec; } set { duckingVitalRec = value; } }
        public bool FacingRight { get { return facingRight; } set { facingRight = value; } }
        public bool KnockedBack { get { return knockedBack; } set { knockedBack = value; } }
        public bool Falling { get { return falling; } set { falling = value; } }
        public bool AttackFloating { get { return attackFloating; } set { attackFloating = value; } }
        public bool HasCellPhone { get { return hasCellPhone; } set { hasCellPhone = value; } }
        public bool AttackFalling { get { return attackFalling; } set { attackFalling = value; } }
        public bool Sprinting { get { return sprinting; } set { sprinting = value; } }

        //--Base states
        public int Strength { get { return strength; } set { strength = value; } }
        public int Health { get { return health; } set { health = value; if (health > maxHealth) health = maxHealth; else if (health < 0) health = 0; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int Defense { get { return defense; } set { defense = value; } }
        public int MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public int AirMoveSpeed { get { return airMoveSpeed; } set { airMoveSpeed = value; } }
        public int PushBlockSpeed { get { return pushBlockSpeed; } set { pushBlockSpeed = value; } }
        public int JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
        public int Level { get { return level; } set { level = value; } }
        public int Experience { get { return experience; } set { experience = value; } }
        public int ExperienceUntilLevel { get { return experienceUntilLevel; } set { experienceUntilLevel = value; } }
        public double Money { get { return money; } set { money = value; } }
        public int Karma { get { return karma; } set { karma = value; CheckSocialRankUp();} }
        public int StatPoints { get { return statPoints; } set { statPoints = value; } }
        public int StrengthPoints { get { return strengthPoints; } set { strengthPoints = value; } }
        public int TolerancePoints { get { return tolerancePoints; } set { tolerancePoints = value; } }
        public int MotivationPoints { get { return motivationPoints; } set { motivationPoints = value; } }
        public String SocialRank { get { return socialRank; } set { socialRank = value; } }
        public int Textbooks { get { return textbooks; } set { textbooks = value; } }
        public float MoneyJustPickedUp { get { return moneyJustPickedUp; } set { moneyJustPickedUp = value; } }

        //--Equipment
        public List<Weapon> OwnedWeapons 
        { 
            get { return ownedWeapons; } 
            set 
            { 
                //Check to see if the player is getting a new weapon. If he is, set the newWeapon value to true in the inventory
                List<Weapon> temp = ownedWeapons; 
                ownedWeapons = value; 
                if (ownedWeapons.Count > temp.Count) 
                { 
                    game.Notebook.Inventory.newWeapon = true; 
                } 
            } 
        }
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }
        public Weapon SecondWeapon { get { return secondWeapon; } set { secondWeapon = value; } }

        public List<Hat> OwnedHats
        {
            get
            {
                return ownedHats;
            }
            set
            {
                List<Hat> temp = ownedHats;
                ownedHats = value;
                if (ownedHats.Count > temp.Count)
                {
                    game.Notebook.Inventory.newHat = true;
                }
            }
        }
        public Hat EquippedHat { get { return equippedHat; } set { equippedHat = value; } }

        public List<Hoodie> OwnedHoodies 
        { 
            get 
            { 
                return ownedHoodies; 
            }
            set 
            {
                List<Hoodie> temp = ownedHoodies;
                ownedHoodies = value;
                if (ownedHoodies.Count > temp.Count)
                {
                    game.Notebook.Inventory.newShirt = true;
                } 
            } 
        }
        public Hoodie EquippedHoodie { get { return equippedHoodie; } set { equippedHoodie = value; } }

        public List<Accessory> OwnedAccessories
        {
            get
            {
                return ownedAccessories;
            }
            set
            {
                List<Accessory> temp = ownedAccessories;
                ownedAccessories = value;
                if (ownedAccessories.Count > temp.Count)
                {
                    game.Notebook.Inventory.newAccessory = true;
                }
            }
        }
        public Accessory EquippedAccessory { get { return equippedAccessory; } set { equippedAccessory = value; } }
        public Accessory SecondAccessory { get { return secondAccessory; } set { secondAccessory = value; } }

        public Dictionary<String, int> EnemyDrops
        {
            get
            {
                return enemyDrops;
            }
            set
            {
                Dictionary<string, int> temp = enemyDrops;
                enemyDrops = value;
                if (enemyDrops.Count > temp.Count)
                {
                    game.Notebook.Inventory.newLoot = true;
                }
            }
        }
        public Dictionary<String, int> CraftingAndCatalysts { get { return craftingAndCatalysts; } set { craftingAndCatalysts = value; } }
        public Dictionary<String, int> StoryItems { get { return storyItems; } set { storyItems = value; } }
        public Dictionary<String, Boolean> AllCharacterBios { get { return allCharacterBios; } set { allCharacterBios = value; } }
        public Dictionary<String, Boolean> AllMonsterBios { get { return allMonsterBios; } set { allMonsterBios = value; } }
        public int MoveFrame { get { return moveFrame; } set { moveFrame = value; } }
        public int FrameDelay { get { return frameDelay; } set { frameDelay = value; } }
        public Platform CurrentPlat { get { return currentPlat; } set { currentPlat = value; } }
        public Ladder CurrentLadder { get { return currentLadder; } }
        public List<Passive> OwnedPassives { get { return ownedPassives; } set { ownedPassives = value; } }
        public Boolean Landing { get { return landing; } set { landing = value; } }
        public Boolean IsStunned { get { return isStunned; } set { isStunned = value; } }
        // CONSTRUCTOR \\
        public Player(Texture2D sheet, SpriteFont font, Game1 g)
            :base()
        {

            //--New lists and stuff
            selectFlinch = new Random();
            game = g;
            rec = new Rectangle(3000, 200, 530, 398);
            vitalRec = new Rectangle(60, 100, 60, 170);
            jumpingVitalRec = new Rectangle(60, 100, 60, 110);
            duckingVitalRec = new Rectangle(60, 100, 60, 30);
            equippedSkills = new List<Skill>();
            learnedSkills = new List<Skill>();
            enemies = new List<Enemy>();
            position = new Vector2(1050, 290); //1350/1050, 290 for DEMO 
            ownedWeapons = new List<Weapon>();
            ownedHats = new List<Hat>();
            ownedHoodies = new List<Hoodie>();
            ownedAccessories = new List<Accessory>();
            allCharacterBios = new Dictionary<string, bool>();
            allMonsterBios = new Dictionary<string, bool>();
            SetCharacterBioDictionary();
            SetMonsterBioDictionary();
            craftingAndCatalysts = new Dictionary<string, int>();
            ownedPassives = new List<Passive>();
            playerSheet = sheet;
            this.font = font;
            levelUpTextTimer = 0;
            yScroll = false;
            enemyDrops = new Dictionary<string, int>();
            pickUps = new List<string>();
            pickUpTimers = new List<int>();
            storyItems = new Dictionary<string, int>();
            mouseRec = new Rectangle(0, 0, Cursor.cursorWidth / 2, Cursor.cursorHeight / 2);
            canJump = true;

            //--Base stats
            socialRank = "New Kid";
            karma = 0;
            maxHealth = 20;
            health = maxHealth;
            strength = 18;
            defense = 1;
            moveSpeed = 7;
            airMoveSpeed = 7;
            jumpHeight = -21;
            level = 1;
            experience = 0;
            experienceUntilLevel = 4;

            //--Status
            playerState = PlayerState.standing;
            knockedBack = false;
            facingRight = true;
            invincible = false;

            platGrabRec = new Rectangle(0, 0, 50, 125);
        }

        // METHODS \\

        //--Returns the sprite for the current frame from the spritesheet
        public Rectangle GetSourceRectangle(int frame)
        {
            if (isStunned)
            {
                if (moveFrame == 0 || playerState == PlayerState.jumping || playerState == PlayerState.attackJumping )
                {
                    if (flinchType == 0)
                        return new Rectangle(3180, 0, 530, 398);
                    else
                        return new Rectangle(0, 398, 530, 398);
                }
                else
                {
                    return new Rectangle(530 + (530 * (moveFrame - 1)), 398, 530, 398);
                }
            }

            //Draw the player flinched
            if (knockedBack)
            {
                if (flinchType == 0)
                    return new Rectangle(3180, 0, 530, 398);
                else
                    return new Rectangle(0, 398, 530, 398);
            }

            //Standing to talk
            if(game.CurrentChapter.TalkingToNPC)
                return new Rectangle(3180, 398, 530, 398);

            switch (playerState)
            {
                case PlayerState.dead:
                    return new Rectangle(1060 + (530 * moveFrame), 3184, 530, 398);

                case PlayerState.relaxedStanding:
                    return new Rectangle(3180, 398, 530, 398);

                case PlayerState.standing:

                    if (landing)
                        return new Rectangle(2120 + (530 * frame), 398, 530, 398);
                    else if (ducking)
                        return new Rectangle(530 * moveFrame, 0, 530, 398);
                    else if (standingBackUp)
                        return new Rectangle(1590, 0, 530, 398);
                    else
                    {
                        if (dancing)
                        {
                            //if (frame < 6)
                            //    return new Rectangle(530 * frame, 0, 530, 398);
                            //else
                            //    return new Rectangle(530 * (frame - 5), 398, 530, 398);
                        }
                        /*else */if (frame == 0)
                            return new Rectangle(3180, 1194, 530, 398);
                        else
                            return new Rectangle(530 * (frame - 1), 1592, 530, 398);
                    }

                case PlayerState.attacking:
                    return new Rectangle(0, 0, 530, 398);

                case PlayerState.running:
                    if (sprinting)
                    {
                        if (frame < 4)
                        {
                            return new Rectangle(1590 + (frame * 530), 398, 530, 398);
                        }
                        else if(frame != 11)
                        {
                            return new Rectangle((frame - 4) * 530, 796, 530, 398);
                        }
                        else
                            return new Rectangle(0, 1194, 530, 398);
                    }
                    else
                    {
                        if (frame < 7)
                        {
                            return new Rectangle(frame * 530, 796, 530, 398);
                        }
                        else
                        {
                            return new Rectangle((frame - 7) * 530, 1194, 530, 398);
                        }
                    }
                case PlayerState.walking:
                    if (frame < 7)
                    {
                        return new Rectangle(frame * 530, 1592, 530, 398);
                    }
                    else
                    {
                        int column = frame - 7;
                        return new Rectangle(column * 530, 1990, 530, 398);
                    }
                case PlayerState.jumping:
                    if (attackFalling)
                    {
                        return new Rectangle(1590, 1990, 530, 398);
                    }

                    if (frame < 3)
                        return new Rectangle(2120 + (frame * 530), 1990, 530, 398);
                    else if (frame < 10)
                        return new Rectangle((frame - 3) * 530, 2388, 530, 398);
                    else if (frame < 17)
                        return new Rectangle((frame - 10) * 530, 2786, 530, 398);
                    else
                        return new Rectangle((frame - 17) * 530, 3184, 530, 398);
                case PlayerState.attackJumping:
                    return new Rectangle(3180, 398, 530, 398);

                case PlayerState.climbingLadder:
                    return new Rectangle(3180, 3980, 530, 398);

                case PlayerState.grabbingLedge:
                    return new Rectangle(530 * 2, 0, 530, 398);

                case PlayerState.pushingBlock:
                    return new Rectangle(3180, 3980, 530, 398);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public void UpdatePosition()
        {
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            vitalRec.X = rec.X + (rec.Width / 2) - 30;
            vitalRec.Y = rec.Y + (rec.Height / 4) + 37;

            jumpingVitalRec.X = rec.X + (rec.Width / 2) - 30;
            jumpingVitalRec.Y = rec.Y + (rec.Height / 4) + 37 + 30;

            duckingVitalRec.X = rec.X + (rec.Width / 2) - 30;
            duckingVitalRec.Y = rec.Y + rec.Height - 100;
        }

        public override void Update()
        {
            base.Update();

            if (dead && game.CurrentChapter.state != Chapter.GameState.dead)
            {
                Move();
                UpdatePosition();

                deathTimer++;

                if (deathTimer >= 500)
                {
                    deathTimer = 0;
                    Game1.deathScreen.LoadContent();
                    game.CurrentChapter.state = Chapter.GameState.dead;
                }
            }
            else if (hitPauseTimer >= 0)
                hitPauseTimer--;

            else
            {
                CheckIsDead();
                if (flinchTimer > 0)
                    flinchTimer--;

                //--Update position
                currentMap = game.CurrentChapter.CurrentMap;
                UpdatePosition();

                //--Keyboard input
                last = current;
                current = Keyboard.GetState();

                //--Call other updating methods every frame.
                //--Gravity, skills, and knockback must be updated every frame
                ImplementGravity();
                UpdateSkills();
                UpdateKnockBack();
                UpdateInvincible();
                PickUpDrops();
                FallOffMap();
                JumpDown();
                levelUpTextTimer--;

                if (moneyJustPickedUpTimer > 0)
                {
                    UpdateMoneyJustPickedUp();
                }

                //Update passive skills
                for (int i = 0; i < ownedPassives.Count; i++)
                {
                    ownedPassives[i].Update();
                }


                //Set velocity equal to the current platforms velocity
                if (currentPlat != null && currentPlat.Velocity != Vector2.Zero)
                {
                    velocity = currentPlat.Velocity;
                }

                //Only do this shit if you aren't stunned
                if (!isStunned)
                {
                    //--Attack is called, but the code is only run if the player is in the attacking state
                    Attack();
                    LevelUp();
                    Move();
                }
                else
                {
                    //If the stun isn't up
                    if (moveFrame < 4)
                    {
                        //Stars
                        starTimer--;

                        if (starTimer <= 0)
                        {
                            starFrame++;
                            starTimer = 15;

                            if (starFrame > 3)
                            {
                                starFrame = 0;
                            }
                        }

                        //End stars


                        frameDelay--;

                        if (moveFrame == 3)
                        {
                            if (frameDelay < 5)
                                alpha = 0;
                            else if (frameDelay > 9 && frameDelay < 15)
                                alpha = 0;
                            else if (frameDelay > 19 && frameDelay < 25)
                                alpha = 0;
                            else if (frameDelay > 29 && frameDelay < 40)
                                alpha = 0;
                            else if (frameDelay > 49 && frameDelay < 60)
                                alpha = 0;
                            else
                                alpha = 1f;
                        }

                        if (frameDelay == 0)
                        {
                            frameDelay = 5;
                            moveFrame++;

                            if (moveFrame == 3)
                            {
                                frameDelay = stunTime;
                            }

                            if (moveFrame == 4)
                            {
                                isStunned = false;
                                Invincible(45);
                                stunTime = 0;
                                alpha = 1f;
                                playerState = PlayerState.standing;
                                moveFrame = 0;
                                landing = false;
                            }
                        }
                    }
                }

                for (int i = 0; i < pickUpTimers.Count; i++)
                {
                    pickUpTimers[i]--;

                    if (pickUpTimers[i] <= 0)
                    {
                        pickUpTimers.RemoveAt(i);
                        pickUps.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void Move()
        {

            if (isStunned == false || dead)
            {
                ClimbLadder();
                //If you start running or something after landing, make it so the player isn't standing back up
                if (playerState != PlayerState.standing && drawStandingBackUpLines)
                    drawStandingBackUpLines = false;
                if (playerState != PlayerState.standing && standingBackUp)
                    standingBackUp = false;


                //--Switch based on player state
                switch (playerState)
                {

                    #region STANDING, DUCKING, LANDING
                    case PlayerState.standing:

                        frameDelay--;

                        //If you just landed from a sprint jump, change air move speed back to normal
                        if (sprintJump)
                        {
                            airMoveSpeed = 7;
                            sprintJump = false;
                        }

                        #region LANDING
                        //LANDING
                        if (landing)
                        {
                            //If this frame ends
                            if (frameDelay <= 0)
                            {
                                moveFrame++;
                                frameDelay = 8;

                                if (moveFrame == 2)
                                {
                                    moveFrame = 0;
                                    landing = false;
                                }
                            }

                            //If you try to duck as you land, make it wait a little bit first
                            if ((current.IsKeyDown(Keys.Down) || MyGamePad.DownAnalogHeld()) && (frameDelay < 3 || moveFrame != 0))
                            {
                                moveFrame = 0;
                                frameDelay = 2;
                                ducking = true;
                                landing = false;
                            }

                        }

                        #endregion

                        #region DUCKING
                        //DUCKING
                        else if (ducking)
                        {
                            Console.WriteLine(frameDelay);
                            //Ducking animation
                            if (moveFrame < 2)
                            {
                                frameDelay--;

                                if (frameDelay <= 0)
                                {
                                    moveFrame++;
                                    frameDelay = 2;
                                }
                            }

                            //If you aren't holding DOWN anymore, stand back up
                            if (!current.IsKeyDown(Keys.Down) && !MyGamePad.DownAnalogHeld())
                            {

                                ducking = false;
                                frameDelay = 5;
                                standingBackUp = true;
                            }

                        }
                        #endregion

                        #region STANDING BACK UP
                        else if (standingBackUp)
                        {
                            //This only lasts about 5 frames, and afterwards, go back to regular standing and draw the motion lines in Draw()
                            if (frameDelay <= 0)
                            {
                                frameDelay = 5;
                                moveFrame = 0;
                                standingBackUp = false;
                                drawStandingBackUpLines = true;
                            }
                        }
                        #endregion

                        #region DANCING AND STANDING
                        //DANCING
                        else
                        {
                            if (dancing)
                            {
                                //If this frame ends
                                if (frameDelay == 0)
                                {
                                    if (drawStandingBackUpLines)
                                        drawStandingBackUpLines = false;

                                    //--If the animation is going forward
                                    if (danceAnimationForward)
                                    {
                                        moveFrame++;

                                        //If the animation is on the second or thired loop, and the third frame, go in the opposite direction
                                        //This creates a quick looping effect of a single dance move
                                        if ((danceState == 2 || danceState == 3) && moveFrame == 3)
                                            danceAnimationForward = false;
                                    }
                                    else
                                    {
                                        //Same as above, but only loop once, and on another move
                                        moveFrame--;
                                        if ((danceState == 5) && moveFrame == 4)
                                        {
                                            danceAnimationForward = true;
                                            danceState++;
                                        }
                                    }

                                    //Reset dance loops
                                    if (danceState > 6)
                                        danceState = 0;

                                    frameDelay = 5;
                                }

                                //If the dance reaches the beginning or end, reverse the direction
                                if (moveFrame > 7)
                                {
                                    moveFrame = 7;
                                    danceAnimationForward = !danceAnimationForward;
                                }
                                if (moveFrame < 0)
                                {
                                    danceState++;
                                    moveFrame = 0;
                                    danceAnimationForward = !danceAnimationForward;
                                }
                            }
                            else
                            {

                                if (drawStandingBackUpLines)
                                    drawStandingBackUpLines = false;

                                frameDelay--;

                                if (frameDelay <= 0)
                                {
                                    moveFrame++;
                                    frameDelay = 9;

                                    if (moveFrame > 7)
                                        moveFrame = 0;
                                }
                            }
                        }
                        #endregion

                        //GOING TO DUCK
                        if (current.IsKeyDown(Keys.Down) || MyGamePad.DownAnalogHeld())
                        {
                            if (!ducking)
                            {
                                ducking = true;
                                moveFrame = 0;
                                frameDelay = 2;
                            }

                        }

                        #region RUNNING

                        //--Avoid running in place
                        if ((current.IsKeyDown(Keys.Left) && current.IsKeyDown(Keys.Right)) || (MyGamePad.RightAnalogHeld() && current.IsKeyDown(Keys.Left)) || (MyGamePad.LeftAnalogHeld() && current.IsKeyDown(Keys.Right)))
                        {
                            playerState = PlayerState.standing;
                        }
                        else if ((current.IsKeyDown(Keys.Right) || current.IsKeyDown(Keys.Left) || MyGamePad.LeftAnalogHeld() || MyGamePad.RightAnalogHeld()) && !ducking)
                        {
                            playerState = PlayerState.running;
                            landing = false;
                        }
                        #endregion

                        if (canJump)
                        {
                            #region JUMPING
                            if (knockedBack == false && (current.IsKeyDown(Keys.Up) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed) && falling == false && currentPlat != null && !ducking)
                            {
                                sprinting = false;
                                playerState = PlayerState.jumping;
                                velocity.Y += jumpHeight;
                                moveFrame = 0;
                                landing = false;
                                Chapter.effectsManager.AddJumpDustPoof(rec, facingRight);

                                if(selectFlinch.Next(2) == 0)
                                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerJump1);
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerJump2);
                            }
                            #endregion
                        }

                        //--Check to see if the player is trying to use a skill
                        CheckSkillPress();
                        break;
                    #endregion

                    #region PUSHING BLOX
                    case PlayerState.pushingBlock:

                        if ((current.IsKeyDown(Keys.Left) && current.IsKeyDown(Keys.Right)) || (MyGamePad.RightAnalogHeld() && current.IsKeyDown(Keys.Left)) || (MyGamePad.LeftAnalogHeld() && current.IsKeyDown(Keys.Right)))
                        {
                            playerState = PlayerState.standing;
                        }

                        //DUCK
                        if (current.IsKeyDown(Keys.Down) || MyGamePad.DownAnalogHeld())
                        {
                            ducking = true;
                            moveFrame = 0;
                            playerState = PlayerState.standing;
                            frameDelay = 2;
                        }

                        #region RUN LEFT
                        else if (current.IsKeyDown(Keys.Left) || MyGamePad.LeftAnalogHeld())
                        {
                            position.X -= pushBlockSpeed;

                            if (!(last.IsKeyDown(Keys.Left)) && MyGamePad.previousState.ThumbSticks.Left.X >= 0)
                                moveFrame = 0;

                            if (facingRight)
                            {
                                facingRight = false;
                                playerState = PlayerState.standing;
                            }

                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;
                            }

                            if (moveFrame > 9)
                            {
                                moveFrame = 0;
                            }
                        }
                        #endregion

                        #region RUN RIGHT
                        else if (current.IsKeyDown(Keys.Right) || MyGamePad.RightAnalogHeld())
                        {

                            position.X += pushBlockSpeed;

                            if (!(last.IsKeyDown(Keys.Right)) && MyGamePad.previousState.ThumbSticks.Left.X <= 0)
                                moveFrame = 0;

                            if (facingRight == false)
                            {
                                facingRight = true;
                                playerState = PlayerState.standing;
                            }
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;
                            }

                            if (moveFrame > 9)
                            {
                                moveFrame = 0;
                            }
                        }
                        #endregion

                        #region STAND
                        if (current.IsKeyUp(Keys.Right) && current.IsKeyUp(Keys.Left) && current.IsKeyUp(Keys.Up) && MyGamePad.previousState.ThumbSticks.Left.X == 0 && MyGamePad.previousState.ThumbSticks.Left.Y >= 0)
                        {
                            playerState = PlayerState.standing;
                        }
                        #endregion


                        if (canJump)
                        {
                            #region JUMP
                            if (knockedBack == false && (current.IsKeyDown(Keys.Up) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed) && falling == false && currentPlat != null)
                            {

                                velocity.Y += jumpHeight;

                                playerState = PlayerState.jumping;

                                moveFrame = 0;

                                Chapter.effectsManager.AddJumpDustPoof(rec, facingRight);
                            }
                            #endregion
                        }
                        break;
                    #endregion

                    #region RUNNING
                    case PlayerState.running:
                        if ((moveFrame == 2 || moveFrame == 9) && frameDelay == 3)
                        {
                            int ran = selectFlinch.Next(3);
                            if(ran == 0)
                                Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom1);
                            else if(ran == 1)
                                Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom2);
                            else
                                Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom3);
                        }

                        if ((moveFrame == 5 || moveFrame == 12) && frameDelay == 5 && !sprinting)
                            Chapter.effectsManager.AddRunningDustPoof(rec, moveFrame, facingRight, false);
                        else if (moveFrame == 3 && sprinting && frameDelay == 4)
                            Chapter.effectsManager.AddRunningDustPoof(rec, moveFrame, facingRight, true);

                        if (current.IsKeyDown(Keys.LeftShift) || MyGamePad.currentState.Triggers.Left > 0)
                        {
                            if (!sprinting)
                            {
                                sprinting = true;
                                moveSpeed = 10;
                                moveFrame = 0;
                            }
                        }
                        else
                        {
                            if (sprinting)
                            {
                                moveSpeed = 8;
                                sprinting = false;
                                moveFrame = 0;
                            }
                        }

                        //--Avoid running in place
                        if ((current.IsKeyDown(Keys.Left) && current.IsKeyDown(Keys.Right)) || (MyGamePad.RightAnalogHeld() && current.IsKeyDown(Keys.Left)) || (MyGamePad.LeftAnalogHeld() && current.IsKeyDown(Keys.Right)))
                        {
                            frameDelay = 8;
                            moveFrame = 0;
                            playerState = PlayerState.standing;
                        }

                        #region RUN LEFT
                        else if ((current.IsKeyDown(Keys.Left) || MyGamePad.LeftAnalogHeld()) && !knockedBack)
                        {
                            position.X -= moveSpeed;

                            if (!(last.IsKeyDown(Keys.Left)) && MyGamePad.previousState.ThumbSticks.Left.X >= 0)
                                moveFrame = 0;

                            facingRight = false;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;

                                if (sprinting)
                                    frameDelay = 4;
                            }

                            if ((moveFrame > 13 && !sprinting) || (moveFrame > 11 && sprinting))
                            {
                                moveFrame = 0;
                            }
                        }
                        #endregion

                        #region RUN RIGHT
                        else if ((current.IsKeyDown(Keys.Right) || MyGamePad.RightAnalogHeld()) && !knockedBack)
                        {

                            position.X += moveSpeed;

                            if (!(last.IsKeyDown(Keys.Right)) && MyGamePad.previousState.ThumbSticks.Left.X <= 0)
                                moveFrame = 0;

                            facingRight = true;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;

                                if (sprinting)
                                    frameDelay = 4;
                            }

                            if ((moveFrame > 13 && !sprinting) || (moveFrame > 11 && sprinting))
                            {
                                moveFrame = 0;
                            }
                        }
                        #endregion

                        #region STAND
                        if (current.IsKeyUp(Keys.Right) && current.IsKeyUp(Keys.Left) && current.IsKeyUp(Keys.Up) && MyGamePad.previousState.ThumbSticks.Left.X == 0 && MyGamePad.previousState.ThumbSticks.Left.Y >= 0)
                        {
                            frameDelay = 8;
                            moveFrame = 0;
                            playerState = PlayerState.standing;
                        }
                        #endregion

                        //DUCK
                        if (current.IsKeyDown(Keys.Down) || MyGamePad.DownAnalogHeld())
                        {
                            ducking = true;
                            moveFrame = 0;
                            playerState = PlayerState.standing;
                            frameDelay = 2;
                        }

                        if (canJump)
                        {
                            #region JUMP
                            if (knockedBack == false && (current.IsKeyDown(Keys.Up) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed) && falling == false && currentPlat != null)
                            {

                                if (sprinting)
                                {
                                    sprinting = false;
                                    sprintJump = true;
                                    airMoveSpeed = 12;
                                    VelocityY += (int)(jumpHeight * .85);
                                    frameDelay = 3;
                                }
                                else
                                {
                                    velocity.Y += jumpHeight;
                                }

                                playerState = PlayerState.jumping;
                                Chapter.effectsManager.AddJumpDustPoof(rec, facingRight);

                                if (selectFlinch.Next(2) == 0)
                                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerJump1);
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerJump2);
                                moveFrame = 0;
                            }
                            #endregion
                        }

                        //--Check to see if the player is trying to use a skill
                        CheckSkillPress();

                        break;
                    #endregion

                    #region JUMPIN
                    case PlayerState.jumping:
                        sprinting = false;
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            if (moveFrame < 3)
                                frameDelay = 3;

                            frameDelay = 1;
                        }

                        if (moveFrame > 18)
                        {
                            moveFrame = 3;
                        }

                        #region MOVING IN AIR
                        if (current.IsKeyDown(Keys.Right) || MyGamePad.RightAnalogHeld())
                        {
                            facingRight = true;
                            position.X += airMoveSpeed;
                        }
                        if (current.IsKeyDown(Keys.Left) || MyGamePad.LeftAnalogHeld())
                        {
                            facingRight = false;
                            position.X -= airMoveSpeed;
                        }


                        #endregion

                        //GrabLedge();

                        CheckSkillPress();
                        break;
                    #endregion

                    #region GRAB LEDGE - NOT USED
                    case PlayerState.grabbingLedge:
                        if (!climbingUp)
                        {
                            timeHanging++;
                            if (facingRight)
                            {
                                PositionX = currentPlat.Rec.X - 300;
                                PositionY = currentPlat.Rec.Y - 100;

                                if (timeHanging > 2)
                                {
                                    if (current.IsKeyUp(Keys.Down) && last.IsKeyDown(Keys.Down))
                                    {
                                        playerState = PlayerState.jumping;
                                        falling = true;
                                    }

                                    if (current.IsKeyDown(Keys.Left) && current.IsKeyDown(Keys.Up))
                                    {
                                        velocity.Y += jumpHeight;
                                        playerState = PlayerState.jumping;
                                    }

                                    else if (current.IsKeyDown(Keys.Up) && current.IsKeyDown(Keys.Right))
                                    {
                                        climbingUp = true;
                                    }
                                }
                            }
                            else
                            {
                                PositionX = currentPlat.Rec.X + currentPlat.Rec.Width - 260;
                                PositionY = currentPlat.Rec.Y - 100;

                                if (timeHanging > 10)
                                {
                                    if (current.IsKeyUp(Keys.Down) && last.IsKeyDown(Keys.Down))
                                    {
                                        playerState = PlayerState.jumping;
                                        falling = true;
                                    }

                                    if (current.IsKeyDown(Keys.Right) && current.IsKeyDown(Keys.Up))
                                    {
                                        velocity.Y += jumpHeight;
                                        playerState = PlayerState.jumping;
                                    }

                                    else if (current.IsKeyDown(Keys.Up) && current.IsKeyDown(Keys.Left))
                                    {
                                        climbingUp = true;
                                    }
                                }
                            }
                        }
                        else
                        {

                            Rectangle feet = new Rectangle((int)vitalRec.X - 10, (int)vitalRec.Y + vitalRec.Height + 20, vitalRec.Width + 20, 20);
                            Rectangle top = new Rectangle(currentPlat.Rec.X - 10, currentPlat.Rec.Y, currentPlat.Rec.Width + 20, 20);

                            if (!feet.Intersects(top))
                            {
                                PositionY -= 10;
                            }
                            else
                            {
                                if (facingRight)
                                    PositionX += 10;
                                else
                                {
                                    PositionX -= 25;
                                    PositionY -= 25;
                                }

                                climbingUp = false;
                                timeHanging = 0;
                                playerState = PlayerState.standing;
                            }
                        }
                        break;
                    #endregion

                    #region DYING
                    case PlayerState.dead:
                        frameDelay--;

                        if (frameDelay == 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame == 5)
                                moveFrame = 0;
                        }

                        PositionY -= 2;
                        break;
                    #endregion
                }
            }
        }

        //--Draw the player and skill animations
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            //--If the player is invincible or not
            if (invincibleTime % 5 == 0 && invincible == true)
                alpha = .5f;
            else if(!isStunned)
                alpha = 1f;

            //s.Draw(Game1.whitefilter, vitalRec, Color.Black * .5f);
            //s.Draw(Game1.whiteFilter, jumpingVitalRec, Color.Red * .5f);
           // s.Draw(Game1.whiteFilter, duckingVitalRec, Color.Purple * .4f);

            #region FACING DIRECTION
            if (playerState != PlayerState.attacking && playerState != PlayerState.attackJumping && !levelingUp)
            {
                if (facingRight)
                {
                    if (isStunned || knockedBack || standingBackUp || (playerState != PlayerState.standing && (!sprinting || sprintJump)) || game.CurrentChapter.TalkingToNPC || (playerState == PlayerState.standing && landing || ducking) || playerState == PlayerState.dead)
                    {
                        s.Draw(playerSheet, rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                    }
                    else if (sprinting)
                    {
                        s.Draw(Game1.danceSprite, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        SpriteEffects flip;

                        if (!dancing)
                            flip = SpriteEffects.None;
                        else
                            flip = SpriteEffects.FlipHorizontally;

                        s.Draw(Game1.danceSprite, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, flip, 0f);
                    }
                    //Draw the air lines when you stand back up, only for the first two frames
                    if (drawStandingBackUpLines)
                    {
                        if (frameDelay == 5)
                            s.Draw(playerSheet, rec, new Rectangle(2120, 0, 530, 398), Color.White);
                        else if (frameDelay == 4)
                            s.Draw(playerSheet, rec, new Rectangle(2680, 0, 530, 398), Color.White);
                    }
                }

                else if (!facingRight)
                {
                    if (isStunned || knockedBack || standingBackUp || (playerState != PlayerState.standing && (!sprinting || sprintJump)) || game.CurrentChapter.TalkingToNPC || (playerState == PlayerState.standing && landing || ducking) || playerState == PlayerState.dead)
                    {
                        s.Draw(playerSheet, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                    else if (sprinting)
                    {
                        s.Draw(Game1.danceSprite, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                    else
                    {
                        SpriteEffects flip;

                        if (!dancing)
                            flip = SpriteEffects.FlipHorizontally;
                        else
                            flip = SpriteEffects.None;

                        s.Draw(Game1.danceSprite, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, flip, 0f);
                    }
                    //Draw the air lines when you stand back up, only for the first two frames
                    if (drawStandingBackUpLines)
                    {
                        if (frameDelay == 5)
                            s.Draw(playerSheet, rec, new Rectangle(2120, 0, 530, 398), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                        else if (frameDelay == 4)
                            s.Draw(playerSheet, rec, new Rectangle(2680, 0, 530, 398), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                }

                //Draw the stars above his head when he's stunned
                if (isStunned)
                {
                    if (facingRight)
                        s.Draw(playerSheet, rec, new Rectangle(530 + (530 * starFrame), 3610, 530, 398), Color.White * alpha);
                    else
                        s.Draw(playerSheet, rec, new Rectangle(530 + (530 * starFrame), 3610, 530, 398), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }

                if (moneyJustPickedUpTimer > 0 && game.CurrentChapter.state == Chapter.GameState.Game)
                {

                    s.DrawString(Game1.moneyFont, "+$" + moneyJustPickedUp.ToString("N2"), new Vector2(rec.X + rec.Width / 2 - Game1.pickUpFont.MeasureString("+$" + moneyJustPickedUp.ToString("N2")).X / 2 - 35, rec.Y + 60), Color.White);
                }
            }
            #endregion

            Chapter.effectsManager.DrawDustPoofsWhileRunning(s);

            //--Always draw the skills. In the skill Draw() method it only draws the player's animation if the skill is being used,
            //--but you have to always call the draw method in case there are left over parts of the skill after the player animation ends,
            //--such as lighting strikes or explosions
            #region Draw Skills
            if (game.CurrentChapter.state == Chapter.GameState.Game && !levelingUp && playerState != PlayerState.dead)
            {
                for (int i = 0; i < equippedSkills.Count; i++)
                {
                    equippedSkills[i].Draw(s);
                }
            }
            #endregion

            if (game.CurrentChapter.state == Chapter.GameState.Game)
                Chapter.effectsManager.DrawPoofs(s);

            //Update passive skills
            for (int i = 0; i < ownedPassives.Count; i++)
            {
                ownedPassives[i].Draw(s);
            }
        }

        public void DrawDamage(SpriteBatch s)
        {
            #region Draw Damage
            for (int i = 0; i < damageVecs.Count; i++)
            {
                damageTimers[i]++;

                if (damageTimers[i] > 30)
                    damageAlphas[i] -= .02f;

                if(game.CurrentChapter.state == Chapter.GameState.Game)
                    s.DrawString(Game1.playerFont, "-" + damageNums[i].ToString() + "$", new Vector2(rec.X + rec.Width / 2 - Game1.playerFont.MeasureString("-" + damageNums[i].ToString() + "$").X / 2, rec.Y + 70 + damageVecs[i].Y), Color.White * damageAlphas[i]);

                damageVecs[i] += new Vector2(0, -.5f);

                if (damageTimers[i] > 80)
                {
                    damageAlphas.RemoveAt(i);
                    damageVecs.RemoveAt(i);
                    damageNums.RemoveAt(i);
                    damageTimers.RemoveAt(i);
                    i--;
                }
            }
            #endregion
        }

        public void AddDamageNum(int damage)
        {

            damageVecs.Add(new Vector2(0,0));
            damageNums.Add(damage);
            damageTimers.Add(0);
            damageAlphas.Add(1f);
        }

        //Player death
        public void CheckIsDead()
        {
            if (health <= 0)
            {
                StopSkills();
                velocity = Vector2.Zero;
                knockedBack = false;
                moveFrame = 0;
                frameDelay = 5;
                Chapter.effectsManager.RemoveToolTip();
                dead = true;
                playerState = PlayerState.dead;
                Chapter.effectsManager.AddSmokePoof(new Rectangle(VitalRecX - 50, VitalRecY, vitalRec.Width + 100, vitalRec.Height), 2);

                UpdatePosition();
            }
        }

        //For when you die or go to the main menu
        public void ResetPlayer()
        {
            //--New lists and stuff
            selectFlinch = new Random();
            rec = new Rectangle(3000, 200, 530, 398);
            vitalRec = new Rectangle(60, 100, 60, 170);
            jumpingVitalRec = new Rectangle(60, 100, 60, 110);
            duckingVitalRec = new Rectangle(60, 100, 60, 30);
            equippedSkills = new List<Skill>();
            learnedSkills = new List<Skill>();
            enemies = new List<Enemy>();
            position = new Vector2(1050, 290); //1350/1050, 290 for DEMO 
            ownedWeapons = new List<Weapon>();
            ownedHats = new List<Hat>();
            ownedHoodies = new List<Hoodie>();
            ownedAccessories = new List<Accessory>();
            allCharacterBios = new Dictionary<string, bool>();
            allMonsterBios = new Dictionary<string, bool>();
            SetCharacterBioDictionary();
            SetMonsterBioDictionary();
            craftingAndCatalysts = new Dictionary<string, int>();
            ownedPassives = new List<Passive>();
            this.font = font;
            levelUpTextTimer = 0;
            yScroll = false;
            enemyDrops = new Dictionary<string, int>();
            pickUps = new List<string>();
            pickUpTimers = new List<int>();
            storyItems = new Dictionary<string, int>();
            mouseRec = new Rectangle(0, 0, Cursor.cursorWidth / 2, Cursor.cursorHeight / 2);
            canJump = true;

            //--Base stats
            socialRank = "New Kid";
            socialRankIndex = 0;
            karma = 0;
            maxHealth = 20;
            health = maxHealth;
            strength = 18;
            defense = 1;
            moveSpeed = 7;
            airMoveSpeed = 7;
            jumpHeight = -21;
            level = 1;
            experience = 0;
            experienceUntilLevel = 4;

            //--Status
            playerState = PlayerState.standing;
            knockedBack = false;
            facingRight = true;
            invincible = false;

            platGrabRec = new Rectangle(0, 0, 50, 125);

            landing = false;
            attackFalling = false;

            attackFloating = false;
            dead = false;

            cutsceneMoving = false;
            canJump = true;
            jumpingDown = false;
            sprinting = false;
            sprintJump = false;
            levelingUp = false;
            danceAnimationForward = false;
            cantPickUp = false;

            moneyJustPickedUpTimer = 0;

            equippedWeapon = null;
            secondWeapon = null;
            equippedHat = null;
            equippedHoodie = null;
            equippedAccessory = null;
            secondAccessory = null;

            ducking = false;
            standingBackUp = false;
            drawStandingBackUpLines = false;
            isStunned = false;
            stunTime = 0;
            money = 0f;
            textbooks = 0;
        }

        //--Jump down a platform if it is passable
        public void JumpDown()
        {
            if (currentPlat != null && currentPlat.Passable)
            {
                //--Hold space and press down
                if ((last.IsKeyDown(Keys.LeftShift) || MyGamePad.currentState.Triggers.Left > 0) && (last.IsKeyDown(Keys.Down) && current.IsKeyUp(Keys.Down) || MyGamePad.currentState.ThumbSticks.Left.Y == -1))
                {
                    //--You jump down for 3 frames and move downward
                    //--Essentially, gravity is only used if you are in mid-air, so you have to remove the currentPlat attribute
                    //--Which is fine because you no longer want to stand on it
                    //--Set the velocity a bit hit to start so gravity doesn't have to do all the work, and move the player down a few
                    //--Pixels to simulate him jumping down
                    jumpingDown = true;
                    jumpDowntimer = 3;
                    currentPlat = null;
                    PositionY += 20;
                    velocity.Y += 10;
                }
            }

            //--Once you are jumping, update the timer and reset the value after 3 frames
            if (jumpingDown)
            {
                jumpDowntimer--;

                if (jumpDowntimer <= 0)
                    jumpingDown = false;
            }
        }

        //Used for bosses/enemies/whatever to make the player fall off a platform
        public void KnockPlayerDown()
        {
            if (currentPlat != null && currentPlat.Passable)
            {

                jumpingDown = true;
                jumpDowntimer = 3;
                currentPlat = null;
                PositionY += 20;
                velocity.Y += 10;
            }

            //--Once you are jumping, update the timer and reset the value after 3 frames
            if (jumpingDown)
            {
                jumpDowntimer--;

                if (jumpDowntimer <= 0)
                    jumpingDown = false;
            }
        }

        //--Method to start climbing a ladder, and the physics for climbing it
        public void ClimbLadder()
        {
            //--Check to see if the player is touching any ladders while not attacking and not climbing a ladder
            for (int i = 0; i < game.CurrentChapter.CurrentMap.Ladders.Count; i++)
            {
                if ((playerState == PlayerState.jumping || playerState == PlayerState.standing || playerState == PlayerState.running)
                    && playerState != PlayerState.climbingLadder && vitalRec.Intersects(game.CurrentChapter.CurrentMap.Ladders[i].MiddleOfLadder))
                {
                    //--If the player is not touching the top of the ladder and presses up, start climbing and move the player up
                    if (current.IsKeyDown(Keys.Up) && !vitalRec.Intersects(game.CurrentChapter.CurrentMap.Ladders[i].TopOfLadder))
                    {
                        PositionY -= 10;

                        currentLadder = game.CurrentChapter.CurrentMap.Ladders[i];
                        playerState = PlayerState.climbingLadder;
                        velocity.Y = 0;
                        //PositionX = currentLadder.Rec.X - 225;
                    }
                    //--If at the top, you must press down to begin climbing
                    else if (current.IsKeyDown(Keys.Down) && vitalRec.Intersects(game.CurrentChapter.CurrentMap.Ladders[i].TopOfLadder))
                    {
                        PositionY += 10;

                        currentLadder = game.CurrentChapter.CurrentMap.Ladders[i];
                        playerState = PlayerState.climbingLadder;
                        velocity.Y = 0;
                        //PositionX = currentLadder.Rec.X - 225;
                    }
                }
            }

            //--Once climbing
            if (playerState == PlayerState.climbingLadder)
            {
                //--If you aren't touching the middle of the ladder, fall off
                if (!vitalRec.Intersects(currentLadder.MiddleOfLadder))
                {
                    PositionY -= 5;
                    playerState = PlayerState.jumping;
                    currentLadder = null;
                }

                //--Move along the ladder
                if (current.IsKeyDown(Keys.Up))
                    PositionY -= 5;
                else if (current.IsKeyDown(Keys.Down))
                    PositionY += 5;
                if (current.IsKeyDown(Keys.Right))
                {
                    PositionX += 3;
                }
                else if (current.IsKeyDown(Keys.Left))
                {
                    PositionX -= 3;
                }
            }
        }

        //--Calls a skill to be used
        public void Attack()
        {
            if (playerState == PlayerState.attacking || playerState == PlayerState.attackJumping)
            {
                //--The skill called is based on which key was pressed
                if (skillUsed == 0)
                {
                    //Use next part of combo if it is a combo skill
                    if ((last.IsKeyDown(Keys.Q) && current.IsKeyUp(Keys.Q)) || MyGamePad.RightTriggerPressed())
                    {
                        if (equippedSkills[0].UseNext[0] == true)
                            equippedSkills[0].UseNext[1] = true;
                        else
                            equippedSkills[0].UseNext[0] = true;
                    }
                    equippedSkills[0].Use(game, Keys.Q);

                    if (equippedSkills[0].AnimationLength <= 0)
                    {
                        if (playerState == PlayerState.attackJumping)
                            playerState = PlayerState.jumping;

                        else
                            playerState = PlayerState.standing;
                    }
                }
                if (skillUsed == 1)
                {
                    if ((last.IsKeyDown(Keys.W) && current.IsKeyUp(Keys.W)) || MyGamePad.XPressed())
                    {
                        if (equippedSkills[1].UseNext[0] == true)
                            equippedSkills[1].UseNext[1] = true;
                        else
                            equippedSkills[1].UseNext[0] = true;
                    }

                    equippedSkills[1].Use(game, Keys.W);

                    if (equippedSkills[1].AnimationLength <= 0)
                    {
                        if (playerState == PlayerState.attackJumping)
                            playerState = PlayerState.jumping;
                        else
                            playerState = PlayerState.standing;
                    }
                }
                if (skillUsed == 2)
                {
                    if ((last.IsKeyDown(Keys.E) && current.IsKeyUp(Keys.E)) || MyGamePad.YPressed())
                    {
                        if (equippedSkills[2].UseNext[0] == true)
                            equippedSkills[2].UseNext[1] = true;
                        else
                            equippedSkills[2].UseNext[0] = true;
                    }

                    equippedSkills[2].Use(game, Keys.E);

                    if (equippedSkills[2].AnimationLength <= 0)
                    {
                        if (playerState == PlayerState.attackJumping)
                            playerState = PlayerState.jumping;
                        else
                            playerState = PlayerState.standing;
                    }
                }
                if (skillUsed == 3)
                {
                    if ((last.IsKeyDown(Keys.R) && current.IsKeyUp(Keys.R)) || MyGamePad.BPressed())
                    {
                        if (equippedSkills[3].UseNext[0] == true)
                            equippedSkills[3].UseNext[1] = true;
                        else
                            equippedSkills[3].UseNext[0] = true;
                    }

                    equippedSkills[3].Use(game, Keys.R);

                    if (equippedSkills[3].AnimationLength <= 0)
                    {
                        if (playerState == PlayerState.attackJumping)
                            playerState = PlayerState.jumping;
                        else
                            playerState = PlayerState.standing;
                    }
                }
            }
        }

        public void CutsceneStand()
        {
            playerState = PlayerState.standing;
            landing = false;
            ducking = false;
            drawStandingBackUpLines = false;

            if (dancing)
            {
                frameDelay--;
                if (frameDelay == 0)
                {
                    if (danceAnimationForward)
                    {
                        moveFrame++;

                        if ((danceState == 2 || danceState == 3) && moveFrame == 3)
                            danceAnimationForward = false;
                    }
                    else
                    {
                        moveFrame--;
                        if ((danceState == 5) && moveFrame == 4)
                        {
                            danceAnimationForward = true;
                            danceState++;
                        }
                    }

                    if (danceState > 6)
                        danceState = 0;

                    frameDelay = 5;
                }

                if (moveFrame > 7)
                {
                    moveFrame = 7;
                    danceAnimationForward = !danceAnimationForward;
                }
                if (moveFrame < 0)
                {
                    danceState++;
                    moveFrame = 0;
                    danceAnimationForward = !danceAnimationForward;
                }
            }
            else
            {
                frameDelay-=2;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 9;

                    if (moveFrame > 7)
                        moveFrame = 0;
                }
            }
        }

        //--Makes the player fall back to the ground if he jumps or is in the air
        public void ImplementGravity()
        {
            //--Update forces

            if ((currentPlat == null || currentPlat.Exists == false) && playerState != PlayerState.climbingLadder && playerState != PlayerState.grabbingLedge && !attackFloating)
                velocity.Y += GameConstants.GRAVITY;

            position.X += velocity.X;
            position.Y += velocity.Y;

            if (velocity.Y > terminalVelocity)
            {
                attackFalling = true;
                velocity.Y = terminalVelocity;
            }

            //Rectangles that represent the sides of the character
            Rectangle feet = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + vitalRec.Height + 20, vitalRec.Width + 30, 20);

            Rectangle rightPlay = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle leftPlay = new Rectangle((int)vitalRec.X - 25, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle topPlay = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle checkForStairs = new Rectangle(feet.X, feet.Y, feet.Width, 150);

            platBelowPlayerForStairs = false;

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {

                //Represents the platform and the sides of it
                Platform plat = currentMap.Platforms[i];

                if (plat.Exists == false)
                    continue;

                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 20);
                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 20, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                if (checkForStairs.Intersects(top) && playerState != PlayerState.jumping && playerState != PlayerState.attackJumping)
                {
                    platBelowPlayerForStairs = true;
                }

                #region NON PASSABLE PLATFORMS
                //--If you run into the side of a nonpassable wall
                //--Depending on what side you're on, and if you're jumping or not, move the player back a frame
                if (playerState != PlayerState.pushingBlock)
                {

                        if ((rightPlay.Intersects(left) || leftPlay.Intersects(right)) && plat.Passable == false)
                        {
                            if (rightPlay.Intersects(left))
                            {
                                if (playerState != PlayerState.jumping)
                                {
                                    position.X -= moveSpeed;
                                }
                                else
                                {
                                    position.X -= airMoveSpeed;
                                }
                                velocity.X = 0;
                            }

                            if (leftPlay.Intersects(right))
                            {
                                if (playerState != PlayerState.jumping)
                                {
                                    position.X += moveSpeed;
                                }
                                else
                                {
                                    position.X += airMoveSpeed;
                                }
                                velocity.X = 0;
                            }
                        }
                    
                    if(knockedBack)
                    {
                        Rectangle checkPlatRec;

                        if (VelocityX >= 0)
                        {
                            checkPlatRec = new Rectangle(rightPlay.X, rightPlay.Y, (int)velocity.X, rightPlay.Height);

                            if (checkPlatRec.Intersects(left))
                            {
                                //playerState = PlayerState.standing;
                                Console.WriteLine("hit left");
                                PositionX -= VelocityX;
                                knockedBack = false;
                                VelocityX = 0;
                               // playerState = PlayerState.standing;
                            }
                        }
                        else
                        {
                            checkPlatRec = new Rectangle(leftPlay.X - Math.Abs((int)VelocityX), leftPlay.Y, Math.Abs((int)velocity.X), leftPlay.Height);

                            if (checkPlatRec.Intersects(right))
                            {
                               // playerState = PlayerState.standing;
                                Console.WriteLine("hit right");
                                PositionX += Math.Abs(VelocityX);
                                knockedBack = false;
                                VelocityX = 0;
                                //playerState = PlayerState.standing;
                            }
                        }
                    }
                }

                //--If you jump up into a nonpassable wall, push him back down
                if (topPlay.Intersects(bottom) && velocity.Y < 0 && plat.Passable == false)
                {
                    velocity.Y = 0;
                    velocity.Y = GameConstants.GRAVITY;
                    playerState = PlayerState.jumping;
                }
                #endregion

                #region PASSABLE PLATFORMS
                //--If you land on top of a passable platform, stay on it
                if (feet.Intersects(top) && velocity.Y >= 0 && playerState != PlayerState.grabbingLedge)
                {

                    if (playerState != PlayerState.climbingLadder && !jumpingDown)
                    {
                        //170 is the height of the vital rec, but it changes during the jump, so just hardcode the number
                        //135 is the magic number of setting the player above platforms
                        //37 is the difference between the placement of daryl on the earlier spritesheets and this one
                        position.Y = plat.Rec.Y - 170 - 135 - 37;
                        velocity.Y = 0;

                        if (VelocityX == 0)
                            knockedBack = false;

                        falling = false;
                    }

                    currentPlat = plat;

                    //--Go back to standing if you were jumping
                    if (playerState == PlayerState.jumping)
                    {
                        playerState = PlayerState.standing;

                        if (!isStunned)
                        {
                            Chapter.effectsManager.AddJumpDustPoof(rec, facingRight);
                            landing = true;
                            frameDelay = 8;
                            moveFrame = 0;
                        }
                        Sound.PlaySoundInstance(Sound.SoundNames.PlayerLanding);
                        attackFalling = false;
                    }

                    if(playerState == PlayerState.climbingLadder && !feet.Intersects(currentLadder.TopOfLadder))
                        playerState = PlayerState.standing;
                }
                #endregion
            }

            //--If you are falling
            if (velocity.Y >= GameConstants.GRAVITY * 2)
            {
                if (playerState == PlayerState.attackJumping)
                { }
                else
                {
                    //--Don't do this if the player is on a downward moving platform
                    //--NOTE: IF AN ISSUE ARISES WITH PLATFORMS/FALLING/JUMPING/WHEN DARYL IS STANDING, CHANGE IT TO 'GRAVITY * 10' INSTEAD OF 'GRAVITY * 5'
                    if ((currentPlat != null && !(currentPlat.Velocity.Y > 0)) || (currentPlat == null && VelocityY >= GameConstants.GRAVITY * 5 && playerState != PlayerState.grabbingLedge) && platBelowPlayerForStairs == false && !isStunned)
                    {
                        if (playerState != PlayerState.jumping)
                            moveFrame = 3;

                        playerState = PlayerState.jumping;
                        falling = true;
                    }
                }
            }

            //--Jumping off of a moving platform causes the player to maintain the platform's velocity for a short period of time
            //--This can be changed in the movement method by setting velocity.x to 0 when the player jumps
            #region Moving Platform Physics
            //--If the player jumps or falls off the platform, set the current plat to null
            if (playerState == PlayerState.jumping || playerState == PlayerState.attackJumping || falling ||
                (currentPlat != null && !feet.Intersects(currentPlat.Rec)))
            {
                if(playerState != PlayerState.grabbingLedge)
                     currentPlat = null;
            }
            //--If there is a current platform, but the velocity is 0 and the player isn't being knockbacked, set the velocity of the
            //--player to 0
            if (currentPlat != null && currentPlat.Velocity.X == 0 && !KnockedBack)
                velocity.X = 0;

            //--If the player isn't knockbacked, slow him down every frame 
            
            if (!knockedBack)
            {
                #region TO THE RIGHT
                //--If the player is thrown to the right
                //--Decrease the velocity by 2 every second, and when it reaches 0 set knockback to false
                if (velocity.X > 0)
                {
                    velocity.X -= .2f;

                    if (velocity.X <= 0)
                    {
                        //knockedBack = false;
                        velocity.X = 0;
                    }
                }
                #endregion

                #region TO THE LEFT
                //--If the player is thrown to the left
                //--Increase velocity by 2, then when it hits 0 set to false
                if (velocity.X < 0)
                {
                    velocity.X += .2f;

                    if (velocity.X >= 0)
                    {
                        // knockedBack = false;
                        velocity.X = 0;
                    }
                }
                #endregion
            }
            #endregion
        }

        //--Call the Update method of all skills currently equipped
        public void UpdateSkills()
        {

            for (int i = 0; i < equippedSkills.Count; i++)
            {
                equippedSkills[i].Update();
            }
        }

        /// <summary>
        /// Used during cutscenes so the player's skills don't continue after the scene ends
        /// </summary>
        public void StopSkills()
        {
            for (int i = 0; i < equippedSkills.Count; i++)
            {
                equippedSkills[i].AnimationLength = -1;
            }
        }

        //--Checks to see which key was pressed: Q, W, E, or R
        public void CheckSkillPress()
        {

            #region Non-hold skills
            //--If Q was pressed, use the skill that is first in the list
            //--Only use it if the cooldown is up, which means canUse is true
            //--Only use if there is a skill in that slot
            //The velocity check makes sure the player just use the skill as soon as he jumps, which could cause errors
            if ((last.IsKeyDown(Keys.Q) && current.IsKeyUp(Keys.Q)) || MyGamePad.RightTriggerPressed())
            {
                if (equippedSkills.Count > 0 && equippedSkills[0].canUse == true && equippedSkills[0].HoldToUse == false)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[0].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 0;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 0;
                    }
                }
            }


            if ((last.IsKeyDown(Keys.W) && current.IsKeyUp(Keys.W)) || MyGamePad.XPressed())
            {
                if (equippedSkills.Count > 1 && equippedSkills[1].canUse == true && equippedSkills[1].HoldToUse == false)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[1].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 1;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 1;
                    }
                }
            }


            if ((last.IsKeyDown(Keys.E) && current.IsKeyUp(Keys.E)) || MyGamePad.YPressed())
            {
                if (equippedSkills.Count > 2 && equippedSkills[2].canUse == true && equippedSkills[2].HoldToUse == false)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[2].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 2;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 2;
                    }
                }
            }


            if ((last.IsKeyDown(Keys.R) && current.IsKeyUp(Keys.R)) || MyGamePad.BPressed())
            {
                if (equippedSkills.Count > 3 && equippedSkills[3].canUse == true && equippedSkills[3].HoldToUse == false)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[3].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 3;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 3;
                    }
                }
            }
            #endregion

            #region Hold skills
            //--If Q was pressed, use the skill that is first in the list
            //--Only use it if the cooldown is up, which means canUse is true
            //--Only use if there is a skill in that slot
            //The velocity check makes sure the player just use the skill as soon as he jumps, which could cause errors
            if (last.IsKeyDown(Keys.Q) || MyGamePad.currentState.Triggers.Right > 0)
            {
                if (equippedSkills.Count > 0 && equippedSkills[0].canUse == true && equippedSkills[0].HoldToUse == true)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[0].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 0;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 0;
                    }


                }
            }


            if (last.IsKeyDown(Keys.W) || MyGamePad.currentState.Buttons.X == ButtonState.Pressed)
            {
                if (equippedSkills.Count > 1 && equippedSkills[1].canUse == true && equippedSkills[1].HoldToUse == true)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[1].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 1;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 1;
                    }
                }
            }


            if (last.IsKeyDown(Keys.E) || MyGamePad.currentState.Buttons.Y == ButtonState.Pressed)
            {
                if (equippedSkills.Count > 2 && equippedSkills[2].canUse == true && equippedSkills[2].HoldToUse == true)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[2].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 2;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 2;
                    }
                }
            }


            if (last.IsKeyDown(Keys.R) || MyGamePad.currentState.Buttons.B == ButtonState.Pressed)
            {
                if (equippedSkills.Count > 3 && equippedSkills[3].canUse == true && equippedSkills[3].HoldToUse == true)
                {
                    if (playerState == PlayerState.jumping)
                    {
                        if (equippedSkills[3].CanUseInAir)
                        {
                            attackFalling = true;
                            playerState = PlayerState.attackJumping;
                            skillUsed = 3;
                        }
                    }
                    else
                    {
                        playerState = PlayerState.attacking;
                        skillUsed = 3;
                    }
                }
            }
            #endregion

        }

        //--Call this method to knock the player back a set amount
        public void KnockPlayerBack(Vector2 kbvel)
        {

            if (invincible == false)
            {
                flinchType = selectFlinch.Next(2);
                Invincible(45);
                knockedBack = true;

                velocity = kbvel;
            }
        }

        //--This updates the knockback of the player
        public void UpdateKnockBack()
        {

            //Rectangles that represent the sides of the character
            Rectangle feet = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + vitalRec.Height + 20, vitalRec.Width + 30, 20);

            Rectangle rightPlay = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle leftPlay = new Rectangle((int)vitalRec.X - 25, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle topPlay = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle checkForStairs = new Rectangle(feet.X, feet.Y, feet.Width, 150);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {

                //Represents the platform and the sides of it
                Platform plat = currentMap.Platforms[i];

                if (plat.Exists == false)
                    continue;

                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 20);
                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 20, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);


                #region NON PASSABLE PLATFORMS
                //--If you run into the side of a nonpassable wall
                //--Depending on what side you're on, and if you're jumping or not, move the player back a frame
                if (playerState != PlayerState.pushingBlock)
                {



                }
                #endregion

            }
            if (knockedBack == true)
            {
                #region TO THE RIGHT
                //--If the player is thrown to the right
                //--Decrease the velocity by 2 every second, and when it reaches 0 set knockback to false
                if (velocity.X > 0)
                {
                    velocity.X -= 2;

                    if (velocity.X <= 0)
                    {
                       // knockedBack = false;
                        velocity.X = 0;
                    }
                }
                #endregion

                #region TO THE LEFT
                //--If the player is thrown to the left
                //--Increase velocity by 2, then when it hits 0 set to false
                if (velocity.X < 0)
                {
                    velocity.X += 2;

                    if (velocity.X >= 0)
                    {
                        //knockedBack = false;
                        velocity.X = 0;
                    }
                }
                #endregion
            }
        }

        //--Causes the player to go invincible for a certain amount of time
        public void Invincible(int time)
        {
            if (invincible == false)
            {
                invincible = true;
                invincibleTime = time;
            }

        }

        //--Updates the invincible property
        public void UpdateInvincible()
        {
            invincibleTime--;

            if (invincibleTime <= 0)
                invincible = false;
        }

        //--Take damage after subtracting tolerance
        public void TakeDamage(int dmg)
        {
            if (invincible == false)
            {
                int newDmg = dmg - defense;

                //--If the player is taking no damage, make him take 1
                if (newDmg <= 0)
                    newDmg = 1;

                Health -= newDmg;

                AddDamageNum(newDmg);
            }
        }

        public void CheckSocialRankUp()
        {
            if (SocialRankManager.allSocialRanks.Count > socialRankIndex && karma >= SocialRankManager.allSocialRanks.ElementAt(socialRankIndex).karmaNeeded)
            {
                socialRank = SocialRankManager.allSocialRanks.ElementAt(socialRankIndex).socialRank;
                LearnPassiveAbility(SocialRankManager.allSocialRanks.ElementAt(socialRankIndex).passiveGrantedThisRank);
                Chapter.effectsManager.notificationQueue.Enqueue(new SocialRankUpNotification(socialRank, SocialRankManager.allSocialRanks.ElementAt(socialRankIndex).passiveGrantedThisRank.Name, socialRankIndex + 1));
                socialRankIndex++;
            }
        }

        public void LearnPassiveAbility(Passive p)
        {

            //Add the passive ability to the player if the equipment has one
            if (!ownedPassives.Contains(p))
            {
                //Make sure to load the passive first
                p.LoadPassive();
                ownedPassives.Add(p);
            }
        }

        public void LevelUp()
        {
            if (experience >= experienceUntilLevel)
            {
                level++;
                levelingUp = true;
                switch (level)
                {
                    case 2:
                        healthAddedDuringLevel = 2;
                        defenseAddedDuringLevel = 1;
                        strengthAddedDuringLevel = 1;
                        experience -= experienceUntilLevel;
                        experienceUntilLevel = 40;
                        break;
                    case 3:
                        healthAddedDuringLevel = 10;
                        strengthAddedDuringLevel = 5;
                        defenseAddedDuringLevel = 7;
                        experience -= experienceUntilLevel;
                        experienceUntilLevel = 250;
                        break;
                    case 4:
                        healthAddedDuringLevel = 6;
                        strengthAddedDuringLevel = 4;
                        defenseAddedDuringLevel = 2;
                        experience -= experienceUntilLevel;
                        experienceUntilLevel = 50000;
                        break;

                    case 5:
                        healthAddedDuringLevel = 1;
                        strengthAddedDuringLevel = 1;
                        defenseAddedDuringLevel = 1;
                        experience -= experienceUntilLevel;
                        experienceUntilLevel = 50000;
                        break;
                    default:
                        experienceUntilLevel = 10000;
                        experience = 0;
                        break;
                }

                maxHealth += healthAddedDuringLevel;
                strength += strengthAddedDuringLevel;
                defense += defenseAddedDuringLevel;
                health = maxHealth;
            }
        }

        public void AddStoryItem(String name, String pickUpName, int num)
        {

            //--If the player doesn't already have a drop of this type
            if (!(storyItems.ContainsKey(name)))
            {
                //--Add it to the dictionary
                storyItems.Add(name, num);
            }
            else
            {
                //--Otherwise, increase how many he has, up to 99
                if (storyItems[name] < 99)
                    storyItems[name]+= num;
            }

            Chapter.effectsManager.AddFoundItem(pickUpName, Game1.storyItemIcons[name]);
        }

        public void AddStoryItemWithoutPopup(String name, int num)
        {

            //--If the player doesn't already have a drop of this type
            if (!(storyItems.ContainsKey(name)))
            {
                //--Add it to the dictionary
                storyItems.Add(name, num);
            }
            else
            {
                //--Otherwise, increase how many he has, up to 99
                if (storyItems[name] < 99)
                    storyItems[name] += num;
            }
        }

        public void PickUpDrops()
        {
            //--If the player presses shift and is touching a drop
            if (current.IsKeyDown(Keys.Space) || MyGamePad.currentState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                pickUpCooldown++;

                if (pickUpCooldown == 5)
                {
                    pickUpCooldown = 0;
                    for (int i = 0; i < currentMap.Drops.Count; i++)
                    {
                        if (vitalRec.Intersects(currentMap.Drops[i].Rec))
                        {
                            EnemyDrop drop = currentMap.Drops[i];

                            //--If it hasn't been picked up yet
                            if (drop.PickedUp == false)
                            {

                                #region Pick up equipment
                                if (drop.Equip != null)
                                {
                                    //--If the equipment is a certain type and the player's inventory is not full
                                    if (drop.Equip is Weapon && ownedWeapons.Count < 75)
                                    {
                                        ownedWeapons.Add(drop.Equip as Weapon);
                                        game.Notebook.Inventory.newWeapon = true;
                                    }
                                    else if (drop.Equip is Weapon && ownedWeapons.Count >= 75)
                                        cantPickUp = true;

                                    if (drop.Equip is Hat && ownedHats.Count < 75)
                                    {
                                        ownedHats.Add(drop.Equip as Hat);
                                        game.Notebook.Inventory.newHat = true;
                                    }
                                    else if (drop.Equip is Hat && ownedHats.Count >= 75)
                                        cantPickUp = true;

                                    if (drop.Equip is Hoodie && OwnedHoodies.Count < 75)
                                    {
                                        ownedHoodies.Add(drop.Equip as Hoodie);
                                        game.Notebook.Inventory.newShirt = true;
                                    }
                                    else if (drop.Equip is Hoodie && ownedHoodies.Count >= 75)
                                        cantPickUp = true;

                                    if (drop.Equip is Accessory && OwnedAccessories.Count < 75)
                                    {
                                        ownedAccessories.Add(drop.Equip as Accessory);
                                        game.Notebook.Inventory.newAccessory = true;
                                    }
                                    else if (drop.Equip is Accessory && ownedAccessories.Count >= 75)
                                        cantPickUp = true;

                                    //--Add an equipment name to the HUD
                                    if (pickUps.Count < 5)
                                    {
                                        if (!cantPickUp)
                                            pickUps.Insert(0, drop.Equip.Name);
                                        else
                                            pickUps.Insert(0, "You don't have room for this item.");
                                        pickUpTimers.Insert(0, 240);
                                    }
                                    else
                                    {
                                        pickUps.RemoveAt(4);
                                        pickUpTimers.RemoveAt(4);
                                        if (!cantPickUp)
                                            pickUps.Insert(0, drop.Equip.Name);
                                        else
                                            pickUps.Insert(0, "You don't have room for this item.");
                                        pickUpTimers.Insert(0, 240);
                                    }
                                }
                                #endregion

                                else if (drop is EnemyBio)
                                {
                                    //Do nothing, the pick-up code is in the EnemyBio class
                                }

                                #region Pick up story item
                                else if (drop.StoryItem != null)
                                {
                                    //--If the player doesn't already have a drop of this type
                                    if (!(storyItems.ContainsKey(drop.StoryItem.Name)))
                                    {
                                        //--Add it to the dictionary
                                        storyItems.Add(drop.StoryItem.Name, 1);
                                    }
                                    else
                                    {
                                        //--Otherwise, increase how many he has, up to 99
                                        if (storyItems[drop.StoryItem.Name] < 99)
                                            storyItems[drop.StoryItem.Name]++;
                                    }

                                    drop.StoryItem.PickedUp = true;
                                    Chapter.effectsManager.AddFoundItem(drop.StoryItem.PickUpName, Game1.storyItemIcons[drop.StoryItem.Name]);
                                }
                                #endregion

                                #region Pick up etc drops
                                else
                                {
                                    if (drop is CraftingItem || drop is Catalyst)
                                    {
                                        //--If the player doesn't already have a drop of this type
                                        if (!(craftingAndCatalysts.ContainsKey(drop.Name)))
                                        {
                                            //--Add it to the dictionary
                                            craftingAndCatalysts.Add(drop.Name, 1);
                                        }
                                        else
                                        {
                                            //--Otherwise, increase how many he has, up to 99
                                            if (craftingAndCatalysts[drop.Name] < 99)
                                                craftingAndCatalysts[drop.Name]++;
                                            else
                                                cantPickUp = true;
                                        }
                                    }
                                    else
                                    {
                                        //--If the player doesn't already have a drop of this type
                                        if (!(enemyDrops.ContainsKey(drop.Name)))
                                        {
                                            //--Add it to the dictionary
                                            enemyDrops.Add(drop.Name, 1);
                                            game.Notebook.Inventory.newLoot = true;
                                        }
                                        else
                                        {
                                            //--Otherwise, increase how many he has, up to 99
                                            if (enemyDrops[drop.Name] < 99)
                                            {
                                                enemyDrops[drop.Name]++;
                                                game.Notebook.Inventory.newLoot = true;
                                            }
                                            else
                                                cantPickUp = true;
                                        }
                                    }

                                    //--Add a drop name to the HUD
                                    if (pickUps.Count < 5)
                                    {
                                        pickUpTimers.Insert(0, 240);

                                        if (!cantPickUp)
                                            pickUps.Insert(0, drop.Name);
                                        else
                                            pickUps.Insert(0, "You don't have room for this item.");
                                    }
                                    else
                                    {
                                        pickUps.RemoveAt(4);
                                        pickUpTimers.RemoveAt(4);
                                        if (!cantPickUp)
                                            pickUps.Insert(0, drop.Name);
                                        else
                                            pickUps.Insert(0, "You don't have room for this item.");
                                        pickUpTimers.Insert(0, 240);
                                    }
                                }
                                #endregion


                                //--Make it so the drop is picked up
                                if(!cantPickUp)
                                    drop.PickedUp = true;

                            }

                            //--Saying break here makes it so you can only pick up one item at a time
                            cantPickUp = false;
                            break;
                        }
                    }
                }
            }
        }

        public void RemoveDrops(String name, int num)
        {
            if (enemyDrops.ContainsKey(name) && enemyDrops[name] >= num)
            {
                enemyDrops[name] -= num;

                if (enemyDrops[name] == 0)
                {
                    enemyDrops.Remove(name);
                }
            }
        }

        //--Places the player at the most recent portal if he falls off the map
        public void FallOffMap()
        {
            if (VitalRecY > 1000)
            {
                health -= (int)maxHealth / 10;

                //Sets the player's position equal to the chapter's starting portal
                //A starting portal is set every time the player enters a map, it is the portal he entered from
                nextMapPos = new Vector2(game.CurrentChapter.StartingPortal.PortalRec.X - 200, 
                    game.CurrentChapter.StartingPortal.PortalRec.Y + 
                    game.CurrentChapter.StartingPortal.PortalRec.Height - 170 - 135 - 100);
                Invincible(90);

                //Set the nextmap equal to this one, using the startingPortal's mapName, and then change maps to cause a fade
                game.CurrentChapter.NextMap = game.CurrentChapter.StartingPortal.MapName;
                game.CurrentChapter.fallingOffMap = true;
                game.CurrentChapter.state = Chapter.GameState.ChangingMaps;
            }
        }

        public Boolean CheckIfHit(Rectangle rec)
        {
            if (playerState == PlayerState.jumping)
            {
                if (rec.Intersects(jumpingVitalRec))
                {

                    attackFalling = true;
                    return true;
                }
            }
            else if ((playerState == PlayerState.standing && ducking) || isStunned)
            {
                if (rec.Intersects(duckingVitalRec))
                {
                    return true;
                }
            }
            else
            {
                if (rec.Intersects(vitalRec))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveStoryItem(String itemName, int num)
        {
            if (storyItems.ContainsKey(itemName))
            {
                storyItems[itemName] -= num;

                if (storyItems[itemName] <= 0)
                    storyItems.Remove(itemName);
            }
        }

        public void GrabLedge()
        {
            for (int i = 0; i < game.CurrentChapter.CurrentMap.Platforms.Count; i++)
            {
                Platform thisPlat = game.CurrentChapter.CurrentMap.Platforms[i];

                Rectangle top = new Rectangle(thisPlat.Rec.X + 5, thisPlat.Rec.Y, thisPlat.Rec.Width - 5, 20);
                Rectangle left = new Rectangle(thisPlat.Rec.X, thisPlat.Rec.Y + 5, 10, thisPlat.Rec.Height - 3);
                Rectangle right = new Rectangle(thisPlat.Rec.X + thisPlat.Rec.Width - 10, thisPlat.Rec.Y + 5, 20, thisPlat.Rec.Height - 3);

                if (current.IsKeyDown(Keys.LeftShift) && currentPlat == null)
                {
                    if (facingRight)
                    {
                        if (platGrabRec.Intersects(top) && platGrabRec.Intersects(left))
                        {
                            playerState = PlayerState.grabbingLedge;
                            currentPlat = thisPlat;
                            falling = false;
                            VelocityY = 0;
                            VelocityX = 0;
                            timeHanging = 0;
                            break;
                        }
                    }
                    else
                    {
                        if (platGrabRec.Intersects(top) && platGrabRec.Intersects(right))
                        {
                            playerState = PlayerState.grabbingLedge;
                            currentPlat = thisPlat;
                            falling = false;
                            VelocityY = 0;
                            VelocityX = 0;
                            timeHanging = 0;
                            break;
                        }
                    }
                }
            }
        }

        public void AddMoneyJustPickedUp(float amount)
        {
            moneyJustPickedUp += amount;
            moneyJustPickedUpTimer = 60;
        }

        public void UpdateMoneyJustPickedUp()
        {
            moneyJustPickedUpTimer--;

            if(moneyJustPickedUpTimer == 0)
            {
                moneyJustPickedUp = 0f;
            }
        }

        public void CutsceneRun(Vector2 velocity)
        {
            landing = false;
            ducking = false;
            drawStandingBackUpLines = false;
            if (velocity.X > 0)
            {
                facingRight = true;
            }
            if (velocity.X < 0)
            {
                facingRight = false;
            }

            playerState = PlayerState.running;

            if (cutsceneMoving == false)
                moveFrame = 0;

            if ((moveFrame == 2 || moveFrame == 7) && frameDelay == 3)
            {
                int ran = selectFlinch.Next(3);
                if (ran == 0)
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom1);
                else if (ran == 1)
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom2);
                else
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom3);
            }

            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;
                frameDelay = 5;
            }

            if (moveFrame > 13)
                moveFrame = 0;

            cutsceneMoving = true;

            position += velocity;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            vitalRec.X = rec.X + (rec.Width / 2) - 30;
            vitalRec.Y = rec.Y + (rec.Height / 4);

        }

        public void CutsceneWalk(Vector2 velocity)
        {
            landing = false;
            ducking = false;
            drawStandingBackUpLines = false;
            if (velocity.X > 0)
            {
                facingRight = true;
            }
            if (velocity.X < 0)
            {
                facingRight = false;
            }

            playerState = PlayerState.walking;

            if (cutsceneMoving == false)
                moveFrame = 0;

            if ((moveFrame == 2 || moveFrame == 7) && frameDelay == 3)
            {
                int ran = selectFlinch.Next(3);
                if (ran == 0)
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom1);
                else if (ran == 1)
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom2);
                else
                    Sound.PlaySoundInstance(Sound.SoundNames.PlayerRunRoom3);
            }

            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;
                frameDelay = 6;
            }

            if (moveFrame > 9)
                moveFrame = 0;

            cutsceneMoving = true;

            position += velocity;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            vitalRec.X = rec.X + (rec.Width / 2) - 30;
            vitalRec.Y = rec.Y + (rec.Height / 4);

        }

        /// <summary>
        /// Stuns daryl for a set time if he isn't already stunned, and can be stunned
        /// </summary>
        public void StunDaryl(int time)
        {
            if (canBeStunned && !isStunned)
            {
                moveFrame = 0;
                frameDelay = 10;
                playerState = PlayerState.standing;
                StopSkills();
                isStunned = true;
                stunTime = time;
            }
        }

        //Removes all of the player's equipment and the stats they give
        public void RemoveAllEquipment()
        {
            if (equippedWeapon != null)
            {
                Strength -= EquippedWeapon.Strength;
                MaxHealth -= EquippedWeapon.Health;
                Health -= EquippedWeapon.Health;
                Defense -= EquippedWeapon.Defense;
                JumpHeight -= EquippedWeapon.JumpHeight;
                MoveSpeed -= EquippedWeapon.MoveSpeed;

                equippedWeapon = null;
            }
            if (secondWeapon != null)
            {
                Strength -= secondWeapon.Strength;
                MaxHealth -= secondWeapon.Health;
                Health -= secondWeapon.Health;
                Defense -= secondWeapon.Defense;
                JumpHeight -= secondWeapon.JumpHeight;
                MoveSpeed -= secondWeapon.MoveSpeed;

                secondWeapon = null;
            }
            if (equippedHoodie != null)
            {
                Strength -= equippedHoodie.Strength;
                MaxHealth -= equippedHoodie.Health;
                Health -= equippedHoodie.Health;
                Defense -= equippedHoodie.Defense;
                JumpHeight -= equippedHoodie.JumpHeight;
                MoveSpeed -= equippedHoodie.MoveSpeed;

                equippedHoodie = null;

            }
            if (equippedHat != null)
            {
                Strength -= equippedHat.Strength;
                MaxHealth -= equippedHat.Health;
                Health -= equippedHat.Health;
                Defense -= equippedHat.Defense;
                JumpHeight -= equippedHat.JumpHeight;
                MoveSpeed -= equippedHat.MoveSpeed;

                equippedHat = null;
            }
            if (equippedAccessory != null)
            {
                Strength -= equippedAccessory.Strength;
                MaxHealth -= equippedAccessory.Health;
                Health -= equippedAccessory.Health;
                Defense -= equippedAccessory.Defense;
                JumpHeight -= equippedAccessory.JumpHeight;
                MoveSpeed -= equippedAccessory.MoveSpeed;

                equippedAccessory = null;
            }
            if (secondAccessory != null)
            {
                Strength -= secondAccessory.Strength;
                MaxHealth -= secondAccessory.Health;
                Health -= secondAccessory.Health;
                Defense -= secondAccessory.Defense;
                JumpHeight -= secondAccessory.JumpHeight;
                MoveSpeed -= secondAccessory.MoveSpeed;

                secondAccessory = null;
            }
        }

        public void SetCharacterBioDictionary()
        {
            allCharacterBios.Add("Paul", false);
            allCharacterBios.Add("Alan", true);
            allCharacterBios.Add("Mr. Robatto", true);
            allCharacterBios.Add("Equipment Instructor", true);
            allCharacterBios.Add("Karma Instructor", true);
            allCharacterBios.Add("Journal Instructor", true);
            allCharacterBios.Add("Save Instructor", true);
            allCharacterBios.Add("Skill Instructor", true);
            allCharacterBios.Add("Chelsea", true);
            allCharacterBios.Add("Mark", true);
            allCharacterBios.Add("Trenchcoat Employee", true);
            allCharacterBios.Add("Gardener", true);
            allCharacterBios.Add("Tim", true);
            allCharacterBios.Add("Julius Caesar", false);
            allCharacterBios.Add("Pelt Kid", true);
            allCharacterBios.Add("Jesse", true);
            allCharacterBios.Add("Squirrel Boy", true);
            allCharacterBios.Add("Bob the Construction Guy", true);
            allCharacterBios.Add("Princess", false);
            allCharacterBios.Add("Janitor", false);
            allCharacterBios.Add("Hangerman", false);
            allCharacterBios.Add("Author", false);
            allCharacterBios.Add("Journal Kid", false);
        }

        public void SetMonsterBioDictionary()
        {
            allMonsterBios.Add("Erl The Flask", true);
            allMonsterBios.Add("Benny Beaker", true);
            allMonsterBios.Add("Crow", true);
            allMonsterBios.Add("Scarecrow", true);
            allMonsterBios.Add("Goblin Gate", true);
            allMonsterBios.Add("Goblin", true);
            allMonsterBios.Add("Field Goblin", false);
            allMonsterBios.Add("Troll", true);
        }

        public void AddWeaponToInventory(Weapon w)
        {
            ownedWeapons.Add(w);
            game.Notebook.Inventory.newWeapon = true;
        }

        public void AddHatToInventory(Hat h)
        {
            ownedHats.Add(h);
            game.Notebook.Inventory.newHat = true;
        }

        public void AddShirtToInventory(Hoodie s)
        {
            ownedHoodies.Add(s);
            game.Notebook.Inventory.newShirt = true;
        }

        public void AddAccessoryToInventory(Accessory a)
        {
            ownedAccessories.Add(a);
            game.Notebook.Inventory.newAccessory = true;
        }
    }
}
