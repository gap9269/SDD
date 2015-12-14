using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class CombustibleConfutationCH2 : Skill
    {
        Rectangle rightSide, leftSide;

        public CombustibleConfutationCH2(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .75f;
            experience = 0;
            experienceUntilLevel = 400;
            skillRank = 1;
            levelToUse = 8;
            name = "Combustible Confutation CH.2";
            canUse = true; 
            fullCooldown = 75;
            description = "Surrounded by things you despise? Use the art of fire to push them away! And burn them too, I guess.";

            //--Animation and skill attributes
            hitPauseTime = 1;

            costToBuy = 2;
            skillBarColor = new Color(255, 135, 0);

            transformLevels = new int[3] { 3, 6, 9 };

            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(10);
            playerLevelRequiredToLevel.Add(10);

            playerLevelRequiredToLevel.Add(11);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(16);

            playerLevelRequiredToLevel.Add(24);
            playerLevelRequiredToLevel.Add(29);
            playerLevelRequiredToLevel.Add(36);
            playerLevelRequiredToLevel.Add(40);
            playerLevelRequiredToLevel.Add(43);

            rangedOrMelee = AttackType.RangedOrMelee.Melee;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            switch (skillRank)
            {
                case 2:
                    damage = .77f;
                    experience = 0;
                    experienceUntilLevel = 1200;
                    
                    break;
                case 3:
                    damage = .79f;
                    experience = 0;
                    experienceUntilLevel = 1800;
                    
                    break;
                case 4:
                    damage = .8f;
                    experience = 0;
                    experienceUntilLevel = 2400;
                    
                    break;
                case 5:
                    damage = .82f;
                    experience = 0;
                    experienceUntilLevel = 3000;
                    
                    break;
                case 6:
                    damage = .85f;
                    experience = 0;
                    experienceUntilLevel = 4700;
                    
                    break;
                case 7:
                    damage = .87f;
                    experience = 0;
                    experienceUntilLevel = 6500;
                    
                    break;
                case 8:
                    damage = .91f;
                    experience = 0;
                    experienceUntilLevel = 8300;
                    
                    break;
                case 9:
                    damage = .94f;
                    experience = 0;
                    experienceUntilLevel = 9500;
                    
                    break;
                case 10:
                    damage = .79f;
                    experienceUntilLevel = 4000;
                    experience = 0;
                    fullCooldown = 65;
                    
                    break;
                case 11:
                    damage = .82f;
                    experienceUntilLevel = 4000;
                    experience = 0;
                    fullCooldown = 65;
                    
                    break;
                case 12:
                    damage = .85f;
                    experienceUntilLevel = 4000;
                    experience = 0;
                    fullCooldown = 65;
                    
                    break;
                case 13:
                    damage = .9f;
                    experienceUntilLevel = 4000;
                    experience = 0;
                    fullCooldown = 65;
                    
                    break;
                case 14:
                    damage = .95f;
                    experienceUntilLevel = 4000;
                    experience = 0;
                    fullCooldown = 65;
                    
                    break;
                case 15:
                    damage = 1f;
                    experience = 0;
                    fullCooldown = 65;
                    break;
            }

            description = "Surrounded by things you despise? Use the art of fire to push them away! And burn them too, I guess.";
        }


        public override Rectangle GetSourceRec()
        {
            switch(skillRank)
            {
                case 1:
                case 2:
                    if (moveFrame < 4)
                        return new Rectangle(moveFrame * 1056, 0, 1056, 430);
                    else
                        return new Rectangle((moveFrame - 4) * 1056, 430, 1056, 430);
                case 3:
                case 4:
                case 5:
                case 6:
                    if(moveFrame== 0)
                        return new Rectangle(3168, 430, 1056, 430);
                    else if (moveFrame < 4)
                        return new Rectangle((moveFrame - 1)* 1056, 860, 1056, 430);
                    else
                        return new Rectangle((moveFrame - 4) * 1056, 1290, 1056, 430);
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (moveFrame == 0)
                        return new Rectangle(3168, 1290, 1056, 430);
                    else if (moveFrame < 4)
                        return new Rectangle((moveFrame - 1) * 1056, 1720, 1056, 430);
                    else
                        return new Rectangle((moveFrame - 4) * 1056, 2150, 1056, 430);
                case 12:
                case 13:
                case 14:
                case 15:
                    if (moveFrame == 0)
                        return new Rectangle(3168, 2150, 1056, 430);
                    else if (moveFrame < 4)
                        return new Rectangle((moveFrame - 1) * 1056, 2580, 1056, 430);
                    else
                        return new Rectangle((moveFrame - 4) * 1056, 3010, 1056, 430);
            }

            return new Rectangle();
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 22;
                useKey = key;
                hitThisTime = 0;
                enemiesHitThisAttack.Clear();
                bossesHitThisAttack.Clear();
                interactiveObjectsThisAttack.Clear();
                if (skillRank < transformLevels[0])
                    PlayRandomUseSound(0, 2);
                else if (skillRank < transformLevels[1])
                    PlayRandomUseSound(3, 5);
                else if (skillRank < transformLevels[2])
                    PlayRandomUseSound(6, 8);
                else
                    PlayRandomUseSound(9, 11);

                if(skillRank >= 7)
                    game.Camera.ShakeCamera(10, (skillRank / 5));
            }
        }

        public override void Update()
        {
            base.Update();

            last = current;
            current = Keyboard.GetState();
            StayInAir();

            if (animationLength >= 0)
            {
                if (animationLength > 18)
                    moveFrame = 0;
                else if (animationLength > 15)
                    moveFrame = 1;
                else if (animationLength > 12)
                    moveFrame = 2;
                else if (animationLength > 9)
                    moveFrame = 3;
                else if (animationLength > 6)
                    moveFrame = 4;
                else if (animationLength > 3)
                    moveFrame = 5;
                else
                    moveFrame = 6;
            }

            if (animationLength > 0)
                player.MoveDuringAttackJump();

            if (moveFrame > 0 && moveFrame < 6)
            {
                float kb = (10 * (skillRank / 5)) - (moveFrame * 4);

                if (kb < 20)
                    kb = 20;

                CheckFiniteCollisions(rightSide, damage, new Vector2(kb, -8), 2 * (int)(skillRank / 7.5f), 2 * (int)(skillRank / 7.5f));
                CheckFiniteCollisions(leftSide, damage, new Vector2(kb, -8), 2 * (int)(skillRank / 7.5f), 2 * (int)(skillRank / 7.5f));
            }

            #region Set rectangles
            int width;
            width = 100 * (moveFrame + 1);
            switch (skillRank)
            {
                case 1:
                case 2:
                    if (width > 270)
                        width = 270;
                    if(player.FacingRight)
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 30,
    width + 50, player.VitalRecHeight + 40);
                        leftSide = new Rectangle(player.VitalRecX  + 50 - width, player.VitalRecY - 30,
    width, player.VitalRecHeight + 40);
                    }
                    else
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 30,
    width, player.VitalRecHeight + 40);
                        leftSide = new Rectangle(player.VitalRecX - width, player.VitalRecY - 30,
    width + 50, player.VitalRecHeight + 40);
                    }
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    if (width > 300)
                        width = 300;
                    if(player.FacingRight)
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 100,
    width + 50, player.VitalRecHeight + 140);
                        leftSide = new Rectangle(player.VitalRecX  + 50 - width, player.VitalRecY - 100,
    width, player.VitalRecHeight + 140);
                    }
                    else
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 100,
    width, player.VitalRecHeight + 140);
                        leftSide = new Rectangle(player.VitalRecX - width, player.VitalRecY - 100,
    width + 50, player.VitalRecHeight + 140);
                    }
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    if (width > 370)
                        width = 370;
                    if(player.FacingRight)
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 140,
    width + 50, player.VitalRecHeight + 180);
                        leftSide = new Rectangle(player.VitalRecX + 50 - width, player.VitalRecY - 140,
    width, player.VitalRecHeight + 180);
                    }
                    else
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 140,
    width, player.VitalRecHeight + 180);
                        leftSide = new Rectangle(player.VitalRecX - width, player.VitalRecY - 140,
    width + 50, player.VitalRecHeight + 180);
                    }
                    break;
                case 12:
                case 13:
                case 14:
                case 15:
                    if (width > 465)
                        width = 465;
                    if(player.FacingRight)
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 110,
    width + 50, player.VitalRecHeight + 180);
                        leftSide = new Rectangle(player.VitalRecX + 50 - width, player.VitalRecY - 110,
    width, player.VitalRecHeight + 180);
                    }
                    else
                    {
                        rightSide = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 110,
    width, player.VitalRecHeight + 180);
                        leftSide = new Rectangle(player.VitalRecX - width, player.VitalRecY - 110,
    width + 50, player.VitalRecHeight + 180);
                    }
                    break;
            }

            #endregion

            //--If the animation is completed
            if (animationLength < 0 && justPressed)
            {
                moveFrame = 0;
                justPressed = false;
            }

            //--If it is on cooldown, you cannot use it
            if (currentCooldown > 0)
            {
                canUse = false;

            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (animationLength >= 0)
            {
                if (player.FacingRight)
                {
                    s.Draw(Game1.skillAnimations[name], new Vector2(player.RecX - 205, player.RecY - 32), GetSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.RecX - 1056 + 784, player.RecY - 32, 1056, 430), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

    }
}
