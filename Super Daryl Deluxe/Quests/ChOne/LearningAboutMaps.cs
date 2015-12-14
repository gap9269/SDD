using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class LearningAboutMaps : Quest
    {
        public LearningAboutMaps(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hello there, kind explorer. I am the Magister of Maps.");
            questDialogue.Add("This school may be large and full of secrets, but a true Dwarves and Druids player uses their maps to ensure they never get lost.");
            questDialogue.Add("It's the Number One Rule.");
            questDialogue.Add("Why don't you go ahead and open up the \'Maps\' section of your Notebook right now and take a look? Talk to me afterward and I'll explain further.");
            questDialogue.Add("Go ahead, it won't do you any good to stay lost forever.");
            completedDialogue.Add("See how useful maps are? They keep track of a lot of stuff...like what maps have quests, shops, lockers, and more.");
            completedDialogue.Add("If you ever need to get somewhere and you forgot exactly where it was, just check our your \'Maps\' page. It even updates when you discover new areas.");
            completedDialogue.Add("In fact, why don't you go explore a new area right now? The Master of the Quests told me that a new dungeon opened Upstairs, when the vent grate fell off.");
            completedDialogue.Add("It probably doesn't lead anywhere cool, probably just the janitor's closet or something. But we all have to start somewhere.");

            rewardObjects.Add(new Karma(3));

            specialConditions.Add("Open up your \'Maps\' Page", false);

            npcName = "The Magister of Maps";

            questName = "Learning about Maps";

            taskForQuestsPage = "Learn about Maps";

            conditionsToComplete = "Open up your \'Maps\' page";

            rewards = "3 Karma";

            descriptionForJournal = "A young explorer taught you about maps.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["openedMaps"])
            {
                completedQuest = true;
                specialConditions["Open up your \'Maps\' Page"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

