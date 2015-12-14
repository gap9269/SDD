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
    class IndoorGarden : MapClass
    {
        static Portal toEasternChamber;

        public static Portal ToEasternChamber { get { return toEasternChamber; } }

        Texture2D foreground, sky, sun;

        public IndoorGarden(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1360;
            mapWidth = 1800;
            mapName = "Indoor Garden";

            mapRec = new Rectangle(0, -300, mapWidth, mapHeight);

            enemyAmount = 4;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\Garden\background"));
            background.Add(Game1.whiteFilter);
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\Garden\foreground");
            sky = content.Load<Texture2D>(@"Maps\History\DryDesert\sky");
            sun = content.Load<Texture2D>(@"Maps\History\Pyramid\Garden\sun");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }

            monsterX = rand.Next(platforms[platformNum].Rec.X - 300, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width);
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);

            SexySaguaro en = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 60;

            en.UpdateRectangles();
            AddEnemyToEnemyList(en);
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount) 
                RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEasternChamber = new Portal(150, platforms[0], "Indoor Garden");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEasternChamber, EasternChamber.ToIndoorGarden);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sun, new Vector2(452, mapRec.Y), Color.White);
            s.End();
        }
    }
}
