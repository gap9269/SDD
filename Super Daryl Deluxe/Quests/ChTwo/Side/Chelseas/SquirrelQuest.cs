using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SquirrelQuest : Quest
    {
        public SquirrelQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh g-god please help me!");
            questDialogue.Add("I was j-just outside enjoying the p-p-party and some lunatic with a gun attacked me!");
            questDialogue.Add("I barely got away by climbing up into this tree. Oh g-god I think he's s-still outside...");
            questDialogue.Add("Listen, if I go outside he's going to k-k-kill me. But I'm going to have a p-panic attack if I \ndon't calm down. Good you get me some food from the party so I can relax a little? I'd really appreciate it!");
            questDialogue.Add("Please...help me.");
            completedDialogue.Add("Thank you so much! Nuts are my favorite.");

            rewardObjects.Add(new Experience(100));
            rewardObjects.Add(new Karma(2));
            rewardObjects.Add(new Textbook());

            itemNames.Add("Assorted Nuts");
            itemsToGather.Add(1);

            questName = "Creepy Squirrel Boy";

            conditionsToComplete = "Find some food for that weird squirrel kid.";

            rewards = "(1) Textbook\n\n- 2 Karma\n\n- 100 Experience";

            descriptionForJournal = "A student asked you to find a bunch of broken glass \nfor him to take home to his father, who seems to pay \nhis son for" + " each piece he brings him. I'm not sure what the \n'Glass Business' even is. \n\nRewards:\n" + rewards;
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

            Game1.Player.RemoveStoryItem("Assorted Nuts", 1);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

