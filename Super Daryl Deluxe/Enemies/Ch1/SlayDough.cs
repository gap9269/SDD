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
    public class SlayDough : Enemy
    {
        int meleeDamage = 42;
        int vaseDamage = 35;
        int rollDamage = 25;
        int meleeRange = 250;

        int meleeWeight = 50;

        int maxVaseTimer = 60;

        int standTimer;
        int fallTimer;
        int vaseTimer;
        int standState;

        Platform lastCurrentPlat;

        //--State Machine
        new public enum EnemyState
        {
            none,
            standing,
            morphingToBall,
            morphingToVase,
            brokenVase,
            moving,
            stopping,
            attacking
        }
        new public EnemyState enemyState;

        public enum AttackState
        {
            none,
            melee,
            vase,
            roll,
        }
        public AttackState attackState;

        public SlayDough(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 345;
            maxHealth = 345;
            level = 8;
            experienceGiven = 35;
            rec = new Rectangle((int)position.X, (int)position.Y, 567, 310);
            currentlyInMoveState = false;
            enemySpeed = 7;
            tolerance = 25;
            vitalRec = new Rectangle(rec.X, rec.Y, 120, 110);
            maxHealthDrop = 8;
            moneyToDrop = 0.18f;

            rectanglePaddingLeftRight = 250;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if(isStunned || knockedBack && enemyState != EnemyState.attacking)
                return new Rectangle(3402, 0, 567, 310);
            switch(enemyState)
            {
                case EnemyState.standing:
                    return new Rectangle(moveFrame * 567, 0, 567, 310);
                case EnemyState.morphingToBall:
                    return new Rectangle(moveFrame * 567, 1860, 567, 310);
                case EnemyState.moving:
                    return new Rectangle(moveFrame * 567, 2170, 567, 310);
                case EnemyState.stopping:
                    return new Rectangle(moveFrame * 567, 2480, 567, 310);
                case EnemyState.morphingToVase:
                    return new Rectangle(moveFrame * 567, 930, 567, 310);
                case EnemyState.brokenVase:
                    if(moveFrame < 5)
                        return new Rectangle((moveFrame * 567) + 1134, 1240, 567, 310);
                    else
                        return new Rectangle((moveFrame - 5) * 567, 1550, 567, 310);

                case EnemyState.attacking:
                    if (attackState == AttackState.melee)
                    {
                        if (attackFrame < 7)
                            return new Rectangle(attackFrame * 567, 310, 567, 310);
                        else
                            return new Rectangle((attackFrame - 7) * 567, 620, 567, 310);
                    }
                    else
                        if(vaseTimer > 0)
                            return new Rectangle(0, 1240, 567, 310);
                        else
                            return new Rectangle(567, 1240, 567, 310);
            }
            return new Rectangle(0, 0, 567, 310);
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

            vitalRec.Y = rec.Y + 155;

            if (!facingRight)
                vitalRec.X = rec.X + 225;
            else
                vitalRec.X = rec.X + 220;

            deathRec = vitalRec;

            if (currentPlat != null)
                lastCurrentPlat = currentPlat;
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

                if (enemyState == EnemyState.brokenVase)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;

                        frameDelay = 5;
                    }

                    if (moveFrame > 10)
                    {
                        moveFrame = 0;
                        enemyState = EnemyState.none;
                        frameDelay = 5;
                    }
                }
                else if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && !knockedBack && enemyState != EnemyState.attacking && enemyState != EnemyState.morphingToVase && enemyState != EnemyState.brokenVase)
                {
                    if (currentlyInMoveState == false)
                    {
                        moveState = moveNum.Next(0, 3);
                        moveTimer = moveTime.Next(60, 200);
                        moveFrame = 0;

                        switch (moveState)
                        {
                            case 0:
                                enemyState = EnemyState.standing;
                                break;
                            case 1:
                                enemyState = EnemyState.morphingToBall;
                                facingRight = true;
                                break;
                            case 2:
                                enemyState = EnemyState.morphingToBall;
                                facingRight = false;
                                break;
                        }
                    }

                    switch (enemyState)
                    {
                        case EnemyState.standing:
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

                        case EnemyState.moving:
                            enemyState = EnemyState.moving;
                            if (currentlyInMoveState == false)
                                moveFrame = 0;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 4;
                            }

                            if (moveFrame > 3)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (facingRight)
                            {
                                if (position.X + 380 <= mapWidth - 6)
                                    position.X += enemySpeed;
                            }
                            else
                            {
                                if (position.X + 170 >= 6)
                                    position.X -= enemySpeed;
                            }
                            break;

                        case EnemyState.morphingToBall:
                            enemyState = EnemyState.morphingToBall;

                            if (currentlyInMoveState == false)
                                moveFrame = 0;

                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 4;
                            }

                            if (moveFrame > 5)
                            {
                                moveFrame = 0;
                                enemyState = EnemyState.moving;
                            }

                            currentlyInMoveState = true;
                            break;
                        case EnemyState.stopping:
                            enemyState = EnemyState.stopping;
                            currentlyInMoveState = true;

                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 3;
                            }

                            if (moveFrame > 4)
                            {
                                enemyState = EnemyState.standing;
                                currentlyInMoveState = false;
                                moveTimer = -1;
                            }
                            break;
                    }

                    if (moveTimer <= 0)
                    {
                        if (enemyState != EnemyState.moving)
                            currentlyInMoveState = false;
                        else
                        {
                            moveTimer = 1;
                            moveFrame = 0;
                            frameDelay = 5;
                            enemyState = EnemyState.stopping;
                        }
                    }
                }
                else if (enemyState != EnemyState.attacking)
                {
                    if (attackState == AttackState.none)
                        attackState = AttackState.melee;
                    switch (attackState)
                    {
                        case AttackState.melee:
                            #region If the player is too far away, move closer
                            if (enemyState == EnemyState.morphingToBall)
                            {
                                enemyState = EnemyState.morphingToBall;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 4;
                                }

                                if (moveFrame > 5)
                                {
                                    moveFrame = 0;
                                    enemyState = EnemyState.moving;
                                }
                            }
                            else if (enemyState == EnemyState.stopping)
                            {
                                enemyState = EnemyState.stopping;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 3;
                                }

                                if (moveFrame > 4)
                                {
                                    enemyState = EnemyState.standing;
                                    moveFrame = 0;
                                    frameDelay = 5;

                                    if (distanceFromPlayer <= meleeDamage && attackFrame <= 0 && !knockedBack)
                                    {
                                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                                        {
                                            facingRight = false;
                                            attackRec = new Rectangle(vitalRec.Center.X - 250, vitalRec.Y, 250, 200);
                                        }
                                        else
                                        {
                                            facingRight = true;
                                            attackRec = new Rectangle(vitalRec.Center.X, vitalRec.Y, 250, 200);
                                        }
                                        attackFrame = 0;
                                        frameDelay = 5;

                                        enemyState = EnemyState.attacking;
                                    }
                                }
                            }
                            else if ((horizontalDistanceToPlayer > meleeRange || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
                            {
                                if (horizontalDistanceToPlayer > meleeRange)
                                {
                                    if (enemyState == EnemyState.moving)
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

                                            if (moveFrame > 3)
                                                moveFrame = 0;

                                            currentlyInMoveState = true;
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

                                            if (moveFrame > 3)
                                                moveFrame = 0;

                                            currentlyInMoveState = true;
                                            if (position.X + 380 <= mapWidth - 6)
                                                position.X += enemySpeed;
                                        }
                                    }
                                    else if (enemyState != EnemyState.stopping)
                                    {
                                        enemyState = EnemyState.morphingToBall;
                                        moveFrame = 0;
                                        frameDelay = 5;
                                    }

                                }
                                else
                                {
                                    if (enemyState == EnemyState.moving)
                                    {
                                        enemyState = EnemyState.stopping;
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
                            }
                            #endregion

                            #region Attack once it is close enough
                            else if (enemyState != EnemyState.attacking)
                            {
                                if (enemyState == EnemyState.moving)
                                {
                                    enemyState = EnemyState.stopping;
                                }
                                else
                                {
                                    //--Only attack if off cooldown
                                    if (attackCooldown <= 0)
                                    {
                                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                                        {
                                            facingRight = false;
                                            attackRec = new Rectangle(vitalRec.Center.X - 250, rec.Y, 250, 250);
                                        }
                                        else
                                        {
                                            facingRight = true;
                                            attackRec = new Rectangle(vitalRec.Center.X, rec.Y, 250, 250);
                                        }
                                        attackFrame = 0;
                                        frameDelay = 10;

                                        enemyState = EnemyState.attacking;
                                    }
                                }
                            }
                            #endregion
                            break;
                        case AttackState.vase:
                            #region If the player is too far away, move closer
                            if (enemyState == EnemyState.morphingToBall)
                            {
                                enemyState = EnemyState.morphingToBall;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 4;
                                }

                                if (moveFrame > 5)
                                {
                                    moveFrame = 0;
                                    enemyState = EnemyState.moving;
                                }
                            }
                            else if (enemyState == EnemyState.stopping)
                            {
                                enemyState = EnemyState.stopping;

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 3;
                                }

                                if (moveFrame > 4)
                                {
                                    enemyState = EnemyState.standing;
                                    moveFrame = 0;
                                    frameDelay = 5;

                                    if (distanceFromPlayer <= meleeDamage && attackFrame <= 0 && !knockedBack)
                                    {
                                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                                        {
                                            facingRight = false;
                                            attackRec = new Rectangle(vitalRec.Center.X - 250, vitalRec.Y, 250, 200);
                                        }
                                        else
                                        {
                                            facingRight = true;
                                            attackRec = new Rectangle(vitalRec.Center.X, vitalRec.Y, 250, 200);
                                        }
                                        attackFrame = 0;
                                        frameDelay = 5;

                                        enemyState = EnemyState.attacking;
                                    }
                                }
                            }
                            else if ((horizontalDistanceToPlayer > meleeRange * 2 || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking && enemyState != EnemyState.morphingToVase)
                            {
                                if (horizontalDistanceToPlayer > meleeRange * 2)
                                {
                                    if (enemyState == EnemyState.moving)
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

                                            if (moveFrame > 3)
                                                moveFrame = 0;

                                            currentlyInMoveState = true;
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

                                            if (moveFrame > 3)
                                                moveFrame = 0;

                                            currentlyInMoveState = true;
                                            if (position.X + 380 <= mapWidth - 6)
                                                position.X += enemySpeed;
                                        }
                                    }
                                    else if (enemyState != EnemyState.stopping)
                                    {
                                        enemyState = EnemyState.morphingToBall;
                                        moveFrame = 0;
                                        frameDelay = 5;
                                    }

                                }
                                else
                                {
                                    if (enemyState == EnemyState.moving)
                                    {
                                        enemyState = EnemyState.stopping;
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
                            }
                            #endregion
                            else
                            {
                                if (enemyState != EnemyState.morphingToVase)
                                {
                                    moveFrame = 0;
                                    frameDelay = 5;
                                    enemyState = EnemyState.morphingToVase;
                                }

                                frameDelay--;
                                if (frameDelay == 0)
                                {
                                    moveFrame++;
                                    frameDelay = 5;
                                }

                                if (moveFrame > 6)
                                {
                                    enemyState = EnemyState.attacking;
                                    vaseTimer = maxVaseTimer;
                                    currentPlat = null;
                                    canBeKnockbacked = false;
                                    canBeStunned = false;
                                }
                            }

                            break;
                    }
                }
                else
                    Attack(0, new Vector2());

                if (enemyState == EnemyState.none)
                {
                    int nextMove = Game1.randomNumberGen.Next(1, 101);

                    if (nextMove <= meleeWeight)
                        attackState = AttackState.melee;
                    else
                        attackState = AttackState.vase;

                    enemyState = EnemyState.standing;
                    moveFrame = 0;
                    frameDelay = 5;
                }
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);

            enemyState = EnemyState.attacking;
            frameDelay--;

            //Don't let the enemy melee if it will fall off a platform or the map
            if (attackState == AttackState.melee)
            {
                Platform testPlat;

                if (currentPlat != null)
                    testPlat = currentPlat;
                else
                    testPlat = lastCurrentPlat;

                if (position.X + 178 + rec.Width > testPlat.Rec.X + testPlat.Rec.Width + 225 || position.X - 178 < testPlat.Rec.X - 200)
                {
                    attackState = AttackState.vase;
                    moveFrame = 0;
                    frameDelay = 5;
                    enemyState = EnemyState.morphingToVase;
                }
                else
                {
                    Rectangle checkPosRec;

                    if (facingRight)
                    {
                        checkPosRec = new Rectangle(vitalRec.X, rec.Y, 178 + VitalRecWidth, 80);
                    }
                    else
                        checkPosRec = new Rectangle(vitalRec.X - 178, rec.Y, 178 + VitalRecWidth, 80);

                    foreach (Platform p in currentMap.Platforms)
                    {
                        if (checkPosRec.Intersects(p.Rec) && p.Passable == false)
                        {
                            attackState = AttackState.vase;
                            moveFrame = 0;
                            frameDelay = 5;
                            enemyState = EnemyState.morphingToVase;
                            break;
                        }
                    }
                }
            }

            switch (attackState)
            {
                case AttackState.melee:
                    if (attackFrame == 0)
                        attackFrame = 1;

                    if (frameDelay == 0)
                    {
                        attackFrame++;

                        frameDelay = 5;
                    }

                    //Hit the player
                    if (attackFrame == 6 || attackFrame == 7)
                    {
                        if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                        {
                            if (facingRight)
                                kb = new Vector2(15, -8);
                            else
                                kb = new Vector2(-15, -8);

                            player.TakeDamage(meleeDamage, level);
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
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AttackState.none;
                        enemyState = EnemyState.none;

                        if (facingRight)
                            PositionX += 178;
                        else
                            PositionX -= 178;

                        UpdateRectangles();
                    }
                    break;

                case AttackState.vase:

                    facingRight = false;
                    if (vaseTimer > 0)
                    {
                        int playerMoveOffset = 0;
                        if (player.playerState == Player.PlayerState.running)
                        {
                            if (player.FacingRight)
                                playerMoveOffset = 90;
                            else
                                playerMoveOffset = -90;
                        }

                        vaseTimer--;
                        Vector2 toPlayer = Vector2.Subtract(new Vector2(player.VitalRec.Center.X + playerMoveOffset, player.VitalRec.Center.Y - 200), vitalRec.Center.ToVector2());
                        toPlayer.Normalize();

                        if (toPlayer.X == 0)
                            toPlayer.X = .001f;
                        if (toPlayer.Y == 0)
                            toPlayer.Y = .001f;
                        if (!(toPlayer.X == .001f && toPlayer.Y == .001f))
                        {
                            float dis = Vector2.Distance(new Vector2(player.VitalRec.Center.X + playerMoveOffset, player.VitalRec.Center.Y - 200), vitalRec.Center.ToVector2());
                            if (dis != 0)
                                velocity = toPlayer * (dis * .05f);
                        }

                        if (vaseTimer <= 0)
                            VelocityY = 5;
                    }
                    else if(enemyState != EnemyState.morphingToVase)
                    {
                        velocity.X = 0;
                        CheckWalkCollisions(vaseDamage, new Vector2(5, -5));
                    }

                    break;
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

        //--Check to see if the player is colliding with the enemy
        //--Takes in a damage amount and the amount of knockback
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

                        player.TakeDamage(damage, level);
                        player.KnockPlayerBack(knockback);
                        hitPauseTimer = 3;
                        player.HitPauseTimer = 3;
                        game.Camera.ShakeCamera(2, 2);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                    }
                }
                #endregion
            }
        }

        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = 80;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
            attackState = AttackState.none;
            enemyState = EnemyState.none;
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
            {
                if (enemyState == EnemyState.attacking && attackState == AttackState.vase && vaseTimer > 0) { }
                else
                    velocity.Y += GameConstants.GRAVITY;
            }
            position += velocity;

            if (velocity.Y > 20)
                velocity.Y = 20;

            if (enemyState == EnemyState.attacking && attackState == AttackState.vase && vaseTimer > 0) { }
            else
            {
                Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 50, rec.Width, 20);
                Rectangle vitalRecFeet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20 - distanceFromFeetToBottomOfRectangle - distanceFromFeetToBottomOfRectangleRandomOffset, vitalRec.Width, 20);
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

                    if (enemyState == EnemyState.attacking && attackState == AttackState.vase && fallTimer > 0) { }
                    else
                    {
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
                    }

                    //--If you jump up into a nonpassable wall, push him back down

                    #region Landing on a platform
                    //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                    if (((feet.Intersects(top) && !knockedBack) || (vitalRecFeet.Intersects(top) && knockedBack) || new Rectangle(feet.X, feet.Y, feet.Width, (int)velocity.Y).Intersects(top)) && velocity.Y > 0)

                    {
                        if (enemyState == EnemyState.attacking && attackState == AttackState.vase && fallTimer <= 0)
                        {
                            if (currentMap.Platforms[i].RecWidth > 50)
                            {
                                enemyState = EnemyState.brokenVase;
                                moveFrame = 0;
                                frameDelay = 5;
                                fallTimer = 0;
                                attackFrame = 0;
                                attackCooldown = 80;
                                currentlyInMoveState = false;
                                attackRec = new Rectangle(0, 0, 0, 0);
                                attackState = AttackState.none;
                                canBeKnockbacked = true;
                                canBeStunned = true;

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
                                }
                                if (velocity.X == 0)
                                    knockedBack = false;
                            }
                        }
                        else
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
                            }
                            if (velocity.X == 0)
                                knockedBack = false;
                        }

                    }
                    #endregion

                    if (enemyState == EnemyState.attacking && attackState == AttackState.vase && fallTimer > 0) { }
                    else
                    {
                        //hit their head on non-passables
                        if (topEn.Intersects(bottom) && velocity.Y < 0 && plat.Passable == false)
                        {
                            velocity.Y = 0;
                            velocity.Y = GameConstants.GRAVITY;
                        }
                    }
                }

                if (enemyState == EnemyState.attacking && attackState == AttackState.vase && fallTimer > 0) { }
                else
                {
                    #region Not falling off a platform
                    //--Don't fall off the platform you're on!
                    if (currentPlat != null)
                    {

                        if (position.X < currentPlat.Rec.X - 200)
                        {
                            velocity.X = 0;
                            position.X = currentPlat.Rec.X - 200;
                        }
                        if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + 225)
                        {
                            velocity.X = 0;
                            position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + 225;
                        }

                    }
                    #endregion
                }
            }

        }

        public override void Draw(SpriteBatch s)
        {

            if (enemyState != EnemyState.none)
            {

                if (facingRight)
                    s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                else
                    s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                #region Health Bar
                if (health < maxHealth)
                {
                    healthBoxRec.Y = vitalRec.Y - 45;
                    healthBarRec.Y = vitalRec.Y - 43;
                    if (vaseTimer > 0 || fallTimer > 0)
                    {
                        healthBoxRec.Y = vitalRec.Y - 105;
                        healthBarRec.Y = vitalRec.Y - 103;
                    }

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
                        Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                    }
                    else
                    {
                        Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                    }
                }
                #endregion

                 //s.Draw(Game1.whiteFilter, attackRec, Color.Black * .5f);

            }
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
                currentMap.Drops.Add(new EnemyDrop("Clay Dough", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
