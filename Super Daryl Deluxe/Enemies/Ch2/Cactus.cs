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
    public class SexySaguaro : Enemy
    {

        int meleeRange = 275;
        int meleeDamage = 45;
        int stabDamage = 45;

        int standState;

        int sparkleFrame;
        int sparkleFrameDelay = 5;

        int flinchTimer = 0;
        int canAttackTimer;

        Boolean surprised = false;
        Boolean canAttack = false;

        public enum AttackState
        {
            none, punching, stabbing
        }
        AttackState attackState;

        int punchNum = 0;

        public SexySaguaro(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 600;
            maxHealth = 600;
            level = 12;
            experienceGiven = 45;
            rec = new Rectangle((int)position.X, (int)position.Y, 777, 465);
            currentlyInMoveState = false;
            enemySpeed = 0;
            tolerance = 40;
            maxHealthDrop = 8;
            moneyToDrop = .23f;
            vitalRec = new Rectangle(100, 100, 165, 240);
            canBeKnockbacked = false;
            maxAttackCooldown = 150;
            hostile = true;

            distanceFromFeetToBottomOfRectangle = 38;
            rectanglePaddingLeftRight = 300;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (flinchTimer > 0 || isStunned)
                return new Rectangle(3108, 1860, 777, 465);
            else if (surprised)
            {
                if (frame < 5)
                    return new Rectangle(frame * 777, 3255, 777, 465);
                else
                    return new Rectangle(2331 + ((frame - 5) * 777), 930, 777, 465);
            }
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (standState == 0)
                        {
                            return new Rectangle(frame * 777, 0, 777, 465);
                        }
                        else if (standState == 2)
                        {
                            return new Rectangle(frame * 777, 465, 777, 465);
                        }
                        else
                            return new Rectangle(3108, 0, 777, 465);

                    case EnemyState.attacking:
                        if (attackState == AttackState.punching)
                        {
                            if (punchNum == 0)
                                return new Rectangle(attackFrame * 777, 930, 777, 465);
                            else
                                return new Rectangle(attackFrame * 777, 1395, 777, 465);

                        }
                        else
                        {
                            if (attackFrame < 5)
                                return new Rectangle(attackFrame * 777, 2325, 777, 465);
                            else
                                return new Rectangle((attackFrame - 5) * 777, 2790, 777, 465);
                        }
                }
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public override void PlaySoundWhenHit()
        {
            //int soundType = moveNum.Next(2);

            //if (soundType == 0)
            //    Sound.enemySoundEffects["ErlHit1"].CreateInstance().Play();
            //else
            //    Sound.enemySoundEffects["ErlHit2"].CreateInstance().Play();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!respawning && !isStunned)
            {

                if (hostile)
                {
                    attackCooldown--;
                }
                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }

            if (flinchTimer > 0)
                flinchTimer--;

            vitalRec.X = rec.X + 300;
            vitalRec.Y = rec.Y + 110;

            deathRec = vitalRec;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (canAttackTimer > 0)
            {
                canAttackTimer--;

                if (canAttackTimer <= 0)
                    canAttack = false;
            } 
            
            if (surprised)
            {
                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;

                    frameDelay = 5;
                }

                if (moveFrame > 6)
                {
                    canAttackTimer = 180;
                    canAttack = true;
                    moveFrame = 0;
                    surprised = false;
                }
            }
            #region Random flexing

            else if ((!hostile || distanceFromPlayer > 700 || verticalDistanceToPlayer > 350 || IsAbovePlayer() || (attackCooldown > 0 || horizontalDistanceToPlayer > meleeRange)) && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveTimer = moveTime.Next(60, 150);
                    moveFrame = 0;
                    standState = Game1.randomNumberGen.Next(0, 3);
                }
                enemyState = EnemyState.standing;

                switch (standState)
                {
                    case 0:
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            frameDelay = 9;
                        }

                        if (moveFrame > 3)
                            moveFrame = 0;
                        break;
                    case 2:
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            frameDelay = 8;
                        }

                        if (moveFrame > 4)
                            moveFrame = 0;
                        break;
                }

                currentlyInMoveState = true;
                moveTimer--;

                if (moveTimer <= 0)
                {
                    currentlyInMoveState = false;
                }
            }
            #endregion

            else if (distanceFromPlayer <= 700 && attackCooldown <= 0 && flinchTimer <= 0 || enemyState == EnemyState.attacking)
            {
                
                //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                if (horizontalDistanceToPlayer <= meleeRange && enemyState != EnemyState.attacking && canAttack)
                {
                    moveFrame = 0;
                    frameDelay = 5;

                    if (Game1.randomNumberGen.Next(2) == 0)
                        attackState = AttackState.punching;
                    else
                        attackState = AttackState.stabbing;

                    enemyState = EnemyState.attacking;

                    //--Face the player if it isn't already. 
                    //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                    //--The wrong way and autoattack in the wrong direction
                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;

                        if(attackState == AttackState.punching)
                            attackRec = new Rectangle(vitalRec.X + VitalRecWidth / 2 - 300, vitalRec.Y, 350, 250);
                        else
                            attackRec = new Rectangle(rec.X + 70, rec.Y + 45, rec.Width - 155, rec.Height - 50);
                    }
                    else
                    {
                        facingRight = true;

                        if (attackState == AttackState.punching)
                            attackRec = new Rectangle(vitalRec.X + VitalRecWidth / 2 - 50, vitalRec.Y, 350, 250);
                        else
                            attackRec = new Rectangle(rec.X + 80, rec.Y + 45, rec.Width - 155, rec.Height - 50);
                    }

                }
                else if (enemyState == EnemyState.attacking)
                {
                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(10, -5);
                    else
                        kb = new Vector2(-10, -5);

                    Attack(meleeDamage, kb);
                }
                else if (canAttack == false && !surprised)
                {
                    surprised = true;
                    moveFrame = 0;
                    frameDelay = 5;
                }
            }
        }

        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
            moveFrame = 0;

            enemyState = EnemyState.attacking;
            currentlyInMoveState = true;

            if (attackState == AttackState.punching)
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;
                    frameDelay = 5;
                }

                if (attackFrame < 2 && player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(meleeDamage, level);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }


                //--Once it has ended, reset
                if (attackFrame > 2)
                {
                    attackFrame = 0;

                    if (punchNum == 0)
                    {
                        punchNum = 1;
                        enemyState = EnemyState.attacking;

                    }
                    else
                    {
                        punchNum = 0;
                        attackCooldown = maxAttackCooldown;
                        enemyState = EnemyState.standing;
                        currentlyInMoveState = false;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        moveFrame = 0;
                        attackState = AttackState.none;
                        canAttack = false;
                    }
                }
            }
            else
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;
                    frameDelay = 5;

                    if (attackFrame == 4)
                        frameDelay = 7;
                    else if (frameDelay > 4 && frameDelay < 7)
                        frameDelay = 6;
                }

                if ((attackFrame ==5 || attackFrame == 6) && player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(stabDamage, level);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }


                //--Once it has ended, reset
                if (attackFrame > 8)
                {
                    canAttack = false;
                    attackFrame = 0;
                    attackCooldown = maxAttackCooldown;
                    enemyState = EnemyState.standing;
                    currentlyInMoveState = false;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    moveFrame = 0;
                    attackState = AttackState.none;

                }
            }

            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (flinchTimer > 0)
            {
                punchNum = 0;
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackState = AttackState.none;
                attackRec = new Rectangle(0, 0, 0, 0);
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();

            punchNum = 0;
            attackFrame = 0;
            attackCooldown = maxAttackCooldown;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackState = AttackState.none;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            flinchTimer = 12;
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy

            if (facingRight)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
            else
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            if (enemyState == EnemyState.standing && flinchTimer <= 0 && !isStunned)
            {
                sparkleFrameDelay--;

                if (sparkleFrameDelay <= 0)
                {
                    sparkleFrame++;
                    sparkleFrameDelay = 7;
                    if (sparkleFrame > 3)
                        sparkleFrame = 0;
                }
                s.Draw(game.EnemySpriteSheets[name], rec, new Rectangle((777 * sparkleFrame), 1860, 777, 465), Color.White * alpha);
            }

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 3), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 3), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion


            //s.Draw(Game1.whiteFilter, attackRec, Color.Red * .5f);

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

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Peyote", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35 && currentMap.MapName == "Indoor Garden" && game.CurrentQuests.Contains(game.SideQuestManager.desertMemorial))
            {
                currentMap.Drops.Add(new EnemyDrop("Cactus Flower", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
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

            Rectangle feet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - distanceFromFeetToBottomOfRectangle, vitalRec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 25, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);



                if (knockedBack)
                {
                    Rectangle checkPlatRec;

                    if (VelocityX >= 0)
                    {
                        checkPlatRec = new Rectangle(rightEn.X, rightEn.Y, (int)velocity.X, rightEn.Height);

                        if (checkPlatRec.Intersects(left))
                        {
                            //playerState = PlayerState.standing;
                            PositionX -= VelocityX;
                            knockedBack = false;
                            VelocityX = 0;
                            // playerState = PlayerState.standing;
                        }
                    }
                    else
                    {
                        checkPlatRec = new Rectangle(leftEn.X - Math.Abs((int)VelocityX), leftEn.Y, Math.Abs((int)velocity.X), leftEn.Height);

                        if (checkPlatRec.Intersects(right))
                        {
                            // playerState = PlayerState.standing;
                            PositionX += Math.Abs(VelocityX);
                            knockedBack = false;
                            VelocityX = 0;
                            //playerState = PlayerState.standing;
                        }
                    }
                }
                //DOn't move through non passable platforms
                if ((rightEn.Intersects(left) || leftEn.Intersects(right)) && plat.Passable == false)
                {
                    if (rightEn.Intersects(left))
                    {
                        position.X -= enemySpeed;

                        if (VelocityX > 0)
                        {
                            PositionX -= (int)VelocityX;
                            velocity.X = 0;
                        }
                    }

                    if (leftEn.Intersects(right))
                    {
                        position.X += enemySpeed;

                        if (VelocityX < 0)
                        {
                            PositionX += (int)Math.Abs(VelocityX);
                            velocity.X = 0;
                        }

                    }
                }


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + distanceFromFeetToBottomOfRectangle;
                    velocity.Y = 0;


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
                if (position.X < currentPlat.Rec.X - 300)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X - 300;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + 300 && currentMap.MapName != "Indoor Garden")
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + 300;
                }
            }
            #endregion

        }
    }
}
