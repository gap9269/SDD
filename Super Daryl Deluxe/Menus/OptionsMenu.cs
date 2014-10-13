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
    public class OptionsMenu
    {
        List<Button> buttons;
        Texture2D background;
        Rectangle backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));
        KeyboardState last;
        KeyboardState current;
        Game1 game;
        Button yesFull, noFull, low, med, high;

        public OptionsMenu(Texture2D tex, Game1 g)
        {
            background = tex;
            buttons = new List<Button>();
            game = g;
            yesFull = new Button(Game1.emptyBox, new Rectangle(420, 210, 80, 40));
            noFull = new Button(Game1.emptyBox, new Rectangle(420, 305, 80, 40));

            low = new Button(Game1.emptyBox, new Rectangle(940, 205, 140, 40));
            med = new Button(Game1.emptyBox, new Rectangle(940, 300, 140, 40));
            high = new Button(Game1.emptyBox, new Rectangle(940, 390, 140, 40));

            UpdateResolution();
        }

        public void UpdateResolution()
        {
            yesFull.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .3) - 6;//210;
            noFull.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .45) - 19;//305;

            low.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .3) -11;//205
            med.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .4) + 12;//300
            high.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .55) - 6;//390
        }

         public void Update()
         {
             last = current;

             current = Keyboard.GetState();

             if((current.IsKeyUp(Keys.Back) && last.IsKeyDown(Keys.Back)) || MyGamePad.BPressed())
             {
                 if(game.chapterState != Game1.ChapterState.mainMenu)
                    game.CurrentChapter.state = Chapter.GameState.Game;
             }

             /*
             if (yesFull.Clicked())
                 game.MakeFull();

             if (noFull.Clicked())
                 game.MakeNotFull();

             if (low.Clicked())
             {
                 game.SetResolution(1024, 768);
             }

             if (med.Clicked())
             {
                 game.SetResolution(1280, 720);
             }

             if (high.Clicked())
             {
                 game.SetResolution(1440, 900);
             }*/
         }

         public virtual void Draw(SpriteBatch s)
         {
             backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));
             s.Draw(background, backgroundRec, Color.White);
         }
    }
}
