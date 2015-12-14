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

    public class MeatSplot
    {
        public Rectangle rec;
        public int timeActive;
        public float alpha;

        public void Update()
        {
            timeActive++;
            if (timeActive > 60)
            {
                alpha -= .01f;
            }
        }

        public void Draw(SpriteBatch s, Texture2D t)
        {
            s.Draw(t, rec, new Rectangle(0, 1464, 761, 244), Color.White * alpha);
        }
    }

    public class Eatball : Enemy
    {

        int meleeRange = 150;
        int biteDamage = 45;

        List<MeatSplot> meatSplots;

        int standState;
        public Eatball(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 600;
            maxHealth = 600;
            level = 8;
            experienceGiven = 45;
            rec = new Rectangle((int)position.X, (int)position.Y, 761, 244);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 40;
            maxHealthDrop = 8;
            moneyToDrop = .23f;
            vitalRec = new Rectangle(100, 100, 165, 145);
            meatSplots = new List<MeatSplot>();

            rectanglePaddingLeftRight = 170;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            enemySpeed = 4;
            if (knockedBack || isStunned)
                return new Rectangle(761, 244, 761, 244);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (standState == 0)
                        {
                            if (frame < 5)
                                return new Rectangle(frame * 761, 0, 761, 244);
                            else
                                return new Rectangle(0, 244, 761, 244);
                        }
                        else
                        {
                            if (frame < 5)
                                return new Rectangle(frame * 761, 1708, 761, 244);
                            else
                                return new Rectangle(0, 1952, 761, 244);
                        }
                    case EnemyState.moving:
                        if (frame < 5)
                            return new Rectangle(frame * 761, 488, 761, 244);
                        else
                            return new Rectangle((frame - 5) * 761, 738, 761, 244);

                    case EnemyState.attacking:
                        if (attackFrame < 2)
                            return new Rectangle(2283 + (attackFrame * 761), 732, 761, 244);
                        else if (attackFrame < 7)
                            return new Rectangle((attackFrame - 2) * 761, 976, 761, 244);
                        else
                            return new Rectangle((attackFrame - 7) * 761, 1220, 761, 244);

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
                vitalRec.X = rec.X + 300;
            else
                vitalRec.X = rec.X + 300;

            vitalRec.Y = rec.Y + 70;
            deathRec = vitalRec;
        }


        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            if (enemyState == EnemyState.moving && moveFrame == 5 && frameDelay == 4)
            {
                MeatSplot splot = new MeatSplot();
                splot.rec = rec;
                splot.timeActive = 0;
                splot.alpha = 1;
                meatSplots.Add(splot);
            }

            for (int i = 0; i < meatSplots.Count; i++)
            {
                meatSplots[i].Update();

                    if (meatSplots[i].alpha <= 0)
                    {
                        meatSplots.RemoveAt(i);
                        i--;
                        continue;
                    }

            }

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region Random movement
            if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                    moveFrame = 0;
                    if (moveState == 0)
                    {
                        standState = Game1.randomNumberGen.Next(2);
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

                        if (moveFrame > 5)
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

                        if (moveFrame > 7)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X <= mapWidth - 6 && moveFrame > 2 && moveFrame < 6)
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

                        if (moveFrame > 7)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        enemySpeed = 6;

                        if (position.X + 170 >= 6 && moveFrame > 2 && moveFrame < 6)
                            position.X -= enemySpeed;
                        break;
                }

                if (moveTimer <= 0)
                {
                    if((enemyState == EnemyState.moving && (moveFrame < 2 || moveFrame > 5)) || enemyState != EnemyState.moving)
                        currentlyInMoveState = false;
                }
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

                        frameDelay = 5;
                    }

                    if (moveFrame > 5)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                }
                //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                else if (attackCooldown <= 0 && knockedBack == false && horizontalDistanceToPlayer <= meleeRange && enemyState != EnemyState.attacking)
                {
                    //--Face the player if it isn't already. 
                    //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                    //--The wrong way and autoattack in the wrong direction
                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;

                        attackRec = new Rectangle(vitalRec.X + VitalRecWidth / 2 - 200, vitalRec.Y, 200, 150);

                    }
                    else
                    {
                        facingRight = true;
                        attackRec = new Rectangle(vitalRec.X + VitalRecWidth / 2, vitalRec.Y, 200, 150);

                    }
                    moveFrame = 0;
                    frameDelay = 5;

                    enemyState = EnemyState.attacking;

                }
                else if (horizontalDistanceToPlayer > meleeRange && enemyState != EnemyState.attacking)
                    MoveTowardPlayer(mapWidth);
                else if(enemyState == EnemyState.attacking)
                {
                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(10, -5);
                    else
                        kb = new Vector2(-10, -5);
                    Attack(biteDamage, kb);
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

                if (moveFrame > 7)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + 170 >= 6 && moveFrame > 2 && moveFrame < 6)
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

                if (moveFrame > 7)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6 && moveFrame > 2 && moveFrame < 6)
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
                frameDelay = 5;
                if (attackFrame > 1 && attackFrame < 6)
                    frameDelay = 3;

                else if (attackFrame >= 6)
                    frameDelay = 7;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame == 5 || attackFrame == 6)
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
            if (attackFrame > 7)
            {
                attackFrame = 0;
                attackCooldown = 80;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);

                moveFrame = 0;
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

            for (int i = 0; i < meatSplots.Count; i++)
            {
                meatSplots[i].Draw(s, game.EnemySpriteSheets[name]);
            }

            if (!facingRight)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
            else
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
                currentMap.Drops.Add(new EnemyDrop("Fuzzy Meat Chunk", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
