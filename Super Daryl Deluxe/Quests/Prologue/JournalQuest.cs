using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class JournalQuest : Quest
    {
        Boolean viewedJournal = false;

        public Boolean ViewedJournal { get { return viewedJournal; } set { viewedJournal = value; } }

        public JournalQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("...And who are you supposed to be?");
            questDialogue.Add("You must be the worst excuse for a 'Dwarves and Druids' player that I've ever seen. " + 
                              "Don't you know the number one rule? Of course you don't. Well I'll tell you: It's stay organized.");
            questDialogue.Add("How will you know what your current story quest is if you don't keep a record of it in " +
                              "your journal? Sure we have a small section on our main sheet that holds all of our side quests," +
                              "but we need information on our main quest, too.");
            questDialogue.Add("And I'm sure you haven't been keeping a record of all of the quests you've done thus far..." +
                              "like the unskilled moron that you are.");
            questDialogue.Add("Why don't you go ahead and take a look at your Journal, maybe you can actually start keeping" +
                              "a useful record of what you've done. Of course you'll never be as organized as I am.");
            questDialogue.Add("Get organized and then we'll talk! I refuse to converse with disorganized peasants.");
            completedDialogue.Add("Feeling a little better?");
            completedDialogue.Add("Just try to remember how important that Journal is. If you forget what quest you're doing " +
                                  "just give it a quick peek. Maybe one day you'll be as organized as me.");
            completedDialogue.Add("Pff...yeah right.");
            rewardObjects.Add(new Karma(1));

            specialConditions.Add("View a Completed Quest in your Journal", false);
            specialConditions.Add("View a Current Quest in your Quests Page", false);

            questName = "Learning about your Journal";

            npcName = "Journal Instructor";

            conditionsToComplete = "View the Story and Quest pages in your Notebook.";

            rewards = "1 Karma";

            taskForQuestsPage = "View the Quests Page and Journal in your Notebook.";

            descriptionForJournal = "A weird kid dressed like an elf taught you how to stay organized on your adventures by keeping a detailed journey of all of your quests! Upon checking, you realized you were already doing it. How convenient!";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.Prologue.PrologueBooleans["firstJournalStoryQuest"] == false || Game1.g.Prologue.PrologueBooleans["firstJournalSideQuest"] == false)
                specialConditions["View a Completed Quest in your Journal"] = true;
            if (Game1.g.Prologue.PrologueBooleans["firstQuestPageQuestCheck"] == false)
                specialConditions["View a Current Quest in your Quests Page"] = true;

            if (specialConditions.ElementAt(0).Value == true && specialConditions.ElementAt(1).Value == true)
                completedQuest = true;

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

