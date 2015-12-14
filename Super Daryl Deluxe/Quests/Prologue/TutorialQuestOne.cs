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
    public class TutorialQuestOne : Quest
    {

        public TutorialQuestOne(Boolean story)
            : base(story)
        {
            questDialogue.Add("Yeah, you owe me.");
            questDialogue.Add("If you don't get that paper for my mother, we can't be friends.");
            questDialogue.Add("Where's our piece of paper? Maybe you don't want to be friends.");
            //completedDialogue.Add("Thanks! Here's your reward!");

            itemNames.Add("Piece of Paper");
            itemsToGather.Add(1);

            itemNames.Add("Dandelion");
            itemsToGather.Add(3);

            taskForQuestsPage = "Go to the Quad for a Piece of Paper and Dandelions";

            questName = "Daryl's New Friends";

            npcName = "Paul";

            conditionsToComplete = "Find the piece of paper that Tim threw into the Quad. While you're there, pick some dandelions."
                + "\n\nMaybe this will get you some friends!";
            descriptionForJournal = "Paul and Alan must put their quest from God on hold until a piece of paper that they lost is back in their hands. They need you to find it and pick a bouquet of dandelions in return for their friendship. Friends love flowers!";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();
            List<Boolean> completedParts = new List<bool>();

            if (Game1.g.Notebook.ComboPage.LockerCombos.Count > 0 && !Game1.Player.StoryItems.ContainsKey("Piece of Paper"))
                Game1.Player.AddStoryItemWithoutPopup("Piece of Paper", 1);

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
            Game1.Player.RemoveStoryItem("Piece of Paper", 1);
        }
    }
}
