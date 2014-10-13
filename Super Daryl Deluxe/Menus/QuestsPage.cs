using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class QuestsPage
    {
        public Dictionary<String, Texture2D> textures;
        Game1 game;

        List<Button> sideQuestButtons, sideQuestCheckBoxes;
        Button storyQuestButton, storyQuestCheckBox, currentSelectedCheckBox, questLeft, questRight, dialogueLeft, dialogueRight;
        public int questPage, dialoguePage;

        public Quest selectedQuest;
        public int selectedIndex;

        //Some fancy string formatting shit here
        public float currentPageDialogueHeight;
        int topOfDialogueContainer = 475;
        public List<Dictionary<String, int>> questDialogueAndPage;

        public Quest currentStoryQuest; //This is set in DarylsNotebook#LoadContent

        public QuestsPage(Game1 g, Dictionary<String, Texture2D> texts)
        {
            game = g;
            textures = texts;
            sideQuestButtons = new List<Button>();
            sideQuestCheckBoxes = new List<Button>();

            AddButtons();
            questDialogueAndPage = new List<Dictionary<string, int>>();
        }

        public void AddButtons()
        {
            storyQuestButton = new Button(new Rectangle(336, 210, 295, 30));
            storyQuestCheckBox = new Button(new Rectangle(645, 210, 25, 25));
            currentSelectedCheckBox = new Button(new Rectangle(790, 655, 34, 34));

            questLeft = new Button(new Rectangle(405, 641, 40, 28));
            questRight = new Button(new Rectangle(523, 635, 35, 29));

            dialogueLeft = new Button(new Rectangle(899, 600, 20, 14));
            dialogueRight = new Button(new Rectangle(958, 598, 17, 15));

            //SIDE QUESTS
            for (int i = 0; i < 10; i++)
            {
                sideQuestButtons.Add(new Button(new Rectangle(336, 319 + (i * 30), 295, 30)));
                sideQuestCheckBoxes.Add(new Button(new Rectangle(645, 321 + (i * 30), 25, 25)));
                
            }
        }

        public void Update()
        {

            //game.CurrentSideQuests.Add(game.CurrentSideQuests[0]);
            #region OTHER TABS
            if (DarylsNotebook.journalTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.journal;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (DarylsNotebook.combosTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.combos;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (DarylsNotebook.bioTab.Clicked())
            {
                //The sound is automatically played once the bio page is loaded, so we need to make sure it doesn't play twice
                if (game.Notebook.BioPage.changeToBioSound == false)
                    Sound.PlaySoundInstance(Sound.SoundNames.UITab);

                game.Notebook.state = DarylsNotebook.State.bios;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (DarylsNotebook.inventoryTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.inventory;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }
            #endregion

            for (int i = 0; i < sideQuestCheckBoxes.Count; i++)
            {
                #region ADD SIDE QUESTS TO HELPER
                if (sideQuestCheckBoxes[i].Clicked() && game.CurrentSideQuests.Count > (i + (questPage * 10)))
                {
                    if (Game1.questHUD.questHelperQuests.Count < 5 && !Game1.questHUD.questHelperQuests.Contains(game.CurrentSideQuests[i + (questPage * 10)]))
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIList1);
                        Game1.questHUD.AddQuestToHelper(game.CurrentSideQuests[i + (questPage * 10)]);
                    }
                    else if (Game1.questHUD.questHelperQuests.Contains(game.CurrentSideQuests[i + (questPage * 10)]))
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                        Game1.questHUD.RemoveQuestFromHelper(game.CurrentSideQuests[i + (questPage * 10)]);
                    }
                }
                #endregion

                //Click a side quest in the list. Updates the "Selected Quest" page
                if (sideQuestButtons[i].Clicked() && game.CurrentSideQuests.Count > (i + (questPage * 10)) && selectedQuest != game.CurrentSideQuests[(i + (questPage * 10))])
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab1);
                    //This is really just so I know where to draw the select box
                    selectedIndex = i;

                    selectedQuest = game.CurrentSideQuests[(i + (questPage * 10))];

                    questDialogueAndPage.Clear();
                    //316x148 is the size of the dialogue container
                    for (int j = 0; j < selectedQuest.QuestDialogue.Count - 1; j++)
                    {
                        String dialogue = WrapText(Game1.twConQuestHudInfo, selectedQuest.QuestDialogue[j], 290);
                        float stringHeight = Game1.twConQuestHudInfo.MeasureString(dialogue).Y;

                        //If there is no dialogue yet, add a new page and add this string to it
                        if (questDialogueAndPage.Count == 0)
                        {
                            questDialogueAndPage.Add(new Dictionary<string, int>() { {dialogue, topOfDialogueContainer} } );
                            currentPageDialogueHeight = stringHeight + 10;
                        }
                        else if (stringHeight + currentPageDialogueHeight > 130)
                        {
                            questDialogueAndPage.Add(new Dictionary<string, int>() { { dialogue, topOfDialogueContainer } });
                            currentPageDialogueHeight = stringHeight + 10;
                        }
                        else
                        {
                            questDialogueAndPage[questDialogueAndPage.Count - 1].Add(dialogue, (int)(topOfDialogueContainer + currentPageDialogueHeight));
                            currentPageDialogueHeight += stringHeight + 10;
                        }
                    }
                }
            }

            if (currentSelectedCheckBox.Clicked())
            {
                if (Game1.questHUD.questHelperQuests.Count < 5 && !Game1.questHUD.questHelperQuests.Contains(selectedQuest))
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList1);
                    Game1.questHUD.AddQuestToHelper(selectedQuest);
                }
                else if (Game1.questHUD.questHelperQuests.Contains(selectedQuest))
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                    Game1.questHUD.RemoveQuestFromHelper(selectedQuest);
                }
            }


            if (storyQuestCheckBox.Clicked() && currentStoryQuest != null)
            {
                if (Game1.questHUD.questHelperQuests.Count < 5 && !Game1.questHUD.questHelperQuests.Contains(currentStoryQuest))
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList1);
                    Game1.questHUD.AddQuestToHelper(currentStoryQuest);
                }
                else if (Game1.questHUD.questHelperQuests.Contains(currentStoryQuest))
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                    Game1.questHUD.RemoveQuestFromHelper(currentStoryQuest);
                }
            }

            if (storyQuestButton.Clicked() && currentStoryQuest != null && selectedQuest != currentStoryQuest)
            {
                selectedQuest = currentStoryQuest;

                questDialogueAndPage.Clear();

                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab1);

                //316x148 is the size of the dialogue container
                for (int i = 0; i < selectedQuest.QuestDialogue.Count - 1; i++)
                {
                    String dialogue = WrapText(Game1.twConQuestHudInfo, selectedQuest.QuestDialogue[i], 290);
                    float stringHeight = Game1.twConQuestHudInfo.MeasureString(dialogue).Y;

                    //If there is no dialogue yet, add a new page and add this string to it
                    if (questDialogueAndPage.Count == 0)
                    {
                        questDialogueAndPage.Add(new Dictionary<string, int>() { { dialogue, topOfDialogueContainer } });
                        currentPageDialogueHeight = stringHeight + 10;
                    }
                    else if (stringHeight + currentPageDialogueHeight > 130)
                    {
                        questDialogueAndPage.Add(new Dictionary<string, int>() { { dialogue, topOfDialogueContainer } });
                        currentPageDialogueHeight = stringHeight + 10;
                    }
                    else
                    {
                        questDialogueAndPage[questDialogueAndPage.Count - 1].Add(dialogue, (int)(topOfDialogueContainer + currentPageDialogueHeight));
                        currentPageDialogueHeight += stringHeight + 10;
                    }
                }
            }

            if (selectedQuest != null)
            {

                if (Game1.g.Prologue.PrologueBooleans["firstQuestPageQuestCheck"])
                {
                    Game1.g.Prologue.PrologueBooleans["firstQuestPageQuestCheck"] = false;
                }

                if (dialogueLeft.Clicked())
                {

                    if (dialoguePage > 0)
                    {
                        dialoguePage--;
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage1);
                    }
                }
                else if (dialogueRight.Clicked())
                {
                    if (dialoguePage < questDialogueAndPage.Count - 1)
                    {
                        dialoguePage++;
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage2);
                    }
                }
            }

            if (questLeft.Clicked())
            {
                if (questPage > 0)
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab2);
                    questPage--;
                }
            }
            else if (questRight.Clicked())
            {
                if (questPage < (game.CurrentSideQuests.Count / 10))
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab4);
                    questPage++;
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["background"], new Rectangle(0, 0, 1280, 720), Color.White);

            if (selectedQuest != null)
            {
                if (!game.Notebook.smallCharacterPortraits.ContainsKey(selectedQuest.npcName.ToLower()))
                {
                    //Load the NPC face
                    game.Notebook.BioPage.UnloadNPCAndEnemySprites();
                    game.Notebook.BioPage.loadedNPCEnemySprites = false;
                    game.Notebook.BioPage.changeToBioSound = true;
                    game.Notebook.smallCharacterPortraits.Add(selectedQuest.npcName.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"SmallNPCFaces\" + selectedQuest.npcName.ToLower()));
                }

                if (selectedQuest.StoryQuest)
                {
                    s.Draw(textures["selectedQuestStory"], new Rectangle(783, 319, textures["selectedQuestSide"].Width, textures["selectedQuestSide"].Height), Color.White);
                    s.Draw(textures["storyQuestSelect"], storyQuestButton.ButtonRec, Color.White);
                }
                else
                {
                    s.Draw(textures["selectedQuestSide"], new Rectangle(783, 319, textures["selectedQuestSide"].Width, textures["selectedQuestSide"].Height), Color.White);
                    s.Draw(textures["sideQuestSelect"], sideQuestButtons[selectedIndex].ButtonRec, Color.White);
                }

                //NAME
                s.DrawString(Game1.twConRegularSmall, selectedQuest.QuestName, new Vector2(947 - Game1.twConRegularSmall.MeasureString(selectedQuest.QuestName).X / 2, 322), Color.White);
                //TASK
                s.DrawString(Game1.twConQuestHudName, Game1.WrapText(Game1.twConQuestHudName, "Task: " + selectedQuest.TaskForQuestsPage, 300), new Vector2(792, 360), Color.Black);

                s.Draw(game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()], new Rectangle(790, 120, game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()].Width, game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()].Height), Color.White);

                if (Game1.questHUD.questHelperQuests.Contains(selectedQuest))
                {
                    s.Draw(textures["currentIsAddedToHelper"], new Rectangle(776, 640, textures["currentIsAddedToHelper"].Width, textures["currentIsAddedToHelper"].Height), Color.White);
                    s.Draw(textures["check"], new Rectangle(790, 650, textures["check"].Width, textures["check"].Height), Color.White);
                }
                else
                {
                    if (Game1.questHUD.questHelperQuests.Count == 5)
                        s.Draw(textures["cantAddCurrentToHelper"], new Rectangle(776, 640, textures["currentIsAddedToHelper"].Width, textures["currentIsAddedToHelper"].Height), Color.White);
                    else
                        s.Draw(textures["addCurrentToHelper"], new Rectangle(776, 640, textures["currentIsAddedToHelper"].Width, textures["currentIsAddedToHelper"].Height), Color.White);
                }
                //REWARDS
                for (int i = 0; i < selectedQuest.RewardObjects.Count; i++)
                {
                    if (selectedQuest.RewardObjects[i] is Equipment)
                    {
                        Equipment eq = selectedQuest.RewardObjects[i] as Equipment;

                        if (eq is Money)
                        {
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "$" + (eq as Money).Amount.ToString("N2"), new Vector2(950, 197 + (i * 27)), Color.Black);
                        }
                        else if (eq is Experience)
                        {
                            s.Draw(Game1.smallTypeIcons["smallExperienceIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "+ " + (eq as Experience).Amount.ToString(), new Vector2(950, 197 + (i * 27)), Color.Black);
                        }
                        else if (eq is Karma)
                        {
                            s.Draw(Game1.smallTypeIcons["smallKarmaIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "+ " + (eq as Karma).Amount.ToString(), new Vector2(950, 197 + (i * 27)), Color.Black);
                        }
                        else
                        {
                            s.DrawString(Game1.font, eq.Name, new Vector2(950, 197 + (i * 27)), Color.Black);
                            if (eq is Weapon)
                                s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            if (eq is Hat)
                                s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            if (eq is Hoodie)
                                s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                            if (eq is Accessory)
                                s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                        }
                    }
                    else if (selectedQuest.RewardObjects[i] is StoryItem)
                    {
                        StoryItem sItem = selectedQuest.RewardObjects[i] as StoryItem;
                        s.DrawString(Game1.twConQuestHudInfo, sItem.PickUpName, new Vector2(950, 197 + (i * 27)), Color.Black);
                        s.Draw(Game1.smallTypeIcons["smallStoryItemIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                    }
                    else if (selectedQuest.RewardObjects[i] is Collectible)
                    {
                        if (selectedQuest.RewardObjects[i] is Textbook)
                        {
                            s.DrawString(Game1.font, "Textbook", new Vector2(950, 197 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is BronzeKey)
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "Bronze Key", new Vector2(950, 197 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is SilverKey)
                        {
                            s.DrawString(Game1.font, "Silver Key", new Vector2(950, 197 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is GoldKey)
                        {
                            s.DrawString(Game1.font, "Gold Key", new Vector2(950, 197 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(922, 199 + (i * 27), 20, 20), Color.White);
                        }
                    }
                }

                //QUEST DIALOGUE
                s.DrawString(Game1.twConQuestHudName, selectedQuest.npcName + ":", new Vector2(789, 446), Color.DarkRed);
                for (int i = 0; i < questDialogueAndPage[dialoguePage].Count; i++)
                {
                    s.DrawString(Game1.twConQuestHudInfo, "\"" + questDialogueAndPage[dialoguePage].ElementAt(i).Key + "\"", new Vector2(800, questDialogueAndPage[dialoguePage].ElementAt(i).Value), Color.Black);
                }

                s.DrawString(Game1.VerySmallTwCondensedFont, (dialoguePage + 1) + "/" + questDialogueAndPage.Count, new Vector2(922, 597), Color.Black);

                if(dialogueLeft.IsOver())
                    s.Draw(textures["dialogueLeftActive"], new Rectangle(892, 596, textures["dialogueLeftActive"].Width, textures["dialogueLeftActive"].Height), Color.White);
                else
                    s.Draw(textures["dialogueLeftStatic"], new Rectangle(892, 596, textures["dialogueLeftActive"].Width, textures["dialogueLeftActive"].Height), Color.White);

                if (dialogueRight.IsOver())
                    s.Draw(textures["dialogueRightActive"], new Rectangle(892, 596, textures["dialogueLeftActive"].Width, textures["dialogueLeftActive"].Height), Color.White);
                else
                    s.Draw(textures["dialogueRightStatic"], new Rectangle(892, 596, textures["dialogueLeftActive"].Width, textures["dialogueLeftActive"].Height), Color.White);
            }

            if (currentStoryQuest != null)
            {
                s.DrawString(Game1.twConRegularSmall, currentStoryQuest.QuestName, new Vector2(340, 210), Color.White);

                if (Game1.questHUD.questHelperQuests.Contains(currentStoryQuest))
                {
                    s.Draw(textures["storyQuestCheckBox"], new Rectangle(storyQuestCheckBox.ButtonRecX - 8, storyQuestCheckBox.ButtonRecY - 7, textures["storyQuestCheckBox"].Width, textures["storyQuestCheckBox"].Height), Color.White);
                    s.Draw(textures["check"], new Rectangle(storyQuestCheckBox.ButtonRecX - 5, storyQuestCheckBox.ButtonRecY - 10, textures["check"].Width, textures["check"].Height), Color.White);
                }
            }
            else
            {
                s.Draw(textures["storyQuestSelect"], storyQuestButton.ButtonRec, Color.Black * .5f);
                s.Draw(textures["storyQuestCheckBox"], new Rectangle(storyQuestCheckBox.ButtonRecX - 6, storyQuestCheckBox.ButtonRecY - 3, textures["sideQuestCheckBox"].Width - 4, textures["sideQuestCheckBox"].Height - 4), Color.Black * .5f);
                s.DrawString(Game1.twConRegularSmall, "No Story Quest!", new Vector2(427, 210), Color.White);
            }

            s.DrawString(Game1.twConQuestHudName, (questPage + 1) + " / " + ((int)(game.CurrentSideQuests.Count / 10) + 1), new Vector2(465, 642), Color.Black);

            if (questLeft.IsOver())
                s.Draw(textures["questLeftActive"], new Rectangle(390, 633, textures["questLeftActive"].Width, textures["questLeftActive"].Height), Color.White);
            else
                s.Draw(textures["questLeftStatic"], new Rectangle(390, 633, textures["questLeftActive"].Width, textures["questLeftActive"].Height), Color.White);

            if (questRight.IsOver())
                s.Draw(textures["questRightActive"], new Rectangle(390, 633, textures["questLeftActive"].Width, textures["questLeftActive"].Height), Color.White);
            else
                s.Draw(textures["questRightStatic"], new Rectangle(390, 633, textures["questLeftActive"].Width, textures["questLeftActive"].Height), Color.White);

            for (int i = 0; i < sideQuestCheckBoxes.Count; i++)
            {
                if (game.CurrentSideQuests.Count > i + (questPage * 10))
                {
                    s.DrawString(Game1.twConRegularSmall, game.CurrentSideQuests[i + (questPage * 10)].QuestName, new Vector2(340, 320 + (i * 30)), Color.White);
                }

                if (game.CurrentSideQuests.Count > i + (questPage * 10) && Game1.questHUD.questHelperQuests.Contains(game.CurrentSideQuests[i + (questPage * 10)]))
                {
                    s.Draw(textures["sideQuestCheckBox"], new Rectangle(sideQuestCheckBoxes[i].ButtonRecX - 8, sideQuestCheckBoxes[i].ButtonRecY - 7, textures["sideQuestCheckBox"].Width, textures["sideQuestCheckBox"].Height), Color.White);
                    s.Draw(textures["check"], new Rectangle(sideQuestCheckBoxes[i].ButtonRecX - 5, sideQuestCheckBoxes[i].ButtonRecY - 10, textures["check"].Width, textures["check"].Height), Color.White);
                }
            }
        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
