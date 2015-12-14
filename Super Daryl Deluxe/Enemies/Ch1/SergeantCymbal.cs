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
    public class SergeantCymbal : Enemy
    {

        int attackDamage = 50;
        int attackRange = 200;


        public SergeantCymbal(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 255;
            maxHealth = 255;
            level = 7;
            experienceGiven = 25;
            rec = new Rectangle((int)position.X, (int)position.Y, 401, 216);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 18;
            vitalRec = new Rectangle(rec.X, rec.Y, 165, 130);
            maxHealthDrop = 5;
            moneyToDrop = .15f;

            maxAttackCooldown = 100;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(2807, 0, 401, 216);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                    case EnemyState.moving:
                        if (moveFrame < 10)
                            return new Rectangle(401 * moveFrame, 216, 401, 216);
                        else
                            return new Rectangle(0, 432, 401, 216);
                    case EnemyState.attacking:
                        return new Rectangle(401 * attackFrame, 0, 401, 216);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);


            if (!respawning && !isStunned)
            {
                attackCooldown--;

                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }
            
            vitalRec.Y = rec.Y - 50;

            if (!facingRight)
                vitalRec.X = rec.X + 115;
            else
                vitalRec.X = rec.X + 120;

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

                #region If the player is too far away, move closer


                //Just walk around and be swaggy
                if (hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer())
                {
                    #region Random movement, not hostile
                    if (currentlyInMoveState == false)
                    {
                        moveState = moveNum.Next(0, 3);
                        moveTimer = moveTime.Next(10, 200);
                        moveFrame = 0;
                    }

                    switch (moveState)
                    {
                        case 0:
                            enemyState = EnemyState.standing;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;

                                frameDelay = 5;
                            }

                            if (moveFrame > 10)
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

                            if (moveFrame > 10)
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

                            if (moveFrame > 10)
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
                else if ((distanceFromPlayer > attackRange || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    if (distanceFromPlayer > attackRange)
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

                            if (moveFrame > 10)
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

                            if (moveFrame > 10)
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
                            frameDelay = 5;
                        }

                        if (moveFrame > 10)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                    }
                }
                #endregion

                #region Attack once it is close enough
                else if (enemyState != EnemyState.attacking)
                {
                    //--Only attack if off cooldown
                    if (attackCooldown <= 0)
                    {
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                            facingRight = false;
                        else
                            facingRight = true;

                        attackFrame = 0;
                        frameDelay = 10;

                        enemyState = EnemyState.attacking;

                        if (facingRight)
                        {
                            attackRec = new Rectangle(vitalRec.X + 35, vitalRec.Y - 20, 220, 160);
                        }
                        else
                        {
                            attackRec = new Rectangle(vitalRec.X - 95, vitalRec.Y - 20, 220, 160);
                        }
                    }
                }

                if (enemyState == EnemyState.attacking)
                {
                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(10, -5);
                    else
                        kb = new Vector2(-10, -5);

                    Attack(attackDamage, kb);
                }
                #endregion

            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);

            enemyState = EnemyState.attacking;

            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;

                frameDelay = 5;

                if (attackFrame == 10)
                    frameDelay = 5;
            
            }

            if (attackFrame == 4)
            {
                if ((player.CheckIfHit(attackRec) || player.CheckIfHit(attackRec)) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage, level);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);
                    MyGamePad.SetRumble(3, .4f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));

                    if (Game1.randomNumberGen.Next(0, 3) == 2)
                    {
                        player.StunDaryl(120);
                    }
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 6)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;

            
            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = maxAttackCooldown;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy
            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y - 100, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y - 100, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 35;
                healthBarRec.Y = vitalRec.Y - 33;

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 30), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 10), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, attackRec, Color.Black * .5f);

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

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = maxAttackCooldown;
                hostile = true;
            }
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Cymbal Polish", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }

    }
}
