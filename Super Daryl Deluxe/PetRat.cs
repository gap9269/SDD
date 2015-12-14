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
    class PetRat
    {
        Rectangle rec;
        Rectangle vitalRec;
        int moveFrame;
        int frameDelay = 5;
        Texture2D spriteSheet;
        Game1 game;
        int timesAnimationLooped = 0;
        int moveState = 0; //0 or 1, determines movement pattern
        Boolean facingRight = true;
        public static SoundEffect object_prologue_rat_die;
        enum State
        {
            blinking,
            standing,
            moving
        }
        State state;

        public PetRat(Game1 g)
        {
            game = g;
            spriteSheet = Game1.petRatSprite;
            rec = new Rectangle(4300, 590, 210, 67);
            vitalRec = new Rectangle(4300, 590, 50, 67);
        }

        public Rectangle GetSourceRec()
        {
            if (game.Prologue.PrologueBooleans["ratDead"] == true)
                return new Rectangle(0, 134, 210, 67);
            else
            {
                if (moveFrame < 5)
                {
                    return new Rectangle(210 * moveFrame, 0, 210, 67);
                }
                else
                {
                    return new Rectangle(210 * (moveFrame - 5), 67, 210, 67);
                }
            }
        }

        public void Update()
        {
            if (game.Prologue.PrologueBooleans["ratDead"] == false)
            {
                vitalRec.X = rec.X + 80;

                if (state == State.blinking)
                {
                    frameDelay--;

                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 1)
                    {
                        moveFrame = 0;
                        timesAnimationLooped++;
                    }

                    if (timesAnimationLooped == 3)
                    {
                        timesAnimationLooped = 0;
                        state = State.moving;
                        moveFrame = 1;
                    }
                }
                else if (state == State.moving)
                {

                    if (moveState == 0)
                    {
                        if (timesAnimationLooped == 0 || timesAnimationLooped == 2)
                        {
                            rec.X += 4;
                            facingRight = true;
                        }
                        else
                        {
                            rec.X -= 4;
                            facingRight = false;
                        }

                    }
                    else
                    {
                        if (timesAnimationLooped == 0 || timesAnimationLooped == 2)
                        {
                            rec.X -= 4;
                            facingRight = false;
                        }
                        else
                        {
                            rec.X += 4;
                            facingRight = true;
                        }
                    }
                    frameDelay--;

                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 3;
                    }

                    if (moveFrame > 9)
                    {
                        moveFrame = 1;
                        timesAnimationLooped++;
                    }

                    if (timesAnimationLooped == 3)
                    {
                        timesAnimationLooped = 0;
                        state = State.standing;
                        frameDelay = 120;

                        if (moveState == 0)
                            moveState = 1;
                        else
                            moveState = 0;
                    }
                }
                else
                {
                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        frameDelay = 5;
                        state = State.blinking;
                    }
                }

                if (Game1.Player.playerState == Player.PlayerState.running)
                {
                    if (Game1.Player.VitalRec.Intersects(vitalRec))
                    {
                        game.Prologue.PrologueBooleans["ratDead"] = true;
                        Sound.PlaySoundInstance(object_prologue_rat_die, Game1.GetFileName(()=>object_prologue_rat_die));
                    }
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (game.Prologue.PrologueBooleans["ratSpawned"] == true)
            {
                if (facingRight)
                    s.Draw(Game1.petRatSprite, rec, GetSourceRec(), Color.White);
                else
                    s.Draw(Game1.petRatSprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}
