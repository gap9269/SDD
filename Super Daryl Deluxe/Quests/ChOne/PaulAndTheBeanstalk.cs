using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class PaulAndTheBeanstalk : Quest
    {
        public PaulAndTheBeanstalk(Boolean story)
            : base(story)
        {
            questDialogue.Add("\"Paul and the Beanstalk\"");
            questDialogue.Add("A great opportunity has presented itself to us, one that requires you to not mess up.");
            questDialogue.Add("One of our fellow students has approached us asking if we have any Magic Beans in stock. I don't know what's wrong with him, but he did offer to trade us a cow off of his family's farm if we were to trade him a handful of magic legumes.");
            questDialogue.Add("This is an offer that we can't refuse. Think of what we could accomplish if we had a cow!");
            questDialogue.Add("I don't know where to get any beans, especially magically endowed ones, but you'll figure it out. As a reward we'll give you the combination to one of our many storage units around the school. Feel free to take whatever you want from it.");
            questDialogue.Add("PS: The kid works on a farm, he'll probably know it if you just go to the store and buy a can of kidney beans. Besides, we don't paid you. You can't afford to buy anything.");
            completedDialogue.Add("Task Complete.");

            rewardObjects.Add(new LockerCombo(0,0, "Ken", Game1.g));
            rewardObjects.Add(new Experience(165));
            rewardObjects.Add(new Karma(2));

            npcName = "Employee Task List";

            itemNames.Add("Mexican Jumping Beans");
            itemsToGather.Add(10);

            questName = "Paul and the Beanstalk";

            conditionsToComplete = "Collect some magic beans for Paul and Alan.";

            taskForQuestsPage = "Collect 10 mexican jumping beans for Paul and Alan.";

            descriptionForJournal = "Pending";
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
                {
                    completedQuest = false;
                    break;
                }

                if (i == completedParts.Count - 1)
                    completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.Player.RemoveDrops("Mexican Jumping Beans", 10);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

