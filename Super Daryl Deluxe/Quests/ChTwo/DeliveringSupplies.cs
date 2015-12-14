using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class DeliveringSupplies : Quest
    {
        public DeliveringSupplies(Boolean story)
            : base(story)
        {
            questDialogue.Add("While we wait for his highness to arrive, why don't you go and deliver a few supplies to my men at ze new camp at ze Battlefield Outskirts?");
            questDialogue.Add("Zey could use ze help, and I am sure zey will want an update on things here.");
            questDialogue.Add("What is taking zat blasted consel so long?");

            completedDialogue.Add("");

            questName = "Delivering Supplies";

            specialConditions.Add("Deliver supplies to Battlefield Outskirts", false);

            conditionsToComplete = "Deliver supplies to Battlefield Outskirts";

            taskForQuestsPage = "Deliver supplies to Battlefield Outskirts";

            npcName = "Napoleon";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!Game1.Player.StoryItems.ContainsKey("Napoleon's Supplies"))
                Game1.Player.AddStoryItem("Napoleon's Supplies", "some supplies", 1);

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"])
                completedQuest = true;

            if (completedQuest)
            {
                specialConditions["Deliver supplies to Battlefield Outskirts"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
