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
    class Chamber44 : MapClass
    {
        static Portal toRoomOfOffering;
        public static Portal ToRoomOfOffering { get { return toRoomOfOffering; } }

        static Portal toPrisonChamber;
        public static Portal ToPrisonChamber { get { return toPrisonChamber; } }

        static Portal toUndergroundTunnelI;
        public static Portal ToUndergroundTunnelI { get { return toUndergroundTunnelI; } }

        static Portal toOrganStorageRoomOne;
        public static Portal ToOrganStorageRoomOne { get { return toOrganStorageRoomOne; } }

        Texture2D foreground, door;
        float doorAlpha;

        public Chamber44(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Chamber 44";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 6;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            interactiveObjects.Add(new Barrel(game, 364, 431 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 2430, 431 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 1302, 433 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .36f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 666, 431 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 1413, 437 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 559, 431 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\Chamber44\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\Chamber44\foreground");
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\door");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
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

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 3)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

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
            else
            {
                Enemy en;

                if (Game1.randomNumberGen.Next(0, 3) == 1)
                    en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
                else
                    en = new Mummy(pos, "Mummy", game, ref player, this);

                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = false;

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

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (spawnEnemies && enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toRoomOfOffering = new Portal(120, platforms[0], "Chamber 44");
            toPrisonChamber = new Portal(1200, platforms[0], "Chamber 44");
            toOrganStorageRoomOne = new Portal(2050, platforms[0], "Chamber 44");
            toUndergroundTunnelI = new Portal(2700, platforms[0], "Chamber 44");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toRoomOfOffering, RoomOfOfferings.ToChamber44);
            portals.Add(toOrganStorageRoomOne, OrgranStorageRoomOne.ToChamber44);
            portals.Add(toPrisonChamber, PrisonChamber.ToChamber44);
            portals.Add(toUndergroundTunnelI, UndergroundTunnelI.ToChamber44);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.VitalRec.X < 1700 && player.VitalRecX > 700)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(665, 0), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
