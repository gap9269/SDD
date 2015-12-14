using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class TripLaser
    {
        public Rectangle rec;
        public int timeActive;
        public int timeInactive;
        public int maxTimeActive;
        public int maxTimeInactive;

        public List<Vector2> path;
        int pointIndex;

        public Boolean tripped = false;
        Boolean active = false;
        Vector2 velocity;
        Vector2 position;
        public int speed;

        public int tripTimer;

        public float stopMag;

        //Static permanent lasers
        public TripLaser(int x, int y)
        {
            rec = new Rectangle(x, y, 18, 18);
            active = true;
        }

        //Static moving lasers
        public TripLaser(int x, int y, List<Vector2> points, int spd, float stopMagnitude)
        {
            rec = new Rectangle(x, y, 18, 18);
            active = true;
            path = points;
            speed = spd;
            stopMag = stopMagnitude;
            position = new Vector2(x, y);
            velocity = new Vector2(0, 0);
        }

        //Blinking lasers
        public TripLaser(int x, int y, int maxActive, int maxInactive, bool actve, int time)
        {
            rec = new Rectangle(x, y, 18, 18);
            active = true;
            maxTimeActive = maxActive;
            maxTimeInactive = maxInactive;


            active = actve;

            if (active)
                timeActive = time;
            else
                timeInactive = time;
        }

        public Vector2 Seek(Vector2 target)
        {
            Vector2 targetPos = new Vector2();
            Vector2 desiredForce = new Vector2();

            targetPos = target;
            desiredForce = targetPos - position;
            desiredForce.Normalize();
            desiredForce *= speed;

            return desiredForce;
        }

        public void Update()
        {
            if (active)
            {
                switch (Game1.Player.playerState)
                {
                    case Player.PlayerState.jumping:
                    case Player.PlayerState.attackJumping:
                        if (Game1.Player.JumpingVitalRec.Intersects(rec))
                        {
                            tripped = true;
                            tripTimer = 80;
                        }
                        break;
                    case Player.PlayerState.standing:
                        if (Game1.Player.Ducking)
                        {
                            if (Game1.Player.DuckingVitalRec.Intersects(rec))
                            {
                                tripped = true;
                                tripTimer = 80;
                            }
                        }
                        else
                        {
                            if (Game1.Player.VitalRec.Intersects(rec))
                            {
                                tripped = true;
                                tripTimer = 80;
                            }
                        }
                        break;
                    default:
                            if (Game1.Player.VitalRec.Intersects(rec))
                            {
                                tripped = true;
                                tripTimer = 80;
                            }
                            break;
                }

                if (tripped)
                {
                    if (Game1.currentChapter.CurrentMap.MapName == "Super Secret Deer Base Alpha" && SuperSecretDeerBaseAlpha.laserTripped == false)
                    {
                        SuperSecretDeerBaseAlpha.laserTripped = true;
                    }
                }

                //FOR BLINKING LASERS
                if (maxTimeActive > 0)
                {
                    timeActive--;
                    if (timeActive <= 0)
                    {
                        active = false;
                        timeInactive = maxTimeInactive;
                    }
                }

                //FOR MOVING LASERS
                if(path != null)
                {
                    position.X += velocity.X;
                    position.Y += velocity.Y;

                    rec.X = (int)position.X;
                    rec.Y = (int)position.Y;

                    if (path.Count > 0)
                    {

                        Vector2 nextPoint = path[pointIndex];
                        velocity = Seek(nextPoint);

                        float dist = (position - nextPoint).Length();
                        float radius = stopMag;

                        if (dist < radius)
                        {
                            velocity.Normalize();
                            velocity *= (speed * dist / radius);
                        }

                        if (rec.Intersects(new Rectangle((int)nextPoint.X, (int)nextPoint.Y, 10, 10)))
                        {
                            if (pointIndex == path.Count - 1)
                                pointIndex = 0;
                            else
                                pointIndex++;
                        }
                    }
                }

                if (tripped)
                {
                    tripTimer--;

                    if (tripTimer == 0)
                        tripped = false;
                }
            }
            else
            {
                //FOR BLINKING LASERS
                if (maxTimeActive > 0)
                {
                    if (tripped)
                    {
                        tripTimer--;

                        if (tripTimer == 0)
                            tripped = false;
                    }

                    timeInactive--;
                    if (timeInactive == 0)
                    {
                        active = true;
                        timeActive = maxTimeActive;
                    }
                }

            }
        }

        public void Draw(SpriteBatch s)
        {
            if (active)
            {
                if (tripped)
                    s.Draw(Game1.whiteFilter, rec, Color.Black);
                else
                    s.Draw(Game1.whiteFilter, rec, Color.Red);
            }
        }

    }
}
