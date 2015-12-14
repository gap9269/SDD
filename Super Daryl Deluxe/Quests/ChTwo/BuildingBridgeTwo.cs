using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class BuildingBridgeTwo : Quest
    {
        public BuildingBridgeTwo(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hey again stranger!");
            questDialogue.Add("Looks like this bridge is also busted. If you get me some lumber, I'll fix it up in a jiffy!");
            questDialogue.Add("Deepity derp");
            completedDialogue.Add("Great! I'll have this up and running soon!");

            itemNames.Add("Haunted Lumber");
            itemsToGather.Add(20);

            questName = "Bridge Kid Strikes Again";

            conditionsToComplete = "Get 20 pieces of Haunted Lumber for the bridge kid";

            rewards = "";

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

            Game1.Player.RemoveStoryItem("Haunted Lumber", 20);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

