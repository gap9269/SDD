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
    class DryDesert : MapClass
    {
        static Portal toMongols;
        static Portal toWindyDesert;

        public static Portal ToWindyDesert { get { return toWindyDesert; } }
        public static Portal ToMongols { get { return toMongols; } }

        Texture2D foreground, sky, parallax, sun;

        public DryDesert(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Dry Desert";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\DryDesert\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\DryDesert\foreground");
            sky = content.Load<Texture2D>(@"Maps\History\DryDesert\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\DryDesert\parallax");
            sun = content.Load<Texture2D>(@"Maps\History\DryDesert\sun");
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

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnFlyingEnemies(new Rectangle(100, 100, mapWidth - 100, 350));
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMongols = new Portal(50, platforms[0], "Dry Desert");
            toWindyDesert = new Portal(3800, platforms[0], "Dry Desert");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMongols, MongolCamp.ToDesert);
            portals.Add(toWindyDesert, WindyDesert.ToDryDesert);
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
            s.Draw(sky, new Vector2(0, 0), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(sun, new Vector2(-200, 0), Color.White);
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallax, new Vector2(00, 0), Color.White);

            s.End();
        }
    }
}
