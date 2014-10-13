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
    public class Cursor
    {
        Texture2D cursorTexture;
        Rectangle cursorRec;
        Vector2 lastPos;
        Vector2 currentPos;

        public static Rectangle cursorOnMapRec; //For map editting

        int idleTime = 0;

        public static Cursor last;

        public static int cursorWidth = 30;
        public static int cursorHeight = 30;

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
            MouseState mouse = Mouse.GetState();
            lastPos = currentPos;

            if (!Game1.gamePadConnected)
            {
                cursorRec.X = (int)(mouse.X / Camera.cursorScale);
                cursorRec.Y = (int)(mouse.Y / Camera.cursorScale);
            }
            else
            {
                if (MyGamePad.currentState.ThumbSticks.Right.X != 0 || MyGamePad.currentState.ThumbSticks.Right.Y != 0)
                {
                    if (cursorRec.X < 0)
                        cursorRec.X = 0;
                    else if (cursorRec.X > Game1.screenWidth)
                        cursorRec.X = Game1.screenWidth - 1;

                    if (cursorRec.Y < 0)
                        cursorRec.Y = 0;
                    else if (cursorRec.Y > Game1.screenHeight)
                        cursorRec.Y = Game1.screenHeight - 1;

                    if(cursorRec.X + (int)(MyGamePad.currentState.ThumbSticks.Right.X * 10) > 0 && cursorRec.X + (int)(MyGamePad.currentState.ThumbSticks.Right.X * 10) < Game1.screenWidth)
                        cursorRec.X += (int)(MyGamePad.currentState.ThumbSticks.Right.X * 10);

                    if (cursorRec.Y - (int)(MyGamePad.currentState.ThumbSticks.Right.Y * 10) > 0 && cursorRec.Y - (int)(MyGamePad.currentState.ThumbSticks.Right.Y * 10) < Game1.screenHeight)
                        cursorRec.Y -= (int)(MyGamePad.currentState.ThumbSticks.Right.Y * 10);
                }
            }

            currentPos = new Vector2(cursorRec.X, cursorRec.Y);

            if (lastPos == currentPos)
            {
                if(idleTime < 141)
                    idleTime++;
            }
            else
                idleTime = 0;
        }

        public void Draw(SpriteBatch s)
        {
            if(idleTime < 140)
                s.Draw(cursorTexture, cursorRec, Color.White);
        }
    }
}
