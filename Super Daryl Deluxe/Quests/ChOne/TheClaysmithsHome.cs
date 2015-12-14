using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class TheClaysmithsHome : Quest
    {
        public TheClaysmithsHome(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh...hello. Welcome to my exotic vase sho-- Ooohhhh...gosh dernit. *sigh*");
            questDialogue.Add("It doesn't matter. After tonight there will be nothing left of my work to sell. Nothing to make a living on.");
            questDialogue.Add("First that short man with his goons shut down the entire shopping and theater district, and that was hard enough on my business! But now I have to deal with a pack of bandits showing up overnight.");
            questDialogue.Add("I just know that they're breaking into my house as we speak and destroying my life's work. I spent years creating those vases you know, exotic, every one of them. *sigh*");
            questDialogue.Add("If only someone would go put an end to those ruffians and their destruction. There's nothing that an old man like myself could do to stop them...");
            questDialogue.Add("*sigh* Oh. You're still here. I'm sorry, but I'm not in the mood to sell anything today. There likely won't be a shop tomorrow anyway, on account of all of my vases being destroyed back home right now...");

            completedDialogue.Add("There's word on the streets of an odd man in bright orange pants fighting off the pack of bandits on my block! Thank you for your help young man, you have done this old man a great kindness!");
            completedDialogue.Add("Please, take my lucky smock. I used to wear it during my younger days and it saved my life more than once. I hope it does the same for you.");

            rewardObjects.Add(new ArtSmock());
            rewardObjects.Add(new Experience(200));
            rewardObjects.Add(new Money(4.50f));

            enemyNames.Add("Fluffles the Bandit");
            enemiesKilledForQuest.Add(0);
            enemiesToKill.Add(10);
            npcName = "Town Claysmith";

            questName = "The Claysmith's Home";

            conditionsToComplete = "Slay a pack of bandit rats to protect the Claysmith's home.";

            taskForQuestsPage = "Slay a pack of bandit rats to protect the Claysmith's home";

            //rewards = "Art Smock\n$20\n2 Karma";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                CompletedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

