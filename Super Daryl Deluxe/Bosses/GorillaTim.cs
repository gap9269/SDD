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
    class GorillaTim : Boss
    {

        int confusedTimer = 0;
        int surprisedTimer = 0;
        int poundTimer;
        int punchTimer;
        int backAwayTimer;
        int standTime;
        int timeBeforePound;
        int minimumtimeBeforePound = 500;

        Boolean surprised = false;
        Boolean confused = false;
        Boolean punching = false;
        Boolean pounding = false;
        Boolean quickPunching = false;
        Boolean randomPound = false;

        Rectangle punchRec;
        Rectangle shockwaveRec;
        Rectangle activeShockRec;
        int activeShockTime;
        Boolean rightShock;

        enum Intentions
        {
            none,
            wantPunch,
            wantPound,
            wantBackAway,
            wantStand
        }
        Intentions intentions;

                // CONSTRUCTOR \\
        public GorillaTim(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 0;
            maxHealth = 45;
            level = 5;
            experienceGiven = 0;
            rec = new Rectangle((int)position.X, (int)position.Y, 300, 300);
            currentlyInMoveState = false;
            tolerance = 6;
            vitalRec = new Rectangle((int)position.X, (int)position.Y, 300, 300);
            moveSpeed = 4;

            addToHealthWidth = 0;
            canBeHurt = false;
            canBeKnockbacked = false;
            healthBarRec.Width = 0;
        }

        public override Rectangle GetHealthSourceRectangle()
        {

            return new Rectangle(xPos, 0, healthBarRec.Width, healthBarRec.Height);
        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            if(knockedBack)
                return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

            if(isStunned)
                return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

            if(surprised)
                return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

            if(confused)
                return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

            switch (bossState)
            {
                case BossState.standing:
                    return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

                case BossState.moving:
                    return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

                case BossState.attacking:

                    if (pounding)
                        return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

                    else if (punching || quickPunching)
                        return new Rectangle(0, 0, game.EnemySpriteSheets["standing"].Width, game.EnemySpriteSheets["standing"].Height);

                    break;
            }

            return new Rectangle();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            Move(currentMap.MapWidth);
            UpdateShockWave();

            xPos = originalHealthWidth - healthBarRec.Width;
            healthBarRec.X = originalHealthX + xPos;

            CheckWalkCollisions(7, new Vector2(5, -10));
            vitalRec.X = rec.X;
            vitalRec.Y = rec.Y;

            if(facingRight)
            {
                punchRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y, 300, 200);
                shockwaveRec = new Rectangle(vitalRec.X + vitalRec.Width - 50, vitalRec.Y + VitalRecHeight / 2, 100, 100);
            }
            else
            {
                punchRec = new Rectangle(vitalRec.X - 300, vitalRec.Y, 300, 200);
                shockwaveRec = new Rectangle(vitalRec.X - 50, vitalRec.Y + VitalRecHeight / 2, 100, 100);
            }

            if (health < 75)
                minimumtimeBeforePound = 350;
            else if (health < 40)
                minimumtimeBeforePound = 150;
            else if (health < 20)
                minimumtimeBeforePound = 60;

            if (IsDead())
            {
                game.CurrentChapter.BossFight = false;
                game.CurrentChapter.CurrentBoss = null;
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
            }
        }

        public void StartAttack(string attackType)
        {
            if (attackType == "Pound")
            {
                bossState = BossState.attacking;
                pounding = true;
                //--Reset this value
                randomPound = false;
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
            if (distanceFromPlayer < 300)
            {
                pounding = false;
                punching = false;
                quickPunching = true;
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
        }
        
        public void Attack()
        {
            #region Quick Punch
            if (quickPunching)
            {
                punchTimer++;

                if (punchTimer == 15)
                {
                    if (player.CheckIfHit(punchRec))
                    {
                        player.TakeDamage(9);

                        if (facingRight)
                            player.KnockPlayerBack(new Vector2(20, -5));
                        else
                            player.KnockPlayerBack(new Vector2(-20, -5));

                        player.HitPauseTimer = 10;
                        hitPauseTimer = 10;
                        game.Camera.ShakeCamera(5, 10);
                    }
                }

                if (punchTimer == 30)
                {
                    backAwayTimer = 35;
                }

                if (backAwayTimer > 0)
                {
                    backAwayTimer--;

                    if (CollideWithBounds())
                        backAwayTimer = 0;

                    if (facingRight)
                        PositionX -= moveSpeed;
                    else
                        PositionX += moveSpeed;

                    if (backAwayTimer == 0)
                    {
                        bossState = BossState.standing;
                        quickPunching = false;
                    }
                }
            }
            #endregion

            #region Pounding
            else if(pounding)
            {
                poundTimer++;

                //--Create attack rec for shockwave and knock player off lights
                if (poundTimer == 70)
                {
                    player.KnockPlayerDown();
                    game.Camera.ShakeCamera(15, 15);
                    activeShockRec = shockwaveRec;
                    activeShockTime = 60;

                    if (facingRight)
                        rightShock = true;
                    else
                        rightShock = false;
                }

                if (poundTimer == 100)
                {
                    bossState = BossState.standing;
                    pounding = false;
                    intentions = Intentions.none;
                }
            }
            #endregion

            #region Punching
            else if (punching)
            {
                punchTimer++;

                if (punchTimer == 15)
                {
                    if (player.CheckIfHit(punchRec))
                    {
                        player.TakeDamage(9);


                        if(facingRight)
                            player.KnockPlayerBack(new Vector2(20, -5));
                        else
                            player.KnockPlayerBack(new Vector2(-20, -5));

                        player.HitPauseTimer = 10;
                        hitPauseTimer = 10;
                        game.Camera.ShakeCamera(5, 10);
                    }
                }

                if (punchTimer == 30)
                {
                    bossState = BossState.standing;
                    punching = false;
                    intentions = Intentions.wantBackAway;
                    backAwayTimer = 35;
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
            backAwayTimer--;

            if (CollideWithBounds())
                backAwayTimer = 0;

            if (backAwayTimer == 0)
                intentions = Intentions.none;

            if (facingRight)
                PositionX -= moveSpeed;
            else
                PositionX += moveSpeed;
        }

        public override void Move(int mapWidth)
        {
            distanceFromPlayer = Vector2.Distance(new Vector2(VitalRec.Center.X, vitalRec.Center.Y), new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y));


            #region IF SURPRISED/STUNNED, DO THINGS ACCORDINGLY
            if (surprised)
                canBeStunned = true;
            else
                canBeStunned = false;

            if (isStunned)
            {
                confused = false;
                surprised = false;
                canBeKnockbacked = true;
                canBeHurt = true;
            }
            else
            {
                canBeKnockbacked = false;
                canBeHurt = false;
            }
            #endregion

            #region If the player is standing on one of the lights and the boss is not attacking, confuse him
            if (player.CurrentPlat != null && player.CurrentPlat.Rec.Y < vitalRec.Y && bossState != BossState.attacking)
            {
                if (timeBeforePound <= 0)
                {
                    timeBeforePound = randomTime.Next(minimumtimeBeforePound, 550);
                }

                timeBeforePound--;

                confused = true;
                confusedTimer = 0;
                bossState = BossState.standing;

                if (timeBeforePound == 0)
                {
                    timeBeforePound = -1;
                    randomPound = true;
                    confused = false;
                    intentions = Intentions.wantPound;
                }
            }
            #endregion

            #region If tim is confused and the player standing, but not on a light, make him surprised after seeing the player
            else if (confused && player.CurrentPlat != null && !randomPound)
            {
                if (facingRight && player.VitalRec.Center.X > vitalRec.Center.X)
                {
                    surprised = true;
                    confused = false;
                    surprisedTimer = 0;

                }
                else if (facingRight == false && player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    surprised = true;
                    confused = false;
                    surprisedTimer = 0;
                }
                else if (confused)
                {

                    confusedTimer++;

                    if (confusedTimer == 60)
                    {
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                            facingRight = false;
                        else
                            facingRight = true;

                        surprised = true;
                        surprisedTimer = 0;
                        confused = false;
                    }
                }
            }
            #endregion

            #region Keep him surprised for 100 frames, he is able to be stunned during this
            if (surprised)
            {
                surprisedTimer++;

                if (surprisedTimer == 100)
                {
                    surprised = false;
                    bossState = BossState.standing;
                    surprisedTimer = 0;

                }
            }
            #endregion

            //When confused, set a random timer to pound after, then shake the screen and call player KnockPlayerDown() if he is standing on a light

            //After stunning and hitting tim, (possibly set after knockedBack is true? pound as a response, then get rid of the lights until he pounds again?

            #region Hitting the walls, move toward the middle
            if (CollideWithBounds() && intentions == Intentions.none)
            {
                if (vitalRec.Intersects(boundaries[0].Rec))
                {
                    facingRight = true;
                    PositionX += moveSpeed;
                    bossState = BossState.moving;
                }
                else
                {
                    facingRight = false;
                    PositionX -= moveSpeed;
                    bossState = BossState.moving;
                }
            }
            #endregion

            #region If he's too far away without any action planned, move towards the player
            if (distanceFromPlayer > 700 && intentions == Intentions.none)
            {
                if (player.VitalRec.Center.X > vitalRec.Center.X)
                {
                    facingRight = true;
                    PositionX += moveSpeed;
                    bossState = BossState.moving;
                }
                else if (player.VitalRec.Center.X < vitalRec.Center.X)
                {
                    facingRight = false;
                    PositionX -= moveSpeed;
                    bossState = BossState.moving;
                }
            }
            #endregion

            #region If not stunned, surprised, or confused, and is standing, choose a random intent
            if (!surprised && !isStunned && !confused && (bossState == BossState.standing || bossState == BossState.moving) && intentions == Intentions.none && !CollideWithBounds() && distanceFromPlayer <= 700)
            {

                int intent = chooseIntent.Next(8);

                if (intent == 0)
                    intentions = Intentions.wantPound;
                else if (intent == 1 || intent == 2)
                {
                    intentions = Intentions.wantStand;
                    standTime = randomTime.Next(30, 80);
                }
                else
                    intentions = Intentions.wantPunch;
            }
            #endregion

            if (intentions == Intentions.wantStand)
            {
                standTime--;
                bossState = BossState.standing;
                if (standTime == 0)
                    intentions = Intentions.none;
            }

            #region INTENT TO GTFO
            if (intentions == Intentions.wantBackAway)
            {
                BackAway();
            }
            #endregion

            #region INTENT TO POUND
            if (intentions == Intentions.wantPound && bossState != BossState.attacking)
            {
                if ((distanceFromPlayer >= 500 && distanceFromPlayer <= 600))
                {
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
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = true;
                        PositionX += moveSpeed;
                        bossState = BossState.moving;
                    }

                    if (CollideWithBounds())
                    {
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
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;
                        PositionX -= moveSpeed;
                        bossState = BossState.moving;
                    }

                    if (CollideWithBounds())
                    {
                        StartAttack("Pound");
                        bossState = BossState.attacking;
                        poundTimer = 0;

                        if (player.VitalRec.Center.X > vitalRec.Center.X)
                            facingRight = true;
                        else
                            facingRight = false;
                    }
                }

                StartQuickAttack();

            }
            #endregion

            #region INTENT TO PUNCH YOU IN THE DICK
            if (intentions == Intentions.wantPunch && bossState != BossState.attacking && !confused && !surprised && !isStunned)
            {
                if ((distanceFromPlayer <= 350))
                {
                    StartAttack("Punch");
                    bossState = BossState.attacking;
                    punchTimer = 0;

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
                    }
                    else if (player.VitalRec.Center.X < vitalRec.Center.X)
                    {
                        facingRight = false;
                        PositionX -= moveSpeed;
                        bossState = BossState.moving;
                    }
                }
            }
            #endregion

            if (bossState == BossState.attacking)
            {
                Attack();
            }
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle coll)
        {
            base.TakeHit(damage, kbvel, coll);

            if ((surprised || confused) && !isStunned)
            {
                confused = false;
                surprised = false;
                intentions = Intentions.wantPunch;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if (facingRight)
            {

                if (knockedBack)
                    s.Draw(game.EnemySpriteSheets["flinch"], rec, GetSourceRectangle(moveFrame), Color.White);

                else if (isStunned)
                    s.Draw(game.EnemySpriteSheets["stunned"], rec, GetSourceRectangle(moveFrame), Color.White);

                else if (bossState == BossState.attacking)
                {
                    if (punching || quickPunching)
                    {
                        s.Draw(game.EnemySpriteSheets["punching"], rec, GetSourceRectangle(moveFrame), Color.White);

                        if (punchTimer == 30)
                            s.Draw(Game1.whiteFilter, punchRec, Color.Red * .4f);
                    }
                    else if (pounding)
                    {
                        s.Draw(game.EnemySpriteSheets["pounding"], rec, GetSourceRectangle(moveFrame), Color.White);
                        if (activeShockTime > 0)
                            s.Draw(Game1.whiteFilter, activeShockRec, Color.Red * .5f);
                    }
                }

                else if (surprised)
                    s.Draw(game.EnemySpriteSheets["surprised"], rec, GetSourceRectangle(moveFrame), Color.White);

                else if (confused)
                    s.Draw(game.EnemySpriteSheets["confused"], rec, GetSourceRectangle(moveFrame), Color.White);

                else
                {
                    switch (bossState)
                    {
                        case BossState.standing:
                            s.Draw(game.EnemySpriteSheets["standing"], rec, GetSourceRectangle(moveFrame), Color.White);
                            break;

                        case BossState.moving:
                            s.Draw(game.EnemySpriteSheets["moving"], rec, GetSourceRectangle(moveFrame), Color.White);
                            break;
                    }
                }
            }

            else
            {
                if (isStunned)
                    s.Draw(game.EnemySpriteSheets["stunned"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                else if (knockedBack)
                    s.Draw(game.EnemySpriteSheets["flinch"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                else if (bossState == BossState.attacking)
                {
                    if (punching || quickPunching)
                    {
                        s.Draw(game.EnemySpriteSheets["punching"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                        if (punchTimer == 30)
                            s.Draw(Game1.whiteFilter, punchRec, Color.Red * .4f);
                    }
                    else if (pounding)
                    {
                        s.Draw(game.EnemySpriteSheets["pounding"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                        if (activeShockTime > 0)
                            s.Draw(Game1.whiteFilter, activeShockRec, Color.Red * .5f);
                    }
                }

                else if (surprised)
                    s.Draw(game.EnemySpriteSheets["surprised"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                else if (confused)
                    s.Draw(game.EnemySpriteSheets["confused"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                else
                {
                    switch (bossState)
                    {
                        case BossState.standing:
                            s.Draw(game.EnemySpriteSheets["standing"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;

                        case BossState.moving:
                            s.Draw(game.EnemySpriteSheets["moving"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;
                    }
                }
            }
        }
    }
}
