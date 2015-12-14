using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class TheClaysmithsWork : Quest
    {
        public TheClaysmithsWork(Boolean story)
            : base(story)
        {
            questDialogue.Add("*sob* It's awful! I just received word from back home. It seems like those bandits managed to destroy all of my vases despite your effort.");
            questDialogue.Add("All of my life's work! Gone! My exotic vases, smashed to pieces in my own home. All of the money inside of them was taken, too. I'm ruined! What did I do to deserve this? *sob*");
            questDialogue.Add("Listen, please, my only hope is to get my hands on some new clay to start rebuilding what I've lost. I have no money to purchase it, but perhaps you could find some for me?");
            questDialogue.Add("I was cursed by a gypsy a few months ago and for a while all of my creations came to life and escaped. They're surely still rolling around here somewhere...and their clay should still be good to use.");
            questDialogue.Add("There's no way I can retire off of anything I make now, but I can at least make enough to survive. Please, I have no other choice.");
            questDialogue.Add("*sob* My life's work...gone! Have you managed to track down those gypsy-cursed clay monsters yet?");

            completedDialogue.Add("Oh thank you, thank you! A silver lining on this dark, depressing cloud of a life-ruining day.");
            completedDialogue.Add("I don't have much to give because now that everything I worked to create was destroyed by those heartless bandits, but I did find some junk lying around that you can have. I hope you make good use of it.");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new ArtistsPalette());
            rewardObjects.Add(new Experience(240));
            rewardObjects.Add(new Karma(3));

            itemNames.Add("Clay Dough");
            itemsToGather.Add(12);
            npcName = "Town Claysmith";
            questName = "The Claysmith's Work";

            conditionsToComplete = "Collect 12 pieces of Clay Dough so the Claysmith can rebuild his life's work.";

            taskForQuestsPage = "Collect 12 pieces of Clay Dough so the Claysmith can rebuild his life's work.";

            //rewards = "Art Smock\n$20\n2 Karma";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

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
            Game1.Player.RemoveDrops("Clay Dough", 12);

            Game1.g.SideQuestManager.nPCs["Town Claysmith"].Dialogue.Clear();
            Game1.g.SideQuestManager.nPCs["Town Claysmith"].Dialogue.Add("Here we go...starting four decades of work over again...");

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

