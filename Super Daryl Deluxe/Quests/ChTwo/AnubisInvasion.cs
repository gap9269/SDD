using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class AnubisInvasion : Quest
    {
        public AnubisInvasion(Boolean story)
            : base(story)
        {
            questDialogue.Add("Ma'at save us! We thought the story of a ravenous warlord in the south was a myth, but it seems that he's found a footing in our sacred land as well.");
            questDialogue.Add("Anubis is not a violent god and he certainly does not dwell in the matters of mortals. Somehow this tyrant has gotten control of the undead armies. How else would he kidnap our beloved Pharaoh?");
            questDialogue.Add("They must be planning to ransack the Great Pyramid of Bavarius. An amazing treasure lies at its center, but only our Pharaoh Cleopatra can open the treasury's doors.");
            questDialogue.Add("Pale one, you must save our Pharaoh and release the Great Pyramid from the warlord's grasp!");
            questDialogue.Add("Go! Go save Cleopatra! We'll stand guard here!");

            completedDialogue.Add("xgdfhsfgh");

            questName = "Anubis Invasion";

            specialConditions.Add("Chase Anubis' force from the Great Pyramid", false);
            specialConditions.Add("Save Cleopatra", false);

            conditionsToComplete = "Chase Anubis' force from the Great Pyramid and save Cleopatra";

            taskForQuestsPage = "Chase Anubis' force from the Great Pyramid and save Cleopatra";

            npcName = "Pharaoh Guard";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            Game1.g.ChapterTwo.ChapterTwoBooleans["savedPharaohGuards"] = true;

            if (completedQuest)
            {
                specialConditions["Chase Anubis' force from the Great Pyramid"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
