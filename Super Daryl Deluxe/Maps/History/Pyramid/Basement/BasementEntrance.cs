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
    class BasementEntrance : MapClass
    {
        static Portal toPyramidChute;
        public static Portal ToPyramidChute { get { return toPyramidChute; } }

        static Portal toDarkenedChamber;
        public static Portal ToDarkenedChamber { get { return toDarkenedChamber; } }

        Texture2D foreground;

        public BasementEntrance(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Basement Entrance";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new Barrel(game, 579, 481 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 416, 481 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 1474, 483 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .36f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1573, 483 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\BasementEntrance\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\BasementEntrance\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
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

            toPyramidChute.PortalRecX = 1980;
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toPyramidChute = new Portal(2100, platforms[0], "Basement Entrance");
            toDarkenedChamber = new Portal(50, platforms[0], "Basement Entrance");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPyramidChute, PyramidChute.ToBasementEntrance);
            portals.Add(toDarkenedChamber, DarkenedChamber.ToBasementEntrance);
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
