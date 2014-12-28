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
    public class HealthDrop : GameObject
    {
        Texture2D texture;
        int healthAmount;
        Boolean chasePlayer = false;
        List<Platform> platforms;
        int timeOnScreen;
        Boolean pickedUp;
        Boolean colliding = false;
        int timeAfterSeek = 0; //Starts ticking when the player gets close to it

        public Boolean PickedUp { get { return pickedUp; } }
        public Boolean ChasePlayer { get { return chasePlayer; } set { chasePlayer = value; } }
        public int HealthAmount { get { return healthAmount; } }
        public int TimeOnScreen { get { return timeOnScreen; } }

        public HealthDrop(Vector2 vel, Rectangle r, int amnt)
        {
            healthAmount = amnt;
            rec = r;
            velocity = vel;

            if (healthAmount == 1)
            {
                texture = Game1.healthDrop;
                rec.Width = 34;
                rec.Height = 33;
            }
            if (healthAmount == 10)
            {
                texture = Game1.healthDrop;
                rec.Width = 68;
                rec.Height = 66;
            }
            if (healthAmount == 25)
            {
                texture = Game1.healthDrop;
                rec.Width = 91;
                rec.Height = 88;
            }
        }

        public void Update(List<Platform> plats, Player player)
        {

            position.X = rec.X;
            position.Y = rec.Y;

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

                    if (rec.Intersects(left) || rec.Intersects(right))
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
                    player.Health += HealthAmount;
                    pickedUp = true;
                }

            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, Color.White);
        }
    }
}
