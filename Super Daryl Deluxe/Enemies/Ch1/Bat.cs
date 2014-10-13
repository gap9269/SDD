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
    class Bat : SteeringEnemy
    {

        public Bat(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound)
            : base(pos, type, g, ref play, cur, bound)
        {
            maxSpeed = 75;
            maxForce = 75;
            mass = .8f;

            wanderAng = 0;
            wanderRad = 8;
            wanderDist = 15;
            wanderMax = 20;

            health = 30;
            maxHealth = 30;
            level = 3;
            experienceGiven = 5;
            rec = new Rectangle((int)position.X, (int)position.Y, 140, 140);
            tolerance = 3;
            vitalRec = new Rectangle(rec.X, rec.Y, 128, 128);
            maxHealthDrop = 5;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (!hostile)
            {
                return new Rectangle(70 * moveFrame, 0, 70, 70);
            }
            else
                return new Rectangle(70 * moveFrame, 70, 70, 70);
        }

        public override void Update(int mapWidth)
        {
            base.Update(mapWidth);
            if (!respawning)
            {

                frameDelay--;

                if (frameDelay <= 0)
                {
                    frameDelay = 4;
                    moveFrame++;
                }

                if (moveFrame > 3)
                    moveFrame = 0;

                wanderRad = 8;
                wanderDist = 15;
                wanderMax = 20;

                CheckWalkCollisions(3, new Vector2(5, -5));
            }
                vitalRec.X = rec.X;
                vitalRec.Y = rec.Y;
            
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

        public override Vector2 calcSteeringForce()
        {
            Vector2 sf = new Vector2();

            if (!hostile)
            {
                sf += Wander() * 5;
                sf += (CheckBoundaries() * 5);
                sf += calcSeparation(50);

                //sf += Align();
            }
            else
            {
                sf += Seek(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y)) * 5;
                sf += calcSeparation(50);
                //sf += Seek(calcCohesion());
            }


            return sf;
            
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 3)
            {
                if (dropType == 0)
                    currentMap.Drops.Add(new EnemyDrop("Ruby (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                else if (dropType == 1)
                    currentMap.Drops.Add(new EnemyDrop("Emerald (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
                else
                    currentMap.Drops.Add(new EnemyDrop("Sapphire (Rough)", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));

            }
            else if (dropType < 98)//28
            {
                currentMap.Drops.Add(new EnemyDrop("Bat Fangs", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                currentMap.Drops.Add(new EnemyDrop(new DunceCap(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 45)
            {
                currentMap.Drops.Add(new EnemyDrop("Coal", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }
    }
}
