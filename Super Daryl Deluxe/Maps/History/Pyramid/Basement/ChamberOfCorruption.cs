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
    class ChamberOfCorruption : MapClass
    {
        static Portal toTunnelOfCertainDeath;
        public static Portal ToTunnelOfCertainDeath { get { return toTunnelOfCertainDeath; } }

        static Portal toSummoningCrypt;
        public static Portal ToSummoningCrypt { get { return toSummoningCrypt; } }

        static Portal toButterflyChamber;
        public static Portal ToButterflyChamber { get { return toButterflyChamber; } }

        Texture2D foreground, rock;
        Rectangle rockRec;
        public ChamberOfCorruption(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2800;
            mapName = "Chamber of Corruption";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            rockRec = new Rectangle(2318, 200, 350, 410);

            interactiveObjects.Add(new Barrel(game, 1613, 437 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 728, 441 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 608, 441 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfCorruption\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfCorruption\foreground");
            rock = content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfCorruption\rock");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.VileMummyEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            VileMummy en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 120;

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

        public override void Update()
        {
            base.Update();

            if(enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();

            if (!game.ChapterTwo.ChapterTwoBooleans["chamberOfCorruptionRockDestroyed"] && toButterflyChamber.IsUseable)
                toButterflyChamber.IsUseable = false;
            else if (game.ChapterTwo.ChapterTwoBooleans["chamberOfCorruptionRockDestroyed"] && toButterflyChamber.IsUseable == false)
                toButterflyChamber.IsUseable = true;

            if (enemiesInMap.Count > 0 && (enemiesInMap[0] as VileMummy).exploding && (enemiesInMap[0] as VileMummy).explosionRec.Intersects(rockRec) && !game.ChapterTwo.ChapterTwoBooleans["chamberOfCorruptionRockDestroyed"])
            {
                game.ChapterTwo.ChapterTwoBooleans["chamberOfCorruptionRockDestroyed"] = true;
                game.Camera.ShakeCamera(15, 25);
                Chapter.effectsManager.AddSmokePoof(rockRec, 3);
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();
            toTunnelOfCertainDeath = new Portal(120, platforms[0], "Chamber of Corruption");
            toButterflyChamber = new Portal(2415, platforms[0], "Chamber of Corruption");
            toSummoningCrypt = new Portal(1340, platforms[0], "Chamber of Corruption");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(!game.ChapterTwo.ChapterTwoBooleans["chamberOfCorruptionRockDestroyed"])
                s.Draw(rock, new Vector2(2285, 0), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTunnelOfCertainDeath, TunnelOfCertainDeath.ToChamberOfCorruption);
            portals.Add(toSummoningCrypt, TheSummoningCrypt.ToChamberOfCorruption);
            portals.Add(toButterflyChamber, ButterflyChamber.ToChamberOfCorruption);
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
