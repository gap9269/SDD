using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class ProtectTITS : Quest
    {
        public ProtectTITS(Boolean story)
            : base(story)
        {
            questDialogue.Add("Alright, alright. I see a solution. I'll simply take control of that boy's mind and use him to fight the apparitions off while we catch your Lock Ghost.");
            questDialogue.Add("Relax mortal, I'm now entering your mind. The Ghost Sucker has a nuclear core, protect it from these ghosts. It is your purpose.");
            questDialogue.Add("This dialogue will never be shown.");

            questName = "Ghost Suckers";

            rewardObjects.Add(new Textbook());

            npcName = "Claire Voyant";

            specialConditions.Add("Protect the Ghost Sucker", false);

            conditionsToComplete = "Protect the Ghost Sucker from the horde of Tuba Ghosts!";

            descriptionForJournal = "Pending";

            taskForQuestsPage = "Protect the Ghost Sucker from the horde of Tuba Ghosts!";

            rewards = "1 Textbook";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (completedQuest)
            {
                specialConditions["Protect the Ghost Sucker"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
