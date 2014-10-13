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
            questDialogue.Add("Well it just so happens that I'm the exact same way! Dangerous souls like us need to stick together " +
                              "and they sure as heck need to obey the Number One Rule of 'Dwarves and Druids': Always save!");
            questDialogue.Add("I bet you're wetting yourself with excitement right now. It sure looks like it.");
            questDialogue.Add("Talk to me when you're ready to hear the secrets of saving progress, Wanderer.");
            questDialogue.Add("");
            completedDialogue.Add("Alright, so here's what you do: Every time you find yourself in super dangerous place or situation " + 
                                  " and nature calls, just go to that stall over there and make sure you record all of your progress and current stats" +
                                  " in your 'Dwarves and Druids' handbook.");
            completedDialogue.Add("It's important, you know! If you lose all of your health the Game Master will make you start from the last time" +
                                  " you saved. It's super lame, so I make sure to save all the time. Listen to me and you'll never find yourself quitting the game in a blind rage.");
            completedDialogue.Add("Happy saving, Wanderer.");

            rewardObjects.Add(new Karma(1));

            npcName = "Save Instructor";

            specialConditions.Add("Learn about saving.", false);

            questName = "Learning about Saving";

            conditionsToComplete = "Talk to the D&D player again to learn about saving.";

            taskForQuestsPage = "Learn about Saving";

            rewards = "1 Karma";

            descriptionForJournal = "You found Goku hanging out in the Bathroom, where he violated the Number One Rule for bathrooms: you never speak to strangers in a bathroom. Of course he DID explain to you how to save your progress, so I guess there's that.";
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
