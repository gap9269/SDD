using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    [Serializable]
    public class MapZoneWrapper
    {
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
        public List<List<InteractiveWrapper>> mapInteractiveObjects;
        public List<List<Boolean>> mapQuestCompleted;
        public List<List<Boolean>> mapQuestRewardsTaken;

        public MapZoneWrapper() { }

        public MapZoneWrapper(bool doYouLikePie)
        {
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
        }

        public void SaveZoneData(List<List<Boolean>> mapStoryItemsPicked, List<List<Boolean>> mapChestOpened, List<List<Boolean>> portalsLocked, List<List<Vector2>> moveBlockPositions, List<List<Boolean>> switchesActive, List<List<int>> mapEnemiesKilled, List<List<List<Boolean>>> takenContents, List<Boolean> discoveredMap, List<List<Boolean>> mapCollectiblePicked, List<List<Boolean>> mapCollectibleAble, List<List<InteractiveWrapper>> mapInteractiveObjects, List<List<Boolean>> mapQuestCompleted, List<List<Boolean>> mapQuestRewardsTaken)
        {             
            this.mapStoryItemsPicked = mapStoryItemsPicked;
            this.mapChestOpened = mapChestOpened;
            this.portalsLocked = portalsLocked;
            this.moveBlockPositions = moveBlockPositions;
            this.switchesActive = switchesActive;
            this.mapEnemiesKilled = mapEnemiesKilled;
            this.takenContents = takenContents;
            this.discoveredMap = discoveredMap;
            this.mapCollectiblePicked = mapCollectiblePicked;
            this.mapCollectibleAble = mapCollectibleAble;
            this.mapInteractiveObjects = mapInteractiveObjects;
            this.mapQuestCompleted = mapQuestCompleted;
            this.mapQuestRewardsTaken = mapQuestRewardsTaken;
        }
    }
}