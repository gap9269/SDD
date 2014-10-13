using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BeerForGoggles : Quest
    {
        public BeerForGoggles(Boolean story)
            : base(story)
        {
            questDialogue.Add("Bro! There are so many babes here!");
            questDialogue.Add("This barn is bumpin', man. Go grab me a brewski would ya? I'll give you these sweet ass glasses if you do.");
            questDialogue.Add("Quick man, these chicks are totally into me!");
            completedDialogue.Add("Awwww yeah bro. This guy's gettin' lucky tonight!");

            rewardObjects.Add(new BeerGoggles());
            rewardObjects.Add(new Experience(20));

            itemNames.Add("Beer");
            itemsToGather.Add(1);

            questName = "Barn Hallucinations";

            conditionsToComplete = "Get Callyn some beer and bring it to him in the barn.";

            rewards = "(1) Beer Goggles\n\n- 50 Experience";

            descriptionForJournal = "A student asked you to find a bunch of broken glass for him to take home to his father, who seems to pay his son for" + " each piece he brings him. I'm not sure what the 'Glass Business' even is.";
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

            Game1.Player.RemoveStoryItem("Beer", 1);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
