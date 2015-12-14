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
    public class Boss : GameObject
    {
          //  ATTRIBUTES\\
        protected int health;
        protected int tolerance;
        protected Texture2D spriteSheet;
        protected Texture2D bossHUD;
        public Texture2D faceTexture;
        protected Texture2D healthFore;
        protected Rectangle bossHUDRec;
        protected Rectangle healthBarRec;
        protected String name;
        protected Game1 game;
        protected Player player;
        protected int level;
        public bool facingRight;
        protected bool knockedBack;
        protected int experienceGiven;
        protected int maxHealth;
        protected Platform currentPlat;
        protected List<Rectangle> rangedAttackRecs;
        protected Boolean canBeKnockbacked = true;
        protected Boolean canBeHurt = true;
        protected float targetHealthWidth;
        protected MapClass currentMap;
        protected float addToHealthWidth;
        protected float distanceFromPlayer;
        protected Boolean drawHUDName = false;
        protected int xPos;
        protected int distanceFromBottomRecToFeet; //This is the distance from the bottom of the texture to where the boss should be standing on a platform
        protected Boolean hangInAir = false;
        protected int hangInAirTime = 0;
        public float horizontalDistanceToPlayer, rectanglePaddingLeftRight;

        //--Health bar stuff
        protected Random healthShakeNum;
        protected Boolean healthShaking = false;
        protected int shakeTimer;
        protected Vector2 shakeOffset;
        protected int originalHealthWidth;
        protected int originalHealthX;

        //--Movement
        protected int frameDelay;
        public int moveFrame;
        protected int attackFrame;
        protected int attackCooldown;
        protected bool currentlyInMoveState;
        protected int moveTimer;
        protected int moveState;
        protected int moveSpeed;
        protected Random chooseIntent;
        protected Random randomTime;
        protected List<Platform> boundaries;

        //Random
        static Random ranX = new Random();
        static Random ranY = new Random();

        int hudRecX = 580;
        int hudRecY = 490;
        //--State Machine
        protected enum BossState
        {
            standing,
            moving,
            attacking,

        }
        protected BossState bossState;

        protected Boolean hasBeenHit = false;
        public Boolean HasBeenHit { get { return hasBeenHit; } set { hasBeenHit = value; } }

        // PROPERTIES \\
        public Rectangle VitalRec { get { return vitalRec; } set { vitalRec = value; } }
        public List<Rectangle> RangedAttackRecs { get { return rangedAttackRecs; } set { rangedAttackRecs = value; } }
        public int Level { get { return level; } set { level = value; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public string Name { get { return name; } }
        public MapClass CurrentMap { get { return currentMap; } set { currentMap = value; } }
        public Boolean DrawHUDName { get { return drawHUDName; } set { drawHUDName = value; } }
        public Boolean CanBeStunned { get { return canBeStunned; } }
        public Boolean CanBeHurt { get { return canBeHurt; } }
        public List<Platform> Boundaries { get { return boundaries; } set { boundaries = value; } }

        // CONSTRUCTOR \\
        public Boss(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            :base()
        {
            position = pos;
            name = type;
            game = g;
            player = play;
            //spriteSheet = game.EnemySpriteSheets[name];
            facingRight = true;
            knockedBack = false;
            healthFore = game.EnemySpriteSheets["BossHealthBar"];
            bossHUD = game.EnemySpriteSheets["BossHUD"];
            bossState = BossState.standing;
            font = game.Font;
            alpha = 1f;
            originalHealthWidth = healthFore.Width;
            rangedAttackRecs = new List<Rectangle>();
            currentMap = cur;

            healthShakeNum = new Random();

            chooseIntent = new Random();
            randomTime = new Random();
            boundaries = new List<Platform>();


            bossHUDRec = new Rectangle(hudRecX, hudRecY, game.EnemySpriteSheets["BossHUD"].Width, game.EnemySpriteSheets["BossHUD"].Height);
            healthBarRec = new Rectangle(hudRecX, hudRecY, game.EnemySpriteSheets["BossHealthBar"].Width, game.EnemySpriteSheets["BossHealthBar"].Height);
            originalHealthX = hudRecX;

            rectanglePaddingLeftRight = 0;
        }

        //--Return the source rectangle for the enemy move frames
        public virtual Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 0, 0);
        }

        //--Return the source rectangle for the enemy's inner health bar
        public virtual Rectangle GetHealthSourceRectangle()
        {
            return new Rectangle(0, 0, healthFore.Width, healthFore.Height);
        }

        public virtual void ShakeHealthBar()
        {
            if (!healthShaking)
            {
                healthShaking = true;
                shakeTimer = 0;
            }
        }

        public virtual void FacePlayer()
        {
            if (player.VitalRec.Center.X > vitalRec.Center.X)
            {
                facingRight = true;
            }
            else if (player.VitalRec.Center.X < vitalRec.Center.X)
            {
                facingRight = false;
            }
        }

        public virtual void UpdateShakeHealthBar()
        {
            xPos = originalHealthWidth - healthBarRec.Width;

            if (healthShaking)
            {
                shakeTimer++;
                if (shakeTimer >= 7)
                {
                    healthShaking = false;
                    shakeTimer = 0;
                }

                float progress = shakeTimer / 7;

                float magnitude = 5 * (1f - (progress * progress));

                shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                bossHUDRec.X += (int)shakeOffset.X;
                bossHUDRec.Y += (int)shakeOffset.Y;
                healthBarRec.X += (int)shakeOffset.X;
                healthBarRec.Y += (int)shakeOffset.Y;

                if (shakeTimer % 2 == 0 || healthShaking == false)
                {
                    bossHUDRec.X = hudRecX;
                    bossHUDRec.Y = 490;
                    healthBarRec.X = healthBarRec.X = originalHealthX + xPos;
                    healthBarRec.Y = 490;
                }
            }
        }


        public float NextFloat()
        {
            return (float)healthShakeNum.NextDouble() * 2f - 1f;
        }

        public virtual void HealthBarGrow()
        {
            targetHealthWidth = (int)((float)originalHealthWidth * ((float)health / (float)maxHealth));
            addToHealthWidth += ((targetHealthWidth - healthBarRec.Width) * .02f);
            healthBarRec.Width = (int)addToHealthWidth;

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            ShakeHealthBar();

            #region Shake health bar a small amount
            if (healthShaking && (originalHealthWidth - healthBarRec.Width) > 40)
            {
                shakeTimer++;
                if (shakeTimer >= 7)
                {
                    healthShaking = false;
                    shakeTimer = 0;
                }

                float progress = shakeTimer / 7;

                float magnitude = 3.5f * (1f - (progress * progress));

                shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                bossHUDRec.X += (int)shakeOffset.X;
                bossHUDRec.Y += (int)shakeOffset.Y;
                healthBarRec.X += (int)shakeOffset.X;
                healthBarRec.Y += (int)shakeOffset.Y;

                if (shakeTimer % 2 == 0 || healthShaking == false)
                {
                    bossHUDRec.X = hudRecX;
                    bossHUDRec.Y = 490;
                    healthBarRec.X = healthBarRec.X = originalHealthX + xPos;
                    healthBarRec.Y = 490;
                }
            }
            else
            {
                bossHUDRec.X = hudRecX;
                bossHUDRec.Y = 490;
                healthBarRec.X = healthBarRec.X = originalHealthX + xPos;
                healthBarRec.Y = 490;
            }
            #endregion

            if (health != maxHealth)
            {
                health = maxHealth;
            }
        }

        public void DrawDamage(SpriteBatch s)
        {
            #region Damage Text
            for (int i = 0; i < damageVecs.Count; i++)
            {
                damageTimers[i]++;

                if (damageTimers[i] > 10)
                    damageAlphas[i] -= .02f;

                s.DrawString(Game1.enemyFont, damageNums[i].ToString() + "DMG", damageVecs[i], Color.White * damageAlphas[i]);
                //damageVecs[i] += new Vector2(0, -1);

                if (damageTimers[i] > 60)
                {
                    damageAlphas.RemoveAt(i);
                    damageVecs.RemoveAt(i);
                    damageNums.RemoveAt(i);
                    endingDamageNumVecs.RemoveAt(i);
                    damageTimers.RemoveAt(i);
                    i--;
                }
            }
            #endregion
        }

        public virtual Boolean CollideWithBounds()
        {
            boundaries[1].RecX = 3750;
            boundaries[0].RecX = 2400;
            for (int i = 0; i < boundaries.Count; i++)
            {
                if (vitalRec.Intersects(boundaries[i].Rec))
                    return true;
            }

            return false;
        }

        public virtual void Move(int mapWidth)
        {

        }

        //--Check to see if the player is colliding with the enemy
        //--Takes in a damage amount and the amount of knockback
        public virtual void CheckWalkCollisions(int damage, Vector2 knockback)
        {

            #region Runs into player
            if (player.CheckIfHit(vitalRec))
            {
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

        //--Take damage
        //--Takes in damage amount and knockback amount
        public virtual void TakeHit(int damage, Vector2 kbvel, Rectangle collision)
        {
            if (canBeHurt)
            {
                ShakeHealthBar();
                damage -= tolerance;

                hasBeenHit = true;

                if (damage <= 0)
                    damage = 1;
                health -= damage;
                KnockBack(kbvel);

                AddDamageNum(damage, collision);
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

        //--If it is dead, return true
        public virtual bool IsDead()
        {
            if (health <= 0)
            {
                player.Experience += experienceGiven;
                return true;
            }

            return false;
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

        //--Make the monster fall back to the ground if it is in the air
        //--Update all forces, X and Y
        public virtual void ImplementGravity()
        {
            velocity.Y += GameConstants.GRAVITY;
            position += velocity;

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - distanceFromBottomRecToFeet, rec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);
                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);

                #region Don't move through non passable platforms
                if ((rightEn.Intersects(left) || leftEn.Intersects(right)) && plat.Passable == false)
                {
                    if (rightEn.Intersects(left))
                    {
                        position.X -= moveSpeed;

                        if (VelocityX > 0)
                        {
                            PositionX -= (int)VelocityX;
                            velocity.X = 0;
                        }
                    }

                    if (leftEn.Intersects(right))
                    {
                        position.X += moveSpeed;

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
                #endregion

                #region Landing on a platform
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + distanceFromBottomRecToFeet;
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

                //if (rec.Intersects(plat.Rec) && plat.Passable == false)
                //{
                //    if (position.X < plat.Rec.X)
                //    {
                //        position.X = plat.Rec.X - rec.Width;
                //    }

                //    else if (position.X < plat.Rec.X + plat.Rec.Width)
                //    {
                //        position.X = plat.Rec.X + plat.Rec.Width;
                //    }
                //}
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

        public new virtual void Draw(SpriteBatch s)
        {

        }

        public virtual void DrawHud(SpriteBatch s)
        {

            s.Draw(game.EnemySpriteSheets["BossHUD"], bossHUDRec, Color.White);
            s.Draw(game.EnemySpriteSheets["BossHealthBar"], healthBarRec, GetHealthSourceRectangle(), Color.White);
            s.Draw(game.EnemySpriteSheets["BossLine"], bossHUDRec, Color.White);


            if(faceTexture != null)
                s.Draw(faceTexture, new Rectangle(1280 - faceTexture.Width + 31, (int)(Game1.aspectRatio * 1280) - faceTexture.Height + 29, faceTexture.Width, faceTexture.Height), Color.White);

            if (drawHUDName)
            {
                Game1.OutlineFont(Game1.questNameFont, s, name, 1, (int)(1070 - (Game1.questNameFont.MeasureString(name).X) + 80), (int)(Game1.aspectRatio * 1280 * .9f) - 26 + 29, Color.Black, Color.White);

            }
        }

        public virtual void UpdateStun()
        {
            if (isStunned)
            {
                stunTime--;

                if (stunTime <= 0)
                {
                    isStunned = false;
                    stunTime = 0;
                }
            }
        }

        public virtual void Update(int mapwidth)
        {

            UpdateStun();

            if (hitPauseTimer >= 0)
                hitPauseTimer--;

            //--Implement forces
            ImplementGravity();
            UpdateKnockBack();
            UpdateShakeHealthBar();

            #region Update Rectangles
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            if (hasBeenHit)
            {
                targetHealthWidth = (int)((float)originalHealthWidth * ((float)health / (float)maxHealth));
                addToHealthWidth += ((targetHealthWidth - healthBarRec.Width) * .07f); //Must add it to this because converting it to a int cuts off numbers
                healthBarRec.Width = (int)addToHealthWidth;
            }


            #endregion

            #region Dont run off map
            if (position.X + rectanglePaddingLeftRight <= 0)
            {
                position.X = -(rectanglePaddingLeftRight);
            }

            if (position.X + rec.Width - rectanglePaddingLeftRight >= mapwidth)
            {
                position.X = mapwidth - rec.Width + rectanglePaddingLeftRight;
            }
            #endregion

        }
    }
}
