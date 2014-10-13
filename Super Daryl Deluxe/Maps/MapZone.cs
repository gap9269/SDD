using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ISurvived
{
    public class MapZone
    {
        public Dictionary<String, MapClass> maps;
        protected ContentManager content;
        Game1 game;

        public List<List<Boolean>> mapStoryItemsPicked;
        public List<List<Boolean>> mapChestOpened;
        public List<List<Boolean>> portalsLocked;
        public List<List<Vector2>> moveBlockPositions;
        public List<List<Boolean>> switchesActive;
        public List<List<int>> mapEnemiesKilled;
        public List<List<List<Boolean>>> takenContents;
        public List<Boolean> discoveredMap;
        public List<List<Boolean>> mapCollectiblePicked;
        public List<List<Boolean>> mapCollectibleAble;
        public List<List<Boolean>> mapQuestCompleted;
        public List<List<Boolean>> mapQuestRewardsTaken;
        public List<List<InteractiveWrapper>> mapInteractiveObjects;

        public MapZone(Game1 g)
        {
            content = new ContentManager(g.Services);
            maps = new Dictionary<string, MapClass>();
            game = g;
            //Save structures
            mapStoryItemsPicked = new List<List<bool>>();
            mapChestOpened = new List<List<bool>>();
            portalsLocked = new List<List<bool>>();
            moveBlockPositions = new List<List<Vector2>>();
            switchesActive = new List<List<bool>>();
            mapEnemiesKilled = new List<List<int>>();
            takenContents = new List<List<List<bool>>>();
            discoveredMap = new List<bool>();
            mapCollectiblePicked = new List<List<bool>>();
            mapCollectibleAble = new List<List<bool>>();
            mapInteractiveObjects = new List<List<InteractiveWrapper>>();
            mapQuestCompleted = new List<List<bool>>();
            mapQuestRewardsTaken = new List<List<bool>>();
            content.RootDirectory = "Content";
        }

        /// <summary>
        /// This should only be called when a new save is created. It populates this zone's save structures with empty/starter values so the 
        /// SaveLoadManager can save and load from it later.
        /// </summary>
        public void PopulateSaveDataOnNewSaveSlot()
        {
            //--Story items in maps
            for (int i = 0; i < maps.Count; i++)
            {
                mapStoryItemsPicked.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.StoryItems.Count; j++)
                {
                    mapStoryItemsPicked[i].Add(maps.ElementAt(i).Value.StoryItems[j].PickedUp);
                }
            }

            //--Chests in maps
            for (int i = 0; i < maps.Count; i++)
            {
                mapChestOpened.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.TreasureChests.Count; j++)
                {
                    mapChestOpened[i].Add(maps.ElementAt(i).Value.TreasureChests[j].Opened);
                }
            }

            //Portal locks
            for (int i = 0; i < maps.Count; i++)
            {
                portalsLocked.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.Portals.Count; j++)
                {
                    if (maps.ElementAt(i).Value.Portals.ElementAt(j).Key.ItemNameToUnlock == null)
                        portalsLocked[i].Add(false);
                    else
                        portalsLocked[i].Add(true);
                }
            }

            //Move block positions
            for (int i = 0; i < maps.Count; i++)
            {
                moveBlockPositions.Add(new List<Vector2>());

                for (int j = 0; j < maps.ElementAt(i).Value.MoveBlocks.Count; j++)
                {
                    moveBlockPositions[i].Add(maps.ElementAt(i).Value.MoveBlocks[j].Position);
                }
            }

            //Switches active 
            for (int i = 0; i < maps.Count; i++)
            {
                switchesActive.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.Switches.Count; j++)
                {
                    switchesActive[i].Add(maps.ElementAt(i).Value.Switches[j].Active);
                }
            }

            //Map Quest Enemies
            for (int i = 0; i < maps.Count; i++)
            {
                mapEnemiesKilled.Add(new List<int>());

                for (int j = 0; j < maps.ElementAt(i).Value.EnemyNames.Count; j++)
                {
                    mapEnemiesKilled[i].Add(maps.ElementAt(i).Value.EnemiesKilledForQuest[j]);
                }
            }

            //--Locker items taken or not
            for (int i = 0; i < maps.Count; i++)
            {
                takenContents.Add(new List<List<bool>>());

                for (int j = 0; j < maps.ElementAt(i).Value.Lockers.Count; j++)
                {
                    takenContents[i].Add(maps.ElementAt(i).Value.Lockers[j].TakenContents);
                }
            }

            //Discovered maps or not
            for (int i = 0; i < maps.Count; i++)
            {
                discoveredMap.Add(maps.ElementAt(i).Value.Discovered);
            }

            //--Collectibles in maps
            for (int i = 0; i < maps.Count; i++)
            {
                mapCollectiblePicked.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    mapCollectiblePicked[i].Add(maps.ElementAt(i).Value.Collectibles[j].PickedUp);
                }
            }
            //Collectibles are able to be picked or not in maps
            for (int i = 0; i < maps.Count; i++)
            {
                mapCollectibleAble.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    mapCollectibleAble[i].Add(maps.ElementAt(i).Value.Collectibles[j].AbleToPickUp);
                }
            }

            //--Interactive objects in maps
            for (int i = 0; i < maps.Count; i++)
            {
                mapInteractiveObjects.Add(new List<InteractiveWrapper>());

                for (int j = 0; j < maps.ElementAt(i).Value.InteractiveObjects.Count; j++)
                {
                    mapInteractiveObjects[i].Add(new InteractiveWrapper(maps.ElementAt(i).Value.InteractiveObjects[j].State, maps.ElementAt(i).Value.InteractiveObjects[j].Finished, maps.ElementAt(i).Value.InteractiveObjects[j].Health));
                }
            }

            //--Completed map quests
            for (int i = 0; i < maps.Count; i++)
            {
                mapQuestCompleted.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    mapQuestCompleted[i].Add(maps.ElementAt(i).Value.MapQuestSigns[j].CompletedQuest);
                }
            }

            //--Map quest rewards taken
            for (int i = 0; i < maps.Count; i++)
            {
                mapQuestRewardsTaken.Add(new List<bool>());

                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    mapQuestRewardsTaken[i].Add(maps.ElementAt(i).Value.MapQuestSigns[j].CompletedQuest);
                }
            }
        }

        public void SaveMapDataToWrapper()
        {

            //--Story items in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.StoryItems.Count; j++)
                {
                    mapStoryItemsPicked[i][j] = (maps.ElementAt(i).Value.StoryItems[j].PickedUp);
                }
            }

            //--Chests in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.TreasureChests.Count; j++)
                {
                    mapChestOpened[i][j] = (maps.ElementAt(i).Value.TreasureChests[j].Opened);
                }
            }

            //Portal locks
            for (int i = 0; i < maps.Count; i++)
            {
              

                for (int j = 0; j < maps.ElementAt(i).Value.Portals.Count; j++)
                {
                    if (maps.ElementAt(i).Value.Portals.ElementAt(j).Key.ItemNameToUnlock == null)
                        portalsLocked[i][j] = (false);
                    else
                        portalsLocked[i][j] = (true);
                }
            }

            //Move block positions
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.MoveBlocks.Count; j++)
                {
                    moveBlockPositions[i][j] = (maps.ElementAt(i).Value.MoveBlocks[j].Position);
                }
            }

            //Switches active 
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.Switches.Count; j++)
                {
                    switchesActive[i][j] = (maps.ElementAt(i).Value.Switches[j].Active);
                }
            }
            
            //Map Quest Enemies
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.EnemyNames.Count; j++)
                {
                    mapEnemiesKilled[i][j] = (maps.ElementAt(i).Value.EnemiesKilledForQuest[j]);
                }
            }

            //--Locker items taken or not
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.Lockers.Count; j++)
                {
                    takenContents[i][j] = (maps.ElementAt(i).Value.Lockers[j].TakenContents);
                }
            }

            //Discovered maps or not
            for (int i = 0; i < maps.Count; i++)
            {
                discoveredMap[i] = (maps.ElementAt(i).Value.Discovered);
            }

            //--Collectibles in maps
            for (int i = 0; i < maps.Count; i++)
            {


                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    mapCollectiblePicked[i][j] = (maps.ElementAt(i).Value.Collectibles[j].PickedUp);
                }
            }
            //Collectibles are able to be picked or not in maps
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    mapCollectibleAble[i][j] = (maps.ElementAt(i).Value.Collectibles[j].AbleToPickUp);
                }
            }

            //--Interactive objects in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.InteractiveObjects.Count; j++)
                {
                    mapInteractiveObjects[i][j] = (new InteractiveWrapper(maps.ElementAt(i).Value.InteractiveObjects[j].State, maps.ElementAt(i).Value.InteractiveObjects[j].Finished, maps.ElementAt(i).Value.InteractiveObjects[j].Health));
                }
            }

            //Map quest completed
            for (int i = 0; i < maps.Count; i++)
            {

                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    mapQuestCompleted[i][j] = (maps.ElementAt(i).Value.MapQuestSigns[j].CompletedQuest);
                }
            }

            //--Map quest rewards taken
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    mapQuestRewardsTaken[i][j] = (maps.ElementAt(i).Value.MapQuestSigns[j].GivenRewards);
                }
            }


            MapZoneWrapper wrapper = new MapZoneWrapper(true);

            wrapper = game.schoolZoneWrapper;

            wrapper.discoveredMap = discoveredMap;
            wrapper.mapChestOpened = mapChestOpened;
            wrapper.mapCollectibleAble = mapCollectibleAble;
            wrapper.mapCollectiblePicked = mapCollectiblePicked;
            wrapper.mapEnemiesKilled = mapEnemiesKilled;
            wrapper.mapInteractiveObjects = mapInteractiveObjects;
            wrapper.mapStoryItemsPicked = mapStoryItemsPicked;
            wrapper.moveBlockPositions = moveBlockPositions;
            wrapper.portalsLocked = portalsLocked;
            wrapper.switchesActive = switchesActive;
            wrapper.takenContents = takenContents;
            wrapper.mapQuestRewardsTaken = mapQuestRewardsTaken;
            wrapper.mapQuestCompleted = mapQuestCompleted;

            game.schoolZoneWrapper = wrapper;
        }

        public void UnloadSchoolZone()
        {
            content.Unload();
            maps.Clear();
            game.ResetEnemySpriteList();
        }

        public virtual void LoadEnemyData()
        {

        }

        public void LoadMapData(MapZoneWrapper wrapper)
        {
            mapStoryItemsPicked = wrapper.mapStoryItemsPicked;
            //Story items in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.StoryItems.Count; j++)
                {
                    maps.ElementAt(i).Value.StoryItems[j].PickedUp = wrapper.mapStoryItemsPicked[i][j];
                }
            }

            mapChestOpened = wrapper.mapChestOpened;
            //Chests in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.TreasureChests.Count; j++)
                {
                    maps.ElementAt(i).Value.TreasureChests[j].Opened = wrapper.mapChestOpened[i][j];

                    if (maps.ElementAt(i).Value.TreasureChests[j].Opened)
                    {
                        maps.ElementAt(i).Value.TreasureChests[j].Empty = true;
                    }
                }
            }

            portalsLocked = wrapper.portalsLocked;
            //Portals are locked or not
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.Portals.Count; j++)
                {
                    if (wrapper.portalsLocked[i][j] == false)
                    {
                        maps.ElementAt(i).Value.Portals.ElementAt(j).Key.ItemNameToUnlock = null;
                        maps.ElementAt(i).Value.Portals.ElementAt(j).Key.PortalTexture = Game1.portalTexture;
                    }

                }
            }

            moveBlockPositions = wrapper.moveBlockPositions;
            //Move blocks in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.MoveBlocks.Count; j++)
                {
                    maps.ElementAt(i).Value.MoveBlocks[j].Position = wrapper.moveBlockPositions[i][j];
                }
            }

            switchesActive = wrapper.switchesActive;
            //Switches in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.Switches.Count; j++)
                {
                    maps.ElementAt(i).Value.Switches[j].Active = wrapper.switchesActive[i][j];
                }
            }

            mapEnemiesKilled = wrapper.mapEnemiesKilled;
            //Map quest enemies
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.EnemyNames.Count; j++)
                {
                    maps.ElementAt(i).Value.EnemiesKilledForQuest[j] = wrapper.mapEnemiesKilled[i][j];
                }
            }

            takenContents = wrapper.takenContents;
            //Locker items taken
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.Lockers.Count; j++)
                {
                    maps.ElementAt(i).Value.Lockers[j].TakenContents = wrapper.takenContents[i][j];
                    maps.ElementAt(i).Value.Lockers[j].UpdateContentsOnLoad();
                }
            }

            discoveredMap = wrapper.discoveredMap;
            //Discovered maps
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.Discovered = wrapper.discoveredMap[i];
            }


            mapInteractiveObjects = wrapper.mapInteractiveObjects;
            //Interactive objects in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.InteractiveObjects.Count; j++)
                {
                    maps.ElementAt(i).Value.InteractiveObjects[j].State = wrapper.mapInteractiveObjects[i][j].state;
                    maps.ElementAt(i).Value.InteractiveObjects[j].Finished = wrapper.mapInteractiveObjects[i][j].finished;
                    maps.ElementAt(i).Value.InteractiveObjects[j].Health = wrapper.mapInteractiveObjects[i][j].health;
                }
            }

            mapCollectiblePicked = wrapper.mapCollectiblePicked;
            //Collectibles in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    maps.ElementAt(i).Value.Collectibles[j].PickedUp = wrapper.mapCollectiblePicked[i][j];
                }
            }

            mapCollectibleAble = wrapper.mapCollectibleAble;
            //Collectibles able to be picked in maps
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.Collectibles.Count; j++)
                {
                    maps.ElementAt(i).Value.Collectibles[j].AbleToPickUp = wrapper.mapCollectibleAble[i][j];
                }
            }

            mapQuestCompleted = wrapper.mapQuestCompleted;
            //Map quest completed
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    maps.ElementAt(i).Value.MapQuestSigns[j].CompletedQuest = wrapper.mapQuestCompleted[i][j];
                }
            }

            mapQuestRewardsTaken = wrapper.mapQuestRewardsTaken;
            //Map quest rewards taken
            for (int i = 0; i < maps.Count; i++)
            {
                for (int j = 0; j < maps.ElementAt(i).Value.MapQuestSigns.Count; j++)
                {
                    maps.ElementAt(i).Value.MapQuestSigns[j].GivenRewards = wrapper.mapQuestRewardsTaken[i][j];
                }
            }
        }
    }
}
