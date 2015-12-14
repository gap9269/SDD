using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class BehindGoblinyLinesPartOne : Quest
    {
        public BehindGoblinyLinesPartOne(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh-hoh! You again! I was hoping zat you would return. You really saved ze day back there. Zis war of mine is still a deadly one, but my men lived to fight another day thanks to you, oh-hoh.");
            questDialogue.Add("Right now zey are sneaking into ze enemy's camp to secure an advantageous position. Zat evil bastard will not zee it coming!");
            questDialogue.Add("You may check on zem if you like, but be sneaky about it, yes? By ze way, an unfamiliar face recently came through ze camp. He was tall, very gray, and had a particularly odd stiffness about him. A friend of yours, I zuspect?");
            questDialogue.Add("I do hope he is as good at protecting himself as you are, oh-hoh. He wandered straight into ze enemy's forces.");
            questDialogue.Add("Do not worry about my men stopping you from entering ze war zone. Zey know to let you pass.");
            questDialogue.Add("Ze battlefield is off to ze right. My men will let you through.");

            completedDialogue.Add("xgdfhsfgh");

            questName = "Behind Gobliny Lines-Part One";

            specialConditions.Add("Find Napoleon's stealth squad in \nthe battlefield", false);

            conditionsToComplete = "Find Napoleon's soldiers that are sneaking into enemy territory";

            taskForQuestsPage = "Find Napoleon's soldiers that are sneaking into enemy territory";

            npcName = "Napoleon";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            Game1.g.ChapterTwo.ChapterTwoBooleans["canEnterBattlefield"] = true;

            if (completedQuest)
            {
                specialConditions["Find Napoleon's stealth squad in \nthe battlefield"] = true;
            }

        }

        public override void RewardPlayer()
        {
            Game1.g.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesOneCompleted"] = true;

            base.RewardPlayer();
        }
    }
}
