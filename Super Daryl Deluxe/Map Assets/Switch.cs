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
    public class Switch
    {
        protected Rectangle rec;
        protected Texture2D texture;
        protected Boolean active = false;
        protected int timeActive;
        protected int maxTimeActive;
        protected Boolean timedSwitch = false;

        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public Boolean Active { get { return active; } set { active = value; } }
        public Boolean TimedSwitch { get { return timedSwitch; } set { timedSwitch = value; } }
        public int TimeActive { get { return timeActive; } }

        public Switch(Texture2D tex, Rectangle r)
        {
            rec = r;
            texture = tex;
        }

        public Switch(Texture2D tex, Rectangle r, int ac)
        {
            maxTimeActive = ac;
            rec = r;
            texture = tex;
            timedSwitch = true;
        }

        public virtual Rectangle GetSourceRectangle()
        {
            return new Rectangle(0, 0, 0, 0);
        }

        public virtual void Update()
        {
            if (timedSwitch && active)
            {
                timeActive++;

                if (timeActive >= maxTimeActive)
                {
                    active = false;
                    timeActive = 0;
                }
            }
        }

        /// <summary>
        /// Changes the switch status from active to nonactive, or vice versa
        /// </summary>
        public virtual void UseSwitch()
        {
            if (active == false)
                active = true;
            else
                active = false;
        }

        public virtual void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, GetSourceRectangle(), Color.White);
        }
    }
}
