using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BeerForHat : Quest
    {
        public BeerForHat(Boolean story)
            : base(story)
        {
            questDialogue.Add("There's a weird guy out behind the party that said I'm not allowed back there because it's under construction.");
            questDialogue.Add("Before he kicked me out he offered me dinner. Best chili I've ever had. I could use a beer to wash it down.");
            questDialogue.Add("You should grab one for me.");
            questDialogue.Add("Where's that beer? That chili left a weird taste in my mouth. And I think I have a hair stuck in my throat.");
            completedDialogue.Add("Thanks, man!");

            rewardObjects.Add(new PartyHat());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(20));

            itemNames.Add("Beer");
            itemsToGather.Add(1);

            questName = "A Quest for Beer";

            taskForQuestsPage = "Find Jesse some beer.";
            conditionsToComplete = "Jesse is slightly perturbed that he can't go to his usual place at the party due to the area being under construction. He did, however, score some tasty dinner and he wants you to fetch him a beer to wash it down with.";

            rewards = "1 Textbook\n1 Party Hat\n 20 Experience";

            descriptionForJournal = "In front of the party you found Jesse standing around eagerly with a hungry look in his eyes. He may have only came to party, but he got a hell of a good dinner out of it too, to hear him tell it. He wanted a beer to wash it down. Now that you've gotten him one, you should stay away from him. I don't like the look he was giving you.";
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
