using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class FieryFlora : Quest
    {
        public FieryFlora(Boolean story)
            : base(story)
        {
            questDialogue.Add("Whoa, don't get to close. See that over there? That's an explosive flower.");
            questDialogue.Add("They're pretty abundant 'round these parts. And mighty dangerous. Just a drop of liquid will set it off and send its surroundings sky high, so don't go sweatin' all over it.");
            questDialogue.Add("What're you starin' at me all dumb for? Think I'm pullin' your leg, orange legs?");
            questDialogue.Add("Go ahead, find some water and dump it on the flower over there and see what happens. I'm not gettin' any closer though, that's for sure.");
            questDialogue.Add("Havin' trouble finding water? Just as well. With these things growin' everywhere I wouldn't want water to be anywhere near here.");
            completedDialogue.Add("Told you so. Careful around here, these things are everywhere.");

            rewardObjects.Add(new Experience(405));
            rewardObjects.Add(new Karma(4));

            npcName = "Henry Horus";

            specialConditions.Add("Find water", false);
            specialConditions.Add("Ignite the exploding flower", false);

            questName = "Fiery Flora";

            conditionsToComplete = "Find water to ignite the explosive flower.";

            taskForQuestsPage = "Find water to ignite the explosive flower.";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["flowerWallBlown"])
            {
                specialConditions["Ignite the exploding flower"] = true;
                completedQuest = true;
            }
            if(Game1.Player.StoryItems.ContainsKey("Pyramid Water"))
            {
                specialConditions["Find water"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

