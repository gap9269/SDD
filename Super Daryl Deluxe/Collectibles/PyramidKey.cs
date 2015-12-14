using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class PyramidKey : Collectible
    {
        int textNum;
        float rayRotation;
        float floatCycle;

        public PyramidKey(int x, int platY)
            : base(Game1.storyItems["Pyramid Key"], x, platY)
        {
            collecName = "Pyramid Key";
            description = "An ancient key made of sandstone";

            icon = Game1.storyItemIcons[collecName];
            rec = new Rectangle(x, platY, 100, 75);
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

            Game1.Player.AddStoryItemWithoutPopup("Pyramid Key", 1);
            Chapter.effectsManager.AddFoundItem("a Pyramid Key", Game1.storyItemIcons["Pyramid Key"]);
        }

        public override void Draw(SpriteBatch s)
        {
            if (pickedUp == false && ableToPickUp)
            {
                s.Draw(Game1.textbookRay, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, (int)(Game1.textbookRay.Width * .8f), (int)(Game1.textbookRay.Height * .8f)), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(Game1.textbookRay.Width / 2, Game1.textbookRay.Height / 2), SpriteEffects.None, 0f);
                s.Draw(textureOnMap, rec, Color.White);
            }
        }
    }
}

