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
    public class FundRaising : Quest
    {
        Game1 game;

        public FundRaising(Boolean story, Game1 g)
            : base(story)
        {

            game = g;

            questDialogue.Add("Well done, you certainly have earned your weekly allowance. It is truly a shame that I am completely broke.");
            questDialogue.Add("I'll make a deal with you. I certainly do not enjoy having you stare at me while I try to work, and I can see you would like to be compensated for your tasks.");
            questDialogue.Add("I recently met a man who described himself to me as a, \"True Business Man\". I'm pleased to say that I quickly won over his approval with a sample from \"Fidelio\".");
            questDialogue.Add("Since then we have been business partners, you see. We reached an agreement wherein he funds my operas and pays for my stay at the Theater An Der Wien, and I do what I do best: make beautiful music.");
            questDialogue.Add("It is about time that Leonardo sends me some of this funding that he promised. Why don't you run along and find the man? If you let him know you are my servant-boy he will be sure to send some money back with you.");
            questDialogue.Add("When you are done I will make sure you are generously compensated for your efforts.");
            questDialogue.Add("Have you found my friend Leonardo yet? There is money to be gotten, boy! On with it!");

            questName = "Fund Raising";
            npcName = "Beethoven";

            specialConditions.Add("Get funding from Leonardo for Beethoven", false);

            conditionsToComplete = "Get funding from Leonardo for Beethoven.";
            taskForQuestsPage = "Get funding from Leonardo for Beethoven.";
            descriptionForJournal = "Pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"] || game.ChapterOne.ChapterOneBooleans["soldPaintingToArtDealer"] && completedQuest == false)
            {
                completedQuest = true;
                specialConditions["Sell Da Vinci's painting"] = true;
                specialConditions["Get funding from Leonardo for Beethoven"] = true;

                completedDialogue.Clear();

                if (game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"])
                {
                    completedDialogue.Add("Ah, there you are. Did you speak to my partner an--SWEET JESUS!");
                    completedDialogue.Add("How did the man have this much money?! And he gave this all to you?? You are sure you didn't rob a baron or ransack the Empress' treasury?");
                    completedDialogue.Add("My word, I was going to write ten symphonies, but now I suspect I will be able to retire after nine!");
                    completedDialogue.Add("You have my gratitude, servant-boy. Ahem...unfortunately it is against the law for me to provide you with currency as repayment for your deeds. It is a new law, you see. One that a servant-boy such as yourself does not need to concern himself with.");
                    completedDialogue.Add("However, I do believe in treating my servants fairly and I can tell that what you truly desire is an instrument to play yourself. Perhaps to win over a maiden's heart? I will present you with a most generous gift: my prized harpsichord.");
                    completedDialogue.Add("Go on, take it. You have earned it.");
                    completedDialogue.Add("It has been a pleasure having you serve me, but I now require peace and quiet to finish my beautiful \"Fidelio\". You are dismissed.");
                }
                else
                {
                    completedDialogue.Add("Ah, there you are. Did you speak to my partner and receive the funding he owes me?");
                    completedDialogue.Add("...Hmmm. That is all? 250 Ducats is the best that greedy man could do? I am the one doing all of the work, how dare he keep me living on such feeble pay!");
                    completedDialogue.Add("Bah, no matter. You have done your job, as was required of you. In return I shall give you my old harpsichord. It is missing a key here and there, but I suspect the termites haven't rendered it unfit to play just yet.");
                    completedDialogue.Add("Go on, take it. It is a prize fitting your efforts.");
                    completedDialogue.Add("You may not have been the best servant that I have had serve me, but I can tell you tried. I now require peace and quiet to finish my beautiful \"Fidelio\". You are dismissed.");

                }
            }
        }

        public override void RewardPlayer()
        {
            rewardObjects.Add(new Harpsichord(0, 0));

            base.RewardPlayer();

            game.ChapterOne.NPCs["Beethoven"].Dialogue.Clear();
            game.ChapterOne.NPCs["Beethoven"].Dialogue.Add("Perhaps you are confused as to who the deaf one is, servant. I said you are dismissed.");
            game.ChapterOne.ChapterOneBooleans["completedFundingQuest"] = true;
            if (game.ChapterOne.ChapterOneBooleans["soldPaintingToTrenchcoat"])
            {
                Game1.Player.RemoveStoryItem("Ten Dollars", 1);
            }
            else
            {
                Game1.Player.RemoveStoryItem("250 Ducats", 1);
            }
            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}