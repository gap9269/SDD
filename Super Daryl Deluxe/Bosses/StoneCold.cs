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
    class StoneCold : Boss
    {
        int punchTimer;
        int backAwayTimer;
        int standTime;
        int runTimer;

        Boolean punching = false;
        Boolean running = false;
        Boolean spawningLawyers;

        Rectangle punchRec;

        int damageState = 0;

        enum Intentions
        {
            none,
            run,
            spawnLawyers,
            waitForLawyers,
            falling,
            punch,
            stand
        }
        Intentions intentions;

        public Boolean SpawningLawyers { get { return spawningLawyers; } set { spawningLawyers = value; } }

        // CONSTRUCTOR \\
        public StoneCold(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 0;
            maxHealth = 80;
            level = 5;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, 171, 270);
            currentlyInMoveState = false;
            tolerance = 8;
            vitalRec = new Rectangle((int)position.X, (int)position.Y, 100, 200);
            moveSpeed = 4;

            addToHealthWidth = 0;
            healthBarRec.Width = 0;
            intentions = Intentions.punch;
        }

        public override Rectangle GetHealthSourceRectangle()
        {

            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            switch (bossState)
            {
                case BossState.attacking:
                    return new Rectangle(1160, 0, 171, 270);
                default:
                    return new Rectangle(1331, 0, 171, 270);
            }
        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0)
            {
                player.Experience += experienceGiven;
                Chapter.effectsManager.AddSmokePoof(vitalRec, 1);
                return true;
            }

            return false;
        }


        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            //Go to spawn lawyers at certain health states
            if ((health <= maxHealth * .75f && damageState == 0) || (health <= maxHealth * .50f && damageState == 1) || (health <= maxHealth * .25f && damageState == 2))
            {
                running = false;
                bossState = BossState.moving;
                intentions = Intentions.spawnLawyers;
            }

            Move(currentMap.MapWidth);

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            if (!running)
                CheckWalkCollisions(3, new Vector2(10, -10));
            else
                CheckWalkCollisions(5, new Vector2(11, -10));

            if (!facingRight)
                vitalRec.X = rec.X + 70;
            else
                vitalRec.X = rec.X;
            vitalRec.Y = rec.Y;

            if (facingRight)
            {
                punchRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y + 20, 80, 50);
            }
            else
            {
                punchRec = new Rectangle(vitalRec.X-80, vitalRec.Y + 20, 80, 50);
            }


            if (IsDead())
            {
                game.CurrentChapter.BossFight = false;
                game.CurrentChapter.CurrentBoss = null;
            }
        }

        public void StartAttack(string attackType)
        {
            if (attackType == "Punch")
            {
                bossState = BossState.attacking;
                punching = true;
                punchTimer = 0;
            }
        }

        public void Attack()
        {
            #region Punching
            if (punching)
            {
                punchTimer++;

                if (punchTimer > 10)
                {
                    if (player.CheckIfHit(punchRec) && player.InvincibleTime <= 0)
                    {
                        player.TakeDamage(12);


                        if (facingRight)
                            player.KnockPlayerBack(new Vector2(20, -5));
                        else
                            player.KnockPlayerBack(new Vector2(-20, -5));

                        player.HitPauseTimer = 3;
                        hitPauseTimer = 3;
                        game.Camera.ShakeCamera(5, 5);
                    }
                }

                if (punchTimer == 30)
                {
                    bossState = BossState.standing;
                    punching = false;
                    intentions = Intentions.stand;
                    standTime = 150;
                }
            }
            #endregion

            else if (running)
            {
                if (runTimer > 0)
                {
                    runTimer--;

                    if (runTimer == 0)
                    {
                        runTimer = 0;
                        running = false;
                        bossState = BossState.standing;
                        intentions = Intentions.none;
                    }

                    if (facingRight)
                        PositionX += moveSpeed * 3;
                    else
                        PositionX -= moveSpeed * 3;
                }
            }
            else if (backAwayTimer > 0)
            {

                BackAway();
            }
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);

            if (player.CheckIfHit(vitalRec) && player.InvincibleTime <= 0)
            {
                player.HitPauseTimer = 3;
                hitPauseTimer = 3;
                game.Camera.ShakeCamera(5, 5);
            }
        }

        public void BackAway()
        {
            backAwayTimer--;

            if (CollideWithBounds())
                backAwayTimer = 0;

            if (backAwayTimer == 0)
            {
                running = true;
                runTimer = 120;
            }

            if (facingRight)
                PositionX -= moveSpeed;
            else
                PositionX += moveSpeed;
        }

        public override void Move(int mapWidth)
        {
            distanceFromPlayer = Vector2.Distance(new Vector2(VitalRec.Center.X, vitalRec.Center.Y), new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y));

            #region If he's too far away without any action planned, move towards the player
            if (distanceFromPlayer > 700 && intentions == Intentions.none)
            {
                if (player.VitalRec.Center.X > vitalRec.Center.X)
                {
                    facingRight = true;
                    PositionX += moveSpeed;
                    bossState = BossState.moving;
                }
                else if (player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    facingRight = false;
                    PositionX -= moveSpeed;
                    bossState = BossState.moving;
                }
            }
            #endregion

            #region If not stunned, surprised, or confused, and is standing, choose a random intent
            if ((bossState == BossState.standing || bossState == BossState.moving) && intentions == Intentions.none && distanceFromPlayer <= 700 && !knockedBack)
            {

                int intent = chooseIntent.Next(8);

                //RUN
                if (intent == 0 || intent == 3 || intent == 4)
                    intentions = Intentions.run;

                //STAND
                else if (intent == 1 || intent == 2)
                {
                    intentions = Intentions.stand;
                    standTime = randomTime.Next(30, 80);
                }
                //PUNCH
                else
                    intentions = Intentions.punch;
            }
            #endregion

            if (intentions == Intentions.stand)
            {
                standTime--;
                bossState = BossState.standing;
                if (standTime == 0)
                    intentions = Intentions.none;
            }

            #region INTENT TO RUN
            if (intentions == Intentions.run && bossState != BossState.attacking)
            {
                //Start backing away first, then charge the player
                bossState = BossState.attacking;
                backAwayTimer = 60;

                if (player.VitalRec.Center.X > vitalRec.Center.X)
                    facingRight = true;
                else
                    facingRight = false;
            }
            #endregion

            #region INTENT TO PUNCH YOU IN THE DICK
            if (intentions == Intentions.punch && bossState != BossState.attacking)
            {
                if ((distanceFromPlayer <= 170))
                {
                    StartAttack("Punch");
                    bossState = BossState.attacking;
                    punchTimer = 0;

                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                        facingRight = true;
                    else
                        facingRight = false;
                }

                else
                {
                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                    {
                        facingRight = true;
                        PositionX += moveSpeed;
                        bossState = BossState.moving;
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;
                        PositionX -= moveSpeed;
                        bossState = BossState.moving;
                    }
                }
            }
            #endregion

            if (intentions == Intentions.spawnLawyers && VelocityY == 0)
            {
                canBeHurt = false;
                canBeKnockbacked = false;

                if (vitalRec.Center.X > 1050)
                {
                    PositionX -= moveSpeed * 2;
                }
                else if (vitalRec.Center.X < 950)
                {
                    PositionX += moveSpeed * 2;
                }
                else
                {
                    VelocityY = -32;
                    damageState++;
                    spawningLawyers = true;
                    intentions = Intentions.waitForLawyers;
                }
            }

            if (intentions == Intentions.waitForLawyers && spawningLawyers == false && currentMap.EnemiesInMap.Count == 0)
            {
                intentions = Intentions.falling;
                PositionY += 30;
                currentPlat = null;
            }

            if (intentions == Intentions.falling && currentPlat != null)
            {
                intentions = Intentions.none;
                canBeHurt = true;
                canBeKnockbacked = true;
            }

            if (bossState == BossState.attacking)
            {
                Attack();
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.whiteFilter, punchRec, Color.Red * .6f);
            //s.Draw(Game1.whiteFilter, vitalRec, Color.Blue * .6f);

            if (!facingRight)
            {
                s.Draw(game.EnemySpriteSheets["Garden Beast"], rec, GetSourceRectangle(moveFrame), Color.White);
            }
            else
            {
                s.Draw(game.EnemySpriteSheets["Garden Beast"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}