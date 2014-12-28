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
            questDialogue.Add("The plan is as follows: We open the gate, clear the camp of enemy scum, and roll our big wooden friend here inside.");
            questDialogue.Add("We're going to blow this thing as close to the enemy barracks as we can, but we'll need to clear a path for her. Just follow my orders.");
            questDialogue.Add("Start by opening that gate!");
            questDialogue.Add("Get that gate open!");
            completedDialogue.Add("xgdfhsfgh");

            questName = "Fort Raid";

            specialConditions.Add("Follow Napoleon's orders to destroy the\nenemy's camp", false);

            conditionsToComplete = "Follow Napoleon's orders to destroy the enemy's camp.";

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
