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
    public class TubaGhost : Enemy
    {

        int meleeRange = 225;
        int meleeDamage = 40;

        int altAttackCooldown = 200;
        public static Dictionary<String, SoundEffect> tubaSounds;

        SoundEffectInstance currentWalkingSound, currentAttackingSound;

        public TubaGhost(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 200;
            maxHealth = 200;
            level = 6;
            experienceGiven = 18;
            rec = new Rectangle((int)position.X, (int)position.Y, 586, 353);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 15;
            maxHealthDrop = 5;
            moneyToDrop = .09f;
            vitalRec = new Rectangle(100, 100, 150, 170);

            rectanglePaddingLeftRight = 170;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(586, 353, 586, 353);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        if (frame < 7)
                            return new Rectangle(frame * 586, 1412, 586, 353);
                        else
                            return new Rectangle((frame - 7) * 586, 1765, 586, 353);

                    case EnemyState.moving:
                        if (frame < 7)
                            return new Rectangle(frame * 586, 0, 586, 353);
                        else
                            return new Rectangle((frame - 7) * 586, 353, 586, 353);

                    case EnemyState.attacking:
                        if(attackFrame < 4)
                            return new Rectangle((attackFrame * 586) + 1172, 353, 586, 353);
                        else if(attackFrame < 10)
                            return new Rectangle((attackFrame - 4) * 586, 706, 586, 353);
                        else
                            return new Rectangle((attackFrame - 10) * 586, 1059, 586, 353);
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

        public void StartWalkSound()
        {
            if (currentWalkingSound == null)
            {
                int randomSound = Game1.randomNumberGen.Next(1, 14);
                String soundEffectName;
                if(randomSound < 10)
                    soundEffectName = "enemy_tuba_ghost_walk_0" + randomSound;
                else
                    soundEffectName = "enemy_tuba_ghost_walk_" + randomSound;

                currentWalkingSound = TubaGhost.tubaSounds[soundEffectName].CreateInstance();

                Sound.PlaySoundInstance(currentWalkingSound, soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 0, 1500, true, 4);
            }
        }

        public void CheckStopWalkSound()
        {
            if (currentWalkingSound != null && enemyState != EnemyState.moving)
            {
                currentWalkingSound.Stop();
                currentWalkingSound = null;
            }
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
                    if (objectToAttack != null)
                        MoveWithAttackableObjectInMap(mapwidth);
                    else
                        Move(mapwidth);
                }
            }


            CheckStopWalkSound();
            if (currentWalkingSound != null && currentWalkingSound.State == SoundState.Stopped)
            {
                currentWalkingSound = null;
            }
            if (currentAttackingSound != null && currentAttackingSound.State == SoundState.Stopped)
            {
                currentAttackingSound = null;
            }

            if (facingRight)
                vitalRec.X = rec.X + 210;
            else
                vitalRec.X = rec.X + 230;
            vitalRec.Y = rec.Y + 150;
            deathRec = vitalRec;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region Random movement
            if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(50, 200);
                    moveFrame = 0;

                    if(moveState == 0)
                        moveTimer = moveTime.Next(120, 200);

                }

                switch (moveState)
                {
                    case 0:
                        enemyState = EnemyState.standing;
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            frameDelay = 8;
                        }

                        if (moveFrame > 11)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        break;

                    case 1:
                        StartWalkSound();
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

                        if (position.X <= mapWidth - 6)
                            position.X += enemySpeed;
                        break;

                    case 2:
                        StartWalkSound();
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

                        frameDelay = 8;
                    }

                    if (moveFrame > 11)
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
                        facingRight = false;
                    else
                        facingRight = true;

                    moveFrame = 0;
                    frameDelay = 5;
                    attackFrame = 0;

                    Vector2 kb;

                    if (facingRight)
                    {
                        kb = new Vector2(20, -8);
                        attackRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y - 20, 220, 100);
                    }
                    else
                    {
                        kb = new Vector2(-20, -8);
                        attackRec = new Rectangle(vitalRec.X - 220, vitalRec.Y - 20, 220, 100);

                    }
                    int randomSound = Game1.randomNumberGen.Next(1, 4);
                    String soundEffectName = soundEffectName = "enemy_tuba_ghost_attack_0" + randomSound;

                    currentAttackingSound = TubaGhost.tubaSounds[soundEffectName].CreateInstance();

                    Sound.PlaySoundInstance(currentAttackingSound, soundEffectName, false, rec.Center.X, rec.Center.Y, 0, 0, 0, true, 4);

                    Attack(meleeDamage, kb);
                }
                else if (horizontalDistanceToPlayer > meleeRange && knockedBack == false && enemyState != EnemyState.attacking)
                    MoveTowardPlayer(mapWidth);
                else if (enemyState == EnemyState.attacking)
                {

                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(20, -8);
                    else
                        kb = new Vector2(-20, -8);
                    Attack(meleeDamage, kb);
                }
            }
        }

        public void MoveTowardPlayer(int mapWidth)
        {
            StartWalkSound();

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

                if (moveFrame > 7)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
            }

        }

        public void MoveWithAttackableObjectInMap(int mapWidth)
        {
            if (altAttackCooldown > 0)
                altAttackCooldown--;

            float distanceToSucker = Math.Abs(vitalRec.Center.X - objectToAttack.VitalRec.Center.X);

            if ((!hostile || (distanceToSucker < horizontalDistanceToPlayer && horizontalDistanceToPlayer > 300) || horizontalDistanceToPlayer > 1000) && enemyState != EnemyState.attacking && !attackingOtherObject)
            {
                if (distanceToSucker > meleeRange && knockedBack == false && enemyState != EnemyState.attacking && altAttackCooldown <= 40)
                {
                    if (objectToAttack.Rec.Center.X > vitalRec.Center.X)
                    {
                        StartWalkSound();
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
                        if (position.X <= mapWidth - 6)
                            position.X += enemySpeed;
                    }
                    else
                    {
                        StartWalkSound();
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
                        if (position.X >= 6)
                            position.X -= enemySpeed;
                    }
                }
                else if (altAttackCooldown > 0 && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    enemyState = EnemyState.standing;
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 8;
                    }

                    if (moveFrame > 7)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                }
                else if (altAttackCooldown <= 0)
                {
                    attackingOtherObject = true;
                }
            }
            else if (attackingOtherObject)
                AttackOtherObject();
            else
            {
                Move(mapWidth);
            }
        }

        public void AttackOtherObject()
        {
            moveFrame = 0;

            if (objectToAttack.Rec.Center.X > vitalRec.Center.X)
                facingRight = true;
            else
                facingRight = false;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;

                frameDelay = 5;

                if (facingRight)
                {
                    attackRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y - 20, 220, 100);
                }
                else
                {
                    attackRec = new Rectangle(vitalRec.X - 220, vitalRec.Y - 20, 220, 100);
                }
                
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
            if (attackFrame == 4 && frameDelay == 4)
            {
                if (objectToAttack is GhostSucker)
                    (objectToAttack as GhostSucker).TakeHit(1, Vector2.Zero, Rectangle.Intersect(attackRec, objectToAttack.VitalRec));
            }

            //--Once it has ended, reset
            if (attackFrame > 13)
            {

                attackingOtherObject = false;
                altAttackCooldown = 200;
                attackFrame = 0;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;


            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                attackingOtherObject = false;
                attackFrame = 0;
                altAttackCooldown = 200;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
            }
        }

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
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 3 && attackFrame < 8)
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
            if (attackFrame > 13)
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

            if (currentWalkingSound != null || currentAttackingSound != null)
            {
                int randomSound = Game1.randomNumberGen.Next(1, 3);
                String soundEffectName = soundEffectName = "enemy_tuba_ghost_interrupt_0" + randomSound;
                if (currentWalkingSound != null)
                {
                    currentWalkingSound.Stop();
                    currentWalkingSound = null;
                }
                if (currentAttackingSound != null)
                {
                    currentAttackingSound.Stop();
                    currentAttackingSound = null;
                }
                Sound.PlaySoundInstance(TubaGhost.tubaSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 0, 0, 0, true, 4);
            }

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
            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 60;
                healthBarRec.Y = vitalRec.Y - 58;

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 12), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 8), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

           // s.Draw(Game1.whiteFilter, attackRec, Color.Red * .5f);

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
                currentMap.Drops.Add(new EnemyDrop("Ectoplasm", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35 && game.CurrentSideQuests.Contains(SideQuestManager.aStarIsBorn))
            {
                currentMap.Drops.Add(new EnemyDrop("Sheet Music", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }

        public void CutsceneStand()
        {
            enemyState = EnemyState.standing;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                frameDelay = 8;
            }

            if (moveFrame > 11)
                moveFrame = 0;

            currentlyInMoveState = true;
        }

    }
}
