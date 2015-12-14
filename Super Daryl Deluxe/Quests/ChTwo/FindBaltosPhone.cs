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
    public class FindBaltosPhone : Quest
    {

        public FindBaltosPhone(Boolean story)
            : base(story)
        {
            questDialogue.Add("Alright Junior Baby Intern, this is officially your fault. You have to get Balto's phone back, otherwise you're fired.");
            questDialogue.Add("Go find Robatto in that old History room and get Balto's phone back. He's pretty easy to confuse, just ask him some unsolvable riddle and he'll freeze right up.");
            questDialogue.Add("Get to it then, this one is easy. Even you can't screw this up.");
            questDialogue.Add("What are you standing around here for? He's just a vice principal, nothing to be afraid of.");

            specialConditions.Add("Get Balto's phone back from Mr. Robatto", false);

            taskForQuestsPage = "Get Balto's phone back from Mr. Robatto";

            questName = "Recovering Balto's Phone";

            npcName = "Paul";

            descriptionForJournal = "After being demoted to \"Junior Baby Intern\" Balto showed up and explained that his phone was confiscated by Mr. Robatto.";
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