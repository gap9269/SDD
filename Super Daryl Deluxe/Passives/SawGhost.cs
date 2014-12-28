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
    public class SawGhost : Passive
    {
        Rectangle ghostRec;
        Boolean facingRight;
        Vector2 seekPoint;
        Boolean active = true;
        static Random randomActiveTime;
        int timeActive;
        int maxTimeActive;
        float alpha = 1f;
        Vector2 velocity, position;
        int speed;
        Vector2 fwd;
        int maxSpeed;
        int maxForce;
        float mass;
        Vector2 rightVector;

        MapClass lastMap;

        List<int> hitCooldowns;
        List<Enemy> enemiesThatCantBeHitYet;

        Boolean alphaIncreasing = false;

        float verticalDistanceToPlayer, horizontalDistanceToPlayer;

        Boolean floatUp = false;
        float yOffset = 0;

        public int Speed { get { return speed; } set { speed = value; } }
        public Vector2 Fwd { get { return fwd; } set { fwd = value; fwd.Normalize(); } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        public int MaxForce { get { return maxForce; } set { maxForce = value; } }


        public SawGhost(Game1 g)
            : base(g)
        {
            name = "Ghost in the Saw";
            seekPoint = new Vector2();
            fwd = new Vector2(1, 1);
            velocity = new Vector2(1, 0);
            position = Vector2.Zero;
            mass = 1f;
            maxSpeed = 150;
            maxForce = 150;

            ghostRec = new Rectangle(0, 0, 139, 123);

            randomActiveTime = new Random();
            timeActive = randomActiveTime.Next(2000, 10000);
            active = false;

            hitCooldowns = new List<int>();
            enemiesThatCantBeHitYet = new List<Enemy>();
        }

        public override void LoadPassive()
        {
            base.LoadPassive();

            spriteSheet = content.Load<Texture2D>(@"SpriteSheets\Passives\ghost");
        }

        public override void Update()
        {
            base.Update();


            //Update the lists for enemies that can't be hit again for some period of time
            for (int i = 0; i < hitCooldowns.Count; i++)
            {
                hitCooldowns[i]--;

                //Once the cooldown for that enemy is up, remove the cooldown and the enemy from the list of
                //enemies that can't be hit
                if (hitCooldowns[i] <= 0)
                {
                    hitCooldowns.RemoveAt(i);
                    enemiesThatCantBeHitYet.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            //Update the last map and make the ghost go inactive if the map has changed
            if (lastMap == null)
            {
                lastMap = game.CurrentChapter.CurrentMap;
            }

            if (lastMap != game.CurrentChapter.CurrentMap)
            {
                active = false;
                timeActive = randomActiveTime.Next(2000, 10000);

                lastMap = game.CurrentChapter.CurrentMap;
            }


            //The ghost is always active in the shed
            if (game.CurrentChapter.CurrentMap.MapName == "Old Shed")
            {
                active = true;
                timeActive = 10;
            }

            //If not active, decrease cooldown until active
            if (!active)
            {
                timeActive--;

                if (timeActive == 0)
                {
                    maxTimeActive = randomActiveTime.Next(200, 600);
                    active = true;

                    //Place the ghost a bit behind the seekPoint when it becomes active. Stuff freaks out if you place it right on its destination
                    if (Game1.Player.FacingRight)
                        position = new Vector2(Game1.Player.PositionX - 200, Game1.Player.PositionY + 200);
                    else
                        position = new Vector2(Game1.Player.PositionX + Game1.Player.Rec.Width - ghostRec.Width + 200, Game1.Player.PositionY + 200);
                }
            }
            else
            {
                timeActive++;

                //Only make it go inactive once it has faded out and the cooldown is up
                if (timeActive >= maxTimeActive && alpha <= .01f)
                {
                    timeActive = randomActiveTime.Next(2000, 10000);
                    active = false;
                }

                #region float up and down
                if (floatUp)
                {
                    yOffset += 1f;

                    if (yOffset >= 50)
                    {
                        yOffset = 50;
                        floatUp = false;
                    }
                }
                else
                {
                    yOffset -= 1f;

                    if (yOffset <= -5)
                    {
                        yOffset = -5;
                        floatUp = true;
                    }
                }
                #endregion

                #region Fade in and out
                if (!alphaIncreasing)
                {
                    alpha -= .005f;

                    if (alpha <= 0)
                    {
                        alpha = 0;
                        alphaIncreasing = true;
                    }
                }
                else
                {
                    alpha += .005f;

                    if (alpha >= .8)
                    {
                        alpha = .8f;
                        alphaIncreasing = false;
                    }
                }
                #endregion

                UpdateRight();

                Vector2 steeringForce = calcSteeringForce();

                ClampSteeringForce(steeringForce);

                Vector2 accel = steeringForce / mass;

                velocity += (accel *= .06f);

                speed = (int)velocity.Length();

                Fwd = velocity;

                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                    velocity = Fwd * speed;
                }

                if (velocity.X > 0)
                    facingRight = true;
                else
                    facingRight = false;

                MoveSteering(velocity * .06f);


                ghostRec.X = (int)position.X;
                ghostRec.Y = (int)position.Y + -(int)yOffset;
            }

        }

        public override void CheckEnemyCollisions(Enemy en)
        {
            base.CheckEnemyCollisions(en);

            //If the ghost is active and hits an enemy, do one damage to cause hostility.
            if (active && ghostRec.Intersects(en.VitalRec) && en.CanBeHit && !enemiesThatCantBeHitYet.Contains(en) && Vector2.Distance(Game1.Player.Position, en.Position) < 600)
            {
                en.TakeHit(1, new Vector2(1, 1), Rectangle.Intersect(en.VitalRec, ghostRec), AttackType.AttackTypes.none, AttackType.RangedOrMelee.none);

                //Add that enemy to a list of enemies that can't be hit, then wait 400 frames before being able to hit it again
                //This code is updated at the top of Update()
                hitCooldowns.Add(400);
                enemiesThatCantBeHitYet.Add(en);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (active)
            {
                if (facingRight)
                    s.Draw(spriteSheet, ghostRec, Color.White * alpha);
                else
                    s.Draw(spriteSheet, ghostRec, null, Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }

        }

        public void MoveSteering(Vector2 moveVec)
        {

            //Clamp the velocity so the ghost doesn't shake back and forth when he's close to his target
            if (moveVec.X > 0 && moveVec.X < 1)
            {
                moveVec.X = 0;

                if (Game1.Player.VitalRec.Center.X > ghostRec.Center.X)
                    facingRight = true;
                else
                    facingRight = false;
            }
            if (moveVec.X < 0 && moveVec.X > -1)
            {
                moveVec.X = 0;

                if (Game1.Player.VitalRec.Center.X > ghostRec.Center.X)
                    facingRight = true;
                else
                    facingRight = false;
            }

            if (horizontalDistanceToPlayer > 210)
            {
                position += moveVec;
            }
        }

        public Vector2 Seek(Vector2 targetPos)
        {
            Vector2 sf = new Vector2();
            Vector2 desiredVel = new Vector2();

            desiredVel = targetPos - position;

            desiredVel.Normalize();
            desiredVel *= maxSpeed;

            sf = desiredVel - velocity;

            return sf;
        }

        //Keeps the velocity clamped
        public void ClampSteeringForce(Vector2 force)
        {
            float length = force.Length();

            if (length > maxForce)
            {
                force /= length;
                force *= maxForce;
            }
        }

        public virtual Vector2 calcSteeringForce()
        {
            Vector2 steeringForce = new Vector2();

            verticalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(0, Game1.Player.PositionY + 200), new Vector2(0, ghostRec.Center.Y)));
            horizontalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(Game1.Player.VitalRec.Center.X, 0), new Vector2(ghostRec.Center.X, 0)));

            if (horizontalDistanceToPlayer > 210)
            {
                //Set the target point
                if (Game1.Player.FacingRight)
                    seekPoint = new Vector2(Game1.Player.PositionX, Game1.Player.PositionY + 200);
                else
                    seekPoint = new Vector2(Game1.Player.PositionX + Game1.Player.Rec.Width - ghostRec.Width, Game1.Player.PositionY + 200);

                steeringForce += Seek(seekPoint);

                float dist = Vector2.Distance(position, seekPoint);
                float radius = 150;
                if (dist < radius)
                {
                    velocity.Normalize();
                    velocity *= maxSpeed * (dist / radius);
                }

            }
            return steeringForce;
        }

        public void UpdateRight()
        {
            rightVector = new Vector2(-velocity.Y, velocity.X);
        }

        public float DOT(Vector2 vec1, Vector2 vec2)
        {
            return (vec1.X * vec2.X + vec1.Y * vec2.Y);
        }

        public void RotateVector(ref Vector2 vec, double degree)
        {
            double radian = degree * (Math.PI / 180);
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            double tempX = vec.X;
            vec.X = (float)(vec.X * cos - vec.Y * sin);
            vec.Y = (float)(vec.Y * cos + tempX * sin);
        }
    }
}
