using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    class InventoryQuest : Quest
    {
        Game1 game;

        public InventoryQuest(Boolean story, Game1 g)
            : base(story)
        {
            game = g;
            questDialogue.Add("Whoa there! Where is all of your equipment? Don't you know anything" +
                               " about 'Dwarves and Druids'?");
            questDialogue.Add("The Number One Rule: It's dangerous to go alo- ...uh, by yourself.");
            questDialogue.Add("I'm surprised you've survived as long as you have. You need someone to teach you about your inventory obviously.");
            questDialogue.Add("In D&D we all have a page in our handbook that represents our inventory. It holds a bunch of information, like our equipment, stats, and items.");
            questDialogue.Add("There are four types of equipment in D&D: Weapons, Hats, Outfits, and Accessories. Our " +
                              "inventory has special tabs for each type. Just click through them to view your equipment.");
            questDialogue.Add("Each type of equipment does something different for your stats. Weapons add strength, and sometimes " +
                              "you can dual wield them.");
            questDialogue.Add("Helmets add health, Chest Plates add defense, and accessories can do a whole bunch of things. " +
                              "You can even wear two accessories!");
            questDialogue.Add("Here, give it a try! Equip this mighty weapon through your inventory and talk to me when " +
                              "you've figured it out.");
            questDialogue.Add("Why haven't you equipped the weapon yet?");
            completedDialogue.Add("You look fierce already!");
            completedDialogue.Add("Keep collecting equipment and you'll be ready to explore dungeons in no time. " +
                                  "Remember that equipment has a level requirement though; the Game Master won't let you wear equipment that is too" +
                                  " strong for you.");
            rewardObjects.Add(new Karma(1));

            specialConditions.Add("Equip a mighty weapon", false);

            questName = "Learning about Equipment";

            npcName = "Equipment Instructor";

            conditionsToComplete = "Equip the Marker and talk to the D&D player again.";

            rewards = "1 Karma";

            taskForQuestsPage = "Equip a mighty weapon";

            descriptionForJournal = "There seems to be a bit of a discrepancy among the Dwarves and Druids players about what the number one rule is. Or what 'rule' even means. Luckily a student covered in weapons was in the Quad and explained to you how important the Second Amendment is. He picked up an old marker off the ground and handed it to you, which you then equipped to defend yourself against the atrocities of the world.";
        }

        public override void UpdateQuest()
        {
            base.UpdateQuest();

            if (!game.Prologue.PrologueBooleans["markerGiven"] && Chapter.effectsManager.notificationQueue.Count == 0)
            {
                game.Prologue.PrologueBooleans["markerGiven"] = true;
                Game1.Player.AddWeaponToInventory(new Marker());
                Chapter.effectsManager.AddFoundItem("a Dried Out Marker", Game1.equipmentTextures["Dried Out Marker"]);
            }

            if (Game1.Player.EquippedWeapon is Marker)
            {
                specialConditions["Equip a mighty weapon"] = true;
                completedQuest = true;
            }
            else
            {
                specialConditions["Equip a mighty weapon"] = false;
                completedQuest = false;
            }
        }

        public override void RewardPlayer()
        {
            base.RewardPlayer();

            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestCompleteNotification(this));
        }
    }
}

