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
    public class AnubisCommanderDemo : Enemy
    {
        Random standRand;

        Rectangle lineOfSight, locustBounds;
        int lineOfSightLength = 600;
        int maxHandDistance = 800;
        int hostileDistance = 1700;
        int meleeRange = 370;
        int standState;
        int dashType = 0;

        int chasePlayerDelay = 30;
        int maxChasePlayerDelay = 30;
        int minLocustCooldown = 1000;
        int maxLocustCooldown = 2500;
        int maxHandCooldown = 300;
        int afterSpawnWaitTime = 180;
        int maxAfterSpawnWaitTime = 180;

        Boolean summoningLocust = false;
        Boolean dashing = false;

        public enum AttackState
        {
            none,
            spear,
            spear2,
            spear3,
            summonHands
        }
        public AttackState attackState;

        int swipeDamage, handDamage, handCooldown, locustCooldown, collisionDamage;

        public AnubisCommanderDemo(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle locustBound)
            : base(pos, type, g, ref play, cur)
        {

            health = 4000;
            maxHealth = 4000;
            level = 15;
            experienceGiven = 250;
            rec = new Rectangle((int)position.X, (int)position.Y, 775, 458);
            currentlyInMoveState = false;
            enemySpeed = 6;
            tolerance = 120;
            vitalRec = new Rectangle(rec.X, rec.Y, 120, 280);
            maxHealthDrop = 80;
            moneyToDrop = .15f;

            standRand = new Random();

            maxAttackCooldown = 90;

            swipeDamage = 55;
            handDamage = 60;

            locustCooldown = maxLocustCooldown;

            locustBounds = locustBound;

            hostile = true;

            canBeStunned = false;
        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            if (dashing)
                return new Rectangle((775 * moveFrame) + 775, 916, 775, 458);

            if (summoningLocust)
            {
                if (moveFrame < 2)
                    return new Rectangle((775 * moveFrame) + 2325, 2290, 775, 458);
                else if (moveFrame < 7)
                    return new Rectangle(775 * (moveFrame - 2), 2748, 775, 458);
                else
                    return new Rectangle(775 * (moveFrame - 7), 3206, 775, 458);
            }

            if (isStunned || knockedBack)
                return new Rectangle(775, 0, 775, 458);

            else if (enemyState == EnemyState.standing)
            {
                if (hostile)
                    if(moveFrame != 5)
                        return new Rectangle(775 * moveFrame, 458, 775, 458);
                    else
                        return new Rectangle(0, 916, 775, 458);
                else
                    return new Rectangle(0, 0, 775, 458);

            }
            else if (enemyState == EnemyState.moving)
            {
                if (moveFrame < 5)
                    return new Rectangle(775 * moveFrame, 1374, 775, 458);
                else if (moveFrame < 10)
                    return new Rectangle(775 * (moveFrame - 5), 1832, 775, 458);
                else
                    return new Rectangle(775 * (moveFrame - 10), 2290, 775, 458);

            }
            else if (enemyState == EnemyState.attacking)
            {
                if (attackState == AnubisCommanderDemo.AttackState.spear || attackState == AnubisCommanderDemo.AttackState.spear3)
                {
                    return new Rectangle(775 * attackFrame, 0, 775, 458);
                }
                else if (attackState == AnubisCommanderDemo.AttackState.spear2)
                {
                    return new Rectangle(775 * attackFrame, 458, 775, 458);
                }
                else if (attackState == AnubisCommanderDemo.AttackState.summonHands)
                {
                    if (attackFrame < 2)
                        return new Rectangle((775 * attackFrame) + 2325, 2290, 775, 458);
                    else if (attackFrame < 7)
                        return new Rectangle(775 * (attackFrame - 2), 2748, 775, 458);
                    else
                        return new Rectangle(775 * (attackFrame - 7), 3206, 775, 458);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            //Do this special because the code to keep them on the map doesn't work well for anubis
            #region Base Update Code
            verticalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(0, player.VitalRec.Center.Y), new Vector2(0, vitalRec.Center.Y)));
            horizontalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(player.VitalRec.Center.X, 0), new Vector2(vitalRec.Center.X, 0)));

            if (isStunned)
            {
                stunTime--;

                if (stunTime <= 0)
                {
                    isStunned = false;
                    stunTime = 0;
                }
            }

            if (hitPauseTimer >= 0)
                hitPauseTimer--;

            ImplementGravity();
            UpdateKnockBack();

            UpdateRectangles();

            #region Fade In
            if (spawnWithPoof)
            {
                if (respawning == true)
                {
                    //alpha += .5f;
                    timeBeforeSpawn--;
                }
                if (timeBeforeSpawn <= 0 && respawning == true)
                {
                    alpha = 1f;
                    Sound.PlayRandomRegularPoof(vitalRec.Center.X, vitalRec.Center.Y);
                    Chapter.effectsManager.AddSmokePoof(vitalRec, 2);
                    respawning = false;
                }
            }
            else
            {
                alpha = 1f;
                respawning = false;
            }
            #endregion

            #region Dont run off map
            if (position.X <= 0)
            {
                position.X = 0;
            }

            if (position.X + rec.Width - 300 >= mapwidth)
            {
                position.X = mapwidth - rec.Width + 300;
            }
            #endregion
            #endregion

            if (!respawning && !isStunned)
            {

                if (hostile)
                {
                    handCooldown--;
                    attackCooldown--;
                    locustCooldown--;
                }

            if (hitPauseTimer <= 0)
                Move(mapwidth);
            }

            vitalRec.X = rec.X + 110;
            vitalRec.Y = rec.Y + 100;

            deathRec = vitalRec;
        }

        public void SummonLocust()
        {
            enemyState = EnemyState.standing;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                frameDelay = 4;
            }

            if (moveFrame == 7 && frameDelay == 2)
            {
                String soundEffectName = "enemy_anubis_spawn_locust";
                Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                Locust en = new Locust(new Vector2(VitalRecX, VitalRecY - 400), "Locust", game, ref player, game.CurrentChapter.CurrentMap, locustBounds);
                en.Hostile = true;
                game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en);


                Locust en1 = new Locust(new Vector2(VitalRecX -200, VitalRecY - 230), "Locust", game, ref player, game.CurrentChapter.CurrentMap, locustBounds);
                en1.Hostile = true;
                game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en1);


                Locust en2 = new Locust(new Vector2(VitalRecX + 200, VitalRecY - 150), "Locust", game, ref player, game.CurrentChapter.CurrentMap, locustBounds);
                en2.Hostile = true;
                game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en2);

                if (game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap.ContainsKey("Locust"))
                    game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Locust"] += 3;
            }

            if (moveFrame > 10)
            {
                afterSpawnWaitTime = maxAfterSpawnWaitTime;
                summoningLocust = false;
                moveFrame = 0;
                frameDelay = 5;
                locustCooldown = moveTime.Next(minLocustCooldown, maxLocustCooldown);
            }
        }

        /// <summary>
        /// 0 is just a dash, 1 is dash then hands, 2 is dash then spawn locust
        /// </summary>
        /// <param name="actionType"></param>
        public void Dash()
        {
            if (moveFrame < 2)
            {
                if (facingRight)
                {
                    position.X -= 28;
                }
                else
                {
                    position.X += 28;
                }

            }
            enemyState = EnemyState.standing;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                frameDelay = 5;
            }

            if (moveFrame > 3)
            {
                dashing = false;
                moveFrame = 0;
                frameDelay = 5;

                if (dashType == 1)
                {
                    String soundEffectName = "enemy_anubis_summon_0" + Game1.randomNumberGen.Next(1, 3).ToString();
                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                    attackState = AttackState.summonHands;

                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(5, -3);
                    else
                        kb = new Vector2(-5, -3);

                    Attack(0, kb);
                }
                else if(dashType == 2)
                {
                    String soundEffectName = "enemy_anubis_summon_0" + Game1.randomNumberGen.Next(1, 3).ToString();
                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                    summoningLocust = true;
                    moveFrame = 0;
                    frameDelay = 4;
                }
                dashType = 0;
            }
        }

        public void Stand()
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

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            meleeRange = 500;

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));


            if (dashing)
                Dash();
            else if (summoningLocust)
                SummonLocust();

            #region Not hostile or too far away
            else if ((hostile == false || distanceFromPlayer > hostileDistance || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && enemyState != EnemyState.attacking)
            {
                if (!summoningLocust)
                {
                    if (hostile == false)
                    {
                        enemyState = EnemyState.standing;
                    }

                    else
                    {
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
                                    frameDelay = 4;
                                }

                                if (moveFrame > 13)
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

                                if (moveFrame > 13)
                                    moveFrame = 0;

                                currentlyInMoveState = true;
                                moveTimer--;

                                if (position.X >= 6)
                                    position.X -= enemySpeed;
                                break;
                        }
                        currentlyInMoveState = true;
                        moveTimer--;

                        if (moveTimer <= 0)
                            currentlyInMoveState = false;
                    }
                }
            }
            #endregion
            else if (hostile && distanceFromPlayer <= hostileDistance)
            {

                //Only summon locust if there is less than 20 enemies in the map
                if (currentMap.EnemiesInMap.Count < 20)
                {
                    if (locustCooldown <= 0 && enemyState != EnemyState.attacking)
                    {
                        String soundEffectName = "enemy_anubis_dash";
                        Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                        moveFrame = 0;
                        frameDelay = 10;
                        dashType = 2;
                        dashing = true;
                    }
                }

                if (!summoningLocust)
                {
                    //ALWAYS MOVE TOWARD PLAYER IF TOO FAR AWAY
                    if (((distanceFromPlayer > maxHandDistance && handCooldown > 0) || (distanceFromPlayer < maxHandDistance && distanceFromPlayer > meleeRange && attackCooldown > 0)) && knockedBack == false && enemyState != EnemyState.attacking)
                    {
                        MoveTowardPlayer(mapWidth);
                    }
                    //IF CLOSE TO PLAYER BUT CANT ATTACK YET, JUST STAND AND BREATHE
                    else if (distanceFromPlayer <= meleeRange && attackCooldown > 0 && handCooldown > 0 && knockedBack == false && enemyState != EnemyState.attacking)
                    {
                        Stand();
                    }
                    //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                    else if ((attackCooldown <= 0 || handCooldown <= 0) && knockedBack == false && enemyState != EnemyState.attacking)
                    {
                        if (horizontalDistanceToPlayer > meleeRange && horizontalDistanceToPlayer <= maxHandDistance && handCooldown <= 0)
                            attackState = AnubisCommanderDemo.AttackState.summonHands;
                        else if (horizontalDistanceToPlayer <= meleeRange)
                        {
                            int randomHandAttack = 0;
                            
                            if(handCooldown <= 0)
                                randomHandAttack = Game1.randomNumberGen.Next(0, 30);

                            if (randomHandAttack == 1)
                            {
                                int dashOrNot = Game1.randomNumberGen.Next(3);

                                if (dashOrNot == 2)
                                {
                                    moveFrame = 0;
                                    frameDelay = 10;
                                    dashType = 1;
                                    dashing = true;

                                    String soundEffectName = "enemy_anubis_dash";
                                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);
                                }
                                else
                                {
                                    String soundEffectName = "enemy_anubis_summon_0" + Game1.randomNumberGen.Next(1, 3).ToString();
                                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                                    attackState = AnubisCommanderDemo.AttackState.summonHands;
                                }
                            }
                            else if (attackCooldown <= 0)
                            {
                                int attackType = Game1.randomNumberGen.Next(3);

                                if (attackType == 0)
                                    attackState = AnubisCommanderDemo.AttackState.spear;
                                else if (attackType == 1)
                                    attackState = AnubisCommanderDemo.AttackState.spear2;
                                else
                                    attackState = AnubisCommanderDemo.AttackState.spear3;
                            }
                            else
                                Stand();
                        }
                        else
                            MoveTowardPlayer(mapWidth);
                    }

                    #region Attack once it is close enough
                    if (attackState == AnubisCommanderDemo.AttackState.spear || attackState == AnubisCommanderDemo.AttackState.spear2 || attackState == AnubisCommanderDemo.AttackState.spear3)
                    {
                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(10, -5);
                        else
                            kb = new Vector2(-10, -5);
                        Attack(swipeDamage, kb);
                    }
                    else if (attackState == AnubisCommanderDemo.AttackState.summonHands)
                    {
                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(5, -3);
                        else
                            kb = new Vector2(-5, -3);

                        Attack(0, kb);
                    }
                    #endregion
                }
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;

                frameDelay = 4;

                //--Face the player if it isn't already. 
                //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                //--The wrong way and autoattack in the wrong direction
                if (player.VitalRec.Center.X < vitalRec.Center.X)
                    facingRight = false;
                else
                    facingRight = true;

                if (attackState == AnubisCommanderDemo.AttackState.spear || attackState == AnubisCommanderDemo.AttackState.spear3)
                {
                    String soundEffectName = "enemy_anubis_swipe_0" + Game1.randomNumberGen.Next(1, 5).ToString();
                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                    chasePlayerDelay = maxChasePlayerDelay;
                    if (facingRight)
                    {
                        attackRec = new Rectangle(vitalRec.X + vitalRec.Width - 60, vitalRec.Y + 20, 550, 200);
                    }
                    else
                    {
                        attackRec = new Rectangle(vitalRec.X - 480, vitalRec.Y + 20, 550, 200);
                    }
                    //RangedAttackRecs.Add(attackRec);
                }
                else if (attackState == AnubisCommanderDemo.AttackState.spear2)
                {
                    String soundEffectName = "enemy_anubis_swipe_0" + Game1.randomNumberGen.Next(1, 5).ToString();
                    Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                    chasePlayerDelay = maxChasePlayerDelay;
                    if (facingRight)
                    {
                        attackRec = new Rectangle(vitalRec.X + vitalRec.Width - 60, vitalRec.Y - 65, 480, 355);
                    }
                    else
                    {
                        attackRec = new Rectangle(vitalRec.X - 420, vitalRec.Y - 65, 480, 355);
                    }
                }
                else
                    chasePlayerDelay = 0;
            }
            enemyState = EnemyState.attacking;

            #region SMACK ATTACK
            if (attackState == AnubisCommanderDemo.AttackState.spear || attackState == AnubisCommanderDemo.AttackState.spear2 || attackState == AnubisCommanderDemo.AttackState.spear3)
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;
                    frameDelay = 4;

                    if (attackFrame > 2)
                        frameDelay = 1;
                }

                //--If the player gets hit in the middle of the animation, do damage and knockback
                if (attackFrame < 3 && attackFrame > 0)
                {
                    if ((player.CheckIfHit(attackRec) || player.CheckIfHit(attackRec)) && player.InvincibleTime <= 0)
                    {

                        player.TakeDamage(damage, level);
                        player.KnockPlayerBack(kb);
                        hitPauseTimer = 5;
                        player.HitPauseTimer = 5;
                        game.Camera.ShakeCamera(3, 3);
                        MyGamePad.SetRumble(3, .4f);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));

                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 4)
                {
                    if (attackState == AttackState.spear3)
                    {
                        attackFrame = 0;
                        attackState = AttackState.spear2;
                        frameDelay = 4;
                    }
                    else
                    {
                        attackCooldown = maxAttackCooldown;

                        attackFrame = 0;
                        enemyState = EnemyState.standing;
                        currentlyInMoveState = false;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AnubisCommanderDemo.AttackState.none;
                    }
                }
            }
            #endregion

            else if (attackState == AnubisCommanderDemo.AttackState.summonHands)
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;

                    frameDelay = 4;

                    if (attackFrame == 7)
                    {
                        LargeHellHand arrow;

                        String soundEffectName = "enemy_anubis_shockwave_0" + Game1.randomNumberGen.Next(1, 4).ToString();
                        Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);

                        if (facingRight)
                        {
                            arrow = new LargeHellHand((int)vitalRec.X + VitalRecWidth + 40, (int)currentPlat.RecY - 190, new Vector2(0, 0), handDamage, new Vector2(10, -4), true, level);
                        }
                        else
                        {
                            arrow = new LargeHellHand((int)vitalRec.X - 40 - 196, (int)currentPlat.RecY - 190, new Vector2(0, 0), handDamage, new Vector2(-10, -4), false, level);
                        }

                        currentMap.Projectiles.Add(arrow);
                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 10)
                {
                    handCooldown = maxHandCooldown;

                    attackFrame = 0;
                    enemyState = EnemyState.standing;
                    currentlyInMoveState = false;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    attackState = AnubisCommanderDemo.AttackState.none;

                }
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
                attackState = AnubisCommanderDemo.AttackState.none;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.emptyBox, rec, Color.Black);
            //s.Draw(Game1.whiteFilter, attackRec, Color.Black);
          //  s.Draw(Game1.whiteFilter, vitalRec, Color.Blue);

            #region Draw Enemy
            if (attackState != AttackState.none && attackState != AttackState.summonHands && !isStunned)
            {
                if (facingRight)
                    s.Draw(game.EnemySpriteSheets[name + "Attacking"], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                else
                    s.Draw(game.EnemySpriteSheets[name + "Attacking"], new Rectangle(rec.X - 435, rec.Y, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                if (facingRight)
                    s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                else
                    s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X - 435, rec.Y, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 67;

                healthBarRec.Y = vitalRec.Y - 65;

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

                if (facingRight)
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) - 220, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) - 220, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

            // if (enemyState == EnemyState.attacking)
            //s.Draw(Game1.emptyBox, attackRec, Color.Blue);

            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

        }

        public void MoveTowardPlayer(int mapWidth)
        {
            if (chasePlayerDelay <= 0)
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

                    if (moveFrame > 13)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;

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

                    if (moveFrame > 13)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;

                    if (position.X <= mapWidth - 6)
                        position.X += enemySpeed;
                }
            }
            else
            {
                chasePlayerDelay--;

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

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            //--If the monster is not respawning
            if (respawning == false && !knockedBack)
            {
                #region Runs into player
                if (player.CheckIfHit(vitalRec))
                {
                    if (player.InvincibleTime <= 0)
                    {
                        MyGamePad.SetRumble(3, .3f);
                    }

                    //--If the player is standing to the left of the enemy, make the knockback.X direction negative so he goes left
                    if (player.Position.X + (player.PlayerRec.Width / 2) < (int)(position.X + (rec.Width / 2)))
                        knockback.X = -(knockback.X);

                    //--Otherwise, bounce to the right and keep the knockback.X positive
                    else if (player.Position.X + (player.PlayerRec.Width / 2) > (int)(position.X + (rec.Width / 2)))
                        knockback.X = Math.Abs(knockback.X);

                    //--Take damage and knock the player back
                    player.TakeDamage(damage, level);
                    player.KnockPlayerBack(knockback);


                }
                #endregion
            }
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (Math.Abs(kbvel.Y) < 10 && Math.Abs(kbvel.X) < 10)
                kbvel = Vector2.Zero;
            else
                kbvel *= .75f;
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
                //currentMap.Drops.Add(new EnemyDrop("Acorn Sack", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
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

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 20 - 30, rec.Width, 20);
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


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + 30;
                    velocity.Y = 0;

                    //--Once it collides with the ground, set the moveTimer to 0 and the boolean to false
                    //--This will make the monster start moving again
                    if (knockedBack == true)
                    {
                        moveTimer = 0;
                        currentlyInMoveState = false;

                        if (!summoningLocust && !dashing && enemyState != EnemyState.attacking)
                        {
                            int ran = Game1.randomNumberGen.Next(10);

                            if (ran == 2)
                            {
                                moveFrame = 0;
                                frameDelay = 10;
                                dashType = 0;
                                dashing = true;

                                String soundEffectName = "enemy_anubis_dash";
                                Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);
                            }
                            else if (ran == 1)
                            {
                                moveFrame = 0;
                                frameDelay = 10;
                                dashType = 1;
                                dashing = true;

                                String soundEffectName = "enemy_anubis_dash";
                                Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);
                            }
                            else if (ran == 3)
                            {
                                moveFrame = 0;
                                frameDelay = 10;
                                dashType = 2;
                                dashing = true;

                                String soundEffectName = "enemy_anubis_dash";
                                Sound.PlaySoundInstance(AnubisWarrior.anubisSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 1500);
                            }
                        }
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
                if (position.X < currentPlat.Rec.X - 200)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X - 200;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + 450)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + 450;
                }
            }
            #endregion

        }
    }
}
