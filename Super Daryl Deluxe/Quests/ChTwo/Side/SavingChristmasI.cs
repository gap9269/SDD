using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SavingChristmasI : Quest
    {
        public SavingChristmasI(Boolean story)
            : base(story)
        {
            questDialogue.Add("This prison. It kills joy and locks up the very spirit of Christmas.");
            questDialogue.Add("I've checked my list, and I've checked it twice. Scrooge isn't just naughty... he's ho ho horrible. Nothing would give me greater happiness than escaping this rotting cell and exacting my ice cold revenge.");
            questDialogue.Add("The kind of revenge that only an immortal being of wonder can produce. But now I'm not just a being of wonder...but of terror.");
            questDialogue.Add("Daryl, now is the time to repent for your recent sins. Scrooge has been ho ho holding me hostage for months now. Find the key that Scrooge keeps to this cage, it's assuredly in this mansion, and I will reward you well.");
            questDialogue.Add("I will avenge my reindeer and elf workers.");
            completedDialogue.Add("Christmas is cancelled, but my revenge isn't.");

            rewardObjects.Add(new Coal(true));
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(1500));
            rewardObjects.Add(new Karma(2));

            npcName = "Santa Claus";

            itemNames.Add("Christmas Cage Key");
            itemsToGather.Add(1);

            questName = "Saving Christmas I";

            conditionsToComplete = "Find the key to release Santa from his prison.";

            taskForQuestsPage = "Find the key to release Santa from his prison.";

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

            Game1.Player.RemoveStoryItem("Christmas Cage Key", 1);
            Game1.g.ChapterTwo.ChapterTwoBooleans["santaReleased"] = true;
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

