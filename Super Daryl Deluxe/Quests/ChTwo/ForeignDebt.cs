using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class ForeignDebt : Quest
    {
        public ForeignDebt(Boolean story)
            : base(story)
        {
            questDialogue.Add("*sigh* Alright. As you know, Genghis is quite angry at me. Furious, really. And with good reason: I owe him some money.");
            questDialogue.Add("Normally such a thing would not be a problem for me, but I seem to have drained my treasury purchasing gifts for the beautiful Cleopatra, my one and only love.");
            questDialogue.Add("Even if I wanted to give that dirty man his money, I simply couldn't. Not yet.");
            questDialogue.Add("However, you must be handy if he sent you in that giant pile of wood instead of coming himself. I can't take the risk of opening the gates now that he's expecting them to be opened, but you can use this glowing floaty thing behind me to go to another world.");
            questDialogue.Add("Once you're there you should be able to find some way of getting some money for me to pay your uncle back with.");
            questDialogue.Add("Good luck, and bring me back some flowers or something for Cleopatra if you get the chance. Women love flowers.");
            questDialogue.Add("I'm quite serious, boy! Right through that portal with you. I must be rid of this debt, my Cleopatra is waiting!");

            completedDialogue.Add("I see you have returned, how did you fare in th-- OH SWEET MOTHER OF ROME!");
            completedDialogue.Add("Where did you get this much money?! Did you loot a king's treasury? *cough* --Ahem, yes, that should settle my debt with Genghis quite nicely. Well done.");
            completedDialogue.Add("Why don't you head on back now and let Napoleon know that I'll be joining your little club?");

            npcName = "Julius Caesar";

            specialConditions.Add("Find money for Caesar", false);

            questName = "Foreign Debt";

            conditionsToComplete = "Find money so Caesar can pay back his debt to Genghis Khan.";

            taskForQuestsPage = "Find money so Caesar can pay back his debt to Genghis Khan.";

            descriptionForJournal = "Description pending";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["moneyReceived"])
            {
                completedQuest = true;
                specialConditions["Find money for Caesar"] = true;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Game1.g.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] = true;
            Game1.Player.RemoveStoryItem("Ten Bucks", 1);

            Game1.g.ChapterTwo.NPCs["Genghis"].MapName = "The Great Wall";
            Game1.g.ChapterTwo.NPCs["Genghis"].RecX = 1623;
            Game1.g.ChapterTwo.NPCs["Genghis"].RecY = 650;
            Game1.g.ChapterTwo.NPCs["Genghis"].PositionX = 1623;
            Game1.g.ChapterTwo.NPCs["Genghis"].PositionY = 650;
            Game1.g.ChapterTwo.NPCs["Genghis"].ClearDialogue();
            Game1.g.ChapterTwo.NPCs["Genghis"].FacingRight = false;
            Game1.g.ChapterTwo.NPCs["Genghis"].Dialogue.Add("Kublai! What took you so long? Where is that Roman rat?");
        }
    }
}

