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
    class CentralSandsRift : MapClass
    {
        static Portal toCentralSands;

        public static Portal ToCentralSands { get { return toCentralSands; } }

        Texture2D sky, foreground, parallax, crater, cannonballSprite;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;
        double timeToComplete = 0;
        int timer;

        Cannonball[] cannonballs;
        int[] cannonballTimers;

        int cannonballAmount = 3;

        public CentralSandsRift(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 2000;
            mapName = "Central Sands - Rift";
            mapHeight = 1780;
            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 12;

            zoomLevel = .9f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            cannonballs = new Cannonball[cannonballAmount];
            cannonballTimers = new int[cannonballAmount];

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Nurse Goblin", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin Mortar", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);
        }

        public override void RespawnGroundEnemies()
        {
            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 3)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                SexySaguaro erl = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
                monsterY = platforms[0].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                AddEnemyToEnemyList(erl);
                enemyNamesAndNumberInMap["Sexy Saguaro"]++;
                
            }
            if (enemyNamesAndNumberInMap["Goblin"] < 1)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
                monsterY = platforms[0].Rec.Y - ben.Rec.Height - 1;
                ben.Position = new Vector2(monsterX, monsterY);

                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                if (benRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
            if (enemyNamesAndNumberInMap["Nurse Goblin"] < 1)
            {
                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);
                NurseGoblin en = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                monsterY = platforms[0].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 120;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Nurse Goblin"]++;
                    AddEnemyToEnemyList(en);
                }
            }

            if (enemyNamesAndNumberInMap["Bomblin"] < 2)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                Bomblin en = new Bomblin(pos, "Bomblin", game, ref player, this);
                monsterY = platforms[0].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.UpdateRectangles();

                Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);

                if (testRec.Intersects(player.VitalRec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 3)
            {
                BurnieBuzzard en = new BurnieBuzzard(pos, "Burnie Buzzard", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Burnie Buzzard"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralSandsRift\background"));
            background.Add(Game1.whiteFilter);
            sky = content.Load<Texture2D>(@"Maps\History\CentralSandsRift\sky");
            foreground = content.Load<Texture2D>(@"Maps\History\CentralSandsRift\foreground");
            parallax = content.Load<Texture2D>(@"Maps\History\CentralSandsRift\parallax");
            crater = content.Load<Texture2D>(@"Maps\History\Battlefield\crater");
            cannonballSprite = content.Load<Texture2D>(@"Maps\History\Battlefield\CannonballSheet");

            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.NurseGoblinEnemy(content);
            EnemyContentLoader.GoblinMortarEnemy(content);
            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
        }


        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            ResetEnemyNamesAndNumberInMap();

            enemiesInMap.Clear();

            game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftStarted"] = false;

            if (game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"] == false)
                spawnEnemies = true;
            else
                spawnEnemies = false;
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"])
            {
                if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                {
                    if (enemyNamesAndNumberInMap["Goblin Mortar"] < 2)
                    {
                        GoblinMortar mortar4 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                        mortar4.Position = new Vector2(1685, mapRec.Y - 106);
                        mortar4.FacingRight = false;

                        mortar4.TimeBeforeSpawn = 0;
                        mortar4.SpawnWithPoof = false;

                        AddEnemyToEnemyList(mortar4);

                        GoblinMortar mortar1 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                        mortar1.Position = new Vector2(-29, mapRec.Y - 182);
                        mortar1.FacingRight = false;

                        mortar1.TimeBeforeSpawn = 0;
                        mortar1.SpawnWithPoof = false;

                        AddEnemyToEnemyList(mortar1);

                        enemyNamesAndNumberInMap["Goblin Mortar"] = 2;
                    }

                    RespawnGroundEnemies();
                    RespawnFlyingEnemies(new Rectangle(150, mapRec.Y + 344, 1684, 962));
                }

                if (enemiesInMap.Count >= enemyAmount && game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftStarted"] == false)
                {
                    spawnEnemies = false;
                    game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftStarted"] = true;
                    toCentralSands.IsUseable = false;
                    game.Camera.ShakeCamera(5, 15);
                    timeToComplete = 120.00;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.AddTimer(timeToComplete);
                }

                if (game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftStarted"] == true)
                {
                    if (timer < 60)
                        timer++;

                    if (timer >= 60)
                    {
                        timeToComplete -= 1;
                        timer = 0;
                    }

                    for (int i = 0; i < cannonballAmount; i++)
                    {
                        if (cannonballs[i] != null)
                        {
                            cannonballs[i].Update();

                            if (cannonballs[i].finished)
                            {
                                cannonballs[i] = null;
                                cannonballTimers[i] = Game1.randomNumberGen.Next(180, 360);
                                i--;
                            }
                        }
                    }

                    if (enemiesInMap.OfType<GoblinMortar>().Any())
                    {
                        for (int i = 0; i < cannonballAmount; i++)
                        {
                            cannonballTimers[i]--;
                            if (cannonballTimers[i] <= 0)
                            {
                                switch (i)
                                {
                                    case 0:
                                        cannonballs[i] = (new Cannonball(247, mapRec.Y + 799, cannonballSprite, 50, true));
                                        cannonballTimers[i] = int.MaxValue;
                                        break;
                                    case 1:
                                        cannonballs[i] = (new Cannonball(984, mapRec.Y + 799, cannonballSprite, 50, true));
                                        cannonballTimers[i] = int.MaxValue;
                                        break;
                                    case 2:
                                        cannonballs[i] = (new Cannonball(1254, mapRec.Y + 792, cannonballSprite, 50, true));
                                        cannonballTimers[i] = int.MaxValue;
                                        break;
                                }
                            }
                        }
                    }

                    if (timeToComplete <= 0 && toCentralSands.IsUseable == false)
                    {
                        toCentralSands.IsUseable = true;
                        game.Camera.ShakeCamera(5, 15);
                        ForceToNewMap(toCentralSands, CentralSands.toRift);
                    }

                    if (enemiesInMap.Count == 0 && timeToComplete > 0)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"] = true;
                        toCentralSands.IsUseable = true;
                        game.Camera.ShakeCamera(5, 15);
                        timeToComplete = 0;
                        Chapter.effectsManager.AddTimer(0);
                    }
                }
            }

            ripDelay--;

            if (ripDelay <= 0)
            {
                ripFrame++;
                ripDelay = 1;

                if (ripFrame > 31)
                    ripFrame = 0;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralSands = new Portal(250, platforms[0], "Central Sands - Rift");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralSands, CentralSands.toRift);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(rip.ElementAt(ripFrame).Value, new Rectangle(-60, mapRec.Y + 896, rip.ElementAt(ripFrame).Value.Width, rip.ElementAt(ripFrame).Value.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);



            s.Draw(crater, new Vector2(247, mapRec.Y + 789), Color.White);
            s.Draw(crater, new Vector2(984, mapRec.Y + 789), Color.White);
            s.Draw(crater, new Vector2(1254, mapRec.Y + 782), Color.White);

            for (int i = 0; i < cannonballAmount; i++)
            {
                if (cannonballs[i] != null)
                    cannonballs[i].Draw(s);
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y + 70), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.15f, 1.05f, this, game));
            s.Draw(sky, new Vector2(-200, mapRec.Y - 80), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y + 950), Color.White);

            s.End();
        }
    }
}
