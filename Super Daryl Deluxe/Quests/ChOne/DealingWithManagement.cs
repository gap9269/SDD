using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class DealingWithManagement : Quest
    {
        Game1 game;

        public DealingWithManagement(Boolean story, Game1 g)
            : base(story)
        {

            game = g;

            questDialogue.Add("....");
            questDialogue.Add("Excellent! Why waste words when there is precious work to be done? I like you, servant boy.");
            questDialogue.Add("Let's get down to business. What you see here is the beautiful Theater An Der Wien, and this is her stage. What am I doing here, you ask? Well, I am writing my Magnum Opus, an opera, \"Fidelio\".");
            questDialogue.Add("The previous owner of this establishment was kind enough to let me stay here as a courtesy...and under the condition that I center this masterpiece on his wife, Leonore.");
            questDialogue.Add("Alas, that is neither here nor there, as there is now a new owner and manager of the Theater, and I fear the philistine does not share the same love and taste of music that the previous did. He wants me gone, and he is making my life, that is the completion of Fidelio, a living hell.");
            questDialogue.Add("As my new servant boy this is now your problem as much as it is mine. Go find the new owner and work something out for me. Take this key to get backstage, it is where his office is located.");
            questDialogue.Add("Do not return without a solution, or you shall not be receiving your food rations this week.");
            questDialogue.Add("Why is it that you have returned to me without working things out with that unreasonable man?");

            completedDialogue.Add("A-hah! And I was just beginning to wonder what was taking my servant-boy so long. I can tell from that look in your eye that you reached an agreement with that odd young man.");

            npcName = "Beethoven";

            questName = "Dealing with Management";

            specialConditions.Add("Convince the Manager to let Beethoven stay", false);

            conditionsToComplete = "Convince the Manager to let Beethoven stay.";
            taskForQuestsPage = "Convince the Manager to let Beethoven stay.";
            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"])
            {
                completedQuest = true;
                specialConditions["Convince the Manager to let Beethoven stay"] = true;
            }

            if (game.ChapterOne.ChapterOneBooleans["givenBackstageKey"] == false)
            {
                Game1.Player.AddStoryItem("Backstage Key", "the backstage key", 1);
                game.ChapterOne.ChapterOneBooleans["givenBackstageKey"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Game1.Player.RemoveStoryItem("Backstage Key", 1);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}