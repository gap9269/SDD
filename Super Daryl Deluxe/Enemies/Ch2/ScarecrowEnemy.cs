using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class ScarecrowEnemy : Enemy
    {
        Boolean summoningCrow = false;
        Boolean takingOutBat = false;
        Boolean activated;
        Boolean touched = false;
        Boolean beingSneaky = true;
        Boolean turnedHostileDuringSummon = false;
        Rectangle crowBounds;
        public Boolean canBeActivated = true;

        public static Dictionary<String, SoundEffect> scarecrowSounds;

        int timeBeforeSummon;

        public Boolean Activated { get { return activated; } set { activated = value; } }

        public ScarecrowEnemy(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Boolean active, Rectangle crowBnds)
            : base(pos, type, g, ref play, cur)
        {
            health = 5500;
            maxHealth = 5500;//45 is the real value
            level = 14;
            experienceGiven = 45;
            rec = new Rectangle((int)position.X, (int)position.Y, 315, 310);
            currentlyInMoveState = false;
            enemySpeed = 3;
            tolerance = 160;
            vitalRec = new Rectangle(rec.X, rec.Y, 150, 180);
            maxHealthDrop = 120;
            moneyToDrop = .18f;
            crowBounds = crowBnds;

            activated = active;

            if (activated)
                beingSneaky = false;

            timeBeforeSummon = moveTime.Next(450, 1200);

            veryEffective = AttackType.AttackTypes.Fire;
            notEffective = AttackType.AttackTypes.Wind;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if ((knockedBack || isStunned) && !takingOutBat && !summoningCrow && enemyState != EnemyState.attacking)
            {
                if (hostile)
                    return new Rectangle(3465, 310, 315, 310);
                else
                    return new Rectangle(3150, 310, 315, 310);
            }
            else if (summoningCrow)
            {
                if (hostile)
                    return new Rectangle(315 * moveFrame, 310, 315, 310);
                else
                    return new Rectangle(315 * moveFrame + 1575, 310, 315, 310);
            }

            else if (takingOutBat)
                return new Rectangle(315 * moveFrame, 620, 315, 310);

            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        {
                            if (hostile)
                                return new Rectangle(1890, 0, 315, 310);

                            return new Rectangle(0, 0, 315, 310);
                        }
                    case EnemyState.moving:
                        {
                            if (hostile)
                                return new Rectangle(1890 + (315 * moveFrame), 0, 315, 310);

                            return new Rectangle(315 * moveFrame, 0, 315, 310);
                        }
                    case EnemyState.attacking:
                        return new Rectangle(1260 + (315 * attackFrame), 620, 315, 310);
                }
            }
            return new Rectangle();
        }

        //Follows the player when he isn't hostile
        public void FollowPlayer(int mapWidth)
        {
            enemySpeed = 2;
            if (activated && !hostile)
            {
                Point distanceFromPlayer = new Point(Math.Abs(player.VitalRec.Center.X - vitalRec.Center.X),
Math.Abs(player.VitalRec.Center.Y - vitalRec.Center.Y));

                if (distanceFromPlayer.X > 150 && currentPlat == player.CurrentPlat)
                {
                    if (player.VitalRec.Center.X > vitalRec.Center.X && player.FacingRight)
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

                        if (moveFrame > 5)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X <= mapWidth - 6 && moveFrame > 1)
                            position.X += enemySpeed;

                    }
                    else if (!player.FacingRight && player.VitalRec.Center.X < vitalRec.Center.X)
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

                        if (moveFrame > 5)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X >= 6 && moveFrame > 1)
                            position.X -= enemySpeed;
                    }
                    else
                    {
                        enemyState = EnemyState.standing;
                    }
                }

                else if (distanceFromPlayer.X <= 150 && currentPlat == player.CurrentPlat)
                {
                    takingOutBat = true;
                    frameDelay = 4;
                    moveFrame = 0;
                }
            }
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!respawning && !isStunned)
            {
                if (hostile)
                    attackCooldown--;
                Move(mapwidth);

                if (!beingSneaky)
                    CheckWalkCollisions(130, new Vector2(10, -5));
            }
            vitalRec.X = rec.X + 90;
            vitalRec.Y = rec.Y + 50;
            deathRec = new Rectangle(vitalRec.X - 25, vitalRec.Y - 25, 175, 175);
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (!activated)
            {
                if (player.VitalRec.Intersects(vitalRec))
                {
                    touched = true;
                }

                if (touched && distanceFromPlayer > 400 && canBeActivated)
                {
                    activated = true;
                    canBeHit = true;
                }
            }

            if (takingOutBat)
            {
                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame == 3)
                        frameDelay = 8;
                }

                if (moveFrame > 3)
                {
                    moveFrame = 0;
                    takingOutBat = false;
                    hostile = true;
                    beingSneaky = false;
                    frameDelay = 5;
                }
            }
            else if (summoningCrow)
            {
                if (moveFrame == 0 && frameDelay == 5)
                {
                    scarecrowSounds["enemy_scarecrow_summon_01"].CreateInstance().Play();
                }

                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame == 4)
                        frameDelay = 10;
                }

                if (moveFrame == 4 && frameDelay == 2)
                {
                    Crow en = new Crow(new Vector2(VitalRecX, VitalRecY - 200), "Crow", game, ref player, game.CurrentChapter.CurrentMap, crowBounds);
                    en.Hostile = true;
                    game.CurrentChapter.CurrentMap.EnemiesInMap.Add(en);
                    game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Crow"]++;
                }

                if (moveFrame > 4)
                {
                    moveFrame = 0;
                    frameDelay = 5;
                    summoningCrow = false;
                    timeBeforeSummon = moveTime.Next(450, 1200);
                }
            }
            else if (turnedHostileDuringSummon)
            {
                //--Don't allow it to attack immediately
                attackCooldown = 80;
                hostile = true;
                takingOutBat = true;
                beingSneaky = false;
                activated = true;
                moveFrame = 0;
                frameDelay = 5;
                turnedHostileDuringSummon = false;
            }
            //--If the scarecrow isn't sneaking around behind you, and isn't hostile, make him walk around randomly
            else if ((hostile == false || distanceFromPlayer >= 2500 || verticalDistanceToPlayer > 300 ) && !beingSneaky && activated)
            {
                if (!summoningCrow)
                {
                    #region Random movement, not hostile
                    if (currentlyInMoveState == false)
                    {
                        moveState = moveNum.Next(0, 3);
                        moveTimer = moveTime.Next(10, 200);
                    }

                    switch (moveState)
                    {
                        case 0:
                            enemyState = EnemyState.standing;
                            frameDelay--;
                            if (frameDelay == 0)
                            {
                                moveFrame++;
                                frameDelay = 12;
                            }

                            if (moveFrame > 1)
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

                            if (moveFrame > 5)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (position.X <= mapWidth - 6 && moveFrame > 1)
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

                            if (moveFrame > 5)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            moveTimer--;
                            if (position.X >= 6 && moveFrame > 1)
                                position.X -= enemySpeed;
                            break;
                    }


                    if (moveTimer <= 0)
                        currentlyInMoveState = false;
                    #endregion
                }
            }
            else if (beingSneaky && !hostile && activated)
            {
                FollowPlayer(mapWidth);
            }
            //--If it is hostile
            else if (hostile && distanceFromPlayer < 2500)
            {

                //Only summon a crow if there is less than 15 enemies in the map
                if (currentMap.EnemiesInMap.Count < 15)
                {
                    timeBeforeSummon--;

                    if (timeBeforeSummon == 0 && enemyState != EnemyState.attacking)
                    {
                        summoningCrow = true;
                        moveFrame = 0;
                        frameDelay = 5;
                    }
                }

                if (!summoningCrow)
                {
                    #region If the player is too far away, move closer
                    //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
                    //--knockback or attacking

                    if ((attackCooldown > 0 && distanceFromPlayer <= 150) && enemyState != EnemyState.attacking)
                    {
                        moveFrame = 0;
                        enemyState = EnemyState.standing;
                    }


                    else if (distanceFromPlayer > 150 && knockedBack == false && enemyState != EnemyState.attacking)
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

                            if (moveFrame > 5)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            if (position.X >= 6 && moveFrame > 1)
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

                            if (moveFrame > 5)
                                moveFrame = 0;

                            currentlyInMoveState = true;
                            if (position.X <= mapWidth - 6 && moveFrame > 1)
                                position.X += enemySpeed;
                        }
                    }
                    #endregion

                    #region Attack once it is close enough
                    else
                    {
                        //--Only attack if off cooldown
                        if (attackCooldown <= 0)
                        {
                            Vector2 kb;
                            if (facingRight)
                                kb = new Vector2(10, -5);
                            else
                                kb = new Vector2(-10, -5);

                            Attack(140, kb);
                        }
                    }
                    #endregion
                }
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
           // moveFrame = 0;

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
                frameDelay = 10;
                if (facingRight)
                {
                    attackRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y - 20, 100, 100);
                }
                else
                {
                    attackRec = new Rectangle(vitalRec.X - 100, vitalRec.Y - 20, 100, 100);
                }
                RangedAttackRecs.Add(attackRec);
            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 4;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 1)
            {
                if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 3;
                    player.HitPauseTimer = 3;
                    game.Camera.ShakeCamera(2, 2);
                    MyGamePad.SetRumble(3, .3f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 3)
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
                attackCooldown = 15;
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
           // s.Draw(Game1.emptyBox, vitalRec, Color.Black);

            #region Draw Enemy
            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
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
                s.DrawString(Game1.descriptionFont, "Lv. " + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2 - 2, rec.Y - 35 - 2), Color.Black);
                s.DrawString(Game1.descriptionFont, "Lv. " + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2, rec.Y - 35), Color.White);
            }
            #endregion



            //s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

            // if (enemyState == EnemyState.attacking)
            //s.Draw(Game1.emptyBox, attackRec, Color.Blue);
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {

            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                if (summoningCrow)
                    turnedHostileDuringSummon = true;

                else
                {
                    //--Don't allow it to attack immediately
                    attackCooldown = 80;
                    hostile = true;
                    takingOutBat = true;
                    beingSneaky = false;
                    activated = true;
                    moveFrame = 0;
                    frameDelay = 5;
                }
            }

        }

        public override void ImplementGravity()
        {
            //if(activated)
                base.ImplementGravity();
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 50)
            {
                currentMap.Drops.Add(new EnemyDrop("Corn Stalk", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
