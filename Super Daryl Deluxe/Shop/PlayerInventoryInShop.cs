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

    public class PlayerInventoryInShop
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

        public Object selectedItem;
        public int selectedItemIndex;

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
        Button shirtTab;
        Button hatTab;
        Button accessoryTab;
        Button lootTab;
        Button itemsTab;
        Game1 game;
        List<Button> tabButtons;

        //--Properties

        public int StoryItemPage { get { return storyItemPage; } set { storyItemPage = value; } }
        public int EquipmentPage { get { return equipmentPage; } set { equipmentPage = value; } }

        //-- CONTSTRUCTOR --\\
        public PlayerInventoryInShop(Player play, Dictionary<String, Texture2D> texts,
            DescriptionBoxManager d, Game1 g)
        {
            buttons = new List<Button>();
            player = play;
            textures = texts;
            game = g;

            #region Tab Buttons
            tabButtons = new List<Button>();
            weaponTab = new Button(new Rectangle(128, 194, 62, 52));
            hatTab = new Button(new Rectangle(232, 201, 61, 37));
            shirtTab = new Button(new Rectangle(334, 199, 63, 50));
            accessoryTab = new Button(new Rectangle(445, 211, 71, 34));
            lootTab = new Button(new Rectangle(556, 205, 49, 52));

            tabButtons.Add(weaponTab);
            tabButtons.Add(shirtTab);
            tabButtons.Add(hatTab);
            tabButtons.Add(lootTab);
            tabButtons.Add(accessoryTab);
            #endregion

            tabState = TabState.weapon;


            //--Set some base things
            equipmentPage = 0;
            inventoryBoxes = new List<Button>();
            storyItemBoxes = new List<Button>();
            descriptionBoxManager = d;
            equipBox = Game1.emptyBox;

            //Arrow buttons
            nextEquipmentPage = new Button(Game1.whiteFilter, new Rectangle(406, 510, 35, 29));
            previousEquipmentPage = new Button(Game1.whiteFilter, new Rectangle(289, 514, 40, 28));

            AddInventoryBoxes();
            //UpdateResolution();
        }

        public void UpdateResolution()
        {

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
            for (int i = 0; i < 15; i++)
            {
                if (i < 5)
                {
                    Button box = new Button(equipBox, new Rectangle(166 + (i * 81), 273,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
                else if (i < 10)
                {
                    Button box = new Button(equipBox, new Rectangle(166 + ((i - 5) * 81), 354,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
                else
                {
                    Button box = new Button(equipBox, new Rectangle(166 + ((i - 10) * 81), 435, //370,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
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
                    else if (tab == shirtTab)
                    {
                        tabState = TabState.shirts;
                    }
                    else if (tab == hatTab)
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

                            if (inventoryBoxes[i].Clicked())
                            {
                                selectedItem = player.OwnedWeapons[boxNumber];
                                selectedItemIndex = boxNumber;
                            }

                        }
                        break;

                    case TabState.hats:

                        if (player.OwnedHats.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHats[boxNumber].Icon;

                            if (inventoryBoxes[i].Clicked())
                            {
                                selectedItem = player.OwnedHats[boxNumber];
                                selectedItemIndex = boxNumber;
                            }
                        } 
                        break;

                    case TabState.shirts:
                        if (player.OwnedHoodies.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHoodies[boxNumber].Icon;

                            if (inventoryBoxes[i].Clicked())
                            {
                                selectedItem = player.OwnedHoodies[boxNumber];
                                selectedItemIndex = boxNumber;
                            }
                        }
                        break;

                    case TabState.accessory:
                        if (player.OwnedAccessories.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedAccessories[boxNumber].Icon;

                            if (inventoryBoxes[i].Clicked())
                            {
                                selectedItem = player.OwnedAccessories[boxNumber];
                                selectedItemIndex = boxNumber;
                            }
                        }
                        break;

                    case TabState.loot:
                        if (player.EnemyDrops.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = EnemyDrop.allDrops[player.EnemyDrops.ElementAt(boxNumber).Key].texture;

                            if (inventoryBoxes[i].Clicked())
                            {
                                selectedItem = EnemyDrop.allDrops[player.EnemyDrops.ElementAt(boxNumber).Key];
                                selectedItemIndex = boxNumber;
                            }
                        }
                        break;

                    #region ITEMS TAB
                    case TabState.items:
                        if (player.CraftingAndCatalysts.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = EnemyDrop.allDrops[player.CraftingAndCatalysts.ElementAt(boxNumber).Key].texture;
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
        }

        //--Reset all of the inventory box textures back to empty
        public void ResetInventoryBoxes()
        {
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                inventoryBoxes[i].ButtonTexture = equipBox;
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["Background"], new Rectangle(96, 173, textures["Background"].Width, textures["Background"].Height), Color.White);

            #region Draw the tab lines
            switch (tabState)
            {
                case TabState.weapon:
                    s.Draw(textures["WeaponsPage"], new Rectangle(99, 179, textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.shirts:
                    s.Draw(textures["ShirtsPage"], new Rectangle(99, 179, textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.loot:
                    s.Draw(textures["LootPage"], new Rectangle(99, 179, textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.hats:
                    s.Draw(textures["HatsPage"], new Rectangle(99, 179, textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.accessory:
                    s.Draw(textures["AccessoriesPage"], new Rectangle(99, 179, textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
            }
            #endregion

            #region Draw tab icons for inventory
            if (weaponTab.IsOver())
                s.Draw(textures["WeaponTabActive"], new Rectangle(118, 173, 503, 99), Color.White);
            else
                s.Draw(textures["WeaponTabStatic"], new Rectangle(118, 173, 503, 99), Color.White);

            if (hatTab.IsOver())
                s.Draw(textures["HatsTabActive"], new Rectangle(118, 173, 503, 99), Color.White);
            else
                s.Draw(textures["HatsTabStatic"], new Rectangle(118, 173, 503, 99), Color.White);

            if (shirtTab.IsOver())
                s.Draw(textures["ShirtsTabActive"], new Rectangle(118, 173, 503, 99), Color.White);
            else
                s.Draw(textures["ShirtsTabStatic"], new Rectangle(118, 173, 503, 99), Color.White);

            if (accessoryTab.IsOver())
                s.Draw(textures["AccessoryTabActive"], new Rectangle(118, 173, 503, 99), Color.White);
            else
                s.Draw(textures["AccessoryTabStatic"], new Rectangle(118, 173, 503, 99), Color.White);

            if (lootTab.IsOver())
                s.Draw(textures["LootTabActive"], new Rectangle(118, 173, 503, 99), Color.White);
            else
                s.Draw(textures["LootTabStatic"], new Rectangle(118, 173, 503, 99), Color.White);

            if (game.Notebook.Inventory.newWeapon)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(748 - 576, 40 + 122, 45, 45), Color.White);
            if (game.Notebook.Inventory.newHat)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(849 - 576, 42 + 122, 45, 45), Color.White);
            if (game.Notebook.Inventory.newShirt)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(960 - 576, 42 + 122, 45, 45), Color.White);
            if (game.Notebook.Inventory.newAccessory)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(1071 - 576, 44 + 122, 45, 45), Color.White);
            if (game.Notebook.Inventory.newLoot)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(1170 - 576, 54 + 122, 45, 45), Color.White);

            #endregion

            #region Depending on the page, draw the page arrows
            //ITEMS
            s.DrawString(Game1.font, "pg " + (equipmentPage + 1).ToString() + "/5", new Vector2(337, 510), Color.Black);

            if (nextEquipmentPage.IsOver() && equipmentPage < 4)
                s.Draw(textures["ItemRightActive"], new Rectangle(274, 506, textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
            else
                s.Draw(textures["ItemRight"], new Rectangle(274, 506, textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);

            if (previousEquipmentPage.IsOver() && equipmentPage > 0)
                s.Draw(textures["ItemLeftActive"], new Rectangle(274, 506, textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
            else
                s.Draw(textures["ItemLeft"], new Rectangle(274, 506, textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
            #endregion

            //--Draw the inventory boxes
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                if (inventoryBoxes[i].ButtonTexture == Game1.emptyBox)
                    s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.White * 0f);
                else
                {
                    if (tabState == TabState.loot)
                        s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.White);
                    else
                    {
                        int boxNumber = i + (equipmentPage * inventoryBoxes.Count);

                        switch (tabState)
                        {
                            case TabState.weapon:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedWeapons[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedWeapons[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.shirts:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedHoodies[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedHoodies[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.hats:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedHats[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedHats[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.accessory:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedAccessories[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedAccessories[boxNumber].Name].Height), Color.White);
                                break;
                        }
                    }

                    if (tabState == TabState.loot)
                    {
                        s.DrawString(Game1.descriptionFont, player.EnemyDrops.ElementAt(i).Value.ToString(),
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
                }
            }
        }
    }
}