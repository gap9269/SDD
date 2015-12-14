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
    public class DiscussDifferencesDemo : Skill
    {
        // ATTRIBUTES \\
        Rectangle punch;
        Rectangle secondPunch;
        Rectangle thirdPunch, checkPlatRec;
        int phase;
        int timer;
        int defaultTimer = 90;
        int lastCooldown = 45;

        // CONSTRUCTOR \\
        public DiscussDifferencesDemo(Texture2D animSheet, Player play, Texture2D ico)
            : base(animSheet, play, ico, false)
        {

            //--Base Stats
            damage = .4f;
            experience = 0;
            experienceUntilLevel = 1;
            skillRank = 1;
            name = "Discuss Differences";
            description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 1";
            fullCooldown = 150;
            //--Animation and skill attributes
            animationLength = 0;
            phase = 0;
            timer = -1;
            canUse = true;
            costToBuy = 1;
            levelToUse = 1;
            skillBarColor = new Color(238, 28, 36);

            transformLevels = new int[2] { 2, 3 };

            skillType = AttackType.AttackTypes.Blunt;
            rangedOrMelee = AttackType.RangedOrMelee.Melee;

            playerLevelRequiredToLevel.Add(1);
            playerLevelRequiredToLevel.Add(1);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(5);

            playerLevelRequiredToLevel.Add(5);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(12);

            playerLevelRequiredToLevel.Add(16);
            playerLevelRequiredToLevel.Add(19);
            playerLevelRequiredToLevel.Add(23);
            playerLevelRequiredToLevel.Add(25);
            playerLevelRequiredToLevel.Add(27);
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
                    if (moveFrame < 5)
                        return new Rectangle(530 * moveFrame, 796, 530, 398);
                    else if (moveFrame < 10)
                        return new Rectangle(530 * (moveFrame - 5), 1194, 530, 398);
                    else
                        return new Rectangle(530 * (moveFrame - 10), 1592, 530, 398);
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
            if (justPressed == false)
            {
                justPressed = true;
                moveFrame = 0;
                if (phase == 0)
                {
                    //currentCooldown = 150;
                    animationLength = 16;
                    timer = defaultTimer;
                    phase = 1;
                    hitPauseTime = 0;
                    if (skillRank == 4)
                        hitPauseTime = 2;
                    PlayRandomUseSound(0, 5);
                }

                //--Second press, cooldown is set back to 100, and restarts the timer
                if (phase == 1 && timer <= defaultTimer - 1)
                {
                    //currentCooldown = 100;
                    animationLength = 16;
                    timer = defaultTimer;
                    phase = 2;
                    hitPauseTime = 0;
                    if (skillRank == 4)
                        hitPauseTime = 2;
                    PlayRandomUseSound(5, 10);
                }

                //--On the third hit, give the skill experience
                if (phase == 2 && timer <= defaultTimer - 1)
                {
                    fullCooldown = lastCooldown;
                    currentCooldown = fullCooldown;
                    animationLength = 19;
                    timer = 25;
                    phase = 3;
                    hitPauseTime = 3;
                    if (skillRank == 4)
                        hitPauseTime = 5;
                    PlayRandomUseSound(10, 15);
                }
            #endregion
            }

            /*
                #region Move while attacking
                //--Move the player at a slower rate
                if (current.IsKeyDown(Keys.Right))
                {
                    player.Position += new Vector2(4, 0);
                }

                if (current.IsKeyDown(Keys.Left))
                {
                    player.Position += new Vector2(-4, 0);
                }
                #endregion
             * */
        }

        public void updateMoveFrameAndCheckCollisions()
        {

            switch (phase)
            {
                case 1:
                    if (animationLength > 13)
                        moveFrame = 0;
                    else if (animationLength > 10)
                        moveFrame = 1;
                    else if(animationLength > 7)
                        moveFrame = 2;
                    else if(animationLength > 3)
                        moveFrame = 3;
                    else
                        moveFrame = 4;


                    if(animationLength == 8)
                        CheckCollisions(punch, damage - .1f, new Vector2(15, -6), 3,3);

                    if (animationLength == 0 && useNext[0] == true && skillRank >= 2)
                    {
                        animationLength = 16;
                        timer = defaultTimer;
                        phase = 2;
                        hitPauseTime = 0;
                        if (skillRank == 4)
                            hitPauseTime = 2;
                        moveFrame = 0;
                        PlayRandomUseSound(5, 10);
                    }

                    break;
                case 2:
                    if (animationLength > 14)
                        moveFrame = 0;
                    else if (animationLength > 11)
                        moveFrame = 1;
                    else if (animationLength > 7)
                        moveFrame = 2;
                    else if (animationLength > 3)
                        moveFrame = 3;
                    else
                        moveFrame = 4;

                    if(animationLength == 8)
                        CheckCollisions(secondPunch, damage - .1f, new Vector2(15, -6),3,3);

                    if (animationLength == 0 && useNext[1] == true && skillRank >= 3)
                    {
                        fullCooldown = lastCooldown;
                        currentCooldown = fullCooldown;
                        animationLength = 19;
                        timer = 25;
                        phase = 3;
                        hitPauseTime = 3;
                        if (skillRank == 4)
                            hitPauseTime = 5;
                        moveFrame = 0;
                        PlayRandomUseSound(10, 15);
                        if (player.VelocityX < 6 && player.VelocityX > -6)
                            player.VelocityX = 0;
                        else
                        {
                            if (player.VelocityX > 0)
                                player.VelocityX -= 5;
                            else
                                player.VelocityX += 5;
                        }
                    }
                    break;
                case 3:

                    
                    if (animationLength > 16)
                        moveFrame = 0;
                    else if (animationLength > 13)
                        moveFrame = 1;
                    else if (animationLength > 11)
                        moveFrame = 2;
                    else if (animationLength > 9)
                        moveFrame = 3;
                    else if (animationLength > 7)
                        moveFrame = 4;
                    else if (animationLength > 6)
                        moveFrame = 5;
                    else if (animationLength > 5)
                        moveFrame = 6;
                    else if (animationLength > 4)
                        moveFrame = 9;
                    else if (animationLength > 3)
                        moveFrame = 10;
                    else if (animationLength > 2)
                        moveFrame = 11;
                    else if (animationLength > 1)
                        moveFrame = 12;
                    else
                        moveFrame = 13;

                    /*
                    if (animationLength > 13)
                        moveFrame = 0;
                    else if (animationLength > 10)
                        moveFrame = 1;
                    else if (animationLength > 8)
                        moveFrame = 2;
                    else if (animationLength > 6)
                        moveFrame = 3;
                    else if (animationLength > 4)
                        moveFrame = 4;
                    else if (animationLength > 2)
                        moveFrame = 5;
                    else
                        moveFrame = 6;*/

                    if(animationLength == 12)
                        CheckCollisions(thirdPunch, damage, new Vector2(20, -6), 4, 4);
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

                if (animationLength > 0 && phase != 3)
                {
                    if (player.VelocityX > -5 && player.VelocityX < 5)
                    {
                        if (player.FacingRight)
                        {
                            player.VelocityX = 5;

                        }
                        else
                        {
                            player.VelocityX = -5;
                        }
                    }
                }

                player.AttackFloating = true;
                player.VelocityY = 0;

                for (int i = 0; i < Game1.currentChapter.CurrentMap.Platforms.Count; i++)
                {
                    if (checkPlatRec.Intersects(Game1.currentChapter.CurrentMap.Platforms[i].Rec) && Game1.currentChapter.CurrentMap.Platforms[i].Passable == false)
                    {
                        if (player.VelocityX < 6 && player.VelocityX > -6)
                            player.VelocityX = 0;
                        else
                        {
                            if (player.VelocityX > 0)
                                player.VelocityX -= 5;
                            else
                                player.VelocityX += 5;
                        }
                    }
                }
            }
            else
            {
                player.AttackFloating = false;

                if (animationLength == 0)
                {
                    if (player.VelocityX < 6 && player.VelocityX > -6)
                        player.VelocityX = 0;
                    else
                    {
                        if (player.VelocityX > 0)
                            player.VelocityX -= 5;
                        else
                            player.VelocityX += 5;
                    }
                }
            }


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
                canUse = false;

            if (animationLength < 0)
            {
                justPressed = false;
                useNext[0] = false;
                useNext[1] = false;
            }

            //--If the timer reaches 0, it means the player stopped the combo early, so reset the skill
            if (timer < 0)
                phase = 0;

            #region SET RECTANGLES
            //--Sets the rectangles based on which way the character is facing
            if (player.FacingRight == true)
            {
                punch = new Rectangle((int)player.VitalRecX, (int)player.VitalRecY, 170, 150);
                secondPunch = new Rectangle((int)player.VitalRecX, (int)player.VitalRecY- 10, 180, 160);
                thirdPunch = new Rectangle((int)player.VitalRecX, (int)player.VitalRecY - 30, 180, 230);
                checkPlatRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY, (int)50,
player.VitalRecHeight - 40);
            }
            else
            {
                punch = new Rectangle((int)player.VitalRecX - player.VitalRecWidth - 50, (int)player.VitalRecY, 170, 150);
                secondPunch = new Rectangle((int)player.VitalRecX - player.VitalRecWidth - 50, (int)player.VitalRecY - 10, 180, 160);
                thirdPunch = new Rectangle((int)player.VitalRecX - player.VitalRecWidth - 50, (int)player.VitalRecY - 30, 180, 230);
                checkPlatRec = new Rectangle(player.VitalRecX - (int)50, player.VitalRecY, (int)50,
player.VitalRecHeight - 40);
            }
            #endregion
        }

        //--Draws based on which phase the skill is in, and if the animation is actually playing (animationLength is greater than 0)
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            switch(phase)
            {
                case 1:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, punch, Color.Black);
                        if(player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec,  GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec,  GetSourceRec(), Color.White,0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    }
                    break;
                case 2:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, secondPunch, Color.Black);
                        if(player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec,  GetSourceRec(), Color.White,0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    }
                    break;
                case 3:
                    if (animationLength >= 0)
                    {
                        //s.Draw(Game1.emptyBox, thirdPunch, Color.Black);
                        if(player.FacingRight)
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White);
                        else
                            s.Draw(Game1.skillAnimations[name], player.Rec, GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                    }
                    break;
                    
            }

        }

        //--If the experience hits what is needed to level up, this is called
        //--Increase stats and other things based on level
        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            if (skillRank == 4)
                skillRank = 10;

            switch (skillRank)
            {
                case 2:
                    damage = .45f;
                    experience = 0;
                    experienceUntilLevel = 12;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 2";
                    break;
                case 3:
                    damage = .5f;
                    experienceUntilLevel = 500;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    experience = 0;
                    break;
                case 4:
                    damage = .52f;
                    experience = 0;
                    experienceUntilLevel = 700;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 5:
                    damage = .54f;
                    experience = 0;
                    experienceUntilLevel = 700;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 6:
                    damage = .56f;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 7:
                    damage = .58f;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 8:
                    damage = .6f;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 9:
                    damage = .63f;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 10:
                    damage = .65f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 11:
                    damage = .68f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 12:
                    damage = .72f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 13:
                    damage = .75f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 14:
                    damage = .8f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
                case 15:
                    damage = .85f;
                    experience = 0;
                    description = "A basic ability that offers a longer combo as its rank increases. \n# of Punches: 3";
                    break;
            }
        }
    }
}
