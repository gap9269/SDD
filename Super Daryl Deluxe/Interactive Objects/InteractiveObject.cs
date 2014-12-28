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
    public class InteractiveObject
    {
        protected int frameState;
        protected Boolean finished = false;
        protected Texture2D sprite;
        protected Rectangle rec;
        protected int health;
        protected int maxHealth;
        protected Game1 game;
        protected Object drop;
        protected String enemyDrop;
        protected Boolean passable;
        protected StoryItem storyItem;
        protected Random ranX;
        protected Random ranY;
        protected int frameTimer = 5;
        protected Rectangle vitalRec;
        protected Boolean foreground = false;
        protected KeyboardState last;
        protected KeyboardState current;
        protected Boolean facingRight = true;

        public Boolean Foreground { get { return foreground; } set { foreground = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }
        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public Rectangle VitalRec { get { return vitalRec; } set { vitalRec = value; } }
        public Boolean Finished { get { return finished; } set { finished = value; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int State { get { return frameState; } set { frameState = value; } }

        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }

        public InteractiveObject(Game1 g, Boolean fore)
        {
            game = g;
            foreground = fore;
            ranX = new Random();
            ranY = new Random();
            vitalRec = new Rectangle();
        }

        public void NewPosition(int x, int y)
        {
            rec.X = x;
            rec.Y = y;
        }

        public virtual void TakeHit()
        {
            if(health > 0)
                 health--;
        }

        public virtual void Update()
        {
            if (!finished)
            {
                last = current;
                current = Keyboard.GetState();
            }
        }
        public virtual void Draw(SpriteBatch s) { }
        public virtual Rectangle GetSourceRec() { return new Rectangle(); }
    }
}
