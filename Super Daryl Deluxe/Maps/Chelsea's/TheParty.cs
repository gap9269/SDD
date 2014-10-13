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
    class TheParty : MapClass
    {
        static Portal toBehindTheParty;
        static Portal toOutsideTheParty;

        public static Portal ToBehindTheParty { get { return toBehindTheParty; } }
        public static Portal ToOutsideTheParty { get { return toOutsideTheParty; } }

        static Button toYourLockerButton;
        public static Button ToYourLockerButton { get { return toYourLockerButton; } }

        Texture2D foreGround;
        Texture2D frontDoor, light, haze1, haze2, haze3, smoke, parallax, parallaxSmoke, partyPeople;

        float doorAlpha = 0f;

        int lightTime;
        int flickAmount;
        int maxFlick;
        Boolean lightOn = false;
        static Random lightRandom;

        float haze1Alpha = 0f;
        float haze2Alpha = .5f;
        float haze3Alpha = 1f;

        Boolean hazeOneIncrease = true;
        Boolean hazeTwoIncrease = true;
        Boolean hazeThreeIncrease = false;

        int smokeFrame;
        int smokeTimer = 5;

        public TheParty(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2440;
            mapName = "The Party";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lightRandom = new Random();
            maxFlick = lightRandom.Next(2, 8);

            backgroundMusicName = "The Party";

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\TheParty"));
            foreGround = content.Load<Texture2D>(@"Maps\Chelseas\ThePartyFore");
            frontDoor = content.Load<Texture2D>(@"Maps\Chelseas\ThePartyDoor");
            light = content.Load<Texture2D>(@"Maps\Chelseas\ThePartyLight");
            partyPeople = content.Load<Texture2D>(@"Maps\Chelseas\PartyPeople");

            game.NPCSprites["Alan"] = content.Load<Texture2D>(@"NPC\Main\alan");
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"NPC\Main\paul");
            game.NPCSprites["Julius Caesar"] = content.Load<Texture2D>(@"NPC\Party\Julius");

            Game1.npcFaces["Alan"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Alan\AlanNormal");
            Game1.npcFaces["Alan"].faces["Arrogant"] = content.Load<Texture2D>(@"NPCFaces\Alan\AlanHappy");

            Game1.npcFaces["Paul"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulNormal");
            Game1.npcFaces["Paul"].faces["Arrogant"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulHappy");
            Game1.npcFaces["Paul"].faces["Fonz"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulFonz");

            Game1.npcFaces["Chelsea"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Chelsea");
            Game1.npcFaces["Julius Caesar"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Party\Julius");
            Game1.npcFaces["Julius Caesar"].faces["Naked"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusNaked");

            parallax = content.Load<Texture2D>(@"Maps\Chelseas\ThePartyParallax");
            parallaxSmoke = content.Load<Texture2D>(@"Maps\Chelseas\ParallaxSmoke");
            haze1 = content.Load<Texture2D>(@"Maps\Chelseas\PartyHaze1");
            haze2 = content.Load<Texture2D>(@"Maps\Chelseas\PartyHaze2");
            haze3 = content.Load<Texture2D>(@"Maps\Chelseas\PartyHaze3");

            //If the last map does not have the same music
            if (Chapter.lastMap != "Behind the Party" && Chapter.lastMap != "Outside the Party")
            {
                SoundEffect bg = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Who Likes to Party");
                SoundEffectInstance backgroundMusic = bg.CreateInstance();
                backgroundMusic.IsLooped = true;
                Sound.music.Add("The Party", backgroundMusic);
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Alan"] = Game1.whiteFilter;
            game.NPCSprites["Paul"] = Game1.whiteFilter;
            game.NPCSprites["Julius Caesar"] = Game1.whiteFilter;
            game.NPCSprites["Chelsea"] = Game1.whiteFilter;

            Game1.npcFaces["Alan"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Alan"].faces["Arrogant"] = Game1.whiteFilter;

            Game1.npcFaces["Paul"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Paul"].faces["Arrogant"] = Game1.whiteFilter;
            Game1.npcFaces["Paul"].faces["Fonz"] = Game1.whiteFilter;

            //DOn't clear the music if the next map is behind the party
            if (Chapter.theNextMap != "BehindtheParty" && Chapter.theNextMap != "OutsidetheParty")
            {
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic("The Party");
        }

        public override void Update()
        {
            base.Update();

            game.CurrentChapter.NPCs["Julius"].RecX = 1800;

            if (hazeOneIncrease)
            {
                haze1Alpha += .01f;

                if (haze1Alpha >= 1f)
                    hazeOneIncrease = false;
            }
            else
            {
                haze1Alpha -= .01f;

                if (haze1Alpha <= 0f)
                    hazeOneIncrease = true;
            }

            if (hazeTwoIncrease)
            {
                haze2Alpha += .01f;

                if (haze2Alpha >= 1f)
                    hazeTwoIncrease = false;
            }
            else
            {
                haze2Alpha -= .01f;

                if (haze2Alpha <= 0f)
                    hazeTwoIncrease = true;
            }

            if (hazeThreeIncrease)
            {
                haze3Alpha += .01f;

                if (haze3Alpha >= 1f)
                    hazeThreeIncrease = false;
            }
            else
            {
                haze3Alpha -= .01f;

                if (haze3Alpha <= 0f)
                    hazeThreeIncrease = true;
            }

            PlayBackgroundMusic();

            smokeTimer--;

            if (smokeTimer == 0)
            {
                smokeFrame++;
                smokeTimer = 8;

                if (smokeFrame > 10)
                    smokeFrame = 0;
            }

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

            if (player.VitalRec.Intersects(toYourLockerButton.ButtonRec) && current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F))
            {
                game.YourLocker.LoadContent();
                game.CurrentChapter.state = Chapter.GameState.YourLocker;
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBehindTheParty = new Portal(1765, 300, "TheParty");
            toBehindTheParty.PortalRecY = 300; //This is a hard coded value. the constructor modifies it so keep this line here
            toOutsideTheParty = new Portal(280, platforms[0], "TheParty");

            toYourLockerButton = new Button(Game1.portalLocker, new Rectangle(860, platforms[0].Rec.Y - Game1.portalLocker.Width,
    Game1.portalLocker.Width - 150, Game1.portalLocker.Height));
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOutsideTheParty, OutsideTheParty.ToTheParty);
            portals.Add(toBehindTheParty, BehindTheParty.ToTheParty);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(partyPeople, mapRec, Color.White);

            if (lightOn)
                s.Draw(light, new Rectangle(1347, 19, 675, 236), Color.White);
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

            s.Draw(foreGround, new Rectangle(563, 580, foreGround.Width, foreGround.Height), Color.White);
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(1.10f, this, game));

            s.Draw(parallax, new Rectangle(1203, 513, parallax.Width, parallax.Height), Color.White);
            s.Draw(parallaxSmoke, new Rectangle(1203, 513, 334, 204), new Rectangle(334 * smokeFrame, 0, 334, 204), Color.White);
            s.End();

        

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));


            if (player.VitalRec.X < 700)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(frontDoor, new Rectangle(188, 387, frontDoor.Width, frontDoor.Height), Color.White * doorAlpha);

            s.Draw(haze1, new Rectangle(82, 2, haze1.Width, haze1.Height), Color.White * haze1Alpha);
            s.Draw(haze2, new Rectangle(82, 2, haze1.Width, haze1.Height), Color.White * haze2Alpha);
            s.Draw(haze3, new Rectangle(82, 2, haze1.Width, haze1.Height), Color.White * haze3Alpha);

            s.End();
        }

    }
}
