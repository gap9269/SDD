using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class PackageForMrScrooge : Quest
    {
        public PackageForMrScrooge(Boolean story)
            : base(story)
        {
            questDialogue.Add("Package for Mr. Scrooge!");
            questDialogue.Add("You must be under his employ. Surprisin', he normally don't spend his money much. Coo.");
            questDialogue.Add("He usually pays me in walnuts. Works for ol' Gary, it does. You cough up the walnuts, I cough up the package. Capisce?");
            questDialogue.Add("You got my payment yet? Coo.");
            completedDialogue.Add("Pleasure doin' bidness.");

            rewardObjects.Add(new OldBattery());
            rewardObjects.Add(new Experience(405));
            rewardObjects.Add(new Karma(4));

            npcName = "Gary R. Pigeon";

            itemNames.Add("Haunted Walnuts");
            itemsToGather.Add(15);

            questName = "Package for Mr. Scrooge";

            conditionsToComplete = "Pay Gary the Pigeon in walnuts for his delivery services.";

            taskForQuestsPage = "Pay Gary the Pigeon in walnuts for his delivery services.";

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

            Game1.Player.RemoveDrops("Haunted Walnuts", 15);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

