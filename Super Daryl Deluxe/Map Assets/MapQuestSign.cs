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
    public class MapQuestSign
    {
        Rectangle rec;
        String quest;
        List<int> enemiesToKill;
        List<int> enemiesDead;
        List<String> enemyNames;
        Player player;
        Boolean active = false;
        List<Object> rewards;
        Boolean completedQuest = false;
        Boolean givenRewards = false;

        int blinkTimer;

        public Boolean CompletedQuest { get { return completedQuest; } set { completedQuest = value; } }
        public Boolean GivenRewards { get { return givenRewards; } set { givenRewards = value; } }
        public Boolean Active { get { return active; } set { active = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }

        public MapQuestSign(int x, int y, String message, List<int> toKill, List<int> dead, List<String> names, Player p)
        {
            rec = new Rectangle(x, y, Game1.mapSign.Width / 3, Game1.mapSign.Height);
            quest = message;
            enemiesDead = dead;
            enemiesToKill = toKill;
            enemyNames = names;
            player = p;
        }

        public MapQuestSign(int x, int y, String message, List<int> toKill, List<int> dead, List<String> names, Player p, List<Object> rewards)
        {
            this.rewards = rewards;
            rec = new Rectangle(x, y, Game1.mapSign.Width / 3, Game1.mapSign.Height);
            quest = message;
            enemiesDead = dead;
            enemiesToKill = toKill;
            enemyNames = names;
            player = p;
        }

        public void GetSourceRec()
        {

        }

        public void ActivateSign()
        {
            //Give rewards
            if (rewards != null && !givenRewards && completedQuest)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                    if (rewards[i] is Equipment)
                    {
                        Equipment eq = rewards[i] as Equipment;

                        if (eq is Weapon)
                        {
                            player.AddWeaponToInventory(eq as Weapon);
                        }
                        if (eq is Hat)
                        {
                            player.AddHatToInventory(eq as Hat);
                        }
                        if (eq is Hoodie)
                        {
                            player.AddShirtToInventory(eq as Hoodie);
                        }
                        if (eq is Accessory)
                        {
                            player.AddAccessoryToInventory(eq as Accessory);
                        }
                        if (eq is Money)
                        {
                            player.Money += (eq as Money).Amount;
                        }
                        if (eq is Experience)
                        {
                            player.Experience += (eq as Experience).Amount;
                        }
                        if (eq is Karma)
                        {
                            player.Karma += (eq as Karma).Amount;
                        }
                    }
                    else if (rewards[i] is StoryItem)
                    {
                        StoryItem temp = rewards[i] as StoryItem;

                        if (player.StoryItems.ContainsKey(temp.Name))
                            player.StoryItems[temp.Name]++;
                        else
                            player.StoryItems.Add(temp.Name, 1);
                    }
                }

                givenRewards = true;
                Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(rewards, false));
            }
            else
                active = true;

        }

        public void Update()
        {
            if (!player.Rec.Intersects(rec))
                active = false;

            if (!active)
            {
                blinkTimer++;

                if (blinkTimer == 90)
                    blinkTimer = 0;
            }
        }

        public void DrawRewards(SpriteBatch s)
        {
            if (rewards != null)
            {
                s.DrawString(Game1.font, "Rewards:", new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 75), Color.Black);
                for (int i = 0; i < rewards.Count; i++)
                {
                    if (rewards[i] is Equipment)
                    {
                        if (rewards[i] is Money)
                        {
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallMoneyIcon"].Width, Game1.smallTypeIcons["smallMoneyIcon"].Height), Color.White);
                            s.DrawString(Game1.font, "     " + (rewards[i] as Money).Amount.ToString("N2"), new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 96 + (i * 23)), Color.Black);
                        }
                        else if (rewards[i] is Karma)
                        {
                            s.Draw(Game1.smallTypeIcons["smallKarmaIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                            s.DrawString(Game1.font, "     " + (rewards[i] as Karma).Amount, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 96 + (i * 23)), Color.Black);
                        }
                        else if (rewards[i] is Experience)
                        {
                            s.Draw(Game1.smallTypeIcons["smallExperienceIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                            s.DrawString(Game1.font, "     " + (rewards[i] as Experience).Amount, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 96 + (i * 23)), Color.Black);
                        }
                        else
                        {
                            if(rewards[i] is Weapon)
                                s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                            if (rewards[i] is Hat)
                                s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                            if (rewards[i] is Hoodie)
                                s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                            if (rewards[i] is Accessory)
                                s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                            s.DrawString(Game1.font, "     " + (rewards[i] as Equipment).Name, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 96 + (i * 23)), Color.Black);
                        }
                    }
                    if (rewards[i] is Collectible)
                    {
                        if(rewards[i] is Textbook)
                            s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                        if (rewards[i] is BronzeKey)
                            s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                        if (rewards[i] is SilverKey)
                            s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                        if (rewards[i] is GoldKey)
                            s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(rec.X + rec.Width / 2 - (int)Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 98 + (i * 23), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                        s.DrawString(Game1.font, "     " + (rewards[i] as Collectible).collecName, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2, rec.Y + 96 + (i * 23)), Color.Black);
                    }
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (active)
            {
                if (!completedQuest)
                {
                    s.Draw(Game1.mapSign, rec, new Rectangle(496, 0, 248, 409), Color.White);
                    s.DrawString(Game1.TwCondensedSmallFont, "MAP QUEST", new Vector2(rec.X + rec.Width / 2 - Game1.TwCondensedSmallFont.MeasureString("MAP QUEST").X / 2 + 2,
                        rec.Y + 10), Color.Black);
                    s.DrawString(Game1.font, quest, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(quest).X / 2 + 2,
                        rec.Y + 35), Color.Black);

                    DrawRewards(s);

                    for (int i = 0; i < enemyNames.Count; i++)
                    {
                        String line = enemyNames[i] + ":   " + enemiesDead[i] + "/" + enemiesToKill[i];
                        s.DrawString(Game1.font, line, new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString(line).X / 2 + 2,
                            rec.Y + 55 + (i * 25)), Color.Black);
                    }
                }

                else
                {
                    s.Draw(Game1.mapSign, rec, new Rectangle(496, 0, 248, 409), Color.White);
                    s.DrawString(Game1.font, "MAP QUEST", new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString("MAP QUEST").X / 2,
                        rec.Y + 15), Color.White);

                    s.DrawString(Game1.font, "COMPLETED!", new Vector2(rec.X + rec.Width / 2 - Game1.font.MeasureString("COMPLETED!").X / 2,
    rec.Y + 35), Color.White);
                }
            }
            else
            {
                s.Draw(Game1.mapSign, rec, new Rectangle(248, 0, 248, 409), Color.White);

                if(blinkTimer < 45)
                    s.Draw(Game1.mapSign, rec, new Rectangle(0, 0, 248, 409), Color.White);
            }
        }
    }
}
