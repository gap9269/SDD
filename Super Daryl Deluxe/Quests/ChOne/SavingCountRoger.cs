using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class SavingCountRoger : Quest
    {
        public SavingCountRoger(Boolean story)
            : base(story)
        {
            questDialogue.Add("*Hisssssssss* Who dares disturb the magnificent Count Roger!");
            questDialogue.Add("Alas, it be but a mere mortal. But wait! A mere mortal could never find Count Roger in his fortress of ventilation unaided by dark magic...Perhaps you can help Count Roger?");
            questDialogue.Add("Ay, there exists within this dark realm many who would rather see Count Roger dead! Vile creatures who will stop at nothing to see this cursed soul expelled from this plane for all eternity!");
            questDialogue.Add("Yes, bats! I knew you would understand. Those damned bats are out to get me, I tell you.");
            questDialogue.Add("Slaughtering fifteen of those demons should give them the message...the message to never trifle with the mighty Count Roger! AHAHAHAHA!");
            questDialogue.Add("What do you say, mortal? Are you willing to lend your soul to an eternity of slavery beneath Count Roger?");
            questDialogue.Add("What happened, mortal? This realm still reeks of the weak and desperate. You have not fulfilled the task Count Roger has assigned you.");
            completedDialogue.Add("Count Roger is safe at last from those pathetic conspirators. He is pleased with your servitude. Prepare for a handsome reward courtesy of...");
            completedDialogue.Add("Count Roger! AHAHAHAHA!");

            rewardObjects.Add(new CapeOfCountRoger());
            rewardObjects.Add(new Karma(1));
            rewardObjects.Add(new Money(2.15f));

            enemiesToKill.Add(15);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Vent Bat");

            questName = "Saving Count Roger";

            npcName = "Count Roger";

            taskForQuestsPage = "Exterminate 15 Vent Bats that are out to get Count Roger.";

            conditionsToComplete = "Exterminate 15 Vent Bats that are out to get Count Roger.";

            rewards = "1 Cape of Count Roger\n1 Karma\n$6.66";

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