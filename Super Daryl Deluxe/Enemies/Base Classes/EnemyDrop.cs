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
    public class CraftingItem : EnemyDrop
    {
        int successRate;

        public int SuccessRate { get { return successRate; } set { successRate = value; } }

        public CraftingItem(String drop, Rectangle r)
            : base(drop, r)
        {
            successRate = allDrops[drop].successRate;
        }
    }

    public class Catalyst : EnemyDrop
    {
        int successRate;

        public int SuccessRate { get { return successRate; } set { successRate = value; } }

        public Catalyst(String drop, Rectangle r)
            : base(drop, r)
        {
            successRate = allDrops[drop].successRate;
        }
    }

    public class EnemyDrop
    {
        protected Rectangle rec;
        protected Texture2D tex;

        StoryItem storyItem;
        Equipment equip;
        protected bool colliding = false;
        protected int floatCycle;
        protected bool pickedUp = false;
        int timeOnScreen;
        protected float pickedUpTimer = 1;
        protected List<Platform> platforms;
        String name;
        protected Vector2 velocity;
        protected static Random ranX, ranY;


        public static Dictionary<String, DropType> allDrops = new Dictionary<string, DropType>();


        public StoryItem StoryItem { get { return storyItem; } set { storyItem = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public Equipment Equip { get { return equip; } }
        public bool PickedUp { get { return pickedUp; } set { pickedUp = value; } }
        public float PickedUpTimer { get { return pickedUpTimer; } set { pickedUpTimer = value; } }
        public string Name { get { return name; } }

        //--If they drop an etc item
        public EnemyDrop(String drop, Rectangle r)
        {
            //dropTextures = new Dictionary<string, Texture2D>();

            if(!(this is EnemyBio))
                tex = allDrops[drop].texture;
            rec = r;
            name = drop;

            ranX = new Random();
            ranY = new Random();
            velocity = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
        }

        //--If they drop equipment
        public EnemyDrop(Equipment eq, Rectangle r)
        {
            //dropTextures = new Dictionary<string, Texture2D>();

            rec = r;
            tex = eq.Icon;
            equip = eq;
            ranX = new Random();
            ranY = new Random();
            velocity = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
        }

        //--If they drop a story item
        public EnemyDrop(StoryItem stryItem, Rectangle r)
        {
            //dropTextures = new Dictionary<string, Texture2D>();

            rec = r;
            tex = stryItem.Icon;
            storyItem = stryItem;
            ranX = new Random();
            ranY = new Random();
            velocity = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
            ranX = new Random();
            ranY = new Random();
            velocity = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
        }

        
        public virtual void Update(List<Platform> plats)
        {
            platforms = plats;

            //if(storyItem == null)
                //timeOnScreen++;

            //--If it has been picked up, decrease the alpha by .05 every second
            if (pickedUp == true)
            {
                pickedUpTimer -= .05f;
            }

            #region FALL
            //--Drop the item until it hits the ground
            if (colliding == false)
            {
                velocity.Y += GameConstants.GRAVITY;
                if (velocity.Y > 25)
                    velocity.Y = 25;

                //--Check to see if it is colliding with a platform
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (rec.Intersects(platforms[i].Rec))
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
                }

                rec.X += (int)velocity.X;
                rec.Y += (int)velocity.Y;
            }
            #endregion

            #region FLOAT UP AND DOWN
                //--Once it hits the ground, make it float up and down
            else
            {
                //--Every 20 frames it changes direction
                //--It floats at 1 pixel per frame, every 2 frames
                if (floatCycle < 20)
                {
                    if (floatCycle % 2 == 0)
                        rec.Y -= 1; floatCycle++;

                }
                else
                {
                    if (floatCycle % 2 == 0)
                        rec.Y += 1; floatCycle++;

                    if (floatCycle >= 40)
                    {
                        floatCycle = 0;
                    }
                }
            }
            #endregion

            //--Check to see if it is colliding with a platform
            for (int i = 0; i < platforms.Count; i++)
            {
                if (rec.Intersects(platforms[i].Rec))
                {
                    colliding = true;
                    rec.Y = platforms[i].Rec.Y - rec.Height;
                }
            }

        }

        public virtual void Draw(SpriteBatch s)
        {
            if (!Game1.g.Prologue.PrologueBooleans["PickedUpDrop"])
            {
                Rectangle spaceRec = new Rectangle(rec.X + rec.Width / 2 - 96 / 2, rec.Y - 70, (int)(120 * .8f), (int)(52 * .8f));

                s.Draw(Game1.spaceInner, spaceRec , Color.White);
                s.Draw(Game1.spaceOuter, spaceRec, Color.White * .7f);
            }

            if(equip != null)
                s.Draw(Game1.equipmentTextures[equip.Name], rec, Color.White * pickedUpTimer);
            else if (storyItem != null)
            {
                s.Draw(tex, rec, Color.White * pickedUpTimer);
            }
            else
                s.Draw(tex, rec, Color.White * pickedUpTimer);
        }

        //--If it has been on screen for a cetain amount of frames, return true so it is removed
        public bool MaxTimeOnScreen()
        {
            if (timeOnScreen >= 1800)
            {
                return true;
            }

            return false;
        }
    }
}
