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
    public class SpookyPresent : Enemy
    {

        int meleeRange = 150;
        int punchDamage = 45;
        int standState;

        float floatCycle;
        int keyRecY;
        public Boolean hasKey = false;
        public float spiritAlpha = 1f;

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

        public static Dictionary<String, SoundEffect> giftSounds;

        public SpookyPresent(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 1600;
            maxHealth = 1600;
            level = 14;
            experienceGiven = 150;
            rec = new Rectangle((int)position.X, (int)position.Y, 461, 190);
            currentlyInMoveState = false;
            enemySpeed = 4;
            tolerance = 45;
            maxHealthDrop = 12;
            moneyToDrop = .30f;
            vitalRec = new Rectangle(197, 84, 71, 62);
            punchDamage = 100;
            spiritAttackDamage = 90;
            spiritRectangle = new Rectangle(0, 0, 370, 370);
            ghostLights = new List<GhostLight>();
            foreach (InteractiveObject ob in currentMap.InteractiveObjects)
            {
                if (ob is GhostLight)
                    ghostLights.Add(ob as GhostLight);
            }

            distanceFromFeetToBottomOfRectangle = 0;
            rectanglePaddingLeftRight = 177;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(2766, 190, 461, 190);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                    case EnemyState.moving:
                        if (standState == 0)
                        {
                            if(moveFrame < 8)
                                return new Rectangle(frame * 461, 0, 461, 190);
                            return new Rectangle((frame - 8) * 461, 190, 461, 190);
                        }
                        else
                        {
                            if (moveFrame < 8)
                                return new Rectangle(frame * 461, 380, 461, 190);
                            return new Rectangle((frame - 8) * 461, 570, 461, 190);
                        }

                    case EnemyState.attacking:
                        if(attackFrame < 8)
                            return new Rectangle(attackFrame * 461, 760, 461, 190);
                        else if(attackFrame < 16)
                            return new Rectangle((attackFrame - 8) * 461, 950, 461, 190);
                        return new Rectangle((attackFrame - 16) * 461, 1140, 461, 190);
                }
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle GetSpiritSource()
        {
            if (spiritFrame != 11)
                return new Rectangle(spiritFrame * 370, 1330, 370, 370);

            return new Rectangle(0, 1700, 370, 370);
        }

        public Rectangle GetSpiritAttackSource()
        {
            if (spiritAttacking)
                return new Rectangle(spiritAttackFrame * 370, 2070, 370, 370);

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
                    attackCooldown--;
                    spiritAttackCooldown--;
                }
                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }

            if(facingRight)
                vitalRec.X = rec.X + 197;
            else
                vitalRec.X = rec.X + 197;

            vitalRec.Y = rec.Y - 16;
            deathRec = vitalRec;

            spiritRectangle.X = rec.X + 40;
            spiritRectangle.Y = rec.Y - 80;
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

                    if (moveState == 0)
                    {
                        standState = Game1.randomNumberGen.Next(2);
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

                        if (moveFrame > 12)
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
                            frameDelay = 5;
                        }

                        if (moveFrame > 13)
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

                        if (moveFrame > 12)
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
                            spiritAttackRectangle = new Rectangle(spiritRectangle.Center.X - 100, spiritRectangle.Center.Y - 200, 200, 200);
                        }
                        else
                        {
                            spiritAttackRectangle = new Rectangle(spiritRectangle.Center.X - 100, spiritRectangle.Center.Y - 200, 200, 200);
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

                            frameDelay = 4;
                        }

                        if (moveFrame > 6)
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
                        {
                            facingRight = false;
                            attackRec = new Rectangle(vitalRec.X - 185, vitalRec.Y, 210, 100);
                        }
                        else
                        {
                            attackRec = new Rectangle(vitalRec.X + 50, vitalRec.Y, 210, 100);
                            facingRight = true;
                        }

                        moveFrame = 0;
                        frameDelay = 5;
                        enemyState = EnemyState.attacking;
                    }
                    else if (horizontalDistanceToPlayer > meleeRange && enemyState != EnemyState.attacking)
                        MoveTowardPlayer(mapWidth);
                    else if (enemyState == EnemyState.attacking)
                    {

                        Vector2 kb;

                        if (facingRight)
                            kb = new Vector2(20, -5);
                        else
                            kb = new Vector2(-20, -5);
                        Attack(punchDamage, kb);
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

                if (moveFrame > 13)
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

                if (moveFrame > 13)
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

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame > 11 && attackFrame < 14)
            {
                if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage, level);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 1;
                    player.HitPauseTimer = 1;
                    game.Camera.ShakeCamera(2, 2);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 23)
            {
                attackFrame = 0;
                attackCooldown = 80;
                enemyState = EnemyState.standing;
                attackRec = new Rectangle(0, 0, 0, 0);

                moveFrame = 0;
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

            attackFrame = 0;
            attackCooldown = 120;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy
            s.Draw(game.EnemySpriteSheets[name], new Rectangle(spiritRectangle.X, spiritRectangle.Y - 100, 370, 370), GetSpiritSource(), Color.White * alpha * spiritAlpha);

            if(spiritAttacking)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(spiritRectangle.X, spiritRectangle.Y - 100, 370, 370), GetSpiritAttackSource(), Color.White * .65f);

            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y - 100, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha * (1f - spiritAlpha));

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], new Rectangle(rec.X, rec.Y - 100, rec.Width, rec.Height), GetSourceRectangle(moveFrame), Color.White * alpha * (1f - spiritAlpha), 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            if (hasKey)
            {
                #region FLOAT UP AND DOWN
                //--Once it hits the ground, make it float up and down
                {
                    //--Every 20 frames it changes direction
                    //--It floats at 1 pixel per frame, every 2 frames
                    if (floatCycle < 50)
                    {
                        if (floatCycle % 5 == 0)
                            keyRecY -= 1; floatCycle++;

                    }
                    else
                    {
                        if (floatCycle % 5 == 0)
                            keyRecY += 1; floatCycle++;

                        if (floatCycle >= 100)
                        {
                            floatCycle = 0;
                        }
                    }
                }
                #endregion


                s.Draw(Game1.storyItemIcons["Gold Key"], new Rectangle(spiritRectangle.Center.X - 35, spiritRectangle.Center.Y - 135 + keyRecY, 70, 70), Color.White * alpha * (.5f + spiritAlpha));
            }
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 60;
                healthBarRec.Y = vitalRec.Y - 58;

                if (facingRight)
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 5;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 5 + 2;
                }
                else
                {
                    healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 5;
                    healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 - 5 + 2;
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
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) , healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2) - 5, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, feet, Color.Red * .5f);

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

        public override bool IsDead()
        {
            if (health <= 0)
            {
                experienceGiven += (int)player.extraExperiencePerKill;

                if (hasKey && !game.ChapterTwo.ChapterTwoBooleans["keyGhostKilled"])
                {
                    player.AddStoryItem("Gold Key", "a Gold Key", 1);
                    game.ChapterTwo.ChapterTwoBooleans["keyGhostKilled"] = true;
                }
                for (int i = 0; i < player.EquippedSkills.Count; i++)
                {
                    if (level >= player.Level - 5)
                    {
                        //Check to see if the skill is below level 4, and that the player's level is high enough to level the skill
                        if (player.EquippedSkills[i].SkillRank < Skill.maxLevel && player.Level >= player.EquippedSkills[i].PlayerLevelsRequiredToLevel[player.EquippedSkills[i].SkillRank - 1])
                            player.EquippedSkills[i].Experience += experienceGiven;

                    }
                }

                if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                    player.quickRetort.Experience += experienceGiven;

                Chapter.effectsManager.AddExpNums(experienceGiven, rec, vitalRec.Y);
                player.Experience += experienceGiven;
                DropItem();
                DropHealth();
                DropMoney();
                Chapter.effectsManager.AddSmokePoof(deathRec, 1);
                Sound.PlayRandomRegularPoof(deathRec.Center.X, deathRec.Center.Y);
                //Unlock enemy bio for this enemy
                if (player.AllMonsterBios[name] == false)
                    player.UnlockEnemyBio(name);
                return true;
            }

            return false;
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
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
