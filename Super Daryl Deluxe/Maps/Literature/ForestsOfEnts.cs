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
    class ForestOfEnts : MapClass
    {
        static Portal toWelcomeToMiddleEarth;
        static Portal toForestPath;
        public static Portal toRift;

        public static Portal ToWelcomeToMiddleEarth { get { return toWelcomeToMiddleEarth; } }
        public static Portal ToForestPath { get { return toForestPath; } }

        Texture2D sky, parallax, parallaxFar, treeOne, treeTwo, treeThree, treeFull, treePlatform;

        Platform leafOne, leafTwo, leafThree;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;
        public ForestOfEnts(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 4000;
            mapName = "Forest of Ents";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 5;
            zoomLevel = .9f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            mapWithMapQuest = true;

            enemyNames.Add("Tree Ent");
            enemiesToKill.Add(20);
            enemiesKilledForQuest.Add(0);

            MapQuestSign sign = new MapQuestSign(3700, platforms[1].Rec.Y - Game1.mapSign.Height, "Sacrifice Ents to create life", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(850), new Money(7.50f) });
            mapQuestSigns.Add(sign);

            enemyNamesAndNumberInMap.Add("Tree Ent", 0);

            leafOne = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1532,340, 450, 50), true, false, false);
            leafTwo = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2036,137, 350, 50), true, false, false);
            leafThree = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1681,-77, 250, 50), true, false, false);

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\background"));
            sky = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\sky");
            parallax = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\parallax");
            parallaxFar = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\parallaxFar");
            treeOne = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treeOne");
            treeTwo = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treeTwo");
            treeThree = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treeThree");
            treeFull = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treeFull");
            treePlatform = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treePlatform");
            game.NPCSprites["Portal Repair Specialist"] = content.Load<Texture2D>(@"NPC\Main\Portal Repair Specialist");
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Portal Repair Specialist Normal");
            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");

        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Portal Repair Specialist"] = Game1.whiteFilter;
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = Game1.whiteFilter;
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

                en.TimeBeforeSpawn = 60;

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

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }

            if (completedMapQuest && !platforms.Contains(leafOne))
            {
                platforms.Add(leafOne);
                platforms.Add(leafTwo);
                platforms.Add(leafThree);
            }
            ripDelay--;

            if (ripDelay <= 0)
            {
                ripFrame++;
                ripDelay = 1;

                if (ripFrame > 31)
                    ripFrame = 0;
            }

            if (game.CurrentQuests.Contains(game.SideQuestManager.anotherMiddleEarth) && game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"] == false)
                toRift.IsUseable = true;
            else
                toRift.IsUseable = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWelcomeToMiddleEarth = new Portal(3750, platforms[0], "Forest of Ents");
            toForestPath = new Portal(50, platforms[1], "Forest of Ents");
            toRift = new Portal(1650, platforms[3], "Forest of Ents");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(treePlatform, new Vector2(78, mapRec.Y + 1042), Color.White);
            s.Draw(treePlatform, new Vector2(2930, mapRec.Y + 1092), Color.White);

            if (completedMapQuest)
                s.Draw(treeFull, new Vector2(1404, mapRec.Y + 445), Color.White);
            else
            {
                if(enemiesKilledForQuest[0] > enemiesToKill[0] * .66f)
                    s.Draw(treeThree, new Vector2(1404, mapRec.Y + 445), Color.White);
                else if (enemiesKilledForQuest[0] > enemiesToKill[0] * .33f)
                    s.Draw(treeTwo, new Vector2(1404, mapRec.Y + 445), Color.White);
                else
                    s.Draw(treeOne, new Vector2(1404, mapRec.Y + 445), Color.White);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"] == false)
                s.Draw(rip.ElementAt(ripFrame).Value, new Vector2(1365, mapRec.Y -50), Color.White);
            else
                s.Draw(rip["dimensional rip healed"], new Vector2(1365, mapRec.Y - 50), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWelcomeToMiddleEarth, WelcomeToMiddleEarth.ToForestOfEnts);
            portals.Add(toForestPath, ForestPath.ToForestOfEnts);
            portals.Add(toRift, ForestOfEntsRift.ToForestOfEnts);
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
null, null, null, null, Game1.camera.GetTransform(.18f, this, game));
            s.Draw(parallaxFar, new Rectangle(0, mapRec.Y, parallaxFar.Width, parallaxFar.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(parallax, new Rectangle(0, mapRec.Y, parallax.Width, parallax.Height), Color.White);
            s.End();
        }
    }
}
