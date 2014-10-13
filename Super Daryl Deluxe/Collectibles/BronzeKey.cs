using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class BronzeKey : Collectible
    {
        int textNum;
        float rayRotation;
        float floatCycle;

        public BronzeKey(int x, int platY)
            : base(Game1.storyItems["Bronze Key"], x, platY)
        {
            collecName = "Bronze Key";
            description = "A bronze room key. Opens bronze locks.";

            icon = Game1.storyItemIcons[collecName];
            rec = new Rectangle(x, platY, 114, 53);
            rec.Y -= rec.Height; //Make it sit on the platform Y that you passed in
        }

        public BronzeKey()
            : base(Game1.storyItems["Bronze Key"])
        {
            collecName = "Bronze Key";
            description = "A bronze room key. Opens bronze locks.";

            rec = new Rectangle(0, 0, 0, 0);
            icon = Game1.storyItemIcons[collecName];
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

            Game1.Player.BronzeKeys++;
            Chapter.effectsManager.AddFoundItem("a Bronze Key", Game1.storyItemIcons["Bronze Key"]);
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

