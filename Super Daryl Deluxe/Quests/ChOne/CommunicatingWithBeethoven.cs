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
    public class CommunicatingWithBeethoven : Quest
    {

        public CommunicatingWithBeethoven(Boolean story)
            : base(story)
        {
            questDialogue.Add("WHAT?");

            questDialogue.Add("HUH?");

            questDialogue.Add("WHAT?");

            completedDialogue.Add("MY EAR HORN!");
            completedDialogue.Add("Thank you, friend.");

            questName = "Communicating with Beethoven";

            conditionsToComplete = "Find a way to communicate with Beethoven.";
            descriptionForJournal = "blahblah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            //if (Game1.Player.StoryItems.ContainsKey("Beethoven's Ear Horn"))
            //{
                completedQuest = true;
            //}
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}