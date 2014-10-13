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
    public class TubaGhost : Enemy
    {
        int attackLoopNum;

        public TubaGhost(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 110;
            maxHealth = 110;
            level = 7;
            experienceGiven = 35;
            rec = new Rectangle((int)position.X, (int)position.Y, 300, 246);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 10;
            vitalRec = new Rectangle(rec.X, rec.Y, 100, 100);
            maxHealthDrop = 10;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(0, 492, 300, 246);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        return new Rectangle(300 * moveFrame, 0, 300, 246);
                    case EnemyState.moving:
                        return new Rectangle(300 * moveFrame, 0, 300, 246);
                    case EnemyState.attacking:
                        return new Rectangle(300 * attackFrame, 246, 300, 246);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);
            if (!respawning)
            {
                if (hostile)
                    attackCooldown--;
                Move(mapwidth);
                CheckWalkCollisions(15, new Vector2(10, -5));

            }

                if (facingRight)
                    vitalRec.X = rec.X + 70;
                else
                    vitalRec.X = rec.X + 140;
                vitalRec.Y = rec.Y + 50;
            
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            //--If the enemy hasn't been hit, walk randomly. If he is hostile but far away from the player, ignore him
            if (hostile == false || distanceFromPlayer > 1600)
            {
                #region Random movement, not hostile
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
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

                        if (moveFrame > 3)
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
            else if (hostile && distanceFromPlayer < 1600)
            {
                #region If the player is too far away, move closer
                //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
                //--knockback or attacking
                if ((distanceFromPlayer > 230 || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
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

                        Attack(25, kb);
                    }
                }
                #endregion
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
                frameDelay = 15;
                if (facingRight)
                {
                    attackRec = new Rectangle(rec.X + rec.Width / 2, rec.Y, 170, 100);
                }
                else
                {
                    attackRec = new Rectangle(rec.X - 20, rec.Y, 170, 100);
                }
                RangedAttackRecs.Add(attackRec);
            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 5;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 0)
            {
                if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 3)
            {
                attackFrame = 1;
                attackLoopNum++;
            }
            if (attackLoopNum == 2)
            {
                attackLoopNum = 0;
                attackFrame = 0;
                
                attackCooldown = moveNum.Next(60, 180);
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;
            

            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackLoopNum = 0;
                attackFrame = 0;
                attackCooldown = moveNum.Next(60, 180);
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }
        }
        public override void StopAttack()
        {
            base.StopAttack();

            attackLoopNum = 0;
            attackFrame = 0;
            attackCooldown = moveNum.Next(60, 180);
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

           // s.Draw(Game1.whiteFilter, vitalRec, Color.Black);
           // s.Draw(Game1.whiteFilter, attackRec, Color.Red);
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

            if (dropType < 3)
            {
                if (dropType == 0)
                    currentMap.Drops.Add(new EnemyDrop("Ruby (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                else if (dropType == 1)
                    currentMap.Drops.Add(new EnemyDrop("Emerald (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                else
                    currentMap.Drops.Add(new EnemyDrop("Sapphire (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));

            }
            else if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Ectoplasm", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                currentMap.Drops.Add(new EnemyDrop("Unfinished Sheet Music", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
