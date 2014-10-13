using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class ScissorsQuest : Quest
    {
        public ScissorsQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("You look like a man who's in touch with the wilderness.");
            questDialogue.Add("I was pursuing what appeared to be a Giant Malaysian Ridge Squirrel when I lost him up in that tree. It's a damned shame, I thought the bastards to be extinct.");
            questDialogue.Add("But just like Pa used to say, \"When one door shuts, you find another animal to skin.\", and look here at what I found. A pack of African Dirt Cows.");
            questDialogue.Add("Can you believe it? Dirt Cows are as rare as you can get! Unfortunately they've set up some mighty formidable defenses here. I'm gonna need something to cut through this wire.");
            questDialogue.Add("Could you go track down something I can use to get in there? Once I skin these here beauties I can make a nice coat for each of us. Whaddya say?");
            questDialogue.Add("African Dirt Cows are known for their exquisite defense mechanisms. Have you found something to cut through this yet?");
            completedDialogue.Add("This is looking to be the luckiest day of my life. If only Pa could see me now! Come on back in a bit and I'll have these bad boys turned into a couple of coats and matching gloves for us.");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(70));
            rewardObjects.Add(new Karma(1));


            itemNames.Add("Scissors");
            itemsToGather.Add(1);

            questName = "To Kill a Goat";

            taskForQuestsPage = "Find scissors for Pelt Kid.";
            conditionsToComplete = "Pelt Kid wants something sharp to cut through the electric fence surrounding Chelsea's goats. A nice pair of scissors should do just fine.";

            rewards = "1 Textbook\n1 Karma\n70 Experience";

            descriptionForJournal = "A weird kid covered in dead animals thought he had discovered a rare breed of cattle when in reality he had just stumbled upon Chelsea's goat pen. He offered to make you a coat and a nice pair of gloves out of the animals if you were to helpnhim get through the electric fence. \n\nHow does it make you feel to have caused a poor, dumb, innocent creature's death?";
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


            Game1.Player.RemoveStoryItem("Scissors", 1);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

