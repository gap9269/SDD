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
    public class GameObject
    {
        protected Rectangle rec;
        protected Rectangle vitalRec;
        protected List<Vector2> endingDamageNumVecs; //This is where the number starts as soon as it is created
        protected Vector2 velocity;
        protected Vector2 position;
        protected List<Vector2> damageVecs;  //Positions for damage numbers above creature's head
        protected List<int> damageNums; //The damage numbers above their heads
        protected List<int> damageTimers; //The damage numbers above their heads
        protected List<float> damageAlphas; //The damage numbers above their heads
        protected List<String> weaknessStrengthOrNormal; //Use this to use a different font color depending on whether the attack was effective or not

       // protected List<Vector2> expVec;
        protected List<Vector2> moneyVec;
       // protected List<int> expNum;
        protected List<int> monNum;
        protected List<int> moneyTimers;
       // protected List<int> expTimers;

        protected float terminalVelocity = 30;
        protected SpriteFont font;
        protected float alpha;
        protected bool yScroll;
        protected bool isStunned = false;
        protected Boolean canBeStunned = true;
        protected int stunTime;

        protected int hitPauseTimer;

        public int HitPauseTimer { get { return hitPauseTimer; } set { hitPauseTimer = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public bool YScroll { get { return yScroll; } set { yScroll = value; } }
        public float PositionX { get { return position.X; } set { position.X = value; } }
        public float PositionY { get { return position.Y; } set { position.Y = value; } }
        public float Alpha { get { return alpha; } set { alpha = value; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }
        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public Rectangle VitalRec { get { return vitalRec; } set { vitalRec = value; } }
        public int VitalRecWidth { get { return vitalRec.Width; } set { vitalRec.Width = value; } }
        public int VitalRecHeight { get { return vitalRec.Height; } set { vitalRec.Height = value; } }
        public int VitalRecX { get { return vitalRec.X; } set { vitalRec.X = value; } }
        public int VitalRecY { get { return vitalRec.Y; } set { vitalRec.Y = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public float VelocityX { get { return velocity.X; } set { velocity.X = value; } }
        public float VelocityY { get { return velocity.Y; } set { velocity.Y = value; } }
        public GameObject()
        {
            alpha = 1f;
            damageVecs = new List<Vector2>();
            damageNums = new List<int>();
            endingDamageNumVecs = new List<Vector2>();
            damageTimers = new List<int>();
            damageAlphas = new List<float>();
            weaknessStrengthOrNormal = new List<string>();

            //expTimers = new List<int>();
            //expVec = new List<Vector2>();
            //expNum = new List<int>();
        }

        public virtual void Stun(int time)
        {
            if (canBeStunned && stunTime <= 0)
            {
                isStunned = true;
                stunTime = time;
            }
        }

        public virtual void Update()
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

        public virtual void StopSound()
        {

        }

        ////--Adds vectors and numbers to the lists, to display when an enemy dies.
        ////--This must be called as "player.addmoneyexpnums" in "enemy", otherwise the lists will be deleted when the enemy is deleted
        //public virtual void AddExpNums(int exp, Rectangle enemyRec, int vitalRecY)
        //{
        //    expVec.Add(new Vector2(enemyRec.X + enemyRec.Width / 2, vitalRecY));
        //    expNum.Add(exp);
        //    expTimers.Add(200);
        //}

        ////--Draws the money and exp numbers above where the enemy was killed
        //public virtual void DrawExpNums(SpriteBatch s)
        //{
        //    #region Exp
        //    for (int i = 0; i < expVec.Count; i++)
        //    {
        //        expTimers[i]--;

        //      //  if (expTimers[i] < 15 || (expTimers[i] > 20 && expTimers[i] < 35) || (expTimers[i] > 40 && expTimers[i] < 55) || (expTimers[i] > 60 && expTimers[i] < 75) || (expTimers[i] > 80 && expTimers[i] < 95) || (expTimers[i] > 100 && expTimers[i] < 115))
        //     //   {
        //            s.DrawString(Game1.xpFont, "+" + expNum[i].ToString() + "XP", new Vector2(expVec[i].X - Game1.xpFont.MeasureString("+" + expNum[i].ToString() + " XP").X / 2, expVec[i].Y - Game1.xpFont.MeasureString("+" + expNum[i].ToString() + "XP").Y), Color.White);
        //            expTimers[i]--;
        //     //   }

        //        if (expTimers[i] <= 0)
        //        {
        //            expVec.RemoveAt(i);
        //            expNum.RemoveAt(i);
        //            expTimers.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //    #endregion
        //}
         

        public virtual void Draw(SpriteBatch s)
        {
            //DrawExpNums(s);
        }

        public virtual Vector2 Seek(GameObject target)
        {
            Vector2 targetPos = new Vector2();
            Vector2 desiredForce = new Vector2();

            targetPos.X = target.vitalRec.Center.X;
            targetPos.Y = target.vitalRec.Center.Y;
            desiredForce = targetPos - position;
            desiredForce.Normalize();
            desiredForce *= 2;

            return desiredForce;
        }
    }
}
