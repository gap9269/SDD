using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class ChapterOneOpening : Cutscene
    {
        Texture2D temp;

        //--Takes in a background and all necessary objects
        public ChapterOneOpening(Game1 g, Camera cam, Player player, Texture2D t)
            : base(g, cam, player)
        {
            temp = t;
        }

        public override void Play()
        {
            base.Play();

            camera.Update(player, game, game.CurrentChapter.CurrentMap);
            switch (state)
            {
                case 0:
                    FadeIn(200);
                    break;
                case 1:
                    FadeOut(120);
                    break;
                case 2:
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.CurrentChapter.CutsceneState++;
                    timer = 0;

                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(temp, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    DrawFade(s, 1);
                    s.End();
                    break;
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(temp, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    DrawFade(s, 0);
                    s.End();

                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
