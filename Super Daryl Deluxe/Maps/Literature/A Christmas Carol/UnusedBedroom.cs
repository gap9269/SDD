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
    class UnusedBedroom : MapClass
    {
        public static Portal toCentralCorridor;

        int spawnDelay, enemiesKilled;

        Texture2D foreground, door, bulb, christmasPresent;
        float doorAlpha;
        GhostLight light1, light2;
        ScroogeFirePlace firePlace;
        int ghostFrame;
        int ghostFrameDelay = 5;
        public float christmasPresentAlpha = 1;
        public UnusedBedroom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Unused Bedroom";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, -548, -50, true, true, false, true);
            interactiveObjects.Add(light1);

            light2 = new GhostLight(game, 400, 58, true, true, true, false);
            interactiveObjects.Add(light2);

            firePlace = new ScroogeFirePlace(game, 37, 415);
            interactiveObjects.Add(firePlace);
            (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Add(firePlace);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SpookyPresentEnemy(content);
            EnemyContentLoader.EerieElfEnemy(content);
            EnemyContentLoader.HauntedNutcrackerEnemy(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnusedBedroom\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnusedBedroom\foreground");
            door = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnusedBedroom\door");
            firePlace.fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");
            bulb = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnusedBedroom\bulb");
            game.NPCSprites["Ebenezer Scrooge"] = content.Load<Texture2D>(@"NPC\Literature\Scrooge");
            Game1.npcFaces["Ebenezer Scrooge"].faces["Scared"] = content.Load<Texture2D>(@"NPCFaces\Literature\Ebenezer Scrooge Scared");
            christmasPresent = content.Load<Texture2D>(@"NPC\Literature\ChristmasPresent");

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

                if (enemiesKilled >= 7 && enemiesInMap.Count == 0 && !game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"])
                {
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                }

                if (!game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
                    game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] = true;

                if (spawnEnemies && !game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"] && enemiesKilled < 7)
                    RespawnGroundEnemies();

                if (!game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"] && toCentralCorridor.IsUseable)
                {
                    toCentralCorridor.IsUseable = false;
                    Chapter.effectsManager.AddInGameDialogue("No! Not more ghosts!", "Ebenezer Scrooge", "Scared", 150);
                }
                else if (game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"] && !toCentralCorridor.IsUseable)
                    toCentralCorridor.IsUseable = true;
            
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralCorridor = new Portal(1130, platforms[0], "Unused Bedroom");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(bulb, new Vector2(0, 140), Color.White);

            if (!game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"] && game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"])
            {
                ghostFrameDelay--;

                if (ghostFrameDelay <= 0)
                {
                    ghostFrame++;
                    ghostFrameDelay = 5;

                    if (ghostFrame > 7)
                        ghostFrame = 0;
                }

                s.Draw(christmasPresent, new Rectangle(246, 182, 516, 388), new Rectangle(516 * ghostFrame, 0, 516, 388), Color.White * christmasPresentAlpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralCorridor, CentralCorridor.toUnusedBedroom);
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

            if (player.VitalRecX > 1100)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"])
                s.Draw(door, new Vector2(845, 361), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
