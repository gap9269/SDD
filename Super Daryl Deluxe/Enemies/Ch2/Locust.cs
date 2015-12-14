﻿using System;
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
    class Locust : SteeringEnemy
    {
        Boolean attacking = false;
        int loopsBeforeBlink = 0; //At 3, blink once and reset
        Boolean blinking = false;
        Boolean isLeader;
        int randomPoint = 0;
        List<Vector2> playerAttackPoints;
        int damage = 30;
        public Locust(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound)
            : base(pos, type, g, ref play, cur, bound)
        {
            maxSpeed = 150;
            maxForce = 150;
            mass = 1f;

            wanderAng = 0;
            wanderRad = 8;
            wanderDist = 15;
            wanderMax = 20;

            health = 400;
            maxHealth = 400;
            level = 13;
            experienceGiven = 15;
            rec = new Rectangle((int)position.X, (int)position.Y, 80, 117);
            tolerance = 30;
            vitalRec = rec;
            maxHealthDrop = 10;
            moneyToDrop = .05f;

            playerAttackPoints = new List<Vector2>() { new Vector2(), new Vector2(), new Vector2(), new Vector2(), new Vector2(), new Vector2(), new Vector2() };
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(0, 0, 80, 117);
            else
                return new Rectangle((80 * moveFrame) + 80, 0, 80, 117);
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

        public override void Update(int mapWidth)
        {
            base.Update(mapWidth);
            if (!respawning && !isStunned)
            {
                if (hitPauseTimer <= 0)
                {
                    UpdatePlayerAttackPoints();

                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        frameDelay = 4;
                        moveFrame++;

                        if (moveFrame > 2)
                            moveFrame = 0;
                    }

                    if (attacking)
                        CheckWalkCollisions(damage, new Vector2(3, -5));
                    else
                        CheckWalkCollisions((int)(damage * .8f), new Vector2(3, -5));
                }
            }
            vitalRec.X = rec.X;
            vitalRec.Y = rec.Y;
            deathRec = vitalRec;
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

        public override Vector2 FollowLeader()
        {
            isLeader = false;
            Locust leaderLocust;

            for (int i = 0; i < currentMap.EnemiesInMap.Count; i++)
            {
                if (currentMap.EnemiesInMap[i] is Locust && (currentMap.EnemiesInMap[i] as Locust).hostile)
                {
                    if (this != currentMap.EnemiesInMap[i])
                    {
                        leaderLocust = (currentMap.EnemiesInMap[i] as Locust);
                        bounds = leaderLocust.Rec;
                        break;
                    }
                    else
                    {
                        isLeader = true;
                        break;
                    }
                }
            }

            if (isLeader)
                return Wander() * 5;

            return new Vector2();
        }

        public override Vector2 calcSteeringForce()
        {
            Vector2 sf = new Vector2();

            if (!hostile)
            {
                sf += Wander() * 5;
                sf += (CheckBoundaries() * 15);

                maxSpeed = 90;
                maxForce = 90;
                mass = 1f;
                wanderRad = 4;
            }
            else
            {


                if (!attacking)
                {
                    attackCooldown--;

                    if (attackCooldown <= 0)
                    {
                        attacking = true;
                        randomPoint = moveNum.Next(7);
                    }

                    if (player.playerState == Player.PlayerState.running)
                        bounds = new Rectangle(player.RecX, player.RecY - 100, player.Rec.Width, 100);

                    sf += Wander() * 5;
                    sf += CheckBoundaries() * 15;
                    sf += calcSeparation(150);

                    maxSpeed = 150;
                    maxForce = 150;
                    mass = 1f;
                    wanderRad = 4;
                }
                else
                {
                    maxSpeed = 200;
                    maxForce = 200;
                    mass = .5f;
                    wanderRad = 4;

                    sf += Seek(playerAttackPoints[randomPoint]) * 10;
                    sf += calcSeparation(50);
                    sf += Seek(calcCohesion());

                    if (Math.Abs(position.X - playerAttackPoints[randomPoint].X) < 50 && Math.Abs(position.Y - playerAttackPoints[randomPoint].Y) < 50)
                    {
                        randomPoint++;
                    }

                    if (randomPoint == 7)
                    {
                        randomPoint = 0;
                        attacking = false;
                        attackCooldown = moveNum.Next(300, 600);
                    }

                }
            }


            return sf;

        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.emptyBox, vitalRec, Color.Black);

            #region Draw Enemy
            if (facingRight)
                s.Draw(game.EnemySpriteSheets["Locust"], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
            else
                s.Draw(game.EnemySpriteSheets["Locust"], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
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

                if (facingRight)
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);

            // if (enemyState == EnemyState.attacking)
            //s.Draw(Game1.emptyBox, attackRec, Color.Blue);
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 40)
            {
               // currentMap.Drops.Add(new EnemyDrop("Crow Feather", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
