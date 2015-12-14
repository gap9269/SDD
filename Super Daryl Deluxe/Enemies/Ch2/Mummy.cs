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
    public class Mummy : Enemy
    {

        int touchDamage = 20;

        Boolean armless;

        int ignorePlayerTimer;

        public static Dictionary<String, SoundEffect> mummySounds;

        public Mummy(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur, Boolean armless = false)
            : base(pos, name, g, ref play, cur)
        {
            health = 1200;
            maxHealth = 1200;
            level = 14;
            experienceGiven = 30;
            rec = new Rectangle((int)position.X, (int)position.Y, 233, 228);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 8;
            maxHealthDrop = 0;
            moneyToDrop = .06f;
            vitalRec = armless ? new Rectangle(100, 100, 50, 180) : new Rectangle(100, 100, 90, 180);
            this.armless = armless;

            distanceFromFeetToBottomOfRectangle = 10;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (armless)
            {
                if (knockedBack || isStunned)
                    return new Rectangle(2796, 456, 233, 228);
                else
                {
                    switch (enemyState)
                    {
                        case EnemyState.standing:
                            return new Rectangle(frame * 233, 456, 233, 228);

                        case EnemyState.moving:
                            return new Rectangle(frame * 233, 685, 233, 228);
                    }
                }
            }
            else
            {
                if (knockedBack || isStunned)
                    return new Rectangle(2796, 0, 233, 228);
                else
                {
                    switch (enemyState)
                    {
                        case EnemyState.standing:
                            return new Rectangle(frame * 233, 0, 233, 228);

                        case EnemyState.moving:
                            return new Rectangle(frame * 233, 228, 233, 228);
                    }
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
                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }

                CheckWalkCollisions(touchDamage, new Vector2(10, -5));
            }

            if (armless)
            {
                if (facingRight)
                    vitalRec.X = rec.X + 90;
                else
                    vitalRec.X = rec.X + 90;
            }
            else
            {
                if (facingRight)
                    vitalRec.X = rec.X + 100;
                else
                    vitalRec.X = rec.X + 40;
            }

            vitalRec.Y = rec.Y + 20;
            deathRec = vitalRec;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (ignorePlayerTimer > 0)
                ignorePlayerTimer--;
            #region Random movement
            if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer() || ignorePlayerTimer > 0) && !knockedBack)
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

                        if (moveFrame > 11)
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

                        if (moveFrame > 12)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X <= mapWidth - 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11 && moveFrame != 12))
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

                        if (moveFrame > 12)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X + 150 >= 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11 && moveFrame != 12))
                            position.X -= enemySpeed;
                        break;
                }

                if (moveTimer <= 0)
                    currentlyInMoveState = false;
            }
            #endregion

            else if (hostile && distanceFromPlayer <= 1700 && !knockedBack)
            {
                if (horizontalDistanceToPlayer < 50)
                {
                    enemyState = EnemyState.standing;
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;

                        frameDelay = 5;
                    }

                    if (moveFrame > 11)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                }
                else
                    MoveTowardPlayer(mapWidth);
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

                if (moveFrame > 12)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + 150 >= 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11 && moveFrame != 12))
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

                if (moveFrame > 12)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11 && moveFrame != 12))
                    position.X += enemySpeed;
            }

        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                hostile = true;
            }
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
                healthBoxRec.Y = vitalRec.Y - 38;
                healthBarRec.Y = vitalRec.Y - 36;

                if (facingRight)
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 2;
                }
                else
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 ;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 2;
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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 24), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 37), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Red * .5f);

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
            //--If the monster is not respawning
            if (respawning == false && !knockedBack)
            {
                #region Runs into player
                if (player.CheckIfHit(vitalRec))
                {
                    if (player.InvincibleTime <= 0)
                        MyGamePad.SetRumble(3, .3f);

                    //--If the player is standing to the left of the enemy, make the knockback.X direction negative so he goes left
                    if (player.Position.X + (player.PlayerRec.Width / 2) < (int)(position.X + (rec.Width / 2)))
                        knockback.X = -(knockback.X);

                    //--Otherwise, bounce to the right and keep the knockback.X positive
                    else if (player.Position.X + (player.PlayerRec.Width / 2) > (int)(position.X + (rec.Width / 2)))
                        knockback.X = Math.Abs(knockback.X);

                    //--Take damage and knock the player back
                    player.TakeDamage(damage, level);
                    player.KnockPlayerBack(knockback);

                    if (facingRight)
                    {
                        currentlyInMoveState = true;
                        moveState = 1;
                    }
                    else
                    {
                        currentlyInMoveState = true;
                        moveState = 2;
                    }
                    ignorePlayerTimer = 60;
                }
                #endregion
            }
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                currentMap.Drops.Add(new EnemyDrop("Half-Eaten Cheese", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
