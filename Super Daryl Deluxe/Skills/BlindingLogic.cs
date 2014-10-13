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
    public class BlindingLogic : Skill
    {

        Rectangle hitBox;

        public BlindingLogic(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .3f;
            experience = 0;
            experienceUntilLevel = 15;
            skillRank = 1;
            name = "Blinding Logic";
            canUse = true;
            description = "Stuns enemies in an area in front of you";
            fullCooldown = 900;
            //--Animation and skill attributes
            animationLength = 0;
            chargeTime = 0;
            canUseInAir = false;

            costToBuy = 1;
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 30;
                useKey = key;
            }
        }

        public override void Update()
        {
            if (released == true)
            {
                base.Update();

                //--If the animation is completed
                if (animationLength < 0)
                {
                    justPressed = false;
                }
                if (animationLength == 20)
                {
                    //--Check to see if an enemy is getting hit, then stun them
                    CheckCollisions(hitBox, damage, Vector2.Zero, 0, 0);
                    StunEnemy(hitBox, 120);
                }

                //--If it is on cooldown, you cannot use it
                if (currentCooldown > 0)
                {
                    canUse = false;
                }

                #region Set rectangles
                if (player.FacingRight == true)
                {
                    hitBox = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY,
                        chargeTime * 10, 200);
                }
                else
                {
                    hitBox = new Rectangle(player.VitalRecX - chargeTime * 10, player.VitalRecY,
                        chargeTime * 10, 200);
                }
                #endregion
            }

            if (current.IsKeyDown(useKey) && released == false)
            {
                if(chargeTime < 60)
                chargeTime++;
            }
            else if (chargeTime > 0)
                released = true;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                s.Draw(animationSheet, hitBox, Color.Red);
            }
        }
    }
}

