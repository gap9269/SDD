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
    public class BioPage
    {
        public Dictionary<String, Texture2D> textures;
        List<Button> bioButtons;
        List<Button> characterPictures;
        List<Button> enemyPictures;
        Dictionary<String, Texture2D> NPCOrEnemySprites;
        Button characterButton, enemyButton, nextPage, previousPage;

        Game1 game;
        String selectedBioName;

        public int selectedIndex;

        int page;

        public int Page { get { return page; } set { page = value; } }
        public String SelectedBioName { get { return selectedBioName; } set { selectedBioName = value; } }

        public enum BioState
        {
            Character,
            Monster,
        }
        public BioState bioState;

        public Boolean loadedNPCEnemySprites = false;

        Boolean pageDownSound = false;
        Boolean pageUpSound = false;
        Boolean changeTabSound = false;
        public Boolean changeToBioSound = false;

        public BioPage(Dictionary<String, Texture2D> texts, Game1 g)
        {
            game = g;
            textures = texts;
            NPCOrEnemySprites = new Dictionary<string, Texture2D>();
            bioState = BioState.Character;
            bioButtons = new List<Button>();
            characterPictures = new List<Button>();
            enemyPictures = new List<Button>();
            nextPage = new Button(new Rectangle(983, 656, 35, 29));
            previousPage = new Button(new Rectangle(866, 660, 40, 28));

            characterButton = new Button(Game1.emptyBox, new Rectangle(679, 124, 37, 71));
            enemyButton = new Button(Game1.emptyBox, new Rectangle(665, 270, 34, 90));

           // UpdateResolution();
            AddBioButtons();

            pageDownSound = false;
            pageUpSound = false;
            changeTabSound = false;
        }

        public void UpdateResolution()
        {

            nextPage.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .85) -12;//600
            previousPage.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .85) - 18;//594

            characterButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2) + 1;//145
            enemyButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .35) -18;//234;
        }

        public void AddBioButtons()
        {
            //Row 1
            bioButtons.Add(new Button(new Rectangle(805, 59, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(950, 59, 126, 126)));

            //Row 2
            bioButtons.Add(new Button(new Rectangle(736, 208, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(880, 208, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(1024, 208, 126, 126)));

            //Row 3
            bioButtons.Add(new Button(new Rectangle(805, 359, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(950, 359, 126, 126)));

            //Row 4
            bioButtons.Add(new Button(new Rectangle(736, 507, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(880, 507, 126, 126)));
            bioButtons.Add(new Button(new Rectangle(1024, 507, 126, 126)));


            //Row 1
            characterPictures.Add(new Button(new Rectangle(812, -4, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(957, -4, 140, 182)));

            //Row 2
            characterPictures.Add(new Button(new Rectangle(741, 145, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(886, 145, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(1029, 145, 140, 182)));

            //Row 3
            characterPictures.Add(new Button(new Rectangle(812, 296, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(957, 296, 140, 182)));

            //Row 4
            characterPictures.Add(new Button(new Rectangle(741, 444, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(886, 444, 140, 182)));
            characterPictures.Add(new Button(new Rectangle(1029, 444, 140, 182)));


            //Row 1
            enemyPictures.Add(new Button(new Rectangle(787, 23, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(932, 23, 137, 156)));

            //Row 2
            enemyPictures.Add(new Button(new Rectangle(717, 172, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(861, 172, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(1005, 172, 137, 156)));

            //Row 3
            enemyPictures.Add(new Button(new Rectangle(787, 324, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(932, 324, 137, 156)));

            //Row 4
            enemyPictures.Add(new Button(new Rectangle(717, 470, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(861, 470, 137, 156)));
            enemyPictures.Add(new Button(new Rectangle(1005, 470, 137, 156)));
        }

        public void UpdateBioPage()
        {
            //--For as many boxes there are on each page
            for (int i = 0; i < bioButtons.Count; i++)
            {
                int boxNumber = i + (page * bioButtons.Count);
                characterPictures[i].ButtonTexture = null;

                if (bioState == BioState.Character)
                {
                    if (boxNumber < Game1.Player.AllCharacterBios.Count)
                    {
                        //OwnedBios is a dictionary of Strings to Booleans. If Bios -> "Paul" = true, it means you own the "Paul" bio
                        if (Game1.Player.AllCharacterBios.ElementAt(boxNumber).Value == true)
                        {
                            //Set the picture equal to the dialogue face of the NPC
                            characterPictures[i].ButtonTexture = game.Notebook.smallCharacterPortraits[Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower()];
                        }
                    }
                }
                else
                {
                    if (boxNumber < Game1.Player.AllMonsterBios.Count)
                    {
                        //MonsterBios is a dictionary of Strings to Booleans. If Bios -> "Crow" = true, it means you own the "Crow" bio
                        if (Game1.Player.AllMonsterBios.Count > boxNumber && Game1.Player.AllMonsterBios.ElementAt(boxNumber).Value == true)
                        {
                            //Set the picture equal to the enemy portrait
                            characterPictures[i].ButtonTexture = game.Notebook.smallEnemyPortraits[Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower()];
                        }
                    }
                }
                
                if (bioState == BioState.Character)
                {
                    if (Game1.Player.AllCharacterBios.Count > boxNumber && bioButtons[i].Clicked() && Game1.Player.AllCharacterBios.ElementAt(boxNumber).Value == true)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                        selectedIndex = i;
                        selectedBioName = Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key;
                    }

                }
                else if(bioState == BioState.Monster)
                {
                    if (Game1.Player.AllMonsterBios.Count > boxNumber && bioButtons[i].Clicked() && Game1.Player.AllMonsterBios.ElementAt(boxNumber).Value == true)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_03);
                        selectedIndex = i;
                        selectedBioName = Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key;
                    }
                }
            }
        }

        public void UnloadNPCAndEnemySprites()
        {
            game.Notebook.smallCharacterPortraits.Clear();
            game.Notebook.smallEnemyPortraits.Clear();
            NPCOrEnemySprites.Clear();
            DarylsNotebook.BioNPCAndEnemyContentLoader.Unload();
        }

        public void Update()
        {
            //Load the NPC and Enemy sprite textures for the current page
            if (loadedNPCEnemySprites == false)
            {
                loadedNPCEnemySprites = true;
                LoadEnemyOrNPCPictures();
            }

            UpdateBioPage();

            if (characterButton.Clicked())
            {
                changeTabSound = true;
                bioState = BioState.Character;
                page = 0;
                selectedBioName = "";
                selectedIndex = 0;
                loadedNPCEnemySprites = false;
            }
            else if (enemyButton.Clicked())
            {
                changeTabSound = true;
                bioState = BioState.Monster;
                page = 0;
                selectedBioName = "";
                selectedIndex = 0;
                loadedNPCEnemySprites = false;
            }

            #region CHANGE NOTEBOOK TABS
            if (DarylsNotebook.journalTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.journal;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
            }

            if (DarylsNotebook.combosTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.combos;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
            }

            if (DarylsNotebook.inventoryTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.inventory;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
            }

            if (DarylsNotebook.questsTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.quests;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
            }

            if (DarylsNotebook.mapsTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.maps;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
            }
            #endregion


            #region Change Pages

            //--If you are not on the last page, go up a page and reset textures
            if ((nextPage.Clicked() || MyGamePad.RightPadPressed()) && page < 5)
            {
                pageUpSound = true;
                page++;
                loadedNPCEnemySprites = false;
            }
            //--If you aren't on the first page, go back a page and reset textures
            if ((previousPage.Clicked()|| MyGamePad.LeftPadPressed()) && page > 0)
            {
                pageDownSound = true;
                page--;
                loadedNPCEnemySprites = false;
            }


            #endregion
        }

        //Loads the NPC or Enemy sprites for whatever page of the bio the player is switching to
        public void LoadEnemyOrNPCPictures()
        {
            NPCOrEnemySprites.Clear();
            game.Notebook.smallCharacterPortraits.Clear();
            game.Notebook.smallEnemyPortraits.Clear();
            DarylsNotebook.BioNPCAndEnemyContentLoader.Unload();

            //If there is a selected bio make sure to load the face immediately after clearing the list
            if (selectedBioName != null && selectedBioName != "")
            {
                if (bioState == BioState.Character)
                    NPCOrEnemySprites.Add(selectedBioName.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"BioPage\NPCs For Bio Page\" + selectedBioName.ToLower()));
                else
                    NPCOrEnemySprites.Add(selectedBioName.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"BioPage\Enemies For Bio Page\" + selectedBioName.ToLower()));
            }

            if (!pageDownSound && !pageUpSound && !changeTabSound)
                System.Threading.Thread.Sleep(100);

            for (int i = 0; i < characterPictures.Count; i++)
            {
                int boxNumber = i + (page * bioButtons.Count);

                if (bioState == BioState.Character && boxNumber < Game1.Player.AllCharacterBios.Count)
                {
                    //OwnedBios is a dictionary of Strings to Booleans. If Bios -> "Paul" = true, it means you own the "Paul" bio
                    if (Game1.Player.AllCharacterBios.ElementAt(boxNumber).Value == true)
                    {
                        //If there is a selectedBio make sure that you don't load the same face again
                        if ((selectedBioName != null && selectedBioName != "" && !NPCOrEnemySprites.ContainsKey(Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower())) || selectedBioName == null || selectedBioName == "")
                        {
                        //Load the NPC Sprite and add it to the dictionary
                        Texture2D sprite = DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"BioPage\NPCs For Bio Page\" + Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower());

                        String name = Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower();
                        NPCOrEnemySprites.Add(name, sprite);
                        }

                        if (!game.Notebook.smallCharacterPortraits.ContainsKey(Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower()))
                        {
                            game.Notebook.smallCharacterPortraits.Add(Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"SmallNPCFaces\" + Game1.Player.AllCharacterBios.ElementAt(boxNumber).Key.ToLower()));
                        }
                    }

                }
                else if (bioState == BioState.Monster && boxNumber < Game1.Player.AllMonsterBios.Count)
                {
                    //MonsterBios is a dictionary of Strings to Booleans. If Bios -> "Crow" = true, it means you own the "Crow" bio
                    if (Game1.Player.AllMonsterBios.ElementAt(boxNumber).Value == true)
                    {
                        //If there is a selectedBio make sure that you don't load the same face again
                        if ((selectedBioName != null && selectedBioName != "" && !NPCOrEnemySprites.ContainsKey(Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower())) || selectedBioName == null || selectedBioName == "")
                        {
                            //Load the NPC Sprite and add it to the dictionary
                            Texture2D sprite = DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"BioPage\Enemies For Bio Page\" + Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower());

                            String name = Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower();
                            NPCOrEnemySprites.Add(name, sprite);
                        }


                        if (!game.Notebook.smallEnemyPortraits.ContainsKey(Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower()))
                        {
                            game.Notebook.smallEnemyPortraits.Add(Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"EnemyPortraits\" + Game1.Player.AllMonsterBios.ElementAt(boxNumber).Key.ToLower()));
                        }
                    }
                }
            }

            if(pageUpSound)
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_02);
            else if(pageDownSound)
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_01);
            else if(changeTabSound)
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_tab_01);

            pageUpSound = false;
            pageDownSound = false;
            changeTabSound = false;
            changeToBioSound = false;
        }

        public void DrawNPCInfoString(CharacterMonsterBioDictionary.CharacterInfo info, SpriteBatch s)
        {
            int startingPosY = 500;

            s.DrawString(Game1.twConQuestHudInfo, "AGE: " + info.age, new Vector2(363, startingPosY), Color.Black);

            startingPosY += 3;

            s.DrawString(Game1.twConQuestHudInfo, "\n" + Game1.WrapText(Game1.twConQuestHudInfo, "YEARBOOK QUOTE: " + info.yearbookQuote, 288), new Vector2(363, startingPosY), Color.Black);

            startingPosY += (int)Game1.twConQuestHudInfo.MeasureString(Game1.WrapText(Game1.twConQuestHudInfo, "YEARBOOK QUOTE: " + info.yearbookQuote, 288)).Y + 3;
           
            s.DrawString(Game1.twConQuestHudInfo, "\n" + Game1.WrapText(Game1.twConQuestHudInfo, "FUN FACT: " + info.funFact, 288), new Vector2(363, startingPosY), Color.Black);

            startingPosY += (int)Game1.twConQuestHudInfo.MeasureString(Game1.WrapText(Game1.twConQuestHudInfo, "FUN FACT: " + info.funFact, 288)).Y + 3;
           
            s.DrawString(Game1.twConQuestHudInfo, "\n" + Game1.WrapText(Game1.twConQuestHudInfo, "SUPERLATIVE: " + info.superlative, 288), new Vector2(363, startingPosY), Color.Black);
        }

        public void Draw(SpriteBatch s)
        {
            //Load the NPC and Enemy sprite textures for the current page
            if (loadedNPCEnemySprites == false)
            {
                loadedNPCEnemySprites = true;
                LoadEnemyOrNPCPictures();
                UpdateBioPage();
            }

            if (bioState == BioState.Character)
                s.Draw(textures["npcBackground"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
            else
                s.Draw(textures["enemyBackground"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            #region Draw the hover texture for buttons

            if (characterButton.IsOver() && bioState == BioState.Monster)
                s.Draw(textures["npcActive"], new Rectangle(652, 74, textures["npcActive"].Width, textures["npcActive"].Height), Color.White);
            else
                s.Draw(textures["npcStatic"], new Rectangle(652, 74, textures["npcActive"].Width, textures["npcStatic"].Height), Color.White);

            if (enemyButton.IsOver() && bioState == BioState.Character)
                s.Draw(textures["enemyActive"], new Rectangle(652, 74, textures["enemyActive"].Width, textures["enemyActive"].Height), Color.White);
            else
                s.Draw(textures["enemyStatic"], new Rectangle(652, 74, textures["enemyActive"].Width, textures["enemyActive"].Height), Color.White);


            #endregion


            //--Draw the inventory boxes
            for (int i = 0; i < bioButtons.Count; i++)
            {
                if (bioState == BioState.Character)
                {
                    if (characterPictures[i].ButtonTexture != Game1.emptyBox && characterPictures[i].ButtonTexture != null)
                    {
                        s.Draw(characterPictures[i].ButtonTexture, characterPictures[i].ButtonRec, Color.White);
                    }
                }
                else
                {
                    if (characterPictures[i].ButtonTexture != Game1.emptyBox && characterPictures[i].ButtonTexture != null)
                    {
                        s.Draw(characterPictures[i].ButtonTexture, enemyPictures[i].ButtonRec, Color.White);
                    }
                }
            }


            if (selectedBioName != null && selectedBioName != "")
            {
                if (bioState == BioState.Character)
                {
                    //Draw the selected NPC and description
                    s.Draw(textures["infoBox"], new Rectangle(350, 488, textures["infoBox"].Width, textures["infoBox"].Height), Color.White);
                    s.Draw(NPCOrEnemySprites[selectedBioName.ToLower()], new Rectangle(233, 100, NPCOrEnemySprites[selectedBioName.ToLower()].Width, NPCOrEnemySprites[selectedBioName.ToLower()].Height), Color.White);
                    s.DrawString(Game1.bioPageNameFont, CharacterMonsterBioDictionary.nameAndInfo[selectedBioName].name, new Vector2(503 - Game1.twConMedium.MeasureString(CharacterMonsterBioDictionary.nameAndInfo[selectedBioName].name).X * .8f / 2, 448), Color.Black, 0f, Vector2.Zero, .9f, SpriteEffects.None, 0);

                    DrawNPCInfoString(CharacterMonsterBioDictionary.nameAndInfo[selectedBioName], s);
                }
                else
                {
                    //Draw the enemy circle and description
                    s.Draw(textures["monsterInfoBox"], new Rectangle(386, 519, textures["monsterInfoBox"].Width, textures["monsterInfoBox"].Height), Color.White);
                    s.Draw(NPCOrEnemySprites[selectedBioName.ToLower()], new Rectangle(245, 100, NPCOrEnemySprites[selectedBioName.ToLower()].Width, NPCOrEnemySprites[selectedBioName.ToLower()].Height), Color.White);
                    s.DrawString(Game1.twConQuestHudInfo, CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].experienceGiven, new Vector2(450, 566), Color.Black);
                    s.DrawString(Game1.twConQuestHudInfo, CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].itemDrop, new Vector2(450, 603), Color.Black);
                    s.DrawString(Game1.font, "Level " + CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].level, new Vector2(479, 525), Color.White);

                    if (Game1.WrapText(Game1.twConQuestHudInfo, "HOBBY: " + CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].hobby, 220).Contains("\n"))
                        s.DrawString(Game1.twConQuestHudInfo, Game1.WrapText(Game1.twConQuestHudInfo, "HOBBY: " + CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].hobby, 220), new Vector2(400, 630), Color.Black);
                    else
                        s.DrawString(Game1.twConQuestHudInfo, Game1.WrapText(Game1.twConQuestHudInfo, "HOBBY: " + CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].hobby, 220), new Vector2(400, 637), Color.Black);

                    s.DrawString(Game1.bioPageNameFont, CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].name, new Vector2(503 - Game1.twConMedium.MeasureString(CharacterMonsterBioDictionary.enemyNameAndInfo[selectedBioName].name).X * .8f / 2, 478), Color.Black, 0f, Vector2.Zero, .9f, SpriteEffects.None, 0);


                    //s.Draw(selectedEnemyCircle, new Rectangle(390, 150, selectedEnemyCircle.Width, selectedEnemyCircle.Width), Color.White);
                    //s.DrawString(Game1.descriptionFont, CharacterMonsterBioDictionary.nameAndInfo[selectedBioName], new Vector2(374, 387), Color.Black);
                }
            }

            #region ARROWS

            s.DrawString(Game1.font, (page + 1).ToString() + " / 6", new Vector2(925, 660), Color.Black);

            if (Game1.gamePadConnected)
                s.Draw(DarylsNotebook.dRight, new Vector2(986, 655), Color.White);
            else
            {
                if (nextPage.IsOver())
                    s.Draw(textures["rightActive"], new Rectangle(851, 652, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                else
                    s.Draw(textures["rightStatic"], new Rectangle(851, 652, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
            }
            if (Game1.gamePadConnected)
                s.Draw(DarylsNotebook.dLeft, new Vector2(870, 655), Color.White);
            else
            {
                if (previousPage.IsOver())
                    s.Draw(textures["leftActive"], new Rectangle(851, 652, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                else
                    s.Draw(textures["leftStatic"], new Rectangle(851, 652, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
            }

            #endregion

        }
    }
}
