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
    class Bat : SteeringEnemy
    {
        Boolean dangerousWhileNotHostile = false;

        public Bat(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound, Boolean dangerousWhileNotHostile = false)
            : base(pos, type, g, ref play, cur, bound)
        {
            maxSpeed = 75;
            maxForce = 75;
            mass = .8f;

            wanderAng = 0;
            wanderRad = 2;
            wanderDist = 15;
            wanderMax = 20;

            health = 90;
            maxHealth = 90;
            level = 3;
            experienceGiven = 8;
            rec = new Rectangle((int)position.X, (int)position.Y, 214, 164);
            tolerance = 8;
            vitalRec = new Rectangle(rec.X + 40, rec.Y, 128, 100);
            maxHealthDrop = 5;
            moneyToDrop = .04f;
                
            this.dangerousWhileNotHostile = dangerousWhileNotHostile;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if(knockedBack || isStunned)
                return new Rectangle(0, 164 * 2, 214, 164);
            if (!hostile && !dangerousWhileNotHostile)
            {
                return new Rectangle(214 * moveFrame, 0, 214, 164);
            }
            else
                return new Rectangle(214 * moveFrame, 164, 214, 164);
        }

        public override void Update(int mapWidth)
        {
            base.Update(mapWidth);
            if (!respawning && !isStunned)
            {

                frameDelay--;

                if (frameDelay <= 0)
                {
                    frameDelay = 5;
                    moveFrame++;
                }

                if (moveFrame > 4)
                    moveFrame = 0;

                wanderRad = 2;
                wanderDist = 15;
                wanderMax = 20;

                if(hostile || dangerousWhileNotHostile)
                    CheckWalkCollisions(20, new Vector2(5, -5));
            }
                vitalRec.X = rec.X + 40;
                vitalRec.Y = rec.Y + 20;
                deathRec = vitalRec;
            
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                hostile = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy
            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 20;
                healthBarRec.Y = vitalRec.Y - 18;

                s.Draw(healthBack, healthBoxRec, Color.White);

                if (health > (maxHealth / 2))
                {
                    greenColor = 1;
                    redColor = (1f - ((float)health / (float)maxHealth));
                }
                else
                {
                    redColor = 1;
                    greenColor = (((float)health / ((float)maxHealth / 2f)));
                }


                s.Draw(healthFore, healthBarRec, new Color(redColor, greenColor, 0));
                s.Draw(healthFore, healthBarRec, Color.Gray * .4f);

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + " " + displayName).X;

                Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 5), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);

            }
            #endregion

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Red * .5f);

            //Draw the stars above his head when he's stunned
            if (isStunned)
            {
                //Stars
                starTimer--;

                if (starTimer <= 0)
                {
                    starFrame++;
                    starTimer = 15;

                    if (starFrame > 3)
                    {
                        starFrame = 0;
                    }
                }

                if (facingRight)
                    s.Draw(player.PlayerSheet, rec, new Rectangle(530 + (530 * starFrame), 3610, 530, 398), Color.White * alpha);
                else
                    s.Draw(player.PlayerSheet, rec, new Rectangle(530 + (530 * starFrame), 3610, 530, 398), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public override Vector2 calcSteeringForce()
        {
            Vector2 sf = new Vector2();

            if (!hostile)
            {
                sf += Wander() * 5;
                sf += (CheckBoundaries() * 5);
                sf += calcSeparation(50);

                //sf += Align();
            }
            else
            {
                sf += Seek(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y)) * 5;
                sf += calcSeparation(50);
                //sf += Seek(calcCohesion());
            }


            return sf;
            
        }

        public override Vector2 Seek(Vector2 targetPos)
        {
            Vector2 sf = new Vector2();
            Vector2 desiredVel = new Vector2();

            if(!hostile)
                desiredVel = targetPos - new Vector2(position.X, position.Y);
            else
                desiredVel = targetPos - new Vector2(vitalRec.Center.X, vitalRec.Center.Y);

            if (desiredVel != Vector2.Zero)
                desiredVel.Normalize();
           
            desiredVel *= maxSpeed;

            sf = desiredVel - velocity;

            return sf;
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 10)//28
            {
                currentMap.Drops.Add(new EnemyDrop(new GarlicNecklace(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                dropType = moveNum.Next(0, 101);
            }
            if (dropType < 30)//28
            {
                currentMap.Drops.Add(new EnemyDrop("Guano", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
