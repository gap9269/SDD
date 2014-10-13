using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Collectible
    {
        protected Texture2D textureOnMap;
        protected Texture2D icon;
        protected Rectangle rec;
        protected Boolean pickedUp;
        protected Boolean ableToPickUp = true; //This is so you can only make them appear after a certain objective is completed
        public String collecName;

        protected String description;

        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        
        public Boolean AbleToPickUp { get { return ableToPickUp; } set { ableToPickUp = value; } }
        public Boolean PickedUp { get { return pickedUp; } set { pickedUp = value; } }
        public Texture2D Icon { get { return icon; } }

        public Collectible(Texture2D mapTex, int x, int platY)
        {
            textureOnMap = mapTex;
        }
        
        /// <summary>
        /// In a chest or reward for quest
        /// </summary>
        /// <param name="mapTex"></param>
        public Collectible(Texture2D mapTex)
        {
            textureOnMap = mapTex;
            ableToPickUp = false;
        }

        public virtual void PickUpCollectible() { }

        public virtual void Update()
        {
            if (pickedUp == false && ableToPickUp)
            {
                if (Game1.Player.VitalRec.Intersects(rec))
                {
                    PickUpCollectible();
                    pickedUp = true;
                }
            }
        }

        public virtual void Draw(SpriteBatch s)
        {
            if (pickedUp == false && ableToPickUp)
            s.Draw(textureOnMap, rec, Color.White);
        }
    }
}
