using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    public class SpinSlash : Skill
    {
        Rectangle rightSide;
        Rectangle leftSide;
        int phase;
        int timer;
        int defaultTimer = 20;
        int lastCooldown = 65;

        int effectFrame1;
        int effectFrame2;
        int effectFrame3;

        int effectTimer1, effectTimer2, effectTimer3;

        Rectangle thirdEffectRec;
        Boolean thirdEffectFacingRight;

        // CONSTRUCTOR \\
        public SpinSlash(Texture2D animSheet, Player play, Texture2D ico)
            : base(animSheet, play, ico, false)
        {

            //--Base Stats
            damage = .4f;
            experience = 0;
            experienceUntilLevel = 500;
            skillRank = 1;
            name = "Sharp Comments";
            description = "Daryl swings a sword around like a maniac in both directions. Tap quickly as rank increases for more swings. # of Swings: 1";
            fullCooldown = 65;
            //--Animation and skill attributes
            animationLength = 0;
            phase = 0;
            timer = -1;
            canUse = true;
            costToBuy = 1;
            levelToUse = 3;
            skillBarColor = new Color(255, 120, 0);

            skillType = AttackType.AttackTypes.Cut;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            playerLevelRequiredToLevel.Add(14);
            playerLevelRequiredToLevel.Add(14);
            playerLevelRequiredToLevel.Add(15);
        }

        public override Rectangle GetSourceRec()
        {
            switch (phase)
            {
                case 1:
                    return new Rectangle(530 * moveFrame, 0, 530, 398);
                case 2:
                    return new Rectangle(530 * moveFrame, 398, 530, 398);
                case 3:
                    return new Rectangle(530 * moveFrame, 796, 530, 398);
            }

            return new Rectangle();
        }

        public Rectangle GetEffectSourceRec(int effectNum)
        {
            switch (effectNum)
            {
                case 1:
                    return new Rectangle(1590 + (530 * effectFrame1), 0, 530, 398);
                case 2:
                    return new Rectangle(1590 + (530 * effectFrame2), 398, 530, 398);
                case 3:
                    return new Rectangle(1590 + (530 * effectFrame3), 796, 530, 398);
            }

            return new Rectangle();
        }

        //--This is the main code for what the skill does
        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            #region 3 Hit Combo


            //--First press. Sets the cooldown for the skill to 150.
            //--Timer is how long the player has to press the key again, else the skill resets
            if (justPressed == false && currentCooldown <= 0)
            {
                justPressed = true;
                moveFrame = 0;
                if (phase == 0)
                {
                    //currentCooldown = 150;
                    animationLength = 12;
                    effectTimer1 = 25;
                    timer = defaultTimer;
                    phase = 1;
                    hitPauseTime = 1;
                    PlayRandomUseSound();
                }

                //--Second press, cooldown is set back to 100, and restarts the timer
                if (phase == 1 && timer <= defaultTimer - 1)
                {
                    //currentCooldown = 100;
                    animationLength = 12;
                    effectTimer2 = 25;
                    timer = defaultTimer;
                    phase = 2;
                    hitPauseTime = 1;
                    hitPauseTime = 1;
                    PlayRandomUseSound();

                }

                //--On the third hit, give the skill experience
                if (phase == 2 && timer <= defaultTimer - 1)
                {
                    currentCooldown = fullCooldown;
                    animationLength = 12;
                    effectTimer3 = 20;
                    thirdEffectRec = player.Rec;
                    thirdEffectFacingRight = player.FacingRight;
                    timer = defaultTimer;
                    phase = 3;
                    hitPauseTime = 1;
                    PlayRandomUseSound();
                }
            #endregion
            }
        }

        public void updateMoveFrameAndCheckCollisions()
        {

            switch (phase)
            {
                case 1:
                    if (animationLength > 5)
                        moveFrame = 0;
                    else if (animationLength > 3)
                        moveFrame = 1;
                    else
                        moveFrame = 2;


                    if (animationLength > 6 && animationLength < 8)
                    {
                        //Depending on which way the player is facing register the first side hit
                        if(player.FacingRight)
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                    }
                    if (animationLength == 8)
                    {
                        //--Reset amount hit for the next rectangle
                        hitThisTime = 0;
                        enemiesHitThisAttack.Clear();
                        bossesHitThisAttack.Clear();
                        interactiveObjectsThisAttack.Clear();
                    }
                    if (animationLength > 8 && animationLength < 10)
                    {
                        if(player.FacingRight)
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                    }


                    if (animationLength == 0 && useNext[0] == true && skillRank >= 2)
                    {
                        animationLength = 12;
                        effectTimer2 = 25;
                        timer = defaultTimer;
                        phase = 2;
                        hitPauseTime = 1;
                        moveFrame = 0;
                        PlayRandomUseSound();
                    }

                    break;
                case 2:
                    if (animationLength > 5)
                        moveFrame = 0;
                    else if (animationLength > 3)
                        moveFrame = 1;
                    else
                        moveFrame = 2;

                    if (animationLength > 6 && animationLength < 8)
                    {
                        if(player.FacingRight)
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                    }
                    if (animationLength == 8)
                    {
                        //--Reset amount hit for the next rectangle
                        hitThisTime = 0;
                        enemiesHitThisAttack.Clear();
                        bossesHitThisAttack.Clear();
                        interactiveObjectsThisAttack.Clear();
                    }
                    if (animationLength > 8 && animationLength < 10)
                    {
                        if(player.FacingRight)
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                    }


                    if (animationLength == 0 && useNext[1] == true && skillRank >= 3)
                    {
                        currentCooldown = fullCooldown;
                        animationLength = 12;
                        effectTimer3 = 20;
                        thirdEffectRec = player.Rec;
                        thirdEffectFacingRight = player.FacingRight;
                        timer = defaultTimer;
                        phase = 3;
                        hitPauseTime = 1;
                        moveFrame = 0;
                        PlayRandomUseSound();
                    }
                    break;
                case 3:


                    if (animationLength > 5)
                        moveFrame = 0;
                    else if (animationLength > 3)
                        moveFrame = 1;
                    else
                        moveFrame = 2;

                    if (animationLength > 6 && animationLength < 8)
                    {
                        if(player.FacingRight)
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                    }
                    if (animationLength == 8)
                    {
                        //--Reset amount hit for the next rectangle
                        hitThisTime = 0;
                        enemiesHitThisAttack.Clear();
                        bossesHitThisAttack.Clear();
                        interactiveObjectsThisAttack.Clear();
                    }
                    if (animationLength > 8 && animationLength < 10)
                    {
                        if(player.FacingRight)
                            CheckFiniteCollisions(leftSide, damage, new Vector2(9, -5), 2, 2);
                        else
                            CheckFiniteCollisions(rightSide, damage, new Vector2(9, -5), 2, 2);
                    }


                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            updateMoveFrameAndCheckCollisions();
            timer--;

            if (effectTimer1 > 0)
            {
                effectTimer1--;

                if (effectTimer1 > 19)
                    effectFrame1 = 0;
                else if (effectTimer1 > 13)
                    effectFrame1 = 1;
                else if (effectTimer1 > 6)
                    effectFrame1 = 2;
                else
                    effectFrame1 = 3;

                if (effectTimer1 == 0)
                    effectFrame1 = 0;
            }

            if (effectTimer2 > 0)
            {
                effectTimer2--;

                if (effectTimer2 > 19)
                    effectFrame2 = 0;
                else if (effectTimer2 > 13)
                    effectFrame2 = 1;
                else if (effectTimer2 > 6)
                    effectFrame2 = 2;
                else
                    effectFrame2 = 3;

                if (effectTimer2 == 0)
                    effectFrame2 = 0;
            }

            if (effectTimer3 > 0)
            {
                effectTimer3--;

                if (effectTimer3 > 15)
                    effectFrame3 = 0;
                else if (effectTimer3 > 10)
                    effectFrame3 = 1;
                else if (effectTimer3 > 5)
                    effectFrame3 = 2;
                else
                    effectFrame3 = 3;

                if (effectTimer3 == 0)
                    effectFrame3 = 0;
            }

            //Stay in air when attacking
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;

            if (skillRank < 2 && phase == 1)
            {
                canUse = false;
                useNext[0] = false;
            }
            else if (skillRank < 3 && phase == 2)
            {
                canUse = false;
                useNext[1] = false;
            }

            //--This stops the player from being able to change direction during his final swing
            //--Essentially, it makes it so you can't use it AFTER the animation is over.
            //--Since you can move when "canUse" is false, you must wait until after or you can change direction during it
            if (phase == 3 && animationLength < 0)
            {
                fullCooldown = lastCooldown;
                currentCooldown = fullCooldown;
                canUse = false;
                phase = 0;
            }

            if (animationLength < 0 && justPressed)
            {
                justPressed = false;
                useNext[0] = false;
                useNext[1] = false;
            }

            //--If the timer reaches 0, it means the player stopped the combo early, so reset the skill
            if (timer < 0 && phase > 0)
            {
                phase = 0;
                currentCooldown = fullCooldown;
                canUse = false;
            }

            #region SET RECTANGLES
            if (phase == 0)
            {
                rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY,
                    260, player.VitalRecHeight);
                leftSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 260, player.VitalRecY,
                    260, player.VitalRecHeight);
            }
            else if (phase == 2)
            {
                if (player.FacingRight)
                {
                    rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY + 50,
        260, player.VitalRecHeight + 50);
                    leftSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 260, player.VitalRecY - 30,
                        260, player.VitalRecHeight + 60);
                }
                else
                {
                    rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY - 30,
260, player.VitalRecHeight + 60);
                    leftSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 260, player.VitalRecY + 50,
                        260, player.VitalRecHeight + 50);
                }
            }
            else if (phase == 3)
            {
                if (player.FacingRight)
                {
                    rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY - 100,
        260, player.VitalRecHeight + 100);
                    leftSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 260, player.VitalRecY + 30,
                        260, player.VitalRecHeight + 60);
                }
                else
                {
                    rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2, player.VitalRecY + 30,
        260, player.VitalRecHeight + 60);
                    leftSide = new Rectangle(player.VitalRecX + player.VitalRecWidth / 2 - 260, player.VitalRecY - 100,
                        260, player.VitalRecHeight + 100);
                }
            }
            #endregion
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            switch (phase)
            {
                case 1:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, punch, Color.Black);
                        if (player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                    break;
                case 2:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, secondPunch, Color.Black);
                        if (player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    }
                    break;
                case 3:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, thirdPunch, Color.Black);
                        if (player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    }
                    break;

            }

            if (effectTimer1 > 0)
            {
                if (player.FacingRight)
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetEffectSourceRec(1), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetEffectSourceRec(1), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }

            if (effectTimer2 > 0)
            {
                if (player.FacingRight)
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetEffectSourceRec(2), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], player.Rec, GetEffectSourceRec(2), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }

            if (effectTimer3 > 0)
            {
                if (thirdEffectFacingRight)
                    s.Draw(Game1.skillAnimations[name], thirdEffectRec, GetEffectSourceRec(3), Color.White);
                else
                    s.Draw(Game1.skillAnimations[name], thirdEffectRec, GetEffectSourceRec(3), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

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
                    damage = .4f;
                    experience = 0;
                    fullCooldown = 65;
                    experienceUntilLevel = 700;
                    description = "Daryl swings a sword around like a maniac. Tap quickly as rank increases for more swings. # of Swings: 2";
                    break;
                case 3:
                    damage = .45f;
                    fullCooldown = 65;
                    experienceUntilLevel = 1200;
                    description = "Daryl swings a sword around like a maniac. Tap quickly as rank increases for more swings. # of Swings: 3";
                    experience = 0;
                    break;
                case 4:
                    damage = .5f;
                    experience = 0;
                    description = "Daryl swings a sword around like a maniac. Tap quickly as rank increases for more swings. # of Swings: 3";
                    //name = "Deliberate Dissimilarities";
                    break;
            }
        }
    }
}