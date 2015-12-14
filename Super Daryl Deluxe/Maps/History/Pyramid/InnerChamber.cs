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
    class InnerChamber : MapClass
    {
        static Portal toPharaohsKeep;
        static Portal toChamberOfSymmetry;
        static Portal toPharaohsRoad;
        static Portal toFlowerSanctuary;
        static Portal toPitOfLongFalls;

        public static Portal ToPitOfLongFalls { get { return toPitOfLongFalls; } }
        public static Portal ToFlowerSanctuary { get { return toFlowerSanctuary; } }
        public static Portal ToPharaohsRoad { get { return toPharaohsRoad; } }
        public static Portal ToChamberOfSymmetry { get { return toChamberOfSymmetry; } }
        public static Portal ToPharaohsKeep { get { return toPharaohsKeep; } }

        Dictionary<String, Texture2D> wallTextures;

        Texture2D foreground;
        int wallFrame;
        int frameDelay = 5;

        WallSwitch doorSwitch;
        Boolean opening = false;
        Platform door;
        public InnerChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Inner Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1740,300, 50, 500), false, false, false);
            platforms.Add(door);

            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(1000, 280, 333, 335));
            switches.Add(doorSwitch);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\InnerChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\InnerChamber\foreground");
            wallTextures = ContentLoader.LoadContent(content, @"Maps\History\Pyramid\InnerChamber\Wall");

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

            if (doorSwitch.Active)
            {
                opening = true;
            }
            if (opening && wallFrame < 8)
            {
                frameDelay--;

                if(frameDelay <= 0)
                {
                    wallFrame++;
                    frameDelay = 5;

                    if (wallFrame == 8)
                        game.ChapterTwo.ChapterTwoBooleans["innerChamberPlatformOpen"] = true;
                }
                Game1.camera.ShakeCamera(1, 2);
            }
            if (game.ChapterTwo.ChapterTwoBooleans["innerChamberPlatformOpen"])
            {
                if (platforms.Contains(door))
                    platforms.Remove(door);
            }

            if (!doorSwitch.Active)
            {
                CheckSwitch(doorSwitch);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toPharaohsKeep = new Portal(70, platforms[0], "Inner Chamber");
            toChamberOfSymmetry = new Portal(540, platforms[0], "Inner Chamber");
            toPharaohsRoad = new Portal(2750, platforms[0], "Inner Chamber", "Pyramid Key");
            toPitOfLongFalls = new Portal(2200, platforms[0], "Inner Chamber");
            toFlowerSanctuary = new Portal(1460, platforms[0], "Inner Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (wallFrame < 8)
                s.Draw(wallTextures["wall" + wallFrame.ToString()], new Vector2(1692, 0), Color.White);
            else
                s.Draw(wallTextures["wall8"], new Vector2(1692, 0), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsKeep, PharaohsKeep.ToInnerChamber);
            portals.Add(ToChamberOfSymmetry, ChamberOfSymmetry.ToInnerChamber);
            portals.Add(toFlowerSanctuary, FlowerSanctuary.ToInnerChamber);
            portals.Add(toPitOfLongFalls, PitOfLongFalls.ToInnerChamber);
            portals.Add(ToPharaohsRoad, PharaohsRoad.ToInnerChamber);
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
