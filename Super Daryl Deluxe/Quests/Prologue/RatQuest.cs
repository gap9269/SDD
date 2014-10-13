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
            questDialogue.Add("...");
            questDialogue.Add("Huh? Who are you? I'm sorry you have to see me like this sweetie. Someone stepped on my poor Riley this morning and I just found him stuck to the floor.");
            questDialogue.Add("What kind of cruel, heartless monster would do this to him and just go on with their day like nothing happened? Who could care so little for such an innocent creature's life?");
            questDialogue.Add("*sob* He was more than just a pet rat you know. He was everything to me. I raised him from birth when his mother abandoned him inside the school. He loved running around and seeing the students while I worked the garden... *sniff*");
            questDialogue.Add("The only thing he loved more was the flower patch in the Far Side. We spent hours together there, just Riley and I... *sob* oh god...");
            questDialogue.Add("Please...could you bury him there for me? I can't... *sniff* I can't take the heartache. Just bury him by his favorite flower patch.");
            questDialogue.Add("...Is Riley resting yet?");

            completedDialogue.Add("You're such a sweetheart. This world needs more boys like you that actually care about an old lady and her poor, sweet Riley.");
            completedDialogue.Add("This is Riley's bow. He always loved this bow. I want you to have it. He would have wanted to thank you for burying him by his flowers. Riley loved flowers.");

            rewardObjects.Add(new Karma(2));
            rewardObjects.Add(new RileysBow());

            npcName = "Gardener";

            specialConditions.Add("Go to the Far Side and bury Riley the Rat.", false);

            questName = "Flowers for Riley";

            conditionsToComplete = "Go to the Far Side and bury Riley the Rat.";

            rewards = "Riley's Bow\n2 Karma";

            taskForQuestsPage = "Bury Riley the Rat in the Far Side";

            descriptionForJournal = "A sweet old lady was crying over her pet rat, Riley, who you brutally murdered as soon as you got to school. Did you think burying him would make up for what you did?" +
                " She even gave you Riley's little pink bow! You're a monster.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}
