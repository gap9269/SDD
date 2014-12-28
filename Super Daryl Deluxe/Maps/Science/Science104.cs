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
    class Science104 : MapClass
    {
        static Portal toScience103;
        static Portal toScience105;
        static Portal toScience101;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToScience103 { get { return toScience103; } }
        public static Portal ToScience105 { get { return toScience105; } }
        public static Portal ToScience101 { get { return toScience101; } }

        Texture2D foreground, galaxy, moon, sputnik, portalSheet, goggles, outhouse;
        Dictionary<String, Texture2D> triceratopsTextures;

        Rectangle gogglesRec;
        float v1, y1, v2, y2;
        Boolean up, up2;
        Sparkles sparkles;

        int triFrame, timeUntilNextBlink, portalFrame;
        int triDelay = 5;
        int portalDelay = 5;
        static Random randomNum;

        int sputnikY, sputnikX, floatCycle;

        public Science104(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 6200;
            mapName = "Science 104";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            MapFire fire = new MapFire(120, 200, 2900, mapRec.Y, game, 8);
            mapHazards.Add(fire);

            MapFire fire2 = new MapFire(120, 200, 3900, mapRec.Y, game, 8);
            mapHazards.Add(fire2);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Benny Beaker", 0);

            randomNum = new Random();

            timeUntilNextBlink = randomNum.Next(240, 500);

            gogglesRec = new Rectangle(5488, mapRec.Y + 728, 44, 41);
            sparkles = new Sparkles(5488, gogglesRec.Y);

            Barrel bar = new Barrel(game, 1000, 415 + 155, Game1.interactiveObjects["Barrel"], true, 1, 3, .04f, true, Barrel.BarrelType.ScienceFlask);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 1370, -275 + 155, Game1.interactiveObjects["Barrel"], true, 1, 4, .07f, false, Barrel.BarrelType.ScienceTube);
            interactiveObjects.Add(bar2);

            Barrel bar3 = new Barrel(game, 3750, 168 + 155, Game1.interactiveObjects["Barrel"], true, 1, 3, .03f, false, Barrel.BarrelType.ScienceBarrel);
            interactiveObjects.Add(bar3);

            Barrel bar4 = new Barrel(game, 3100, 168 + 155, Game1.interactiveObjects["Barrel"], true, 1, 4, .07f, false, Barrel.BarrelType.ScienceBarrel);
            interactiveObjects.Add(bar4);

            Barrel bar5 = new Barrel(game, 5330, -50 + 155, Game1.interactiveObjects["Barrel"], true, 1, 3, .03f, true, Barrel.BarrelType.ScienceFlask);
            interactiveObjects.Add(bar5);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.prologue:
                    if (game.MapBooleans.prologueMapBooleans["spawnedBennys"] == false)
                    {
                        BennyBeaker en = new BennyBeaker(pos, "Benny Beaker", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
                        en.Position = new Vector2(monsterX, monsterY);

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(en);
                        }
                    }
                    break;

                default:
                    if (enemyNamesAndNumberInMap["Erl The Flask"] < 4)
                    {
                        ErlTheFlask erl = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - erl.Rec.Height - 1;
                        erl.Position = new Vector2(monsterX, monsterY);

                        Rectangle erlRec = new Rectangle(erl.RecX, monsterY, erl.Rec.Width, erl.Rec.Height);
                        if (erlRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(erl);
                            enemyNamesAndNumberInMap["Erl The Flask"]++;
                        }
                    }
                    if (enemyNamesAndNumberInMap["Benny Beaker"] < 4)
                    {
                        BennyBeaker ben = new BennyBeaker(pos, "Benny Beaker", game, ref player, this);
                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Benny Beaker"]++;
                        }
                    }
                    break;
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps/Science/104/background1"));
            background.Add(content.Load<Texture2D>(@"Maps/Science/104/background2"));

            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");
            goggles = content.Load<Texture2D>(@"Maps/Science/101/float2");
            foreground = content.Load<Texture2D>(@"Maps/Science/104/foreground");
            galaxy = content.Load<Texture2D>(@"Maps/Science/104/galaxy");
            moon = content.Load<Texture2D>(@"Maps/Science/104/moon");
            sputnik = content.Load<Texture2D>(@"Maps/Science/104/sputnik");
            portalSheet = content.Load<Texture2D>(@"Maps/Science/104/PortalSheet");

            triceratopsTextures = ContentLoader.LoadContent(content, "Maps\\Science\\104\\TriBlink");
            Game1.npcFaces["Triceratops"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Triceratops");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Triceratops"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.ErlTheFlask(content);
            EnemyContentLoader.BennyBeaker(content);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;

            //--Clears the map during the transition of prologue to chapter 1
            if (game.chapterState == Game1.ChapterState.chapterOne && enemiesInMap.Count > 0 && enemyNamesAndNumberInMap["Erl The Flask"] == 0 && enemyNamesAndNumberInMap["Benny Beaker"] == 0)
            {
                enemiesInMap.Clear();
            }
        }

        public override void Update()
        {
            base.Update();

            if (!up2)
            {
                v2 += .006f;
                y2 += v2;

                if (v2 >= .5)
                    up2 = true;
            }
            else
            {
                v2 -= .006f;
                y2 += v2;

                if (v2 <= -.5)
                    up2 = false;
            }

            interactiveObjects[1].RecY = 137 + (int)y2;


            sputnikX += 4;

            if (sputnikX > mapRec.Width)
                sputnikX = -sputnik.Width - 50;

            if (floatCycle < 75)
            {
                if (floatCycle % 3 == 0)
                    sputnikY -= 1; floatCycle++;
            }
            else
            {
                if (floatCycle % 3 == 0)
                    sputnikY += 1; floatCycle++;

                if (floatCycle >= 150)
                {
                    floatCycle = 0;
                }
            }

            #region Triceratops
            timeUntilNextBlink--;

            if (timeUntilNextBlink <= 0)
            {
                triDelay--;

                if (triDelay <= 0)
                {
                    triFrame++;
                    triDelay = 5;

                    if (triFrame > 5)
                    {
                        triFrame = 0;
                        timeUntilNextBlink = randomNum.Next(240, 500);
                    }
                }
            }
            #endregion

            portalDelay--;

            if (portalDelay <= 0)
            {
                portalFrame++;

                if (portalFrame > 17)
                    portalFrame = 0;

                portalDelay = 4;
            }

            //--If there aren't max enemies on the screen, spawn more
            if (game.chapterState == Game1.ChapterState.prologue)
            {
                if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                    RespawnGroundEnemies();
                if (enemiesInMap.Count == enemyAmount)
                {
                    spawnEnemies = false;
                    game.MapBooleans.prologueMapBooleans["spawnedBennys"] = true;
                }
            }
            else
            {
                if (enemiesInMap.Count < enemyAmount)
                    RespawnGroundEnemies();
            }


            if (!up)
            {
                v1 += .008f;
                y1 += v1;

                if (v1 >= .45)
                    up = true;
            }
            else
            {
                v1 -= .008f;
                y1 += v1;

                if (v1 <= -.45)
                    up = false;
            }

            if (player.VitalRec.Intersects(gogglesRec) && last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space) && game.Prologue.PrologueBooleans["FoundGoggles"] == false)
            {
                player.AddAccessoryToInventory(new LabGoggles());
                game.Prologue.PrologueBooleans["FoundGoggles"] = true;

                Chapter.effectsManager.AddFoundItem("Lab Goggles", Game1.equipmentTextures["Lab Goggles"]);

                Chapter.effectsManager.AddInGameDialogue("You haven't seen any of my relatives around, have you?", "Triceratops", "Normal", 180);
            }
            if (game.Prologue.PrologueBooleans["FoundGoggles"] == false)
            {
                if(Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(gogglesRec.Center.X, gogglesRec.Center.Y)) < 300)
                {
                    Chapter.effectsManager.AddInGameDialogue("Press 'SPACE' to pick up shiny objects such as that one over there. Get it? SPACE? I'm in space.", "Triceratops", "Normal", 1);
                }
                sparkles.Update();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScience103 = new Portal(0, platforms[0], "Science104");
            toScience105 = new Portal(6020, -40, "Science104");
            toScience105.FButtonYOffset = -10;
            toScience105.PortalNameYOffset = -10;

            toBathroom = new Portal(5120, 14, "Science104");
            toBathroom.PortalRecY = -160;

            toScience101 = new Portal(6040, 640, "Science104");
            toScience101.FButtonYOffset = -10;
            toScience101.PortalNameYOffset = -10;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScience103, Science103.ToScience104);
            portals.Add(toScience105, Science105.ToScience104);
            portals.Add(toScience101, Science101.ToScience104);
            portals.Add(ToBathroom, Bathroom.ToLastMap);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Rectangle(5020, -190, outhouse.Width, outhouse.Height), Color.White);

            if(portalFrame < 9)
                s.Draw(portalSheet, new Rectangle(6021, mapRec.Y + 693, 179, 484), new Rectangle(179 * portalFrame, 0, 179, 484), Color.White);

            if (game.Prologue.PrologueBooleans["FoundGoggles"] == false)
            {
                s.Draw(goggles, new Vector2(gogglesRec.X, gogglesRec.Y + y1), Color.White);
                sparkles.Draw(s);
                sparkles.rec.X = 5474;
                sparkles.rec.Y = 295 + (int)y1;
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(6200 - foreground.Width, mapRec.Y), Color.White);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(sputnik, new Vector2(sputnikX, mapRec.Y + 915 +  sputnikY), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(Game1.whiteFilter, new Rectangle(0, mapRec.Y, mapRec.Width, mapRec.Height), new Color(25, 25, 25));
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.88f, this, game));
            s.Draw(galaxy, new Rectangle(28, mapRec.Y + 29, galaxy.Width, galaxy.Height), Color.White);
            s.Draw(triceratopsTextures["triceratops" + triFrame.ToString()], new Rectangle(28, mapRec.Y + 29, galaxy.Width, galaxy.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.95f, this, game));
            s.Draw(moon, new Rectangle(207, mapRec.Y + 479, moon.Width, moon.Height), Color.White);
            s.End();
        }
    }
}