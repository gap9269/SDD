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

    //--This enemy runs through knockbacks
    public class ChaseEnemy : Enemy
    {
        public ChaseEnemy(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 30;
            maxHealth = 30;
            level = 2;
            experienceGiven = 5;
            rec = new Rectangle((int)position.X, (int)position.Y, 128, 128);
            currentlyInMoveState = false;
            enemySpeed = 1;
            tolerance = 3;
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            Move(mapwidth);
            CheckWalkCollisions(5, new Vector2(10, -5));
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            if (hostile == false)
            {
                #region Random movement, not hostile
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

                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;
                            frameDelay = 5;
                        }

                        if (moveFrame > 7)
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

                        frameDelay--;
                        if (frameDelay == 0)
                        {
                            moveFrame++;
                            frameDelay = 5;
                        }

                        if (moveFrame > 7)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X >= 6)
                            position.X -= enemySpeed;
                        break;
                }


                if (moveTimer <= 0)
                    currentlyInMoveState = false;
                #endregion
            }
            else
            {
                //--If the player is to the left
                if (player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    facingRight = false;
                    enemyState = EnemyState.moving;
                    if (currentlyInMoveState == false)
                        moveFrame = 0;

                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 7)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    if (position.X >= 6)
                        position.X -= enemySpeed;
                }
                //Player to the right
                else
                {
                    facingRight = true;
                    enemyState = EnemyState.moving;
                    if (currentlyInMoveState == false)
                        moveFrame = 0;

                    frameDelay--;
                    if (frameDelay == 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 7)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    if (position.X <= mapWidth - 6)
                        position.X += enemySpeed;
                }
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

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            //--If this is the first time being hit, make him hostile
            if (hostile == false)
                hostile = true;
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 4);

            switch (dropType)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    RileysBow sun = new RileysBow();
                    currentMap.Drops.Add(new EnemyDrop(sun, new Rectangle(rec.X + 20, rec.Y + 20, dropDiameter, dropDiameter)));
                    break;
                case 3:
                    break;
            }
        }
    }
}
