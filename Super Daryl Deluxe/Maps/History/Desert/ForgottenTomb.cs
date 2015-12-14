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
    class ForgottenTomb : MapClass
    {
        static Portal toWindyDesert;
        static Portal toTombOfCactusKing;

        public static Portal ToTombOfCactusKing { get { return toTombOfCactusKing; } }
        public static Portal ToWindyDesert { get { return toWindyDesert; } }

        Texture2D foreground;

        public ForgottenTomb(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2400;
            mapWidth = 5500;
            mapName = "Forgotten Tomb";

            zoomLevel = .95f;

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 4;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\ForgottenTomb\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\ForgottenTomb\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\ForgottenTomb\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.ScorpadilloEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Scorpadillo en = new Scorpadillo(pos, "Scorpadillo", game, ref player, this);
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
                AddEnemyToEnemyList(en);
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();
            }

            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWindyDesert = new Portal(50, platforms[0], "Forgotten Tomb");
            toTombOfCactusKing = new Portal(1000, -100, "Forgotten Tomb");
            toTombOfCactusKing.PortalRecY = -50;

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWindyDesert, WindyDesert.ToTomb);
           // portals.Add(toTombOfCactusKing, CentralSands.ToWindyDesert);
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
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));

            s.End();
        }
    }
}
