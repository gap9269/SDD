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
    class TheGreatPyramid : MapClass
    {
        static Portal toCursedSands;
        static Portal toEntrance;
        public static Portal ToEntrance { get { return toEntrance; } }
        public static Portal ToCursedSands { get { return toCursedSands; } }

        Texture2D foreground, sky, parallax, sun;

        public TheGreatPyramid(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "The Great Pyramid";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 2;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\TheGreatPyramid\background"));
            sky = content.Load<Texture2D>(@"Maps\History\DryDesert\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\TheGreatPyramid\parallax");
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

            AnubisWarrior en = new AnubisWarrior(new Vector2(2800, platforms[0].Rec.Y - 396 - 10), "Anubis Warrior", game, ref player, this);
            en.SpawnWithPoof = false;
            en.FacingRight = false;

            en.UpdateRectangles();
            AddEnemyToEnemyList(en);

            AnubisWarrior en2 = new AnubisWarrior(new Vector2(3200, platforms[0].Rec.Y - 396 - 10), "Anubis Warrior", game, ref player, this);
            en2.SpawnWithPoof = false;
            en2.FacingRight = false;

            en2.UpdateRectangles();
            AddEnemyToEnemyList(en2);

        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
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
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCursedSands = new Portal(50, platforms[0], "The Great Pyramid");
            toEntrance = new Portal(3023, platforms[0], "The Great Pyramid");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCursedSands, CursedSands.ToPyramid);
            portals.Add(toEntrance, PyramidEntrance.ToGreatPyramid);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

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
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallax, new Vector2(0, 0), Color.White);
            s.End();
        }
    }
}
