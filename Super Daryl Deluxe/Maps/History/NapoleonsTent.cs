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
    class NapoleonsTent : MapClass
    {
        static Portal toNapoleonsCamp;

        public static Portal ToNapoleonsCamp { get { return toNapoleonsCamp; } }

        Texture2D foreground;

        public NapoleonsTent(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Napoleon's Tent";

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
            background.Add(content.Load<Texture2D>(@"Maps\History\Napoleon's Tent\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Napoleon's Tent\foreground");
            game.NPCSprites["Napoleon"] = content.Load<Texture2D>(@"NPC\History\Napoleon");
            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Napoleon"] = Game1.whiteFilter;
            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNapoleonsCamp = new Portal(550, platforms[0], "Napoleon's Tent");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNapoleonsCamp, NapoleonsCamp.ToTent);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));

            s.End();
        }
    }
}
