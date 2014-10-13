using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class KarmaQuest : Quest
    {
        public KarmaQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("Greetings fellow role player!");
            questDialogue.Add("Wait a sec...I don't think I've seen you around here before. You DO play" +
                              " 'Dwarves and Druids', right?");
            questDialogue.Add("I'm a Level 5 'Summoner'. By the look of you, I'd guess you're a Level 1 'Flower Boy'.");
            questDialogue.Add("In D&D we level up based on experience, but we also have 'Social Ranks'. They are independent of each other" +
                              ", but they are both important!");
            questDialogue.Add("No one will be your friend if you don't have an impressive Social Rank, you know. That's why I have a ton of friends.");
            questDialogue.Add("And of course you need 'Karma' to rank up. Ranking up is the Number One Rule! Since you're new, I'll give you a bit of help.");
            questDialogue.Add("Just talk to me again when you're ready to learn more about Karma, alright?");
            questDialogue.Add("");
            completedDialogue.Add("I'm glad you're so excited about Karma. I aim to have the best rank in this realm some day!");
            completedDialogue.Add("I do as many quests as I can for the commonfolk to gain as much Karma as possible." + 
                                  " Sometimes when you rank up the Game Master will even allow you to wear better equipment!");
            completedDialogue.Add("Good things come to those with a good 'Social Rank'. You're on your way, Flower Boy.");

            rewardObjects.Add(new Karma(3));

            npcName = "Karma Instructor";

            specialConditions.Add("Talk to the D&D player to learn more about Karma.", false);

            questName = "Learning about Karma";

            taskForQuestsPage = "Learn about Karma";

            conditionsToComplete = "Talk to the D&D player to learn more about Karma.";

            rewards = "3 Karma";

            descriptionForJournal = "A wise, young shaman spoke to you of the wonders of karma and social status. He said that the reason he has so many friends is his esteemed D&D rank. You should listen to him, he had a banana taped to his head.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (completedQuest == false)
                completedQuest = true;

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

