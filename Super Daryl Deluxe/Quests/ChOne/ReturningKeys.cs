using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class ReturningKeys : Quest
    {

        public ReturningKeys(Boolean story)
            : base(story)
        {
            questDialogue.Add("In times like these we need to rise up above the rest and prove that we aren't just a bunch of \nbrainless minions doing the administration's work for them. Sometimes we get backlash, but we must, MUST, do what we can to \ncontinue fighting the good fight.");
            questDialogue.Add("That's why you need to go to the janitor's closet and return that key that we stole. And be quick \nabout it; you only have a few hours until the end of the day.");
            questDialogue.Add("Don't let us down, buddy.");

            questName = "Returning Keys";

            conditionsToComplete = "Return the Closet Key and the Janitor's Keys to the Janitor's Closet";
            descriptionForJournal = "blahblah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}