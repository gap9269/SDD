﻿using System;
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
    public class OutsideStoneFort : MapClass
    {
        static Portal toCampMiddle;
        static Portal toBathroom;

        public static Portal ToCampMiddle { get { return toCampMiddle; } }
        public static Portal ToBathroom { get { return toBathroom; } }

        public static TrojanHorse horse;

        WallSwitch doorSwitch;

        Platform bridge;
        GoblinHut hut;

        Texture2D foreground, foreground2, sky, sky2, doorFallen, mountains, castleParallax, outhouse;
        public static Texture2D door;
        public static Dictionary<String, Texture2D> doorFalling;

        LivingLocker locker;

        public OutsideStoneFort(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            zoomLevel = .8f;

            mapWidth = 5000;
            mapHeight = 1750;
            mapName = "Outside Stone Fort";

            //backgroundMusicName = "Noir Halls";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            bridge = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3380, 737, 2000, 50), false, false, false);
            horse = new TrojanHorse(750, -60);

            yScroll = true;
            
            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(4426, -640, 333, 335));
            switches.Add(doorSwitch);

            locker = new LivingLocker(game, new Rectangle(1850, 200, 500, 400));
            interactiveObjects.Add(locker);

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\OutsideFort\background1"));
            background.Add(content.Load<Texture2D>(@"Maps\History\OutsideFort\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\OutsideFort\foreground1");
            foreground2 = content.Load<Texture2D>(@"Maps\History\OutsideFort\foreground2");
            castleParallax = content.Load<Texture2D>(@"Maps\History\OutsideFort\castleParallax");
            mountains = content.Load<Texture2D>(@"Maps\History\OutsideFort\mountains");
            sky = content.Load<Texture2D>(@"Maps\History\OutsideFort\sky");
            sky2 = content.Load<Texture2D>(@"Maps\History\OutsideFort\sky2");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            doorFallen = content.Load<Texture2D>(@"Maps\History\OutsideFort\fallenDoor");

            if (game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"] == false)
            {
                horse.LoadContent(content);
                doorFalling = ContentLoader.LoadContent(content, "Maps\\History\\OutsideFort\\DoorFall");
                door = content.Load<Texture2D>(@"Maps\History\OutsideFort\door");
            }

            game.NPCSprites["Napoleon"] = content.Load<Texture2D>(@"NPC\History\Napoleon");
            game.NPCSprites["Cleopatra"] = content.Load<Texture2D>(@"NPC\History\Cleopatra");

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");
            Game1.npcFaces["Cleopatra"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\CleopatraNormal");

            game.NPCSprites["Bob the Construction Guy"] = content.Load<Texture2D>(@"NPC\Party\ConstructionBob");
            Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Bob");

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

             game.NPCSprites["Napoleon"] = Game1.whiteFilter;
             game.NPCSprites["Cleopatra"] = Game1.whiteFilter;
             game.NPCSprites["Bob the Construction Guy"] = Game1.whiteFilter;

             Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = Game1.whiteFilter;
             Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;
             Game1.npcFaces["Cleopatra"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "EastHall")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.Bomblin(content);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            if (!game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"] && game.ChapterTwo.ChapterTwoBooleans["spawnedOutsideCampGuards"] && enemiesInMap.Count == 0)
            {
                game.ChapterTwo.ChapterTwoBooleans["spawnedOutsideCampGuards"] = false;
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

            if (!game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"] && !game.ChapterTwo.ChapterTwoBooleans["spawnedOutsideCampGuards"])
            {
                //Spawn initial enemies
                Bomblin ben = new Bomblin(new Vector2(2600, 266 - 371 * 1.1f), "Bomblin", game, ref player, this);
                ben.Hostile = false;
                ben.SpawnWithPoof = false;
                enemiesInMap.Add(ben);

                Bomblin ben1 = new Bomblin(new Vector2(1500, 16 - 371 * 1.1f), "Bomblin", game, ref player, this);
                ben1.Hostile = false;
                ben1.SpawnWithPoof = false;
                enemiesInMap.Add(ben1);

                Bomblin ben2 = new Bomblin(new Vector2(2700, -205 - 371 * 1.1f), "Bomblin", game, ref player, this);
                ben2.Hostile = false;
                ben2.SpawnWithPoof = false;
                enemiesInMap.Add(ben2);

                Bomblin ben3 = new Bomblin(new Vector2(4400, -366 - 371 * 1.1f), "Bomblin", game, ref player, this);
                ben3.Hostile = false;
                ben3.SpawnWithPoof = false;
                enemiesInMap.Add(ben3);

                game.ChapterTwo.ChapterTwoBooleans["spawnedOutsideCampGuards"] = true;
            }

            //Change the map once the bridge has fallen
            if (game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"])
            {
                if (!platforms.Contains(bridge))
                {
                    platforms.Add(bridge);
                    game.CurrentChapter.NPCs["Napoleon"].RecX = -1000;
                    game.CurrentChapter.NPCs["Cleopatra"].RecX = -1000;
                }

                if (interactiveObjects.Contains(locker))
                {
                    interactiveObjects.Remove(locker);
                }
            }
            else
                horse.Update();

            //Start a cutscene and set some variables when you hit the switch
            if (CheckSwitch(doorSwitch) && !game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"])
            {
                if (!game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"])
                   game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"] = true;

                game.CurrentChapter.state = Chapter.GameState.Cutscene;
                game.CurrentChapter.NPCs["BobTheConstructionGuyTwo"].ClearDialogue();
                game.CurrentChapter.NPCs["BobTheConstructionGuyTwo"].Dialogue.Add("Well that sure was lucky! Now we don't have to worry about splashes.");

            }

           // PlayBackgroundMusic();
           // PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCampMiddle = new Portal(4400, 737, "OutsideEnemyCamp");
            toCampMiddle.FButtonYOffset = -100;
            toCampMiddle.PortalNameYOffset = -100;

            toBathroom = new Portal(1420, platforms[0], "OutsideEnemyCamp");
            toBathroom.FButtonYOffset = -60;
            toBathroom.PortalNameYOffset = -60;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toCampMiddle, StoneFortCentral.ToOutsideCamp);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"])
            {
                s.Draw(doorFallen, new Vector2(3161, mapRec.Y + 1245), Color.White);

            }
            else
                horse.Draw(s);

            s.Draw(outhouse, new Rectangle(1310, 435, outhouse.Width, outhouse.Height), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);

            if(!game.ChapterTwo.ChapterTwoBooleans["campDoorFallen"] && game.CurrentChapter.state != Chapter.GameState.Cutscene)
                s.Draw(door, new Vector2(3161, mapRec.Y + 352), Color.White);
            
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(sky2, new Vector2(sky.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.4f, this, game));
            s.Draw(mountains, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.96f, this, game));
            s.Draw(castleParallax, new Vector2(1880, mapRec.Y + 133), Color.White);
            s.End();
        }
    }
}
