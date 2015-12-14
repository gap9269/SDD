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
    public class TutoringThePrincess : Quest
    {

        public TutoringThePrincess(Boolean story)
            : base(story)
        {

            questDialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");
            questDialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");
            questDialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");
            questDialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");
            questDialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");

            specialConditions.Add("Go speak with The Princess", false);

            taskForQuestsPage = "Go speak with The Princess";

            questName = "Tutoring The Princess";

            npcName = "The Princess";

            descriptionForJournal = "Principal Hangerman requested that you tutor The Princess, with the utmost secrecy.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}