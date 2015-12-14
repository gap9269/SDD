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
    public class CaptainSax : Enemy
    {
        Boolean hasBoombox = true;
        Boolean boomboxDestroyed = false;
        int boomboxDamage = 40;
        int boomboxRange = 400;
        int boomboxHealth;

        int standTimer;
        int roamTimer;

        int timeWithoutBoombox;

        Boolean summoningBoombox = false;

        Boombox boombox;

        public CaptainSax(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 225;
            maxHealth = 225;
            boomboxHealth = maxHealth;
            level = 7;
            experienceGiven = 25;
            rec = new Rectangle((int)position.X, (int)position.Y, 268, 209);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 18;
            vitalRec = new Rectangle(rec.X, rec.Y, 125, 170);
            maxHealthDrop = 5;
            moneyToDrop = .18f;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
            {
                if (hasBoombox)
                    return new Rectangle(2948, 209, 268, 209);
                else
                    return new Rectangle(3216, 0, 268, 209);
            }
            else if (summoningBoombox)
                return new Rectangle(268 * moveFrame, 210, 268, 209);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (hasBoombox)
                            return new Rectangle(268 * moveFrame, 0, 268, 209);
                        else
                            return new Rectangle(268 * (moveFrame + 6), 0, 268, 209);
                    case EnemyState.moving:
                        if (hasBoombox)
                            return new Rectangle(268 * (moveFrame + 5), 628, 268, 209);
                        else
                            return new Rectangle(268 * moveFrame, 628, 268, 209);
                    case EnemyState.attacking:
                        return new Rectangle(268 * attackFrame, 419, 268, 209);
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
            
            vitalRec.Y = rec.Y + 20;

            if (!facingRight)
                vitalRec.X = rec.X + 90;
            else
                vitalRec.X = rec.X + 60;

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

                //In case it falls of the map or something dumb
                if (!hasBoombox)
                {
                    timeWithoutBoombox++;

                    if (timeWithoutBoombox > 800 && !summoningBoombox)
                    {
                        summoningBoombox = true;
                        frameDelay = 5;
                        moveFrame = 0;
                    }
                }

                #region If the player is too far away, move closer

                if (summoningBoombox)
                {
                    enemyState = EnemyState.standing;
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }
                    if (moveFrame == 5 && frameDelay == 5)
                    {
                        boombox.StartDisappearing();
                    }
                    if (moveFrame > 10)
                    {
                        summoningBoombox = false;
                        timeWithoutBoombox = 0;
                        attackCooldown = 100;
                        moveFrame = 0;
                        hasBoombox = true;
                        game.CurrentChapter.CurrentMap.Projectiles.Remove(boombox);
                        standTimer = 100;
                    }

                    currentlyInMoveState = true;
                    standTimer--;
                }
                //Stand right after throwing it or pickig it up
                else if (standTimer > 0 && enemyState != EnemyState.attacking)
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
                    standTimer--;

                    if (standTimer == 0)
                    {
                        if(hasBoombox)
                            roamTimer = moveTime.Next(200, 500);
                        else
                            roamTimer = moveTime.Next(200, 300);
                    }
                }
                //Just walk around and be swaggy
                else if (roamTimer > 0 && enemyState != EnemyState.attacking)
                {
                    roamTimer--;

                    if (currentlyInMoveState == false)
                    {
                        moveState = moveNum.Next(0, 3);
                        moveTimer = moveTime.Next(60, 300);
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

                            if (moveFrame > 4)
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

                            if (moveFrame > 4)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (position.X >= 6)
                                position.X -= enemySpeed;
                            break;

                    }

                    if (moveTimer <= 0)
                        currentlyInMoveState = false;
                }
                else if (!hasBoombox && enemyState != EnemyState.attacking)
                {
                    //Move toward it to get it
                    if (Vector2.Distance(new Vector2(boombox.rec.Center.X, boombox.rec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y)) > 100)
                    {
                        //--If the boombox is to the left
                        if (boombox.rec.Center.X < vitalRec.Center.X)
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

                            if (moveFrame > 4)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            if (position.X >= 6)
                                position.X -= enemySpeed;
                        }
                        //boombox to the right
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

                            if (moveFrame > 4)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            if (position.X <= mapWidth - 6)
                                position.X += enemySpeed;
                        }
                    }
                    else
                    {
                        summoningBoombox = true;
                        frameDelay = 5;
                        moveFrame = 0;
                    }
                }
                else if ((distanceFromPlayer > boomboxRange || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking && hasBoombox)
                {
                    if (distanceFromPlayer > boomboxRange)
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

                            if (moveFrame > 4)
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

                            if (moveFrame > 4)
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

                        if (moveFrame > 5)
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
                    if (attackCooldown <= 0 && hasBoombox)
                    {
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                            facingRight = false;
                        else
                            facingRight = true;

                        attackFrame = 0;
                        frameDelay = 10;

                        enemyState = EnemyState.attacking;
                    }
                }

                if (enemyState == EnemyState.attacking)
                    Attack(boomboxDamage, new Vector2());
                #endregion

            }
        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0)
            {

                experienceGiven += (int)player.extraExperiencePerKill;

                for (int i = 0; i < player.EquippedSkills.Count; i++)
                {
                    if (level >= player.Level - 5)
                    {
                        //Check to see if the skill is below level 4, and that the player's level is high enough to level the skill
                        if (player.EquippedSkills[i].SkillRank < Skill.maxLevel && player.Level >= player.EquippedSkills[i].PlayerLevelsRequiredToLevel[player.EquippedSkills[i].SkillRank - 1])
                            player.EquippedSkills[i].Experience += experienceGiven;
                    }
                }

                Chapter.effectsManager.AddExpNums(experienceGiven, rec, vitalRec.Y);
                player.Experience += experienceGiven;
                DropItem();
                DropHealth();
                DropMoney();
                Chapter.effectsManager.AddSmokePoof(deathRec, 1);
                Sound.PlayRandomRegularPoof(deathRec.Center.X, deathRec.Center.Y);
                //Unlock enemy bio for this enemy
                if (player.AllMonsterBios[name] == false)
                    player.UnlockEnemyBio(name);


                if (Game1.currentChapter.CurrentMap.Projectiles.Contains(boombox))
                {
                    Game1.currentChapter.CurrentMap.Projectiles.Remove(boombox);
                    Chapter.effectsManager.AddSmokePoof(boombox.vitalRec, 2);
                }


                return true;
            }

            return false;
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

            if (attackFrame == 9 && frameDelay == 5)
            {
                hasBoombox = false;

                if (facingRight)
                {
                    boombox = new Boombox((int)vitalRec.X + VitalRecWidth - 20, (int)vitalRec.Y + 20, new Vector2(15, 0), boomboxDamage, new Vector2(10, -5), boomboxDamage, level, boomboxHealth, tolerance / 2, true);
                }
                else
                {
                    boombox = new Boombox((int)vitalRec.X - 85, (int)vitalRec.Y + 20, new Vector2(-15, 0), boomboxDamage, new Vector2(-10, -5), boomboxDamage, level, boomboxHealth, tolerance / 2, false);
                }

                currentMap.Projectiles.Add(boombox);

                standTimer = 100;
            }

            //--Once it has ended, reset
            if (attackFrame > 10)
            {
                attackFrame = 0;
                attackCooldown = 80;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;

            
            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackFrame = 0;
                attackCooldown = 80;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = 80;
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
                healthBoxRec.Y = vitalRec.Y - 45;
                healthBarRec.Y = vitalRec.Y - 43;

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 13), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 13), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

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
                currentMap.Drops.Add(new EnemyDrop("Mix Tape", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
