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
    class SideChamberII : MapClass
    {
        static Portal toSideChamberIII;
        static Portal toSideChamberI;

        public static Portal ToSideChamberI { get { return toSideChamberI; } }
        public static Portal ToSideChamberIII { get { return toSideChamberIII; } }

        Texture2D foreground;

        public SideChamberII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1850;
            mapName = "Side Chamber II";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            SpikeTrap spikeTrap1 = new SpikeTrap(70, 1193, 475, game, 60);
            SpikeTrap spikeTrap2 = new SpikeTrap(70, 1193 - 308, 475, game, 60);
            SpikeTrap spikeTrap3 = new SpikeTrap(70, 1193 - 616, 475, game, 60);
            SpikeTrap spikeTrap4 = new SpikeTrap(70, 1193 - 924, 475, game, 60);

            spikeTrap1.Timer = 65;
            spikeTrap3.Timer = 65;
            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
            mapHazards.Add(spikeTrap3);
            mapHazards.Add(spikeTrap4);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber2\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SideChamber2\foreground");
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

            toSideChamberIII.PortalRecX = 105;
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toSideChamberIII = new Portal(105, platforms[0], "Side Chamber II");
            toSideChamberI = new Portal(1600, platforms[0], "Side Chamber II");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSideChamberI, SideChamberI.ToSideChamberII);
            portals.Add(toSideChamberIII, SideChamberIII.ToSideChamberII);
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
