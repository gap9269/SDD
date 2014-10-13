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
    public class MapHazard
    {
        protected int timeNotActive;
        protected int timeActive;
        protected int timer;
        protected Boolean active;
        protected Rectangle rec;
        protected Texture2D texture;
        protected Game1 game;
        protected int frameTimer;
        protected int damage;

        public int TimeActive { get { return timeActive; } set { timeActive = value; } }
        public int Timer { get { return timer; } set { timer = value; } }
        public Boolean Active { get { return active; } set { active = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }

        public MapHazard(int x, int y, Game1 g)
        {
            game = g;
        }

        public virtual void Update()
        {

        }

        public virtual void DamagePlayer()
        {
        }

        public virtual void Draw(SpriteBatch s)
        {
            
        }
    }
}
