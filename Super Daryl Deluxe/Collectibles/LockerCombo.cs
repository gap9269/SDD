using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class LockerCombo : Collectible
    {
        Game1 game;
        public String name, combo;
        float rayRotation;
        float floatCycle;

        public LockerCombo(int x, int platY, String n, Game1 g)
            : base
                (Game1.storyItems["Locker Combo"], x, platY)
        {
            game = g;
            name = n;

            rec = new Rectangle(x, platY - (44), 100, 100);
            collecName = "Locker Combo";
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

            Chapter.effectsManager.AddFoundItem("a Locker Combo", Game1.storyItemIcons["Locker Combo"]);

            combo = GenerateLockerCombinations.combinations[name + "'s Locker"];

            combo = combo.Remove(0, 1);
            combo = combo.Remove(1, 1);
            combo = combo.Remove(2, 1);
            combo = combo.Insert(1, "-");
            combo = combo.Insert(3, "-");

            game.Notebook.ComboPage.AddCombo(name, combo);
            Sound.PlaySoundInstance(Sound.SoundNames.object_pickup_combo);

        }

        public override void Draw(SpriteBatch s)
        {
            if (pickedUp == false && ableToPickUp)
            {
                s.Draw(Game1.textbookRay, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, Game1.textbookRay.Width, Game1.textbookRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(Game1.textbookRay.Width / 2, Game1.textbookRay.Height / 2), SpriteEffects.None, 0f);
                s.Draw(textureOnMap, rec, Color.White);
            }
        }

    }
}