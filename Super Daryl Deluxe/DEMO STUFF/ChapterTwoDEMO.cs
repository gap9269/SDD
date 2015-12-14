using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class ChapterTwoDEMO : Chapter
    {

        //History
        NPC napoleon, cleopatra, genghis;
        Julius julius;
        BridgeKid outsideCampBob;

        //--Story quests
        public FortRaidDemo fortRaid;

        //--Cutscenes

        //History
        CampGateOpenSceneDemo campGateOpenScene;
        TrollAppearDemo trollAppearScene;
        BombExplode bombExplodeScene;
        HorseDestroyed horseDestroyedScene;
        Ch2DemoOpening fortDemoOpening;
        DemoEnd demoEnd;

        //Switch state to decision making, then pick the decision
        public enum Decisions
        {
            none,
            test,
            tutorialResolution
        }
        public Decisions decisions;

        //--Story Quest attributes
        Dictionary<String, Boolean> chapterTwoBooleans;

        static Random ran = new Random();
        public Dictionary<String, Boolean> ChapterTwoBooleans { get { return chapterTwoBooleans; } set { chapterTwoBooleans = value; } }

        public ChapterTwoDEMO(Game1 g, ref Player p,
            HUD nHud, Cursor cur, Camera cam, Dictionary<String, Texture2D> textures)
            : base(g, ref p, nHud, cur, cam, textures)
        {

            fortRaid = new FortRaidDemo(true);

            game.AllQuests.Add(fortRaid.QuestName, fortRaid);

            //Booleans
            chapterTwoBooleans = new Dictionary<String, bool>();

            //History room
            chapterTwoBooleans.Add("campDoorFallen", false);
            chapterTwoBooleans.Add("clearedCentralFortFirstTime", false);
            chapterTwoBooleans.Add("justEnteredFort", false);
            chapterTwoBooleans.Add("enemyReinforcementsSpawning", false);
            chapterTwoBooleans.Add("clearedEastHuts", false);
            chapterTwoBooleans.Add("clearedWestHuts", false);
            chapterTwoBooleans.Add("spawnedOutsideCampGuards", false);
            chapterTwoBooleans.Add("eastCommanderSpawned", false);
            chapterTwoBooleans.Add("westCommanderSpawned", false);
            chapterTwoBooleans.Add("westCommanderKilled", false);
            chapterTwoBooleans.Add("eastCommanderKilled", false);
            chapterTwoBooleans.Add("centralCommanderSpawned", false);
            chapterTwoBooleans.Add("centralCommanderKilled", false);
            chapterTwoBooleans.Add("horseInCentral", true);
            chapterTwoBooleans.Add("horseInWest", false);
            chapterTwoBooleans.Add("horseInEast", false);
            chapterTwoBooleans.Add("trollSpawnedInWest", false);
            chapterTwoBooleans.Add("westTrollKilled", false);
            chapterTwoBooleans.Add("bombExploded", false);
            chapterTwoBooleans.Add("demoEndPlayed", false);

            AddNPCs();

            //Cutscenes

            fortDemoOpening = new Ch2DemoOpening(game, camera, player);
            chapterScenes.Add(fortDemoOpening);

            campGateOpenScene = new CampGateOpenSceneDemo(game, camera, player);
            chapterScenes.Add(campGateOpenScene);

            trollAppearScene = new TrollAppearDemo(game, camera, player);
            chapterScenes.Add(trollAppearScene);

            bombExplodeScene = new BombExplode(game, camera, player);
            chapterScenes.Add(bombExplodeScene);

            demoEnd = new DemoEnd(game, camera, player);
            chapterScenes.Add(demoEnd);

            horseDestroyedScene = new HorseDestroyed(game, camera, player);
            //Add it when the horse is actually destroyed

            //Change to cutscene to play scene
            state = GameState.Game;
            cutsceneState = 0;
            synopsis = "";
        }

        public override void Update()
        {
            if (player.playerState == Player.PlayerState.dead)
            {
                base.Update();
                player.Update();
            }

#if DEBUG
            if(current.IsKeyUp(Keys.P) && last.IsKeyDown(Keys.P))
            {
                player.EquippedSkills[0].Experience = player.EquippedSkills[0].ExperienceUntilLevel;
           //     player.EquippedSkills[1].Experience = player.EquippedSkills[1].ExperienceUntilLevel;
            //    player.EquippedSkills[2].Experience = player.EquippedSkills[2].ExperienceUntilLevel;
             //   player.EquippedSkills[3].Experience = player.EquippedSkills[3].ExperienceUntilLevel;
            }
#endif

            if (!player.LevelingUp && player.playerState != Player.PlayerState.dead)
            {
                base.Update();
                cursor.Update();
                AddNPCs();
                game.SideQuestManager.AddNPCs();

                switch (state)
                {
                    case GameState.Game:
                        UpdateNPCs();

                        if (TalkingToNPC == false && makingDecision == false)
                        {
                            player.Update();
                            hud.Update();
                            currentMap.Update();

                            camera.Update(player, game, currentMap);
                            player.Enemies = currentMap.EnemiesInMap;

                            String checkPortal = currentMap.CheckPortals();

                            //--Change states to start the fade out
                            if (checkPortal != "null")
                            {
                                nextMap = checkPortal;
                                state = GameState.ChangingMaps;
                            }
                        }
                        break;
                    case GameState.ChangingMaps:

                        //DEMO END
                        if (nextMap == "Axis of Historical Reality" && chapterTwoBooleans["demoEndPlayed"] == false)
                        {
                            state = GameState.Cutscene;
                            chapterTwoBooleans["demoEndPlayed"] = true;
                        }

                        break;
                }
            }

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case GameState.Cutscene:

                    if (game.SideQuestManager.sideQuestScenes == SideQuestManager.SideQuestScenes.none)
                    {
                        chapterScenes[cutsceneState].Draw(s);
                        if (chapterScenes[cutsceneState].skippingCutscene)
                            chapterScenes[cutsceneState].DrawSkipCutscene(s);
                    }
                    else
                        game.SideQuestManager.DrawSideQuestScene(s);
                    break;
                case GameState.Game:
                    break;
            }
        }

        public override void AddNPCs()
        {
            base.AddNPCs();


            if (!nPCs.ContainsKey("BobTheConstructionGuyTwo"))
            {
                //--Skill Instructor next to Daryl's Locker
                List<String> dialogue = new List<string>();

                dialogue.Add("Whoa there, pal! Watch where you step, alright?");
                dialogue.Add("You're free to drown in this moat all you want, but don't expect anything realistic. There have been a lot of budget cuts lately, we can't afford a splash.");
                outsideCampBob = new BridgeKid(game.NPCSprites["Bob the Construction Guy"], dialogue,
                    new Rectangle(3150, 385, 516, 388), player, game.Font, game, "Stone Fort Gate", "Bob the Construction Guy", false);
                nPCs.Add("BobTheConstructionGuyTwo", outsideCampBob);
            }

            if (!nPCs.ContainsKey("Julius"))
            {
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Have a gander at that one over there! I sure would like to conquer -her- lands. I do think I'll go ask for her numerals.");
                julius = new Julius(game.NPCSprites["Julius Caesar"], dialogue2, new Rectangle(2350, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Julius Caesar", false);
                julius.FacingRight = false;
                julius.BattleReady = true;
                nPCs.Add("Julius", julius);
            }

            if (!nPCs.ContainsKey("Napoleon"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("This dialogue does not matter because I have a quest");
                napoleon = new NPC(game.NPCSprites["Napoleon"], dialogue2, fortRaid, new Rectangle(1840, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Napoleon", false);
                napoleon.FacingRight = true;
                nPCs.Add("Napoleon", napoleon);
            }

            if (!nPCs.ContainsKey("Cleopatra"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("I am starting to believe that my soldiers may be underdressed for the occasion.");
                cleopatra = new NPC(game.NPCSprites["Cleopatra"], dialogue2, new Rectangle(1690, 370, 516, 388), player, game.Font, game, "Stone Fort Gate", "Cleopatra", false);
                cleopatra.FacingRight = true;
                nPCs.Add("Cleopatra", cleopatra);
            }

            if (!nPCs.ContainsKey("Genghis"))
            {
                //--Friend One
                List<String> dialogue2 = new List<string>();
                dialogue2.Add("Kublai! Where have you been?");
                genghis = new NPC(game.NPCSprites["Genghis"], dialogue2, new Rectangle(1470, 350, 516, 388), player, game.Font, game, "Stone Fort Gate", "Genghis", false);
                genghis.FacingRight = true;
                nPCs.Add("Genghis", genghis);
            }
        }

        public void PlayHorseExplosion()
        {
            chapterScenes.Add(horseDestroyedScene);
            cutsceneState = chapterScenes.Count - 1;
            state = GameState.Cutscene;
        }

        public void SetPlayerStatsForDemo()
        {
            Game1.Player.LevelUpToLevel(14);
            Game1.Player.PositionX = 1350;
            Game1.Player.PositionY = 290;

            Game1.Player.Textbooks = 16;

            Game1.Player.OwnedAccessories.Add(new ArtistsPaletteDemo());
            Game1.Player.OwnedAccessories.Add(new EighthOfButterDemo());
            Game1.Player.OwnedAccessories.Add(new SuperWeeniePurseDemo());
            Game1.Player.OwnedAccessories.Add(new VenusOfWillendorfDemo());
            Game1.Player.OwnedAccessories.Add(new YinYangNecklaceDemo());

            Game1.Player.OwnedHoodies.Add(new TogaDemo());
            Game1.Player.OwnedHoodies.Add(new LabCoatDemo());
            Game1.Player.OwnedHoodies.Add(new BandUniformDemo());
            Game1.Player.OwnedHoodies.Add(new CapeOfCountRogerDemo());
            Game1.Player.OwnedHoodies.Add(new ArtSmockDemo());
            Game1.Player.OwnedHoodies.Add(new SuperWeenieShirtDemo());

            Game1.Player.OwnedHats.Add(new PartyHatDemo());
            Game1.Player.OwnedHats.Add(new FezDemo());
            Game1.Player.OwnedHats.Add(new BandHatDemo());
            Game1.Player.OwnedHats.Add(new BeretDemo());
            Game1.Player.OwnedHats.Add(new SuperWeenieHatDemo());

            Game1.Player.OwnedWeapons.Add(new HandSawDemo());
            Game1.Player.OwnedWeapons.Add(new HandSawDemo());
            Game1.Player.OwnedWeapons.Add(new MarkerDemo());
            Game1.Player.OwnedWeapons.Add(new MarkerDemo());
            Game1.Player.OwnedWeapons.Add(new PaintbrushDemo());
            Game1.Player.OwnedWeapons.Add(new PaintbrushDemo());
            Game1.Player.OwnedWeapons.Add(new SuperWeenieSwordDemo());
            Game1.Player.OwnedWeapons.Add(new CoalShovelDemo());

            Game1.Player.UnlockAllCharacterBios();
            Game1.Player.UnlockEnemyBio("Crow");
            Game1.Player.UnlockEnemyBio("Scarecrow");
            Game1.Player.UnlockEnemyBio("Benny Beaker");
            Game1.Player.UnlockEnemyBio("Erl The Flask");
            Game1.Player.UnlockEnemyBio("Fez");
            Chapter.effectsManager.secondNotificationQueue.Clear();

            Game1.g.SkillManager.AddDemoSkills();

            Game1.Player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
            Game1.Player.EquippedSkills.Add(Game1.Player.LearnedSkills[0]);
            Game1.Player.LearnedSkills[0].Equipped = true;
            Game1.Player.LearnedSkills[0].SkillRank = 3;
            Game1.Player.LearnedSkills[0].ApplyLevelUp(true);
            Game1.Player.LearnedSkills[0].LoadContent();

            Game1.Player.CanJump = true;


            game.ChapterOne.ChapterOneBooleans["quickRetortObtained"] = true;
            Game1.Player.quickRetort.SkillRank = 5;
            Game1.Player.quickRetort.ApplyLevelUp(true);
        }
    }
}
