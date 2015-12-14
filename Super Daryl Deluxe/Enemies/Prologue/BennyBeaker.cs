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
    public class BennyBeaker : Enemy
    {

        int standState = 0;
        Random standRand;

        public BennyBeaker(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 45;
            maxHealth = 45;//45 is the real value
            level = 2;
            experienceGiven = 5;
            rec = new Rectangle((int)position.X, (int)position.Y, 300, 200);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 3;
            vitalRec = new Rectangle(rec.X, rec.Y, 125, 125);
            maxHealthDrop = 5;
            moneyToDrop = .05f;
            distanceFromFeetToBottomOfRectangle = 7;
            standRand = new Random();
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(0, 1023, 510, 341);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (standState == 0)
                            return new Rectangle(510 * moveFrame, 341, 510, 341);
                        else
                            return new Rectangle(510 * (moveFrame + 2), 341, 510, 341);
                    case EnemyState.moving:
                        return new Rectangle(510 * moveFrame, 0, 510, 341);
                    case EnemyState.attacking:
                        return new Rectangle(510 * attackFrame, 682, 510, 341);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!respawning && !isStunned)
            {
                if (hitPauseTimer <= 0)
                {
                    if (hostile)
                        attackCooldown--;
                    Move(mapwidth);
                    CheckWalkCollisions(25, new Vector2(10, -5));
                }
            }
            vitalRec.X = rec.X + 90;
            vitalRec.Y = rec.Y + 50;
            deathRec = vitalRec;
        }

        public override void PlaySoundWhenHit()
        {
            //int soundType = moveNum.Next(2);

            //if (soundType == 0)
            //    Sound.enemySoundEffects["BennyHit1"].CreateInstance().Play();
            //else
            //    Sound.enemySoundEffects["BennyHit2"].CreateInstance().Play();
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);
            if (isStunned == false)
            {
                //--Calculate the distance from the player
                float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

                //--If the enemy hasn't been hit, walk randomly. If he is hostile but far away from the player, ignore him
                if (hostile == false || distanceFromPlayer > 1200)
                {
                    #region Random movement, not hostile
                    if (currentlyInMoveState == false)
                    {
                        moveState = moveNum.Next(0, 3);
                        moveTimer = moveTime.Next(10, 200);

                        if (moveState == 0)
                        {
                            standState = standRand.Next(0, 2);
                        }
                    }

                    switch (moveState)
                    {
                        case 0:
                            enemyState = EnemyState.standing;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 12;
                            }

                            if (moveFrame > 1)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            break;

                        case 1:
                            facingRight = true;
                            enemyState = EnemyState.moving;
                            if (currentlyInMoveState == false)
                                moveFrame = 0;

                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;
                            }

                            if (moveFrame > 3)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (position.X <= mapWidth - 6)
                                position.X += enemySpeed;
                            break;

                        case 2:
                            facingRight = false;
                            enemyState = EnemyState.moving;
                            if (currentlyInMoveState == false)
                                moveFrame = 0;

                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 5;
                            }

                            if (moveFrame > 3)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (position.X >= 6)
                                position.X -= enemySpeed;
                            break;
                    }


                    if (moveTimer <= 0)
                        currentlyInMoveState = false;
                    #endregion
                }
                //--If it is hostile
                else if (hostile && distanceFromPlayer < 1200)
                {
                    #region If the player is too far away, move closer
                    //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
                    //--knockback or attacking
                    if ((distanceFromPlayer > 230 || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
                    {
                        if (distanceFromPlayer > 230)
                        {
                            //--If the player is to the left
                            if (player.VitalRec.Center.X < vitalRec.Center.X)
                            {
                                facingRight = false;
                                enemyState = EnemyState.moving;
                                if (currentlyInMoveState == false)
                                    moveFrame = 0;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 5;
                                }

                                if (moveFrame > 3)
                                    moveFrame = 0;

                                currentlyInMoveState = true;
                                if (position.X >= 6)
                                    position.X -= enemySpeed;
                            }
                            //Player to the right
                            else
                            {
                                facingRight = true;
                                enemyState = EnemyState.moving;
                                if (currentlyInMoveState == false)
                                    moveFrame = 0;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 5;
                                }

                                if (moveFrame > 3)
                                    moveFrame = 0;

                                currentlyInMoveState = true;
                                if (position.X <= mapWidth - 6)
                                    position.X += enemySpeed;
                            }
                        }
                        else
                        {
                            enemyState = EnemyState.standing;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 12;
                            }

                            if (moveFrame > 1)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                        }
                    }
                    #endregion

                    #region Attack once it is close enough
                    else
                    {
                        //--Only attack if off cooldown
                        if (attackCooldown <= 0)
                        {
                            Vector2 kb;
                            if (facingRight)
                                kb = new Vector2(10, -5);
                            else
                                kb = new Vector2(-10, -5);

                            Attack(30, kb);
                        }
                    }
                    #endregion
                }
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
            moveFrame = 0;

            //--Face the player if it isn't already. 
            //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
            //--The wrong way and autoattack in the wrong direction
            if (player.VitalRec.Center.X < vitalRec.Center.X)
                facingRight = false;
            else
                facingRight = true;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;
                frameDelay = 10;
                if (facingRight)
                {
                    attackRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y, 130, 50);
                }
                else
                {
                    attackRec = new Rectangle(vitalRec.X - 130, vitalRec.Y, 130, 50);
                }
                RangedAttackRecs.Add(attackRec);
            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 2;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 1)
            {
                if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage, level);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 4)
            {
                attackFrame = 0;
                attackCooldown = 80;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                standState = standRand.Next(0, 2);
            }

            currentlyInMoveState = true;

            
            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackFrame = 0;
                attackCooldown = 120;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                standState = standRand.Next(0, 2);
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = 120;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
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
                healthBoxRec.Y = vitalRec.Y - 50;
                healthBarRec.Y = vitalRec.Y - 48;

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

                if (facingRight)
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 7), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 4), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

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

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = 120;
                hostile = true;
            }
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 35)
            {
                currentMap.Drops.Add(new EnemyDrop("Broken Glass", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 45)
            {
                currentMap.Drops.Add(new EnemyDrop("Spinach", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
