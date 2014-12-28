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
    class GorillaTim : Boss
    {

        int confusedTimer = 0;
        int surprisedTimer = 0;
        int poundTimer;
        int punchTimer;
        int backAwayTimer;
        int standTime;
        int timeBeforeConfusedPound;
        int minimumtimeBeforeConfusedPound = 500;
        int poundNum = 0;

        int timeBeforeRandomPound;
        int minimumTimeBeforeRandomPound = 600;

        Boolean platformsFalling = false;
        Boolean platformsDisappearing = false;
        int platFallTimer = 0;
        float platAlpha = 1f;
        float leftPlatY, rightPlatY, platVelocity;

        public static Dictionary<String, Texture2D> animationTextures;

        Boolean surprised = false;
        Boolean confused = false;
        Boolean punching = false;
        Boolean pounding = false;
        Boolean quickPounding = false;
        Boolean randomPound = false;
        Boolean snarling = false;
        Boolean hasSunglasses = false;
        Boolean platformsGone = false;

        int damageDealtBeforePound = 0; //When this hits a certain number, Tim will pound and spawn more platforms if they aren't present
        int maxDamageBeforePound = 5;
        int blinkAmount = 0;

        Rectangle punchRec, poundRec;
        Rectangle shockwaveRec;
        Rectangle activeShockRec;
        int activeShockTime;
        Boolean rightShock;

        int punchDamage = 13;
        int poundDamage = 14;

        enum Intentions
        {
            none,
            wantPunch,
            wantPound,
            wantBackAway,
            wantStand,
            wantSnarl,
            puttingOnGlasses,
        }
        Intentions intentions;

                // CONSTRUCTOR \\
        public GorillaTim(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 0;
            maxHealth = 200;
            level = 5;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, (int)(940 * .65f), (int)(796 * .65f));
            currentlyInMoveState = false;
            tolerance = 6;
            vitalRec = new Rectangle((int)position.X, (int)position.Y, 300, 300);
            moveSpeed = 4;

            addToHealthWidth = 0;
            canBeHurt = false;
            canBeKnockbacked = false;
            healthBarRec.Width = 0;
            distanceFromBottomRecToFeet = 0; //Not going to use it for Tim
            drawHUDName = true;
        }

        public override Rectangle GetHealthSourceRectangle()
        {

            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (!isStunned)
            {
                canBeKnockbacked = false;
                canBeHurt = false;
                Move(currentMap.MapWidth);
            }
            else
            {
                #region ANIMATION
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 6)
                        frameDelay = 5;

                    if (moveFrame > 10)
                    {
                        moveFrame = 7;
                    }
                }
                #endregion

                confused = false;
                surprised = false;
                canBeKnockbacked = true;
                canBeHurt = true;
            }

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            vitalRec.X = rec.X + 145;
            vitalRec.Y = rec.Y + 200;

            vitalRec.Width = 210;
            vitalRec.Height = 300;

            if(facingRight)
            {
                punchRec = new Rectangle(vitalRec.X + vitalRec.Width - 175, vitalRec.Y - 20, 430, 220);
                poundRec = new Rectangle(vitalRec.X + vitalRec.Width - 165, rec.Y + 100, 340, 400);
               // shockwaveRec = new Rectangle(vitalRec.X + vitalRec.Width - 50, vitalRec.Y + VitalRecHeight / 2, 100, 100);
            }
            else
            {
                punchRec = new Rectangle(vitalRec.X - 270, vitalRec.Y - 20, 430, 220);
                poundRec = new Rectangle(vitalRec.X - 180, rec.Y + 100, 340, 400);
              //  shockwaveRec = new Rectangle(vitalRec.X - 50, vitalRec.Y + VitalRecHeight / 2, 100, 100);
            }

            #region Minimum time before knocking platforms down while confused
            if (health < health * .75f)
                minimumtimeBeforeConfusedPound = 350;
            else if (health < health * .40f)
                minimumtimeBeforeConfusedPound = 150;
            else if (health < health * .20f)
                minimumtimeBeforeConfusedPound = 60;
            #endregion

            if (IsDead())
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
            }
        }

        public void StartAttack(string attackType)
        {
            if (attackType == "Pound")
            {
                bossState = BossState.attacking;
                pounding = true;
            }

            if (attackType == "Punch")
            {
                bossState = BossState.attacking;
                punching = true;
                punchTimer = 0;
            }
        }

        public void StartQuickAttack()
        {

            pounding = false;
            punching = false;
            quickPounding = true;
            intentions = Intentions.wantPound;
            bossState = BossState.attacking;
            punchTimer = 0;

            if (player.VitalRec.Center.X > vitalRec.Center.X)
            {
                facingRight = true;

            }
            else if (player.VitalRec.Center.X < vitalRec.Center.X)
            {
                facingRight = false;

            }

        }
        
        public void Attack()
        {
            #region Quick Pound
            if (quickPounding)
            {
                punching = false;
                pounding = false;

                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame == 7)
                    {
                        player.KnockPlayerDown();

                        if (player.CheckIfHit(poundRec))
                        {
                            player.TakeDamage(poundDamage);

                            if (player.VelocityY < 0)
                                player.VelocityY = 0;

                            if (facingRight)
                                player.KnockPlayerBack(new Vector2(20, 15));
                            else
                                player.KnockPlayerBack(new Vector2(-20, 15));

                            player.HitPauseTimer = 6;
                            hitPauseTimer = 6;
                        }

                        game.Camera.ShakeCamera(30, 15);
                    }

                    if (moveFrame > 12)
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        bossState = BossState.standing;
                        quickPounding = false;
                        pounding = false;
                        snarling = true;
                        intentions = Intentions.wantSnarl;
                    }
                }
            }
            #endregion

            #region Pounding
            else if(pounding)
            {
                punching = false;
                quickPounding = false;

                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if(moveFrame >= 7 && moveFrame < 9)
                        player.KnockPlayerDown();

                    if (moveFrame == 7)
                    {
                       // activeShockRec = shockwaveRec;
                       // activeShockTime = 60;

                        //if (facingRight)
                        //    rightShock = true;
                        //else
                        //    rightShock = false;

                        if (player.CheckIfHit(poundRec))
                        {
                            player.TakeDamage(poundDamage);


                            if (facingRight)
                                player.KnockPlayerBack(new Vector2(10, 5));
                            else
                                player.KnockPlayerBack(new Vector2(-10, 5));

                            player.HitPauseTimer = 6;
                            hitPauseTimer = 6;
                        }

                        if (!platformsGone && poundNum == 1)
                        {
                            if (currentMap.Platforms.Contains(NorthHall.leftTimPlat))
                            {
                                platformsDisappearing = true;

                                currentMap.Platforms.Remove(NorthHall.leftTimPlat);
                                currentMap.Platforms.Remove(NorthHall.rightTimPlat);
                            }
                        }
                        else if (platformsGone && poundNum == 1)
                        {
                            platformsFalling = true;
                        }

                        game.Camera.ShakeCamera(30, 15);
                    }

                    if (moveFrame > 12)
                    {
                        poundNum++;

                        if (poundNum == 2)
                        {
                            moveFrame = 0;
                            frameDelay = 5;
                            bossState = BossState.standing;
                            pounding = false;
                            intentions = Intentions.none;
                            //--Reset this value
                            randomPound = false;
                            poundNum = 0;
                            damageDealtBeforePound = 0;
                            timeBeforeRandomPound = 0;
                            platformsGone = !platformsGone;

                            minimumTimeBeforeRandomPound = randomTime.Next(600, 1000);
                        }
                        else
                        {
                            moveFrame = 0;
                            frameDelay = 5;

                            damageDealtBeforePound = 0;
                        }
                    }
                }

            }
            #endregion

            #region Punching
            else if (punching)
            {
                quickPounding = false;
                pounding = false;

                frameDelay--;

                if (moveFrame == 6)
                {
                    if (player.CheckIfHit(punchRec))
                    {
                        player.TakeDamage(punchDamage);


                        if (facingRight)
                            player.KnockPlayerBack(new Vector2(30, -10));
                        else
                            player.KnockPlayerBack(new Vector2(-30, -10));

                        player.HitPauseTimer = 6;
                        hitPauseTimer = 6;
                        game.Camera.ShakeCamera(5, 10);
                    }
                }

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame < 5)
                        frameDelay = 4;

                    if (moveFrame > 9)
                    {
                        int randomAction;

                        randomAction = randomTime.Next(3);

                        //0 = snarl, 1 = back away, 2 = pound
                        switch (randomAction)
                        {
                            case 0:
                                moveFrame = 0;
                                bossState = BossState.standing;
                                snarling = true;
                                intentions = Intentions.wantSnarl;
                                break;
                            case 1:
                                moveFrame = 8;
                                frameDelay = 5;
                                bossState = BossState.moving;
                                intentions = Intentions.wantBackAway;
                                break;
                            case 2:
                                moveFrame = 0;
                                bossState = BossState.standing;
                                StartQuickAttack();
                                break;
                        }
                        frameDelay = 5;
                        punching = false;
                    }
                }
            }
            #endregion
        }

        public void UpdateShockWave()
        {
            if (activeShockTime > 0)
            {
                activeShockTime--;

                if (rightShock)
                    activeShockRec.X += 18;
                else
                    activeShockRec.X -= 18;

                if (player.CheckIfHit(activeShockRec))
                {
                    player.TakeDamage(7);
                    player.KnockPlayerBack(new Vector2(0, -7));
                    activeShockTime = 0;
                }
            }
        }

        public void BackAway()
        {
            bossState = BossState.moving;

            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame--;
                frameDelay = 5;
            }

            if (moveFrame < 0 || CollideWithBounds())
            {
                bossState = BossState.standing;
                moveFrame = 0;
                frameDelay = 5;
                intentions = Intentions.wantStand;
                standTime = randomTime.Next(30, 80);
            }

            //backAwayTimer--;

            //if (CollideWithBounds())
            //    backAwayTimer = 0;

            //if (backAwayTimer == 0)
            //    intentions = Intentions.none;

            
                if (facingRight)
                    PositionX -= moveSpeed;
                else
                    PositionX += moveSpeed;
            
        }

        public void CutsceneStand()
        {
            bossState = BossState.standing;

            frameDelay--;

            if (frameDelay <= 0)
            {
                moveFrame++;
                frameDelay = 5;

                if (moveFrame > 5)
                {
                    moveFrame = 0;
                }
            }

        }

        public override void Move(int mapWidth)
        {
            distanceFromPlayer = Vector2.Distance(new Vector2(VitalRec.Center.X, vitalRec.Center.Y), new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y));

            timeBeforeRandomPound++;

            if (!surprised)
                canBeStunned = false;

            #region IF SURPRISED/STUNNED, DO THINGS ACCORDINGLY
            if (surprised)
            {
                #region ANIMATION
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 4)
                        frameDelay = 7;

                    if (moveFrame > 6)
                    {
                        moveFrame = 5;
                    }
                }
                #endregion

                bossState = BossState.standing;

                canBeStunned = true;

                surprisedTimer++;

                if (surprisedTimer == 100)
                {
                    surprised = false;
                    bossState = BossState.standing;
                    surprisedTimer = 0;
                    moveFrame = 0;
                    frameDelay = 5;
                }
            }

            #endregion

            #region If the player is standing on one of the lights and the boss is not attacking, confuse him
            if ((player.CurrentPlat != null && player.CurrentPlat.Rec.Y < vitalRec.Y && !randomPound && bossState != BossState.attacking && !confused) || confused)
            {

                #region ANIMATION
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 5)
                        frameDelay = 7;

                    if (moveFrame > 9)
                    {
                        moveFrame = 6;
                    }
                }
                #endregion

                if (timeBeforeConfusedPound <= 0)
                {
                    timeBeforeConfusedPound = randomTime.Next(minimumtimeBeforeConfusedPound, 550);
                }

                timeBeforeConfusedPound--;

                //Set the starting frame
                if (!confused)
                {
                    surprised = false;
                    surprisedTimer = 0;
                    moveFrame = 0;
                    frameDelay = 5;
                    intentions = Intentions.none;
                    pounding = false;
                    punching = false;
                    quickPounding = false;
                    snarling = false;
                }

                confused = true;
                confusedTimer = 0;
                bossState = BossState.standing;


                if (timeBeforeConfusedPound == 0)
                {
                    moveFrame = 0;
                    frameDelay = 5;
                    timeBeforeConfusedPound = -1;
                    randomPound = true;
                    confused = false;
                    intentions = Intentions.wantPound;
                }
            }
            #endregion

            #region If tim is confused and the player standing, but not on a light, make him surprised after seeing the player
            if (confused && player.CurrentPlat != null && !randomPound && !(player.CurrentPlat.Rec.Y < vitalRec.Y))
            {
                if (facingRight && player.VitalRec.Center.X > vitalRec.Center.X) //Player lands in front of Tim
                {
                    frameDelay = 5;
                    moveFrame = 0;
                    surprised = true;
                    confused = false;
                    surprisedTimer = 0;
                    intentions = Intentions.none;
                }
                else if (facingRight == false && player.VitalRec.Center.X < vitalRec.Center.X) //Player lands in front of Tim
                {
                    frameDelay = 5;
                    moveFrame = 0;
                    surprised = true;
                    confused = false;
                    surprisedTimer = 0;
                    intentions = Intentions.none;
                }
                else if (confused) //Lands behind tim, wait before turning
                {

                    if (player.VitalRec.Center.X < vitalRec.Center.X)
                        facingRight = false;
                    else
                        facingRight = true;

                    frameDelay = 5;
                    moveFrame = 0;
                    surprised = true;
                    surprisedTimer = 0;
                    confused = false;
                    intentions = Intentions.none;
                }

            }
            #endregion

            #region Hitting the walls, move toward the middle
            if (CollideWithBounds() && intentions == Intentions.none)
            {
                if (vitalRec.Intersects(boundaries[0].Rec))
                {
                    facingRight = true;
                    PositionX += moveSpeed;
                    bossState = BossState.moving;

                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;

                        if (moveFrame > 8)
                        {
                            moveFrame = 0;
                        }
                    }
                }
                else
                {
                    facingRight = false;
                    PositionX -= moveSpeed;
                    bossState = BossState.moving;

                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;

                        if (moveFrame > 8)
                        {
                            moveFrame = 0;
                        }
                    }
                }
            }
            #endregion

            #region If he's too far away without any action planned, move towards the player
            if (distanceFromPlayer > 700 && intentions == Intentions.none && !confused && !surprised && !snarling && bossState != BossState.attacking)
            {
                if (player.VitalRec.Center.X > vitalRec.Center.X)
                {
                    facingRight = true;

                    
                    PositionX += moveSpeed;
                    bossState = BossState.moving;

                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;

                        if (moveFrame > 8)
                        {
                            moveFrame = 0;
                        }
                    }
                }
                else if (player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    facingRight = false;
                    
                        PositionX -= moveSpeed;
                    bossState = BossState.moving;

                    frameDelay--;

                    if (frameDelay <= 0)
                    {
                        moveFrame++;
                        frameDelay = 5;

                        if (moveFrame > 8)
                        {
                            moveFrame = 0;
                        }
                    }
                }
            }
            #endregion

            #region If not stunned, surprised, snarling, or confused, and is standing, choose a random intent
            else if (!snarling && !surprised && !isStunned && !confused && (bossState == BossState.standing || bossState == BossState.moving) && intentions == Intentions.none && !CollideWithBounds() && distanceFromPlayer <= 700)
            {
                if ((damageDealtBeforePound >= maxDamageBeforePound || timeBeforeRandomPound >= minimumTimeBeforeRandomPound) && !currentMap.Platforms.Contains(NorthHall.rightTimPlat))
                {
                    moveFrame = 0;
                    frameDelay = 5;
                    intentions = Intentions.wantPound;
                }
                else if (distanceFromPlayer < 230)
                {
                    StartQuickAttack();
                }
                else
                {
                    int intent = chooseIntent.Next(8);

                    if (intent < 3)
                    {
                        if (intent == 1)
                        {
                            moveFrame = 0;
                            frameDelay = 5;
                            bossState = BossState.standing;
                            snarling = true;
                            intentions = Intentions.wantSnarl;
                        }
                        else
                        {
                            moveFrame = 0;
                            frameDelay = 5;
                            intentions = Intentions.wantStand;
                            standTime = randomTime.Next(30, 80);
                        }
                    }
                    else
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        intentions = Intentions.wantPunch;
                    }
                }
            }
            #endregion

            #region SNARLING
            else if (snarling)
            {
                bossState = BossState.standing;
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 6)
                    {
                        frameDelay = 5;
                        moveFrame = 0;
                        intentions = Intentions.none;
                        snarling = false;
                    }
                }
            }
            #endregion

            #region PUTTING ON SUNGLASSES
            else if (intentions == Intentions.puttingOnGlasses)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 15)
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        intentions = Intentions.none;
                        hasSunglasses = true;
                    }
                }
            }
            #endregion

            #region INTENT TO STAND
            else if (intentions == Intentions.wantStand)
            {
                standTime--;
                bossState = BossState.standing;

                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 5)
                    {
                        moveFrame = 0;
                    }
                }

                if (standTime == 0)
                {
                    frameDelay = 5;
                    moveFrame = 0;
                    intentions = Intentions.none;
                }
            }
            #endregion

            #region INTENT TO GTFO
            else if (intentions == Intentions.wantBackAway)
            {
                BackAway();
            }
            #endregion

            #region INTENT TO POUND
            else if (intentions == Intentions.wantPound && bossState != BossState.attacking && !confused && !surprised && !isStunned && !snarling)
            {
                if ((distanceFromPlayer >= 500 && distanceFromPlayer <= 600) || randomPound)
                {
                    moveFrame = 0;
                    frameDelay = 5;
                    StartAttack("Pound");
                    bossState = BossState.attacking;
                    poundTimer = 0;

                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                        facingRight = true;
                    else
                        facingRight = false;

                }
                else if (distanceFromPlayer < 500)
                {
                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                    {
                        facingRight = false;
                            PositionX -= moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = true;
                
                            PositionX += moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }

                    if (CollideWithBounds())
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        StartAttack("Pound");
                        bossState = BossState.attacking;
                        poundTimer = 0;

                        if (player.VitalRec.Center.X > vitalRec.Center.X)
                            facingRight = true;
                        else
                            facingRight = false;
                    }
                }
                else
                {
                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                    {
                        facingRight = true;
                        PositionX += moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;
                        PositionX -= moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }

                    if (CollideWithBounds())
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        StartAttack("Pound");
                        bossState = BossState.attacking;
                        poundTimer = 0;

                        if (player.VitalRec.Center.X > vitalRec.Center.X)
                            facingRight = true;
                        else
                            facingRight = false;
                    }
                }
            }
            #endregion

            #region INTENT TO PUNCH YOU IN THE DICK
            else if (intentions == Intentions.wantPunch && bossState != BossState.attacking && !confused && !surprised && !isStunned && !snarling)
            {
                if ((distanceFromPlayer <= 300))
                {
                    StartAttack("Punch");
                    bossState = BossState.attacking;
                    moveFrame = 0;
                    frameDelay = 10;

                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                        facingRight = true;
                    else
                        facingRight = false;
                }
                
                else
                {
                    if (player.VitalRec.Center.X > vitalRec.Center.X)
                    {
                        facingRight = true;

                        PositionX += moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;
                            PositionX -= moveSpeed;
                        bossState = BossState.moving;

                        frameDelay--;

                        if (frameDelay <= 0)
                        {
                            moveFrame++;
                            frameDelay = 5;

                            if (moveFrame > 8)
                            {
                                moveFrame = 0;
                            }
                        }
                    }
                }
            }
            #endregion

            if (bossState == BossState.attacking)
            {
                Attack();
            }
        }

        public override void Stun(int time)
        {
            if ((canBeStunned || !hasSunglasses) && intentions != Intentions.puttingOnGlasses &&  stunTime <= 0)
            {
                isStunned = true;
                stunTime = 120;
                moveFrame = 0;
                frameDelay = 5;
            }
        }

        public override void UpdateStun()
        {
            if (isStunned)
            {
                stunTime--;

                bossState = BossState.standing;
                canBeKnockbacked = true;

                if (stunTime <= 0)
                {
                    canBeKnockbacked = false;
                    isStunned = false;
                    stunTime = 0;
                    moveFrame = 0;
                    frameDelay = 5;
                    intentions = Intentions.none;

                    if (hasSunglasses == false)
                    {
                        intentions = Intentions.puttingOnGlasses;
                        bossState = BossState.standing;
                    }
                    else
                    {
                        intentions = Intentions.wantPound;
                    }
                }
            }
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle coll)
        {
            if (!isStunned)
            {
                damage = 1;

                damageDealtBeforePound++;
            }
            else
                damage = (int)(damage * 1.75f);

            ShakeHealthBar();
            damage -= tolerance;

            hasBeenHit = true;

            if (damage <= 0)
                damage = 1;

            health -= damage;

            kbvel = kbvel / 2;

            if(isStunned)
                KnockBack(kbvel);

            AddDamageNum(damage, coll);

            if ((surprised || confused) && !isStunned)
            {
                confused = false;
                surprised = false;
                moveFrame = 0;
                frameDelay = 0;
            }
        }

        public override void Draw(SpriteBatch s)
        {

            //s.Draw(Game1.whiteFilter, poundRec, Color.Blue);

            #region HAS SUNGLASSES ON
            if (hasSunglasses)
            {
                if (facingRight)
                {
                    if (isStunned)
                        s.Draw(GorillaTim.animationTextures["StunnedSun" + moveFrame], rec, Color.White);

                    else if (bossState == BossState.attacking)
                    {
                        if (punching)
                        {
                            s.Draw(GorillaTim.animationTextures["PunchSun" + moveFrame], rec, Color.White);
                        }
                        else if (pounding || quickPounding)
                        {
                            s.Draw(GorillaTim.animationTextures["poundSun" + moveFrame], rec, Color.White);
                        }
                    }

                    else if (surprised)
                        s.Draw(GorillaTim.animationTextures["SurprisedSun" + moveFrame], rec, Color.White);

                    else if (confused)
                        s.Draw(GorillaTim.animationTextures["ConfusedSun" + moveFrame], rec, Color.White);

                    else
                    {
                        switch (bossState)
                        {
                            case BossState.standing:
                                if (snarling)
                                {
                                    if (moveFrame < 1)
                                        s.Draw(GorillaTim.animationTextures["StandSun" + moveFrame], rec, Color.White);
                                    else
                                        s.Draw(GorillaTim.animationTextures["SnarlSun" + (moveFrame - 1)], rec, Color.White);
                                }
                                else if (intentions == Intentions.puttingOnGlasses)
                                    s.Draw(GorillaTim.animationTextures["PutOn" + moveFrame], rec, Color.White);
                                else
                                    s.Draw(GorillaTim.animationTextures["StandSun" + moveFrame], rec, Color.White);
                                break;

                            case BossState.moving:
                                s.Draw(GorillaTim.animationTextures["walkSun" + moveFrame], rec, Color.White);
                                break;
                        }
                    }
                }

                else
                {
                    if (isStunned)
                        s.Draw(GorillaTim.animationTextures["StunnedSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else if (bossState == BossState.attacking)
                    {
                        if (punching)
                        {
                            s.Draw(GorillaTim.animationTextures["PunchSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                            if (punchTimer == 30)
                                s.Draw(Game1.whiteFilter, punchRec, Color.Red * .4f);
                        }
                        else if (pounding || quickPounding)
                        {
                            s.Draw(GorillaTim.animationTextures["poundSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                            //if (activeShockTime > 0)
                                //s.Draw(Game1.whiteFilter, activeShockRec, Color.Red * .5f);
                        }
                    }

                    else if (surprised)
                        s.Draw(GorillaTim.animationTextures["SurprisedSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else if (confused)
                        s.Draw(GorillaTim.animationTextures["ConfusedSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else
                    {
                        switch (bossState)
                        {
                            case BossState.standing:
                                if (snarling)
                                {
                                    if (moveFrame < 1)
                                        s.Draw(GorillaTim.animationTextures["StandSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                    else
                                        s.Draw(GorillaTim.animationTextures["SnarlSun" + (moveFrame - 1)], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                }
                                else if (intentions == Intentions.puttingOnGlasses)
                                    s.Draw(GorillaTim.animationTextures["PutOn" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                else
                                    s.Draw(GorillaTim.animationTextures["StandSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;

                            case BossState.moving:
                                s.Draw(GorillaTim.animationTextures["walkSun" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                        }
                    }
                }
            }
            #endregion

            #region NO SUNGLASSES
            else
            {
                if (facingRight)
                {
                    if (isStunned)
                        s.Draw(GorillaTim.animationTextures["Stunned" + moveFrame], rec, Color.White);

                    else if (bossState == BossState.attacking)
                    {
                        if (punching)
                        {
                            s.Draw(GorillaTim.animationTextures["punch" + moveFrame], rec, Color.White);
                        }
                        else if (pounding || quickPounding)
                        {
                            s.Draw(GorillaTim.animationTextures["pound" + moveFrame], rec, Color.White);
                        }
                    }

                    else if (surprised)
                        s.Draw(GorillaTim.animationTextures["Surprised" + moveFrame], rec, Color.White);

                    else if (confused)
                        s.Draw(GorillaTim.animationTextures["Confused" + moveFrame], rec, Color.White);

                    else
                    {
                        switch (bossState)
                        {
                            case BossState.standing:
                                if (snarling)
                                {
                                    if (moveFrame < 1)
                                        s.Draw(GorillaTim.animationTextures["Stand" + moveFrame], rec, Color.White);
                                    else
                                        s.Draw(GorillaTim.animationTextures["Snarl" + (moveFrame - 1)], rec, Color.White);
                                }
                                else if (intentions == Intentions.puttingOnGlasses)
                                    s.Draw(GorillaTim.animationTextures["PutOn" + moveFrame], rec, Color.White);
                                else
                                    s.Draw(GorillaTim.animationTextures["Stand" + moveFrame], rec, Color.White);
                                break;

                            case BossState.moving:
                                s.Draw(GorillaTim.animationTextures["walk" + moveFrame], rec, Color.White);
                                break;
                        }
                    }
                }

                else
                {
                    if (isStunned)
                        s.Draw(GorillaTim.animationTextures["Stunned" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else if (bossState == BossState.attacking)
                    {
                        if (punching)
                        {
                            s.Draw(GorillaTim.animationTextures["punch" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                            //if (punchTimer == 30)
                               // s.Draw(Game1.whiteFilter, punchRec, Color.Red * .4f);
                        }
                        else if (pounding || quickPounding)
                        {
                            s.Draw(GorillaTim.animationTextures["pound" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                            if (activeShockTime > 0)
                                s.Draw(Game1.whiteFilter, activeShockRec, Color.Red * .5f);
                        }
                    }

                    else if (surprised)
                        s.Draw(GorillaTim.animationTextures["Surprised" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else if (confused)
                        s.Draw(GorillaTim.animationTextures["Confused" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    else
                    {
                        switch (bossState)
                        {
                            case BossState.standing:
                                if (snarling)
                                {
                                    if (moveFrame < 1)
                                        s.Draw(GorillaTim.animationTextures["Stand" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                    else
                                        s.Draw(GorillaTim.animationTextures["Snarl" + (moveFrame - 1)], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                }
                                else if (intentions == Intentions.puttingOnGlasses)
                                    s.Draw(GorillaTim.animationTextures["PutOn" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                else
                                    s.Draw(GorillaTim.animationTextures["Stand" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;

                            case BossState.moving:
                                s.Draw(GorillaTim.animationTextures["walk" + moveFrame], new Rectangle(rec.X - 122, rec.Y, rec.Width, rec.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                                break;
                        }
                    }
                }
            }
            #endregion
        }

        public void DrawPlatformsFalling(SpriteBatch s)
        {
            if (platformsFalling)
            {
                platVelocity += GameConstants.GRAVITY;
                if (leftPlatY < 190)
                {
                    leftPlatY += platVelocity;

                    if (leftPlatY > 175)
                        leftPlatY = 175;
                }
                if (rightPlatY < 175)
                {
                    rightPlatY += platVelocity;

                    if (rightPlatY > 175)
                        rightPlatY = 175;
                }

                if (leftPlatY == 175 && rightPlatY == 175)
                {
                    currentMap.Platforms.Add(NorthHall.leftTimPlat);
                    currentMap.Platforms.Add(NorthHall.rightTimPlat);
                    platformsFalling = false;

                    platVelocity = 0;
                    leftPlatY = -150;
                    rightPlatY = -75;
                }
                else
                {

                    s.Draw(NorthHall.timPlatform, new Rectangle(2590, (int)leftPlatY, 420, 50), Color.White);
                    s.Draw(NorthHall.timPlatform, new Rectangle(3240, (int)rightPlatY, 420, 50), Color.White);
                }
            }
        }

        public void DrawPlatformsDisappearing(SpriteBatch s)
        {
            if (platformsDisappearing)
            {
                platFallTimer++;

                if (platFallTimer <= 15)
                    platAlpha = 1f;
                else
                    platAlpha = 0f;

                if (platFallTimer > 30)
                {
                    blinkAmount++;
                    platFallTimer = 0;
                }

                if (blinkAmount == 4)
                {
                    blinkAmount = 0;
                    platFallTimer = 0;
                    platformsDisappearing = false;
                }

                s.Draw(NorthHall.timPlatform, new Rectangle(NorthHall.rightTimPlat.RecX - 10 + NorthHall.rightTimPlat.RecWidth / 2, NorthHall.rightTimPlat.RecY + NorthHall.rightTimPlat.RecHeight / 2, NorthHall.rightTimPlat.RecWidth + 20, NorthHall.rightTimPlat.RecHeight), null, Color.White * platAlpha, .4f, new Vector2(NorthHall.timPlatform.Width / 2, NorthHall.timPlatform.Height / 2), SpriteEffects.None, 0);

                s.Draw(NorthHall.timPlatform, new Rectangle(NorthHall.leftTimPlat.RecX - 10 + NorthHall.leftTimPlat.RecWidth / 2, NorthHall.leftTimPlat.RecY + NorthHall.leftTimPlat.RecHeight / 2, NorthHall.leftTimPlat.RecWidth + 20, NorthHall.leftTimPlat.RecHeight), null, Color.White * platAlpha, -.2f, new Vector2(NorthHall.timPlatform.Width / 2, NorthHall.timPlatform.Height / 2), SpriteEffects.None, 0);
            }
        }
    }
}
