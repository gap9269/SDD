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
    class Dandelion : StoryItem
    {

        public Dandelion(int x, int platY)
            : base(x, platY)
        {
            name = "Dandelion";
            pickUpName = "a Dandelion";
            description = "A small dandelion that grows outside of the school.";

            rec = new Rectangle(x, platY, 100, 105);
            frameDelay = 10;
            icon = Game1.storyItemIcons[name];
            texture = Game1.storyItems[name];
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(100 * moveFrame, 0, 100, 105);
        }
        public override void Draw(SpriteBatch s)
        {
            if (!inChest)
            {
                if (!pickedUp)
                {
                    if (facingRight)
                        s.Draw(texture, rec, GetSourceRec(), Color.White);
                    else
                        s.Draw(texture, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                }

                Rectangle frec = new Rectangle(rec.X + rec.Width / 2 - 43 / 2, rec.Y - 100, 43,
                    65);

                if (NearPlayer() && !pickedUp && showFButton)
                {

                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);

                }
                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }

        }

        public override bool NearPlayer()
        {
            if (Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(rec.Center.X, rec.Center.Y)) < 300)
                return true;

            return false;

        }

        public override void Update()
        {
            base.Update();

            frameDelay--;

            if (!NearPlayer())
            {

                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 10;
                }

                if (moveFrame > 3)
                {
                    moveFrame = 0;
                }
            }
            else
            {
                //--This is backwards because the spritesheet has them facing left.
                if (player.VitalRec.Center.X < rec.Center.X)
                    facingRight = true;
                else
                    facingRight = false;

                if (frameDelay == 0)
                {
                    if (moveFrame == 4)
                        moveFrame = 5;
                    else
                        moveFrame = 4;

                    frameDelay = 20;
                }
            }
        }
    }
}
