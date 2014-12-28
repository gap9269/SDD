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
    public class TrojanHorse : GameObject
    {
        public int health, maxHealth;
        int frame;
        int frameDelay = 5;
        Rectangle frec;
        Boolean facingRight = true;
        public Boolean hasLocker, drawFButton;

        public Rectangle lockerRec;

        Dictionary<String, Texture2D> textures;

        KeyboardState current, last;

        public TrojanHorse(int x, int y)
        {
            position = new Vector2(x, y);
            rec = new Rectangle(x, y, 638, 745);
            vitalRec = new Rectangle(rec.X, rec.Y, 638, 745);
            lockerRec = rec;
        }

        public void LoadContent(ContentManager content)
        {
            textures = ContentLoader.LoadContent(content, "Maps\\History\\TrojanHorse");
        }

        public override void Update()
        {
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;
            vitalRec = rec;


            if (hasLocker)
            {
                last = current;
                current = Keyboard.GetState();

                if (facingRight)
                    frec = new Rectangle((rec.X + rec.Width / 2 - 43 / 2), rec.Y + 265, 43, 65);
                else
                    frec = new Rectangle((rec.X + rec.Width / 2 - 43 / 2), rec.Y + 265, 43, 65);

                #region Draw the F Button if you are intersecting with him
                if (Game1.Player.VitalRec.Intersects(vitalRec) && !Game1.g.CurrentChapter.TalkingToNPC && Game1.g.CurrentChapter.state == Chapter.GameState.Game && Game1.g.CurrentChapter.BossFight == false)
                    drawFButton = true;
                else
                    drawFButton = false;

                if (drawFButton)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }

                #endregion

                //If you press F, go to your locker
                if (Game1.Player.VitalRec.Intersects(vitalRec) && current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F) && Game1.Player.LearnedSkills.Count > 0 /*&& game.CurrentChapter.BossFight == false*/)
                {
                    Game1.g.YourLocker.LoadContent();
                    Game1.g.CurrentChapter.state = Chapter.GameState.YourLocker;
                }
            }
        }

        public void Move(int speed)
        {

            if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                Chapter.effectsManager.fButtonRecs.Remove(frec);

            frameDelay--;

            if (frameDelay <= 0)
            {
                frameDelay = 5;
                frame++;

                if (frame > 18)
                    frame = 0;
            }

            position.X += speed;

            if (speed > 0)
                facingRight = true;
            else
                facingRight = false;
        }

        public override void Draw(SpriteBatch s)
        {

            if(facingRight)
                s.Draw(textures.ElementAt(frame).Value, rec, Color.White);
            else
                s.Draw(textures.ElementAt(frame).Value, rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}