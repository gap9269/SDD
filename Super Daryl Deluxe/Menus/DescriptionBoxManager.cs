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
    public class DescriptionBoxManager
    {
        SpriteBatch spriteBatch;
        Rectangle descriptionBoxRec;
        SpriteFont font;
        int boxWidth = 430; //Height and width of the box texture, but not the whole texture itself
        int boxHeight = 221;

        public DescriptionBoxManager(SpriteBatch s, SpriteFont f)
        {
            spriteBatch = s;
            font = f;
        }

        public void DrawSkillDescriptions(Skill skill, Rectangle box)
        {
           // Game1.Player.EquippedSkills[0].Experience = 100;
            descriptionBoxRec = box;
            descriptionBoxRec.X = 20;
            descriptionBoxRec.Y += 170;

            if (descriptionBoxRec.X + descriptionBoxRec.Width > 720)
            {
                descriptionBoxRec.X -= 450;
            }


            spriteBatch.Draw(Game1.skillDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White * .8f);
            spriteBatch.DrawString(Game1.skillLevelMoireFont, skill.Name, new Vector2(205 - (Game1.skillLevelMoireFont.MeasureString(skill.Name).X / 2), descriptionBoxRec.Y + 5), Color.White);
            spriteBatch.DrawString(Game1.skillInfoImpactFont, skill.SkillRank.ToString(), new Vector2(85, descriptionBoxRec.Y + 39), Color.White);

            //Tell the player when they can rank up the skill if they can't yet
            if (skill.SkillRank > 1 && skill.SkillRank < 4 && Game1.Player.Level < skill.PlayerLevelsRequiredToLevel[skill.SkillRank - 1])
            {
                spriteBatch.DrawString(Game1.twConQuestHudInfo, "(NEXT RANK AT LVL. " + skill.PlayerLevelsRequiredToLevel[skill.SkillRank - 1] + ")", new Vector2(descriptionBoxRec.X + 28, descriptionBoxRec.Y + 41) + new Vector2(Game1.twConQuestHudInfo.MeasureString(skill.Experience + " / " + skill.ExperienceUntilLevel).X, 0), Color.Red);
            }

            spriteBatch.DrawString(Game1.skillInfoImpactFont, skill.Damage.ToString("N2") + "%", new Vector2(descriptionBoxRec.X + 310, descriptionBoxRec.Y + 39), Color.White);

            spriteBatch.DrawString(Game1.skillInfoImpactFont, Game1.WrapText(Game1.skillInfoImpactFont, skill.Description, 345), new Vector2(descriptionBoxRec.X + 25, descriptionBoxRec.Y + 62), Color.White);

            if (skill.SkillRank < 4)
            {
                //Tell the player when they can rank up the skill if they can't yet
                if (skill.SkillRank > 1 && Game1.Player.Level < skill.PlayerLevelsRequiredToLevel[skill.SkillRank - 1])
                {
                    spriteBatch.DrawString(Game1.twConQuestHudInfo, "(NEXT RANK AT LVL. " + skill.PlayerLevelsRequiredToLevel[skill.SkillRank - 1] + ")", new Vector2(descriptionBoxRec.X + 60, descriptionBoxRec.Y + 135) + new Vector2(Game1.skillInfoImpactFont.MeasureString(skill.Experience + " / " + skill.ExperienceUntilLevel).X, 0), Color.Red);

                    spriteBatch.DrawString(Game1.skillInfoImpactFont, skill.Experience + " / " + skill.ExperienceUntilLevel, new Vector2(descriptionBoxRec.X + 50, descriptionBoxRec.Y + 132), Color.White * .8f);
                }
                else
                    spriteBatch.DrawString(Game1.skillInfoImpactFont, skill.Experience + " / " + skill.ExperienceUntilLevel, new Vector2(descriptionBoxRec.X + 50, descriptionBoxRec.Y + 132), Color.White);
            }
            else
                spriteBatch.DrawString(font, "Max Rank", new Vector2(descriptionBoxRec.X + 50, descriptionBoxRec.Y + 132), Color.White);

            ////The type of skill
            //String skillType = "(" + skill.SkillType.ToString() + ", " + skill.RangedOrMelee.ToString() + ")";
            //spriteBatch.DrawString(font, skillType, new Vector2(descriptionBoxRec.X + 25, descriptionBoxRec.Y + 225), Color.Black);

        }

        public void DrawEquipDescriptions(Equipment equip, Rectangle box)
        {

            descriptionBoxRec = box;
            descriptionBoxRec.X -= 50;
            descriptionBoxRec.Y += 50;

            if (descriptionBoxRec.X + boxWidth > 1230)
            {
                descriptionBoxRec.X = 1280 - 520;
            }
            if (descriptionBoxRec.Y < 0)
            {
                descriptionBoxRec.Y += 250;
            }
            if (descriptionBoxRec.Y + 250 > 720)
            {
                descriptionBoxRec.Y = box.Y - 315;
            }

            if (!(equip is Money) && !(equip is Karma) && !(equip is Experience))
            {
                spriteBatch.Draw(Game1.equipDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White);
            }
            else
                spriteBatch.Draw(Game1.otherDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White);

            //NAME
            spriteBatch.DrawString(font, equip.Name, new Vector2(descriptionBoxRec.X + 115, descriptionBoxRec.Y + 52), Color.Black);

            //REQUIRED LEVEL
            if (!(equip is Money) && !(equip is Karma) && !(equip is Experience))
            {
                if (Game1.Player.Level >= equip.Level)
                    spriteBatch.DrawString(font, equip.Level.ToString(), new Vector2(descriptionBoxRec.X + 195, descriptionBoxRec.Y + 75), Color.Black);
                else
                    spriteBatch.DrawString(font, equip.Level.ToString(), new Vector2(descriptionBoxRec.X + 195, descriptionBoxRec.Y + 75), Color.Red);
            }
            //DESCRIPTION
            if (!(equip is Money) && !(equip is Karma) && !(equip is Experience))
                spriteBatch.DrawString(font, Game1.WrapText(font, equip.Description, 364), new Vector2(descriptionBoxRec.X + 103, descriptionBoxRec.Y + 96), Color.Black);
            else
                spriteBatch.DrawString(font, Game1.WrapText(font, equip.Description, 364), new Vector2(descriptionBoxRec.X + 103, descriptionBoxRec.Y + 76), Color.Black);

            if (equip is Money)
            {
                spriteBatch.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(descriptionBoxRec.X + 390, descriptionBoxRec.Y + 52), Color.White);
                spriteBatch.DrawString(font, (equip as Money).Amount.ToString("N2"), new Vector2(descriptionBoxRec.X + 413, descriptionBoxRec.Y + 54), Color.Black);
            }

            Vector2 vec = new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y);
            Vector2 healthVec = vec + new Vector2(104, 175);
            Vector2 defenseVec = vec + new Vector2(320, 175);
            Vector2 strengthVec = vec + new Vector2(104, 225);
            //Draw the original stats that the item gives

            if (!(equip is Money) && !(equip is Karma) && !(equip is Experience))
            {
                spriteBatch.DrawString(font, "+ " + equip.Health.ToString(), healthVec, Color.White);
                spriteBatch.DrawString(font, "+ " + equip.Strength.ToString(), strengthVec, Color.White);
                spriteBatch.DrawString(font, "+ " + equip.Defense.ToString(), defenseVec, Color.White);
                if (equip.PassiveAbility != null && equip.PassiveAbility.Name != "")
                {
                    spriteBatch.DrawString(font, equip.PassiveAbility.Name, new Vector2(descriptionBoxRec.X + 131, descriptionBoxRec.Y + 136), Color.DarkCyan);
                }
                else
                    spriteBatch.DrawString(font, "No Passive", new Vector2(descriptionBoxRec.X + 131, descriptionBoxRec.Y + 136), Color.Red);
            }

            #region Draw the stat differences for Weapons
            if (equip is Weapon)
            {
                if ((equip as Weapon).CanHoldTwo)
                {
                    spriteBatch.Draw(Game1.dualWieldIcon, new Vector2(descriptionBoxRec.X + 274,descriptionBoxRec.Y + 217), Color.White);
                    spriteBatch.DrawString(font, "Dual Wield Allowed", vec + new Vector2(315, 224), Color.White);
                }
                if (Game1.Player.EquippedWeapon != null && equip != Game1.Player.EquippedWeapon && equip != Game1.Player.SecondWeapon)
                {
                    //Get the difference between the equipped weapon and the one the player is viewing
                    int healthDiff = equip.Health - Game1.Player.EquippedWeapon.Health;
                    int defenseDiff = equip.Defense - Game1.Player.EquippedWeapon.Defense;
                    int strengthDiff = equip.Strength - Game1.Player.EquippedWeapon.Strength;

                    #region Draw the bonus differences for the first Weapon
                    if (healthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Health)).X, 0), Color.Red);

                    if (strengthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Strength)).X, 0), Color.Red);

                    if (defenseDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Defense)).X, 0), Color.Red);
                    #endregion

                    //Get the difference between the second equipped weapon and the one the player is viewing
                    if (Game1.Player.SecondWeapon != null)
                    {
                        int secondHealthDiff = equip.Health - Game1.Player.SecondWeapon.Health;
                        int secondDefenseDiff = equip.Defense - Game1.Player.SecondWeapon.Defense;
                        int secondStrengthDiff = equip.Strength - Game1.Player.SecondWeapon.Strength;

                        #region Draw the bonus differences for the second Weapon
                        if (secondHealthDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                        if (secondStrengthDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                        if (secondDefenseDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                        #endregion
                    }
                    else //The player has no second weapon, so it's all positive stat differences
                    {
                        #region Draw the positive stat differences next to the first accessory differences
                        spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                        spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                        spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                        #endregion
                    }

                }
                else if(Game1.Player.EquippedWeapon == null)//No weapons, so it's all positive
                {
                    #region Draw the stat differences, all positive
                    spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    #endregion
                }
            }
            #endregion

            #region Draw the stat differences for Accessories
            if (equip is Accessory)
            {

                if (Game1.Player.EquippedAccessory != null && equip != Game1.Player.EquippedAccessory && equip != Game1.Player.SecondAccessory)
                {
                    //Get the difference between the equipped weapon and the one the player is viewing in the shop
                    int healthDiff = equip.Health - Game1.Player.EquippedAccessory.Health;
                    int defenseDiff = equip.Defense - Game1.Player.EquippedAccessory.Defense;
                    int strengthDiff = equip.Strength - Game1.Player.EquippedAccessory.Strength;

                    #region Draw the bonus differences for the first accessory
                    if (healthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Health)).X, 0), Color.Red);

                    if (strengthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Strength)).X, 0), Color.Red);

                    if (defenseDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(equip.Defense)).X, 0), Color.Red);
                    #endregion

                    //Get the difference between the second equipped weapon and the one the player is viewing in the shop
                    if (Game1.Player.SecondAccessory != null)
                    {
                        int secondHealthDiff = equip.Health - Game1.Player.SecondAccessory.Health;
                        int secondDefenseDiff = equip.Defense - Game1.Player.SecondAccessory.Defense;
                        int secondStrengthDiff = equip.Strength - Game1.Player.SecondAccessory.Strength;

                        #region Draw the bonus differences for the second accessory
                        if (secondHealthDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                        if (secondStrengthDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                        if (secondDefenseDiff >= 0)
                            spriteBatch.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                        else
                            spriteBatch.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                        #endregion
                    }
                    else //The player has no second accessory, so it's all positive stat differences
                    {
                        #region Draw the positive stat differences next to the first accessory differences
                        spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                        spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                        spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(equip.Defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                        #endregion
                    }

                }
                else if (Game1.Player.EquippedAccessory == null)//No accessories, so it's all positive
                {
                    #region Draw the stat differences, all positive
                    spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    #endregion
                }
            }
            #endregion

            #region Draw the stat differences for Hats
            if (equip is Hat)
            {

                if (Game1.Player.EquippedHat != null && equip != Game1.Player.EquippedHat)
                {
                    //Get the difference between the equipped hat and the one the player is viewing in the shop
                    int healthDiff = equip.Health - Game1.Player.EquippedHat.Health;
                    int defenseDiff = equip.Defense - Game1.Player.EquippedHat.Defense;
                    int strengthDiff = equip.Strength - Game1.Player.EquippedHat.Strength;

                    #region Draw the bonus differences for the first hat
                    if (healthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Red);

                    if (strengthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Red);

                    if (defenseDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Red);
                    #endregion
                }
                else if (Game1.Player.EquippedHat == null)//No hat, so it's all positive
                {
                    #region Draw the stat differences, all positive
                    spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    #endregion
                }
            }
            #endregion

            #region Draw the stat differences for Outfits
            if (equip is Hoodie)
            {

                if (Game1.Player.EquippedHoodie != null && equip != Game1.Player.EquippedHoodie)
                {
                    //Get the difference between the equipped outfit and the one the player is viewing in the shop
                    int healthDiff = equip.Health - Game1.Player.EquippedHoodie.Health;
                    int defenseDiff = equip.Defense - Game1.Player.EquippedHoodie.Defense;
                    int strengthDiff = equip.Strength - Game1.Player.EquippedHoodie.Strength;

                    #region Draw the bonus differences for the first outfit
                    if (healthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Health)).X, 0), Color.Red);

                    if (strengthDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Strength)).X, 0), Color.Red);

                    if (defenseDiff >= 0)
                        spriteBatch.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    else
                        spriteBatch.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(equip.Defense)).X, 0), Color.Red);
                    #endregion
                }
                else if (Game1.Player.EquippedHoodie == null)//No outfit, so it's all positive
                {
                    #region Draw the stat differences, all positive
                    spriteBatch.DrawString(font, "(+" + equip.Health + ")", healthVec + new Vector2(font.MeasureString("  +" + Math.Abs(equip.Health)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Strength + ")", strengthVec + new Vector2(font.MeasureString("  +" + Math.Abs(equip.Strength)).X, 0), Color.Green);
                    spriteBatch.DrawString(font, "(+" + equip.Defense + ")", defenseVec + new Vector2(font.MeasureString("  +" + Math.Abs(equip.Defense)).X, 0), Color.Green);
                    #endregion
                }
            }
            #endregion
        }

        public void DrawCollectibleDescriptions(String collectibleName, String collectibleDescription, Rectangle box)
        {
            descriptionBoxRec = box;
            descriptionBoxRec.X -= 50;
            descriptionBoxRec.Y += 50;

            if (descriptionBoxRec.X + boxWidth > 1230)
            {
                descriptionBoxRec.X = 1280 - 520;
            }
            if (descriptionBoxRec.Y < 0)
            {
                descriptionBoxRec.Y += 250;
            }
            if (descriptionBoxRec.Y + 250 > 720)
            {
                descriptionBoxRec.Y = box.Y - 170;
            }

            spriteBatch.Draw(Game1.otherDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White);
            spriteBatch.DrawString(font, collectibleName, new Vector2(descriptionBoxRec.X + 114, descriptionBoxRec.Y + 53), Color.Black);

            spriteBatch.DrawString(font, Game1.WrapText(font, collectibleDescription, 360), new Vector2(descriptionBoxRec.X + 106, descriptionBoxRec.Y + 76), Color.Black);
        }
        public void DrawStoryItemDescriptions(String storyItemName, Rectangle box)
        {

            descriptionBoxRec = box;
            descriptionBoxRec.X -= 50;
            descriptionBoxRec.Y += 50;

            if (descriptionBoxRec.X + boxWidth > 1230)
            {
                descriptionBoxRec.X = 1280 - 520;
            }
            if (descriptionBoxRec.Y < 0)
            {
                descriptionBoxRec.Y += 250;
            }
            if (descriptionBoxRec.Y + 250 > 720)
            {
                descriptionBoxRec.Y = box.Y - 170;
            }

            spriteBatch.Draw(Game1.otherDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White);
            spriteBatch.DrawString(font, storyItemName, new Vector2(descriptionBoxRec.X + 114, descriptionBoxRec.Y + 53), Color.Black);

            spriteBatch.DrawString(font, Game1.WrapText(font, Game1.allStoryItems.allStoryItems[storyItemName], 360), new Vector2(descriptionBoxRec.X + 106, descriptionBoxRec.Y + 76), Color.Black);


        }

        public void DrawLootDescriptions(String dropName, Rectangle box)
        {

            descriptionBoxRec = box;
            descriptionBoxRec.X -= 50;
            descriptionBoxRec.Y += 50;

            if (descriptionBoxRec.X + boxWidth > 1230)
            {
                descriptionBoxRec.X = 1280 - 520;
            }
            if (descriptionBoxRec.Y < 0)
            {
                descriptionBoxRec.Y += 250;
            }
            if (descriptionBoxRec.Y + 250 > 720)
            {
                descriptionBoxRec.Y = box.Y - 170;
            }

            spriteBatch.Draw(Game1.otherDescriptionBox, new Vector2(descriptionBoxRec.X, descriptionBoxRec.Y), Color.White);
            spriteBatch.DrawString(font, EnemyDrop.allDrops[dropName].name, new Vector2(descriptionBoxRec.X + 114, descriptionBoxRec.Y + 53), Color.Black);

            spriteBatch.DrawString(font, EnemyDrop.allDrops[dropName].description, new Vector2(descriptionBoxRec.X + 106, descriptionBoxRec.Y + 76), Color.Black);
            spriteBatch.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Vector2(descriptionBoxRec.X + 470 - Game1.font.MeasureString(EnemyDrop.allDrops[dropName].sellCost.ToString("N2")).X - 25, descriptionBoxRec.Y + 125), Color.White);
            spriteBatch.DrawString(font, EnemyDrop.allDrops[dropName].sellCost.ToString("N2"), new Vector2(descriptionBoxRec.X + 470 - Game1.font.MeasureString(EnemyDrop.allDrops[dropName].sellCost.ToString("N2")).X, descriptionBoxRec.Y + 127), Color.Black);
        }
    }
}
