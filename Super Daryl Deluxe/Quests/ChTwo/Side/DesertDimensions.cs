using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class DesertDimensions : Quest
    {
        public DesertDimensions(Boolean story)
            : base(story)
        {
            questDialogue.Add("You again, hello there! Looks like we got another one, and this one looks dangerous! I'm sure as hell not going in there.");
            questDialogue.Add("We aren't any closer to figuring our what causes these rifts in time and space, but I guess that's what you get when you mess with alternate dimensions, ha!");
            questDialogue.Add("Well, you're up little buddy. Get on in there and put a stop to it.");
            questDialogue.Add("Oh! Of course, you want to know what you're getting into. Well, let's see...");
            questDialogue.Add("Not too much, it seems to be a mix of the deadlist creatures from this desert and some odd humanoid creatures from a nearby area. Intelligence levels are minimal, but violence levels are very high! I'd die almost instantly if I went in there, no doubt.");
            questDialogue.Add("Welp, off you go. By my calculations say you have about two minutes before the portal vomits you back out like bad breakfast. Good luck!");
            questDialogue.Add("Remember: You have two minutes to exterminate all life inside.");
            
            completedDialogue.Add("Well done! You really are shaping up to be a helpful assistant. My job has never been easier.");
            completedDialogue.Add("Of course, here's your reward.");
            completedDialogue.Add("These portals have been showing up with increasing frequency. I'm sure I'll see you soon.");

            rewardObjects.Add(new Karma(2));
            rewardObjects.Add(new Experience(200));

            specialConditions.Add("Close the Central Sands portal", false);

            npcName = "Portal Repair Specialist";

            questName = "Desert Dimensions";

            conditionsToComplete = "Close the Central Sands portal";

            taskForQuestsPage = "Close the Central Sands portal";

            //rewards = "1 Bronze Key\n20 Experience\n2 Karma";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"])
            {
                specialConditions["Close the Central Sands portal"] = true;
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

