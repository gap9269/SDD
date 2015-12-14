#define DEMO

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
            state = State.selecting;

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
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
                            state = State.startNewGame;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }

                        //Press Load Game
                        if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
                            state = State.loadGame;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }

                        //Press Options
                        if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
                            state = State.options;
                            game.SaveLoadManager.InitiateCheck();
                            selectionState = 0;
                        }


                        #region Change current selection
                        if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                        {
                            if (selectionState == 0)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                                selectionState = 1;
                            }
                            else if (selectionState == 1)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                                selectionState = 2;
                            }
                        }
                        else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                        {
                            if (selectionState == 1)
                            {
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                                selectionState = 0;
                            }
                            else if (selectionState == 2)
                            {
                                selectionState = 1;
                                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
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

                #region CH2 Demo
                case State.options:

                    rayRotation += .25f;

                    if (rayRotation == 360)
                        rayRotation = 0;


                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
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
                        game.CurrentChapter = game.ChapterTwoDemo;

                        Player player = Game1.Player;

                        Game1.schoolMaps.maps["Stone Fort Gate"] = new OutsideStoneFortDemo(new List<Texture2D>(), game, ref player);
                        Game1.schoolMaps.maps["Stone Fort - Central"] = new StoneFortCentralDemo(new List<Texture2D>(), game, ref player);
                        Game1.schoolMaps.maps["Stone Fort - West"] = new StoneFortWestDemo(new List<Texture2D>(), game, ref player);
                        Game1.schoolMaps.maps["Stone Fort - East"] = new StoneFortEastDemo(new List<Texture2D>(), game, ref player);
                        Game1.schoolMaps.maps["Stone Fort Wasteland"] = new StoneFortWastelandDemo(new List<Texture2D>(), game, ref player);
                        Game1.schoolMaps.maps["Axis of Historical Reality"] = new AxisOfHistoricalRealityDemo(new List<Texture2D>(), game, ref player);

                        Game1.schoolMaps.maps["Stone Fort Gate"].SetDestinationPortals();
                        Game1.schoolMaps.maps["Stone Fort - Central"].SetDestinationPortals();
                        Game1.schoolMaps.maps["Stone Fort - West"].SetDestinationPortals();
                        Game1.schoolMaps.maps["Stone Fort - East"].SetDestinationPortals();
                        Game1.schoolMaps.maps["Stone Fort Wasteland"].SetDestinationPortals();
                        Game1.schoolMaps.maps["Axis of Historical Reality"].SetDestinationPortals();

                        game.CurrentChapter.StartingPortal = OutsideStoneFort.ToBathroom;
                        game.chapterState = Game1.ChapterState.demo;

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Stone Fort Gate"];
                        Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;

                        game.CurrentChapter.CurrentMap.LoadContent();
                        Game1.Player.HasCellPhone = true;
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        //Starting without the tutorial set-up
                        game.CurrentChapter.CutsceneState = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;

                        game.ChapterTwoDemo.SetPlayerStatsForDemo();

                        Chapter.effectsManager.skillMessageColor = Color.White;
                        Chapter.effectsManager.skillMessageTime = 0;

                        game.YourLocker.SkillsOnSale.Clear();

                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Blinding Logic"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.3"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.2"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Cutting Corners"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Crushing Realization"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.3"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.2"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
                        //game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);

                        player.LearnedSkills.Add(SkillManager.AllSkills["Cutting Corners"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.3"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.2"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Shocking Statements CH.3"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Shocking Statements CH.2"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Mopping Up"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Crushing Realization"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Fowl Mouth"]);
                        player.LearnedSkills.Add(SkillManager.AllSkills["Sharp Comments"]);

                        foreach (Skill s in Game1.Player.LearnedSkills)
                        {
                            s.SkillRank = 13;
                            s.ApplyLevelUp(true);
                        }



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
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 1;
                        }
                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab); 
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            selectionState = 0;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                        }

                        else if (selectionState == 2)
                        {
                            selectionState = 1;
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
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
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 1;
                        }
                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 0;
                        }
                        else if (selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 1;
                        }
                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
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

                        Sound.PlaySoundInstance(Sound.SoundNames.popup_load_game);

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
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 0;
                        }
                    }
                    #endregion

                    if ((KeyPressed(Keys.Enter) || MyGamePad.APressed()) && fadeOut.State == 0)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);
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
                            UnloadContent();
                            //FOR THE TUTORIAl/DEMO SHIT
                            game.CurrentChapter = game.ChapterTwoDemo;

                            Player player = Game1.Player;

                            Game1.schoolMaps.maps["Stone Fort Gate"] = new OutsideStoneFortDemo(new List<Texture2D>(), game, ref player);
                            Game1.schoolMaps.maps["Stone Fort - Central"] = new StoneFortCentralDemo(new List<Texture2D>(), game, ref player);
                            Game1.schoolMaps.maps["Stone Fort - West"] = new StoneFortWestDemo(new List<Texture2D>(), game, ref player);
                            Game1.schoolMaps.maps["Stone Fort - East"] = new StoneFortEastDemo(new List<Texture2D>(), game, ref player);
                            Game1.schoolMaps.maps["Stone Fort Wasteland"] = new StoneFortWastelandDemo(new List<Texture2D>(), game, ref player);
                            Game1.schoolMaps.maps["Axis of Historical Reality"] = new AxisOfHistoricalRealityDemo(new List<Texture2D>(), game, ref player);

                            Game1.schoolMaps.maps["Stone Fort Gate"].SetDestinationPortals();
                            Game1.schoolMaps.maps["Stone Fort - Central"].SetDestinationPortals();
                            Game1.schoolMaps.maps["Stone Fort - West"].SetDestinationPortals();
                            Game1.schoolMaps.maps["Stone Fort - East"].SetDestinationPortals();
                            Game1.schoolMaps.maps["Stone Fort Wasteland"].SetDestinationPortals();
                            Game1.schoolMaps.maps["Axis of Historical Reality"].SetDestinationPortals();

                            game.CurrentChapter.StartingPortal = OutsideStoneFort.ToBathroom;
                            game.chapterState = Game1.ChapterState.demo;

                            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Stone Fort Gate"];
                            Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;

                            game.CurrentChapter.CurrentMap.LoadContent();
                            Game1.Player.HasCellPhone = true;
                            game.CurrentChapter.CurrentMap.LoadEnemyData();

                            //Starting without the tutorial set-up
                            game.CurrentChapter.CutsceneState = 0;
                            game.CurrentChapter.state = Chapter.GameState.Game;

                            game.ChapterTwoDemo.SetPlayerStatsForDemo();

                            Chapter.effectsManager.skillMessageColor = Color.White;
                            Chapter.effectsManager.skillMessageTime = 0;

                            game.YourLocker.SkillsOnSale.Clear();

                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Quick Retort"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Blinding Logic"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.3"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.2"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Cutting Corners"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Crushing Realization"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.3"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.2"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
                            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);

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
                            SetVariablesPrologue();
                            Game1.schoolMaps.LoadEnemyData();
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
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_enter);

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
                        game.chapterState = Game1.ChapterState.chapterTwo;
                        game.CurrentChapter = game.ChapterTwo;
                        game.CurrentChapter.state = Chapter.GameState.Cutscene;
                        game.CurrentChapter.CutsceneState = 1;
                        game.CurrentChapter.StartingPortal = NorthHall.ToBathroom;
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["North Hall"];
                        game.CurrentChapter.AddNPCs();

                        //Temp shit
                        game.ChapterOne.ChapterOneBooleans["quickRetortObtained"] = true;
                        game.ChapterOne.ChapterOneBooleans["meetingNapleonSceneStarted"] = true;
                        game.ChapterOne.ChapterOneBooleans["battlefieldCleared"] = true;
                        NorthHall.ToScienceIntroRoom.ItemNameToUnlock = "";
                        game.ChapterOne.ChapterOneBooleans["completedMapsQuest"] = true;
                        Game1.schoolMaps.maps["Upper Vents I"].Switches[0].Active = true;
                        //end temp shit

                        Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;
                        game.CurrentChapter.CurrentMap.LoadContent();


                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Cutting Corners"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.2"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.3"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.2"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.3"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);

                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Fowl Mouth"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Sharp Comments"]);
                        Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Crushing Realization"]);
                        Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);
                        Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[3]);
                        Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[4]);
                        Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[2]);

                        Game1.Player.EquippedSkills[0].LoadContent();
                        Game1.Player.EquippedSkills[0].Equipped = true;
                        Game1.Player.EquippedSkills[1].LoadContent();
                        Game1.Player.EquippedSkills[1].Equipped = true;
                        Game1.Player.EquippedSkills[2].LoadContent();
                        Game1.Player.EquippedSkills[2].Equipped = true;
                        Game1.Player.EquippedSkills[3].LoadContent();
                        Game1.Player.EquippedSkills[3].Equipped = true;

                        Game1.Player.OwnedWeapons.Add(new ComposersWand());
                        Game1.Player.OwnedWeapons.Add(new AverageWeenieProtractor());
                        Game1.Player.OwnedWeapons.Add(new Paintbrush());
                        Game1.Player.OwnedHats.Add(new BandHat());
                        Game1.Player.OwnedHats.Add(new AverageWeenieHat());
                        Game1.Player.OwnedHats.Add(new Beret());
                        Game1.Player.OwnedAccessories.Add(new EighthOfButter());
                        Game1.Player.OwnedAccessories.Add(new AverageWeenieCalculator());
                        Game1.Player.OwnedAccessories.Add(new VenusOfWillendorf());
                        Game1.Player.OwnedAccessories.Add(new YinYangNecklace());
                        Game1.Player.OwnedHoodies.Add(new BandUniform());
                        Game1.Player.OwnedHoodies.Add(new ArtSmock());
                        Game1.Player.OwnedHoodies.Add(new AverageWeenieShirt());

                        Game1.Player.OwnedWeapons.Add(new Marker());
                        Game1.Player.OwnedHats.Add(new Fez());
                        Game1.Player.OwnedAccessories.Add(new RileysBow());
                        Game1.Player.OwnedAccessories.Add(new LabGoggles());
                        Game1.Player.OwnedHoodies.Add(new LabCoat());

                        Game1.Player.EquippedSkills[0].SkillRank = 3;
                        Game1.Player.EquippedSkills[0].ApplyLevelUp(true);
                        foreach (Skill s in Game1.Player.LearnedSkills)
                        {
                            s.SkillRank = 7;
                            s.ApplyLevelUp(true);
                        }

                        Game1.Player.quickRetort.SkillRank = 7;
                        Game1.Player.quickRetort.ApplyLevelUp(true);

                        Game1.Player.CanJump = true;
                        Game1.Player.HasCellPhone = false;
                        Game1.Player.Karma = 33;
                        Game1.Player.LevelUpToLevel(9);

                        Game1.Player.PositionX = 420;
                        Game1.Player.PositionY = 0;
                        Game1.schoolMaps.LoadEnemyData();
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        //UnloadContent();
                        //SetVariablesPrologue();
                        //Game1.schoolMaps.LoadEnemyData();
                    }

                    #region Change current selection
                    if (KeyPressed(Keys.Down) || MyGamePad.DownPadPressed())
                    {
                        if (selectionState == 0)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 1;
                        }

                        else if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 2;
                        }
                    }
                    else if (KeyPressed(Keys.Up) || MyGamePad.UpPadPressed())
                    {
                        if (selectionState == 1)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                            selectionState = 0;
                        }
                        else if (selectionState == 2)
                        {
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
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

                        game.CurrentChapter.CurrentMap = Game1.chelseasPartyMaps.maps["The Party"];
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
                        game.CurrentChapter.CurrentMap = Game1.scienceMaps.maps["Science 105"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.loadedZoneOne = Game1.scienceMaps;
                        game.CurrentChapter.currentMapZoneState = Chapter.CurrentMapZone.science;
                        Game1.scienceMaps.LoadEnemyData();

                        (Game1.alwaysLoadedMaps as MapsThatAreAlwaysLoaded).SetAlwaysLoadedMapsDestinationPortals();*/

                        //FOR THE TUTORIAl/DEMO SHIT
                        game.CurrentChapter = game.ChapterTwo;
                        game.CurrentChapter.StartingPortal = OutsideTheParty.ToTheParty;
                        game.chapterState = Game1.ChapterState.chapterTwo;

                        game.CurrentChapter.CurrentMap = Game1.chelseasPartyMaps.maps["Outside the Party"];
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
                    //s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
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
                    //s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
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
                    //s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
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
                   // s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
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
                    //s.Draw(darylLoop, new Rectangle(0, 540, darylLoop.Width, darylLoop.Height), new Rectangle(darylPosX, 0, darylLoop.Width, darylLoop.Height), Color.White);
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

        public void SetVariablesChapterTwo()
        {
            game.chapterState = Game1.ChapterState.chapterTwo;
            game.CurrentChapter = game.ChapterTwo;
            game.CurrentChapter.state = Chapter.GameState.Game;
            game.CurrentChapter.CutsceneState = 2;
            game.CurrentChapter.StartingPortal = WindyDesert.ToDryDesert;
            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Main Lobby"];
            game.CurrentChapter.AddNPCs();
            Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;

            Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;
            game.CurrentChapter.CurrentMap.LoadContent();

            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);
            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.2"]);

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[1]);

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Mopping Up"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Combustible Confutation CH.2"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Fowl Mouth"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Sharp Comments"]);
            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Crushing Realization"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[4]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[5]);

            Game1.Player.EquippedSkills[0].LoadContent();
            Game1.Player.LearnedSkills[0].Equipped = true;
            Game1.Player.EquippedSkills[1].LoadContent();
            Game1.Player.LearnedSkills[1].Equipped = true;
            Game1.Player.EquippedSkills[2].LoadContent();
            Game1.Player.LearnedSkills[2].Equipped = true;
            Game1.Player.EquippedSkills[3].LoadContent();
            Game1.Player.LearnedSkills[3].Equipped = true;

            Game1.Player.OwnedWeapons.Add(new ComposersWand());
            Game1.Player.OwnedWeapons.Add(new Paintbrush());
            Game1.Player.OwnedHats.Add(new BandHat());
            Game1.Player.OwnedHats.Add(new Beret());
            Game1.Player.OwnedAccessories.Add(new ArtistsPalette());
            Game1.Player.OwnedAccessories.Add(new VenusOfWillendorf());
            Game1.Player.OwnedAccessories.Add(new YinYangNecklace());
            Game1.Player.OwnedHoodies.Add(new BandUniform());
            Game1.Player.OwnedHoodies.Add(new ArtSmock());

            foreach (Skill s in Game1.Player.LearnedSkills)
            {
                s.SkillRank = 5;
                s.ApplyLevelUp(true);
            }

            Game1.Player.CanJump = true;
            Game1.Player.HasCellPhone = false;
            Game1.Player.Karma = 33;
            Game1.Player.LevelUpToLevel(10);

            Game1.Player.PositionX = 1420;
            Game1.Player.PositionY = 0;
        }

        public void SetVariablesChapterOne()
        {
            game.chapterState = Game1.ChapterState.chapterOne;
            game.CurrentChapter = game.ChapterOne;
            game.CurrentChapter.state = Chapter.GameState.Game;
            game.CurrentChapter.CutsceneState = 1;
            game.CurrentChapter.StartingPortal = GymLobby.ToNorthHall;
            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Gym Lobby"];
            game.CurrentChapter.AddNPCs();
            Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;

            Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;
            game.CurrentChapter.CurrentMap.LoadContent();

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Blinding Logic"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[1]);

            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Quick Retort"]);
            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
            game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Crushing Realization"]);

            Game1.Player.OwnedWeapons.Add(new Marker());
            Game1.Player.OwnedHats.Add(new Fez());
            Game1.Player.OwnedAccessories.Add(new RileysBow());
            Game1.Player.OwnedAccessories.Add(new LabGoggles());
            Game1.Player.OwnedHoodies.Add(new LabCoat());

            Game1.Player.EquippedSkills[0].SkillRank = 3;
            Game1.Player.EquippedSkills[0].ApplyLevelUp(true);
            Game1.Player.EquippedSkills[0].LoadContent();
            Game1.Player.EquippedSkills[1].LoadContent();

            Game1.Player.CanJump = true;
            Game1.Player.HasCellPhone = false;
            Game1.Player.Karma = 10;
            Game1.Player.LevelUpToLevel(4);

            Game1.Player.PositionX = 1420;
            Game1.Player.PositionY = 0;
        }

        public void SetVariablesPrologue()
        {
            game.CurrentChapter = game.Prologue;
            game.CurrentChapter.StartingPortal = MainLobby.ToArtHall;
            game.chapterState = Game1.ChapterState.prologue;
            game.CurrentChapter.state = Chapter.GameState.Cutscene;
            game.CurrentChapter.CutsceneState = 1;
            game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Main Lobby"];
            game.CurrentChapter.CurrentMap.LoadContent();
            Game1.Player.HasCellPhone = false;
        }


    }
}
