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
    public class FlufflesTheBandit : Enemy
    {

        int meleeRange = 150;
        int stabDamage = 45;

        Boolean pullingKnife = false;
        Boolean tossingKnife = false;

        public FlufflesTheBandit(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 450;
            maxHealth = 450;
            level = 8;
            experienceGiven = 28;
            rec = new Rectangle((int)position.X, (int)position.Y, 531, 169);
            currentlyInMoveState = false;
            enemySpeed = 4;
            tolerance = 25;
            maxHealthDrop = 5;
            moneyToDrop = .17f;
            vitalRec = new Rectangle(100, 100, 100, 130);

            rectanglePaddingLeftRight = 170;
            distanceFromFeetToBottomOfRectangle = 10;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            enemySpeed = 4;
            if (knockedBack || isStunned)
                return new Rectangle(2655, 338, 531, 169);
            if (pullingKnife)
            {
                return new Rectangle(moveFrame * 531, 169, 531, 169);
            }
            else if (tossingKnife)
            {
                if (frame < 6)
                    return new Rectangle(frame * 531, 507, 531, 169);
                else
                    return new Rectangle((frame - 6) * 531, 676, 531, 169);
            }
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (frame < 7)
                            return new Rectangle(frame * 531, 845, 531, 169);
                        else
                            return new Rectangle((frame - 7) * 531, 845, 531, 169);

                    case EnemyState.moving:
                        return new Rectangle(frame * 531, 0, 531, 169);

                    case EnemyState.attacking:
                        return new Rectangle(attackFrame * 531, 338, 531, 169);
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

            if(facingRight)
                vitalRec.X = rec.X + 230;
            else
                vitalRec.X = rec.X + 200;
            vitalRec.Y = rec.Y + 20;
            deathRec = vitalRec;
        }

        public void TossKnife()
        {
            enemyState = EnemyState.standing;
            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame++;
                frameDelay = 5;

                if (moveFrame > 12)
                {
                    moveFrame = 0;
                    tossingKnife = false;
                }
            }

            currentlyInMoveState = true;
        }

        public void PullKnife(Boolean attackAfter)
        {
            meleeRange = 180;
            enemyState = EnemyState.standing;
            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame++;
                frameDelay = 5;

                if (moveFrame > 5)
                {
                    moveFrame = 0;
                    pullingKnife = false;

                    if (attackAfter)
                    {
                        attackFrame = 0;
                        frameDelay = 5;
                        if (facingRight)
                        {
                            attackRec = new Rectangle(vitalRec.X + vitalRec.Width - 60, vitalRec.Y, 245, 100);
                        }
                        else
                        {
                            attackRec = new Rectangle(vitalRec.X - 185, vitalRec.Y, 245, 100);
                        }
                        RangedAttackRecs.Add(attackRec);

                        enemyState = EnemyState.attacking;
                    }
                    else
                    {
                        tossingKnife = true;
                    }
                }
            }

            currentlyInMoveState = true;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (pullingKnife)
            {
                PullKnife(hostile);
            }
            else if (tossingKnife)
                TossKnife();

            #region Random movement
            else if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                    moveFrame = 0;
                    if (moveState == 0)
                    {
                        if (Game1.randomNumberGen.Next(0, 6) == 1)
                        {
                            frameDelay = 5;
                            pullingKnife = true;
                        }
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

                            frameDelay = 5;
                        }

                        if (moveFrame > 9)
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
                            frameDelay = 5;
                        }

                        if (moveFrame > 6)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X + 170 >= 6)
                            position.X -= enemySpeed;
                        break;
                }

                if (moveTimer <= 0)
                    currentlyInMoveState = false;
            }
            #endregion

            else if (hostile && distanceFromPlayer <= 1700)
            {

                //ALWAYS MOVE TOWARD PLAYER IF CAN'T ATTACK
                if (horizontalDistanceToPlayer > meleeRange && knockedBack == false && enemyState != EnemyState.attacking && attackCooldown > 0)
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

                        frameDelay = 4;
                    }

                    if (moveFrame > 6)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                }
                //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                else if (attackCooldown <= 0 && knockedBack == false && horizontalDistanceToPlayer <= meleeRange && enemyState != EnemyState.attacking && !pullingKnife)
                {
                    //--Face the player if it isn't already. 
                    //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                    //--The wrong way and autoattack in the wrong direction
                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;

                    moveFrame = 0;
                    frameDelay = 5;
                    pullingKnife = true;
                }
                else if (horizontalDistanceToPlayer > meleeRange && knockedBack == false && enemyState != EnemyState.attacking && !pullingKnife)
                    MoveTowardPlayer(mapWidth);
                else if(enemyState == EnemyState.attacking)
                {

                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(10, -5);
                    else
                        kb = new Vector2(-10, -5);
                    Attack(stabDamage, kb);
                }
            }
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
                    frameDelay = 5;
                }

                if (moveFrame > 6)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + 170 >= 6)
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

                if (moveFrame > 6)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
            }

        }

        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
            moveFrame = 0;

            enemyState = EnemyState.attacking;

            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 3;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 0)
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

                moveFrame = 0;

                if(Game1.randomNumberGen.Next(0, 3) == 1)
                    tossingKnife = true;
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

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = maxAttackCooldown;
                hostile = true;
                pullingKnife = false;
            }

            tossingKnife = false;

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 15 - 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 12), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
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
                currentMap.Drops.Add(new EnemyDrop("Stolen Painting", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
