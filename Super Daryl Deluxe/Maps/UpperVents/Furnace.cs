using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class Furnace : MapClass
    {
        static Portal toUpperVentsVI;

        public static Portal ToUpperVentsVI { get { return toUpperVentsVI; } }

        Texture2D foreground, middle, glow;
        Dictionary<String, Texture2D> steam;
        int steamFrame;
        int steamDelay = 5;
        float glowAlpha = 1f;
        Boolean glowDecrease = true;
        public Furnace(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Furnace Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Furnace\background"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Furnace\foreground");
            glow = content.Load<Texture2D>(@"Maps\Vents\Furnace\glow");
            middle = content.Load<Texture2D>(@"Maps\Vents\Furnace\middleGround");
            Sound.LoadVentZoneSounds();

            steam = ContentLoader.LoadContent(content, @"Maps\Vents\Furnace\steam");

            game.NPCSprites["Furnace Man"] = content.Load<Texture2D>(@"NPC\Side Quest\Furnace Man");

            Game1.npcFaces["Furnace Man"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Side Quest\FurnaceManNormal");

            SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_furnace");
            SoundEffectInstance amb = am.CreateInstance();
            amb.IsLooped = true;
            Sound.ambience.Add("ambience_furnace", amb);
            
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Furnace Man"] = Game1.whiteFilter;

            Game1.npcFaces["Furnace Man"].faces["Normal"] = Game1.whiteFilter;

            Sound.UnloadAmbience();
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_furnace");
        }
        public override void Update()
        {
            PlayAmbience();
            base.Update();
            steamDelay--;

            if (steamDelay <= 0)
            {
                steamDelay = 7;
                steamFrame++;

                if (steamFrame > 9)
                    steamFrame = 0;
            }

            if (glowDecrease)
            {
                glowAlpha -= .01f;

                if (glowAlpha <= .4)
                    glowDecrease = false;
            }
            else
            {
                glowAlpha += .01f;

                if (glowAlpha >= 1)
                    glowDecrease = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVentsVI = new Portal(50, platforms[0], "Furnace Room");
            toUpperVentsVI.PortalRecY = 445;
            toUpperVentsVI.PortalNameYOffset = 20;
            toUpperVentsVI.FButtonYOffset = 20;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVentsVI, UpperVentsVI.ToFurnace);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(steam.ElementAt(steamFrame).Value, new Vector2(297, -20), Color.White);
            s.Draw(middle, new Vector2(0, 0), Color.White);
            s.Draw(glow, new Vector2(429, 258), Color.White * glowAlpha);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Rectangle(0, 0, foreground.Width, 720), Color.White);
            s.End();
        }

    }
}