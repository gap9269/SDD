using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class RatQuest : Quest
    {
        Boolean viewedJournal = false;

        public Boolean ViewedJournal { get { return viewedJournal; } set { viewedJournal = value; } }

        public RatQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("*sob* Some heartless monster murdered my sweet Riley...Right over there, my baby was smashed on the floor ...What kind of sick animal would smush a poor old woman's friend?!");
            questDialogue.Add("Oh, you can't begin to imagine my heartache...If her favorite bow wasn't attached to her tail, I might not have even recognized her...smeared across the floor like a bug *sob*");
            questDialogue.Add("She was so much more than a pet... she was my family. My only family *sniff* Raised her from birth, I did. Oh how she loved running around and playing with you kids while I worked the gardens...");
            questDialogue.Add("The only thing she loved more than you kids was the flower patch in the Far Side. Oh..the afternoons we spent there... If only I knew how precious the time was!");
            questDialogue.Add("Please...could you bury her there for me? I can't...*sniff* I can't take the heartache. Please bury her in her favorite flower patch. You're the friend I need in a sorrowful time like this.");
            questDialogue.Add("...Is Riley at rest yet?");

            completedDialogue.Add("This is Riley's bow. She always loved this bow. I want you to have it. She would have wanted to thank you for burying her by the flowers. Riley loved flowers.");

            rewardObjects.Add(new Karma(2));
            rewardObjects.Add(new RileysBow());

            npcName = "Gardener";

            specialConditions.Add("Go to the Far Side and bury Riley the Rat.", false);

            questName = "Flowers for Riley";

            conditionsToComplete = "Go to the Far Side and bury Riley the Rat.";

            rewards = "Riley's Bow\n2 Karma";

            taskForQuestsPage = "Bury Riley the Rat in the Far Side";

            descriptionForJournal = "A sweet old lady was crying over her pet rat, Riley, who you brutally murdered as soon as you got to school. Did you think burying her would make up for what you did?" +
                " She even gave you Riley's little pink bow! You're a monster.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (completedQuest)
            {
                specialConditions["Go to the Far Side and bury Riley the Rat."] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
