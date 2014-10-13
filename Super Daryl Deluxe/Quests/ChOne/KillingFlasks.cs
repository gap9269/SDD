using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class KillingFlasks : Quest
    {
        public KillingFlasks(Boolean story)
            : base(story)
        {
            questDialogue.Add("You again! I need your help!");
            questDialogue.Add("Mr. Robatto gave me detention for carrying around shards of broken glass.");
            questDialogue.Add("As revenge, I want you to smash all of the equipment in the old science room. I mean, I'd do it, but" +
                "\nI don't want to get another detention.");
            questDialogue.Add("Feeling up to it?");
            questDialogue.Add("Smash that equipment yet?");
            completedDialogue.Add("Thanks again! Here's a bandaid I found stuck to my shoe earlier.");

            //Give accessory as well
            rewardObjects.Add(new Experience(20));

            enemyNames.Add("Erl The Flask");
            enemyNames.Add("Benny Beaker");
            enemiesToKill.Add(10);
            enemiesKilledForQuest.Add(0);
            enemiesToKill.Add(15);
            enemiesKilledForQuest.Add(0);
            questName = "Daryl vs. Benny and Erl";

            conditionsToComplete = "Defeat 10 Erl The Flasks and 15 Benny Beakers.";

            rewards = "- (1) Dirty Bandaid\n\n- 20 Experience";

            descriptionForJournal = "The broken glass kid wanted to take revenge on the \nschool for his detention by smashing a bunch of science equipment." + 
                "\ninstead of doing it himself, he had you do it. Seems like \na dumb revenge anyway. \n\nRewards:\n" + rewards;
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

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

