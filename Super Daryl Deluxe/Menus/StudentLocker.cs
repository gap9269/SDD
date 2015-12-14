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

    public class LootForStudentLocker
    {

        public int amount;
        public String type;

        public LootForStudentLocker(String type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }

    public class StoryItemStack
    {

        public int amount;
        public String type;

        public StoryItemStack(String type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }

    public class StudentLocker
    {

        public enum LockerState
        {
            closed,
            opening,
            open
        }
        public LockerState state;
        public static ContentManager Content;

        float rayRotation;

        float lockPosY;
        float boxOneY, boxTwoY, boxThreeY;
        float darylPosX = 1280;

        Texture2D backgroundClosed, activeDial, RB, foregroundClosed, descriptionBox, itemBox, numbers, openBlurred, openRegular, spinningLock, overlay, tooltip, staticBoxTex, comboPage, leftStatic, leftActive, rightStatic, rightActive, piggy, piggyRays, keys, dPadTip, backspace, b;
        Button leftPageArrow, rightPageArrow;
        int page;

        protected String code;
        protected int lockerNumber;
        protected Player player;
        protected Game1 game;

        protected KeyboardState current;
        protected KeyboardState last;
        protected Rectangle rec;
        protected Texture2D tex;
        protected List<Object> contents;
        protected List<Boolean> takenContents;
        protected List<Button> contentBoxes;
        protected List<Rectangle> itemBoxLocations;

        protected Button dial1;
        protected Button dial2;
        protected Button dial3;

        protected string input;

        protected int first;
        protected int second;
        protected int third;

        Button backspaceRec;

        SoundEffectInstance ui_locker_spin_loop;

        int dialSelected = 1; //Which dial is currently active

        int openingTimer;

        float dialRotation;
        float dialVelocity;
        float dialAcceleration;
        Boolean spinRight = true;

        String lockerName;

        DescriptionBoxManager descriptionBoxManager;

        public Texture2D Tex { get { return tex; } set { tex = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public List<Boolean> TakenContents { get { return takenContents; } set { takenContents = value; } }
        public String LockerName { get { return lockerName; } }
        public List<Object> Contents { get { return contents; } }
        public StudentLocker(Player p, Game1 g, Rectangle r, List<Object> content, String lockerName, Texture2D t)
        {
            Content = new ContentManager(g.Services);
            Content.RootDirectory = "Content";

            lockPosY = boxOneY = boxTwoY = boxThreeY = 720;
            tex = t;
            state = LockerState.closed;
            player = p;
            game = g;
            rec = r;
            contentBoxes = new List<Button>();
            takenContents = new List<bool>();
            descriptionBoxManager = game.DescriptionBoxManager;
            this.lockerName = lockerName;

            input = "000000"; //00-00-00 is the default
            dial1 = new Button(new Rectangle(755, 520, 80, 80));
            dial2 = new Button(new Rectangle(885, 520, 80, 80));
            dial3 = new Button(new Rectangle(1000, 520, 80, 80));

            leftPageArrow = new Button(new Rectangle(258, 646, 40, 28));
            rightPageArrow = new Button(new Rectangle(375, 642, 35, 29));
            itemBoxLocations = new List<Rectangle>();
            SetContentBoxes();
            contents = content;

            for (int i = 0; i < contents.Count; i++)
            {
                takenContents.Add(false);
            }

            backspaceRec = new Button(new Rectangle(1166, 7, 110, 18));

        }

        public void LoadContent()
        {
            backgroundClosed = Content.Load<Texture2D>(@"Menus\StudentLocker\BackgroundClosed");
            activeDial = Content.Load<Texture2D>(@"Menus\StudentLocker\boxActive");
            foregroundClosed = Content.Load<Texture2D>(@"Menus\StudentLocker\ForegroundClosed");
            itemBox = Content.Load<Texture2D>(@"Menus\StudentLocker\itemBox");
            numbers = Content.Load<Texture2D>(@"Menus\StudentLocker\Numbers");
            openBlurred = Content.Load<Texture2D>(@"Menus\StudentLocker\openBlurred");
            openRegular = Content.Load<Texture2D>(@"Menus\StudentLocker\openRegular");
            spinningLock = Content.Load<Texture2D>(@"Menus\StudentLocker\spinningLock");
            overlay = Content.Load<Texture2D>(@"Menus\StudentLocker\overlay");
            tooltip = Content.Load<Texture2D>(@"Menus\StudentLocker\tooltip");
            dPadTip = Content.Load<Texture2D>(@"Menus\StudentLocker\DPadTip");
            staticBoxTex = Content.Load<Texture2D>(@"Menus\StudentLocker\boxStatic");
            backspace = Content.Load<Texture2D>(@"Menus\BackspaceNotebook");
            b = Content.Load<Texture2D>(@"Menus\B");
            piggy = Content.Load<Texture2D>(@"Menus\Shop\Piggy");
            keys = Content.Load<Texture2D>(@"Menus\Shop\Keys");
            piggyRays = Content.Load<Texture2D>(@"Menus\Shop\PiggyRay");
            RB = Content.Load<Texture2D>(@"Notifications\RB");
            //Combo page
            comboPage = Content.Load<Texture2D>(@"Menus\StudentLocker\ComboPage");
            rightActive = Content.Load<Texture2D>(@"Menus\StudentLocker\rightArrowActive");
            rightStatic = Content.Load<Texture2D>(@"Menus\StudentLocker\rightArrowStatic");
            leftActive = Content.Load<Texture2D>(@"Menus\StudentLocker\leftArrowActive");
            leftStatic = Content.Load<Texture2D>(@"Menus\StudentLocker\leftArrowStatic");

            Sound.LoadStudentLockerSounds();
            ui_locker_spin_loop = Sound.menuSoundEffects["ui_locker_spin_loop"].CreateInstance();
        }

        public void UnloadContent()
        {
            Content.Unload();
            Sound.UnloadMenuSounds();
        }

        //--Draws all of the item slots in the locker
        public void SetContentBoxes()
        {

            Button contentBox;
            Rectangle itemBoxLocation;
            for (int i = 0; i < 9; i++)
            {

                if (i < 3)
                {
                    itemBoxLocation = new Rectangle(740 + (i * 128), 234, 70, 70);
                    contentBox = new Button(Game1.emptyBox, new Rectangle(760 + (i * 127), 250, 70, 70));
                }
                else if (i < 6)
                {
                    itemBoxLocation = new Rectangle(740 + ((i - 3) * 128), 358, 70, 70);
                    contentBox = new Button(Game1.emptyBox, new Rectangle(760 + ((i - 3) * 127), 374, 70, 70));
                }
                else
                {
                    itemBoxLocation = new Rectangle(740 + ((i - 6) * 128), 486, 70, 70);
                    contentBox = new Button(Game1.emptyBox, new Rectangle(760 + ((i - 6) * 127), 502, 70, 70));
                }
                contentBoxes.Add(contentBox);
                itemBoxLocations.Add(itemBoxLocation);
            }
        }

        public void UpdateResolution()
        {
            for (int i = 0; i < 12; i++)
            {
                if (i < 3)
                {
                    contentBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 - 260;
                }
                else if (i < 6)
                {
                    contentBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 - 160;

                }
                else if (i < 9)
                {
                    contentBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 - 60;
                }
                else
                {
                    contentBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 + 40;
                }

            }

            dial1.ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 + 5;
            dial2.ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 + 5;
            dial3.ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 + 5;
        }

        //--Sets the box icons to equal what is in them
        public void SetContents()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if(contents[i] is Equipment)
                {
                    Equipment eq = contents[i] as Equipment;
                    contentBoxes[i].ButtonTexture = eq.Icon;
                }
                else if (contents[i] is StoryItem)
                {
                    StoryItem stry = contents[i] as StoryItem;
                    contentBoxes[i].ButtonTexture = stry.Icon;
                }
                else if (contents[i] is Collectible)
                {
                    if(contents[i] is GoldKey)
                        contentBoxes[i].ButtonTexture = Game1.storyItemIcons["Gold Key"];
                    if (contents[i] is BronzeKey)
                        contentBoxes[i].ButtonTexture = Game1.storyItemIcons["Bronze Key"];
                    if (contents[i] is SilverKey)
                        contentBoxes[i].ButtonTexture = Game1.storyItemIcons["Silver Key"];
                    if (contents[i] is Textbook)
                        contentBoxes[i].ButtonTexture = Game1.textbookTextures;
                }
                else if (contents[i] is LootForStudentLocker)
                {
                    LootForStudentLocker tmp = contents[i] as LootForStudentLocker;
                    contentBoxes[i].ButtonTexture = EnemyDrop.allDrops[tmp.type].texture;
                }
            }
        }

        public void UpdateContentsOnLoad()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (takenContents[i] == true)
                {
                    contents.RemoveAt(i);
                    i--;
                    ResetBoxes();
                }
            }
        }

        //--Checks if the player is opening the locker
        public void CheckIfOpening()
        {
            last = current;
            current = Keyboard.GetState();

            if (player.VitalRec.Intersects(rec) && (current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F) || MyGamePad.LeftBumperPressed()))
            {
                LoadContent();
                game.CurrentChapter.CurrentLocker = this;
                game.CurrentChapter.state = Chapter.GameState.BreakingLocker;
                code = GenerateLockerCombinations.combinations[lockerName];
                Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_other_menu_open);
            }
        }

        public void PlayClickDialSound()
        {
            String soundEffectName = "ui_locker_dial_click_0" + Game1.randomNumberGen.Next(1, 7);
            Sound.PlaySoundInstance(Sound.menuSoundEffects[soundEffectName], soundEffectName, false);
        }

        public virtual void Update()
        {

            //Rotate the ray
            rayRotation += .5f;
            if (rayRotation == 360)
                rayRotation = 0;

            //Find a better place for this
            // UpdateResolution();
            SetContents();
            last = current;
            current = Keyboard.GetState();

            #region Exit the screen
            if (((last.IsKeyDown(Keys.Escape) && current.IsKeyUp(Keys.Escape))) || ((last.IsKeyDown(Keys.Back) && current.IsKeyUp(Keys.Back))) || MyGamePad.BPressed() || (backspaceRec.Clicked() && !Game1.gamePadConnected))
            {
                first = 0;
                second = 0;
                third = 0;
                dialRotation = 0;
                dialAcceleration = 0;
                dialVelocity = 0;
                dialSelected = 0;

                lockPosY = boxOneY = boxTwoY = boxThreeY = 720;
                darylPosX = 1280;
                openingTimer = 0;


                if (state == LockerState.open)
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_shut);
                else
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_close);

                state = LockerState.closed;
                UnloadContent();
                game.CurrentChapter.state = Chapter.GameState.Game;
                game.CurrentChapter.CurrentLocker = null;
                MyGamePad.ResetStates();

            }
            #endregion

            switch (state)
            {
                #region Closed Locker
                case LockerState.closed:

                    if (boxTwoY > 517)
                    {
                        if (lockPosY > 0)
                        {
                            float distance = lockPosY - 1;

                            lockPosY -= 3 * (distance / 40);

                            if (lockPosY <= 1.5)
                            {
                                lockPosY = 0;
                            }
                        }
                        else
                        {
                            if (boxOneY > 517)
                            {
                                float distance = boxOneY - 517;

                                boxOneY -= 3 * (distance / 40);

                                if (boxOneY <= 518)
                                {
                                    boxOneY = 517;
                                }
                            }

                            if (boxThreeY > 517 && boxOneY < 650)
                            {
                                float distance = boxThreeY - 517;

                                boxThreeY -= 3 * (distance / 40);

                                if (boxThreeY <= 518)
                                {
                                    boxThreeY = 517;
                                }
                            }


                            if (boxTwoY > 517 && boxThreeY < 650)
                            {
                                float distance = boxTwoY - 517;

                                boxTwoY -= 3 * (distance / 40);

                                if (boxTwoY <= 518)
                                {
                                    boxTwoY = 517;

                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_other_menu_appear);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (spinRight)
                        {
                            if (dialVelocity > 0)
                            {
                                dialAcceleration -= .1f;
                                dialVelocity += dialAcceleration;
                            }

                            dialRotation += dialVelocity;

                            if (dialVelocity < 0)
                            {
                                dialVelocity = 0;
                                dialAcceleration = 0;
                            }

                            if (dialRotation >= 360)
                                dialRotation = 0;
                            else if (dialRotation <= 0)
                                dialRotation = 360;
                        }
                        else
                        {
                            if (dialVelocity < 0)
                            {
                                dialAcceleration += .1f;
                                dialVelocity += dialAcceleration;
                            }

                            dialRotation += dialVelocity;

                            if (dialVelocity > 0)
                            {
                                dialVelocity = 0;
                                dialAcceleration = 0;
                            }

                            if (dialRotation >= 360)
                                dialRotation = 0;

                            else if (dialRotation <= 0)
                                dialRotation = 360;
                        }

                        if (dialVelocity != 0)
                        {
                            Sound.PlaySoundInstance(ui_locker_spin_loop, Game1.GetFileName(() => ui_locker_spin_loop), true);

                            double t = Math.Round(Math.Abs(dialVelocity), 1);

                            if (t == 2.2)
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_spin_loop_end);
                        }
                        else
                        {
                            ui_locker_spin_loop.Stop();
                        }

                        #region Change Dials
                        //Which dial is currently selected
                        if (Game1.gamePadConnected)
                        {
                            if (dialSelected == 0)
                                dialSelected = 1;

                            if(MyGamePad.RightPadPressed())
                            {
                                dialSelected++;

                                if (dialSelected > 3)
                                    dialSelected = 1;
                            }
                            else if (MyGamePad.LeftPadPressed())
                                {
                                    dialSelected--;

                                    if (dialSelected < 1)
                                        dialSelected = 3;
                                }
                        }
                        else
                        {
                            if (dial1.IsOver())
                                dialSelected = 1;
                            else if (dial2.IsOver())
                                dialSelected = 2;
                            else if (dial3.IsOver())
                                dialSelected = 3;
                            else dialSelected = 0;
                        }



                        //--If you click a dial, increment the number. Go back to 0 after 9
                        if ((dial1.Clicked()  && !Game1.gamePadConnected) || ((MyGamePad.APressed() || MyGamePad.UpPadPressed()) && dialSelected == 1))
                        {
                            dialVelocity = 10;
                            dialAcceleration = 0;
                            spinRight = true;

                            first++;

                            if (first > 10)
                                first = 0;

                            PlayClickDialSound();
                        }
                        if ((dial2.Clicked()  && !Game1.gamePadConnected) || ((MyGamePad.APressed() || MyGamePad.UpPadPressed()) && dialSelected == 2))
                        {
                            dialVelocity = -10;
                            dialAcceleration = 0;
                            spinRight = false;

                            second++;

                            if (second > 10)
                                second = 0;

                            PlayClickDialSound();
                        }
                        if ((dial3.Clicked()  && !Game1.gamePadConnected) || ((MyGamePad.APressed() || MyGamePad.UpPadPressed()) && dialSelected == 3))
                        {
                            dialVelocity = 10;
                            dialAcceleration = 0;
                            spinRight = true;

                            third++;

                            if (third > 10)
                                third = 0;

                            PlayClickDialSound();
                        }

                        if ((dial1.RightClicked() && !Game1.gamePadConnected)  || ((MyGamePad.XPressed() || MyGamePad.DownPadPressed()) && dialSelected == 1))
                        {
                            dialVelocity = -10;
                            dialAcceleration = 0;
                            spinRight = false;

                            first--;

                            if (first < 0)
                                first = 10;
                            PlayClickDialSound();
                        }
                        if ((dial2.RightClicked() && !Game1.gamePadConnected)  || ((MyGamePad.XPressed() || MyGamePad.DownPadPressed()) && dialSelected == 2))
                        {
                            dialVelocity = 10;
                            dialAcceleration = 0;
                            spinRight = true;

                            second--;

                            if (second < 0)
                                second = 10;
                            PlayClickDialSound();
                        }
                        if ((dial3.RightClicked() && !Game1.gamePadConnected) || ((MyGamePad.XPressed() || MyGamePad.DownPadPressed()) && dialSelected == 3))
                        {
                            dialVelocity = -10;
                            dialAcceleration = 0;
                            spinRight = false;

                            third--;

                            if (third < 0)
                                third = 10;
                            PlayClickDialSound();
                        }
                        #endregion


                        //If it is less than 10, add a 0 in front
                        if (first != 10)
                            input = "0" + first.ToString();
                        else
                            input = first.ToString();
                        //If it is less than 10, add a 0 in front
                        if (second != 10)
                            input += "0" + second.ToString();
                        else
                            input += second.ToString();
                        //If it is less than 10, add a 0 in front
                        if (third != 10)
                            input += "0" + third.ToString();
                        else
                            input += third.ToString();

                        //COMBO PAGE
                        if ((leftPageArrow.Clicked() || MyGamePad.LeftBumperPressed()) && page > 0)
                        {
                            PlayChangePageSound();
                            page--;
                        }
                        if ((rightPageArrow.Clicked() || MyGamePad.RightBumperPressed()) && page < 5)
                        {
                            PlayChangePageSound();
                            page++;
                        }
                        if (input == code)
                        {
                            ui_locker_spin_loop.Stop();
                            state = LockerState.opening;
                           Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_unlock);
                        }
                    }

                break;
                #endregion

                case LockerState.opening:
                openingTimer++;
                if (openingTimer == 1)
                    dialVelocity = 3;

                if (openingTimer < 30)
                {
                    dialAcceleration += .09f;
                    dialVelocity += dialAcceleration;
                    dialRotation += dialVelocity;
                    if (dialRotation >= 360)
                        dialRotation = 0;
                }
                else if (openingTimer <= 150)
                {
                    if (openingTimer == 45)
                        dialAcceleration = 0;

                    dialAcceleration -= .1f;
                    float distance = dialVelocity - 1;
                    dialVelocity -= 3 * (distance / 50);

                    if (dialVelocity <= 1.5f)
                        dialVelocity = 0;

                    dialRotation += dialVelocity;

                    if (dialRotation >= 360)
                        dialRotation = 0;
                    else if (dialRotation <= 0)
                        dialRotation = 360;
                }
                else
                    state = LockerState.open;
                break;

                case LockerState.open:

                if (darylPosX > 563)
                {
                    float distance = darylPosX - 563;

                    darylPosX -= 3 * (distance / 40);

                    if (darylPosX <= 564)
                    {
                        darylPosX = 563;
                    }
                }
                else
                    UpdateContents();
                break;
            }
        }

        public void ResetBoxes()
        {
            foreach (Button b in contentBoxes)
            {
                b.ButtonTexture = Game1.emptyBox;
            }
        }

        public void PlayChangePageSound()
        {
            String soundEffectName = "ui_inventory_list_0" + Game1.randomNumberGen.Next(1, 3);
            Sound.PlaySoundInstance(Sound.menuSoundEffects[soundEffectName], soundEffectName, false);
        }

        public void UpdateContents()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                if (contentBoxes[i].DoubleClicked())
                {
                    if(contents[i] is Equipment)
                    {
                        Equipment eq = contents[i] as Equipment;

                        if (eq is Weapon)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_weapon);
                            player.AddWeaponToInventory(eq as Weapon);
                        }
                        if (eq is Hat)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_outfit);
                            player.AddHatToInventory(eq as Hat);
                        }
                        if (eq is Outfit)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_outfit);
                            player.AddShirtToInventory(eq as Outfit);
                        }
                        if (eq is Accessory)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_outfit);
                            player.AddAccessoryToInventory(eq as Accessory);
                        }
                        if (eq is Money)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_money);
                            player.Money += (eq as Money).Amount;
                        }

                        contents.RemoveAt(i);

                        takenContents[i] = true;
                        i--;
                        ResetBoxes();
                    }
                    else if (contents[i] is StoryItem)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_key);
                        StoryItem temp = contents[i] as StoryItem;

                        if (player.StoryItems.ContainsKey(temp.Name))
                            player.StoryItems[temp.Name]++;
                        else
                            player.StoryItems.Add(temp.Name, 1);

                        contents.RemoveAt(i);

                        takenContents[i] = true;
                        i--;
                        ResetBoxes();
                    }
                    else if (contents[i] is Collectible)
                    {
                        if (contents[i] is Textbook)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_book);
                            player.Textbooks++;
                        }
                        else if (contents[i] is BronzeKey)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_key);
                            player.BronzeKeys++;
                        }
                        else if (contents[i] is SilverKey)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_key);
                            player.SilverKeys++;
                        }
                        else if (contents[i] is GoldKey)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_locker_take_item_key);
                            player.GoldKeys++;
                        }
                        contents.RemoveAt(i);

                        takenContents[i] = true;
                        i--;
                        ResetBoxes();
                    }
                    else if (contents[i] is LootForStudentLocker)
                    {
                        LootForStudentLocker tmp = contents[i] as LootForStudentLocker;

                        if (this.lockerName.Equals("Drew\'s Locker") && tmp.type == "Broken Glass")
                        {
                            Game1.g.ChapterOne.ChapterOneBooleans["stolenGlass"] = true;
                        }

                        if (player.AddLoot(tmp.type, tmp.amount))
                        {
                            contents.RemoveAt(i);
                            takenContents[i] = true;
                            i--;
                            ResetBoxes();
                        }
                    }
                }
            }
        }

        //--Draws the description boxes for the items
        public void DrawDescriptions()
        {
            if (state == LockerState.open && !(darylPosX > 563))
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    if (contentBoxes[i].IsOver())
                    {
                        if (contents[i] is Equipment)
                            descriptionBoxManager.DrawEquipDescriptions(contents[i] as Equipment, contentBoxes[i].ButtonRec);
                        else if (contents[i] is StoryItem)
                            descriptionBoxManager.DrawStoryItemDescriptions((contents[i] as StoryItem).Name, contentBoxes[i].ButtonRec);
                        else if (contents[i] is Collectible)
                            descriptionBoxManager.DrawCollectibleDescriptions((contents[i] as Collectible).collecName, (contents[i] as Collectible).Description, contentBoxes[i].ButtonRec);
                        else if (contents[i] is LootForStudentLocker)
                            descriptionBoxManager.DrawLootDescriptions((contents[i] as LootForStudentLocker).type, contentBoxes[i].ButtonRec);
                    }
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(overlay, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            switch (state)
            {
                case LockerState.opening:
                case LockerState.closed:
                    s.Draw(backgroundClosed, new Rectangle(1280 - backgroundClosed.Width, (int)lockPosY, backgroundClosed.Width, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.Draw(spinningLock, new Rectangle(789 + 133, (int)lockPosY + 292 + 133, spinningLock.Width, spinningLock.Height), null, Color.White, (float)(dialRotation * (Math.PI / 180)), new Vector2(spinningLock.Width / 2, spinningLock.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(foregroundClosed, new Rectangle(1280 - foregroundClosed.Width, (int)lockPosY, foregroundClosed.Width, (int)(Game1.aspectRatio * 1280)), Color.White);

                    s.Draw(staticBoxTex, new Rectangle(759, (int)boxOneY, staticBoxTex.Width, staticBoxTex.Height), Color.White);
                    s.Draw(staticBoxTex, new Rectangle(882, (int)boxTwoY, staticBoxTex.Width, staticBoxTex.Height), Color.White);
                    s.Draw(staticBoxTex, new Rectangle(1005, (int)boxThreeY, staticBoxTex.Width, staticBoxTex.Height), Color.White);

                    if(lockPosY == 0)
                        s.DrawString(Game1.twConLarge, lockerName, new Vector2(920 - Game1.twConLarge.MeasureString(lockerName).X / 2, 123), Color.White);

                    if (boxTwoY == 517)
                    {
                        if(!Game1.gamePadConnected)
                            s.Draw(tooltip, new Rectangle(1280 - tooltip.Width, 0, tooltip.Width, (int)(Game1.aspectRatio * 1280)), Color.White);
                        else
                            s.Draw(dPadTip, new Rectangle(1280 - tooltip.Width, 0, tooltip.Width, (int)(Game1.aspectRatio * 1280)), Color.White);

                        //DRAW STATIC NUMBERS
                        s.Draw(numbers, new Rectangle(770, 540, 57, 47), new Rectangle(57 * first, 0, 57, 47), Color.White);
                        s.Draw(numbers, new Rectangle(893, 540, 57, 47), new Rectangle(57 * second, 0, 57, 47), Color.White);
                        s.Draw(numbers, new Rectangle(1016, 540, 57, 47), new Rectangle(57 * third, 0, 57, 47), Color.White);

                        //DRAW ACTIVE BOX AND NUMBER
                        switch (dialSelected)
                        {
                            case 1:
                                s.Draw(activeDial, new Rectangle(745, 506, activeDial.Width, activeDial.Height), Color.White);
                                s.Draw(numbers, new Rectangle(766, 538, 65, 51), new Rectangle(65 * first, 48, 65, 51), Color.White);
                                break;
                            case 2:
                                s.Draw(activeDial, new Rectangle(868, 506, activeDial.Width, activeDial.Height), Color.White);
                                s.Draw(numbers, new Rectangle(890, 538, 65, 51), new Rectangle(65 * second, 48, 65, 51), Color.White);
                                break;
                            case 3:
                                s.Draw(activeDial, new Rectangle(991, 506, activeDial.Width, activeDial.Height), Color.White);
                                s.Draw(numbers, new Rectangle(1012, 538, 65, 51), new Rectangle(65 * third, 48, 65, 51), Color.White);
                                break;
                        }

                        s.Draw(comboPage, new Rectangle(0, 0, comboPage.Width, comboPage.Height), Color.White);

                        if (!Game1.gamePadConnected)
                        {
                            if (leftPageArrow.IsOver())
                                s.Draw(leftActive, new Rectangle(243, 638, leftActive.Width, leftActive.Height), Color.White);
                            else
                                s.Draw(leftStatic, new Rectangle(243, 638, leftActive.Width, leftActive.Height), Color.White);

                            if (rightPageArrow.IsOver())
                                s.Draw(rightActive, new Rectangle(243, 638, leftActive.Width, leftActive.Height), Color.White);
                            else
                                s.Draw(rightStatic, new Rectangle(243, 638, leftActive.Width, leftActive.Height), Color.White);
                        }
                        else
                        {
                            s.Draw(Game1.lbBack, new Vector2(248, 637), Color.White);
                            s.Draw(Game1.lbOutline, new Vector2(248, 637), Color.White);
                            s.Draw(RB, new Vector2(360, 635), Color.White);
                        }
                        s.DrawString(Game1.font, (page + 1).ToString() + " / 6", new Vector2(316, 646), Color.Black);

                        //9 is the number of combos per page for this menu
                        for (int i = 0; i < 9; i++)
                        {
                            if (game.Notebook.ComboPage.LockerCombos.Count > i + (page * 9))
                            {
                                //--Draw the person's name
                                s.DrawString(Game1.twConMedium, game.Notebook.ComboPage.LockerCombos.ElementAt(i + (page * 9)).Key, new Vector2(190, (180) + (i * 50)), Color.Black);

                                //--Draw the combo
                                s.DrawString(Game1.twConMedium, game.Notebook.ComboPage.LockerCombos.ElementAt(i + (page * 9)).Value, new Vector2(370, (180) + (i * 50)), Color.Black);
                            }
                        }
                    }
                    break;

                case LockerState.open:
                    if (darylPosX > 563)
                        s.Draw(openRegular, new Rectangle((int)darylPosX, (int)(Game1.aspectRatio * 1280) - openBlurred.Height, openBlurred.Width, openBlurred.Height), Color.White);

                    else
                    {
                        s.Draw(openBlurred, new Rectangle((int)darylPosX, (int)(Game1.aspectRatio * 1280) - openBlurred.Height, openBlurred.Width, openBlurred.Height), Color.White);

                        for (int i = 0; i < contentBoxes.Count; i++)
                        {

                            //s.Draw(itemBox, contentBoxes[i].ButtonRec, Color.White);
                            s.Draw(itemBox, new Rectangle(itemBoxLocations[i].X, itemBoxLocations[i].Y, itemBox.Width, itemBox.Height), Color.White);
                            if (contentBoxes[i].ButtonTexture != Game1.emptyBox)
                            {
                                //if (contents[i] is StoryItem)
                                //{
                                //    s.Draw(Game1.storyItemIcons[(contents[i] as StoryItem).Name], contentBoxes[i].ButtonRec, Color.White);
                                //}
                                //else
                                if (!(contents[i] is Textbook))
                                {
                                    contentBoxes[i].Draw(s);

                                    if (contents[i] is LootForStudentLocker)
                                    {
                                        LootForStudentLocker tmp = contents[i] as LootForStudentLocker;

                                        Game1.OutlineFont(Game1.font, s, "5", 2, contentBoxes[i].ButtonRec.X + 75 - (int)Game1.font.MeasureString("5").X, contentBoxes[i].ButtonRec.Y + 52, Color.White, Color.Black);
                                    }
                                }
                                else
                                    s.Draw(contentBoxes[i].ButtonTexture, contentBoxes[i].ButtonRec, new Rectangle(0, 0, 94, 90), Color.White);


                            }
                        }

                        String money = Math.Round(player.Money, 2).ToString("N2");
                        s.Draw(piggyRays, new Rectangle(11 + 169 / 2, 3 + 169 / 2, piggyRays.Width, piggyRays.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(piggyRays.Width / 2, piggyRays.Height / 2), SpriteEffects.None, 0f);
                        s.Draw(piggy, new Rectangle(11, 3, 169, 169), Color.White);
                        s.DrawString(Game1.TwCondensedSmallFont, "$" + money, new Vector2(91 - Game1.TwCondensedSmallFont.MeasureString("$" + money).X / 2, (int)(Game1.aspectRatio * 1280 * .11)), Color.White);


                        s.Draw(keys, new Rectangle(208, 68, keys.Width, keys.Height), Color.White);
                        s.DrawString(Game1.font, "x", new Vector2(256, 85), Color.White);
                        s.DrawString(Game1.font, player.BronzeKeys.ToString(), new Vector2(271, 86), Color.White);
                        s.DrawString(Game1.font, "x", new Vector2(359, 85), Color.White);
                        s.DrawString(Game1.font, player.SilverKeys.ToString(), new Vector2(374, 86), Color.White);
                        s.DrawString(Game1.font, "x", new Vector2(462, 85), Color.White);
                        s.DrawString(Game1.font, player.GoldKeys.ToString(), new Vector2(477, 86), Color.White);
                    }
                    break;
            }

            if (!Game1.gamePadConnected)
                s.Draw(backspace, new Vector2(1280 - backspace.Width - 5, 5), Color.White);
            else
                s.Draw(b, new Vector2(1280 - backspace.Width + 20, 5), Color.White);
        }
    }
}
