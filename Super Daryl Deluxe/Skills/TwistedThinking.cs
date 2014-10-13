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
    public class TwistedThinking : Skill
    {

        //--At max level, make all enemies inside a rec around you seek you, and stun them while it is happening

        Rectangle hitBox;
        int maxCharge;
        float velocityX;

        public TwistedThinking(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .1f;
            experience = 0;
            experienceUntilLevel = 15;
            skillRank = 1;
            levelToUse = 1;
            name = "Twisted Thinking";
            canUse = true;
            description = "Daryl starts spinning slowly, eventually \npicking nup speed and turning into a small \ntornado." +
                "\nHold to use, arrow keys to move during \nthe attack";
            fullCooldown = 200;
            //--Animation and skill attributes
            animationLength = 0;
            chargeTime = 0;
            maxCharge = 130;
            costToBuy = 1;

            skillType = AttackType.AttackTypes.Wind;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            playerLevelRequiredToLevel.Add(2);
            playerLevelRequiredToLevel.Add(2);
            playerLevelRequiredToLevel.Add(4);
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
                player.Invincible(maxCharge + 1);
                player.InvincibleTime = maxCharge + 1;
                
            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();

            //--Currently being used
            if (current.IsKeyDown(useKey) && released == false)
            {
                currentCooldown = fullCooldown;
                animationLength = 300;
                #region Move while spinning (Picks up speed)
                if (chargeTime < 30)
                {
                    if (current.IsKeyDown(Keys.Left))
                    {
                        velocityX += -.2f;
                    }
                    if (current.IsKeyDown(Keys.Right))
                    {
                        velocityX += .2f;
                    }
                }
                else if(chargeTime < 60)
                {
                    if (current.IsKeyDown(Keys.Left))
                    {
                        velocityX += -.4f;
                    }
                    if (current.IsKeyDown(Keys.Right))
                    {
                        velocityX += .4f;
                    }
                }
                else if (chargeTime < 100)
                {
                    if (current.IsKeyDown(Keys.Left))
                    {
                        velocityX += -.5f;
                    }
                    if (current.IsKeyDown(Keys.Right))
                    {
                        velocityX += .5f;
                    }
                }
                else
                {
                    if (current.IsKeyDown(Keys.Left))
                    {
                        velocityX += -.7f;
                    }
                    if (current.IsKeyDown(Keys.Right))
                    {
                        velocityX += .7f;
                    }
                }
                #endregion

                if (velocityX > 13)
                    velocityX = 13;
                if (velocityX < -13)
                    velocityX = -13;

                player.PositionX += velocityX;

                chargeTime++;

                Console.WriteLine(player.playerState);


                if (chargeTime % 15 == 0)
                {
                    //--Check to see if an enemy is getting hit on every 5th frame
                    CheckCollisions(hitBox, damage, new Vector2(0, -6), 1, 1);
                }
                if (chargeTime == maxCharge)
                {
                    CheckCollisions(hitBox, damage, new Vector2(25, -5), 1, 1);
                }

                if (chargeTime >= maxCharge)
                {
                    player.InvincibleTime = 15;
                    released = true;
                    animationLength = 5;
                    currentCooldown = fullCooldown;
                }
                #region Set rectangles
                if (chargeTime < 30)
                {
                    hitBox = new Rectangle(player.VitalRecX - 100, player.VitalRecY,
                        200 + player.VitalRecWidth, player.VitalRecHeight - 50);
                }
                else
                {
                    hitBox = new Rectangle(player.VitalRecX - 200, player.VitalRecY,
                        400 + player.VitalRecWidth, player.VitalRecHeight - 50);
                }
                #endregion

                if (chargeTime > 99)
                {
                    for (int i = 0; i < platforms.Count; i++)
                    {
                        if (hitBox.Intersects(platforms[i].Rec) && !platforms[i].Passable)
                        {
                            if (player.VitalRec.Center.X < platforms[i].Rec.X)
                                player.PositionX -= velocityX * 2;
                            else
                                player.PositionX += velocityX * 2;
                        }
                    }
                }
            }
            else if (chargeTime > 0 && !released)
            {
                player.InvincibleTime = 15;
                released = true;
                animationLength = 5;
                currentCooldown = fullCooldown;
            }

            //--Let go, or ran out of spin
            if (released)
            {
                velocityX = 0;
                //--If the animation is completed
                if (animationLength < 0)
                {
                    justPressed = false;
                }

                //--If it is on cooldown, you cannot use it
                if (currentCooldown > 0)
                {
                    canUse = false;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength > 0)
            {
                if(!released)
                    s.Draw(Game1.whiteFilter, hitBox, Color.Red);
                else
                    s.Draw(animationSheet, player.VitalRec, Color.Red);
            }
        }

        public override void ApplyLevelUp()
        {
            base.ApplyLevelUp();

            switch (skillRank)
            {
                case 2:
                    damage = .1f;
                    experience = 0;
                    experienceUntilLevel = 300;
                    maxCharge = 150;
                    break;
                case 3:
                    damage = .15f;
                    experienceUntilLevel = 700;
                    experience = 0;
                    fullCooldown = 180;
                    break;
                case 4:
                    damage = .2f;
                    experience = 0;
                    //name = "Lightning pun";
                    maxCharge = 170;
                    break;
            }
        }
    }
}

