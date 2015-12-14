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
    public class VileMummy : Enemy
    {

        int touchDamage = 20;
        int explosionDamage = 50;

        int ignorePlayerTimer;

        public static Dictionary<String, SoundEffect> mummySounds;

        public Boolean exploding = false;
        Boolean doneExploding = false;
        public Rectangle explosionRec;

        public VileMummy(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 1200;
            maxHealth = 1200;
            level = 14;
            experienceGiven = 30;
            rec = new Rectangle((int)position.X, (int)position.Y, 261, 251);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 8;
            maxHealthDrop = 0;
            moneyToDrop = .06f;
            vitalRec = new Rectangle(100, 100, 90, 180);
            distanceFromFeetToBottomOfRectangle = 10;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {

            if(exploding)
                return new Rectangle(frame * 261, 522, 261, 251);

            else if (knockedBack || isStunned)
                return new Rectangle(3132, 261, 261, 251);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        return new Rectangle(frame * 261, 0, 261, 251);

                    case EnemyState.moving:
                        return new Rectangle(frame * 261, 251, 261, 251);
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

                if(!exploding)
                    CheckWalkCollisions(touchDamage, new Vector2(10, -5));
            }

            if (facingRight)
                vitalRec.X = rec.X + 80;
            else
                vitalRec.X = rec.X + 80;


            vitalRec.Y = rec.Y + 20;
            deathRec = vitalRec;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (ignorePlayerTimer > 0)
                ignorePlayerTimer--;

            if (exploding)
            {
                Explode();

                if (explosionRec != new Rectangle())
                {

                    if (player.CheckIfHit(explosionRec) && player.InvincibleTime <= 0)
                    {
                        Vector2 kb;
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                        {
                            kb = new Vector2(-30, -10);
                        }
                        else
                        {
                            kb = new Vector2(30, -10);
                        }

                        player.TakeDamage(explosionDamage, level);
                        player.KnockPlayerBack(kb);
                        hitPauseTimer = 3;
                        player.HitPauseTimer = 3;
                        game.Camera.ShakeCamera(2, 2);

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                    }

                    for (int i = 0; i < game.CurrentChapter.CurrentMap.InteractiveObjects.Count; i++)
                    {

                        InteractiveObject intObj = game.CurrentChapter.CurrentMap.InteractiveObjects[i];

                        //--If the skill's attack hits the enemy vitals
                        if (explosionRec.Intersects(intObj.VitalRec) && intObj.Finished == false && intObj.IsHidden == false)
                        {

                            if (intObj is ExplodingFlower)
                            {
                                ExplodingFlower temp = intObj as ExplodingFlower;

                                if (temp.flowerState == ExplodingFlower.FlowerState.idle || temp.flowerState == ExplodingFlower.FlowerState.scared)
                                {
                                    temp.flowerState = ExplodingFlower.FlowerState.exploding;
                                    temp.frameState = 8;
                                    temp.frameTimer = 5;
                                }
                            }
                        }
                    }
                }
            }
            #region Random movement
            else if ((hostile == false || distanceFromPlayer > 1700 || verticalDistanceToPlayer > 350 || IsAbovePlayer() || ignorePlayerTimer > 0) && !knockedBack)
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

                        if (moveFrame > 11)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X <= mapWidth - 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11))
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

                        if (moveFrame > 11)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;

                        if (position.X + 150 >= 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11))
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

                if (moveFrame > 11)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + 150 >= 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11))
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

                if (moveFrame > 11)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6 && (moveFrame != 0 && moveFrame != 6 && moveFrame != 11))
                    position.X += enemySpeed;
            }

        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0 && !exploding)
            {
                isStunned = false;
                exploding = true;
                moveFrame = 0;
                frameDelay = 5;
            }
            if (health <= 0 && doneExploding)
            {
                return true;
            }

            return false;
        }

        public void Explode()
        {
            isStunned = false;
            knockedBack = false;
            velocity = Vector2.Zero;

            enemyState = EnemyState.standing;
            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                frameDelay = 5;
            }

            if (moveFrame > 11)
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

                moveFrame = 0;
                frameDelay = 4;
                game.Camera.ShakeCamera(15, 10);
                Chapter.effectsManager.AddExpNums(experienceGiven, rec, vitalRec.Y);
                player.Experience += experienceGiven;
                DropItem();
                DropHealth();
                DropMoney();
                Sound.PlayRandomRegularPoof(deathRec.Center.X, deathRec.Center.Y);
                explosionRec = new Rectangle(rec.Center.X - 300, rec.Center.Y - 225, 600, 600);
                //Unlock enemy bio for this enemy
                if (player.AllMonsterBios[name] == false)
                    player.UnlockEnemyBio(name);
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
            if (!doneExploding)
            {
                if (explosionRec != new Rectangle())
                {
                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;

                        frameDelay = 4;
                    }

                    if (moveFrame > 8)
                    {
                        doneExploding = true;
                    }

                    s.Draw(EffectsManager.deathSpriteSheet, new Rectangle(explosionRec.X, explosionRec.Y, explosionRec.Width, (int)(explosionRec.Width * .75f)),
                    new Rectangle(moveFrame * 400, 600, 400, 300), Color.White);
                }
                else
                {
                    if (facingRight == true)
                        s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

                    if (facingRight == false)
                        s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 20;
                healthBarRec.Y = vitalRec.Y - 18;

                //if (facingRight)
                //{
                //    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 25;
                //    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 25 + 2;
                //}
                //else
                //{
                //    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 25;
                //    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 25 + 2;
                //}

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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 10), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 16), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
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
