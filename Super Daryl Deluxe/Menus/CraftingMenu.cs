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


//--UNFINISHED, WILL RETURN TO THIS FEATURE IN THE FUTURE IF WE DECIDE TO ADD IT TO THE GAME

namespace ISurvived
{
    /*
    public class PlayerInventoryForCrafting
    {
        Player player;
        KeyboardState current;
        KeyboardState last;
        Dictionary<String, Texture2D> textures;

        Button nextEquipmentPage;
        Button previousEquipmentPage;
        List<Button> buttons;

        //--Other
        Texture2D equipBox;
        List<Button> inventoryBoxes;
        List<Button> storyItemBoxes;
        int equipmentPage;
        int storyItemPage;
        DescriptionBoxManager descriptionBoxManager;

        public Equipment selectedEquip;
        public int selectedEquipIndex;

        public DropType selectedCraftingItem;
        public int selectedCraftingItemIndex;

        public DropType selectedCatalyst;
        public int selectedCatalystIndex;

        public enum TabState
        {
            weapon,
            hats,
            shirts,
            accessory,
            loot,
            items
        }
        public TabState tabState;

        Button weaponTab;
        Button toleranceTab;
        Button motivationTab;
        Button accessoryTab;
        Button lootTab;
        Button itemsTab;
        Game1 game;
        List<Button> tabButtons;

        //--Properties

        public int StoryItemPage { get { return storyItemPage; } set { storyItemPage = value; } }
        public int EquipmentPage { get { return equipmentPage; } set { equipmentPage = value; } }

        //-- CONTSTRUCTOR --\\
        public PlayerInventoryForCrafting(Player play, Dictionary<String, Texture2D> texts,
            DescriptionBoxManager d, Game1 g)
        {
            buttons = new List<Button>();
            player = play;
            textures = texts;
            game = g;

            #region Tab Buttons
            tabButtons = new List<Button>();
            weaponTab = new Button(Game1.whiteFilter, new Rectangle(40, 115, 90, 75));
            motivationTab = new Button(Game1.whiteFilter, new Rectangle(135, 140, 25, 50));
            toleranceTab = new Button(Game1.whiteFilter, new Rectangle(170, 140, 25, 50));
            accessoryTab = new Button(Game1.whiteFilter, new Rectangle(205, 140, 30, 50));
            itemsTab = new Button(Game1.whiteFilter, new Rectangle(250, 140, 25, 50));
            lootTab = new Button(Game1.whiteFilter, new Rectangle(282, 140, 20, 50));

            tabButtons.Add(weaponTab);
            tabButtons.Add(toleranceTab);
            tabButtons.Add(motivationTab);
            tabButtons.Add(lootTab);
            tabButtons.Add(accessoryTab);
            tabButtons.Add(itemsTab);
            #endregion

            tabState = TabState.weapon;


            //--Set some base things
            equipmentPage = 0;
            inventoryBoxes = new List<Button>();
            storyItemBoxes = new List<Button>();
            descriptionBoxManager = d;
            equipBox = Game1.emptyBox;

            //Arrow buttons
            nextEquipmentPage = new Button(Game1.rightArrow, new Rectangle(285, 295, 15, 50));
            previousEquipmentPage = new Button(Game1.leftArrow, new Rectangle(45, 295, 15, 55));

            AddInventoryBoxes();
            UpdateResolution();
        }

        public void UpdateResolution()
        {
            UpdateTabButtons();


            nextEquipmentPage.ButtonRecY = (int)(Game1.aspectRatio * 1280) / 3 + 55;//295
            previousEquipmentPage.ButtonRecY = (int)(Game1.aspectRatio * 1280) / 3 + 55; //295

            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .3) + 4;
                }
                else if (i < 6)
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 - 65;
                }
                else
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.aspectRatio * 1280) / 2 + 10;
                }
            }
        }

        public void AddInventoryBoxes()
        {
            //--Add the boxes to the list after creating them
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                {
                    Button box = new Button(equipBox, new Rectangle(65 + (i * 73), (int)(Game1.aspectRatio * 1280 * .3) + 4,
                        equipBox.Width, equipBox.Height));
                    inventoryBoxes.Add(box);
                }
                else if (i < 6)
                {
                    Button box = new Button(equipBox, new Rectangle(65 + ((i - 3) * 73), (int)(Game1.aspectRatio * 1280) / 2 - 65,
                        equipBox.Width, equipBox.Height));
                    inventoryBoxes.Add(box);
                }
                else
                {
                    Button box = new Button(equipBox, new Rectangle(65 + ((i - 6) * 73), (int)(Game1.aspectRatio * 1280) / 2 + 10, //370,
                        equipBox.Width, equipBox.Height));
                    inventoryBoxes.Add(box);
                }
            }

            //--Story item boxes
            for (int i = 0; i < 3; i++)
            {
                Button box = new Button(equipBox, new Rectangle(67 + (i * 73), 540,
                        equipBox.Width, equipBox.Height));

                storyItemBoxes.Add(box);
            }
        }

        public void UpdateTabButtons()
        {
            switch (tabState)
            {
                case TabState.weapon:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 65, 90, 75);
                    motivationTab.ButtonRec = new Rectangle(135, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    toleranceTab.ButtonRec = new Rectangle(170, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    accessoryTab.ButtonRec = new Rectangle(205, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    itemsTab.ButtonRec = new Rectangle(250, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    lootTab.ButtonRec = new Rectangle(282, (int)(Game1.aspectRatio * 1280) / 4 - 40, 20, 50);
                    break;

                case TabState.hats:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    motivationTab.ButtonRec = new Rectangle(80, (int)(Game1.aspectRatio * 1280) / 4 - 65, 80, 75);
                    toleranceTab.ButtonRec = new Rectangle(170, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    accessoryTab.ButtonRec = new Rectangle(205, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    itemsTab.ButtonRec = new Rectangle(250, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    lootTab.ButtonRec = new Rectangle(282, (int)(Game1.aspectRatio * 1280) / 4 - 40, 20, 50);
                    break;

                case TabState.shirts:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    motivationTab.ButtonRec = new Rectangle(80, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    toleranceTab.ButtonRec = new Rectangle(115, (int)(Game1.aspectRatio * 1280) / 4 - 65, 80, 75);
                    accessoryTab.ButtonRec = new Rectangle(205, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    itemsTab.ButtonRec = new Rectangle(250, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    lootTab.ButtonRec = new Rectangle(282, (int)(Game1.aspectRatio * 1280) / 4 - 40, 20, 50);
                    break;

                case TabState.accessory:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    motivationTab.ButtonRec = new Rectangle(80, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    toleranceTab.ButtonRec = new Rectangle(115, (int)(Game1.aspectRatio * 1280) / 4 - 40, 35, 50);
                    accessoryTab.ButtonRec = new Rectangle(160, (int)(Game1.aspectRatio * 1280) / 4 - 65, 80, 75);
                    itemsTab.ButtonRec = new Rectangle(250, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    lootTab.ButtonRec = new Rectangle(282, (int)(Game1.aspectRatio * 1280) / 4 - 40, 20, 50);
                    break;

                case TabState.items:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    motivationTab.ButtonRec = new Rectangle(80, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    toleranceTab.ButtonRec = new Rectangle(115, (int)(Game1.aspectRatio * 1280) / 4 - 40, 80, 50);
                    accessoryTab.ButtonRec = new Rectangle(160, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    itemsTab.ButtonRec = new Rectangle(200, (int)(Game1.aspectRatio * 1280) / 4 - 65, 75, 75);
                    lootTab.ButtonRec = new Rectangle(282, (int)(Game1.aspectRatio * 1280) / 4 - 40, 20, 50);
                    break;

                case TabState.loot:
                    weaponTab.ButtonRec = new Rectangle(40, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    motivationTab.ButtonRec = new Rectangle(80, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    toleranceTab.ButtonRec = new Rectangle(115, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    accessoryTab.ButtonRec = new Rectangle(160, (int)(Game1.aspectRatio * 1280) / 4 - 40, 30, 50);
                    itemsTab.ButtonRec = new Rectangle(195, (int)(Game1.aspectRatio * 1280) / 4 - 40, 25, 50);
                    lootTab.ButtonRec = new Rectangle(235, (int)(Game1.aspectRatio * 1280) / 4 - 65, 80, 75);
                    break;
            }
        }

        public void Update()
        {
            //UpdateResolution();
            last = current;
            current = Keyboard.GetState();

            UpdateInventory();
            ChangeTab();

        }

        //--Changes between the inventory tabs
        public void ChangeTab()
        {

            for (int i = 0; i < tabButtons.Count; i++)
            {
                if (tabButtons[i].Clicked())
                {
                    Button tab = tabButtons[i];
                    ResetInventoryBoxes();
                    equipmentPage = 0;
                    if (tab == weaponTab)
                    {
                        tabState = TabState.weapon;
                    }
                    else if (tab == toleranceTab)
                    {
                        tabState = TabState.shirts;
                    }
                    else if (tab == motivationTab)
                    {
                        tabState = TabState.hats;
                    }
                    else if (tab == accessoryTab)
                    {
                        tabState = TabState.accessory;
                    }
                    else if (tab == lootTab)
                    {
                        tabState = TabState.loot;
                    }
                    else if (tab == itemsTab)
                    {
                        tabState = TabState.items;
                    }
                    UpdateTabButtons();
                }
            }
        }

        //--Update inventory boxes
        public void UpdateInventory()
        {

            #region Draw And Update Inventory Items
            //--For as many boxes there are on each page
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                int boxNumber = i + (equipmentPage * inventoryBoxes.Count);

                switch (tabState)
                {

                    case TabState.weapon:
                        if (player.OwnedWeapons.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedWeapons[boxNumber].Icon;

                            if (inventoryBoxes[i].DoubleClicked())
                            {
                                selectedEquip = player.OwnedWeapons[boxNumber];
                                selectedEquipIndex = boxNumber;
                            }

                        }
                        break;

                    case TabState.hats:

                        if (player.OwnedHats.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHats[boxNumber].Icon;

                            if (inventoryBoxes[i].DoubleClicked())
                            {
                                selectedEquip = player.OwnedHats[boxNumber];
                                selectedEquipIndex = boxNumber;
                            }
                        } 
                        break;

                    case TabState.shirts:
                        if (player.OwnedHoodies.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHoodies[boxNumber].Icon;

                            if (inventoryBoxes[i].DoubleClicked())
                            {
                                selectedEquip = player.OwnedHoodies[boxNumber];
                                selectedEquipIndex = boxNumber;
                            }
                        }
                        break;

                    case TabState.accessory:
                        if (player.OwnedAccessories.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedAccessories[boxNumber].Icon;

                            if (inventoryBoxes[i].DoubleClicked())
                            {
                                selectedEquip = player.OwnedAccessories[boxNumber];
                                selectedEquipIndex = boxNumber;
                            }
                        }
                        break;

                    case TabState.loot:
                        if (player.EnemyDrops.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = EnemyDrop.allDrops[player.EnemyDrops.ElementAt(boxNumber).Key].texture;
                        }
                        break;

                    #region ITEMS TAB
                    case TabState.items:
                        if (player.CraftingAndCatalysts.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key].texture;

                            if (inventoryBoxes[i].DoubleClicked())
                            {
                                if (EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key].type == "Craft")
                                {
                                    selectedCraftingItem = EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key];
                                    selectedCraftingItemIndex = boxNumber;
                                }
                                else if (EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key].type == "Catalyst")
                                {
                                    selectedCatalyst = EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key];
                                    selectedCatalystIndex = boxNumber;
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
            #endregion

            #region Change Pages

            //--If you are not on the last page, go up a page and reset textures
            if (nextEquipmentPage.Clicked() && equipmentPage < 4)
            {
                equipmentPage++;

                ResetInventoryBoxes();
            }
            //--If you aren't on the first page, go back a page and reset textures
            if (previousEquipmentPage.Clicked() && equipmentPage > 0)
            {
                equipmentPage--;

                ResetInventoryBoxes();
            }
            #endregion

            #region Story Items

            for (int i = 0; i < storyItemBoxes.Count; i++)
            {
                int boxNumber = i + (storyItemPage * storyItemBoxes.Count);

                if (player.StoryItems.Count > boxNumber)
                    storyItemBoxes[i].ButtonTexture = Game1.storyItemIcons[player.StoryItems.ElementAt(boxNumber).Key];
            }

            #endregion
        }

        //--Reset all of the inventory box textures back to empty
        public void ResetInventoryBoxes()
        {
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                inventoryBoxes[i].ButtonTexture = equipBox;
            }

            for (int i = 0; i < storyItemBoxes.Count; i++)
            {
                storyItemBoxes[i].ButtonTexture = equipBox;
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["Background"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);

            #region Draw the hover-over-tab light
            if (!game.Prologue.PrologueBooleans["firstTimeOpened"])
            {
                for (int i = 0; i < tabButtons.Count; i++)
                {
                    if (tabButtons[i].IsOver() && tabButtons[i].ButtonRecWidth < 40)
                    {
                        s.Draw(textures["ActiveTabLight"], new Rectangle(tabButtons[i].ButtonRec.Center.X - textures["ActiveTabLight"].Width / 2,
                            tabButtons[i].ButtonRec.Center.Y - textures["ActiveTabLight"].Height / 2, textures["ActiveTabLight"].Width,
                            textures["ActiveTabLight"].Height), Color.White);
                    }
                }
            }
            #endregion

            #region Draw the tabs
            switch (tabState)
            {
                case TabState.weapon:
                    s.Draw(textures["WeaponsPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
                case TabState.shirts:
                    s.Draw(textures["ShirtsPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
                case TabState.loot:
                    s.Draw(textures["LootPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
                case TabState.items:
                    s.Draw(textures["ItemsPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
                case TabState.hats:
                    s.Draw(textures["HatsPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
                case TabState.accessory:
                    s.Draw(textures["AccessoriesPage"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    break;
            }
            #endregion

            #region Depending on the page, draw the page arrows

            if (game.Prologue.PrologueBooleans["firstTimeOpened"] == false)
            {
                s.DrawString(Game1.font, (equipmentPage + 1).ToString(), new Vector2(270, (int)(Game1.aspectRatio * 1280 * .6) + 12), Color.Black);
                if (equipmentPage < 4)
                {
                    if (nextEquipmentPage.IsOver())
                        s.Draw(textures["ItemRightActive"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    else
                        s.Draw(textures["ItemRight"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                }
                if (equipmentPage > 0)
                {
                    if (previousEquipmentPage.IsOver())
                        s.Draw(textures["ItemLeftActive"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                    else
                        s.Draw(textures["ItemLeft"], new Rectangle(-500, 0, 1080, (int)(Game1.aspectRatio * 1280)), Color.White);
                }

                s.DrawString(Game1.font, (storyItemPage + 1).ToString(), new Vector2(270, (int)(Game1.aspectRatio * 1280 * .85) - 12), Color.Black);

            }

            #endregion

            for (int i = 0; i < storyItemBoxes.Count; i++)
            {
                if (storyItemBoxes[i].ButtonTexture == Game1.emptyBox)
                    s.Draw(storyItemBoxes[i].ButtonTexture, storyItemBoxes[i].ButtonRec, Color.White * 0f);
                else
                {
                    if (i >= player.StoryItems.Count)
                    {
                        storyItemBoxes[i].ButtonTexture = Game1.emptyBox;
                        continue;
                    }
                    s.Draw(storyItemBoxes[i].ButtonTexture, storyItemBoxes[i].ButtonRec, Color.White);

                    s.DrawString(Game1.descriptionFont, player.StoryItems.ElementAt(i).Value.ToString(),
                        new Vector2(storyItemBoxes[i].ButtonRec.X + 7, storyItemBoxes[i].ButtonRec.Y + 5), Color.DarkRed);
                }
            }


            //--Draw the inventory boxes
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                if (inventoryBoxes[i].ButtonTexture == Game1.emptyBox)
                    s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.White * 0f);
                else
                {
                    if ((((tabState == TabState.weapon && selectedEquip is Weapon) || (tabState == TabState.shirts && selectedEquip is Hoodie) || (tabState == TabState.hats && selectedEquip is Hat) || (tabState == TabState.accessory && selectedEquip is Accessory)) && i == selectedEquipIndex && selectedEquip != null) || (tabState == TabState.items && ((i == selectedCatalystIndex && selectedCatalyst != null ) || (i == selectedCraftingItemIndex && selectedCraftingItem != null))))
                        s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.Gray);
                    else
                        s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.White);

                    if (tabState == TabState.loot)
                    {
                        s.DrawString(Game1.descriptionFont, player.EnemyDrops.ElementAt(i).Value.ToString(),
                            new Vector2(inventoryBoxes[i].ButtonRec.X + 7, inventoryBoxes[i].ButtonRec.Y + 5), Color.DarkRed);
                    }
                    else if (tabState == TabState.items)
                    {
                        s.DrawString(Game1.descriptionFont, player.CraftingAndCatalysts.ElementAt(i).Value.ToString(),
                            new Vector2(inventoryBoxes[i].ButtonRec.X + 7, inventoryBoxes[i].ButtonRec.Y + 5), Color.DarkRed);
                    }
                }
            }



            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                //--Draw description Boxes
                int boxNumber = i + (equipmentPage * inventoryBoxes.Count);
                if (inventoryBoxes[i].IsOver())
                {

                    #region WEAPONS
                    if (tabState == TabState.weapon)
                    {
                        if (player.OwnedWeapons.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawEquipDescriptions(player.OwnedWeapons[boxNumber], inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                    #region MOTIVATION
                    if (tabState == TabState.hats)
                    {
                        if (player.OwnedHats.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawEquipDescriptions(player.OwnedHats[boxNumber], inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                    #region TOLERANCE
                    if (tabState == TabState.shirts)
                    {
                        if (player.OwnedHoodies.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawEquipDescriptions(player.OwnedHoodies[boxNumber], inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                    #region ACCESSORIES
                    if (tabState == TabState.accessory)
                    {
                        if (player.OwnedAccessories.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawEquipDescriptions(player.OwnedAccessories[boxNumber], inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                    #region LOOT
                    if (tabState == TabState.loot)
                    {
                        if (player.EnemyDrops.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawLootDescriptions(player.EnemyDrops.ElementAt(boxNumber).Key, inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                    #region CRAFTING ITEMS
                    if (tabState == TabState.items)
                    {
                        if (player.CraftingAndCatalysts.Count > boxNumber)
                        {
                            descriptionBoxManager.DrawLootDescriptions(player.CraftingAndCatalysts.ElementAt(boxNumber).Key, inventoryBoxes[i].ButtonRec);
                        }
                    }
                    #endregion

                }
            }

            for (int i = 0; i < 3; i++)
            {
                int boxNumber = i + (storyItemPage * 3);
                if (storyItemBoxes[i].IsOver())
                {
                    if (player.StoryItems.Count > boxNumber)
                    {
                        descriptionBoxManager.DrawStoryItemDescriptions(player.StoryItems.ElementAt(boxNumber).Key, storyItemBoxes[i].ButtonRec);
                    }
                }
            }
            
        }
    }

    public class CraftingMenu : BaseMenu
    {

        Texture2D bigCraft, smallCraft, outerBar, innerBar, successRateTexture, successTexture, failureTexture;
        Button craftButton, equipButton, catalystButton, craftingItemButton;
        int craftTimer, successRate;
        Equipment equipToUpgrade;
        Catalyst catalyst;
        CraftingItem craftingItem;
        Boolean canCraft = false;
        PlayerInventoryForCrafting playerInventory;

        enum craftState
        {
            waitingItems,
            crafting,
            success,
            failure
        }
        craftState state;

        public CraftingMenu(Texture2D tex, Texture2D big, Texture2D small, Texture2D outer, Texture2D inner, Texture2D rate, Texture2D suc, Texture2D fail, Game1 g, PlayerInventoryForCrafting inven)
            : base(tex, g)
        {
            bigCraft = big;
            smallCraft = small;
            innerBar = inner;
            outerBar = outer;
            successRateTexture = rate;
            successTexture = suc;
            failureTexture = fail;
            playerInventory = inven;

            craftButton = new Button(new Rectangle(890, 515, 210, 60));
            equipButton = new Button(new Rectangle(773, 189, Game1.whiteFilter.Width, Game1.whiteFilter.Height));
            catalystButton = new Button(new Rectangle(1161, 189, Game1.whiteFilter.Width, Game1.whiteFilter.Height));
            craftingItemButton = new Button(new Rectangle(893, 189, Game1.whiteFilter.Width, Game1.whiteFilter.Height));

            UpdateResolution();
        }

        public void UpdateResolution()
        {
            craftButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .7f + 12);
            equipButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .25f + 9);
            catalystButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .25f + 9);
            craftingItemButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .25f + 9);
        }

        public override void Update()
        {
            base.Update();

            if (previous.IsKeyDown(Keys.Back) && current.IsKeyUp(Keys.Back))
            {
                playerInventory.selectedEquip = null;
                playerInventory.selectedCatalyst = null;
                playerInventory.selectedCraftingItem = null;

                state = craftState.waitingItems;

                game.CurrentChapter.state = Chapter.GameState.Game;
            }

            switch (state)
            {
                case craftState.waitingItems:
                    playerInventory.Update();

                    if (playerInventory.selectedEquip != null && playerInventory.selectedCraftingItem != null)
                        canCraft = true;
                    else
                        canCraft = false;

                    if (equipButton.DoubleClicked() && playerInventory.selectedEquip != null)
                        playerInventory.selectedEquip = null;

                    if (catalystButton.DoubleClicked() && playerInventory.selectedCatalyst != null)
                        playerInventory.selectedCatalyst = null;

                    if (craftingItemButton.DoubleClicked() && playerInventory.selectedCraftingItem != null)
                        playerInventory.selectedCraftingItem = null;

                    break;
                   
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(background, backgroundRec, Color.White);
            playerInventory.Draw(s);

            if(playerInventory.selectedEquip != null)
                s.Draw(playerInventory.selectedEquip.Icon, equipButton.ButtonRec, Color.White);

            if (playerInventory.selectedCraftingItem != null)
                s.Draw(playerInventory.selectedCraftingItem.texture, craftingItemButton.ButtonRec, Color.White);

            switch (state)
            {
                case craftState.waitingItems:
                    if (canCraft)
                    {
                        if (craftButton.IsOver())
                            s.Draw(smallCraft, backgroundRec, Color.White);
                        else
                            s.Draw(bigCraft, backgroundRec, Color.White);
                    }
                    else
                    {
                            s.Draw(smallCraft, backgroundRec, Color.White * .5f);
                    }

                    s.Draw(successRateTexture, backgroundRec, Color.White);
                    s.Draw(outerBar, backgroundRec, Color.White);

                    break;
            }
        }
    }*/
}
