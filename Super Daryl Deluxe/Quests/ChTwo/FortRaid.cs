using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class FortRaid : Quest
    {
        public FortRaid(Boolean story)
            : base(story)
        {
            questDialogue.Add("Ze horse iss ready!");
            questDialogue.Add("We're going to blow ziss thing as close to ze enemy barracks as we can.");
            questDialogue.Add("Start by opening zat gate!");
            questDialogue.Add("Get zat gate open!");
            completedDialogue.Add("xgdfhsfgh");

            questName = "Fort Raid";

            specialConditions.Add("Follow Napoleon's orders to destroy the\nenemy's camp", false);

            conditionsToComplete = "Follow Napoleon's orders to destroy the enemy's camp.";

            npcName = "Napoleon";

            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (completedQuest)
            {
                specialConditions["Destroy the enemy camp"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}
