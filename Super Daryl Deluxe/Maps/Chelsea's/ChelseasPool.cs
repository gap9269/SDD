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
    class ChelseasPool : MapClass
    {
        static Portal toOutsideTheParty;
        static Portal toOldShed;
        static Portal toOldShedTop;

        public static Portal ToOldShedTop { get { return toOldShedTop; } }
        public static Portal ToOutsideTheParty { get { return toOutsideTheParty; } }
        public static Portal ToOldShed { get { return toOldShed; } }
        Beer beer;
        Sparkles sparkles;
        Texture2D foreGroundBush, back, house, sky, field;
        public ChelseasPool(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2200;
            mapName = "Chelsea's Pool";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            beer = new Beer(1930, 625);
            beer.ShowFButton = false;
            storyItems.Add(beer);

            sparkles = new Sparkles(1930, 625);

        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("Exploring");
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

            if (!beer.PickedUp)
            {
                sparkles.Update();
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPool"));
            foreGroundBush = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolFore");
            field = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolField");
            sky = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolSky");
            field = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolField");
            back = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolBack");
            house = content.Load<Texture2D>(@"Maps\Chelseas\ChelseasPoolHouse");

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");

            ////If the last map does not have the same music
            if (Chapter.lastMap == "Old Shed")
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
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap == "Old Shed")
            {
                //Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
            }
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toOutsideTheParty = new Portal(50, platforms[0], "Chelsea's Pool");
            toOldShed = new Portal(1420, platforms[0].Rec.Y - 30, "Chelsea's Pool");
            toOldShedTop = new Portal(1650, 240, "Chelsea's Pool");
            toOldShedTop.PortalRecY = 50;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOutsideTheParty, OutsideTheParty.ToChelseasPool);
            portals.Add(toOldShed, OldShed.ToChelseasPool);
            portals.Add(toOldShedTop, OldShed.ToChelseasPoolTop);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1f, this, game));

            //Draw the beer in the foreground
            if (beer.PickedUp == false)
            {
                beer.Draw(s);
                sparkles.Draw(s);
            }

            s.Draw(foreGroundBush, new Vector2(0, 0), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(0, mapRec.Y, sky.Width, sky.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(back, new Rectangle(50, 143, back.Width, back.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.4f, this, game));
            s.Draw(house, new Rectangle(421, 7, house.Width, house.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.5f, this, game));
            s.Draw(field, new Rectangle(0, 228, field.Width, field.Height), Color.White);
            s.End();

        }
    }
}
