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
    class OrgranStorageRoomTwo : MapClass
    {
        static Portal toTortureChamber;
        public static Portal ToTortureChamber { get { return toTortureChamber; } }

        Texture2D foreground, jar;
        Sparkles sparkles;

        public OrgranStorageRoomTwo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Organ Storage Room Two";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            sparkles = new Sparkles(322, 474);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            interactiveObjects.Add(new Barrel(game, 623, 481 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 42, 471 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 743, 479 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomTwo\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomTwo\foreground");
            jar = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomTwo\jar");
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

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 3)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.SpawnWithPoof = true;
                en.Hostile = true;

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

            if (player.VitalRec.Intersects(sparkles.rec) && ((last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space)) || MyGamePad.RightTriggerPressed()) && game.ChapterTwo.ChapterTwoBooleans["organChamberTwoJarObtained"] == false)
            {
                player.AddStoryItem("Jarred Liver", "a Jarred Liver", 1);
                game.ChapterTwo.ChapterTwoBooleans["organChamberTwoJarObtained"] = true;
            }
            if (!game.ChapterTwo.ChapterTwoBooleans["organChamberTwoJarObtained"])
            {
                sparkles.Update();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toTortureChamber = new Portal(1000, platforms[0], "Organ Storage Room Two");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["organChamberTwoJarObtained"])
            {
                s.Draw(jar, new Vector2(332, 444), Color.White);
                sparkles.Draw(s);

            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTortureChamber, TortureChamber.ToOrganStorageRoomTwo);
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
