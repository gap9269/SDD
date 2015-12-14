using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class CoalQuest : Quest
    {
        public CoalQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("Eh? ...Wuh");
            questDialogue.Add("...coal. YOU. Do you understand how important this job is? No? Yes, of course, of course.");
            questDialogue.Add("It's been years since you left, you know. Just me and the shovel. And the coal. So much coal.");
            questDialogue.Add("...");
            questDialogue.Add("But, no, no the coal has left me, too. All gone. It needs more coal to keep the ones above warm. Always warm, but with no coal, it freezes.");
            questDialogue.Add("Does coal freeze? Where have you been? It's been so long. Did you take the coal, too? Get more. That was always your job. Get more coal so we can go home.");
            questDialogue.Add("Is it over? Will the coal stay gone?");
            completedDialogue.Add("Nnnnh. The warmth is back.");

            rewardObjects.Add(new CoalShovel());
            rewardObjects.Add(new Experience(105));
            rewardObjects.Add(new Karma(2));

            itemNames.Add("Coal");
            itemsToGather.Add(15);

            npcName = "Furnace Man";

            questName = "Man in the Furnace";

            conditionsToComplete = "Collect 15 pieces of coal for the Furnace Man.";

            taskForQuestsPage = "Collect 15 pieces of coal for the Furnace Man.";

            rewards = "Coal Shovel\n1 Karma\n35 Experience";

            descriptionForJournal = "A crazy furnace man seems to have been stuck in the vents for decades because he ran out of coal to keep the school warm. You did him a favor and collected a whopping 15 pieces of coal. I'm sure that's enough to let him return home to his family.";
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

            Game1.Player.RemoveStoryItem("Coal", 15);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

