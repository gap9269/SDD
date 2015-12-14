using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class ANewArtisticMedium : Quest
    {
        public ANewArtisticMedium(Boolean story)
            : base(story)
        {
            questDialogue.Add("You! You look like a creative individual. No normal human would ever wear a headband such as that.");
            questDialogue.Add("I have a problem, you see. I want to work with a new medium, a medium that speaks to the masses! A medium that commoditizes itself through other's creation!");
            questDialogue.Add("And if they're not creative enough, it will show them HOW to create!");
            questDialogue.Add("That's right, I need clay dough! And a lot of it!");
            questDialogue.Add("Quickly, scurry off and find me some while I think of other ways to revolutionize the way we create art!");
            questDialogue.Add("Together we, well mostly I, will teach the world how to create!");
            completedDialogue.Add("This is the medium of the television age!");

            rewardObjects.Add(new Beret());
            rewardObjects.Add(new Experience(205));
            rewardObjects.Add(new Karma(2));

            npcName = "Andy Warhol";

            itemNames.Add("Clay Dough");
            itemsToGather.Add(7);

            questName = "A New Artistic Medium";

            conditionsToComplete = "Collect 7 tubes of Clay Dough for Andy Warhol.";

            taskForQuestsPage = "Collect 7 tubes of Clay Dough for Andy Warhol.";

            //rewards = "Coal Shovel\n1 Karma\n35 Experience";

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

            Game1.Player.RemoveDrops("Clay Dough", 7);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

