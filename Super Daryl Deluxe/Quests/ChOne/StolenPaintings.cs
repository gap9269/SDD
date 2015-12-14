using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class StolenPaintings : Quest
    {
        public StolenPaintings(Boolean story)
            : base(story)
        {
            questDialogue.Add("You again. I can smell Beethoven's booze on you before you arrive, you know. It's disgusting. How did all of that business go, anyway? I do hope he's done taking advantage of your willingess to blindly follow orders.");
            questDialogue.Add("In any case, I have something for you to do.");
            questDialogue.Add("I was recently admiring the scenery in this newly constructed world that we find ourselves in, and against the suggestions of my wiser friends I was doing so at night.");
            questDialogue.Add("Now I'm not one to believe in giant humanoid rats, especially of the knife-wielding variety, but I swear on my honor as an artist that that's what I saw!");
            questDialogue.Add("How do I know that the dark was not playing tricks on my eyes? Well, they also robbed me of my entire collection of Cheese Paintings. You have to get close to do that sort of thing, and I know what I saw.");
            questDialogue.Add("Those paintings were the first full collection of Still Life work that I had ever done, back when I was still in my \"starving artist\" years. It would happen that painting food does not satisfy the hunger that a lack of real food creates, but it did make me famous. Fancy that.");
            questDialogue.Add("But, I digress. Will you help me recover my paintings? They are important to me. I will reimburse you favorably, of course.");

            questDialogue.Add("Don't tell me that you'll work for a drunk, deaf idiot, but refuse to do a small favor for a famous artist and inventor!");
            completedDialogue.Add("Hmm...you know now that I think of it, these really weren't worth getting back.");

            rewardObjects.Add(new Paintbrush());
            rewardObjects.Add(new Experience(205));
            rewardObjects.Add(new Karma(2));

            itemNames.Add("Stolen Painting");
            itemsToGather.Add(10);
            npcName = "Leonardo Da Vinci";

            questName = "Stolen Paintings";

            conditionsToComplete = "Recover 10 of Da Vinci's stolen paintings of cheese";

            taskForQuestsPage = "Recover 10 of Da Vinci's stolen paintings of cheese.";

            rewards = "Paintbrush\n1 Karma\n35 Experience";

            descriptionForJournal = "Pending";
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

            Game1.Player.RemoveDrops("Stolen Painting", 10);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

