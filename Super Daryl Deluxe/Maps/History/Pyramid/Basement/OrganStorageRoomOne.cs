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
    class OrgranStorageRoomOne : MapClass
    {
        static Portal toChamber44;
        public static Portal ToChamber44 { get { return toChamber44; } }

        Texture2D foreground, jar;
        Sparkles sparkles;

        public OrgranStorageRoomOne(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Organ Storage Room One";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 2;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            sparkles = new Sparkles(954, 458);

            interactiveObjects.Add(new Barrel(game, 1082, mapRec.Y + 473 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 755, 473 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomOne\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomOne\foreground");
            jar = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomOne\jar");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

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
                    AddEnemyToEnemyList(en);
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

            if (player.VitalRec.Intersects(sparkles.rec) && ((last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space)) || MyGamePad.RightTriggerPressed()) && game.ChapterTwo.ChapterTwoBooleans["organChamberOneJarObtained"] == false)
            {
                player.AddStoryItem("Jarred Heart", "a Jarred Heart", 1);
                game.ChapterTwo.ChapterTwoBooleans["organChamberOneJarObtained"] = true;
            }
            if (!game.ChapterTwo.ChapterTwoBooleans["organChamberOneJarObtained"])
            {
                sparkles.Update();
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toChamber44 = new Portal(120, platforms[0], "Organ Storage Room One");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["organChamberOneJarObtained"])
            {
                s.Draw(jar, new Vector2(965, 428), Color.White);
                sparkles.Draw(s);

            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toChamber44, Chamber44.ToOrganStorageRoomOne);
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
