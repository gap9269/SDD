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
    public class YourLocker : BaseMenu
    {

        #region ATTRIBUTES
        // ATTRIBUTES \\
        float rayRotation;
        Texture2D rays, textbook;
        public static ContentManager Content;
        Dictionary<String,Texture2D> largeSkillIcons;
        Dictionary<String, Texture2D> mediumSkillIcons;
        //--LOCKER
        Button nextLockerPage;
        Button previousLockerPage;
        Button equipButton;
        Button unequipButton;
        Button toShop;
        List<Button> ownedSkillBoxes;

        Button skillQ;
        Button skillW;
        Button skillE;
        Button skillR;

        //--SHOP
        Button nextShopPage;
        Button previousShopPage;
        Button toLocker;
        Button buy;
        List<Skill> skillsOnSale;
        List<Button> saleIcons;
        List<Button> skillPages;

        float paulHandY = 720;
        int shopOpeningTimer;

        //--Other
        Dictionary<String, Texture2D> textures;
        Boolean goingToShop = true;
        float backgroundPosX;

        Player player;
        int page;

        Boolean blur = false;
        Skill selectedSkill = null;

        float blurAlpha = 0f;
        int timeBeforeBlur;
        int maxTimeBeforeBlur = 30;
        int timeBeforeMove;
        int maxTimeBeforeMove = 10;

        //--Enum that shows which tab is selected
        enum tabState
        {
            locker,
            shop,
            moving,
        }
        tabState state;
        #endregion

        public List<Skill> SkillsOnSale { get { return skillsOnSale; } set { skillsOnSale = value; } }

        public YourLocker(Game1 g, Player play)
            : base(Game1.whiteFilter, g)
        {

            Content = new ContentManager(g.Services);
            Content.RootDirectory = "Content";

            //--Set some base things
            page = 0;
            player = play;
            ownedSkillBoxes = new List<Button>();
            saleIcons = new List<Button>();
            skillsOnSale = new List<Skill>();
            skillPages = new List<Button>();

            backgroundPosX = -1280;
            backgroundRec = new Rectangle(-1280, 0, 2560, (int)(Game1.aspectRatio * 1280));

            //Arrow buttons
            previousLockerPage = new Button(Game1.whiteFilter, new Rectangle(803, 590, 30, 30));
            nextLockerPage = new Button(Game1.whiteFilter, new Rectangle(908, 590, 30, 30));

            toShop = new Button(new Rectangle(210, 70, 200, 50));
            toLocker = new Button(new Rectangle(975, 70, 175, 50));

            equipButton = new Button(new Rectangle(276, 560, 133, 68));
            unequipButton = new Button(new Rectangle(433, 560, 133, 68));

            buy = new Button(new Rectangle(865, 560, 85, 85));

            //--Set up the skill boxes
            skillQ = new Button(new Rectangle(1988 - 1280, 167, 77, 77));
            skillW = new Button(new Rectangle(2073 - 1280, (int)(Game1.aspectRatio * 1280 * .2f) + 25, 77, 77));
            skillE = new Button(new Rectangle(2158 - 1280, (int)(Game1.aspectRatio * 1280 * .2f) + 25, 77, 77));
            skillR = new Button(new Rectangle(2243 - 1280, (int)(Game1.aspectRatio * 1280 * .2f) + 25, 77, 77));

            //largeIcon = new Button(new Rectangle(290, (int)(Game1.aspectRatio * 1280 * .15f) - 2, 203, 203));
            //--Add the equipment boxes and skills boxes to the list of buttons

            buttons.Add(skillQ);
            buttons.Add(skillW);
            buttons.Add(skillE);
            buttons.Add(skillR);

            //--Call the method to add the inventory boxes to the inventory at the bottom
            AddInventoryBoxes();

            state = tabState.locker;

            UpdateResolution();

            textures = new Dictionary<string, Texture2D>();
            largeSkillIcons = new Dictionary<String, Texture2D>();
            mediumSkillIcons = new Dictionary<string, Texture2D>();
        }

        public void LoadContent()
        {
            rays = Content.Load<Texture2D>(@"Menus\Shop\PiggyRay");

            background = Content.Load<Texture2D>(@"Menus\Locker\lockerMenu");
            textures.Add("blurBackground", Content.Load<Texture2D>(@"Menus\Locker\lockerBlur"));
            textures.Add("lockerShop", Content.Load<Texture2D>(@"Menus\Locker\lockerShop"));
            textures.Add("lockerBookWithSkillShop", Content.Load<Texture2D>(@"Menus\Locker\lockerBook"));
            textures.Add("lockerArrowStatic", Content.Load<Texture2D>(@"Menus\Locker\lockerArrowStatic"));
            textures.Add("lockerArrowOver", Content.Load<Texture2D>(@"Menus\Locker\lockerArrow"));
            textures.Add("skillArrowStatic", Content.Load<Texture2D>(@"Menus\Locker\SkillShopArrowStatic"));
            textures.Add("skillArrowOver", Content.Load<Texture2D>(@"Menus\Locker\SkillShopArrowActive"));
            textures.Add("buyActive", Content.Load<Texture2D>(@"Menus\Locker\buyActive"));
            textures.Add("buyStatic", Content.Load<Texture2D>(@"Menus\Locker\buyStatic"));
            textures.Add("equipActive", Content.Load<Texture2D>(@"Menus\Locker\equipActive"));
            textures.Add("equipStatic", Content.Load<Texture2D>(@"Menus\Locker\equipStatic"));
            textures.Add("unequipActive", Content.Load<Texture2D>(@"Menus\Locker\unequipActive"));
            textures.Add("unequipStatic", Content.Load<Texture2D>(@"Menus\Locker\unequipStatic"));
            textures.Add("ownedSkillsLeftStatic", Content.Load<Texture2D>(@"Menus\Locker\leftArrowStatic"));
            textures.Add("ownedSkillsLeftActive", Content.Load<Texture2D>(@"Menus\Locker\leftArrowActive"));
            textures.Add("ownedSkillsRightStatic", Content.Load<Texture2D>(@"Menus\Locker\rightArrowStatic"));
            textures.Add("ownedSkillsRightActive", Content.Load<Texture2D>(@"Menus\Locker\rightArrowActive"));
            textures.Add("shopLeftStatic", Content.Load<Texture2D>(@"Menus\Locker\shopLeftArrowStatic"));
            textures.Add("shopLeftActive", Content.Load<Texture2D>(@"Menus\Locker\shopLeftArrowActive"));
            textures.Add("shopRightStatic", Content.Load<Texture2D>(@"Menus\Locker\shopRightArrowStatic"));
            textures.Add("shopRightActive", Content.Load<Texture2D>(@"Menus\Locker\shopRightArrowActive"));
            textures.Add("skillPage", Content.Load<Texture2D>(@"Menus\Locker\skillPage"));
            textures.Add("skillShopArrowActive", Content.Load<Texture2D>(@"Menus\Locker\SkillShopArrowActive"));
            textures.Add("skillShopArrowStatic", Content.Load<Texture2D>(@"Menus\Locker\SkillShopArrowStatic"));
            textures.Add("darkBackground", Content.Load<Texture2D>(@"Menus\Locker\darkBackground"));
            textures.Add("shopBox", Content.Load<Texture2D>(@"Menus\Locker\shopBox"));
            textures.Add("skillBox", Content.Load<Texture2D>(@"Menus\Locker\skillBox"));
            textbook = Content.Load<Texture2D>(@"Menus\Locker\textbook");
            //Skill Icons
            for (int i = 0; i < SkillManager.AllSkills.Count; i++)
            {
                largeSkillIcons.Add(SkillManager.AllSkills.ElementAt(i).Value.Name, Content.Load<Texture2D>(@"Menus\Locker\LargeIcons\" + SkillManager.AllSkills.ElementAt(i).Value.Name));
            }

            for (int i = 0; i < SkillManager.AllSkills.Count; i++)
            {
                mediumSkillIcons.Add(SkillManager.AllSkills.ElementAt(i).Value.Name, Content.Load<Texture2D>(@"Menus\Locker\MediumIcons\" + SkillManager.AllSkills.ElementAt(i).Value.Name));
            }
        }

        public void UnloadContent()
        {
            mediumSkillIcons.Clear();
            largeSkillIcons.Clear();
            textures.Clear();
            Content.Unload();
        }

        public void UpdateResolution()
        {
            //backgroundRec.Height = (int)(Game1.aspectRatio * 1280);

            ////Arrow buttons
            //previousLockerPage.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .85f) - 2;
            //nextLockerPage.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .85f) - 2;

            //toShop.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .1f) - 16;

            //equipButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .8f) + 13;
            //unequipButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .8f) + 13;

            //buy.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .8f) + 13;

            ////--Set up the skill boxes
            //skillQ.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2f) + 25;
            //skillW.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2f) + 25;
            //skillE.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2f) + 25;
            //skillR.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2f) + 25;

            ////--Add the player's owned-skill boxes
            //for (int i = 0; i < 9; i++)
            //{
            //    if (i < 3)
            //    {
            //        ownedSkillBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4f) + 12;
            //    }
            //    else if (i < 6)
            //    {
            //        ownedSkillBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .55f) + 4;
            //    }
            //    else
            //    {
            //        ownedSkillBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .7f) - 4;
            //    }
            //}

            ////--The skills on sale buttons

            ////--Add the player's owned-skill boxes
            //for (int i = 0; i < 9; i++)
            //{
            //    if (i < 3)
            //    {
            //        saleButtons[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .15f) - 13;
            //    }
            //    else if (i < 6)
            //    {
            //        saleButtons[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4f) + 12;
            //    }
            //    else
            //    {
            //        saleButtons[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .7f) + 1;
            //    }
            //}
        }

        public override void Update()
        {
            base.Update();

            //Rotate the ray
            rayRotation += .5f;
            if (rayRotation == 360)
                rayRotation = 0;

            switch (state)
            {
                case tabState.locker:

                    //TOOLTIP FOR FIRST TIME
                    //if (blur && blurAlpha == 1 && game.Prologue.PrologueBooleans["firstSkillLocker"] == true)
                    //{
                    //    game.Prologue.PrologueBooleans["firstSkillLocker"] = false;
                    //    Chapter.effectsManager.AddToolTip("Here you can view and equip the skills you have \npurchased. You currently own no skills. They can \nbe bought in the Skill Shop.", 625, 130);
                    //}

                    //if (blur && blurAlpha == 1 && game.Prologue.PrologueBooleans["firstSkillLockerWithSkill"] == true && player.LearnedSkills.Count > 0 && game.Prologue.PrologueBooleans["firstSkillLocker"] == false)
                    //{
                    //    Chapter.effectsManager.AddToolTip("Click a skill to see its details, then click \"Equip\" to \nequip it. Right clicking will automatically equip the skill. \nYou can only equip a skill if you have matched its level \nrequirement.", 625, 130);
                    //}

                    //if (blur && blurAlpha == 1 && game.Prologue.PrologueBooleans["firstSkillLockerWithSkill"] == true && player.LearnedSkills.Count > 0 && game.Prologue.PrologueBooleans["firstSkillLocker"] == false && player.EquippedSkills.Count > 0)
                    //{
                    //    game.Prologue.PrologueBooleans["firstSkillLockerWithSkill"] = false;
                    //    Chapter.effectsManager.AddToolTip("Your equipped skills are displayed here. You can unequip\nthem the same way you equipped them: by clicking one, \nthen \"Unequip\", or simply right clicking it.", 625, 250);
                    //}

                    //--Update the inventory boxes
                    UpdateSkillInventory();
                    RemoveSkills();

                    if (toShop.Clicked())
                    {
                        state = tabState.moving;
                        blur = false;
                        goingToShop = true;
                    }

                    //--Exit the locker
                    if (KeyPressed(Keys.Back) || MyGamePad.BPressed())
                    {
                        Chapter.effectsManager.RemoveToolTip();
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        backgroundRec.X = -1280;
                        backgroundPosX = -1280;
                        state = tabState.locker;
                        timeBeforeBlur = 0;
                        timeBeforeMove = 0;
                        blurAlpha = 0f;
                        page = 0;
                        selectedSkill = null;
                        UnloadContent();
                    }

                    //backgroundPosX = -1280;
                    break;
                case tabState.moving:

                    Chapter.effectsManager.RemoveToolTip();

                    if (timeBeforeMove < maxTimeBeforeMove)
                        timeBeforeMove++;

                    if (timeBeforeMove == maxTimeBeforeMove)
                    {
                        if (goingToShop)
                        {
                            if (backgroundPosX != 0)
                            {
                                if (backgroundPosX < 0)
                                {
                                    float distance = backgroundPosX - 300;

                                    backgroundPosX -= 1.2f * (distance / 40);

                                    if (backgroundPosX >= 0)
                                    {
                                        backgroundPosX = 0;
                                    }
                                }
                            }
                            backgroundRec.X = (int)backgroundPosX;

                            if (backgroundPosX >= 0)
                            {
                                paulHandY = 720;
                                backgroundPosX = 0;
                                state = tabState.shop;
                                timeBeforeBlur = 0;
                                timeBeforeMove = 0;
                                blurAlpha = 0f;
                                page = 0;
                                selectedSkill = null;
                            }
                        }
                        else
                        {
                            if (backgroundPosX != -1280)
                            {
                                if (backgroundPosX > -1280)
                                {
                                    float distance = backgroundPosX + 1600;

                                    backgroundPosX -= 1.2f * (distance / 40);

                                    if (backgroundPosX <= -1280)
                                    {
                                        backgroundPosX = -1280;
                                    }
                                }
                            }
                            backgroundRec.X = (int)backgroundPosX;

                            if (backgroundPosX <= -1280)
                            {
                                backgroundPosX = -1280;
                                state = tabState.locker;
                                timeBeforeBlur = 0;
                                timeBeforeMove = 0;
                                blurAlpha = 0f;
                                page = 0;
                                selectedSkill = null;
                            }
                        }
                    }
                    break;

                case tabState.shop:

                    if (paulHandY != 0 && blur && blurAlpha == 1)
                    {
                        if (paulHandY > 0)
                        {
                            float distance = paulHandY + 30;

                            paulHandY -= 3 * (distance / 40);

                            if (paulHandY <= 0)
                            {
                                paulHandY = 0;
                            }

                        }
                    }
                    else
                    {
                        ////TOOLTIP FOR FIRST TIME
                        //if (blur && blurAlpha == 1 && game.Prologue.PrologueBooleans["firstShop"] == true)
                        //{
                        //    game.Prologue.PrologueBooleans["firstShop"] = false;
                        //    Chapter.effectsManager.AddToolTip("Here you can trade Textbooks for Skills. Clicking a \nskill on the left will display it on the right, where \nyou can click \"Buy\" to purchase it.", 430, 10);
                        //}

                        if (toLocker.Clicked())
                        {
                            state = tabState.moving;
                            blur = false;
                            goingToShop = false;
                        }

                        UpdateShopInventory();

                        //--Exit the locker
                        if (KeyPressed(Keys.Back) || MyGamePad.BPressed())
                        {
                            Chapter.effectsManager.RemoveToolTip();
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            backgroundRec.X = -1280;
                            backgroundPosX = -1280;
                            state = tabState.locker;
                            timeBeforeBlur = 0;
                            timeBeforeMove = 0;
                            blurAlpha = 0f;
                            page = 0;
                            selectedSkill = null;
                            UnloadContent();
                        }
                    }
                    break;
            }

            #region Blur the screen
            if (blur == false && state != tabState.moving)
            {
                timeBeforeBlur++;

                if (timeBeforeBlur == maxTimeBeforeBlur)
                    blur = true;
            }

            if (blur == true && blurAlpha < 1)
            {
                blurAlpha += .03f;

                if (blurAlpha >= 1)
                    blurAlpha = 1;
            }
            #endregion

        }

        public void AddInventoryBoxes()
        {
            //--Add the player's owned-skill boxes
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                {
                    Button box = new Button(new Rectangle(2021 - 1280 + (i * 95), 300, 77, 77));
                    ownedSkillBoxes.Add(box);
                }
                else if (i < 6)
                {
                    Button box = new Button(new Rectangle(2021 - 1280 + ((i - 3) * 95), 395, 77, 77));
                    ownedSkillBoxes.Add(box);
                }
                else
                {
                    Button box = new Button(new Rectangle(2021 - 1280 + ((i - 6) * 95), 490, 77, 77));
                    ownedSkillBoxes.Add(box);
                }
            }

            //--The skills on sale buttons

            //--Add the skill pages and icon locations
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                {
                    Button icon = new Button(new Rectangle(253 + (i * 136), 155, 77, 77));
                    saleIcons.Add(icon);

                    Button page = new Button(new Rectangle(231 + (i * 136), 136, 110, 152));
                    skillPages.Add(page);
                }
                else if (i < 6)
                {
                    Button icon = new Button(new Rectangle(253 + ((i - 3) * 136), 340, 77, 77));
                    saleIcons.Add(icon);

                    Button page = new Button(new Rectangle(231 + ((i - 3) * 136), 320, 110, 152));
                    skillPages.Add(page);
                }
                else
                {
                    Button icon = new Button(new Rectangle(253 + ((i - 6) * 136), 515, 77, 77));
                    saleIcons.Add(icon);

                    Button page = new Button(new Rectangle(231 + ((i - 6) * 136), 498, 110, 152));
                    skillPages.Add(page);
                }
            }

        }

        public void RemoveSkills()
        {
            #region Remove the skill if you click the unequip button
            if (selectedSkill != null && unequipButton.Clicked() && selectedSkill.Equipped)
            {
                selectedSkill.Equipped = false;
                player.EquippedSkills.Remove(selectedSkill);

                if(selectedSkill.Icon == skillQ.ButtonTexture)
                    skillQ.ButtonTexture = Game1.emptyBox;

                if (selectedSkill.Icon == skillW.ButtonTexture)
                    skillW.ButtonTexture = Game1.emptyBox;

                if (selectedSkill.Icon == skillE.ButtonTexture)
                    skillE.ButtonTexture = Game1.emptyBox;

                if (selectedSkill.Icon == skillR.ButtonTexture)
                    skillR.ButtonTexture = Game1.emptyBox;

                ResetSkillBoxes();
                selectedSkill = null;
            }
            #endregion

            //--Check every button in the list
            for (int i = 0; i < buttons.Count; i++)
            {
                //--If it was clicked
                if (buttons[i].Clicked())
                {
                    #region REMOVE SKILLS
                    if (buttons[i] == skillQ && player.EquippedSkills.Count > 0)
                    {
                        selectedSkill = player.EquippedSkills[0];
                    }

                    if (buttons[i] == skillW && player.EquippedSkills.Count > 1)
                    {
                        selectedSkill = player.EquippedSkills[1];
                    }
                    if (buttons[i] == skillE && player.EquippedSkills.Count > 2)
                    {
                        selectedSkill = player.EquippedSkills[2];
                    }
                    if (buttons[i] == skillR && player.EquippedSkills.Count > 3)
                    {
                        selectedSkill = player.EquippedSkills[3];
                    }
                    #endregion
                }
                else if (buttons[i].RightClicked())
                {
                    #region REMOVE SKILLS
                    if (buttons[i] == skillQ && player.EquippedSkills.Count > 0)
                    {
                        player.EquippedSkills[0].Equipped = false;
                        player.EquippedSkills.RemoveAt(0);

                        skillQ.ButtonTexture = Game1.emptyBox;

                        ResetSkillBoxes();
                        selectedSkill = null;
                    }

                    if (buttons[i] == skillW && player.EquippedSkills.Count > 1)
                    {
                        player.EquippedSkills[1].Equipped = false;
                        player.EquippedSkills.RemoveAt(1);

                        skillW.ButtonTexture = Game1.emptyBox;

                        ResetSkillBoxes();
                        selectedSkill = null;
                    }
                    if (buttons[i] == skillE && player.EquippedSkills.Count > 2)
                    {
                        player.EquippedSkills[2].Equipped = false;
                        player.EquippedSkills.RemoveAt(2);

                        skillE.ButtonTexture = Game1.emptyBox;

                        ResetSkillBoxes();
                        selectedSkill = null;
                    }
                    if (buttons[i] == skillR && player.EquippedSkills.Count > 3)
                    {
                        player.EquippedSkills[3].Equipped = false;
                        player.EquippedSkills.RemoveAt(3);

                        skillR.ButtonTexture = Game1.emptyBox;

                        ResetSkillBoxes();
                        selectedSkill = null;
                    }
                    #endregion
                } 

            }
        }

        public void UpdateShopInventory()
        {

            #region BUYING THE SKILL
            if (buy.Clicked() && selectedSkill != null && player.Textbooks >= selectedSkill.CostToBuy)
            {
                player.Textbooks -= selectedSkill.CostToBuy;
                player.LearnedSkills.Add(selectedSkill);
                skillsOnSale.Remove(selectedSkill);
                selectedSkill = null;

                ResetInventoryBoxes();
            }
            #endregion

            #region SETTING THE SKILLS ON SALE TEXTURE, AND SETTING SELECTED SKILL
            for (int i = 0; i < saleIcons.Count; i++)
            {
                int boxNumber = i + (page * saleIcons.Count);

                if (skillsOnSale.Count > boxNumber)
                {
                    saleIcons[i].ButtonTexture = mediumSkillIcons[skillsOnSale[boxNumber].Name];

                    if (saleIcons[i].Clicked())
                    {
                        selectedSkill = skillsOnSale[boxNumber];
                    }
                }
            }
            #endregion
        }

        //--Update inventory boxes
        public void UpdateSkillInventory()
        {

            if (equipButton.Clicked() && selectedSkill != null && player.EquippedSkills.Count < 4 &&
    selectedSkill.Equipped == false && selectedSkill.LevelToUse <= player.Level)
            {
                //--Add dat skill and set it to equipped
                player.EquippedSkills.Add(selectedSkill);
                selectedSkill.Equipped = true;
                selectedSkill = null;
            }

            #region Set equipped skill textures
            for (int i = 0; i < player.EquippedSkills.Count; i++)
            {
                //--Draw the equipped skills in the player's skill slot
                switch (i)
                {
                    case 0:
                        skillQ.ButtonTexture = mediumSkillIcons[player.EquippedSkills[0].Name];
                        break;
                    case 1:
                        skillW.ButtonTexture = mediumSkillIcons[player.EquippedSkills[1].Name];
                        break;
                    case 2:
                        skillE.ButtonTexture = mediumSkillIcons[player.EquippedSkills[2].Name];
                        break;
                    case 3:
                        skillR.ButtonTexture = mediumSkillIcons[player.EquippedSkills[3].Name];
                        break;
                }
            }
            #endregion

            #region Draw And Update Skills
            //--For as many boxes there are on each page
            for (int i = 0; i < ownedSkillBoxes.Count; i++)
            {
                //Based on which tab you are currently in

                int boxNumber = i + (page * ownedSkillBoxes.Count);


                #region SKILL TAB


                //--If there are as many learned skills as the current box you are on
                //--Example : If 'i' = 2, and 'page' = 1, it would be the second box on page 1 (Which is actually the second page)
                //-- So 2 + ( 1 * 5 ) = 7. That means we're drawing the 7th skill overall, so we must make sure there are 7 learned skills
                if (player.LearnedSkills.Count > boxNumber)
                {
                    //The box on the screen is replaced with the skill we are drawing
                    //So if it is the 2nd box on page 1, that means it is the 7th skill. 2 + (1 * 2
                    ownedSkillBoxes[i].ButtonTexture = mediumSkillIcons[player.LearnedSkills[boxNumber].Name];
                    //--If the player clicks the box and doesn't have 4 skills equipped already, and the skill is currently unequipped
                    if (ownedSkillBoxes[i].Clicked())
                    {
                        selectedSkill = player.LearnedSkills[boxNumber];
                    }

                    //Right click to auto equip
                    else if (ownedSkillBoxes[i].RightClicked() && player.EquippedSkills.Count < 4 &&
   player.LearnedSkills[boxNumber].Equipped == false && player.LearnedSkills[boxNumber].LevelToUse <= player.Level)
                    {

                        //--Add dat skill and set it to equipped
                        player.EquippedSkills.Add(player.LearnedSkills[boxNumber]);
                        player.LearnedSkills[boxNumber].Equipped = true;
                        selectedSkill = null;
                    }
                }

                #endregion

            }

            #endregion

            #region Change Pages
            //--If you are not on the last page, go up a page and reset textures
            if (nextLockerPage.Clicked() && page < 4)
            {
                page++;

                ResetInventoryBoxes();
            }
            //--If you aren't on the first page, go back a page and reset textures
            if (previousLockerPage.Clicked() && page > 0)
            {
                page--;

                ResetInventoryBoxes();
            }
            #endregion

        }

        //--Reset all of the inventory box textures back to empty
        public void ResetInventoryBoxes()
        {
            for (int i = 0; i < ownedSkillBoxes.Count; i++)
            {
                ownedSkillBoxes[i].ButtonTexture = Game1.emptyBox;
            }

            for (int i = 0; i < saleIcons.Count; i++)
            {
                saleIcons[i].ButtonTexture = Game1.emptyBox;
            }
        }

        //--Reset all of the skill box textures back to empty
        public void ResetSkillBoxes()
        {

            skillQ.ButtonTexture = Game1.emptyBox;
            skillW.ButtonTexture = Game1.emptyBox;
            skillE.ButtonTexture = Game1.emptyBox;
            skillR.ButtonTexture = Game1.emptyBox;
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(textures["darkBackground"], backgroundRec, Color.White);
            if (!blur)
                s.Draw(background, backgroundRec, Color.White);

            else if (blurAlpha != 1)
            {
                s.Draw(background, backgroundRec, Color.White);
                s.Draw(textures["blurBackground"], backgroundRec, Color.White * blurAlpha);
            }

            else if (blur && blurAlpha == 1)
            {
               
                switch (state)
                {
                    #region LOCKER STATE
                    case tabState.locker:
                        s.Draw(textures["blurBackground"], backgroundRec, Color.White);
                        s.Draw(textures["lockerBookWithSkillShop"], backgroundRec, Color.White);

                        for (int i = 0; i < buttons.Count; i++)
                        {
                            if (buttons[i].ButtonTexture != Game1.emptyBox)
                                buttons[i].Draw(s);
                            else
                                s.Draw(mediumSkillIcons.ElementAt(0).Value, buttons[i].ButtonRec, Color.Black * .1f);

                        }

                        s.DrawString(Game1.font, "Page " + (page + 1).ToString(), new Vector2(844, (int)(Game1.aspectRatio * 1280 * .89f) - 6), Color.Black);

                        if(nextLockerPage.IsOver())
                            s.Draw(textures["ownedSkillsRightActive"], new Rectangle(2070 - 1280, 574, textures["ownedSkillsLeftStatic"].Width, textures["ownedSkillsLeftStatic"].Height), Color.White);
                        else
                            s.Draw(textures["ownedSkillsRightStatic"], new Rectangle(2070 - 1280, 574, textures["ownedSkillsLeftStatic"].Width, textures["ownedSkillsLeftStatic"].Height), Color.White);

                        if(previousLockerPage.IsOver())
                            s.Draw(textures["ownedSkillsLeftActive"], new Rectangle(2070 - 1280, 574, textures["ownedSkillsLeftStatic"].Width, textures["ownedSkillsLeftStatic"].Height), Color.White);
                        else
                            s.Draw(textures["ownedSkillsLeftStatic"], new Rectangle(2070 - 1280, 574, textures["ownedSkillsLeftStatic"].Width, textures["ownedSkillsLeftStatic"].Height), Color.White);

                        //--Draw the inventory boxes
                        for (int i = 0; i < ownedSkillBoxes.Count; i++)
                        {
                            int boxNumber = i + (page * ownedSkillBoxes.Count);

                            if (ownedSkillBoxes[i].ButtonTexture != Game1.emptyBox)
                            {
                                if (player.LearnedSkills[boxNumber].Equipped)
                                    s.Draw(ownedSkillBoxes[i].ButtonTexture, ownedSkillBoxes[i].ButtonRec, Color.White * .5f);
                                else
                                    s.Draw(ownedSkillBoxes[i].ButtonTexture, ownedSkillBoxes[i].ButtonRec, Color.White);
                            }
                            else
                                s.Draw(mediumSkillIcons.ElementAt(0).Value, ownedSkillBoxes[i].ButtonRec, Color.Black * .1f);
                        }

                        #region SELECTED SKILL
                        if (selectedSkill != null)
                        {
                            s.Draw(textures["skillBox"], new Vector2(1518 - 1280, 371), Color.White);
                            s.Draw(largeSkillIcons[selectedSkill.Name], new Rectangle(1604 - 1280, 130, 203, 203), Color.White);

                            if (player.Level >= selectedSkill.LevelToUse)
                                s.DrawString(Game1.skillLevelMoireFont, "LEVEL REQUIRED: " + selectedSkill.LevelToUse, new Vector2(428 - Game1.skillLevelMoireFont.MeasureString("LEVEL REQUIRED: " + selectedSkill.LevelToUse).X / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 17), Color.White);
                            else
                                s.DrawString(Game1.skillLevelMoireFont, "LEVEL REQUIRED: " + selectedSkill.LevelToUse, new Vector2(428 - Game1.skillLevelMoireFont.MeasureString("LEVEL REQUIRED: " + selectedSkill.LevelToUse).X / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 17), Color.Red);

                            s.DrawString(Game1.skillNameMoireFont, selectedSkill.Name.ToUpper(), new Vector2(1715 - 1280 - Game1.skillNameMoireFont.MeasureString(selectedSkill.Name.ToUpper()).X / 2, (int)(Game1.aspectRatio * 1280 * .475f) + 4), Color.Black);

                            s.DrawString(Game1.skillInfoImpactFont, selectedSkill.SkillRank.ToString(), new Vector2(300, (int)(Game1.aspectRatio * 1280 * .59f) - 14), Color.Black);

                            //Tell the player when they can rank up the skill if they can't yet
                            if (selectedSkill.SkillRank > 1 && Game1.Player.Level < selectedSkill.PlayerLevelsRequiredToLevel[selectedSkill.SkillRank - 1])
                            {
                                s.DrawString(Game1.twConQuestHudInfo, "(NEXT RANK AT LVL. " + selectedSkill.PlayerLevelsRequiredToLevel[selectedSkill.SkillRank - 1] + ")", new Vector2(300, (int)(Game1.aspectRatio * 1280 * .59f) - 12) + new Vector2(Game1.skillInfoImpactFont.MeasureString(selectedSkill.SkillRank.ToString()).X + 5, 0), Color.Red);
                            }

                            s.DrawString(Game1.skillInfoImpactFont, selectedSkill.Damage.ToString("N2") + "%", new Vector2(545, (int)(Game1.aspectRatio * 1280 * .59f) - 13), Color.Black);

                            s.DrawString(Game1.skillInfoImpactFont, Game1.WrapText(Game1.skillInfoImpactFont, selectedSkill.Description, 345), new Vector2(250, (int)(Game1.aspectRatio * 1280 * .59f + 10)), Color.DarkGreen);

                            //-If the skill isn't max level show the experience, otherwise write "Max level"
                            if (selectedSkill.SkillRank < 4)
                            {
                                //Tell the player when they can rank up the skill if they can't yet
                                if (selectedSkill.SkillRank > 1 && Game1.Player.Level < selectedSkill.PlayerLevelsRequiredToLevel[selectedSkill.SkillRank - 1])
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, "(NEXT RANK AT LVL. " + selectedSkill.PlayerLevelsRequiredToLevel[selectedSkill.SkillRank - 1] + ")", new Vector2(295, (int)(Game1.aspectRatio * 1280 * .69f) + 8) + new Vector2(Game1.skillInfoImpactFont.MeasureString(selectedSkill.Experience + " / " + selectedSkill.ExperienceUntilLevel).X, 0), Color.Red);

                                    s.DrawString(Game1.skillInfoImpactFont, selectedSkill.Experience + " / " + selectedSkill.ExperienceUntilLevel, new Vector2(290, (int)(Game1.aspectRatio * 1280 * .69f) + 6), Color.Black * .8f);
                                }
                                else
                                    s.DrawString(Game1.skillInfoImpactFont, selectedSkill.Experience + " / " + selectedSkill.ExperienceUntilLevel, new Vector2(290, (int)(Game1.aspectRatio * 1280 * .69f) + 6), Color.Black);
                            }
                            else
                                s.DrawString(Game1.twConQuestHudName, "MAX RANK", new Vector2(290, (int)(Game1.aspectRatio * 1280 * .69f) + 6), Color.DarkBlue);

                            if (equipButton.IsOver())
                                s.Draw(textures["equipActive"], new Rectangle(1505 - 1280, 515, textures["equipActive"].Width, textures["equipActive"].Height), Color.White);
                            else
                                s.Draw(textures["equipStatic"], new Rectangle(1505 - 1280, 515, textures["equipActive"].Width, textures["equipActive"].Height), Color.White);

                            if (unequipButton.IsOver())
                                s.Draw(textures["unequipActive"], new Rectangle(1505 - 1280, 515, textures["unequipActive"].Width, textures["equipActive"].Height), Color.White);
                            else
                                s.Draw(textures["unequipStatic"], new Rectangle(1505 - 1280, 515, textures["unequipActive"].Width, textures["equipActive"].Height), Color.White);
                        }
                        #endregion

                        if (toShop.IsOver())
                            s.Draw(textures["skillShopArrowActive"], new Rectangle(1478 - 1298, 58, textures["skillShopArrowActive"].Width, textures["skillShopArrowActive"].Height), Color.White);
                        else
                            s.Draw(textures["skillShopArrowStatic"], new Rectangle(1478 - 1298, 58, textures["skillShopArrowStatic"].Width, textures["skillShopArrowActive"].Height), Color.White);

                        break;
                    #endregion

                    #region SHOP STATE
                    case tabState.shop:

                        s.Draw(textures["blurBackground"], backgroundRec, Color.White);
                        s.Draw(textures["lockerShop"], new Rectangle(0, (int)paulHandY,textures["lockerShop"].Width, textures["lockerShop"].Height) , Color.White);

                        if (paulHandY == 0)
                        {
                            s.Draw(rays, new Rectangle(119, 120, 178, 178), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                            s.Draw(textbook, new Rectangle(0, 0, textbook.Width, textbook.Height), Color.White);

                            for (int i = 0; i < saleIcons.Count; i++)
                            {
                                if (saleIcons[i].ButtonTexture != Game1.emptyBox)
                                {
                                    s.Draw(textures["skillPage"], skillPages[i].ButtonRec, Color.White);
                                    s.Draw(saleIcons[i].ButtonTexture, saleIcons[i].ButtonRec, Color.White);
                                }
                            }



                            if (toLocker.IsOver())
                                s.Draw(textures["lockerArrowOver"], new Rectangle(950, 59, textures["lockerArrowOver"].Width, textures["lockerArrowOver"].Height), Color.White);
                            else
                                s.Draw(textures["lockerArrowStatic"], new Rectangle(950, 59, textures["lockerArrowStatic"].Width, textures["lockerArrowOver"].Height), Color.White);

                            #region Draw how many textbooks the player has
                            if (player.Textbooks == 0)
                            {
                                s.DrawString(Game1.lockerTextbookFont, "0", new Vector2(99, 88), Color.White);

                            }
                            else if (player.Textbooks < 10)
                            {
                                s.DrawString(Game1.lockerTextbookFont, "0", new Vector2(85, 88), Color.White);

                                s.DrawString(Game1.lockerTextbookFont, player.Textbooks.ToString(), new Vector2(114, 88), Color.White);
                            }
                            else
                            {
                                s.DrawString(Game1.lockerTextbookFont, player.Textbooks.ToString(), new Vector2(85, 88), Color.White);
                            }
                            #endregion

                            #region Draw skill info for selected skill
                            if (selectedSkill != null)
                            {
                                s.Draw(textures["shopBox"], new Vector2(730, 367), Color.White);
                                s.Draw(largeSkillIcons[selectedSkill.Name], new Rectangle(814, 130, 203, 203), Color.White);

                                if (player.Level >= selectedSkill.LevelToUse)
                                    s.DrawString(Game1.skillLevelMoireFont, "LEVEL REQUIRED: " + selectedSkill.LevelToUse, new Vector2(922 - Game1.skillLevelMoireFont.MeasureString("LEVEL REQUIRED: " + selectedSkill.LevelToUse).X / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 14), Color.Black);
                                else
                                    s.DrawString(Game1.skillLevelMoireFont, "LEVEL REQUIRED: " + selectedSkill.LevelToUse, new Vector2(922 - Game1.skillLevelMoireFont.MeasureString("LEVEL REQUIRED: " + selectedSkill.LevelToUse).X / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 14), Color.Red);

                                s.DrawString(Game1.skillNameMoireFont, selectedSkill.Name.ToUpper(), new Vector2(923 - Game1.skillNameMoireFont.MeasureString(selectedSkill.Name.ToUpper()).X / 2, (int)(Game1.aspectRatio * 1280 * .475f) - 2), Color.Black);

                                s.DrawString(Game1.skillInfoImpactFont, selectedSkill.SkillRank.ToString(), new Vector2(793, (int)(Game1.aspectRatio * 1280 * .59f) - 18), Color.Black);

                                s.DrawString(Game1.skillInfoImpactFont, selectedSkill.Damage.ToString("N2") + "%", new Vector2(1037, (int)(Game1.aspectRatio * 1280 * .59f) - 20), Color.Black);

                                s.DrawString(Game1.skillInfoImpactFont, Game1.WrapText(Game1.skillInfoImpactFont, selectedSkill.Description, 345), new Vector2(745, (int)(Game1.aspectRatio * 1280 * .6f)), Color.DarkGreen);

                                s.DrawString(Game1.questNameFont, "Cost: " + selectedSkill.CostToBuy.ToString(), new Vector2(740, (int)(Game1.aspectRatio * 1280 * .74f) - 2), Color.White);

                                if (player.Textbooks >= selectedSkill.CostToBuy)
                                {
                                    if (buy.IsOver())
                                        s.Draw(textures["buyActive"], new Rectangle(815, 505, textures["buyActive"].Width, textures["buyActive"].Height), Color.White);
                                    else
                                        s.Draw(textures["buyStatic"], new Rectangle(815, 505, textures["buyActive"].Width, textures["buyActive"].Height), Color.White);
                                }
                                else
                                    s.Draw(textures["buyStatic"], new Rectangle(815, 505, textures["buyActive"].Width, textures["buyActive"].Height), Color.Gray * .5f);


                            }
                            #endregion

                            for (int i = 0; i < saleIcons.Count; i++)
                            {
                                int boxNumber = i + (page * saleIcons.Count);

                                if (skillsOnSale.Count > boxNumber)
                                {
                                    s.DrawString(Game1.lockerCostFont, "Cost: " + skillsOnSale[i].CostToBuy, new Vector2(saleIcons[i].ButtonRecX + saleIcons[i].ButtonRecWidth / 2 - Game1.lockerCostFont.MeasureString("Cost: " + skillsOnSale[i].CostToBuy).X / 2, saleIcons[i].ButtonRecY + saleIcons[i].ButtonRecHeight + 5), Color.Purple);
                                }
                            }
                        }
                        break;

                    #endregion
                }

                s.Draw(Game1.backspaceTexture, new Rectangle(1126, 16, Game1.backspaceTexture.Width, Game1.backspaceTexture.Height), Color.White);
            }
        }
    }
}
