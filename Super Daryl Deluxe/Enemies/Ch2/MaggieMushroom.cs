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
    public class MaggieMushroom : Enemy
    {

        int standState = 0;
        Random standRand;
        Boolean shakingSpores, launching;
        float rotation;
        Vector2 nextPos;
        Rectangle drawRec;
        int newHeadTimer;
        Boolean secondHead = false;

        public MaggieMushroom(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 110;
            maxHealth = 110;//45 is the real value
            level = 4;
            experienceGiven = 25;
            rec = new Rectangle((int)position.X, (int)position.Y, 132, 136);
            currentlyInMoveState = false;
            enemySpeed = 4;
            tolerance = 8;
            vitalRec = new Rectangle(rec.X, rec.Y, 152, 156);
            maxHealthDrop = 6;
            moneyToDrop = .15f;

            standRand = new Random();
            shakingSpores = false;
            launching = false;

            maxAttackCooldown = 70;

            drawRec = rec;

            newHeadTimer = standRand.Next(300, 1000);
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 152, 156);
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!secondHead)
            {
                newHeadTimer--;

                if (newHeadTimer == 0)
                {
                    secondHead = true;

                    newHeadTimer = standRand.Next(300, 1000);
                }
            }

            if (!respawning)
            {

                if (hostile)
                    attackCooldown--;

                    Move(mapwidth);

                CheckWalkCollisions(20, new Vector2(15, -5));
            }

            drawRec.X = rec.X + rec.Width / 2;
            drawRec.Y = rec.Y + rec.Height / 2;

            vitalRec.X = rec.X;
            vitalRec.Y = rec.Y;
            deathRec = vitalRec;

        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            //--If the enemy hasn't been hit, walk randomly. If he is hostile but far away from the player, ignore him
            if (hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350)
            {
                #region Random movement, not hostile
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                    moveFrame = 0;

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

                            if (moveFrame == 3)
                                frameDelay = 30;

                            frameDelay = 15;
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
                            frameDelay = 4;
                        }

                        if (moveFrame > 6)
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
                            frameDelay = 4;
                        }

                        if (moveFrame > 6)
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

            else if (hostile && distanceFromPlayer < 1700)
            {

                //ALWAYS MOVE TOWARD PLAYER IF CAN'T ATTACK
                if ((distanceFromPlayer > 135) && knockedBack == false && enemyState != EnemyState.attacking && attackCooldown > 0)
                {
                    MoveTowardPlayer(mapWidth);
                }
                //IF CLOSE TO PLAYER BUT CANT ATTACK YET, JUST STAND AND BREATHE
                else if (attackCooldown > 0 && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    enemyState = EnemyState.standing;
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;

                        if (moveFrame == 3)
                            frameDelay = 30;

                        frameDelay = 15;
                    }

                    if (moveFrame > 3)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                }
                //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                else if (attackCooldown <= 0 && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    if (horizontalDistanceToPlayer > 300 && horizontalDistanceToPlayer < 800)
                        launching = true;
                    else if (horizontalDistanceToPlayer <= 135)
                        shakingSpores = true;
                    else
                        MoveTowardPlayer(mapWidth);
                }

                #region Attack once it is close enough
                if (shakingSpores || launching)
                {
                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(10, -5);
                    else
                        kb = new Vector2(-10, -5);

                    //--Only attack if off cooldown
                    if (attackCooldown <= 0)
                    {
                        if (shakingSpores)
                            Attack(24, Vector2.Zero);
                        else
                            Launch(30, kb);
                    }
                }
                #endregion
            }
        }

        public void Launch(int damage, Vector2 kb)
        {
            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;

                frameDelay = 5;
            }
            enemyState = EnemyState.attacking;

            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0 && attackFrame < 2)
            {
                attackFrame++;
                frameDelay = 2;
            }

            if (attackFrame == 2 && frameDelay == 2)
            {
                if (facingRight)
                    velocity = new Vector2(17, -15);
                else
                    velocity = new Vector2(-17, -15);
            }

            if (attackFrame == 2)
            {
                nextPos = position + velocity;

                Vector2 difference = nextPos - position;
                difference.Normalize();
                rotation = (float)Math.Atan2(difference.Y, difference.X);
                rotation -= (float)((3 * Math.PI) / 2);

                Console.WriteLine(rotation);

                if ((player.CheckIfHit(attackRec) || player.CheckIfHit(attackRec)) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));

                    attackFrame = 0;
                    attackCooldown = maxAttackCooldown;
                    enemyState = EnemyState.standing;
                    currentlyInMoveState = false;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    launching = false;
                    rotation = 0;
                    velocity = Vector2.Zero;
                }

            }

            if (knockedBack)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                launching = false;
                rotation = 0;
                velocity = Vector2.Zero;
            }
        }

        public override void ImplementGravity()
        {
            if (VelocityY < 1 && VelocityY > -1 && hangInAir)
            {
                hangInAirTime++;

                VelocityY = 0;

                if (hangInAirTime == 10)
                {
                    hangInAirTime = 0;
                    hangInAir = false;
                }
            }
            else
                velocity.Y += GameConstants.GRAVITY;

            position += velocity;

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 20, rec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 20, plat.Rec.Y + 5, 20, plat.Rec.Height - 3);

                //Don't move through non passable platforms
                if ((rightEn.Intersects(left) || leftEn.Intersects(right)) && plat.Passable == false)
                {
                    if (rightEn.Intersects(left))
                    {
                        if (knockedBack)
                        {
                            PositionX -= knockBackVec.X;
                            knockBackVec.X = 0;
                        }
                        else
                        {
                                position.X -= enemySpeed;
                        }
                        velocity.X = 0;
                    }

                    if (leftEn.Intersects(right))
                    {
                        if (knockedBack)
                        {
                            PositionX += -knockBackVec.X;
                            knockBackVec.X = 0;
                        }
                        else
                        {
                                position.X += enemySpeed;
                        }

                        velocity.X = 0;

                    }
                }


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height;
                    velocity.Y = 0;

                    if (launching && attackFrame == 2)
                    {
                        attackFrame = 0;
                        attackCooldown = maxAttackCooldown;
                        enemyState = EnemyState.standing;
                        currentlyInMoveState = false;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        launching = false;
                        rotation = 0;
                        velocity = Vector2.Zero;
                    }

                    //--Once it collides with the ground, set the moveTimer to 0 and the boolean to false
                    //--This will make the monster start moving again
                    if (knockedBack == true)
                    {
                        moveTimer = 0;
                        currentlyInMoveState = false;
                    }
                    if (velocity.X == 0)
                        knockedBack = false;
                }
                #endregion

                //hit their head on non-passables
                if (topEn.Intersects(bottom) && velocity.Y < 0 && plat.Passable == false)
                {
                    velocity.Y = 0;
                    velocity.Y = GameConstants.GRAVITY;
                }
            }

            #region Not falling off a platform
            //--Don't fall off the platform you're on!
            if (currentPlat != null)
            {
                if (position.X < currentPlat.Rec.X)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width;
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
            {
                attackFrame = 0;

                frameDelay = 5;

                attackRec = new Rectangle(vitalRec.X - 50, vitalRec.Y - 50, VitalRecWidth + 100, VitalRecHeight + 100);

            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 2;
            }

            //SHAKES SPORES EVERYWHERE
            if (attackFrame > 1)
            {
                if ((player.CheckIfHit(attackRec) || player.CheckIfHit(attackRec)) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }
            

            //--Once it has ended, reset
            if (attackFrame > 4)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                shakingSpores = false;
            }

            currentlyInMoveState = true;

            /*
            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                shakingSpores = false;
            }*/
        }

        public override void Draw(SpriteBatch s)
        {


            #region Draw Enemy
            if (!facingRight)
            {
                s.Draw(game.EnemySpriteSheets[name], drawRec, GetSourceRectangle(moveFrame), Color.White * alpha, rotation, new Vector2(rec.Width / 2, rec.Height / 2), SpriteEffects.None, 0f);

            }

            if (facingRight)
            {
                s.Draw(game.EnemySpriteSheets[name], drawRec, GetSourceRectangle(moveFrame), Color.White * alpha, rotation, new Vector2(rec.Width / 2, rec.Height / 2), SpriteEffects.FlipHorizontally, 0f);
            }
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
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

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + "  " + displayName).X;

                s.DrawString(Game1.descriptionFont, "Lv." + level + "  " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2, rec.Y - 35), Color.Black);
            }
            #endregion

            //s.Draw(Game1.emptyBox, rec, Color.Black * .5f);
            //s.Draw(Game1.whiteFilter, vitalRec, Color.Red * .5f);

            if(secondHead)
                s.Draw(Game1.emptyBox, drawRec, Color.Green * .3f);

             if (enemyState == EnemyState.attacking)
                 s.Draw(Game1.emptyBox, attackRec, Color.Blue);
        }

        public void MoveTowardPlayer(int mapWidth)
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
                    frameDelay = 4;
                }

                if (moveFrame > 6)
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
                    frameDelay = 4;
                }

                if (moveFrame > 6)
                    moveFrame = 0;

                currentlyInMoveState = true;
                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
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
            shakingSpores = false;
            launching = false;
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
                attackCooldown = maxAttackCooldown;
                hostile = true;
            }
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Acorn Sack", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
