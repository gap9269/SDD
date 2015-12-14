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
    class ForestOfEntsRift : MapClass
    {
        static Portal toForestOfEnts;

        public static Portal ToForestOfEnts { get { return toForestOfEnts; } }

        Texture2D sky, tree, parallax, wall, backgroundHole, foreground;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;
        double timeToComplete = 0;
        int timer;

        WallSwitch switch1, switch2, switch3, switch4;

        Boolean wallBroken = false;
        Boolean spawnAnubisEnemies = false;

        Platform tree1, tree2, tree3, wall1;
        ExplodingFlower flower;
        public ForestOfEntsRift(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 6000;
            mapName = "Forest of Ents - Rift";
            mapHeight = 720;
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 8;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Tree Ent", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            switch1 = new WallSwitch(Game1.switchTexture, new Rectangle(800, 270, 333, 335));
            switches.Add(switch1);

            switch2 = new WallSwitch(Game1.switchTexture, new Rectangle(2050, 270, 333, 335));
            switches.Add(switch2);

            switch3 = new WallSwitch(Game1.switchTexture, new Rectangle(3260, 270, 333, 335));
            switches.Add(switch3);

            switch4 = new WallSwitch(Game1.switchTexture, new Rectangle(4088, 270, 333, 335));
            switches.Add(switch4);

            tree1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1097, 12, 50, 650), false, false, false);
            platforms.Add(tree1);

            wall1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4467, -6, 50, 650), false, false, false);
            platforms.Add(wall1);

            tree3 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3612, -9, 50, 650), false, false, false);
            platforms.Add(tree3);

            tree2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2356, 30, 50, 650), false, false, false);
            platforms.Add(tree2);

            flower = new ExplodingFlower(game, 4160, 497, false, 300);
            interactiveObjects.Add(flower);
        }

        public override void RespawnGroundEnemies()
        {
            int platformNum;

               platformNum = 0;

            if (enemyNamesAndNumberInMap["Tree Ent"] < 8)
            {

                if(enemyNamesAndNumberInMap["Tree Ent"] < 2)
                    monsterX = 1200;
                else if (enemyNamesAndNumberInMap["Tree Ent"] < 5)
                    monsterX = 2400;
                else if (enemyNamesAndNumberInMap["Tree Ent"] < 8)
                    monsterX = 3600;

                TreeEnt erl = new TreeEnt(pos, "Tree Ent", game, ref player, this);
                monsterY = platforms[0].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                AddEnemyToEnemyList(erl);
                enemyNamesAndNumberInMap["Tree Ent"]++;
                
            }
        }

        public void SpawnAnubis()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < 4)
            {

                monsterX = rand.Next(4700, 5900);
                monsterY = 0;
                pos = new Vector2(monsterX, monsterY);

                AnubisWarrior erl = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[0].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                erl.FacingRight = false;

                AddEnemyToEnemyList(erl);
                enemyNamesAndNumberInMap["Anubis Warrior"]++;

            }
            else
                spawnAnubisEnemies = false;

        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ForestOfEntsRift\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ForestOfEntsRift\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\CentralSandsRift\sky");
            foreground = content.Load<Texture2D>(@"Maps\Literature\ForestOfEntsRift\foreground");
            parallax = content.Load<Texture2D>(@"Maps\Literature\ForestOfEntsRift\parallax");
            wall = content.Load<Texture2D>(@"Maps\History\Pyramid\InnerChamber\Wall\wall0");
            backgroundHole = content.Load<Texture2D>(@"Maps\Literature\ForestOfEntsRift\background2Hole");
            tree = content.Load<Texture2D>(@"Maps\Literature\Forest of Ents\treeThree");

            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
            EnemyContentLoader.TreeEntEnemy(content);
        }


        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            ResetEnemyNamesAndNumberInMap();

            enemiesInMap.Clear();

            game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftStarted"] = false;

            if (game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"] == false)
                spawnEnemies = true;
            else
                spawnEnemies = false;

            foreach (Switch s in switches)
            {
                s.Active = false;
            }

            wallBroken = false;
            spawnAnubisEnemies = false;

            if (!platforms.Contains(tree1))
            {
                tree1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1097, 12, 50, 650), false, false, false);
                platforms.Add(tree1);
            }

            if (!platforms.Contains(wall1))
            {
                wall1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4467, -6, 50, 650), false, false, false);
                platforms.Add(wall1);
            }
            if (!platforms.Contains(tree3))
            {
                tree3 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3612, -9, 50, 650), false, false, false);
                platforms.Add(tree3);
            }
            if (!platforms.Contains(tree2))
            {
                tree2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2356, 30, 50, 650), false, false, false);
                platforms.Add(tree2);
            }

            flower.RecX = 4160;
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"])
            {

                if (flower.flowerState == ExplodingFlower.FlowerState.dead && !wallBroken)
                {
                    wallBroken = true;
                    game.Camera.ShakeCamera(10, 25);
                }

                if (!switch1.Active)
                    CheckSwitch(switch1);
                else if (platforms.Contains(tree1))
                    platforms.Remove(tree1);

                if (!switch2.Active)
                {
                    if (enemiesInMap.Count <= 6)
                        CheckSwitch(switch2);
                }
                else if (platforms.Contains(tree2))
                    platforms.Remove(tree2);

                if (!switch3.Active)
                {
                    if (enemiesInMap.Count <= 3)
                        CheckSwitch(switch3);
                }
                else if (platforms.Contains(tree3))
                    platforms.Remove(tree3);

                if (wallBroken)
                {
                    switch4.RecY = 270;

                    if (!switch4.Active)
                    {
                        if (enemiesInMap.Count == 0)
                            CheckSwitch(switch4);
                    }
                    else if (platforms.Contains(wall1))
                    {
                        platforms.Remove(wall1);
                        spawnAnubisEnemies = true;
                    }
                }
                else
                    switch4.RecY = -1000;

                if (!platforms.Contains(wall1) && spawnAnubisEnemies)
                {
                    SpawnAnubis();
                }

                if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                {
                    RespawnGroundEnemies();
                }

                if (enemiesInMap.Count >= enemyAmount && game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftStarted"] == false)
                {
                    spawnEnemies = false;
                    game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftStarted"] = true;
                    toForestOfEnts.IsUseable = false;
                    game.Camera.ShakeCamera(5, 15);
                    timeToComplete = 120.00;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.AddTimer(timeToComplete);
                }

                if (game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftStarted"] == true)
                {
                    if (timer < 60)
                        timer++;

                    if (timer >= 60)
                    {
                        timeToComplete -= 1;
                        timer = 0;
                    }

                    if (timeToComplete <= 0 && toForestOfEnts.IsUseable == false)
                    {
                        toForestOfEnts.IsUseable = true;
                        game.Camera.ShakeCamera(5, 15);
                        ForceToNewMap(toForestOfEnts, ForestOfEnts.toRift);
                    }

                    if (!enemiesInMap.OfType<AnubisWarrior>().Any() && timeToComplete > 0 && !platforms.Contains(wall1) && !spawnAnubisEnemies)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["forestOfEntsRiftCompleted"] = true;
                        toForestOfEnts.IsUseable = true;
                        game.Camera.ShakeCamera(5, 15);
                        timeToComplete = 0;
                        Chapter.effectsManager.AddTimer(0);
                    }
                }
            }

            ripDelay--;

            if (ripDelay <= 0)
            {
                ripFrame++;
                ripDelay = 1;

                if (ripFrame > 31)
                    ripFrame = 0;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toForestOfEnts = new Portal(250, platforms[0], "Forest of Ents - Rift");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toForestOfEnts, ForestOfEnts.toRift);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (wallBroken && background[1] != backgroundHole)
            {
                flower.RecX = 3800;
                background[1] = backgroundHole;
            }

            s.Draw(rip.ElementAt(ripFrame).Value, new Rectangle(-80, 100, rip.ElementAt(ripFrame).Value.Width, rip.ElementAt(ripFrame).Value.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            if (!switch1.Active)
                s.Draw(tree, new Vector2(522, -175), Color.White);
            if (!switch2.Active)
                s.Draw(tree, new Vector2(1794, -175), Color.White);
            if (!switch3.Active)
                s.Draw(tree, new Vector2(3027, -175), Color.White);
            if (!switch4.Active)
                s.Draw(wall, new Vector2(4438, 2), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (!switch4.Active)
                s.Draw(foreground, new Vector2(mapWidth - foreground.Width, 0), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(sky, new Vector2(-120, -401), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.5f, this, game));
            s.Draw(parallax, new Vector2(0, 0), Color.White);

            s.End();
        }
    }
}
