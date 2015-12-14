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
    class SideChamberI : MapClass
    {
        static Portal toSideChamberII;
        static Portal toOuterChamber;

        public static Portal fromSideChamberIV;

        public static Portal ToOuterChamber { get { return toOuterChamber; } }
        public static Portal ToSideChamberII { get { return toSideChamberII; } }

        Texture2D foreground;

        public SideChamberI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1450;
            mapName = "Side Chamber I";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            PyramidKey key = new PyramidKey(853, 118);
            collectibles.Add(key);

            Barrel bar = new Barrel(game, 548, 480 + 155, Game1.interactiveObjects["Barrel"], true, 1, 3, .18f, false, Barrel.BarrelType.pyramidPitcher);
            interactiveObjects.Add(bar);

            Barrel bar1 = new Barrel(game, 685, 480 + 155, Game1.interactiveObjects["Barrel"], true, 2, 3, .08f, false, Barrel.BarrelType.pyramidUrn);
            bar1.facingRight = false;
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 841, 480 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .16f, false, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar2);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber1\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber1\foreground");
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

            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 3)
            {
                SexySaguaro en = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 30;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Sexy Saguaro"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 2)
            {
                BurnieBuzzard en = new BurnieBuzzard(pos, "Burnie Buzzard", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Burnie Buzzard"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toSideChamberII = new Portal(90, platforms[0], "Side Chamber I");
            toOuterChamber = new Portal(1175, platforms[0], "Side Chamber I");
            fromSideChamberIV = new Portal(425, -100, mapName);

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOuterChamber, OuterChamber.ToSideChamberI);
            portals.Add(toSideChamberII, SideChamberII.ToSideChamberI);
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
