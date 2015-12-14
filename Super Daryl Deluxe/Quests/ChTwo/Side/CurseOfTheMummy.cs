using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class CurseOfTheMummy : Quest
    {
        public CurseOfTheMummy(Boolean story)
            : base(story)
        {
            questDialogue.Add("Bahahahaha! I am FREE! This world will TREMBLE at the hands of King Hasbended!");
            questDialogue.Add("You! Monkey boy! You have freed me from my tomb, and you have earned the privilege of becoming my slave.");
            questDialogue.Add("When I was imprisoned I corrupted the lifeless souls in this pyramid. It has taken ages, but the curses have aged well. I can smell them.");
            questDialogue.Add("Ruin the bodies of the peasants buried here and bring me the curses they carry within, so that I may unleash them on the world. This is your task.");
            questDialogue.Add("Do not disappoint Kind Hasbended, or you will know true agony!");
            questDialogue.Add("Where are my curses, you feeble flesh-man? This world isn't going to destroy itself.");
            completedDialogue.Add("Bahahaha! At last, here they are! What sort of curse have I wrought upon this world?");
            completedDialogue.Add("Tell me, oh spirit of the cursed ones, what you have in store...");
            completedDialogue.Add("Hm? What is this? What in the name of Anubis is a \"toy-let\"? And what would compel a person to sit on one?");
            completedDialogue.Add("Bah! Begone from my sights while I decipher this curse! And take this musty book that I was buried with, the smell of it is making my skin rot!");

            rewardObjects.Add(new Textbook());
            rewardObjects.Add(new Experience(1005));

            npcName = "Kind Hasbended";

            itemNames.Add("Mummy's Curse");
            itemsToGather.Add(20);

            questName = "Curse of the Mummy";

            conditionsToComplete = "Find 20 Mummy's Curse to help destroy the world.";

            taskForQuestsPage = "Find 20 Mummy's Curse to help destroy the world.";

            descriptionForJournal = "Description pending";
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

            Game1.Player.RemoveDrops("Mummy's Curse", 20);

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

