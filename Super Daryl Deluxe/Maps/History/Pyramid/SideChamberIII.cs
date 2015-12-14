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
    class SideChamberIII : MapClass
    {
        static Portal toSideChamberIV;
        static Portal toSideChamberII;

        public static Portal ToSideChamberII { get { return toSideChamberII; } }
        public static Portal ToSideChamberIV { get { return toSideChamberIV; } }
        ExplodingFlower flower;
        Textbook text;
        Texture2D foreground, bars, hole;

        public SideChamberIII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1850;
            mapName = "Side Chamber III";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 2;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            SpikeTrap spikeTrap1 = new SpikeTrap(70, 386, 488, game, 60);
            SpikeTrap spikeTrap2 = new SpikeTrap(70, 1096, 488, game, 60);

            text = new Textbook(960, 540, 1);
            text.AbleToPickUp = false;
            collectibles.Add(text);

            flower = new ExplodingFlower(game, 900, 485, false, 300);
            interactiveObjects.Add(flower);

            spikeTrap1.Timer = 65;
            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber3\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber3\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber3\bars");
            hole = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber3\backgroundHole");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 2)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = false;
                en.FacingRight = false;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Anubis Warrior"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && game.ChapterTwo.ChapterTwoBooleans["sideChamberIIICleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"])
            {
                RespawnGroundEnemies();

                toSideChamberII.IsUseable = false;
                toSideChamberIV.IsUseable = false;
            }
            else if (enemiesInMap.Count == enemyAmount && game.ChapterTwo.ChapterTwoBooleans["sideChamberIIICleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"] = true;
                game.Camera.ShakeCamera(5, 15);
            }
            else if (enemiesInMap.Count == 0 && game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"] = false;
                game.ChapterTwo.ChapterTwoBooleans["sideChamberIIICleared"] = true;
                game.Camera.ShakeCamera(5, 15);

                toSideChamberII.IsUseable = true;
                toSideChamberIV.IsUseable = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["sideChamberIIIWallBlown"] == false)
            {
                if (flower.flowerState == ExplodingFlower.FlowerState.dead)
                {
                    text.AbleToPickUp = true;
                    game.ChapterTwo.ChapterTwoBooleans["sideChamberIIIWallBlown"] = true;
                    game.Camera.ShakeCamera(10, 25);

                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toSideChamberIV = new Portal(1600, platforms[0], "Side Chamber III");
            toSideChamberII = new Portal(105, platforms[0], "Side Chamber III");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["sideChamberIIIWallBlown"] && background[0] != hole)
            {
                background[0] = hole;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["sideChamberIIILocked"])
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideChamberII, SideChamberII.ToSideChamberIII);
            portals.Add(toSideChamberIV, SideChamberIV.ToSideChamberIII);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
