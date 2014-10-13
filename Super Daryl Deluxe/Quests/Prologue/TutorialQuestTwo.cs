using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class TutorialQuestTwo : Quest
    {

        public TutorialQuestTwo(Boolean story)
            : base(story)
        {
            questDialogue.Add("Just keep that locker combination in your notebook. I'm sure Tim will want more flowers later on.");
            questDialogue.Add("Come back when you're done. We have more jobs for you to do so you can earn our friendship.");
            questDialogue.Add("Did you do it yet?");
            completedDialogue.Add("Tim didn't see you, right?");

            npcName = "Paul";

            questName = "Flower Delivery";

            taskForQuestsPage = "Plant dandelions in Tim's locker.";

            specialConditions.Add("Plant dandelions in Tim's locker.", false);

            conditionsToComplete = "Break into Tim's locker and plant the dandelions, then go back and talk to Paul and Alan.";
            descriptionForJournal = "Paul and Alan requested that you break into Tim'slocker and spruce it up with flowers. I have a feeling Tim does not, in fact, love flowers. ";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if ((Game1.currentChapter as Prologue).BrokeIntoLocker == true)
            {
                specialConditions["Plant dandelions in Tim's locker."] = true;
                CompletedQuest = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}
