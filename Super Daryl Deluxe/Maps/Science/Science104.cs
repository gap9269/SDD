﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
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

        public static Portal ToScience103 { get { return toScience103; } }
        public static Portal ToScience105 { get { return toScience105; } }
        public static Portal ToScience101 { get { return toScience101; } }

        Texture2D foreground, galaxy, moon, sputnik, portalSheet;
        Dictionary<String, Texture2D> triceratopsTextures;

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

            MapFire fire = new MapFire(120, 200, 2900, mapRec.Y, game, 5);
            mapHazards.Add(fire);

            MapFire fire2 = new MapFire(120, 200, 3900, mapRec.Y, game, 5);
            mapHazards.Add(fire2);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Benny Beaker", 0);

            randomNum = new Random();

            timeUntilNextBlink = randomNum.Next(240, 500);
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

            foreground = content.Load<Texture2D>(@"Maps/Science/104/foreground");
            galaxy = content.Load<Texture2D>(@"Maps/Science/104/galaxy");
            moon = content.Load<Texture2D>(@"Maps/Science/104/moon");
            sputnik = content.Load<Texture2D>(@"Maps/Science/104/sputnik");
            portalSheet = content.Load<Texture2D>(@"Maps/Science/104/PortalSheet");

            triceratopsTextures = ContentLoader.LoadContent(content, "Maps\\Science\\104\\TriBlink");
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

            sputnikX += 4;

            if (sputnikX > mapRec.Width)
                sputnikX = -sputnik.Width - 50;

            if (floatCycle < 75)
            {
                if(floatCycle % 3 == 0)
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
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScience103 = new Portal(0, platforms[0], "Science104");
            toScience105 = new Portal(6020, -40, "Science104");
            toScience105.FButtonYOffset = -10;
            toScience105.PortalNameYOffset = -10;

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
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(portalFrame < 9)
                s.Draw(portalSheet, new Rectangle(6021, mapRec.Y + 693, 179, 484), new Rectangle(179 * portalFrame, 0, 179, 484), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(6200 - foreground.Width, mapRec.Y), Color.White);

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