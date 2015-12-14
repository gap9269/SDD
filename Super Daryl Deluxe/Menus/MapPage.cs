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
using System.IO;
using Newtonsoft.Json;
namespace ISurvived
{
    public class MapNode
    {
        public String name, parentName;
        public int xPos, yPos, iconPosX, iconPosY, width, height;
        public MapGroup parent;
        public Boolean discovered, trenchcoat, bathroom, flyingLocker, locker;
        public int treasure, sideAvailable, sideInProgress, sideComplete, storyAvailable, storyInprogress, storyComplete;

        public struct Locks
        {
            public String type, destinationMapName;
            public int x, y;
        }

        public List<Locks> locks;
    }

    public class MapGroup
    {
        public string name;
        public string parentName;
        public int width, height;
        public Dictionary<String, MapNode> mapNodes;
        public String entranceMap;
    }

    public class MapPage
    {
        public Dictionary<String, MapGroup> mapGroups;
        public Dictionary<String, Texture2D> menuTextures;
        public Dictionary<String, Texture2D> activeMapButtonTextures;
        public Dictionary<String, Texture2D> staticMapButtonTextures;
        public Dictionary<String, Texture2D> mapIconTextures;
        Game1 game;

        //Map Pieces
        public Dictionary<String, Dictionary<String, Texture2D>> mapPieceTextures;

        public Boolean lastMouseDown, currentMouseDown, dragging;
        public int afterDragTimer;
        int mapPositionX, mapPositionY;
    //    float lastMouseX, lastMouseY, currentMouseX, currentMouseY;

        int menuPosX = 821;
        public String selectedMapGroupName, darylMapGroupName;
        List<String> currentMapButtonNames;
        List<Button> currentMapButtons;
        Button waterFallsHighSchoolButton, menuArrowButton;

        Vector2 darylPos;

        public enum MenuState
        {
            open, opening, closing, closed
        }
        public MenuState menuState;

        public MapPage(Game1 g)
        {
            game = g;

            //Don't add WFHS because it's always the root node
            MakeOrResetMapButtons();
            mapPieceTextures = new Dictionary<string, Dictionary<string, Texture2D>>();
            waterFallsHighSchoolButton = new Button(new Rectangle(1021, 100, 184, 46));
            menuArrowButton = new Button(new Rectangle(947, 51, 35, 29));
            MakeMapGroups();
            LoadMapNodes();
        }

        public void MakeOrResetMapButtons()
        {
            currentMapButtonNames = new List<string>();
            currentMapButtonNames.Add("The Vents");
            currentMapButtonNames.Add("Science");
            currentMapButtonNames.Add("Music & Art");
            currentMapButtonNames.Add("History");
            currentMapButtonNames.Add("Literature");
            currentMapButtons = new List<Button>();
            currentMapButtons.Add(new Button(new Rectangle(1046, 0, 160, 35)));
            currentMapButtons.Add(new Button(new Rectangle(1046, 0, 160, 35)));
            currentMapButtons.Add(new Button(new Rectangle(1046, 0, 160, 35)));
            currentMapButtons.Add(new Button(new Rectangle(1046, 0, 160, 35)));
            currentMapButtons.Add(new Button(new Rectangle(1046, 0, 160, 35)));
        }

        public void MakeMapGroups()
        {
            mapGroups = new Dictionary<string, MapGroup>();
            mapGroups.Add("Water Falls High School", new MapGroup() { name = "Water Falls High School", mapNodes = new Dictionary<string,MapNode>(), width = 850, height = 1100 });
            mapGroups.Add("The Vents", new MapGroup() { name = "The Vents", mapNodes = new Dictionary<string, MapNode>(), parentName = "Water Falls High School", entranceMap = "Upper Vents I" });
            mapGroups.Add("Science", new MapGroup() { name = "Science", mapNodes = new Dictionary<string, MapNode>(), parentName = "Water Falls High School", entranceMap = "Intro to Science", width = 1200, height = -200 });
            mapGroups.Add("Music & Art", new MapGroup() { name = "Music & Art", mapNodes = new Dictionary<string, MapNode>(), parentName = "Water Falls High School", entranceMap = "Intro To Music" });
            mapGroups.Add("History", new MapGroup() { name = "History", mapNodes = new Dictionary<string, MapNode>(), parentName = "Water Falls High School", entranceMap = "Intro to History", width = 0, height = 0 });
            mapGroups.Add("Literature", new MapGroup() { name = "Literature", mapNodes = new Dictionary<string, MapNode>(), parentName = "Water Falls High School", entranceMap = "The Goats" });

            mapGroups.Add("Pyramid", new MapGroup() { name = "Pyramid", mapNodes = new Dictionary<string, MapNode>(), parentName = "History", entranceMap = "The Goats" });
        }

        public void AddSelectedMapButtons()
        {
            MakeOrResetMapButtons();
            int indexOfCurrentMapGroup = currentMapButtonNames.IndexOf(selectedMapGroupName);
            for (int i = 0; i < mapGroups.Count; i++ )
            {
                if (mapGroups.ElementAt(i).Value.parentName == selectedMapGroupName)
                {
                    currentMapButtonNames.Insert(indexOfCurrentMapGroup + 1, mapGroups.ElementAt(i).Value.name);
                    currentMapButtons.Insert(indexOfCurrentMapGroup + 1, new Button(new Rectangle(1078, 0, 160, 35)));
                }
            }
        }

        public void LoadMapNodes()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(MapNodeJSON.mapNodeJSON)))
            {
                IEnumerable<MapNode> allMapNodes = ReadJson<MapNode>(stream);

                foreach (var mNode in allMapNodes)
                {
                    mapGroups[mNode.parentName].mapNodes.Add(mNode.name, mNode);
                }
            }
        }

        public void CloseMenu()
        {
            if (menuState == MenuState.closing)
            {
                if (menuPosX < 1100)
                {
                    float distance = menuPosX - 300;

                    menuPosX += (int)(1.2f * (distance / 40));

                    if (menuPosX > 1100)
                        menuPosX = 1100;

                }
                else
                {
                    menuPosX = 1100;
                    menuState = MenuState.closed;
                    menuArrowButton.ButtonRecX = 1221;
                }
            }
        }

        public void OpenMenu()
        {
            if (menuState == MenuState.opening)
            {
                if (menuPosX > 821)
                {
                    float distance = menuPosX - 300 ;

                    menuPosX -= (int)(1.2f * (distance / 40));

                    if (menuPosX < 821)
                        menuPosX = 821;
                }
                else
                {
                    menuPosX = 821;
                    menuState = MenuState.open;
                    menuArrowButton.ButtonRecX = 947;
                }
            }
        }

        public void OpenMapPage()
        {
            //Set the defaults just in case. They should be overwritten below
            darylMapGroupName = "Water Falls High School";
            selectedMapGroupName = "Water Falls High School";

            for (int i = 0; i < mapGroups.Count; i++)
            {
                for (int j = 0; j < mapGroups.ElementAt(i).Value.mapNodes.Count; j++)
                {
                    //Set the current iteration's map node
                    MapNode mNode = mapGroups.ElementAt(i).Value.mapNodes[mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Key];
                    mNode.treasure = 0;
                    mNode.storyInprogress = 0;
                    mNode.storyAvailable = 0;
                    mNode.storyComplete = 0;
                    mNode.sideInProgress = 0;
                    mNode.sideAvailable = 0;
                    mNode.sideComplete = 0;
                    mNode.flyingLocker = false;
                    for (int k = 0; k < Game1.schoolMaps.maps[mNode.name].TreasureChests.Count; k++)
                    {
                        if (!Game1.schoolMaps.maps[mNode.name].TreasureChests[k].Opened)
                            mNode.treasure++;
                    }
                    for (int k = 0; k < Game1.schoolMaps.maps[mNode.name].InteractiveObjects.Count; k++)
                    {
                        if (Game1.schoolMaps.maps[mNode.name].InteractiveObjects[k] is LivingLocker)
                        {
                            mNode.flyingLocker = true;
                            break;
                        }
                    }

                    for (int k = 0; k < game.SideQuestManager.nPCs.Count; k++)
                    {
                        NPC npc = game.SideQuestManager.nPCs.ElementAt(k).Value;
                        if (npc.MapName == mNode.name)
                        {
                            if (npc.Quest != null)
                            {
                                if (npc.AcceptedQuest)
                                {
                                    if (npc.Quest.CompletedQuest)
                                    {
                                        if (npc.Quest.StoryQuest)
                                            mNode.storyComplete++;
                                        else
                                            mNode.sideComplete++;
                                    }
                                    else
                                    {
                                        if (npc.Quest.StoryQuest)
                                            mNode.storyInprogress++;
                                        else
                                            mNode.sideInProgress++;
                                    }
                                }
                                else
                                {
                                    if (npc.Quest.StoryQuest)
                                        mNode.storyAvailable++;
                                    else
                                        mNode.sideAvailable++;
                                }
                            }
                        }
                    }

                    for (int k = 0; k < game.CurrentChapter.NPCs.Count; k++)
                    {
                        NPC npc = game.CurrentChapter.NPCs.ElementAt(k).Value;
                        if (npc.MapName == mNode.name)
                        {
                            if (npc.Quest != null)
                            {
                                if (npc.AcceptedQuest)
                                {
                                    if (npc.Quest.CompletedQuest)
                                    {
                                        if (npc.Quest.StoryQuest)
                                            mNode.storyComplete++;
                                        else
                                            mNode.sideComplete++;
                                    }
                                    else
                                    {
                                        if (npc.Quest.StoryQuest)
                                            mNode.storyInprogress++;
                                        else
                                            mNode.sideInProgress++;
                                    }
                                }
                                else
                                {
                                    if (npc.Quest.StoryQuest)
                                        mNode.storyAvailable++;
                                    else
                                        mNode.sideAvailable++;
                                }
                            }
                        }
                    }

                    if (mNode.name == game.CurrentChapter.CurrentMap.MapName)
                    {
                        //Set the map group name based on where you are located
                        darylMapGroupName = mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Value.parentName;
                        selectedMapGroupName = darylMapGroupName;

                        darylPos = new Vector2(mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Value.xPos + mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Value.width / 2, mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Value.yPos + mapGroups.ElementAt(i).Value.mapNodes.ElementAt(j).Value.height / 2 + 15);

                        mapPositionX = (int)darylPos.X - 560;
                        mapPositionY = (int)darylPos.Y - 396;

                    }
                }
            }
        }

        public void Update()
        {
            if (game.chapterState == Game1.ChapterState.chapterOne && game.ChapterOne.ChapterOneBooleans["openedMaps"] == false && game.CurrentSideQuests.Contains(game.ChapterOne.learningAboutMaps))
                game.ChapterOne.ChapterOneBooleans["openedMaps"] = true;

            #region Change Pages
            if (DarylsNotebook.journalTab.Clicked() && !dragging && afterDragTimer <= 0)
            {
                game.Notebook.state = DarylsNotebook.State.journal;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                Chapter.effectsManager.RemoveToolTip();
            }

            if (DarylsNotebook.inventoryTab.Clicked() && !dragging && afterDragTimer <= 0)
            {
                game.Notebook.state = DarylsNotebook.State.inventory;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                Chapter.effectsManager.RemoveToolTip();
            }

            if (DarylsNotebook.bioTab.Clicked() && !dragging && afterDragTimer <= 0)
            {
                game.Notebook.state = DarylsNotebook.State.bios;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                Chapter.effectsManager.RemoveToolTip();
            }

            if (DarylsNotebook.questsTab.Clicked() && !dragging && afterDragTimer <= 0)
            {
                game.Notebook.state = DarylsNotebook.State.quests;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                Chapter.effectsManager.RemoveToolTip();
            }

            if (DarylsNotebook.combosTab.Clicked() && !dragging && afterDragTimer <= 0)
            {
                game.Notebook.state = DarylsNotebook.State.combos;
                Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                Chapter.effectsManager.RemoveToolTip();
            }
            #endregion

            #region Move map around
            MouseState mouse = Mouse.GetState();
            lastMouseDown = currentMouseDown;
            currentMouseDown = mouse.LeftButton == ButtonState.Pressed;
            
            if ((lastMouseDown && currentMouseDown) || MyGamePad.AHeld())
            {
                if (Math.Abs(Cursor.currentPos.X - Cursor.lastPos.X) > 5 || Math.Abs(Cursor.currentPos.Y - Cursor.lastPos.Y) > 5)
                    dragging = true;
            }
            else if (MyGamePad.DownPadHeld() || MyGamePad.UpPadHeld() || MyGamePad.LeftPadHeld() || MyGamePad.RightPadHeld())
            {
                dragging = true;
            }
            else
            {
                if (dragging)
                {
                    dragging = false;
                    afterDragTimer = 10;
                }
            }

            if (dragging)
            {
                if (MyGamePad.DownPadHeld() || MyGamePad.UpPadHeld() || MyGamePad.LeftPadHeld() || MyGamePad.RightPadHeld())
                {
                    if(MyGamePad.DownPadHeld())
                        mapPositionY += 10;
                    if (MyGamePad.UpPadHeld())
                        mapPositionY -= 10;
                    if (MyGamePad.RightPadHeld())
                        mapPositionX += 10;
                    if (MyGamePad.LeftPadHeld())
                        mapPositionX -= 10;
                }
                else
                {
                    mapPositionX -= (int)(Cursor.currentPos.X - Cursor.lastPos.X);
                    mapPositionY -= (int)(Cursor.currentPos.Y - Cursor.lastPos.Y);
                }
            }

            if (afterDragTimer > 0)
                afterDragTimer--;

            if (mapPositionX > mapGroups[selectedMapGroupName].width)
                mapPositionX = mapGroups[selectedMapGroupName].width;
            else if (mapPositionX < -300)
                mapPositionX = -300;

            if (mapPositionY > mapGroups[selectedMapGroupName].height)
                mapPositionY = mapGroups[selectedMapGroupName].height;
            else if (mapPositionY < -300)
                mapPositionY = -300;
            #endregion

            if (menuState == MenuState.open)
            {
                if (menuArrowButton.Clicked() && !dragging && afterDragTimer <= 0)
                {
                    menuState = MenuState.closing;
                }
            }
            else
            {
                if (menuArrowButton.Clicked() && !dragging && afterDragTimer <= 0)
                {
                    menuState = MenuState.opening;
                }
            }

            CloseMenu();
            OpenMenu();

            if (menuState == MenuState.open && !dragging && afterDragTimer <= 0)
            {
                for (int i = 0; i < currentMapButtons.Count; i++)
                {
                    if (Game1.schoolMaps.maps[mapGroups[currentMapButtonNames[i]].entranceMap].Discovered)
                    {
                        if (currentMapButtons[i].Clicked())
                        {
                            selectedMapGroupName = currentMapButtonNames[i];

                            if (mapGroups[selectedMapGroupName].parentName == "Water Falls High School")
                                AddSelectedMapButtons();
                        }
                    }
                }
                if (waterFallsHighSchoolButton.Clicked())
                {
                    selectedMapGroupName = "Water Falls High School";
                    MakeOrResetMapButtons();
                }
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(menuTextures["background"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            //Draw the map
            for (int i = 0; i < mapGroups[selectedMapGroupName].mapNodes.Count; i++)
            {
                MapNode mNode = mapGroups[selectedMapGroupName].mapNodes.ElementAt(i).Value;

                if (Game1.schoolMaps.maps[mNode.name].Discovered)
                {
                    s.Draw(mapPieceTextures[selectedMapGroupName][mNode.name], new Vector2(mNode.xPos - mapPositionX, mNode.yPos - mapPositionY), Color.White);

                    if (i == 0)
                    {
                        switch (selectedMapGroupName)
                        {
                            case "Water Falls High School":
                                if (Game1.schoolMaps.maps["Intro to Science"].Discovered)
                                    s.Draw(mapPieceTextures[selectedMapGroupName]["Science"], new Vector2(300 - mapPositionX, 389 - mapPositionY), Color.White);
                                //if (Game1.schoolMaps.maps["Intro to Science"].Discovered)
                                //    s.Draw(mapPieceTextures[selectedMapGroupName]["Literature"], new Vector2(344 - mapPositionX, 59 - mapPositionY), Color.White);
                                if (Game1.schoolMaps.maps["Upper Vents I"].Discovered)
                                    s.Draw(mapPieceTextures[selectedMapGroupName]["The Vents"], new Vector2(0 - mapPositionX, 242 - mapPositionY), Color.White);
                                if (Game1.schoolMaps.maps["Intro to History"].Discovered)
                                    s.Draw(mapPieceTextures[selectedMapGroupName]["History"], new Vector2(536 - mapPositionX, 355 - mapPositionY), Color.White);
                                //if (Game1.schoolMaps.maps["Intro to Science"].Discovered)
                                //    s.Draw(mapPieceTextures[selectedMapGroupName]["Library"], new Vector2(619 - mapPositionX, 1438 - mapPositionY), Color.White);
                                if (Game1.schoolMaps.maps["Intro To Music"].Discovered)
                                    s.Draw(mapPieceTextures[selectedMapGroupName]["Music & Art"], new Vector2(382 - mapPositionX, 1438 - mapPositionY), Color.White);
                                break;

                        }
                    }

                    List<Texture2D> mapIcons = new List<Texture2D>();

                    for (int j = 0; j < mNode.storyAvailable; j++)
                        mapIcons.Add(mapIconTextures["Story Quest Available"]);

                    for (int j = 0; j < mNode.storyInprogress; j++)
                        mapIcons.Add(mapIconTextures["Story Quest In-Progress"]);

                    for (int j = 0; j < mNode.storyComplete; j++)
                        mapIcons.Add(mapIconTextures["Story Quest Complete"]);

                    if (mNode.bathroom)
                        mapIcons.Add(mapIconTextures["Bathroom"]);
                    if (mNode.flyingLocker)
                        mapIcons.Add(mapIconTextures["Flying Locker"]);
                    if (mNode.locker)
                        mapIcons.Add(mapIconTextures["Locker"]);
                    if (mNode.trenchcoat)
                        mapIcons.Add(mapIconTextures["Trenchcoat"]);

                    for (int j = 0; j < mNode.sideAvailable; j++)
                        mapIcons.Add(mapIconTextures["Side Quest Available"]);

                    for (int j = 0; j < mNode.sideInProgress; j++)
                        mapIcons.Add(mapIconTextures["Side Quest In-Progress"]);

                    for (int j = 0; j < mNode.sideComplete; j++)
                        mapIcons.Add(mapIconTextures["Side Quest Complete"]);

                    for (int j = 0; j < mNode.treasure; j++)
                        mapIcons.Add(mapIconTextures["Chest"]);

                    if (mNode.locks != null)
                    {
                        for (int j = 0; j < mNode.locks.Count; j++)
                        {
                            for (int k = 0; k < Game1.schoolMaps.maps[mNode.name].Portals.Count; k++)
                            {
                                if (Game1.schoolMaps.maps[mNode.name].Portals.ElementAt(k).Value.MapName == mNode.locks[j].destinationMapName && Game1.schoolMaps.maps[mNode.name].Portals.ElementAt(k).Key.ItemNameToUnlock != "" && Game1.schoolMaps.maps[mNode.name].Portals.ElementAt(k).Key.ItemNameToUnlock != null)
                                {
                                    s.Draw(mapIconTextures[mNode.locks[j].type], new Vector2(-mapPositionX + mNode.locks[j].x, -mapPositionY + mNode.locks[j].y), Color.White);
                                }
                            }
                        }
                    }

                    for (int j = 0; j < mapIcons.Count; j++)
                    {
                        int padding = 0;
                        if (j - 1 >= 0 && (mapIcons[j - 1] == mapIconTextures["Story Quest Available"] || mapIcons[j - 1] == mapIconTextures["Story Quest Complete"] || mapIcons[j - 1] == mapIconTextures["Story Quest In-Progress"]))
                            padding = 7;
                        s.Draw(mapIcons[j], new Vector2(mNode.xPos - mapPositionX + mNode.iconPosX - 3 + (50 * j) + padding, mNode.yPos - mapPositionY + mNode.iconPosY - 13), Color.White);
                    }

                }
            }

            if (selectedMapGroupName == darylMapGroupName)
                s.Draw(mapIconTextures["Daryl's Position"], new Vector2(darylPos.X - mapPositionX, darylPos.Y - mapPositionY), Color.White);

            s.Draw(menuTextures["foreground"], new Vector2(0, 0), Color.White);

            #region Map Menu
            float menuAlpha = 1f;
            if ((dragging || afterDragTimer > 0) && menuState == MenuState.open)
                menuAlpha = .2f;

            s.Draw(menuTextures["menu"], new Vector2(menuPosX, 0), Color.White * menuAlpha);

            if (menuState == MenuState.open || menuState == MenuState.closing)
            {
                if (menuArrowButton.IsOver())
                    s.Draw(menuTextures["closeActive"], new Vector2(menuPosX, 0), Color.White * menuAlpha);
                else
                    s.Draw(menuTextures["closeStatic"], new Vector2(menuPosX, 0), Color.White * menuAlpha);
            }
            else
            {
                if (menuArrowButton.IsOver())
                    s.Draw(menuTextures["openActive"], new Vector2(menuPosX - 279, 0), Color.White * menuAlpha);
                else
                    s.Draw(menuTextures["openStatic"], new Vector2(menuPosX - 279, 0), Color.White * menuAlpha);
            }

            if (menuState == MenuState.open || darylMapGroupName == "Water Falls High School")
            {

                if (darylMapGroupName == "Water Falls High School")
                {
                    s.Draw(mapIconTextures["Daryl's Position"], new Vector2(menuPosX + 155, 88), Color.White * menuAlpha);
                }
                if (menuState == MenuState.open && (waterFallsHighSchoolButton.IsOver() || selectedMapGroupName == "Water Falls High School"))
                    s.Draw(activeMapButtonTextures["Water Falls High School"], new Vector2(menuPosX + 196, 83), Color.White * menuAlpha);
                else
                    s.Draw(staticMapButtonTextures["Water Falls High School"], new Vector2(menuPosX + 196, 83), Color.White * menuAlpha);
            }
            else
                s.Draw(staticMapButtonTextures["Water Falls High School"], new Vector2(menuPosX + 196, 83), Color.White * menuAlpha);

            for (int i = 0; i < currentMapButtons.Count; i++)
            {
                currentMapButtons[i].ButtonRecY = 165 + (52 * i);
                if (menuState == MenuState.open || currentMapButtonNames[i] == darylMapGroupName)
                {
                    if (currentMapButtonNames[i] == darylMapGroupName)
                    {
                        s.Draw(mapIconTextures["Daryl's Position"], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 841), 148 + (52 * i)), Color.White * menuAlpha);

                    }
                    if (Game1.schoolMaps.maps[mapGroups[currentMapButtonNames[i]].entranceMap].Discovered)
                    {
                        if (menuState == MenuState.open && (currentMapButtons[i].IsOver() || selectedMapGroupName == currentMapButtonNames[i]))
                            s.Draw(activeMapButtonTextures[currentMapButtonNames[i]], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 801), 148 + (52 * i)), Color.White * menuAlpha);
                        else
                            s.Draw(staticMapButtonTextures[currentMapButtonNames[i]], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 801), 148 + (52 * i)), Color.White * menuAlpha);
                    }
                    else
                        s.Draw(staticMapButtonTextures["Locked"], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 801), 148 + (52 * i)), Color.White * menuAlpha);
                }
                else
                {
                    if (Game1.schoolMaps.maps[mapGroups[currentMapButtonNames[i]].entranceMap].Discovered)
                        s.Draw(staticMapButtonTextures[currentMapButtonNames[i]], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 801), 148 + (52 * i)), Color.White * menuAlpha);
                    else
                        s.Draw(staticMapButtonTextures["Locked"], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 801), 148 + (52 * i)), Color.White * menuAlpha);
                }

                if (darylMapGroupName != null && !currentMapButtonNames.Contains(darylMapGroupName) && currentMapButtonNames[i] == mapGroups[darylMapGroupName].parentName)
                {
                    s.Draw(mapIconTextures["Daryl's Position"], new Vector2(menuPosX + (currentMapButtons[i].ButtonRecX - 24 - 841), 148 + (52 * i)), Color.White * menuAlpha);
                }
            }
            #endregion
        }

        public static IEnumerable<TResult> ReadJson<TResult>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                jsonReader.SupportMultipleContent = true;

                while (jsonReader.Read())
                {
                    yield return serializer.Deserialize<TResult>(jsonReader);
                }
            }
        }
    }
}
