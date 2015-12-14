using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    class CombustibleConfutationCH3Demo : Skill
    {
        Rectangle attackRec;

        public CombustibleConfutationCH3Demo(Texture2D sheet, Player play, Texture2D ico)
            : base(sheet, play, ico, true)
        {
            //--Base Stats
            damage = .58f;
            experience = 0;
            experienceUntilLevel = 400;
            skillRank = 1;
            levelToUse = 1;
            name = "Combustible Confutation CH.3";
            canUse = true;
            description = "Settle your differences with a firey kick to the face-parts.";
            fullCooldown = 75;
            //--Animation and skill attributes
            hitPauseTime = 1;
            maxHit = 3;

            transformLevels = new int[3] { 3, 6, 10 };

            costToBuy = 1;
            skillBarColor = new Color(255, 135, 0);

            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(6);
            playerLevelRequiredToLevel.Add(7);
            playerLevelRequiredToLevel.Add(9);

            playerLevelRequiredToLevel.Add(11);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(13);
            playerLevelRequiredToLevel.Add(16);
            playerLevelRequiredToLevel.Add(20);

            playerLevelRequiredToLevel.Add(24);
            playerLevelRequiredToLevel.Add(29);
            playerLevelRequiredToLevel.Add(36);
            playerLevelRequiredToLevel.Add(40);
            playerLevelRequiredToLevel.Add(43);

            rangedOrMelee = AttackType.RangedOrMelee.Melee;
            skillType = AttackType.AttackTypes.Blunt;
        }

        public override Rectangle GetSourceRec()
        {
            switch (skillRank)
            {
                case 1:
                case 2:
                    if (moveFrame < 5)
                        return new Rectangle(moveFrame * 778, 0, 778, 527);
                    else
                        return new Rectangle((moveFrame - 5) * 778, 527, 778, 527);
                case 3:
                case 4:
                case 5:
                    if (moveFrame < 3)
                        return new Rectangle((moveFrame * 778) + 1556, 527, 778, 527);
                    else
                        return new Rectangle((moveFrame - 3) * 778, 1054, 778, 527);
                case 6:
                case 7:
                case 8:
                case 9:
                    if (moveFrame == 0)
                        return new Rectangle(3112, 1054, 778, 527);
                    else if (moveFrame < 6)
                        return new Rectangle((moveFrame - 1) * 778, 1581, 778, 527);
                    else
                        return new Rectangle(0, 2108, 778, 527);
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    if (moveFrame < 4)
                        return new Rectangle((moveFrame * 778) + 778, 2108, 778, 527);
                    else
                        return new Rectangle((moveFrame - 4) * 778, 2635, 778, 527);
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

                if (skillRank >= 6)
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
                CheckFiniteCollisions(attackRec, damage, new Vector2(12, -12), 2, 2);
            }

            #region Set rectangles
            if (player.FacingRight)
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 60,
    250, player.VitalRecHeight + 70);
                        break;
                    case 3:
                    case 4:
                    case 5:

                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 65,
    285, player.VitalRecHeight + 90);
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 100,
    345, player.VitalRecHeight + 130);
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        attackRec = new Rectangle(player.VitalRecX + player.VitalRecWidth, player.VitalRecY - 140,
    420, player.VitalRecHeight + 180);
                        break;
                }

            }
            else
            {
                switch (skillRank)
                {
                    case 1:
                    case 2:
                        attackRec = new Rectangle(player.VitalRecX - 250, player.VitalRecY - 60,
                            250, player.VitalRecHeight + 70);
                        break;
                    case 3:
                    case 4:
                    case 5:

                        attackRec = new Rectangle(player.VitalRecX - 285, player.VitalRecY - 65,
                            285, player.VitalRecHeight + 90);
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:

                        attackRec = new Rectangle(player.VitalRecX - 345, player.VitalRecY - 100,
                            345, player.VitalRecHeight + 130);
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        attackRec = new Rectangle(player.VitalRecX - 420, player.VitalRecY - 140,
                            420, player.VitalRecHeight + 180);
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
                    s.Draw(Game1.skillAnimations[name], new Vector2(player.RecX, player.RecY - 128), GetSourceRec(), Color.White);
                }
                else
                {
                    s.Draw(Game1.skillAnimations[name], new Rectangle(player.RecX - 248, player.RecY - 128, 778, 527), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

        public override void ApplyLevelUp(Boolean silent = false)
        {
            base.ApplyLevelUp(silent);

            if (skillRank == 2)
                skillRank = 3;
            else if (skillRank == 4)
                skillRank = 6;
            else if (skillRank == 7)
                skillRank = 10;

            switch (skillRank)
            {
                case 3:
                    damage = .59f;
                    experience = 0;
                    experienceUntilLevel = 800;
                    maxHit = 4;
                    break;
                case 6:
                    damage = .67f;
                    experience = 0;
                    experienceUntilLevel = 2000;
                    maxHit = 5;
                    break;
                case 10:
                    damage = .75f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 6;
                    break;
                case 11:
                    damage = .8f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 6;
                    break;
                case 12:
                    damage = .85f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 6;
                    break;
                case 13:
                    damage = .9f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 6;
                    break;
                case 14:
                    damage = .95f;
                    experienceUntilLevel = 1650;
                    experience = 0;
                    fullCooldown = 65;
                    maxHit = 6;
                    break;
                case 15:
                    damage = 1f;
                    experience = 0;
                    maxHit = 6;
                    fullCooldown = 65;
                    break;
            }

            description = "Settle your differences with a firey kick to the face-parts. \nEnemies hit: " + maxHit;

        }
    }
}
