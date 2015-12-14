using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;

//http://gavindraper.com/2010/11/25/how-to-loadsave-game-state-in-xna/

namespace ISurvived
{
    public class SaveLoadManager
    {
        Player player;
        Game1 game;

        int chapterStateTemp;

        //Misc
        List<String> generatedComboNamesTemp;
        List<String> generatedCombosTemp;

        //Player
        List<String> learnedSkillsTemp;
        List<Boolean> equippedSkillsTemp;
        List<SkillWrapper> skillwrapperTemp;
        SkillWrapper quickRetortTemp;
        List<String> storyItemsNameTemp;
        List<int> storyItemsNumTemp;
        List<String> enemyDropsNameTemp;
        List<int> enemyDropsNumTemp;
        List<String> lockerComboNamesTemp;
        List<String> lockerCombosTemp;
        List<String> characterBioNamesTemp;
        List<String> monsterBioNamesTemp;
        List<Boolean> characterBioTemp;
        List<Boolean> monsterBioTemp;
        List<String> passiveNamesTemp;
        List<String> equippedSkillNamesTemp;

        #region Equipment
        List<String> ownedWeaponsTemp;
        List<EquipmentWrapper> ownedWeaponWrappersTemp;
        List<String> ownedShirtsTemp;
        List<EquipmentWrapper> ownedShirtWrappersTemp;
        List<String> ownedHatsTemp;
        List<EquipmentWrapper> ownedHatWrappersTemp;
        List<String> ownedAccessoriesTemp;
        List<EquipmentWrapper> ownedAccessoryWrappersTemp;
        String weaponOneTemp;
        String weaponTwoTemp;
        String hatTemp;
        String shirtTemp;
        String accessoryOneTemp;
        String accessoryTwoTemp;
        EquipmentWrapper w1;
        EquipmentWrapper w2;
        EquipmentWrapper s;
        EquipmentWrapper h;
        EquipmentWrapper a1;
        EquipmentWrapper a2;
        #endregion

        //Quests
        List<QuestWrapper> allQuestsTemp;
        List<String> currentQuestsTemp;
        List<String> currentSideQuestsTemp;

        List<String> prologueCompletedSideTemp;
        List<String> prologueCompletedStoryTemp;

        List<String> chOneCompletedSideTemp;
        List<String> chOneCompletedStoryTemp;

        List<String> chTwoCompletedSideTemp;
        List<String> chTwoCompletedStoryTemp;

        //Skills
        List<String> skillsInShopTemp;

        //Chapter
        int currentMapZoneStateTemp;
        List<NPCWrapper> sideQuestNPCWrapperTemp;
        List<Boolean> prologueBooleanTemp;
        List<String> prologueBooleanNameTemp;
        List<NPCWrapper> prologueNPCWrapperTemp;

        List<NPCWrapper> chapterOneNPCWrapperTemp;
        List<Boolean> chapterOneBooleanTemp;
        List<String> chapterOneBooleanNameTemp;

        List<NPCWrapper> chapterTwoNPCWrapperTemp;
        List<Boolean> chapterTwoBooleanTemp;
        List<String> chapterTwoBooleanNameTemp;

        //Maps
        List<List<Boolean>> mapStoryItemTemp;
        List<List<Boolean>> mapCollectiblePickedTemp;
        List<List<Boolean>> mapCollectibleAbleTemp;
        List<List<Boolean>> mapChestTemp;
        List<List<Boolean>> portalLockedTemp;
        List<List<Vector2>> moveBlockPosTemp;
        List<List<Boolean>> switchActiveTemp;
        List<List<int>> mapEnemiesKilledTemp;
        List<Boolean> mapPrologueBooleansTemp;
        List<String> mapPrologueBooleanNamesTemp;
        List<List<List<Boolean>>> takenContentsTemp;
        List<Boolean> discoveredMapTemp;
        List<Boolean> mapChapterOneBooleansTemp;
        List<String> mapChapterOneBooleanNamesTemp;
        List<Boolean> mapChapterTwoBooleansTemp;
        List<String> mapChapterTwoBooleanNamesTemp;
        List<Boolean> mapTutorialBooleansTemp;
        List<String> mapTutorialBooleanNamesTemp;
        List<List<InteractiveWrapper>> mapInteractiveTemp;

        StorageDevice device;
        string containerName = "GameSaves";
        public string filename = "save1.sav";
        public string deleteFileName = "";

        public Boolean saveOne = false;
        public Boolean saveTwo = false;
        public Boolean saveThree = false;

        public String saveOnePreview;
        public String saveTwoPreview;
        public String saveThreePreview;

        [Serializable]
        public struct SaveGame
        {
            //Game
            public int chapterState; //0 = prologue, 1 = ch1, etc
            public String portalDestinationName;
            public Rectangle portalDestination;
            public List<NPCWrapper> sideQuestNPCs;
            public List<String> generatedComboNames;
            public List<String> generatedCombos;
            //Skills
            public List<Boolean> equippedSkills;
            public List<String> learnedSkills;
            public List<SkillWrapper> skillWrappers;
            public SkillWrapper quickRetort;
            public List<String> skillsInShop;
            //Player
            public String playerRank;
            public int karma;
            public int socialRankIndex;
            public int playerLevel;
            public double playerMoney;
            public List<String> storyItemsName;
            public List<String> enemyDropsName;
            public List<int> storyItemsNum;
            public List<int> enemyDropsNum;
            public int maxMotivation;
            public int tolerance;
            public int strength;
            public int experience;
            public int experienceUntilLevel;
            public int textbooks;
            public int statPoints;
            public int bronzeKeys, silverKeys, goldKeys;
            public Boolean hasPhone;
            public List<String> lockerComboNames;
            public List<String> lockerCombos;
            public List<String> characterBioNames;
            public List<String> monsterBioNames;
            public List<Boolean> characterBios;
            public List<Boolean> monsterBios;
            public List<String> passiveNames;
            public List<String> equippedSkillNames;

            public List<Boolean> prologueSideQuestsRead;
            public List<Boolean> prologueStoryQuestsRead;
            public Boolean prologueSynopsisRead;

            public List<Boolean> chOneSideQuestsRead;
            public List<Boolean> chOneStoryQuestsRead;
            public Boolean chOneSynopsisRead;

            public List<Boolean> chTwoSideQuestsRead;
            public List<Boolean> chTwoStoryQuestsRead;
            public Boolean chTwoSynopsisRead;
            //--Equipment
            public List<String> ownedWeapons;
            public List<EquipmentWrapper> ownedWeaponWrappers;
            public List<String> ownedShirts;
            public List<EquipmentWrapper> ownedShirtWrappers;
            public List<String> ownedHats;
            public List<EquipmentWrapper> ownedHatWrappers;
            public List<String> ownedAccessories;
            public List<EquipmentWrapper> ownedAccessoryWrappers;
            public String weaponOne;
            public String weaponTwo;
            public String hat;
            public String shirt;
            public String accessoryOne;
            public String accessoryTwo;
            public EquipmentWrapper wep1Wrap;
            public EquipmentWrapper wep2Wrap;
            public EquipmentWrapper hatWrap;
            public EquipmentWrapper shirtWrap;
            public EquipmentWrapper acc1Wrap;
            public EquipmentWrapper acc2Wrap;
            public Boolean newWeapon, newHat, newShirt, newAccessory, newLoot;
            //Quests
            public List<QuestWrapper> allQuests;
            public List<String> currentQuests;
            public List<String> currentSideQuests;
            public List<String> prologueCompletedSideQuests;
            public List<String> prologueCompletedStoryQuests;
            public List<String> chOneCompletedSideQuests;
            public List<String> chOneCompletedStoryQuests;
            public List<String> chTwoCompletedSideQuests;
            public List<String> chTwoCompletedStoryQuests;
            //Maps
            public List<List<Boolean>> mapStoryItemsPicked;
            public List<List<Boolean>> mapChestOpened;
            public List<List<Boolean>> portalsLocked;
            public List<List<Vector2>> moveBlockPositions;
            public List<List<Boolean>> switchesActive;
            public List<List<int>> mapEnemiesKilled;
            public List<Boolean> mapPrologueBooleans;
            public List<String> mapPrologueBooleanNames;
            public List<List<List<Boolean>>> takenContents;
            public List<Boolean> discoveredMap;
            public List<List<Boolean>> mapCollectiblePicked;
            public List<List<Boolean>> mapCollectibleAble;
            public List<Boolean> mapChapterOneBooleans;
            public List<String> mapChapterOneBooleanNames;
            public List<Boolean> mapChapterTwoBooleans;
            public List<String> mapChapterTwoBooleanNames;
            public List<List<InteractiveWrapper>> mapInteractiveObjects;
            public List<Boolean> mapTutorialBooleans;
            public List<String> mapTutorialBooleanNames;

            //Map zones
            public MapZoneWrapper schoolZoneWrapper;

            //Chapters
            public int currentMapZoneState; //Look at Chapter for the enum and numerals
            public int cutsceneState;
            public List<Boolean> prologueBooleans;
            public List<String> prologueBooleanNames;
            public String prologueSynopsis;
            public String chapterOneSynopsis;
            public List<Boolean> chapterOneBooleans;
            public List<String> chapterOneBooleanNames;

            public String chapterTwoSynopsis;
            public List<Boolean> chapterTwoBooleans;
            public List<String> chapterTwoBooleanNames;


            
            //NPCs
            public List<NPCWrapper> prologueNPCWrappers;
            public List<NPCWrapper> chapterOneNPCWrappers;
            public List<NPCWrapper> chapterTwoNPCWrappers;
        }

        public SaveLoadManager()
        {
        }

        public SaveLoadManager(Player p, Game1 g)
        {
            player = p;
            game = g;
            learnedSkillsTemp = new List<string>();
            equippedSkillsTemp = new List<Boolean>();
            skillwrapperTemp = new List<SkillWrapper>();
            quickRetortTemp = new SkillWrapper();

            sideQuestNPCWrapperTemp = new List<NPCWrapper>();

            prologueBooleanTemp = new List<bool>();
            prologueBooleanNameTemp = new List<string>();
            prologueNPCWrapperTemp = new List<NPCWrapper>();

            lockerComboNamesTemp = new List<string>();
            lockerCombosTemp = new List<string>();

            ownedAccessoriesTemp = new List<string>();
            ownedHatsTemp = new List<string>();
            ownedShirtsTemp = new List<string>();
            ownedWeaponsTemp = new List<string>();

            ownedWeaponWrappersTemp = new List<EquipmentWrapper>();
            ownedAccessoryWrappersTemp = new List<EquipmentWrapper>();
            ownedHatWrappersTemp = new List<EquipmentWrapper>();
            ownedShirtWrappersTemp = new List<EquipmentWrapper>();

            currentQuestsTemp = new List<string>();
            currentSideQuestsTemp = new List<string>();
            allQuestsTemp = new List<QuestWrapper>();
            prologueCompletedSideTemp = new List<string>();
            prologueCompletedStoryTemp = new List<string>();
            chOneCompletedSideTemp = new List<string>();
            chOneCompletedStoryTemp = new List<string>();
            chTwoCompletedSideTemp = new List<string>();
            chTwoCompletedStoryTemp = new List<string>();

            storyItemsNameTemp = new List<string>();
            storyItemsNumTemp = new List<int>();
            enemyDropsNameTemp = new List<string>();
            enemyDropsNumTemp = new List<int>();

            mapStoryItemTemp = new List<List<bool>>();
            mapChestTemp = new List<List<bool>>();
            portalLockedTemp = new List<List<bool>>();
            moveBlockPosTemp = new List<List<Vector2>>();
            switchActiveTemp = new List<List<bool>>();
            mapEnemiesKilledTemp = new List<List<int>>();
            mapPrologueBooleanNamesTemp = new List<string>();
            mapPrologueBooleansTemp = new List<bool>();
            takenContentsTemp = new List<List<List<Boolean>>>();
            discoveredMapTemp = new List<bool>();
            mapCollectiblePickedTemp = new List<List<bool>>();
            mapCollectibleAbleTemp = new List<List<bool>>();

            mapChapterOneBooleanNamesTemp = new List<string>();
            mapChapterOneBooleansTemp = new List<bool>();
            mapInteractiveTemp = new List<List<InteractiveWrapper>>();

            chapterOneNPCWrapperTemp = new List<NPCWrapper>();
            chapterOneBooleanNameTemp = new List<string>();
            chapterOneBooleanTemp = new List<bool>();

            chapterTwoBooleanNameTemp = new List<string>();
            chapterTwoBooleanTemp = new List<bool>();
            chapterTwoNPCWrapperTemp = new List<NPCWrapper>();

            mapChapterTwoBooleanNamesTemp = new List<string>();
            mapChapterTwoBooleansTemp = new List<bool>();

            mapTutorialBooleanNamesTemp = new List<string>();
            mapTutorialBooleansTemp = new List<bool>();

            characterBioTemp = new List<Boolean>();
            monsterBioTemp = new List<Boolean>();
            characterBioNamesTemp = new List<string>();
            monsterBioNamesTemp = new List<string>();
            passiveNamesTemp = new List<string>();

            skillsInShopTemp = new List<string>();

            generatedComboNamesTemp = new List<string>();
            generatedCombosTemp = new List<string>();
            equippedSkillNamesTemp = new List<string>();
        }

        public void ResetListsForSave()
        {
            //Player equipment
            weaponOneTemp = null;
            weaponTwoTemp = null;
            hatTemp = null;
            shirtTemp = null;
            accessoryOneTemp = null;
            accessoryTwoTemp = null;
            w1 = null;
            w2 = null;
            s = null;
            h = null;
            a1 = null;
            a2 = null;

            learnedSkillsTemp = new List<string>();
            equippedSkillsTemp = new List<Boolean>();
            skillwrapperTemp = new List<SkillWrapper>();
            quickRetortTemp = new SkillWrapper();

            sideQuestNPCWrapperTemp = new List<NPCWrapper>();

            prologueBooleanTemp = new List<bool>();
            prologueBooleanNameTemp = new List<string>();
            prologueNPCWrapperTemp = new List<NPCWrapper>();

            ownedAccessoriesTemp = new List<string>();
            ownedHatsTemp = new List<string>();
            ownedShirtsTemp = new List<string>();
            ownedWeaponsTemp = new List<string>();

            ownedWeaponWrappersTemp = new List<EquipmentWrapper>();
            ownedAccessoryWrappersTemp = new List<EquipmentWrapper>();
            ownedHatWrappersTemp = new List<EquipmentWrapper>();
            ownedShirtWrappersTemp = new List<EquipmentWrapper>();

            currentQuestsTemp = new List<string>();
            currentSideQuestsTemp = new List<string>();
            allQuestsTemp = new List<QuestWrapper>();
            prologueCompletedSideTemp = new List<string>();
            prologueCompletedStoryTemp = new List<string>();
            chOneCompletedSideTemp = new List<string>();
            chOneCompletedStoryTemp = new List<string>();
            chTwoCompletedSideTemp = new List<string>();
            chTwoCompletedStoryTemp = new List<string>();

            storyItemsNameTemp = new List<string>();
            storyItemsNumTemp = new List<int>();
            enemyDropsNameTemp = new List<string>();
            enemyDropsNumTemp = new List<int>();

            mapStoryItemTemp = new List<List<bool>>();
            mapChestTemp = new List<List<bool>>();
            portalLockedTemp = new List<List<bool>>();
            moveBlockPosTemp = new List<List<Vector2>>();
            switchActiveTemp = new List<List<bool>>();
            mapEnemiesKilledTemp = new List<List<int>>();
            mapPrologueBooleanNamesTemp = new List<string>();
            mapPrologueBooleansTemp = new List<bool>();
            takenContentsTemp = new List<List<List<Boolean>>>();

            lockerComboNamesTemp = new List<string>();
            lockerCombosTemp = new List<string>();
            discoveredMapTemp = new List<bool>();
            mapCollectiblePickedTemp = new List<List<bool>>();
            mapCollectibleAbleTemp = new List<List<bool>>();
            mapChapterOneBooleanNamesTemp = new List<string>();
            mapChapterOneBooleansTemp = new List<bool>();
            mapInteractiveTemp = new List<List<InteractiveWrapper>>();

            chapterOneNPCWrapperTemp = new List<NPCWrapper>();
            chapterOneBooleanNameTemp = new List<string>();
            chapterOneBooleanTemp = new List<bool>();

            chapterTwoBooleanNameTemp = new List<string>();
            chapterTwoBooleanTemp = new List<bool>();
            chapterTwoNPCWrapperTemp = new List<NPCWrapper>();

            mapChapterTwoBooleanNamesTemp = new List<string>();
            mapChapterTwoBooleansTemp = new List<bool>();

            mapTutorialBooleanNamesTemp = new List<string>();
            mapTutorialBooleansTemp = new List<bool>();

            characterBioTemp = new List<Boolean>();
            monsterBioTemp = new List<Boolean>();
            characterBioNamesTemp = new List<string>();
            monsterBioNamesTemp = new List<string>();
            passiveNamesTemp = new List<string>();

            skillsInShopTemp = new List<string>();

            generatedComboNamesTemp = new List<string>();
            generatedCombosTemp = new List<string>();
            equippedSkillNamesTemp = new List<string>();

        }

        public void InitiateSave()
        {
            ResetListsForSave();
            SaveToDevice();
        }

        void SaveToDevice()
        {
            IsolatedStorageFile dataFile = IsolatedStorageFile.GetUserStoreForDomain();
            IsolatedStorageFileStream isolatedFileStream = null;

            #region SAVE STUFF
            //Skills in shop
                for (int i = 0; i < game.YourLocker.SkillsOnSale.Count; i++)
                {
                    skillsInShopTemp.Add(game.YourLocker.SkillsOnSale[i].Name);
                }

                //--Prologue bools
                for (int i = 0; i < game.Prologue.PrologueBooleans.Count; i++)
                {
                    prologueBooleanTemp.Add(game.Prologue.PrologueBooleans.ElementAt(i).Value);
                    prologueBooleanNameTemp.Add(game.Prologue.PrologueBooleans.ElementAt(i).Key);
                }
                //--Chapter One bools
                for (int i = 0; i < game.ChapterOne.ChapterOneBooleans.Count; i++)
                {
                    chapterOneBooleanTemp.Add(game.ChapterOne.ChapterOneBooleans.ElementAt(i).Value);
                    chapterOneBooleanNameTemp.Add(game.ChapterOne.ChapterOneBooleans.ElementAt(i).Key);
                }
                //--Chapter Two bools
                for (int i = 0; i < game.ChapterTwo.ChapterTwoBooleans.Count; i++)
                {
                    chapterTwoBooleanTemp.Add(game.ChapterTwo.ChapterTwoBooleans.ElementAt(i).Value);
                    chapterTwoBooleanNameTemp.Add(game.ChapterTwo.ChapterTwoBooleans.ElementAt(i).Key);
                }

                //--Tutorial Map bools
                for (int i = 0; i < game.MapBooleans.tutorialMapBooleans.Count; i++)
                {
                    mapTutorialBooleansTemp.Add(game.MapBooleans.tutorialMapBooleans.ElementAt(i).Value);
                    mapTutorialBooleanNamesTemp.Add(game.MapBooleans.tutorialMapBooleans.ElementAt(i).Key);
                }

                //--Prologue Map bools
                for (int i = 0; i < game.MapBooleans.prologueMapBooleans.Count; i++)
                {
                    mapPrologueBooleansTemp.Add(game.MapBooleans.prologueMapBooleans.ElementAt(i).Value);
                    mapPrologueBooleanNamesTemp.Add(game.MapBooleans.prologueMapBooleans.ElementAt(i).Key);
                }

                //--Chapter One Map bools
                for (int i = 0; i < game.MapBooleans.chapterOneMapBooleans.Count; i++)
                {
                    mapChapterOneBooleansTemp.Add(game.MapBooleans.chapterOneMapBooleans.ElementAt(i).Value);
                    mapChapterOneBooleanNamesTemp.Add(game.MapBooleans.chapterOneMapBooleans.ElementAt(i).Key);
                }

                
                //--Chapter Two Map bools
                for (int i = 0; i < game.MapBooleans.chapterTwoMapBooleans.Count; i++)
                {
                    mapChapterTwoBooleansTemp.Add(game.MapBooleans.chapterTwoMapBooleans.ElementAt(i).Value);
                    mapChapterTwoBooleanNamesTemp.Add(game.MapBooleans.chapterTwoMapBooleans.ElementAt(i).Key);
                }

                //--NPCs
                for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
                {
                    if (game.SideQuestManager.nPCs.ElementAt(i).Value.Quest != null)
                    {
                        sideQuestNPCWrapperTemp.Add(new NPCWrapper(game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue, game.SideQuestManager.nPCs.ElementAt(i).Value.QuestDialogue, game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState, game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight, game.SideQuestManager.nPCs.ElementAt(i).Value.Quest.QuestName, game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest, game.SideQuestManager.nPCs.ElementAt(i).Value.MapName, game.SideQuestManager.nPCs.ElementAt(i).Key, game.SideQuestManager.nPCs.ElementAt(i).Value.canTalk, game.SideQuestManager.nPCs.ElementAt(i).Value.RecX, game.SideQuestManager.nPCs.ElementAt(i).Value.RecY));
                    }
                    else if (!(game.SideQuestManager.nPCs.ElementAt(i).Value is TrenchcoatKid))
                    {
                        sideQuestNPCWrapperTemp.Add(new NPCWrapper(game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue, game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState, game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight, game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest, game.SideQuestManager.nPCs.ElementAt(i).Value.MapName, game.SideQuestManager.nPCs.ElementAt(i).Key, game.SideQuestManager.nPCs.ElementAt(i).Value.canTalk, game.SideQuestManager.nPCs.ElementAt(i).Value.RecX, game.SideQuestManager.nPCs.ElementAt(i).Value.RecY));
                    }
                    //Trenchcoat NPCs
                    else
                    {
                        sideQuestNPCWrapperTemp.Add(new NPCWrapper(game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue, game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState, game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight, game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest, (game.SideQuestManager.nPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut, game.SideQuestManager.nPCs.ElementAt(i).Value.MapName, game.SideQuestManager.nPCs.ElementAt(i).Key, game.SideQuestManager.nPCs.ElementAt(i).Value.canTalk, game.SideQuestManager.nPCs.ElementAt(i).Value.RecX, game.SideQuestManager.nPCs.ElementAt(i).Value.RecY));
                    }
                }

                //Prologue
                for (int i = 0; i < game.Prologue.NPCs.Count; i++)
                {
                    //Quest NPCs
                    if (game.Prologue.NPCs.ElementAt(i).Value.Quest != null)
                    {
                        prologueNPCWrapperTemp.Add(new NPCWrapper(game.Prologue.NPCs.ElementAt(i).Value.Dialogue, game.Prologue.NPCs.ElementAt(i).Value.QuestDialogue, game.Prologue.NPCs.ElementAt(i).Value.DialogueState, game.Prologue.NPCs.ElementAt(i).Value.FacingRight, game.Prologue.NPCs.ElementAt(i).Value.Quest.QuestName, game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest, game.Prologue.NPCs.ElementAt(i).Value.MapName, game.Prologue.NPCs.ElementAt(i).Key, game.Prologue.NPCs.ElementAt(i).Value.canTalk, game.Prologue.NPCs.ElementAt(i).Value.RecX, game.Prologue.NPCs.ElementAt(i).Value.RecY));
                    }
                    //Non-Quest, Non-Trenchcoat NPCs
                    else if (!(game.Prologue.NPCs.ElementAt(i).Value is TrenchcoatKid))
                    {
                        prologueNPCWrapperTemp.Add(new NPCWrapper(game.Prologue.NPCs.ElementAt(i).Value.Dialogue, game.Prologue.NPCs.ElementAt(i).Value.DialogueState, game.Prologue.NPCs.ElementAt(i).Value.FacingRight, game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest, game.Prologue.NPCs.ElementAt(i).Value.MapName, game.Prologue.NPCs.ElementAt(i).Key, game.Prologue.NPCs.ElementAt(i).Value.canTalk, game.Prologue.NPCs.ElementAt(i).Value.RecX, game.Prologue.NPCs.ElementAt(i).Value.RecY));
                    }
                    //Trenchcoat NPCs
                    else
                    {
                        prologueNPCWrapperTemp.Add(new NPCWrapper(game.Prologue.NPCs.ElementAt(i).Value.Dialogue, game.Prologue.NPCs.ElementAt(i).Value.DialogueState, game.Prologue.NPCs.ElementAt(i).Value.FacingRight, game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest, (game.Prologue.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut, game.Prologue.NPCs.ElementAt(i).Value.MapName, game.Prologue.NPCs.ElementAt(i).Key, game.Prologue.NPCs.ElementAt(i).Value.canTalk, game.Prologue.NPCs.ElementAt(i).Value.RecX, game.Prologue.NPCs.ElementAt(i).Value.RecY));
                    }
                }
                //Ch 1
                for (int i = 0; i < game.ChapterOne.NPCs.Count; i++)
                {
                    if (game.ChapterOne.NPCs.ElementAt(i).Value.Quest != null)
                    {
                        chapterOneNPCWrapperTemp.Add(new NPCWrapper(game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue, game.ChapterOne.NPCs.ElementAt(i).Value.QuestDialogue, game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState, game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight, game.ChapterOne.NPCs.ElementAt(i).Value.Quest.QuestName, game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest, game.ChapterOne.NPCs.ElementAt(i).Value.MapName, game.ChapterOne.NPCs.ElementAt(i).Key, game.ChapterOne.NPCs.ElementAt(i).Value.canTalk, game.ChapterOne.NPCs.ElementAt(i).Value.RecX, game.ChapterOne.NPCs.ElementAt(i).Value.RecY));
                    }
                    else if (!(game.ChapterOne.NPCs.ElementAt(i).Value is TrenchcoatKid))
                    {
                        chapterOneNPCWrapperTemp.Add(new NPCWrapper(game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue, game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState, game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight, game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest, game.ChapterOne.NPCs.ElementAt(i).Value.MapName, game.ChapterOne.NPCs.ElementAt(i).Key, game.ChapterOne.NPCs.ElementAt(i).Value.canTalk, game.ChapterOne.NPCs.ElementAt(i).Value.RecX, game.ChapterOne.NPCs.ElementAt(i).Value.RecY));
                    }
                    else
                    {
                        chapterOneNPCWrapperTemp.Add(new NPCWrapper(game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue, game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState, game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight, game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest, (game.ChapterOne.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut, game.ChapterOne.NPCs.ElementAt(i).Value.MapName, game.ChapterOne.NPCs.ElementAt(i).Key, game.ChapterOne.NPCs.ElementAt(i).Value.canTalk, game.ChapterOne.NPCs.ElementAt(i).Value.RecX, game.ChapterOne.NPCs.ElementAt(i).Value.RecY));
                    }
                }

                //Ch 2
                for (int i = 0; i < game.ChapterTwo.NPCs.Count; i++)
                {
                    if (game.ChapterTwo.NPCs.ElementAt(i).Value.Quest != null)
                    {
                        chapterTwoNPCWrapperTemp.Add(new NPCWrapper(game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue, game.ChapterTwo.NPCs.ElementAt(i).Value.QuestDialogue, game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState, game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight, game.ChapterTwo.NPCs.ElementAt(i).Value.Quest.QuestName, game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest, game.ChapterTwo.NPCs.ElementAt(i).Value.MapName, game.ChapterTwo.NPCs.ElementAt(i).Key, game.ChapterTwo.NPCs.ElementAt(i).Value.canTalk, game.ChapterTwo.NPCs.ElementAt(i).Value.RecX, game.ChapterTwo.NPCs.ElementAt(i).Value.RecY));
                    }
                    else if (!(game.ChapterTwo.NPCs.ElementAt(i).Value is TrenchcoatKid))
                    {
                        chapterTwoNPCWrapperTemp.Add(new NPCWrapper(game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue, game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState, game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight, game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest, game.ChapterTwo.NPCs.ElementAt(i).Value.MapName, game.ChapterTwo.NPCs.ElementAt(i).Key, game.ChapterTwo.NPCs.ElementAt(i).Value.canTalk, game.ChapterTwo.NPCs.ElementAt(i).Value.RecX, game.ChapterTwo.NPCs.ElementAt(i).Value.RecY));
                    }
                    else
                    {
                        chapterTwoNPCWrapperTemp.Add(new NPCWrapper(game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue, game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState, game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight, game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest, (game.ChapterTwo.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut, game.ChapterTwo.NPCs.ElementAt(i).Value.MapName, game.ChapterTwo.NPCs.ElementAt(i).Key, game.ChapterTwo.NPCs.ElementAt(i).Value.canTalk, game.ChapterTwo.NPCs.ElementAt(i).Value.RecX, game.ChapterTwo.NPCs.ElementAt(i).Value.RecY));
                    }
                }

                //Generated locker combos
                for (int i = 0; i < GenerateLockerCombinations.combinations.Count; i++)
                {
                    generatedCombosTemp.Add(GenerateLockerCombinations.combinations.ElementAt(i).Value);
                    generatedComboNamesTemp.Add(GenerateLockerCombinations.combinations.ElementAt(i).Key);
                }

                //--Locker combos in journal
                for (int i = 0; i < game.Notebook.ComboPage.LockerCombos.Count; i++)
                {
                    lockerComboNamesTemp.Add(game.Notebook.ComboPage.LockerCombos.ElementAt(i).Key);
                    lockerCombosTemp.Add(game.Notebook.ComboPage.LockerCombos.ElementAt(i).Value);
                }

                //--Learned skills
                for (int i = 0; i < player.LearnedSkills.Count; i++)
                {
                    learnedSkillsTemp.Add(player.LearnedSkills[i].Name);
                    skillwrapperTemp.Add(new SkillWrapper(player.LearnedSkills[i].SkillRank, player.LearnedSkills[i].Experience, player.LearnedSkills[i].ExperienceUntilLevel, player.LearnedSkills[i].Damage, player.LearnedSkills[i].FullCooldown));

                    if (player.LearnedSkills[i].Equipped)
                        equippedSkillsTemp.Add(true);
                    else
                        equippedSkillsTemp.Add(false);
                }

                if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                    quickRetortTemp = new SkillWrapper(player.quickRetort.SkillRank, player.quickRetort.Experience, player.quickRetort.ExperienceUntilLevel, player.quickRetort.Damage, player.quickRetort.FullCooldown);

                //Equipped skills in order
                for (int i = 0; i < player.EquippedSkills.Count; i++)
                {
                    equippedSkillNamesTemp.Add(player.EquippedSkills[i].Name);
                }

                //Owned weapons
                for (int i = 0; i < player.OwnedWeapons.Count; i++)
                {
                    ownedWeaponsTemp.Add(player.OwnedWeapons[i].Name);

                    if (player.OwnedWeapons[i].PassiveAbility != null)
                        ownedWeaponWrappersTemp.Add(new EquipmentWrapper(player.OwnedWeapons[i].Strength, player.OwnedWeapons[i].Defense, player.OwnedWeapons[i].Health, player.OwnedWeapons[i].UpgradeSlots, player.OwnedWeapons[i].PassiveAbility.Name));
                    else
                        ownedWeaponWrappersTemp.Add(new EquipmentWrapper(player.OwnedWeapons[i].Strength, player.OwnedWeapons[i].Defense, player.OwnedWeapons[i].Health, player.OwnedWeapons[i].UpgradeSlots, ""));

                }
                //Owned shirts
                for (int i = 0; i < player.OwnedHoodies.Count; i++)
                {
                    ownedShirtsTemp.Add(player.OwnedHoodies[i].Name);

                    if (player.OwnedHoodies[i].PassiveAbility != null)
                        ownedShirtWrappersTemp.Add(new EquipmentWrapper(player.OwnedHoodies[i].Strength, player.OwnedHoodies[i].Defense, player.OwnedHoodies[i].Health, player.OwnedHoodies[i].UpgradeSlots, player.OwnedHoodies[i].PassiveAbility.Name));
                    else
                        ownedShirtWrappersTemp.Add(new EquipmentWrapper(player.OwnedHoodies[i].Strength, player.OwnedHoodies[i].Defense, player.OwnedHoodies[i].Health, player.OwnedHoodies[i].UpgradeSlots, ""));
                }
                //Owned hats
                for (int i = 0; i < player.OwnedHats.Count; i++)
                {
                    ownedHatsTemp.Add(player.OwnedHats[i].Name);

                    if (player.OwnedHats[i].PassiveAbility != null)
                        ownedHatWrappersTemp.Add(new EquipmentWrapper(player.OwnedHats[i].Strength, player.OwnedHats[i].Defense, player.OwnedHats[i].Health, player.OwnedHats[i].UpgradeSlots, player.OwnedHats[i].PassiveAbility.Name));
                    else
                        ownedHatWrappersTemp.Add(new EquipmentWrapper(player.OwnedHats[i].Strength, player.OwnedHats[i].Defense, player.OwnedHats[i].Health, player.OwnedHats[i].UpgradeSlots, ""));
                }
                //Owned accessories
                for (int i = 0; i < player.OwnedAccessories.Count; i++)
                {
                    ownedAccessoriesTemp.Add(player.OwnedAccessories[i].Name);

                    if (player.OwnedAccessories[i].PassiveAbility != null)
                        ownedAccessoryWrappersTemp.Add(new EquipmentWrapper(player.OwnedAccessories[i].Strength, player.OwnedAccessories[i].Defense, player.OwnedAccessories[i].Health, player.OwnedAccessories[i].UpgradeSlots, player.OwnedAccessories[i].PassiveAbility.Name));
                    else
                        ownedAccessoryWrappersTemp.Add(new EquipmentWrapper(player.OwnedAccessories[i].Strength, player.OwnedAccessories[i].Defense, player.OwnedAccessories[i].Health, player.OwnedAccessories[i].UpgradeSlots, ""));
                }

                //Bio pages
                for (int i = 0; i < player.AllCharacterBios.Count; i++)
                {
                    //Save the key and value (String and boolean...("Paul", false)
                    characterBioTemp.Add(player.AllCharacterBios.ElementAt(i).Value);
                    characterBioNamesTemp.Add(player.AllCharacterBios.ElementAt(i).Key);
                }
                for (int i = 0; i < player.AllMonsterBios.Count; i++)
                {
                    monsterBioTemp.Add(player.AllMonsterBios.ElementAt(i).Value);
                    monsterBioNamesTemp.Add(player.AllMonsterBios.ElementAt(i).Key);
                }

                #region Player equipped items
                if (player.EquippedWeapon != null)
                {
                    weaponOneTemp = player.EquippedWeapon.Name;

                    if (player.EquippedWeapon.PassiveAbility != null)
                        w1 = new EquipmentWrapper(player.EquippedWeapon.Strength, player.EquippedWeapon.Defense, player.EquippedWeapon.Health, player.EquippedWeapon.UpgradeSlots, player.EquippedWeapon.PassiveAbility.Name);
                    else
                        w1 = new EquipmentWrapper(player.EquippedWeapon.Strength, player.EquippedWeapon.Defense, player.EquippedWeapon.Health, player.EquippedWeapon.UpgradeSlots, "");
                }
                if (player.SecondWeapon != null)
                {
                    weaponTwoTemp = player.SecondWeapon.Name;
                    if (player.SecondWeapon.PassiveAbility != null)
                        w2 = new EquipmentWrapper(player.SecondWeapon.Strength, player.SecondWeapon.Defense, player.SecondWeapon.Health, player.SecondWeapon.UpgradeSlots, player.SecondWeapon.PassiveAbility.Name);
                    else
                        w2 = new EquipmentWrapper(player.SecondWeapon.Strength, player.SecondWeapon.Defense, player.SecondWeapon.Health, player.SecondWeapon.UpgradeSlots, "");
                }
                if (player.EquippedHoodie != null)
                {
                    shirtTemp = player.EquippedHoodie.Name;

                    if(player.EquippedHoodie.PassiveAbility != null)
                        s = new EquipmentWrapper(player.EquippedHoodie.Strength, player.EquippedHoodie.Defense, player.EquippedHoodie.Health, player.EquippedHoodie.UpgradeSlots, player.EquippedHoodie.PassiveAbility.Name);
                    else
                        s = new EquipmentWrapper(player.EquippedHoodie.Strength, player.EquippedHoodie.Defense, player.EquippedHoodie.Health, player.EquippedHoodie.UpgradeSlots, "");
                }
                if (player.EquippedHat != null)
                {
                    hatTemp = player.EquippedHat.Name;
                    if (player.EquippedHat.PassiveAbility != null)
                        h = new EquipmentWrapper(player.EquippedHat.Strength, player.EquippedHat.Defense, player.EquippedHat.Health, player.EquippedHat.UpgradeSlots, player.EquippedHat.PassiveAbility.Name);
                    else
                        h = new EquipmentWrapper(player.EquippedHat.Strength, player.EquippedHat.Defense, player.EquippedHat.Health, player.EquippedHat.UpgradeSlots, "");
                }
                if (player.EquippedAccessory != null)
                {
                    accessoryOneTemp = player.EquippedAccessory.Name;
                    if (player.EquippedAccessory.PassiveAbility != null)
                        a1 = new EquipmentWrapper(player.EquippedAccessory.Strength, player.EquippedAccessory.Defense, player.EquippedAccessory.Health, player.EquippedAccessory.UpgradeSlots, player.EquippedAccessory.PassiveAbility.Name);
                    else
                        a1 = new EquipmentWrapper(player.EquippedAccessory.Strength, player.EquippedAccessory.Defense, player.EquippedAccessory.Health, player.EquippedAccessory.UpgradeSlots, "");
                }
                if (player.SecondAccessory != null)
                {
                    accessoryTwoTemp = player.SecondAccessory.Name;
                    if (player.SecondAccessory.PassiveAbility != null)
                        a2 = new EquipmentWrapper(player.SecondAccessory.Strength, player.SecondAccessory.Defense, player.SecondAccessory.Health, player.SecondAccessory.UpgradeSlots, player.SecondAccessory.PassiveAbility.Name);
                    else
                        a2 = new EquipmentWrapper(player.SecondAccessory.Strength, player.SecondAccessory.Defense, player.SecondAccessory.Health, player.SecondAccessory.UpgradeSlots, "");
                }
                #endregion

                //--Story Items
                for (int i = 0; i < player.StoryItems.Count; i++)
                {
                    storyItemsNameTemp.Add(player.StoryItems.ElementAt(i).Key);
                    storyItemsNumTemp.Add(player.StoryItems.ElementAt(i).Value);
                }

                //Passive skills
                for (int i = 0; i < player.OwnedPassives.Count; i++)
                {
                    passiveNamesTemp.Add(player.OwnedPassives[i].Name);
                }


                //--Enemy Drops
                for (int i = 0; i < player.EnemyDrops.Count; i++)
                {
                    enemyDropsNameTemp.Add(player.EnemyDrops.ElementAt(i).Key);
                    enemyDropsNumTemp.Add(player.EnemyDrops.ElementAt(i).Value);
                }

                for (int i = 0; i < game.AllQuests.Count; i++)
                {
                    allQuestsTemp.Add(new QuestWrapper(game.AllQuests.ElementAt(i).Value.QuestName, game.AllQuests.ElementAt(i).Value.CompletedQuest, game.AllQuests.ElementAt(i).Value.EnemiesKilledForQuest, game.AllQuests.ElementAt(i).Value.inQuestHelper, game.AllQuests.ElementAt(i).Value.npcName));

                }

                //--Current Quests
                for (int i = 0; i < game.CurrentQuests.Count; i++)
                {
                    currentQuestsTemp.Add(game.CurrentQuests[i].QuestName);

                    if (game.CurrentQuests[i].StoryQuest == false)
                        currentSideQuestsTemp.Add(game.CurrentQuests[i].QuestName);
                }

                //--Completed story quests
                for (int i = 0; i < game.Prologue.CompletedStoryQuests.Count; i++)
                {
                    prologueCompletedStoryTemp.Add(game.Prologue.CompletedStoryQuests.ElementAt(i).Key);
                }
                for (int i = 0; i < game.ChapterOne.CompletedStoryQuests.Count; i++)
                {
                    chOneCompletedStoryTemp.Add(game.ChapterOne.CompletedStoryQuests.ElementAt(i).Key);
                }
                for (int i = 0; i < game.ChapterTwo.CompletedStoryQuests.Count; i++)
                {
                    chTwoCompletedStoryTemp.Add(game.ChapterTwo.CompletedStoryQuests.ElementAt(i).Key);
                }

                //--Completed side quests
                for (int i = 0; i < game.Prologue.CompletedSideQuests.Count; i++)
                {
                    prologueCompletedSideTemp.Add(game.Prologue.CompletedSideQuests.ElementAt(i).Key);
                }
                for (int i = 0; i < game.ChapterOne.CompletedSideQuests.Count; i++)
                {
                    chOneCompletedSideTemp.Add(game.ChapterOne.CompletedSideQuests.ElementAt(i).Key);
                }
                for (int i = 0; i < game.ChapterTwo.CompletedSideQuests.Count; i++)
                {
                    chTwoCompletedSideTemp.Add(game.ChapterTwo.CompletedSideQuests.ElementAt(i).Key);
                }

                switch (game.chapterState)
                {
                    case Game1.ChapterState.prologue:
                        chapterStateTemp = 0;
                        break;
                    case Game1.ChapterState.chapterOne:
                        chapterStateTemp = 1;
                        break;
                    case Game1.ChapterState.chapterTwo:
                        chapterStateTemp = 2;
                        break;
                }


                currentMapZoneStateTemp = 0;

                SaveGame SaveData = new SaveGame()
                {
                    portalDestinationName = Bathroom.LastMapPortal.MapName,
                    portalDestination = Bathroom.LastMapPortal.PortalRec,
                    sideQuestNPCs = sideQuestNPCWrapperTemp,
                    generatedComboNames = generatedComboNamesTemp,
                    generatedCombos = generatedCombosTemp,

                    //Chapter
                    currentMapZoneState = currentMapZoneStateTemp,
                    chapterState = chapterStateTemp,
                    prologueBooleans = prologueBooleanTemp,
                    prologueBooleanNames = prologueBooleanNameTemp,
                    prologueNPCWrappers = prologueNPCWrapperTemp,
                    prologueSynopsis = game.Prologue.Synopsis,
                    chapterOneSynopsis = game.ChapterOne.Synopsis,
                    cutsceneState = game.CurrentChapter.CutsceneState,
                    chapterOneNPCWrappers = chapterOneNPCWrapperTemp,
                    chapterOneBooleanNames = chapterOneBooleanNameTemp,
                    chapterOneBooleans = chapterOneBooleanTemp,
                    chapterTwoNPCWrappers = chapterTwoNPCWrapperTemp,
                    chapterTwoBooleanNames = chapterTwoBooleanNameTemp,
                    chapterTwoBooleans = chapterTwoBooleanTemp,
                    chapterTwoSynopsis = game.ChapterTwo.Synopsis,


                    //Skills
                    learnedSkills = learnedSkillsTemp,
                    equippedSkills = equippedSkillsTemp,
                    skillWrappers = skillwrapperTemp,
                    quickRetort = quickRetortTemp,
                    skillsInShop = skillsInShopTemp,
                    equippedSkillNames = equippedSkillNamesTemp,

                    //Maps
                    mapStoryItemsPicked = mapStoryItemTemp,
                    mapChestOpened = mapChestTemp,
                    portalsLocked = portalLockedTemp,
                    moveBlockPositions = moveBlockPosTemp,
                    switchesActive = switchActiveTemp,
                    mapEnemiesKilled = mapEnemiesKilledTemp,
                    mapPrologueBooleans = mapPrologueBooleansTemp,
                    mapPrologueBooleanNames = mapPrologueBooleanNamesTemp,
                    takenContents = takenContentsTemp,
                    discoveredMap = discoveredMapTemp,
                    mapCollectiblePicked = mapCollectiblePickedTemp,
                    mapCollectibleAble = mapCollectibleAbleTemp,
                    mapChapterOneBooleanNames = mapChapterOneBooleanNamesTemp,
                    mapChapterOneBooleans = mapChapterOneBooleansTemp,
                    mapInteractiveObjects = mapInteractiveTemp,
                    mapChapterTwoBooleanNames = mapChapterTwoBooleanNamesTemp,
                    mapChapterTwoBooleans = mapChapterTwoBooleansTemp,
                    mapTutorialBooleanNames = mapTutorialBooleanNamesTemp,
                    mapTutorialBooleans = mapTutorialBooleansTemp,

                    //Map zones
                    schoolZoneWrapper = game.schoolZoneWrapper,

                    //Player
                    playerLevel = player.Level,
                    socialRankIndex = player.SocialRankIndex,
                    hasPhone = player.HasCellPhone,
                    playerMoney = player.Money,
                    playerRank = player.SocialRank,
                    maxMotivation = player.BaseMaxHealth,
                    strength = player.BaseStrength,
                    tolerance = player.BaseDefense,
                    experience = player.Experience,
                    experienceUntilLevel = player.ExperienceUntilLevel,
                    storyItemsName = storyItemsNameTemp,
                    storyItemsNum = storyItemsNumTemp,
                    enemyDropsName = enemyDropsNameTemp,
                    enemyDropsNum = enemyDropsNumTemp,
                    textbooks = player.Textbooks,
                    bronzeKeys = player.BronzeKeys,
                    silverKeys = player.SilverKeys,
                    goldKeys = player.GoldKeys,
                    karma = player.Karma,
                    statPoints = player.StatPoints,
                    lockerComboNames = lockerComboNamesTemp,
                    lockerCombos = lockerCombosTemp,
                    characterBios = characterBioTemp,
                    monsterBios = monsterBioTemp,
                    characterBioNames = characterBioNamesTemp,
                    monsterBioNames = monsterBioNamesTemp,
                    passiveNames = passiveNamesTemp,
                    prologueSynopsisRead = game.Notebook.Journal.prologueSynopsisRead,
                    prologueSideQuestsRead = game.Notebook.Journal.prologueSideQuestsRead,
                    prologueStoryQuestsRead = game.Notebook.Journal.prologueStoryQuestsRead,
                    chOneSynopsisRead = game.Notebook.Journal.chOneSynopsisRead,
                    chOneSideQuestsRead = game.Notebook.Journal.chOneSideQuestsRead,
                    chOneStoryQuestsRead = game.Notebook.Journal.chOneStoryQuestsRead,
                    chTwoSynopsisRead = game.Notebook.Journal.chTwoSynopsisRead,
                    chTwoSideQuestsRead = game.Notebook.Journal.chTwoSideQuestsRead,
                    chTwoStoryQuestsRead = game.Notebook.Journal.chTwoStoryQuestsRead,

                    //Equipment
                    ownedWeapons = ownedWeaponsTemp,
                    ownedWeaponWrappers = ownedWeaponWrappersTemp,
                    ownedHats = ownedHatsTemp,
                    ownedAccessories = ownedAccessoriesTemp,
                    ownedAccessoryWrappers = ownedAccessoryWrappersTemp,
                    ownedHatWrappers = ownedHatWrappersTemp,
                    ownedShirts = ownedShirtsTemp,
                    ownedShirtWrappers = ownedShirtWrappersTemp,
                    weaponOne = weaponOneTemp,
                    weaponTwo = weaponTwoTemp,
                    accessoryOne = accessoryOneTemp,
                    accessoryTwo = accessoryTwoTemp,
                    hat = hatTemp,
                    shirt = shirtTemp,
                    wep1Wrap = w1,
                    wep2Wrap = w2,
                    hatWrap = h,
                    shirtWrap = s,
                    acc1Wrap = a1,
                    acc2Wrap = a2,
                    newWeapon = game.Notebook.Inventory.newWeapon,
                    newHat = game.Notebook.Inventory.newHat,
                    newShirt = game.Notebook.Inventory.newShirt,
                    newAccessory = game.Notebook.Inventory.newAccessory,
                    newLoot = game.Notebook.Inventory.newLoot, 

                    //Quests
                    currentQuests = currentQuestsTemp,
                    currentSideQuests = currentSideQuestsTemp,
                    prologueCompletedStoryQuests = prologueCompletedStoryTemp,
                    prologueCompletedSideQuests = prologueCompletedSideTemp,
                    chOneCompletedSideQuests = chOneCompletedSideTemp,
                    chOneCompletedStoryQuests = chOneCompletedStoryTemp,
                    chTwoCompletedSideQuests = chTwoCompletedSideTemp,
                    chTwoCompletedStoryQuests = chTwoCompletedStoryTemp,

                    allQuests= allQuestsTemp,
                };

#endregion


                if (dataFile.FileExists(filename))
                    dataFile.DeleteFile(filename);

                using (isolatedFileStream = dataFile.CreateFile(filename))
                {
                    // Read the data from the file.
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                    // Store the deserialized data object.

                    // Set the position to the begining of the file.
                    isolatedFileStream.Seek(0, SeekOrigin.Begin);
                    // Serialize the new data object.
                    serializer.Serialize(isolatedFileStream, SaveData);
                    // Set the length of the file.
                    isolatedFileStream.SetLength(isolatedFileStream.Position);

                }

                isolatedFileStream.Close();
        }

        public void InitiateLoad()
        {
            LoadFromDevice();
        }

        void LoadFromDevice()
        {
            IsolatedStorageFile dataFile = IsolatedStorageFile.GetUserStoreForDomain();
            IsolatedStorageFileStream isolatedFileStream = null;

            if (dataFile.FileExists(filename))
            {

                // Open the file using the established file stream.
                using (isolatedFileStream = dataFile.OpenFile(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    // Read the data from the file.
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                    // Store the deserialized data object.
                    SaveGame SaveData = (SaveGame)serializer.Deserialize(isolatedFileStream);
                    game.saveData = SaveData;

                    //Update the game based on the save game file
                    switch (SaveData.chapterState)
                    {
                        case 0:
                            game.CurrentChapter = game.Prologue;
                            game.chapterState = Game1.ChapterState.prologue;
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            break;

                        case 1:
                            game.CurrentChapter = game.ChapterOne;
                            game.chapterState = Game1.ChapterState.chapterOne;
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            break;

                        case 2:
                            game.CurrentChapter = game.ChapterTwo;
                            game.chapterState = Game1.ChapterState.chapterTwo;
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            break;
                    }

                    Bathroom.LastMapPortal = new Portal(SaveData.portalDestination.X, SaveData.portalDestination.Y + Game1.portalTexture.Height, SaveData.portalDestinationName);

                    game.CurrentChapter.CutsceneState = SaveData.cutsceneState;

                    //Chapter booleans
                    //Prologue
                    for (int i = 0; i < SaveData.prologueBooleans.Count; i++)
                    {
                        game.Prologue.PrologueBooleans[SaveData.prologueBooleanNames[i]] = SaveData.prologueBooleans[i];

                    }
                    //Chapter One
                    for (int i = 0; i < SaveData.chapterOneBooleans.Count; i++)
                    {
                        game.ChapterOne.ChapterOneBooleans[SaveData.chapterOneBooleanNames[i]] = SaveData.chapterOneBooleans[i];

                    }
                    //Chapter Two
                    for (int i = 0; i < SaveData.chapterTwoBooleans.Count; i++)
                    {
                        game.ChapterTwo.ChapterTwoBooleans[SaveData.chapterTwoBooleanNames[i]] = SaveData.chapterTwoBooleans[i];

                    }

                    game.Prologue.Synopsis = SaveData.prologueSynopsis;
                    game.ChapterOne.Synopsis = SaveData.chapterOneSynopsis;
                    game.ChapterTwo.Synopsis = SaveData.chapterTwoSynopsis;

                    //--Locker combos
                    for (int i = 0; i < SaveData.lockerCombos.Count; i++)
                    {
                        if (!(game.Notebook.ComboPage.LockerCombos.ContainsKey(SaveData.lockerComboNames[i])))
                            game.Notebook.ComboPage.LockerCombos.Add(SaveData.lockerComboNames[i], SaveData.lockerCombos[i]);
                    }

                    //Clear the ones made on game start first
                    GenerateLockerCombinations.combinations.Clear();

                    //Generated locker combos
                    for (int i = 0; i < SaveData.generatedCombos.Count; i++)
                    {
                        GenerateLockerCombinations.combinations.Add(SaveData.generatedComboNames[i], SaveData.generatedCombos[i]);
                    }

                    //Map booleans
                    //Prologue
                    for (int i = 0; i < SaveData.mapPrologueBooleans.Count; i++)
                    {
                        game.MapBooleans.prologueMapBooleans[SaveData.mapPrologueBooleanNames[i]] = SaveData.mapPrologueBooleans[i];
                    }
                    //Ch1
                    for (int i = 0; i < SaveData.mapChapterOneBooleans.Count; i++)
                    {
                        game.MapBooleans.chapterOneMapBooleans[SaveData.mapChapterOneBooleanNames[i]] = SaveData.mapChapterOneBooleans[i];
                    }
                    //Tutorial
                    for (int i = 0; i < SaveData.mapTutorialBooleans.Count; i++)
                    {
                        game.MapBooleans.tutorialMapBooleans[SaveData.mapTutorialBooleanNames[i]] = SaveData.mapTutorialBooleans[i];
                    }

                    for (int i = 0; i < SaveData.mapChapterTwoBooleans.Count; i++)
                    {
                        game.MapBooleans.chapterTwoMapBooleans[SaveData.mapChapterTwoBooleanNames[i]] = SaveData.mapChapterTwoBooleans[i];
                    }

                    //Map zone
                    game.schoolZoneWrapper = SaveData.schoolZoneWrapper;
                    Game1.schoolMaps.LoadSchoolZone();
                    Game1.schoolMaps.LoadMapData(game.schoolZoneWrapper);
                    Game1.schoolMaps.LoadEnemyData();

                    //Bio pages
                    player.AllCharacterBios.Clear(); //Clear them so we can read the correct values
                    player.AllMonsterBios.Clear();

                    for (int i = 0; i < SaveData.characterBios.Count; i++)
                    {
                        player.AllCharacterBios.Add(SaveData.characterBioNames[i], SaveData.characterBios[i]);
                    }

                    for (int i = 0; i < SaveData.monsterBios.Count; i++)
                    {
                        player.AllMonsterBios.Add(SaveData.monsterBioNames[i], SaveData.monsterBios[i]);
                    }

                    //Owned skills
                    for (int i = 0; i < SaveData.learnedSkills.Count; i++)
                    {
                        player.LearnedSkills.Add(SkillManager.AllSkills[SaveData.learnedSkills[i]]);
                        player.LearnedSkills[i].Equipped = SaveData.equippedSkills[i];
                        player.LearnedSkills[i].SkillRank = SaveData.skillWrappers[i].level;
                        player.LearnedSkills[i].ExperienceUntilLevel = SaveData.skillWrappers[i].experienceUntilLevel;
                        player.LearnedSkills[i].Damage = SaveData.skillWrappers[i].damage;
                        player.LearnedSkills[i].FullCooldown = SaveData.skillWrappers[i].fullCooldown;

                        player.LearnedSkills[i].ApplyLevelUp(true);

                        player.LearnedSkills[i].Experience = SaveData.skillWrappers[i].experience;
                    }

                    if (game.ChapterOne.ChapterOneBooleans["quickRetortObtained"])
                    {
                        player.quickRetort.SkillRank = SaveData.quickRetort.level;
                        player.quickRetort.ExperienceUntilLevel = SaveData.quickRetort.experienceUntilLevel;
                        player.quickRetort.Damage = SaveData.quickRetort.damage;
                        player.quickRetort.FullCooldown = SaveData.quickRetort.fullCooldown;
                        player.quickRetort.ApplyLevelUp(true);

                        player.quickRetort.Experience = SaveData.quickRetort.experience;

                    }

                    //Add skills in correct order
                    for (int i = 0; i < SaveData.equippedSkillNames.Count; i++)
                    {
                        player.EquippedSkills.Add(SkillManager.AllSkills[SaveData.equippedSkillNames[i]]);
                        SkillManager.AllSkills[SaveData.equippedSkillNames[i]].LoadContent();
                    }

                    Chapter.effectsManager.skillMessageTime = 0;
                    Chapter.effectsManager.skillMessageColor = Color.White;

                    game.YourLocker.SkillsOnSale.Clear();

                    //Skills in the shop
                    for (int i = 0; i < SaveData.skillsInShop.Count; i++)
                    {
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills[SaveData.skillsInShop[i]]);
                    }

                    //Load the player's passives and add them to his list
                    for (int i = 0; i < SaveData.passiveNames.Count; i++)
                    {
                        player.OwnedPassives.Add(PassiveManager.allPassives[SaveData.passiveNames[i]]);
                        player.OwnedPassives[i].LoadPassive();
                    }

                    //Owned weapons
                    for (int i = 0; i < SaveData.ownedWeapons.Count; i++)
                    {
                        player.OwnedWeapons.Add(game.AllEquipment[SaveData.ownedWeapons[i]] as Weapon);
                        player.OwnedWeapons[i].UpgradeSlots = SaveData.ownedWeaponWrappers[i].upgradeSlots;
                        player.OwnedWeapons[i].Defense = SaveData.ownedWeaponWrappers[i].tolerance;
                        player.OwnedWeapons[i].Health = SaveData.ownedWeaponWrappers[i].motivation;
                        player.OwnedWeapons[i].Strength = SaveData.ownedWeaponWrappers[i].strength;

                        if (SaveData.ownedWeaponWrappers[i].passiveName != "")
                            player.OwnedWeapons[i].PassiveAbility = PassiveManager.allPassives[SaveData.ownedWeaponWrappers[i].passiveName];

                       // player.OwnedWeapons[i].UpdateDescription();
                    }
                    //Owned Hats
                    for (int i = 0; i < SaveData.ownedHats.Count; i++)
                    {
                        player.OwnedHats.Add(game.AllEquipment[SaveData.ownedHats[i]] as Hat);
                        player.OwnedHats[i].UpgradeSlots = SaveData.ownedHatWrappers[i].upgradeSlots;
                        player.OwnedHats[i].Defense = SaveData.ownedHatWrappers[i].tolerance;
                        player.OwnedHats[i].Health = SaveData.ownedHatWrappers[i].motivation;
                        player.OwnedHats[i].Strength = SaveData.ownedHatWrappers[i].strength;

                        if (SaveData.ownedHatWrappers[i].passiveName != "")
                            player.OwnedHats[i].PassiveAbility = PassiveManager.allPassives[SaveData.ownedHatWrappers[i].passiveName];

                       // player.OwnedHats[i].UpdateDescription();
                    }
                    //Owned Shirts
                    for (int i = 0; i < SaveData.ownedShirts.Count; i++)
                    {
                        player.OwnedHoodies.Add(game.AllEquipment[SaveData.ownedShirts[i]] as Outfit);
                        player.OwnedHoodies[i].UpgradeSlots = SaveData.ownedShirtWrappers[i].upgradeSlots;
                        player.OwnedHoodies[i].Defense = SaveData.ownedShirtWrappers[i].tolerance;
                        player.OwnedHoodies[i].Health = SaveData.ownedShirtWrappers[i].motivation;
                        player.OwnedHoodies[i].Strength = SaveData.ownedShirtWrappers[i].strength;

                        if (SaveData.ownedShirtWrappers[i].passiveName != "")
                            player.OwnedHoodies[i].PassiveAbility = PassiveManager.allPassives[SaveData.ownedShirtWrappers[i].passiveName];

                       // player.OwnedHoodies[i].UpdateDescription();
                    }
                    //Owned Accessories
                    for (int i = 0; i < SaveData.ownedAccessories.Count; i++)
                    {
                        player.OwnedAccessories.Add(game.AllEquipment[SaveData.ownedAccessories[i]] as Accessory);
                        player.OwnedAccessories[i].UpgradeSlots = SaveData.ownedAccessoryWrappers[i].upgradeSlots;
                        player.OwnedAccessories[i].Defense = SaveData.ownedAccessoryWrappers[i].tolerance;
                        player.OwnedAccessories[i].Health = SaveData.ownedAccessoryWrappers[i].motivation;
                        player.OwnedAccessories[i].Strength = SaveData.ownedAccessoryWrappers[i].strength;

                        if (SaveData.ownedAccessoryWrappers[i].passiveName != "")
                            player.OwnedAccessories[i].PassiveAbility = PassiveManager.allPassives[SaveData.ownedAccessoryWrappers[i].passiveName];

                     //   player.OwnedAccessories[i].UpdateDescription();
                    }


                    if (SaveData.weaponOne != null)
                    {
                        player.EquippedWeapon = game.AllEquipment[SaveData.weaponOne] as Weapon;
                        player.EquippedWeapon.UpgradeSlots = SaveData.wep1Wrap.upgradeSlots;
                        player.EquippedWeapon.Strength = SaveData.wep1Wrap.strength;
                        player.EquippedWeapon.Defense = SaveData.wep1Wrap.tolerance;
                        player.EquippedWeapon.Health = SaveData.wep1Wrap.motivation;

                        if (SaveData.wep1Wrap.passiveName != "")
                            player.EquippedWeapon.PassiveAbility = PassiveManager.allPassives[SaveData.wep1Wrap.passiveName];

                      //  player.EquippedWeapon.UpdateDescription();
                    }
                    if (SaveData.weaponTwo != null)
                    {
                        player.SecondWeapon = game.AllEquipment[SaveData.weaponTwo] as Weapon;
                        player.SecondWeapon.UpgradeSlots = SaveData.wep2Wrap.upgradeSlots;
                        player.SecondWeapon.Strength = SaveData.wep2Wrap.strength;
                        player.SecondWeapon.Defense = SaveData.wep2Wrap.tolerance;
                        player.SecondWeapon.Health = SaveData.wep2Wrap.motivation;

                        if (SaveData.wep2Wrap.passiveName != "")
                            player.SecondWeapon.PassiveAbility = PassiveManager.allPassives[SaveData.wep2Wrap.passiveName];

                       // player.SecondWeapon.UpdateDescription();


                    }
                    if (SaveData.hat != null)
                    {
                        player.EquippedHat = game.AllEquipment[SaveData.hat] as Hat;
                        player.EquippedHat.UpgradeSlots = SaveData.hatWrap.upgradeSlots;
                        player.EquippedHat.Strength = SaveData.hatWrap.strength;
                        player.EquippedHat.Defense = SaveData.hatWrap.tolerance;
                        player.EquippedHat.Health = SaveData.hatWrap.motivation;

                        if (SaveData.hatWrap.passiveName != "")
                            player.EquippedHat.PassiveAbility = PassiveManager.allPassives[SaveData.hatWrap.passiveName];

                      //  player.EquippedHat.UpdateDescription();
                    }
                    if (SaveData.shirt != null)
                    {
                        player.EquippedHoodie = game.AllEquipment[SaveData.shirt] as Outfit;
                        player.EquippedHoodie.UpgradeSlots = SaveData.shirtWrap.upgradeSlots;
                        player.EquippedHoodie.Strength = SaveData.shirtWrap.strength;
                        player.EquippedHoodie.Defense = SaveData.shirtWrap.tolerance;
                        player.EquippedHoodie.Health = SaveData.shirtWrap.motivation;

                        if (SaveData.shirtWrap.passiveName != "")
                            player.EquippedHoodie.PassiveAbility = PassiveManager.allPassives[SaveData.shirtWrap.passiveName];


                     //   player.EquippedHoodie.UpdateDescription();
                    }
                    if (SaveData.accessoryOne != null)
                    {
                        player.EquippedAccessory = game.AllEquipment[SaveData.accessoryOne] as Accessory;
                        player.EquippedAccessory.UpgradeSlots = SaveData.acc1Wrap.upgradeSlots;
                        player.EquippedAccessory.Strength = SaveData.acc1Wrap.strength;
                        player.EquippedAccessory.Defense = SaveData.acc1Wrap.tolerance;
                        player.EquippedAccessory.Health = SaveData.acc1Wrap.motivation;

                        if (SaveData.acc1Wrap.passiveName != "")
                            player.EquippedAccessory.PassiveAbility = PassiveManager.allPassives[SaveData.acc1Wrap.passiveName];
                      //  player.EquippedAccessory.UpdateDescription();
                    }
                    if (SaveData.accessoryTwo != null)
                    {
                        player.SecondAccessory = game.AllEquipment[SaveData.accessoryTwo] as Accessory;
                        player.SecondAccessory.UpgradeSlots = SaveData.acc2Wrap.upgradeSlots;
                        player.SecondAccessory.Strength = SaveData.acc2Wrap.strength;
                        player.SecondAccessory.Defense = SaveData.acc2Wrap.tolerance;
                        player.SecondAccessory.Health = SaveData.acc2Wrap.motivation;

                        if (SaveData.acc2Wrap.passiveName != "")
                            player.SecondAccessory.PassiveAbility = PassiveManager.allPassives[SaveData.acc2Wrap.passiveName];
                     //   player.SecondAccessory.UpdateDescription();
                    }

                    //Player attributes
                    player.SocialRank = SaveData.playerRank;
                    player.Money = SaveData.playerMoney;
                    player.Level = SaveData.playerLevel;
                    player.BaseMaxHealth = SaveData.maxMotivation;
                    player.BaseStrength = SaveData.strength;
                    player.BaseDefense = SaveData.tolerance;
                    player.UpdateStats();
                    player.Health = player.BaseMaxHealth;
                    player.ExperienceUntilLevel = SaveData.experienceUntilLevel;
                    player.Experience = SaveData.experience;
                    player.Textbooks = SaveData.textbooks;
                    player.BronzeKeys = SaveData.bronzeKeys;
                    player.SilverKeys = SaveData.silverKeys;
                    player.GoldKeys = SaveData.goldKeys;
                    player.Karma = SaveData.karma;
                    player.Position = new Vector2(650, 610 - player.VitalRec.Height - 120);
                    player.SocialRankIndex = SaveData.socialRankIndex;
                    player.HasCellPhone = SaveData.hasPhone;
                    player.CanJump = true;

                    for (int i = 0; i < player.SocialRankIndex; i++)
                    {
                        player.SocialRank = SocialRankManager.allSocialRanks.ElementAt(i).socialRank;
                        player.LearnPassiveAbility(SocialRankManager.allSocialRanks.ElementAt(i).passiveGrantedThisRank);
                    }

                    Chapter.effectsManager.notificationQueue.Clear();

                    //Story Items
                    for (int i = 0; i < SaveData.storyItemsName.Count; i++)
                    {
                        player.StoryItems.Add(SaveData.storyItemsName[i], SaveData.storyItemsNum[i]);
                    }

                    //Enemy drops
                    for (int i = 0; i < SaveData.enemyDropsName.Count; i++)
                    {
                        player.EnemyDrops.Add(SaveData.enemyDropsName[i], SaveData.enemyDropsNum[i]);
                    }

                    //New equipment / Loot tabs in the inventory. Do this AFTER giving the player all their enemy drops and equipment
                    game.Notebook.Inventory.newWeapon = SaveData.newWeapon;
                    game.Notebook.Inventory.newHat = SaveData.newHat;
                    game.Notebook.Inventory.newShirt = SaveData.newShirt;
                    game.Notebook.Inventory.newAccessory = SaveData.newAccessory;
                    game.Notebook.Inventory.newLoot = SaveData.newLoot;

                    //All quests, set completed or not
                    for (int i = 0; i < game.AllQuests.Count; i++)
                    {
                        for (int j = 0; j < SaveData.allQuests.Count; j++)
                        {
                            if (game.AllQuests.ElementAt(i).Key == SaveData.allQuests[j].questName)
                            {
                                game.AllQuests.ElementAt(i).Value.CompletedQuest = SaveData.allQuests[j].completedQuest;
                                game.AllQuests.ElementAt(i).Value.npcName = SaveData.allQuests[j].npcName;
                                game.AllQuests.ElementAt(i).Value.EnemiesKilledForQuest = SaveData.allQuests[j].enemiesKilledForQuest;
                                game.AllQuests.ElementAt(i).Value.inQuestHelper = SaveData.allQuests[j].inQuestHelper;

                                if (game.AllQuests.ElementAt(i).Value.inQuestHelper == true)
                                {
                                    Game1.questHUD.AddQuestToHelper(game.AllQuests.ElementAt(i).Value);
                                }
                            }
                        }
                    }

                    //Current quests
                    for (int i = 0; i < SaveData.currentQuests.Count; i++)
                    {
                        //All current quests
                        game.CurrentQuests.Add(game.AllQuests[SaveData.currentQuests[i]]);
                    }

                    for (int i = 0; i < SaveData.currentSideQuests.Count; i++)
                    {
                        game.CurrentSideQuests.Add(game.AllQuests[SaveData.currentSideQuests[i]]);
                    }

                    //Side quests for journal
                    for (int i = 0; i < SaveData.prologueCompletedSideQuests.Count; i++)
                    {
                        game.Prologue.CompletedSideQuests.Add(SaveData.prologueCompletedSideQuests[i], game.AllQuests[SaveData.prologueCompletedSideQuests[i]]);
                    }
                    for (int i = 0; i < SaveData.chOneCompletedSideQuests.Count; i++)
                    {
                        game.ChapterOne.CompletedSideQuests.Add(SaveData.chOneCompletedSideQuests[i], game.AllQuests[SaveData.chOneCompletedSideQuests[i]]);
                    }
                    for (int i = 0; i < SaveData.chTwoCompletedSideQuests.Count; i++)
                    {
                        game.ChapterTwo.CompletedSideQuests.Add(SaveData.chTwoCompletedSideQuests[i], game.AllQuests[SaveData.chTwoCompletedSideQuests[i]]);
                    }

                    //Story quests for journal
                    for (int i = 0; i < SaveData.prologueCompletedStoryQuests.Count; i++)
                    {
                        game.Prologue.CompletedStoryQuests.Add(SaveData.prologueCompletedStoryQuests[i], game.AllQuests[SaveData.prologueCompletedStoryQuests[i]]);
                    }
                    for (int i = 0; i < SaveData.chOneCompletedStoryQuests.Count; i++)
                    {
                        game.ChapterOne.CompletedStoryQuests.Add(SaveData.chOneCompletedStoryQuests[i], game.AllQuests[SaveData.chOneCompletedStoryQuests[i]]);
                    }
                    for (int i = 0; i < SaveData.chTwoCompletedStoryQuests.Count; i++)
                    {
                        game.ChapterTwo.CompletedStoryQuests.Add(SaveData.chTwoCompletedStoryQuests[i], game.AllQuests[SaveData.chTwoCompletedStoryQuests[i]]);
                    }

                    //JOURNAL 'NEW' TAGS
                    game.Notebook.Journal.prologueSynopsisRead = SaveData.prologueSynopsisRead;
                    game.Notebook.Journal.chOneSynopsisRead = SaveData.chOneSynopsisRead;
                    game.Notebook.Journal.chTwoSynopsisRead = SaveData.chTwoSynopsisRead;

                    game.Notebook.Journal.prologueSideQuestsRead = SaveData.prologueSideQuestsRead;
                    game.Notebook.Journal.chOneSideQuestsRead = SaveData.chOneSideQuestsRead;
                    game.Notebook.Journal.chTwoSideQuestsRead = SaveData.chTwoSideQuestsRead;

                    game.Notebook.Journal.prologueStoryQuestsRead = SaveData.prologueStoryQuestsRead;
                    game.Notebook.Journal.chOneStoryQuestsRead = SaveData.chOneStoryQuestsRead;
                    game.Notebook.Journal.chTwoStoryQuestsRead = SaveData.chTwoStoryQuestsRead;


                    //Add NPCs first, so you can change their values
                    game.CurrentChapter.AddNPCs();
                    game.SideQuestManager.AddNPCs();

                    //--WHEN ADDING NPCS INTO THE CHAPTERS, ADD THEM IN THE ORDER THAT THEY SHOW UP IN (AS IN, THE ORDER THEY ARE ADDED TO THE CHAPTER IN.)

                    ////NPCs
                    //for (int i = 0; i < game.SideQuestManager.nPCs.Count; i++)
                    //{
                    //    if (SaveData.sideQuestNPCs[i].questName != null)
                    //    {
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue = SaveData.sideQuestNPCs[i].dialogue;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.QuestDialogue = SaveData.sideQuestNPCs[i].questDialogue;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState = SaveData.sideQuestNPCs[i].dialogueState;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight = SaveData.sideQuestNPCs[i].facingRight;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Quest = game.AllQuests[SaveData.sideQuestNPCs[i].questName];
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest = SaveData.sideQuestNPCs[i].acceptedQuest;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.MapName = SaveData.sideQuestNPCs[i].mapName;
                    //    }

                    //    else if (SaveData.sideQuestNPCs[i].trenchCoat == false)
                    //    {
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue = SaveData.sideQuestNPCs[i].dialogue;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState = SaveData.sideQuestNPCs[i].dialogueState;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight = SaveData.sideQuestNPCs[i].facingRight;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Quest = null;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.MapName = SaveData.sideQuestNPCs[i].mapName;
                    //    }
                    //    else
                    //    {
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Dialogue = SaveData.sideQuestNPCs[i].dialogue;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.DialogueState = SaveData.sideQuestNPCs[i].dialogueState;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.FacingRight = SaveData.sideQuestNPCs[i].facingRight;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.Quest = null;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        (game.SideQuestManager.nPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut = SaveData.sideQuestNPCs[i].trenchcoatSoldOut;
                    //        game.SideQuestManager.nPCs.ElementAt(i).Value.MapName = SaveData.sideQuestNPCs[i].mapName;
                    //    }
                    //}
                    //Prologue
                    //for (int i = 0; i < game.Prologue.NPCs.Count; i++)
                    //{
                    //    if (SaveData.prologueNPCWrappers[i].questName != null)
                    //    {
                    //        game.Prologue.NPCs.ElementAt(i).Value.Dialogue = SaveData.prologueNPCWrappers[i].dialogue;
                    //        game.Prologue.NPCs.ElementAt(i).Value.QuestDialogue = SaveData.prologueNPCWrappers[i].questDialogue;
                    //        game.Prologue.NPCs.ElementAt(i).Value.DialogueState = SaveData.prologueNPCWrappers[i].dialogueState;
                    //        game.Prologue.NPCs.ElementAt(i).Value.FacingRight = SaveData.prologueNPCWrappers[i].facingRight;
                    //        game.Prologue.NPCs.ElementAt(i).Value.Quest = game.AllQuests[SaveData.prologueNPCWrappers[i].questName];
                    //        game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest = SaveData.prologueNPCWrappers[i].acceptedQuest;
                    //        game.Prologue.NPCs.ElementAt(i).Value.MapName = SaveData.prologueNPCWrappers[i].mapName;
                    //    }
                    //    else if (SaveData.prologueNPCWrappers[i].trenchCoat == false)
                    //    {
                    //        game.Prologue.NPCs.ElementAt(i).Value.Dialogue = SaveData.prologueNPCWrappers[i].dialogue;
                    //        game.Prologue.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.Prologue.NPCs.ElementAt(i).Value.DialogueState = SaveData.prologueNPCWrappers[i].dialogueState;
                    //        game.Prologue.NPCs.ElementAt(i).Value.FacingRight = SaveData.prologueNPCWrappers[i].facingRight;
                    //        game.Prologue.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        game.Prologue.NPCs.ElementAt(i).Value.MapName = SaveData.prologueNPCWrappers[i].mapName;
                    //    }
                    //    else
                    //    {
                    //        game.Prologue.NPCs.ElementAt(i).Value.Dialogue = SaveData.prologueNPCWrappers[i].dialogue;
                    //        game.Prologue.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.Prologue.NPCs.ElementAt(i).Value.DialogueState = SaveData.prologueNPCWrappers[i].dialogueState;
                    //        game.Prologue.NPCs.ElementAt(i).Value.FacingRight = SaveData.prologueNPCWrappers[i].facingRight;
                    //        game.Prologue.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.Prologue.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        (game.Prologue.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut = SaveData.prologueNPCWrappers[i].trenchcoatSoldOut;
                    //        game.Prologue.NPCs.ElementAt(i).Value.MapName = SaveData.prologueNPCWrappers[i].mapName;
                    //    }
                    //}

                    //Chapter One
                    //for (int i = 0; i < game.ChapterOne.NPCs.Count; i++)
                    //{
                    //    if (SaveData.chapterOneNPCWrappers[i].questName != null)
                    //    {
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterOneNPCWrappers[i].dialogue;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.QuestDialogue = SaveData.chapterOneNPCWrappers[i].questDialogue;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterOneNPCWrappers[i].dialogueState;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterOneNPCWrappers[i].facingRight;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Quest = game.AllQuests[SaveData.chapterOneNPCWrappers[i].questName];
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest = SaveData.chapterOneNPCWrappers[i].acceptedQuest;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.MapName = SaveData.chapterOneNPCWrappers[i].mapName;
                    //    }
                    //    else if (SaveData.chapterOneNPCWrappers[i].trenchCoat == false)
                    //    {
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterOneNPCWrappers[i].dialogue;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterOneNPCWrappers[i].dialogueState;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterOneNPCWrappers[i].facingRight;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.MapName = SaveData.chapterOneNPCWrappers[i].mapName;
                    //    }
                    //    else
                    //    {
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterOneNPCWrappers[i].dialogue;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterOneNPCWrappers[i].dialogueState;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterOneNPCWrappers[i].facingRight;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        (game.ChapterOne.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut = SaveData.chapterOneNPCWrappers[i].trenchcoatSoldOut;
                    //        game.ChapterOne.NPCs.ElementAt(i).Value.MapName = SaveData.chapterOneNPCWrappers[i].mapName;
                    //    }
                    //}

                    //Chapter Two
                    //for (int i = 0; i < game.ChapterTwo.NPCs.Count; i++)
                    //{
                    //    if (SaveData.chapterTwoNPCWrappers[i].questName != null)
                    //    {
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterTwoNPCWrappers[i].dialogue;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.QuestDialogue = SaveData.chapterTwoNPCWrappers[i].questDialogue;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterTwoNPCWrappers[i].dialogueState;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterTwoNPCWrappers[i].facingRight;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Quest = game.AllQuests[SaveData.chapterTwoNPCWrappers[i].questName];
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest = SaveData.chapterTwoNPCWrappers[i].acceptedQuest;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.MapName = SaveData.chapterTwoNPCWrappers[i].mapName;
                    //    }
                    //    else if (SaveData.chapterTwoNPCWrappers[i].trenchCoat == false)
                    //    {
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterTwoNPCWrappers[i].dialogue;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterTwoNPCWrappers[i].dialogueState;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterTwoNPCWrappers[i].facingRight;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.MapName = SaveData.chapterTwoNPCWrappers[i].mapName;
                    //    }
                    //    else
                    //    {
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Dialogue = SaveData.chapterTwoNPCWrappers[i].dialogue;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.QuestDialogue = null;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.DialogueState = SaveData.chapterTwoNPCWrappers[i].dialogueState;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.FacingRight = SaveData.chapterTwoNPCWrappers[i].facingRight;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.Quest = null;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.AcceptedQuest = false;
                    //        (game.ChapterTwo.NPCs.ElementAt(i).Value as TrenchcoatKid).SoldOut = SaveData.chapterTwoNPCWrappers[i].trenchcoatSoldOut;
                    //        game.ChapterTwo.NPCs.ElementAt(i).Value.MapName = SaveData.chapterTwoNPCWrappers[i].mapName;
                    //    }
                    //}

                    player.Health = player.realMaxHealth;

                    game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                    game.Camera.center = game.Camera.centerTarget;
                }
                isolatedFileStream.Close();
            }
        }

        public void InitiateCheck()
        {
            CheckSaves();
        }

        public void CheckSaves()
        {

            IsolatedStorageFile dataFile = IsolatedStorageFile.GetUserStoreForDomain();
            IsolatedStorageFileStream isolatedFileStream = null;

            if (dataFile.FileExists("save1.sav"))
            {
                saveOne = true;

                // Open the file using the established file stream.
                using (isolatedFileStream = dataFile.OpenFile("save1.sav", FileMode.Open, FileAccess.ReadWrite))
                {
                    // Read the data from the file.
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                    // Store the deserialized data object.
                    SaveGame SaveData = (SaveGame)serializer.Deserialize(isolatedFileStream);

                    switch (SaveData.chapterState)
                    {
                        case 0:
                            saveOnePreview = "Prologue";
                            break;
                        case 1:
                            saveOnePreview = "Chapter One";
                            break;
                        case 2:
                            saveOnePreview = "Chapter Two";
                            break;
                        case 3:
                            saveOnePreview = "Chapter Three";
                            break;
                        case 4:
                            saveOnePreview = "Chapter Four";
                            break;
                    }

                    saveOnePreview += "\n" + SaveData.playerRank + ", Level " + SaveData.playerLevel;
                    saveOnePreview += "\n" + "Lunch Money: $" + SaveData.playerMoney.ToString("N2");
                    saveOnePreview += "\n" + "Textbooks: " + SaveData.textbooks;

                    //saveOnePreview += Random fact here 
                }

                isolatedFileStream.Close();
            }
            else
            {
                saveOnePreview = "";
                saveOne = false;
            }

            if (dataFile.FileExists("save2.sav"))
            {
                saveTwo = true;

                // Open the file using the established file stream.
                using (isolatedFileStream = dataFile.OpenFile("save2.sav", FileMode.Open, FileAccess.ReadWrite))
                {
                    // Read the data from the file.
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                    // Store the deserialized data object.
                    SaveGame SaveData = (SaveGame)serializer.Deserialize(isolatedFileStream);

                    switch (SaveData.chapterState)
                    {
                        case 0:
                            saveTwoPreview = "Prologue";
                            break;
                        case 1:
                            saveTwoPreview = "Chapter One";
                            break;
                        case 2:
                            saveTwoPreview = "Chapter Two";
                            break;
                        case 3:
                            saveTwoPreview = "Chapter Three";
                            break;
                        case 4:
                            saveTwoPreview = "Chapter Four";
                            break;
                    }

                    saveTwoPreview += "\n" + SaveData.playerRank + ", Level " + SaveData.playerLevel;
                    saveTwoPreview += "\n" + "Lunch Money: $" + SaveData.playerMoney.ToString("N2");
                    saveTwoPreview += "\n" + "Textbooks: " + SaveData.textbooks;
                    //saveTwoPreview += Random fact here 
                }

                isolatedFileStream.Close();
            }
            else
            {
                saveTwoPreview = "";
                saveTwo = false;
            }

            if (dataFile.FileExists("save3.sav"))
            {
                saveThree = true;

                // Open the file using the established file stream.
                using (isolatedFileStream = dataFile.OpenFile("save3.sav", FileMode.Open, FileAccess.ReadWrite))
                {
                    // Read the data from the file.
                    XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
                    // Store the deserialized data object.
                    SaveGame SaveData = (SaveGame)serializer.Deserialize(isolatedFileStream);
                    switch (SaveData.chapterState)
                    {
                        case 0:
                            saveThreePreview = "Prologue";
                            break;
                        case 1:
                            saveThreePreview = "Chapter One";
                            break;
                        case 2:
                            saveThreePreview = "Chapter Two";
                            break;
                        case 3:
                            saveThreePreview = "Chapter Three";
                            break;
                        case 4:
                            saveThreePreview = "Chapter Four";
                            break;
                    }

                    saveThreePreview += "\n" + SaveData.playerRank + ", Level " + SaveData.playerLevel;
                    saveThreePreview += "\n" + "Lunch Money: $" + SaveData.playerMoney.ToString("N2");
                    saveThreePreview += "\n" + "Textbooks: " + SaveData.textbooks;
                    //saveThreePreview += Random fact here 
                }

                isolatedFileStream.Close();
            }
            else
            {
                saveThree = false;
                saveThreePreview = "";
            }
        }

        public void InitiateDeleteSave(String saveFile)
        {
            deleteFileName = saveFile;

            DeleteSave();

        }

        public void DeleteSave()
        {
            IsolatedStorageFile dataFile = IsolatedStorageFile.GetUserStoreForDomain();

            if (dataFile.FileExists(deleteFileName))
                dataFile.DeleteFile(deleteFileName);

        }
    }
}
