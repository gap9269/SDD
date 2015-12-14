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
    class TortureChamber : MapClass
    {
        static Portal toPrisonChamber;
        public static Portal ToPrisonChamber { get { return toPrisonChamber; } }

        static Portal toOrganStorageRoomTwo;
        public static Portal ToOrganStorageRoomTwo { get { return toOrganStorageRoomTwo; } }

        Texture2D foreground, door, gradient;

        int vileMummyChance = 10;
        int numKilled;

        SpriteFont terminalFont;
        public TortureChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Torture Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            //Room is barred
            enemiesToKill.Add(50);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Mummy");
            enemiesToKill.Add(5000);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Vile Mummy");
            mapWithMapQuest = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new ExplodingFlower(game, 310, 480, false, 250));
            interactiveObjects.Add(new ExplodingFlower(game, 663, 480, false, 180));
            interactiveObjects.Add(new ExplodingFlower(game, 1300, 480, false, 215));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\TortureChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\TortureChamber\foreground");
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\TortureChamber\door");
            gradient = content.Load<Texture2D>(@"Maps\History\Pyramid\TortureChamber\gradient");
            terminalFont = content.Load<SpriteFont>(@"Fonts\TortureChamberFont");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Enemy en;

            if (Game1.randomNumberGen.Next(0, 101) < vileMummyChance)
                en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
            else
                en = new Mummy(pos, "Mummy", game, ref player, this);

            en.Hostile = true;

            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.SpawnWithPoof = true;
            en.TimeBeforeSpawn = 50 - (enemyAmount * 2);

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

            if (!game.ChapterTwo.ChapterTwoBooleans["tortureChamberOpened"])
            {
                numKilled = enemiesKilledForQuest[0] + enemiesKilledForQuest[1];

                if (numKilled > enemiesToKill[0])
                {
                    game.ChapterTwo.ChapterTwoBooleans["tortureChamberOpened"] = true;
                    game.Camera.ShakeCamera(10, 20);
                }
                else
                {
                    if (enemiesInMap.Count < enemyAmount)
                        RespawnGroundEnemies();

                    enemyAmount = 5 + (int)(numKilled / 7f);
                    vileMummyChance = enemyAmount + 5 + ((int)(numKilled / 4f) * 2);
                }

                if (toOrganStorageRoomTwo.IsUseable)
                    toOrganStorageRoomTwo.IsUseable = false;
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["tortureChamberOpened"] && !toOrganStorageRoomTwo.IsUseable)
                toOrganStorageRoomTwo.IsUseable = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toPrisonChamber = new Portal(45, platforms[0], "Torture Chamber");
            toOrganStorageRoomTwo = new Portal(930, platforms[0], "Torture Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["tortureChamberOpened"])
            {
                s.Draw(door, new Vector2(930, 211), Color.White);
                s.DrawString(terminalFont, numKilled.ToString("D2"), new Vector2(952, 70), Color.Red);
                s.Draw(gradient, new Vector2(0, 0), Color.White);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPrisonChamber, PrisonChamber.ToTortureChamber);
            portals.Add(toOrganStorageRoomTwo, OrgranStorageRoomTwo.ToTortureChamber);
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
