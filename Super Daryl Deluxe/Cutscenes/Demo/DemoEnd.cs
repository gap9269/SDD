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
    class DemoEnd : Cutscene
    {

        float alpha = 0;

        //--Takes in a background and all necessary objects
        public DemoEnd(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {

        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {

                case 0:
                    FadeOut(120);
                    break;
                case 1:
                    if (alpha < 1 && timer > 120)
                        alpha += .017f;
                    if(alpha >= 1 && (MyGamePad.APressed() || (Game1.g.current.IsKeyUp(Keys.Enter) && Game1.g.last.IsKeyDown(Keys.Enter))))
                    {
                        state++;
                    }
                    break;
                case 2:
                    FadeOut(120);
                    break;

                case 3:
                    game.ResetGameAndGoToMain();
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFadeWhite(s, 0f);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280,720), Color.White);
                    if(timer > 120)
                        s.Draw(StoneFortWastelandDemo.demoEndTexture, new Rectangle(0, 0, 1280, 720), Color.White * alpha);
                    s.End();
                break;

                case 2:
                s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                s.Draw(StoneFortWastelandDemo.demoEndTexture, new Rectangle(0, 0, 1280, 720), Color.White);
                DrawFade(s, 0f);
                s.End();
                break;

                case 3:
                s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                s.End();
                break;
            }
        }
    }
}