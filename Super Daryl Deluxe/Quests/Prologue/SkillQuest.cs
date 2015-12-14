using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class SkillQuest : Quest
    {
        public SkillQuest(Boolean story)
            : base(story)
        {
            questDialogue.Add("Hey there handsome, you look like you know how to interact with others.");
            questDialogue.Add("In 'Dwarves and Druids' players need to work together to get things done! Of course, that goes hand in hand with the Number One Rule: getting the best skills!");
            questDialogue.Add("In D&D, we can have up to four skills equipped at a time. There lots of different types of skills we can choose from. Some can be used immediately, but some need to charge first. My favorite skill is 'Hat Summoning.'");
            questDialogue.Add("With a flick of my wand I can summon any hat I want...They're really mean though, and sometimes a bit hard to wear without fighting first.");
            questDialogue.Add("You look like the sort of guy that already knows a skill, let me summon a mean hat for you to try it out on. Don't worry about getting a little sweaty, teehee...");
            questDialogue.Add("Go on, beat up that hat next to me. Don't you wanna impress me? *wink*");
            completedDialogue.Add("The way you destroyed that fez was so manly. If you hang around with me I could show you some new skills. *wink*");

            rewardObjects.Add(new Fez());
            rewardObjects.Add(new Karma(1));
            enemiesToKill.Add(1);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Fez");

            npcName = "Skill Instructor";

            questName = "Learning about Skills";

            taskForQuestsPage = "Learn about Skills";

            conditionsToComplete = "Destroy the fez";

            rewards = "1 Fez\n1 Karma";

            descriptionForJournal = "A girl that looked like your type stood next to your locker and explained to you how important skills are. To prove your masculinity, you destroyed a fez...and the innocent goblin wearing it. Boy, was she impressed! I think with a bit of work you two could go far. *wink*";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (Game1.schoolMaps.maps["North Hall"].EnemiesInMap.Count == 0 && !Game1.g.Prologue.PrologueBooleans["addBox"])
            {
                Game1.schoolMaps.maps["North Hall"].AddEnemyToEnemyList(new FezGoblin(new Microsoft.Xna.Framework.Vector2(3695, 500), "Fez", Game1.g, ref Game1.g.Prologue.player, Game1.schoolMaps.maps["North Hall"]));
                Game1.g.Prologue.PrologueBooleans["addBox"] = true;
            }

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                CompletedQuest = true;
                Game1.g.Prologue.PrologueBooleans["addedBox"] = true;
            }
            
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}