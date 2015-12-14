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
    public class HUD
    {
        //--Textures
        Texture2D backgroundTexture, healthBarTexture, skillBarTexture,
                  experienceBarTexture, qBarTex, wBarTex, eBarTex, rBarTex,
                  hudFront, skillFront, skillFrontPad, piggy, skillLevelCircle, skillTip, hudGradient, lootBox;

        //--Rectangles
        Rectangle HUDRec, healthRec, experienceRec, skillBarRec;

        //--Buttons
        Button skillQButton, skillWButton, skillEButton, skillRButton,
               skillQExperienceBarButton, skillWExperienceBarButton, skillEExperienceBarButton, skillRExperienceBarButton, quickRetortBarButton,
               minimizeQuestButton, expBarButton, quickRetortButton;

        //--Attributes
        List<Button> skillButtons;
        List<Button> skillExperienceBars;
        List<Button> skillExperienceBacks;
        List<Rectangle> pickUps;
        Player player;
        int originalHealthWidth;
        int originalExperienceWidth;
        int originalSkillExperienceHeight;
        float addToHealthWidth;

        int cooldownQWidth;
        int cooldownWWidth;
        int cooldownEWidth;
        int cooldownRWidth;

        float targetHealthWidth;
        float targetExpWidth;
        float skillAlpha = .75f;
        Boolean minimized;
        Boolean skillsHidden = false;

        KeyboardState current;
        KeyboardState previous;
        int questSelected = 0;
        
        SpriteFont font;
        Game1 game;
        DescriptionBoxManager skillBoxManager;

        public Boolean SkillsHidden { get { return skillsHidden; } set { skillsHidden = value; } }
        //  CONSTRUCTOR  \\
        public HUD(Texture2D back, Texture2D health, Texture2D skillBar,
            Texture2D experience, Texture2D qBar, Texture2D wBar, Texture2D eBar, Texture2D rBar, Player play, SpriteFont font, Game1 g, 
            DescriptionBoxManager desc, Texture2D hudTop, Texture2D skillTop, Texture2D skillTopPad, Texture2D pig, Texture2D skillLevel, Texture2D gradient, Texture2D skillTool, Texture2D lootBox)
        {
            //--Set attributes
            hudFront = hudTop;
            skillFront = skillTop;
            backgroundTexture = back;
            healthBarTexture = health;
            experienceBarTexture = experience;
            skillBarTexture = skillBar;
            player = play;
            qBarTex = qBar;
            wBarTex = wBar;
            eBarTex = eBar;
            rBarTex = rBar;
            piggy = pig;
            this.font = font;
            game = g;
            skillButtons = new List<Button>();
            skillExperienceBars = new List<Button>();
            skillExperienceBacks = new List<Button>();
            pickUps = new List<Rectangle>();
            skillBoxManager = desc;
            skillFrontPad = skillTopPad;
            skillTip = skillTool;
            hudGradient = gradient;
            this.lootBox = lootBox;

            //--Set the bar's widths
            originalHealthWidth = healthBarTexture.Width;
            originalExperienceWidth = experienceBarTexture.Width;
            originalSkillExperienceHeight = qBarTex.Height;

            //--HUD and Experience/Health bars
            HUDRec = new Rectangle(0, 0, backgroundTexture.Width, backgroundTexture.Height);
            healthRec = new Rectangle(21, 36, healthBarTexture.Width, healthBarTexture.Height);
            experienceRec = new Rectangle(95, 99, experienceBarTexture.Width, experienceBarTexture.Height);
            skillBarRec = new Rectangle(2, 644, skillBarTexture.Width, skillBarTexture.Height);

            //--Skill Boxes
            skillQButton = new Button(Game1.emptyBox, new Rectangle(11, 650, 66, 66));
            skillWButton = new Button(Game1.emptyBox, new Rectangle(91, 650, 66, 66));
            skillEButton = new Button(Game1.emptyBox, new Rectangle(169, 650, 66, 66));
            skillRButton = new Button(Game1.emptyBox, new Rectangle(247, 650, 66, 66));
            quickRetortButton = new Button(new Rectangle(325, 650, 66, 66));
            skillButtons.Add(skillQButton);
            skillButtons.Add(skillWButton);
            skillButtons.Add(skillEButton);
            skillButtons.Add(skillRButton);

            //--The actual colored bars. These are buttons for simplicity of drawing
            skillQExperienceBarButton = new Button(qBarTex,
                                        new Rectangle(6, 648, qBarTex.Width, qBarTex.Height));
            skillWExperienceBarButton = new Button(qBarTex,
                                        new Rectangle(86, 648, wBarTex.Width, wBarTex.Height));
            skillEExperienceBarButton = new Button(qBarTex,
                                        new Rectangle(164, 648, eBarTex.Width, eBarTex.Height));
            skillRExperienceBarButton = new Button(qBarTex,
                                        new Rectangle(242, 648, rBarTex.Width, rBarTex.Height));
            quickRetortBarButton = new Button(qBarTex, new Rectangle(320, 648, rBarTex.Width, rBarTex.Height));

            skillExperienceBars.Add(skillQExperienceBarButton);
            skillExperienceBars.Add(skillWExperienceBarButton);
            skillExperienceBars.Add(skillEExperienceBarButton);
            skillExperienceBars.Add(skillRExperienceBarButton);

            //--Text boxes when items are picked up
            Rectangle dropButton = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 160, 200, 25);
            pickUps.Add(dropButton);
            Rectangle dropButton2 = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 185, 200, 25);
            pickUps.Add(dropButton2);
            Rectangle dropButton3 = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 210, 200, 25);
            pickUps.Add(dropButton3);
            Rectangle dropButton4 = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 235, 200, 25);
            pickUps.Add(dropButton4);
            Rectangle dropButton5 = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 260, 200, 25);
            pickUps.Add(dropButton5);

            skillLevelCircle = skillLevel;

            expBarButton = new Button(experienceRec);

            //--Minimize the quest box
            minimizeQuestButton = new Button(new Rectangle(1250, 10, 30, 20));

            UpdateResolution();
            experienceRec.Width = 0;

            skillsHidden = false;
        }

        public void UpdateResolution()
        {
            //skillQButton.ButtonRecY = (int)(1280 * Game1.aspectRatio) - 82;
            //skillWButton.ButtonRecY = (int)(1280 * Game1.aspectRatio) - 82;
            //skillEButton.ButtonRecY = (int)(1280 * Game1.aspectRatio) - 82;
            //skillRButton.ButtonRecY = (int)(1280 * Game1.aspectRatio) - 82;
            //skillBarRec.Y = (int)(1280 * Game1.aspectRatio) - 94;
            //pickUps[0] = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 160, 200, 25);
            //pickUps[1] = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 185, 200, 25);
            //pickUps[2] = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 210, 200, 25);
            //pickUps[3] = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 235, 200, 25);
            //pickUps[4] = new Rectangle(50, (int)(1280 * Game1.aspectRatio) - 260, 200, 25);

            // 638 = 720 * x
            // 878 = 960 * x
            
        }

        /// <summary>
        /// Changes the size of the health and experience bars
        /// </summary>
        public void Update()
        {
            previous = current;
            current = Keyboard.GetState();

            Game1.questHUD.Update();

            targetHealthWidth = (int)((float)originalHealthWidth * ((float)player.Health / (float)player.realMaxHealth));
            targetExpWidth = (int)((float)originalExperienceWidth * ((float)player.Experience / (float)player.ExperienceUntilLevel));

            addToHealthWidth += ((targetHealthWidth - healthRec.Width) * .1f);
            healthRec.Width = (int)addToHealthWidth;
            experienceRec.Width += (int)((targetExpWidth - experienceRec.Width) * .1f);

            #region Skill Experience Bars
            //--If the player has at least 1 skill

            if (player.EquippedSkills.Count > 0)
            {
                skillQExperienceBarButton.ButtonRecHeight = (int)((float)originalSkillExperienceHeight *
                    ((float)player.EquippedSkills[0].Experience / (float)player.EquippedSkills[0].ExperienceUntilLevel));
            }
            if (player.EquippedSkills.Count > 1)
            {
                skillWExperienceBarButton.ButtonRecHeight = (int)((float)originalSkillExperienceHeight *
                    ((float)player.EquippedSkills[1].Experience / (float)player.EquippedSkills[1].ExperienceUntilLevel));
            }

            if (player.EquippedSkills.Count > 2)
                skillEExperienceBarButton.ButtonRecHeight = (int)((float)originalSkillExperienceHeight *
                    ((float)player.EquippedSkills[2].Experience / (float)player.EquippedSkills[2].ExperienceUntilLevel));

            if (player.EquippedSkills.Count > 3)
                skillRExperienceBarButton.ButtonRecHeight = (int)((float)originalSkillExperienceHeight *
                    ((float)player.EquippedSkills[3].Experience / (float)player.EquippedSkills[3].ExperienceUntilLevel));
            #endregion

            if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                quickRetortBarButton.ButtonRecHeight = (int)((float)originalSkillExperienceHeight * ((float)player.quickRetort.Experience / (float)player.quickRetort.ExperienceUntilLevel));

            //MinimizeQuest();

            if((current.IsKeyUp(Keys.T) && previous.IsKeyDown(Keys.T)) || MyGamePad.RightBumperPressed())
            {
               // if(skillsHidden)
                    skillsHidden = false;
               // else
                   // skillsHidden = true;
            }
        }


        public void Draw(SpriteBatch s)
        {
            //--Background color for the HUD
            s.Draw(hudGradient, new Rectangle(0, 0, hudGradient.Width, hudGradient.Height), Color.White);
            s.Draw(backgroundTexture, HUDRec, Color.White);

            //--Health and experience bars
            s.Draw(healthBarTexture, healthRec, new Rectangle(0, 0, healthRec.Width, healthRec.Height), Color.White);
            s.Draw(experienceBarTexture, experienceRec, new Rectangle(0, 0, experienceRec.Width, experienceRec.Height), Color.White);

            s.Draw(hudFront, HUDRec, Color.White);

            //--Fonts on the HUD
            //s.DrawString(Game1.smallHUDFont, player.Health + " / " + player.realMaxHealth, new Vector2(330 - Game1.smallHUDFont.MeasureString(player.Health + " / " + player.realMaxHealth).X, 50), Color.White * 1f);

            Game1.OutlineFont(Game1.hudStatsFont, s, player.Health + " / " + player.realMaxHealth, 1, (int)(320 - Game1.hudStatsFont.MeasureString(player.Health + " / " + player.realMaxHealth).X), 71, Color.Black, Color.White);


            if(!expBarButton.IsOver())
                Game1.OutlineFont(Game1.hudStatsFont, s, ((float)player.Experience / (float)player.ExperienceUntilLevel * 100).ToString("N0") + "%", 1, (int)(260 - Game1.hudStatsFont.MeasureString(((float)player.Experience / (float)player.ExperienceUntilLevel * 100).ToString("N0") + "%").X), 96, Color.Black, Color.White);
            else
                Game1.OutlineFont(Game1.hudStatsFont, s, player.Experience + " / " + player.ExperienceUntilLevel, 1, (int)(260 - Game1.hudStatsFont.MeasureString(player.Experience + " / " + player.ExperienceUntilLevel).X), 96, Color.Black, Color.White);

            Game1.OutlineFont(Game1.hudLevelFont, s, "Lvl " + player.Level + ": " + player.SocialRank, 1, 110, 36, Color.Black, Color.White);
            //--Draw how many textbooks the player has
            if (player.Textbooks == 0)
            {
                s.DrawString(Game1.font, "0", new Vector2(58, 142), Color.White); //54
            }

            else if (player.Textbooks < 10)
            {
                s.DrawString(Game1.font, "0", new Vector2(53, 142), Color.White);

                s.DrawString(Game1.font, player.Textbooks.ToString(), new Vector2(64, 142), Color.White);
            }
            else
            {
                s.DrawString(Game1.font, player.Textbooks.ToString(), new Vector2(53, 142), Color.White);
            }

            //--End Draw Textbooks
            s.Draw(piggy, new Rectangle(115, 123, 83, 58), Color.White);
            s.DrawString(Game1.VerySmallTwCondensedFont, "$" + Math.Round(player.Money, 2).ToString("N2"), new Vector2(155 - Game1.VerySmallTwCondensedFont.MeasureString("$" + Math.Round(player.Money, 2).ToString("N2")).X / 2, 143), Color.White);

            DrawPickUpText(s);

            if (player.LearnedSkills.Count > 0)//!skillsHidden)
            {
                DrawSkillBoxes(s);

                if (player.EquippedSkills.Count > 0)
                    s.DrawString(Game1.descriptionFont, player.EquippedSkills[0].SkillRank.ToString(), new Vector2(63 - Game1.descriptionFont.MeasureString(player.EquippedSkills[0].SkillRank.ToString()).X / 2, 643), Color.Black);

                if (player.EquippedSkills.Count > 1)
                    s.DrawString(Game1.descriptionFont, player.EquippedSkills[1].SkillRank.ToString(), new Vector2(143 - Game1.descriptionFont.MeasureString(player.EquippedSkills[1].SkillRank.ToString()).X / 2, 643), Color.Black);

                if (player.EquippedSkills.Count > 2)
                    s.DrawString(Game1.descriptionFont, player.EquippedSkills[2].SkillRank.ToString(), new Vector2(221 - Game1.descriptionFont.MeasureString(player.EquippedSkills[2].SkillRank.ToString()).X / 2, 643), Color.Black);

                if (player.EquippedSkills.Count > 3)
                    s.DrawString(Game1.descriptionFont, player.EquippedSkills[3].SkillRank.ToString(), new Vector2(299 - Game1.descriptionFont.MeasureString(player.EquippedSkills[3].SkillRank.ToString()).X / 2, 643), Color.Black);
            }

            if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                s.DrawString(Game1.descriptionFont, player.quickRetort.SkillRank.ToString(), new Vector2(377 - Game1.descriptionFont.MeasureString(player.quickRetort.SkillRank.ToString()).X / 2, 643), Color.Black);

            //s.Draw(skillTip, new Rectangle(10, (int)(1280 * Game1.aspectRatio) - 16, skillTip.Width, skillTip.Height), Color.White);

            Game1.questHUD.Draw(s);
        }

        /// <summary>
        /// Draws the experience bars, cooldowns, and skillboxes on the HUD
        /// </summary>
        /// <param name="s"></param>
        void DrawSkillBoxes(SpriteBatch s)
        {

            //Draws each skill box icon and draws the description box if you hover over the icon
            for (int i = 0; i < skillButtons.Count; i++)
            {
                if (skillButtons[i].ButtonTexture != Game1.emptyBox)
                {
                    skillButtons[i].Draw(s, skillAlpha);
                }
                if (skillButtons[i].IsOver() && player.EquippedSkills.Count >= i + 1)
                {
                    //Rectangle descRec = skillButtons[i].ButtonRec; descRec.X += 120; descRec.Y += 60;
                    Rectangle descRec = new Rectangle(300, 270, 1, 1);
                    skillBoxManager.DrawSkillDescriptions(player.EquippedSkills[i], descRec);
                }
            }

            if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
            {
                s.Draw(player.quickRetort.Icon, quickRetortButton.ButtonRec, Color.White * skillAlpha);

                if (quickRetortButton.IsOver())
                {
                    Rectangle descRec = new Rectangle(300, 270, 1, 1);
                    skillBoxManager.DrawSkillDescriptions(player.quickRetort, descRec);
                }
            }

            //--Updates the skill icons and cooldown bars
            #region Skill Icon Update / Cooldown drawing
            //-- Q SKILL
            if (player.EquippedSkills.Count >= 1)
            {
                skillButtons[0].ButtonTexture = player.EquippedSkills[0].Icon;

                cooldownQWidth = (int)((float)60 * ((float)player.EquippedSkills[0].currentCooldown / (float)player.EquippedSkills[0].FullCooldown));
                Rectangle cooldownRec = skillButtons[0].ButtonRec;
                cooldownRec.Width = cooldownQWidth;
                s.Draw(skillButtons[0].ButtonTexture, new Rectangle(skillButtons[0].ButtonRecX, skillButtons[0].ButtonRecY, cooldownRec.Width, skillButtons[0].ButtonRec.Height), new Rectangle(0, 0, cooldownRec.Width, skillButtons[0].ButtonRec.Height), Color.Black * .9f);

                s.Draw(skillLevelCircle, new Rectangle(skillButtons[0].ButtonRec.X + 36, skillButtons[0].ButtonRecY - 14, (int)(skillLevelCircle.Width), (int)(skillLevelCircle.Height)), Color.White * skillAlpha);
            }
            else
                skillButtons[0].ButtonTexture = Game1.emptyBox;


            //--W SKILL
            if (player.EquippedSkills.Count >= 2)
            {
                skillButtons[1].ButtonTexture = player.EquippedSkills[1].Icon;

                cooldownWWidth = (int)((float)60 * ((float)player.EquippedSkills[1].currentCooldown / (float)player.EquippedSkills[1].FullCooldown));
                Rectangle cooldownRec = skillButtons[1].ButtonRec;
                cooldownRec.Width = cooldownWWidth;
                s.Draw(skillButtons[1].ButtonTexture, new Rectangle(skillButtons[1].ButtonRecX, skillButtons[1].ButtonRecY, cooldownRec.Width, skillButtons[1].ButtonRec.Height), new Rectangle(0, 0, cooldownRec.Width, skillButtons[1].ButtonRec.Height), Color.Black * .9f);

                s.Draw(skillLevelCircle, new Rectangle(skillButtons[1].ButtonRec.X + 36, skillButtons[0].ButtonRecY - 14, (int)(skillLevelCircle.Width), (int)(skillLevelCircle.Height)), Color.White * skillAlpha);
            }
            else
                skillButtons[1].ButtonTexture = Game1.emptyBox;


            //--E SKILL
            if (player.EquippedSkills.Count >= 3)
            {
                skillButtons[2].ButtonTexture = player.EquippedSkills[2].Icon;
                cooldownEWidth = (int)((float)60 * ((float)player.EquippedSkills[2].currentCooldown / (float)player.EquippedSkills[2].FullCooldown));
                Rectangle cooldownRec = skillButtons[2].ButtonRec;
                cooldownRec.Width = cooldownEWidth;
                s.Draw(skillButtons[2].ButtonTexture, new Rectangle(skillButtons[2].ButtonRecX, skillButtons[2].ButtonRecY, cooldownRec.Width, skillButtons[2].ButtonRec.Height), new Rectangle(0, 0, cooldownRec.Width, skillButtons[2].ButtonRec.Height), Color.Black * .9f);
                s.Draw(skillLevelCircle, new Rectangle(skillButtons[2].ButtonRec.X + 36, skillButtons[2].ButtonRecY - 14, skillLevelCircle.Width, skillLevelCircle.Height), Color.White * skillAlpha);
            }
            else
                skillButtons[2].ButtonTexture = Game1.emptyBox;



            //--R SKILL
            if (player.EquippedSkills.Count >= 4)
            {
                skillButtons[3].ButtonTexture = player.EquippedSkills[3].Icon;
                cooldownRWidth = (int)((float)60 * ((float)player.EquippedSkills[3].currentCooldown / (float)player.EquippedSkills[3].FullCooldown));
                Rectangle cooldownRec = skillButtons[3].ButtonRec;
                cooldownRec.Width = cooldownRWidth;
                s.Draw(skillButtons[3].ButtonTexture, new Rectangle(skillButtons[3].ButtonRecX, skillButtons[3].ButtonRecY, cooldownRec.Width, skillButtons[3].ButtonRec.Height), new Rectangle(0, 0, cooldownRec.Width, skillButtons[3].ButtonRec.Height), Color.Black * .9f);
                s.Draw(skillLevelCircle, new Rectangle(skillButtons[3].ButtonRec.X + 36, skillButtons[3].ButtonRecY - 14, skillLevelCircle.Width, skillLevelCircle.Height), Color.White * skillAlpha);
            }
            else
                skillButtons[3].ButtonTexture = Game1.emptyBox;
            #endregion

            if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
            {
                //Quick Retort
                int cooldownQuickRetortWidth = (int)((float)60 * ((float)player.quickRetort.currentCooldown / (float)player.quickRetort.FullCooldown));
                Rectangle cooldownQuickRetortRec = quickRetortButton.ButtonRec;
                cooldownQuickRetortRec.Width = cooldownQuickRetortWidth;
                s.Draw(player.quickRetort.Icon, new Rectangle(cooldownQuickRetortRec.X, cooldownQuickRetortRec.Y, cooldownQuickRetortRec.Width, cooldownQuickRetortRec.Height), new Rectangle(0, 0, cooldownQuickRetortRec.Width, cooldownQuickRetortRec.Height), Color.Black * .9f);
                s.Draw(skillLevelCircle, new Rectangle(cooldownQuickRetortRec.X + 36, cooldownQuickRetortRec.Y - 14, skillLevelCircle.Width, skillLevelCircle.Height), Color.White * skillAlpha);
            }


            s.Draw(skillBarTexture, skillBarRec, Color.White * skillAlpha);

            //--Draws the experience bars
            for (int i = 0; i < player.EquippedSkills.Count; i++)
            {
                //--Max leveled skill, make the bar full and a deep blue
                if (player.EquippedSkills[i].SkillRank == Skill.maxLevel)
                {
                    skillExperienceBars[i].ButtonRecY = 630;
                    skillExperienceBars[i].ButtonRecHeight = 63;
                    s.Draw(skillExperienceBars[i].ButtonTexture, skillExperienceBars[i].ButtonRec, player.EquippedSkills[i].SkillBarColor * skillAlpha);
                }
                //--Otherwise draw it normally
                else
                {
                    float yPos = originalSkillExperienceHeight - skillExperienceBars[i].ButtonRecHeight;
                    skillExperienceBars[i].ButtonRecY = (int)(648 + yPos);

                    s.Draw(skillExperienceBars[i].ButtonTexture, skillExperienceBars[i].ButtonRec, new Rectangle(0, originalSkillExperienceHeight - skillExperienceBars[i].ButtonRecHeight, skillExperienceBars[i].ButtonRecWidth, skillExperienceBars[i].ButtonRecHeight), player.EquippedSkills[i].SkillBarColor * skillAlpha);
                }
            }

            if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
            {
                s.Draw(Game1.singleSkillBarBack, new Vector2(quickRetortBarButton.ButtonRecX - 4, 644), Color.White * skillAlpha);
                //--Max leveled skill, make the bar full and a deep blue
                if (player.quickRetort.SkillRank == Skill.maxLevel)
                {
                    quickRetortBarButton.ButtonRecY = 630;
                    quickRetortBarButton.ButtonRecHeight = 63;
                    s.Draw(quickRetortBarButton.ButtonTexture, quickRetortBarButton.ButtonRec, player.quickRetort.SkillBarColor * skillAlpha);
                }
                //--Otherwise draw it normally
                else
                {
                    float yPos = originalSkillExperienceHeight - quickRetortBarButton.ButtonRecHeight;
                    quickRetortBarButton.ButtonRecY = (int)(648 + yPos);

                    s.Draw(quickRetortBarButton.ButtonTexture, quickRetortBarButton.ButtonRec, new Rectangle(0, originalSkillExperienceHeight - quickRetortBarButton.ButtonRecHeight, quickRetortBarButton.ButtonRecWidth, quickRetortBarButton.ButtonRecHeight), player.quickRetort.SkillBarColor * skillAlpha);
                }
            }

            if (!Game1.gamePadConnected)
                s.Draw(skillFront, skillBarRec, Color.White * skillAlpha);
            else
                s.Draw(skillFrontPad, new Vector2(0, 720 - skillFrontPad.Height + 10), Color.White * skillAlpha);
        }

        /// <summary>
        /// Draws the text when you pick up items
        /// </summary>
        /// <param name="s"></param>
        void DrawPickUpText(SpriteBatch s)
        {
            //--Draws a transparent box where the text for pick ups is drawn, making it easier to see
            if(player.PickUps.Count > 0)
                s.Draw(lootBox, new Rectangle(0, 469, lootBox.Width, lootBox.Height), Color.White);

            for (int i = 0; i < player.PickUps.Count; i++)
            {
                if(player.PickUps[i] != "You don't have room for this item.")
                    s.DrawString(Game1.twConQuestHudInfo, "Picked up: " + player.PickUps[i], new Vector2(pickUps[i].X - 40, pickUps[i].Y + 15), Color.White);
                else
                    s.DrawString(Game1.twConQuestHudInfo, player.PickUps[i], new Vector2(pickUps[i].X - 20, pickUps[i].Y), Color.Maroon);
            }
        }

        void DrawQuestInfo(SpriteBatch s)
        {
            //--Where the box for the quest helper starts
            float questHudX = 910f;

            //--If the player has a quest or more
            if (game.CurrentQuests.Count > 0)
            {
                //--If the currently selected quest no longer exists, reset back to 0
                if (questSelected >= game.CurrentQuests.Count)
                    questSelected = 0;

                Quest thisQuest = game.CurrentQuests[questSelected];

                //--This measures the length of the quest name and centers the name in the quest helper
                Vector2 questNameMeasure = Game1.pickUpFont.MeasureString(thisQuest.QuestName);
                float vecX = (float)(370 / 2) - (float)(questNameMeasure.X / 2) + questHudX;

                //--Draw the name and quest helper box
                if (minimized == false)
                    s.Draw(Game1.whiteFilter, new Rectangle((int)questHudX, 5, 370, 150), Color.Blue * .4f);
                else
                    s.Draw(Game1.whiteFilter, new Rectangle((int)questHudX, 5, 370, 40), Color.Blue * .4f);

                if(thisQuest.StoryQuest)
                    s.DrawString(Game1.pickUpFont, thisQuest.QuestName, new Vector2(vecX, 20), Color.Red);
                else
                    s.DrawString(Game1.pickUpFont, thisQuest.QuestName, new Vector2(vecX, 20), Color.Purple);

                #region Draw Info If Not Minimized
                if (minimized == false)
                {
                    //--If this is a special quest
                    if (thisQuest.SpecialConditions.Count > 0)
                    {
                        for (int i = 0; i < thisQuest.SpecialConditions.Count; i++)
                        {
                            //If this condition is completed, draw it in dark red
                            if(thisQuest.SpecialConditions.ElementAt(i).Value == true)
                                s.DrawString(Game1.pickUpFont, thisQuest.SpecialConditions.ElementAt(i).Key, new Vector2(questHudX + 5, 50 + (i * 20)), Color.DarkRed);
                            //Otherwise draw it normally
                            else
                                s.DrawString(Game1.pickUpFont, thisQuest.SpecialConditions.ElementAt(i).Key, 
                                new Vector2(questHudX + 5, 50 + (i * 20)), Color.White);
                        }
                    }

                    //--If this is a killing quest
                    if (thisQuest.EnemyNames.Count > 0)
                    {
                        for (int i = 0; i < thisQuest.EnemyNames.Count; i++)
                        {
                        //--Write out the string that contains the info for enemies killed and how many to kill
                        String enemiesKilled = thisQuest.EnemyNames[i] + "'s killed: " +
                                                thisQuest.EnemiesKilledForQuest[i].ToString() + " / " +
                                                thisQuest.EnemiesToKill[i].ToString();

                        //--Change text color if the quest is completed
                        if (thisQuest.EnemiesKilledForQuest[i] == thisQuest.EnemiesToKill[i])
                            s.DrawString(Game1.pickUpFont, enemiesKilled, new Vector2(questHudX + 5, 50 + (i * 20)), Color.DarkRed);
                        else
                            s.DrawString(Game1.pickUpFont, enemiesKilled, new Vector2(questHudX + 5, 50 + (i * 20)), Color.White);
                        }
                    }
                            

                    //--Gathering quests
                    else if (thisQuest.ItemsToGather.Count > 0)
                    {
                        List<int> gathered = new List<int>();

                        for (int i = 0; i < thisQuest.ItemName.Count; i++)
                        {
                            //--If the player has at least one, set the attribute. Otherwise it is 0
                            if (player.EnemyDrops.ContainsKey(thisQuest.ItemName[i]))
                                gathered.Add(player.EnemyDrops[thisQuest.ItemName[i]]);
                            else if (player.StoryItems.ContainsKey(thisQuest.ItemName[i]))
                                gathered.Add(player.StoryItems[thisQuest.ItemName[i]]);
                            else
                                gathered.Add(0);

                            //--Set the string
                            String itemsGathered = thisQuest.ItemName[i] + "'s : " +
                                                    gathered[i].ToString() + " / " +
                                                    thisQuest.ItemsToGather[i].ToString();

                            //--Change text color based on completion
                            if (gathered[i] >= thisQuest.ItemsToGather[i])
                                s.DrawString(Game1.pickUpFont, itemsGathered, new Vector2(questHudX + 5, 50 + (i * 20)), Color.DarkRed);
                            else
                                s.DrawString(Game1.pickUpFont, itemsGathered, new Vector2(questHudX + 5, 50 + (i * 20)), Color.White);
                        }
                    }
                }
                #endregion
            }
            
        }
    }

    public class QuestHud
    {
        public List<Quest> questHelperQuests; //Max is five
        KeyboardState last, current;
        ContentManager Content;
        Game1 game;

        Boolean expanded = true; 
        Boolean holdingTab = false;
        int timeHoldingTab;
        int questSelected;

        Texture2D storyExpanded, sideExpanded, minimized, selectBox, storyExpandedPad, sideExpandedPad, minimizedPad;

        public QuestHud(Game1 g)
        {
            game = g;
            Content = new ContentManager(g.Services);
            Content.RootDirectory = "Content";
            questHelperQuests = new List<Quest>();
        }

        public void LoadContent()
        {
            storyExpanded = Content.Load<Texture2D>(@"HUD\storyQuestBox");
            sideExpanded = Content.Load<Texture2D>(@"HUD\sideQuestBox");
            minimized = Content.Load<Texture2D>(@"HUD\minimizedQuestHud");
            selectBox = Content.Load<Texture2D>(@"HUD\questSelectBox");

            storyExpandedPad = Content.Load<Texture2D>(@"HUD\storyQuestBoxController");
            sideExpandedPad = Content.Load<Texture2D>(@"HUD\sideQuestBoxController");
            minimizedPad = Content.Load<Texture2D>(@"HUD\minimizedQuestHudController");
        }

        public void AddQuestToHelper(Quest q)
        {
            //If it isn't already in the helper and the helper isn't full
            if (questHelperQuests.Count < 5 && !questHelperQuests.Contains(q))
            {
                q.inQuestHelper = true;
                questHelperQuests.Add(q);
            }
        }

        public void RemoveQuestFromHelper(Quest q)
        {
            //If the quest is in the helper
            if (questHelperQuests.Contains(q))
            {
                q.inQuestHelper = false;
                questHelperQuests.Remove(q);
            }
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            //Holding tab, increment the amount of time until you hit 2 seconds
            if (current.IsKeyDown(Keys.Tab))
            {
                timeHoldingTab++;

                if (timeHoldingTab >= 30)
                    holdingTab = true;
            }
            else
                timeHoldingTab = 0;

            //Change selected quest
            if ((last.IsKeyDown(Keys.Tab) && current.IsKeyUp(Keys.Tab) && !holdingTab) || MyGamePad.RightDownAnalogPressed())
            {
                questSelected++;

                if (questSelected == 5 || questSelected > questHelperQuests.Count - 1)
                    questSelected = 0;

                timeHoldingTab = 0;
            }
                //Expand or minimize the quest HUD
            else if ((((last.IsKeyDown(Keys.Tab) && current.IsKeyUp(Keys.Tab)) || current.IsKeyDown(Keys.Tab)) && timeHoldingTab > 30) || MyGamePad.RightAnalogPressedIn())
            {
                expanded = !expanded;

                timeHoldingTab = 0;
            }

            if (MyGamePad.RightUpAnalogPressed())
            {
                questSelected--;

                if (questSelected < 0)
                    questSelected = questHelperQuests.Count - 1;
            } 

            if (current.IsKeyUp(Keys.Tab) && holdingTab)
                holdingTab = false;
        }

        public void Draw(SpriteBatch s)
        {
            //--If the currently selected quest no longer exists, reset back to 0
            //This is a failsafe because pressing Tab at the max quest makes it cycle around, however removing a quest from the list will have to make it move back 1
            if (questSelected >= questHelperQuests.Count)
            {
                questSelected -= 1;

                //If that was the last quest, put it back to 0. The HUD will stop showing now
                if (questSelected < 0)
                    questSelected = 0;
            }

            //Only draw this if you have a quest in your quest helper
            if (questHelperQuests.Count > 0)
            {
                //Draw the expanded quest HUD
                if (expanded)
                {
                    if (questHelperQuests[questSelected].StoryQuest)
                    {
                        if(!Game1.gamePadConnected)
                            s.Draw(storyExpanded, new Rectangle(1280 - storyExpanded.Width, 0, storyExpanded.Width, storyExpanded.Height), Color.White);
                        else
                            s.Draw(storyExpandedPad, new Rectangle(1280 - storyExpandedPad.Width, 0, storyExpandedPad.Width, storyExpandedPad.Height), Color.White);

                    }

                    if (questHelperQuests[questSelected].StoryQuest == false)
                    {
                        if (!Game1.gamePadConnected)
                            s.Draw(sideExpanded, new Rectangle(1280 - sideExpanded.Width, 0, sideExpanded.Width, sideExpanded.Height), Color.White);
                        else
                            s.Draw(sideExpandedPad, new Rectangle(1280 - sideExpandedPad.Width, 0, sideExpandedPad.Width, sideExpandedPad.Height), Color.White);

                    }

                    s.Draw(selectBox, new Rectangle(1280 - selectBox.Width, 108 + (22 * questSelected), selectBox.Width, selectBox.Height), Color.White);

                    for (int i = 0; i < questHelperQuests.Count; i++)
                    {
                        if (!questHelperQuests[i].StoryQuest)
                            s.DrawString(Game1.twConQuestHudInfo, questHelperQuests[i].QuestName, new Vector2(1130 - Game1.twConQuestHudInfo.MeasureString(questHelperQuests[i].QuestName).X / 2, 98 + 15 + (i * 22)), new Color(102, 45, 145));
                        else
                            s.DrawString(Game1.twConQuestHudInfo, questHelperQuests[i].QuestName, new Vector2(1130 - Game1.twConQuestHudInfo.MeasureString(questHelperQuests[i].QuestName).X / 2, 98 + 15 + (i * 22)), new Color(237, 0, 6));
                    }

                    #region Draw the Quest Info

                    s.DrawString(Game1.twConRegularSmall, questHelperQuests[questSelected].QuestName, new Vector2(1130 - Game1.twConRegularSmall.MeasureString(questHelperQuests[questSelected].QuestName).X / 2, 13), Color.White);

                    //--If this is a special quest
                    if (questHelperQuests[questSelected].SpecialConditions.Count > 0)
                    {
                        for (int i = 0; i < questHelperQuests[questSelected].SpecialConditions.Count; i++)
                        {
                            //If this condition is completed, draw it in dark red
                            if (questHelperQuests[questSelected].SpecialConditions.ElementAt(i).Value == true)
                                s.DrawString(Game1.twConQuestHudInfo, "-" + questHelperQuests[questSelected].SpecialConditions.ElementAt(i).Key, new Vector2(1000, 40 + (i * 20)), Color.LightGreen);
                            //Otherwise draw it normally
                            else
                                s.DrawString(Game1.twConQuestHudInfo, "-" + questHelperQuests[questSelected].SpecialConditions.ElementAt(i).Key, new Vector2(1000, 40 + (i * 20)), Color.White);
                        }
                    }

                    //--If this is a killing quest
                    if (questHelperQuests[questSelected].EnemyNames.Count > 0)
                    {
                        for (int i = 0; i < questHelperQuests[questSelected].EnemyNames.Count; i++)
                        {
                            //--Write out the string that contains the info for enemies killed and how many to kill
                            String enemiesKilled = questHelperQuests[questSelected].EnemyNames[i] + "'s killed: " +
                                                    questHelperQuests[questSelected].EnemiesKilledForQuest[i].ToString() + " / " +
                                                    questHelperQuests[questSelected].EnemiesToKill[i].ToString();

                            //--Change text color if the quest is completed
                            if (questHelperQuests[questSelected].EnemiesKilledForQuest[i] == questHelperQuests[questSelected].EnemiesToKill[i])
                                s.DrawString(Game1.twConQuestHudInfo, "-" + enemiesKilled, new Vector2(1000, 40 + (i * 20)), Color.LightGreen);
                            else
                                s.DrawString(Game1.twConQuestHudInfo, "-" + enemiesKilled, new Vector2(1000, 40 + (i * 20)), Color.White);
                        }
                    }


                    //--Gathering quests
                    else if (questHelperQuests[questSelected].ItemsToGather.Count > 0)
                    {
                        List<int> gathered = new List<int>();

                        for (int i = 0; i < questHelperQuests[questSelected].ItemName.Count; i++)
                        {
                            //--If the player has at least one, set the attribute. Otherwise it is 0
                            if (Game1.Player.EnemyDrops.ContainsKey(questHelperQuests[questSelected].ItemName[i]))
                                gathered.Add(Game1.Player.EnemyDrops[questHelperQuests[questSelected].ItemName[i]]);
                            else if (Game1.Player.StoryItems.ContainsKey(questHelperQuests[questSelected].ItemName[i]))
                                gathered.Add(Game1.Player.StoryItems[questHelperQuests[questSelected].ItemName[i]]);
                            else
                                gathered.Add(0);

                            //--Set the string
                            String tmp = questHelperQuests[questSelected].ItemName[i];

                            //if (questHelperQuests[questSelected].ItemsToGather[i] > 1)
                            //    tmp += "'s";
                            String itemsGathered = tmp + " : " +
                                                    gathered[i].ToString() + " / " +
                                                    questHelperQuests[questSelected].ItemsToGather[i].ToString();

                            //--Change text color based on completion
                            if (gathered[i] >= questHelperQuests[questSelected].ItemsToGather[i])
                                s.DrawString(Game1.twConQuestHudInfo, "-" + itemsGathered, new Vector2(1000, 40 + (i * 20)), Color.LightGreen);
                            else
                                s.DrawString(Game1.twConQuestHudInfo, "-" + itemsGathered, new Vector2(1000, 40 + (i * 20)), Color.White);
                        }
                    }
                    #endregion
                }
                //Otherwise draw the minimized version
                else
                {
                    if(!Game1.gamePadConnected)
                        s.Draw(minimized, new Rectangle(1280 - minimized.Width, 0, minimized.Width, minimized.Height), Color.White);
                    else
                        s.Draw(minimizedPad, new Rectangle(1280 - minimizedPad.Width, 0, minimizedPad.Width, minimizedPad.Height), Color.White);


                    s.Draw(selectBox, new Rectangle(1280 - selectBox.Width, 10 + (22 * questSelected), selectBox.Width, selectBox.Height), Color.White);

                    for (int i = 0; i < questHelperQuests.Count; i++)
                    {
                        if(!questHelperQuests[i].StoryQuest)
                            s.DrawString(Game1.twConQuestHudInfo, questHelperQuests[i].QuestName, new Vector2(1130 - Game1.twConQuestHudInfo.MeasureString(questHelperQuests[i].QuestName).X / 2, 15 + (i * 22)), new Color(102, 45, 145));
                        else
                            s.DrawString(Game1.twConQuestHudInfo, questHelperQuests[i].QuestName, new Vector2(1130 - Game1.twConQuestHudInfo.MeasureString(questHelperQuests[i].QuestName).X / 2, 15 + (i * 22)), new Color(237, 0, 6));
                    }
                }
            }
        }
    }
}
