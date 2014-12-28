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
    public class Bomblin : Enemy
    {

        int standState = 0, runAwayTimer;
        Random standRand;
        Boolean throwing, swiping, summoningBomb;

        Boolean hasBomb = true;
        Rectangle attackExtensionRec;

        int swipeDamage, bombDamage, throwCooldown, bombCollisionDamage;

        int maxThrowCooldown = 150;

        public int StandState { get { return standState; } set { standState = value; } }

        public Bomblin(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {

            health = 6300;
            maxHealth = 6300;
            level = 15;
            experienceGiven = 75;
            rec = new Rectangle((int)position.X, (int)position.Y, 360, 408);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 190;
            vitalRec = new Rectangle(rec.X, rec.Y, 61, 220);
            maxHealthDrop = 260;
            moneyToDrop = .15f;

            standRand = new Random();
            throwing = false;
            swiping = false;

            maxAttackCooldown = 70;

            swipeDamage = 155;
            bombDamage = 145;
            bombCollisionDamage = 100;

            
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!respawning && !isStunned)
            {

                if (hostile)
                {
                    throwCooldown--;
                    attackCooldown--;
                }

                Move(mapwidth);

                CheckWalkCollisions(135, new Vector2(15, -5));
            }

            vitalRec.X = rec.X + 150;
            vitalRec.Y = rec.Y + 130;
            deathRec = vitalRec;

        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (summoningBomb && !knockedBack)
            {
                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 4;
                }

                if (moveFrame > 18)
                {
                    moveFrame = 10;
                    hasBomb = true;
                    summoningBomb = false;
                    frameDelay = 5;
                }
            }
            //--If the enemy hasn't been hit, walk randomly. If he is hostile but far away from the player, ignore him
            else if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && enemyState != EnemyState.attacking)
            {
                #region Random movement, not hostile
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

                            frameDelay = 7;
                        }

                        if (moveFrame > 15)
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

                        if (moveFrame > 10)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (moveFrame == 10)
                            enemySpeed = 2;
                        else
                            enemySpeed = 3;
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

                        if (moveFrame > 10)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (moveFrame == 10)
                            enemySpeed = 2;
                        else
                            enemySpeed = 3;
                        if (position.X >= 6)
                            position.X -= enemySpeed;
                        break;
                }


                if (moveTimer <= 0)
                    currentlyInMoveState = false;
                #endregion
            }

            else if (hostile && distanceFromPlayer < 1700)
            {

                //ALWAYS MOVE TOWARD PLAYER IF CAN'T ATTACK
                if (((distanceFromPlayer > 200 && distanceFromPlayer < 270) || distanceFromPlayer > 800) && knockedBack == false && enemyState != EnemyState.attacking && attackCooldown > 0)
                {
                    MoveTowardPlayer(mapWidth);
                }
                //IF CLOSE TO PLAYER BUT CANT ATTACK YET, JUST STAND AND BREATHE
                else if (((throwCooldown > 0 && distanceFromPlayer >= 270 && distanceFromPlayer <= 800) || attackCooldown > 0) && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    enemyState = EnemyState.standing;
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;

                        frameDelay = 7;
                    }

                    if (moveFrame > 15)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                }
                //IF YOU CAN ATTACK, CHOOSE WHAT TO DO BASED ON DISTANCE FROM PLAYER
                else if ((attackCooldown <= 0 || throwCooldown <= 0) && knockedBack == false && enemyState != EnemyState.attacking)
                {
                    if (horizontalDistanceToPlayer >= 270 && horizontalDistanceToPlayer <= 800 && throwCooldown <= 0)
                        throwing = true;
                    else if (horizontalDistanceToPlayer <= 200 && attackCooldown <= 0)
                        swiping = true;
                    else if ((horizontalDistanceToPlayer > 200 && horizontalDistanceToPlayer < 270) || horizontalDistanceToPlayer > 800)
                        MoveTowardPlayer(mapWidth);
                }

                #region Attack once it is close enough
                if (swiping)
                {
                    //--Only attack if off cooldown
                    if (attackCooldown <= 0)
                    {

                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(10, -5);
                        else
                            kb = new Vector2(-10, -5);
                        Attack(swipeDamage, kb);
                    }
                }
                else if (throwing)
                {
                    //--Only attack if off cooldown
                    if (throwCooldown <= 0)
                    {

                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(5, -3);
                        else
                            kb = new Vector2(-5, -3);

                        Attack(0, kb);
                    }
                }
                #endregion
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
            moveFrame = 0;

            //--Face the player if it isn't already. 
            //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
            //--The wrong way and autoattack in the wrong direction
            if (player.VitalRec.Center.X < vitalRec.Center.X)
                facingRight = false;
            else
                facingRight = true;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;

                frameDelay = 5;

                if (swiping)
                {
                    if (facingRight)
                    {
                        attackRec = new Rectangle(vitalRec.X + vitalRec.Width - 60, vitalRec.Y + 20, 200, 180);
                    }
                    else
                    {
                        attackRec = new Rectangle(vitalRec.X - 140, vitalRec.Y + 20, 200, 180);
                    }
                    //RangedAttackRecs.Add(attackRec);
                }
            }
            enemyState = EnemyState.attacking;

            #region SMACK ATTACK
            if (swiping)
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;
                    frameDelay = 2;
                }

                //--If the player gets hit in the middle of the animation, do damage and knockback
                if (attackFrame > 1)
                {
                    if ((player.CheckIfHit(attackRec) || player.CheckIfHit(attackRec)) && player.InvincibleTime <= 0)
                    {
                        int randomExplode = standRand.Next(2);

                        if (randomExplode == 0)
                        {
                            hasBomb = false;
                            summoningBomb = true;
                            Chapter.effectsManager.AddSmokePoof(new Rectangle(attackRec.X, attackRec.Y + 30, 150, 150), 3);

                            Vector2 kb1;
                            if(facingRight)
                                kb1 = new Vector2(-35, -8);
                            else
                                kb1 = new Vector2(35, -8);

                            TakeHit(bombDamage * 4, kb1, new Rectangle(attackRec.X, attackRec.Y, 190, 190), AttackType.AttackTypes.Blunt, AttackType.RangedOrMelee.Melee);

                            Game1.Player.TakeDamage(damage);

                            if (rec.Center.X < Game1.Player.VitalRec.Center.X)
                                Game1.Player.KnockPlayerBack(new Vector2(25, -8));
                            else
                                Game1.Player.KnockPlayerBack(new Vector2(-25, -8));

                            Game1.Player.HitPauseTimer = 3;
                            Game1.camera.ShakeCamera(3, 5);
                            MyGamePad.SetRumble(3, (float)((float)5 / 100f) * 10f);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, Game1.Player.VitalRec));
                        }
                        else
                        {
                            player.TakeDamage(damage);
                            player.KnockPlayerBack(kb);
                            hitPauseTimer = 3;
                            player.HitPauseTimer = 3;
                            game.Camera.ShakeCamera(2, 2);
                            MyGamePad.SetRumble(3, .4f);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                        }
                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 4)
                {
                    attackCooldown = maxAttackCooldown;

                    attackFrame = 0;
                    enemyState = EnemyState.standing;
                    currentlyInMoveState = false;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    throwing = false;
                    swiping = false;
                }
            }
            #endregion
            
            //THROW BOMBZ BOI
            else
            {
                //--Go through the animation
                frameDelay--;
                if (frameDelay == 0)
                {
                    attackFrame++;

                    frameDelay = 4;

                    if (attackFrame == 14)
                    {
                        hasBomb = false;
                        Bomb arrow;

                        float randomVelocityDifference = standRand.Next(-25, 25);

                        float bombVelocity = horizontalDistanceToPlayer / 44f;

                        if (bombVelocity > 5)
                            bombVelocity += (randomVelocityDifference / 10);

                        if (facingRight)
                        {
                            arrow = new Bomb((int)vitalRec.X + VitalRecWidth + 20, (int)vitalRec.Y + 30, new Vector2(bombVelocity, 0), bombDamage, new Vector2(25, -8), bombCollisionDamage);
                        }
                        else
                        {
                            arrow = new Bomb((int)vitalRec.X - 20, (int)vitalRec.Y + 30, new Vector2(-bombVelocity, 0), bombDamage, new Vector2(-25, -8), bombCollisionDamage);
                        }

                        currentMap.Projectiles.Add(arrow);
                    }
                }

                //--Once it has ended, reset
                if (attackFrame > 30)
                {
                    throwCooldown = maxThrowCooldown;

                    summoningBomb = true;
                    attackFrame = 0;
                    enemyState = EnemyState.standing;
                    currentlyInMoveState = false;
                    attackRec = new Rectangle(0, 0, 0, 0);
                    throwing = false;
                    swiping = false;
                }
            }
            currentlyInMoveState = true;


            //--If the enemy gets hit during the attack, cancel the attack. Keep this at the end
            if (knockedBack)
            {
                if (throwing && attackFrame >= 14)
                {
                    hasBomb = false;
                    summoningBomb = true;
                }

                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);
                throwing = false;
                swiping = false;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.emptyBox, rec, Color.Black);
            //s.Draw(Game1.whiteFilter, attackRec, Color.Black);
            String texName = "stand00";
            
            #region Draw Enemy

            if (knockedBack)
            {
                texName = "flinchBomb";

                if (!hasBomb)
                    texName = "flinch";
            }
            else if (summoningBomb)
            {
                texName = "summon" + (moveFrame + 2);
            }
            else if (enemyState == EnemyState.moving)
            {
                texName = "walk" + (moveFrame + 1);
            }
            else if (enemyState == EnemyState.attacking)
            {
                if (swiping)
                {
                    texName = "smack" + (attackFrame);
                }
                else
                    texName = "throw" + attackFrame;
            }
            else if (enemyState == EnemyState.standing)
            {
                if (standState == 0)
                {
                    texName = "stand" + moveFrame;
                }
                else
                {
                    texName = "standTwo" + moveFrame;
                }
            }

            if (facingRight)
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

                s.DrawString(Game1.descriptionFont, "Lv." + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2 - 2, vitalRec.Y - 35 - 2), Color.Black);
                s.DrawString(Game1.descriptionFont, "Lv." + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2, vitalRec.Y - 35), Color.White);
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

                if (moveFrame > 10)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;
                if (moveFrame == 10)
                    enemySpeed = 2;
                else
                    enemySpeed = 3;
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

                if (moveFrame > 10)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (moveFrame == 10)
                    enemySpeed = 2;
                else
                    enemySpeed = 3;
                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
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
            throwing = false;
            swiping = false;
        }

        public void CutsceneStand()
        {
            enemyState = EnemyState.standing;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                if (moveFrame == 3)
                    frameDelay = 30;

                frameDelay = 15;
            }

            if (moveFrame > 3)
                moveFrame = 0;

            currentlyInMoveState = true;
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
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(knockback);


                }
                #endregion
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

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 20, rec.Width, 20);
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
                            Console.WriteLine("hit left");
                            // playerState = PlayerState.standing;
                        }
                    }
                    else
                    {
                        checkPlatRec = new Rectangle(leftEn.X - Math.Abs((int)VelocityX), leftEn.Y, Math.Abs((int)velocity.X), leftEn.Height);

                        if (checkPlatRec.Intersects(right))
                        {
                            // playerState = PlayerState.standing;
                            Console.WriteLine("hit right");
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

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height;
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
                if (position.X < currentPlat.Rec.X)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width;
                }
            }
            #endregion

        }
    }
}
