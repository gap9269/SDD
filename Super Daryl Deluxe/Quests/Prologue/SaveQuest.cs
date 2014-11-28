using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class SaveQuest : Quest
    {
        public SaveQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("You there! Wanderer!");
            questDialogue.Add("You look like someone who seeks out excitement and lives life in the danger zone.");
            questDialogue.Add("I know my kind when I see it! Dangerous souls like us must stick together, and we sure as heck need to obey the Number One Rule of 'Dwarves and Druids': Always save!");
            questDialogue.Add("I bet you're wetting yourself with excitement right now. I know I am!");
            questDialogue.Add("Talk to me when you're ready to hear the secrets of saving progress, Wanderer.");
            questDialogue.Add("");
            completedDialogue.Add("Alright, so here's what you do: Every time you find yourself in a dangerous spot  and nature calls, just find the nearest bathroom and head to that stall over there to update all your progress and stats in your 'Dwarves and Druids' handbook.");
            completedDialogue.Add("If you lose all your health, the Game Master makes you start over from your last save. It's super lame. I make sure to save all the time. Listen to me, and you'll never quit the game in a blind rage again.");
            completedDialogue.Add("Happy saving, Wanderer.");

            rewardObjects.Add(new Karma(1));

            npcName = "Save Instructor";

            specialConditions.Add("Learn about saving.", false);

            questName = "Learning about Saving";

            conditionsToComplete = "Talk to the D&D player again to learn about saving.";

            taskForQuestsPage = "Learn about Saving";

            rewards = "1 Karma";

            descriptionForJournal = "You found Goku hanging out in the Bathroom, where he violated the Number One Rule for bathrooms: you never speak to strangers in a bathroom. Of course he -did- explain to you how to save your progress, so I guess there's that.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

                completedQuest = true;

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
