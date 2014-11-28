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
    public class TutorialQuestFive : Quest
    {

        public TutorialQuestFive(Boolean story)
            : base(story)
        {
            questDialogue.Add("What took you so long? We found it forever ago.");
            questDialogue.Add("The Janitor's Closet is in the East Hall, and his keys should be in there somewhere.");
            questDialogue.Add("Take the keys and go get us some textbooks!");
            questDialogue.Add("Did you get into the science room yet?");

            questName = "Starting a Business 2";

            taskForQuestsPage = "Steal the Janitor's keys, then find a Textbook to trade for a second skill.";
            specialConditions.Add("Steal the Janitor's keys", false);
            specialConditions.Add("Find a Textbook", false);
            specialConditions.Add("Equip a second skill", false);

            npcName = "Paul";

            conditionsToComplete = "Get the janitor's keys\n-Find a physics book for Paul and Alan\nEquip a second skill";
            descriptionForJournal = "Paul and Alan have become textbook entrepreneurs and needed your help to get their first stock of product. This required you to steal a ring of keys from the Janitor's Closet to break into the science room. Seems like a lot of crime for your first day.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.Player.StoryItems.ContainsKey("Key Ring"))
                specialConditions["Steal the Janitor's keys"] = true;
            if (Game1.Player.Textbooks > 0 || Game1.Player.LearnedSkills.Count > 1)
                specialConditions["Find a Textbook"] = true;
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

           // Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}