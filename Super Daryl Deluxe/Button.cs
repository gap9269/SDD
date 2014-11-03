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
    public class Button
    {
        // ATTRIBUTES \\

        ButtonState previous;
        ButtonState current;

        ButtonState rightPrevious;
        ButtonState rightCurrent;

        Rectangle buttonRec;
        Texture2D buttonTexture;

        int timeSinceClicked = 0;
        bool singleClick = false;

        public Rectangle ButtonRec { get { return buttonRec; } set { buttonRec = value; } }
        public Texture2D ButtonTexture { get { return buttonTexture; } set { buttonTexture = value; } }
        public int ButtonRecY { get { return buttonRec.Y; } set { buttonRec.Y = value; } }
        public int ButtonRecX { get { return buttonRec.X; } set { buttonRec.X = value; } }
        public int ButtonRecHeight { get { return buttonRec.Height; } set { buttonRec.Height = value; } }
        public int ButtonRecWidth { get { return buttonRec.Width; } set { buttonRec.Width = value; } }

        //  CONSTRUCTOR \\

        public Button(Texture2D texture, Rectangle rec)
        {
            buttonTexture = texture;
            buttonRec = rec;
        }

        public Button(Rectangle rec)
        {
            buttonRec = rec;
            buttonTexture = Game1.emptyBox;
        }

        //  METHODS \\

        /// <summary>
        /// Checks to see if the mouse is over the button or not
        /// </summary>
        /// <returns></returns>
        public bool IsOver()
        {
            if (Game1.g.IsActive)
            {
                MouseState mouse = Mouse.GetState();

                //--If the mouse is within the x and y boundaries of the button, return true
                if (Cursor.last.CursorRec.X > buttonRec.X && Cursor.last.CursorRec.X < buttonRec.X + buttonRec.Width)
                {
                    if (Cursor.last.CursorRec.Y > buttonRec.Y && Cursor.last.CursorRec.Y < buttonRec.Y + buttonRec.Height)
                    {
                        Cursor.hoverTimer = 1;
                        return true;
                    }
                }
            }
                return false;
        }

        public bool MouseDown()
        {
            if (Game1.g.IsActive)
            {
                MouseState mouse = Mouse.GetState();

                if (IsOver())
                {
                    if (mouse.LeftButton == ButtonState.Pressed || MyGamePad.currentState.Buttons.RightStick == ButtonState.Pressed)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //--Checks to see if the button has been right clicked or not
        public bool RightClicked()
        {
            if (Game1.g.IsActive)
            {
                MouseState mouse = Mouse.GetState();

                if (IsOver())
                {
                    //--Update the mouse states, so last state is set to "previous"
                    rightPrevious = rightCurrent;

                    //--If the mouse is right clicking, make "current" equal to pressed.
                    //--If not, make it released
                    if (mouse.RightButton == ButtonState.Pressed)
                        rightCurrent = ButtonState.Pressed;
                    else
                        rightCurrent = ButtonState.Released;


                    //--If the previous state was pressed, and it is now released, the user must have right clicked
                    //--Return true
                    if ((rightCurrent == ButtonState.Released && rightPrevious == ButtonState.Pressed))
                    {
                        Cursor.clickTimer = 5;
                        return true;
                    }
                }
            }

            return false;
        }

        //--Checks to see if the button has been clicked or not
        public bool Clicked()
        {
            if (Game1.g.IsActive)
            {
                MouseState mouse = Mouse.GetState();
                //--Update the mouse states, so last state is set to "previous"
                previous = current;
                if (IsOver())
                {


                    //--If the mouse is clicking, make "current" equal to pressed.
                    //--If not, make it released
                    if (mouse.LeftButton == ButtonState.Pressed)
                        current = ButtonState.Pressed;
                    else
                        current = ButtonState.Released;


                    //--If the previous state was pressed, and it is now released, the user must have clicked
                    //--Return true
                    if ((current == ButtonState.Released && previous == ButtonState.Pressed) || MyGamePad.RightAnalogPressedIn())
                    {
                        Cursor.clickTimer = 5;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DoubleClicked()
        {
            if (Game1.g.IsActive)
            {
                MouseState mouse = Mouse.GetState();

                if (singleClick)
                {
                    timeSinceClicked++;

                    if (timeSinceClicked >= 30)
                    {
                        timeSinceClicked = 0;
                        singleClick = false;
                    }
                }

                if (IsOver())
                {
                    //--Update the mouse states, so last state is set to "previous"
                    previous = current;

                    //--If the mouse is clicking, make "current" equal to pressed.
                    //--If not, make it released
                    if (mouse.LeftButton == ButtonState.Pressed)
                        current = ButtonState.Pressed;
                    else
                        current = ButtonState.Released;


                    //--If the previous state was pressed, and it is now released, the user must have clicked
                    //--Return true
                    if ((current == ButtonState.Released && previous == ButtonState.Pressed) || MyGamePad.RightAnalogPressedIn())
                    {
                        if (!singleClick)
                        {
                            singleClick = true;
                            timeSinceClicked = 0;
                        }
                        else
                        {
                            singleClick = false;
                            timeSinceClicked = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //--Draw the button
        public void Draw(SpriteBatch s)
        {
            s.Draw(buttonTexture, buttonRec, Color.White);
        }

        //--Draw the button with an alpha
        public void Draw(SpriteBatch s, float alpha)
        {
            s.Draw(buttonTexture, buttonRec, Color.White * alpha);
        }

    }
}
