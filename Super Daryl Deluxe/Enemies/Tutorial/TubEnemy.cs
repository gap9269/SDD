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
    class TubEnemy : SteeringEnemy
    {
        public TubEnemy(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, Rectangle bound)
            : base(pos, "Garden Beast", g, ref play, cur, bound)
        {
            maxSpeed = 50;
            maxForce = 75;
            mass = .8f;

            wanderAng = 0;
            wanderRad = 8;
            wanderDist = 15;
            wanderMax = 20;

            health = 20;
            maxHealth = 20;
            level = 3;
            experienceGiven = 1;
            rec = new Rectangle((int)position.X, (int)position.Y, 324, 147);
            tolerance = 2;
            vitalRec = new Rectangle(rec.X, rec.Y, 220, 120);
            maxHealthDrop = 5;
            moneyToDrop = .03f;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(659, 0, 324, 147);
        }

        public override void Update(int mapWidth)
        {
            base.Update(mapWidth);

            wanderRad = 8;
            wanderDist = 15;
            wanderMax = 20;

            CheckWalkCollisions(1, new Vector2(5, -5));
            vitalRec.X = rec.X + 35;
            vitalRec.Y = rec.Y + 20;
        }
        public override Vector2 calcSteeringForce()
        {
            Vector2 sf = new Vector2();

            if (!hostile)
            {
                sf += Wander();
                sf += (CheckBoundaries() * 5);
            }
            else
            {
                sf += Seek(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y)) * 5;
                sf += calcSeparation(50);
            }


            return sf;

        }

        public override void DropItem()
        {
            base.DropItem();
        }
    }
}
