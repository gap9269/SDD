using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class AGuardsRevenge : Quest
    {
        public AGuardsRevenge(Boolean story)
            : base(story)
        {
            questDialogue.Add("Those ugly dogs!");
            questDialogue.Add("They took the Pharaoh and chained up all of my friends. Kill them all!");
            questDialogue.Add("Or, maybe like, 40 of them!");
            questDialogue.Add("Do this and I'll reward you handsomely.");
            questDialogue.Add("I won't rest until those undead bastards are all dead. And once these chains are off of me.");
            completedDialogue.Add("Well done! Much celebration is warranted!");
            completedDialogue.Add("...How about you get these chains off of me?");

            rewardObjects.Add(new Karma(1));
            rewardObjects.Add(new Money(2.15f));

            enemiesToKill.Add(40);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Anubis Warrior");

            questName = "Pharaoh's Revenge";

            npcName = "Pharaoh Guard";

            taskForQuestsPage = "Slaughter 40 Anubis Warriors";

            conditionsToComplete = "Slaughter 40 Anubis Warriors.";

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