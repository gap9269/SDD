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
    class BehindTheParty : MapClass
    {
        static Portal toTheParty;

        public static Portal ToTheParty { get { return toTheParty; } }

        public BehindTheParty(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 3550;
            mapName = "Behind the Party";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            zoomLevel = .9f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            backgroundMusicName = "The Party";
        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("The Party");
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\BehindTheParty"));
            game.NPCSprites["Jesse"] = content.Load<Texture2D>(@"NPC\Main\Jesse");
            game.NPCSprites["Mark"] = content.Load<Texture2D>(@"NPC\Main\Mark");
            game.NPCSprites["Bob the Construction Guy"] = content.Load<Texture2D>(@"NPC\Party\ConstructionBob");

            Game1.npcFaces["Jesse"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Jesse");
            Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Bob");
            //If the last map does not have the same music
            //if (Chapter.lastMap != "The Party")
            //{
            //    SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Who Likes to Party");
            //    SoundEffectInstance backgroundMusic = bg.CreateInstance();
            //    backgroundMusic.IsLooped = true;
            //    Sound.music.Add("The Party", backgroundMusic);
            //}

            //Sound.backgroundVolume = .01f;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Jesse"] = Game1.whiteFilter;
            game.NPCSprites["Mark"] = Game1.whiteFilter;
            game.NPCSprites["Bob the Construction Guy"] = Game1.whiteFilter;

            Game1.npcFaces["Jesse"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = Game1.whiteFilter;

            ////DOn't clear the music if the next map is behind the party
            //if (Chapter.theNextMap != "TheParty")
            //{
            //    Sound.UnloadBackgroundMusic();
            //}
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheParty = new Portal(350, platforms[0], "BehindtheParty");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheParty, TheParty.ToBehindTheParty);
        }
    }
}
