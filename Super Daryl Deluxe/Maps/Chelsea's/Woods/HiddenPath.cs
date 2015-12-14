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
    class HiddenPath : MapClass
    {
        static Portal toCrossroads;
        static Portal toDeerShack;

        public static Portal ToDeerShack { get { return toDeerShack; } }
        public static Portal ToCrossroads { get { return toCrossroads; } }

        public HiddenPath(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Hidden Path";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 1;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            MaggieMushroom en = new MaggieMushroom(pos, "Maggie Mushroom", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 10;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                AddEnemyToEnemyList(en);
            }

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\Dirty Path"));
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Maggie Mushroom", content.Load<Texture2D>(@"EnemySprites\MaggieMushroom"));
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

            toCrossroads = new Portal(100, platforms[0], "Hidden Path");
            toDeerShack = new Portal(1600, platforms[0], "Hidden Path");//, "Gold Key");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCrossroads, Crossroads.ToPathFour);
            portals.Add(toDeerShack, DeerShack.ToHiddenPath);
        }
    }
}
