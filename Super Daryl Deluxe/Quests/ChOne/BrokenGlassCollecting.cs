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
            questDialogue.Add("You look like a guy that can get things done!");
            questDialogue.Add("My dad is in the glass business. I get a raise in my allowance for every day \nthat I bring " +
                "glass home for him to work his magic on.");
            questDialogue.Add("If you get me 20 pieces of glass, preferably broken, I'm sure I can give you \nsomething really useful.");
            questDialogue.Add("I'm not sure where you'll get some without getting caught. I mean the old science room has loads of" +
                "\nold flasks and other glassy sciency things, but the room has been locked for months.");
            questDialogue.Add("Well, care to give it a shot anyway?");
            questDialogue.Add("Did you find any glass?");
            completedDialogue.Add("Thanks! I'll be rolling in dough this week.");

            rewardObjects.Add(new Experience(20));
            rewardObjects.Add(new LabCoat());

            itemNames.Add("Broken Glass");
            itemsToGather.Add(20);

            questName = "Collecting Broken Glass";

            conditionsToComplete = "Collect 20 pieces of Broken Glass for that weird glass kid.";

            rewards = "- (1) Dirty Gym Shirt\n\n- 20 Experience";

            descriptionForJournal = "A student asked you to find a bunch of broken glass \nfor him to take home to his father, who seems to pay \nhis son for" + " each piece he brings him. I'm not sure what the \n'Glass Business' even is. \n\nRewards:\n" + rewards;
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
                    break;

                if (i == completedParts.Count - 1)
                    completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.Player.RemoveStoryItem("Broken Glass", 20);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

