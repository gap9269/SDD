using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class BuildingACornBridge : Quest
    {
        public BuildingACornBridge(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hey stranger!");
            questDialogue.Add("Looks like the bridge is busted. Get me 25 Corn Stalks and I'll repair it for ya.");
            questDialogue.Add("Deepity derp");
            completedDialogue.Add("Nice. I'll have this fixed up in a jiff.");

            itemNames.Add("Corn Stalk");
            itemsToGather.Add(25);

            questName = "Repairing a Bridge";

            conditionsToComplete = "Get 25 corn stalks for the bridge guy";

            rewards = "";

            descriptionForJournal = "A student asked you to find a bunch of broken glass for him to take home to his father, who seems to pay his son for each piece he brings him. I'm not sure what the 'Glass Business' even is.";
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

            Game1.Player.RemoveStoryItem("Corn Stalk", 25);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

