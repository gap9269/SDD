using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class LivingLocker : InteractiveObject
    {
        public enum movestate
        {
            flying, falling, landed, gettingUp
        }
        public movestate mState;

        int moveFrame;
        int timeOnGround = 600;
        Boolean drawFButton = false;

        Boolean facingRight;
        protected int speed;
        protected Vector2 fwd;
        protected Vector2 velocity;
        protected Vector2 position;
        protected int maxSpeed;
        protected int maxForce;
        protected float mass;
        protected Rectangle bounds;
        protected Vector2 rightVector;

        protected double wanderAng;
        protected int wanderRad;
        protected int wanderDist;
        protected int wanderMax;
        protected Random wanderRandom;

        public int Speed { get { return speed; } set { speed = value; } }
        public Vector2 Fwd { get { return fwd; } set { fwd = value; fwd.Normalize(); } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        public int MaxForce { get { return maxForce; } set { maxForce = value; } }

        public LivingLocker(Game1 g, Rectangle bnds)
            :base(g, false)
        {

            sprite = Game1.flyingLockerSprite;
            bounds = bnds;
            bounds.Height -= 307;
            rec = new Rectangle(bounds.X + 2, bounds.Y + 2, 240, 266);
            mState = movestate.flying;
            health = 1;

            wanderRandom = new Random();
            fwd = new Vector2(1, 1);

            //--Wander values
            maxSpeed = 75;
            maxForce = 75;
            mass = .8f;

            wanderAng = 0;
            wanderRad = 8;
            wanderDist = 25;
            wanderMax = 30;

            vitalRec = new Rectangle(100, 33, 45, 192);
        }

        public virtual Vector2 calcSteeringForce()
        {
            Vector2 sf = new Vector2();

                sf += Wander();
                sf += (CheckBoundaries() * 2);

            return sf;
        }

        public override Rectangle GetSourceRec()
        {
            switch (mState)
            {
                case movestate.falling:
                    return new Rectangle(480, 0, 240, 266);
                case movestate.landed:
                    return new Rectangle(moveFrame * 240, 0, 240, 266);
                case movestate.gettingUp:
                    return new Rectangle(2880 + (moveFrame * 240), 0, 240, 266);
                case movestate.flying:
                    return new Rectangle(720 + (moveFrame * 240), 0, 240, 266);
            }

            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            switch (mState)
            {
                #region Flying
                case movestate.flying:

                    //--If it has been hit, fall
                    if (health <= 0)
                    {
                        mState = movestate.falling;
                        velocity.Y = 0;
                        velocity.X = 0;
                    }

                    #region Animation
                    frameTimer--;

                    if (frameTimer == 0)
                    {
                        moveFrame++;
                        frameTimer = 5;

                        if (moveFrame > 8)
                            moveFrame = 0;
                    }
                    #endregion

                    #region Wander
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


                    if (velocity.X > 0)
                        facingRight = true;
                    else
                        facingRight = false;
                    #endregion

                    break;
                #endregion

                #region Falling
                case movestate.falling:

                    velocity.Y += GameConstants.GRAVITY;
                    position.Y += velocity.Y;

                    //--Clamp velocity
                    if (velocity.Y > 25)
                        velocity.Y = 25;

                    //--Hit a platform
                    for (int i = 0; i < game.CurrentChapter.CurrentMap.Platforms.Count; i++)
                    {
                        //Represents the platform and the sides of it
                        Platform plat = game.CurrentChapter.CurrentMap.Platforms[i];
                        Rectangle top = new Rectangle(plat.Rec.X + 5, plat.Rec.Y, plat.Rec.Width - 5, 20);

                        Rectangle lockerBot = new Rectangle(rec.X + 20, rec.Y + rec.Height - 5, rec.Width - 40, 25);

                        if (lockerBot.Intersects(top))
                        {
                            mState = movestate.landed;
                            velocity.Y = 0;
                            frameTimer = 8;
                            moveFrame = 0;
                        }
                    }

                    break;
                #endregion

                #region Landed
                case movestate.landed:

                    timeOnGround--;

                    Rectangle frec;

                    if(facingRight)
                        frec = new Rectangle((rec.X + rec.Width / 2 - 43 / 2) + 25, rec.Y - 65, 43, 65);
                    else
                        frec = new Rectangle((rec.X + rec.Width / 2 - 43 / 2) - 25, rec.Y - 65, 43, 65);

                    //--When he first hits the ground he squishes a bit before he pop backs up. It only lasts 8 frames
                    if (moveFrame == 0)
                    {
                        frameTimer--;

                        if (frameTimer == 0)
                        {
                            moveFrame++;
                            frameTimer = 8;
                        }
                    }
                    //--If he isn't squished
                    else
                    {

                        #region Draw the F Button if you are intersecting with him
                        if (Game1.Player.VitalRec.Intersects(vitalRec) && !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game && game.CurrentChapter.BossFight == false)
                            drawFButton = true;
                        else
                            drawFButton = false;

                        if (drawFButton)
                        {
                            if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                                Chapter.effectsManager.AddFButton(frec);
                        }
                        else
                        {
                            if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                                Chapter.effectsManager.fButtonRecs.Remove(frec);
                        }
                        #endregion

                        //If you press F, go to your locker
                        if (Game1.Player.VitalRec.Intersects(vitalRec) && current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F) && Game1.Player.LearnedSkills.Count > 0 /*&& game.CurrentChapter.BossFight == false*/)
                        {
                            game.YourLocker.LoadContent();
                            game.CurrentChapter.state = Chapter.GameState.YourLocker;
                        }
                    }

                    //--Once he has been on the ground for 10 seconds, jump back up and remove the fRec so it doesn't get stuck on screen
                    if (timeOnGround == 0)
                    {
                        mState = movestate.gettingUp;
                        timeOnGround = 600;
                        moveFrame = 0;

                        if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                            Chapter.effectsManager.fButtonRecs.Remove(frec);
                    }
                    break;
                #endregion

                #region Getting Up
                case movestate.gettingUp:
                        frameTimer--;

                        if (frameTimer == 0)
                        {
                            moveFrame++;
                            frameTimer = 20;

                            if (moveFrame > 1)
                            {
                                mState = movestate.flying;
                                health = 1;
                            }
                        }

                        if (moveFrame == 1)
                        {
                            velocity.Y = -10;
                            position.Y += velocity.Y;
                            health = 1;
                        }
                    

                    break;
                #endregion
            }

            //Update rectangles
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;
            vitalRec = new Rectangle(rec.X + 100, rec.Y + 33, 45, 192);

        }

        public override void Draw(SpriteBatch s)
        {

            //--When on the ground
            if (mState == movestate.landed && moveFrame == 1)
            {

                #region Draw name above locker when downed
                Point distanceFromLocker = new Point(Math.Abs(Game1.Player.VitalRec.Center.X - vitalRec.Center.X), Math.Abs(Game1.Player.VitalRec.Center.Y - vitalRec.Center.Y));

                if (distanceFromLocker.X < 250 && distanceFromLocker.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game 
                    && game.CurrentChapter.BossFight == false && !drawFButton)
                {
                    s.DrawString(game.Font, "Daryl's Locker / Skill Shop", new Vector2(vitalRec.X - 50 -2, vitalRec.Y - 50 - 2), Color.Black);
                    s.DrawString(game.Font, "Daryl's Locker / Skill Shop", new Vector2(vitalRec.X - 50, vitalRec.Y - 50), Color.White);
                }
                #endregion
            }

            if(facingRight)
                s.Draw(sprite, rec, GetSourceRec(), Color.White);
            else
                s.Draw(sprite, rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

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

        public Vector2 Wander()
        {
            wanderAng += wanderRandom.NextDouble() * wanderMax * 2 - wanderMax;

            Vector2 seekPt = position + (fwd * wanderDist);

            Vector2 offset = Fwd * wanderRad;
            RotateVector(ref offset, wanderAng);
            seekPt += offset;
            return Seek(seekPt);

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
            if (((distance - obstacleRadius) - (vitalRec.Width / 2)) > safeDistance)
            {
                return new Vector2();
            }

            //--If the object is behind me
            if (DOT(vectorToObstacleCenter, Fwd) > 0)
            {
                return new Vector2();
            }

            float rightDotVTOC = DOT(vectorToObstacleCenter, rightVector);

            if ((obstacleRadius + (vitalRec.Width / 2)) < Math.Abs(rightDotVTOC))
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
