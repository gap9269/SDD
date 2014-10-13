using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class Textbook : Collectible
    {
        int textNum;
        float rayRotation;
        float floatCycle;

        public Textbook(int x, int platY, int texNum):base
            (Game1.textbookTextures, x, platY)
        {
            rec = new Rectangle(x, platY - 94, 94, 90);
            textNum = texNum;
            collecName = "Textbook";
            description = "An old textbook. A hot commodity in Water \nFalls High School.";
        }


        public Textbook()
            : base(Game1.textbookTextures)
        {
            collecName = "Textbook";
            description = "An old textbook. A hot commodity in Water Falls High School.";
        }

        public Rectangle GetSourceRec()
        {
            switch (textNum)
            {
                case 0:
                    return new Rectangle(0, 0, 94, 90);
                case 1:
                    return new Rectangle(94, 0, 94, 90);
                case 2:
                    return new Rectangle(188, 0, 94, 90);
                case 3:
                    return new Rectangle(188 + 94, 0, 94, 90);
            }

            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (pickedUp == false && ableToPickUp)
            {
                rayRotation++;

                if (rayRotation == 360)
                    rayRotation = 0;

                #region FLOAT UP AND DOWN
                //--Once it hits the ground, make it float up and down
                else
                {
                    //--Every 20 frames it changes direction
                    //--It floats at 1 pixel per frame, every 2 frames
                    if (floatCycle < 50)
                    {
                        if (floatCycle % 5 == 0)
                            rec.Y -= 1; floatCycle++;

                    }
                    else
                    {
                        if (floatCycle % 5 == 0)
                            rec.Y += 1; floatCycle++;

                        if (floatCycle >= 100)
                        {
                            floatCycle = 0;
                        }
                    }
                }
                #endregion
            }
        }

        public override void PickUpCollectible()
        {
            base.PickUpCollectible();

            Game1.Player.Textbooks++;
            Chapter.effectsManager.AddFoundItem("a Textbook", Game1.equipmentTextures["Textbook"]);
        }

        public override void Draw(SpriteBatch s)
        {
            if (pickedUp == false && ableToPickUp)
            {
                s.Draw(Game1.textbookRay, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height/2, Game1.textbookRay.Width, Game1.textbookRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(Game1.textbookRay.Width / 2, Game1.textbookRay.Height / 2), SpriteEffects.None, 0f);
                s.Draw(textureOnMap, rec, GetSourceRec(), Color.White);
            }
        }

    }
}
