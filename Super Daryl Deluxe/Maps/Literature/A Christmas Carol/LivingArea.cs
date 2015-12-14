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
    class LivingArea : MapClass
    {
        static Portal toCentralCorridor;
        static Portal toEbenezersMansion;

        public static Portal ToCentralCorridor { get { return toCentralCorridor; } }
        public static Portal ToEbenezersMansion { get { return toEbenezersMansion; } }

        Texture2D door;
        GhostLight light1;
        float doorAlpha;

        TreasureChest chest;

        ScroogeFirePlace firePlace;

        public LivingArea(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Living Area";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            chest = new TreasureChest(Game1.treasureChestSheet, 10000, 452 + 180, player, 0, new ChristmasCageKey(0,0), this); //1300

            treasureChests.Add(chest);

            light1 = new GhostLight(game, 95, 0, true, false, false, true);
            interactiveObjects.Add(light1);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);

            firePlace = new ScroogeFirePlace(game, 272, 414);
            interactiveObjects.Add(firePlace);
            (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Add(firePlace);
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

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\LivingArea\background"));
            door = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\LivingArea\door");

            firePlace.fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");
        }

        public override void Update()
        {
            base.Update();

            chest.RecY = 482;

            if (game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                {
                    spawnEnemies = false;
                }
            }

            if (enemiesInMap.Count == 0 && !spawnEnemies && !game.ChapterTwo.ChapterTwoBooleans["livingRoomChestSpawned"])
            {
                game.ChapterTwo.ChapterTwoBooleans["livingRoomChestSpawned"] = true;
                Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(1282, 454, 266, 266), 2);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["livingRoomChestSpawned"] && chest.RecX != 1300)
            {
                chest.RecX = 1300;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralCorridor = new Portal(50, platforms[0], "Living Area");
            toEbenezersMansion = new Portal(1690, platforms[0], "Living Area");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralCorridor, CentralCorridor.toLivingArea);
            portals.Add(toEbenezersMansion, EbenezersMansion.ToLivingArea);
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

            s.Draw(door, new Vector2(1151, 0), Color.White * doorAlpha);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
