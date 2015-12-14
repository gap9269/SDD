using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class SousChefFirstCourse : Quest
    {
        public SousChefFirstCourse(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh great, so what, you gonna give me shit about not orderin' more food, too?");
            questDialogue.Add("I ain't gotta apologize or explain myself to nobody. I tell ya what, you want me to make somethin' today you go get it an' bring it to me yourself.");
            questDialogue.Add("I'm sick of all you little bastards comin' in here in my kitchen and tellin' me what I ain't doin' right.");
            questDialogue.Add("We got a deal or we got a deal? Either way, why don't you get outta here?");
            questDialogue.Add("I don't see you carryin' any food. Shouldn't you damn kids be in class anyway?");
            completedDialogue.Add("Well wouldya look at that. This cheese might just be the moldiest cheese I ever seen. Must be exceptionally fancy.");
            completedDialogue.Add("Tell ya what, kid. You did me a favor. Now none of them kids are gonna come crawlin' in here bitchin' to me about how I ain't makin' them lunch an' how they gonna sue.");
            completedDialogue.Add("For that, I'll give ya a small taste of my very own special butter. I call it Luck Butter. Rub it on your skin an' all your dreams could come true.");
            completedDialogue.Add("It's how I stay lookin' so young an' employed.");
            completedDialogue.Add("You come on back here tomorrah an' we'll do business again.");

            rewardObjects.Add(new EighthOfButter());
            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(325));
            rewardObjects.Add(new Karma(2));

            itemNames.Add("Spinach");
            itemsToGather.Add(5);
            itemNames.Add("Half-Eaten Cheese");
            itemsToGather.Add(10);
            itemNames.Add("Fuzzy Meat Chunk");
            itemsToGather.Add(13);

            npcName = "Chef Flex";

            questName = "Sous Chef: First Course";

            conditionsToComplete = "Collect food from around the school to give to Chef Flex so he can finally start doing his job.";

            rewards = "1/8 Stick of Luck Butter\n50 Experience\n2 Karma\n1 Textbook";

            taskForQuestsPage = "Collect food from around the school to give to Chef Flex.";

            descriptionForJournal = "The school chef hasn't cooked a day in his life. It turns out all he needed was a trustworthy Sous Chef. Unfortunately all he got was you, but it was enough to at least provide the students of WFHS with lunch for the first time in years. This seems like the start to a beautiful partnership.";
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

            Game1.Player.RemoveDrops("Spinach", 5);
            Game1.Player.RemoveDrops("Half-Eaten Cheese", 10);
            Game1.Player.RemoveDrops("Fuzzy Meat Chunk", 13);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

