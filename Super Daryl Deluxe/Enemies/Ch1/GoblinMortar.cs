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
    public class GoblinMortar : Enemy
    {

        float flinchAlpha = 0f;

        public GoblinMortar(Vector2 pos, String type, Game1 g, ref Player play, MapClass cur)
            : base(pos, type, g, ref play, cur)
        {
            health = 705;
            maxHealth = 705;
            level = 10;
            experienceGiven = 150;
            rec = new Rectangle((int)position.X, (int)position.Y, 435, 723);
            currentlyInMoveState = false;
            enemySpeed = 0;
            tolerance = 65;
            vitalRec = new Rectangle(rec.X, rec.Y, 186, 300);
            maxHealthDrop = 10;
            moneyToDrop = 4.35f;

            maxAttackCooldown = 80;

            canBeStunned = false;
            canBeKnockbacked = false;
        }

        //--Return the source rectangle for the enemy move frames
        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(435 * moveFrame, 0, 435, 723);
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            if (flinchAlpha > 0)
                flinchAlpha -= .04f;

            if (enemyState == EnemyState.standing)
            {
                attackCooldown--;

                if (attackCooldown <= 0)
                    enemyState = EnemyState.attacking;
            }
            else if (enemyState == EnemyState.attacking)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    frameDelay = 5;
                    moveFrame++;

                    if (moveFrame > 8)
                    {
                        enemyState = EnemyState.standing;
                        attackCooldown = Game1.randomNumberGen.Next(maxAttackCooldown - 20, maxAttackCooldown + 60);
                        moveFrame = 0;
                    }
                }

            }
            vitalRec.Y = rec.Y + 340;

            if (!facingRight)
                vitalRec.X = rec.X + 133;
            else
                vitalRec.X = rec.X + 78;

            deathRec = vitalRec;
        }

        public override void TakeHit(int damage, Vector2 kbvel, Rectangle collision, AttackType.AttackTypes skillType, AttackType.RangedOrMelee meleeOrRanged)
        {
            base.TakeHit(damage, kbvel, collision, skillType, meleeOrRanged);

            flinchAlpha = 1f;
        }

        public override void PlaySoundWhenHit()
        {
            //int soundType = moveNum.Next(2);

            //if (soundType == 0)
            //    Sound.enemySoundEffects["BennyHit1"].CreateInstance().Play();
            //else
            //    Sound.enemySoundEffects["BennyHit2"].CreateInstance().Play();
        }

        public override void Draw(SpriteBatch s)
        {
            #region Draw Enemy
            if (facingRight == true)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);

            if (facingRight == false)
                s.Draw(game.EnemySpriteSheets[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            #endregion

            #region Health Bar
            if (health < maxHealth)
            {
                healthBoxRec.Y = vitalRec.Y - 35;
                healthBarRec.Y = vitalRec.Y - 33;

                s.Draw(healthBack, healthBoxRec, Color.White);

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

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + " " + displayName).X;

                if (facingRight)
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 - 30), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
                else
                {
                    Game1.OutlineFont(Game1.font, s, "Lv. " + level + " " + displayName, 1, (int)(rec.X + rec.Width / 2 - measX / 2 + 10), healthBoxRec.Y - 25 - 2, Color.Black, Color.White);
                }
            }
            #endregion

         //   s.Draw(Game1.whiteFilter, vitalRec, Color.Black * .5f);
            if (flinchAlpha > 0)
            {
                if(facingRight)
                    s.Draw(game.EnemySpriteSheets[name], rec, new Rectangle(0, 723, 435, 723), Color.White * flinchAlpha);
                else
                    s.Draw(game.EnemySpriteSheets[name], rec, new Rectangle(0, 723, 435, 723), Color.White * flinchAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }
        }

        public override void DropItem()
        {
            base.DropItem();

            int dropType = moveNum.Next(0, 101);

            if (dropType < 25)
            {
                //currentMap.Drops.Add(new EnemyDrop("Cymbal Polish", new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
            else if (dropType < 35)
            {
                //currentMap.Drops.Add(new EnemyDrop(new Marker(), new Rectangle(rec.Center.X, rec.Center.Y, dropDiameter, dropDiameter)));
            }
        }

    }
}
