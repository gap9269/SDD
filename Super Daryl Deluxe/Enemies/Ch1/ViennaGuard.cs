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
    public class ViennaGuard : Enemy
    {

        public ViennaGuard(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 1;
            maxHealth = 150;
            level = 8;
            experienceGiven = 43;
            rec = new Rectangle((int)position.X, (int)position.Y, 316, 255);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 10;
            vitalRec = new Rectangle(rec.X, rec.Y, 100, 150);
            maxHealthDrop = 10;
            hostile = true;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(316, 1020, 316, 255);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        return new Rectangle(0, 1020, 316, 255);
                    case EnemyState.moving:
                        if(moveFrame < 4)
                        return new Rectangle(316 * moveFrame, 0, 316, 255);
                        else if(moveFrame < 8)
                            return new Rectangle(316 * (moveFrame - 4), 255, 316, 255);
                        else
                            return new Rectangle(0, 510, 316, 255);
                    case EnemyState.attacking:
                        return new Rectangle(316 * attackFrame, 765, 316, 255);
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
                //CheckWalkCollisions(15, new Vector2(10, -5));


            }
                if (facingRight == false)
                    vitalRec.X = rec.X + 70;
                else
                    vitalRec.X = rec.X + 115;
                vitalRec.Y = rec.Y + 20;
            
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region If the player is too far away, move closer
            //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
            //--knockback or attacking
            if ((distanceFromPlayer > 170) && knockedBack == false && enemyState != EnemyState.attacking)
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

                    if (moveFrame > 8)
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

                    if (moveFrame > 8)
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
                        kb = new Vector2(20, -5);
                    else
                        kb = new Vector2(-20, -5);

                    Attack(30, kb);
                }
                else
                {
                    enemyState = EnemyState.standing;
                }
            }
            #endregion

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
            

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                if (player.VitalRec.Center.X < vitalRec.Center.X)
                    facingRight = false;
                else
                    facingRight = true;

                attackFrame = 0;
                frameDelay = 15;
                if (facingRight)
                {
                    attackRec = new Rectangle(rec.X + rec.Width / 2, rec.Y, 150, 100);
                }
                else
                {
                    attackRec = new Rectangle(rec.X - 20, rec.Y, 150, 100);
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
            if (attackFrame > 3)
            {
                attackFrame = 0;
                attackCooldown = moveNum.Next(40, 90);
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;


            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                //StopAttack();
            }
        }
        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = moveNum.Next(40, 90);
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

             //s.Draw(Game1.whiteFilter, vitalRec, Color.Black);
             //s.Draw(Game1.whiteFilter, attackRec, Color.Red);
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (enemyState == EnemyState.attacking)
                kbvel = Vector2.Zero;

            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = moveNum.Next(60, 180);
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
                currentMap.Drops.Add(new EnemyDrop("ID Card", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
