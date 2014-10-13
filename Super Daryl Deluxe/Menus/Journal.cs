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
    public class Journal
    {
        public Dictionary<String, Texture2D> textures;
        List<Button> questButtons;
        public List<String> synopsisPages;

        public Boolean drawNewIcon = false;

        Button synopsisButton, sideButton, storyButton, questLeft, questRight, synLeft, synRight;

        List<Button> chapterButtons;
        Game1 game;
        Boolean viewingSpecificQuest = false;
        public Quest selectedQuest;
        public int selectedIndex;

        public Boolean openedToSpecificQuest = false;

        public int questPage;
        public int synopsisPage;
        int maxStoryQuestPage, maxSideQuestPage;

        public List<Boolean> prologueStoryQuestsRead;
        public List<Boolean> prologueSideQuestsRead;
        public Boolean prologueSynopsisRead = true;

        public List<Boolean> chOneStoryQuestsRead;
        public List<Boolean> chOneSideQuestsRead;
        public Boolean chOneSynopsisRead = true;

        public List<Boolean> chTwoStoryQuestsRead;
        public List<Boolean> chTwoSideQuestsRead;
        public Boolean chTwoSynopsisRead = true;

        public Boolean ViewingSpecificQuest { get { return viewingSpecificQuest; } set { viewingSpecificQuest = value; } }

        public enum ChapterState
        {
            none,
            Prologue,
            ChapterOne,
            ChapterTwo
        }
        public ChapterState chapterState;

        public enum InsideChapterState
        {
            story,
            side
        }
        public InsideChapterState insideChapterState;
        int currentChapterInt;

        public Boolean firstFrameOverPrologue = true;
        public Boolean firstFrameOverCh1 = true;
        public Boolean firstFrameOverCh2 = true;
        public Boolean firstFrameOverCh3 = true;
        public Boolean firstFrameOverCh4 = true;
        public Boolean firstFrameOverCh5 = true;
        public Boolean firstFrameOverCh6 = true;
        public Boolean firstFrameOverSynopsis = true;
        public Boolean firstFrameOverSide = true;
        public Boolean firstFrameOverStory = true;

        public Journal(Dictionary<String, Texture2D> texts, Game1 g)
        {
            synopsisPages = new List<string>();
            game = g;
            textures = texts;
            chapterState = ChapterState.none;
            insideChapterState = InsideChapterState.story;
            chapterButtons = new List<Button>();

            synopsisButton = new Button(Game1.emptyBox, new Rectangle(360, 200, 276, 42));
            storyButton = new Button(Game1.emptyBox, new Rectangle(356, 311, 145, 36));
            sideButton = new Button(Game1.emptyBox, new Rectangle(505, 310, 147, 37));

            synLeft = new Button(new Rectangle(861, 659, 40, 29));
            synRight = new Button(new Rectangle(978, 657, 35, 29));

            questLeft = new Button(new Rectangle(425, 663, 40, 28));
            questRight = new Button(new Rectangle(542, 659, 35, 29));

            for (int i = 0; i < 7; i++)
            {
                chapterButtons.Add(new Button(new Rectangle(377 + (i * 39), 108, 31, 32)));
            }

            questButtons = new List<Button>();
            for (int i = 0; i < 10; i++)
            {
                questButtons.Add(new Button(new Rectangle(356, 348 + (i * 30), 293, 30)));
            }

            prologueStoryQuestsRead = new List<bool>();
            prologueSideQuestsRead = new List<bool>();

            chOneStoryQuestsRead = new List<bool>();
            chOneSideQuestsRead = new List<bool>();

            chTwoStoryQuestsRead = new List<bool>();
            chTwoSideQuestsRead = new List<bool>();

            //UpdateResolution();
        }

        public void ResetJournal()
        {
            prologueStoryQuestsRead = new List<bool>();
            prologueSideQuestsRead = new List<bool>();

            chOneStoryQuestsRead = new List<bool>();
            chOneSideQuestsRead = new List<bool>();

            chTwoStoryQuestsRead = new List<bool>();
            chTwoSideQuestsRead = new List<bool>();

            chOneSynopsisRead = true;
            prologueSynopsisRead = true;
            chTwoSynopsisRead = true;
        }

        public void UpdateResolution()
        {

            synopsisButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .2) + 1;//145
            storyButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4) + 7;//295
            sideButton.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) + 3;//435;

            chapterButtons[0].ButtonRecY = (int)(Game1.aspectRatio * 1280 * .25) + 15;// 195;
        }

        /// <summary>
        /// Add to the synopsis of a chapter
        /// </summary>
        /// <param name="syn"></param>
        public void UpdateSynopsis(String syn)
        {
            game.CurrentChapter.Synopsis += syn;

            switch (game.chapterState)
            {
                case Game1.ChapterState.prologue:
                    prologueSynopsisRead = false;
                    break;

                case Game1.ChapterState.chapterOne:
                    chOneSynopsisRead = false;
                    break;

                case Game1.ChapterState.chapterTwo:
                    chTwoSynopsisRead = false;
                    break;
            }

            Chapter.effectsManager.secondNotificationQueue.Enqueue(new JournalUpdateNotification(1));
        }

        public void Update()
        {

            #region OTHER TABS
            if (DarylsNotebook.questsTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.quests;
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
                game.Notebook.state = DarylsNotebook.State.bios;
                Chapter.effectsManager.RemoveToolTip();

                //The sound is automatically played once the bio page is loaded, so we need to make sure it doesn't play twice
                if (game.Notebook.BioPage.changeToBioSound == false)
                    Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }

            if (DarylsNotebook.inventoryTab.Clicked())
            {
                game.Notebook.state = DarylsNotebook.State.inventory;
                Chapter.effectsManager.RemoveToolTip();
                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
            }
            #endregion

            switch (game.chapterState)
            {
                case Game1.ChapterState.prologue:
                    currentChapterInt = 0;
                    break;
                case Game1.ChapterState.chapterOne:
                    currentChapterInt = 1;
                    break;
                case Game1.ChapterState.chapterTwo:
                    currentChapterInt = 2;
                    break;
            }

            if (chapterState == ChapterState.none)
            {
                switch (game.chapterState)
                {
                    case Game1.ChapterState.prologue:
                        chapterState = ChapterState.Prologue;
                        break;
                    case Game1.ChapterState.chapterOne:
                        chapterState = ChapterState.ChapterOne;
                        break;
                    case Game1.ChapterState.chapterTwo:
                        chapterState = ChapterState.ChapterTwo;
                        break;
                }
            }


            #region Chapter buttons
            for (int i = 0; i < chapterButtons.Count; i++)
            {
                if (chapterButtons[i].Clicked())
                {
                    if (currentChapterInt >= i)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                        synopsisPages.Clear();
                        synopsisPage = 0;

                        switch (i)
                        {
                            case 0:
                                chapterState = ChapterState.Prologue;
                                insideChapterState = InsideChapterState.story;
                                viewingSpecificQuest = false;
                                selectedQuest = null;
                                break;
                            case 1:
                                chapterState = ChapterState.ChapterOne;
                                insideChapterState = InsideChapterState.story;
                                viewingSpecificQuest = false;
                                selectedQuest = null;
                                break;
                            case 2:
                                chapterState = ChapterState.ChapterTwo;
                                insideChapterState = InsideChapterState.story;
                                viewingSpecificQuest = false;
                                selectedQuest = null;
                                break;
                        }
                    }
                }
            }
            #endregion

            #region CLICK SYNOPSIS

            if (synopsisButton.Clicked())
            {
                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab4);
                viewingSpecificQuest = false;
            }

            if (viewingSpecificQuest == false)
            {
                viewingSpecificQuest = false;
                selectedQuest = null;
                selectedIndex = 0;

                switch (chapterState)
                {
                    case ChapterState.Prologue:
                        prologueSynopsisRead = true;
                        break;

                    case ChapterState.ChapterOne:
                        chOneSynopsisRead = true;
                        break;

                    case ChapterState.ChapterTwo:
                        chTwoSynopsisRead = true;
                        break;
                }
            }
            #endregion


            //VIEWING SIDE QUESTS
            if (sideButton.Clicked())
            {
                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab2);
                Chapter.effectsManager.RemoveToolTip();
                insideChapterState = InsideChapterState.side;
                selectedQuest = null;
                selectedIndex = 0;
                viewingSpecificQuest = false;
                questPage = 0;
            }
            //VIEWING STORY QUESTS
            if (storyButton.Clicked())
            {
                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab2);
                Chapter.effectsManager.RemoveToolTip();
                insideChapterState = InsideChapterState.story;
                selectedQuest = null;
                selectedIndex = 0;
                viewingSpecificQuest = false;
                questPage = 0;
            }

            switch (chapterState)
            {
                case ChapterState.Prologue:

                    if (synopsisPages.Count == 0)
                        synopsisPages = WrapSynopsisText(Game1.expMoneyFloatingNumFont, game.Prologue.Synopsis, 280);
                    maxStoryQuestPage = game.Prologue.CompletedStoryQuests.Count / 11; //11 because you can fit 10 on a page, and if you do /10 it would mean there should be 2 pages for 10 quests
                    maxSideQuestPage = game.Prologue.CompletedSideQuests.Count / 11;

                    if (openedToSpecificQuest)
                    {
                        openedToSpecificQuest = false;

                        if (insideChapterState == InsideChapterState.side)
                        {
                            selectedIndex = game.Prologue.CompletedSideQuests.Count - 1 - (10 * maxSideQuestPage);
                            prologueSideQuestsRead[game.Prologue.CompletedSideQuests.Count - 1 - (10 * maxSideQuestPage)] = true;
                        }
                        else
                        {
                            selectedIndex = game.Prologue.CompletedStoryQuests.Count - 1 - (10 * maxStoryQuestPage);
                            prologueSideQuestsRead[game.Prologue.CompletedStoryQuests.Count - 1 - (10 * maxStoryQuestPage)] = true;
                        }

                    }

                    break;
                case ChapterState.ChapterOne:
                    if (synopsisPages.Count == 0)
                        synopsisPages = WrapSynopsisText(Game1.expMoneyFloatingNumFont, game.ChapterOne.Synopsis, 280);
                    maxStoryQuestPage = game.ChapterOne.CompletedStoryQuests.Count / 11; //11 because you can fit 10 on a page, and if you do /10 it would mean there should be 2 pages for 10 quests
                    maxSideQuestPage = game.ChapterOne.CompletedSideQuests.Count / 11;
                    break;
                case ChapterState.ChapterTwo:
                    if (synopsisPages.Count == 0)
                        synopsisPages = WrapSynopsisText(Game1.expMoneyFloatingNumFont, game.ChapterTwo.Synopsis, 280);
                    maxStoryQuestPage = game.ChapterTwo.CompletedStoryQuests.Count / 11; //11 because you can fit 10 on a page, and if you do /10 it would mean there should be 2 pages for 10 quests
                    maxSideQuestPage = game.ChapterTwo.CompletedSideQuests.Count / 11;

                    if (openedToSpecificQuest)
                    {
                        openedToSpecificQuest = false;

                        if (insideChapterState == InsideChapterState.side)
                        {
                            selectedIndex = game.ChapterTwo.CompletedSideQuests.Count - 1 - (10 * maxSideQuestPage);
                            chTwoSideQuestsRead[game.ChapterTwo.CompletedSideQuests.Count - 1 - (10 * maxSideQuestPage)] = true;
                        }
                        else
                        {
                            selectedIndex = game.ChapterTwo.CompletedStoryQuests.Count - 1 - (10 * maxStoryQuestPage);
                            chTwoSideQuestsRead[game.ChapterTwo.CompletedStoryQuests.Count - 1 - (10 * maxStoryQuestPage)] = true;
                        }

                    }

                    break;
            }

            #region Synopsis page
            if (!viewingSpecificQuest)
            {
                if (synopsisPages.Count > 1)
                {
                    if (synRight.Clicked() && synopsisPage < synopsisPages.Count - 1)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage2);
                        synopsisPage++;
                    }
                    if (synLeft.Clicked() && synopsisPage > 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage1);
                        synopsisPage--;
                    }
                }
            }
            #endregion

            #region QUEST PAGE ARROWS
            if (insideChapterState == InsideChapterState.story && maxStoryQuestPage > 0)
            {
                if (questLeft.Clicked())
                {
                    if (questPage > 0)
                    {
                        selectedQuest = null;
                        selectedIndex = 0;
                        questPage--;
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage1);
                    }
                }
                else if (questRight.Clicked())
                {
                    if (questPage < maxStoryQuestPage)
                    {
                        questPage++;
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPage2);
                        selectedQuest = null;
                        selectedIndex = 0;
                    }
                }
            }
            else if (insideChapterState == InsideChapterState.side && maxSideQuestPage > 0)
            {
                if (questLeft.Clicked())
                {
                    if (questPage > 0)
                    {
                        selectedQuest = null;
                        selectedIndex = 0;
                        questPage--;
                    }
                }
                else if (questRight.Clicked())
                {
                    if (questPage < maxSideQuestPage)
                    {
                        questPage++;
                        selectedQuest = null;
                        selectedIndex = 0;
                    }
                }
            }
            #endregion

            #region CLICK QUEST
            for (int i = 0; i < questButtons.Count; i++)
            {
                int questNum = i + (questPage * 10);

                Boolean validClick = true;

                if (questButtons[i].Clicked())
                {
                    if (insideChapterState == InsideChapterState.side)
                    {
                        switch (chapterState)
                        {
                            case ChapterState.Prologue:
                                if ((questNum > game.Prologue.CompletedSideQuests.Count - 1))
                                    validClick = false;
                                break;
                            case ChapterState.ChapterOne:
                                if ((questNum > game.ChapterOne.CompletedSideQuests.Count - 1))
                                    validClick = false;
                                break;
                            case ChapterState.ChapterTwo:
                                if ((questNum > game.ChapterTwo.CompletedSideQuests.Count - 1))
                                    validClick = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (chapterState)
                        {
                            case ChapterState.Prologue:
                                if ((questNum > game.Prologue.CompletedStoryQuests.Count - 1))
                                    validClick = false;
                                break;
                            case ChapterState.ChapterOne:
                                if ((questNum > game.ChapterOne.CompletedStoryQuests.Count - 1))
                                    validClick = false;
                                break;
                            case ChapterState.ChapterTwo:
                                if ((questNum > game.ChapterTwo.CompletedStoryQuests.Count - 1))
                                    validClick = false;
                                break;
                        }
                    }
                    if (validClick)
                    {
                        viewingSpecificQuest = true;
                        selectedIndex = i;
                        Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab1);

                        #region SIDE QUESTS
                        if (insideChapterState == InsideChapterState.side)
                        {

                            if (Game1.g.Prologue.PrologueBooleans["firstJournalSideQuest"])
                            {
                                Game1.g.Prologue.PrologueBooleans["firstJournalSideQuest"] = false;
                            }

                            if (chapterState == ChapterState.Prologue)
                            {
                                selectedQuest = game.Prologue.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value;

                                prologueSideQuestsRead[i + (questPage * 10)] = true;
                            }

                            if (chapterState == ChapterState.ChapterOne)
                            {
                                selectedQuest = game.ChapterOne.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value;

                                chOneSideQuestsRead[i + (questPage * 10)] = true;
                            }

                            if (chapterState == ChapterState.ChapterTwo)
                            {
                                selectedQuest = game.ChapterTwo.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value;

                                chTwoSideQuestsRead[i + (questPage * 10)] = true;
                            }
                        }
                        #endregion

                        #region STORY QUESTS
                        else if (insideChapterState == InsideChapterState.story)
                        {
                            if (Game1.g.Prologue.PrologueBooleans["firstJournalStoryQuest"])
                            {
                                Game1.g.Prologue.PrologueBooleans["firstJournalStoryQuest"] = false;
                            }

                            if (chapterState == ChapterState.Prologue)
                            {
                                selectedQuest = game.Prologue.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value;

                                prologueStoryQuestsRead[i + (questPage * 10)] = true;
                            }
                            if (chapterState == ChapterState.ChapterOne)
                            {
                                selectedQuest = game.ChapterOne.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value;

                                chOneStoryQuestsRead[i + (questPage * 10)] = true;
                            }
                            if (chapterState == ChapterState.ChapterTwo)
                            {
                                selectedQuest = game.ChapterTwo.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value;
                                chTwoStoryQuestsRead[i + (questPage * 10)] = true;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["journal"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            if (insideChapterState == InsideChapterState.story)
                s.Draw(textures["storyBox"], new Rectangle(352, 308, textures["storyBox"].Width, textures["storyBox"].Height), Color.White);
            else
                s.Draw(textures["sideBox"], new Rectangle(352, 308, textures["storyBox"].Width, textures["storyBox"].Height), Color.White);

            if (synopsisButton.IsOver())
            {
                if (firstFrameOverSynopsis)
                {
                    firstFrameOverSynopsis = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab1);
                }
                s.Draw(textures["synopsisActive"], new Rectangle(341, 191, textures["synopsisActive"].Width, textures["synopsisActive"].Height), Color.White);
            }
            else
            {
                firstFrameOverSynopsis = true;
                s.Draw(textures["synopsisStatic"], new Rectangle(341, 191, textures["synopsisActive"].Width, textures["synopsisActive"].Height), Color.White);
            }

            if (storyButton.IsOver() && insideChapterState != InsideChapterState.story)
            {
                if (firstFrameOverStory)
                {
                    firstFrameOverStory = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
                s.Draw(textures["storyActive"], new Rectangle(352, 308, textures["storyActive"].Width, textures["storyActive"].Height), Color.White);
            }
            else if (insideChapterState != InsideChapterState.story)
            {
                firstFrameOverStory = true;
                s.Draw(textures["storyStatic"], new Rectangle(391, 318, textures["storyStatic"].Width, textures["storyStatic"].Height), Color.White);
            }

            if (sideButton.IsOver() && insideChapterState != InsideChapterState.side)
            {
                if (firstFrameOverSide)
                {
                    firstFrameOverSide = false;
                    Sound.PlaySoundInstance(Sound.SoundNames.UIList2);
                }
                s.Draw(textures["sideActive"], new Rectangle(352, 308, textures["sideActive"].Width, textures["sideActive"].Height), Color.White);
            }
            else if (insideChapterState != InsideChapterState.side)
            {
                firstFrameOverSide = true;
                s.Draw(textures["sideStatic"], new Rectangle(391, 318, textures["sideStatic"].Width, textures["sideStatic"].Height), Color.White);
            }

            #region Quest Arrows
            if ((maxSideQuestPage > 0 && insideChapterState == InsideChapterState.side) || (maxStoryQuestPage > 0 && insideChapterState == InsideChapterState.story))
            {
                if (questLeft.IsOver())
                    s.Draw(textures["leftActive"], new Rectangle(410, 655, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                else
                    s.Draw(textures["leftStatic"], new Rectangle(410, 655, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);

                if(insideChapterState == InsideChapterState.side)
                    s.DrawString(Game1.twConQuestHudName, (questPage + 1) + " / " + (maxSideQuestPage + 1), new Vector2(482, 664), Color.Black);
                else
                    s.DrawString(Game1.twConQuestHudName, (questPage + 1) + " / " + (maxStoryQuestPage + 1), new Vector2(482, 664), Color.Black);

                if (questRight.IsOver())
                    s.Draw(textures["rightActive"], new Rectangle(410, 655, textures["rightActive"].Width, textures["rightActive"].Height), Color.White);
                else
                    s.Draw(textures["rightStatic"], new Rectangle(410, 655, textures["rightActive"].Width, textures["rightActive"].Height), Color.White);
            }
            #endregion

            #region Synopsis Arrows
            if ((synopsisPages.Count > 0 && !viewingSpecificQuest))
            {
                if (synLeft.IsOver())
                    s.Draw(textures["leftActive"], new Rectangle(846, 653, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                else
                    s.Draw(textures["leftStatic"], new Rectangle(846, 653, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);

                s.DrawString(Game1.twConQuestHudName, (synopsisPage + 1) + " / " + (synopsisPages.Count), new Vector2(918, 662), Color.Black);

                if (synRight.IsOver())
                    s.Draw(textures["rightActive"], new Rectangle(846, 653, textures["rightActive"].Width, textures["rightActive"].Height), Color.White);
                else
                    s.Draw(textures["rightStatic"], new Rectangle(846, 653, textures["rightActive"].Width, textures["rightActive"].Height), Color.White);
            }
            #endregion

            #region HOVER CIRCLE FOR CH BUTTONS
            for (int i = 0; i < chapterButtons.Count; i++)
            {
                if (chapterButtons[i].IsOver() && i <= currentChapterInt)
                {
                    s.Draw(textures["selectCircle"], new Rectangle(chapterButtons[i].ButtonRecX - 8, chapterButtons[i].ButtonRecY - 7, textures["selectCircle"].Width, textures["selectCircle"].Height), Color.White * .5f);

                    switch (i)
                    {
                        case 0:
                            if (firstFrameOverPrologue)
                            {
                                firstFrameOverPrologue = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 1:
                            if (firstFrameOverCh1)
                            {
                                firstFrameOverCh1 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 2:
                            if (firstFrameOverCh2)
                            {
                                firstFrameOverCh2 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 3:
                            if (firstFrameOverCh3)
                            {
                                firstFrameOverCh3 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 4:
                            if (firstFrameOverCh4)
                            {
                                firstFrameOverCh4 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 5:
                            if (firstFrameOverCh5)
                            {
                                firstFrameOverCh5 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                        case 6:
                            if (firstFrameOverCh6)
                            {
                                firstFrameOverCh6 = false;
                                Sound.PlaySoundInstance(Sound.SoundNames.UIPaperTab3);
                            }
                            break;
                    }
                }
                else if(!chapterButtons[i].IsOver())
                {
                    switch (i)
                    {
                        case 0:
                            firstFrameOverPrologue = true;
                            break;
                        case 1:
                            firstFrameOverCh1 = true;
                            break;
                        case 2:
                            firstFrameOverCh2 = true;
                            break;
                        case 3:
                            firstFrameOverCh3 = true;
                            break;
                        case 4:
                            firstFrameOverCh4 = true;
                            break;
                        case 5:
                            firstFrameOverCh5 = true;
                            break;
                        case 6:
                            firstFrameOverCh6 = true;
                            break;
                    }
                }

            }
            #endregion

            #region SELECT CIRCLE FOR CH BUTTONS
            switch (chapterState)
            {
                case ChapterState.Prologue:
                    s.Draw(textures["selectCircle"], new Rectangle(chapterButtons[0].ButtonRecX - 8, chapterButtons[0].ButtonRecY - 7, textures["selectCircle"].Width, textures["selectCircle"].Height), Color.White);
                    break;
                case ChapterState.ChapterOne:
                    s.Draw(textures["selectCircle"], new Rectangle(chapterButtons[1].ButtonRecX - 8, chapterButtons[1].ButtonRecY - 7, textures["selectCircle"].Width, textures["selectCircle"].Height), Color.White);
                    break;
                case ChapterState.ChapterTwo:
                    s.Draw(textures["selectCircle"], new Rectangle(chapterButtons[2].ButtonRecX - 7, chapterButtons[2].ButtonRecY - 7, textures["selectCircle"].Width, textures["selectCircle"].Height), Color.White);
                    break;
            }
            #endregion

            #region CHAPTER BUTTONS
            switch (game.chapterState)
            {
                case Game1.ChapterState.prologue:
                    s.Draw(textures["prologue"], new Rectangle(377, 108, textures["prologue"].Width, textures["prologue"].Height), Color.White);
                    break;
                case Game1.ChapterState.chapterOne:
                    s.Draw(textures["chOne"], new Rectangle(377, 108, textures["prologue"].Width, textures["prologue"].Height), Color.White);
                    break;
                case Game1.ChapterState.chapterTwo:
                    s.Draw(textures["chTwo"], new Rectangle(377, 108, textures["prologue"].Width, textures["prologue"].Height), Color.White);
                    break;
            }
            #endregion

            #region NEW ICON FOR CHAPTER BUTTONS
            if (prologueSynopsisRead == false || prologueSideQuestsRead.Contains(false) || prologueStoryQuestsRead.Contains(false))
            {
                s.Draw(DarylsNotebook.newIcon, new Rectangle(chapterButtons[0].ButtonRecX - 7, chapterButtons[0].ButtonRecY - 15, 28, 28), Color.White);
            }
            if (chOneSynopsisRead == false || chOneSideQuestsRead.Contains(false) || chOneStoryQuestsRead.Contains(false))
            {
                s.Draw(DarylsNotebook.newIcon, new Rectangle(chapterButtons[1].ButtonRecX - 7, chapterButtons[1].ButtonRecY - 15, 28, 28), Color.White);
            }
            if (chTwoSynopsisRead == false || chTwoSideQuestsRead.Contains(false) || chTwoStoryQuestsRead.Contains(false))
            {
                s.Draw(DarylsNotebook.newIcon, new Rectangle(chapterButtons[2].ButtonRecX - 7, chapterButtons[2].ButtonRecY - 15, 28, 28), Color.White);
            }
            #endregion

            #region SYNOPSIS AND SPECIFIC QUEST
            if (!viewingSpecificQuest)
            {

                s.Draw(textures["synopsisBox"], new Rectangle(787, 82, textures["synopsisBox"].Width, textures["synopsisBox"].Height), Color.White);

                if(synopsisPages.Count > 0)
                    s.DrawString(Game1.expMoneyFloatingNumFont, synopsisPages[synopsisPage], new Vector2(805, 150), Color.Black);

                switch (chapterState)
                {
                    case ChapterState.Prologue:
                        s.DrawString(Game1.twConMedium, "PROLOGUE", new Vector2(945 - Game1.twConMedium.MeasureString("PROLOGUE").X / 2, 88), Color.Black);
                        break;
                    case ChapterState.ChapterOne:
                        s.DrawString(Game1.twConMedium, "CHAPTER 1", new Vector2(945 - Game1.twConMedium.MeasureString("CHAPTER 1").X / 2, 88), Color.Black);
                        break;
                    case ChapterState.ChapterTwo:
                        s.DrawString(Game1.twConMedium, "CHAPTER 2", new Vector2(945 - Game1.twConMedium.MeasureString("CHAPTER 2").X / 2, 88), Color.Black);
                        break;
                }
            }
            else
            {
                switch (chapterState)
                {
                    case ChapterState.Prologue:
                        if (prologueSynopsisRead == false)
                        {
                            s.Draw(DarylsNotebook.newIcon, new Rectangle(synopsisButton.ButtonRecX - 45, synopsisButton.ButtonRecY + 12, 38, 38), Color.White);
                        }
                        break;
                    case ChapterState.ChapterOne:
                        if (chOneSynopsisRead == false)
                        {
                            s.Draw(DarylsNotebook.newIcon, new Rectangle(synopsisButton.ButtonRecX - 45, synopsisButton.ButtonRecY + 12, 38, 38), Color.White);
                        }
                        break;
                    case ChapterState.ChapterTwo:
                        if (chTwoSynopsisRead == false)
                        {
                            s.Draw(DarylsNotebook.newIcon, new Rectangle(synopsisButton.ButtonRecX - 45, synopsisButton.ButtonRecY + 12, 38, 38), Color.White);
                        }
                        break;
                }

                if (!game.Notebook.smallCharacterPortraits.ContainsKey(selectedQuest.npcName.ToLower()))
                {
                    //Load the NPC face
                    game.Notebook.BioPage.UnloadNPCAndEnemySprites();
                    game.Notebook.BioPage.loadedNPCEnemySprites = false;
                    game.Notebook.BioPage.changeToBioSound = true;
                    game.Notebook.smallCharacterPortraits.Add(selectedQuest.npcName.ToLower(), DarylsNotebook.BioNPCAndEnemyContentLoader.Load<Texture2D>(@"SmallNPCFaces\" + selectedQuest.npcName.ToLower()));
                }

                s.Draw(textures["questPage"], new Rectangle(778, 71, textures["questPage"].Width, textures["questPage"].Height), Color.White);

                if (selectedQuest.StoryQuest)
                {
                    s.Draw(textures["storyBar"], new Rectangle(783, 245, textures["storyBar"].Width, textures["storyBar"].Height), Color.White);
                    s.Draw(textures["storyQuestSelect"], questButtons[selectedIndex].ButtonRec, Color.White);
                }
                else
                {
                    s.Draw(textures["sideBar"], new Rectangle(783, 245, textures["sideBar"].Width, textures["sideBar"].Height), Color.White);
                    s.Draw(textures["sideQuestSelect"], questButtons[selectedIndex].ButtonRec, Color.White);
                }

                //NAME
                s.DrawString(Game1.twConRegularSmall, selectedQuest.QuestName, new Vector2(947 - Game1.twConRegularSmall.MeasureString(selectedQuest.QuestName).X / 2, 247), Color.White);
                //TASK
                s.DrawString(Game1.twConQuestHudName, Game1.WrapText(Game1.twConQuestHudName, "Task: " + selectedQuest.TaskForQuestsPage, 300), new Vector2(792, 285), Color.Black);

                s.Draw(game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()], new Rectangle(790, 45, game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()].Width, game.Notebook.smallCharacterPortraits[selectedQuest.npcName.ToLower()].Height), Color.White);

                s.DrawString(Game1.twConQuestHudInfo, Game1.WrapText(Game1.twConQuestHudInfo, selectedQuest.DescriptionForJournal, 305), new Vector2(795, 388), Color.Black);

                #region REWARDS
                for (int i = 0; i < selectedQuest.RewardObjects.Count; i++)
                {
                    if (selectedQuest.RewardObjects[i] is Equipment)
                    {
                        Equipment eq = selectedQuest.RewardObjects[i] as Equipment;

                        if (eq is Money)
                        {
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "$" + (eq as Money).Amount.ToString("N2"), new Vector2(950, 122 + (i * 27)), Color.Black);
                        }
                        else if (eq is Experience)
                        {
                            s.Draw(Game1.smallTypeIcons["smallExperienceIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "+ " + (eq as Experience).Amount.ToString(), new Vector2(950, 122 + (i * 27)), Color.Black);
                        }
                        else if (eq is Karma)
                        {
                            s.Draw(Game1.smallTypeIcons["smallKarmaIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            s.DrawString(Game1.font, "+ " + (eq as Karma).Amount.ToString(), new Vector2(950, 122 + (i * 27)), Color.Black);
                        }
                        else
                        {
                            s.DrawString(Game1.font, eq.Name, new Vector2(950, 122 + (i * 27)), Color.Black);
                            if (eq is Weapon)
                                s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            if (eq is Hat)
                                s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            if (eq is Hoodie)
                                s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                            if (eq is Accessory)
                                s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                        }
                    }
                    else if (selectedQuest.RewardObjects[i] is StoryItem)
                    {
                        StoryItem sItem = selectedQuest.RewardObjects[i] as StoryItem;
                        s.DrawString(Game1.twConQuestHudInfo, sItem.PickUpName, new Vector2(950, 122 + (i * 27)), Color.Black);
                        s.Draw(Game1.smallTypeIcons["smallStoryItemIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                    }
                    else if (selectedQuest.RewardObjects[i] is Collectible)
                    {
                        if (selectedQuest.RewardObjects[i] is Textbook)
                        {
                            s.DrawString(Game1.font, "Textbook", new Vector2(950, 122 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is BronzeKey)
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "Bronze Key", new Vector2(950, 122 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is SilverKey)
                        {
                            s.DrawString(Game1.font, "Silver Key", new Vector2(950, 122 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                        }
                        else if (selectedQuest.RewardObjects[i] is GoldKey)
                        {
                            s.DrawString(Game1.font, "Gold Key", new Vector2(950, 122 + (i * 27)), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(922, 124 + (i * 27), 20, 20), Color.White);
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region VIEWING SIDE QUESTS
            if (insideChapterState == InsideChapterState.side)
            {
                if (chapterState == ChapterState.Prologue)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.Prologue.CompletedSideQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.Prologue.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);
                            if (prologueSideQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY, 30, 30), Color.White);
                            }
                        }
                    }
                }

                if (chapterState == ChapterState.ChapterOne)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.ChapterOne.CompletedSideQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.ChapterOne.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);
                            if (chOneSideQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY, 30, 30), Color.White);
                            }
                        }
                    }
                }

                if (chapterState == ChapterState.ChapterTwo)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.ChapterTwo.CompletedSideQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.ChapterTwo.CompletedSideQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);

                            if (chTwoSideQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY, 30, 30), Color.White);
                            }
                        }
                    }
                }
            }
            #endregion

            #region VIEWING STORY QUESTS
            else if (insideChapterState == InsideChapterState.story)
            {
                if (chapterState == ChapterState.Prologue)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.Prologue.CompletedStoryQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.Prologue.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);
                            if (prologueStoryQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY, 30, 30), Color.White);
                            }
                        }
                    }
                }

                if (chapterState == ChapterState.ChapterOne)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.ChapterOne.CompletedStoryQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.ChapterOne.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);

                            if (chOneStoryQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY, 30, 30), Color.White);
                            }
                        }
                    }
                }

                if (chapterState == ChapterState.ChapterTwo)
                {
                    for (int i = 0; i < questButtons.Count; i++)
                    {
                        if (game.ChapterTwo.CompletedStoryQuests.Count > i + (questPage * 10))
                        {
                            s.DrawString(Game1.twConRegularSmall, game.ChapterTwo.CompletedStoryQuests.ElementAt(i + (questPage * 10)).Value.QuestName, new Vector2(360, 349 + (i * 30)), Color.White);

                            if (chTwoStoryQuestsRead[i + (questPage * 10)] == false)
                            {
                                s.Draw(DarylsNotebook.newIcon, new Rectangle(questButtons[i].ButtonRecX - 30, questButtons[i].ButtonRecY , 30, 30), Color.White);
                            }
                        }
                    }
                }
            }
            #endregion

        }

        //Takes in a string and adds line breaks according to the linewidth passed in
        public static List<string> WrapSynopsisText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            List<String> synopsisStrings = new List<string>();
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if(word.Contains("\n"))
                {
                    lineWidth = 0;
                }
                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    float y = spriteFont.MeasureString(sb.ToString() + "\n" + word + "+").Y;
                    if (y > 475)
                    {
                        synopsisStrings.Add(sb.ToString());
                        sb.Clear();
                    }

                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }

            }

            synopsisStrings.Add(sb.ToString());

            return synopsisStrings;
        }
    }
}
