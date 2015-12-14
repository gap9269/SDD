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
            questDialogue.Add("HUH?");

            questDialogue.Add("WHAT?");

            questDialogue.Add("YOUR MOUTH IS OPEN BUT I DON'T HEAR ANY SOUNDS.");

            questDialogue.Add("I CANNOT HEAR YOU. I AM GROWING SOMEWHAT AUDITORILY CHALLENGED AS I NEAR MY OLDER YEARS.");

            questDialogue.Add("WHAT?");

            completedDialogue.Add("That is much better, you have my gratitude.");

            questName = "Speaking with Beethoven";

            npcName = "Beethoven";

            specialConditions.Add("Find a way to communicate with \nBeethoven", false);

            taskForQuestsPage = "Find a way to communicate with Beethoven.";

            conditionsToComplete = "Find a way to communicate with Beethoven.";
            descriptionForJournal = "You ran into your favorite dead Musician, but he couldn't hear you! You found his ear trumpet after breaking into his room with the help of the Transparanormal Investigation Team Squad, letting him communicate once again.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["spawnedGhostHunters"] == false)
                Game1.g.ChapterOne.ChapterOneBooleans["spawnedGhostHunters"] = true;

            if (Game1.Player.StoryItems.ContainsKey("Ear Trumpet"))
            {
                completedQuest = true;
                specialConditions["Find a way to communicate with \nBeethoven"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.Player.RemoveStoryItem("Ear Trumpet", 1);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));

        }
    }
}