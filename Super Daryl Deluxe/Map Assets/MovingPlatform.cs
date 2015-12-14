using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    public class MovingPlatform : Platform
    {
        protected List<Vector2> path;
        int pointIndex;
        protected Rectangle centerOfPlat;
        protected Vector2 centerPos;
        int speed;
        protected float stopMag;

        public int PointIndex { get { return pointIndex; } set { pointIndex = value; } }

        SoundEffectInstance object_platform_loop;


        public MovingPlatform(Texture2D t, Rectangle r, bool pass, bool spawn, bool invis, List<Vector2> p, int s, float stopMagnitude, PlatformType platType = PlatformType.rock)
            : base(t, r, pass, spawn, invis, platType)
        {
            speed = s;
            stopMag = stopMagnitude;
            position.X = rec.X;
            position.Y = rec.Y;
            path = p;
            centerOfPlat = new Rectangle(0, 0, 5, 5);
            centerOfPlat.X = rec.X;
            centerOfPlat.Y = rec.Y;

            centerPos.X += centerOfPlat.X;
            centerPos.Y += centerOfPlat.Y;

            object_platform_loop = Sound.permanentSoundEffects["object_platform_loop"].CreateInstance();
        }

        public override void StopSound()
        {
            object_platform_loop.Stop();
        }

        public override void Update()
        {
            position.X += velocity.X;
            position.Y += velocity.Y;

            centerPos.X += velocity.X;
            centerPos.Y += velocity.Y;

            rec.X = (int)position.X;
            rec.Y = (int)position.Y;


            centerOfPlat.X = (int)centerPos.X;
            centerOfPlat.Y = (int)centerPos.Y;

            if (path.Count > 0)
            {

                if (object_platform_loop.State != SoundState.Playing)
                {
                    Sound.PlaySoundInstance(object_platform_loop, Game1.GetFileName(() => object_platform_loop), true, rec.Center.X, rec.Center.Y, 600, 500, 3000);
                }

                Vector2 nextPoint = path[pointIndex];
                velocity = Seek(nextPoint);

                float dist = (centerPos - nextPoint).Length();
			    float radius = stopMag;
                
			    if (dist < radius)
			    {
			    	velocity.Normalize();
			    	velocity *= (speed * dist / radius);
			    }

                if (centerOfPlat.Intersects(new Rectangle((int)nextPoint.X, (int)nextPoint.Y, 10, 10)))
                {
                    if (pointIndex == path.Count - 1)
                        pointIndex = 0;
                    else
                        pointIndex++;
                }
            }
        }

        public Vector2 Seek(Vector2 target)
        {
            Vector2 targetPos = new Vector2();
            Vector2 desiredForce = new Vector2();

            targetPos = target;
            desiredForce = targetPos - centerPos;
            desiredForce.Normalize();
            desiredForce *= speed;

            return desiredForce;
        }

        public Vector2 SeekWithCustomSpeed(Vector2 target, int sp)
        {
            Vector2 targetPos = new Vector2();
            Vector2 desiredForce = new Vector2();

            targetPos = target;
            desiredForce = targetPos - centerPos;
            desiredForce.Normalize();
            desiredForce *= sp;

            return desiredForce;
        }
    }
}
