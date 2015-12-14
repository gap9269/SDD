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
    class DarkenedChamber : MapClass
    {
        static Portal toBasementEntrance;
        public static Portal ToBasementEntrance { get { return toBasementEntrance; } }

        static Portal toTheMoaningHallway;
        public static Portal ToTheMoaningHallway { get { return toTheMoaningHallway; } }

        static Portal toTheRestingChamber;
        public static Portal ToTheRestingChamber { get { return toTheRestingChamber; } }

        static Portal toSmallTreasureRoom;
        public static Portal ToSmallTreasureRoom { get { return toSmallTreasureRoom; } }

        Texture2D foreground, door;
        float doorAlpha;

        public DarkenedChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Darkened Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 4;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);
            interactiveObjects.Add(new Barrel(game, 430, 445 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 510, 443 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 928, 445 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2404, 443 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 2558, 443 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\DarkenedChamber\background"));
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\door");
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\DarkenedChamber\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
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

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toBasementEntrance = new Portal(2080, platforms[0], "Darkened Chamber");
            toTheMoaningHallway = new Portal(1360, platforms[0], "Darkened Chamber");
            toTheRestingChamber = new Portal(80, platforms[0], "Darkened Chamber");
            toSmallTreasureRoom = new Portal(2730, platforms[0], "Darkened Chamber", "Silver Key");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBasementEntrance, BasementEntrance.ToDarkenedChamber);
            portals.Add(toSmallTreasureRoom, SmallTreasureRoom.ToDarkenedChamber);
            portals.Add(toTheRestingChamber, RestingChamber.ToDarkenedChamber);
            portals.Add(toTheMoaningHallway, TheMoaningHallway.ToDarkenedChamber);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.VitalRec.X < 2500 && player.VitalRecX > 1700)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(1570, 0), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
