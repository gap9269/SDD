using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class BehindGoblinyLinesPartTwo : Quest
    {
        public BehindGoblinyLinesPartTwo(Boolean story)
            : base(story)
        {
            questDialogue.Add("God has answered my prayers! You are ze crazy floppy killing machine from before! We are in trouble, and could use a bit of zat floppy berserk murder that you dish out with your bare hands, oh-hoh.");
            questDialogue.Add("My men and I were sneaking through ze battlefield when we spotted a safe area up ahead. Zat was when disaster struck. A tall, gray man, showing no concern for being spotted by ze dirty enemy, walked right up to us and spoke loudly about a hall pass, whatever zat is.");
            questDialogue.Add("In a stealth mission such as zis, he may as well have been sounding an alarm. Zat is when the enemy sounded the alarm. Enemies appeared out of nowhere, and ze mysterious gray man disappeared as quickly as he arrived.");
            questDialogue.Add("Ze only place he could have gone was up ahead passed ze mortars, but we cannot get out of here until someone takes care of zem. Would you do us a favor, oh floppy berserker, and give us a chance to take revenge on ze tall gray man?");
            questDialogue.Add("Do you think it is weird zat ze mortars never aim far enough to hit us?");

            completedDialogue.Add("A miracle! Ze sky has stopped raining death down on us all!");
            completedDialogue.Add("We will finish our mission of establishing a camp up ahead. You will let General Bonaparte know of the events here, I assume?");

            questName = "Behind Gobliny Lines-Part Two";

            specialConditions.Add("Destroy the enemy mortars", false);

            conditionsToComplete = "Rescue the French soldiers by destroying the enemy mortars";

            taskForQuestsPage = "Rescue the French soldiers by destroying the enemy mortars.";

            npcName = "Private Brian";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesOneCompleted"] == false)
            {
                Game1.g.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesOneCompleted"] = true;
                Game1.g.ChapterTwo.NPCs["Napoleon"].CompleteQuestSilently(Game1.g.ChapterTwo.behindGoblinyLinesPartOne);
            }

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["valleyMortarsDestroyed"])
            {
                completedQuest = true;
                specialConditions["Destroy the enemy mortars"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Game1.g.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesTwoCompleted"] = true;
        }
    }
}
