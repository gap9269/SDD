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
    public class Lawyer : Enemy
    {
        //This is slightly different for each enemy so they don't stack
        int minDistanceToAttack;

        public Lawyer(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            hostile = true;
            health = 45;
            maxHealth = 45;
            level = 2;
            experienceGiven = 1;
            rec = new Rectangle((int)position.X, (int)position.Y, 76, 228);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 3;
            vitalRec = new Rectangle(rec.X + 10, rec.Y + 25, 50, 200);
            maxHealthDrop = 8;
            displayName = "Copyright Lawyer";
            minDistanceToAttack = moveTime.Next(100, 150);
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(983, 0, 76, 228);
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            attackCooldown--;

            Move(mapwidth);
            CheckWalkCollisions(5, new Vector2(10, -5));

            vitalRec.X = rec.X + 10;
            vitalRec.Y = rec.Y + 25;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region If the player is too far away, move closer
            //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
            //--knockback or attacking
            if ((distanceFromPlayer > 130) && knockedBack == false && enemyState != EnemyState.attacking)
            {
                //--If the player is to the left
                if (player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    facingRight = false;
                    enemyState = EnemyState.moving;
                   
                    currentlyInMoveState = true;
                    if (position.X >= 6)
                        position.X -= enemySpeed;
                }
                //Player to the right
                else
                {
                    facingRight = true;
                    enemyState = EnemyState.moving;
                    
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

                    Attack(7, kb);
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
            if (player.VitalRec.Center.X < vitalRec.Center.X)
                facingRight = false;
            else
                facingRight = true;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
                attackFrame = 0;

            enemyState = EnemyState.attacking;


            if (facingRight)
            {
                if(attackFrame < 9)
                    PositionX += 10;
                else
                    PositionX -= 20;

            }
            else
            {
                if (attackFrame < 9)
                    PositionX -= 10;
                else
                    PositionX += 20;
            }
            attackFrame++;

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 1)
            {
                if (attackRec.Intersects(player.VitalRec) && player.InvincibleTime <= 0)
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
            if (attackFrame > 12)
            {
                attackFrame = 0;
                attackCooldown = moveTime.Next(70, 150);
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
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
            base.Draw(s);

        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle col, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, col, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = moveTime.Next(70, 150);
                hostile = true;
            }
        }

        public override void DropItem()
        {
            base.DropItem();
        }
    }
}
