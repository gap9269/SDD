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
    public class KidCage
    {
        Rectangle rec;
        Texture2D spriteSheet;
        int timeToOpen;
        Boolean opened = false;
        Boolean finished = false;
        Boolean opening = false;
        Player player;
        KeyboardState last;
        KeyboardState current;
        Rectangle openBar;
        float openBarWidth;
        public Boolean showFButton = true;

        public Boolean Opening { get { return opening; } set { opening = value; } }
        public Boolean Opened { get { return opened; } set { opened = value; } }
        public Boolean Finished { get { return finished; } set { finished = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public Rectangle OpenBar { get { return openBar; } set { openBar = value; } }

        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }

        public KidCage(Texture2D sprite, int x, int platY, Player p)
        {
            spriteSheet = sprite;
            rec = new Rectangle(x, platY, 250, 237);
            timeToOpen = 180;
            player = p;
            openBar = new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 0, 20);
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            if (!opened)
            {
                if (last.IsKeyDown(Keys.F) && current.IsKeyDown(Keys.F) && nearPlayer() && player.playerState == Player.PlayerState.standing)
                {
                    openBar.X = rec.X + rec.Width / 2 - 50;
                    openBar.Y = rec.Y;
                    opening = true;
                    OpenChest();
                    openBarWidth += 100f / 60f;
                    openBar.Width = (int)openBarWidth;
                }
                else
                {
                    if (timeToOpen != 60 && !opened)
                        timeToOpen = 60;

                    openBarWidth = 0;
                    openBar.Width = 0;
                    opening = false;
                }
            }
            else if (opened && !finished)
            {
                opening = false;
                finished = true;
                Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X, rec.Y, rec.Width, rec.Width - 6), 2);
            }
        }

        public bool nearPlayer()
        {
            Point distanceFromPlayer = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

            
            if (distanceFromPlayer.X < 150 && distanceFromPlayer.Y < 100 && distanceFromPlayer.X > 75 && player.Rec.Center.X < rec.Center.X)
                return true;

            return false;
        }

        public void OpenChest()
        {
            if (timeToOpen > 0)
                timeToOpen--;
            if (timeToOpen <= 0)
                opened = true;
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(spriteSheet, rec, Color.White);

            Rectangle fRec = new Rectangle(rec.X + rec.Width / 2 - 43 / 2, rec.Y - 50, 43,
65);

            if (opening)
            {
                s.DrawString(Game1.font, "Opening...", new Vector2(rec.X + rec.Width / 2 - 50, rec.Y - 25), Color.Black);
                s.Draw(Game1.emptyBox, new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 100, 20), Color.DarkGray * .8f);
                s.Draw(Game1.emptyBox, openBar, Color.LightBlue);
                Console.WriteLine(openBar.ToString());

                if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                    Chapter.effectsManager.fButtonRecs.Remove(fRec);
            }

            else if (opened)
            {
                if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                    Chapter.effectsManager.fButtonRecs.Remove(fRec);
            }
            else
            {
                if (nearPlayer())
                {

                    if (showFButton)
                    {
                        if (!Chapter.effectsManager.fButtonRecs.Contains(fRec))
                            Chapter.effectsManager.AddFButton(fRec);
                    }
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                        Chapter.effectsManager.fButtonRecs.Remove(fRec);
                }
            }
        }

    }
}
