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
    class SideChamberIV : MapClass
    {
        static Portal toSideChamberIII;

        public static Portal ToSideChamberIII { get { return toSideChamberIII; } }

        Texture2D foreground, brokenFloor, floor;
        Platform crumblingFloor;

        public SideChamberIV(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1450;
            mapName = "Side Chamber IV";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            crumblingFloor = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(795, 615, 50, 50), false, false, false, 0);
            platforms.Add(crumblingFloor);

            treasureChests.Add(new TreasureChest(Game1.treasureChestSheet, 1170, 435 + 180, player, 3.50f, new EnemyDrop("Topaz", new Rectangle()), this));

            Barrel bar = new Barrel(game, 984, 456 + 155, Game1.interactiveObjects["Barrel"], true, 3, 3, .38f, false, Barrel.BarrelType.pyramidPitcher);
            interactiveObjects.Add(bar);

            Barrel bar1 = new Barrel(game, 1056, 459 + 155, Game1.interactiveObjects["Barrel"], true, 3, 10, .08f, false, Barrel.BarrelType.pyramidPitcher);
            bar1.facingRight = false;
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 1372, 456 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .46f, false, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar2);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber4\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber4\foreground");
            floor = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber4\tile");
            brokenFloor = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber4\crackedTile");

            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Chained Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
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
            if (player.Landing && player.CurrentPlat == crumblingFloor && platforms.Contains(crumblingFloor))
            {
                player.Landing = false;
                player.CurrentPlat = null;
                player.PositionY += 20;
                player.VelocityY += 10;
                platforms.Remove(crumblingFloor);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(713, 563, 200, 200), 2);
            }

            if (player.VitalRecY > 800)
            {
                ForceToNewMap(new Portal(600, 800, "Side Chamber IV"), SideChamberI.fromSideChamberIV);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toSideChamberIII = new Portal(125, platforms[0], "Side Chamber IV");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(platforms.Contains(crumblingFloor))
                s.Draw(brokenFloor, new Vector2(733, 600), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideChamberIII, SideChamberIII.ToSideChamberIV);
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
