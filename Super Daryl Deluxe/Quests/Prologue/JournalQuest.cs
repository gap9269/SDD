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
            questDialogue.Add("...You have got to be kidding me.");
            questDialogue.Add("You must be the worst 'Dwarves and Druids' player I've ever seen. Wandering around without a clue... You're completely oblivious of the Number One Rule: Keep your quests organized.");
            questDialogue.Add("Do you even know what you're supposed to be doing right now? If you even care about D&D, you'd know to check your Quests Page to find out your current Story Quest.");
            questDialogue.Add("If you weren't a complete slob, you might know to keep track of all your Side Quests, too. All the information you could possibly need to complete your quests is right there in your Quest Page.");
            questDialogue.Add("Do you even realize you can add quests your Quest Helper to keep updated on 5 quests?! You literally only have to checkmark a box...Pff. You probably can't even figure out how to use a pencil.");
            questDialogue.Add("I bet you haven't even kept a Journal of all your quests so far... like the unskilled moron that you are. ");
            questDialogue.Add("Check out your Quest Page now. And while you're at it, open your Journal and start keeping track of everything you've done. It's people like you that make D&D so frustrating to play. Nobody knows what they're doing...");
            questDialogue.Add("Get organized and then we'll talk! I refuse to converse with disorganized peasants.");
            completedDialogue.Add("Feeling a little better?");
            completedDialogue.Add("Just try to remember how important your Quests Page is. If you forget what quest you're doing, just give it a quick peek. Maybe one day you'll be as organized as me.");
            completedDialogue.Add("Pff...yeah right.");
            rewardObjects.Add(new Karma(1));

            specialConditions.Add("View a Completed Quest in your Journal", false);
            specialConditions.Add("View a Current Quest in your Quests Page", false);

            questName = "Learning about your Journal";

            npcName = "Journal Instructor";

            conditionsToComplete = "View the Journal and Quest pages in your Notebook.";

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

