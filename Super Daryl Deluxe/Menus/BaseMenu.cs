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
    public class BaseMenu
    {
        protected List<Button> buttons;
        protected Texture2D background;
        protected Rectangle backgroundRec = new Rectangle(0, 0, 1280, (int)(1280 * Game1.aspectRatio));
        protected KeyboardState previous;
        protected KeyboardState current;
        protected Game1 game;

        public BaseMenu(Texture2D tex, Game1 g)
        {
            background = tex;
            buttons = new List<Button>();
            game = g;
        }

        public virtual void Update()
        {
            previous = current;

            current = Keyboard.GetState();
        }

        public bool KeyPressed(Keys k)
        {

            if (current.IsKeyUp(k) && previous.IsKeyDown(k))
            {
                return true;
            }

            return false;
        }

        public virtual void Draw(SpriteBatch s)
        {

        }
    }
}
