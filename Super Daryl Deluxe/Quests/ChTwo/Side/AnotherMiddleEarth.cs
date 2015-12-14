using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class AnotherMiddleEarth : Quest
    {
        public AnotherMiddleEarth(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hey there, new friend! Glad to see you'be been sticking around long enough to run into another one of these creepy-ass portals.");
            questDialogue.Add("Why don't you saddle on up and jump in there for me, huh? I'm sure as hell not going in there.");
            questDialogue.Add("I have to warn you though, this one seems more dangerous.");
            questDialogue.Add("Welp, off you go. By my calculations say you have about two minutes before the portal vomits you back out like bad breakfast. Good luck!");
            questDialogue.Add("Remember: You have two minutes to exterminate all life inside.");
            
            completedDialogue.Add("Well done! You really are shaping up to be a helpful assistant. My job has never been easier.");
            completedDialogue.Add("Of course, here's your reward.");
            completedDialogue.Add("These portals have been showing up with increasing frequency. I'm sure I'll see you soon.");

            rewardObjects.Add(new Karma(2));
            rewardObjects.Add(new Experience(2000));

            specialConditions.Add("Close the Forest of Ents portal", false);

            npcName = "Portal Repair Specialist";

            questName = "Another Middle Earth";

            conditionsToComplete = "Close the Forest of Ents portal";

            taskForQuestsPage = "Close the Forest of Ents portal";

            //rewards = "1 Bronze Key\n20 Experience\n2 Karma";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"])
            {
                specialConditions["Close the Forest of Ents portal"] = true;
                completedQuest = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

