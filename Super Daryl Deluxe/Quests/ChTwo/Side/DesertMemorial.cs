using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class DesertMemorial : Quest
    {
        public DesertMemorial(Boolean story)
            : base(story)
        {
            questDialogue.Add("*sniff* Oh, young man! Please, stay a moment and hear this poor old gardener's tale...");
            questDialogue.Add("Would you believe only moments ago that I had my heart ripped out right from my chest?");
            questDialogue.Add("It's true! It was taken away along with my only friend in this barren desert - my dear little monkey, my sweet Pepy.");
            questDialogue.Add("I keep a garden in the next room you see, a beautiful garden full of flowers for my sweet Pepy to play in. Pepy loved flowers.");
            questDialogue.Add("It was only a day or two ago when the wall collapsed and large, attractive plants invaded my innocent garden. I was too late to notice, and before I could stop it a fierce buzzard had swooped down and swept my dear Pepy away! *sniff*");
            questDialogue.Add("Now it is too dangerous for me to tend to my garden, and I can't even make a memorial for my poor best friend. There is nothing that Pepy loved more than cactus flowers. Could you perhaps help mend an old lady's heart and get some from my garden?");
            questDialogue.Add("*sniff* My poor, poor Pepy. I hope a beautiful flower memorial can put my heart at ease.");
            completedDialogue.Add("Thank you, young man. I insist that you take this, in memory of Pepy...");

            rewardObjects.Add(new MonkeysPaw());
            rewardObjects.Add(new Experience(405));
            rewardObjects.Add(new Karma(4));

            npcName = "The Pyramid Gardener";

            itemNames.Add("Cactus Flower");
            itemsToGather.Add(9);

            questName = "A Desert Memorial";

            conditionsToComplete = "Find 9 Cactus Flowers for Pepy's memorial.";

            taskForQuestsPage = "Find 9 Cactus Flowers for Pepy's memorial.";

            descriptionForJournal = "Description pending";
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

            Game1.Player.RemoveDrops("Cactus Flower", 9);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

