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
    public class Cursor
    {
        Texture2D cursorTexture;
         Rectangle cursorRec;
        public static Vector2 lastPos;
        public static Vector2 currentPos;
        public static int hoverTimer, clickTimer; //These are set to 1 when you hover or click a button

        public static Rectangle cursorOnMapRec; //For map editting

        int idleTime = 0;
        public Boolean active;

        float analogSensitivity = 13f;

        public static Cursor last;

        public static int cursorWidth = 18;
        public static int cursorHeight = 18;

        public Rectangle CursorRec { get { return cursorRec; } }

        public Cursor(Texture2D tex)
        {
            last = this;
            cursorTexture = tex;
            cursorRec = new Rectangle(0, 0, cursorWidth, cursorHeight);
        }

        public void Update()
        {
            cursorOnMapRec = cursorRec;
            cursorOnMapRec.X += (int)Game1.camera.Center.X - 640;

            if (Game1.currentChapter != null && Game1.currentChapter.CurrentMap.yScroll)
                cursorOnMapRec.Y += (int)Game1.camera.Center.Y - 360;
            else
                cursorOnMapRec.Y += (int)Game1.camera.Center.Y;

            lastPos = currentPos;

            analogSensitivity = 20f;

            //--If the previous state was pressed, and it is now released, the user must have right clicked
            //--Return true
            if (((MouseManager.rightCurrent == ButtonState.Released && MouseManager.rightPrevious == ButtonState.Pressed)) || ((MouseManager.current == ButtonState.Released && MouseManager.previous == ButtonState.Pressed) || (MyGamePad.APressed() && Game1.g.CurrentChapter != null && Game1.g.CurrentChapter.state != Chapter.GameState.Game)))
            {
                clickTimer = 5;
                active = true;
                idleTime = 0;
            }

            if (!Game1.gamePadConnected)
            {
                cursorRec.X = (int)(MouseManager.mouse.X / Camera.cursorScale);
                cursorRec.Y = (int)(MouseManager.mouse.Y / Camera.cursorScale);
            }
            else
            {
                if (MyGamePad.currentState.ThumbSticks.Left.X != 0 || MyGamePad.currentState.ThumbSticks.Left.Y != 0)
                {
                    if (cursorRec.X < 0)
                        cursorRec.X = 0;
                    else if (cursorRec.X > Game1.screenWidth)
                        cursorRec.X = Game1.screenWidth - 1;

                    if (cursorRec.Y < 0)
                        cursorRec.Y = 0;
                    else if (cursorRec.Y > Game1.screenHeight)
                        cursorRec.Y = Game1.screenHeight - 1;

                    if (cursorRec.X + (int)(MyGamePad.currentState.ThumbSticks.Left.X * analogSensitivity) > 0 && cursorRec.X + (int)(MyGamePad.currentState.ThumbSticks.Left.X * analogSensitivity) < Game1.screenWidth)
                        cursorRec.X += (int)(MyGamePad.currentState.ThumbSticks.Left.X * analogSensitivity);

                    if (cursorRec.Y - (int)(MyGamePad.currentState.ThumbSticks.Left.Y * analogSensitivity) > 0 && cursorRec.Y - (int)(MyGamePad.currentState.ThumbSticks.Left.Y * analogSensitivity) < Game1.screenHeight)
                        cursorRec.Y -= (int)(MyGamePad.currentState.ThumbSticks.Left.Y * analogSensitivity);
                }
            }

            currentPos = new Vector2(cursorRec.X, cursorRec.Y);

            if (lastPos == currentPos)
            {
                if (idleTime < 141)
                    idleTime++;
            }
            else
            {
                active = true;
                idleTime = 0;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (idleTime < 140)
            {
                if(clickTimer > 0)
                    s.Draw(cursorTexture, new Rectangle(cursorRec.X - 30, cursorRec.Y - 9, 101, 93), new Rectangle(202, 0, 101, 93), Color.White);
                else if(hoverTimer > 0)
                    s.Draw(cursorTexture, new Rectangle(cursorRec.X - 30, cursorRec.Y - 9, 101, 93), new Rectangle(101, 0, 101, 93), Color.White);
                else
                    s.Draw(cursorTexture, new Rectangle(cursorRec.X - 30, cursorRec.Y - 9, 101, 93), new Rectangle(0, 0, 101, 93), Color.White);
            }
            else
                active = false;

            if(clickTimer > 0)
            clickTimer--;
            if(hoverTimer > 0)
            hoverTimer--;
        }
    }
}
