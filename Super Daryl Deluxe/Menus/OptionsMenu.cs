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
    public class OptionsMenu
    {
        List<Button> buttons;
        Texture2D background;
        Rectangle backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));
        KeyboardState last;
        KeyboardState current;
        Game1 game;
        Button yesFull, noFull, low, med, borderless, mainMenu;

        Video flashbackAndOpening;
        VideoPlayer videoPlayer;
        Boolean playingVideo = false;

        //We need two arrows for resolution, a button for full screen or not, button for borderless or not
        //volumes for sound and music

        public OptionsMenu(Texture2D tex, Game1 g)
        {
            background = tex;
            buttons = new List<Button>();
            game = g;
            yesFull = new Button(Game1.emptyBox, new Rectangle(420, 210, 80, 40));
            noFull = new Button(Game1.emptyBox, new Rectangle(420, 305, 80, 40));

            low = new Button(Game1.emptyBox, new Rectangle(940, 205, 140, 40));
            med = new Button(Game1.emptyBox, new Rectangle(940, 300, 140, 40));
            mainMenu = new Button(new Rectangle(575, 10, 150, 28));

            borderless = new Button(Game1.emptyBox, new Rectangle(940, 395, 140, 40));

            UpdateResolution();

            videoPlayer = new VideoPlayer();
            videoPlayer.Volume = .15f;

            flashbackAndOpening = game.Content.Load<Video>(@"Demo Stuff\GDC Demo Reel - No Resampling");
        }

        public void UpdateResolution()
        {
            yesFull.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .3) - 6;//210;
            noFull.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .45) - 19;//305;

            low.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .3) -11;//205
            med.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4) + 12;//300
            borderless.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4) + 107;//395
        }

         public void Update()
         {
             last = current;

             current = Keyboard.GetState();

             if (playingVideo == false && ((current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back)) || (current.IsKeyUp(Keys.Escape) && last.IsKeyDown(Keys.Escape)) || MyGamePad.BPressed() || MyGamePad.StartPressed()))
             {
                 if (game.chapterState != Game1.ChapterState.mainMenu)
                 {
                     MyGamePad.ResetStates();
                     Sound.ResumeAllSoundEffects();
                     game.CurrentChapter.state = Chapter.GameState.Game;
                 }
             }

             //if ((current.IsKeyUp(Keys.Enter) && last.IsKeyDown(Keys.Enter)))
             //{
             //    if (playingVideo == false)
             //    {
             //        videoPlayer.Volume = .15f;
             //        videoPlayer.Play(flashbackAndOpening);
             //        playingVideo = true;
             //    }
             //    else
             //    {
             //        playingVideo = false;
             //        videoPlayer.Stop();
             //        videoPlayer.Play(flashbackAndOpening);
             //        videoPlayer.Stop();
             //    }
             //}

             if (mainMenu.Clicked())
             {
                 game.ResetGameAndGoToMain();
             }


             if ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)))
                 game.MakeFull();
             if ((current.IsKeyUp(Keys.G) && last.IsKeyDown(Keys.G)))
                 game.MakeNotFull();
             if ((current.IsKeyUp(Keys.B) && last.IsKeyDown(Keys.B)))
                 game.ToggleBorderless();

             //if (yesFull.Clicked())
             //    game.MakeFull();

             //if (noFull.Clicked())
             //    game.MakeNotFull();

             //if (!game.IsFullScreen)
             //{
             //    if (low.Clicked())
             //    {
             //        game.ChangeCurrentResolution(-1);
             //    }

             //    if (med.Clicked())
             //    {
             //        game.ChangeCurrentResolution(1);
             //    }

             //    if (borderless.Clicked())
             //    {
             //        game.ToggleBorderless();
             //    }
             //}
         }

         public virtual void Draw(SpriteBatch s)
         {
             backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));
             s.Draw(background, backgroundRec, Color.White);
            // yesFull.Draw(s);
            // noFull.Draw(s);
            // low.Draw(s);
            // med.Draw(s);
            // borderless.Draw(s);

             if(!mainMenu.IsOver())
                s.Draw(Game1.whiteFilter, mainMenu.ButtonRec, Color.White);
             else
                s.Draw(Game1.whiteFilter, mainMenu.ButtonRec, Color.Gray);
             s.DrawString(Game1.font, "Exit to main menu", new Vector2(580, 10), Color.Black);

             if (videoPlayer != null && playingVideo)
             {

                 Texture2D sceneTex = videoPlayer.GetTexture();

                 if (sceneTex != null)
                 {
                     s.End();

                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, game.Camera.StaticTransform);
                     s.Draw(sceneTex, new Rectangle(0, 0, 1280, 720), Color.White);
                     s.End();
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, game.Camera.StaticTransform);
                     sceneTex.Dispose();
                 }

                 if (videoPlayer.State == MediaState.Stopped)
                 {
                     videoPlayer.Stop();
                     videoPlayer.Play(flashbackAndOpening);
                 }
             }
         }
    }
}
