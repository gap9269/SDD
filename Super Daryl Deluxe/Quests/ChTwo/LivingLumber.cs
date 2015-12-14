using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class LivingLumber : Quest
    {
        public LivingLumber(Boolean story)
            : base(story)
        {
            questDialogue.Add("Kublai! Where have you been?");
            questDialogue.Add("Caesar has still not given us what is owed. He has employed legions of monsters to assist him in avoiding us. We cannot get passed that wall, gods curse him!");
            questDialogue.Add("Do not lose hope, Kublai. I have a plan.");
            questDialogue.Add("We will build a large gift for the conniving, brainless man. He is too proud to turn down a gift, and not intelligent enough to see through the trap.");
            questDialogue.Add("You see, it will be hollow and you will be hiding inside. Once he lets it through it will be your job to jump out and open the gate for us. I will not hold it against you if you delay a moment or two to take Caesar's ugly head from his shoulders.");
            questDialogue.Add("Now, go! Gather lumber so my master craftsmen may build us a gift suitable for 'his royal majesty'!");
            questDialogue.Add("Kublai! What are you doing? Gather lumber for our deceptive gift!");
            completedDialogue.Add("Excellent job, Kublai.");
            completedDialogue.Add("My master craftsmen should have Caesar's gift constructed immediately! There is no time to waste, Kublai. With me!");

            npcName = "Genghis";

            itemNames.Add("Lumber");
            itemsToGather.Add(10);

            questName = "Living Lumber";

            conditionsToComplete = "Gather 10 pieces of lumber to build a fake gift for Caesar.";

            taskForQuestsPage = "Gather 10 pieces of lumber to build a fake gift for Caesar.";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
            List<Boolean> completedParts = new List<bool>();

            for (int i = 0; i < itemNames.Count; i++)
            {
                if (Game1.Player.EnemyDrops.ContainsKey(itemNames[i]) && Game1.Player.EnemyDrops[itemNames[i]] >= itemsToGather[i] ||
                    Game1.Player.StoryItems.ContainsKey(itemNames[i]) && Game1.Player.StoryItems[itemNames[i]] >= itemsToGather[i])
                {
                    completedParts.Add(true);
                }
                else
                    completedParts.Add(false);
            }

            for (int i = 0; i < completedParts.Count; i++)
            {
                if (completedParts[i] == false)
                {
                    completedQuest = false;
                    break;
                }

                if (i == completedParts.Count - 1)
                    completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Game1.g.ChapterTwo.ChapterTwoBooleans["lumberQuestFinished"] = true;
            Game1.Player.RemoveDrops("Lumber", 10);
        }
    }
}

