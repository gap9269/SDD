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
    public class PointedJabs : Skill
    {
        // ATTRIBUTES \\
        Rectangle stab;
        Rectangle secondStab;
        Rectangle thirdStab;
        int phase;
        int timer;
        int defaultTimer = 40;
        int lastCooldown = 80;

        // CONSTRUCTOR \\
        public PointedJabs(Texture2D animSheet, Player play, Texture2D ico)
            : base(animSheet, play, ico, false)
        {

            //--Base Stats
            damage = .4f;
            experience = 0;
            experienceUntilLevel = 100;
            skillRank = 1;
            name = "Pointed Jabs";
            description = "Daryl stabs the area in front of him up to three times quickly, \nslowly increasing the damage.";
            fullCooldown = 80;
            //--Animation and skill attributes
            animationLength = 0;
            phase = 0;
            timer = -1;
            canUse = true;
        }

        //--This is the main code for what the skill does
        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);


            #region 3 Hit Combo


            //--First press. Sets the cooldown for the skill to 150.
            //--Timer is how long the player has to press the key again, else the skill resets
            if (justPressed == false)
            {


                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();


                justPressed = true;

                if (phase == 0)
                {
                    //currentCooldown = 150;
                    animationLength = 10;
                    timer = defaultTimer;
                    phase = 1;
                    hitPauseTime = 2;
                }

                //--Second press, cooldown is set back to 100, and restarts the timer
                if (phase == 1 && timer <= defaultTimer - 1)
                {
                    //currentCooldown = 100;
                    animationLength = 10;
                    timer = defaultTimer;
                    phase = 2;
                    hitPauseTime = 2;
                }

                //--On the third hit, give the skill experience
                if (phase == 2 && timer <= defaultTimer - 1)
                {
                    fullCooldown = lastCooldown;
                    currentCooldown = fullCooldown;
                    animationLength = 12;
                    timer = 45;
                    phase = 3;
                    hitPauseTime = 4;

                    //Re-enable this to allow the player to change direction during his final swing
                    //canUse = false;
                }
            #endregion
            }
        }

        public override Rectangle GetSourceRec()
        {
            switch (phase)
            {
                case 1:
                    return new Rectangle(562 * moveFrame, 0, 562, 360);
                case 2:
                    return new Rectangle(562 * moveFrame, 360, 562, 360);
                case 3:
                    return new Rectangle(562 * moveFrame, 720, 562, 360);
            }

            return new Rectangle();
        }

        public void updateMoveFrameAndCheckCollisions()
        {
            switch (phase)
            {
                case 1:
                    if (animationLength > 8)
                        moveFrame = 0;
                    else if (animationLength > 5)
                        moveFrame = 1;
                    else
                        moveFrame = 2;

                    if (animationLength > 4 && animationLength < 9)
                        CheckFiniteCollisions(stab, damage, new Vector2(10, -5), 3, 3);

                    if (animationLength == 0 && useNext[0] == true)
                    {
                        animationLength = 10;
                        timer = defaultTimer;
                        phase = 2;
                        hitPauseTime = 2;
                        hitThisTime = 0;
                        enemiesHitThisAttack.Clear();
                        bossesHitThisAttack.Clear();
                    }

                    break;
                case 2:
                    if (animationLength > 10)
                        moveFrame = 0;
                    else if (animationLength > 8)
                        moveFrame = 1;
                    else
                        moveFrame = 2;

                    if (animationLength > 4 && animationLength < 9)
                        CheckFiniteCollisions(secondStab, damage * .05f, new Vector2(10, -5), 3, 3);

                    if (animationLength == 0 && useNext[1] == true)
                    {
                        fullCooldown = lastCooldown;
                        currentCooldown = fullCooldown;
                        animationLength = 12;
                        timer = defaultTimer;
                        phase = 3;
                        hitPauseTime = 4;
                        hitThisTime = 0;
                        enemiesHitThisAttack.Clear();
                        bossesHitThisAttack.Clear();
                    }
                    break;
                case 3:
                    if (animationLength > 18)
                        moveFrame = 0;
                    else if (animationLength > 14)
                        moveFrame = 1;
                    else if (animationLength > 10)
                        moveFrame = 2;
                    else
                        moveFrame = 3;

                    if (animationLength > 6 && animationLength < 12)
                        CheckFiniteCollisions(thirdStab, damage + .1f, new Vector2(15, -5), 4, 4);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            timer--;
            updateMoveFrameAndCheckCollisions();

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            //--This stops the player from being able to change direction during his final swing
            //--Essentially, it makes it so you can't use it AFTER the animation is over.
            //--Since you can move when "canUse" is false, you must wait until after or you can change direction during it
            if (phase == 3 && animationLength < 0)
                canUse = false;

            if (animationLength < 0)
            {
                justPressed = false;
                useNext[0] = false;
                useNext[1] = false;
            }

            //--If the timer reaches 0, it means the player stopped the combo early, so reset the skill
            if (timer < 0 && currentCooldown <= 0)
            {

                if(phase > 0 && phase != 3)
                    currentCooldown = fullCooldown;
                phase = 0;
            }

            if (currentCooldown > 0)
                canUse = false;

            #region SET RECTANGLES
            //--Sets the rectangles based on which way the character is facing
            if (player.FacingRight == true)
            {
                stab = new Rectangle(player.VitalRecX + player.VitalRecWidth, (int)player.VitalRecY, 250, 150);
                secondStab = new Rectangle(player.VitalRecX + player.VitalRecWidth, (int)player.VitalRecY, 250, 150);
                thirdStab = new Rectangle(player.VitalRecX + player.VitalRecWidth, (int)player.VitalRecY, 250, 150);
            }
            else
            {
                stab = new Rectangle(player.VitalRecX - 250, (int)player.VitalRecY, 250, 150);
                secondStab = new Rectangle(player.VitalRecX - 250, (int)player.VitalRecY, 250, 150);
                thirdStab = new Rectangle(player.VitalRecX - 250, (int)player.VitalRecY, 250, 150);
            }
            #endregion
        }

        //--Draws based on which phase the skill is in, and if the animation is actually playing (animationLength is greater than 0)
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (animationLength > 0)
            {
                switch (phase)
                {
                    case 1:

                        s.Draw(Game1.whiteFilter, stab, Color.White);
                        break;
                    case 2:

                        s.Draw(Game1.whiteFilter, secondStab, Color.Red);
                        break;
                    case 3:

                        s.Draw(Game1.whiteFilter, thirdStab, Color.Blue);
                        break;

                }
            }

        }

        //--If the experience hits what is needed to level up, this is called
        //--Increase stats and other things based on level
        public override void ApplyLevelUp()
        {
            base.ApplyLevelUp();

            switch (skillRank)
            {
                case 2:
                    damage = .5f;
                    experience = 0;
                    experienceUntilLevel = 225;
                    break;
                case 3:
                    damage = .55f;
                    experienceUntilLevel = 500;
                    experience = 0;
                    break;
                case 4:
                    damage = .6f;
                    experience = 0;
                    //name = "Deliberate Dissimilarities";
                    defaultTimer = 70;
                    lastCooldown = 75;
                    break;
            }
        }
    }
}
