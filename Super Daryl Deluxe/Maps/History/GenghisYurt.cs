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
    class GenghisYurt : MapClass
    {
        static Portal toCamp;

        public static Portal ToCamp { get { return toCamp; } }

        Texture2D foreground;

        public GenghisYurt(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "The Yurt of Khan";

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
            background.Add(content.Load<Texture2D>(@"Maps\History\Yurt\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Yurt\foreground");

            game.NPCSprites["Genghis"] = content.Load<Texture2D>(@"NPC\History\Genghis");
            Game1.npcFaces["Genghis"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\GenghisNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Genghis"] = Game1.whiteFilter;
            Game1.npcFaces["Genghis"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCamp = new Portal(215, platforms[0], "The Yurt of Khan");
            ToCamp.FButtonYOffset = -80;
            ToCamp.PortalNameYOffset = -80;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCamp, MongolCamp.ToTent);
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
