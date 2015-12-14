using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SupplyRun : Quest
    {
        public SupplyRun(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oooohoho bonjour my friend. Yes you are endowed with some nice long, slender limbs, aren't you? I certainly vould love to have a chance to doctor those appendages.");
            questDialogue.Add("Oh if only I had ze resources. That cheap-a-skate Bonaparte does not believe in furnishing heez talented doctor with more zan ze bare minimum.");
            questDialogue.Add("Would you believe he haz ze gall to complain to me of ze screaming keeping his camp awake throughout ze night?");
            questDialogue.Add("Between you and me, zose cries of agony are sweet music to my ears, but nonezeless zat shrimp thinks I am supposed to maintain a peaceful butchering with only a single dull saw.");
            questDialogue.Add("Did I say butchering? I meant doctoring.");
            questDialogue.Add("If only I could test whatever ze half-human monsters use on their injured. What beautiful disastrous results might I see? Oohoho.");
            questDialogue.Add("Back for me to butc--doctor- your nice long limbs?");
            completedDialogue.Add("Vhat is zis? Ohoho! Did you bring me new toys to play with? Perhaps zomething here will be as fun as zis dull saw.");

            rewardObjects.Add(new NurseHat());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(515));
            rewardObjects.Add(new Karma(4));

            npcName = "Dr. Dominique Jean Larrey";

            itemNames.Add("First Aid Kit");
            itemsToGather.Add(15);

            questName = "Dr. Dominique's Supply Run";

            conditionsToComplete = "Find 15 First Aid Kits for the good War Doctor.";

            taskForQuestsPage = "Find 15 First Aid Kits for the good War Doctor.";

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

            Game1.Player.RemoveDrops("First Aid Kit", 15);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

