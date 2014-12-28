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
    public class BreakableObject : InteractiveObject
    {
        protected int healthDropAmount;
        protected float money;
        
        public BreakableObject(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, Object content, float mon, bool fore)
            : base(g, fore)
        {
            health = maxHealth = hlth;
            passable = pass;
            drop = content;
            sprite = s;
            money = mon;
           // rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }

        public BreakableObject(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, int hlthDrop, float mon, bool fore)
            : base(g, fore)
        {
            health = maxHealth = hlth;
            passable = pass;
            healthDropAmount = hlthDrop;
            sprite = s;
            money = mon;
            //rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
        }

        public BreakableObject(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, String content, float mon, bool fore)
            : base(g, fore)
        {
            health = maxHealth = hlth;
            passable = pass;
            sprite = s;
            money = mon;
            //rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
            enemyDrop = content;
        }

        public BreakableObject(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, StoryItem story, float mon, bool fore)
            : base(g, fore)
        {
            health = maxHealth = hlth;
            passable = pass;
            sprite = s;
        //rec = new Rectangle(x, y - sprite.Height, s.Width / 3, s.Height);
            storyItem = story;
            money = mon;
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (!finished)
            {
                #region If it isn't passable, stop the player from moving through it
                if (!passable)
                {
                    Rectangle top = new Rectangle(rec.X + 5, rec.Y, rec.Width - 5, 20);
                    Rectangle left = new Rectangle(rec.X, rec.Y + 5, 10, rec.Height - 3);
                    Rectangle right = new Rectangle(rec.X + rec.Width, rec.Y + 5, 10, rec.Height - 3);
                    Rectangle bottom = new Rectangle(rec.X + 5, rec.Y + rec.Height - 10, rec.Width - 5, 10);

                    Rectangle rightPlay = new Rectangle((int)Game1.Player.VitalRec.X + Game1.Player.VitalRec.Width, (int)Game1.Player.VitalRec.Y + 5, 15, Game1.Player.VitalRec.Height);
                    Rectangle leftPlay = new Rectangle((int)Game1.Player.VitalRec.X, (int)Game1.Player.VitalRec.Y + 5, 15, Game1.Player.VitalRec.Height);
                    Rectangle topPlay = new Rectangle((int)Game1.Player.VitalRec.X + 5, (int)Game1.Player.VitalRec.Y, Game1.Player.VitalRec.Width - 5, 10);
                    Rectangle botPlay = new Rectangle((int)Game1.Player.VitalRec.X, (int)Game1.Player.VitalRec.Y + Game1.Player.VitalRec.Height + 20, Game1.Player.VitalRec.Width, 20);


                    if (rightPlay.Intersects(left))
                    {
                        if (Game1.Player.playerState != Player.PlayerState.jumping)
                        {
                            Game1.Player.PositionX -= Game1.Player.MoveSpeed;
                        }
                        else
                        {
                            Game1.Player.PositionX -= Game1.Player.AirMoveSpeed;
                        }
                        Game1.Player.VelocityX = 0;
                    }

                    if (leftPlay.Intersects(right))
                    {
                        if (Game1.Player.playerState != Player.PlayerState.jumping)
                        {
                            Game1.Player.PositionX += Game1.Player.MoveSpeed;
                        }
                        else
                        {
                            Game1.Player.PositionX += Game1.Player.AirMoveSpeed;
                        }
                        Game1.Player.VelocityX = 0;
                    }
                }
                #endregion
            }
        }

        public void DropHealth()
        {
            for (int i = 0; i < healthDropAmount; i++)
            {
                Vector2 vel = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
                HealthDrop newHealth = new HealthDrop(vel, new Rectangle(rec.Center.X, rec.Center.Y, 0, 0), 1);

                game.CurrentChapter.CurrentMap.HealthDrops.Add(newHealth);
            }
        }

        public void DropMoney()
        {
            float increment = 0f;

            float moneyLeft = money;

            for (float i = 0; i < money; i += increment)
            {
                Vector2 vel;

                if (moneyLeft < .05)
                {
                    increment = .01f;
                    vel = new Vector2(ranX.Next(-10, 10), -ranY.Next(6, 20));
                }
                else if (moneyLeft < .25)
                {
                    increment = .05f;
                    vel = new Vector2(ranX.Next(-9, 9), -ranY.Next(5, 17));
                }
                else if (moneyLeft < 1.00)
                {
                    increment = .25f;
                    vel = new Vector2(ranX.Next(-8, 8), -ranY.Next(4, 15));
                }
                else
                {
                    increment = 1f;
                    vel = new Vector2(ranX.Next(-7, 7), -ranY.Next(3, 13));
                }

                moneyLeft -= increment;

                MoneyDrop newMoney = new MoneyDrop(vel, new Rectangle(rec.Center.X, rec.Center.Y, 0, 0), increment);

                game.CurrentChapter.CurrentMap.MoneyDrops.Add(newMoney);
            }
        }


        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!finished)
            {
                //s.Draw(Game1.whiteFilter, vitalRec, Color.Black);

                if(facingRight)
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);
                else
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }
        }
    }
}
