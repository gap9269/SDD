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
    class ChelseasField : MapClass
    {
        static Portal toTheGoats;
        static Portal toSpookyField;
        static Portal toTheWoods;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToTheWoods { get { return toTheWoods; } }
        public static Portal ToTheGoats { get { return toTheGoats; } }
        public static Portal ToSpookyField { get { return toSpookyField; } }

        Texture2D back, barn, fore, sky, staticSky, tree, outhouse;

        public ChelseasField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2550;
            mapName = "Chelsea's Field";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("Exploring");
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\ChelseasField"));
            sky = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldSky");
            staticSky = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldStaticSky");
            back = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldBackField");
            fore = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldFore");
            barn = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldBarn");
            tree = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasFieldTree");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            ////If the last map does not have the same music
            if (Chapter.lastMap != "The Goats")
            {
                //SoundEffect bg1 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Hidden Agenda");
                //SoundEffectInstance backgroundMusic1 = bg1.CreateInstance();
                //backgroundMusic1.IsLooped = true;
                //Sound.music.Add("Exploring", backgroundMusic1);

                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_outdoors_night");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_outdoors_night", amb);
            }

            //Sound.backgroundVolume = 1f;

            game.NPCSprites["Weapons Master"] = content.Load<Texture2D>(@"NPC\DD\inventory");
            game.NPCSprites["Skill Sorceress"] = content.Load<Texture2D>(@"NPC\DD\skill");
            game.NPCSprites["Saving Instructor"] = content.Load<Texture2D>(@"NPC\DD\save");

            Game1.npcFaces["Saving Instructor"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Save");
            Game1.npcFaces["Skill Sorceress"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Skill");
            Game1.npcFaces["Weapons Master"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Equipment");


        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Weapons Master"] = Game1.whiteFilter;
            game.NPCSprites["Skill Instructor"] = Game1.whiteFilter;
            game.NPCSprites["Save Instructor"] = Game1.whiteFilter;

            Game1.npcFaces["Weapons Master"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Saving Instructor"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Skill Sorceress"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "The Goats")
            {
                //Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
            }
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_outdoors_night");
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Rectangle(1930, 570 - outhouse.Height, outhouse.Width, outhouse.Height), Color.White);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSpookyField = new Portal(1525, platforms[0], "Chelsea's Field");
            toTheGoats = new Portal(2350, platforms[0], "Chelsea's Field");
            toTheWoods = new Portal(100, platforms[0], "Chelsea's Field");
            ToTheWoods.IsUseable = false;
            toBathroom = new Portal(2040, 330, "Chelsea's Field");
            toBathroom.PortalRecY = 330;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toSpookyField, SpookyField.ToChelseasField);
            portals.Add(toTheGoats, TheGoats.ToChelseasField);
            portals.Add(toTheWoods, TheWoods.ToTheField);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(fore, new Vector2(2272, 635), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(.95f, this, game));
            s.Draw(tree, new Vector2(0, 0), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(-900, mapRec.Y, sky.Width, sky.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));

            s.Draw(staticSky, new Rectangle(1193, 0, staticSky.Width, staticSky.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(barn, new Rectangle(100, 150, barn.Width, barn.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(back, new Rectangle(0, 194, back.Width, back.Height), Color.White);
            s.End();

        }
    }
}
