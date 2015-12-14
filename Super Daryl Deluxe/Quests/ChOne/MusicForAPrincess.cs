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
    public class MusicForAPrincess : Quest
    {

        public MusicForAPrincess(Boolean story)
            : base(story)
        {
            questDialogue.Add("Well that took you long enough. Who in their right mind would keep a princess waiting so long?");
            questDialogue.Add("...*sniff* Ugh! And you smell like cheap booze! Have you been getting drunk instead of getting me my instrument? You really want Daddy to rip you limb from limb, don't you? Because he will, all I have to do is say so.");
            questDialogue.Add("So let's see it, stupid boy. What instrument fit for a princess did you bring me?");
            questDialogue.Add("...");
            questDialogue.Add("...What the hell is this? Is this some sort of joke? An old piano?");
            questDialogue.Add("Where did you even find this? How on Earth did you carry it with you?");
            questDialogue.Add("Ugh! First you ruin my bed with your filthy shoes, and now you bring me oversized firewood instead of a real instrument. Princesses should play flutes, or harps, or ocarinas, stuff like that! Go back and get me something that I can actually play, or I'll tell Daddy all about the hole you put in my ceiling!");

            questDialogue.Add("If you stand on my bed again I'll scream.");

            questName = "Music For a Princess";

            specialConditions.Add("Find a better instrument for The Princess", false);

            taskForQuestsPage = "Find a better musical instrument for The Princess.";

            conditionsToComplete = "Find a better musical instrument for The Princess.";

            rewards = "";

            npcName = "The Princess";

            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!Game1.g.ChapterOne.ChapterOneBooleans["addedNapoleon"])
                Game1.g.ChapterOne.ChapterOneBooleans["addedNapoleon"] = true;

            if (Game1.Player.StoryItems.ContainsKey("Recorder"))
            {
                completedQuest = true;
                specialConditions["Find a better instrument for The Princess"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
            Game1.Player.RemoveStoryItem("Recorder", 1);
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));

            //Add second quest to the player's quest list and to the princess
        }
    }
}