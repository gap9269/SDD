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
    public class SmallHellHand : Projectile
    {
        int handNumber = 8;
        int timeBetweenHands = 20;
        int horizontalDistanceBetweenHands = 193;
        float backgroundHandSize = .93f;
        Boolean facingRight;

        List<Hand> hands;

        public struct Hand
        {
            public int frame;
            public int frameDelay;
            public Boolean active;
            public Rectangle rec;
        }
        public SmallHellHand(int x, int y, Vector2 vel, int dam, Vector2 kb, Boolean facingRight, int level)
            : base(x, y, -1, vel, 0, dam, kb, 2, 3, -1, ProjType.hellHandSmall, level)
        {
            hands = new List<Hand>();
            Hand hand1 = new Hand();
            hand1.active = true;
            hand1.frameDelay = 3;
            hand1.rec = new Rectangle(x, y, (int)(246 * .8f), (int)(245 * .8f));
            hands.Add(hand1);
            this.facingRight = facingRight;

            horizontalDistanceBetweenHands = 90;
            backgroundHandSize = .7f;
        }

        public override void Update()
        {
            for (int i = 0; i < hands.Count; i++)
            {
                Hand temp = hands[i];
                temp.frameDelay--;

                if (hands[i].frameDelay <= 0)
                {
                    temp.frame++;
                    temp.frameDelay = 3;

                    if (temp.frame > 14)
                        temp.active = false;

                    if (temp.frame == 2 && hands.Count != handNumber)
                    {
                        Hand newHand = new Hand();

                        if (hands.Count % 2 == 0)
                        {
                            if(facingRight)
                                newHand.rec = new Rectangle(hands[i].rec.X + horizontalDistanceBetweenHands, hands[i].rec.Y, (int)(246 * .8f), (int)(245 * .8f));
                            else
                                newHand.rec = new Rectangle(hands[i].rec.X - horizontalDistanceBetweenHands, hands[i].rec.Y, (int)(246 * .8f), (int)(245 * .8f));

                        }
                        else
                        {
                            if(facingRight)
                                newHand.rec = new Rectangle(hands[i].rec.X + horizontalDistanceBetweenHands, hands[i].rec.Y - 3, (int)(246 * backgroundHandSize), (int)(245 * backgroundHandSize));
                            else
                                newHand.rec = new Rectangle(hands[i].rec.X - horizontalDistanceBetweenHands, hands[i].rec.Y - 3, (int)(246 * backgroundHandSize), (int)(245 * backgroundHandSize));

                        }
                        newHand.active = true;
                        newHand.frameDelay = 5;
                        hands.Add(newHand);
                    }
                }

                hands[i] = temp;

                if (Game1.Player.CheckIfHit(hands[i].rec) && Game1.Player.InvincibleTime <= 0 && hands[i].frame > 4 && hands[i].frame < 9)
                {
                    Game1.Player.TakeDamage(damage, level);

                    if (hands[i].rec.Center.X < Game1.Player.VitalRec.Center.X)
                        Game1.Player.KnockPlayerBack(new Vector2(3, -3));
                    else
                        Game1.Player.KnockPlayerBack(new Vector2(-3, -3));

                    Game1.Player.HitPauseTimer = 1;
                    Game1.camera.ShakeCamera(3, 1);
                    MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(hands[i].rec, Game1.Player.VitalRec));
                }
            }

            if (hands.Count == handNumber && hands[handNumber - 1].active == false)
            {
                dead = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            for (int i = 0; i < hands.Count; i++)
            {
                if (i % 2 != 0)
                {
                    if (facingRight)
                        s.Draw(Game1.g.EnemySpriteSheets["Anubis Warrior"], hands[i].rec, new Rectangle(246 * hands[i].frame, 3564, 246, 245), Color.White * .6f, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    else
                        s.Draw(Game1.g.EnemySpriteSheets["Anubis Warrior"], hands[i].rec, new Rectangle(246 * hands[i].frame, 3564, 246, 245), Color.White * .6f, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }

            for (int i = 0; i < hands.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (facingRight)
                        s.Draw(Game1.g.EnemySpriteSheets["Anubis Warrior"], hands[i].rec, new Rectangle(246 * hands[i].frame, 3564, 246, 245), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    else
                        s.Draw(Game1.g.EnemySpriteSheets["Anubis Warrior"], hands[i].rec, new Rectangle(246 * hands[i].frame, 3564, 246, 245), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }
}
