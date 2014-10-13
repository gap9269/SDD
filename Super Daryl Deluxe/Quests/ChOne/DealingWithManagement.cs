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
    public class DealingWithManagement : Quest
    {
        Game1 game;

        public DealingWithManagement(Boolean story, Game1 g)
            : base(story)
        {

            game = g;

            questDialogue.Add("You must be my new servant boy, yes. Well I'm afraid to inform you that management is gay shit balls and wants to kick me out. Go deal with them for me, and take this key to get backstage.");
            questDialogue.Add("Only return once you have a new contract for me!");
            questDialogue.Add("Only return once you have a new contract for me!");

            completedDialogue.Add("You are a life saver, servent boy.");

            questName = "Dealing with Management";

            conditionsToComplete = "Get a renewed contract for Beethoven.";
            descriptionForJournal = "blahblah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.Player.StoryItems.ContainsKey("Renewed Contract"))
            {
                completedQuest = true;
            }

            if (game.ChapterOne.ChapterOneBooleans["givenBackstageKey"] == false)
            {
                Game1.Player.AddStoryItem("Bronze Key", "a Bronze Key", 1);
                game.ChapterOne.ChapterOneBooleans["givenBackstageKey"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}