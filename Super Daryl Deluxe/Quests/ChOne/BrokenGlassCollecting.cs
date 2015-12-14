using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BrokenGlassCollecting : Quest
    {
        public BrokenGlassCollecting(Boolean story)
            : base(story)
        {
            questDialogue.Add("I saw this awesome documentary last night about a guy who can eat glass. It was really inspiring!");
            questDialogue.Add("I bet if I could do that I would have a ton of friends and be rich.");
            questDialogue.Add("Hey, you look like a guy that can get things done.");
            questDialogue.Add("If you get me 10 pieces of glass, preferably broken, I'm sure I can cut you in on my future career as a glass-eater.");
            questDialogue.Add("Of course I'll give you something nice up front too. Consider it an investment. Care to give it a shot?");
            questDialogue.Add("Did you find any glass? I haven't eaten all day!");
            completedDialogue.Add("Thanks! These look delicious.");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(50));
            rewardObjects.Add(new Karma(2));

            npcName = "Drew";

            itemNames.Add("Broken Glass");
            itemsToGather.Add(10);

            questName = "Collecting Broken Glass";

            conditionsToComplete = "Collect 10 pieces of Broken Glass to further Drew's dream of becoming a famous glass-eater.";

            taskForQuestsPage = "Collect 10 pieces of Broken Glass";

            rewards = "1 Textbook\n20 Experience\n2 Karma";

            descriptionForJournal = "We've all heard of those people that can swallow swords, but what about glass? You may have just helped create the world's first glass-eater after giving Drew 10 pieces of broken glass to practice with. Keep an eye on your local newspaper for stories of a student swallowing glass, he's going places!";
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

            Game1.Player.RemoveDrops("Broken Glass", 10);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

