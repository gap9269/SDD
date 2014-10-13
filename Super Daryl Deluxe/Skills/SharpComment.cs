using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class SharpComment : Skill
    {
        Rectangle attackRec;
        Random critStrike;
        int critChance;

        public SharpComment(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .3f;
            experience = 0;
            experienceUntilLevel = 15;
            skillRank = 1;
            levelToUse = 1;
            name = "Sharp Comment";
            canUse = true;
            description = "Daryl attacks an area in front of him with a sword. \nHas a 15% chance to do double damage.";
            fullCooldown = 75;
            //--Animation and skill attributes
            hitPauseTime = 1;
            critChance = 15;
            critStrike = new Random();

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(8);
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 12;
                useKey = key;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            if (animationLength > 6 && animationLength < 10)
            {
                //--Check to see if an enemy is getting hit on every 5th frame
                CheckFiniteCollisions(attackRec, damage, new Vector2(10, -5), 2, 2);
            }

            #region Set rectangles
            if (player.FacingRight)
            {
                attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 30,
                    225, player.VitalRecHeight + 40);
            }
            else
            {
                attackRec = new Rectangle(player.VitalRecX - 225, player.VitalRecY - 30,
                    225, player.VitalRecHeight + 40);
            }
            #endregion

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }
        }

        public override void CheckFiniteCollisions(Rectangle attackRec, float damage, Vector2 kbvel, int shakeTime, int shakeMag)
        {
            damage *= player.Strength;

            //--Bosses
            if (game.CurrentChapter.BossFight && attackRec.Intersects(currentBoss.VitalRec) && !bossesHitThisAttack.Contains(currentBoss))
            {
                float kbX = 0;

                if (shakeTime > 0)
                    Game1.camera.ShakeCamera(shakeTime, shakeMag);

                //--Knock them back
                if (player.VitalRec.Center.X < currentBoss.Rec.X)
                    kbX = Math.Abs(kbvel.X);
                else if (player.VitalRec.Center.X > currentBoss.Rec.X)
                {
                    kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                }

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, currentBoss.VitalRec));

                int random = critStrike.Next(101);

                if (random < critChance)
                {
                    currentBoss.TakeHit((int)damage * 2, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, currentBoss.VitalRec));

                    currentBoss.HitPauseTimer = hitPauseTime * 2;
                    player.HitPauseTimer = hitPauseTime * 2;
                }
                else
                {
                    currentBoss.TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, currentBoss.VitalRec));

                    currentBoss.HitPauseTimer = hitPauseTime;
                    player.HitPauseTimer = hitPauseTime;
                }
                bossesHitThisAttack.Add(currentBoss);
            }

            //--For every enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                float kbX = 0;

                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(enemies[i].VitalRec) && !enemiesHitThisAttack.Contains(enemies[i]))
                {
                    //--Shake camera
                    if (shakeTime > 0)
                        Game1.camera.ShakeCamera(shakeTime, shakeMag);

                    //--Knock them back
                    if (player.VitalRec.Center.X < enemies[i].VitalRec.X)
                        kbX = Math.Abs(kbvel.X);
                    else if (player.VitalRec.Center.X > enemies[i].VitalRec.X)
                    {
                        kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                    }

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(attackRec, enemies[i].VitalRec));

                    int random = critStrike.Next(101);

                    if (random < critChance)
                    {
                        enemies[i].TakeHit((int)damage * 2, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, enemies[i].VitalRec), skillType, rangedOrMelee);
                        enemies[i].HitPauseTimer = hitPauseTime * 2;
                        player.HitPauseTimer = hitPauseTime * 2;
                    }
                    else
                    {
                        enemies[i].TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, enemies[i].VitalRec), skillType, rangedOrMelee);
                        enemies[i].HitPauseTimer = hitPauseTime;
                        player.HitPauseTimer = hitPauseTime;
                    }

                    hitThisTime++;
                    enemiesHitThisAttack.Add(enemies[i]);
                }

                if (hitThisTime == maxHit && maxHit > 0)
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                //player.Draw(s);
                s.Draw(Game1.whiteFilter, attackRec, Color.Red);
            }
        }

        public override void ApplyLevelUp()
        {
            base.ApplyLevelUp();

            switch (skillRank)
            {
                case 2:
                    damage = .4f;
                    critChance = 20;
                    experience = 0;
                    experienceUntilLevel = 300;
                    break;
                case 3:
                    damage = .45f;
                    critChance = 25;
                    experienceUntilLevel = 700;
                    experience = 0;
                    fullCooldown = 65;
                    break;
                case 4:
                    damage = .55f;
                    critChance = 30;
                    experience = 0;
                    //name = "Lightning pun";
                    break;
            }

            description = "Daryl attacks an area in front of him with a sword. \nHas a " + critChance + "% chance to do double damage.";
        }
    }
}
