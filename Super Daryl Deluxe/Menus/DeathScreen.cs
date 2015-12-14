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

    public class DeathScreen : BaseMenu
    {
        int holdBlackScreenTimer;
        Button mainMenu, continueLoad;
        Cutscene fadeOut;
        ContentManager content;
        Boolean exitingScreen = false;

        float wordsAlpha = 0f;
        int deathFrame;
        int frameDelay = 5;
        float playerPosY;

        int currentSelection; //0 = Main Menu, 1 = load game

        public bool load, main;

        Texture2D fog, words, menuActive, continueActive, light;

        Boolean canContinue = false;

        public DeathScreen(Game1 g)
            : base(Game1.whiteFilter, g)
        {
            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";

            fadeOut = new Cutscene(game, game.Camera);
            mainMenu = new Button(new Rectangle(505, 595, 295, 40));
            buttons.Add(mainMenu);
            continueLoad = new Button(new Rectangle(433, 650, 450, 50));
            buttons.Add(continueLoad);
            load = false;
            main = false;
        }

        public void LoadContent()
        {
            background = content.Load<Texture2D>(@"Menus\Death\background");
            light = content.Load<Texture2D>(@"Menus\Death\holyLight");
            continueActive = content.Load<Texture2D>(@"Menus\Death\continueActive");
            fog = content.Load<Texture2D>(@"Menus\Death\fog");
            words = content.Load<Texture2D>(@"Menus\Death\words");
            menuActive = content.Load<Texture2D>(@"Menus\Death\mainMenuActive");
            playerPosY = 720;

            game.SaveLoadManager.InitiateCheck();

            if ((game.SaveLoadManager.filename == "save1.sav" && game.SaveLoadManager.saveOne) || (game.SaveLoadManager.filename == "save2.sav" && game.SaveLoadManager.saveTwo) || (game.SaveLoadManager.filename == "save3.sav" && game.SaveLoadManager.saveThree))
            {
                canContinue = true;
            }
        }

        public void UnloadContent()
        {

        }

        public override void Update()
        {
            base.Update();

            if (!exitingScreen)
            {
                if ((((current.IsKeyUp(Keys.Down) && previous.IsKeyDown(Keys.Down)) || MyGamePad.DownPadPressed()) && currentSelection == 0) || continueLoad.IsOver())
                    currentSelection = 1;
                else if ((((current.IsKeyUp(Keys.Up) && previous.IsKeyDown(Keys.Up)) || MyGamePad.UpPadPressed()) && currentSelection == 1) || mainMenu.IsOver())
                    currentSelection = 0;
            }

            if (wordsAlpha < 1)
            {
                wordsAlpha += .005f;
            }

            frameDelay--;

            if (frameDelay == 0)
            {
                deathFrame++;
                frameDelay = 8;

                if (deathFrame == 5)
                    deathFrame = 0;
            }

            if (playerPosY != 111)
            {

                playerPosY -= 2;

                if (playerPosY < 111)
                    playerPosY = 111;
            }

            fadeOut.Play();

            if (mainMenu.Clicked() || (((current.IsKeyUp(Keys.Enter) && previous.IsKeyDown(Keys.Enter)) || MyGamePad.APressed()) && currentSelection == 0))
            {
                main = true;
                exitingScreen = true;
            }

            if (continueLoad.Clicked() || (((current.IsKeyUp(Keys.Enter) && previous.IsKeyDown(Keys.Enter)) || MyGamePad.APressed()) && currentSelection == 1))
            {
                if (canContinue)
                {
                    load = true;
                    exitingScreen = true;
                }
            }

            if (load == true || main == true)
            {
                fadeOut.FadeOut(120);
            }

            if (fadeOut.State == 1)
            {
                if (main)
                {
                    holdBlackScreenTimer++;

                    //Hold the black screen for a couple seconds
                    if (holdBlackScreenTimer >= 120)
                    {
                        game.ResetGameAndGoToMain();
                        Reset();
                    }
                }
                else if (load)
                {
                    holdBlackScreenTimer++;

                    //Hold it for 2 frames, that way the "loading" text is drawn
                    if (holdBlackScreenTimer >= 2)
                    {
                        Game1.schoolMaps.UnloadSchoolZone();

                        game.ResetWithoutGoingToMain();
                        game.SaveLoadManager.InitiateLoad();
                        game.CurrentChapter.StartingPortal = Bathroom.ToLastMap;
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Bathroom"];
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.LoadContent();
                        Chapter.effectsManager.RemoveToolTip();
                        Reset();

                    }
                }
            }
        }

        public void Reset()
        {
            fadeOut = new Cutscene(game, game.Camera);
            load = false;
            main = false;
            holdBlackScreenTimer = 0;
            wordsAlpha = 0f;
            exitingScreen = false;
            canContinue = false;
            UnloadContent();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (background != null)
            {
                s.Draw(background, backgroundRec, Color.White);
                s.Draw(words, new Rectangle(0, 286, words.Width, words.Height), Color.White * wordsAlpha);

                if (currentSelection == 0)
                    s.Draw(menuActive, new Rectangle(436, 581, menuActive.Width, menuActive.Height), Color.White);
                else
                {
                    if(canContinue)
                        s.Draw(continueActive, new Rectangle(436, 581, menuActive.Width, menuActive.Height), Color.White);
                    else
                        s.Draw(continueActive, new Rectangle(436, 581, menuActive.Width, menuActive.Height), Color.Gray * .5f);
                }

                s.Draw(Game1.Player.PlayerSheet, new Rectangle(389, (int)playerPosY, 530, 398), GetSourceRectangle(), Color.White);
                s.Draw(fog, new Rectangle(0, 333, fog.Width, fog.Height), Color.White);
                s.Draw(light, new Rectangle(319, 0, light.Width, light.Height), Color.White);

            }
            fadeOut.DrawFade(s, 0);

            if (fadeOut.State == 1)
            {
                s.Draw(Game1.whiteFilter, backgroundRec, Color.Black);
                s.Draw(Game1.loadingScreenText, new Rectangle(1280 - Game1.loadingScreenText.Width, (int)(Game1.aspectRatio * 1280) - Game1.loadingScreenText.Height, Game1.loadingScreenText.Width, Game1.loadingScreenText.Height), Color.White);
            }
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(1060 + (530 * deathFrame), 3184, 530, 398);
        }

    }
}
