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
    class SteeringEnemy: Enemy
    {
        protected int speed;
        protected Vector2 fwd;
        protected int maxSpeed;
        protected int maxForce;
        protected float mass;
        protected Rectangle bounds;
        protected Vector2 rightVector;

        protected double wanderAng;
        protected int wanderRad;
        protected int wanderDist;
        protected int wanderMax;
        protected static Random wanderRandom;

        public int Speed { get { return speed; } set { speed = value; } }
        public Vector2 Fwd { get { return fwd; } set { fwd = value; fwd.Normalize(); } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        public int MaxForce { get { return maxForce; } set { maxForce = value; } }

        public SteeringEnemy(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound)
            : base(pos, type, g, ref play, cur)
        {
            bounds = bound;
            wanderRandom = new Random();
            fwd = new Vector2(1,1);
        }

        public virtual Vector2 calcSteeringForce()
        {
            Vector2 steeringForce = new Vector2();

            return steeringForce;
        }

        public virtual Vector2 calcCohesion()
        {
            Vector2 center = new Vector2();
            int numEnemies = 0;
			
			//tell the characters to do their update
			for(int i = 0; i < currentMap.EnemiesInMap.Count; i++)
			{
                if (currentMap.EnemiesInMap[i] is SteeringEnemy)
                {
                    center.X += (currentMap.EnemiesInMap[i].VitalRec.Center.X);
                    center.Y += (currentMap.EnemiesInMap[i].VitalRec.Center.Y);
                    numEnemies++;
                }
			}

            center /= (numEnemies);
			
			return center;
        }

        public override void Update(int mapwidth)
        {
 	        base.Update(mapwidth);

            if (!knockedBack && !isStunned)
            {
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

                MoveSteering(velocity * .06f);
            }

            if (VelocityX > 0)
                facingRight = true;
            else
                facingRight = false;
        }

        public void MoveSteering(Vector2 moveVec)
        {
            position += moveVec;
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

        public Vector2 CheckBoundaries()
        {
            Vector2 sf = new Vector2();

            if (bounds != null)
            {
                if (position.X < bounds.X || position.X > bounds.X + bounds.Width)
                {
                    Vector2 seekPt = new Vector2(bounds.X + bounds.Width / 2, position.Y);
                    sf += Seek(seekPt);
                }

                if (position.Y < bounds.Y || position.Y > bounds.Y + bounds.Height)
                {
                    Vector2 seekPt = new Vector2(position.X, bounds.Y + bounds.Height / 2);
                    sf += Seek(seekPt);

                }
            }

            return sf;
        }

        public virtual Vector2 FollowLeader()
        {
            if (this == currentMap.EnemiesInMap[0])
                return Wander() * 5;
            else
                bounds = currentMap.EnemiesInMap[0].Rec;

            return new Vector2();
        }

        public Vector2 Wander()
        {
            wanderAng += wanderRandom.NextDouble() * wanderMax * 2 - wanderMax;

            Vector2 seekPt = position + (fwd * wanderDist);

            Vector2 offset = Fwd * wanderRad;
            RotateVector(ref offset, wanderAng);
            seekPt += offset;
            return Seek(seekPt);

        }

        public Vector2 calcAlignment()
        {
			Vector2 sum = new Vector2();
			
			//tell the characters to do their update
            for (int i = 0; i < currentMap.EnemiesInMap.Count; i++)
			{
                if (currentMap.EnemiesInMap[i] is SteeringEnemy)
                {
                    sum += ((currentMap.EnemiesInMap[i] as SteeringEnemy).fwd);
                }
			}
			
			return sum;
        }

        public Vector2 Align()
		{
			Vector2 desVel = calcAlignment();
			desVel.Normalize();
			desVel *= (maxSpeed);
            desVel -= velocity;
            return desVel;
		}

        public Vector2 calcSeparation(int distance)
        {
            Vector2 sf = new Vector2(0,0);
            Vector2 temp = new Vector2();

            for (int i = 0; i < currentMap.EnemiesInMap.Count; i++)
            {
                if (this == currentMap.EnemiesInMap[i])
                    continue;
                if (Vector2.Distance(currentMap.EnemiesInMap[i].Position, this.position) <= distance)
                {
                    temp = Flee(currentMap.EnemiesInMap[i].Position);
                    temp.Normalize();
                    temp *= (1 / Vector2.Distance(currentMap.EnemiesInMap[i].Position, this.position) + 1);
                    sf += temp;
                }
            }

            if (sf != Vector2.Zero)
            {
                sf.Normalize();
            }
                sf *= maxSpeed;
                sf -= velocity;

                return sf;
        }

        public virtual Vector2 Seek(Vector2 targetPos)
        {
            Vector2 sf = new Vector2();
            Vector2 desiredVel = new Vector2();

            desiredVel = targetPos - position;

            if (desiredVel != Vector2.Zero)
                desiredVel.Normalize();

            desiredVel *= maxSpeed;

            sf = desiredVel - velocity;

            return sf;
        }

        public Vector2 Flee(Vector2 targetPos)
        {
            Vector2 sf = new Vector2();
            Vector2 desiredVel = new Vector2();

            desiredVel = position - targetPos;

            desiredVel.Normalize();
            desiredVel *= maxSpeed;

            sf = desiredVel - velocity;

            return sf;
        }

        public Vector2 Avoid(Vector2 obstaclePos, int obstacleRadius, int safeDistance)
        {
            Vector2 sf;
            Vector2 desVel;

            Vector2 vectorToObstacleCenter = obstaclePos - position;
            float distance = vectorToObstacleCenter.Length();

            //--If the object is farther away than my safe distance
            if (((distance - obstacleRadius) - (VitalRecWidth / 2)) > safeDistance)
            {
                return new Vector2();
            }

            //--If the object is behind me
            if (DOT(vectorToObstacleCenter, Fwd) > 0)
            {
                return new Vector2();
            }

            float rightDotVTOC = DOT(vectorToObstacleCenter, rightVector);

            if ((obstacleRadius + (VitalRecWidth / 2)) < Math.Abs(rightDotVTOC))
            {
                return new Vector2();
            }

            if (rightDotVTOC > 0)
            {
                desVel = rightVector * -(maxSpeed);
            }
            else
            {
                desVel = rightVector * maxSpeed;
            }

            sf = desVel - velocity;

            sf *= safeDistance / distance;

            return sf;
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
