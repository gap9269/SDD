using System;
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
    public class Science103 : MapClass
    {
        static Portal toScience102;
        static Portal toScience104;

        Platform door;
        WallSwitch doorSwitch;
        MovingPlatform elevator;
        List<Vector2> targets;
        TreasureChest chest;

        public static Dictionary<String, Texture2D> flowerTextures;
        public static Dictionary<String, Texture2D> laserTextures;
        public static Dictionary<String, Texture2D> doorTextures;
        public static Texture2D gate;

        //FLOWER POSITION IS 0,925
        int flowerFrame, flowerDelay;

        Texture2D elevatorTex, fore1, fore2, parallax;

        public static Portal ToScience102 { get { return toScience102; } }
        public static Portal ToScience104 { get { return toScience104; } }

        public Science103(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 4400;
            mapHeight = 3000;
            mapName = "Science 103";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = true;
            zoomLevel = .85f;
            mapWithMapQuest = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            flowerDelay = 4;

            //--Map Quest
            enemiesToKill.Add(6);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Erl The Flask");

            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1700, -1000, 50, 800), false, false, false);
            platforms.Add(door);

            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(700, -100, 333, 335));
            switches.Add(doorSwitch);

            targets = new List<Vector2>();

            elevator =  new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3060, -650, 270, 50),
                true, false, false, targets, 3, 100);

            platforms.Add(elevator);

            MapQuestSign sign = new MapQuestSign(800, 633 - Game1.mapSign.Height + 20, "Slay Six Flasks to Proceed", enemiesToKill,
                enemiesKilledForQuest, enemyNames, player);
            mapQuestSigns.Add(sign);

            chest = new TreasureChest(Game1.treasureChestSheet, 3700, -1330, player, 0, new GymShirt(), this);
            treasureChests.Add(chest);
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.ErlTheFlask(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps/Science/103/background1"));
            background.Add(content.Load<Texture2D>(@"Maps/Science/103/background2"));

            fore1 = content.Load<Texture2D>(@"Maps/Science/103/foreground1");
            fore2 = content.Load<Texture2D>(@"Maps/Science/103/foreground2");

            elevatorTex = content.Load<Texture2D>(@"Maps/Science/103/elevator");

            parallax = content.Load<Texture2D>(@"Maps/Science/103/parallax");

            if (!game.MapBooleans.prologueMapBooleans["spawnEnemies"])
            {
                gate = content.Load<Texture2D>(@"Maps/Science/103/gate");
                doorTextures = ContentLoader.LoadContent(content, "Maps\\Science\\103\\GateDestroy");
                laserTextures = ContentLoader.LoadContent(content, "Maps\\Science\\103\\Lasers");
            }

            flowerTextures = ContentLoader.LoadContent(content, "Maps\\Science\\103\\Head Bob");
        }

        public override void RespawnGroundEnemies()
        {
            if (game.MapBooleans.prologueMapBooleans["spawnEnemies"] && enemiesToKill[0] - enemiesKilledForQuest[0] > enemiesInMap.Count)
            {
                base.RespawnGroundEnemies();

                switch (game.chapterState)
                {
                    case Game1.ChapterState.prologue:
                        ErlTheFlask en = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
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
                        break;
                }
            }
        }

        public override void Update()
        {
            base.Update();

            #region Flower Animation
            flowerDelay--;

            if(flowerDelay <= 0)
            {
                flowerDelay = 4;
                flowerFrame++;

                if (flowerFrame > 18)
                    flowerFrame = 0;
            }
            #endregion

            //--If there aren't max enemies on the screen, spawn more
            if (enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();

            if (doorSwitch.Active)
            {
                if (!game.MapBooleans.prologueMapBooleans["spawnEnemies"])
                    game.MapBooleans.prologueMapBooleans["spawnEnemies"] = true;

                if (platforms.Contains(door))
                    platforms.Remove(door);
            }

            if (CheckSwitch(doorSwitch) && !game.MapBooleans.prologueMapBooleans["spawnEnemies"])
            {
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
                flowerDelay = 4;
                flowerFrame = 0;
            }


            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }

            if (completedMapQuest && targets.Count == 0)
            {
                targets.Add(new Vector2(3060, -1400));
                targets.Add(new Vector2(3060, -650));
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScience102 = new Portal(10, platforms[0], "Science103");
            toScience102.FButtonYOffset = -16;
            toScience102.PortalNameYOffset = -16;
            toScience104 = new Portal(4190, -1050 - Game1.portalTexture.Height, "Science103");
            toScience104.FButtonYOffset = -40;
            toScience104.PortalNameYOffset = -40;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScience102, Science102.ToScience103);
            portals.Add(toScience104, Science104.ToScience103);
        }

        public void DrawFlower(SpriteBatch s)
        {
            String textureString = "science103";
            if (flowerFrame < 10)
                textureString += "0" + flowerFrame.ToString();
            else
                textureString += flowerFrame.ToString();

            s.Draw(flowerTextures.ElementAt(flowerFrame).Value, new Vector2(0, mapRec.Y + 925), Color.White);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(elevatorTex, new Vector2(elevator.RecX - 65, elevator.RecY - 100), Color.White);

            if (platforms.Contains(door) && game.CurrentChapter.state != Chapter.GameState.Cutscene)
                s.Draw(gate, new Vector2(1654, mapRec.Y + 858), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(fore1, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(fore2, new Vector2(mapRec.Width - fore2.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.13f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(Game1.whiteFilter, new Rectangle(0, mapRec.Y, mapRec.Width, mapRec.Height), new Color(25, 25, 25));

            if (game.CurrentChapter.state != Chapter.GameState.Cutscene)
            {
                DrawFlower(s);
            }

            s.End();
        }
    }
}
