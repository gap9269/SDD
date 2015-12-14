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
    class BasementStairs : MapClass
    {
        static Portal toMainChamber;
        static Portal toUndergroundTunnelI;
        public static Portal ToUndergroundTunnelI { get { return toUndergroundTunnelI; } }
        public static Portal ToMainChamber { get { return toMainChamber; } }

        Texture2D foreground;

        public BasementStairs(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Basement Stairs";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Scorpadillo", 0);

            SpikeTrap spikeTrap1 = new SpikeTrap(110, 386, 488, game, 60);
            SpikeTrap spikeTrap2 = new SpikeTrap(110, 896, 488, game, 60);
            SpikeTrap spikeTrap3 = new SpikeTrap(110, 1306, 488, game, 60);

            spikeTrap1.Timer = 105;
            spikeTrap2.Timer = 55;
            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
            mapHazards.Add(spikeTrap3);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\BasementStairs\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\BasementStairs\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();
            EnemyContentLoader.ScorpadilloEnemy(content);

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Scorpadillo"] < enemyAmount)
            {
                Scorpadillo en = new Scorpadillo(pos, "Scorpadillo", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                if (Game1.randomNumberGen.Next(0, 3) == 1)
                    en.Hostile = true;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Scorpadillo"]++;
                    AddEnemyToEnemyList(en);
                }
            }
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

            toMainChamber = new Portal(100, platforms[0], "Basement Stairs");
            toUndergroundTunnelI = new Portal(1610, platforms[0], "Basement Stairs");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainChamber, MainChamber.ToBasementStairs);
            portals.Add(toUndergroundTunnelI, UndergroundTunnelI.ToBasementStairs);
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
