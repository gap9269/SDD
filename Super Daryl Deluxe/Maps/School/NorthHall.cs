using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class NorthHall : MapClass
    {

        static Portal toArtHall;
        static Portal toScienceIntroRoom;
        StudentLocker timsLocker;
        static Portal toBathroom;
        static Portal toUpstairs;
        static Portal toGymLobby;

        //PROLOGUE BOSS FIGHT SHIT
        public static Boolean drawTimMap = false;
        public static Platform leftTimPlat, rightTimPlat, leftPillar, leftStep, rightPillar, rightStep;
        public static Barrel timBar1, timBar2, timBar3;

        int lightTime;
        int flickAmount;
        int maxFlick;
        Boolean lightOn = false;
        static Random lightRandom;

        static Button toYourLockerButton;

        PetRat petRat;

        Texture2D fore, fore2, light, darylLocker, timMap;
        public static Texture2D timPlatform;

        public static Portal ToUpstairs { get { return toUpstairs; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToArtHall { get { return toArtHall; } }
        public static Portal ToGymLobby { get { return toGymLobby; } }
        public static Portal ToScienceIntroRoom { get { return toScienceIntroRoom; } }
        public static Button ToYourLockerButton { get { return toYourLockerButton; } }

        public StudentLocker TimsLocker { get { return timsLocker; } }
        public NorthHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5000;// 7352;
            mapHeight = 720;
            mapName = "North Hall";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            petRat = new PetRat(game);

            lightRandom = new Random();
            maxFlick = lightRandom.Next(2, 8);

            backgroundMusicName = "Noir Halls";

            #region Student Locker
            Money mon = new Money(5.00f);
            List<object> contents = new List<object>();
            contents.Add(mon);
            timsLocker = new StudentLocker(player, game, new Rectangle(1526, platforms[0].Rec.Y - 264, 100, 200), contents, "Tim's Locker", Game1.studentLockerTex);

            lockers.Add(timsLocker);
            #endregion
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\NorthHall1"));
            background.Add(content.Load<Texture2D>(@"Maps\School\NorthHall2"));

            fore = content.Load<Texture2D>(@"Maps\School\NorthHallFore1");
            fore2 = content.Load<Texture2D>(@"Maps\School\NorthHallFore2");

            light = content.Load<Texture2D>(@"Maps\School\light");
            darylLocker = content.Load<Texture2D>(@"Maps\School\DarylLocker");

            game.NPCSprites["Alan"] = content.Load<Texture2D>(@"NPC\Main\alan");
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"NPC\Main\paul");

            Game1.npcFaces["Alan"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Alan\AlanNormal");
            Game1.npcFaces["Alan"].faces["Arrogant"] = content.Load<Texture2D>(@"NPCFaces\Alan\AlanHappy");

            Game1.npcFaces["Paul"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulNormal");
            Game1.npcFaces["Paul"].faces["Arrogant"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulHappy");
            Game1.npcFaces["Paul"].faces["Fonz"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulFonz");

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Skill Sorceress"] = content.Load<Texture2D>(@"NPC\DD\skill");
                game.NPCSprites["Keeper of the Quests"] = content.Load<Texture2D>(@"NPC\DD\journal");
                game.NPCSprites["Tim"] = content.Load<Texture2D>(@"NPC\Main\tim");
                Game1.npcFaces["Tim"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Tim");

                Game1.npcFaces["Keeper of the Quests"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Journal");
                Game1.npcFaces["Skill Sorceress"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Skill");
            }

            if (Chapter.lastMap != "East Hall")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("North Hall", backgroundMusic);
            }

            if (Chapter.lastMap != "East Hall")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("North Hall", amb);
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                EnemyContentLoader.FezGoblin(content);
                if (game.Prologue.PrologueBooleans["addBox"] == true && game.Prologue.PrologueBooleans["addedBox"] == false && enemiesInMap.Count == 0)
                {
                    FezGoblin goblin = new FezGoblin(new Microsoft.Xna.Framework.Vector2(3700, 500), "Fez Goblin", Game1.g, ref Game1.g.Prologue.player, Game1.schoolMaps.maps["NorthHall"]);
                    goblin.SpawnWithPoof = false;

                    Game1.schoolMaps.maps["NorthHall"].EnemiesInMap.Add(goblin);
                }

                if (game.Prologue.PrologueBooleans["gotTextbook"] == true)
                {
                    EnemyContentLoader.GorillaTimBoss(content);
                    timMap = content.Load<Texture2D>(@"Maps\School\Tim Boss Map");
                    timPlatform = content.Load<Texture2D>(@"Maps\School\bossPlat");
                }
            }
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic("North Hall");
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("North Hall");
        }


        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Alan"] = Game1.whiteFilter;
            game.NPCSprites["Paul"] = Game1.whiteFilter;

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Skill Sorceress"] = Game1.whiteFilter;
                game.NPCSprites["Keeper of the Quests"] = Game1.whiteFilter;

                Game1.npcFaces["Skill Sorceress"].faces["Normal"] = Game1.whiteFilter;
                Game1.npcFaces["Keeper of the Quests"].faces["Arrogant"] = Game1.whiteFilter;

                game.NPCSprites["Tim"] = Game1.whiteFilter;
                Game1.npcFaces["Tim"].faces["Normal"] = Game1.whiteFilter;

                if (game.Prologue.PrologueBooleans["gotTextbook"] == true)
                {
                    GorillaTim.animationTextures.Clear();
                    drawTimMap = false;
                }
            }

            Game1.npcFaces["Alan"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Alan"].faces["Arrogant"] = Game1.whiteFilter;

            Game1.npcFaces["Paul"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Paul"].faces["Arrogant"] = Game1.whiteFilter;
            Game1.npcFaces["Paul"].faces["Fonz"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "EastHall")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toArtHall = new Portal(4760, platforms[0], "NorthHall");
            toArtHall.FButtonYOffset = -30;
            toArtHall.PortalNameYOffset = -30;

            toScienceIntroRoom = new Portal(1227, platforms[0], "NorthHall", "Key Ring");
            toScienceIntroRoom.FButtonYOffset = -30;
            toScienceIntroRoom.PortalNameYOffset = -30;

            toYourLockerButton = new Button(Game1.portalLocker, new Rectangle(3283, platforms[0].Rec.Y - Game1.portalLocker.Width, 150, Game1.portalLocker.Height)); 

            toBathroom = new Portal(4040, platforms[0], "NorthHall");
            toBathroom.FButtonYOffset = -30;
            toBathroom.PortalNameYOffset = -30;

            toUpstairs = new Portal(350, platforms[0], "NorthHall");
            toGymLobby = new Portal(100, platforms[0], "NorthHall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toArtHall, ArtHall.ToNorthHall);
            portals.Add(toScienceIntroRoom, ScienceIntroRoom.ToNorthHall);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toUpstairs, Upstairs.ToNorthHall);
            portals.Add(toGymLobby, GymLobby.ToNorthHall);
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
            PlayAmbience();

            #region Light Flicker
            lightTime--;

            if (lightTime <= 0)
            {
                lightOn = !lightOn;
                lightTime = lightRandom.Next(2, 5);
                flickAmount++;

                if (flickAmount == maxFlick)
                {
                    int onOff = lightRandom.Next(2);

                    if (onOff == 0)
                        lightOn = true;
                    else
                        lightOn = false;

                    flickAmount = 0;
                    maxFlick = lightRandom.Next(2, 8);
                    lightTime = lightRandom.Next(60, 300);
                }
            }
            #endregion

            if (player.VitalRec.Intersects(toYourLockerButton.ButtonRec) && current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F) && player.LearnedSkills.Count > 0 && game.CurrentChapter.BossFight == false)
            {
                game.YourLocker.LoadContent();
                game.CurrentChapter.state = Chapter.GameState.YourLocker;
            }

            if (game.Prologue.PrologueBooleans["secondSceneNotPlayed"] == true)
            {
                petRat.Update();
            }
            if (drawTimMap)
            {
                timBar1.Update();
                timBar2.Update();
                timBar3.Update();

            }
        }

        public override string CheckPortals()
        {
            String name = base.CheckPortals();

                return name;
        }



        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (player.VitalRec.Intersects(NorthHall.ToYourLockerButton.ButtonRec) && !game.CurrentChapter.TalkingToNPC && game.CurrentChapter.state == Chapter.GameState.Game && player.LearnedSkills.Count > 0 && game.CurrentChapter.BossFight == false)
            {
                s.Draw(darylLocker, new Vector2(toYourLockerButton.ButtonRecX + 22, toYourLockerButton.ButtonRecY - 24), Color.White);
            }

            if (game.chapterState == Game1.ChapterState.prologue && game.CurrentChapter.state != Chapter.GameState.Cutscene)
            {
                if (game.Prologue.PrologueBooleans["secondSceneNotPlayed"] == true)
                {
                    petRat.Draw(s);
                }
            }

            if (drawTimMap)
            {
                s.Draw(timMap, new Vector2(2232, 0), Color.White);

                if (game.CurrentChapter.CurrentBoss != null)
                {
                    (game.CurrentChapter.CurrentBoss as GorillaTim).DrawPlatformsDisappearing(s);
                    (game.CurrentChapter.CurrentBoss as GorillaTim).DrawPlatformsFalling(s);
                }
                //If the map has the platforms in it for Tim, draw the textures over them
                if (platforms.Contains(rightTimPlat))
                {

                    s.Draw(timPlatform, new Rectangle(rightTimPlat.RecX - 10, rightTimPlat.RecY, rightTimPlat.RecWidth + 20, rightTimPlat.RecHeight), null, Color.White);
                    s.Draw(timPlatform, new Rectangle(leftTimPlat.RecX - 10, leftTimPlat.RecY, leftTimPlat.RecWidth + 20, leftTimPlat.RecHeight), null, Color.White);
                }
                if (timBar1 != null)
                {
                    timBar1.Draw(s);
                    timBar2.Draw(s);
                }
            }

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(fore, new Rectangle(0, 0, fore.Width, 720), Color.White);
            s.Draw(fore2, new Rectangle(fore.Width, 0, fore2.Width, 720), Color.White);

            if (lightOn && !drawTimMap)
                s.Draw(light, new Vector2(3009, 0), Color.White);

            if (drawTimMap && timBar3 != null)
                timBar3.Draw(s);

            s.End();
        }

    }
}
