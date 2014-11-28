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
    public class GroundHole
    {
        int timer;
        float alpha;
        Rectangle rec;
        Boolean facingRight;
        public bool finished = false;
        Texture2D sprite;

        public GroundHole(Boolean facingRight, Rectangle r, Texture2D s)
        {
            this.facingRight = facingRight;
            rec = r;
            alpha = 1f;
            timer = 300;
            sprite = s;
        }

        public void Update()
        {
            if (timer > 0)
                timer--;

            else
            {
                alpha -= .05f;

                if (alpha <= 0)
                    finished = true;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if(!facingRight)
                s.Draw(sprite, rec, new Rectangle(2820, 2775, 940, 555), Color.White * alpha);
            else
                s.Draw(sprite, rec, new Rectangle(2820, 2775, 940, 555), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }

    public class FartCloud
    {
        int timer;
        int moveframe;
        Rectangle rec;
        Boolean facingRight;
        public bool finished = false;
        Texture2D sprite;

        public FartCloud(Boolean facingRight, Rectangle r, Texture2D s)
        {
            this.facingRight = facingRight;
            rec = r;
            sprite = s;
            timer = 2;

            if (facingRight)
            {
                rec.X = rec.X + 56;
                rec.Y = rec.Y + 266;
            }
            else
            {
                rec.X = rec.X + 512;
                rec.Y = rec.Y + 266;
            }

            rec.Width = 372;
            rec.Height = 255;
        }

        public void Update()
        {
            timer--;

            if (timer == 0)
            {
                moveframe++;
                timer = 2;

                if (moveframe > 12)
                    finished = true;
            }

            if (moveframe > 5 && Game1.Player.CheckIfHit(rec))
            {
                Game1.Player.StunDaryl(120);
            }

        }

        public Rectangle GetBackSourceRec()
        {
            if (moveframe < 5)
                return new Rectangle(1880 + (moveframe * 372), 2785, 372, 255);
            else if (moveframe < 10)
                return new Rectangle(1880 + ((moveframe - 5) * 372), 3040, 372, 255);
            else
                return new Rectangle(((moveframe - 10) * 372), 3342, 372, 255);

        }

        public Rectangle GetForeSourceRec()
        {
            if (moveframe < 7 && moveframe > 0)
                return new Rectangle(1116 + ((moveframe - 1) * 372), 3342, 372, 255);
            else
                return new Rectangle(((moveframe - 8) * 372), 3597, 372, 255);
        }

        public void Draw(SpriteBatch s)
        {
            if(!facingRight)
                s.Draw(sprite, rec, GetBackSourceRec(), Color.White);
            else
                s.Draw(sprite, rec, GetBackSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }

        public void DrawFore(SpriteBatch s)
        {
            if (!facingRight)
                s.Draw(sprite, rec, GetForeSourceRec(), Color.White);
            else
                s.Draw(sprite, rec, GetForeSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }

    public class FieldTroll : Enemy
    {
        Texture2D attackSprite, fallSprite1, clubGoneSprite;
        Boolean falling = false;
        Boolean pickingUpClub = false;
        Boolean kicking = false;
        Boolean spitting = false;
        Boolean farting = false;
        Boolean liftPoofing = false;

        Rectangle overAttackRec;

        int damageTakenInFiveSeconds;
        int fiveSecondTimer = 0;
        int fallCooldown;
        int damageToFall = 3000;
        int fallSmokeFrame = 0;
        int fallSmokeTimer = 0;

        public static Dictionary<String, SoundEffect> trollSounds;
        public static List<SoundEffectInstance> currentlyPlayingSounds;

        int goblinSpitAmmo;

        int spitCooldown, kickCooldown, liftPoofTimer, liftPoofFrame;

        Rectangle clubDisappearRec, liftDisappearRec, kickRec;
        Boolean deadFalling = false;

        float stepShakeMag;

        List<GroundHole> groundHoles;

        TreasureChest trollChest;

        int clubDamage, kickDamage;

        FartCloud currentFart;

        public FieldTroll(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur, TreasureChest chest)
            : base(pos, type, g, ref play, cur)
        {
            health = 30000;
            maxHealth = 30000;//45 is the real value
            level = 16;
            experienceGiven = 1000;
            rec = new Rectangle((int)position.X, (int)position.Y, 940, 555);
            currentlyInMoveState = false;
            enemySpeed = 2;
            tolerance = 250;
            vitalRec = new Rectangle(rec.X, rec.Y, 250, 350);
            maxHealthDrop = 0;
            moneyToDrop = 0f;
            canBeKnockbacked = false;
            canBeStunned = false;
            attackSprite = game.EnemySpriteSheets["TrollAttack"];
            fallSprite1 = game.EnemySpriteSheets["TrollFall"];
            clubGoneSprite = game.EnemySpriteSheets["TrollClubGone"];
            groundHoles = new List<GroundHole>();
            deathRec = new Rectangle(0, 0, 250, 250);

            currentlyPlayingSounds = new List<SoundEffectInstance>();

            trollChest = chest;

            kickRec = new Rectangle(0, 0, 125, 100);

            kickDamage = 200;
            clubDamage = 300;

            kickCooldown = moveTime.Next(200, 300);
            spitCooldown = moveTime.Next(400, 1200);

            goblinSpitAmmo = 10;

            notEffectiveRangedMelee = AttackType.RangedOrMelee.Ranged;

            notEffectiveMultiplier = .8f;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            if (falling)
            {
                if (moveFrame < 4)
                    return new Rectangle((940 * moveFrame), 0, 940, 555);
                else if (moveFrame < 8)
                    return new Rectangle(940 * (moveFrame - 4), 555, 940, 555);
                else if (moveFrame < 12)
                    return new Rectangle(940 * (moveFrame - 8), 1110, 940, 555);
                else if (moveFrame < 16)
                    return new Rectangle(940 * (moveFrame - 12), 1665, 940, 555);
                else if (moveFrame < 20)
                    return new Rectangle(940 * (moveFrame - 16), 2220, 940, 555);
                else
                    return new Rectangle(940 * (moveFrame - 20), 2775, 940, 555);
            }
            else if (pickingUpClub)
            {
                if (moveFrame == 0)
                    return new Rectangle(2820, 1665, 940, 555);
                else if (moveFrame < 5)
                    return new Rectangle(940 * (moveFrame - 1), 2220, 940, 555);
                else if (moveFrame < 9)
                    return new Rectangle(940 * (moveFrame - 5), 2775, 940, 555);
                else
                    return new Rectangle(0, 3330, 940, 555);

            }
            else if (kicking)
            {
                if (moveFrame == 0)
                    return new Rectangle(2820, 555, 940, 555);
                else if (moveFrame < 5)
                    return new Rectangle(940 * (moveFrame - 1), 1110, 940, 555);
                else
                    return new Rectangle(940 * (moveFrame - 5), 1665, 940, 555);

            }
            else if (spitting)
            {
                if (moveFrame < 4)
                    return new Rectangle(940 * moveFrame, 2220, 940, 555);
                else
                    return new Rectangle(940 * (moveFrame - 4), 2775, 940, 555);
            }
            else
            {
                switch (enemyState)
                {
                    case EnemyState.standing:
                        return new Rectangle(0, 0, 940, 555);

                    case EnemyState.moving:
                        if (moveFrame < 3)
                            return new Rectangle(940 + (940 * moveFrame), 0, 940, 555);
                        else if (moveFrame < 7)
                            return new Rectangle(940 * (moveFrame - 3), 555, 940, 555);
                        else if (moveFrame < 11)
                            return new Rectangle(940 * (moveFrame - 7), 1110, 940, 555);
                        else
                            return new Rectangle(940 * (moveFrame - 11), 1665, 940, 555);

                    case EnemyState.attacking:
                        if (attackFrame < 4)
                            return new Rectangle((940 * attackFrame), 0, 940, 555);
                        else if (attackFrame < 8)
                            return new Rectangle(940 * (attackFrame - 4), 555, 940, 555);
                        else if (attackFrame < 12)
                            return new Rectangle(940 * (attackFrame - 8), 1110, 940, 555);
                        else if (attackFrame < 16)
                            return new Rectangle(940 * (attackFrame - 12), 1665, 940, 555);
                        else if (attackFrame < 20)
                            return new Rectangle(940 * (attackFrame - 16), 2220, 940, 555);
                        else
                            return new Rectangle(940 * (attackFrame - 20), 2775, 940, 555);
                }
            }

            return new Rectangle();
        }

        public Rectangle GetClubDisappearSource()
        {
            if (moveFrame < 4)
                return new Rectangle(940 * moveFrame, 0, 940, 555);
            else if (moveFrame < 6)
                return new Rectangle(940 * (moveFrame - 4), 555, 940, 555);
            else
                return new Rectangle();
        }

        public void Spit()
        {
            if (moveFrame == 2 && frameDelay == 4)
            {
                float dis = Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));
                float vol = 500 / Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));
                if (dis < 1800)
                {
                    currentlyPlayingSounds.Add(trollSounds["enemy_troll_summon"].CreateInstance());
                    if (vol > 1)
                        vol = 1;
                    currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Volume = vol;
                    currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();
                }
            }
            frameDelay--;

            if (frameDelay == 0)
            {
                moveFrame++;
                if (moveFrame == 2 || moveFrame == 3)
                    frameDelay = 10;
                else if (moveFrame == 5)
                    frameDelay = 10;
                else
                    frameDelay = 5;


                if (moveFrame == 3)
                {
                    if (!facingRight)
                    {
                        Goblin spit = new Goblin(new Vector2(rec.X + 211, rec.Y + 265), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                        spit.TimeBeforeSpawn = 0;
                        spit.SpawnWithPoof = false;
                        spit.Flying = true;
                        spit.FacingRight = false;
                        spit.VelocityX = -12;
                        spit.CurrentPlat = this.currentPlat;

                        game.CurrentChapter.CurrentMap.EnemiesInMap.Add(spit);
                    }
                    else
                    {
                        Goblin spit = new Goblin(new Vector2(rec.X + 577, rec.Y + 265), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                        spit.TimeBeforeSpawn = 0;
                        spit.SpawnWithPoof = false;
                        spit.Flying = true;
                        spit.FacingRight = true;
                        spit.VelocityX = 12;
                        spit.CurrentPlat = this.currentPlat;

                        game.CurrentChapter.CurrentMap.EnemiesInMap.Add(spit);
                    }
                }

                if (moveFrame == 6)
                {
                    frameDelay = 5;
                    moveFrame = 0;
                    spitting = false;
                    spitCooldown = moveTime.Next(400, 1800);
                    goblinSpitAmmo--;
                }
            }
        }

        public void Kick()
        {
            frameDelay--;

            if (frameDelay == 0)
            {
                frameDelay = 5;

                if (moveFrame == 8)
                    frameDelay = 15;

                moveFrame++;

                if (moveFrame == 4)
                {
                    for (int i = 0; i < game.CurrentChapter.CurrentMap.EnemiesInMap.Count; i++)
                    {
                        if (game.CurrentChapter.CurrentMap.EnemiesInMap[i] is Goblin && (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).Kicked == false && game.CurrentChapter.CurrentMap.EnemiesInMap[i].VitalRec.Intersects(kickRec))
                        {
                            (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).Kicked = true;
                            (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).Flying = true;

                            if (facingRight)
                            {
                                (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).KnockBack(new Vector2(45, -10));
                                game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = true;
                                (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).GotKicked();
                            }
                            else
                            {
                                (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).KnockBack(new Vector2(-45, -10));
                                game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = false;
                                (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).GotKicked();
                            }

                            float dis = Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));
                            float vol = 500 / Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));
                            if ( dis < 1800)
                                game.TempPlayHitSound(vol);

                            Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(kickRec, game.CurrentChapter.CurrentMap.EnemiesInMap[i].VitalRec));
                        }
                    }

                    if (player.CheckIfHit(kickRec) && player.InvincibleTime <= 0)
                    {
                        player.TakeDamage(kickDamage);

                        if(facingRight)
                            player.KnockPlayerBack(new Vector2(35, -10));
                        else
                            player.KnockPlayerBack(new Vector2(-35, -10));
                        hitPauseTimer = 4;
                        player.HitPauseTimer = 4;
                        game.Camera.ShakeCamera(3, 3);
                        MyGamePad.SetRumble(5, 1f);

                            game.TempPlayHitSound();

                        Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                    }
                }

                if (moveFrame == 9)
                {
                    moveFrame = 5;
                    frameDelay = 5;
                    kicking = false;

                    kickCooldown = moveTime.Next(200, 300);
                }
            }
        }

        public void StopAllSounds()
        {
            for (int i = currentlyPlayingSounds.Count - 1; i >= 0; i--)
            {
                currentlyPlayingSounds[i].Stop();
                currentlyPlayingSounds.RemoveAt(i);
            }
        }

        /// <summary>
        /// Only do this when kickCooldown is less than 0, and only check every 60 frames or so (kickCooldown % 10 == 0)
        /// </summary>
        public void CheckForKickVictim()
        {
            for (int i = 0; i < game.CurrentChapter.CurrentMap.EnemiesInMap.Count; i++)
            {
                if (game.CurrentChapter.CurrentMap.EnemiesInMap[i] is Goblin && (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).Kicked == false && game.CurrentChapter.CurrentMap.EnemiesInMap[i].VitalRec.Intersects(kickRec))
                {
                    kicking = true;
                    moveFrame = 0;
                    frameDelay = 5;
                }
            }

            if (player.CheckIfHit(kickRec))
            {
                kicking = true;
                moveFrame = 0;
                frameDelay = 5;
            }
        }

        //Clears the ground hole list. For when you leave a map
        public void ClearGroundHoles()
        {
            groundHoles.Clear();
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            //If the troll is dead, make him fall over
            if (health <= 0 && !deadFalling)
            {
                deadFalling = true;
                canBeHit = false;

                moveFrame = 0;
                frameDelay = 5;
                pickingUpClub = false;
                falling = true;
                spitting = false;
                kicking = false;
                enemyState = EnemyState.standing;
                StopAttack();
            }

            for (int i = 0; i < currentlyPlayingSounds.Count; i++)
            {
                if (currentlyPlayingSounds[i].State == SoundState.Stopped)
                {
                    currentlyPlayingSounds.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            //Make the lift poof away when the troll falls and gets back up
            if (liftPoofing)
            {
                liftPoofTimer--;

                if (liftPoofTimer == 0)
                {
                    liftPoofTimer = 5;
                    liftPoofFrame++;

                    if (liftPoofFrame == 5)
                    {
                        liftPoofing = false;
                    }
                }
            }

            //Update the fart
            if (currentFart != null)
            {
                currentFart.Update();

                if (currentFart.finished)
                {
                    currentFart = null;
                }
            }



            #region Do damage quickly to knock him down
            if (fiveSecondTimer > 0)
                fiveSecondTimer--;
            if (fiveSecondTimer == 0)
                damageTakenInFiveSeconds = 0;
            #endregion

            if (!respawning)
            {
                if (hostile)
                {
                    attackCooldown--;
                    kickCooldown--;
                    spitCooldown--;
                }

                if (deadFalling)
                {
                    Fall();
                }
                else if (!falling && !pickingUpClub && !spitting && !kicking)
                {
                    if(fallCooldown > 0)
                        fallCooldown--;
                    Move(mapwidth);
                }
                else if (pickingUpClub)
                {
                    PickUpClub();
                }
                else if (falling)
                {
                    Fall();
                }
                else if (kicking)
                {
                    Kick();
                }
                else if (spitting)
                {
                    Spit();
                }

                for (int i = 0; i < groundHoles.Count; i++)
                {
                    groundHoles[i].Update();

                    if (groundHoles[i].finished)
                    {
                        groundHoles.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

            #region Update rectangles
            //If he isn't falling the vital recs stay on him
            if (!falling)
            {
                if (!facingRight)
                {
                    vitalRec.X = rec.X + 320;
                    kickRec.X = rec.X + 300;
                    kickRec.Y = rec.Y + 418;
                }
                else
                {
                    vitalRec.X = rec.X + 350;
                    kickRec.X = rec.X + 513;
                    kickRec.Y = rec.Y + 418;
                }

                vitalRec.Y = rec.Y + 150;
            }
                //If he is falling, but getting back up
            else if (falling && moveFrame > 13)
            {
                if (!facingRight)
                    vitalRec.X = rec.X + 400;
                else
                    vitalRec.X = rec.X + 230;

                vitalRec.Y = rec.Y + 150;
            }
                //If he is falling, make it sideways on the ground
            else
            {
                VitalRecY = Rec.Y + 300;

                if (!facingRight)
                {
                    vitalRec.X = rec.X + 420;

                }
                else
                {
                    vitalRec.X = rec.X + 160;

                }
            }

            deathRec = new Rectangle(0, 0, 200, 200);

            deathRec.Y = rec.Y + 280;
            if (!facingRight)
                deathRec.X = rec.X + 500;
            else
                deathRec.X = rec.X + 240;
            #endregion



            //Shake the screen based on how close the troll is to you
            stepShakeMag = 2000 / Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));
            if (stepShakeMag < 1)
                stepShakeMag = 0f;
            if (stepShakeMag > 4)
                stepShakeMag = 4f;

            stepShakeMag *= 2;

            //Shake camera when he steps
            if (enemyState == EnemyState.moving && (moveFrame == 2 || moveFrame == 9))
            {
                Game1.camera.ShakeCamera(3, stepShakeMag);

                int walkSound = moveNum.Next(1,4);

                if (frameDelay == 5)
                {
                    SoundEffectInstance step = trollSounds["enemy_troll_walk_0" + walkSound].CreateInstance();
                    float vol = 500 / Vector2.Distance(new Vector2(rec.Center.X, rec.Center.Y), new Vector2(player.Rec.Center.X, player.Rec.Center.Y));

                    if (vol > 1)
                        vol = 1;
                    step.Volume = vol;

                    currentlyPlayingSounds.Add(step);
                    currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();
                }
            }

        }

        //--If it is dead, return true
        public override bool IsDead()
        {
            if (health <= 0 && deadFalling && moveFrame >= 8)
            {
                for (int i = 0; i < player.EquippedSkills.Count; i++)
                {
                    if (level >= player.Level - 5)
                    {
                        if (player.EquippedSkills[i].SkillRank < 4 && player.Level >= player.EquippedSkills[i].PlayerLevelsRequiredToLevel[player.EquippedSkills[i].SkillRank - 1])
                            player.EquippedSkills[i].Experience += experienceGiven;
                    }
                }

                player.AddExpNums(experienceGiven, rec, vitalRec.Y);
                player.Experience += experienceGiven;
                DropItem();
                DropHealth();
                DropMoney();
                Chapter.effectsManager.AddSmokePoof(deathRec, 1);

                trollChest.RecY = rec.Y + 340;
                if (!facingRight)
                    trollChest.RecX = rec.X + 500;
                else
                    trollChest.RecX = rec.X + 240;
                return true;
            }

            return false;
        }

        public void Fall()
        {
            if (moveFrame == 0 && frameDelay == 5)
            {
                StopAllSounds();
                currentlyPlayingSounds.Add(trollSounds["enemy_troll_fall"].CreateInstance());
                currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();
            }

            frameDelay--;
            if (frameDelay <= 0)
            {
                moveFrame++;

                frameDelay = 5;

                //Slow it down after he falls
                /*
                if (moveFrame > 5)
                    frameDelay = 7;*/

                if (moveFrame == 10)
                {
                    currentlyPlayingSounds.Add(trollSounds["enemy_troll_getup"].CreateInstance());               
                    currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();

                }
                //If he is getting up, change the vital rec size
                if (moveFrame == 13)
                {
                    VitalRecHeight = 350;
                    VitalRecWidth = 250;
                }

                //Shake the camera as he hits the ground
                if (moveFrame == 6)
                    Game1.camera.ShakeCamera(15, 15);

                //If he landed, start the smoke poof and keep him down for a bit
                if (moveFrame == 8)
                {
                    frameDelay = 120;
                    fallSmokeFrame = 8;
                    fallSmokeTimer = 5;
                }
            }

            #region SMoke animation
            if (fallSmokeFrame != 0)
                fallSmokeTimer--;

            if (fallSmokeTimer == 0 && fallSmokeFrame != 0)
            {
                fallSmokeFrame++;
                fallSmokeTimer = 5;
            }

            if (fallSmokeFrame > 10)
            {
                fallSmokeFrame = 0;
                fallSmokeTimer = 0;
            }
            #endregion

            //Get back up
            if (moveFrame > 22)
            {
                moveFrame = 0;
                falling = false;
                damageTakenInFiveSeconds = 0;
                fallCooldown = 1000;
                fiveSecondTimer = 0;
                frameDelay = 5;

                if(facingRight)
                    liftDisappearRec = new Rectangle(rec.X + 189, rec.Y + 100, 369, 432);
                else
                    liftDisappearRec = new Rectangle(rec.X + 383, rec.Y + 100, 369, 432);

                liftPoofFrame = 0;
                liftPoofTimer = 5;
                liftPoofing = true;
            }

            currentlyInMoveState = true;
        }

        public void PickUpClub()
        {
             frameDelay--;

             if (frameDelay == 0)
             {
                 moveFrame++;

                 frameDelay = 5;
             }

             if (moveFrame > 9)
             {
                 pickingUpClub = false;
                 moveFrame = 0;
                 frameDelay = 10;
                 attackCooldown = 150;
             }
        }

        public override void Move(int mapWidth)
        {
            base.Move(mapWidth);

            //--Calculate the distance from the player
            float distanceFromPlayer = Vector2.Distance(new Vector2(player.VitalRec.Center.X,player.VitalRec.Center.Y) , new Vector2(vitalRec.Center.X, vitalRec.Center.Y));

            //--If the enemy hasn't been hit, walk randomly. If he is hostile but far away from the player, ignore him
            if (hostile == false || verticalDistanceToPlayer > 900)
            {
                #region Random movement, not hostile
                if (currentlyInMoveState == false)
                {
                    moveState = moveNum.Next(0, 3);
                    moveTimer = moveTime.Next(300, 500);
                }

                switch (moveState)
                {
                    case 0:
                        enemyState = EnemyState.standing;
                        moveTimer--;
                        currentlyInMoveState = true;
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
                            if (moveFrame < 2 || (moveFrame > 4 && moveFrame < 9) || moveFrame > 11)
                                frameDelay = 8;
                            else
                                frameDelay = 10;
                        }

                        if (moveFrame > 13)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X <= mapWidth - 6 && moveFrame != 2 && moveFrame != 3 && moveFrame != 4 && moveFrame != 9 && moveFrame != 10 && moveFrame != 11 && moveFrame != 12)
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
                            if (moveFrame < 2 || (moveFrame > 4 && moveFrame < 9) || moveFrame > 11)
                                frameDelay = 8;
                            else
                                frameDelay = 10;
                        }

                        if (moveFrame > 13)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        moveTimer--;
                        if (position.X >= 6 && moveFrame != 2 && moveFrame != 3 && moveFrame != 4 && moveFrame != 9 && moveFrame != 10 && moveFrame != 11 && moveFrame != 12)
                            position.X -= enemySpeed;
                        break;
                }


                if (moveTimer <= 0)
                    currentlyInMoveState = false;
                #endregion
            }
            //--If it is hostile
            else if(hostile)
            {
                #region If the player is too far away, move closer
                if((attackCooldown > 0 && horizontalDistanceToPlayer <= 200) && enemyState != EnemyState.attacking)
                {
                    if (horizontalDistanceToPlayer > 100)
                    {
                        if (player.VitalRec.Center.X < vitalRec.Center.X)
                            facingRight = false;
                        else
                            facingRight = true;
                    }

                    moveFrame = 0;
                    enemyState = EnemyState.standing;

                    //Spit if the cooldown is up
                    if (spitCooldown <= 0 && goblinSpitAmmo > 0)
                    {
                        spitting = true;
                        moveFrame = 0;
                        frameDelay = 5;
                    }

                    //Check for a kick victim every second if the cooldown is up
                    if (kickCooldown <= 0 && (Math.Abs(kickCooldown) % 3 == 0))
                    {
                        CheckForKickVictim();
                    }
                }

                //--If the player is too far, or the enemy cannot attack yet. Can't be in the middle of a 
                //--knockback or attacking
                else if ((distanceFromPlayer > 400 || (attackCooldown > 0 && distanceFromPlayer > 200)) && enemyState != EnemyState.attacking)
                {

                    //Spit if the cooldown is up
                    if (spitCooldown <= 0 && goblinSpitAmmo > 0)
                    {
                        spitting = true;
                        moveFrame = 0;
                        frameDelay = 5;
                    }

                    //Check for a kick victim every second if the cooldown is up
                    if (kickCooldown <= 0 && (Math.Abs(kickCooldown) % 3 == 0))
                    {
                        CheckForKickVictim();
                    }

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

                            if (moveFrame < 2 || (moveFrame > 4 && moveFrame < 9) || moveFrame > 11)
                                frameDelay = 8;
                            else
                                frameDelay = 10;
                        }

                        if (moveFrame > 13)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        if (position.X >= 6 && moveFrame != 2 && moveFrame != 3 && moveFrame != 4 && moveFrame != 9 && moveFrame != 10 && moveFrame != 11 && moveFrame != 12)
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

                            if (moveFrame < 2 || (moveFrame > 4 && moveFrame < 9) || moveFrame > 11)
                                frameDelay = 8;
                            else
                                frameDelay = 10;
                        }

                        if (moveFrame > 13)
                            moveFrame = 0;

                        currentlyInMoveState = true;
                        if (position.X <= mapWidth - 6 && moveFrame != 2 && moveFrame != 3 && moveFrame != 4 && moveFrame != 9 && moveFrame != 10 && moveFrame != 11 && moveFrame != 12)
                            position.X += enemySpeed;
                    }
                }
                #endregion

                #region Attack once it is close enough
                else
                {
                    //--Only attack if off cooldown
                    if (attackCooldown <= 0)
                    {
                        Vector2 kb;
                        if (facingRight)
                            kb = new Vector2(20, -5);
                        else
                            kb = new Vector2(-20, 5);

                        Attack(clubDamage, kb);
                    }
                }
                #endregion
            }
        }

        //--Attack the player. Cycles through animation, and creates the attack box
        public override void Attack(int damage, Vector2 kb)
        {
            base.Attack(damage, kb);

            //--First frame of attack, set the attack rec up and reset the frames
            if (enemyState != EnemyState.attacking)
            {
                attackFrame = 0;
                frameDelay = 5;

                //--Face the player if it isn't already. 
                //--This is necessary if the player attacks the enemy from close range, otherwise the enemy might be facing
                //--The wrong way and autoattack in the wrong direction
                if (player.VitalRec.Center.X < vitalRec.Center.X)
                    facingRight = false;
                else
                    facingRight = true;

                if (facingRight)
                {
                    overAttackRec = new Rectangle(vitalRec.X, vitalRec.Y - 50, 400, 150);
                    attackRec = new Rectangle(vitalRec.X + vitalRec.Width, vitalRec.Y, 280, vitalRec.Height);
                }
                else
                {
                    overAttackRec = new Rectangle(vitalRec.X, vitalRec.Y - 50, 400, 150);
                    attackRec = new Rectangle(vitalRec.X - 280, vitalRec.Y, 280, vitalRec.Height);
                }

                RangedAttackRecs.Add(attackRec);

                //if (moveFrame == 0 && frameDelay == 5)
                currentlyPlayingSounds.Add(trollSounds["enemy_troll_attack"].CreateInstance());
                currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();
            }
            enemyState = EnemyState.attacking;


            //--Go through the animation
            frameDelay--;
            if (frameDelay == 0)
            {
                attackFrame++;
                frameDelay = 6;

                if (attackFrame == 19)
                {
                    int ran = moveTime.Next(0, 3);

                    if (ran == 1)
                    {
                        currentlyPlayingSounds.Add(trollSounds["enemy_troll_fart"].CreateInstance());
                        currentlyPlayingSounds[currentlyPlayingSounds.Count - 1].Play();
                        
                        FartCloud fart = new FartCloud(facingRight, rec, clubGoneSprite);
                        currentFart = fart;
                    }
                }

            }

            if (attackFrame == 18 && frameDelay == 3)
            {
                Game1.camera.ShakeCamera(10, 20);
                groundHoles.Add(new GroundHole(facingRight, rec, attackSprite));
            }

            //--If the player gets hit in the middle of the animation, do damage and knockback
            if ((attackFrame == 18  && frameDelay >= 3 && player.IsStunned == false && player.Ducking == false) || attackFrame == 19)
            {
                if (player.CheckIfHit(attackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 5;
                    player.HitPauseTimer = 5;
                    game.Camera.ShakeCamera(3, 3);
                    MyGamePad.SetRumble(10, 1f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, player.VitalRec));
                }
            }
            else if (attackFrame == 18)
            {
                if (player.CheckIfHit(overAttackRec) && player.InvincibleTime <= 0)
                {
                    player.TakeDamage(damage);
                    player.KnockPlayerBack(kb);
                    hitPauseTimer = 5;
                    player.HitPauseTimer = 5;
                    game.Camera.ShakeCamera(3, 3);
                    MyGamePad.SetRumble(10, 1f);

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(overAttackRec, player.VitalRec));
                }
            }

            //--Once it has ended, reset
            if (attackFrame > 22)
            {
                attackFrame = 0;
                moveFrame = 0;
                frameDelay = 5;
                attackCooldown = 150;
                clubDisappearRec = rec;
                enemyState = EnemyState.standing;
                currentlyInMoveState = false;
                pickingUpClub = true;
                attackRec = new Rectangle(0, 0, 0, 0);
            }

            currentlyInMoveState = true;
        }

        public override void StopAttack()
        {
            base.StopAttack();

            attackFrame = 0;
            attackCooldown = 120;
            enemyState = EnemyState.standing;
            currentlyInMoveState = false;
            attackRec = new Rectangle(0, 0, 0, 0);
        }

        public override void CheckWalkCollisions(int damage, Vector2 knockback)
        {
            base.CheckWalkCollisions(damage, knockback);
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {

            if (respawning == false)
            {
                if (knockedBack && VelocityY > 0)
                    velocity.Y = 0;

                #region Strength and weakness modifiers
                //Increase damage if the skill type is equal to the enemy's weakness
                if (skillType == veryEffective || meleeOrRanged == veryEffectiveRangedMelee)
                {
                    damage = (int)(damage * veryEffectiveMultiplier);
                    weaknessStrengthOrNormal.Add("Weakness");
                }
                //Opposite if it's the enemy's strength
                else if (skillType == notEffective || meleeOrRanged == notEffectiveRangedMelee)
                {
                    damage = (int)(damage * notEffectiveMultiplier);
                    weaknessStrengthOrNormal.Add("Strength");
                }
                else
                {
                    weaknessStrengthOrNormal.Add("Normal");
                }
                #endregion

                damage -= tolerance;

                if (damage <= 0)
                    damage = 1;

                health -= damage;

                //DAMAGE TO FALL OVER
                if (fallCooldown == 0 && !falling)
                {
                    damageTakenInFiveSeconds += damage;

                    if (fiveSecondTimer == 0)
                    {
                        fiveSecondTimer = 300;
                    }

                    if (damageTakenInFiveSeconds >= damageToFall)
                    {
                        moveFrame = 0;
                        frameDelay = 5;
                        falling = true;
                        pickingUpClub = false;
                        kicking = false;
                        spitting = false;
                        VitalRecHeight = 150;
                        VitalRecWidth = 350;

                        enemyState = EnemyState.standing;
                        StopAttack();
                    }
                }

                //--If this is the first time being hit, make him hostile
                if (hostile == false)
                {
                    //--Don't allow it to attack immediately
                    attackCooldown = 120;
                    hostile = true;
                }

                AddDamageNum(damage, collision);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            //s.Draw(Game1.whiteFilter, attackRec, Color.Red * .5f);

            //Draw the lift disappearing
            if (liftPoofing)
            {
                s.Draw(game.EnemySpriteSheets["TrollFall"], liftDisappearRec, new Rectangle((369 * liftPoofFrame) + 1880, 3330, 369, 432), Color.White);

            }

            for (int i = 0; i < groundHoles.Count; i++)
            {
                groundHoles[i].Draw(s);
            }

            attackSprite = game.EnemySpriteSheets["TrollAttack"];
            fallSprite1 = game.EnemySpriteSheets["TrollFall"];
            clubGoneSprite = game.EnemySpriteSheets["TrollClubGone"];

            #region Draw Enemy
            if (facingRight == false)
            {
                if (pickingUpClub)
                {
                    s.Draw(game.EnemySpriteSheets["TrollClubGone"], rec, GetClubDisappearSource(), Color.White);
                }

                if (spitting || kicking)
                {
                    s.Draw(game.EnemySpriteSheets["TrollClubGone"], rec, GetSourceRectangle(moveFrame), Color.White);
                }
                else if (falling)
                {
                        s.Draw(game.EnemySpriteSheets["TrollFall"], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

                    switch (fallSmokeFrame)
                    {
                        case 8:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(2880, 2775, 940, 555), Color.White * alpha);
                            break;
                        case 9:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(0, 3330, 940, 555), Color.White * alpha);
                            break;
                        case 10:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(940, 3330, 940, 555), Color.White * alpha);
                            break;
                    }
                }
                else if (enemyState == EnemyState.attacking)
                    s.Draw(game.EnemySpriteSheets["TrollAttack"], rec, GetSourceRectangle(attackFrame), Color.White * alpha);
                else
                {
                        s.Draw(game.EnemySpriteSheets["Field Troll"], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                }
            }

            if (facingRight == true)
            {
                if (pickingUpClub)
                {
                        s.Draw(game.EnemySpriteSheets["TrollClubGone"], rec, GetClubDisappearSource(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                }

                if (spitting || kicking)
                {
                    s.Draw(game.EnemySpriteSheets["TrollClubGone"], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
                else if (falling)
                {
                        s.Draw(game.EnemySpriteSheets["TrollFall"], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);


                    switch (fallSmokeFrame)
                    {
                        case 8:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(2880, 2775, 940, 555), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;
                        case 9:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(0, 3330, 940, 555), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;
                        case 10:
                            s.Draw(game.EnemySpriteSheets["TrollFall"], rec, new Rectangle(940, 3330, 940, 555), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                            break;
                    }
                }
                else if (enemyState == EnemyState.attacking)
                    s.Draw(game.EnemySpriteSheets["TrollAttack"], rec, GetSourceRectangle(attackFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                else
                {
                        s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                }
            }
            #endregion

            if (currentFart != null)
                currentFart.Draw(s);

            /*
            if (enemyState == EnemyState.attacking)
            {
                s.Draw(Game1.whiteFilter, attackRec, Color.Red * .5f);
                s.Draw(Game1.whiteFilter, overAttackRec, Color.Red * .5f);
            }*/

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = VitalRecY - 32;
                s.Draw(healthBack, healthBoxRec, Color.White);

                healthBarRec.Y = VitalRecY - 30;

                if (health > (maxHealth / 2))
                {
                    greenColor = 1;
                    redColor = (1f - ((float)health / (float)maxHealth));
                }
                else
                {
                    redColor = 1;
                    greenColor = (((float)health / ((float)maxHealth / 2f)));
                }

                s.Draw(healthFore, healthBarRec, new Color(redColor, greenColor, 0));
                s.Draw(healthFore, healthBarRec, Color.Gray * .4f);

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + "  " + displayName).X;

                s.DrawString(Game1.descriptionFont, "Lv. " + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2 - 2, rec.Y - 35 - 2), Color.Black);
                s.DrawString(Game1.descriptionFont, "Lv. " + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2, rec.Y - 35), Color.White);
            }
            #endregion
        }

        public override void DrawForegroundEffect(SpriteBatch s)
        {
            base.DrawForegroundEffect(s);

            if (currentFart != null)
                currentFart.DrawFore(s);
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
            else
                velocity.Y += GameConstants.GRAVITY;

            position += velocity;

            Rectangle feet = new Rectangle((int)position.X, (int)position.Y + rec.Height - 40, rec.Width, 20);
            Rectangle topEn = new Rectangle((int)vitalRec.X + 5, (int)vitalRec.Y, vitalRec.Width - 5, 10);
            Rectangle rightEn = new Rectangle((int)vitalRec.X + vitalRec.Width, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);
            Rectangle leftEn = new Rectangle((int)vitalRec.X - 15, (int)vitalRec.Y + 5, 15, vitalRec.Height + 35);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle top = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 5);
                Rectangle bottom = new Rectangle(plat.Rec.X + 5, plat.Rec.Y + plat.Rec.Height - 10, plat.Rec.Width - 5, 10);

                Rectangle left = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle right = new Rectangle(plat.Rec.X + plat.Rec.Width - 10, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);



                //DOn't move through non passable platforms
                if ((rightEn.Intersects(left) || leftEn.Intersects(right)) && plat.Passable == false)
                {
                    if (rightEn.Intersects(left))
                    {
                        position.X -= enemySpeed;
                        velocity.X = 0;
                    }

                    if (leftEn.Intersects(right))
                    {
                        position.X += enemySpeed;
                        velocity.X = 0;
                    }
                }


                //--If you jump up into a nonpassable wall, push him back down

                #region Landing on a platform
                //--REPLACE THIS WITH COLLIDES WITH GROUND CHECK
                if (feet.Intersects(top) && velocity.Y > 0)
                {
                    //Set the platform it's currently on to currentPlat
                    currentPlat = plat;

                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + 40;
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

        public override void DropItem()
        {
            //Drop bio if you don't have it yet
           // if (!player.AllMonsterBios[name])
           // {
                    //currentMap.Drops.Add(new EnemyBio("This String Doesn't Matter for Bios", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter), name));
           // }

        }


    }
}
