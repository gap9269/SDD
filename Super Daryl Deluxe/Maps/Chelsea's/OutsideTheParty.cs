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
    class OutsideTheParty : MapClass
    {
        static Portal toTheParty;
        static Portal toTheGoats;
        static Portal toChelseasPool;

        public static Portal ToTheParty { get { return toTheParty; } }
        public static Portal ToChelseasPool { get { return toChelseasPool; } }
        public static Portal ToTheGoats { get { return toTheGoats; } }

        Texture2D fore, para, sky;

        public OutsideTheParty(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Outside the Party";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            
            Barrel bar = new Barrel(game, 1824, 630, Game1.interactiveObjects["Barrel"], true, 1, 5, 1.38f, false, 0);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 2900, 590, Game1.interactiveObjects["Barrel"], true, 1, 0, .46f, false, 2);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 2800, 610, Game1.interactiveObjects["Barrel"], true, 1, 0, .76f, false, 1);
            interactiveObjects.Add(bar1);

            backgroundMusicName = "The Party";
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\OutsideParty"));
            fore = content.Load<Texture2D>(@"Maps\Chelseas\OutsidePartyFore");
            para = content.Load<Texture2D>(@"Maps\Chelseas\OutsidePartyParallax");
            sky = content.Load<Texture2D>(@"Maps\Chelseas\OutsidePartySky");


            game.NPCSprites["Balto"] = content.Load<Texture2D>(@"NPC\Main\balto");
            game.NPCSprites["Mark"] = content.Load<Texture2D>(@"NPC\Main\Mark");
            game.NPCSprites["Chelsea"] = content.Load<Texture2D>(@"NPC\Main\Chelsea");
            game.NPCSprites["Jesse"] = content.Load<Texture2D>(@"NPC\Main\Jesse");
            game.NPCSprites["Blurso"] = content.Load<Texture2D>(@"NPC\Party\Durso");

            Game1.npcFaces["Blurso"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Jesse");
            Game1.npcFaces["Jesse"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Jesse");

            Game1.npcFaces["Chelsea"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Chelsea");
            Game1.npcFaces["Mark"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Mark");
            Game1.npcFaces["Blurso"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Party\Blurso");
            //If the last map does not have the same music
            if (Chapter.lastMap != "The Party")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Who Likes to Party");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("The Party", backgroundMusic);
            }

            if(!Sound.music.ContainsKey("Exploring"))
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

            game.NPCSprites["Balto"] = Game1.whiteFilter;
            game.NPCSprites["Mark"] = Game1.whiteFilter;
            game.NPCSprites["Chelsea"] = Game1.whiteFilter;
            game.NPCSprites["Jesse"] = Game1.whiteFilter;
            game.NPCSprites["Blurso"] = Game1.whiteFilter;

            Game1.npcFaces["Blurso"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Jesse"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Chelsea"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Blurso"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Mark"].faces["Normal"] = Game1.whiteFilter;

            //DOn't clear the music if the next map is the party
            if (Chapter.theNextMap != "TheParty")
            {
                Sound.UnloadBackgroundMusic();
            }
            else
                Sound.music["Exploring"].Pause();
        }

        public override void PlayBackgroundMusic()
        {
            if (game.CurrentChapter.state != Chapter.GameState.ChangingMaps)
            {

                //1300 is the point where the music is originating from
                float partyVolume;

                //If the player is on the left side of the sound, make it fade out slower because he's still close to the target sound
                if (player.PositionX < 1300)
                    partyVolume = 1000 / Math.Abs(player.PositionX - 1300);
                else//Fade out faster because there is more room and the player gets farther away from the party quicker
                    partyVolume = 900 / Math.Abs(player.PositionX - 1300);

                //Clamp it between 0 and 1
                if (partyVolume < 0)
                    partyVolume = 0;
                if (partyVolume > 1)
                    partyVolume = 1;

                //Make it a bit quieter than inside
                partyVolume = partyVolume - .5f;

                //Pan depending on the player's position to the sound
                float partyPan = -((player.PositionX - 1300) / 300);

                Sound.PlayBackGroundMusic("The Party", partyVolume, partyPan);
                Sound.PlayBackGroundMusic("Exploring", .6f - partyVolume, 0);
            }
            else
            {
                //If the player is on the left side of the sound, play the party as you're changing maps because you're close to the party
                if (player.PositionX < 1300)
                {
                    if (Sound.backgroundVolume > .6f)
                        Sound.backgroundVolume = .6f;
                    Sound.PlayBackGroundMusic("The Party");
                }
                else//right side means the only thing you can hear is the exploring song, so play that as you fade out
                    Sound.PlayBackGroundMusic("Exploring");
            }
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheGoats = new Portal(0, platforms[0], "OutsidetheParty");
            toChelseasPool = new Portal(3800, platforms[0], "OutsidetheParty");
            toTheParty = new Portal(1180, platforms[0], "OutsidetheParty");
            toTheParty.PortalNameYOffset = -90;
            toTheParty.FButtonYOffset = -90;
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(fore, new Rectangle(0, 0, fore.Width, fore.Height), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.95f, this, game));

            s.Draw(sky, new Rectangle(0, 0, sky.Width, sky.Height), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.93f, this, game));
            s.Draw(para, new Rectangle(0, mapRec.Y, para.Width, para.Height), Color.White);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheParty, TheParty.ToOutsideTheParty);
            portals.Add(toTheGoats, TheGoats.ToOutsideTheParty);
            portals.Add(toChelseasPool, ChelseasPool.ToOutsideTheParty);
        }
    }
}
