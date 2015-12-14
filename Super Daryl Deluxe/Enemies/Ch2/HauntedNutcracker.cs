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
    public class HauntedNutcracker : Enemy
    {

        int meleeRange = 210;
        int laserRange = 180;
        int shootDamage = 45;
        int laserDamage = 45;

        float spiritAlpha = 1f;

        Rectangle spiritRectangle;

        int spiritFrame, spiritAttackFrame;
        int spiritDelay = 5;
        int spiritAttackDelay = 5;
        int spiritAttackCooldown;
        int spiritAttackDamage = 30;
        int spiritMeleeRange = 100;
        Rectangle spiritAttackRectangle;

        Boolean spiritAttacking = false;

        List<GhostLight> ghostLights;

        public enum AttackState
        {
            none, shoot, lasers
        }
        AttackState attackState;

        int laserCooldown;
        int maxLaserCooldown = 130;

        public static Dictionary<String, SoundEffect> nutcrackerSounds;

        public HauntedNutcracker(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 1750;
            maxHealth = 1750;
            level = 14;
            experienceGiven = 150;
            rec = new Rectangle((int)position.X, (int)position.Y, 676, 266);
            currentlyInMoveState = false;
            enemySpeed = 4;
            tolerance = 48;
            maxHealthDrop = 7;
            moneyToDrop = .45f;
            vitalRec = new Rectangle(317, 38, 55, 173);

            shootDamage = 97;
            laserDamage = 105;
            spiritAttackDamage = 90;
            spiritRectangle = new Rectangle(0, 0, 370, 370);
            ghostLights = new List<GhostLight>();
            foreach (InteractiveObject ob in currentMap.InteractiveObjects)
            {
                if (ob is GhostLight)
                    ghostLights.Add(ob as GhostLight);
            }

            distanceFromFeetToBottomOfRectangle = 0;
            rectanglePaddingLeftRight = 290;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(3380, 798, 676, 266);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.moving:
                        if (moveFrame < 6)
                            return new Rectangle(frame * 676, 0, 676, 266);
                        else if (moveFrame < 12)
                            return new Rectangle((frame - 6) * 676, 266, 676, 266);
                        else if (moveFrame < 18)
                            return new Rectangle((frame - 12) * 676, 266 * 2, 676, 266);
                        else
                            return new Rectangle((frame - 18) * 676, 266 * 3, 676, 266);

                    case EnemyState.standing:
                        if (moveFrame < 6)
                            return new Rectangle(frame * 676, 1064, 676, 266);
                        else if (moveFrame < 12)
                            return new Rectangle((frame - 6) * 676, 1064 + 266, 676, 266);
                        else if (moveFrame < 18)
                            return new Rectangle((frame - 12) * 676, 1064 + 266 * 2, 676, 266);
                        else
                            return new Rectangle((frame - 18) * 676, 1064 + 266 * 3, 676, 266);

                    case EnemyState.attacking:
                        if (attackState == AttackState.shoot)
                        {
                            if (attackFrame < 6)
                                return new Rectangle(attackFrame * 676, 2128, 676, 266);
                            else if (attackFrame < 12)
                                return new Rectangle((attackFrame - 6) * 676, 2128 + 266, 676, 266);
                            else if (attackFrame < 18)
                                return new Rectangle((attackFrame - 12) * 676, 2128 + 266 * 2, 676, 266);
                            else
                                return new Rectangle((attackFrame - 18) * 676, 2128 + 266 * 3, 676, 266);
                        }
                        else
                        {
                            if (attackFrame < 4)
                                return new Rectangle(1352 + (attackFrame * 676), 2926, 676, 266);
                            else
                                return new Rectangle((attackFrame - 4) * 676, 2926 + 266, 676, 266);
                        }
                }
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle GetSpiritSource()
        {
            if (spiritFrame != 11)
                return new Rectangle(spiritFrame * 370, 0, 370, 370);

            return new Rectangle(0, 370, 370, 370);
        }

        public Rectangle GetSpiritAttackSource()
        {
            if (spiritAttacking)
                return new Rectangle(spiritAttackFrame * 370, 740, 370, 370);

            return new Rectangle();
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
                    if (enemyState != EnemyState.attacking)
                    {
                        attackCooldown--;
                        laserCooldown--;
                    }

                    spiritAttackCooldown--;

                }
                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }

            if(facingRight)
                vitalRec.X = rec.X + 317;
            else
                vitalRec.X = rec.X + 300;

            vitalRec.Y = rec.Y + 38;
            deathRec = vitalRec;

            spiritRectangle.X = rec.X + 150;
            spiritRectangle.Y = rec.Y - 50;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            #region Spirit Movement
            if ((enemyState == EnemyState.standing || enemyState == EnemyState.moving) && !spiritAttacking)
            {
                spiritDelay--;
                if (spiritDelay == 0)
                {
                    spiritFrame++;

                    spiritDelay = 6;
                }

                if (spiritFrame > 11)
                    spiritFrame = 0;
            }

            for(int i = 0; i < ghostLights.Count; i++)
            {
                if (vitalRec.Intersects(ghostLights[i].outsideRec) && ghostLights[i].active && game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
                {
                    if (vitalRec.Intersects(ghostLights[i].lightRec))
                    {
                        Rectangle intersectingRec = Rectangle.Intersect(vitalRec, ghostLights[i].lightRec);

                        spiritAlpha = (1 - ((float)intersectingRec.Width / (float)vitalRec.Width)) - .1f;
                    }
                    else if(spiritAlpha != .9f)
                    {
                        if (spiritAlpha > .9f)
                        {
                            spiritAlpha -= 025f;

                            if (spiritAlpha < .9f)
                                spiritAlpha = .9f;
                        }
                        else
                        {
                            spiritAlpha += 025f;

                            if (spiritAlpha > .9f)
                                spiritAlpha = .9f;
                        }
                    }
                    break;
                }

                if (i == ghostLights.Count - 1 && spiritAlpha != 1)
                    spiritAlpha += .04f;
            }

            if (spiritAlpha >= 1f)
            {
                canBeStunned = false;
            }
            else if (canBeStunned == false)
            {
                spiritAttacking = false;
                spiritAttackFrame = 0;
                spiritAttackRectangle = new Rectangle();
                canBeStunned = true;
                canBeHit = true;
            }
            #endregion

            #region Random movement
            if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && !knockedBack && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                    moveFrame = 0;
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

                        if (moveFrame > 22)
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

                        if (moveFrame > 22)
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

                        if (moveFrame > 22)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X + rectanglePaddingLeftRight - 20 >= 6)
                            position.X -= enemySpeed;
                        break;
                }

                if (moveTimer <= 0)
                    currentlyInMoveState = false;
            }
            #endregion

            else if (hostile && distanceFromPlayer <= 1700 && !knockedBack)
            {
                if (spiritAlpha >= 1f && enemyState != EnemyState.attacking)
                {
                    //ALWAYS MOVE TOWARD PLAYER IF CAN'T ATTACK
                    if (horizontalDistanceToPlayer > spiritMeleeRange && knockedBack == false && enemyState != EnemyState.attacking && !spiritAttacking && spiritAttackCooldown > 0)
                    {
                        MoveTowardPlayer(mapWidth);
                    }
                    //IF CLOSE TO PLAYER BUT CANT ATTACK YET, JUST STAND AND BREATHE
                    else if (spiritAttackCooldown > 0 && knockedBack == false && enemyState != EnemyState.attacking && !spiritAttacking)
                    {
                        enemyState = EnemyState.standing;
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            frameDelay = 5;
                        }

                        if (moveFrame > 22)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                    }
                    //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                    else if (spiritAttackCooldown <= 0 && knockedBack == false && horizontalDistanceToPlayer <= spiritMeleeRange && enemyState != EnemyState.attacking && !spiritAttacking)
                    {
                        //--Face the player if it isn't already. 
                        //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                        //--The wrong way and autoattack in the wrong direction
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                        {
                            facingRight = false;
                            spiritAttackRectangle = new Rectangle(spiritRectangle.Center.X - 100, spiritRectangle.Center.Y - 100, 200, 200);
                        }
                        else
                        {
                            spiritAttackRectangle = new Rectangle(spiritRectangle.Center.X - 100, spiritRectangle.Center.Y - 100, 200, 200);
                            facingRight = true;
                        }

                        spiritFrame = 0;
                        spiritDelay = 5;
                        spiritAttacking = true;
                    }
                    else if (horizontalDistanceToPlayer > spiritMeleeRange && enemyState != EnemyState.attacking && !spiritAttacking)
                        MoveTowardPlayer(mapWidth);
                    else if (spiritAttacking)
                    {

                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(10, -5);
                        else
                            kb = new Vector2(-10, -5);
                        SpiritAttack(spiritAttackDamage, kb);
                    }
                }
                else
                {
                    //ALWAYS MOVE TOWARD PLAYER IF CAN'T ATTACK
                    if (horizontalDistanceToPlayer > laserRange && knockedBack == false && enemyState != EnemyState.attacking && attackCooldown > 0 && laserCooldown > 0)
                    {
                        MoveTowardPlayer(mapWidth);
                    }
                    //IF CLOSE TO PLAYER BUT CANT ATTACK YET, JUST STAND AND BREATHE
                    else if (attackCooldown > 0 && knockedBack == false && enemyState != EnemyState.attacking && laserCooldown > 0)
                    {
                        enemyState = EnemyState.standing;
                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;

                            frameDelay = 4;
                        }

                        if (moveFrame > 22)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                    }
                    //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                    else if (((attackCooldown <= 0 && horizontalDistanceToPlayer <= meleeRange) || (laserCooldown <= 0 && horizontalDistanceToPlayer <= laserRange)) && knockedBack == false && enemyState != EnemyState.attacking)
                    {

                        moveFrame = 0;
                        frameDelay = 5;
                        enemyState = EnemyState.attacking;

                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                        {
                            facingRight = false;
                            attackRec = new Rectangle(vitalRec.X - 275, vitalRec.Y + 50, 210, 40);
                        }
                        else
                        {
                            attackRec = new Rectangle(vitalRec.X + 125, vitalRec.Y + 50, 210, 40);
                            facingRight = true;
                        }

                        if (attackCooldown <= 0)
                        {
                            attackState = AttackState.shoot;
                        }
                        else
                        {
                            attackState = AttackState.lasers;
                        }
                    }
                    else if (horizontalDistanceToPlayer > laserRange && enemyState != EnemyState.attacking)
                        MoveTowardPlayer(mapWidth);
                    else if (enemyState == EnemyState.attacking)
                    {
                        Vector2 kb;

                        if (attackState == AttackState.shoot)
                        {
                            if (facingRight)
                                kb = new Vector2(15, -5);
                            else
                                kb = new Vector2(-15, -5);
                        }
                        else
                        {
                            if (facingRight)
                                kb = new Vector2(5, -3);
                            else
                                kb = new Vector2(-5, -3);
                        }
                        Attack(0, kb);
                    }
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

                if (moveFrame > 22)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + rectanglePaddingLeftRight - 20 >= 6)
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

                if (moveFrame > 22)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
            }

        }

        public void SpiritAttack(int damage, Vector2 kb)
        {
            enemyState = EnemyState.standing;
            spiritAttacking = true;
            //--Go through the animation
            spiritAttackDelay--;
            if (spiritAttackDelay == 0)
            {
                spiritAttackFrame++;
                spiritAttackDelay = 5;
            }

            if (player.CheckIfHit(spiritAttackRectangle) && player.InvincibleTime <= 0)
            {
                player.TakeDamage(damage, level);
                player.KnockPlayerBack(kb);
                hitPauseTimer = 0;
                player.HitPauseTimer = 0;
                game.Camera.ShakeCamera(2, 2);

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
            }
            
            //--Once it has ended, reset
            if (spiritAttackFrame > 4)
            {
                spiritAttackFrame = 0;
                spiritAttackCooldown = 80;
                enemyState = EnemyState.standing;
                spiritAttacking = false;
                spiritAttackRectangle = new Rectangle(0, 0, 0, 0);

                spiritFrame = 0;
            }

            currentlyInMoveState = true;
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

            if (attackState == AttackState.shoot)
            {
                //--If the player gets hit in the middle of the animation, do damage and knockback
                if (attackFrame > 9 && attackFrame < 13)
                {
                    if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                    {
                        player.TakeDamage(shootDamage, level);
                        player.KnockPlayerBack(kb);
                        hitPauseTimer = 3;
                        player.HitPauseTimer = 3;
                        game.Camera.ShakeCamera(2, 2);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 19)
                {
                    attackFrame = 0;
                    attackCooldown = 80;
                    enemyState = EnemyState.standing;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    attackState = AttackState.none;
                    moveFrame = 0;
                }
            }
            else
            {
                //--If the player gets hit in the middle of the animation, do damage and knockback
                if (attackFrame > 3)
                {
                    Rectangle laserRec1;
                    Rectangle laserRec2;

                    if (facingRight)
                    {
                         laserRec1= new Rectangle(vitalRec.X + 15, vitalRec.Y + 35, 100, 100);
                         laserRec2= new Rectangle(vitalRec.X + 80, vitalRec.Y + 135, 120, 70);
                    }
                    else
                    {
                         laserRec1 = new Rectangle(vitalRec.X - 55, vitalRec.Y + 35, 100, 100);
                         laserRec2 = new Rectangle(vitalRec.X - 155, vitalRec.Y + 135, 120, 70);
                    }

                    if ((player.CheckIfHit(laserRec1)  || player.CheckIfHit(laserRec2)) && player.InvincibleTime <= 0)
                    {
                        player.TakeDamage(laserDamage, level);
                        player.KnockPlayerBack(kb);
                        hitPauseTimer = 1;
                        player.HitPauseTimer = 1;
                        game.Camera.ShakeCamera(3, 3);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 9)
                {
                    attackFrame = 0;
                    laserCooldown = maxLaserCooldown;
                    enemyState = EnemyState.standing;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    attackState = AttackState.none;
                    moveFrame = 0;
                }
            }

            currentlyInMoveState = true;
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (spiritAlpha < 1f)
            {
                if (enemyState == EnemyState.attacking)
                {
                    attackFrame = 0;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    spiritAttackRectangle = new Rectangle(0, 0, 0, 0);
                    spiritAttackFrame = 0;
                    spiritAttacking = false;
                    attackState = AttackState.none;

                }

                damage = (int)(damage * (1f - spiritAlpha));

                base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);
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

            if (attackState == AttackState.shoot)
                attackCooldown = 80;
            else
                laserCooldown = maxLaserCooldown;

            attackFrame = 0;
            attackState = AttackState.none;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy
            s.Draw(game.EnemySpriteSheets["Haunted Nutcracker Gas"], new Rectangle(spiritRectangle.X, spiritRectangle.Y, 370, 370), GetSpiritSource(), Color.White * alpha * spiritAlpha);

            if(spiritAttacking)
                s.Draw(game.EnemySpriteSheets["Haunted Nutcracker Gas"], new Rectangle(spiritRectangle.X, spiritRectangle.Y, 370, 370), GetSpiritAttackSource(), Color.White * .65f);

            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha * (1f - spiritAlpha));

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha * (1f - spiritAlpha), 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 40;
                healthBarRec.Y = vitalRec.Y - 38;

                if (facingRight)
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 8;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 8 + 2;
                }
                else
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 10;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 10 + 2;
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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) - 15, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) - 10, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

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
                currentMap.Drops.Add(new EnemyDrop("Haunted Present", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 40)
            {
                currentMap.Drops.Add(new EnemyDrop("Haunted Walnuts", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
