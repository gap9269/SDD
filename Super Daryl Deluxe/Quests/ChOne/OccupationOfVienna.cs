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
    public class OccupationOfVienna : Quest
    {
        public OccupationOfVienna(Boolean story)
            : base(story)
        {
            questDialogue.Add("Servant! For god's sake, where have you been? I have long since finished \"Fidelio\", no thanks to your extended absence. ");

            questDialogue.Add("We will determine the consequences fitting for abandoning your duties later, as there are much more pressing matters to attend to in the present:");

            questDialogue.Add("Not a single ticket has sold for the premiere of my opera! This is outrageous, and downright impossible under any normal circumstance.");

            questDialogue.Add("At first I was suspicious of that new manager that you dealt with some time back. However I have rarely seen him as of late, and when I do he seems quite busy with other matters. I do doubt that he has anything to do with it, despite what he may feel about my genius.");

            questDialogue.Add("After speaking with the other composers in the Theater I have learned that none of their showings have brought in a single watching eye either, be it peasant or lord. I do doubt that it is their mediocrity that drives away the crowd; usually a few beggars will wander in off the streets at the very least.");

            questDialogue.Add("But no! Not a single living creature as stepped foot inside of this Theater since you left several weeks ago. I haven't the slightest idea what is causing this, but I do know that it is your responsibility to figure it out and fix it.");

            questDialogue.Add("Think of this as an opportunity to redeem yourself! Begone!");

            questDialogue.Add("I will not have an empty house on the day my masterpiece is revealed!");

            completedDialogue.Add("You sure do have a knack for disappearing for conspicuous lengths of time. No matter, it would appear that whatever you were doing worked. I had nearly half of a full house for my opera, a new record for such a pathetic establishment, I must say!");
            completedDialogue.Add("Unfortunately it would appear that the critics in this town have not developed even half of the taste that my masterpiece requires, and they have done me no favors in their reviews. Alas, I will need to return to my work and dumb it down for such tone-deaf men to enjoy.");
            completedDialogue.Add("In any case you have done your job as my servant, and I no longer require your services. I will give you something to remember me by as a parting gift; something that perhaps will lead you on the path of the musician yourself. Good day.");

            rewardObjects.Add(new Recorder(0, 0));

            rewards = "1 Recorder";

            npcName = "Beethoven";

            questName = "Where'd the Crowd Go?";

            specialConditions.Add("Find out why Theater tickets aren't selling", false);
            specialConditions.Add("Fix it", false);

            taskForQuestsPage = "Find out why Theater tickets aren't selling, and then fix it.";

            conditionsToComplete = "Find out why Theater tickets aren't selling, and then fix it.";
            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["protectCampQuestComplete"])
            {
                completedQuest = true;
                specialConditions["Find out why Theater tickets aren't selling"] = true;
                specialConditions["Fix it"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            //Game1.Player.RemoveStoryItem("Ear Trumpet", 1);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}