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
    public class DemoQuestOne : Quest
    {

        public DemoQuestOne(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hi Daryl! I'm your friend and I have a problem that you can solve! You can tell because I had that bouncy red circle above my head!");
            questDialogue.Add("Daryl, friends love flowers. But my flowers are currently being destroyed by a bunch of evil monsters in my garden!");
            questDialogue.Add("I just wish there was a way for you to stop them! If only you had a set of skills that could be used to smite the creatures stomping around my precious flowers.");
            questDialogue.Add("I mean, that'd be great. But the chances of the next four rooms being some convenient sequence of tasks that teach you how to fight is pretty low. I guess there's nothing you can do.");
            questDialogue.Add("If only you could somehow stumble upon a convenient chain of tutorial-like tasks to teach you how to fight monsters. Oh well.");

            questName = "A Convenient Sequence of Tasks";

            specialConditions.Add("-Break into a locker for a key", false);
            specialConditions.Add("-Find a Physics Textbook", false);
            specialConditions.Add("-Buy and equip a Skill", false);

            conditionsToComplete = "-Break into a locker and steal a key\n-Find a Physics Textbook\n-Buy and equip a skill";
            descriptionForJournal = "blahblah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.mapBooleans.tutorialMapBooleans["FinishedTutorialLocker"] == true)
                specialConditions["-Break into a locker for a key"] = true;

            if (Game1.mapBooleans.tutorialMapBooleans["FoundTextbook"] == true)
                specialConditions["-Find a Physics Textbook"] = true;

            if (Game1.Player.EquippedSkills.Count > 0)
            {
                specialConditions["-Buy and equip a Skill"] = true;
                completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}