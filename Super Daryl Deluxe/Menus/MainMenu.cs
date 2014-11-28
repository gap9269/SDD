#define DEMO

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

    public class MainMenu : BaseMenu
    {
        Button selectionBox;
        Vector2 saveBox1, saveBox2, saveBox3;
        int selectionState; // 0 is new, 1 is continue, 2 is options
        Cutscene fadeOut;
        Cursor cursor;
        int introTimer;
        int timeBeforeAfterIntro;
        float introAlpha = 1f;
        Texture2D logo, disclaimer, boxSmall, choices, savesTexture, overwriteTexture, saveBoxTextureStatic, saveBoxTextureActive, overwriteBoxTexture, loading, mainWords, selectGameText, overwriteYes, overwriteNo, rays, darylLoop, gradient, colorSplash;
        float rayRotation;
        int confirmSlot = 0;
        int introState = 0;
        float mainAlpha = 0f;
        int darylPosX;

        int otherTimer;

        int introFrame;
        int introDelay = 5;

        Dictionary<String, Texture2D> gamesLogoAnimation;

        Boolean overwriteWithoutTutorial = false;

        public static ContentManager content;

        enum State
        {
            intro,
            selecting,
            startNewGame,
            overwriteConfirm,
            loadGame,
            options
        }
        State state;

        public MainMenu(Game1 g, Cursor c):base(Game1.whiteFilter, g)
        {
            selectionState = 0;
            fadeOut = new Cutscene(game, game.Camera);
            cursor = c;
            state = State.intro;

            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";
        }

        public void LoadContent()
        {
            boxSmall = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuBoxSmall");
            logo = content.Load<Texture2D>(@"Menus\MainMenu\intro");
            disclaimer = content.Load<Texture2D>(@"Menus\MainMenu\intro2");
            background = content.Load<Texture2D>(@"Menus\MainMenu\MainMenu");
            choices = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuChoicesPax");
            mainWords = content.Load<Texture2D>(@"Menus\MainMenu\MainWordsPax");
            darylLoop = content.Load<Texture2D>(@"Menus\MainMenu\darylLoop");
            colorSplash = content.Load<Texture2D>(@"Menus\MainMenu\splashColor");
            gradient = content.Load<Texture2D>(@"Menus\MainMenu\gradientStrip");
            rays = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuRays");

            gamesLogoAnimation = ContentLoader.LoadContent(content, "Menus\\MainMenu\\D&GG Logo");

            saveBoxTextureStatic = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuSaveBoxStatic");
            saveBoxTextureActive = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuSaveBoxActive");
            overwriteBoxTexture = content.Load<Texture2D>(@"Menus\MainMenu\MainMenuOverwriteBox");
            overwriteTexture = content.Load<Texture2D>(@"Menus\MainMenu\OverwriteChoices");
            loading = content.Load<Texture2D>(@"Menus\MainMenu\Loading");

            selectGameText = content.Load<Texture2D>(@"Menus\MainMenu\selectGameText");
            overwriteYes = content.Load<Texture2D>(@"Menus\MainMenu\overWriteYes");
            overwriteNo = content.Load<Texture2D>(@"Menus\MainMenu\overWriteNo");
            selectionBox = new Button(boxSmall, new Rectangle(867, 439, boxSmall.Width, boxSmall.Height));
            saveBox1 = new Vector2(843, 77);
            saveBox2 = new Vector2(843, 288);
            saveBox3 = new Vector2(843, 498);
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public override void Update()
        {
            base.Update();
            cursor.Update();
            game.Camera.UpdateStaticTransform(game);

            //Sound.BackGoundMusic("Title", .5f);

            if (state != State.intro && mainAlpha < 1f)
            {
                mainAlpha += .05f;

                if(mainAlpha > 1f)
                    mainAlpha = 1f;
            }

            //THE DEMO ONLY
#if DEMO
            switch (state)
            {
                #region INTRO
                case State.intro:

                    if (introFrame < 9)
                    {
                        if (otherTimer > 60)
                        {
                            introDelay--;

                            if (introDelay <= 0)
                            {
                                introDelay = 5;
                                introFrame++;
                            }
                        }
                        else
                            otherTimer++;

                    }
                    else
                    {

                        timeBeforeAfterIntro++;

                        //--Fade the intro in, then wait 200 frames
                        if (introAlpha < 1 && introTimer == 0)
                        {
                            if (timeBeforeAfterIntro > 60)
                                introAlpha += .005f;
                        }
                        else
                            introTimer++;

                        //--Fade it back out

                            if (((introState == 0 && introTimer > 100) || (introState == 1 && introTimer > 550)) && introAlpha > 0)
                                introAlpha -= .015f;

                            if (introAlpha <= 0 && introTimer > 0)
                            {
                                if (introState == 0)
                                {
                                    introState = 1;
                                    introTimer = 0;
                                }
                                else
                                {
                                    introState = 2;
                                    introTimer = 0;
                                }

                            }


                        if (introTimer == 5 && introState == 2)
                            state = State.selecting;
                    }
                    break;
                #endregion

                #region SELECT NEW OR CONTINUE
                case State.selecting:

                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;

                    if (mainAlpha == 1f)
                    {
                        //--Press the New Game option
                        if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 0)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                            state = State.startNewGame;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }

                        //Press Load Game
                        if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                            state = State.loadGame;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }

                        //Press Options
                        if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                            state = State.options;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }


                        #region Change current selection
                        if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                        {
                            if (selectionState == 0)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                                selectionState = 1;
                            }
                            else if (selectionState == 1)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                                selectionState = 2;
                            }
                        }
                        else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                        {
                            if (selectionState == 1)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                                selectionState = 0;
                            }
                            else if (selectionState == 2)
                            {
                                selectionState = 1;
                                Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            }
                        }
                        #endregion
                    }


                    //--Changes the selection circle's position
                    switch (selectionState)
                    {
                        case 0:
                            selectionBox.ButtonRecX = 867;
                            selectionBox.ButtonRecY = 439;//(int)(1280 * Game1.aspectRatio * .6f) + 11;//443;
                            break;
                        case 1:
                            selectionBox.ButtonRecX = 867;
                            selectionBox.ButtonRecY = 516;//(int)(1280 * Game1.aspectRatio * .7f) + 23;//495;
                            break;
                        case 2:
                            selectionBox.ButtonRecX = 867;
                            selectionBox.ButtonRecY = 589;//(int)(1280 * Game1.aspectRatio * .85f) - 8; //604
                            break;

                    }


                    break;
                #endregion

                #region PLAY WITHOUT TUTORIAL
                case State.options:

                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;


                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                        if (selectionState == 0)
                        {
                            if (game.SaveLoadManager.saveOne)
                            {
                                overwriteWithoutTutorial = true;
                                state = State.overwriteConfirm;
                                confirmSlot = 1;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save1.sav";
                                fadeOut.State++;
                            }
                        }
                        else if (selectionState == 1)
                        {
                            if (game.SaveLoadManager.saveTwo)
                            {
                                overwriteWithoutTutorial = true;
                                state = State.overwriteConfirm;
                                confirmSlot = 2;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save2.sav";
                                fadeOut.State++;
                            }
                        }
                        else
                        {
                            if (game.SaveLoadManager.saveThree)
                            {
                                overwriteWithoutTutorial = true;
                                state = State.overwriteConfirm;
                                confirmSlot = 3;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save3.sav";
                                fadeOut.State++;
                            }
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        state = State.selecting;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        //FOR THE TUTORIAl/DEMO SHIT
                        game.CurrentChapter = game.ChapterTwo;
                        game.CurrentChapter.StartingPortal = TheParty.ToBehindTheParty;
                        game.chapterState = Game1.ChapterState.chapterTwo;

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["TheParty"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        Game1.Player.HasCellPhone = true;
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        //Starting without the tutorial set-up
                        game.CurrentChapter.CutsceneState = 4;
                        game.CurrentChapter.state = Chapter.GameState.Game;

                        //Game1.Player.Experience = Game1.Player.ExperienceUntilLevel;
                        //Game1.Player.LevelUp();
                        //Game1.Player.LevelingUp = false;
                        Game1.Player.Level = 14;
                        Game1.Player.MaxHealth = 1050;
                        Game1.Player.Health = 1050;
                        Game1.Player.Defense = 97;
                        Game1.Player.Strength = 1100;

                        game.CurrentChapter.HUD.SkillsHidden = false;

                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
                        Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);
                        Game1.Player.LearnedSkills[0].Equipped = true;
                        Game1.Player.LearnedSkills[0].SkillRank = 3;
                        Game1.Player.LearnedSkills[0].ApplyLevelUp();
                        Game1.Player.ExperienceUntilLevel = 3700;
                        Game1.Player.CanJump = true;

                        Chapter.effectsManager.skillMessageColor = Color.White;
                        Chapter.effectsManager.skillMessageTime = 0;

                        game.CurrentChapter.NPCs["Alan"].Dialogue.Clear();
                        game.CurrentChapter.NPCs["Alan"].Dialogue.Add("Keep your eye out for any of Trenchcoat Kid's employees. They always follow us here and try to steal our Textbook market.");

                        game.CurrentChapter.NPCs["Paul"].Dialogue.Clear();
                        game.CurrentChapter.NPCs["Paul"].Dialogue.Add("While I do respect a healthy dose of asset seizure, I don't understand why you still have Balto's cell phone. He's been bitching about it all night, and I can't imagine the texts he gets are very interesting.");

                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Quick Retort"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statement"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
                        game.Prologue.PrologueBooleans["firstSkillLocker"] = false;
                        game.Prologue.PrologueBooleans["firstSkillLockerWithSkill"] = false;
                        game.Prologue.PrologueBooleans["firstShop"] = false;
                        game.Prologue.PrologueBooleans["firstCombo"] = false;
                        game.Prologue.PrologueBooleans["firstInventory"] = false;
                        game.Prologue.PrologueBooleans["firstEquipped"] = false;
                        game.Prologue.PrologueBooleans["firstJournalChapter"] = false;
                        game.Prologue.PrologueBooleans["firstJournalSynopsis"] = false;
                        game.Prologue.PrologueBooleans["firstJournalStoryQuest"] = false;
                        game.Prologue.PrologueBooleans["firstJournalSideQuest"] = false;
                        game.MapBooleans.tutorialMapBooleans["TutorialSaved"] = true;

                        //(Game1.alwaysLoadedMaps as MapsThatAreAlwaysLoaded).SetAlwaysLoadedMapsDestinationPortals();
                    }

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 1;
                        }
                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab); 
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            selectionState = 0;
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                        }

                        else if (selectionState == 2)
                        {
                            selectionState = 1;
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                        }
                    }
                    #endregion
                    break;
                #endregion

                #region LOAD
                case State.loadGame:

                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 1;
                        }
                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 0;
                        }
                        else if (selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 1;
                        }
                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                        if (selectionState == 0 && game.SaveLoadManager.saveOne)
                        {
                            game.SaveLoadManager.filename = "save1.sav";
                            fadeOut.State++;
                        }
                        else if (selectionState == 1 && game.SaveLoadManager.saveTwo)
                        {
                            game.SaveLoadManager.filename = "save2.sav";
                            fadeOut.State++;
                        }
                        else if (selectionState == 2 && game.SaveLoadManager.saveThree)
                        {
                            game.SaveLoadManager.filename = "save3.sav";
                            fadeOut.State++;
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        state = State.selecting;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        game.SaveLoadManager.InitiateLoad();
                        game.CurrentChapter.StartingPortal = Bathroom.ToLastMap;
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Bathroom"];
                        game.CurrentChapter.CurrentMap.LoadContent();

                    }

                    break;
                #endregion

                #region OVERWRITE
                case State.overwriteConfirm:

                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                        {
                            selectionState = 1;
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 0;
                        }
                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);
                        if (selectionState == 0)
                        {
                            switch (confirmSlot)
                            {
                                case 1:
                                    game.SaveLoadManager.InitiateDeleteSave("save1.sav");
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save1.sav";
                                    fadeOut.State++;
                                    break;
                                case 2:
                                    game.SaveLoadManager.InitiateDeleteSave("save2.sav");
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save2.sav";
                                    fadeOut.State++;
                                    break;
                                case 3:
                                    game.SaveLoadManager.InitiateDeleteSave("save3.sav");
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save3.sav";
                                    fadeOut.State++;
                                    break;
                            }
                        }
                        else
                        {
                            selectionState = 0;
                            confirmSlot = 0;

                            if (overwriteWithoutTutorial == true)
                            {
                                overwriteWithoutTutorial = false;
                                state = State.options; //options is actually the "play w/o tutorial" option for the DEMO
                            }
                            else
                                state = State.startNewGame;
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        confirmSlot = 0;

                        if (overwriteWithoutTutorial == true)
                        {
                            overwriteWithoutTutorial = false;
                            state = State.options; //options is actually the "play w/o tutorial" option for the DEMO
                        }
                        else
                            state = State.startNewGame;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();

                        //If this is to play w/o tutorial, set the shit up correctly
                        if (overwriteWithoutTutorial == true)
                        {
                            game.CurrentChapter = game.ChapterTwo;
                            game.CurrentChapter.StartingPortal = TheParty.ToBehindTheParty;
                            game.chapterState = Game1.ChapterState.chapterTwo;

                            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["TheParty"];
                            game.CurrentChapter.CurrentMap.LoadContent();
                            Game1.Player.HasCellPhone = true;

                            //Starting without the tutorial set-up
                            game.CurrentChapter.CutsceneState = 4;
                            game.CurrentChapter.state = Chapter.GameState.Game;

                            //Game1.Player.Experience = Game1.Player.ExperienceUntilLevel;
                            //Game1.Player.LevelUp();
                            //Game1.Player.LevelingUp = false;
                            Game1.Player.Level = 14;
                            Game1.Player.MaxHealth = 1050;
                            Game1.Player.Health = 1050;
                            Game1.Player.Defense = 97;
                            Game1.Player.Strength = 1100;
                            Game1.Player.ExperienceUntilLevel = 3700;
                            game.CurrentChapter.HUD.SkillsHidden = false;

                            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
                            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);
                            Game1.Player.LearnedSkills[0].Equipped = true;
                            Game1.Player.LearnedSkills[0].SkillRank = 3;
                            Game1.Player.LearnedSkills[0].ApplyLevelUp();
                            Game1.Player.CanJump = true;

                            Chapter.effectsManager.skillMessageColor = Color.White;
                            Chapter.effectsManager.skillMessageTime = 0;

                            game.CurrentChapter.NPCs["Alan"].Dialogue.Clear();
                            game.CurrentChapter.NPCs["Alan"].Dialogue.Add("Keep your eye out for any of Trenchcoat Kid's employees. They always follow us here and try to steal our Textbook market.");

                            game.CurrentChapter.NPCs["Paul"].Dialogue.Clear();
                            game.CurrentChapter.NPCs["Paul"].Dialogue.Add("While I do respect a healthy dose of asset seizure, I don't understand why you still have Balto's cell phone. He's been bitching about it all night, and I can't imagine the texts he gets are very interesting.");

                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Quick Retort"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statement"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
                            game.Prologue.PrologueBooleans["firstSkillLocker"] = false;
                            game.Prologue.PrologueBooleans["firstSkillLockerWithSkill"] = false;
                            game.Prologue.PrologueBooleans["firstShop"] = false;
                            game.Prologue.PrologueBooleans["firstCombo"] = false;
                            game.Prologue.PrologueBooleans["firstInventory"] = false;
                            game.Prologue.PrologueBooleans["firstEquipped"] = false;
                            game.Prologue.PrologueBooleans["firstJournalChapter"] = false;
                            game.Prologue.PrologueBooleans["firstJournalSynopsis"] = false;
                            game.Prologue.PrologueBooleans["firstJournalStoryQuest"] = false;
                            game.Prologue.PrologueBooleans["firstJournalSideQuest"] = false;
                            game.MapBooleans.tutorialMapBooleans["TutorialSaved"] = true;

                        }//Otherwise start as normal
                        else
                        {
                            UnloadContent();
                            game.CurrentChapter = game.Prologue;
                            game.CurrentChapter.StartingPortal = MainLobby.ToArtHall;
                            game.chapterState = Game1.ChapterState.prologue;

                            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["MainLobby"];
                            game.CurrentChapter.CurrentMap.LoadContent();
                            Game1.Player.HasCellPhone = false;
                            Game1.schoolMaps.LoadEnemyData();
                            game.CurrentChapter.CurrentMap.LoadEnemyData();
                        }
                        game.CurrentChapter.CurrentMap.LoadEnemyData();
                    }
                    break;
                #endregion

                #region NEW
                case State.startNewGame:
                    
                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.UIEnter);

                        if (selectionState == 0)
                        {
                            if (game.SaveLoadManager.saveOne)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 1;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save1.sav";
                                fadeOut.State++;
                            }
                        }
                        else if (selectionState == 1)
                        {
                            if (game.SaveLoadManager.saveTwo)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 2;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save2.sav";
                                fadeOut.State++;
                            }
                        }
                        else
                        {
                            if (game.SaveLoadManager.saveThree)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 3;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save3.sav";
                                fadeOut.State++;
                            }
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        state = State.selecting;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        game.CurrentChapter = game.Prologue;
                        game.CurrentChapter.StartingPortal = MainLobby.ToArtHall;
                        game.chapterState = Game1.ChapterState.prologue;

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["MainLobby"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        Game1.Player.HasCellPhone = false;
                        Game1.schoolMaps.LoadEnemyData();
                        game.CurrentChapter.CurrentMap.LoadEnemyData();
                    }

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 1;
                        }

                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 0;
                        }
                        else if (selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.UITab);
                            selectionState = 1;
                        }
                    }
                    #endregion
                    break;
                #endregion
            }
#else
            switch (state)
            {
                #region INTRO
                case State.intro:


                    timeBeforeAfterIntro++;

                    //--Fade the intro in, then wait 200 frames
                    if (introAlpha < 1 && introTimer == 0)
                    {
                        if(timeBeforeAfterIntro > 60)
                            introAlpha += .005f;
                    }
                    else
                        introTimer++;

                    //--Fade it back out
                    if (((introState == 0 && introTimer > 100) || (introState == 1 && introTimer > 200)) && introAlpha > 0)
                        introAlpha -= .015f;

                    if (introAlpha <= 0 && introTimer > 0)
                    {
                        if (introState == 0)
                        {
                            introState = 1;
                            introTimer = 0;
                        }
                        else
                        {
                            introState = 2;
                            introTimer = 0;
                        }

                    }

                    if(introTimer == 5 && introState == 2)
                        state = State.selecting;

                    break;
                #endregion

                #region SELECT NEW OR CONTINUE
                case State.selecting:

                rayRotation += .5f;

                if (rayRotation == 360)
                    rayRotation = 0;

                if (mainAlpha == 1f)
                {
                    //--Press the New Game option
                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 0)
                    {
                        state = State.startNewGame;
                        game.SaveLoadManager.InitiateCheck();
                    }

                    //Press Load Game
                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 1)
                    {
                        state = State.loadGame;
                        game.SaveLoadManager.InitiateCheck();
                        selectionState = 0;
                    }

                    //Press Options
                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 2)
                    {
                        state = State.options;
                    }


                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                            selectionState = 1;

                        else if (selectionState == 1)
                            selectionState = 2;
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                            selectionState = 0;

                        else if (selectionState == 2)
                            selectionState = 1;
                    }
                    #endregion
                }


                    //--Changes the selection circle's position
                    switch (selectionState)
                    {
                        case 0:
                            selectionBox.ButtonRecX = 553;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio * .6f) + 11;//443;
                            break;
                        case 1:
                            selectionBox.ButtonRecX = 553;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio * .7f) + 23;//495;
                            break;
                        case 2:
                            selectionBox.ButtonRecX = 547;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio * .85f) - 8; //604
                            break;

                    }

                
                    break;
                #endregion

                #region OPTIONS
                case State.options:
                    game.Options.Update();

                    if (KeyPressed(Keys.Back) || MyGamePad.BPressed())
                        state = State.selecting;
                    break;
                #endregion

                #region LOAD
                case State.loadGame:

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                            selectionState = 1;

                        else if (selectionState == 1)
                            selectionState = 2;
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                            selectionState = 0;

                        else if (selectionState == 2)
                            selectionState = 1;
                    }


                    //--Changes the selection circle's position
                    switch (selectionState)
                    {
                        case 0:
                            selectionBox.ButtonRecX = 553;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 - 20;//340;
                            break;
                        case 1:
                            selectionBox.ButtonRecX = 553;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 + 50;//495;
                            break;
                        case 2:
                            selectionBox.ButtonRecX = 553;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 + 140;
                            break;

                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        if (selectionState == 0 && game.SaveLoadManager.saveOne)
                        {
                            game.SaveLoadManager.filename = "save1.sav";
                            fadeOut.State++;
                        }
                        else if (selectionState == 1 && game.SaveLoadManager.saveTwo)
                        {
                            game.SaveLoadManager.filename = "save2.sav";
                            fadeOut.State++;
                        }
                        else if(selectionState == 2 && game.SaveLoadManager.saveThree)
                        {
                            game.SaveLoadManager.filename = "save3.sav";
                            fadeOut.State++;
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        state = State.selecting;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        game.SaveLoadManager.InitiateLoad();
                        game.CurrentChapter.StartingPortal = Bathroom.ToLastMap;
                        game.CurrentChapter.CurrentMap = Game1.alwaysLoadedMaps.maps["Bathroom"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        
                    }

                    break;
                #endregion

                #region OVERWRITE
                case State.overwriteConfirm:

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                            selectionState = 1;

                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                            selectionState = 0;
                    }


                    //--Changes the selection circle's position
                    switch (selectionState)
                    {
                        case 0:
                            selectionBox.ButtonRecX = 500;
                            selectionBox.ButtonRecY = 275;
                            break;
                        case 1:
                            selectionBox.ButtonRecX = 500;
                            selectionBox.ButtonRecY = 375;
                            break;

                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        if (selectionState == 0)
                        {
                            switch (confirmSlot)
                            {
                                case 1:
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save1.sav";
                                    fadeOut.State++;
                                    break;
                                case 2:
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save2.sav";
                                    fadeOut.State++;
                                    break;
                                case 3:
                                    game.CreateNewSaveFileMapData();
                                    game.SaveLoadManager.filename = "save3.sav";
                                    fadeOut.State++;
                                    break;
                            }
                        }
                        else
                        {
                            selectionState = 0;
                            confirmSlot = 0;
                            state = State.startNewGame;
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        confirmSlot = 0;
                        state = State.startNewGame;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        //CHANGE THIS FOR STARTING IN OTHER CHAPTERS
                        game.CurrentChapter = game.ChapterTwo;
                        game.CurrentChapter.StartingPortal = OutsideTheParty.ToTheParty;
                        game.chapterState = Game1.ChapterState.chapterTwo;

                        game.CurrentChapter.CurrentMap = Game1.chelseasPartyMaps.maps["TheParty"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.loadedZoneOne = Game1.chelseasPartyMaps;
                        game.CurrentChapter.currentMapZoneState = Chapter.CurrentMapZone.chelseas;
                        Game1.Player.HasCellPhone = false;
                        Game1.chelseasPartyMaps.LoadEnemyData();
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        (Game1.alwaysLoadedMaps as MapsThatAreAlwaysLoaded).SetAlwaysLoadedMapsDestinationPortals();
                    }
                    break;
                #endregion

                #region NEW
                case State.startNewGame:
                    
                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        if (selectionState == 0)
                        {
                            if (game.SaveLoadManager.saveOne)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 1;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save1.sav";
                                fadeOut.State++;
                            }
                        }
                        else if (selectionState == 1)
                        {
                            if (game.SaveLoadManager.saveTwo)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 2;
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save2.sav";
                                fadeOut.State++;
                            }
                        }
                        else
                        {
                            if (game.SaveLoadManager.saveThree)
                            {
                                state = State.overwriteConfirm;
                                confirmSlot = 3; 
                                selectionState = 0;
                            }
                            else
                            {
                                game.CreateNewSaveFileMapData();
                                game.SaveLoadManager.filename = "save3.sav";
                                fadeOut.State++;
                            }
                        }
                    }

                    if ((KeyPressed(Keys.Back) || MyGamePad.BPressed()) && fadeOut.State == 0)
                    {
                        selectionState = 0;
                        state = State.selecting;
                    }

                    if (fadeOut.State == 1)
                    {
                        fadeOut.FadeOut(120);
                        fadeOut.Play();
                    }

                    if (fadeOut.State == 2)
                    {
                        UnloadContent();
                        /*
                        game.CurrentChapter = game.Prologue;
                        game.CurrentChapter.StartingPortal = NorthHall.ToArtHall;
                        game.chapterState = Game1.ChapterState.prologue;
                        game.CurrentChapter.CurrentMap = Game1.scienceMaps.maps["Science105"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.loadedZoneOne = Game1.scienceMaps;
                        game.CurrentChapter.currentMapZoneState = Chapter.CurrentMapZone.science;
                        Game1.scienceMaps.LoadEnemyData();

                        (Game1.alwaysLoadedMaps as MapsThatAreAlwaysLoaded).SetAlwaysLoadedMapsDestinationPortals();*/

                        //FOR THE TUTORIAl/DEMO SHIT
                        game.CurrentChapter = game.ChapterTwo;
                        game.CurrentChapter.StartingPortal = OutsideTheParty.ToTheParty;
                        game.chapterState = Game1.ChapterState.chapterTwo;

                        game.CurrentChapter.CurrentMap = Game1.chelseasPartyMaps.maps["OutsidetheParty"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.loadedZoneOne = Game1.chelseasPartyMaps;
                        game.CurrentChapter.currentMapZoneState = Chapter.CurrentMapZone.chelseas;
                        Game1.Player.HasCellPhone = false;
                        Game1.chelseasPartyMaps.LoadEnemyData();
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        (Game1.alwaysLoadedMaps as MapsThatAreAlwaysLoaded).SetAlwaysLoadedMapsDestinationPortals();
                    }

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                            selectionState = 1;

                        else if (selectionState == 1)
                            selectionState = 2;
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                            selectionState = 0;

                        else if (selectionState == 2)
                            selectionState = 1;
                    }
                    #endregion

                    //--Changes the selection circle's position
                    switch (selectionState)
                    {
                        case 0:
                            selectionBox.ButtonRecX = 500;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 - 20;//340;
                            break;
                        case 1:
                            selectionBox.ButtonRecX = 500;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 + 50;//495;
                            break;
                        case 2:
                            selectionBox.ButtonRecX = 500;
                            selectionBox.ButtonRecY = (int)(1280 * Game1.aspectRatio) / 2 + 140;
                            break;

                    }
                    break;
                #endregion
            }
#endif
        }

        public void ResetMainMenu()
        {
            selectionState = 0;
            fadeOut = new Cutscene(game, game.Camera);
            state = State.selecting;
        }

        public override void Draw(SpriteBatch s)
        {


            switch (state)
            {
                case State.intro:
                    if (introState == 0)
                    {
                        if (otherTimer > 59)
                            s.Draw(gamesLogoAnimation.ElementAt(introFrame).Value, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * introAlpha);
                        else
                            s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.White);
                    }
                    else if (introState == 1)
                        s.Draw(disclaimer, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * introAlpha);
                    break;

                case State.selecting:
                    backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(25, 25, 25) * mainAlpha);
                    s.Draw(gradient, new Vector2(0, 283), Color.White * mainAlpha);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
                    Game1.camera.Update(Game1.Player, game);
                    darylPosX -= 3;
                    s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.StaticTransform);
                    s.Draw(colorSplash, backgroundRec, Color.White * mainAlpha);
                    s.Draw(rays, new Rectangle(980, 220, 2200, 2200), null, Color.White * (mainAlpha - .5f), (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    s.Draw(choices, new Rectangle(845, 416, choices.Width, choices.Height), Color.White * mainAlpha);
                    selectionBox.Draw(s, mainAlpha);
                    s.Draw(mainWords, new Rectangle(845, 416, mainWords.Width, mainWords.Height), Color.White * mainAlpha);
                    break;

                case State.overwriteConfirm:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(25, 25, 25) * mainAlpha);
                    s.Draw(gradient, new Vector2(0, 283), Color.White * mainAlpha);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
                    Game1.camera.Update(Game1.Player, game);
                    darylPosX -= 3;
                    s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.StaticTransform);
                    s.Draw(colorSplash, backgroundRec, Color.White * mainAlpha);
                    s.Draw(rays, new Rectangle(980, 220, 2200, 2200), null, Color.White * (mainAlpha - .5f), (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                    {
                        if (selectionState == 0)
                            s.Draw(overwriteYes, new Vector2(778, 414), Color.White);
                        else
                            s.Draw(overwriteNo, new Vector2(778, 410), Color.White);
                    }
                    s.Draw(overwriteTexture, new Rectangle(778, 414, overwriteTexture.Width, overwriteTexture.Height), Color.White * mainAlpha);
                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.options:
                    //s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(25, 25, 25) * mainAlpha);
                    s.Draw(gradient, new Vector2(0, 283), Color.White * mainAlpha);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
                    Game1.camera.Update(Game1.Player, game);
                    darylPosX -= 3;
                    s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.StaticTransform);
                    s.Draw(colorSplash, backgroundRec, Color.White * mainAlpha);
                    s.Draw(rays, new Rectangle(980, 220, 2200, 2200), null, Color.White * (mainAlpha - .5f), (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                    {
                        switch (selectionState)
                        {
                            case 0:
                                s.Draw(saveBoxTextureActive, saveBox1, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 1:
                                s.Draw(saveBoxTextureActive, saveBox2, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 2:
                                s.Draw(saveBoxTextureActive, saveBox3, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                break;
                        }
                    }
                    s.Draw(selectGameText, new Vector2(950, 35), Color.White);
                    DrawSaves(s);
                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.startNewGame:

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(25, 25, 25) * mainAlpha);
                    s.Draw(gradient, new Vector2(0, 283), Color.White * mainAlpha);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
                    Game1.camera.Update(Game1.Player, game);
                    darylPosX -= 3;
                    s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.StaticTransform);
                    s.Draw(colorSplash, backgroundRec, Color.White * mainAlpha);
                    s.Draw(rays, new Rectangle(980, 220, 2200, 2200), null, Color.White * (mainAlpha - .5f), (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                    {
                        switch (selectionState)
                        {
                            case 0:
                                s.Draw(saveBoxTextureActive, saveBox1, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 1:
                                s.Draw(saveBoxTextureActive, saveBox2, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 2:
                                s.Draw(saveBoxTextureActive, saveBox3, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                break;
                        }
                    }
                    s.Draw(selectGameText, new Vector2(950, 35), Color.White);
                    DrawSaves(s);
                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.loadGame:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(25, 25, 25) * mainAlpha);
                    s.Draw(gradient, new Vector2(0, 283), Color.White * mainAlpha);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
                    Game1.camera.Update(Game1.Player, game);
                    darylPosX -= 3;
                    s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.StaticTransform);
                    s.Draw(colorSplash, backgroundRec, Color.White * mainAlpha);
                    s.Draw(rays, new Rectangle(980, 220, 2200, 2200), null, Color.White * (mainAlpha - .5f), (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                    {
                        switch (selectionState)
                        {
                            case 0:
                                s.Draw(saveBoxTextureActive, saveBox1, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 1:
                                s.Draw(saveBoxTextureActive, saveBox2, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox3, Color.White);
                                break;
                            case 2:
                                s.Draw(saveBoxTextureActive, saveBox3, Color.White);

                                s.Draw(saveBoxTextureStatic, saveBox1, Color.White);
                                s.Draw(saveBoxTextureStatic, saveBox2, Color.White);
                                break;
                        }
                    }
                    s.Draw(selectGameText, new Vector2(950, 35), Color.White);
                    DrawSaves(s);
                    fadeOut.DrawFade(s, 0f);
                    break;
            }
        }

        /*
        public override void Draw(SpriteBatch s)
        {


            switch (state)
            {
                case State.intro:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    if(introState == 0)
                        s.Draw(logo, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * introAlpha);
                    else if(introState == 1)
                        s.Draw(disclaimer, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * introAlpha);
                    break;

                case State.selecting:
                    backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));

                    //s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), new Color(225, 222, 222) * mainAlpha);

                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    selectionBox.Draw(s, mainAlpha);
                    s.Draw(rays, new Rectangle(478, 371, rays.Width, rays.Height), null, Color.White * mainAlpha, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(mainLogo, backgroundRec, Color.White * mainAlpha);
                    s.Draw(choices, backgroundRec, Color.White * mainAlpha);


                    break;

                case State.overwriteConfirm:
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                        overwriteBox.Draw(s);
                    s.Draw(rays, new Rectangle(478, 371, rays.Width, rays.Height), null, Color.White * mainAlpha, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(mainLogo, backgroundRec, Color.White * mainAlpha);
                    s.Draw(overwriteTexture, new Rectangle(796, 401, overwriteTexture.Width, overwriteTexture.Height), Color.White * mainAlpha);
                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.options:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);
                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                        saveBox.Draw(s);
                    s.Draw(rays, new Rectangle(478, 371, rays.Width, rays.Height), null, Color.White * mainAlpha, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(mainLogo, backgroundRec, Color.White * mainAlpha);
                    s.Draw(savesTexture, new Rectangle(940, 31, savesTexture.Width, savesTexture.Height), Color.White * mainAlpha);
                    DrawSaves(s);
                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.startNewGame:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);

                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                        saveBox.Draw(s);
                    s.Draw(rays, new Rectangle(478, 371, rays.Width, rays.Height), null, Color.White * mainAlpha, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(mainLogo, backgroundRec, Color.White * mainAlpha);
                    s.Draw(savesTexture, new Rectangle(940, 31, savesTexture.Width, savesTexture.Height), Color.White * mainAlpha);
                    DrawSaves(s);

                    fadeOut.DrawFade(s, 0f);
                    break;

                case State.loadGame:
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * .7f);

                    s.Draw(background, backgroundRec, Color.White * mainAlpha);
                    if (fadeOut.State < 2)
                        saveBox.Draw(s);
                    s.Draw(rays, new Rectangle(478, 371, rays.Width, rays.Height), null, Color.White * mainAlpha, (float)(rayRotation * (Math.PI / 180)), new Vector2(rays.Width / 2, rays.Height / 2), SpriteEffects.None, 0f);
                    s.Draw(mainLogo, backgroundRec, Color.White * mainAlpha);
                    s.Draw(savesTexture, new Rectangle(940, 31, savesTexture.Width, savesTexture.Height), Color.White * mainAlpha);

                    DrawSaves(s);
                    fadeOut.DrawFade(s, 0f);
                    break;
            }
        }

        */

        public void DrawSaves(SpriteBatch s)
        {
            if (game.SaveLoadManager.saveOne)
            {
                String[] preview = game.SaveLoadManager.saveOnePreview.Split('\n');

                for (int i = 0; i < preview.Length; i++)
                {
                    s.DrawString(Game1.twConQuestHudName, preview[i], new Vector2(902, 96 + (i * 23)), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                }
            }
            else
            {
                s.DrawString(Game1.twConQuestHudName, "Empty Slot", new Vector2(902, 96), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            }


            if (game.SaveLoadManager.saveTwo)
            {
                String[] preview = game.SaveLoadManager.saveTwoPreview.Split('\n');

                for (int i = 0; i < preview.Length; i++)
                {
                    s.DrawString(Game1.twConQuestHudName, preview[i], new Vector2(902, 309 + (i * 23)), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                }
            }
            else
            {
                s.DrawString(Game1.twConQuestHudName, "Empty Slot", new Vector2(902, 309), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            }


            if (game.SaveLoadManager.saveThree)
            {
                String[] preview = game.SaveLoadManager.saveThreePreview.Split('\n');

                for (int i = 0; i < preview.Length; i++)
                {
                    s.DrawString(Game1.twConQuestHudName, preview[i], new Vector2(902, 521 + (i * 23)), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
                }
            }
            else
            {
                s.DrawString(Game1.twConQuestHudName, "Empty Slot", new Vector2(902, 521), Color.White, 0, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            }
        }
    }
}
