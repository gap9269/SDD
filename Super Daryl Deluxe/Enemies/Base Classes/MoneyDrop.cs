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
    public class MoneyDrop : GameObject
    {
        Texture2D texture;
        float moneyAmount;
        Boolean chasePlayer = false;
        List<Platform> platforms;
        int timeOnScreen;
        Boolean pickedUp;
        Boolean colliding = false;
        int timeAfterSeek = 0; //Starts ticking when the player gets close to it
        int frameDelay = 5;
        int currentFrame = 0;

        Random ranFrame;

        public Boolean PickedUp { get { return pickedUp; } }
        public Boolean ChasePlayer { get { return chasePlayer; } set { chasePlayer = value; } }
        public float MoneyAmount { get { return moneyAmount; } }
        public int TimeOnScreen { get { return timeOnScreen; } }

        public MoneyDrop(Vector2 vel, Rectangle r, float amnt)
        {

            ranFrame = new Random();
            moneyAmount = amnt;
            rec = r;
            velocity = vel;

            texture = Game1.moneySprite;

            currentFrame = ranFrame.Next(8);

            if (amnt == .01f)
            {
                rec.Width = 40;
                rec.Height = 40;
            }
            else if (amnt == .05f)
            {
                rec.Width = 50;
                rec.Height = 50;
            }
            else if (amnt == .25f)
            {
                rec.Width = 65;
                rec.Height = 65;
            }
            else if (amnt == 1f)
            {
                rec.Width = 75;
                rec.Height = 75;
            }
        }

        public Rectangle GetSourceRectangle()
        {
            if (moneyAmount == .01f)
            {
                return new Rectangle(40 * currentFrame, 0, 40, 40);
            }
            else if (moneyAmount == .05f)
            {
                return new Rectangle(50 * currentFrame, 40, 50, 50);
            }
            else if (moneyAmount == .25f)
            {
                return new Rectangle(65 * currentFrame, 90, 65, 65);
            }
            else if (moneyAmount == 1f)
            {
                return new Rectangle(75 * currentFrame, 155, 75, 75);
            }


            return new Rectangle();
        }

        public void Update(List<Platform> plats, Player player)
        {

            position.X = rec.X;
            position.Y = rec.Y;

            frameDelay--;

            if (frameDelay == 0)
            {
                currentFrame++;
                frameDelay = 5;
            }

            if (moneyAmount < .25)
            {
                if (currentFrame > 7)
                    currentFrame = 0;
            }
            else
            {
                if (currentFrame > 15)
                    currentFrame = 0;
            }

            //When it is dropping to the ground
            if (!chasePlayer)
            {

                platforms = plats;
                timeOnScreen++;

                rec.X += (int)velocity.X;
                rec.Y += (int)velocity.Y;

                velocity.Y += GameConstants.GRAVITY;

                //--Check to see if it is colliding with a platform
                for (int i = 0; i < platforms.Count; i++)
                {
                    Platform plat = platforms[i];

                    //Sides of the current plat it is checking
                    Rectangle top = new Rectangle(plat.Rec.X + 5, plat.Rec.Y, plat.Rec.Width - 5, 20);
                    Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                    Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                    Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                    if (rec.Intersects(top) || rec.Intersects(bottom))
                    {
                        velocity.Y = -velocity.Y / 2;
                        velocity.X = velocity.X / 2;


                        Vector2 temp = velocity;
                        if (Math.Abs(temp.X) < 1.5f)
                        {
                            velocity.X = 0;
                        }

                        if (Math.Abs(temp.Y) < 1.5f)
                        {
                            velocity.Y = 0;
                            colliding = true;
                        }
                    }

                    if(rec.Intersects(left) || rec.Intersects(right))
                    {
                        velocity.X = -velocity.X;
                    }
                }

                if (player.Rec.Intersects(rec) && colliding)
                {
                    chasePlayer = true;
                }
            }
            else
            {
                timeAfterSeek++;
                velocity += Seek(player);
                rec.X += (int)velocity.X;
                rec.Y += (int)velocity.Y;

                if (player.VitalRec.Intersects(rec) || timeAfterSeek > 150)
                {
                    player.AddMoneyJustPickedUp(moneyAmount);
                    player.Money += MoneyAmount;
                    pickedUp = true;

                    String name = "object_pickup_coin_0" + Game1.randomNumberGen.Next(1, 6);
                    Sound.PlaySoundInstance(Sound.permanentSoundEffects[name], name);

                }

            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRectangle(), Color.White);
        }
    }
}
