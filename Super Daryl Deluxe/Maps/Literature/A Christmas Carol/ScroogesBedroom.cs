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
    class ScroogesBedroom : MapClass
    {
        static Portal toWesternCorridor;

        public static Portal ToWesternCorridor { get { return toWesternCorridor; } }

        Texture2D foreground, door, christmasPast;
        float doorAlpha;
        GhostLight light1;
        ScroogeFirePlace firePlace;
        public float christmasPastAlpha = 1;
        int spawnDelay, enemiesKilled;

        int ghostFrame;
        int ghostFrameDelay = 5;

        public ScroogesBedroom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Scrooge's Bedroom";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 2;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, 137, -10, true, false, true, true);
            interactiveObjects.Add(light1);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);

            firePlace = new ScroogeFirePlace(game, -200, 382);
            interactiveObjects.Add(firePlace);
            (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Add(firePlace);
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

                int spawnPosX;
                if (Game1.randomNumberGen.Next(2) == 0)
                    spawnPosX = -100;
                else
                    spawnPosX = 1200;

                switch (enemyType)
                {
                    case 0:
                        en = new HauntedNutcracker(pos, "Haunted Nutcracker", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;

                        en.Position = new Vector2(spawnPosX, monsterY);

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
                        en.Position = new Vector2(spawnPosX, monsterY);

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
                        en.Position = new Vector2(spawnPosX, monsterY);

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

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SpookyPresentEnemy(content);
            EnemyContentLoader.EerieElfEnemy(content);
            EnemyContentLoader.HauntedNutcrackerEnemy(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\ScroogeBedroom\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\ScroogeBedroom\foreground");
            firePlace.fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");
            door = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\ScroogeBedroom\door");
            game.NPCSprites["Ebenezer Scrooge"] = content.Load<Texture2D>(@"NPC\Literature\Scrooge");
            Game1.npcFaces["Ebenezer Scrooge"].faces["Scared"] = content.Load<Texture2D>(@"NPCFaces\Literature\Ebenezer Scrooge Scared");

            christmasPast = content.Load<Texture2D>(@"NPC\Literature\ChristmasPastSprite");
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

        public override void Update()
        {
            base.Update();

            if (enemiesKilled >= 5 && enemiesInMap.Count == 0 && !game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"])
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
                game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] = true;

            if (spawnEnemies && !game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"] && enemiesKilled < 5)
                RespawnGroundEnemies();

            if (!game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"] && toWesternCorridor.IsUseable)
            {
                Chapter.effectsManager.AddInGameDialogue("Oh! Make it stop! Go away, spirits!", "Ebenezer Scrooge", "Scared", 150);
                toWesternCorridor.IsUseable = false;
                Chapter.effectsManager.foregroundFButtonRecs.Clear();
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"] && !toWesternCorridor.IsUseable)
                toWesternCorridor.IsUseable = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWesternCorridor = new Portal(160, platforms[0], "Scrooge's Bedroom");
            toWesternCorridor.FButtonYOffset = -20;
            toWesternCorridor.PortalNameYOffset = -20;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


            if (!game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"])
            {
                ghostFrameDelay--;

                if (ghostFrameDelay <= 0)
                {
                    ghostFrame++;
                    ghostFrameDelay = 5;

                    if (ghostFrame > 7)
                        ghostFrame = 0;
                }

                s.Draw(christmasPast, new Vector2(663, 110), new Rectangle(516 * ghostFrame, 0, 516, 388), Color.White * christmasPastAlpha);

            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWesternCorridor, WesternCorridor.ToScroogesBedroom);
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

            if (player.VitalRec.X < 450)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            if(game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"])
                s.Draw(door, new Vector2(0, 337), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
