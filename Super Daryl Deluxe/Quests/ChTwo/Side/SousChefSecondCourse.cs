using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SousChefSecondCourse : Quest
    {
        public SousChefSecondCourse(Boolean story)
            : base(story)
        {
            questDialogue.Add("There he is! My own personal Sous Chef. Bout fuckin' time. I was supposetah start cookin' lunch hours ago.");
            questDialogue.Add("Not-a-one of them little bastards touched the stew I made yesterday with the ingredients you brought me, so how about we get a little fancy for 'em?");
            questDialogue.Add("Bring me somethin' exotic this time. We'll see if you damn kids like some real cuisine. I'll really flex my chef muscles on this one.");
            questDialogue.Add("...");
            questDialogue.Add("You know, I've been tough on you kids. Tell you what, I got some brownie batter that's goin' bad soon, I'm gonna whip you kids up a nice treat for lunch today, how 'bout that?");
            questDialogue.Add("While you're out there, get me some fresh walnuts.");
            questDialogue.Add("We got a deal?");
            questDialogue.Add("I don't see no walmuts. You want a nice treat or what?");
            completedDialogue.Add("Uncracked nuts? What the hell good are these? And what are these, haunted?");
            completedDialogue.Add("Where the hell did you get uncracked haunted nuts?");
            completedDialogue.Add("Well, whatevah. Just make the brownies a bit tougher to get down, I s'pose. You kids are young, got strong jaws. It'll be fine. The rest of this looks good, too.");
            completedDialogue.Add("I'll give you some more of my Luck Butter for doin' such a dandy job. This is a bigger batch, should make you real lucky if you rub it all over yourself.");
            completedDialogue.Add("You come on back here tomorrah an' we'll do business again.");

            rewardObjects.Add(new QuarterOfButter());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(325));
            rewardObjects.Add(new Karma(2));

            itemNames.Add("Peyote");
            itemsToGather.Add(8);
            itemNames.Add("Haunted Walnuts");
            itemsToGather.Add(10);
            itemNames.Add("Mummified Food");
            itemsToGather.Add(1);

            npcName = "Chef Flex";

            questName = "Sous Chef: Second Course";

            conditionsToComplete = "Collect exotic food from around the school to give to Chef Flex so he can make a fancy lunch.";

            taskForQuestsPage = "Collect exotic food from around the school to give to Chef Flex.";

            descriptionForJournal = "";
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

            Game1.Player.RemoveDrops("Peyote", 8);
            Game1.Player.RemoveDrops("Haunted Walnuts", 10);
            Game1.Player.RemoveStoryItem("Mummified Food", 1);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

