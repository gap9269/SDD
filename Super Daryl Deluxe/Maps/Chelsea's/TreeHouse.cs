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
    class TreeHouse : MapClass
    {
        static Portal toTheGoats;

        public static Portal ToTheGoats { get { return toTheGoats; } }

        Texture2D fore;
        Sparkles sparkles;
        Beer beer;
        public TreeHouse(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tree House";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            beer = new Beer(325, 494);
            beer.ShowFButton = false;
            storyItems.Add(beer);

            backgroundMusicName = "Outside the Party";
            sparkles = new Sparkles(335, 485);
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic("Exploring");
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\TreeHouse"));
            fore = content.Load<Texture2D>(@"Maps\Chelseas\TreeHouseFore");
            game.NPCSprites["Squirrel Boy"] = content.Load<Texture2D>(@"NPC\Party\squirrelKid");
            Game1.npcFaces["Squirrel Boy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Party\SquirrelBoy");

            //If the last map does not have the same music
            if (Chapter.lastMap != "The Goats")
            {
                SoundEffect bg1 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Hidden Agenda");
                SoundEffectInstance backgroundMusic1 = bg1.CreateInstance();
                backgroundMusic1.IsLooped = true;
                Sound.music.Add("Exploring", backgroundMusic1);
            }

            Sound.backgroundVolume = 1f;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Squirrel Boy"] = Game1.whiteFilter;

            Game1.npcFaces["Squirrel Boy"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "TheGoats")
            {
                Sound.UnloadBackgroundMusic();
            }
        }
        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();

            if (!beer.PickedUp)
            {
                sparkles.Update();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheGoats = new Portal(430, platforms[0], "TreeHouse");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);



            if (!beer.PickedUp)
            {
                sparkles.Draw(s);
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(fore, new Vector2(274, 501), Color.White);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheGoats, TheGoats.ToTreeHouse);
        }
    }
}
