using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SavingChristmasII : Quest
    {
        public SavingChristmasII(Boolean story)
            : base(story)
        {
            questDialogue.Add("I may be free from my prison, but the spirit of Christmas is still dead.");
            questDialogue.Add("Or at the very least, behind schedule.");
            questDialogue.Add("The children of this world should not be made to suffer because people like Scrooge ho ho horde all of my presents for themselves.");
            questDialogue.Add("Go reclaim my stash of presents for the sweet children");
            questDialogue.Add("Something something ho ho ho");
            completedDialogue.Add("Yipee presents");

            rewardObjects.Add(new Coal(true));
            rewardObjects.Add(new Experience(2000));
            rewardObjects.Add(new Karma(2));


            npcName = "Santa Claus";

            itemNames.Add("Haunted Present");
            itemsToGather.Add(30);

            questName = "Saving Christmas II";

            conditionsToComplete = "Gather 30 presents for Santa Claus.";

            taskForQuestsPage = "Gather 30 presents for Santa Claus.";

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

            Game1.Player.RemoveDrops("Haunted Present", 30);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

