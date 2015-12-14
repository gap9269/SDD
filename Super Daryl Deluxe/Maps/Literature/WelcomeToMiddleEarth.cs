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
    class WelcomeToMiddleEarth : MapClass
    {
        static Portal toTallTaleTerrace;
        static Portal toForestOfEnts;

        public static Portal ToTallTaleTerrace { get { return toTallTaleTerrace; } }
        public static Portal ToForestOfEnts { get { return toForestOfEnts; } }

        Texture2D sky, parallaxFar, parallax;

        public WelcomeToMiddleEarth(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1360;
            mapWidth = 5556;
            mapName = "Welcome to Middle Earth";

            mapRec = new Rectangle(0, -410, mapWidth, mapHeight);
            enemyAmount = 7;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Tree Ent", 0);

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\WelcomeToMiddleEarth\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Literature\WelcomeToMiddleEarth\background2"));
            sky = content.Load<Texture2D>(@"Maps\Literature\WelcomeToMiddleEarth\sky");
            parallax = content.Load<Texture2D>(@"Maps\Literature\WelcomeToMiddleEarth\parallax");
            parallaxFar = content.Load<Texture2D>(@"Maps\Literature\WelcomeToMiddleEarth\parallaxFar");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.TreeEntEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Tree Ent"] < enemyAmount)
            {
                TreeEnt en = new TreeEnt(pos, "Tree Ent", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 160;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Tree Ent"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemyAmount > enemiesInMap.Count)
                RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTallTaleTerrace = new Portal(5100, platforms[0], "Welcome to Middle Earth");
            toForestOfEnts = new Portal(300, platforms[0], "Welcome to Middle Earth");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTallTaleTerrace, TallTaleTerrace.ToWelcomeToMiddleEarth);
            portals.Add(toForestOfEnts, ForestOfEnts.ToWelcomeToMiddleEarth);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
           // s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.9f, this, game));
            s.Draw(parallaxFar, new Rectangle(mapWidth - parallaxFar.Width, mapRec.Y, parallaxFar.Width, parallaxFar.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.45f, this, game));
            s.Draw(parallax, new Rectangle(mapWidth - parallax.Width - 2500, mapRec.Y + 70, parallax.Width, parallax.Height), Color.White);
            s.End();
        }
    }
}
