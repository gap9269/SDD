using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    public static class MouseManager
    {
        public static ButtonState previous;
        public static ButtonState current;

        public static ButtonState rightPrevious;
        public static ButtonState rightCurrent;
        public static MouseState mouse;
        public static void Update()
        {
            mouse = Mouse.GetState();

            //--Update the mouse states, so last state is set to "previous"
            rightPrevious = rightCurrent;
            previous = current;
            //--If the mouse is right clicking, make "current" equal to pressed.
            //--If not, make it released
            if (mouse.RightButton == ButtonState.Pressed)
                rightCurrent = ButtonState.Pressed;
            else
                rightCurrent = ButtonState.Released;

            if (mouse.LeftButton == ButtonState.Pressed)
                current = ButtonState.Pressed;
            else
                current = ButtonState.Released;
        }
    }
}
