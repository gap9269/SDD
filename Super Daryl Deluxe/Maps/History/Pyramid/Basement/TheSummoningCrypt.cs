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
    class TheSummoningCrypt : MapClass
    {
        static Portal toRestingChamber;
        public static Portal ToRestingChamber { get { return toRestingChamber; } }

        static Portal toHallOfTrials;
        public static Portal ToHallOfTrialss { get { return toHallOfTrials; } }

        static Portal toChamberOfCorruption;
        public static Portal ToChamberOfCorruption { get { return toChamberOfCorruption; } }

        Texture2D foreground, tomb, wall;

        CorruptedCoffin corruptedCoffin;
        BreakableObject breakableWall;

        public TheSummoningCrypt(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 4000;
            mapName = "The Summoning Crypt";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 5;
            yScroll = true;
            zoomLevel = .88f;
            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            corruptedCoffin = new CorruptedCoffin(new Vector2(0, mapRec.Y), "Corrupted Coffin", game, ref player, this);

            breakableWall = new BreakableObject(game, 832, mapRec.Y + 450, Game1.whiteFilter, false, 6, 0, 0, false);
            breakableWall.Rec = new Rectangle(832, mapRec.Y + 450, 90, 444);
            breakableWall.VitalRec = breakableWall.Rec;
            interactiveObjects.Add(breakableWall);

            interactiveObjects.Add(new ExplodingFlower(game, 2668, mapRec.Y + 1335, false, 800));
            interactiveObjects.Add(new ExplodingFlower(game, 2268, mapRec.Y + 1335, false, 800));
            interactiveObjects.Add(new ExplodingFlower(game, 1677, mapRec.Y + 1335, false, 800));
            interactiveObjects.Add(new ExplodingFlower(game, 1277, mapRec.Y + 1335, false, 800));

            interactiveObjects.Add(new Barrel(game, 1001, mapRec.Y + 1325 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 1136, mapRec.Y + 1325 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 1268, mapRec.Y + 1325 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 2945, mapRec.Y + 1325 +155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2822, mapRec.Y + 1325 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2699, mapRec.Y + 1325 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .36f, false, Barrel.BarrelType.pyramidUrn));

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\SummoningCrypt\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\SummoningCrypt\foreground");
            tomb = content.Load<Texture2D>(@"Maps\History\Pyramid\SummoningCrypt\tomb");
            wall = content.Load<Texture2D>(@"Maps\History\Pyramid\SummoningCrypt\wall");
            corruptedCoffin.faceTexture = content.Load<Texture2D>(@"Bosses\CorruptedCoffinFace");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
            EnemyContentLoader.CorruptedCoffinBoss(content);
        }

        public override void LeaveMap()
        {
            base.LeaveMap();

            if (game.CurrentChapter.CurrentBoss != null)
            {
                game.CurrentChapter.CurrentBoss = null;
                game.CurrentChapter.BossFight = false;
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (game.Camera.center.Y > 280)
            {
                game.Camera.center.Y = 279;
            }

            if (breakableWall.Health <= 0 && breakableWall.Finished == false)
            {
                Chapter.effectsManager.AddSmokePoof(new Rectangle(breakableWall.Rec.Center.X - 125, breakableWall.Rec.Center.Y - 125, 250, 250), 2);
                breakableWall.Finished = true;
                game.Camera.ShakeCamera(5, 15);
            }

            //If the gate hasn't been destroyed start the boss fight and make the troll, but don't add him to the map yet
            if (game.ChapterTwo.ChapterTwoBooleans["corruptedCoffinDestroyed"] == false)
            {
                if (game.CurrentChapter.CurrentBoss == null)
                {
                    game.CurrentChapter.CurrentBoss = corruptedCoffin;
                    game.CurrentChapter.BossFight = true;
                }
                if (toChamberOfCorruption.IsUseable)
                    toChamberOfCorruption.IsUseable = false;
            }
            else
            {
                if (!toChamberOfCorruption.IsUseable)
                    toChamberOfCorruption.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toRestingChamber = new Portal(3800, platforms[0], "The Summoning Crypt");
            toHallOfTrials = new Portal(50, platforms[0], "The Summoning Crypt", "Pyramid Key");
            toChamberOfCorruption = new Portal(1935, 612, "The Summoning Crypt");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.CurrentChapter.BossFight)
                s.Draw(tomb, new Vector2(0, mapRec.Y), Color.White);

            if (!breakableWall.Finished)
            {
                s.Draw(wall, new Vector2(0, mapRec.Y + 404), Color.White);
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toRestingChamber, RestingChamber.ToTheSummoningCrypt);
            portals.Add(toHallOfTrials, HallofTrials.ToSummoningCrypt);
            portals.Add(toChamberOfCorruption, ChamberOfCorruption.ToSummoningCrypt);
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
        }
    }
}
