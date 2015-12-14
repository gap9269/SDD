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
    class PharaohsGate : MapClass
    {
        static Portal toHiddenPassage;
        static Portal toCenterOfPyramid;
        public static Portal ToCenterOfPyramid { get { return toCenterOfPyramid; } }
        public static Portal ToHiddenPassage { get { return toHiddenPassage; } }

        Texture2D foreground;

        AnubisCommander commander;

        public PharaohsGate(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Pharaoh's Gate";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            interactiveObjects.Add(new Barrel(game, 963, 434 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .36f, false, Barrel.BarrelType.pyramidPitcher));
            interactiveObjects.Add(new Barrel(game, 702, 425 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 1388, 426 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 1634, 426 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 2052, 444 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .06f, false, Barrel.BarrelType.pyramidPitcher));
            interactiveObjects.Add(new Barrel(game, 2312, 444 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2704, 444 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 2976, 444 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .06f, false, Barrel.BarrelType.pyramidUrn));

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\RoomOfRedundancy\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\RoomOfRedundancy\foreground");
            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Chained Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.AnubisCommanderEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
            EnemyContentLoader.LocustEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 2)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 200;
                en.Hostile = true;
                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Anubis Warrior"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["aspRoomUnlocked"])
            {
                if (toCenterOfPyramid.IsUseable)
                {
                    commander = new AnubisCommander(new Vector2(), "Commander Anubis", game, ref player, this, new Rectangle(100, 100, mapWidth - 200, 400));
                    commander.PositionY = platforms[0].RecY - commander.Rec.Height - 10;
                    commander.PositionX = 3000;
                    commander.FacingRight = false;
                    commander.Hostile = true;
                    toCenterOfPyramid.IsUseable = false;
                    enemiesInMap.Add(commander);
                }

                RespawnGroundEnemies();
            }

            toCenterOfPyramid.PortalRecX = 3600;
            if (commander.Health <= 0 && !game.ChapterTwo.ChapterTwoBooleans["aspRoomUnlocked"])
            {
                foreach (Enemy e in enemiesInMap)
                {
                    e.Health = 0;
                }

                game.ChapterTwo.ChapterTwoBooleans["aspRoomUnlocked"] = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["aspRoomUnlocked"] && !toCenterOfPyramid.IsUseable)
            {
                toCenterOfPyramid.IsUseable = true;
                game.ChapterTwo.NPCs["Pyramid Guard 8"].canTalk = true;
                game.ChapterTwo.NPCs["Pyramid Guard 9"].canTalk = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toHiddenPassage = new Portal(50, platforms[0], "Pharaoh's Gate");
            toCenterOfPyramid = new Portal(3800, platforms[0], "Pharaoh's Gate");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHiddenPassage, HiddenPassage.ToRoomOfRedundancy);
            portals.Add(toCenterOfPyramid, CenterOfThePyramid.toChamberOfRedundancy);
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
