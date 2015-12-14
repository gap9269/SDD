using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class PoolesBoy : Quest
    {
        public PoolesBoy(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hello, my dear boy! I did need an extra hand or two around here. Mmm, yes, quite right.");
            questDialogue.Add("This place is freezing! Master Scrooge must have blown out all of the fireplaces again. He claims he sees specters in the flames. Pish posh.");
            questDialogue.Add("Do see if you can't start them up for me, will you? You'll need some firewood of course.");
            questDialogue.Add("I do think you may have missed some, my boy. There is still quite a chill here.");
            completedDialogue.Add("Well done, my boy! I suppose you never caught sight of any ghouls? How absurd.");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(2500));
            rewardObjects.Add(new Karma(3));
            rewardObjects.Add(new Money(120.00f));

            npcName = "Poole";

            specialConditions.Add("Fire Places Lit: 0/5", false);

            questName = "Poole's Boy";

            conditionsToComplete = "Light the fireplaces around Scrooge's mansion.";

            taskForQuestsPage = "Light the fireplaces around Scrooge's mansion.";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            int firePlacesLit = 0;

            for (int i = 0; i < (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Count; i++)
            {
                ScroogeFirePlace fp = ((Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces[i] as ScroogeFirePlace);

                if (fp.Finished)
                    firePlacesLit++;
            }

            specialConditions.Clear();

            if (firePlacesLit == 5)
            {
                completedQuest = true;
                specialConditions.Add("Fire Places Lit: 5/5", true);
            }
            else
            {
                specialConditions.Add("Fire Places Lit: " + firePlacesLit + "/5", false);
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

