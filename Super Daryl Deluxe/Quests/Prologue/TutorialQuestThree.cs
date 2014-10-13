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
    public class TutorialQuestThree : Quest
    {

        public TutorialQuestThree(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh and by the way, don't carry that book around with you.");
            questDialogue.Add("It's the schools only copy and if they find out you stole it and ripped the pages out of it they'll probably send you to jail.");
            questDialogue.Add("Do you know what happens in jail to kids like you? I suggest leaving it in your locker.");
            questDialogue.Add("I can tell you're kind of an idiot though, so maybe it would be smart to carry around some of the pages with you.");
            questDialogue.Add("Now go get us some textbooks!");
            questDialogue.Add("Where's our textbooks?");

            taskForQuestsPage = "Equip your first skill, then find a Textbook.";
            specialConditions.Add("Equip 'Discuss Differences'", false);
            specialConditions.Add("Find a Textbook.", false);

            questName = "Starting a Business";

            npcName = "Paul";

            conditionsToComplete = "-Equip Discuss Differences\n-Find a physics book for Paul and Alan!";
            descriptionForJournal = "Paul and Alan have become textbook entrepreneurs and needed your help to get their first stock of product. This required you to steal a ring of keys from the Janitor's Closet to break into the science room. Seems like a lot of crime for your first day.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.Player.EquippedSkills.Count > 0)
                specialConditions["Equip 'Discuss Differences'"] = true;
            else
                specialConditions["Equip 'Discuss Differences'"] = false;
            /*
            if (enemiesKilledForQuest >= enemiesToKill)
            {
                completedQuest = true;
            }*/
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}
