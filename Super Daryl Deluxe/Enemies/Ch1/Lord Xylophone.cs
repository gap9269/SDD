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
    public class XylophoneKey : Projectile
    {

        Boolean exploding = false;
        Rectangle explosionRec;
        int timer;

        public XylophoneKey(int x, int y, int time, Vector2 vel, float rot, int dam, Vector2 kb, int hitPause, int shake, int spd, ProjType type, int level) 
            : base(x, y, time, vel, rot, dam, kb, hitPause, shake, spd, type, level)
        {
        }

        public override void Update()
        {
            
            if (!exploding)
            {
                timeInAir++;
                position += velocity * speed;
                rec.X = (int)position.X;
                rec.Y = (int)position.Y;

                if (timeInAir >= maxTimeInAir)
                {
                    exploding = true;
                    explosionRec = new Rectangle(rec.Center.X - 75, rec.Center.Y - 75, 150, 150);
                    Chapter.effectsManager.AddSmokePoof(explosionRec, 3);
                }

                if (Game1.Player.CheckIfHit(rec) && Game1.Player.InvincibleTime <= 0)
                {
                    exploding = true;
                    explosionRec = new Rectangle(rec.Center.X - 75, rec.Center.Y - 75, 150, 150);
                    Chapter.effectsManager.AddSmokePoof(explosionRec, 3);

                    if (velocity.X > 0)
                        knockback.X = Math.Abs(knockback.X);
                    else
                        knockback.X = -(Math.Abs(knockback.X));

                    Game1.Player.TakeDamage(damage, level);
                    Game1.Player.KnockPlayerBack(knockback);
                    Game1.Player.HitPauseTimer = hitPauseTime;
                    Game1.camera.ShakeCamera(3, cameraShake);
                    MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                }
            }
            else
            {
                if (Game1.Player.CheckIfHit(explosionRec) && Game1.Player.InvincibleTime <= 0)
                {

                    if (velocity.X > 0)
                        knockback.X = Math.Abs(knockback.X);
                    else
                        knockback.X = -(Math.Abs(knockback.X));

                    Game1.Player.TakeDamage(damage, level);
                    Game1.Player.KnockPlayerBack(knockback);
                    Game1.Player.HitPauseTimer = hitPauseTime;
                    Game1.camera.ShakeCamera(3, cameraShake);
                    MyGamePad.SetRumble(3, (float)((float)cameraShake / 100f) * 10f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                }
                timer++;
                if (timer >= 20)
                    dead = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if(!exploding)
                s.Draw(texture, rec, GetSourceRectangle(), Color.White, rotation, new Vector2(0, 12), SpriteEffects.None, 0f);
        }
        
    }

    public class LordXylophone : Enemy
    {
        int meleeDamage = 50;
        int keyExplosionDamage = 20;
        int spinCollisionDamage = 30;
        int meleeRange = 250;

        int meleeWeight = 50;
        int spinWeight = 30;
        int soloWeight = 20;
        int standWeight = 10;
        int midAirSpinWeight = 0;

        int standTimer;
        int tiredTimer;
        int spinTimer;
        int soloTimer;
        int standState;
        Boolean longRangeSolo = false;
        Boolean hardMode = false;

        int headSpinTimer;
        int maxTiredTimer = 200;
        int keySpawnRate = 50; //Smaller number = more frequent (spinTimer % keySpawnRate == 0) = new key
        int keySpawnRateMidAir = 17; //Smaller number = more frequent (spinTimer % keySpawnRate == 0) = new key
        //--State Machine
        new public enum EnemyState
        {
            none,
            standing,
            moving,
            spinMoving,
            attacking,
            headSpinning,
            jumping,
            falling,
            landing,
            catchingHead,
            tired
        }
        new public EnemyState enemyState;

        public enum AttackState
        {
            none,
            xylophoneSolo,
            melee,
            spinAcrossMap,
            spinInAir
        }
        public AttackState attackState;

        public LordXylophone(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 1345;
            maxHealth = 1345;
            level = 8;
            experienceGiven = 205;
            rec = new Rectangle((int)position.X, (int)position.Y, 514, 328);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 40;
            vitalRec = new Rectangle(rec.X, rec.Y, 514, 328);
            maxHealthDrop = 0;
            moneyToDrop = 3.55f;
            canBeStunned = false;
            canBeKnockbacked = false;

            rectanglePaddingLeftRight = 200;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 514, 328);
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (enemyState == EnemyState.tired && !canBeKnockbacked)
            {
                canBeKnockbacked = true;
            }
            else if (enemyState != EnemyState.tired && canBeKnockbacked)
            {
                canBeKnockbacked = false;
            }

            if (health <= maxHealth * .30f)
            {
                meleeWeight = 15;
                spinWeight = 30;
                soloWeight = 25;
                standWeight = 0;
                midAirSpinWeight = 30;
                maxTiredTimer = 100;

                keySpawnRate = 30;
                keySpawnRateMidAir = 12;

                if (health <= maxHealth * .1f)
                {
                    if (!game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"])
                    {
                        meleeWeight = 0;
                        spinWeight = 0;
                        soloWeight = 0;
                        standWeight = 0;
                        midAirSpinWeight = 100;
                    }

                }

            }
            else if (health <= maxHealth * .50f)
            {
                meleeWeight = 25;
                spinWeight = 25;
                soloWeight = 25;
                standWeight = 5;
                midAirSpinWeight = 20;
                maxTiredTimer = 150;

                keySpawnRate = 38;
            }
            else if (health <= maxHealth * .75f)
            {
                meleeWeight = 40;
                spinWeight = 35;
                soloWeight = 25;
                standWeight = 10;
                midAirSpinWeight = 0;
                keySpawnRate = 45;
            }

            if (!respawning && !isStunned)
            {
                attackCooldown--;

                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }

            vitalRec.Y = rec.Y + 60;

            vitalRec.Width = 150;
            vitalRec.Height = 240;

            if (!facingRight)
                vitalRec.X = rec.X + 180;
            else
                vitalRec.X = rec.X + 180;

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

                if (standTimer > 0)
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

                    standTimer--;

                    if (standTimer <= 0)
                        enemyState = EnemyState.none;

                }
                else if (enemyState == EnemyState.jumping)
                {
                    if (moveFrame < 3)
                        frameDelay--;

                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 3;
                    }

                    if (VelocityY > -20 && moveFrame == 3)
                    {
                        VelocityY = -20;
                    }

                    if (vitalRec.Center.Y <= 200)
                    {
                        VelocityY = 0;
                        enemyState = EnemyState.attacking;
                        attackFrame = 0;
                        frameDelay = 10;
                        spinTimer = 300;
                    }
                }
                else if (enemyState == EnemyState.headSpinning)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 3;
                    }

                    if (moveFrame > 13)
                        moveFrame = 2;

                    headSpinTimer--;

                    if (headSpinTimer <= 0)
                    {
                        enemyState = EnemyState.catchingHead;
                        frameDelay = 5;
                        moveFrame = 0;
                    }
                }
                else if (enemyState == EnemyState.catchingHead)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 2)
                    {
                        tiredTimer = maxTiredTimer;
                        enemyState = EnemyState.tired;
                        frameDelay = 5;
                        moveFrame = 0;
                    }
                }
                else if (enemyState == EnemyState.falling)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 3;
                    }

                    if (moveFrame > 7)
                    {
                        frameDelay = 5;
                        moveFrame = 2;
                    }
                }
                else if (enemyState == EnemyState.landing)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 13)
                    {
                        frameDelay = 5;
                        moveFrame = 0;
                        enemyState = EnemyState.headSpinning;
                        headSpinTimer = 25;
                    }
                }
                else if (enemyState == EnemyState.tired)
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 12)
                        moveFrame = 1;

                    tiredTimer--;

                    if (tiredTimer <= 0)
                        enemyState = EnemyState.none;
                }
                else if (enemyState != EnemyState.attacking)
                {
                    switch (attackState)
                    {
                        case AttackState.melee:
                            #region If the player is too far away, move closer
                            if ((distanceFromPlayer > meleeRange || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
                            {
                                if (distanceFromPlayer > meleeRange)
                                {
                                    if (hardMode)
                                    {
                                        //--If the player is to the left
                                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                                        {
                                            facingRight = false;
                                            enemyState = EnemyState.spinMoving;
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
                                                position.X -= 20;
                                        }
                                        //Player to the right
                                        else
                                        {
                                            facingRight = true;
                                            enemyState = EnemyState.spinMoving;
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
                                                position.X += 20;
                                        }
                                    }
                                    else
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

                                            if (moveFrame > 9)
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

                                            if (moveFrame > 9)
                                                moveFrame = 0;

                                            currentlyInMoveState = true;
                                            if (position.X <= mapWidth - 6)
                                                position.X += enemySpeed;
                                        }
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
                                if (attackCooldown <= 0)
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
                                    frameDelay = 10;

                                    enemyState = EnemyState.attacking;
                                }
                            }
                            #endregion
                            break;
                        case AttackState.spinAcrossMap:
                            if (knockedBack == false)
                            {
                                attackFrame = 0;
                                frameDelay = 5;
                                enemyState = EnemyState.attacking;
                                spinTimer = 500;
                                if (facingRight)
                                    VelocityX = 1;
                                else
                                    VelocityX = -1;
                            }
                            break;
                        case AttackState.xylophoneSolo:
                            if (knockedBack == false)
                            {
                                attackFrame = 0;
                                frameDelay = 5;
                                enemyState = EnemyState.attacking;
                                soloTimer = 300;

                                if (player.VitalRec.Center.X < vitalRec.Center.X)
                                {
                                    facingRight = false;
                                }
                                else
                                {
                                    facingRight = true;
                                }

                                if (horizontalDistanceToPlayer > 400 && Game1.randomNumberGen.Next(4) == 2)
                                    longRangeSolo = true;
                                else
                                    longRangeSolo = false;
                            }
                            break;
                        case AttackState.spinInAir:
                            if (knockedBack == false)
                            {
                                #region If the player is too far away, move closer
                                if (Math.Abs(vitalRec.Center.X - currentMap.MapWidth / 2) > 10)
                                {
                                    frameDelay--;

                                    if (frameDelay == 0)
                                    {
                                        moveFrame++;
                                        frameDelay = 5;
                                    }

                                    if (moveFrame > 7)
                                        moveFrame = 0;

                                    CheckWalkCollisions(spinCollisionDamage, new Vector2(VelocityX * 1.5f, -10));


                                    enemyState = EnemyState.spinMoving;
                                    if (vitalRec.Center.X > currentMap.MapWidth / 2)
                                    {
                                        facingRight = false;
                                    }
                                    else
                                    {
                                        facingRight = true;
                                    }

                                    if (facingRight)
                                    {
                                        if (VelocityX < 20)
                                            VelocityX += 1f;
                                    }
                                    else
                                    {
                                        if (VelocityX > -20)
                                            VelocityX -= 1f;
                                    }
                                }
                                else
                                {
                                    VelocityX = 0;

                                    enemyState = EnemyState.jumping;
                                    frameDelay = 5;
                                    moveFrame = 0;

                                }
                                #endregion
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
                    else if (nextMove <= (meleeWeight + spinWeight))
                        attackState = AttackState.spinAcrossMap;
                    else if (nextMove <= (meleeWeight + spinWeight + soloWeight))
                        attackState = AttackState.xylophoneSolo;
                    else if (nextMove <= (meleeWeight + spinWeight + soloWeight + standWeight))
                    {
                        attackState = AttackState.none;
                        standTimer = Game1.randomNumberGen.Next(30, 120);
                        standState = Game1.randomNumberGen.Next(0, 2);
                    }
                    else if (nextMove <= (meleeWeight + spinWeight + soloWeight + standWeight + midAirSpinWeight))
                        attackState = AttackState.spinInAir;
                    else
                    {
                        attackState = AttackState.none;
                        standTimer = Game1.randomNumberGen.Next(30, 120);
                        standState = Game1.randomNumberGen.Next(0, 2);

                    }

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

            switch (attackState)
            {
                case AttackState.melee:
                    if (frameDelay == 0)
                    {
                        attackFrame++;

                        frameDelay = 5;

                        if (attackFrame < 4)
                            frameDelay = 4;

                    }

                    //Hit the player
                    if (attackFrame > 4)
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
                    if (attackFrame > 8)
                    {
                        attackFrame = 0;
                        attackCooldown = 80;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AttackState.none;
                        enemyState = EnemyState.none;
                    }
                    break;
                case AttackState.spinAcrossMap:
                    if (frameDelay == 0)
                    {
                        attackFrame++;

                        frameDelay = 5;
                    }

                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        attackFrame++;
                        frameDelay = 5;
                    }

                    if (attackFrame > 7)
                        attackFrame = 0;

                    CheckWalkCollisions(spinCollisionDamage, new Vector2(VelocityX * 1.5f, -10));

                    //Spit out keys
                    if (hardMode)
                    {
                        if (spinTimer % keySpawnRate == 0 && Math.Abs(VelocityX) >= 15)
                        {
                            Vector2 v = new Vector2((float)Game1.randomNumberGen.NextDouble() * 4 - 2, -1);

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y, 10, v, rot, keyExplosionDamage, new Vector2(30, -10), 1, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }
                    else
                    {
                        if (spinTimer % keySpawnRate == 0 && Math.Abs(VelocityX) >= 15)
                        {
                            Vector2 v = new Vector2((float)Game1.randomNumberGen.NextDouble() * 2 - 1, (float)-Game1.randomNumberGen.NextDouble());
                            v.Normalize();

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y, 20, v, rot, keyExplosionDamage, new Vector2(30, -10), 1, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }
                    spinTimer--;

                    if (spinTimer > 60)
                    {
                        if (facingRight)
                        {
                            if (VelocityX < 25)
                                VelocityX += .2f;
                        }
                        else
                        {
                            if (VelocityX > -25)
                                VelocityX -= .2f;
                        }
                    }
                    else
                    {
                        if (VelocityX > 0)
                        {
                            VelocityX -= .5f;

                            if (VelocityX <= 0)
                                spinTimer = 0;
                        }
                        else
                        {
                            VelocityX += .5f;

                            if (VelocityX >= 0)
                                spinTimer = 0;
                        }
                    }


                    //--Once it has ended, reset
                    if (spinTimer <= 0)
                    {
                        VelocityX = 0;
                        headSpinTimer = 28;
                        attackFrame = 0;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AttackState.none;
                        enemyState = EnemyState.headSpinning;
                    }
                    break;

                case AttackState.xylophoneSolo:
                    if (frameDelay == 0)
                    {
                        attackFrame++;

                        frameDelay = 6;
                    }

                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        attackFrame++;
                        frameDelay = 5;
                    }

                    if (attackFrame > 23)
                        attackFrame = 3;

                    //Spit out keys
                    if (longRangeSolo == false)
                    {
                        if (soloTimer % 10 == 0)
                        {
                            Vector2 v;

                            if (facingRight)
                                v = new Vector2(1, 0);
                            else
                                v = new Vector2(-1, 0);

                            v.Normalize();

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y + Game1.randomNumberGen.Next(-50, 50), Game1.randomNumberGen.Next(10, 20), v, rot, keyExplosionDamage, new Vector2(30, -10), 1, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }
                    else
                    {
                        if (soloTimer % 55 == 0)
                        {
                            Vector2 v;

                            if (facingRight)
                                v = new Vector2(1, 0);
                            else
                                v = new Vector2(-1, 0);

                            v.Normalize();

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y + Game1.randomNumberGen.Next(-50, 50), 45, v, rot, keyExplosionDamage, new Vector2(30, -10), 1, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }

                    soloTimer--;

                    //--Once it has ended, reset
                    if (soloTimer <= 0)
                    {
                        VelocityX = 0;
                        standTimer = 120;
                        standState = Game1.randomNumberGen.Next(0, 2);
                        attackFrame = 0;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AttackState.none;
                        enemyState = EnemyState.tired;
                    }
                    break;
                case AttackState.spinInAir:
                    if (frameDelay == 0)
                    {
                        attackFrame++;

                        frameDelay = 5;
                    }

                    if (vitalRec.Center.X < player.VitalRec.Center.X)
                        kb = new Vector2(30, -8);
                    else
                        kb = new Vector2(-30, -8);

                    CheckWalkCollisions(spinCollisionDamage, kb);

                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        attackFrame++;
                        frameDelay = 5;
                    }

                    if (attackFrame > 7)
                        attackFrame = 0;

                    //Spit out keys
                    if (hardMode)
                    {
                        if (spinTimer % keySpawnRateMidAir == 0)
                        {
                            Vector2 v = new Vector2((float)Game1.randomNumberGen.NextDouble() * 4 - 2, 1);

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y, 20, v, rot, 1, new Vector2(30, -10), keyExplosionDamage, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }
                    else
                    {
                        if (spinTimer % keySpawnRateMidAir == 0)
                        {
                            Vector2 v = new Vector2((float)Game1.randomNumberGen.NextDouble() * 4 - 2, (float)Game1.randomNumberGen.NextDouble());
                            v.Normalize();

                            float rot = (float)Math.Atan2(v.Y, v.X);

                            XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X, (int)vitalRec.Center.Y, 15 + (int)(Math.Abs(v.X * 25f)), v, rot, keyExplosionDamage, new Vector2(30, -10), 1, 0, 25, Projectile.ProjType.xylophoneKey, level);

                            currentMap.Projectiles.Add(arrow);
                        }
                    }

                    spinTimer--;

                    //--Once it has ended, reset
                    if (spinTimer <= 0)
                    {
                        VelocityY = GameConstants.GRAVITY;
                        attackFrame = 0;
                        moveFrame = 0;
                        attackRec = new Rectangle(0, 0, 0, 0);
                        attackState = AttackState.none;
                        enemyState = EnemyState.falling;
                    }
                    break;
            }
            
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (respawning == false)
            {
                if (knockedBack && VelocityY > 0)
                    velocity.Y = 0;

                weaknessStrengthOrNormal.Add("Normal");

                damage = (int)(damage * (250f / (250f + tolerance)));

                if (damage <= 0)
                    damage = 1;

                if(game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"] == false && health <= maxHealth * .1f)
                {
                    if (enemyState != EnemyState.tired || Math.Abs(vitalRec.Center.X - currentMap.mapRec.Center.X) > 100)
                        damage = 0;
                }

                PlaySoundWhenHit();

               // enemyState = EnemyState.standing;
                health -= damage;
                KnockBack(kbvel);
                knockBackVec = kbvel;

                if (knockBackVec.Y < -10)
                    hangInAir = true;

                if (hangInAir == true)
                {
                    hangInAirTime = 0;
                }

                AddDamageNum(damage, collision);
            }
        }

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
                Chapter.effectsManager.AddSmokePoof(deathRec,1);
                Sound.PlayRandomRegularPoof(deathRec.Center.X, deathRec.Center.Y);

                //Unlock enemy bio for this enemy
               // if (player.AllMonsterBios[name] == false)
               //     player.UnlockEnemyBio(name);
                if (!game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"])
                {
                    game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"] = true;
                    game.Camera.ShakeCamera(15, 5);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(905, 360, 290, 290), 3);
                }
                return true;
            }

            return false;

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
            enemyState = EnemyState.standing;
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
            else if (attackState == AttackState.spinInAir && enemyState == EnemyState.attacking)
            {
            }
            else
                velocity.Y += GameConstants.GRAVITY;

            position += velocity;

            Rectangle vitalRecFeet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20 - distanceFromFeetToBottomOfRectangle - distanceFromFeetToBottomOfRectangleRandomOffset, vitalRec.Width, 20);
            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 20, rec.Width, 20);
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
                    if (attackState != AttackState.spinAcrossMap && enemyState != EnemyState.attacking)
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
                    else if (attackState == AttackState.spinAcrossMap && enemyState == EnemyState.attacking)
                    {
                        if (rightEn.Intersects(left))
                        {
                            VelocityY = -(Math.Abs(VelocityX) / 2);
                            VelocityX = -(VelocityX * .5f);
                            PositionX -= 50;
                            facingRight = false;
                        }

                        if (leftEn.Intersects(right))
                        {
                            VelocityY = -(Math.Abs(VelocityX) / 2);
                            VelocityX = -(VelocityX * .5f);
                            PositionX += 50;
                            facingRight = true;
                        }
                    }
                }


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (((feet.Intersects(top) && !knockedBack) || (vitalRecFeet.Intersects(top) && knockedBack) || new Rectangle(feet.X, feet.Y, feet.Width, (int)velocity.Y).Intersects(top)) && velocity.Y > 0)
                {
                    if (enemyState == EnemyState.falling)
                    {
                        enemyState = EnemyState.landing;
                        moveFrame = 8;
                        frameDelay = 5;
                    }
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height;

                    if (velocity.Y > 7)
                        Game1.camera.ShakeCamera(3, 3);
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
                if (position.X < currentPlat.Rec.X - 200)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X - 200;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + 200)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + 200;
                }
            }
            #endregion

        }

        public override void Draw(SpriteBatch s)
        {

            if (enemyState != EnemyState.none)
            {
                String texName = "";

                #region Draw Enemy

                if (isStunned)
                {
                    texName = "flinch0";
                }
                else if (knockedBack)
                {
                    texName = "flinch0";
                }
                else if (enemyState == EnemyState.moving)
                {
                    texName = "walk" + (moveFrame);
                }
                else if (enemyState == EnemyState.spinMoving)
                {
                    texName = "spin" + moveFrame;
                }
                else if (enemyState == EnemyState.jumping)
                {
                    texName = "jump" + moveFrame;
                }
                else if (enemyState == EnemyState.landing || enemyState == EnemyState.falling)
                {
                    texName = "land" + moveFrame;
                }
                else if (enemyState == EnemyState.headSpinning)
                {
                    texName = "ground spin" + moveFrame;
                }
                else if (enemyState == EnemyState.catchingHead)
                {
                    texName = "head catch" + moveFrame;
                }
                else if (enemyState == EnemyState.tired)
                {
                    texName = "dizzy" + moveFrame;
                }
                else if (enemyState == EnemyState.attacking)
                {
                    switch (attackState)
                    {
                        case AttackState.melee:
                            texName = "attack" + attackFrame;
                            break;
                        case AttackState.spinAcrossMap:
                            texName = "spin" + attackFrame;
                            break;
                        case AttackState.spinInAir:
                            texName = "spin" + attackFrame;
                            break;
                        case AttackState.xylophoneSolo:
                            texName = "solo" + attackFrame;
                            break;
                    }
                }
                else if (enemyState == EnemyState.standing)
                {
                    if (standState == 0)
                    {
                        texName = "static" + moveFrame;
                    }
                    else
                    {
                        texName = "static head spin" + moveFrame;
                    }
                }

                if (!facingRight)
                {
                    s.Draw(game.EnemySpriteSheets[texName], rec, Color.White * alpha);
                }
                else
                {
                    s.Draw(game.EnemySpriteSheets[texName], rec, null, Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
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

               // s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

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
                currentMap.Drops.Add(new EnemyDrop("Mix Tape", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }

        public void CutsceneMove(int speed)
        {

            enemyState = EnemyState.spinMoving;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;
                frameDelay = 5;
            }

            if (moveFrame > 7)
                moveFrame = 0;

            position.X += speed;

            UpdateRectangles();
        }

        public void CutsceneStand()
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

            UpdateRectangles();
        }
    }
}
