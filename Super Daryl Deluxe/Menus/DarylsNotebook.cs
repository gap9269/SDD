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
    public class DarylsNotebook
    {
        PlayerInventory inventory;
        ComboPage comboPage;
        Journal journal;
        BioPage bioPage;
        QuestsPage questsPage;
        public static ContentManager Content;
        public static ContentManager BioNPCAndEnemyContentLoader;
        public static Texture2D newIcon;

        Texture2D staticTabs, inventoryTabActive, bioTabActive, mapTabActive, journalTabActive, comboTabActive, questsTabActive;

        //Notebook tabs
        public static Button inventoryTab, combosTab, journalTab, bioTab, mapsTab, questsTab;

        Boolean firstFrameOverInventory = true;
        Boolean firstFrameOverCombos = true;
        Boolean firstFrameOverMaps = true;
        Boolean firstFrameOverBios = true;
        Boolean firstFrameOverJournal = true;
        Boolean firstFrameOverQuests = true;

        public Dictionary<String, Texture2D> smallCharacterPortraits;
        public Dictionary<String, Texture2D> smallEnemyPortraits;
        Dictionary<String, Texture2D> darylDrawings;
        Dictionary<String, Texture2D> inventoryTextures;
        Dictionary<String, Texture2D> passiveBoxes;
        Dictionary<String, Texture2D> comboTextures;
        Dictionary<String, Texture2D> journalTextures;
        Dictionary<String, Texture2D> bioTextures;
        Dictionary<String, Texture2D> questTextures;
        KeyboardState current;
        KeyboardState last;
        protected Game1 game;
        public enum State
        {
            inventory,
            combos,
            journal,
            bios,
            maps,
            quests
        }
        public State state;

        public ComboPage ComboPage { get { return comboPage; } set { comboPage = value; } }
        public PlayerInventory Inventory { get { return inventory; } }
        public BioPage BioPage { get { return bioPage; } }
        public Journal Journal { get { return journal; } }
        public QuestsPage QuestsPage { get { return questsPage; } }

        public DarylsNotebook(Game1 g)
        {
            game = g;
            inventoryTextures = new Dictionary<string, Texture2D>();
            darylDrawings = new Dictionary<string, Texture2D>();
            comboTextures = new Dictionary<string, Texture2D>();
            journalTextures = new Dictionary<string, Texture2D>();
            bioTextures = new Dictionary<string, Texture2D>();
            questTextures = new Dictionary<string, Texture2D>();
            smallCharacterPortraits = new Dictionary<string, Texture2D>();
            smallEnemyPortraits = new Dictionary<string, Texture2D>();
            passiveBoxes = new Dictionary<string, Texture2D>();

            inventory = new PlayerInventory(Game1.Player, inventoryTextures, darylDrawings, game.DescriptionBoxManager, game, passiveBoxes);
            comboPage = new ComboPage(comboTextures, game);
            journal = new Journal(journalTextures, game);
            bioPage = new BioPage(bioTextures, game);
            questsPage = new QuestsPage(game, questTextures);

            Content = new ContentManager(g.Services);
            Content.RootDirectory = "Content";

            BioNPCAndEnemyContentLoader = new ContentManager(g.Services);
            BioNPCAndEnemyContentLoader.RootDirectory = "Content";

            state = State.inventory;

            inventoryTab = new Button(new Rectangle(0, 82, 177, 53));
            mapsTab = new Button(new Rectangle(0, 163, 177, 53));
            combosTab = new Button(new Rectangle(0, 249, 177, 53));
            journalTab = new Button(new Rectangle(0, 333, 177, 53));
            bioTab = new Button(new Rectangle(0, 416, 177, 53));
            questsTab = new Button(new Rectangle(0, 494, 177, 53));
        }

        public void UpdateGameResolution()
        {
            inventory.UpdateResolution();
            comboPage.UpdateResolution();
            journal.UpdateResolution();
            bioPage.UpdateResolution();
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

                //--Show or hide the inventory
            if (((current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back)) || MyGamePad.BPressed()) && !inventory.showingPassives)
            {
                Chapter.effectsManager.RemoveToolTip();
                
                bioPage.bioState = BioPage.BioState.Character;
                bioPage.Page = 0;
                bioPage.SelectedBioName = null;
                bioPage.selectedIndex = 0;
                bioPage.loadedNPCEnemySprites = false;
                bioPage.UnloadNPCAndEnemySprites();

                journal.chapterState = Journal.ChapterState.none;
                journal.insideChapterState = Journal.InsideChapterState.story;
                journal.selectedQuest = null;
                journal.ViewingSpecificQuest = false;
                journal.selectedIndex = 0;
                journal.questPage = 0;
                journal.synopsisPages.Clear();

                inventory.tabState = PlayerInventory.TabState.weapon;
                inventory.StoryItemPage = 0;
                inventory.EquipmentPage = 0;

                questsPage.questPage = 0;
                questsPage.dialoguePage =0;
                questsPage.selectedQuest = null;
                questsPage.selectedIndex = 0;
                questsPage.currentPageDialogueHeight = 0;
                questsPage.questDialogueAndPage.Clear();

                comboPage.Page = 0;

                game.CurrentChapter.state = Chapter.GameState.Game;

                state = State.inventory;

                Sound.PlaySoundInstance(Sound.SoundNames.UIClose);

                UnloadContent();
            }

            if (current.IsKeyUp(Keys.I) && last.IsKeyDown(Keys.I) && state != State.inventory)
            {
                state = State.inventory;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (current.IsKeyUp(Keys.K) && last.IsKeyDown(Keys.K) && state != State.quests)
            {
                state = State.quests;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (current.IsKeyUp(Keys.J) && last.IsKeyDown(Keys.J) && state != State.journal)
            {
                state = State.journal;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (current.IsKeyUp(Keys.B) && last.IsKeyDown(Keys.B) && state != State.bios)
            {
                state = State.bios;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }
            
            if (current.IsKeyUp(Keys.L) && last.IsKeyDown(Keys.L) && state != State.combos)
            {
                state = State.combos;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (journal.drawNewIcon)
                journal.drawNewIcon = false;

            if (journal.prologueSynopsisRead == false || journal.prologueSideQuestsRead.Contains(false) || journal.prologueStoryQuestsRead.Contains(false))
            {
                journal.drawNewIcon = true;
            }
            if (journal.chOneSynopsisRead == false || journal.chOneSideQuestsRead.Contains(false) || journal.chOneStoryQuestsRead.Contains(false))
            {
                journal.drawNewIcon = true;
            }
            if (journal.chTwoSynopsisRead == false || journal.chTwoSideQuestsRead.Contains(false) || journal.chTwoStoryQuestsRead.Contains(false))
            {
                journal.drawNewIcon = true;
            }

            switch (state)
            {
                case State.inventory:
                    inventory.Update();
                    break;

                case State.combos:
                    comboPage.Update();
                    break;

                case State.journal:
                    journal.Update();
                    break;

                case State.bios:
                    bioPage.Update();
                    break;

                case State.quests:
                    questsPage.Update();
                    break;
            }
        }

        public void LoadContent()
        {
            newIcon = Content.Load<Texture2D>(@"Inventory\newEquipmentIcon");

            //Notebook tabs
            staticTabs = Content.Load<Texture2D>(@"Inventory\tabsStatic");
            bioTabActive = Content.Load<Texture2D>(@"Inventory\biosTabActive");
            comboTabActive = Content.Load<Texture2D>(@"Inventory\combosTabActive");
            inventoryTabActive = Content.Load<Texture2D>(@"Inventory\inventoryTabActive");
            journalTabActive = Content.Load<Texture2D>(@"Inventory\journalTabActive");
            mapTabActive = Content.Load<Texture2D>(@"Inventory\mapTabActive");
            questsTabActive = Content.Load<Texture2D>(@"Inventory\questTabActive");

            Sound.LoadMenuSounds();

            #region Inventory
            inventoryTextures.Add("Background", Content.Load<Texture2D>(@"Inventory\inventoryBackground"));
            inventoryTextures.Add("PassiveTabActive", Content.Load<Texture2D>(@"Inventory\passivesTabActive"));
            inventoryTextures.Add("PassivePage", Content.Load<Texture2D>(@"Inventory\passives"));
            inventoryTextures.Add("newEquipmentIcon", Content.Load<Texture2D>(@"Inventory\newEquipmentIcon"));
            inventoryTextures.Add("iconInfoBox", Content.Load<Texture2D>(@"Inventory\iconInfoBoxes"));

            //Equipment tabs
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

            inventoryTextures.Add("PassiveLeft", Content.Load<Texture2D>(@"Inventory\passiveLeftStatic"));
            inventoryTextures.Add("PassiveLeftActive", Content.Load<Texture2D>(@"Inventory\passiveLeftActive"));
            inventoryTextures.Add("PassiveRight", Content.Load<Texture2D>(@"Inventory\passiveRightStatic"));
            inventoryTextures.Add("PassiveRightActive", Content.Load<Texture2D>(@"Inventory\passiveRightActive"));

            inventoryTextures.Add("StoryLeft", Content.Load<Texture2D>(@"Inventory\storyLeftStatic"));
            inventoryTextures.Add("StoryLeftActive", Content.Load<Texture2D>(@"Inventory\storyLeftActive"));
            inventoryTextures.Add("StoryRight", Content.Load<Texture2D>(@"Inventory\storyRightStatic"));
            inventoryTextures.Add("StoryRightActive", Content.Load<Texture2D>(@"Inventory\storyRightActive"));

            passiveBoxes = ContentLoader.LoadContent(Content, "Inventory\\Passives Boxes");
            inventory.passiveBoxes = passiveBoxes;
            #endregion

            #region Combo Page
            comboTextures.Add("Background", Content.Load<Texture2D>(@"ComboPage\background"));
            comboTextures.Add("leftStatic", Content.Load<Texture2D>(@"ComboPage\leftStatic"));
            comboTextures.Add("rightStatic", Content.Load<Texture2D>(@"ComboPage\rightStatic"));
            comboTextures.Add("rightActive", Content.Load<Texture2D>(@"ComboPage\rightActive"));
            comboTextures.Add("leftActive", Content.Load<Texture2D>(@"ComboPage\leftActive"));
            #endregion

            #region Journal

            journalTextures = ContentLoader.LoadContent(Content, "Journal");
            journal.textures = journalTextures;
            #endregion

            #region BIO
            bioTextures = ContentLoader.LoadContent(Content, "BioPage");
            bioPage.textures = bioTextures;
            #endregion

            #region Daryldrawings
            darylDrawings.Add("HatClean", Game1.whiteFilter);
            darylDrawings.Add("Dunce Cap", Game1.whiteFilter);
            darylDrawings.Add("Powdered Wig", Game1.whiteFilter);
            darylDrawings.Add("Band Hat", Game1.whiteFilter);
            darylDrawings.Add("Gardening Hat", Game1.whiteFilter);
            darylDrawings.Add("Scarecrow Hat", Game1.whiteFilter);
            darylDrawings.Add("Party Hat", Game1.whiteFilter);
            darylDrawings.Add("Pelt Kid's Hat", Game1.whiteFilter);

            darylDrawings.Add("MainClean", Game1.whiteFilter);
            darylDrawings.Add("Dried Out Marker", Game1.whiteFilter);
            darylDrawings.Add("Coal Shovel", Game1.whiteFilter);
            darylDrawings.Add("Conductor's Wand", Game1.whiteFilter);
            darylDrawings.Add("Melon-Mashing Mallet", Game1.whiteFilter);
            darylDrawings.Add("Dirty Broken Hoe", Game1.whiteFilter);
            darylDrawings.Add("Hand Saw", Game1.whiteFilter);

            darylDrawings.Add("SecondClean", Game1.whiteFilter);
            darylDrawings.Add("Dried Out Marker Second", Game1.whiteFilter);
            darylDrawings.Add("Conductor's Wand Second", Game1.whiteFilter);
            darylDrawings.Add("Hand Saw Second", Game1.whiteFilter);

            darylDrawings.Add("ShirtClean", Game1.whiteFilter);
            darylDrawings.Add("Lab Coat", Game1.whiteFilter);
            darylDrawings.Add("Band Uniform", Game1.whiteFilter);
            darylDrawings.Add("'I Love Melons' Band Tee", Game1.whiteFilter);
            darylDrawings.Add("'I Hate Melons' Band Tee", Game1.whiteFilter);
            darylDrawings.Add("Scarecrow Vest", Game1.whiteFilter);
            darylDrawings.Add("Toga", Game1.whiteFilter);


            //Load the currently equipped stuff
            if (Game1.Player.EquippedHat == null)
                darylDrawings["HatClean"] = Content.Load<Texture2D>(@"DarylDrawing\Hats\HatClean");
            else
                darylDrawings[Game1.Player.EquippedHat.Name] = Content.Load<Texture2D>(@"DarylDrawing\Hats\" + Game1.Player.EquippedHat.Name);

            if (Game1.Player.EquippedHoodie == null)
                darylDrawings["ShirtClean"] = Content.Load<Texture2D>(@"DarylDrawing\Shirts\ShirtClean");
            else
                darylDrawings[Game1.Player.EquippedHoodie.Name] = Content.Load<Texture2D>(@"DarylDrawing\Shirts\" + Game1.Player.EquippedHoodie.Name);

            if (Game1.Player.EquippedWeapon == null)
                darylDrawings["MainClean"] = Content.Load<Texture2D>(@"DarylDrawing\Main\MainClean");
            else
                darylDrawings[Game1.Player.EquippedWeapon.Name] = Content.Load<Texture2D>(@"DarylDrawing\Main\" + Game1.Player.EquippedWeapon.Name);

            if (Game1.Player.SecondWeapon == null)
                darylDrawings["SecondClean"] = Content.Load<Texture2D>(@"DarylDrawing\Second\SecondClean");
            else
                darylDrawings[Game1.Player.SecondWeapon.Name + " Second"] = Content.Load<Texture2D>(@"DarylDrawing\Second\" + Game1.Player.SecondWeapon.Name + " Second");
            #endregion

            #region Quests Page
            questTextures = ContentLoader.LoadContent(Content, "QuestsPage");
            questsPage.textures = questTextures;

            for (int i = 0; i < game.CurrentQuests.Count; i++)
            {
                if (game.CurrentQuests[i].StoryQuest)
                {
                    questsPage.currentStoryQuest = game.CurrentQuests[i];
                    break;
                }

                if (i == game.CurrentQuests.Count - 1)
                    questsPage.currentStoryQuest = null;
            }
            #endregion

            //Play opening sound
            Sound.PlaySoundInstance(Sound.SoundNames.UIOpen);
        }

        public void UnloadContent()
        {
            darylDrawings.Clear();
            bioTextures.Clear();
            inventoryTextures.Clear();
            comboTextures.Clear();
            journalTextures.Clear();
            questTextures.Clear();
            smallCharacterPortraits.Clear();
            smallEnemyPortraits.Clear();
            Sound.UnloadMenuSounds();
            Content.Unload();
        }

        public void Draw(SpriteBatch s)
        {
            switch (state)
            {
                case State.inventory:
                    inventory.Draw(s);
                    break;

                case State.combos:
                    comboPage.Draw(s);
                    break;

                case State.journal:
                    journal.Draw(s);
                    break;

                case State.bios:
                    bioPage.Draw(s);
                    break;

                case State.quests:
                    questsPage.Draw(s);
                    break;
            }

            s.Draw(staticTabs, new Rectangle(0, 0, staticTabs.Width, staticTabs.Height), Color.White);

            if ((inventoryTab.IsOver() && !inventory.showingPassives) || state == State.inventory)
            {
                s.Draw(inventoryTabActive, new Rectangle(0, 0, 202, 503), Color.White);

                if (inventoryTab.IsOver() && firstFrameOverInventory)
                {
                    firstFrameOverInventory = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverInventory = true;

            if ((mapsTab.IsOver() && !inventory.showingPassives)|| state == State.maps)
            {
                s.Draw(mapTabActive, new Rectangle(0, 0, 202, 503), Color.White);

                if (mapsTab.IsOver() && firstFrameOverMaps)
                {
                    firstFrameOverMaps = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverMaps = true;

            if ((combosTab.IsOver() && !inventory.showingPassives) || state == State.combos)
            {
                s.Draw(comboTabActive, new Rectangle(0, 0, 202, 503), Color.White);

                if (combosTab.IsOver() && firstFrameOverCombos)
                {
                    firstFrameOverCombos = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverCombos = true;

            if ((journalTab.IsOver() && !inventory.showingPassives)|| state == State.journal)
            {
                s.Draw(journalTabActive, new Rectangle(0, 0, 202, 503), Color.White);

                if (journalTab.IsOver() && firstFrameOverJournal)
                {
                    firstFrameOverJournal = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverJournal = true;

            if ((bioTab.IsOver() && !inventory.showingPassives) || state == State.bios)
            {
                s.Draw(bioTabActive, new Rectangle(0, 0, 202, 503), Color.White);

                if (bioTab.IsOver() && firstFrameOverBios)
                {
                    firstFrameOverBios = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverBios = true;

            if ((questsTab.IsOver() && !inventory.showingPassives) || state == State.quests)
            {
                s.Draw(questsTabActive, new Rectangle(0, 0, 193, 571), Color.White);

                if (questsTab.IsOver() && firstFrameOverQuests)
                {
                    firstFrameOverQuests = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
            }
            else
                firstFrameOverQuests = true;

            //'NEW' ICONS ON TABS
            if (inventory.newAccessory || inventory.newHat || inventory.newLoot || inventory.newShirt || inventory.newWeapon)
            {
                s.Draw(inventoryTextures["newEquipmentIcon"], new Rectangle(158, 55, 45, 45), Color.White);
            }
            if (journal.drawNewIcon)
            {
                s.Draw(inventoryTextures["newEquipmentIcon"], new Rectangle(158, 305, 45, 45), Color.White);
            }
        }
    }
}
