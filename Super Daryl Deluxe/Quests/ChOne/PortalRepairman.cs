using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class PortalRepairman : Quest
    {
        public PortalRepairman(Boolean story)
            : base(story)
        {
            questDialogue.Add("Why, hello there! I'm the Portal Repair Specialist, and it's my job to find someone to repair these portals. I'm sure as hell not going in there.");
            questDialogue.Add("I can tell you appreciate knowledge though, so I'll share with you what I can.");
            questDialogue.Add("I'm afraid we aren't entirely sure what these tears are or what causes them. They've been showing up in all of the classrooms since we modified them, and I can only assume that it's a result of some sort of instability in the dimension software.");
            questDialogue.Add("What we do know is that each one is fundamentally different in nature. Take this one for example. It would seem that it is an even mixture of the Science room and this room.");
            questDialogue.Add("If I had to guess, I'd say there are probably a whole bunch of nasty Science and Music-related creatures inside. But my job isn't to guess, it's to get someone else to close these portals up nice and tight.");
            questDialogue.Add("I've calculated that to close this particular tear you would need to remove all of the critters living inside of it. Unfortunately I doubt it would be very effective after you've been inside for more than a minute and a half.");
            questDialogue.Add("It won't collapse on you or anything silly like that, no. It will most likely just violently spit you out. Yep, that's science. Good ol' physics.");
            questDialogue.Add("So, how about it? Nothing like traveling through wormholes in the name of doing my job for me.");
            questDialogue.Add("Remember: You have 90 seconds to exterminate all life inside.");
            
            completedDialogue.Add("How peculiar. The tear closed itself as soon as you came through it. Well done!");
            completedDialogue.Add("This will assuredly require more research... and more importantly more of you closing the portals throughout the classrooms for me.");
            completedDialogue.Add("In return for keeping me from being fired, you can be my lab assistant. We'll get to the bottom of this mystery.");
            completedDialogue.Add("Keep an eye out while for other wormholes. I'll compensate you nicely for each one that you close.");
            completedDialogue.Add("See you at the next one, assistant!");

            rewardObjects.Add(new BronzeKey());

            specialConditions.Add("Close the Bridge of Armanhand portal", false);

            npcName = "Portal Repair Specialist";

            questName = "Portal Repairman";

            conditionsToComplete = "Close the Bridge of Armanhand portal";

            taskForQuestsPage = "Close the Bridge of Armanhand portal";

            //rewards = "1 Bronze Key\n20 Experience\n2 Karma";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"])
            {
                specialConditions["Close the Bridge of Armanhand portal"] = true;
                completedQuest = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.g.CurrentChapter.NPCs["Portal Repair Specialist Bridge"].Dialogue.Clear();
            Game1.g.CurrentChapter.NPCs["Portal Repair Specialist Bridge"].Dialogue.Add("Keep an eye out for other wormholes. I'll compensate you nicely for each one that you close.");

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

