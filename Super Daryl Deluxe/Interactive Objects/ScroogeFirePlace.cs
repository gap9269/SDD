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
    public class ScroogeFirePlace : InteractiveObject
    {
        public Dictionary<String, Texture2D> fire;

        int lumberPlacedCooldown = 20;

        public ScroogeFirePlace(Game1 g, int x, int y)
            :base(g, false)
        {
            rec = new Rectangle(x, y, 1400, 300);
            frameTimer = 10;
            health = 2;
        }

        public override void Update()
        {
            if (finished)
            {
                frameTimer--;

                if (frameTimer == 0)
                {
                    frameState++;
                    frameTimer = 5;

                    if (frameState == 4)
                    {
                        frameState = 0;
                    }
                }
            }
            else if(game.CurrentSideQuests.Contains(game.SideQuestManager.poolesBoy))
            {
                Rectangle fRec = new Rectangle(rec.Center.X + 5, rec.Center.Y - 150, 43, 65);

                if (Game1.Player.EnemyDrops.ContainsKey("Lumber") && health != 1)
                {
                    if (Math.Abs(Game1.Player.VitalRec.Center.X - rec.Center.X) < 200)
                    {

                        if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.AddForeroundFButton(fRec);

                        if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                        {
                            if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);

                            Game1.Player.RemoveDrops("Lumber", 1);
                            health = 1;
                        }

                    }
                    else
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                    }
                }
                else if (health == 1)
                {
                    if (lumberPlacedCooldown <= 0 && !finished)
                    {
                        if (Math.Abs(Game1.Player.VitalRec.Center.X - rec.Center.X) < 200)
                        {

                            if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                Chapter.effectsManager.AddForeroundFButton(fRec);

                            if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                            {
                                if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                    Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);

                                finished = true;
                                health = 0;
                            }

                        }
                        else
                        {
                            if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                        }
                    }
                    else
                    {
                        lumberPlacedCooldown--;

                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {

            if (health <= 1)
                s.Draw(EnemyDrop.allDrops["Lumber"].texture, new Vector2(rec.X + 693, rec.Y + 100), Color.White);

            if (finished)
                s.Draw(fire.ElementAt(frameState).Value, new Vector2(rec.X, rec.Y), Color.White);
        }
    }
}
