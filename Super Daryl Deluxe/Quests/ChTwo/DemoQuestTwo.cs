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
    public class DemoQuestTwo : Quest
    {

        public DemoQuestTwo(Boolean story)
            : base(story)
        {
            questDialogue.Add("Wow! Those maps were actually a chain of tutorial-like tasks to get you a skill! What are the odds?");
            questDialogue.Add("Now you are prepared to clear my garden of those nasty beasts.");
            questDialogue.Add("I hope they haven't destroyed my precious fruit yet. Quick! Go save my fruit!");
            questDialogue.Add("I hope they haven't destroyed my precious fruit yet.");
            completedDialogue.Add("You did it! And my precious flying melons were spared as well. Thank you, Daryl!");
            
            questName = "Clearing the Garden";

            specialConditions.Add("-Clear your friend's garden and save \nthe fruit!", false);

            conditionsToComplete = "-Clear your friend's garden and save the fruit!";
            descriptionForJournal = "A bunch of weird monsters were in your friend's garden, so you destroyed them. Good job";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();


            if (Game1.mapBooleans.tutorialMapBooleans["ClearedGarden"] == true)
            {
                if (Game1.mapBooleans.tutorialMapBooleans["DestroyedAllFruit"] == true)
                {
                    completedDialogue[0] = "*sob*  I just checked my fruit garden. Those bastard monsters destroyed all \nof my precious flying melons. They were my prized possessions, Daryl. I don't \nknow what I'll do without them...my life is over";
                }
                else if (Game1.mapBooleans.tutorialMapBooleans["DestroyedSomeFruit"] == true)
                {
                    completedDialogue[0] = "Why did they have to destroy my precious flying melons?? Why?? *sigh*...At least \na few were spared. Thanks for killing the monsters.";
                }

                specialConditions["-Clear your friend's garden and save \nthe fruit!"] = true;
                completedQuest = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            if (Game1.mapBooleans.tutorialMapBooleans["DestroyedSomeFruit"] == false)
            {
                rewardObjects.Add(new ILoveMelons());
                rewardObjects.Add(new DirtyBrokenHoe());
                Game1.Player.AddShirtToInventory(new ILoveMelons());
                Game1.Player.AddWeaponToInventory(new DirtyBrokenHoe());

            }
            else if (Game1.mapBooleans.tutorialMapBooleans["DestroyedAllFruit"] == false)
            {
                rewardObjects.Add(new DirtyBrokenHoe());
                Game1.Player.AddWeaponToInventory(new DirtyBrokenHoe());
                Game1.currentChapter.NPCs["YourFriend"].Dialogue[0] = "I'll have to make due with what melons I have left, I suppose...";
            }
            else
            {
                Game1.currentChapter.NPCs["YourFriend"].Dialogue[0] = "I'll never be happy again.";
            }

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}