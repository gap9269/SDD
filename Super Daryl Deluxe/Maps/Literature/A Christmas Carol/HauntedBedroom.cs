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
    class HauntedBedroom : MapClass
    {
        public static Portal toEasternCorridor;

        Texture2D foreground;
        GhostLight light1, light2;

        ScroogeFirePlace firePlace;
        int spawnDelay, enemiesKilled;
        public HauntedBedroom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Haunted Bedroom";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 4;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, -90, 0, false, false, true, true);
            interactiveObjects.Add(light1);

            light2 = new GhostLight(game, 875, 0, false, false, true, true);
            interactiveObjects.Add(light2);

            firePlace = new ScroogeFirePlace(game, 271, 420);
            interactiveObjects.Add(firePlace);
            (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Add(firePlace);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\RedundantBedroom\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\RedundantBedroom\foreground");

            firePlace.fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");
            game.NPCSprites["Ebenezer Scrooge"] = content.Load<Texture2D>(@"NPC\Literature\Scrooge");
            Game1.npcFaces["Ebenezer Scrooge"].faces["Scared"] = content.Load<Texture2D>(@"NPCFaces\Literature\Ebenezer Scrooge Scared");
            Game1.npcFaces["Ebenezer Scrooge"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Ebenezer Scrooge Normal");

            game.NPCSprites["Jacob Marley"] = content.Load<Texture2D>(@"NPC\Literature\Jacob Marley");
            Game1.npcFaces["Jacob Marley"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Jacob Marley Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Ebenezer Scrooge"] = Game1.whiteFilter;
            Game1.npcFaces["Ebenezer Scrooge"].faces["Scared"] = Game1.whiteFilter;

            game.NPCSprites["Jacob Marley"] = Game1.whiteFilter;
            Game1.npcFaces["Jacob Marley"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.LitGuardian(content);
            EnemyContentLoader.SpookyPresentEnemy(content);
            EnemyContentLoader.EerieElfEnemy(content);
            EnemyContentLoader.HauntedNutcrackerEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (spawnDelay > 0)
                spawnDelay--;

            if (enemiesInMap.Count < enemyAmount && spawnDelay <= 0)
            {
                int enemyType = Game1.randomNumberGen.Next(3);

                Enemy en;

                switch (enemyType)
                {
                    case 0:
                        en = new HauntedNutcracker(pos, "Haunted Nutcracker", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;

                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;
                        en.Hostile = true;
                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            enemiesKilled++;
                            spawnDelay = 30;
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Haunted Nutcracker"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 1:
                        en = new SpookyPresent(pos, "Spooky Present", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.Hostile = true;
                        en.TimeBeforeSpawn = 30;

                        Rectangle testRec2 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec2.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            enemiesKilled++;
                            spawnDelay = 60;
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Spooky Present"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 2:
                        en = new EerieElf(pos, "Eerie Elf", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.Hostile = true;
                        en.TimeBeforeSpawn = 30;

                        Rectangle testRec3 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec3.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            enemiesKilled++;
                            spawnDelay = 60;
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Eerie Elf"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (player.PositionX > 700 && enemiesKilled >= 12  && enemiesInMap.Count == 0 && !game.ChapterTwo.ChapterTwoBooleans["bedroomThreeCleared"])
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["summoningDeathDialoguePlayed"] && game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"])
            {
                Chapter.effectsManager.AddInGameDialogue("This one always take a while, BUT SIT TIGHT, GREEDY MAN. SOON YOU WILL MEET YOUR DEATH.", "Jacob Marley", "Normal", 300);
                game.ChapterTwo.ChapterTwoBooleans["summoningDeathDialoguePlayed"] = true;
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
                game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] = true;

            if (spawnEnemies && !game.ChapterTwo.ChapterTwoBooleans["bedroomThreeCleared"] && enemiesKilled < 12)
                RespawnGroundEnemies();

            if (game.ChapterTwo.ChapterTwoBooleans["batteryPlaced"] && !light1.active)
            {
                light1.active = true;
                light2.active = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["bedroomThreeCleared"])
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["literatureGuardianDefeated"] && toEasternCorridor.IsUseable)
                    toEasternCorridor.IsUseable = false;
                else if (game.ChapterTwo.ChapterTwoBooleans["literatureGuardianDefeated"] && !toEasternCorridor.IsUseable)
                    toEasternCorridor.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEasternCorridor = new Portal(50, platforms[0], "Haunted Bedroom");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEasternCorridor, EasternCorridor.toRedundantBedroom);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }

                if (interactiveObjects[i] is GhostLight)
                {
                    (interactiveObjects[i] as GhostLight).DrawGlow(s);
                }
            }

                s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
