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
    class LiteratureGuardian : Boss
    {
        #region Cooldowns
        int lightningSmashCooldown;
        int maxLightningSmashCooldown = 1000;

        int bulletCooldown;
        int maxBulletCooldown = 700;
        int bulletSpawnTimer = 100;

        int fastAttacksCooldown;
        int maxFastAttacksCooldown = 600;

        int flinchCooldown;
        int maxFlinchCooldown = 200;

        int vulnerableTime;
        int maxVulnerableTime = 150;

        int idleTime;
        int maxIdleTime = 100;

        int idleCooldown;
        int maxIdleCooldown = 900;
        #endregion

        int rangeDamage = 1;
        int basicAttackDamage = 4;
        int smashDamage = 5;
        int quickAttackDamage = 3;

        float attackPositionY;

        int meleeRange = 250;
        int basicAttackState;
        int maxAttackCooldown = 30;
        Rectangle attackRec, teleportRec;

        int quickAttackState;
        int lightningAttackState;

        int holdFlinchTimer;

        Boolean inBulletPosition;
        Boolean fightEnd = false;
        Boolean droppedHealth = false;

        public enum Movestate
        {
            idle, teleport, attacking, moving, retreating, flinching
        }
        public Movestate movementState;

        public enum AttackState
        {
            none, basic, smash, bullets, quick
        }
        public AttackState attackState;

        public enum Intentions
        {
            none, basic, smash, bullets, fastAttack
        }
        public Intentions intentions;

        int knockbackTime;

        int standingPositionY;

        // CONSTRUCTOR \\
        public LiteratureGuardian(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, int groundPosY)
            : base(pos, type, g, ref play, cur)
        {
            health = 0;
            maxHealth = 12000;
            level = 18;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, 848, 664);
            currentlyInMoveState = false;
            tolerance = 100;
            vitalRec = new Rectangle((int)position.X, (int)position.Y, 100, 200);
            moveSpeed = 5;

            distanceFromBottomRecToFeet = 0;
            addToHealthWidth = 0;
            healthBarRec.Width = 0;

            drawHUDName = true;
            standingPositionY = groundPosY;
            rectanglePaddingLeftRight = 366;

            rangeDamage = 120;
            basicAttackDamage = 130;
            smashDamage = 150;
            quickAttackDamage = 130;
            maxVulnerableTime = 150;
        }

        public override Rectangle GetHealthSourceRectangle()
        {
            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }

            return false;
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);
            Move(currentMap.MapWidth);

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            vitalRec.X = rec.X + 370;
            vitalRec.Y = rec.Y + 165;

            //if (IsDead())
            //{
            //    game.CurrentChapter.BossFight = false;
            //    game.CurrentChapter.CurrentBoss = null;
            //}

            UpdateCooldowns();

        }

        public void UpdateCooldowns()
        {
            if (knockbackTime > 0)
            {
                knockbackTime--;

                if(knockbackTime == 0)
                {
                    knockedBack = false;
                    velocity = new Vector2();
                }
            }

            if (holdFlinchTimer > 0)
            {
                holdFlinchTimer--;
            }

            if (holdFlinchTimer <= 0 && movementState == Movestate.flinching)
                movementState = Movestate.idle;

            if (attackCooldown > 0)
            {
                attackCooldown--;
            }

            if (fastAttacksCooldown > 0)
            {
                fastAttacksCooldown--;
            }

            if (bulletCooldown > 0)
            {
                bulletCooldown--;
            }

            if (lightningSmashCooldown > 0)
            {
                lightningSmashCooldown--;
            }

            if (idleTime > 0)
                idleTime--;

            if (idleCooldown > 0)
                idleCooldown--;

            if (vulnerableTime > 0)
            {
                vulnerableTime--;

                if (vulnerableTime <= 0)
                {
                    flinchCooldown = maxFlinchCooldown;

                    if (knockedBack)
                        movementState = Movestate.retreating;
                }
            }
            else if (flinchCooldown > 0)
            {
                flinchCooldown--;
            }
        }

        public void Attack()
        {
            switch (attackState)
            {
                case AttackState.basic:
                    Vector2 kb = new Vector2();
                    if (attackFrame > 0)
                    {
                        switch (basicAttackState)
                        {

                            case 0:
                                if (facingRight)
                                {
                                    attackRec = new Rectangle(VitalRec.Center.X + 30, VitalRecY + 65, 300, 140);
                                    if (position.X <= currentMap.MapWidth - 6)
                                        position.X += 3;

                                    kb = new Vector2(10, -8);
                                }
                                else
                                {
                                    attackRec = new Rectangle(VitalRec.Center.X - 300, VitalRecY + 65, 300, 140);
                                    if (position.X + rectanglePaddingLeftRight - 20 >= 6)
                                        position.X -= 3;

                                    kb = new Vector2(-10, -8);
                                }
                                break;
                            case 1:
                                if (facingRight)
                                {
                                    attackRec = new Rectangle(VitalRec.Center.X + 50, VitalRecY - 20, 250, 230);
                                    if (position.X <= currentMap.MapWidth - 6)
                                        position.X += 3;

                                    kb = new Vector2(8, -3);
                                }
                                else
                                {
                                    attackRec = new Rectangle(VitalRec.Center.X - 290, VitalRecY - 20, 250, 230);
                                    if (position.X + rectanglePaddingLeftRight - 20 >= 6)
                                        position.X -= 3;

                                    kb = new Vector2(-8, -3);
                                }
                                break;
                            case 2:
                                attackRec = new Rectangle(VitalRec.Center.X - 350, VitalRecY - 10, 700, 130);

                                if(player.VitalRec.Center.X < vitalRec.Center.X)
                                    kb = new Vector2(-20, -8);
                                else
                                    kb = new Vector2(20, -8);
                                
                                break;
                        }
                    }

                    frameDelay--;

                    if (attackFrame > 0)
                    {
                        if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0 && (attackFrame > 1 || (basicAttackState == 1 && attackFrame > 0)))
                        {
                            player.TakeDamage(basicAttackDamage, level);
                            player.KnockPlayerBack(kb);
                            hitPauseTimer = basicAttackState;
                            player.HitPauseTimer = basicAttackState;
                            game.Camera.ShakeCamera(basicAttackState, basicAttackState);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                        }
                    }

                    if (frameDelay <= 0)
                    {
                        frameDelay = 4;
                        attackFrame++;

                        if (attackFrame > 5 || (attackFrame > 3 && basicAttackState == 1))
                        {
                            attackFrame = 0;
                            basicAttackState++;

                            if (basicAttackState == 3)
                            {
                                movementState = Movestate.idle;
                                moveFrame = 0;
                                basicAttackState = 0;
                                attackCooldown = maxAttackCooldown;
                            }
                        }
                    }
                    break;
                case AttackState.quick:
                    QuickAttack();
                    break;
                case AttackState.bullets:
                    attackRec = new Rectangle();
                    RangedAttack();
                    break;
                case AttackState.smash:
                    LightningSmashAttack();
                    break;
            }
        }

        public void LightningSmashAttack()
        {
            if (attackFrame < 9)
                canBeHurt = false;
            else
                canBeHurt = true;

            if (lightningAttackState < 4)
            {
                if (attackFrame == 0 && frameDelay == 4)
                {
                    teleportRec = new Rectangle(player.VitalRec.Center.X - Game1.randomNumberGen.Next(0, 41) - rec.Width / 2, 0, rec.Width, rec.Height);
                    
                    frameDelay--;
                }
                else
                {
                    velocity.Y = 0;
                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        attackFrame++;

                        frameDelay = 4;

                        if (attackFrame == 1 || attackFrame == 6)
                        {
                            game.Camera.ShakeCamera(7, 2);
                        }
                        if(attackFrame == 9)
                            game.Camera.ShakeCamera(10, 10);

                        if (attackFrame == 5)
                        {
                            //If she is going to the left, face left
                            if (PositionX > teleportRec.X)
                                facingRight = false;
                            else
                                facingRight = true;

                            PositionX = teleportRec.X;
                            PositionY = teleportRec.Y;
                        }

                        if (attackFrame == 13)
                        {
                            attackRec = new Rectangle();
                            lightningAttackState++;
                            attackFrame = 0;
                        }
                    }

                    if (attackFrame > 8)
                    {
                        attackRec = new Rectangle(VitalRec.Center.X - 50, VitalRecY + 50, 100, 600);

                        Vector2 kb;

                        if (player.VitalRec.Center.X < attackRec.Center.X)
                            kb = new Vector2(15, -8);
                        else
                            kb = new Vector2(15, -8);

                        if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                        {
                            player.TakeDamage(smashDamage, level);
                            player.KnockPlayerBack(kb);
                            hitPauseTimer = 3;
                            player.HitPauseTimer = 3;
                            game.Camera.ShakeCamera(4, 4);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                        }

                    }
                }
            }
            else
            {
                teleportRec = new Rectangle(player.VitalRec.Center.X - Game1.randomNumberGen.Next(0, 41) - rec.Width / 2, 0, rec.Width, rec.Height);
                canBeHurt = true;
                movementState = Movestate.teleport;
                moveFrame = 0;
                attackFrame = 0;
                frameDelay = 5;
                intentions = Intentions.none;
                attackState = AttackState.none;
                lightningAttackState = 0;
                lightningSmashCooldown = maxLightningSmashCooldown;
            }
        }

        public void RangedAttack()
        {
            frameDelay--;

            if (frameDelay <= 0)
            {
                attackFrame++;
                frameDelay = 4;

                if (attackFrame == 1)
                {
                    game.Camera.ShakeCamera(7, 2);
                }

                if (attackFrame == 14 && bulletSpawnTimer > 0)
                    attackFrame = 9;

                else if (attackFrame >= 14)
                {

                    int teleNum = Game1.randomNumberGen.Next(0, 2);

                    if ((teleNum == 0 && player.VitalRec.Center.X - 300 - rec.Width / 2 >= 6) || (teleNum == 1 && player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight > currentMap.mapRec.Width - 6))
                    {
                        teleportRec = new Rectangle(player.VitalRec.Center.X - Game1.randomNumberGen.Next(120, 301) - rec.Width / 2, standingPositionY, rec.Width, rec.Height);
                    }
                    else if (player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight <= currentMap.mapRec.Width - 6)
                    {
                        teleportRec = new Rectangle(player.VitalRec.Center.X + Game1.randomNumberGen.Next(-200, -39), standingPositionY, rec.Width, rec.Height);
                    }
                    movementState = Movestate.teleport;

                    moveFrame = 0;
                    attackFrame = 0;
                    bulletCooldown = maxBulletCooldown;
                    bulletSpawnTimer = 60;
                }
            }

            if (attackFrame > 9)
            {
                if (bulletSpawnTimer >= 0)
                {
                    bulletSpawnTimer--;
                    if (bulletSpawnTimer == 50)
                    {
                        Vector2 v;
                        int bulletOffset = 100;
                        if (!facingRight)
                        {
                            v = new Vector2(-800, rec.Y + 514);
                            bulletOffset = -bulletOffset;
                        }
                        else
                            v = new Vector2(800, rec.Y + 514);

                        v.Normalize();
                        float rot = (float)Math.Atan2(v.Y, v.X);

                        XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X + bulletOffset, (int)vitalRec.Center.Y, 20, v, rot, rangeDamage, new Vector2(30, -10), 1, 0, 15, Projectile.ProjType.xylophoneKey, level);

                        currentMap.Projectiles.Add(arrow);
                    }

                    else if (bulletSpawnTimer == 30)
                    {

                        Vector2 v;
                        int bulletOffset = 100;

                        if (!facingRight)
                        {
                            bulletOffset = -bulletOffset;
                            v = new Vector2(-1200, rec.Y + 114);
                        }
                        else
                            v = new Vector2(1200, rec.Y + 114);

                        v.Normalize();
                        float rot = (float)Math.Atan2(v.Y, v.X);

                        XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X + bulletOffset, (int)vitalRec.Center.Y, 60, v, rot, rangeDamage, new Vector2(30, -10), 1, 0, 15, Projectile.ProjType.xylophoneKey, level);

                        currentMap.Projectiles.Add(arrow);

                    }


                    else if (bulletSpawnTimer == 10)
                    {

                        Vector2 v;
                        int bulletOffset = 100;
                        if (!facingRight)
                        {
                            bulletOffset = -bulletOffset;
                            v = new Vector2(-1600, rec.Y - 100);
                        }
                        else
                            v = new Vector2(1600, rec.Y - 100);

                        v.Normalize();
                        float rot = (float)Math.Atan2(v.Y, v.X);

                        XylophoneKey arrow = new XylophoneKey((int)vitalRec.Center.X + bulletOffset, (int)vitalRec.Center.Y, 80, v, rot, rangeDamage, new Vector2(30, -10), 1, 0, 15, Projectile.ProjType.xylophoneKey, level);

                        currentMap.Projectiles.Add(arrow);

                    }
                }
            }
        }

        public void QuickAttack()
        {
            if (attackFrame < 9)
                canBeHurt = false;
            else
                canBeHurt = true;

            if (quickAttackState < 4)
            {
                if (attackFrame == 0 && frameDelay == 5)
                {
                    int teleNum = Game1.randomNumberGen.Next(0, 2);

                    if ((teleNum == 0 && player.VitalRec.Center.X - 300 - rec.Width / 2 >= 6) || (teleNum == 1 && player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight > currentMap.mapRec.Width - 6))
                    {
                        teleportRec = new Rectangle(player.VitalRec.Center.X - Game1.randomNumberGen.Next(120, 301) - rec.Width / 2, player.VitalRecY - rec.Height / 2, rec.Width, rec.Height);
                    }
                    else if (player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight <= currentMap.mapRec.Width - 6)
                    {
                        teleportRec = new Rectangle(player.VitalRec.Center.X + Game1.randomNumberGen.Next(-200, -39), player.VitalRecY - rec.Height / 2, rec.Width, rec.Height);
                    }

                    if (player.Ducking)
                    {
                        teleportRec.Y = player.VitalRecY - 150;
                    }

                    frameDelay--;
                }
                else
                {
                    velocity.Y = 0;
                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        attackFrame++;

                        frameDelay = 5;

                        if (attackFrame == 1 || attackFrame == 6)
                        {
                            game.Camera.ShakeCamera(7, 2);
                        }

                        if (attackFrame == 5)
                        {
                            PositionX = teleportRec.X;
                            PositionY = teleportRec.Y;
                        }
                        if (attackFrame == 6)
                        {
                            if (player.VitalRec.Center.X < vitalRec.Center.X)
                                facingRight = false;
                            else
                                facingRight = true;
                        }
                        if (attackFrame == 13)
                        {
                            attackRec = new Rectangle();
                            quickAttackState++;
                            attackFrame = 0;
                        }
                    }

                    if (attackFrame > 9)
                    {
                        if (facingRight)
                        {
                            attackRec = new Rectangle(VitalRec.Center.X + 100, VitalRecY - 60, 270, 305);
                        }
                        else
                        {
                            attackRec = new Rectangle(VitalRec.Center.X - 370, VitalRecY - 60, 270, 305);
                        }

                        Vector2 kb;
                        if (facingRight)
                            kb = new Vector2(20, -8);
                        else
                            kb = new Vector2(20, -8);


                        if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                        {
                            player.TakeDamage(quickAttackDamage, level);
                            player.KnockPlayerBack(kb);
                            hitPauseTimer = 2;
                            player.HitPauseTimer = 2;
                            game.Camera.ShakeCamera(2, 2);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                        }
                    }
                }
            }
            else
            {
                int teleNum = Game1.randomNumberGen.Next(0, 2);

                if ((teleNum == 0 && player.VitalRec.Center.X - 300 - rec.Width / 2 >= 6) || (teleNum == 1 && player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight > currentMap.mapRec.Width - 6))
                {
                    teleportRec = new Rectangle(player.VitalRec.Center.X - Game1.randomNumberGen.Next(120, 301) - rec.Width / 2, standingPositionY, rec.Width, rec.Height);
                }
                else if (player.VitalRec.Center.X + (-40) + rec.Width - rectanglePaddingLeftRight <= currentMap.mapRec.Width - 6)
                {
                    teleportRec = new Rectangle(player.VitalRec.Center.X + Game1.randomNumberGen.Next(-200, -39), standingPositionY, rec.Width, rec.Height);
                }
                movementState = Movestate.teleport;

                canBeHurt = true;
                moveFrame = 0;
                attackFrame = 0;
                frameDelay = 5;
                intentions = Intentions.none;
                attackState = AttackState.none;
                quickAttackState = 0;
                fastAttacksCooldown = maxFastAttacksCooldown;
            }
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision)
        {
            if (flinchCooldown <= 0 && vulnerableTime <= 0)
            {
                vulnerableTime = maxVulnerableTime;
            }

            if (canBeHurt && ((movementState != Movestate.retreating && movementState != Movestate.teleport) || moveFrame < 2 || moveFrame > 6))
            {
                ShakeHealthBar();
                damage = (int)(damage * (250f / (250f + tolerance)));

                if (damage <= 0)
                    damage = 1;
                health -= damage;
                hasBeenHit = true;

                if (vulnerableTime > 0)
                {
                    CancelAttackAndMovement();
                    KnockBack(kbvel);
                    knockbackTime = 10;
                    holdFlinchTimer = 30;
                }

                if (kbvel.Y < -10)
                    hangInAir = true;

                if (hangInAir == true)
                {
                    hangInAirTime = 0;
                }

                AddDamageNum(damage, collision);
            }
        }

        public void Teleport()
        {
            movementState = Movestate.teleport;
            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame++;

                frameDelay = 5;

                if (moveFrame == 1 || moveFrame == 6)
                {
                    game.Camera.ShakeCamera(7, 2);
                }

                if (moveFrame == 5)
                {
                    PositionX = teleportRec.X;
                    PositionY = teleportRec.Y;
                }
                if (moveFrame == 6)
                {
                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;
                }
                if (moveFrame == 10)
                {
                    movementState = Movestate.idle;
                    moveFrame = 0;
                    teleportRec = new Rectangle();
                }
            }
        }

        public void CutsceneTeleport(Rectangle teleRec)
        {
            movementState = Movestate.teleport;
            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame++;

                frameDelay = 5;

                if (moveFrame == 1 || moveFrame == 6)
                {
                    game.Camera.ShakeCamera(7, 2);
                }

                if (moveFrame == 5)
                {
                    PositionX = teleRec.X;
                    PositionY = teleRec.Y;

                    rec.X = (int)position.X;
                    rec.Y = (int)position.Y;

                    vitalRec.X = rec.X + 370;
                    vitalRec.Y = rec.Y + 165;
                }
                if (moveFrame == 6)
                {
                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;
                }
                if (moveFrame == 10)
                {
                    movementState = Movestate.idle;
                    moveFrame = 0;
                    teleportRec = new Rectangle();
                }
            }
        }

        public void CutsceneFloat()
        {
            movementState = Movestate.idle;

        }

        public void CancelAttackAndMovement()
        {
            if (movementState == Movestate.attacking)
            {
                switch (attackState)
                {
                    case AttackState.basic:
                        basicAttackState = 0;
                        attackCooldown = maxAttackCooldown * 2;
                        break;
                }
            }

            canBeHurt = true;
            movementState = Movestate.idle;
            attackFrame = 0;
            moveFrame = 0;
            frameDelay = 5;
            attackState = AttackState.none;
            teleportRec = new Rectangle();
            intentions = Intentions.none;
        }

        public void Retreat()
        {
            if (teleportRec == new Rectangle())
            {
                attackPositionY = PositionY;
                int teleNum = Game1.randomNumberGen.Next(0, 2);
                if ((teleNum == 0 && position.X + rectanglePaddingLeftRight - 320 >= 6) || (teleNum == 1 && position.X + rec.Width - rectanglePaddingLeftRight + 320 > currentMap.mapRec.Width - 6))
                {
                    teleportRec = new Rectangle(rec.X - 300, rec.Y, rec.Width, rec.Height);
                }
                else if (position.X + rec.Width - rectanglePaddingLeftRight + 320 <= currentMap.mapRec.Width - 6)
                {
                    teleportRec = new Rectangle(rec.X + 300, rec.Y, rec.Width, rec.Height);

                }
                else
                {
                    teleportRec = new Rectangle(currentMap.mapRec.Width / 2 - rec.Width / 2, rec.Y, rec.Width, rec.Height);
                }
            }
            else
            {
                velocity.Y = 0;
                PositionY = attackPositionY;
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;

                    frameDelay = 5;

                    if (moveFrame == 1 || moveFrame == 6)
                    {

                        //Drop some health when at 75%
                        if (health <= (int)(maxHealth * .75f) && !droppedHealth)
                        {
                            droppedHealth = true;
                            for (int i = 0; i < 30; i++)
                            {
                                Vector2 vel = new Vector2(Game1.randomNumberGen.Next(-8, 8), -Game1.randomNumberGen.Next(3, 14));
                                HealthDrop newHealth = new HealthDrop(vel, new Rectangle(rec.Center.X, rec.Center.Y, 0, 0), 1);

                                game.CurrentChapter.CurrentMap.HealthDrops.Add(newHealth);
                            }
                        }

                        game.Camera.ShakeCamera(7, 2);
                    }

                    if (moveFrame == 5)
                    {
                        PositionX = teleportRec.X;
                        PositionY = teleportRec.Y;
                    }
                    if(moveFrame == 6)
                    {
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                            facingRight = false;
                        else
                            facingRight = true;
                    }
                    if (moveFrame == 10)
                    {
                        movementState = Movestate.idle;
                        moveFrame = 0;
                        vulnerableTime = 0;
                        flinchCooldown = maxFlinchCooldown;
                        teleportRec = new Rectangle();
                    }
                }
            }
        }

        public override void Move(int mapWidth)
        {
            distanceFromPlayer = Vector2.Distance(new Vector2(VitalRec.Center.X, vitalRec.Center.Y), new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y));
            horizontalDistanceToPlayer = Math.Abs(Vector2.Distance(new Vector2(player.VitalRec.Center.X, 0), new Vector2(vitalRec.Center.X, 0)));

            if (movementState == Movestate.retreating)
                Retreat();
            else if (!knockedBack && holdFlinchTimer > 0)
            {
                movementState = Movestate.flinching;
            }
            else if (vulnerableTime > 0 && !knockedBack)
            {
                movementState = Movestate.retreating;
            }
            else if (movementState == Movestate.teleport)
            {
                Teleport();
            }
            else if (!knockedBack && movementState != Movestate.attacking)
            {
                if (movementState == Movestate.idle && intentions == Intentions.none)
                {
                    if (health <= maxHealth / 2)
                    {
                        canBeHurt = false;
                        game.CurrentChapter.state = Chapter.GameState.Cutscene;

                    }
                    else if (idleTime <= 0)
                    {
                        int randomAction = Game1.randomNumberGen.Next(5);

                        if (randomAction == 0)
                            intentions = Intentions.basic;
                        else if (randomAction == 1 && fastAttacksCooldown <= 0)
                            intentions = Intentions.fastAttack;
                        else if (randomAction == 2 && bulletCooldown <= 0)
                        {
                            inBulletPosition = false;
                            intentions = Intentions.bullets;
                        }
                        else if (randomAction == 3 && lightningSmashCooldown <= 0)
                            intentions = Intentions.smash;
                        else if (randomAction == 4 && idleCooldown <= 0)
                        {
                            intentions = Intentions.none;
                            idleTime = maxIdleTime;
                        }
                    }
                    else
                    {
                        idleTime--;

                        if (idleTime <= 0)
                            idleCooldown = maxIdleCooldown;
                    }
                }

                switch (intentions)
                {
                    case Intentions.basic:
                        if (standingPositionY - PositionY > 50)
                        {
                            teleportRec = new Rectangle(rec.X, standingPositionY, 0, 0);
                            movementState = Movestate.teleport;
                        }
                        else if (horizontalDistanceToPlayer > meleeRange)
                            MoveTowardPlayer(mapWidth);
                        else if (attackCooldown <= 0)
                        {
                            if (player.VitalRec.Center.X < vitalRec.Center.X)
                                facingRight = false;
                            else
                                facingRight = true;

                            movementState = Movestate.attacking;
                            attackState = AttackState.basic;
                            frameDelay = 5;
                            intentions = Intentions.none;
                        }
                        break;
                    case Intentions.fastAttack:
                        movementState = Movestate.attacking;
                        attackState = AttackState.quick;
                        frameDelay = 5;
                        intentions = Intentions.none;
                        break;
                    case Intentions.smash:
                        movementState = Movestate.attacking;
                        attackState = AttackState.smash;
                        frameDelay = 5;
                        intentions = Intentions.none;
                        break;
                    case Intentions.bullets:
                        if (!inBulletPosition)
                        {
                            int side = Game1.randomNumberGen.Next(2);

                            if (side == 0)
                            {
                                teleportRec = new Rectangle(rec.X - 450, 189, 0, 0);

                                if (teleportRec.X < -40) teleportRec.X = -40;
                            }
                            else
                            {
                                teleportRec = new Rectangle(rec.X + 450, 189, 0, 0);

                                if (teleportRec.X > 1580) teleportRec.X = 1580;
                            }
                            inBulletPosition = true;
                            movementState = Movestate.teleport;
                        }
                        else
                        {
                            movementState = Movestate.attacking;
                            attackState = AttackState.bullets;
                            frameDelay = 5;
                            intentions = Intentions.none;
                        }
                        break;
                }

            }
            else if (movementState == Movestate.attacking)
            {
                Attack();
            }
        }

        public void MoveTowardPlayer(int mapWidth)
        {
            //--If the player is to the left
            if (player.VitalRec.Center.X < vitalRec.Center.X)
            {
                facingRight = false;
                movementState = Movestate.moving;
                if (currentlyInMoveState == false)
                    moveFrame = 0;

                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 4;
                }

                if (moveFrame > 13)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X + rectanglePaddingLeftRight - 20 >= 6)
                    position.X -= moveSpeed;
            }
            //Player to the right
            else
            {
                facingRight = true;
                movementState = Movestate.moving;
                if (currentlyInMoveState == false)
                    moveFrame = 0;

                frameDelay--;
                if (frameDelay == 0)
                {
                    moveFrame++;
                    frameDelay = 4;
                }

                if (moveFrame > 13)
                    moveFrame = 0;

                currentlyInMoveState = true;
                moveTimer--;

                if (position.X <= mapWidth - 6)
                    position.X += moveSpeed;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //if(movementState == Movestate.attacking)
            //    s.Draw(Game1.whiteFilter, attackRec, Color.Red * .6f);

            //s.Draw(Game1.whiteFilter, vitalRec, Color.Blue * .6f);

            //if(vulnerableTime > 0)
            //    s.Draw(Game1.whiteFilter, rec, Color.Green * .4f);

            SpriteEffects spriteEffect = SpriteEffects.None;

            if (!facingRight)
                spriteEffect = SpriteEffects.FlipHorizontally;

            if (isStunned || knockedBack || movementState == Movestate.flinching)
                s.Draw(game.EnemySpriteSheets["frontstatic0"], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
            if (movementState == Movestate.idle)
                s.Draw(game.EnemySpriteSheets["frontstatic0"], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
            else if (movementState == Movestate.moving)
                s.Draw(game.EnemySpriteSheets["float0"], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
            else if (movementState == Movestate.teleport || movementState == Movestate.retreating)
            {
                s.Draw(game.EnemySpriteSheets["teleport" + moveFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);

            }
            else if (movementState == Movestate.attacking)
            {
                switch (attackState)
                {
                    case AttackState.basic:
                        if(basicAttackState == 0)
                            s.Draw(game.EnemySpriteSheets["comboattackOne" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        else if (basicAttackState == 1)
                            s.Draw(game.EnemySpriteSheets["comboattackTwo" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        else if (basicAttackState == 2)
                            s.Draw(game.EnemySpriteSheets["comboattackThree" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        break;
                    case AttackState.quick:
                        if(attackFrame < 5 && quickAttackState == 0)
                            s.Draw(game.EnemySpriteSheets["teleport" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        else
                            s.Draw(game.EnemySpriteSheets["quickAttack" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        break;
                    case AttackState.smash:
                        if (attackFrame < 5 && lightningAttackState == 0)
                            s.Draw(game.EnemySpriteSheets["teleport" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        else
                            s.Draw(game.EnemySpriteSheets["flameSmash" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        break;
                    case AttackState.bullets:
                        s.Draw(game.EnemySpriteSheets["rangedAttack" + attackFrame], rec, null, Color.White * alpha, 0, Vector2.Zero, spriteEffect, 0);
                        break;
                }
            }

            //s.DrawString(Game1.questNameFont, movementState.ToString(), new Vector2(rec.X, rec.Y), Color.Red);

        }

        public override void ImplementGravity()
        {
            if (VelocityY < 1 && VelocityY > -1 && hangInAir)
            {
                hangInAirTime++;

                VelocityY = 0;

                if (hangInAirTime == 10)
                {
                    hangInAirTime = 0;
                    hangInAir = false;
                }
            }
            else if(knockbackTime > 0)
                velocity.Y += GameConstants.GRAVITY;

            position += velocity;

            Rectangle feet = new Rectangle((int)rec.X, (int)position.Y + rec.Height - 20, rec.Width, 20);
            Rectangle vitalRecFeet = new Rectangle((int)vitalRec.X, (int)position.Y + rec.Height - 20, vitalRec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 25, (int)vitalRec.Y + 5, 25, vitalRec.Height + 35);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);



                if (knockedBack)
                {
                    Rectangle checkPlatRec;

                    if (VelocityX >= 0)
                    {
                        checkPlatRec = new Rectangle(rightEn.X, rightEn.Y, (int)velocity.X, rightEn.Height);

                        if (checkPlatRec.Intersects(left))
                        {
                            //playerState = PlayerState.standing;
                            PositionX -= VelocityX;
                            knockedBack = false;
                            VelocityX = 0;
                            // playerState = PlayerState.standing;
                        }
                    }
                    else
                    {
                        checkPlatRec = new Rectangle(leftEn.X - Math.Abs((int)VelocityX), leftEn.Y, Math.Abs((int)velocity.X), leftEn.Height);

                        if (checkPlatRec.Intersects(right))
                        {
                            // playerState = PlayerState.standing;
                            PositionX += Math.Abs(VelocityX);
                            knockedBack = false;
                            VelocityX = 0;
                            //playerState = PlayerState.standing;
                        }
                    }
                }
                //DOn't move through non passable platforms
                if ((rightEn.Intersects(left) || leftEn.Intersects(right)) && plat.Passable == false)
                {
                    if (rightEn.Intersects(left))
                    {
                        position.X -= moveSpeed;

                        if (VelocityX > 0)
                        {
                            PositionX -= (int)VelocityX;
                            velocity.X = 0;
                        }
                    }

                    if (leftEn.Intersects(right))
                    {
                        position.X += moveSpeed;

                        if (VelocityX < 0)
                        {
                            PositionX += (int)Math.Abs(VelocityX);
                            velocity.X = 0;
                        }

                    }
                }

                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (((feet.Intersects(top) && !knockedBack) || (vitalRecFeet.Intersects(top) && knockedBack) || new Rectangle(feet.X, feet.Y, feet.Width, (int)velocity.Y).Intersects(top)) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height;
                    velocity.Y = 0;


                    //--Once it collides with the ground, set the moveTimer to 0 and the boolean to false
                    //--This will make the monster start moving again
                    if (knockedBack == true)
                    {
                        moveTimer = 0;
                        currentlyInMoveState = false;
                    }
                    if (velocity.X == 0)
                        knockedBack = false;
                }
                #endregion

                //hit their head on non-passables
                if (topEn.Intersects(bottom) && velocity.Y < 0 && plat.Passable == false)
                {
                    velocity.Y = 0;
                    velocity.Y = GameConstants.GRAVITY;
                }
            }

            #region Not falling off a platform
            //--Don't fall off the platform you're on!
            if (currentPlat != null)
            {
                if (position.X < currentPlat.Rec.X - rectanglePaddingLeftRight)
                {
                    velocity.X = 0;
                    position.X = currentPlat.Rec.X - rectanglePaddingLeftRight;
                }
                if (position.X + rec.Width > currentPlat.Rec.X + currentPlat.Rec.Width + rectanglePaddingLeftRight)
                {
                    velocity.X = 0;
                    position.X = (currentPlat.Rec.X + currentPlat.Rec.Width) - rec.Width + rectanglePaddingLeftRight;
                }
            }
            #endregion

        }
    }
}