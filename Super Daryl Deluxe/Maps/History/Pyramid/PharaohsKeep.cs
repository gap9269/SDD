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
    class PharaohsKeep : MapClass
    {
        static Portal toMainChamber;
        static Portal toInnerChamber;
        static Portal toEasternChamber;
        static Portal toChamberOfSymmetry;

        public static Portal ToChamberOfSymmetry { get { return toChamberOfSymmetry; } }
        public static Portal ToEasternChamber { get { return toEasternChamber; } }
        public static Portal ToInnerChamber { get { return toInnerChamber; } }
        public static Portal ToMainChamber { get { return toMainChamber; } }

        Texture2D foreground, door;
        float doorAlpha;

        public PharaohsKeep(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Pharaoh's Keep";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);


            interactiveObjects.Add(new Barrel(game, 514, 449 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .26f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 1116, 446 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1700, 449 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .36f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1048, 446 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .16f, false, Barrel.BarrelType.pyramidPitcher));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsKeep\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsKeep\foreground");
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\door");
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

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < enemyAmount)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);
                en.SpawnWithPoof = false;

                if (Game1.randomNumberGen.Next(0, 2) == 0)
                    en.FacingRight = true;
                else
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

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemyAmount > enemiesInMap.Count && spawnEnemies)
                RespawnGroundEnemies();

            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMainChamber = new Portal(850, platforms[0], "Pharaoh's Keep");
            toInnerChamber = new Portal(1300, platforms[0], "Pharaoh's Keep");
            toEasternChamber = new Portal(150, platforms[0], "Pharaoh's Keep");
            toChamberOfSymmetry = new Portal(1920, platforms[0], "Pharaoh's Keep");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainChamber, MainChamber.ToPharaohsKeep);
            portals.Add(toEasternChamber, EasternChamber.ToPharaohsKeep);
            portals.Add(toChamberOfSymmetry, ChamberOfSymmetry.ToPharaohsKeep);
            portals.Add(toInnerChamber, InnerChamber.ToPharaohsKeep);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.VitalRec.X < 1300 && player.VitalRecX > 550)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(300, 0), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
