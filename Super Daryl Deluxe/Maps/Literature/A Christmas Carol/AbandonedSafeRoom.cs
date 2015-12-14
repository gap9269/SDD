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
    class AbandonedSafeRoom : MapClass
    {
        public static Portal toUnderTheFoyer;

        public Texture2D cageForeground, cage, backgroundTex;

        public AbandonedSafeRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Abandoned Safe Room";

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
            backgroundTex = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\AbandonedSafeRoom\background");
            cageForeground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\AbandonedSafeRoom\foreground");
            cage = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\AbandonedSafeRoom\cage");

            game.NPCSprites["Santa Claus"] = content.Load<Texture2D>(@"NPC\Literature\Santa Claus");
            Game1.npcFaces["Santa Claus"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Santa Claus Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Santa Claus"] = Game1.whiteFilter;
            Game1.npcFaces["Santa Claus"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUnderTheFoyer = new Portal(275, platforms[0], "Abandoned Safe Room");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

        }
        public override void DrawInFrontOfNPC(SpriteBatch s)
        {
            if (game.ChapterTwo.ChapterTwoBooleans["santaReleased"] == false)
            {
                s.Draw(cageForeground, new Vector2(0, 0), Color.White);
            }
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUnderTheFoyer, UnderTheMansion.toAbandonedSafeRoom);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(backgroundTex, new Vector2(0, 0), Color.White);

            if (game.ChapterTwo.ChapterTwoBooleans["santaReleased"] == false)
            {
                s.Draw(cage, new Vector2(777, 199), Color.White);
            }

            s.End();
        }
    }
}
