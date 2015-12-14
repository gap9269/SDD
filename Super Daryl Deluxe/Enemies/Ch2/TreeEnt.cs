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
    public class TreeEnt : Enemy
    {

        int meleeRange = 400;
        int fallDamage = 25;

        Boolean growing = false;
        int timerAfterAttack = 0;
        public static Dictionary<String, SoundEffect> treeSounds;


        public TreeEnt(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 1450;
            maxHealth = 1450;
            level = 12;
            experienceGiven = 100;
            rec = new Rectangle((int)position.X, (int)position.Y, 1368, 571);
            currentlyInMoveState = false;
            enemySpeed = 4;
            tolerance = 44;
            maxHealthDrop = 12;
            moneyToDrop = .35f;
            rectanglePaddingLeftRight = 430;
            vitalRec = new Rectangle(624, 161, 120, 381);
            maxAttackCooldown = 220;

            fallDamage = 79;
        }

        public override void PlaySoundWhenHit()
        {
            //int soundType = moveNum.Next(2);

            //if (soundType == 0)
            //    Sound.enemySoundEffects["ErlHit1"].CreateInstance().Play();
            //else
            //    Sound.enemySoundEffects["ErlHit2"].CreateInstance().Play();
        }

        //need this for the great wall scene
        public override void UpdateRectangles()
        {
            base.UpdateRectangles();

            if (enemyState != EnemyState.attacking || attackFrame < 13)
            {
                if (facingRight)
                    vitalRec = new Rectangle(rec.X + 624, rec.Y + 161, 120, 381);
                else
                    vitalRec = new Rectangle(rec.X + 624, rec.Y + 161, 120, 381);
            }
            else
            {
                if (facingRight)
                    vitalRec = new Rectangle(rec.X + 750, rec.Y + 450, 500, 100);
                else
                    vitalRec = new Rectangle(rec.X + 130, rec.Y + 450, 500, 100);
            }
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!(timerAfterAttack <= 0 && (!growing || moveFrame > 3) && (enemyState != EnemyState.attacking || attackFrame < 19)))
                canBeHit = false;
            else if (canBeHit == false)
                canBeHit = true;

            if (enemyState == EnemyState.attacking || growing)
                canBeStunned = false;
            else if (canBeStunned == false)
                canBeStunned = true;
            if (!respawning && !isStunned)
            {

                if (hostile)
                {
                    attackCooldown--;
                }
                if (hitPauseTimer <= 0)
                {
                    Move(mapwidth);
                }
            }

            if (enemyState != EnemyState.attacking || attackFrame < 13)
            {
                if (facingRight)
                    vitalRec = new Rectangle(rec.X + 624, rec.Y + 161, 120, 381);
                else
                    vitalRec = new Rectangle(rec.X + 624, rec.Y + 161, 120, 381);
            }
            else
            {
                if (facingRight)
                    vitalRec = new Rectangle(rec.X + 750, rec.Y + 450, 500, 100);
                else
                    vitalRec = new Rectangle(rec.X + 130, rec.Y + 450, 500, 100);
            }
            deathRec = new Rectangle(vitalRec.Center.X - 150, vitalRec.Center.Y - 150, 300, 300);
        }

        public void GrowAfterAttack()
        {
            if (timerAfterAttack <= 0)
            {
                enemyState = EnemyState.standing;

                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 21)
                    {
                        moveFrame = 0;
                        growing = false;
                        attackCooldown = maxAttackCooldown;
                    }
                }
            }
            else
                timerAfterAttack--;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            if (growing)
            {
                GrowAfterAttack();
            }

            #region Random movement
            else if ((hostile == false || distanceFromPlayer > 2000 || verticalDistanceToPlayer > 350 || IsAbovePlayer()) && !knockedBack && enemyState != EnemyState.attacking)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                    moveFrame = 0;
                    frameDelay = 5;
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

                        if (moveFrame > 7)
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

                        if (moveFrame > 9)
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

                        if (moveFrame > 9)
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

            else if (hostile && distanceFromPlayer <= 2000 && !knockedBack)
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
                        facingRight = false;
                    else
                        facingRight = true;

                    moveFrame = 0;
                    frameDelay = 5;

                    enemyState = EnemyState.attacking;
                }
                else if (horizontalDistanceToPlayer > meleeRange && knockedBack == false && enemyState != EnemyState.attacking)
                    MoveTowardPlayer(mapWidth);
                else if(enemyState == EnemyState.attacking)
                {

                    Vector2 kb;

                    if (facingRight)
                        kb = new Vector2(25, -8);
                    else
                        kb = new Vector2(-25, -8);
                    Attack(fallDamage, kb);
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

                if (moveFrame > 9)
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

                if (moveFrame > 9)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6)
                    position.X += enemySpeed;
            }

        }

        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);
            //--Temporary. Remove once animation is introduced
            moveFrame = 0;

            enemyState = EnemyState.attacking;

            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 5;
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if (attackFrame == 13 || attackFrame == 14)
            {
                if (player.CheckIfHit(new Rectangle(vitalRec.X, rec.Y, vitalRec.Width, rec.Height)) && player.InvincibleTime <= 0)
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
            if (attackFrame > 28)
            {
                attackFrame = 0;
                attackCooldown = maxAttackCooldown;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                attackRec = new Rectangle(0, 0, 0, 0);

                frameDelay = 5;
                moveFrame = 0;
                timerAfterAttack = 60;
                growing = true;
            }

            currentlyInMoveState = true;
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (respawning == false)
            {
                if (knockedBack && VelocityY > 0)
                    velocity.Y = 0;

                #region Strength and weakness modifiers
                //Increase damage if the skill type is equal to the enemy's weakness
                if ((skillType == veryEffective || meleeOrRanged == veryEffectiveRangedMelee) && skillType != AttackType.AttackTypes.none)
                {
                    damage = (int)(damage * veryEffectiveMultiplier);
                    weaknessStrengthOrNormal.Add("Weakness");
                }
                //Opposite if it's the enemy's strength
                else if ((skillType == notEffective || meleeOrRanged == notEffectiveRangedMelee) && skillType != AttackType.AttackTypes.none)
                {
                    damage = (int)(damage * notEffectiveMultiplier);
                    weaknessStrengthOrNormal.Add("Strength");
                }
                else
                {
                    weaknessStrengthOrNormal.Add("Normal");
                }
                #endregion

                damage = (int)(damage * (250f / (250f + tolerance)));

                PlaySoundWhenHit();

                if (damage <= 0)
                    damage = 1;

                if (enemyState != EnemyState.attacking && !growing)
                {
                    enemyState = EnemyState.standing;

                    kbvel *= .6f;

                    KnockBack(kbvel);
                    knockBackVec = kbvel;

                    if (knockBackVec.Y < -10)
                        hangInAir = true;

                    if (hangInAir == true)
                    {
                        hangInAirTime = 0;
                    }
                }
                health -= damage;

                AddDamageNum(damage, collision);
            }

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
            {
                //--Don't allow it to attack immediately
                attackCooldown = maxAttackCooldown;
                hostile = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.emptyBox, rec, Color.Black);
            //s.Draw(Game1.whiteFilter, attackRec, Color.Black);
            String texName = "stand0";

            #region Draw Enemy

            if (isStunned)
            {
                texName = "flinch0";
            }
            else if (enemyState == EnemyState.attacking)
            {
                texName = "attack" + attackFrame;
            }
            else if (growing)
            {
                texName = "respawn" + moveFrame;
            }
            else if (knockedBack)
            {
                texName = "flinch0";
            }
            else if (enemyState == EnemyState.moving)
            {
                texName = "walk" + moveFrame;
            }
            else if (enemyState == EnemyState.standing)
            {
                texName = "stand" + moveFrame;
            }

            if (timerAfterAttack <= 0)
            {
                if (facingRight)
                {
                    s.Draw(game.EnemySpriteSheets[texName], rec, Color.White * alpha);
                }
                else
                {
                    s.Draw(game.EnemySpriteSheets[texName], rec, null, Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            #endregion

            #region Health Bar
            if (health < maxHealth && timerAfterAttack <= 0 && (!growing || moveFrame > 3) && (enemyState != EnemyState.attacking || attackFrame < 19))
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

                if (facingRight)
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(vitalRec.X + vitalRec.Width / 2 - measX / 2) - 4, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(vitalRec.X + vitalRec.Width / 2 - measX / 2) - 2, healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //Rectangle feet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20, vitalRec.Width, 20);

            // s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

            // if (enemyState == EnemyState.attacking)
            //s.Draw(Game1.emptyBox, attackRec, Color.Blue);

            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

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
                currentMap.Drops.Add(new EnemyDrop("Lumber", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 30)
            {
                int equipmentType = Game1.randomNumberGen.Next(3);

                switch (equipmentType)
                {
                    case 0:
                        currentMap.Drops.Add(new EnemyDrop(new TreeTop(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                        break;
                    case 1:
                        currentMap.Drops.Add(new EnemyDrop(new TreeTrunk(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                        break;
                    case 2:
                        currentMap.Drops.Add(new EnemyDrop(new TreeBranch(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                        break;
                }
            }
        }
    }
}
