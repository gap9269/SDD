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
    class OuterChamber : MapClass
    {
        static Portal toEntrance;
        static Portal toSideChamberI;
        static Portal toMainChamber;

        public static Portal ToMainChamber { get { return toMainChamber; } }
        public static Portal ToSideChamberI { get { return toSideChamberI; } }
        public static Portal ToEntrance { get { return toEntrance; } }

        Texture2D foreground, bars;

        public OuterChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1800;
            mapName = "Outer Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 2;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\OuterChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\OuterChamber\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\OuterChamber\bars");
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

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 2)
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

            if (enemiesInMap.Count < enemyAmount && game.ChapterTwo.ChapterTwoBooleans["outerChamberCleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"])
            {
                RespawnGroundEnemies();

                toEntrance.IsUseable = false;
                toSideChamberI.IsUseable = false;
                toMainChamber.IsUseable = false;
            }
            else if (enemiesInMap.Count == enemyAmount && game.ChapterTwo.ChapterTwoBooleans["outerChamberCleared"] == false && !game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"] = true;
                game.Camera.ShakeCamera(5, 15);
            }
            else if (enemiesInMap.Count == 0 && game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"])
            {
                game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"] = false;
                game.ChapterTwo.ChapterTwoBooleans["outerChamberCleared"] = true;
                game.Camera.ShakeCamera(5, 15);

                toEntrance.IsUseable = true;
                toSideChamberI.IsUseable = true;
                toMainChamber.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEntrance = new Portal(90, platforms[0], "Outer Chamber");
            toSideChamberI = new Portal(845, platforms[0], "Outer Chamber");
            toMainChamber = new Portal(1555, platforms[0], "Outer Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["outerChamberLocked"])
            {
                s.Draw(bars, new Vector2(0, 0), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntrance, PyramidEntrance.ToOuterChamber);
            portals.Add(ToSideChamberI, SideChamberI.ToOuterChamber);
            portals.Add(ToMainChamber, MainChamber.ToOuterChamber);
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
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));;
            s.End();
        }
    }
}
