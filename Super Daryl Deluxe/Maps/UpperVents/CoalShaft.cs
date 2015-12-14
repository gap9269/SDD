using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class CoalShaft : MapClass
    {
        static Portal toUpperVents3;

        public static Portal ToUpperVents3 { get { return toUpperVents3; } }

        CoalDeposit coal1, coal2, coal3, coal4, coal5, coal6, coal7;

        Texture2D coalTexture;

        public CoalShaft(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;

            zoomLevel = .9f;

            mapWidth = 2700;
            mapHeight = 2020;
            mapName = "Coal Shaft";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();
            overlayType = 1;
            enemyAmount = 8;

            enemiesToKill.Add(15);
            enemiesKilledForQuest.Add(0); enemyNames.Add("Fluffles the Rat");

            //--Map Quest
            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(750, 630 - Game1.mapSign.Height, "Exterminate 15 rats", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(75), new Karma(2), new LabGoggles()});
            mapQuestSigns.Add(sign);

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);

            coal1 = new CoalDeposit(game, 412, mapRec.Y + 1461, Game1.whiteFilter, 2, new Coal(412, mapRec.Y + 1461), 1, true, false);
            interactiveObjects.Add(coal1);

            coal2 = new CoalDeposit(game, 1727, mapRec.Y + 1400, Game1.whiteFilter, 3, new Coal(1727, 1400), 2, true, false);
            interactiveObjects.Add(coal2);

            coal3 = new CoalDeposit(game, 0, mapRec.Y + 320, Game1.whiteFilter, 5, new Coal(0, mapRec.Y + 320), 3, false, false);
            interactiveObjects.Add(coal3);

            coal4 = new CoalDeposit(game, 2203, mapRec.Y + 1075, Game1.whiteFilter, 3, new Coal(2203, mapRec.Y + 1075), 2, false, true);
            interactiveObjects.Add(coal4);

            coal5 = new CoalDeposit(game, 511, mapRec.Y + 823, Game1.whiteFilter, 2, new Coal(511, mapRec.Y + 823), 1, false, true);
            interactiveObjects.Add(coal5);

            coal6 = new CoalDeposit(game, 2400, mapRec.Y + 1480, Game1.whiteFilter, 3, new Coal(2400, mapRec.Y + 1480), 2, true, true);
            interactiveObjects.Add(coal6);

            coal7 = new CoalDeposit(game, 1206, mapRec.Y + 1075, Game1.whiteFilter, 5, new Coal(1206, mapRec.Y + 1075), 3, false, true);
            interactiveObjects.Add(coal7);
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Fluffles the Rat"] < 8)
            {
                FlufflesTheRat en = new FlufflesTheRat(pos, "Fluffles the Rat", game, ref player, this);
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
                    enemyNamesAndNumberInMap["Fluffles the Rat"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Coal Shaft\background"));
            coalTexture = content.Load<Texture2D>(@"InteractiveObjects\CoalSprite");
            Sound.LoadVentZoneSounds();

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i] is CoalDeposit)
                    interactiveObjects[i].Sprite = coalTexture;
            }
        }
        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i] is CoalDeposit)
                {
                    interactiveObjects[i].Finished = false;
                    interactiveObjects[i].State = 0;
                    interactiveObjects[i].Health = coal1.MaxHealth;
                }
            }
        }

        public override void Update()
        {
            base.Update();
            PlayAmbience();
            if (enemiesInMap.Count < enemyAmount)
            {
               RespawnGroundEnemies();
            }

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents3 = new Portal(150, 630, "Coal Shaft");
            toUpperVents3.FButtonYOffset = -30;
            toUpperVents3.PortalNameYOffset = -30;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents3, UpperVentsIII.ToCoalShaft);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
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
            }
            s.End();
        }
    }
}
