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
    class TutorialMapTen : MapClass
    {
        static Portal toMapNine;
        static Portal toMapEleven;
        static Portal toFruitGarden;

        public static Portal ToFruitGarden { get { return toFruitGarden; } }
        public static Portal ToMapNine { get { return toMapNine; } }
        public static Portal ToMapEleven { get { return toMapEleven; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        Platform step;

        int levelTimer;

        static Random ran = new Random();

        public TutorialMapTen(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1300;
            mapWidth = 2500;
            mapName = "Tutorial Map Ten";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            zoomLevel = .9f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();


            //--Map Quest
            mapWithMapQuest = true;
            enemiesToKill.Add(10);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Garden Beast");

            MapQuestSign sign = new MapQuestSign(940, 630 - Game1.mapSign.Height + 20, "Clear the map to continue!", enemiesToKill, enemiesKilledForQuest, enemyNames, player);
            mapQuestSigns.Add(sign);


            step = new Platform(Game1.platformTextures["TutorialMoving"], new Rectangle(1140, 360, 273, 85), true, false, false);
            step.DrawPlatform = true;
            enemyAmount = 3;

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Garden Beast", content.Load<Texture2D>(@"Tutorial\EnemieSheet"));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map10"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\Map10Fore");
        }

        public override void RespawnGroundEnemies()
        {
            if (enemiesToKill[0] - enemiesKilledForQuest[0] > enemiesInMap.Count)
            {
                base.RespawnGroundEnemies();

                int choose = ran.Next(0, 2);

                Enemy en;

                if (choose == 0)
                {
                    en = new GnomeEnemy(pos, "Garden Beast", game, ref player, this);
                }
                else
                {
                    en = new SharkEnemy(pos, "Garden Beast", game, ref player, this);
                }

                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
                en.Position = new Vector2(monsterX, monsterY);
                en.TimeBeforeSpawn = 80;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();

                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {

            if (enemiesToKill[0] - enemiesKilledForQuest[0] > enemiesInMap.Count)
            {

                base.RespawnFlyingEnemies(mapRec);

                int choose = ran.Next(0, 2);

                Enemy en;

                if (choose == 0)
                {
                    en = new ForkEnemy(pos, "Garden Beast", game, ref player, this, mapRec);
                }
                else
                {
                    en = new TubEnemy(pos, "Garden Beast", game, ref player, this, mapRec);
                }
                en.TimeBeforeSpawn = 80;
                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(en);
                }
            }


        }
        public override void Update()
        {
            base.Update();

            if (!completedMapQuest && player.Level == 1 && !game.ChapterTwo.ChapterTwoBooleans["spawnTutorialEnemies"])
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][37], 400, 100, game.ChapterTwo.associateOneTex);
            }
            else if (!completedMapQuest && player.Level == 2 && levelTimer < 200 && !player.LevelingUp)
            {
                if(levelTimer == 1)
                    //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][38], 400, 100, game.ChapterTwo.associateOneTex);

                if (levelTimer == 199)
                    Chapter.effectsManager.RemoveToolTip();

                levelTimer++;
            }

            //Only spawn enemies after you see the map quest sign
            if (mapQuestSigns[0].Active && !game.ChapterTwo.ChapterTwoBooleans["spawnTutorialEnemies"])
            {
                game.ChapterTwo.ChapterTwoBooleans["spawnTutorialEnemies"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            //--If there aren't max enemies on the screen, spawn more
            if (enemiesInMap.Count < enemyAmount && game.ChapterTwo.ChapterTwoBooleans["spawnTutorialEnemies"])
            {

                int choose = ran.Next(0, 10);

                if(choose < 5)
                    RespawnGroundEnemies();
                else
                    RespawnFlyingEnemies(new Rectangle(100, 200, 1900, 400));
            }

            if (enemiesKilledForQuest[0] >= enemiesToKill[0] && completedMapQuest == false)
            {
                completedMapQuest = true;
                game.MapBooleans.tutorialMapBooleans["ClearedGarden"] = true;
                game.CurrentChapter.NPCs["YourFriend"].MapName = "Tutorial Map Eleven";
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][39], 400, 100, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionY < -190)
                Chapter.effectsManager.RemoveToolTip();

            if (completedMapQuest && !platforms.Contains(step))
            {
                platforms.Add(step);
                toMapNine.IsUseable = true;
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapNine = new Portal(-10, 630, "TutorialMapTen");
            toMapNine.IsUseable = false;
            toFruitGarden = new Portal(2300, 350, "TutorialMapTen");
            toMapEleven = new Portal(1200, 111, "TutorialMapTen");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapNine, TutorialMapNine.ToMapTen);
            portals.Add(toFruitGarden, FruitGarden.ToMapTen);
            portals.Add(toMapEleven, TutorialMapEleven.ToMapTen);
        }
    }
}