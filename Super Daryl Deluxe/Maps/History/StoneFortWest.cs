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
    public class StoneFortWest : MapClass
    {
        static Portal toCentral;
        static Portal toEast;

        public static Portal ToCentral { get { return toCentral; } }
        public static Portal ToEast { get { return toEast; } }

        GoblinHut hut, hut2, hut1, hut3, hut4, hut5;
        int goblinAmount;

        int timeBeforeSpawn = 160;

        BennyBeaker commander;

        Texture2D foreground, foreground2, sky, doorParallax, wallParallax, leftForeParallax, rightForeParallax, hutSprite;

        public static Dictionary<String, Texture2D> fireTextures;
        public static TrojanHorse horse;

        struct TorchFire
        {
            public int frame;
            public int frameDelay;
        }

        List<TorchFire> torches;


        public StoneFortWest(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 8000;
            mapHeight = 1160;
            mapName = "Stone Fort - West";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 10;

            yScroll = true;
            horse = new TrojanHorse(0, -70);
            horse.hasLocker = true;

            hut = new GoblinHut(game, 483, 700, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut);

            hut1 = new GoblinHut(game, 3130, 660, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut1);

            hut2 = new GoblinHut(game, 4222, 630, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut2);

            hut3 = new GoblinHut(game, 5000, 660, Game1.whiteFilter, 10, 5, true);
            interactiveObjects.Add(hut3);

            hut4 = new GoblinHut(game, 6000, 630, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut4);

            hut5 = new GoblinHut(game, 7000, 630, Game1.whiteFilter, 10, 5, false);
            interactiveObjects.Add(hut5);

            //#region Torches
            //torches = new List<TorchFire>();
            //TorchFire torch1 = new TorchFire();
            //TorchFire torch2 = new TorchFire();
            //TorchFire torch3 = new TorchFire();
            //TorchFire torch4 = new TorchFire();

            //torch1.frameDelay = 3;
            //torch2.frameDelay = 5;
            //torch3.frameDelay = 2;
            //torch4.frameDelay = 4;


            //torch2.frame = 1;
            //torch3.frame = 2;

            //torches.Add(torch1);
            //torches.Add(torch2);
            //torches.Add(torch3);
            //torches.Add(torch4);
            //#endregion

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\WestFort\background"));
           // background.Add(content.Load<Texture2D>(@"Maps\History\CentralFort\background2"));
            //foreground = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground");
            //foreground2 = content.Load<Texture2D>(@"Maps\History\CentralFort\foreground2");

            //sky = content.Load<Texture2D>(@"Maps\History\CentralFort\sky");
            //doorParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundDoorParallax");
            //wallParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\backgroundWallParallax");
            //leftForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\leftForegroundParallax");
            //rightForeParallax = content.Load<Texture2D>(@"Maps\History\CentralFort\rightForegroundParallax");

            //fireTextures = ContentLoader.LoadContent(content, "Maps\\History\\CentralFort\\Fire");

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
            {
                horse.LoadContent(content);
            }

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            hutSprite = content.Load<Texture2D>(@"Maps\History\OutsideFort\GoblinHut");
            hut.Sprite = hutSprite;
            hut1.Sprite = hutSprite;
            hut2.Sprite = hutSprite;
            hut3.Sprite = hutSprite;
            hut4.Sprite = hutSprite;
            hut5.Sprite = hutSprite;

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
                    if (enemyNamesAndNumberInMap["Goblin"] < 4)
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

        public override void Update()
        {
            base.Update();

            //Spawn initial enemies
            if (spawnEnemies)
            {
                RespawnInitialGroundEnemies();

                if (enemiesInMap.Count == 4)
                    spawnEnemies = false;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"])
            {
                horse.Update();
                if (toCentral.IsUseable)
                {
                    toCentral.IsUseable = false;
                    toEast.IsUseable = false;
                }

                if (horse.RecX < 4000)
                {
                    horse.Move(4);
                }
                else
                {
                    Chapter.effectsManager.AddInGameDialogue("We made it. Now you mus-- What is THAT?!", "Napoleon", "Normal", 150);
                    Chapter.effectsManager.AddInGameDialogue("Incoming! Take that monster down, soldier!", "Napoleon", "Normal", 150);
                    game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                    game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the Troll", false);
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] && enemiesInMap.Count < enemyAmount && (!game.ChapterTwo.ChapterTwoBooleans["westCommanderSpawned"] || game.ChapterTwo.ChapterTwoBooleans["horseInWest"]))
            {
                RespawnGroundEnemies();
            }
            if (game.ChapterTwo.ChapterTwoBooleans["westCommanderSpawned"])
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"] && commander == null)
                {
                    enemiesInMap.Clear();
                    commander = new BennyBeaker(new Vector2(3500, platforms[0].RecY - 600), "Benny Beaker", game, ref player, this);
                    enemiesInMap.Add(commander);
                    //TODO: Spawn a bunch of enemies around the commander here
                    //
                    //
                    //
                }

                if (commander != null && commander.Health <= 0 && !game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"])
                {
                    game.ChapterTwo.ChapterTwoBooleans["westCommanderKilled"] = true;

                    if (game.ChapterTwo.ChapterTwoBooleans["eastCommanderKilled"] && !game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"])
                    {
                        Chapter.effectsManager.AddInGameDialogue("The enemy is attacking the cargo! Get back there and protect it!", "Napoleon", "Normal", 400);
                        game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwo.fortRaid.SpecialConditions.Add("Protect the horse in the \nCentral area", false);
                        game.ChapterTwo.ChapterTwoBooleans["centralCommanderSpawned"] = true;
                    }
                    else
                    {
                        Chapter.effectsManager.AddInGameDialogue("Another commander has showned up in the eastern part of camp. Take him out before he can gather too many enemy troops.", "Napoleon", "Normal", 400);
                        game.ChapterTwo.ChapterTwoBooleans["eastCommanderSpawned"] = true;
                        game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                        game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the enemy commander in the \nEastern area", false);
                    }
                }
            }
            else
            {
                if (game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false || game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] == false)
                {
                    int hutsDestroyed = 0;

                    for (int i = 0; i < interactiveObjects.Count; i++)
                    {
                        if (interactiveObjects[i].Finished)
                            hutsDestroyed++;

                    }

                    if (hutsDestroyed >= 4 && game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] == false)
                    {
                        Chapter.effectsManager.AddInGameDialogue("They've sounded the alarms! Prepare for enemy reinforcements!", "Napoleon", "Normal", 300);
                        game.ChapterTwo.ChapterTwoBooleans["enemyReinforcementsSpawning"] = true;
                    }

                    if (hutsDestroyed == interactiveObjects.Count && game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] == false)
                    {
                        if (game.ChapterTwo.ChapterTwoBooleans["clearedEastHuts"])
                        {
                            Chapter.effectsManager.AddInGameDialogue("Excellent job, soldier! Your work isn't done though, an enemy commander is rallying troops in the East. Shut him down!", "Napoleon", "Normal", 400);
                            game.ChapterTwo.ChapterTwoBooleans["eastCommanderSpawned"] = true;

                            game.ChapterTwo.fortRaid.SpecialConditions.Clear();
                            game.ChapterTwo.fortRaid.SpecialConditions.Add("Kill the enemy commander in the \nEastern area", false);
                        }
                        else
                            Chapter.effectsManager.AddInGameDialogue("That area is clear! Move on to the eastern side!", "Napoleon", "Normal", 300);

                        game.ChapterTwo.ChapterTwoBooleans["clearedWestHuts"] = true;
                    }

                }
            }

            //for (int i = 0; i < torches.Count; i++)
            //{

            //    TorchFire temp = torches[i];

            //    temp.frameDelay--;

            //    if (temp.frameDelay <= 0)
            //    {
            //        temp.frameDelay = 5;
            //        temp.frame++;

            //        if (temp.frame >= 3)
            //            temp.frame = 0;
            //    }

            //    torches[i] = temp;
            //}

            // PlayBackgroundMusic();
            // PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentral = new Portal(7800, platforms[0], "StoneFort-West");
            toCentral.FButtonYOffset = -60;
            toCentral.PortalNameYOffset = -60;

            toEast = new Portal(50, platforms[0], "StoneFort-West");
            toEast.FButtonYOffset = -60;
            toEast.PortalNameYOffset = -60;
            //toBathroom = new Portal(500, platforms[0], "MainLobby");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentral, StoneFortCentral.ToFortWest);
            portals.Add(ToEast, StoneFortEast.ToWest);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            //s.Draw(fireTextures.ElementAt(torches[0].frame).Value, new Vector2(143, mapRec.Y + 425), Color.White);
            //s.Draw(fireTextures.ElementAt(torches[1].frame).Value, new Vector2(1471, mapRec.Y + 444), Color.White);
            //s.Draw(fireTextures.ElementAt(torches[2].frame).Value, new Vector2(2084, mapRec.Y + 410), Color.White);
            //s.Draw(fireTextures.ElementAt(torches[3].frame).Value, new Vector2(4311, mapRec.Y + 367), Color.White);

            if (game.ChapterTwo.ChapterTwoBooleans["horseInWest"] && horse != null)
                horse.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.Transform);
//            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
//            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
//            s.Draw(leftForeParallax, new Vector2(0, mapRec.Y), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransform(1.23f, this, game));
//            s.Draw(rightForeParallax, new Vector2(mapWidth + 150, mapRec.Y), Color.White);

//            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransform(1f, this, game));
//            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, 800), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransform(.9f, this, game));
//            s.Draw(doorParallax, new Vector2(-200, mapRec.Y), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransform(.85f, this, game));
//            s.Draw(wallParallax, new Vector2(mapWidth - wallParallax.Width - 300, mapRec.Y), Color.White);
//            s.End();
        }
    }
}
