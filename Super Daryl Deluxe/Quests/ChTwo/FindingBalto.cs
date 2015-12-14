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
    public class FindingBalto : Quest
    {

        public FindingBalto(Boolean story)
            : base(story)
        {
            questDialogue.Add("...Duuuuuuuude.");
            questDialogue.Add("Have you...like...seen Balto around or something, man?");
            questDialogue.Add("...He was like, supposed to accompany me in certain, uh, like...recreational activities, but he's temporarily M.I.A and stuff.");
            questDialogue.Add("Man, I can't handle this right now. Could you...like...go find him for me?");
            questDialogue.Add("Haha dude, have you, like, found Balto yet, man?");
            taskForQuestsPage = "Find Balto";
            questName = "Finding Balto";

            specialConditions.Add("Find Balto", false);

            conditionsToComplete = "Find Balto";
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