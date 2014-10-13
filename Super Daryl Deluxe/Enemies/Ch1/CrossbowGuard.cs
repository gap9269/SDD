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
    public class CrossbowGuard : Enemy
    {


        float gunRotation;
        int arrowSpeed;
        Vector2 gunPos;
        int reloadCooldown = 90;

        public CrossbowGuard(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 180;
            maxHealth = 180;
            level = 8;
            experienceGiven = 50;
            rec = new Rectangle((int)position.X, (int)position.Y, 340, 155);
            currentlyInMoveState = false;
            enemySpeed = 5;
            tolerance = 15;
            vitalRec = new Rectangle(rec.X, rec.Y, 175, 75);
            maxHealthDrop = 10;
            hostile = true;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(0, 0, 680, 310);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        return new Rectangle(0, 382, 680, 310);
                    case EnemyState.moving:
                        return new Rectangle(680 * moveFrame, 382, 680, 310);
                    case EnemyState.attacking:
                        return new Rectangle(0, 382, 680, 310);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);
            if (!respawning)
            {
                if (reloadCooldown > 0)
                    reloadCooldown--;

                if (hostile && reloadCooldown <= 0)
                    attackCooldown--;

                Move(mapwidth);

                if (facingRight)
                    gunPos = new Vector2(position.X + 125, position.Y + 107);
                else
                    gunPos = new Vector2(position.X + 180, position.Y + 107);

                Vector2 gunToPlayer = new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y) - gunPos;

                if (reloadCooldown <= 0)
                    gunRotation = (float)Math.Atan2(gunToPlayer.Y, gunToPlayer.X);
            }

            vitalRec.X = rec.X + 75;
            vitalRec.Y = rec.Y + 75;

        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region If the player is too far away, move closer
            //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
            //--knockback or attacking
            if ((distanceFromPlayer > 1000) && knockedBack == false && enemyState != EnemyState.attacking)
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

                    if (moveFrame > 2)
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

                    if (moveFrame > 2)
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
                        kb = new Vector2(7, -5);
                    else
                        kb = new Vector2(-7, -5);

                    Attack(18, kb);
                }
                else if(reloadCooldown <= 0)
                {
                    enemyState = EnemyState.standing;

                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;
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

            if (player.VitalRec.Center.X < vitalRec.Center.X)
                facingRight = false;
            else
                facingRight = true;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                Vector2 v = new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y) - gunPos;
                v.Normalize();

                Projectile arrow = new Projectile((int)gunPos.X, (int)gunPos.Y, 180, v, gunRotation, 10, new Vector2(5,-5), 1, 0, 10, Projectile.ProjType.arrow);

                currentMap.Projectiles.Add(arrow);
            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;

            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 5;
            }


            //--Once it has ended, reset
            if (attackFrame > 0)
            {
                attackFrame = 0;
                reloadCooldown = 90;
                attackCooldown = moveNum.Next(60, 120);
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
            reloadCooldown = 90;
            attackCooldown = moveNum.Next(60, 120);
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            int whichGunToDraw = 0;

            if (reloadCooldown > 0)
                whichGunToDraw = 38;
            

            if(facingRight)
            s.Draw(spriteSheet, new Rectangle((int)position.X + 125, (int)position.Y + 107, 133, 19), new Rectangle(1360, whichGunToDraw, 266, 38), Color.White, gunRotation, new Vector2(20, 9), SpriteEffects.None, 0f);

            else
                s.Draw(spriteSheet, new Rectangle((int)position.X + 180, (int)position.Y + 107, 133, 19), new Rectangle(1360, whichGunToDraw, 266, 38), Color.White, gunRotation, new Vector2(20, 9), SpriteEffects.FlipVertically, 0f);

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
                reloadCooldown = 90;
                attackCooldown = moveNum.Next(60, 120);
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
