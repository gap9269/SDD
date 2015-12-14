using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class BlowingUpLove : Quest
    {
        public BlowingUpLove(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh, woe is moi! My love, it has perished!");
            questDialogue.Add("I fell in love with him and his brilliant marble sculptures. Our romance blossomed, the world had never seen such passion between two souls. At last, in autumn, I decided to ask for his hand in marriage.");
            questDialogue.Add("But it turned out, his heart was made of marble all along.");
            questDialogue.Add("My home is now filled with sculptures of our love, big and small, cliche and profound. I cannot bear to return there, as it rends my poor, withering heart.");
            questDialogue.Add("I must ask of you: can you let your fellow man suffer as I am? Amidst the permanent, marble reminders of the love I had shared with another?");
            questDialogue.Add("My only hope is to forget the past, by blowing it all to pieces. If you can find it in your heart to provide me with a cart full of explosives, perhaps I can begin repairing mine.");
            questDialogue.Add("Oh, am I damned to live among the stone-cold remnants of my love forever?");

            completedDialogue.Add("Among the endless void of marbled despair that I now find myself in, you are a beacon of light, showing me the best in man.");
            completedDialogue.Add("Tonight I will do the deed and blow my former lover's work, and the chains around my heart, to smithereens.");

            rewardObjects.Add(new ChiselOfForgottenLove());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(450));
            rewardObjects.Add(new Karma(4));

            npcName = "French Soldier";

            itemNames.Add("Insta-Bomb!");
            itemsToGather.Add(10);

            questName = "Blowing Up Love";

            conditionsToComplete = "Find 10 cans of \"Insta-Bomb!\" for the heart-broken soldier";

            taskForQuestsPage = "Find 10 cans of \"Insta-Bomb!\" for the heart-broken soldier";

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

            Game1.Player.RemoveDrops("Insta-Bomb!", 10);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

