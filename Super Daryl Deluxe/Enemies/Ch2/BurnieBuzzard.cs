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
    //Steering code from https://www.youtube.com/watch?v=ouE24QAyzeo

    class BurnieBuzzard : SteeringEnemy
    {
        Boolean attacking = false;
        int loopsBeforeBlink = 0; //At 3, blink once and reset
        Boolean blinking = false;
        Boolean isLeader;
        int randomPoint = 0;
        List<Vector2> playerAttackPoints;
        Vector2 acceleration = Vector2.Zero;
        float rotation;

        float nextA;
        float delt;
        Rectangle center;

        Vector2 playerAttackPosOffset;

        int fireDamage = 45;

        public BurnieBuzzard(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound)
            : base(pos, type, g, ref play, cur, bound)
        {
            speed = 5;

            velocity = new Vector2(speed * (float)Math.Cos(MathHelper.ToRadians(Game1.randomNumberGen.Next(20, 40))), speed * (float)Math.Sin(MathHelper.ToRadians(Game1.randomNumberGen.Next(20, 40))));
            rotation = MathHelper.ToRadians(Game1.randomNumberGen.Next(20, 40));
            delt = MathHelper.ToRadians(Game1.randomNumberGen.Next(60, 90));
            nextA = rotation + delt;

            health = 2700;
            maxHealth = 2700;
            level = 13;
            experienceGiven = 30;
            rec = new Rectangle((int)position.X, (int)position.Y, 555, 213);
            tolerance = 40;
            vitalRec = new Rectangle(0, 0, 220, 130);
            maxHealthDrop = 60;
            moneyToDrop = .13f;

            maxAttackCooldown = 120;

            playerAttackPosOffset = new Vector2(Game1.randomNumberGen.Next(-80, 80), Game1.randomNumberGen.Next(-80, 80));
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(2775, 0, 555, 213);

            if (enemyState == EnemyState.moving)
            {
                if(!hostile)
                    return new Rectangle((555 * moveFrame), 0, 555, 213);
                else
                    return new Rectangle((555 * moveFrame), 213, 555, 213);

            }
            else if (enemyState == EnemyState.attacking)
            {
                if(attackFrame < 7)
                    return new Rectangle(555 * attackFrame, 426, 555, 213);
                else if(attackFrame < 14)
                    return new Rectangle(555 * (attackFrame - 7), 639, 555, 213);
                else
                    return new Rectangle(555 * (attackFrame - 14), 852, 555, 213);


            }
            return new Rectangle();
        }

        public void UpdatePlayerAttackPoints()
        {
            //Upper left
            playerAttackPoints[0] = new Vector2(player.RecX + 30, player.RecY);

            //Right
            playerAttackPoints[1] = new Vector2(player.RecX + player.Rec.Width - 80, player.RecY + player.Rec.Height / 2);

            //Bottom mid left
            playerAttackPoints[2] = new Vector2(player.RecX + 120, player.RecY + player.Rec.Height);

            //Upper middle
            playerAttackPoints[3] = new Vector2(player.RecX + player.Rec.Width / 2, player.RecY);

            //Left
            playerAttackPoints[4] = new Vector2(player.RecX + 30, player.RecY + player.Rec.Height / 2);

            //Bottom mid right
            playerAttackPoints[5] = new Vector2(player.RecX + player.Rec.Width - 120, player.RecY + player.Rec.Height);

            //Upper Right
            playerAttackPoints[6] = new Vector2(player.RecX + player.Rec.Width - 80, player.RecY);
        }

        public void BaseUpdate(int mapWidth)
        {
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
            else
            {
                //--Implement forces
                if (!(this is SteeringEnemy))
                {
                    ImplementGravity();

                    UpdateKnockBack();
                }
                else
                    UpdateFlyingKnockback();

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
            }

            #region Dont run off map
            if (position.X + 200 <= 0)
            {
                position.X = 200;
            }

            if (position.X + rec.Width - 200 >= mapWidth)
            {
                position.X = mapWidth - rec.Width - 200;
            }
            #endregion

            if (!hostile)
            {
                if (VelocityX > 0)
                    facingRight = true;
                else
                    facingRight = false;
            }

        }

        public override void Update(int mapWidth)
        {
            BaseUpdate(mapWidth);

            if (enemyState == EnemyState.moving)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    frameDelay = 8;
                    moveFrame++;

                    if (moveFrame > 4)
                        moveFrame = 0;
                }
            }

            if (!respawning && !isStunned)
            {
                if (hitPauseTimer <= 0)
                {
                    if (!hostile && !knockedBack)
                    {
                        enemyState = EnemyState.moving;
                        Wander(100);
                    }
                    else if (enemyState != EnemyState.attacking && !knockedBack)
                    {
                        enemyState = EnemyState.moving;
                        attackCooldown--;

                        int playerMoveOffset = 0;
                        if (player.playerState == Player.PlayerState.running)
                        {
                            if (player.FacingRight)
                                playerMoveOffset = 90;
                            else
                                playerMoveOffset = -90;
                        }


                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                        {
                            facingRight = false;
                            attackRec = new Rectangle(vitalRec.Center.X -220 - 50, vitalRec.Y + 90, 220, 90);
                        }
                        else
                        {
                            facingRight = true;
                            attackRec = new Rectangle(vitalRec.Center.X + 50, vitalRec.Y + 90, 220, 100);
                        }

                        Vector2 toPlayer;
                        if (facingRight)
                            toPlayer = Vector2.Subtract(new Vector2(player.VitalRec.Center.X - 200 + playerMoveOffset + playerAttackPosOffset.X, player.VitalRec.Center.Y - 100 + playerAttackPosOffset.Y), vitalRec.Center.ToVector2());
                        else
                            toPlayer = Vector2.Subtract(new Vector2(player.VitalRec.Center.X + 200 + playerMoveOffset + playerAttackPosOffset.X, player.VitalRec.Center.Y - 100 + playerAttackPosOffset.Y), vitalRec.Center.ToVector2());

                        toPlayer.Normalize();

                        if (toPlayer.X == 0)
                            toPlayer.X = .001f;
                        if (toPlayer.Y == 0)
                            toPlayer.Y = .001f;
                        if (!(toPlayer.X == .001f && toPlayer.Y == .001f))
                        {
                            float dis;
                            if (facingRight)
                                dis = Vector2.Distance(new Vector2(player.VitalRec.Center.X - 200 + playerMoveOffset + playerAttackPosOffset.X, player.VitalRec.Center.Y - 100 + playerAttackPosOffset.Y), vitalRec.Center.ToVector2());
                            else
                                dis = Vector2.Distance(new Vector2(player.VitalRec.Center.X + 200 + playerMoveOffset + playerAttackPosOffset.X, player.VitalRec.Center.Y - 100 + playerAttackPosOffset.Y), vitalRec.Center.ToVector2());

                            if (dis != 0)
                                velocity = toPlayer * (dis * .02f);
                        }

                        if (attackCooldown <= 0 && enemyState != EnemyState.attacking && attackRec.Intersects(player.VitalRec))
                        {
                            enemyState = EnemyState.attacking;
                            frameDelay = 5;
                        }

                    }
                    else if(!knockedBack)
                    {
                        Vector2 kb;
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                        {
                            kb = new Vector2(-10, -5);
                        }
                        else
                        {
                            kb = new Vector2(10, -5);
                        }
                        canBeKnockbacked = false;
                        Attack(kb);
                    }
                    if (enemyState != EnemyState.attacking && !knockedBack)
                    {
                        canBeKnockbacked = true;
                        velocity += (acceleration);
                        position += (velocity);
                    }
           
                }
            }

            vitalRec.X = rec.X + 150;
            vitalRec.Y = rec.Y + 50;

            deathRec = vitalRec;
        }

        public void Attack(Vector2 kb)
        {
            base.Attack(fireDamage, kb);

            enemyState = EnemyState.attacking;
            currentlyInMoveState = true;

            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 5;
            }

            if (attackFrame > 6 && attackFrame < 18 && player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
            {
                player.TakeDamage(fireDamage, level);
                player.KnockPlayerBack(kb);
                hitPauseTimer = 3;
                player.HitPauseTimer = 3;
                game.Camera.ShakeCamera(2, 2);

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
            }


            //--Once it has ended, reset
            if (attackFrame > 19 || isStunned)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.moving;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                moveFrame = 0;
            }
        }
        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                hostile = true;
                speed = 9;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.emptyBox, vitalRec, Color.Black);

            #region Draw Enemy
            if (facingRight)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
            else
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {

                healthBoxRec.Y = vitalRec.Y - 37;
                healthBarRec.Y = vitalRec.Y - 35;

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 23), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 23), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            // if (enemyState == EnemyState.attacking)

           //s.Draw(Game1.whiteFilter, bounds, Color.Red * .1f);
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Grilled Buzzard Leg", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }


        public void SetVelocityLengthToSpeed()
        {
            if (velocity.X == 0)
                velocity.X = .001f;
            if (velocity.Y == 0)
                velocity.Y = .001f;

            velocity.Normalize();

            velocity = velocity * speed;
        }

        private void Wander(int threshold)
        {
            center = new Rectangle(rec.Center.X - 25, rec.Center.Y - 25, 50, 50);
            Vector2 adjustment = Vector2.Zero;
            if (center.Left < bounds.X)
            {
                int distance = bounds.X - center.Left;
                adjustment.X = distance;
            }
            else if (center.Right > bounds.X + bounds.Width)
            {
                int distance = (bounds.X + bounds.Width - threshold) - center.Right;
                adjustment.X = distance;
            }
            if (center.Top < bounds.Y)
            {
                int distance = threshold - center.Top;
                adjustment.Y = distance;
            }
            else if (center.Bottom > bounds.Y + bounds.Height)
            {
                int distance = (bounds.Y + bounds.Height - threshold) - center.Bottom;
                adjustment.Y = distance;
            }

            if (adjustment != Vector2.Zero)
            {
                velocity += adjustment;
                SetVelocityLengthToSpeed();
                rotation = (float)Math.Atan2(velocity.Y, velocity.X);

                delt *= -1;
                nextA = rotation + delt;
            }
            else if (Math.Abs(rotation - nextA) < 0.1f)
            {
                delt *= -1;
                nextA = rotation + delt;
            }
            else
            {
                rotation = MathHelper.Lerp(rotation, nextA, 0.01f);
                velocity = new Vector2(speed * (float)Math.Cos(rotation),
                    speed * (float)Math.Sin(rotation));
            }

        }

    }
}
