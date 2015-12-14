using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class AStarIsBorn : Quest
    {
        public AStarIsBorn(Boolean story)
            : base(story)
        {
            questDialogue.Add("Oh those silly ghosts! Up to their antics as always. I do love games, but aren't I completely preoccupied with the 50 new ideas I've just had in the last half hour!");
            questDialogue.Add("I turn away for not more than a minute to write some of those ideas down, and when I turn back my newest composition is gone!");
            questDialogue.Add("Haha, but what are you going to do? I'm not supposed to talk to strangers, but you're my friend now. Could you go ask those ghosts to give me my compositions back?");
            questDialogue.Add("If I wasn't so busy with new compositions I'd go ask those ghosts for my music back myself!");

            completedDialogue.Add("Thank you, mister! I managed to write a few symphonies while you were gone, so it wasn't a complete waste of time.");
            completedDialogue.Add("Now that I have this composition back I can finally finish it. Be sure to be on the look out for \"Twinkle, Twinkle Little Star\" soon.");

            rewardObjects.Add(new ComposersWand());
            rewardObjects.Add(new Experience(170));
            rewardObjects.Add(new Karma(2));

            itemNames.Add("Sheet Music");
            itemsToGather.Add(5);

            npcName = "Mozart";

            questName = "A Star is Born";

            conditionsToComplete = "Recover 5 of Mozart's stolen Sheet Music";

            taskForQuestsPage = "Recover 5 of Mozart's stolen Sheet Music";

            rewards = "Composer's Wand\n20 Experience\n2 Karma";

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

            Game1.Player.RemoveDrops("Sheet Music", 5);

            Game1.g.SideQuestManager.nPCs["Mozart"].Dialogue.Clear();
            Game1.g.SideQuestManager.nPCs["Mozart"].Dialogue.Add("Thank you again for the help, mister! I wish I could go home and work there so ghosts would stop stealing my compositions.");
            Game1.g.SideQuestManager.nPCs["Mozart"].Dialogue.Add("I can't though because the man with the blue glasses says I need to stay here and work on my masterpieces...what a fun challenge! Don't you agree?");

            Game1.g.SideQuestManager.nPCs["Mozart"].Dialogue.Add("Maybe he'll like \"Twinkle, Twinkle Little Star\" so much that he'll say I can go home!");

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

