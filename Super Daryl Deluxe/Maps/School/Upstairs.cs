using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{

    class SpookyEye
    {
        int moveFrame = 0;
        int frameDelay = 5;
        int inactiveTime;
        Dictionary<String, Texture2D> texture;
        Rectangle rec;
        public SpookyEye(Dictionary<String, Texture2D> tex, int x, int y)
        {
            texture = tex;
            rec = new Rectangle(x, y, 85, 512);
        }

        public void Update()
        {
            if (inactiveTime <= 0)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 8;

                    if (moveFrame > 11)
                    {
                        moveFrame = 0;
                        inactiveTime = Game1.randomNumberGen.Next(180, 400);
                    }
                }
            }
            else
                inactiveTime--;
        }

        public void Draw(SpriteBatch s)
        {
            if(inactiveTime <= 0)
                s.Draw(texture.ElementAt(moveFrame).Value, rec, Color.White);
        }
    }

    class Upstairs : MapClass
    {

        static Portal toBathroom;
        static Portal toTheRoof;
        static Portal toUpperVents;
        static Portal toNorthHallLeft;
        static Portal toNorthHallRight;
        static Portal toTITS;
        StudentLocker drewsLocker, kensLocker;

        public static Portal ToTITS { get { return toTITS; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToNorthHallLeft { get { return toNorthHallLeft; } }
        public static Portal ToNorthHallRight { get { return toNorthHallRight; } }
        public static Portal ToTheRoof { get { return toTheRoof; } }
        public static Portal ToUpperVents { get { return toUpperVents; } }
        public StudentLocker DrewsLocker { get { return drewsLocker; } }

        Texture2D foreground, foreground2, titsLabel, titsStickers, literatureLabel;
        SpookyEye eye;
        public Upstairs(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5000;
            mapHeight = 720;
            mapName = "Upstairs";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;

            #region Student Locker
            Money mon = new Money(1.50f);
            List<object> contents = new List<object>();
            contents.Add(new YinYangNecklace());
            contents.Add(mon);
            drewsLocker = new StudentLocker(player, game, new Rectangle(1526, platforms[0].Rec.Y - 264, 100, 200), contents, "Drew's Locker", Game1.studentLockerTex);
            kensLocker = new StudentLocker(player, game, new Rectangle(3540, platforms[0].Rec.Y - 264, 100, 200), new List<object>() { new LootForStudentLocker("Ectoplasm", 8), new Money(3.50f) }, "Ken's Locker", Game1.studentLockerTex);

            lockers.Add(drewsLocker);
            lockers.Add(kensLocker);
            #endregion
        }
        public override void LoadContent()
        {

            //DDLabel = content.Load<Texture2D>(@"Maps\RoomLabels\D&D Banner");
            //DDPoster = content.Load<Texture2D>(@"Maps\RoomLabels\D&D Poster");


            titsLabel = content.Load<Texture2D>(@"Maps\RoomLabels\TITS Banner");
            titsStickers = content.Load<Texture2D>(@"Maps\RoomLabels\TITS Stickers");
            literatureLabel = content.Load<Texture2D>(@"Maps\RoomLabels\Literature");

            if (game.ChapterOne.ChapterOneBooleans["completedMapsQuest"])
            {
                toUpperVents.IsUseable = true;
                background.Add(content.Load<Texture2D>(@"Maps\School\Upstairs\background"));
            }
            else
            {
                toUpperVents.IsUseable = false;
                background.Add(content.Load<Texture2D>(@"Maps\School\Upstairs\background with vent"));
            }

            if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["Drew"] = content.Load<Texture2D>(@"NPC\Kickstarter\Drew");
                Game1.npcFaces["Drew"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Kickstarter\DrewNormal");
            }

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");

            background.Add(content.Load<Texture2D>(@"Maps\School\Upstairs\background2"));
            foreground = content.Load<Texture2D>(@"Maps\School\Upstairs\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\School\Upstairs\foreground2");

            eye = new SpookyEye(ContentLoader.LoadContent(content, "Maps\\School\\Upstairs\\Eye"), 3000, 0);

            if (Chapter.lastMap != "North Hall" && Chapter.lastMap != "Paranormal Club") 
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Music\Noir Halls");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("NoirHalls", backgroundMusic);
            }

            if (Chapter.lastMap != "North Hall" && Chapter.lastMap != "Paranormal Club") 
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_school_empty");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_school_empty", amb);
            }
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());


        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_school_empty");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            eye = null;

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;

            if (game.chapterState == Game1.ChapterState.chapterOne)
            {
                game.NPCSprites["Drew"] = Game1.whiteFilter;
                Game1.npcFaces["Drew"].faces["Normal"] = Game1.whiteFilter;
            }

            if (Chapter.theNextMap != "North Hall" && Chapter.theNextMap != "Paranormal Club") 
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void Update()
        {
            base.Update();
            PlayBackgroundMusic();
            PlayAmbience();
            eye.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNorthHallLeft = new Portal(350, platforms[0], "Upstairs", Portal.DoorType.movement_stairs);
            toNorthHallRight = new Portal(4475, platforms[0], "Upstairs", Portal.DoorType.movement_stairs);
            toBathroom = new Portal(4035, platforms[0], "Upstairs");
           // toTheRoof = new Portal(3100, platforms[0], "Upstairs", "Roof Key");
            toUpperVents = new Portal(0, platforms[0], "Upstairs");
            toTITS = new Portal(1225, platforms[0], "Upstairs");

            toTITS.FButtonYOffset = -30;
            toTITS.PortalNameYOffset = -30;

            ToNorthHallRight.FButtonYOffset = -30;
            ToNorthHallRight.PortalNameYOffset = -30;

            ToNorthHallLeft.FButtonYOffset = -30;
            ToNorthHallLeft.PortalNameYOffset = -30;

            ToBathroom.FButtonYOffset = -30;
            ToBathroom.PortalNameYOffset = -30;

            toUpperVents.FButtonYOffset = 60;
            toUpperVents.PortalNameYOffset = 60;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toNorthHallLeft, NorthHall.ToUpstairsLeft);
            portals.Add(toNorthHallRight, NorthHall.ToUpstairsRight);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toTITS, TITSRoom.ToUpstairs);
            portals.Add(toUpperVents, UpperVents1.ToUpstairs);
        }


        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            //s.Draw(DDLabel, new Vector2(1195, 305), Color.White);
            //s.Draw(DDPoster, new Vector2(1315, 405), Color.White);

            s.Draw(titsLabel, new Vector2(1248, 285), Color.White);
            s.Draw(titsStickers, new Rectangle(1290, 405, titsStickers.Width, titsStickers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            s.Draw(literatureLabel, new Vector2(2105, 305), Color.White);

            eye.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Rectangle(0, 0, foreground.Width, 720), Color.White);
            s.Draw(foreground2, new Rectangle(foreground.Width, 0, foreground2.Width, 720), Color.White);
            s.End();
        }
    }
}
