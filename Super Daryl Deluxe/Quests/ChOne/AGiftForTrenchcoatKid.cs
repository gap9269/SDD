using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class AGiftForTrenchcoatKid : Quest
    {
        public AGiftForTrenchcoatKid(Boolean story)
            : base(story)
        {
            questDialogue.Add("\"A Gift for Trenchcoat Kid\"");
            questDialogue.Add("Trenchcoat Kid thinks he's the school's greatest salesman just because he has hundreds of his goons in every dark corner peddling their low quality goods to every mouth breather that glances at them. Lucky for us he hasn't taken into account our secret weapon: you.");
            questDialogue.Add("Trenchcoat Kid has instructed his employees to purchase pretty much anything that these filthy students try to sell. They never give a good price for what you're selling, and you'll definitely walk away feeling some sort of ripped off, but we can use that to our advantage.");
            questDialogue.Add("Our spies have told us that at the end of the day all of the Trenchcoats meet up and bring their day's earnings and purchases to the Trenchcoat Kid himself. What we want you to do is something that we have never been capable of doing ourselves.");
            questDialogue.Add("Go get a bunch of bat droppings from the vents and sell them to one of the Trenchcoats.");
            questDialogue.Add("Those morons will buy it right off of you, no questions asked, and then turn it over to Trenchcoat Kid at the end of the day. It's sure to smell really bad, and it will probably even keep any customers away from the Trenchcoat you choose all day.");
            questDialogue.Add("As payment, you get to keep whatever profit you sell the bat crap for. That's some incentive to get as much as your hands can carry, eh? Don't let us down.");
            questDialogue.Add("P.S: Don't you dare think about accepting this task and then cancelling it.");
            completedDialogue.Add("Task complete.");

            rewardObjects.Add(new Experience(65));
            rewardObjects.Add(new Karma(1));

            itemNames.Add("Guano");
            itemsToGather.Add(1);

            npcName = "Employee Task List";

            specialConditions.Add("", false);
            specialConditions.Add("Sell the Guano to a Trenchcoat Employee", false);

            questName = "A Gift for Trenchcoat Kid";

            conditionsToComplete = "Collect Guano and sell it to a Trenchcoat Employee.";

            taskForQuestsPage = "Collect Guano and sell it to a Trenchcoat Employee.";

            rewards = "1 Karma\n35 Experience";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
            List<Boolean> completedParts = new List<bool>();

            for (int i = 0; i < itemNames.Count; i++)
            {
                if ((Game1.Player.EnemyDrops.ContainsKey(itemNames[i]) && Game1.Player.EnemyDrops[itemNames[i]] >= itemsToGather[i] ||
                    Game1.Player.StoryItems.ContainsKey(itemNames[i]) && Game1.Player.StoryItems[itemNames[i]] >= itemsToGather[i]) ||
                    Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
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
                {
                    if (Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
                        completedQuest = true;
                }
            }
            if (Game1.g.ChapterOne.ChapterOneBooleans["soldGuanoToTrenchcoat"])
                specialConditions["Sell the Guano to a Trenchcoat Employee"] = true;

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.g.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Sharp Comments"]);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
            Chapter.effectsManager.notificationQueue.Enqueue(new NewSkillsUnlockedNotification());
        }
    }
}

