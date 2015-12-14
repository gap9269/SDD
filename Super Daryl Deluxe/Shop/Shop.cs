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
    public class Shop : BaseMenu
    {
        Button toBuy, toSell, buyItem, sellItem, sellOne, sellFive, sellAll;

        TrenchcoatKid trenchcoatKid;
        List<Button> saleButtons;
        Texture2D equipBox;
        ContentManager Content;

        List<Vector2> boxPositions;

        float rayRotation;

        Dictionary<String, Texture2D> inventoryTextures;

        Texture2D filter, kid, kidBlurred, clipboard, piggy, itemBoxes, sellStatic, sellActive, buyStatic, buyActive, shopStatic, shopActive, sell1Static, sell1Active, sell5Static, sell5Active, sellAllStatic, sellAllActive, cantSell5, cantBuy, piggyRay, keys, equipBoxTexture, otherBoxTexture, backspace, b;

        ItemForSale selectedItem;
        object selectedSaleItem;

        int selectedItemIndex;

        Texture2D currentItemTexture;
        String currentDescription;
        float currentCost;
        Boolean showBuyButton = false;
        Button backspaceButton;

        PlayerInventoryInShop playerInventory;

        float kidPosY, clipboardPosY;
        float blurAlpha = 0f;

        public enum State
        {
            opening,
            buying,
            selling
        }
        public State state;

        int openingState; //Increment this over time to decide when to draw additional stuff during the opening scene
        int openingTimer;

        Player player;

        public TrenchcoatKid TrenchcoatKid { get { return trenchcoatKid; } set { trenchcoatKid = value; } }
        public Shop(Game1 g, Player p):base
            (Game1.whiteFilter, g)
        {
            player = p;
            saleButtons = new List<Button>();
            equipBox = Game1.emptyBox;
            inventoryTextures = new Dictionary<string, Texture2D>();
            playerInventory = new PlayerInventoryInShop(player, inventoryTextures, game.DescriptionBoxManager, game);

            Content = new ContentManager(g.Services);
            Content.RootDirectory = "Content";

            boxPositions = new List<Vector2>();

            //--Sale buttons
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * i), 196, 70, 70)));
                else if (i < 6)
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * (i - 3)),361, 70, 70)));
                else
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * (i - 6)), 526, 70, 70)));
            }

            //--Box Positions
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                    boxPositions.Add(new Vector2(160 + (141 * i), 140));
                else if (i < 6)
                    boxPositions.Add(new Vector2(160 + (141 * (i - 3)), 140 + 165));
                else
                    boxPositions.Add(new Vector2(160 + (141 * (i - 6)), 140 + 165 * 2));
            }

            buyItem = new Button(new Rectangle(887, 614, 90, 50));

            toBuy = new Button(new Rectangle(825, 250, 99, 56));
            toSell = new Button(new Rectangle(949, 250, 101, 56));

            sellOne = new Button(new Rectangle(764, 610, 99, 50));
            sellFive = new Button(new Rectangle(870, 612, 105, 48));
            sellAll = new Button(new Rectangle(979, 610, 107, 50));

            //UpdateResolution();

            backspaceButton = new Button(new Rectangle(1166, 7, 110, 18));

        }

        public void UnloadContent()
        {
            inventoryTextures.Clear();
            Content.Unload();
            Sound.UnloadMenuSounds();
        }

        public void LoadContent()
        {
            Sound.LoadTrenchcoatSounds();
            itemBoxes = Content.Load<Texture2D>(@"Menus\Shop\Boxes");
            buyActive = Content.Load<Texture2D>(@"Menus\Shop\BuyActive");
            cantBuy = Content.Load<Texture2D>(@"Menus\Shop\CantBuy");
            cantSell5 = Content.Load<Texture2D>(@"Menus\Shop\CantSell5");
            clipboard = Content.Load<Texture2D>(@"Menus\Shop\Clipboard");
            kid = Content.Load<Texture2D>(@"Menus\Shop\Kid");
            kidBlurred = Content.Load<Texture2D>(@"Menus\Shop\KidBlurred");
            piggy = Content.Load<Texture2D>(@"Menus\Shop\Piggy");
            sell1Active = Content.Load<Texture2D>(@"Menus\Shop\Sell1Active");
            sell1Static = Content.Load<Texture2D>(@"Menus\Shop\Sell1Static");
            sell5Active = Content.Load<Texture2D>(@"Menus\Shop\Sell5Active");
            sell5Static = Content.Load<Texture2D>(@"Menus\Shop\Sell5Static");
            buyStatic = Content.Load<Texture2D>(@"Menus\Shop\BuyStatic");
            sellActive = Content.Load<Texture2D>(@"Menus\Shop\SellActive");
            sellStatic = Content.Load<Texture2D>(@"Menus\Shop\SellStatic");
            sellAllActive = Content.Load<Texture2D>(@"Menus\Shop\SellAllActive");
            sellAllStatic = Content.Load<Texture2D>(@"Menus\Shop\SellAllStatic");
            keys = Content.Load<Texture2D>(@"Menus\Shop\Keys");
            equipBoxTexture = Content.Load<Texture2D>(@"Menus\Shop\EquipBox");
            otherBoxTexture = Content.Load<Texture2D>(@"Menus\Shop\OtherBox");
            backspace = Content.Load<Texture2D>(@"Menus\BackspaceNotebook");
            b = Content.Load<Texture2D>(@"Menus\B");
            piggyRay = Content.Load<Texture2D>(@"Menus\Shop\PiggyRay");

            filter = Content.Load<Texture2D>(@"Menus\Shop\Shader");
            shopActive = Content.Load<Texture2D>(@"Menus\Shop\ShopActive");
            shopStatic = Content.Load<Texture2D>(@"Menus\Shop\ShopStatic");

            inventoryTextures.Add("Background", Content.Load<Texture2D>(@"Menus\Shop\inventoryPage"));
            inventoryTextures.Add("PassiveTabActive", Content.Load<Texture2D>(@"Inventory\passivesTabActive"));
            inventoryTextures.Add("PassivePage", Content.Load<Texture2D>(@"Inventory\passives"));
            inventoryTextures.Add("newEquipmentIcon", Content.Load<Texture2D>(@"Inventory\newEquipmentIcon"));

            inventoryTextures.Add("AccessoriesPage", Content.Load<Texture2D>(@"Inventory\accessoryTab"));
            inventoryTextures.Add("AccessoryTabActive", Content.Load<Texture2D>(@"Inventory\accessoryActive"));
            inventoryTextures.Add("AccessoryTabStatic", Content.Load<Texture2D>(@"Inventory\accessoryStatic"));

            inventoryTextures.Add("HatsPage", Content.Load<Texture2D>(@"Inventory\hatTab"));
            inventoryTextures.Add("HatsTabActive", Content.Load<Texture2D>(@"Inventory\hatsActive"));
            inventoryTextures.Add("HatsTabStatic", Content.Load<Texture2D>(@"Inventory\hatsStatic"));

            inventoryTextures.Add("WeaponsPage", Content.Load<Texture2D>(@"Inventory\weaponTab"));
            inventoryTextures.Add("WeaponTabActive", Content.Load<Texture2D>(@"Inventory\weaponActive"));
            inventoryTextures.Add("WeaponTabStatic", Content.Load<Texture2D>(@"Inventory\weaponstatic"));

            inventoryTextures.Add("LootPage", Content.Load<Texture2D>(@"Inventory\lootTab"));
            inventoryTextures.Add("LootTabActive", Content.Load<Texture2D>(@"Inventory\lootActive"));
            inventoryTextures.Add("LootTabStatic", Content.Load<Texture2D>(@"Inventory\lootstatic"));

            inventoryTextures.Add("ShirtsPage", Content.Load<Texture2D>(@"Inventory\shirtTab"));
            inventoryTextures.Add("ShirtsTabActive", Content.Load<Texture2D>(@"Inventory\shirtsActive"));
            inventoryTextures.Add("ShirtsTabStatic", Content.Load<Texture2D>(@"Inventory\shirtsStatic"));

            //Arrows
            inventoryTextures.Add("ItemLeft", Content.Load<Texture2D>(@"Inventory\equipmentLeftStatic"));
            inventoryTextures.Add("ItemLeftActive", Content.Load<Texture2D>(@"Inventory\equipmentLeftActive"));
            inventoryTextures.Add("ItemRight", Content.Load<Texture2D>(@"Inventory\equipmentRightStatic"));
            inventoryTextures.Add("ItemRightActive", Content.Load<Texture2D>(@"Inventory\equipmentRightActive"));

            playerInventory = new PlayerInventoryInShop(player, inventoryTextures, game.DescriptionBoxManager, game);

            kidPosY = clipboardPosY = (int)(Game1.aspectRatio * 1280);

            Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_open);
        }

        public void UpdateResolution()
        {
            saleButtons.Clear();
            //--Sale buttons
            for (int i = 0; i < 9; i++)
            {
                if (i < 3)
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * i), (int)(Game1.aspectRatio * 1280 * .27) + 10, 70, 70)));
                else if (i < 6)
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * (i - 3)), (int)(Game1.aspectRatio * 1280 * .5) + 10, 70, 70)));
                else
                    saleButtons.Add(new Button(new Rectangle(218 + (140 * (i - 6)), (int)(Game1.aspectRatio * 1280 * .76) - 14, 70, 70)));
            }

            buyItem.ButtonRecY = 575;
            toBuy.ButtonRecY = 250;
            toSell.ButtonRecY = 250;

            sellOne.ButtonRecY = 550;
            sellFive.ButtonRecY = 553;
            sellAll.ButtonRecY = 550;
        }

        public override void Update()
        {
            base.Update();

            
            //Rotate the ray
            rayRotation+= .5f;
            if (rayRotation == 360)
                rayRotation = 0;

            if (state != State.opening)
            {

                #region Change States
                if (KeyPressed(Keys.Back) || MyGamePad.BPressed() || backspaceButton.Clicked())
                {
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    selectedItem = null;
                    showBuyButton = false;
                    state = State.opening;
                    Chapter.effectsManager.RemoveToolTip();
                    ResetSaleBoxes();
                    UnloadContent();
                    kidPosY = clipboardPosY = (int)(Game1.aspectRatio * 1280);
                    openingTimer = 0;
                    openingState = 0;
                    blurAlpha = 0f;
                    MyGamePad.ResetStates();

                }
                if (toBuy.Clicked())
                {
                    selectedSaleItem = null;
                    playerInventory.selectedItem = null;
                    playerInventory.selectedItemIndex = 0;
                    showBuyButton = false;
                    state = State.buying;
                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_01);

                }
                if (toSell.Clicked())
                {
                    selectedItem = null;
                    showBuyButton = false;
                    state = State.selling;
                    playerInventory.ResetInventoryBoxes();
                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_01);

                }
                #endregion

                #region BUYING
                if (state == State.buying)
                {
                    //#region TOOLTIPS FOR THE FIRST TIME IN THE SHOP
                    //if (game.Prologue.PrologueBooleans["firstTrench"] == true)
                    //{
                    //    game.Prologue.PrologueBooleans["firstTrench"] = false;
                    //    Chapter.effectsManager.AddToolTip("Here you can purchase items that the Trenchcoat Crony\nhas for sale. Click an item to see its details on the \nright side. To sell your own items, click \"Sell\"", 200, 10);
                    //}
                    //else if (game.Prologue.PrologueBooleans["firstTrenchSelectedItem"] == true && selectedItem != null)
                    //{
                    //    Chapter.effectsManager.AddToolTip("Here are the item's details and your current Lunch\nMoney. Click \"Buy\" to purchase the item if you have\nenough money.", 200, 10);
                    //}
                    //#endregion

                    UpdateSalesButtons();

                    //Loop through all of the items on salle
                    for (int i = 0; i < trenchcoatKid.ItemsOnSale.Count; i++)
                    {
                        //If you click on an item, select it
                        if (saleButtons[i].Clicked() && trenchcoatKid.SoldOut[i] == false)
                        {
                            selectedItem = trenchcoatKid.ItemsOnSale[i];
                            showBuyButton = true;
                            selectedItemIndex = i;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_select);

                            if (game.Prologue.PrologueBooleans["firstTrenchSelectedItem"] == true)
                            {
                                game.Prologue.PrologueBooleans["firstTrenchSelectedItem"] = false;
                                Chapter.effectsManager.RemoveToolTip();
                            }
                        }
                    }

                    //If you click buy and have enough money
                    if (buyItem.Clicked() && selectedItem != null && player.Money >= selectedItem.cost)
                    {
                        #region ADD ITEM TO INVENTORY
                        if (selectedItem is TextbookForSale)
                        {
                            player.Textbooks++;
                        }

                        if (selectedItem is KeyForSale)
                        {
                            if ((selectedItem as KeyForSale).keyType == KeyForSale.KeyType.Bronze)
                                player.BronzeKeys++;
                            else if ((selectedItem as KeyForSale).keyType == KeyForSale.KeyType.Silver)
                                player.SilverKeys++;
                            else if ((selectedItem as KeyForSale).keyType == KeyForSale.KeyType.Gold)
                                player.GoldKeys++;
                        }

                        if (selectedItem is WeaponForSale)
                        {
                            player.AddWeaponToInventory((selectedItem as WeaponForSale).weapon);
                        }

                        if (selectedItem is OutfitForSale)
                        {
                            player.AddShirtToInventory((selectedItem as OutfitForSale).hoodie);
                        }

                        if (selectedItem is AccessoryForSale)
                        {
                            player.AddAccessoryToInventory((selectedItem as AccessoryForSale).access);
                        }

                        if (selectedItem is HatForSale)
                        {
                            player.AddHatToInventory((selectedItem as HatForSale).hat);
                        }

                        if (selectedItem is StoryItemForSale)
                        {
                            if (player.StoryItems.ContainsKey(selectedItem.name))
                                player.StoryItems[selectedItem.name]++;
                            else
                                player.StoryItems.Add(selectedItem.name, 1);

                        }
                        #endregion

                        //--KEEP THIS AT THE END
                        player.Money -= selectedItem.cost;
                        trenchcoatKid.SoldOut[selectedItemIndex] = true;
                        selectedItem = null;
                        showBuyButton = false;
                        ResetSaleBoxes();
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_buy);

                    }


                }
                #endregion

                #region SELLING
                if (state == State.selling)
                {
                    //if (game.Prologue.PrologueBooleans["firstTrenchSell"] == true)
                    //{
                    //    game.Prologue.PrologueBooleans["firstTrenchSell"] = false;
                    //    Chapter.effectsManager.AddToolTip("Here you can sell any equipment or loot you have. Just \nclick it in your inventory to see the details on the right, \nand then click one of the sale options. Click \"Shop\" to \nreturn to the shop.", 600, 20);
                    //}

                    playerInventory.Update();
                    selectedSaleItem = playerInventory.selectedItem;
                    selectedItemIndex = playerInventory.selectedItemIndex;

                    #region SELLING AN ITEM
                    if (selectedSaleItem != null)
                    {
                        if (sellOne.Clicked())
                        {
                            if (selectedSaleItem is Weapon)
                            {
                                player.Money += (selectedSaleItem as Weapon).SellPrice;
                                player.OwnedWeapons.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Hat)
                            {
                                player.Money += (selectedSaleItem as Hat).SellPrice;
                                player.OwnedHats.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Outfit)
                            {
                                player.Money += (selectedSaleItem as Outfit).SellPrice;
                                player.OwnedHoodies.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Accessory)
                            {
                                player.Money += (selectedSaleItem as Accessory).SellPrice;
                                player.OwnedAccessories.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is DropType)
                            {
                                player.Money += (selectedSaleItem as DropType).buyCost;
                                player.EnemyDrops[(selectedSaleItem as DropType).name]--;

                                if ((selectedSaleItem as DropType).name == "Guano" && !Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
                                {
                                    Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"] = true;
                                }

                                if (player.EnemyDrops[(selectedSaleItem as DropType).name] == 0)
                                {
                                    player.EnemyDrops.Remove((selectedSaleItem as DropType).name);
                                    playerInventory.selectedItemIndex = 0;
                                    playerInventory.selectedItem = null;
                                    selectedSaleItem = null;
                                    playerInventory.ResetInventoryBoxes();
                                }

                            }

                            Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_sell);

                        }
                        else if (sellFive.Clicked() && selectedSaleItem is DropType && player.EnemyDrops[(selectedSaleItem as DropType).name] >= 5)
                        {
                            player.Money += (selectedSaleItem as DropType).buyCost * 5;
                            player.EnemyDrops[(selectedSaleItem as DropType).name] -= 5;

                            if ((selectedSaleItem as DropType).name == "Guano" && !Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
                            {
                                Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"] = true;
                            }

                            if (player.EnemyDrops[(selectedSaleItem as DropType).name] == 0)
                            {
                                player.EnemyDrops.Remove((selectedSaleItem as DropType).name);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }

                            Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_sell);

                        }
                        else if (sellAll.Clicked())
                        {
                            if (selectedSaleItem is Weapon)
                            {
                                player.Money += (selectedSaleItem as Weapon).SellPrice;
                                player.OwnedWeapons.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Hat)
                            {
                                player.Money += (selectedSaleItem as Hat).SellPrice;
                                player.OwnedHats.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Outfit)
                            {
                                player.Money += (selectedSaleItem as Outfit).SellPrice;
                                player.OwnedHoodies.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is Accessory)
                            {
                                player.Money += (selectedSaleItem as Accessory).SellPrice;
                                player.OwnedAccessories.RemoveAt(selectedItemIndex);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }
                            else if (selectedSaleItem is DropType)
                            {

                                if ((selectedSaleItem as DropType).name == "Guano" && !Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
                                {
                                    Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"] = true;
                                }

                                player.Money += (selectedSaleItem as DropType).buyCost * (player.EnemyDrops[(selectedSaleItem as DropType).name]);

                                player.EnemyDrops.Remove((selectedSaleItem as DropType).name);
                                playerInventory.selectedItemIndex = 0;
                                playerInventory.selectedItem = null;
                                selectedSaleItem = null;
                                playerInventory.ResetInventoryBoxes();
                            }

                            Sound.PlaySoundInstance(Sound.SoundNames.ui_trenchcoat_sell);

                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                openingTimer++;

                if (openingState == 0)
                {
                    if (kidPosY > 72 && openingTimer > 30)
                    {
                        float distance = kidPosY - 50;

                        kidPosY -= 3 * (distance / 50);

                        if (kidPosY <= 73)
                        {
                            kidPosY = 72;
                        }

                    }
                    if (clipboardPosY > 72 && openingTimer > 45)
                    {
                        float distance = clipboardPosY - 50;
                        clipboardPosY -= 3 * (distance / 50);

                        if (clipboardPosY <= 73)
                        {
                            clipboardPosY = 72;
                            openingState++;
                            openingTimer = 0;
                        }
                    }
                }
                else
                {

                    blurAlpha += .03f;

                    if (blurAlpha >= 1)
                    {
                        blurAlpha = 1;
                        state = State.buying;
                    }
                    /* This makes some stuff pop up one at a time during the opening scene
                    if (openingTimer >= 10)
                    {
                        openingTimer = 0;
                        openingState++;
                    }

                    if (openingState == 3)
                    {
                        state = State.buying;
                    }*/
                }
            }
        }

        //--Reset all of the sales textures back to empty
        public void ResetSaleBoxes()
        {
            for (int i = 0; i < saleButtons.Count; i++)
            {
                saleButtons[i].ButtonTexture = equipBox;
            }
        }

        public void UpdateSalesButtons()
        {
            for (int i = 0; i < trenchcoatKid.ItemsOnSale.Count; i++)
            {
                if (trenchcoatKid.SoldOut[i] == false)
                {
                    saleButtons[i].ButtonTexture = trenchcoatKid.ItemsOnSale[i].icon;
                }
                else
                {
                    saleButtons[i].ButtonTexture = equipBox;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(filter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            if(blurAlpha != -1)
                s.Draw(kid, new Rectangle(337, (int)kidPosY, kid.Width, kid.Height), Color.White);


            s.Draw(kidBlurred, new Rectangle(337, 72, kid.Width, kid.Height), Color.White * blurAlpha);

            s.Draw(clipboard, new Rectangle(337, (int)clipboardPosY, clipboard.Width, clipboard.Height), Color.White);

            if (state != State.opening || openingState >= 1)
            {
                if (toSell.IsOver() && state != State.opening)
                    s.Draw(sellActive, new Rectangle(toSell.ButtonRecX - 30, toSell.ButtonRecY - 30, 161, 116), Color.White);
                else
                    s.Draw(sellStatic, new Rectangle(toSell.ButtonRecX - 30, toSell.ButtonRecY - 30, 161, 116), Color.White);

                if (toBuy.IsOver() && state != State.opening)
                    s.Draw(shopActive, new Rectangle(toBuy.ButtonRecX - 33, toBuy.ButtonRecY - 30, 161, 116), Color.White);
                else
                    s.Draw(shopStatic, new Rectangle(toBuy.ButtonRecX - 33, toBuy.ButtonRecY - 30, 161, 116), Color.White);


            }
            if (state == State.opening)
            {
                if (openingState >= 2)
                {
                    String money = Math.Round(player.Money, 2).ToString("N2");
                    s.Draw(piggyRay, new Rectangle(11 + 169 / 2, 3 + 169 / 2, piggyRay.Width, piggyRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(piggyRay.Width / 2, piggyRay.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(piggy, new Rectangle(11, 3, 169, 169), Color.White);
                    s.DrawString(Game1.TwCondensedSmallFont, "$" + money, new Vector2(91 - Game1.TwCondensedSmallFont.MeasureString("$" + money).X / 2, (int)(Game1.aspectRatio * 1280 * .11)), Color.White);
                }
            }

            if (state == State.buying)
            {
                for (int i = 0; i < trenchcoatKid.ItemsOnSale.Count; i++)
                {
                    s.Draw(itemBoxes, new Rectangle((int)boxPositions[i].X, (int)boxPositions[i].Y, 191, 191), Color.White);
                }

                #region Money
                String money = Math.Round(player.Money, 2).ToString("N2"); //Money string
                //Rays
                s.Draw(piggyRay, new Rectangle(11 + 169 / 2, 3 + 169 / 2, piggyRay.Width, piggyRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(piggyRay.Width / 2, piggyRay.Height / 2), SpriteEffects.None, 0f);
                //Piggy bank
                s.Draw(piggy, new Rectangle(11, 3, 169, 169), Color.White);
                //Money text
                s.DrawString(Game1.TwCondensedSmallFont, "$" + money, new Vector2(91 - Game1.TwCondensedSmallFont.MeasureString("$" + money).X / 2, (int)(Game1.aspectRatio * 1280 * .11)), Color.White);
                #endregion

                s.Draw(keys, new Rectangle(208, 8, keys.Width, keys.Height), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(256, 35), Color.White);
                s.DrawString(Game1.font, player.BronzeKeys.ToString(), new Vector2(271, 38), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(359, 38), Color.White);
                s.DrawString(Game1.font, player.SilverKeys.ToString(), new Vector2(374, 38), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(462, 38), Color.White);
                s.DrawString(Game1.font, player.GoldKeys.ToString(), new Vector2(477, 38), Color.White);

                for (int i = 0; i < saleButtons.Count; i++)
                {
                    if (saleButtons[i].ButtonTexture != Game1.emptyBox)
                    {
                        #region Draw the correct shit
                        if (trenchcoatKid.ItemsOnSale[i] is TextbookForSale)
                        {
                            s.Draw(Game1.textbookTextures, saleButtons[i].ButtonRec, new Rectangle(0 + (94 * (trenchcoatKid.ItemsOnSale[i] as TextbookForSale).textureNum), 0, 94, 90), Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is KeyForSale)
                        {
                            s.Draw((trenchcoatKid.ItemsOnSale[i] as KeyForSale).icon, saleButtons[i].ButtonRec, Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is WeaponForSale)
                        {
                            s.Draw(Game1.equipmentTextures[(trenchcoatKid.ItemsOnSale[i].name)], saleButtons[i].ButtonRec, Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is OutfitForSale)
                        {
                            s.Draw(Game1.equipmentTextures[(trenchcoatKid.ItemsOnSale[i].name)], saleButtons[i].ButtonRec, Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is AccessoryForSale)
                        {
                            s.Draw(Game1.equipmentTextures[(trenchcoatKid.ItemsOnSale[i].name)], saleButtons[i].ButtonRec, Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is HatForSale)
                        {
                            s.Draw(Game1.equipmentTextures[(trenchcoatKid.ItemsOnSale[i].name)], saleButtons[i].ButtonRec, Color.White);
                        }

                        if (trenchcoatKid.ItemsOnSale[i] is StoryItemForSale)
                        {
                            s.Draw(Game1.storyItemIcons[trenchcoatKid.ItemsOnSale[i].name], saleButtons[i].ButtonRec, Color.White);
                        }
                        #endregion

                        if (player.Money >= trenchcoatKid.ItemsOnSale[i].cost)
                            s.DrawString(Game1.TwCondensedSmallFont, "$" + trenchcoatKid.ItemsOnSale[i].cost.ToString("N2"), new Vector2(saleButtons[i].ButtonRec.Center.X - Game1.TwCondensedSmallFont.MeasureString("$" + trenchcoatKid.ItemsOnSale[i].cost.ToString("N2")).X / 2, saleButtons[i].ButtonRecY + 80), Color.White);
                        else
                            s.DrawString(Game1.TwCondensedSmallFont, "$" + trenchcoatKid.ItemsOnSale[i].cost.ToString("N2"), new Vector2(saleButtons[i].ButtonRec.Center.X - Game1.TwCondensedSmallFont.MeasureString("$" + trenchcoatKid.ItemsOnSale[i].cost.ToString("N2")).X / 2, saleButtons[i].ButtonRecY + 80), Color.White);
                    }
                }

                if (selectedItem != null)
                {
                    String cost = Math.Round(selectedItem.cost, 2).ToString("N2");

                    #region DRAW ICON AND TYPE

                    s.DrawString(Game1.pickUpFont, selectedItem.name, new Vector2(935 - Game1.pickUpFont.MeasureString(selectedItem.name).X / 2, (int)(Game1.aspectRatio * 1280 * .45) - 6), Color.Black);

                    if (selectedItem is TextbookForSale)
                    {
                        s.Draw(otherBoxTexture, new Vector2(690, 415), Color.White);
                        s.Draw(Game1.textbookTextures, new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 10, 70, 70), (selectedItem as TextbookForSale).GetSourceRec(), Color.White);


                        //PRICE
                        int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                        s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 466), Color.White);
                        s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 465), Color.Black);
                        
                    }
                    else if (selectedItem is KeyForSale)
                    {
                        s.Draw(otherBoxTexture, new Vector2(690, 415), Color.White);
                        s.Draw(selectedItem.icon, new Rectangle(935 - selectedItem.icon.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 10, selectedItem.icon.Width, selectedItem.icon.Height), Color.White);

                        //PRICE
                        int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                        s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 466), Color.White);
                        s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 465), Color.Black);
                    }
                    else
                    {
                        if (selectedItem is WeaponForSale)
                        {
                            s.Draw(equipBoxTexture, new Vector2(690, 387), Color.White);
                            s.Draw(Game1.equipmentTextures[(selectedItem.name)], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f), Game1.equipmentTextures[(selectedItem.name)].Width, Game1.equipmentTextures[(selectedItem.name)].Height), Color.White);

                            s.DrawString(Game1.expMoneyFloatingNumFont, "Weapon", new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString("Weapon").X / 2, (int)(Game1.aspectRatio * 1280 * .49) - 10), Color.Gray);
                            //PRICE
                            int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 436), Color.White);
                            s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 435), Color.Black);
                        }

                        if (selectedItem is OutfitForSale)
                        {
                            s.Draw(equipBoxTexture, new Vector2(690, 387), Color.White);
                            s.Draw(Game1.equipmentTextures[(selectedItem.name)], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f), Game1.equipmentTextures[(selectedItem.name)].Width, Game1.equipmentTextures[(selectedItem.name)].Height), Color.White);

                            s.DrawString(Game1.expMoneyFloatingNumFont, "Outfit", new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString("Outfit").X / 2, (int)(Game1.aspectRatio * 1280 * .49) - 10), Color.Gray);

                            //PRICE
                            int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 436), Color.White);
                            s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 435), Color.Black);
                        }

                        if (selectedItem is AccessoryForSale)
                        {
                            s.Draw(equipBoxTexture, new Vector2(690, 387), Color.White);
                            s.Draw(Game1.equipmentTextures[(selectedItem.name)], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f), Game1.equipmentTextures[(selectedItem.name)].Width, Game1.equipmentTextures[(selectedItem.name)].Height), Color.White);

                            s.DrawString(Game1.expMoneyFloatingNumFont, "Accessory", new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString("Accessory").X / 2, (int)(Game1.aspectRatio * 1280 * .49) - 10), Color.Gray);
                            //PRICE
                            int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 436), Color.White);
                            s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 435), Color.Black);
                        }

                        if (selectedItem is HatForSale)
                        {
                            s.Draw(equipBoxTexture, new Vector2(690, 387), Color.White);
                            s.Draw(Game1.equipmentTextures[(selectedItem.name)], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f), Game1.equipmentTextures[(selectedItem.name)].Width, Game1.equipmentTextures[(selectedItem.name)].Height), Color.White);

                            s.DrawString(Game1.expMoneyFloatingNumFont, "Hat", new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString("Hat").X / 2, (int)(Game1.aspectRatio * 1280 * .49) - 10), Color.Gray);

                            //PRICE
                            int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 436), Color.White);
                            s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 435), Color.Black);
                        }

                        if (selectedItem is StoryItemForSale)
                        {
                            s.Draw(otherBoxTexture, new Vector2(690, 415), Color.White);

                            s.Draw(Game1.storyItemIcons[selectedItem.name], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f) + 10, Game1.storyItemIcons[selectedItem.name].Width, Game1.storyItemIcons[selectedItem.name].Height), Color.White);

                            s.DrawString(Game1.expMoneyFloatingNumFont, "Quest Item", new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString("Quest Item").X / 2, (int)(Game1.aspectRatio * 1280 * .49)), Color.Gray);

                            //PRICE
                            int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString(selectedItem.cost.ToString("N2")).X;
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 466), Color.White);
                            s.DrawString(Game1.twConQuestHudName, selectedItem.cost.ToString("N2"), new Vector2(posX, 465), Color.Black);

                        }

                    #endregion
                    }
                    //DRAW THE DESCRIPTION
                    selectedItem.DrawEquipDescription(s, Game1.descriptionFont, new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString(selectedItem.description).X / 2, (int)(Game1.aspectRatio * 1280 * .6f)));



                    #region Draw the buy buttons
                    if (showBuyButton && player.Money >= selectedItem.cost)
                    {
                        if(buyItem.IsOver())
                            s.Draw(buyActive, new Vector2(797, 530), Color.White);
                        else
                            s.Draw(buyStatic, new Vector2(797, 530), Color.White);
                    }
                    else if (showBuyButton && player.Money < selectedItem.cost)
                    {
                        s.Draw(cantBuy, new Vector2(797, 530), Color.Gray);
                    }
                    #endregion
                }
            }


            //Selling
            else if(state == State.selling)
            {
                #region Money
                String money = Math.Round(player.Money, 2).ToString("N2"); //Money string
                //Rays
                s.Draw(piggyRay, new Rectangle(11 + 169 / 2, 3 + 169 / 2, piggyRay.Width, piggyRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(piggyRay.Width / 2, piggyRay.Height / 2), SpriteEffects.None, 0f);
                //Piggy bank
                s.Draw(piggy, new Rectangle(11, 3, 169, 169), Color.White);
                //Money text
                s.DrawString(Game1.TwCondensedSmallFont, "$" + money, new Vector2(91 - Game1.TwCondensedSmallFont.MeasureString("$" + money).X / 2, (int)(Game1.aspectRatio * 1280 * .11)), Color.White);
                #endregion

                s.Draw(keys, new Rectangle(208, 8, keys.Width, keys.Height), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(256, 35), Color.White);
                s.DrawString(Game1.font, player.BronzeKeys.ToString(), new Vector2(271, 38), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(359, 38), Color.White);
                s.DrawString(Game1.font, player.SilverKeys.ToString(), new Vector2(374, 38), Color.White);
                s.DrawString(Game1.font, "x", new Vector2(462, 38), Color.White);
                s.DrawString(Game1.font, player.GoldKeys.ToString(), new Vector2(477, 38), Color.White);

                if (selectedSaleItem != null)
                {
                    if (selectedSaleItem is Equipment)
                    {
                        s.Draw(equipBoxTexture, new Vector2(690, 387), Color.White);
                        #region Draw equipment info
                        //NAME
                        s.DrawString(Game1.pickUpFont, (selectedSaleItem as Equipment).Name, new Vector2(935 - Game1.pickUpFont.MeasureString((selectedSaleItem as Equipment).Name).X / 2, (int)(Game1.aspectRatio * 1280 * .45) - 4), Color.Black);

                        //ICON
                        s.Draw(Game1.equipmentTextures[(selectedSaleItem as Equipment).Name], new Rectangle(935 - equipBox.Width / 2, (int)(Game1.aspectRatio * 1280 * .5f - 4), Game1.equipmentTextures[(selectedSaleItem as Equipment).Name].Width, Game1.equipmentTextures[(selectedSaleItem as Equipment).Name].Height), Color.White);

                        //DESCRIPTION
                        SellingItemWrapper selectedSaleItemDescription = new SellingItemWrapper(selectedSaleItem);
                        selectedSaleItemDescription.DrawEquipDescription(s, Game1.descriptionFont, new Vector2(935 - Game1.expMoneyFloatingNumFont.MeasureString((selectedSaleItem as Equipment).Description).X / 2, (int)(Game1.aspectRatio * 1280 * .6f)));

                        //PRICE
                        int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString((selectedSaleItem as Equipment).SellPrice.ToString("N2")).X;
                        s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 438), Color.White);
                        s.DrawString(Game1.twConQuestHudName, (selectedSaleItem as Equipment).SellPrice.ToString("N2"), new Vector2(posX, 437), Color.Black);
                        #endregion
                    }
                    else if (selectedSaleItem is DropType)
                    {
                        s.Draw(otherBoxTexture, new Vector2(690, 415), Color.White);

                        #region Draw loot info
                        //NAME
                        s.DrawString(Game1.pickUpFont, (selectedSaleItem as DropType).name, new Vector2(935 - Game1.pickUpFont.MeasureString((selectedSaleItem as DropType).name).X / 2, (int)(Game1.aspectRatio * 1280 * .45) - 4), Color.Black);

                        //ICON
                        s.Draw((selectedSaleItem as DropType).texture, new Rectangle(935 - 70 / 2, (int)(Game1.aspectRatio * 1280 * .5f) - 4, 70, 70), Color.White);

                        //DESCRIPTION
                        s.DrawString(Game1.descriptionFont, Game1.WrapText(Game1.descriptionFont, (selectedSaleItem as DropType).description, 315), new Vector2(794, 495), Color.Black);

                        // s.DrawString(Game1.twConQuestHudName, (selectedSaleItem as DropType).buyCost.ToString("N2"), new Vector2(1040, 466), Color.Black);

                        //PRICE
                        int posX = 1105 - (int)Game1.twConQuestHudName.MeasureString((selectedSaleItem as DropType).buyCost.ToString("N2")).X;
                        s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(posX - 25, 466), Color.White);
                        s.DrawString(Game1.twConQuestHudName, (selectedSaleItem as DropType).buyCost.ToString("N2"), new Vector2(posX, 465), Color.Black);
                        #endregion
                    }

                    #region Draw the Sell buttons (1, 5, All)
                    if (sellOne.IsOver())
                        s.Draw(sell1Active, new Rectangle(sellOne.ButtonRecX - 30, 585, sell1Active.Width, sell1Active.Height), Color.White);
                    else
                        s.Draw(sell1Static, new Rectangle(sellOne.ButtonRecX - 30, 585, sell1Active.Width, sell1Active.Height), Color.White);

                    if (sellAll.IsOver())
                        s.Draw(sellAllActive, new Rectangle(sellAll.ButtonRecX - 30, 585, sellAllActive.Width, sellAllActive.Height), Color.White);
                    else
                        s.Draw(sellAllStatic, new Rectangle(sellAll.ButtonRecX - 30, 585, sellAllActive.Width, sellAllActive.Height), Color.White);

                    if (selectedSaleItem is Equipment || player.EnemyDrops[(selectedSaleItem as DropType).name] < 5)
                        s.Draw(cantSell5, new Rectangle(sellFive.ButtonRecX - 30, 585, cantSell5.Width, cantSell5.Height), Color.White);
                    else
                    {
                        if (sellFive.IsOver())
                            s.Draw(sell5Active, new Rectangle(sellFive.ButtonRecX - 30, 585, sell5Active.Width, sell5Active.Height), Color.White);
                        else
                            s.Draw(sell5Static, new Rectangle(sellFive.ButtonRecX - 30, 585, sell5Active.Width, sell5Active.Height), Color.White);
                    }
                    #endregion
                }

                playerInventory.Draw(s);
            }

            if (!Game1.gamePadConnected)
                s.Draw(backspace, new Vector2(1280 - backspace.Width - 5, 5), Color.White);
            else
                s.Draw(b, new Vector2(1280 - backspace.Width + 20, 5), Color.White);

        }
    }


    //Use this to draw the stats for an item that the player is selling
    public class SellingItemWrapper
    {
        Object item;
        public float cost;
        public String description, name, passive;
        protected int health, defense, strength, level;

        public SellingItemWrapper(Object o)
    {
        item = o;

        if (item is Equipment)
        {
            health = (item as Equipment).Health;
            defense = (item as Equipment).Defense;
            strength = (item as Equipment).Strength;
            description = (item as Equipment).Description;
            level = (item as Equipment).Level;
            cost = (item as Equipment).SellPrice;
            if((item as Equipment).PassiveAbility !=null)
                passive = (item as Equipment).PassiveAbility.Name;
        }
    }

        public virtual void DrawEquipDescription(SpriteBatch s, SpriteFont font, Vector2 vec)
        {
            //Only draw stats if it is equipment
            if (item is Equipment)
            {
                if (Game1.Player.Level >= level)
                    s.DrawString(font, level.ToString(), new Vector2(885, 439), Color.Black);
                else
                    s.DrawString(font, level.ToString(), new Vector2(885, 439), Color.Red);
                s.DrawString(font, Game1.WrapText(Game1.descriptionFont, description, 315), new Vector2(794, 461), Color.Black);

                Vector2 healthVec = new Vector2(788, 538);
                Vector2 defenseVec = new Vector2(985, 538);
                Vector2 strengthVec = new Vector2(795, 580);

                s.DrawString(font, "+ " + health.ToString(), new Vector2(788, 538), Color.White);
                s.DrawString(font, "+ " + strength.ToString(), new Vector2(795, 580), Color.White);
                s.DrawString(font, "+ " + defense.ToString(), new Vector2(985, 538), Color.White);

                if (passive != null && passive != "")
                {
                    s.DrawString(font, passive, new Vector2(823, 500), Color.DarkCyan);
                }
                else
                    s.DrawString(font, "No Passive", new Vector2(823, 500), Color.Red);


                #region Draw the stat differences for Accessories
                if (item is Accessory)
                {
                    if (Game1.Player.EquippedAccessory != null)
                    {
                        //Get the difference between the equipped accessory and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedAccessory.Health;
                        int defenseDiff = defense - Game1.Player.EquippedAccessory.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedAccessory.Strength;

                        #region Draw the bonus differences for the first accessory
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion

                        //Get the difference between the second equipped accessory and the one the player is viewing in the shop
                        if (Game1.Player.SecondAccessory != null)
                        {
                            int secondHealthDiff = health - Game1.Player.SecondAccessory.Health;
                            int secondDefenseDiff = defense - Game1.Player.SecondAccessory.Defense;
                            int secondStrengthDiff = strength - Game1.Player.SecondAccessory.Strength;

                            #region Draw the bonus differences for the second accessory
                            if (secondHealthDiff >= 0)
                                s.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondStrengthDiff >= 0)
                                s.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondDefenseDiff >= 0)
                                s.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                            #endregion
                        }
                        else //The player has no second accessory, so it's all positive stat differences
                        {
                            #region Draw the positive stat differences next to the first accessory differences
                            s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            #endregion
                        }

                    }
                    else //No accessories, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Weapons
                if (item is Weapon)
                {
                    //Draw whether or not dual wielding is allowed

                    if ((item as Weapon).CanHoldTwo)
                    {
                        s.Draw(Game1.dualWieldIcon, new Vector2(935, 576), Color.White);
                        s.DrawString(font, "Dual Wield Allowed", new Vector2(977, 582), Color.White, 0, Vector2.Zero, .93f, SpriteEffects.None, 0);
                    }

                    if (Game1.Player.EquippedWeapon != null)
                    {
                        //Get the difference between the equipped weapon and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedWeapon.Health;
                        int defenseDiff = defense - Game1.Player.EquippedWeapon.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedWeapon.Strength;

                        #region Draw the bonus differences for the first Weapon
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion

                        //Get the difference between the second equipped weapon and the one the player is viewing in the shop
                        if (Game1.Player.SecondWeapon != null)
                        {
                            int secondHealthDiff = health - Game1.Player.SecondWeapon.Health;
                            int secondDefenseDiff = defense - Game1.Player.SecondWeapon.Defense;
                            int secondStrengthDiff = strength - Game1.Player.SecondWeapon.Strength;

                            #region Draw the bonus differences for the second Weapon
                            if (secondHealthDiff >= 0)
                                s.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondStrengthDiff >= 0)
                                s.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondDefenseDiff >= 0)
                                s.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                            #endregion
                        }
                        else //The player has no second weapon, so it's all positive stat differences
                        {
                            #region Draw the positive stat differences next to the first accessory differences
                            s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            #endregion
                        }

                    }
                    else //No weapons, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", new Vector2(788, 538) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", new Vector2(795, 580) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", new Vector2(985, 538) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Hats
                if (item is Hat)
                {
                    if (Game1.Player.EquippedHat != null)
                    {
                        //Get the difference between the equipped hat and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedHat.Health;
                        int defenseDiff = defense - Game1.Player.EquippedHat.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedHat.Strength;

                        #region Draw the bonus differences for the first hat
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion
                    }
                    else //No hat, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Outfits
                if (item is Outfit)
                {
                    if (Game1.Player.EquippedHoodie != null)
                    {
                        //Get the difference between the equipped outfit and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedHoodie.Health;
                        int defenseDiff = defense - Game1.Player.EquippedHoodie.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedHoodie.Strength;

                        #region Draw the bonus differences for the first outfit
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion
                    }
                    else //No outfit, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("  +" + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("  +" + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("  +" + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion
            }
        }
    }
}

