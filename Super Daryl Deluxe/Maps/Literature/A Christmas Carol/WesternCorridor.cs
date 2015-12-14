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
    class WesternCorridor : MapClass
    {
        static Portal toScroogesBedroom;
        static Portal toEbenezersMansion;

        public static Portal ToScroogesBedroom { get { return toScroogesBedroom; } }
        public static Portal ToEbenezersMansion { get { return toEbenezersMansion; } }

        Texture2D foreground;
        GhostLight light1, light2, light3, light4, light5;

        public WesternCorridor(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Western Corridor";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            light1 = new GhostLight(game, 111, 22);
            interactiveObjects.Add(light1);

            light2 = new GhostLight(game, 890, 22);
            interactiveObjects.Add(light2);

            light3 = new GhostLight(game, 2111, 22);
            interactiveObjects.Add(light3);

            light4 = new GhostLight(game, 2890, 22);
            interactiveObjects.Add(light4);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\WesternCorridor\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\WesternCorridor\foreground");
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

            RespawnGroundEnemies();

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScroogesBedroom = new Portal(100, platforms[0], "Western Corridor");
            toEbenezersMansion = new Portal(3800, platforms[0], "Western Corridor");

            toScroogesBedroom.PortalNameYOffset = -20;
            toScroogesBedroom.FButtonYOffset = -20;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScroogesBedroom, ScroogesBedroom.ToWesternCorridor);
            portals.Add(toEbenezersMansion, EbenezersMansion.ToWesternCorridor);
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
