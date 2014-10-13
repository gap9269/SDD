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
    //  HOW THIS WORKS \\

    //--The class has two string lists: one for the quest and one for the completion of the quest.
    //--Add the dialogue to the list one text box at a time, and add the completion dialogue
    //--Set the experience and money rewards in the constructor. If the quest requires killing an enemy, set that as well
    //--Check the completion condition inside of Update(), and give the player additional rewards inside of RewardPlayer() if necessary
    public class Quest
    {
        //protected Player player;
        protected List<String> questDialogue;
        protected List<String> completedDialogue;
        protected String questName;
        protected bool completedQuest;
        protected List<String> enemyNames;
        protected List<int> enemiesKilledForQuest;
        protected List<int> enemiesToKill;
        protected List<int> itemsToGather;
        protected List<String> itemNames;
        protected Dictionary<String, Boolean> specialConditions;
        protected Boolean storyQuest;
        protected String taskForQuestsPage;
        //--These are for the quest fliers that appear when talking to NPCs
        protected String conditionsToComplete;
        protected String rewards;
        public Boolean inQuestHelper = false;
        public String npcName;
        protected String descriptionForJournal;

        protected List<Object> rewardObjects;

        public String TaskForQuestsPage { get { return taskForQuestsPage; } set { taskForQuestsPage = value; } }
        public List<Object> RewardObjects { get { return rewardObjects; } }
        public String DescriptionForJournal { get { return descriptionForJournal; } set { descriptionForJournal = value; } }
        public Boolean StoryQuest { get { return storyQuest; } set { storyQuest = value; } }
        public List<String> QuestDialogue { get { return questDialogue; } }
        public List<String> CompletedDialogue { get { return completedDialogue; } }
        public String QuestName { get { return questName; } set { questName = value; } }
        public String ConditionsToComplete { get { return conditionsToComplete; } set { conditionsToComplete = value; } }
        public String Rewards { get { return rewards; } set { rewards = value; } }
        public List<String> EnemyNames { get { return enemyNames; } set { enemyNames = value; } }
        public List<int> EnemiesKilledForQuest { get { return enemiesKilledForQuest; } set { enemiesKilledForQuest = value; } }
        public List<int> EnemiesToKill { get { return enemiesToKill; } set { enemiesToKill = value; } }
        public List<int> ItemsToGather { get { return itemsToGather; } set { itemsToGather = value; } }
        public List<String> ItemName { get { return itemNames; } set { itemNames = value; } }
        public Boolean CompletedQuest { get { return completedQuest; } set { completedQuest = value; } }
        public Dictionary<String, Boolean> SpecialConditions { get { return specialConditions; } set { specialConditions = value; } }
        public Quest(Boolean story)
        {
            questDialogue = new List<string>();
            completedDialogue = new List<string>();
            //player = play;
            completedQuest = false;
            //game = g;
            storyQuest = story;
            enemiesToKill = new List<int>();
            itemsToGather = new List<int>();
            enemiesKilledForQuest = new List<int>();
            itemNames = new List<string>();
            enemyNames = new List<string>();
            specialConditions = new Dictionary<string, bool>();

            rewardObjects = new List<object>();

        }

        public Quest() { }

        public virtual void UpdateQuest()
        {
            
        }

        /// <summary>
        /// Adds rewards to the player's inventory
        /// </summary>
        public virtual void RewardPlayer()
        {

            for (int i = 0; i < rewardObjects.Count; i++)
            {
                if (rewardObjects[i] is Equipment)
                {
                    if(rewardObjects[i] is Money)
                        Game1.Player.Money += (rewardObjects[i] as Money).Amount;
                    if (rewardObjects[i] is Experience)
                        Game1.Player.Experience += (rewardObjects[i] as Experience).Amount;
                    if (rewardObjects[i] is Karma)
                        Game1.Player.Karma += (rewardObjects[i] as Karma).Amount;

                    if (rewardObjects[i] is Weapon)
                        Game1.Player.AddWeaponToInventory(rewardObjects[i] as Weapon);
                    if (rewardObjects[i] is Hoodie)
                        Game1.Player.AddShirtToInventory(rewardObjects[i] as Hoodie);
                    if (rewardObjects[i] is Hat)
                        Game1.Player.AddHatToInventory(rewardObjects[i] as Hat);
                    if (rewardObjects[i] is Accessory)
                        Game1.Player.AddAccessoryToInventory(rewardObjects[i] as Accessory);
                }
                else if (rewardObjects[i] is Collectible)
                {
                    if (rewardObjects[i] is BronzeKey)
                        Game1.Player.BronzeKeys++;
                    if (rewardObjects[i] is SilverKey)
                        Game1.Player.SilverKeys++;
                    if (rewardObjects[i] is GoldKey)
                        Game1.Player.GoldKeys++;
                    if (rewardObjects[i] is Textbook)
                        Game1.Player.Textbooks++;
                }
                else if (rewardObjects[i] is StoryItem)
                {
                    if (Game1.Player.StoryItems.ContainsKey((rewardObjects[i] as StoryItem).Name))
                        Game1.Player.StoryItems[(rewardObjects[i] as StoryItem).Name]++;
                    else
                        Game1.Player.StoryItems.Add((rewardObjects[i] as StoryItem).Name, 1);

                    (rewardObjects[i] as StoryItem).PickedUp = true;
                }
            }
            
            if (!storyQuest)
            {
                Game1.currentChapter.CompletedSideQuests.Add(QuestName, this);
                Chapter.effectsManager.secondNotificationQueue.Enqueue(new JournalUpdateNotification(3, this));

                //Add a 'false' to the list of side quests that are read in the journal. When the player looks at the entry, it will be marked as true
                if (Game1.g.chapterState == Game1.ChapterState.prologue)
                {
                    Game1.g.Notebook.Journal.prologueSideQuestsRead.Add(false);
                }
                if (Game1.g.chapterState == Game1.ChapterState.chapterOne)
                {
                    Game1.g.Notebook.Journal.chOneSideQuestsRead.Add(false);
                }
                if (Game1.g.chapterState == Game1.ChapterState.chapterTwo)
                {
                    Game1.g.Notebook.Journal.chTwoSideQuestsRead.Add(false);
                }
            }
            else
            {
                Game1.currentChapter.CompletedStoryQuests.Add(QuestName, this);
                Chapter.effectsManager.secondNotificationQueue.Enqueue(new JournalUpdateNotification(2, this));

                //Add a 'false' to the list of story quests that are read in the journal. When the player looks at the entry, it will be marked as true
                if (Game1.g.chapterState == Game1.ChapterState.prologue)
                {
                    Game1.g.Notebook.Journal.prologueStoryQuestsRead.Add(false);
                }
                if (Game1.g.chapterState == Game1.ChapterState.chapterOne)
                {
                    Game1.g.Notebook.Journal.chOneStoryQuestsRead.Add(false);
                }
                if (Game1.g.chapterState == Game1.ChapterState.chapterTwo)
                {
                    Game1.g.Notebook.Journal.chTwoStoryQuestsRead.Add(false);
                }
            }
        }

    }
}
