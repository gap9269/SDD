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
    public class BookSprayQuest : Quest
    {

        public BookSprayQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("What's the difference between four and five counts of theft, anyway? You may as well make the most of it at this point.");
            questDialogue.Add("That reminds me, I should tell you about our Employee Task List. Paul and I are very busy running things behind the scenes and dealing with the side of things that you wouldn't understand. As our new employee we are going to need you to handle some of the more menial tasks.");
            questDialogue.Add("Every time something new pops up we'll have it posted on that board over there, to the right of your locker. These tasks may seem simple, but they're important. Who knows what opportunities may arise that we'll need you to take care of for us?");
            questDialogue.Add("We won't pay you for them of course, as it is your job as an employee to do them. I'm sure we can work out some sort of incentive for each task to be completed, though.");
            questDialogue.Add("And one last thing-- I don't want you getting confused and mixing up packages or business acquisitions, so you're only allowed to handle one of those tasks at a time. Prove yourself worthy and maybe we'll start letting you do less boring stuff.");
            questDialogue.Add("Well, that's it. Get to work, and we better see that Book Spray by the end of the day.");
            questDialogue.Add("Don't forget to finish all of the tasks on the Employee Task List over there by your locker. Balto gets at least a couple done a day, and he's the most hopeless human being at this school.");

            itemNames.Add("Book Spray");
            itemsToGather.Add(1);

            taskForQuestsPage = "Get Book Spray from the Janitor's Closet";

            questName = "Righting Your Wrongs";

            npcName = "Alan";

            conditionsToComplete = "";
            descriptionForJournal = "After the short crime spree that was your first day of school, Paul and Alan needed you to find Book Spray in the janitor's closet so they could clean their textbook inventory and your crime-ridden soul.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            List<Boolean> completedParts = new List<bool>();

            for (int i = 0; i < itemNames.Count; i++)
            {
                if (Game1.Player.EnemyDrops.ContainsKey(itemNames[i]) && Game1.Player.EnemyDrops[itemNames[i]] >= itemsToGather[i] ||
                    Game1.Player.StoryItems.ContainsKey(itemNames[i]) && Game1.Player.StoryItems[itemNames[i]] >= itemsToGather[i])
                {
                    completedParts.Add(true);
                }
                else
                    completedParts.Add(false);
            }

            for (int i = 0; i < completedParts.Count; i++)
            {
                if (completedParts[i] == false)
                {
                    completedQuest = false;
                    break;
                }

                if (i == completedParts.Count - 1)
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