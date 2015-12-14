using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class JoiningForcesPartOne : Quest
    {
        public JoiningForcesPartOne(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh-hoh! Just in time. My soldiers have finally pushed a way to ze enemy's camp. Of course we do not have ze forces to attack, but we are in a much better position now.");
            questDialogue.Add("We-- ....Why are you staring at me with ze look of a brainless infidel? Do not tell me you do not know about ze plight zat my people are facing?");
            questDialogue.Add("*sigh* How can one be so uninformed in a time zuch as zis? Have you not noticed ze war happening all across zis new, mashed up world we find ourselves in? It must not have been more zan a week in zis place before zat bastard warlord showed up with his army of monsters!");
            questDialogue.Add("Whoever he is, he possess great power and extensive knowledge of war strategy. Zis man has held back my forces at every turn and has managed to, for ze most part, confine us to zis small camp. I zuzpect zat he has already begun destroying ze rest of ze classroom, but what can I do?");
            questDialogue.Add("I am ze only person here zat is man enough to stand up to ze fiend. Zat blasted Consul Caesar is too busy conquering useless land to take notice, and not one of my messengers has returned with word from Cleopatra. I fear ze worst is happening in Egypt, but I cannot help without giving up ground here.");
            questDialogue.Add("As I was saying before, my men have finally managed to set up a small camp close to ze enemy. The land in between is still riddled with ze enemy's monsters, but if we were to have a major force we could counterattack.");
            questDialogue.Add("Zis is where you come in. I have reason to believe zat if we have Cleopatra on our side zat Caesar would take a break from his idiotic games and join forces with us as well. I have forged a letter from Cleopatra to Caesar claiming zat she has joined me.");
            questDialogue.Add("Find Caesar and give him zis letter. It is our only hope.");
            questDialogue.Add("I believe Caesar was conquering China, last I heard of him.");

            completedDialogue.Add("You're back! Excellent, I can only imagine that Caesar and his troupe are not too far behind.");

            questName = "Joining Forces-Part 1";

            specialConditions.Add("Convince Caesar to join forces with \nNapoleon", false);

            conditionsToComplete = "Convince Caesar to join forces with Napoleon";

            taskForQuestsPage = "Convince Caesar to join forces with Napoleon";

            npcName = "Napoleon";
            rewards = "";

            descriptionForJournal = "Napoleon and bladfgsdfg blah";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!Game1.Player.StoryItems.ContainsKey("Letter to Caesar"))
                Game1.Player.AddStoryItem("Letter to Caesar", "an envelope", 1);

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
                completedQuest = true;

            if (completedQuest)
            {
                specialConditions["Convince Caesar to join forces with \nNapoleon"] = true;
            }

        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();
        }
    }
}
