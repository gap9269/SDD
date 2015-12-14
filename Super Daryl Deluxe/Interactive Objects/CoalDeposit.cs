using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class CoalDeposit : BreakableObject
    {
        float redDamageAlpha = 0;
        int dropAmount;
        public CoalDeposit(Game1 g, int x, int y, Texture2D s, int hlth, StoryItem story, int amount, Boolean fore, Boolean facingRight)
            : base(g, x, y, s, true, hlth, story, 0, fore)
        {
            rec = new Rectangle(x, y, 268, 225);
            vitalRec = new Rectangle(rec.X + 40, rec.Y, rec.Width - 80, rec.Height);
            this.facingRight = facingRight;
            dropAmount = amount;
        }

        public override Rectangle GetSourceRec()
        {
            if (frameState == 0)
                return new Rectangle(0, 0, 268, 225);
            else
                return new Rectangle(536, 0, 268, 225);
        }

        public override void Update()
        {
            if (!finished)
            {
                vitalRec.X = rec.X + 40;
                vitalRec.Y = rec.Y;

                if (redDamageAlpha > 0)
                    redDamageAlpha -= .04f;

                if (health <= 0 && finished == false)
                {
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.Center.X - 100, rec.Center.Y - 100, 200, 200), 2);
                    finished = true;

                    int soundNum = Game1.randomNumberGen.Next(1, 3);

                    switch (soundNum)
                    {
                        case 1:
                            Sound.PlaySoundInstance(Sound.SoundNames.object_coal_break_01);
                            break;
                        case 2:
                            Sound.PlaySoundInstance(Sound.SoundNames.object_coal_break_02);
                            break;
                    }

                    if (storyItem != null)
                    {
                        for (int i = 0; i < dropAmount; i++)
                        {
                            EnemyDrop tempDrop = new EnemyDrop(storyItem, new Rectangle(rec.Center.X, rec.Center.Y, 70, 70));
                            game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                        }
                    }
                }
            }
            else
            {
                redDamageAlpha = 0;
                frameState = 1;
            }
            base.Update();
        }

        public override void TakeHit(int damage = 1)
        {
            if (health > 0)
            {
                if (health > 0)
                {
                    int soundNum = Game1.randomNumberGen.Next(1, 4);

                    switch (soundNum)
                    {
                        case 1:
                            Sound.PlaySoundInstance(Sound.SoundNames.object_coal_damage_01);
                            break;
                        case 2:
                            Sound.PlaySoundInstance(Sound.SoundNames.object_coal_damage_02);
                            break;
                        case 3:
                            Sound.PlaySoundInstance(Sound.SoundNames.object_coal_damage_03);
                            break;
                    }
                }
                health-= damage;
            }
            redDamageAlpha = 1f;
        }

        public override void Draw(SpriteBatch s)
        {
            if (!isHidden)
            {
                base.Draw(s);

                if (facingRight)
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White);

                    if(!finished)
                        s.Draw(sprite, rec, new Rectangle(268, 0, 268, 225), Color.White * redDamageAlpha);
                }
                else
                {
                    s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    if (!finished)
                        s.Draw(sprite, rec, new Rectangle(268, 0, 268, 225), Color.White * redDamageAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

    }
}
