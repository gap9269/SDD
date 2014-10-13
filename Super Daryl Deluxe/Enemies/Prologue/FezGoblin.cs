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
    public class FezGoblin : Enemy
    {
        public FezGoblin(Vector2 pos, String name, Game1 g, ref Player play, MapClass cur)
            : base(pos, name, g, ref play, cur)
        {
            health = 3;
            maxHealth = 3;
            level = 1;
            experienceGiven = 1;
            rec = new Rectangle((int)position.X, (int)position.Y, 157, 135);
            vitalRec = new Rectangle(rec.X, rec.Y, rec.Width, rec.Height);
            currentlyInMoveState = false;
            enemySpeed = 0;
            tolerance = 100;
            maxHealthDrop = 0;
            canBeKnockbacked = false;
        }

        public override void Update(int mapwidth)
        {
            base.Update(mapwidth);

            deathRec = vitalRec;

            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;

                if (moveFrame == 3)
                    frameDelay = 30;

                frameDelay = 15;
            }

            if (moveFrame > 3)
                moveFrame = 0;
        }

        public override void Draw(SpriteBatch s)
        {

            #region Health Bar
            if (health < maxHealth)
            {
                s.Draw(healthBack, new Rectangle(healthBoxRec.X - 30, healthBoxRec.Y, healthBoxRec.Width, healthBoxRec.Height), Color.White);

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

                s.Draw(healthFore, new Rectangle(healthBarRec.X - 30, healthBarRec.Y, healthBarRec.Width, healthBarRec.Height) , new Color(redColor, greenColor, 0));
                s.Draw(healthFore, new Rectangle(healthBarRec.X - 30, healthBarRec.Y, healthBarRec.Width, healthBarRec.Height), Color.Gray * .4f);

                float measX = Game1.descriptionFont.MeasureString("Lv." + level + "  " + displayName).X;

                s.DrawString(Game1.descriptionFont, "Lv." + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2 - 32, rec.Y - 35 - 2), Color.Black);
                s.DrawString(Game1.descriptionFont, "Lv." + level + " " + displayName, new Vector2(rec.X + rec.Width / 2 - measX / 2 - 30, rec.Y - 35), Color.White);
            }
            #endregion

            s.Draw(game.EnemySpriteSheets[name], rec, new Rectangle(moveFrame * 157, 0, 157, 135), Color.White);
        }
    }
}