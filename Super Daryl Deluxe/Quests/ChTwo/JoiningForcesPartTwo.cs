using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class JoiningForcesPartTwo : Quest
    {
        public JoiningForcesPartTwo(Boolean story)
            : base(story)
        {
            questDialogue.Add("I need you to go zee what is happening in Egypt. None of my messengers to Cleopatra have returned, and I fear zat Caesar won't stay still for very long.");
            questDialogue.Add("I cannot afford to lose him not zat he is finally here with his armies.");
            questDialogue.Add("Here, take zis letter and deliver it to her, by hand would be best.");
            questDialogue.Add("And hurry back...before zis insane man drives me mad as well.");
            questDialogue.Add("Where is Cleopatra?? Zis man will not shut up!");

            completedDialogue.Add("Ohoh! Excellent job, my little zoldier! You have even convinced ze horse barbarian to join us.");
            completedDialogue.Add("Julius has finally left me alone now zat Cleopatra is around, although I do not think she appreciates his affection.");
            completedDialogue.Add("We are ready to attack, once you are.");

            questName = "Joining Forces-Part 2";

            specialConditions.Add("Convince Cleopatra to join forces with \nNapoleon", false);

            conditionsToComplete = "Convince Cleopatra to join forces with Napoleon";

            taskForQuestsPage = "Convince Cleopatra to join forces with Napoleon";

            npcName = "Napoleon";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!Game1.Player.StoryItems.ContainsKey("Letter to Cleopatra"))
                Game1.Player.AddStoryItem("Letter to Cleopatra", "an envelope", 1);

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["princessSceneOnePlayed"])
            {
                completedQuest = true;
            }

            if (completedQuest)
            {
                specialConditions["Convince Cleopatra to join forces with \nNapoleon"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.g.ChapterTwo.ChapterTwoBooleans["completedJoiningForcesTwo"] = true;
        }
    }
}
