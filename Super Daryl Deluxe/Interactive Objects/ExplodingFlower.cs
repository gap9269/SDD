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
    public class ExplodingFlower : InteractiveObject
    {
        Rectangle explosionRec, fRec;

        public enum FlowerState
        {
            idle,
            exploding,
            sprouting,
            scared,
            dead
        }
        public FlowerState flowerState;
        public int deathTime, maxDeathTime;
        List<Object> objectsHitThisTime;

        public ExplodingFlower(Game1 g, int x, int y, Boolean fore, int deathTime)
            : base(g, fore)
        {
            rec = new Rectangle(x, y, 175, 168);
            explosionRec = new Rectangle(x -315, y - 388, 800, 800);
            vitalRec = rec;
            maxDeathTime = deathTime;
            objectsHitThisTime = new List<Object>();
            canBeHit = false;
        }

        public override Rectangle GetSourceRec()
        {
            switch (flowerState)
            {
                case FlowerState.idle:
                    return new Rectangle(frameState * 175, 0, 175, 168);
                case FlowerState.exploding:
                    return new Rectangle(frameState * 175, 336, 175, 168);
                case FlowerState.sprouting:
                    return new Rectangle(frameState * 175, 168, 175, 168);
                case FlowerState.scared:
                    return new Rectangle(700 + (frameState * 175), 0, 175, 168);
            }

            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (Chapter.effectsManager.deathRecs.Contains(explosionRec) && Chapter.effectsManager.deathFrames[Chapter.effectsManager.deathRecs.IndexOf(explosionRec)] < 6)
            {
                if (Game1.Player.CheckIfHit(explosionRec) && Game1.Player.InvincibleTime <= 0)
                {
                    Game1.Player.TakeDamage(80, 18);

                    if (Game1.Player.VitalRecX < rec.Center.X)
                        Game1.Player.KnockPlayerBack(new Vector2(-25, -8));
                    else
                        Game1.Player.KnockPlayerBack(new Vector2(25, -8));

                    Game1.Player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);
                }

                foreach (Enemy e in game.CurrentChapter.CurrentMap.EnemiesInMap)
                {
                    if (explosionRec.Intersects(e.VitalRec) && e.CanBeHit && e.Respawning == false && !objectsHitThisTime.Contains(e))
                    {
                        MyGamePad.SetRumble(4, (float)((float)4 / 100f) * 10f);

                        Vector2 kbVec;
                        if (e.VitalRecX < rec.Center.X)
                            kbVec = new Vector2(-25, -8);
                        else
                            kbVec = new Vector2(25, -8);

                        if (e is VileMummy)
                        {
                            e.Health = 0;
                            e.moveFrame = 10;
                        }
                        else
                        {
                            e.TakeHit(300, kbVec, Rectangle.Intersect(explosionRec, e.VitalRec), AttackType.AttackTypes.none, AttackType.RangedOrMelee.none);
                            e.HitPauseTimer = 3;
                            objectsHitThisTime.Add(e);

                        }
                    }
                }

                for (int i = 0; i < game.CurrentChapter.CurrentMap.InteractiveObjects.Count; i++)
                {

                    InteractiveObject intObj = game.CurrentChapter.CurrentMap.InteractiveObjects[i];
                    if (game.CurrentChapter.CurrentMap.InteractiveObjects[i] == this)
                        continue;
                    //--If the skill's attack hits the enemy vitals
                    if (explosionRec.Intersects(intObj.VitalRec) && intObj.Finished == false && intObj.IsHidden == false && !objectsHitThisTime.Contains(intObj))
                    {

                        if (intObj is ExplodingFlower)
                        {
                            ExplodingFlower temp = intObj as ExplodingFlower;

                            if (temp.flowerState == FlowerState.idle || temp.flowerState == FlowerState.scared)
                            {
                                temp.flowerState = FlowerState.exploding;
                                temp.frameState = 8;
                                temp.frameTimer = 5;
                            }
                        }
                        else
                        {
                            intObj.TakeHit(3);
                        }

                        objectsHitThisTime.Add(intObj);

                    }
                }

                if (game.CurrentChapter.BossFight && game.CurrentChapter.CurrentBoss != null && explosionRec.Intersects(game.CurrentChapter.CurrentBoss.VitalRec) && game.CurrentChapter.CurrentBoss.CanBeHurt && !objectsHitThisTime.Contains(game.CurrentChapter.CurrentBoss))
                {
                    float kbX = 0;

                    //--Knock them back
                    if (rec.Center.X < game.CurrentChapter.CurrentBoss.VitalRec.X)
                        kbX = 25;
                    else if (rec.Center.X > game.CurrentChapter.CurrentBoss.VitalRec.X)
                    {
                        kbX = -25; //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                    }
                    objectsHitThisTime.Add(game.CurrentChapter.CurrentBoss);

                    game.CurrentChapter.CurrentBoss.TakeHit((int)150, new Vector2(kbX, -8), Rectangle.Intersect(explosionRec, game.CurrentChapter.CurrentBoss.VitalRec));
                    game.CurrentChapter.CurrentBoss.HitPauseTimer = 3;
                }
            }

            switch (flowerState)
            {
                case FlowerState.idle:
                    frameTimer--;

                    if (Vector2.Distance(new Vector2(Game1.Player.VitalRec.Center.X, Game1.Player.VitalRec.Center.Y), new Vector2(rec.Center.X, rec.Center.Y)) < 300)
                    {
                        flowerState = FlowerState.scared;
                        frameTimer = 20;
                        frameState = 0;
                    }

                    if (frameTimer <= 0)
                    {
                        frameState++;
                        frameTimer = 10;
                        if (frameState > 3)
                            frameState = 0;
                    }
                    break;
                case FlowerState.exploding:
                    frameTimer--;

                    if (frameTimer <= 0)
                    {
                        frameState++;
                        frameTimer = 7;
                        if (frameState > 9)
                        {
                            explosionRec = new Rectangle(rec.X - 315, rec.Y - 388, 800, 800);
                            Chapter.effectsManager.AddSmokePoofSpecifySize(explosionRec, 3);
                            objectsHitThisTime.Clear();
                            MyGamePad.SetRumble(4, (float)((float)4 / 100f) * 10f);
                            frameState = 0;
                            deathTime = maxDeathTime;
                            flowerState = FlowerState.dead;

                            game.Camera.ShakeCamera(13, 25);
                        }
                    }
                    break;
                case FlowerState.sprouting:
                    frameTimer--;

                    if (frameTimer <= 0)
                    {
                        frameState++;
                        frameTimer = 5;
                        if (frameState > 11)
                        {
                            frameState = 0;
                            flowerState = FlowerState.idle;
                        }
                    }
                    break;
                case FlowerState.scared:

                    Rectangle fRec = new Rectangle(rec.X + 46, rec.Y - 75, 43, 65);

                    if (Game1.Player.VitalRecX < rec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;

                    if (Vector2.Distance(new Vector2(Game1.Player.VitalRec.Center.X, Game1.Player.VitalRec.Center.Y), new Vector2(rec.Center.X, rec.Center.Y)) >= 300)
                    {
                        flowerState = FlowerState.idle;
                        frameTimer = 10;
                        frameState = 0;
                    }

                    if (Game1.Player.StoryItems.ContainsKey("Pyramid Water"))
                    {
                        if (NearPlayer())
                        {

                            if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                Chapter.effectsManager.AddForeroundFButton(fRec);

                            if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                            {
                                if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                    Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);

                                frameState = 0;
                                frameTimer = 10;
                                flowerState = FlowerState.exploding;
                            }

                        }
                        else
                        {
                            if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                                Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                        }
                    }
                    frameTimer--;

                    if (frameTimer <= 0)
                    {
                        frameState++;
                        frameTimer = 20;

                        if (frameState > 1)
                            frameState = 0;
                    }
                    break;
                case FlowerState.dead:

                    deathTime--;

                    if (deathTime <= 0)
                    {
                        flowerState = FlowerState.sprouting;
                    }
                    break;
            }
            
        }

        public bool NearPlayer()
        {
            if (Vector2.Distance(new Vector2(Game1.Player.VitalRec.Center.X, Game1.Player.VitalRec.Center.Y), new Vector2(rec.Center.X, rec.Center.Y)) < 150)
                return true;

            return false;

        }

        public override void Draw(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(Game1.interactiveObjects["ExplodingFlower"], rec, GetSourceRec(), Color.White);
            else
                s.Draw(Game1.interactiveObjects["ExplodingFlower"], rec, GetSourceRec(), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

        }
    }
}