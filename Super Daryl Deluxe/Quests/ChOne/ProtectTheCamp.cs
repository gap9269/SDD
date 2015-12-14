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
    public class ProtectTheCamp : Quest
    {
        public ProtectTheCamp(Boolean story)
            : base(story)
        {
            questDialogue.Add("Curses! Boy, if you were one of zem I don't believe that you would be stupid enough to walk into ze enemy. Or, more likely, you would have killed us all by now.");
            questDialogue.Add("However, you remind me too much of zem to not be a coincidence. I believe if we can survive zis upcoming attack I can lift ze curfew off zis area and pursue more promising leads.");
            questDialogue.Add("I must return and aid my men now. If you would like to continue zis conversation, you can meet me...");
            questDialogue.Add("...in ze History Room!");

            questDialogue.Add("I do not have time for zis right now, boy! My camp is under attack by those disgusting creatures. Either help us or get out of here!");

            completedDialogue.Add("Well then...it would zeem that I was mistaken about you.");
            completedDialogue.Add("What you just witnessed was the result of ze war brewing here. (Something about the warlord showing up and making a monster army.)");
            completedDialogue.Add("(Trying to recruit allies, but so far I'm the only opposition. We could use your help to stop the warlord.)");
            completedDialogue.Add("(I've pulled my men from Vienna and lifted the curfew, I hope I can count on you in the future for help.)");

            questName = "Protect the Camp";

            npcName = "Napoleon";

            specialConditions.Add("Help protect Naploeon's Camp in the \nHistory Room", false);

            taskForQuestsPage = "Help protect Naploeon's Camp in the History Room.";

            conditionsToComplete = "Help protect Naploeon's Camp in the History Room.";
            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterOne.ChapterOneBooleans["battlefieldCleared"])
            {
                completedQuest = true;
                specialConditions["Help protect Naploeon's Camp in the \nHistory Room"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            //Game1.Player.RemoveStoryItem("Ear Trumpet", 1);

            Game1.g.ChapterOne.NPCs["Napoleon"].ClearDialogue();
            Game1.g.ChapterOne.NPCs["Napoleon"].Dialogue.Add("We're going to need your help to stop zis madman. His army grows stronger while we wait...we must act zoon!");

            Game1.g.ChapterOne.ChapterOneBooleans["protectCampQuestComplete"] = true;
            NapoleonsCamp.ToTrenchfootField.IsUseable = true;

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}