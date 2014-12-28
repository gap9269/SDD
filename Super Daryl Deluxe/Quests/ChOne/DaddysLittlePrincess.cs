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
    public class DaddysLittlePrincess : Quest
    {

        public DaddysLittlePrincess(Boolean story)
            : base(story)
        {
            questDialogue.Add("Let's make a deal, stupid boy. I won't tell my Daddy about you finding me if you promise to go find me one of my \ninstruments that he took away. He says that it's dangerous to express myself around here, and it's especially dangerous to make \nnoise because people might find me.");

            questDialogue.Add("But I don't care! I'm so bored here and I want them back! I bet he locked them up in the Music Room. You did a \npretty good job of breaking in here, so I'm sure you can figure out how to get into the Music room. You'll know they're my \ninstruments when you see them. Go!");

            questDialogue.Add("I'm going to tell Daddy you found me if you don't hurry up. And he will be soooo mad.");

            questName = "Daddy's Little Princess";

            conditionsToComplete = "Go to the Music room and find one of the Princess's instruments.";
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