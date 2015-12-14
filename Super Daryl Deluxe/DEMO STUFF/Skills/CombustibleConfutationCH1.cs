using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class CombustibleConfutationCH1Demo : Skill
    {
        Rectangle attackRec;

        public CombustibleConfutationCH1Demo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .55f;
            experience = 0;
            experienceUntilLevel = 400;
            skillRank = 1;
            levelToUse = 3;
            name = "Combustible Confutation CH.1";
            canUse = true;
            description = "Daryl coats his fist in fire and brings the smack-down. \nEnemies Hit: 3";
            fullCooldown = 75;
            //--Animation and skill attributes
            hitPauseTime = 3;

            maxHit = 3;
            costToBuy = 1;
            skillBarColor = new Color(254, 138, 0);

            transformLevels = new int[3] { 3, 7, 12 };

            playerLevelRequiredToLevel.Add(3);
            playerLevelRequiredToLevel.Add(4);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(6);

            playerLevelRequiredToLevel.Add(8);
            playerLevelRequiredToLevel.Add(11);
            playerLevelRequiredToLevel.Add(12);
            playerLevelRequiredToLevel.Add(15);
            playerLevelRequiredToLevel.Add(19);

            playerLevelRequiredToLevel.Add(23);
            playerLevelRequiredToLevel.Add(27);
            playerLevelRequiredToLevel.Add(34);
            playerLevelRequiredToLevel.Add(38);
            playerLevelRequiredToLevel.Add(41);

            rangedOrMelee = AttackType.RangedOrMelee.Melee;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            if (skillRank == 2)
                skillRank = 3;
            else if (skillRank == 4)
                skillRank = 7;
            else if (skillRank == 8)
                skillRank = 12;

            switch (skillRank)
            {

                case 3:
                    damage = .59f;
                    experience = 0;
                    experienceUntilLevel = 550;
                    maxHit = 3;
                    break;
                case 7:
                    damage = .69f;
                    experience = 0;
                    experienceUntilLevel = 1800;
                    maxHit = 4;
                    break;

                case 12:
                    damage = .85f;
                    experienceUntilLevel = 1950;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 5;
                    break;
                case 13:
                    damage = .9f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 5;
                    break;
                case 14:
                    damage = .95f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 5;
                    break;
                case 15:
                    damage = 1f;
                    experience = 0;
                    maxHit = 6;
                    fullCooldown = 65;
                    break;
            }

            description = "Daryl coats his fist in fire and brings the smack-down. \nEnemies Hit: " + maxHit;

        }

        public override Rectangle GetSourceRec()
        {
            if (skillRank >= 12)
            {
                if (moveFrame < 5)
                    return new Rectangle(749 * moveFrame, 0 + ((3) * 796), 749, 398);
                else
                    return new Rectangle(749 * (moveFrame - 5), 398 + ((3) * 796), 749, 398);
            }
            else if (skillRank >= 7)
            {
                if (moveFrame < 5)
                    return new Rectangle(737 * moveFrame, 398 * 4, 737, 398);
                else
                    return new Rectangle(737 * (moveFrame - 5), 398 * 5, 737, 398);
            }
            else if (skillRank >= 3)
            {
                if (moveFrame < 5)
                    return new Rectangle(737 * moveFrame, 398 * 2, 737, 398);
                else
                    return new Rectangle(737 * (moveFrame - 5), 398 * 3, 737, 398);
            }
            else
            {
                if (moveFrame < 5)
                    return new Rectangle(737 * moveFrame, 0, 737, 398);
                else
                    return new Rectangle(737 * (moveFrame - 5), 398, 737, 398);
            }
        }

        public override void Use(Game1 g, Keys key)
        {
            base.Use(g, key);

            if (justPressed == false)
            {
                justPressed = true;
                //--Set cooldown and other base stuff
                currentCooldown = fullCooldown;
                animationLength = 20;
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

                if (skillRank >= 7)
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
                if (animationLength > 17)
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

            if (moveFrame > 1 && moveFrame < 6)
            {
                CheckFiniteCollisions(attackRec, damage, new Vector2(10, -5), 2, 2);
            }

            #region Set rectangles
            if (player.FacingRight)
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 30,
    225, player.VitalRecHeight + 40);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 45,
    285, player.VitalRecHeight + 70);
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 60,
    345, player.VitalRecHeight + 100);
                        break;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 80,
    420, player.VitalRecHeight + 120);
                        break;
                }

            }
            else
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:

                        attackRec = new Rectangle(player.VitalRecX - 225, player.VitalRecY - 30,
                            225, player.VitalRecHeight + 40);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        attackRec = new Rectangle(player.VitalRecX - 285, player.VitalRecY - 45,
                            285, player.VitalRecHeight + 70);
                        break;
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        attackRec = new Rectangle(player.VitalRecX - 345, player.VitalRecY - 60,
                            345, player.VitalRecHeight + 100);
                        break;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        attackRec = new Rectangle(player.VitalRecX - 420, player.VitalRecY - 80,
                            420, player.VitalRecHeight + 120);
                        break;
                }
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
                    s.Draw(Game1.skillAnimations[name], new Vector2(player.RecX, player.RecY) , GetSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.RecX - 207, player.RecY, 737, 398), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }
}
