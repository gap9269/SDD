using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class SoldierRepairs : Quest
    {
        public SoldierRepairs(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oooohoho you have come back I see. Or perhaps you were standing here ze whole time? You are quiet as a mouse, perhaps one day I will examine your vocal chords first hand.");
            questDialogue.Add("But for now I have another job for you, oohoho.");
            questDialogue.Add("Ze supplies you brought to me contain a serum zat undoubtedly produces marvelous effects in ze victi-- I mean, ze patient.");
            questDialogue.Add("I suspect zat if you were to administer ze serum to ze men injured during ze attack, it would heal zem in moments. It is unfortunate zat ze imbecile Bonaparte does not allow me to continue my research on creating super soldiers from unwilling participants. Perhaps you would like to volunteer for ze next round of tests?");
            questDialogue.Add("Now get out of here and heal ze soldiers with zis serum before I am tempted longer to begin extracting your less vital organs in favor of extra limbs.");
            questDialogue.Add("Back for a new arm? Perhaps a seond pair of kidneys? Oohoho.");
            completedDialogue.Add("Cutscene time");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(550));
            rewardObjects.Add(new Karma(4));

            npcName = "Dr. Dominique Jean Larrey";

            specialConditions.Add("Use all of the morphine on fallen soldiers", false);

            questName = "Dr. Dominique's Soldier Repairs";

            conditionsToComplete = "Use all of the morphine on fallen soldiers.";

            taskForQuestsPage = "Use all of the morphine on fallen soldiers.";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!Game1.g.ChapterTwo.ChapterTwoBooleans["serumGiven"])
            {
                Game1.g.ChapterTwo.ChapterTwoBooleans["serumGiven"] = true;

                Game1.Player.AddStoryItem("Goblin Morphine", "Goblin Morphine", 12);
            }

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["serumGiven"] && !Game1.Player.StoryItems.ContainsKey("Goblin Morphine") && !completedQuest)
            {
                specialConditions["Use all of the morphine on fallen soldiers"] = true;

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

