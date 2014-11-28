using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BeerForToga : Quest
    {
        public BeerForToga(Boolean story)
            : base(story)
        {
            questDialogue.Add("What a glorious bash!");
            questDialogue.Add("I do say, I have the sudden urge to party in the nude! Daryl my dear boy, go fetch me another beverage, quickly!");
            questDialogue.Add("The urge to strip down to my natural self is overwhelming, child! Quickly, a \nnew beverage!");
            completedDialogue.Add("Move aside everyone! Your highness is here to cause a ruckus!");
            rewardObjects.Add(new Toga());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(400));

            itemNames.Add("Beer");
            itemsToGather.Add(1);

            questName = "I came, I saw, I partied naked";

            taskForQuestsPage = "Find Julius Caesar some beer.";
            conditionsToComplete = "You had no idea Julius Caesar was such a party animal! He wants to party in the nude, and who are we to stop our beloved Consul from doing as he pleases? \n \nJulius wants a case of beer, and you should oblige him.";

            rewards = "1 Textbook\n1 Toga\n20 Experience";

            descriptionForJournal = "It was generous of you to invite Caesar to Chelsea's party, but a barn party is no place for a fully clothed Roman Emperor. Luckily he knew that was the case and offered to party in his birthday suit if you simply found him a bit o' liquid confidence. I think we're all better people for having taken the deal.";
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

            Game1.Player.RemoveStoryItem("Beer", 1);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

