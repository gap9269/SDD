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
    public class Enemy : GameObject
    {

        //  ATTRIBUTES\\
        protected AttackType.AttackTypes veryEffective;//////////////
        protected AttackType.AttackTypes notEffective;///////////////
        protected AttackType.RangedOrMelee veryEffectiveRangedMelee;//Use these to change the damage done by skills enemies are weak/strong to
        protected AttackType.RangedOrMelee notEffectiveRangedMelee;///////////////
        protected float notEffectiveMultiplier = .8f; //////////////////////
        protected float veryEffectiveMultiplier = 1.2f; //////////////

        public GameObject objectToAttack;
        public Boolean attackingOtherObject = false;

        public int distanceFromFeetToBottomOfRectangle = 20;
        public int distanceFromFeetToBottomOfRectangleRandomOffset;
        protected int rectanglePaddingLeftRight = 0;

        public Boolean fellOffMap = false;
        protected int health;
        protected int tolerance;
        protected Texture2D spriteSheet;
        protected Texture2D healthBack;
        protected Texture2D healthFore;
        protected Rectangle healthBoxRec;
        protected Rectangle healthBarRec;
        protected String name;
        protected String displayName; //The name that is drawn over their heads
        protected Game1 game;
        protected Player player;
        protected int frameDelay = 5;
        public int moveFrame;
        protected int attackFrame;
        protected int attackCooldown;
        protected int enemySpeed;
        protected int level;
        protected bool facingRight;
        protected bool currentlyInMoveState;
        protected bool knockedBack;
        protected int experienceGiven;
        protected int originalHealthWidth;
        protected int maxHealth;
        protected Platform currentPlat;
        protected bool respawning;
        protected List<Rectangle> rangedAttackRecs;
        protected Boolean canBeKnockbacked = true;
        protected Vector2 knockBackVec;
        protected Boolean hangInAir = false;
        protected int hangInAirTime = 0;
        protected int maxAttackCooldown;
        protected bool canBeHit = true;
        protected bool spawnWithPoof = true;
        protected float verticalDistanceToPlayer;
        protected float horizontalDistanceToPlayer;
        protected int timeBeforeSpawn = 2;
        protected Rectangle deathRec;

        protected float redColor, greenColor; //FOR HEALTH BAR

        protected int maxHealthDrop;
        protected float moneyToDrop;

        static Random ranX = new Random();
        static Random ranY = new Random();

        protected int starFrame, starTimer;

        protected int dropDiameter = 70;
        protected MapClass currentMap;
        //--For randomly moving enemies
        protected static Random moveNum = new Random();
        protected static Random moveTime = new Random();
        protected static Random healthGiven = new Random();
        public int moveTimer;
        protected int moveState;
        protected bool hostile;
        protected Vector2 distanceFromPlayer;
        protected Rectangle attackRec;
        //--State Machine
        public enum EnemyState
        {
            standing,
            moving,
            attacking
        }
        public EnemyState enemyState;

        // PROPERTIES \\
        public Rectangle AttackRec { get { return attackRec; } set { attackRec = value; } }
        public List<Rectangle> RangedAttackRecs { get { return rangedAttackRecs; } set { rangedAttackRecs = value; } }
        public int Level { get { return level; } set { level = value; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public string EnemyType { get { return name; } }
        public Boolean FacingRight { get { return facingRight; } set { facingRight = value; } }
        public Boolean CanBeHit { get { return canBeHit; } set { canBeHit = value; } }
        public Boolean SpawnWithPoof { get { return spawnWithPoof; } set { spawnWithPoof = value; } }
        public Boolean Hostile { get { return hostile; } set { hostile = value; } }
        public Boolean Respawning { get { return respawning; } set { respawning = value; } }
        public int TimeBeforeSpawn { get { return timeBeforeSpawn; } set { timeBeforeSpawn = value; } }
        public Platform CurrentPlat { get { return currentPlat; } set { currentPlat = value; } }
        public float HoriztonalDistanceToPlayer { get { return horizontalDistanceToPlayer; } }

        public Enemy() { }
        
        // CONSTRUCTOR \\
        public Enemy(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            :base()
        {
            position = pos;
            name = displayName = type;
            game = g;
            player = play;
            enemyState = EnemyState.standing;
            spriteSheet = game.EnemySpriteSheets[name];
            facingRight = true;
            knockedBack = false;
            healthFore = game.EnemySpriteSheets["HealthBar"];
            healthBack = game.EnemySpriteSheets["HealthBox"];
            font = game.Font;
            respawning = true;
            alpha = -1f;
            hostile = false;
            healthBoxRec = new Rectangle(rec.X + rec.Width / 2 - healthBack.Width / 2, rec.Y - 25, healthBack.Width, healthBack.Height);
            healthBarRec = new Rectangle(rec.X + rec.Width / 2 - healthBack.Width / 2 + 2, rec.Y - 23, healthFore.Width, healthFore.Height);
            originalHealthWidth = healthFore.Width;
            distanceFromPlayer = new Vector2();
            rangedAttackRecs = new List<Rectangle>();
            currentMap = cur;
            starTimer = 15;

            distanceFromFeetToBottomOfRectangleRandomOffset = Game1.randomNumberGen.Next(-13, 0);
        }

        //--Return the source rectangle for the enemy move frames
        public virtual Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 0, 0);
        }


        // METHODS TO PASS TO CHILDREN \\

        public virtual void DrawForegroundEffect(SpriteBatch s)
        {

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            #region Draw Enemy
            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == false)
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
               // }

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + " " + displayName).X;

                Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
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

        public void AddDamageNum(int damage, Rectangle collision)
        {
            endingDamageNumVecs.Add(new Vector2(vitalRec.X + 20, vitalRec.Y - 10));
            Vector2 damVec = new Vector2(collision.X + ranX.Next(-75, 75), collision.Y + ranY.Next(-75, 75));

            damageVecs.Add(damVec);
            damageNums.Add(damage);
            damageTimers.Add(0);
            damageAlphas.Add(1f);
        }

        public void DrawDamage(SpriteBatch s)
        {
            
            #region Damage Text
            for (int i = 0; i < damageVecs.Count; i++)
            {
                damageTimers[i]++;

                if (damageTimers[i] > 10)
                    damageAlphas[i] -= .02f;

                if (weaknessStrengthOrNormal.Count > i)
                {

                    if (weaknessStrengthOrNormal[i] == "Normal")
                        s.DrawString(Game1.enemyFont, damageNums[i].ToString() + "DMG", damageVecs[i], Color.White * damageAlphas[i]);
                    else if (weaknessStrengthOrNormal[i] == "Strength")
                        s.DrawString(Game1.enemyFontWeak, damageNums[i].ToString() + "DMG", damageVecs[i], Color.White * damageAlphas[i]);
                    else if (weaknessStrengthOrNormal[i] == "Weakness")
                        s.DrawString(Game1.enemyFontStrong, damageNums[i].ToString() + " DMG", damageVecs[i], Color.White);
                }
                else
                    s.DrawString(Game1.enemyFont, damageNums[i].ToString() + "DMG", damageVecs[i], Color.White * damageAlphas[i]);

                if (damageTimers[i] > 60)
                {
                    damageAlphas.RemoveAt(i);
                    damageVecs.RemoveAt(i);
                    damageNums.RemoveAt(i);
                    endingDamageNumVecs.RemoveAt(i);
                    damageTimers.RemoveAt(i);

                    if (weaknessStrengthOrNormal.Count > i)
                    {
                        weaknessStrengthOrNormal.RemoveAt(i);
                    }
                    i--;
                }
            }
            #endregion

        }

        public Boolean IsAbovePlayer()
        {
            if (player.VitalRecY > vitalRec.Y + vitalRec.Height)
            {
                return true;
            }

            return false;
        }

        public virtual void Update(int mapwidth)
        {
           verticalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(0, player.VitalRec.Center.Y), new Vector2(0, vitalRec.Center.Y)));
           horizontalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(player.VitalRec.Center.X, 0), new Vector2(vitalRec.Center.X, 0)));

           if (PositionY > 3000)
           {
               fellOffMap = true;
           }
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
            if (position.X + rectanglePaddingLeftRight  <= 0)
            {
                position.X = -(rectanglePaddingLeftRight);
            }

            if (position.X + rec.Width - rectanglePaddingLeftRight >= mapwidth)
            {
                position.X = mapwidth - rec.Width + rectanglePaddingLeftRight;
            }
            #endregion
        }

        public virtual void UpdateRectangles()
        {
            #region Update Rectangles
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            healthBoxRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2;
            healthBoxRec.Y = vitalRec.Y - 17;

            healthBarRec.X = vitalRec.X + vitalRec.Width / 2 - healthBack.Width / 2 + 2;
            healthBarRec.Y = vitalRec.Y - 15;

            healthBarRec.Width = (int)((float)originalHealthWidth * ((float)health / (float)maxHealth));
            #endregion
        }

        public virtual void StopAttack()
        {

        }

        //--Check to see if the player is colliding with the enemy
        //--Takes in a damage amount and the amount of knockback
        public virtual void CheckWalkCollisions(int damage, Vector2 knockback)
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


                }
                #endregion
            }
        }

        //--Take damage
        //--Takes in damage amount and knockback amount
        public virtual void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
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

                enemyState = EnemyState.standing;
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

        public virtual void PlaySoundWhenHit()
        {

        }

        //--Sets knockback to true and sets the velocity of the knockback
        public virtual void KnockBack(Vector2 kbvel)
        {
            if (canBeKnockbacked)
            {
                knockedBack = true;
                velocity = kbvel;
            }
        }

        //--Updates the knockback effect
        public virtual void UpdateKnockBack()
        {
            //--If the monster is currently being knocked back
            if (knockedBack == true)
            {
                //--If the monster is bouncing to the right, subtract velocity in the X direction
                if (velocity.X > 0)
                {
                    velocity.X -= 2;

                    //--If the velocity hits 0 or goes negative, make the velocity 0
                    if (velocity.X <= 0)
                        velocity.X = 0;
                }
                //--If the monster is bouncing to the left, add velocity
                if (velocity.X < 0)
                {
                    velocity.X += 2;

                    //--If it goes positive or hits 0, make it 0
                    if (velocity.X >= 0)
                        velocity.X = 0;
                }

                //--While the monster is being knocked back, make it so it cannot change movestates
                //--This is done by changing the boolean to true and making the move timer a negative number
                //--moveState = 0 makes it so the enemy is in the standing position
                moveState = 0;
                moveTimer = -1;
                currentlyInMoveState = true;
            }
        }

        public void UpdateFlyingKnockback()
        {
            //--If the monster is currently being knocked back
            if (knockedBack == true)
            {
                position += velocity;

                //--If the monster is bouncing to the right, subtract velocity in the X direction
                if (knockBackVec.X > 0)
                {
                    knockBackVec.X -= 2;
                    velocity.X -= 2;

                    //--If the velocity hits 0 or goes negative, make the velocity 0
                    if (knockBackVec.X <= 0)
                        knockBackVec.X = 0;
                }
                //--If the monster is bouncing to the left, add velocity
                else if (knockBackVec.X < 0)
                {
                    knockBackVec.X += 2;
                    velocity.X += 2;

                    //--If it goes positive or hits 0, make it 0
                    if (knockBackVec.X >= 0)
                        knockBackVec.X = 0;
                }

                if (knockBackVec.Y < 0)
                {
                    knockBackVec.Y += GameConstants.GRAVITY;
                    velocity.Y += GameConstants.GRAVITY;
                }
                if (knockBackVec.Y >= 0)
                {
                    knockBackVec.Y = 0;
                    velocity.Y = 0;
                }

                if (knockBackVec.X == 0 && knockBackVec.Y == 0)
                    knockedBack = false;
                
            }
        }

        //--Make the monster fall back to the ground if it is in the air
        //--Update all forces, X and Y
        public virtual void ImplementGravity()
        {
            if (VelocityY < 1  && VelocityY > -1 && hangInAir)
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

           // Rectangle feet = new Rectangle((int)rec.X, (int)position.Y + rec.Height - 20 - distanceFromFeetToBottomOfRectangle - distanceFromFeetToBottomOfRectangleRandomOffset, rec.Width, 20);
            Rectangle feet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20 - distanceFromFeetToBottomOfRectangle - distanceFromFeetToBottomOfRectangleRandomOffset, vitalRec.Width, 20);
            Rectangle vitalRecFeet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20 - distanceFromFeetToBottomOfRectangle - distanceFromFeetToBottomOfRectangleRandomOffset, vitalRec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

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
                        checkPlatRec = new Rectangle(rightEn.X, rightEn.Y, Math.Abs((int)velocity.X), rightEn.Height);

                        if (checkPlatRec.Intersects(left))
                        {
                            PositionX -= VelocityX;
                            VelocityX = 0;
                        }
                    }
                    else
                    {
                        checkPlatRec = new Rectangle(leftEn.X - Math.Abs((int)VelocityX), leftEn.Y, Math.Abs((int)velocity.X), leftEn.Height);

                        if (checkPlatRec.Intersects(right))
                        {
                            PositionX += Math.Abs(VelocityX);
                            VelocityX = 0;
                        }
                    }
                }
                //DOn't move through non passable platforms
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


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (((feet.Intersects(top) && !knockedBack) || (vitalRecFeet.Intersects(top) && knockedBack) || new Rectangle(feet.X, feet.Y, feet.Width, (int)velocity.Y).Intersects(top)) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + distanceFromFeetToBottomOfRectangle + distanceFromFeetToBottomOfRectangleRandomOffset;
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
            //if (currentPlat != null)
            //{
            //    if (position.X < currentPlat.Rec.X - rectanglePaddingLeftRight)
            //    {
            //        velocity.X = 0;
            //        position.X = currentPlat.Rec.X - rectanglePaddingLeftRight;
            //    }
            //    if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + rectanglePaddingLeftRight)
            //    {
            //        velocity.X = 0;
            //        position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + rectanglePaddingLeftRight;
            //    }
            //}

            if (currentPlat != null)
            {
                if (position.X + (Math.Abs(rec.X - feet.X)) < currentPlat.Rec.X)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X - (Math.Abs(rec.X - feet.X));
                }
                if (position.X + (Math.Abs(rec.X - feet.X)) + feet.Width > currentPlat.Rec.X + currentPlat.Rec.Width)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - feet.Width - (Math.Abs(rec.X - feet.X));
                }
            }
            #endregion

        }

        //--If it is dead, return true
        public virtual bool IsDead()
        {
            if (health <= 0)
            {
                if (player.Level > level)
                {
                    int levelOffset = player.Level - level;

                    if (levelOffset > 4)
                        levelOffset = 4;

                    experienceGiven = experienceGiven - (int)(experienceGiven * (.2f * levelOffset));
                }

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

                if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                {
                    if (player.quickRetort.SkillRank < Skill.maxLevel && player.Level >= player.quickRetort.PlayerLevelsRequiredToLevel[player.quickRetort.SkillRank - 1])
                        player.quickRetort.Experience += experienceGiven;
                }

                Chapter.effectsManager.AddExpNums(experienceGiven, rec, vitalRec.Y);
                player.Experience += experienceGiven;
                DropItem();
                DropHealth();
                DropMoney();
                Chapter.effectsManager.AddSmokePoof(deathRec,1);
                Sound.PlayRandomRegularPoof(deathRec.Center.X, deathRec.Center.Y);
                //Unlock enemy bio for this enemy
                if (player.AllMonsterBios[name] == false)
                    player.UnlockEnemyBio(name);
                return true;
            }

            return false;
        }

        public void DropHealth()
        {
            int healthToDrop = healthGiven.Next(0, maxHealthDrop + 1);

            for (int i = 0; i < healthToDrop; i++)
            {
                Vector2 vel = new Vector2(ranX.Next(-8, 8), -ranY.Next(3, 14));
                HealthDrop newHealth = new HealthDrop(vel, new Rectangle(rec.Center.X, rec.Center.Y, 0, 0), 1);

                game.CurrentChapter.CurrentMap.HealthDrops.Add(newHealth);
            }
        }

        public void DropMoney()
        {
            float increment = 0f;
            moneyToDrop = (float)Math.Round((double)(moneyToDrop * (1 + player.moneyModifier / 100)), 2);
            float moneyLeft = moneyToDrop;

            for (float i = 0; i < moneyToDrop; i += increment)
            {
                Vector2 vel;

                if (moneyLeft < .05)
                {
                    increment = .01f;
                     vel = new Vector2(ranX.Next(-10, 10), -ranY.Next(6, 20));
                }
                else if (moneyLeft < .25)
                {
                    increment = .05f;
                     vel = new Vector2(ranX.Next(-9, 9), -ranY.Next(5, 17));
                }
                else if (moneyLeft < 1.00)
                {
                    increment = .25f;
                     vel = new Vector2(ranX.Next(-8,8), -ranY.Next(4, 15));
                }
                else
                {
                    increment = 1f;
                     vel = new Vector2(ranX.Next(-7, 7), -ranY.Next(3, 13));
                }

                moneyLeft -= increment;

                MoneyDrop newMoney = new MoneyDrop(vel, new Rectangle(rec.Center.X, rec.Center.Y, 0, 0), increment);

                game.CurrentChapter.CurrentMap.MoneyDrops.Add(newMoney);
            }
        }

        public virtual void Move(int mapWidth)
        {
            
        }

        public virtual void DropItem()
        {
            ////Drop bio if you don't have it yet
            //if (player.AllMonsterBios.ContainsKey(name) && !player.AllMonsterBios[name])
            //{

            //    int dropType = moveNum.Next(0, 101);

            //    if (dropType < 10)
            //    {
            //        currentMap.Drops.Add(new EnemyBio("This String Doesn't Matter for Bios", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter), name));
            //    }
            //}

            int gem = Game1.randomNumberGen.Next(101);

            switch (player.Luck)
            {
                case 1:
                    if (gem == 1)
                    {
                        currentMap.Drops.Add(new EnemyDrop("Topaz", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    }
                    break;
                case 2:
                    if (gem < 2)
                        currentMap.Drops.Add(new EnemyDrop("Topaz", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if(gem == 2)
                        currentMap.Drops.Add(new EnemyDrop("Ruby", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    break;
                case 3:
                    if (gem < 2)
                        currentMap.Drops.Add(new EnemyDrop("Topaz", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if(gem < 4)
                        currentMap.Drops.Add(new EnemyDrop("Ruby", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if(gem == 4)
                        currentMap.Drops.Add(new EnemyDrop("Sapphire", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));

                    break;
                case 4:
                    if (gem < 3)
                        currentMap.Drops.Add(new EnemyDrop("Topaz", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem < 5)
                        currentMap.Drops.Add(new EnemyDrop("Ruby", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem == 5)
                        currentMap.Drops.Add(new EnemyDrop("Sapphire", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem == 6)
                        currentMap.Drops.Add(new EnemyDrop("Emerald", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));

                    break;
                case 5:
                case 6:
                case 7:
                    if (gem < 4)
                        currentMap.Drops.Add(new EnemyDrop("Topaz", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem < 6)
                        currentMap.Drops.Add(new EnemyDrop("Ruby", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem < 8)
                        currentMap.Drops.Add(new EnemyDrop("Sapphire", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem == 8)
                        currentMap.Drops.Add(new EnemyDrop("Emerald", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    else if (gem == 9)
                        currentMap.Drops.Add(new EnemyDrop("Diamond", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                    break;
            }

        }

        public virtual void Attack(int damage, Vector2 kb)
        {
        }
    }
}
