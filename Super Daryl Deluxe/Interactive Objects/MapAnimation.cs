using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class MapAnimation : BreakableObject
    {
        int frameDelay = 5;
        int frame;
        int maxFrame;

        public enum AnimationType
        {
            boxing, fencing, shoot, strangle,
            BurntBomblin, EgyptianCorpseFish, EgyptianCorpse, FrenchCorpse, MongolCorpse, RomanCorpseGoblin, RomanCorpse, EgyptianCorpseMongol
        }
        public AnimationType animationType;

        Boolean animated;

        public MapAnimation(Game1 g, int x, int y, Boolean facingRight, AnimationType type, Boolean foreground)
            : base(g, x, y, Game1.whiteFilter, true, 2, 0, 0, false)
        {
            rec = new Rectangle(x, y, 549, 466);
            animationType = type;
            this.foreground = foreground;

            switch (animationType)
            {
                case AnimationType.shoot:
                    rec.Height = 265;
                    rec.Width = 475;
                    maxFrame = 19;
                    animated = true;
                    break;
                case AnimationType.boxing:
                    rec.Height = 234;
                    rec.Width = 336;
                    animated = true;
                    maxFrame = 8;
                    break;
                case AnimationType.fencing:
                    rec.Height = 207;
                    rec.Width = 488;
                    maxFrame = 4;
                    animated = true;
                    break;
                case AnimationType.strangle:
                    rec.Height = 258;
                    rec.Width = 263;
                    maxFrame = 10;
                    animated = true;
                    break;
                case AnimationType.BurntBomblin:
                case AnimationType.EgyptianCorpse:
                case AnimationType.EgyptianCorpseFish:
                case AnimationType.FrenchCorpse:
                case AnimationType.MongolCorpse:
                case AnimationType.RomanCorpse:
                case AnimationType.RomanCorpseGoblin:
                case AnimationType.EgyptianCorpseMongol:
                    rec.Height = 315;
                    rec.Width = 247;
                    animated = false;
                    break;
            }
            this.facingRight = facingRight;
        }

        public override void Update()
        {
            if (animated)
            {
                frameDelay--;

                if (frameDelay == 0)
                {
                    frame++;
                    frameDelay = 5;

                    if (frame > maxFrame)
                        frame = 0;
                }
            }
            base.Update();
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden)
            {
                if (facingRight)
                {
                    switch (animationType)
                    {
                        case AnimationType.shoot:
                        case AnimationType.boxing:
                        case AnimationType.fencing:
                        case AnimationType.strangle:
                        case AnimationType.BurntBomblin:
                        case AnimationType.EgyptianCorpse:
                        case AnimationType.EgyptianCorpseFish:
                        case AnimationType.EgyptianCorpseMongol:
                        case AnimationType.FrenchCorpse:
                        case AnimationType.MongolCorpse:
                        case AnimationType.RomanCorpse:
                        case AnimationType.RomanCorpseGoblin:
                            if (animated)
                                s.Draw(StoneFortCentral.soldierAnimations[animationType.ToString() + frame.ToString()], rec, Color.White);
                            else
                                s.Draw(StoneFortCentral.soldierAnimations[animationType.ToString()], rec, Color.White);
                            break;
                    }
                }
                else
                {
                    switch (animationType)
                    {
                        case AnimationType.shoot:
                        case AnimationType.boxing:
                        case AnimationType.fencing:
                        case AnimationType.strangle:
                        case AnimationType.BurntBomblin:
                        case AnimationType.EgyptianCorpse:
                        case AnimationType.EgyptianCorpseFish:
                        case AnimationType.EgyptianCorpseMongol:
                        case AnimationType.FrenchCorpse:
                        case AnimationType.MongolCorpse:
                        case AnimationType.RomanCorpse:
                        case AnimationType.RomanCorpseGoblin:
                            if (animated)
                                s.Draw(StoneFortCentral.soldierAnimations[animationType.ToString() + frame.ToString()], rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            else
                                s.Draw(StoneFortCentral.soldierAnimations[animationType.ToString()], rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;
                    }
                }
            }
        }
    }
}
