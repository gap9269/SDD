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
    class EasternCorridor : MapClass
    {
        public static Portal toTheFoyer;
        public static Portal toRedundantBedroom;
        public static Portal toBallroom;

        Texture2D foreground, statueOne, statueTwo;
        GhostLight light1, light2, light3, light4;

        BreakableObject statueOneObj, statueTwoObj, statueThreeObj;

        List<BreakableObject> statues;

        SpookyPresent keyGhost;

        public EasternCorridor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Eastern Corridor";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 6;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, 290, 30, false, false);
            interactiveObjects.Add(light1);

            light3 = new GhostLight(game, 2191, 30, false, false);
            interactiveObjects.Add(light3);

            light4 = new GhostLight(game, 2912, 20, false, false, false, false);
            interactiveObjects.Add(light4);

            statueOneObj = new BreakableObject(game, 742, 312, Game1.whiteFilter, true, 8, 5, 0, false);
            statueOneObj.Rec = new Rectangle(742, 312, 88, 302);
            statueOneObj.VitalRec = statueOneObj.Rec;
            interactiveObjects.Add(statueOneObj);

            statueTwoObj = new BreakableObject(game, 2646, 312, Game1.whiteFilter, true, 8, 3, 0, false);
            statueTwoObj.Rec = new Rectangle(2646, 312, 88, 302);
            statueTwoObj.VitalRec = statueTwoObj.Rec;
            interactiveObjects.Add(statueTwoObj);

            statueThreeObj = new BreakableObject(game, 3354, 312, Game1.whiteFilter, true, 8, 7, 0, false);
            statueThreeObj.Rec = new Rectangle(3354, 312, 88, 302);
            statueThreeObj.VitalRec = statueThreeObj.Rec;
            interactiveObjects.Add(statueThreeObj);

            statues = new List<BreakableObject>();
            statues.Add(statueOneObj);
            statues.Add(statueTwoObj);
            statues.Add(statueThreeObj);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EasternCorridor\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EasternCorridor\foreground");
            statueOne = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EasternCorridor\statueOne");
            statueTwo = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EasternCorridor\statueTwo");
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

            if (!enemiesInMap.Contains(keyGhost) && !game.ChapterTwo.ChapterTwoBooleans["keyGhostKilled"] && game.ChapterTwo.ChapterTwoBooleans["keyGhostDisappeared"])
            {
                keyGhost = new SpookyPresent(pos, "Spooky Present", game, ref player, this);
                monsterY = platforms[0].Rec.Y - keyGhost.Rec.Height - 10;
                keyGhost.Position = new Vector2(1400, monsterY);

                keyGhost.TimeBeforeSpawn = 0;
                keyGhost.hasKey = true;

                keyGhost.UpdateRectangles();
                enemyNamesAndNumberInMap["Spooky Present"]++;
                AddEnemyToEnemyList(keyGhost);

            }

            RespawnGroundEnemies();


            foreach(BreakableObject statue in statues)
            {
                if (statue.Health <= 0 && statue.Finished == false)
                {
                    Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(statue.Rec.Center.X - 175, statue.Rec.Center.Y - 175, 350, 350), 2);
                    statue.Finished = true;
                    game.Camera.ShakeCamera(5, 15);
                }
            }

            if (statueOneObj.Finished && !light1.active)
                light1.active = true;

            if (statueTwoObj.Finished && !light3.active)
                light3.active = true;

            if (statueThreeObj.Finished && !light4.active)
                light4.active = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheFoyer = new Portal(50, platforms[0], "Eastern Corridor");
            toRedundantBedroom = new Portal(3665, platforms[0], "Eastern Corridor");
            toBallroom = new Portal(1800, platforms[0], "Eastern Corridor");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!statueOneObj.Finished)
                s.Draw(statueOne, new Vector2(720, 283), Color.White);
            if (!statueThreeObj.Finished)
                s.Draw(statueOne, new Vector2(3330, 283), Color.White);
            if (!statueTwoObj.Finished)
                s.Draw(statueTwo, new Vector2(2622, 283), Color.White);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheFoyer, TheGrandCorridor.toEasternCorridor);
            portals.Add(toRedundantBedroom, HauntedBedroom.toEasternCorridor);
            portals.Add(toBallroom, TheHauntedBallroom.toEasternCorridor);
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
