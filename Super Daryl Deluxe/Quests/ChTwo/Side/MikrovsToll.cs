using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class MikrovsToll : Quest
    {
        public MikrovsToll(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hhhrrrrnnumph! No, no no! You want use bridge, you pay!");
            questDialogue.Add("This MIKROV'S bridge! Only Mikrov use it!");
            questDialogue.Add("Mikrov like goblin shiny. Only use bridge when give Mikrov fifteen shinies!");
            questDialogue.Add("Don't make Mikrov hurt you, ugly human!");
            questDialogue.Add("This Mikrov's bridge ONLY! Go away! Human stink!");
            completedDialogue.Add("You pay Mikrov, you use bridge. Goblin shiny help Mikrov pay for bridge upkeep and hill maintenance.");

            rewardObjects.Add(new MikrovsEyePatch());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(485));
            rewardObjects.Add(new Money(3.50f));

            npcName = "Mikrov";

            itemNames.Add("Goblin Gold");
            itemsToGather.Add(15);

            questName = "Mikrov's Toll";

            conditionsToComplete = "Find 15 pouches of Goblin Gold for Mikrov";

            taskForQuestsPage = "Find 15 pouches of Goblin Gold for Mikrov";

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

            Game1.Player.RemoveDrops("Goblin Gold", 15);
            Game1.g.ChapterTwo.ChapterTwoBooleans["bridgeUsable"] = true;

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

