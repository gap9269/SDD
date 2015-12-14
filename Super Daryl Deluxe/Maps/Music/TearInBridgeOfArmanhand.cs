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
    class BridgeOfArmanhandRift : MapClass
    {
        static Portal toBridge;

        public static Portal ToBridge { get { return toBridge; } }

        Texture2D sky, elevator;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;
        double timeToComplete = 0;
        int timer;
        MovingPlatform movingPlat2;
        List<Vector2> targets2;

        public BridgeOfArmanhandRift(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 3000;
            mapName = "Bridge of Armanhand - Rift";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 15;

            zoomLevel = .9f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            targets2 = new List<Vector2>();
            targets2.Add(new Vector2(1270, mapRec.Y + 419));
            targets2.Add(new Vector2(1270, mapRec.Y + 1075));

            movingPlat2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1270, mapRec.Y + 1075, 300, 50),
                true, false, false, targets2, 3, 100, Platform.PlatformType.rock);

            platforms.Add(movingPlat2);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Benny Beaker", 0);
            enemyNamesAndNumberInMap.Add("Captain Sax", 0);
            enemyNamesAndNumberInMap.Add("Maracas Hermanos", 0);
            enemyNamesAndNumberInMap.Add("Sergeant Cymbal", 0);
            enemyNamesAndNumberInMap.Add("Tuba Ghost", 0);
        }

        public override void RespawnGroundEnemies()
        {
            if (enemyNamesAndNumberInMap["Erl The Flask"] < 5)
            {

                monsterX = rand.Next(platforms[3].Rec.X, platforms[3].Rec.X + platforms[3].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                ErlTheFlask erl = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
                monsterY = platforms[3].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                Rectangle erlRec = new Rectangle(erl.RecX, monsterY, erl.Rec.Width, erl.Rec.Height);
                if (erlRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(erl);
                    enemyNamesAndNumberInMap["Erl The Flask"]++;
                }
            }
            if (enemyNamesAndNumberInMap["Benny Beaker"] < 3)
            {

                monsterX = rand.Next(platforms[2].Rec.X, platforms[2].Rec.X + platforms[2].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                BennyBeaker ben = new BennyBeaker(pos, "Benny Beaker", game, ref player, this);
                monsterY = platforms[2].Rec.Y - ben.Rec.Height - 1;
                ben.Position = new Vector2(monsterX, monsterY);

                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                if (benRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Benny Beaker"]++;
                }
            }
            if (enemyNamesAndNumberInMap["Tuba Ghost"] < 2)
            {
                if (enemyNamesAndNumberInMap["Tuba Ghost"] == 0)
                    platformNum = 4;
                else
                    platformNum = 0;

                monsterX = rand.Next(platforms[platformNum].Rec.X, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);
                TubaGhost en = new TubaGhost(pos, "Tuba Ghost", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 120;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Tuba Ghost"]++;
                    AddEnemyToEnemyList(en);
                }
            }

            if (enemyNamesAndNumberInMap["Maracas Hermanos"] < 2)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                MaracasHermanos en = new MaracasHermanos(pos, "Maracas Hermanos", game, ref player, this);
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
                    enemyNamesAndNumberInMap["Maracas Hermanos"]++;
                }
            }

            if (enemyNamesAndNumberInMap["Sergeant Cymbal"] < 2)
            {

                if (enemyNamesAndNumberInMap["Sergeant Cymbal"] == 0)
                    platformNum = 4;
                else
                    platformNum = 0;

                monsterX = rand.Next(platforms[platformNum].Rec.X, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                SergeantCymbal en = new SergeantCymbal(pos, "Sergeant Cymbal", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
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
                    enemyNamesAndNumberInMap["Sergeant Cymbal"]++;
                }
            }

            if (enemyNamesAndNumberInMap["Captain Sax"] < 1)
            {

                monsterX = rand.Next(platforms[0].Rec.X, platforms[0].Rec.X + platforms[0].Rec.Width);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                CaptainSax en = new CaptainSax(pos, "Captain Sax", game, ref player, this);
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
                    enemyNamesAndNumberInMap["Captain Sax"]++;
                }
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Bridge of Armanhand - Rift\background"));
            sky = content.Load<Texture2D>(@"Maps\Music\Bridge of Armanhand - Rift\sky");

            elevator = content.Load<Texture2D>(@"Maps\Science\101\elevator");

            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.CaptainSaxEnemy(content);
            EnemyContentLoader.MaracasHermanosEnemy(content);
            EnemyContentLoader.SergeantCymbalEnemy(content);
            EnemyContentLoader.ErlTheFlask(content);
            EnemyContentLoader.TubaGhostEnemy(content);
            EnemyContentLoader.BennyBeaker(content);
        }


        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            platforms.Remove(movingPlat2);
            targets2 = new List<Vector2>();
            targets2.Add(new Vector2(1270, mapRec.Y + 419));
            targets2.Add(new Vector2(1270, mapRec.Y + 1075));

            movingPlat2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1270, mapRec.Y + 1075, 300, 50),
                true, false, false, targets2, 3, 100, Platform.PlatformType.rock);

            platforms.Add(movingPlat2);

            ResetEnemyNamesAndNumberInMap();

            enemiesInMap.Clear();

            game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftStarted"] = false;

            if (game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"] == false)
                spawnEnemies = true;
            else
                spawnEnemies = false;
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"])
            {
                if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                    RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount && game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftStarted"] == false)
                {
                    spawnEnemies = false;
                    game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftStarted"] = true;
                    toBridge.IsUseable = false;
                    game.Camera.ShakeCamera(5, 15);
                    timeToComplete = 90.00;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.AddTimer(timeToComplete);
                }

                if (game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftStarted"] == true)
                {
                    if (timer < 60)
                        timer++;

                    if (timer >= 60)
                    {
                        timeToComplete -= 1;
                        timer = 0;
                    }

                    if (timeToComplete <= 0 && toBridge.IsUseable == false)
                    {
                        toBridge.IsUseable = true;
                        game.Camera.ShakeCamera(5, 15);
                        ForceToNewMap(toBridge, BridgeOfArmanhand.ToRift);
                    }

                    if (enemiesInMap.Count == 0 && timeToComplete > 0)
                    {
                        game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"] = true;
                        //collectibles[0].AbleToPickUp = true;
                        //collectibles[1].AbleToPickUp = true;
                        toBridge.IsUseable = true;
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

            toBridge.PortalRecY = 370;
            toBridge.PortalRecX = 865;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBridge = new Portal(1000, platforms[0], "Bridge of Armanhand - Rift");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBridge, BridgeOfArmanhand.ToRift);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(rip.ElementAt(ripFrame).Value, new Rectangle(344, mapRec.Y + 907, rip.ElementAt(ripFrame).Value.Width, rip.ElementAt(ripFrame).Value.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            s.Draw(elevator, new Vector2(movingPlat2.RecX - 65, movingPlat2.RecY - 100), Color.White);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            //s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(mapRec.Width - foreground2.Width, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransformWithVertParallax(.15f, 1.05f, this, game));
            s.Draw(sky, new Vector2(-500, mapRec.Y - 40), Color.White);
            s.End();
        }
    }
}
