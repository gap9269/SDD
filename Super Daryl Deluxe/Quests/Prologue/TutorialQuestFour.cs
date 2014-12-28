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
    public class TutorialQuestFour : Quest
    {

        public TutorialQuestFour(Boolean story)
            : base(story)
        {
            questDialogue.Add("The door is locked?  Makes sense... luckily I stole the key to the janitor's closet. He should have keys to the science room in there.");
            questDialogue.Add("Unfortunately Alan's an idiot and lost the key in his locker, so we have to look for it.");
            questDialogue.Add("Why don't you go away so we can find it in peace?");
            questDialogue.Add("Jesus, dude! There are other students around here for you to bother, why don't you go meet some of them?");

            questName = "Exploring The School";

            taskForQuestsPage = "Meet the students of WFHS.";
            specialConditions.Add("Meet the students of WFHS.", false);
            npcName = "Paul";

            conditionsToComplete = "-Explore the school and meet new students while Paul and Alan find the Closet Key.";

            descriptionForJournal = "The science room was locked, but Paul and Alan had a key to the Janitor's closet where the classroom keys were kept...until Alan lost it. You met several other students while they looked for it.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if(Game1.g.CurrentQuests.Contains(Game1.g.Prologue.QuestThree))
            {
                Game1.g.Prologue.QuestThree.RewardPlayer();
                Game1.questHUD.RemoveQuestFromHelper(Game1.g.Prologue.QuestThree);
                Game1.g.CurrentQuests.Remove(Game1.g.Prologue.QuestThree);
            }
            
            if (Game1.currentChapter.CompletedSideQuests.ContainsKey("Learning about Karma") &&
                Game1.currentChapter.CompletedSideQuests.ContainsKey("Learning about Equipment") &&
                Game1.currentChapter.CompletedSideQuests.ContainsKey("Learning about your Journal") &&
                Game1.currentChapter.CompletedSideQuests.ContainsKey("Learning about Skills") &&
                Game1.currentChapter.CompletedSideQuests.ContainsKey("Learning about Saving"))
            {
                specialConditions["Meet the students of WFHS."] = true;
                completedQuest = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}