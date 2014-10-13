using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class FindingCoal : Quest
    {
        public FindingCoal(Boolean story)
            : base(story)
        {
            questDialogue.Add("We're not starving anymore, and my men thank you. But we're still stuck down here until we finish" +
                "\nheating the school.");
            questDialogue.Add("We can't go wandering around to the coal storage because of all of the rabid bats in the vents, and word has it that" +
                "\nall of it has been stolen by the critters that inhabit this godforsaken place anyway.");
            questDialogue.Add("Could you do us one more favor, sonny? Exterminate those critters, and fetch us a bunch of the coal they took.");
            questDialogue.Add("Can you do that?");
            questDialogue.Add("Lord I hope I can return home soon. I miss my children.");
            completedDialogue.Add("Oh thank you, sonny! You've saved an old man's life!");

            rewardObjects.Add(new Money(3));
            rewardObjects.Add(new Experience(50));
            rewardObjects.Add(new Karma(1));
            rewardObjects.Add(new CoalShovel());

            enemyNames.Add("Bat");
            enemyNames.Add("Rat");
            enemiesToKill.Add(25);
            enemiesKilledForQuest.Add(0);
            enemiesToKill.Add(25);
            enemiesKilledForQuest.Add(0);
            itemNames.Add("Coal");
            itemsToGather.Add(10);

            questName = "Finding Coal";

            conditionsToComplete = "Defeat 20 Bats and 20 Rats, and find 10 pieces of Coal for the Furnace Worker.";

            rewards = "- (1) Coal Shovel\n\n - $3.00\n\n - 1 Karma\n\n- 50 Experience";

            descriptionForJournal = "The Furnace worker and his men can't leave the vents until they finish the job given to them. He had you exterminate the creatures in the vents and retrieve the coal that they had been stealing. \n\nRewards:\n" + rewards;
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            List<Boolean> completedParts = new List<bool>();

            for (int i = 0; i < enemyNames.Count; i++)
            {
                if (enemiesKilledForQuest[i] >= enemiesToKill[i])
                {
                    completedParts.Add(true);
                }
                else
                    completedParts.Add(false);
            }

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

            Game1.Player.RemoveStoryItem("Coal", 10);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
