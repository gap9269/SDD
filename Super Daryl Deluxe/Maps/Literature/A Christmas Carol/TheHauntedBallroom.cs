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
    class TheHauntedBallroom : MapClass
    {
        public static Portal toTheDiningHall;
        public static Portal toEasternCorridor;
        public static Portal toTheFoyer;

        Texture2D foreground, rails;

        Platform lightPlat;
        List<Vector2> targets2;
        WallSwitch doorSwitch;

        GhostLight light;

        ScroogeFirePlace firePlace;

        public TheHauntedBallroom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 4000;
            mapName = "The Haunted Ballroom";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 15;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            targets2 = new List<Vector2>();

            lightPlat = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2315, -2000, 300, 50),
                true, false, false, targets2, 4, 25, Platform.PlatformType.rock);
            platforms.Add(lightPlat);

            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(3200, mapRec.Y + 403, 333, 335));
            switches.Add(doorSwitch);

            light = new GhostLight(game, lightPlat.RecX, 0, false, false, false, true);
            interactiveObjects.Add(light);

            //--Map Quest
            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(1772, mapRec.Y + 900, "Clear the area of enemies!", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Textbook(), new Experience(1050), new Money(70.00f) });
            mapQuestSigns.Add(sign);

            enemyNamesAndNumberInMap.Add("Spooky Present", 0);
            enemyNamesAndNumberInMap.Add("Eerie Elf", 0);
            enemyNamesAndNumberInMap.Add("Haunted Nutcracker", 0);

            firePlace = new ScroogeFirePlace(game, 1990, mapRec.Y + 1103);
            interactiveObjects.Add(firePlace);
            (Game1.schoolMaps.maps["Ebenezer's Mansion"] as EbenezersMansion).firePlaces.Add(firePlace);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\HauntedBallroom\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\HauntedBallroom\foreground");
            rails = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\HauntedBallroom\rails");

            firePlace.fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
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

            CheckSwitch(doorSwitch, 0);

            if (spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                {
                    spawnEnemies = false;
                }
            }

            if (doorSwitch.Active && targets2.Count == 0)
            {
                targets2.Add(new Vector2(250, -2000));
                targets2.Add(new Vector2(2315,- 2000));
                light.active = true;
            }

            light.RecX = lightPlat.RecX;
            light.UpdatePosition();

            if (enemiesInMap.Count == 0 && !spawnEnemies)
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheDiningHall = new Portal(3750, platforms[0], "The Haunted Ballroom", "Ghost Key");
            toEasternCorridor = new Portal(3800, platforms[1], "The Haunted Ballroom");
            toTheFoyer = new Portal(50, platforms[0], "The Haunted Ballroom");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheFoyer, TheGrandCorridor.toBallroom);
            portals.Add(toEasternCorridor, EasternCorridor.toBallroom);
            portals.Add(toTheDiningHall, DiningHall.toTheBallroom);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(rails, new Vector2(3166, mapRec.Y + 620), Color.White);

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
