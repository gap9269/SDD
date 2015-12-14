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
    public class LightningDash : Skill
    {

        Rectangle checkPlatRec;
        Rectangle attackRec;
        Rectangle explosionRec;

        public LightningDash(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, false)
        {
            //--Base Stats
            damage = .3f;
            experience = 0;
            experienceUntilLevel = 15;
            skillRank = 1;
            levelToUse = 1;
            name = "Lightning Dash";
            canUse = true;
            description = "Daryl turns into an orb of electricity and\nzaps forward a set distance, damaging all in\nhis path.";
            fullCooldown = 200;
            //--Animation and skill attributes
            animationLength = 0;
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false && canUse)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 25;
                useKey = key;
                player.Invincible(10);
            }
        }

        public override void Update()
        {
            base.Update();

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            if (animationLength > 0)
            {
                player.InvincibleTime = 10;
                if (player.FacingRight)
                {
                    player.VelocityX = 35;
                }
                else
                    player.VelocityX = -35;
            }

            if (animationLength > 0 && current.IsKeyUp(useKey) && last.IsKeyDown(useKey))
            {
                player.VelocityX = 0;
                justPressed = false;
                animationLength = -1;
                canUse = false;

                player.InvincibleTime = 15;
                CheckCollisions(explosionRec, damage, new Vector2(25, -5), 3, 3);
            }

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                player.VelocityX = 0;
            }

            //--Check to see if an enemy is getting hit, then stun them
            if (animationLength % 4 == 0)
            {
                CheckCollisions(attackRec, damage, new Vector2(2, -2), 1, 1);
            }
            if (animationLength == 1)
            {
                player.InvincibleTime = 15;
                CheckCollisions(explosionRec, damage, new Vector2(25, -5), 3, 3);
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;
            }

            #region Set rectangles

            explosionRec = player.Rec;

            if (player.FacingRight == true)
            {
                attackRec = new Rectangle(player.VitalRecX + 20, player.VitalRecY + 30,
                    player.VitalRecWidth - 20, player.VitalRecHeight - 30);

                checkPlatRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY, 100,
                    player.VitalRecHeight - 40);
            }
            else
            {
                attackRec = new Rectangle(player.VitalRecX + 20, player.VitalRecY + 30,
                    player.VitalRecWidth - 20, player.VitalRecHeight - 30);

                checkPlatRec = new Rectangle(player.VitalRecX - 100, player.VitalRecY, 100,
                    player.VitalRecHeight - 40);
            }
            #endregion

            if (animationLength > 0)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (checkPlatRec.Intersects(platforms[i].Rec))
                    {
                        player.VelocityX = 0;
                        animationLength = 0;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                s.Draw(Game1.whiteFilter, attackRec, Color.Red);
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .4f;
                    experience = 0;
                    experienceUntilLevel = 200;
                    break;
                case 3:
                    damage = .4f;
                    experienceUntilLevel = 500;
                    experience = 0;
                    break;
                case 4:
                    damage = .4f;
                    experience = 0;
                    //name = "Lightning pun";
                    break;
            }
        }
    }
}
