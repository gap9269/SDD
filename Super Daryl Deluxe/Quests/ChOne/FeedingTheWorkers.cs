using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class FeedingTheWorkers : Quest
    {
        public FeedingTheWorkers(Boolean story)
            : base(story)
        {
            questDialogue.Add("The lord has mercy!");
            questDialogue.Add("My men and I have been stuck working this furnace for days without break. We've run out of food rations" +
                "\nand some of my men are growing weak.");
            questDialogue.Add("Please sonny, go find us some food, eh? I found an old textbook lying around here that you can have in" +
                "\nexchange for it. It's not much, but it's all we have down here. We've even run out of coal...");
            questDialogue.Add("Please sonny, we're so hungry...");
            completedDialogue.Add("Praise you, sonny! Here's that textbook that I promised you. I hope it helps your education.");

            rewardObjects.Add(new Experience(35));
            rewardObjects.Add(new Karma(1));
            rewardObjects.Add(new Textbook());

            itemNames.Add("Half Eaten Cheese");
            itemsToGather.Add(20);

            questName = "Feeding the Workers";

            conditionsToComplete = "Collect 20 pieces of Half Eaten Cheese for the Furnace worker.";

            rewards = "- (1) Textbook\n\n- 1 Karma\n\n- 35 Experience";

            descriptionForJournal = "A starving worker in the depths of the vents asked you to collect food for him and his hungry workers. \n\nRewards:\n" + rewards;
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
            List<Boolean> completedParts = new List<bool>();

            for (int i = 0; i < itemNames.Count; i++)
            {
                if (Game1.Player.EnemyDrops.ContainsKey(itemNames[i]) && Game1.Player.EnemyDrops[itemNames[i]] >= itemsToGather[i] ||
                    Game1.Player.StoryItems.ContainsKey(itemNames[i]) && Game1.Player.StoryItems[itemNames[i]] >= itemsToGather[i])
                {
                    completedParts.Add(true);
                }
                else
                    completedParts.Add(false);
            }

            for (int i = 0; i < completedParts.Count; i++)
            {
                if (completedParts[i] == false)
                    break;

                if (i == completedParts.Count - 1)
                    completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.Player.RemoveStoryItem("Half Eaten Cheese", 20);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

