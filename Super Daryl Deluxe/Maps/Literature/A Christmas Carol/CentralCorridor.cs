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
    class CentralCorridor : MapClass
    {
        public static Portal toLivingArea;
        public static Portal toUnusedBedroom;
        public static Portal toWalkInSafe;

        Texture2D foreground, painting;
        SpookyPresent keyGhost;
        GhostLight light1, light2, light3, light4;
        BreakableObject paintingObj;
        public CentralCorridor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Central Corridor";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);

            light1 = new GhostLight(game, 103, 22, true, true);
            interactiveObjects.Add(light1);

            light2 = new GhostLight(game, 1427, 22, true, true);
            interactiveObjects.Add(light2);

            light3 = new GhostLight(game, 2120, 22, true, true);
            interactiveObjects.Add(light3);

            light4 = new GhostLight(game, 2797, 22, true, true);
            interactiveObjects.Add(light4);

            paintingObj = new BreakableObject(game, 1172, 247, Game1.whiteFilter, true, 10, 0, 0, false);
            paintingObj.Rec = new Rectangle(1172, 247, 88, 302);
            paintingObj.VitalRec = paintingObj.Rec;
            interactiveObjects.Add(paintingObj);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\Central Corridor\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\Central Corridor\foreground");
            painting = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\Central Corridor\painting");
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemiesInMap.Count < enemyAmount)
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

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Haunted Nutcracker"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 1:
                        en = new SpookyPresent(pos, "Spooky Present", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;

                        Rectangle testRec2 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec2.Intersects(player.Rec))
                        {
                        }

                        else
                        {
                            en.UpdateRectangles();
                            enemyNamesAndNumberInMap["Spooky Present"]++;
                            AddEnemyToEnemyList(en);
                        }
                        break;
                    case 2:
                        en = new EerieElf(pos, "Eerie Elf", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                        en.Position = new Vector2(monsterX, monsterY);

                        en.TimeBeforeSpawn = 60;

                        Rectangle testRec3 = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec3.Intersects(player.Rec))
                        {
                        }

                        else
                        {
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

        public override void Update()
        {
            base.Update();

            if(game.ChapterTwo.ChapterTwoBooleans["keyGhostDisappeared"])
                RespawnGroundEnemies();

            if (paintingObj.Health <= 0 && paintingObj.Finished == false)
            {
                Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(paintingObj.Rec.Center.X - 175, paintingObj.Rec.Center.Y - 175, 350, 350), 2);
                paintingObj.Finished = true;
                game.Camera.ShakeCamera(5, 15);
            }

            if (paintingObj.Finished && toWalkInSafe.IsUseable == false)
            {
                toWalkInSafe.IsUseable = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["keyGhostDisappeared"] == false && !enemiesInMap.Contains(keyGhost) && game.ChapterTwo.ChapterTwoBooleans["bedroomOneCleared"])
            {
                keyGhost = new SpookyPresent(new Vector2(-80, 434), "Spooky Present", game, ref player, this);
                keyGhost.hasKey = true;
                AddEnemyToEnemyList(keyGhost);
            }

            if (enemiesInMap.Contains(keyGhost))
            {
                keyGhost.enemyState = Enemy.EnemyState.standing;
                keyGhost.PositionX = -80;
                keyGhost.moveTimer = 100;
                keyGhost.CanBeHit = false;

                if (player.VitalRecX < 560)
                {
                    keyGhost.Alpha -= .01f;

                    if (keyGhost.Alpha <= 0)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["keyGhostDisappeared"] = true;
                        enemiesInMap.Remove(keyGhost);
                    }
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toLivingArea = new Portal(3800, platforms[0], "Central Corridor");
            toUnusedBedroom = new Portal(150, platforms[0], "Central Corridor", "Gold Key");
            toWalkInSafe = new Portal(1180, platforms[0], "Central Corridor", "Silver Key");

            toWalkInSafe.IsUseable = false;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


            if(!paintingObj.Finished)
                s.Draw(painting, new Vector2(1134, 219), Color.White);

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toLivingArea, LivingArea.ToCentralCorridor);
            portals.Add(toUnusedBedroom, UnusedBedroom.toCentralCorridor);
            portals.Add(toWalkInSafe, WalkInSafe.toCentralCorridor);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
                s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

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

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
