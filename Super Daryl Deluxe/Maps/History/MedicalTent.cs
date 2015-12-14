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
    class MedicalTent : MapClass
    {
        static Portal toNapoleonsCamp;

        public static Portal ToNapoleonsCamp { get { return toNapoleonsCamp; } }

        Texture2D foreground;

        public MedicalTent(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Medical Tent";

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
            background.Add(content.Load<Texture2D>(@"Maps\History\MedicalTent\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\MedicalTent\foreground");

            game.NPCSprites["Dr. Dominique Jean Larrey"] = content.Load<Texture2D>(@"NPC\History\Dr. Dominique Jean Larrey");
            Game1.npcFaces["Dr. Dominique Jean Larrey"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Dr. Dominique Jean Larrey Normal");
            Game1.npcFaces["Dr. Dominique Jean Larrey"].faces["Goblin"] = content.Load<Texture2D>(@"NPCFaces\History\Dr. Dominique Jean Larrey Goblin");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Dr. Dominique Jean Larrey"] = Game1.whiteFilter;
            Game1.npcFaces["Dr. Dominique Jean Larrey"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Dr. Dominique Jean Larrey"].faces["Goblin"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNapoleonsCamp = new Portal(550, platforms[0], "Medical Tent");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNapoleonsCamp, NapoleonsCamp.ToMedicalTent);
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
