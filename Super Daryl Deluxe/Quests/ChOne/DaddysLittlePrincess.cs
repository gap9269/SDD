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
            questDialogue.Add(".....");

            questDialogue.Add("What a surprise. Did you hit your head or something on the way down? Why were you crawling around in the vents anyway?");

            questDialogue.Add("...Actually, you can probably get anywhere from those vents, right? I have an idea, and maybe I won't have to tell anyone about you being here if you do me a favor.");

            questDialogue.Add("As generous as Daddy is, he has neglected to provide me with some outlet for creativity. Rather than trouble him about it, I think now that you've ruined my bed, you owe it to me to bring me back something fun.");

            questDialogue.Add("Yes - If you value your well-being, I suggest you crawl off into those vents and bring me back something entertaining. An instrument, in fact. Something fit for a princess. I've been feeling very musical lately.");

            questDialogue.Add("And don't you dare come back here until you have something to give me.");

            questDialogue.Add("UGH. Why are you still here??");

            completedDialogue.Add("Well that took you long enough. Who in their right mind would keep a princess waiting so long?");

            questName = "The Urge to Make Music";

            specialConditions.Add("Find a musical instrument for The Princess", false);

            taskForQuestsPage = "Find a musical instrument for The Princess.";

            conditionsToComplete = "Find a musical instrument for The Princess.";

            rewards = "";

            descriptionForJournal = "Pending";

            npcName = "The Princess";

        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["completedFundingQuest"])
            {
                completedQuest = true;
                specialConditions["Find a musical instrument for The Princess"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}