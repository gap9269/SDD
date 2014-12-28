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
    public class MelonEnemy : Enemy
    {
        public MelonEnemy(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, "Garden Beast", g, ref play, cur)
        {
            health = 20;
            maxHealth = 20;
            level = 1;
            experienceGiven = 1;
            rec = new Rectangle((int)position.X, (int)position.Y, 92, 200);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 2;
            maxHealthDrop = 3;
            moneyToDrop = .07f;
            vitalRec = new Rectangle(100, 100, 92, 110);
            displayName = "Flying Melon";
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(252, 0, 92, 200);
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            Move(mapwidth);

            vitalRec.X = rec.X + (rec.Width / 4) - 10;
            vitalRec.Y = rec.Y + (rec.Height / 4) + 10;
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);
            if (isStunned == false)
            {
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(10, 200);
                }

                switch (moveState)
                {
                    case 0:
                        enemyState = EnemyState.standing;

                        currentlyInMoveState = true;
                        moveTimer--;
                        break;

                    case 1:
                        facingRight = true;
                        enemyState = EnemyState.moving;
                        if (currentlyInMoveState == false)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X <= mapWidth - 6)
                            position.X += enemySpeed;
                        break;

                    case 2:
                        facingRight = false;
                        enemyState = EnemyState.moving;
                        if (currentlyInMoveState == false)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X >= 6)
                            position.X -= enemySpeed;
                        break;

                }

                if (moveTimer <= 0)
                    currentlyInMoveState = false;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void DropItem()
        {
            base.DropItem();
        }
    }
}