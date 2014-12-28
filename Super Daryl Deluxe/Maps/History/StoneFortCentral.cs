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
    public class StoneFortCentral : MapClass
    {
        static Portal toOutsideCamp;
        static Portal toFortWest;
        static Portal toFortEast;

        TrojanHorse horse;

        public static Portal ToFortEast { get { return toFortEast; } }
        public static Portal ToFortWest { get { return toFortWest; } }
        public static Portal ToOutsideCamp { get { return toOutsideCamp; } }

        BennyBeaker commander;

        GoblinHut hut, hut2, hut1;
        int goblinAmount;
        int timeBeforeSpawn = 160;

        Texture2D foreground, foreground2, sky, doorParallax, wallParallax, leftForeParallax, rightForeParallax, hutSprite;

        public static Dictionary<String, Texture2D> fireTextures;

        struct TorchFire
        {
            public int frame;
            public int frameDelay;
        }

        List<TorchFire> torches;

      
        public StoneFortCentral(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5000;
            mapHeight = 1160;
            mapName = "Stone Fort - Central";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 8;

            yScroll = true;

            hut = new GoblinHut(game, 483, 700, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut);

            hut1 = new GoblinHut(game, 3130, 660, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut1);

            hut2 = new GoblinHut(game, 4222, 630, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut2);

            #region Torches
            torches = new List<TorchFire>();
            TorchFire torch1 = new TorchFire();
            TorchFire torch2 = new TorchFire();
            TorchFire torch3 = new TorchFire();
            TorchFire torch4 = new TorchFire();

            torch1.frameDelay = 3;
            torch2.frameDelay = 5;
            torch3.frameDelay = 2;
            torch4.frameDelay = 4;


            torch2.frame = 1;
            torch3.frame = 2;

            torches.Add(torch1);
            torches.Add(torch2);
            torches.Add(torch3);
            torches.Add(torch4);
            #endregion

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground2");

            sky = content.Load<Texture2D>(@"Maps\History\CentralFort\sky");
            doorParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundDoorParallax");
            wallParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundWallParallax");
            leftForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\leftForegroundParallax");
            rightForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\rightForegroundParallax");

            if (game.ChapterTwo.ChapterTwoBooleans["horseInCentral"])
            {
                horse = new TrojanHorse(2000, -70);
                horse.hasLocker = true;
                horse.LoadContent(content);
            }

            fireTextures = ContentLoader.LoadContent(content, "Maps\\History\\CentralFort\\Fire");

            hutSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\GoblinHut");
            hut.Sprite = hutSprite;
            hut1.Sprite = hutSprite;
            hut2.Sprite = hutSprite;

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("North Hall", backgroundMusic);
            //}

            //if (Chapter.lastMap != "East Hall")
            //{
            //    SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
            //    SoundEffectInstance amb = am.CreateInstance();
            //    amb.IsLooped = true;
            //    Sound.ambience.Add("North Hall", amb);
            //}
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "EastHall")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BennyBeaker(content);
            EnemyContentLoader.Bomblin(content);
            EnemyContentLoader.Goblin(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:

                    int randomEnemy = rand.Next(2);

                    if (randomEnemy == 0)
                    {
                        Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBeforeSpawn;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Goblin"]++;
                        }
                    }
                    else if (randomEnemy == 1)
                    {
                        Bomblin ben = new Bomblin(pos, "Bomblin", game, ref player, this);
                        ben.Hostile = true;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = timeBeforeSpawn;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Bomblin"]++;
                        }
                    }

                    break;
            }
        }


        public void RespawnInitialGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {

                case Game1.ChapterState.chapterTwo:
                    if (enemyNamesAndNumberInMap["Goblin"] < 2)
                    {
                        Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
                        ben.Hostile = false;

                        monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                        ben.Position = new Vector2(monsterX, monsterY);

                        Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                        ben.TimeBeforeSpawn = 0;
                        ben.SpawnWithPoof = false;

                        if (benRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(ben);
                            enemyNamesAndNumberInMap["Goblin"]++;
                        }
                    }

                    break;
            }
        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("North Hall");
        }

        public override void PlayAmbience()
        {
            //Sound.PlayAmbience("North Hall");
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            Chapter.effectsManager.ClearDialogue();

            if (!game.ChapterTwo.ChapterTwoBooleans["clearedCentralFortFirstTime"] && !hut1.Finished && !hut.Finished && !hut2.Finished)
            {
                game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwo.fortRaid.SpecialConditions.Add("Clear the map of enemies and buildings", false);

                Chapter.effectsManager.AddInGameDialogue("The enemy does not know we're here yet. Quickly, clear this area of troops to secure our position!", "Napoleon", "Normal", 400);
            }
        }

        public override void Update()
        {
            base.Update();

            if (horse != null && game.ChapterTwo.ChapterTwoBooleans["horseInCentral"])
                horse.Update();

            #region Torches
            for (int i = 0; i < torches.Count; i++)
            {

                TorchFire temp = torches[i];

                temp.frameDelay--;

                if (temp.frameDelay <= 0)
                {
                    temp.frameDelay = 5;
                    temp.frame++;

                    if (temp.frame >= 3)
                        temp.frame = 0;
                }

                torches[i] = temp;
            }
            #endregion

            //Spawn initial enemies
            if(spawnEnemies)
            {
                RespawnInitialGroundEnemies();

                if (enemiesInMap.Count == 2)
                    spawnEnemies = false;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] && enemiesInMap.Count < enemyAmount && !game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"])
            {
                RespawnGroundEnemies();
            }

            if (game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"])
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["centralCommanderKilled"] && commander == null)
                {
                    enemiesInMap.Clear();
                    commander = new BennyBeaker(new Vector2(3500, platforms[0].RecY - 600), "Benny Beaker", game, ref player, this);
                    enemiesInMap.Add(commander);
                    //TODO: Spawn a bunch of enemies around the commander here
                    //
                    //
                    //
                }

                if (commander != null && commander.Health <= 0 && !game.ChapterTwo.ChapterTwoBooleans["centralCommanderKilled"])
                {
                    game.ChapterTwo.ChapterTwoBooleans["centralCommanderKilled"] = true;

                    Chapter.effectsManager.AddInGameDialogue("We can't wait any longer. Get that horse to the enemy barracks! And keep it safe!", "Napoleon", "Normal", 400);
                    game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                    game.ChapterTwo.fortRaid.SpecialConditions.Add("Protect the horse", false);

                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["centralCommanderKilled"] && game.ChapterTwo.ChapterTwoBooleans["horseInCentral"])
            {
                horse.Move(4);

                if (toFortEast.IsUseable)
                {
                    toFortEast.IsUseable = false;
                    toFortWest.IsUseable = false;
                    toOutsideCamp.IsUseable = false;
                }

                if (horse.RecX > mapWidth)
                {
                    game.ChapterTwo.ChapterTwoBooleans["horseInCentral"] = false;
                    game.ChapterTwo.ChapterTwoBooleans["horseInEast"] = true;

                    StoneFortEast.horse.health = horse.health;

                    toFortEast.IsUseable = true;
                    horse = null;
                }
            }
                

            if (hut.Finished && hut1.Finished && hut2.Finished && enemiesInMap.Count == 0 && game.ChapterTwo.ChapterTwoBooleans["clearedCentralFortFirstTime"] == false)
            {
                game.ChapterTwo.ChapterTwoBooleans["clearedCentralFortFirstTime"] = true;
                Chapter.effectsManager.AddInGameDialogue("Well done. Now work your way through the rest of the camp and destroy the rest of their huts and pyramids.", "Napoleon", "Normal", 400);
                game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                game.ChapterTwo.fortRaid.SpecialConditions.Add("Clear the entire camp of huts and \npyramids", false);
            }

            if (toFortEast.IsUseable == false && game.ChapterTwo.ChapterTwoBooleans["clearedCentralFortFirstTime"] && !game.ChapterTwo.ChapterTwoBooleans["centralCommanderKilled"])
            {
                toFortWest.IsUseable = true;
                ToFortEast.IsUseable = true;
            }
            // PlayBackgroundMusic();
            // PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toOutsideCamp = new Portal(1600, platforms[0], "StoneFort-Central");
            toOutsideCamp.FButtonYOffset = -60;
            toOutsideCamp.PortalNameYOffset = -60;

            toFortWest = new Portal(50, platforms[0], "StoneFort-Central");
            toFortWest.FButtonYOffset = -60;
            toFortWest.PortalNameYOffset = -60;

            toFortEast = new Portal(4800, platforms[0], "StoneFort-Central");
            toFortEast.FButtonYOffset = -60;
            toFortEast.PortalNameYOffset = -60;

            toFortEast.IsUseable = false;
            toFortWest.IsUseable = false;

            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOutsideCamp, OutsideStoneFort.ToCampMiddle);
            portals.Add(toFortWest, StoneFortWest.ToCentral);
            portals.Add(toFortEast, StoneFortEast.ToCentral);

        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(fireTextures.ElementAt(torches[0].frame).Value, new Vector2(143, mapRec.Y + 425), Color.White);
            s.Draw(fireTextures.ElementAt(torches[1].frame).Value, new Vector2(1471, mapRec.Y + 444), Color.White);
            s.Draw(fireTextures.ElementAt(torches[2].frame).Value, new Vector2(2084, mapRec.Y + 410), Color.White);
            s.Draw(fireTextures.ElementAt(torches[3].frame).Value, new Vector2(4311, mapRec.Y + 367), Color.White);

            if(game.ChapterTwo.ChapterTwoBooleans["horseInCentral"] && horse != null)
                horse.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(leftForeParallax, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
            s.Draw(rightForeParallax, new Vector2(mapWidth + 150, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, 800), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.9f, this, game));
            s.Draw(doorParallax, new Vector2(-200, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
            s.Draw(wallParallax, new Vector2(mapWidth - wallParallax.Width - 300, mapRec.Y), Color.White);
            s.End();
        }
    }
}
