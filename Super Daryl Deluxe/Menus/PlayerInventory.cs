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
    public class PlayerInventory
    {
        Player player;
        KeyboardState current;
        KeyboardState last;
        Boolean canEquip;
        Dictionary<String, Texture2D> textures;
        Dictionary<String, Texture2D> darylDrawings;
        public Dictionary<String, Texture2D> passiveBoxes;

        Button passiveTab;
        int passivePage;

        Button passiveBack;

        Button nextEquipmentPage;
        Button previousEquipmentPage;
        Button nextStoryPage;
        Button previousStoryPage;
        Button motivationEquip;
        Button toleranceEquip;
        Button weaponEquip2;
        Button weaponEquip1;
        Button miscEquip1;
        Button miscEquip2;
        List<Button> buttons;
        List<Vector2> passivePositions;

        int timeOverWeaponIcon, timeOverHatIcon, timeOverShirtIcon, timeOverAccessoryIcon, timeOverLootIcon;

        Button healthIcon, defenseIcon, strengthIcon, moneyIcon, experienceIcon, karmaIcon, bronzeKeyIcon, silverKeyIcon, goldKeyIcon, textbookIcon;

        public Boolean newWeapon, newHat, newShirt, newAccessory, newLoot;

        public Boolean firstFrameOverWeapon = true;
        public Boolean firstFrameOverOutfit = true;
        public Boolean firstFrameOverHat = true;
        public Boolean firstFrameOverAccessory = true;
        public Boolean firstFrameOverLoot = true;
        public Boolean firstFrameOverPassive = true;

        //--Other
        Texture2D equipBox;
        List<Button> inventoryBoxes;
        List<Button> storyItemBoxes;
        int equipmentPage;
        int storyItemPage;
        int textTimer;
        String equippedText = "";
        DescriptionBoxManager descriptionBoxManager;

        public enum TabState
        {
            weapon,
            hats,
            shirts,
            accessory,
            loot
        }
        public TabState tabState;

        Button weaponTab;
        Button shirtTab;
        Button hatTab;
        Button accessoryTab;
        Button lootTab;
        Game1 game;
        List<Button> tabButtons;

        public Boolean showingPassives = false;
        public int passivesPage = 0;
        Button passivesLeft, passivesRight;

        //--Properties

        public Boolean CanEquip { get { return canEquip; } set { canEquip = value; } }
        public int StoryItemPage { get { return storyItemPage; } set { storyItemPage = value; } }
        public int EquipmentPage { get { return equipmentPage; } set { equipmentPage = value; } }
        public int PassivePage { get { return passivePage; } set { passivePage = value; } }
        //-- CONTSTRUCTOR --\\
        public PlayerInventory(Player play, Dictionary<String, Texture2D> texts, Dictionary<String, Texture2D> darylDraw, DescriptionBoxManager d, Game1 g, Dictionary<String, Texture2D> passiv)
        {
            buttons = new List<Button>();
            player = play;
            textures = texts;
            darylDrawings = darylDraw;
            game = g;
            passiveBoxes = passiv;
            passivePositions = new List<Vector2>();

            for (int i = 0; i < 8; i++)
            {
                if(i % 2 == 0)
                    passivePositions.Add(new Vector2(337, 80 + (149 * (i / 2))));
                else
                    passivePositions.Add(new Vector2(683, 80 + (149 * (i / 2))));
            }

            #region Tab Buttons
            tabButtons = new List<Button>();
            weaponTab = new Button(new Rectangle(704, 72, 62, 52));
            hatTab = new Button(new Rectangle(808, 79, 61, 37));
            shirtTab = new Button(new Rectangle(920, 77, 63, 50));
            accessoryTab = new Button(new Rectangle(1021, 89, 71, 34));
            lootTab = new Button(new Rectangle(1132, 83, 49, 52));

            tabButtons.Add(weaponTab);
            tabButtons.Add(shirtTab);
            tabButtons.Add(hatTab);
            tabButtons.Add(lootTab);
            tabButtons.Add(accessoryTab);
            #endregion

            tabState = TabState.weapon;

            passiveTab = new Button(new Rectangle(0, 639, 135, 81));
            passiveBack = new Button(new Rectangle(918, 12, 95, 18));
            #region Icon buttons
            healthIcon = new Button(new Rectangle(377, 557, 34, 31));
            defenseIcon = new Button(new Rectangle(545, 558, 27, 31));
            experienceIcon = new Button(new Rectangle(380, 600, 29, 24));
            strengthIcon = new Button(new Rectangle(538, 594, 39, 29));
            karmaIcon = new Button(new Rectangle(542, 630, 33, 30));
            moneyIcon = new Button(new Rectangle(379, 635, 32, 24));

            bronzeKeyIcon = new Button(new Rectangle(790, 511, 37, 43));
            silverKeyIcon = new Button(new Rectangle(886, 513, 39, 39));
            goldKeyIcon = new Button(new Rectangle(993, 511, 35, 41));

            textbookIcon = new Button(new Rectangle(486, 649, 63, 43));
            #endregion

            //--Set some base things
            equipmentPage = 0;
            inventoryBoxes = new List<Button>();
            storyItemBoxes = new List<Button>();
            descriptionBoxManager = d;
            equipBox = Game1.emptyBox;
            canEquip = true;

            //Arrow buttons
            previousEquipmentPage = new Button(new Rectangle(865, 392, 40, 28));
            nextEquipmentPage = new Button(new Rectangle(982, 388, 35, 29));

            nextStoryPage = new Button(new Rectangle(974, 650, 35, 29));
            previousStoryPage = new Button(new Rectangle(857, 654, 40, 28));

            passivesLeft = new Button(new Rectangle(620, 674, 29, 29));
            passivesRight = new Button(new Rectangle(735, 674, 29, 29));

            //--Set up the equipment boxes
            weaponEquip1 = new Button(equipBox, new Rectangle(221, 63, 70, 70));
            weaponEquip2 = new Button(equipBox, new Rectangle(221, 143, 70, 70));
            motivationEquip = new Button(equipBox, new Rectangle(221, 223, 70, 70));
            toleranceEquip = new Button(equipBox, new Rectangle(221, 303, 70, 70));
            miscEquip1 = new Button(equipBox, new Rectangle(221, 383, 70, 70));
            miscEquip2 = new Button(equipBox, new Rectangle(221, 463, 70, 70));

            //--Add the equipment boxes and skills boxes to the list of buttons
            buttons.Add(weaponEquip2);
            buttons.Add(weaponEquip1);
            buttons.Add(motivationEquip);
            buttons.Add(toleranceEquip);
            buttons.Add(miscEquip1);
            buttons.Add(miscEquip2);

            newWeapon = newHat = newShirt = newAccessory = newLoot = false;

            AddInventoryBoxes();
            //UpdateResolution();
        }

        public void DrawIconInfoBox(SpriteBatch s, String text)
        {
            int boxWidth = (int)(Game1.twConRegularSmall.MeasureString(text).X + 30);
            switch (text)
            { 
                case "Health":
                    s.Draw(textures["iconInfoBox"], new Rectangle(377 - boxWidth, (int)(Game1.screenHeightAspect * .75 - 3), boxWidth , 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(377 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .75)), Color.Black);
                    break;

                case "Experience":
                    s.Draw(textures["iconInfoBox"], new Rectangle(377 - boxWidth, (int)(Game1.screenHeightAspect * .8 + 5), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(377 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .8 + 8)), Color.Black);
                    break;

                case "Money":
                    s.Draw(textures["iconInfoBox"], new Rectangle(377 - boxWidth, (int)(Game1.screenHeightAspect * .85 + 13), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(377 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .85 + 16)), Color.Black);
                    break;

                case "Defense":
                    s.Draw(textures["iconInfoBox"], new Rectangle(543 - boxWidth, (int)(Game1.screenHeightAspect * .75 - 2), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(543 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .75 + 1)), Color.Black);
                    break;

                case "Strength":
                    s.Draw(textures["iconInfoBox"], new Rectangle(543 - boxWidth, (int)(Game1.screenHeightAspect * .8 + 4), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(543 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .8 + 7)), Color.Black);
                    break;

                case "Karma":
                    s.Draw(textures["iconInfoBox"], new Rectangle(543 - boxWidth, (int)(Game1.screenHeightAspect * .85 + 5), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(543 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .85 + 8)), Color.Black);
                    break;

                case "Bronze Keys":
                    s.Draw(textures["iconInfoBox"], new Rectangle(794 - boxWidth, (int)(Game1.screenHeightAspect * .65 + 11), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(794 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .65 + 14)), Color.Black);
                    break;

                case "Silver Keys":
                    s.Draw(textures["iconInfoBox"], new Rectangle(900 - boxWidth, (int)(Game1.screenHeightAspect * .65 + 11), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(900 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .65 + 14)), Color.Black);
                    break;

                case "Gold Keys":
                    s.Draw(textures["iconInfoBox"], new Rectangle(1003 - boxWidth, (int)(Game1.screenHeightAspect * .65 + 11), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(1003 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .65 + 14)), Color.Black);
                    break;

                case "Weapons":
                    s.Draw(textures["iconInfoBox"], new Rectangle(732 - boxWidth, (int)(Game1.screenHeightAspect * .05 - 10), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(732 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .05 - 7)), Color.Black);
                    break;

                case "Hats":
                    s.Draw(textures["iconInfoBox"], new Rectangle(833 - boxWidth, (int)(Game1.screenHeightAspect * .05 - 10), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(833 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .05 - 7)), Color.Black);
                    break;

                case "Outfits":
                    s.Draw(textures["iconInfoBox"], new Rectangle(939 - boxWidth, (int)(Game1.screenHeightAspect * .05 - 10), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(939 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .05 - 7)), Color.Black);
                    break;

                case "Accessories":
                    s.Draw(textures["iconInfoBox"], new Rectangle(1050 - boxWidth, (int)(Game1.screenHeightAspect * .05 - 10), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(1050 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .05 - 7)), Color.Black);
                    break;

                case "Loot":
                    s.Draw(textures["iconInfoBox"], new Rectangle(1153 - boxWidth, (int)(Game1.screenHeightAspect * .05 - 10), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(1153 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .05 - 7)), Color.Black);
                    break;

                case "Textbooks":
                    s.Draw(textures["iconInfoBox"], new Rectangle(486 - boxWidth, (int)(Game1.screenHeightAspect * .95 - 14), boxWidth, 34), Color.White);
                    s.DrawString(Game1.twConRegularSmall, text, new Vector2(486 - boxWidth / 2 - Game1.twConRegularSmall.MeasureString(text).X / 2, (int)(Game1.screenHeightAspect * .95 - 11)), Color.Black);
                    break;
            }
        }

        public void UpdateResolution()
        {
            nextEquipmentPage.ButtonRecY = (int)(Game1.screenHeightAspect * .55 - 4);//392;
            previousEquipmentPage.ButtonRecY = (int)(Game1.screenHeightAspect * .55 - 8);//388;

            nextStoryPage.ButtonRecY = (int)(Game1.screenHeightAspect * .9 + 2); //650;
            previousStoryPage.ButtonRecY = (int)(Game1.screenHeightAspect * .9 + 6);//654;

            passivesLeft.ButtonRecY = (int)(Game1.screenHeightAspect * .95 -10); //674;
            passivesRight.ButtonRecY = (int)(Game1.screenHeightAspect * .95 - 10); //674;

            passiveTab.ButtonRecY = (int)(Game1.screenHeightAspect * .9 - 9); //639;

            weaponEquip1.ButtonRecY = (int)(Game1.screenHeightAspect * .1 - 9); //63;
            weaponEquip2.ButtonRecY = (int)(Game1.screenHeightAspect * .2 - 1); //143;
            motivationEquip.ButtonRecY = (int)(Game1.screenHeightAspect * .3 + 7); //223;
            toleranceEquip.ButtonRecY = (int)(Game1.screenHeightAspect * .4 + 15); //303;
            miscEquip1.ButtonRecY = (int)(Game1.screenHeightAspect * .55 - 13); //383;
            miscEquip2.ButtonRecY = (int)(Game1.screenHeightAspect * .65 - 5); //463;

            healthIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .75 + 17); //557;
            defenseIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .75 + 18); //558;
            experienceIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .85 - 12); //600;
            strengthIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .85 - 18); //594;
            karmaIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .9 - 18); //630;
            moneyIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .9 - 13); //635;

            bronzeKeyIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .7 + 7); //511;
            silverKeyIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .7 + 9); //513;
            goldKeyIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .7 + 7); //511;

            textbookIcon.ButtonRecY = (int)(Game1.screenHeightAspect * .9 + 1); //649;

            weaponTab.ButtonRecY = (int)(Game1.screenHeightAspect * .1); //72;
            hatTab.ButtonRecY = (int)(Game1.screenHeightAspect * .1 + 7); //79;
            shirtTab.ButtonRecY = (int)(Game1.screenHeightAspect * .1 + 5); //77;
            accessoryTab.ButtonRecY = (int)(Game1.screenHeightAspect * .1 + 17); //89;
            lootTab.ButtonRecY = (int)(Game1.screenHeightAspect * .1 + 11); //83;

            for (int i = 0; i < 15; i++)
            {
                if (i < 5)
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.screenHeightAspect * .2 + 7); //151;
                }
                else if (i < 10)
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.screenHeightAspect * .35 - 20); //232;
                }
                else
                {
                    inventoryBoxes[i].ButtonRecY = (int)(Game1.screenHeightAspect * .45 - 11); //313;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                storyItemBoxes[i].ButtonRecY = (int)(Game1.screenHeightAspect * .8 - 11); //565;
            }
        }

        public void AddInventoryBoxes()
        {
            //--Add the boxes to the list after creating them
            for (int i = 0; i < 15; i++)
            {
                if (i < 5)
                {
                    Button box = new Button(equipBox, new Rectangle(742 + (i * 81), 151,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
                else if (i < 10)
                {
                    Button box = new Button(equipBox, new Rectangle(742 + ((i - 5) * 81), 232,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
                else
                {
                    Button box = new Button(equipBox, new Rectangle(742 + ((i - 10) * 81), 313, //370,
                        70, 70));
                    inventoryBoxes.Add(box);
                }
            }

            //--Story item boxes
            for (int i = 0; i < 4; i++)
            {
                Button box = new Button(equipBox, new Rectangle(778 + (i * 81), 565,
                        70, 70));

                storyItemBoxes.Add(box);
            }
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            if (((current.IsKeyUp(Keys.Escape) && last.IsKeyDown(Keys.Escape)) || (current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back)) || passiveBack.Clicked() || MyGamePad.BPressed()) && showingPassives)
            {
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_02);
                showingPassives = false;
            }
            textTimer--;

            UpdateEquipment();
            UpdateInventory();
            player.UpdateStats();
            ChangeTab();

            #region New Equipment Icons Update
            if (tabState == TabState.weapon && newWeapon)
                newWeapon = false;
            if (tabState == TabState.hats && newHat)
                newHat = false;
            if (tabState == TabState.shirts && newShirt)
                newShirt = false;
            if (tabState == TabState.accessory && newAccessory)
                newAccessory = false;
            if (tabState == TabState.loot && newLoot)
                newLoot = false;
            #endregion

            if (!showingPassives)
            {

                if (DarylsNotebook.journalTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.journal;

                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                }

                if (DarylsNotebook.combosTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.combos;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }

                if (DarylsNotebook.bioTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.bios;
                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                }

                if (DarylsNotebook.questsTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.quests;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }

                if (DarylsNotebook.mapsTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.maps;
                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                }
            }
        }

        //--Changes between the inventory tabs
        public void ChangeTab()
        {
            if (!showingPassives)
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
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);
                        }
                        else if (tab == shirtTab)
                        {
                            tabState = TabState.shirts;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);
                        }
                        else if (tab == hatTab)
                        {
                            tabState = TabState.hats;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);
                        }
                        else if (tab == accessoryTab)
                        {
                            tabState = TabState.accessory;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);
                        }
                        else if (tab == lootTab)
                        {
                            tabState = TabState.loot;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);
                        }
                    }
                }
            }
        }

        public void UpdateEquipment()
        {
            if (!showingPassives)
            {
                //--Check every button in the list
                for (int i = 0; i < buttons.Count; i++)
                {
                    //--If it was clicked
                    if (buttons[i].DoubleClicked() || buttons[i].RightClicked())
                    {

                        #region REMOVE WEAPONS

                        //--Remove the first weapon
                        if (buttons[i] == weaponEquip1 && player.EquippedWeapon != null)
                        {
                            if (player.SecondWeapon != null && player.OwnedWeapons.Count < 44 || player.SecondWeapon == null && player.OwnedWeapons.Count < 45)
                            {

                                //--Remove stats, set equip box texture, and set the player's weapon to null
                                //--Make sure to add the weapon back into the player's inventory
                                if (player.SecondWeapon == null)
                                    DrawEquippedText(player.EquippedWeapon.Name, -1);
                                else
                                    DrawEquippedText(player.EquippedWeapon.Name + " and " + player.SecondWeapon.Name, -1);

                                AdjustEquipmentStats("Weapon", -1);
                                weaponEquip1.ButtonTexture = equipBox;
                                player.OwnedWeapons.Add(player.EquippedWeapon);
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_weapon_01);

                                //Remove the passive ability to the player if the equipment has one
                                if (player.EquippedWeapon.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedWeapon.PassiveAbility))
                                {
                                    //Make sure to unload the passive first
                                    player.EquippedWeapon.PassiveAbility.UnloadPassive();
                                    player.OwnedPassives.Remove(player.EquippedWeapon.PassiveAbility);

                                }

                                player.EquippedWeapon = null;

                                darylDrawings["MainClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Main\MainClean");

                                //--If you remove the first weapon and you are wearing a second, automatically remove the second
                                if (player.SecondWeapon != null)
                                {

                                    AdjustEquipmentStats("SecondWeapon", -1);
                                    weaponEquip2.ButtonTexture = equipBox;
                                    player.OwnedWeapons.Add(player.SecondWeapon);

                                    //Remove the passive ability to the player if the equipment has one
                                    if (player.SecondWeapon.PassiveAbility != null && player.OwnedPassives.Contains(player.SecondWeapon.PassiveAbility))
                                    {
                                        //Make sure to unload the passive first
                                        player.SecondWeapon.PassiveAbility.UnloadPassive();
                                        player.OwnedPassives.Remove(player.SecondWeapon.PassiveAbility);

                                    }
                                    player.SecondWeapon = null;

                                    darylDrawings["SecondClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\SecondClean");
                                }
                            }
                        }

                        //--If you remove the second
                        if (buttons[i] == weaponEquip2 && player.SecondWeapon != null && player.OwnedWeapons.Count < 45)
                        {
                            DrawEquippedText(player.SecondWeapon.Name, -1);
                            AdjustEquipmentStats("SecondWeapon", -1);
                            weaponEquip2.ButtonTexture = equipBox;
                            player.OwnedWeapons.Add(player.SecondWeapon);
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_weapon_02);

                            darylDrawings["SecondClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\SecondClean");

                            //Remove the passive ability to the player if the equipment has one
                            if (player.SecondWeapon.PassiveAbility != null && player.OwnedPassives.Contains(player.SecondWeapon.PassiveAbility) && (player.EquippedWeapon.PassiveAbility == null || (player.EquippedWeapon.PassiveAbility != player.SecondWeapon.PassiveAbility)))
                            {
                                //Make sure to unload the passive first
                                player.SecondWeapon.PassiveAbility.UnloadPassive();
                                player.OwnedPassives.Remove(player.SecondWeapon.PassiveAbility);

                            }

                            player.SecondWeapon = null;
                        }
                        #endregion

                        #region REMOVE HAT
                        if (buttons[i] == motivationEquip && player.EquippedHat != null && player.OwnedHats.Count < 45)
                        {
                            DrawEquippedText(player.EquippedHat.Name, -1);

                            AdjustEquipmentStats("Hat", -1);
                            player.OwnedHats.Add(player.EquippedHat);
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                            //Remove the passive ability to the player if the equipment has one
                            if (player.EquippedHat.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedHat.PassiveAbility))
                            {
                                //Make sure to unload the passive first
                                player.EquippedHat.PassiveAbility.UnloadPassive();
                                player.OwnedPassives.Remove(player.EquippedHat.PassiveAbility);

                            }

                            player.EquippedHat = null;

                            darylDrawings["HatClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Hats\HatClean");

                            motivationEquip.ButtonTexture = equipBox;
                        }
                        #endregion

                        #region REMOVE HOODIE
                        if (buttons[i] == toleranceEquip && player.EquippedHoodie != null && player.OwnedHoodies.Count < 45)
                        {
                            DrawEquippedText(player.EquippedHoodie.Name, -1);

                            AdjustEquipmentStats("Hoodie", -1);
                            player.OwnedHoodies.Add(player.EquippedHoodie);
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                            //Remove the passive ability to the player if the equipment has one
                            if (player.EquippedHoodie.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedHoodie.PassiveAbility))
                            {
                                //Make sure to unload the passive first
                                player.EquippedHoodie.PassiveAbility.UnloadPassive();
                                player.OwnedPassives.Remove(player.EquippedHoodie.PassiveAbility);

                            }

                            player.EquippedHoodie = null;

                            darylDrawings["ShirtClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Shirts\ShirtClean");

                            toleranceEquip.ButtonTexture = equipBox;
                        }
                        #endregion

                        #region REMOVE ACCESSORIES

                        //--Remove the first accessory
                        if (buttons[i] == miscEquip1 && player.EquippedAccessory != null && player.OwnedAccessories.Count < 45)
                        {
                            //--Remove stats, set equip box texture, and set the player's weapon to null
                            //--Make sure to add the weapon back into the player's inventory
                            DrawEquippedText(player.EquippedAccessory.Name, -1);
                            AdjustEquipmentStats("Accessory", -1);
                            miscEquip1.ButtonTexture = equipBox;
                            player.OwnedAccessories.Add(player.EquippedAccessory);
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                            //Remove the passive ability to the player if the equipment has one
                            if (player.EquippedAccessory.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedAccessory.PassiveAbility))
                            {
                                //Make sure to unload the passive first
                                player.EquippedAccessory.PassiveAbility.UnloadPassive();
                                player.OwnedPassives.Remove(player.EquippedAccessory.PassiveAbility);

                            }

                            player.EquippedAccessory = null;
                        }

                        //--If you remove the second
                        if (buttons[i] == miscEquip2 && player.SecondAccessory != null && player.OwnedAccessories.Count < 45)
                        {
                            DrawEquippedText(player.SecondAccessory.Name, -1);
                            AdjustEquipmentStats("SecondAccessory", -1);
                            miscEquip2.ButtonTexture = equipBox;
                            player.OwnedAccessories.Add(player.SecondAccessory);
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                            //Remove the passive ability to the player if the equipment has one
                            if (player.SecondAccessory.PassiveAbility != null && player.OwnedPassives.Contains(player.SecondAccessory.PassiveAbility) && ((player.EquippedAccessory == null || player.EquippedAccessory.PassiveAbility == null) || (player.EquippedAccessory.PassiveAbility != player.SecondAccessory.PassiveAbility)))
                            {
                                //Make sure to unload the passive first
                                player.SecondAccessory.PassiveAbility.UnloadPassive();
                                player.OwnedPassives.Remove(player.SecondAccessory.PassiveAbility);

                            }

                            player.SecondAccessory = null;
                        }
                        #endregion
                    }
                }
            }
        }

        //--Update inventory boxes
        public void UpdateInventory()
        {

            if(player.EquippedWeapon != null)
                weaponEquip1.ButtonTexture = player.EquippedWeapon.Icon;
            if(player.SecondWeapon != null)
                weaponEquip2.ButtonTexture = player.SecondWeapon.Icon;

            if (player.EquippedHoodie != null)
                toleranceEquip.ButtonTexture = player.EquippedHoodie.Icon;
            if (player.EquippedHat != null)
                motivationEquip.ButtonTexture = player.EquippedHat.Icon;

            if (player.EquippedAccessory != null)
                miscEquip1.ButtonTexture = player.EquippedAccessory.Icon;
            if (player.SecondAccessory != null)
                miscEquip2.ButtonTexture = player.SecondAccessory.Icon;

            #region Draw And Update Inventory Items
            //--For as many boxes there are on each page
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                //Based on which tab you are currently in

                int boxNumber = i + (equipmentPage * inventoryBoxes.Count);

                switch (tabState)
                {
                    #region WEAPON TAB
                    case TabState.weapon:
                        if (player.OwnedWeapons.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedWeapons[boxNumber].Icon;

                            if (player.EquippedWeapon == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedWeapons[boxNumber].Level)
                                    {
                                        player.EquippedWeapon = player.OwnedWeapons[boxNumber];

                                        darylDrawings[player.EquippedWeapon.Name] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Main\" + Game1.Player.EquippedWeapon.Name);
                                        
                                        if(player.EquippedWeapon.Name == "Chisel of Forgotten Love")
                                            darylDrawings["Chisel of Forgotten Love Second"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\" + "Chisel of Forgotten Love Second");

                                        player.OwnedWeapons.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        weaponEquip1.ButtonTexture = player.EquippedWeapon.Icon;
                                        AdjustEquipmentStats("Weapon", 1);
                                        DrawEquippedText(player.EquippedWeapon.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_weapon_01);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedWeapon.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedWeapon.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedWeapon.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedWeapon.PassiveAbility);
                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }

                            else if (player.SecondWeapon == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && player.EquippedWeapon.CanHoldTwo && player.OwnedWeapons[boxNumber].CanHoldTwo == true && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedWeapons[boxNumber].Level)
                                    {
                                        player.SecondWeapon = player.OwnedWeapons[boxNumber];

                                        darylDrawings[Game1.Player.SecondWeapon.Name + " Second"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\" + Game1.Player.SecondWeapon.Name + " Second");

                                        player.OwnedWeapons.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        weaponEquip2.ButtonTexture = player.SecondWeapon.Icon;
                                        AdjustEquipmentStats("SecondWeapon", 1);
                                        DrawEquippedText(player.SecondWeapon.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_weapon_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.SecondWeapon.PassiveAbility != null && !player.OwnedPassives.Contains(player.SecondWeapon.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.SecondWeapon.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.SecondWeapon.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }
                            else if (inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked())
                            {
                                if (player.OwnedWeapons[boxNumber].CanHoldTwo == true && player.EquippedWeapon.CanHoldTwo && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedWeapons[boxNumber].Level)
                                    {
                                        //Remove the second weapon
                                        AdjustEquipmentStats("SecondWeapon", -1);
                                        weaponEquip2.ButtonTexture = equipBox;
                                        player.OwnedWeapons.Add(player.SecondWeapon);

                                        //Remove the passive ability to the player if the equipment has one
                                        if (player.SecondWeapon.PassiveAbility != null && player.OwnedPassives.Contains(player.SecondWeapon.PassiveAbility))
                                        {
                                            //Make sure to unload the passive first
                                            player.SecondWeapon.PassiveAbility.UnloadPassive();
                                            player.OwnedPassives.Remove(player.SecondWeapon.PassiveAbility);

                                        }
                                        player.SecondWeapon = null;
                                        darylDrawings["SecondClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\SecondClean");

                                        //Equip the new one
                                        player.SecondWeapon = player.OwnedWeapons[boxNumber];

                                        darylDrawings[Game1.Player.SecondWeapon.Name + " Second"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Second\" + Game1.Player.SecondWeapon.Name + " Second");

                                        player.OwnedWeapons.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        weaponEquip2.ButtonTexture = player.SecondWeapon.Icon;
                                        AdjustEquipmentStats("SecondWeapon", 1);
                                        DrawEquippedText(player.SecondWeapon.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_weapon_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.SecondWeapon.PassiveAbility != null && !player.OwnedPassives.Contains(player.SecondWeapon.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.SecondWeapon.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.SecondWeapon.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                            }
                        }
                        break;
                    #endregion

                    #region HAT TAB
                    case TabState.hats:

                        if (player.OwnedHats.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHats[boxNumber].Icon;
                            if (player.EquippedHat == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedHats[boxNumber].Level)
                                    {

                                        player.EquippedHat = player.OwnedHats[boxNumber];
                                        darylDrawings[player.EquippedHat.Name] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Hats\" + Game1.Player.EquippedHat.Name);
                                        ResetInventoryBoxes();
                                        player.OwnedHats.RemoveAt(boxNumber);
                                        motivationEquip.ButtonTexture = player.EquippedHat.Icon;
                                        AdjustEquipmentStats("Hat", 1);
                                        DrawEquippedText(player.EquippedHat.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedHat.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedHat.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedHat.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedHat.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }
                            else if (inventoryBoxes[i].RightClicked() || inventoryBoxes[i].DoubleClicked())
                            {
                                if (canEquip)
                                {
                                    if (player.Level >= player.OwnedHats[boxNumber].Level)
                                    {
                                        //Remove hat
                                        AdjustEquipmentStats("Hat", -1);
                                        player.OwnedHats.Add(player.EquippedHat);

                                        //Remove the passive ability to the player if the equipment has one
                                        if (player.EquippedHat.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedHat.PassiveAbility))
                                        {
                                            //Make sure to unload the passive first
                                            player.EquippedHat.PassiveAbility.UnloadPassive();
                                            player.OwnedPassives.Remove(player.EquippedHat.PassiveAbility);

                                        }

                                        player.EquippedHat = null;

                                        darylDrawings["HatClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Hats\HatClean");

                                        motivationEquip.ButtonTexture = equipBox;

                                        //Add new hat
                                        player.EquippedHat = player.OwnedHats[boxNumber];
                                        darylDrawings[player.EquippedHat.Name] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Hats\" + Game1.Player.EquippedHat.Name);
                                        ResetInventoryBoxes();
                                        player.OwnedHats.RemoveAt(boxNumber);
                                        motivationEquip.ButtonTexture = player.EquippedHat.Icon;
                                        AdjustEquipmentStats("Hat", 1);
                                        DrawEquippedText(player.EquippedHat.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedHat.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedHat.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedHat.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedHat.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                            }
                        } break;
                    #endregion

                    #region HOODIE TAB
                    case TabState.shirts:
                        if (player.OwnedHoodies.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedHoodies[boxNumber].Icon;

                            if (player.EquippedHoodie == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedHoodies[boxNumber].Level)
                                    {
                                        player.EquippedHoodie = player.OwnedHoodies[boxNumber];
                                        darylDrawings[player.EquippedHoodie.Name] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Shirts\" + Game1.Player.EquippedHoodie.Name);
                                        ResetInventoryBoxes();
                                        player.OwnedHoodies.RemoveAt(boxNumber);
                                        toleranceEquip.ButtonTexture = player.EquippedHoodie.Icon;
                                        AdjustEquipmentStats("Hoodie", 1);
                                        DrawEquippedText(player.EquippedHoodie.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedHoodie.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedHoodie.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedHoodie.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedHoodie.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("Your are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }
                            else if (inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked())
                            {
                                if (canEquip)
                                {
                                    if (player.Level >= player.OwnedHoodies[boxNumber].Level)
                                    {
                                        //Remove old outfit
                                        AdjustEquipmentStats("Hoodie", -1);
                                        player.OwnedHoodies.Add(player.EquippedHoodie);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                                        //Remove the passive ability to the player if the equipment has one
                                        if (player.EquippedHoodie.PassiveAbility != null && player.OwnedPassives.Contains(player.EquippedHoodie.PassiveAbility))
                                        {
                                            //Make sure to unload the passive first
                                            player.EquippedHoodie.PassiveAbility.UnloadPassive();
                                            player.OwnedPassives.Remove(player.EquippedHoodie.PassiveAbility);

                                        }

                                        player.EquippedHoodie = null;

                                        darylDrawings["ShirtClean"] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Shirts\ShirtClean");

                                        toleranceEquip.ButtonTexture = equipBox;

                                        //Add new one
                                        player.EquippedHoodie = player.OwnedHoodies[boxNumber];
                                        darylDrawings[player.EquippedHoodie.Name] = DarylsNotebook.Content.Load<Texture2D>(@"DarylDrawing\Shirts\" + Game1.Player.EquippedHoodie.Name);
                                        ResetInventoryBoxes();
                                        player.OwnedHoodies.RemoveAt(boxNumber);
                                        toleranceEquip.ButtonTexture = player.EquippedHoodie.Icon;
                                        AdjustEquipmentStats("Hoodie", 1);
                                        DrawEquippedText(player.EquippedHoodie.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedHoodie.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedHoodie.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedHoodie.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedHoodie.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("Your are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                            }
                        }
                        break;
                    #endregion

                    #region ACCESSORY TAB
                    case TabState.accessory:
                        if (player.OwnedAccessories.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = player.OwnedAccessories[boxNumber].Icon;

                            if (player.EquippedAccessory == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedAccessories[boxNumber].Level)
                                    {
                                        player.EquippedAccessory = player.OwnedAccessories[boxNumber];
                                        player.OwnedAccessories.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        miscEquip1.ButtonTexture = player.EquippedAccessory.Icon;
                                        AdjustEquipmentStats("Accessory", 1);
                                        DrawEquippedText(player.EquippedAccessory.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_01);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.EquippedAccessory.PassiveAbility != null && !player.OwnedPassives.Contains(player.EquippedAccessory.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.EquippedAccessory.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.EquippedAccessory.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }

                            else if (player.SecondAccessory == null)
                            {
                                if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()) && canEquip == true)
                                {
                                    if (player.Level >= player.OwnedAccessories[boxNumber].Level)
                                    {
                                        player.SecondAccessory = player.OwnedAccessories[boxNumber];
                                        player.OwnedAccessories.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        miscEquip2.ButtonTexture = player.SecondAccessory.Icon;
                                        AdjustEquipmentStats("SecondAccessory", 1);
                                        DrawEquippedText(player.SecondAccessory.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.SecondAccessory.PassiveAbility != null && !player.OwnedPassives.Contains(player.SecondAccessory.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.SecondAccessory.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.SecondAccessory.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                            }
                            else if ((inventoryBoxes[i].DoubleClicked() || inventoryBoxes[i].RightClicked()))
                            {
                                if (canEquip)
                                {
                                    DrawEquippedText(player.SecondAccessory.Name, -1);
                                    AdjustEquipmentStats("SecondAccessory", -1);
                                    miscEquip2.ButtonTexture = equipBox;
                                    player.OwnedAccessories.Add(player.SecondAccessory);
                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                                    //Remove the passive ability to the player if the equipment has one
                                    if (player.SecondAccessory.PassiveAbility != null && player.OwnedPassives.Contains(player.SecondAccessory.PassiveAbility) && ((player.EquippedAccessory == null || player.EquippedAccessory.PassiveAbility == null) || (player.EquippedAccessory.PassiveAbility != player.SecondAccessory.PassiveAbility)))
                                    {
                                        //Make sure to unload the passive first
                                        player.SecondAccessory.PassiveAbility.UnloadPassive();
                                        player.OwnedPassives.Remove(player.SecondAccessory.PassiveAbility);

                                    }

                                    player.SecondAccessory = null;

                                    //Add new one
                                    if (player.Level >= player.OwnedAccessories[boxNumber].Level)
                                    {
                                        player.SecondAccessory = player.OwnedAccessories[boxNumber];
                                        player.OwnedAccessories.RemoveAt(boxNumber);
                                        ResetInventoryBoxes();
                                        miscEquip2.ButtonTexture = player.SecondAccessory.Icon;
                                        AdjustEquipmentStats("SecondAccessory", 1);
                                        DrawEquippedText(player.SecondAccessory.Name, 1);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_equip_clothes_02);

                                        //Add the passive ability to the player if the equipment has one
                                        if (player.SecondAccessory.PassiveAbility != null && !player.OwnedPassives.Contains(player.SecondAccessory.PassiveAbility))
                                        {
                                            //Make sure to load the passive first
                                            player.SecondAccessory.PassiveAbility.LoadPassive();
                                            player.OwnedPassives.Add(player.SecondAccessory.PassiveAbility);

                                        }
                                    }
                                    else
                                    {
                                        DrawEquippedText("You are not a high enough level", 0);
                                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                                    }
                                }
                                else
                                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                            }
                        }
                        break;
                    #endregion

                    #region DROPS/ETC TAB
                    case TabState.loot:
                        if (player.EnemyDrops.Count > boxNumber)
                        {
                            inventoryBoxes[i].ButtonTexture = EnemyDrop.allDrops[player.EnemyDrops.ElementAt(boxNumber).Key].texture;
                        }
                        break;
                    #endregion
                }
            }
            #endregion


            if (!showingPassives)
            {
                #region Change Pages

                //--If you are not on the last page, go up a page and reset textures
                if ((nextEquipmentPage.Clicked() || MyGamePad.RightPadPressed()) && equipmentPage < 4)
                {
                    equipmentPage++;

                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_list_02);

                    ResetInventoryBoxes();
                }
                //--If you aren't on the first page, go back a page and reset textures
                if ((previousEquipmentPage.Clicked() || MyGamePad.LeftPadPressed())&& equipmentPage > 0)
                {
                    equipmentPage--;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_list_01);
                    ResetInventoryBoxes();
                }

                //--Story pages
                if ((nextStoryPage.Clicked() || MyGamePad.DownPadPressed())&& storyItemPage < 4)
                {
                    storyItemPage++;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_list_02);
                    ResetStoryBoxes();
                }
                //--If you aren't on the first page, go back a page and reset textures
                if ((previousStoryPage.Clicked() || MyGamePad.UpPadPressed()) && storyItemPage > 0)
                {
                    storyItemPage--;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_list_01);
                    ResetStoryBoxes();
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

            #region Passives

            if (passiveTab.Clicked() && !showingPassives)
            {
                showingPassives = true;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_04);
            }

            if ((passivesLeft.Clicked() || (showingPassives && MyGamePad.LeftPadPressed())) && passivePage > 0)
            {
                passivePage--;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_01);
            }
            else if ((passivesRight.Clicked() || (showingPassives && MyGamePad.RightPadPressed())) && player.OwnedPassives.Count > ((passivesPage + 1) * 8))
            {
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_02);
                passivePage++;
            }
            #endregion
        }

        //--Adds or subtracts the item's stats to the player's stats
        //-- Pass in '1' for addition, '-1' for subtraction
        public void AdjustEquipmentStats(String type, int addOrRemove)
        {
            if(game.Prologue.PrologueBooleans["firstInventory"] == true)
                game.Prologue.PrologueBooleans["firstInventory"] = false;

            switch (type)
            {
                case "Weapon":
                    player.BaseStrength += player.EquippedWeapon.Strength * addOrRemove;
                    player.BaseMaxHealth += player.EquippedWeapon.Health * addOrRemove;
                    player.UpdateStats();

                   // player.Health += player.EquippedWeapon.Health * addOrRemove;
                    player.BaseDefense += player.EquippedWeapon.Defense * addOrRemove;
                    player.JumpHeight += player.EquippedWeapon.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.EquippedWeapon.MoveSpeed * addOrRemove;
                    break;

                case "SecondWeapon":

                    player.BaseStrength += player.SecondWeapon.Strength * addOrRemove;
                    player.BaseMaxHealth += player.SecondWeapon.Health * addOrRemove;
                    player.UpdateStats();

                    //player.Health += player.SecondWeapon.Health * addOrRemove;
                    player.BaseDefense += player.SecondWeapon.Defense * addOrRemove;
                    player.JumpHeight += player.SecondWeapon.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.SecondWeapon.MoveSpeed * addOrRemove;
                    break;

                case "Hat":

                    player.BaseStrength += player.EquippedHat.Strength * addOrRemove;
                    player.BaseMaxHealth += player.EquippedHat.Health * addOrRemove;
                    player.UpdateStats();
                   // player.Health += player.EquippedHat.Health * addOrRemove;
                    player.BaseDefense += player.EquippedHat.Defense * addOrRemove;
                    player.JumpHeight += player.EquippedHat.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.EquippedHat.MoveSpeed * addOrRemove;
                    break;

                case "Hoodie":

                    player.BaseStrength += player.EquippedHoodie.Strength * addOrRemove;
                    player.BaseMaxHealth += player.EquippedHoodie.Health * addOrRemove;
                    player.UpdateStats();

                   // player.Health += player.EquippedHoodie.Health * addOrRemove;
                    player.BaseDefense += player.EquippedHoodie.Defense * addOrRemove;
                    player.JumpHeight += player.EquippedHoodie.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.EquippedHoodie.MoveSpeed * addOrRemove;
                    break;

                case "Accessory":

                    player.BaseStrength += player.EquippedAccessory.Strength * addOrRemove;
                    player.BaseMaxHealth += player.EquippedAccessory.Health * addOrRemove;
                    player.UpdateStats();

                  //  player.Health += player.EquippedAccessory.Health * addOrRemove;
                    player.BaseDefense += player.EquippedAccessory.Defense * addOrRemove;
                    player.JumpHeight += player.EquippedAccessory.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.EquippedAccessory.MoveSpeed * addOrRemove;
                    break;

                case "SecondAccessory":

                    player.BaseStrength += player.SecondAccessory.Strength * addOrRemove;
                    player.BaseMaxHealth += player.SecondAccessory.Health * addOrRemove;
                    player.UpdateStats();

                   // player.Health += player.SecondAccessory.Health * addOrRemove;
                    player.BaseDefense += player.SecondAccessory.Defense * addOrRemove;
                    player.JumpHeight += player.SecondAccessory.JumpHeight * addOrRemove;
                    player.MoveSpeed += player.SecondAccessory.MoveSpeed * addOrRemove;
                    break;
            }

            if (player.Health < 1)
                player.Health = 1;

            if (player.Health > player.realMaxHealth)
                player.Health = player.realMaxHealth;
        }

        public void DrawDaryl(SpriteBatch s)
        {
            if (player.EquippedHat == null)
                s.Draw(darylDrawings["HatClean"], new Rectangle(200, (int)(Game1.screenHeightAspect * .05 - 6), 473, 270), Color.White);
            else
                s.Draw(darylDrawings[player.EquippedHat.Name], new Rectangle(200, (int)(Game1.screenHeightAspect * .05 - 6), 473, 270), Color.White);

            if (player.EquippedHoodie == null)
                s.Draw(darylDrawings["ShirtClean"], new Rectangle(200, (int)(Game1.screenHeightAspect * .25 + 10), 392, 286), Color.White);
            else
                s.Draw(darylDrawings[player.EquippedHoodie.Name], new Rectangle(200, (int)(Game1.screenHeightAspect * .25 + 10), 392, 286), Color.White);

            if (player.EquippedWeapon == null)
                s.Draw(darylDrawings["MainClean"], new Rectangle(402, (int)(Game1.screenHeightAspect * .05 - 12), 316, 474), Color.White);
            else
                s.Draw(darylDrawings[player.EquippedWeapon.Name], new Rectangle(402, (int)(Game1.screenHeightAspect * .05 - 11), 316, 474), Color.White);

            if (player.SecondWeapon == null)
            {
                s.Draw(darylDrawings["SecondClean"], new Rectangle(290, (int)(Game1.screenHeightAspect * .05 - 11), 258, 485), Color.White);

                if (player.EquippedWeapon != null && player.EquippedWeapon.Name == "Chisel of Forgotten Love")
                    s.Draw(darylDrawings[player.EquippedWeapon.Name + " Second"], new Rectangle(290, (int)(Game1.screenHeightAspect * .05 - 11), 258, 485), Color.White);

            }
            else
                s.Draw(darylDrawings[player.SecondWeapon.Name + " Second"], new Rectangle(290, (int)(Game1.screenHeightAspect * .05 - 11), 258, 485), Color.White);

        }

        //--Reset all of the inventory box textures back to empty
        public void ResetInventoryBoxes()
        {
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                inventoryBoxes[i].ButtonTexture = equipBox;
            }
        }

        //--Reset all of the inventory box textures back to empty
        public void ResetStoryBoxes()
        {
            for (int i = 0; i < storyItemBoxes.Count; i++)
            {
                storyItemBoxes[i].ButtonTexture = equipBox;
            }
        }

        public void ResetInventoryForGameOver()
        {
            ResetInventoryBoxes();
            ResetStoryBoxes();
            weaponEquip1.ButtonTexture = equipBox;
            weaponEquip1.ButtonTexture = equipBox;
            toleranceEquip.ButtonTexture = equipBox;
            motivationEquip.ButtonTexture = equipBox;
            miscEquip1.ButtonTexture = equipBox;
            miscEquip2.ButtonTexture = equipBox;
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["Background"], new Rectangle(0,0,1280,(int)(Game1.aspectRatio * 1280)), Color.White);

            if (passiveTab.IsOver() && !showingPassives)
            {
                s.Draw(textures["PassiveTabActive"], new Rectangle(0, (int)(Game1.aspectRatio * 1280) - textures["PassiveTabActive"].Height, textures["PassiveTabActive"].Width, textures["PassiveTabActive"].Height), Color.White);

                if (firstFrameOverPassive)
                {
                    firstFrameOverPassive = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }
            }
            else
                firstFrameOverPassive = true;


            DrawDaryl(s);

            #region Draw the tab lines
            switch (tabState)
            {
                case TabState.weapon:
                    s.Draw(textures["WeaponsPage"], new Rectangle(675, (int)(Game1.screenHeightAspect * .1 - 15), textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.shirts:
                    s.Draw(textures["ShirtsPage"], new Rectangle(675, (int)(Game1.screenHeightAspect * .1 - 15), textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.loot:
                    s.Draw(textures["LootPage"], new Rectangle(675, (int)(Game1.screenHeightAspect * .1 - 15), textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.hats:
                    s.Draw(textures["HatsPage"], new Rectangle(675, (int)(Game1.screenHeightAspect * .1 - 15), textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
                case TabState.accessory:
                    s.Draw(textures["AccessoriesPage"], new Rectangle(675, (int)(Game1.screenHeightAspect * .1 - 15), textures["WeaponsPage"].Width, textures["WeaponsPage"].Height), Color.White);
                    break;
            }
            #endregion

            #region Draw tab icons for inventory
            if (weaponTab.IsOver() && !showingPassives)
            {
                timeOverWeaponIcon++;
                s.Draw(textures["WeaponTabActive"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);

                if (firstFrameOverWeapon)
                {
                    firstFrameOverWeapon = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }
            }
            else
            {
                firstFrameOverWeapon = true;
                s.Draw(textures["WeaponTabStatic"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverWeaponIcon = 0;
            }

            if (hatTab.IsOver() && !showingPassives)
            {
                s.Draw(textures["HatsTabActive"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverHatIcon++;

                if (firstFrameOverHat)
                {
                    firstFrameOverHat = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }
            }
            else
            {
                firstFrameOverHat = true;
                s.Draw(textures["HatsTabStatic"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverHatIcon = 0;
            }

            if (shirtTab.IsOver() && !showingPassives)
            {
                s.Draw(textures["ShirtsTabActive"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverShirtIcon++;

                if (firstFrameOverOutfit)
                {
                    firstFrameOverOutfit = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }
            }
            else
            {
                firstFrameOverOutfit = true;
                s.Draw(textures["ShirtsTabStatic"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverShirtIcon = 0;
            }

            if (accessoryTab.IsOver() && !showingPassives)
            {
                s.Draw(textures["AccessoryTabActive"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverAccessoryIcon++;

                if (firstFrameOverAccessory)
                {
                    firstFrameOverAccessory = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }
            }
            else
            {
                firstFrameOverAccessory = true;
                s.Draw(textures["AccessoryTabStatic"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverAccessoryIcon = 0;
            }

            if (lootTab.IsOver() && !showingPassives)
            {

                if (firstFrameOverLoot)
                {
                    firstFrameOverLoot = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                }

                s.Draw(textures["LootTabActive"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverLootIcon++;
            }
            else
            {
                firstFrameOverLoot = true;
                s.Draw(textures["LootTabStatic"], new Rectangle(694, (int)(Game1.screenHeightAspect * .1 - 21), 503, 99), Color.White);
                timeOverLootIcon = 0;
            }
            if(newWeapon)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(748, (int)(Game1.screenHeightAspect * .05 + 4), 45, 45), Color.White);
            if (newHat)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(849, (int)(Game1.screenHeightAspect * .05 + 6), 45, 45), Color.White);
            if (newShirt)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(960, (int)(Game1.screenHeightAspect * .05 + 6), 45, 45), Color.White);
            if (newAccessory)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(1071, (int)(Game1.screenHeightAspect * .05 + 8), 45, 45), Color.White);
            if (newLoot)
                s.Draw(textures["newEquipmentIcon"], new Rectangle(1170, (int)(Game1.screenHeightAspect * .05 + 18), 45, 45), Color.White);

            #endregion

            #region Depending on the page, draw the page arrows
            //ITEMS
            s.DrawString(Game1.font,"pg " + (equipmentPage + 1).ToString() + "/5", new Vector2(913, (int)(392)), Color.Black);

            if (equipmentPage < 4)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dRight, new Vector2(985, (int)(Game1.screenHeightAspect * .55 - 8)), Color.White);
                else
                {
                    if (nextEquipmentPage.IsOver())
                        s.Draw(textures["ItemRightActive"], new Rectangle(850, (int)(Game1.screenHeightAspect * .55 - 8), textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
                    else
                        s.Draw(textures["ItemRight"], new Rectangle(850, (int)(Game1.screenHeightAspect * .55 - 8), textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
                }
            }
            if (equipmentPage > 0)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dLeft, new Vector2(865, (int)(Game1.screenHeightAspect * .55 - 8)), Color.White);
                else
                {
                    if (previousEquipmentPage.IsOver() && equipmentPage > 0)
                        s.Draw(textures["ItemLeftActive"], new Rectangle(850, (int)(Game1.screenHeightAspect * .55 - 8), textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
                    else
                        s.Draw(textures["ItemLeft"], new Rectangle(850, (int)(Game1.screenHeightAspect * .55 - 8), textures["ItemRightActive"].Width, textures["ItemRightActive"].Height), Color.White);
                }
            }
            //STORY ITEMS
            s.DrawString(Game1.font, "pg " + (storyItemPage + 1).ToString() + "/5", new Vector2(903, (int)(Game1.screenHeightAspect * .9 + 2)), Color.Black);

            if (storyItemPage < 4)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dDown, new Vector2(970, (int)(Game1.screenHeightAspect * .9 - 2)), Color.White);
                else
                {
                    if (nextStoryPage.IsOver())
                        s.Draw(textures["StoryRightActive"], new Rectangle(842, (int)(Game1.screenHeightAspect * .9 - 2), textures["StoryRightActive"].Width, textures["StoryRightActive"].Height), Color.White);
                    else
                        s.Draw(textures["StoryRight"], new Rectangle(842, (int)(Game1.screenHeightAspect * .9 - 2), textures["StoryRightActive"].Width, textures["StoryRightActive"].Height), Color.White);
                }
            }
            if (storyItemPage > 0)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dUp, new Vector2(856, (int)(Game1.screenHeightAspect * .9 - 2)), Color.White);
                else
                {
                    if (previousStoryPage.IsOver())
                        s.Draw(textures["StoryLeftActive"], new Rectangle(842, (int)(Game1.screenHeightAspect * .9 - 2), textures["StoryRightActive"].Width, textures["StoryRightActive"].Height), Color.White);
                    else
                        s.Draw(textures["StoryLeft"], new Rectangle(842, (int)(Game1.screenHeightAspect * .9 - 2), textures["StoryRightActive"].Width, textures["StoryRightActive"].Height), Color.White);
                }
            }

            #endregion

            #region Draw Stats
            s.DrawString(Game1.twConQuestHudName, "Level " + player.Level + ": " + player.SocialRank, new Vector2(506 - Game1.twConQuestHudName.MeasureString("Level " + player.Level + ": " + player.SocialRank).X * 1.1f / 2, (int)(Game1.screenHeightAspect * .75 - 17)), Color.White, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);

            s.DrawString(Game1.twConQuestHudInfo, Math.Round(player.Money, 2).ToString("N2"), new Vector2(420, (int)(Game1.screenHeightAspect * .9 - 10)), Color.Black);
            s.DrawString(Game1.twConQuestHudInfo, player.Experience.ToString() + "/" + player.ExperienceUntilLevel.ToString(), new Vector2(420, (int)(Game1.screenHeightAspect * .85 - 10)), Color.Black);
            s.DrawString(Game1.twConQuestHudInfo, player.realMaxHealth.ToString(), new Vector2(420, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Black);
            s.DrawString(Game1.twConQuestHudInfo, player.realStrength.ToString(), new Vector2(586, (int)(Game1.screenHeightAspect * .85 - 11)), Color.Black);
            s.DrawString(Game1.twConQuestHudInfo, player.realDefense.ToString(), new Vector2(580, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Black);
            s.DrawString(Game1.twConQuestHudInfo, player.Karma.ToString(), new Vector2(583, (int)(Game1.screenHeightAspect * .9 - 10)), Color.Black);
            if(SocialRankManager.allSocialRanks.Count > player.SocialRankIndex)
                Game1.OutlineFont(Game1.twConQuestHudInfo, s, "(Next at " + SocialRankManager.allSocialRanks[player.SocialRankIndex].karmaNeeded + ")", 1, 583 + (int)Game1.twConQuestHudInfo.MeasureString(player.Karma.ToString()).X + 5, (int)(Game1.screenHeightAspect * .9 - 10), Color.White, Color.DarkRed);
            #region Stat Modifiers
            if (player.defenseModifier > 0)
                s.DrawString(Game1.twConQuestHudInfo, "(+" + player.defenseModifier.ToString() + "%)", new Vector2(582 + Game1.twConQuestHudInfo.MeasureString(player.realDefense.ToString()).X, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Green);
            else if (player.defenseModifier < 0)
                s.DrawString(Game1.twConQuestHudInfo, "(" + player.defenseModifier.ToString() + "%)", new Vector2(582 + Game1.twConQuestHudInfo.MeasureString(player.realDefense.ToString()).X, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Red);

            if (player.strengthModifier > 0)
                s.DrawString(Game1.twConQuestHudInfo, "(+" + player.strengthModifier.ToString() + "%)", new Vector2(588 + Game1.twConQuestHudInfo.MeasureString(player.realStrength.ToString()).X, (int)(Game1.screenHeightAspect * .85 - 11)), Color.Green);
            else if (player.strengthModifier < 0)
                s.DrawString(Game1.twConQuestHudInfo, "(" + player.strengthModifier.ToString() + "%)", new Vector2(588 + Game1.twConQuestHudInfo.MeasureString(player.realStrength.ToString()).X, (int)(Game1.screenHeightAspect * .85 - 11)), Color.Red);

            if (player.healthModifier > 0)
                s.DrawString(Game1.twConQuestHudInfo, "(+" + player.healthModifier.ToString() + "%)", new Vector2(422 + Game1.twConQuestHudInfo.MeasureString(player.realMaxHealth.ToString()).X, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Green);
            else if (player.healthModifier < 0)
                s.DrawString(Game1.twConQuestHudInfo, "(" + player.healthModifier.ToString() + "%)", new Vector2(422 + Game1.twConQuestHudInfo.MeasureString(player.realMaxHealth.ToString()).X, (int)(Game1.screenHeightAspect * .8 - 11)), Color.Red);

            #endregion

            if (player.Textbooks == 0)
            {
                s.DrawString(Game1.font, "0", new Vector2(504, (int)(Game1.screenHeightAspect * .91 + 1)), Color.White); //630
            }
            else if (player.Textbooks < 10)
            {
                s.DrawString(Game1.font, "0", new Vector2(499, (int)(Game1.screenHeightAspect * .91 + 1)), Color.White);

                s.DrawString(Game1.font, player.Textbooks.ToString(), new Vector2(509, (int)(Game1.screenHeightAspect * .91 + 1)), Color.White);
            }
            else
            {
                s.DrawString(Game1.font, player.Textbooks.ToString(), new Vector2(499, (int)(Game1.screenHeightAspect * .91 + 1)), Color.White);
            }
            #endregion
            
            #region Draw equipped items
            for (int i = 0; i < buttons.Count; i++)
            {
                if (player.EquippedWeapon != null)
                {
                    s.Draw(player.EquippedWeapon.Icon, new Rectangle(weaponEquip1.ButtonRec.X, weaponEquip1.ButtonRec.Y, Game1.equipmentTextures[player.EquippedWeapon.Name].Width, Game1.equipmentTextures[player.EquippedWeapon.Name].Height), Color.White);
                }
                if (player.SecondWeapon != null)
                {
                    s.Draw(player.SecondWeapon.Icon, new Rectangle(weaponEquip2.ButtonRec.X, weaponEquip2.ButtonRec.Y, Game1.equipmentTextures[player.SecondWeapon.Name].Width, Game1.equipmentTextures[player.SecondWeapon.Name].Height), Color.White);
                }

                if (player.EquippedHoodie != null)
                {
                    s.Draw(player.EquippedHoodie.Icon, new Rectangle(toleranceEquip.ButtonRec.X, toleranceEquip.ButtonRec.Y, Game1.equipmentTextures[player.EquippedHoodie.Name].Width, Game1.equipmentTextures[player.EquippedHoodie.Name].Height), Color.White);
                }
                if (player.EquippedHat != null)
                {
                    s.Draw(player.EquippedHat.Icon, new Rectangle(motivationEquip.ButtonRec.X, motivationEquip.ButtonRec.Y, Game1.equipmentTextures[player.EquippedHat.Name].Width, Game1.equipmentTextures[player.EquippedHat.Name].Height), Color.White);
                }

                if (player.EquippedAccessory != null)
                {
                    s.Draw(player.EquippedAccessory.Icon, new Rectangle(miscEquip1.ButtonRec.X, miscEquip1.ButtonRec.Y, Game1.equipmentTextures[player.EquippedAccessory.Name].Width, Game1.equipmentTextures[player.EquippedAccessory.Name].Height), Color.White);
                }
                if (player.SecondAccessory != null)
                {
                    s.Draw(player.SecondAccessory.Icon, new Rectangle(miscEquip2.ButtonRec.X, miscEquip2.ButtonRec.Y, Game1.equipmentTextures[player.SecondAccessory.Name].Width, Game1.equipmentTextures[player.SecondAccessory.Name].Height), Color.White);
                }

            }
            #endregion

            //TOOLTIPS FOR THE FIRST TIME USING THE INVENTORY
            if (game.Prologue.PrologueBooleans["firstInventory"] && game.Prologue.PrologueBooleans["markerGiven"])
            {
                if(Game1.gamePadConnected)
                    s.Draw(textures["inventoryTipController"], new Vector2(450, 50), Color.White);
                else
                    s.Draw(textures["inventoryTip"], new Vector2(450, 50), Color.White);
            }
            ////FIRST TIME EQUIPPING AN ITEM
            //else if (game.Prologue.PrologueBooleans["firstEquipped"] == true && (player.EquippedAccessory != null || player.EquippedHat != null || player.EquippedHoodie != null || player.EquippedWeapon != null))
            //{
            //    game.Prologue.PrologueBooleans["firstEquipped"] = false;
            //    Chapter.effectsManager.AddToolTip("Your equipped items and stats are displayed on the left \npage. Double click an equipped item to unequip it.", 50, 0);
            //}

            #region Story Items
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
                    int boxNumber = i + (storyItemPage * storyItemBoxes.Count);
                    s.Draw(Game1.storyItemIcons[player.StoryItems.ElementAt(boxNumber).Key], storyItemBoxes[i].ButtonRec, Color.White);

                    Game1.OutlineFont(Game1.font, s, player.StoryItems.ElementAt(i).Value.ToString(), 2, storyItemBoxes[i].ButtonRec.X + 65 - (int)Game1.font.MeasureString(player.StoryItems.ElementAt(i).Value.ToString()).X, storyItemBoxes[i].ButtonRec.Y + 48, Color.White, Color.Black);
                }
            }

            s.DrawString(Game1.font, "x", new Vector2(827, (int)(Game1.screenHeightAspect * .75 - 19)), Color.Black);
            s.DrawString(Game1.font, player.BronzeKeys.ToString(), new Vector2(842, (int)(Game1.screenHeightAspect * .75 - 16)), Color.Black);
            s.DrawString(Game1.font, "x", new Vector2(925, (int)(Game1.screenHeightAspect * .75 - 19)), Color.Black);
            s.DrawString(Game1.font, player.SilverKeys.ToString(), new Vector2(940, (int)(Game1.screenHeightAspect * .75 - 16)), Color.Black);
            s.DrawString(Game1.font, "x", new Vector2(1028, (int)(Game1.screenHeightAspect * .75 - 19)), Color.Black);
            s.DrawString(Game1.font, player.GoldKeys.ToString(), new Vector2(1043, (int)(Game1.screenHeightAspect * .75 - 16)), Color.Black);
            #endregion


            //--Draw the inventory boxes
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                if (inventoryBoxes[i].ButtonTexture == Game1.emptyBox)
                    s.Draw(inventoryBoxes[i].ButtonTexture, inventoryBoxes[i].ButtonRec, Color.White * 0f);
                else
                {
                    if (tabState == TabState.loot)
                        s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, inventoryBoxes[i].ButtonTexture.Width, inventoryBoxes[i].ButtonTexture.Height), Color.White);
                    else
                    {
                        int boxNumber = i + (equipmentPage * inventoryBoxes.Count);

                        switch(tabState)
                        {
                            case TabState.weapon:
                            s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X,inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedWeapons[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedWeapons[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.shirts:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedHoodies[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedHoodies[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.hats:
                                s.Draw(Game1.equipmentTextures[player.OwnedHats[boxNumber].Name], new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedHats[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedHats[boxNumber].Name].Height), Color.White);
                                break;
                            case TabState.accessory:
                                s.Draw(inventoryBoxes[i].ButtonTexture, new Rectangle(inventoryBoxes[i].ButtonRec.X, inventoryBoxes[i].ButtonRec.Y, Game1.equipmentTextures[player.OwnedAccessories[boxNumber].Name].Width, Game1.equipmentTextures[player.OwnedAccessories[boxNumber].Name].Height), Color.White);
                                break;
                        }
                    }

                    if (tabState == TabState.loot)
                    {
                        Game1.OutlineFont(Game1.font, s, player.EnemyDrops.ElementAt(i).Value.ToString(), 2, inventoryBoxes[i].ButtonRec.X + 65 - (int)Game1.font.MeasureString(player.EnemyDrops.ElementAt(i).Value.ToString()).X, inventoryBoxes[i].ButtonRec.Y + 48, Color.White, Color.Black);
                    }
                }
            }


            
            for (int i = 0; i < inventoryBoxes.Count; i++)
            {
                //--Draw description Boxes
                int boxNumber = i + (equipmentPage * inventoryBoxes.Count);
                if (inventoryBoxes[i].IsOver() && !showingPassives)
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

            for (int i = 0; i < 4; i++)
            {
                int boxNumber = i + (storyItemPage * 3);
                if (storyItemBoxes[i].IsOver() && !showingPassives)
                {
                    if (player.StoryItems.Count > boxNumber)
                    {
                        descriptionBoxManager.DrawStoryItemDescriptions(player.StoryItems.ElementAt(boxNumber).Key, storyItemBoxes[i].ButtonRec);
                    }
                }
            }

            #region Icon tool-tips on hover
            if (healthIcon.IsOver())
                DrawIconInfoBox(s, "Health");
            else if (defenseIcon.IsOver())
                DrawIconInfoBox(s, "Defense");
            else if (strengthIcon.IsOver())
                DrawIconInfoBox(s, "Strength");
            else if (karmaIcon.IsOver())
                DrawIconInfoBox(s, "Karma");
            else if (moneyIcon.IsOver())
                DrawIconInfoBox(s, "Money");
            else if (experienceIcon.IsOver())
                DrawIconInfoBox(s, "Experience");
            else if (bronzeKeyIcon.IsOver())
                DrawIconInfoBox(s, "Bronze Keys");
            else if (silverKeyIcon.IsOver())
                DrawIconInfoBox(s, "Silver Keys");
            else if (goldKeyIcon.IsOver())
                DrawIconInfoBox(s, "Gold Keys");
            else if (textbookIcon.IsOver())
                DrawIconInfoBox(s, "Textbooks");

            else if (timeOverWeaponIcon > 20)
                DrawIconInfoBox(s, "Weapons");
            else if (timeOverHatIcon > 20)
                DrawIconInfoBox(s, "Hats");
            else if (timeOverShirtIcon > 20)
                DrawIconInfoBox(s, "Outfits");
            else if (timeOverAccessoryIcon > 20)
                DrawIconInfoBox(s, "Accessories");
            else if (timeOverLootIcon > 20)
                DrawIconInfoBox(s, "Loot");
            #endregion

            if(!showingPassives)
                DrawEquippedDescriptions();

            if (showingPassives)
            {
                s.Draw(textures["PassivePage"], new Rectangle(0, 0, textures["PassivePage"].Width, textures["PassivePage"].Height), Color.White);

                for (int i = 0; i < 8; i++)
                {
                    int currentPassiveIndex = i + (passivePage * 8);

                    if (currentPassiveIndex < player.OwnedPassives.Count)
                    {
                        s.Draw(passiveBoxes[player.OwnedPassives[currentPassiveIndex].Name.ToLower()], passivePositions[i], Color.White);
                    }
                }

                if (!Game1.gamePadConnected)
                {
                    if (passivesLeft.IsOver())
                        s.Draw(textures["PassiveLeftActive"], new Rectangle(598, (int)(Game1.screenHeightAspect * .9 + 7), textures["PassiveLeft"].Width, textures["PassiveLeft"].Height), Color.White);
                    else
                        s.Draw(textures["PassiveLeft"], new Rectangle(598, (int)(Game1.screenHeightAspect * .9 + 7), textures["PassiveLeft"].Width, textures["PassiveLeft"].Height), Color.White);
                }
                else
                    s.Draw(DarylsNotebook.dLeft, new Vector2(615, 668), Color.White);

                s.DrawString(Game1.font, "pg " + (passivesPage + 1).ToString() + "/" + ((player.OwnedPassives.Count / 8) + 1).ToString(), new Vector2(663, (int)(Game1.screenHeightAspect * .95 - 7)), Color.White);

                if (!Game1.gamePadConnected)
                {
                    if (passivesRight.IsOver())
                        s.Draw(textures["PassiveRightActive"], new Rectangle(598, (int)(Game1.screenHeightAspect * .9 + 7), textures["PassiveLeft"].Width, textures["PassiveLeft"].Height), Color.White);
                    else
                        s.Draw(textures["PassiveRight"], new Rectangle(598, (int)(Game1.screenHeightAspect * .9 + 7), textures["PassiveLeft"].Width, textures["PassiveLeft"].Height), Color.White);
                }
                else
                    s.Draw(DarylsNotebook.dRight, new Vector2(733, 668), Color.White);

            }
        }

        public void DrawEquippedDescriptions()
        {
            #region EQUIPMENT
            if (weaponEquip1.IsOver() && player.EquippedWeapon != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.EquippedWeapon, weaponEquip1.ButtonRec);
            }

            if (weaponEquip2.IsOver() && player.SecondWeapon != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.SecondWeapon, weaponEquip2.ButtonRec);
            }

            if (motivationEquip.IsOver() && player.EquippedHat != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.EquippedHat, motivationEquip.ButtonRec);
            }

            if (toleranceEquip.IsOver() && player.EquippedHoodie != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.EquippedHoodie, toleranceEquip.ButtonRec);
            }

            if (miscEquip1.IsOver() && player.EquippedAccessory != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.EquippedAccessory, miscEquip1.ButtonRec);
            }

            if (miscEquip2.IsOver() && player.SecondAccessory != null)
            {
                descriptionBoxManager.DrawEquipDescriptions(player.SecondAccessory, miscEquip2.ButtonRec);
            }
            #endregion
        }

        public void DrawEquippedText(String name, int addOrRemove)
        {

            if (addOrRemove == 1)
            {
                equippedText = "You have equipped " + name;
            }
            if (addOrRemove == -1)
            {
                equippedText = "You have removed " + name;
            }
            if (addOrRemove == 0)
            {
                equippedText = name;
            }
            textTimer = 90;
        }

    }
}