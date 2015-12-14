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
    public class AttackBottle
    {
        int type;
        Rectangle rec;
        Vector2 velocity;
        Vector2 position;
        Player player;
        Boolean flying;
        MapClass currentMap;
        Texture2D sprite;

        int armorPen;
        int landTimer;
        Boolean active;
        int explosionFrame;
        Rectangle explosionRec;
        Boolean splitAlready = false;

        Scientist owner;

        public Boolean Active { get { return active; } }

        public AttackBottle(int typ, float x, Vector2 vel, Player p, MapClass cur, Scientist own)
        {
            type = typ;
            rec = new Rectangle((int)x, 450, 50, 50);
            position = new Vector2(x, 450);
            velocity = vel;
            player = p;
            flying = true;
            currentMap = cur;
            owner = own;

            if (type == 0)
                sprite = Game1.whiteFilter;
            else if (type == 1)
                sprite = Game1.whiteFilter;
            else
                sprite = Game1.whiteFilter;

            active = true;
        }

        public void Update()
        {
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            if (flying)
            {
                velocity.Y += GameConstants.GRAVITY;

                position += velocity;

                if(rec.Intersects(player.VitalRec))
                {
                    Land();
                    flying = false;
                }

                for (int i = 0; i < currentMap.Platforms.Count; i++)
                {
                    if (rec.Intersects(currentMap.Platforms[i].Rec))
                    {
                        flying = false;
                    }
                }

                if (type == 1 && Math.Abs(velocity.Y) <= 1 && splitAlready == false)
                {
                    velocity.Y = 0;
                    velocity.X = 5;
                    splitAlready = true;
                    AttackBottle newBottle = new AttackBottle(1, rec.X, velocity, player, currentMap, owner);
                    newBottle.velocity.X = -5;
                    newBottle.splitAlready = true;
                    newBottle.position = position;
                    newBottle.rec = rec;

                    owner.BottlesInAir.Add(newBottle);
                }
            }
            else
            {
                Land();
            }
        }

        public Rectangle GetDeathSource(int recNum)
        {
            return new Rectangle(explosionFrame * 300, 0, 300, 300);
        }

        public void Land()
        {
            landTimer++;

            switch (type)
            {
                    //--Big damage potion
                case 0:
                    if (explosionRec.Width == 0)
                    {
                        Game1.camera.ShakeCamera(5, 10);
                        explosionRec = new Rectangle(rec.X - 175, rec.Y - 175, 350, 350);
                    }

                    if (landTimer == 4)
                    {
                        explosionFrame++;
                        landTimer = 0;
                    }

                    if (explosionFrame == 7)
                    {
                        active = false;
                    }

                    if (player.CheckIfHit(explosionRec) && explosionFrame < 6)
                    {
                        //player.TakeDamage(25);
                        player.KnockPlayerBack(new Vector2(5, -5));
                    }
                break;

                //--Splitting potion
                case 1:
                if (explosionRec.Width == 0)
                {
                    Game1.camera.ShakeCamera(5, 10);
                    explosionRec = new Rectangle(rec.X - 125, rec.Y - 125, 250, 250);
                }

                if (landTimer == 4)
                {
                    explosionFrame++;
                    landTimer = 0;
                }

                if (explosionFrame == 7)
                {
                    active = false;
                }

                if (player.CheckIfHit(explosionRec) && explosionFrame < 6)
                {
                    //player.TakeDamage(15);
                    player.KnockPlayerBack(new Vector2(5, -5));
                }
                break;
                    //--Stun potion
                case 2:
                    if (explosionRec.Width == 0)
                    {
                        Game1.camera.ShakeCamera(5, 10);
                        explosionRec = new Rectangle(rec.X - 125, rec.Y - 125, 250, 250);
                    }

                    if (landTimer == 4)
                    {
                        explosionFrame++;
                        landTimer = 0;
                    }

                    if (explosionFrame == 7)
                    {
                        active = false;
                    }

                    if (player.CheckIfHit(explosionRec))
                    {
                       // player.TakeDamage(15);
                        player.Stun(80);
                        player.KnockPlayerBack(new Vector2(1, 1));
                    }
                break;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if(flying && active)
                s.Draw(sprite, rec, Color.White);

            if (explosionRec != null)
            {
                s.Draw(EffectsManager.deathSpriteSheet, explosionRec, GetDeathSource(explosionFrame), Color.White);
            }
        }
    }

    public class Scientist : Enemy
    {
        List<int> standingPoints;
        int currentPoint;
        Boolean runRight;
        List<AttackBottle> bottlesInAir;


        public List<AttackBottle> BottlesInAir { get { return bottlesInAir; } set { bottlesInAir = value; } }
        public Scientist(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 5;
            maxHealth = 45;
            level = 7;
            experienceGiven = 50;
            rec = new Rectangle((int)position.X, (int)position.Y, 120, 170);
            currentlyInMoveState = false;
            enemySpeed = 9;
            tolerance = 10;
            vitalRec = new Rectangle(rec.X + 20, rec.Y + 40, 80, 100);
            maxHealthDrop = 0;
            canBeStunned = false;

            bottlesInAir = new List<AttackBottle>();

            standingPoints = new List<int>();
            standingPoints.Add(cur.MapWidth / 10);
            standingPoints.Add(cur.MapWidth / 4);
            standingPoints.Add(cur.MapWidth / 2);
            standingPoints.Add((cur.MapWidth / 4) * 3);
            standingPoints.Add((cur.MapWidth / 10) * 8);

            enemyState = EnemyState.moving;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (knockedBack || isStunned)
                return new Rectangle(24, 0, 24, 34);
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                            return new Rectangle(48, 0, 24, 34);
                    case EnemyState.moving:
                        return new Rectangle(24 * moveFrame, 0, 24, 34);
                    case EnemyState.attacking:
                        return new Rectangle(48 + (24 * attackFrame), 682, 24, 34);
                }
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            Move(mapwidth);

            CheckWalkCollisions(20, new Vector2(10, -5));

            vitalRec.X = rec.X + 20;
            vitalRec.Y = rec.Y + 40;

            for (int i = 0; i < bottlesInAir.Count; i++)
            {
                bottlesInAir[i].Update();

                if (bottlesInAir[i].Active == false)
                {
                    bottlesInAir.Remove(bottlesInAir[i]);
                    i--;
                }
            }
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--After done standing around, pick a point and start running
            if (currentlyInMoveState == false)
            {
                //Which point he picks
                moveState = moveNum.Next(0, 5);
                //Length of running
                moveTimer = moveTime.Next(300, 600);

                currentPoint = standingPoints[moveState];
                enemyState = EnemyState.moving;
            }

            //--Stand still
            if (enemyState == EnemyState.standing)
            {
                moveTimer--;
                canBeKnockbacked = true;
                if(moveTimer <= 0 && knockedBack == false)
                {
                    currentlyInMoveState = false;
                }
            }

            //--Run around
            if (moveTimer > 0 && enemyState == EnemyState.moving)
            {
                canBeKnockbacked = false;

                #region Run right
                if (runRight)
                {
                    facingRight = true;
                    enemyState = EnemyState.moving;
                    if (currentlyInMoveState == false)
                        moveFrame = 0;

                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 1)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                    if (position.X <= mapWidth - 6)
                        position.X += enemySpeed;

                    //--Switch directions
                    if (PositionX + rec.Width >= mapWidth - 50)
                    {
                        runRight = false;
                    }
                }
                #endregion

                #region Run Left
                else
                {
                    facingRight = false;
                    enemyState = EnemyState.moving;

                    if (currentlyInMoveState == false)
                        moveFrame = 0;

                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 1)
                        moveFrame = 0;

                    currentlyInMoveState = true;
                    moveTimer--;
                    if (position.X >= 6)
                        position.X -= enemySpeed;

                    if (PositionX <= 50)
                    {
                        runRight = true;
                    }
                }
                #endregion

            }

            //--If he is done running around, find the point you were seeking
            if (moveTimer <= 0 && enemyState == EnemyState.moving || enemyState == EnemyState.attacking)
            {
                
                float distanceFromPoint = Math.Abs(PositionX - currentPoint);

                //--Attack when you are there
                if (distanceFromPoint < 10)
                {
                    ThrowBottles();
                }

                #region Seek point
                else if (currentPoint < PositionX)
                {
                    facingRight = false;
                    position.X -= enemySpeed;

                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 1)
                        moveFrame = 0;
                }
                else if (currentPoint > PositionX)
                {
                    facingRight = true;
                    position.X += enemySpeed;

                    frameDelay--;
                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;
                    }

                    if (moveFrame > 1)
                        moveFrame = 0;
                }
                #endregion
            }

        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public void ThrowBottles()
        {

            //--Face the player if it isn't already. 
            //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
            //--The wrong way and autoattack in the wrong direction
            if (player.VitalRec.Center.X < vitalRec.Center.X)
                facingRight = false;
            else
                facingRight = true;

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;
                frameDelay = 10;

                float vel1, vel2, vel3;

                vel1 = -5;
                vel2 = -12;
                vel3 = 10;

                switch (moveState)
                {
                    case 0:
                    case 1:
                        vel1 = 5;
                        vel2 = 15;
                        vel3 = 20;
                        break;
                    case 2:
                        int random = moveNum.Next(0, 2);

                        if (random == 0)
                        {
                            vel1 = -5;
                            vel2 = -20;
                            vel3 = 13;
                        }
                        else
                        {
                            vel1 = 5;
                            vel2 = 20;
                            vel3 = -13;
                        }
                        break;
                    case 4:
                    case 5:
                        vel1 = -5;
                        vel2 = -15;
                        vel3 = -20;
                        break;
                }

                AttackBottle bot1 = new AttackBottle(moveNum.Next(0, 3), PositionX + rec.Width / 2, new Vector2(vel1, -23), player, currentMap, this);
                AttackBottle bot2 = new AttackBottle(moveNum.Next(0, 3), PositionX + rec.Width / 2, new Vector2(vel2, -23), player, currentMap, this);
                AttackBottle bot3 = new AttackBottle(moveNum.Next(0, 3), PositionX + rec.Width / 2, new Vector2(vel3, -23), player, currentMap, this);

                bottlesInAir.Add(bot1);
                bottlesInAir.Add(bot2);
                bottlesInAir.Add(bot3);
                frameDelay = 20;

            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                enemyState = EnemyState.standing;
                moveTimer = 250;
            }

            currentlyInMoveState = true;
        }


        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            foreach (AttackBottle b in bottlesInAir)
            {
                b.Draw(s);
            }
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        //--Make the monster fall back to the ground if it is in the air
        //--Update all forces, X and Y
        public override void ImplementGravity()
        {
            velocity.Y += GameConstants.GRAVITY;
            position += velocity;

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 20, rec.Width, 20);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height;
                    velocity.Y = 0;

                    if (velocity.X == 0)
                        knockedBack = false;
                }
                #endregion

                if (rec.Intersects(plat.Rec) && plat.Passable == false)
                {
                    if (position.X < plat.Rec.X)
                    {
                        position.X = plat.Rec.X - rec.Width;
                    }

                    else if (position.X < plat.Rec.X + plat.Rec.Width)
                    {
                        position.X = plat.Rec.X + plat.Rec.Width;
                    }
                }
            }

            #region Not falling off a platform
            //--Don't fall off the platform you're on!
            if (currentPlat != null)
            {
                if (position.X < currentPlat.Rec.X)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width;
                }
            }
            #endregion

        }

        //--Take damage
        //--Takes in damage amount and knockback amount
        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            if (respawning == false)
            {
                damage -= tolerance;

                if (damage <= 0)
                    damage = 1;
                health -= damage;
                KnockBack(kbvel);
                knockBackVec = kbvel;

                if (knockBackVec.Y < -10)
                    hangInAir = true;

                if (hangInAir == true)
                {
                    hangInAirTime = 0;
                }


                AddDamageNum(damage, collision);
            }

        }

        //--Updates the knockback effect
        public override void UpdateKnockBack()
        {
            //--If the monster is currently being knocked back
            if (knockedBack == true)
            {
                //--If the monster is bouncing to the right, subtract velocity in the X direction
                if (velocity.X > 0)
                {
                    velocity.X -= 2;

                    //--If the velocity hits 0 or goes negative, make the velocity 0
                    if (velocity.X <= 0)
                        velocity.X = 0;
                }
                //--If the monster is bouncing to the left, add velocity
                if (velocity.X < 0)
                {
                    velocity.X += 2;

                    //--If it goes positive or hits 0, make it 0
                    if (velocity.X >= 0)
                        velocity.X = 0;
                }
            }
        }


        public override void DropItem()
        {
            base.DropItem();


            //Add story item to the map, keycard

            currentMap.Drops.Add(new EnemyDrop(new SecurityClearanceID(0,0), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));

        }
    }
}
