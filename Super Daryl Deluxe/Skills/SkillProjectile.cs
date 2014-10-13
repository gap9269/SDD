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
    public class SkillProjectile
    {

        AttackType.AttackTypes skillType;
        AttackType.RangedOrMelee rangedOrMelee;
        float rotation;
        Vector2 position;
        Rectangle rec;
        int timeInAir;
        Vector2 normalizedVelocity;
        Texture2D texture;
        int maxTimeInAir;
        Boolean dead = false;
        float damage;
        Vector2 knockback;
        int cameraShake;
        int hitPauseTime;
        Boolean facingRight;
        int speed;
        int shakeTime;
        List<Enemy> enemies;
        List<InteractiveObject> interactiveObjectsInMap;
        Boss currentBoss;
        Game1 game;
        Boolean piercingShot = false;
        int pierceAmount = 0;

        public Boolean Dead { get { return dead; } set { dead = value; } }
        Rectangle sourceRectangle;

        public SkillProjectile(Texture2D tex, Rectangle r, int time, Vector2 normVel, float rot, float dam, Vector2 kb, int hitPause, int shake, int shakeTim, int spd, Game1 g, Boolean pierce, int pierceAmnt, Rectangle sourceRec, Boolean facingRite, AttackType.AttackTypes type, AttackType.RangedOrMelee meleeOrRanged)
        {
            facingRight = facingRite;
            sourceRectangle = sourceRec;
            texture = tex;
            rec = r;
            position = new Vector2(r.X, r.Y);
            knockback = kb;
            damage = dam;
            damage *= Game1.Player.Strength;
            maxTimeInAir = time;
            normalizedVelocity = normVel;
            speed = spd;
            rotation = rot;
            game = g;
            shakeTime = shakeTim;
            cameraShake = shake;
            pierceAmount = pierceAmnt;
            piercingShot = pierce;
            skillType = type;
            rangedOrMelee = meleeOrRanged;
        }

        public void Update()
        {
            enemies = game.CurrentChapter.CurrentMap.EnemiesInMap;
            interactiveObjectsInMap = game.CurrentChapter.CurrentMap.InteractiveObjects;
            currentBoss = game.CurrentChapter.CurrentBoss;

            timeInAir++;
            position += normalizedVelocity * speed;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            CheckCollisions();

            if (timeInAir >= maxTimeInAir)
            {
                dead = true;
            }
        }

        public virtual void CheckCollisions()
        {
            //--For every enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                //--This is here because the KB vector has to be the same for every monster hit, so you
                //--have to change a different variable and not the original vector. Changing the vector2 for every enemy
                //--hit was making it so every other enemy would fly in the opposite direction
                float kbX = 0;

                //--If the skill's attack hits the enemy vitals
                if (rec.Intersects(enemies[i].VitalRec) && enemies[i].CanBeHit && enemies[i].Respawning == false)
                {
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, cameraShake);
                        MyGamePad.SetRumble(shakeTime, (float)((float)cameraShake / 100f) * 10f);
                    }
                    else
                        MyGamePad.SetRumble(3, .3f);

                    //--Knock them back
                    if (Game1.Player.VitalRec.Center.X < enemies[i].VitalRec.X)
                        kbX = Math.Abs(knockback.X);
                    else if (Game1.Player.VitalRec.Center.X > enemies[i].VitalRec.X)
                    {
                        kbX = -(knockback.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                    }

                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, enemies[i].VitalRec));

                    enemies[i].TakeHit((int)damage, new Vector2(kbX, knockback.Y), Rectangle.Intersect(rec, enemies[i].VitalRec), skillType, rangedOrMelee);
                    enemies[i].HitPauseTimer = hitPauseTime;
                    Game1.Player.HitPauseTimer = hitPauseTime;

                    //Piece through enemies and objects if it has any pierce left. Otherwise, kill the bullet
                    if (piercingShot)
                    {
                        pierceAmount--;

                        if (pierceAmount <= 0)
                            dead = true;
                    }
                    else
                        dead = true;
                }
            }

            for (int i = 0; i < interactiveObjectsInMap.Count; i++)
            {
                //--If the skill's attack hits the enemy vitals
                if (rec.Intersects(interactiveObjectsInMap[i].VitalRec) && interactiveObjectsInMap[i].Finished == false)
                {
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, cameraShake);
                        MyGamePad.SetRumble(shakeTime, (float)((float)cameraShake / 100f) * 10f);
                    }


                    Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, interactiveObjectsInMap[i].VitalRec));

                    interactiveObjectsInMap[i].TakeHit();

                    if (piercingShot)
                    {
                        pierceAmount--;

                        if (pierceAmount <= 0)
                            dead = true;
                    }
                    else
                        dead = true;
                }
            }

            if (game.CurrentChapter.BossFight && rec.Intersects(currentBoss.VitalRec))
            {
                float kbX = 0;

                if (shakeTime > 0)
                {
                    Game1.camera.ShakeCamera(shakeTime, cameraShake);
                    MyGamePad.SetRumble(shakeTime, (float)((float)cameraShake / 100f) * 10f);
                }

                //--Knock them back
                if (Game1.Player.VitalRec.Center.X < currentBoss.Rec.X)
                    kbX = Math.Abs(knockback.X);
                else if (Game1.Player.VitalRec.Center.X > currentBoss.Rec.X)
                {
                    kbX = -(knockback.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                }

                Chapter.effectsManager.AddDamageFX(10, Rectangle.Intersect(rec, currentBoss.VitalRec));

                currentBoss.TakeHit((int)damage, new Vector2(kbX, knockback.Y), Rectangle.Intersect(rec, currentBoss.VitalRec));
                currentBoss.HitPauseTimer = hitPauseTime;
                Game1.Player.HitPauseTimer = hitPauseTime;

                if (piercingShot)
                {
                    pierceAmount--;

                    if (pierceAmount <= 0)
                        dead = true;
                }
                else
                    dead = true;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(texture, rec, sourceRectangle, Color.White, rotation, new Vector2(0, 12), SpriteEffects.None, 0f);
            if (!facingRight)
                s.Draw(texture, rec, sourceRectangle, Color.White, rotation, new Vector2(0, 12), SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
