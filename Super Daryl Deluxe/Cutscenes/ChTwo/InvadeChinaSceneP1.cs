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
    class InvadeChinaSceneP1 : Cutscene
    {
        public InvadeChinaSceneP1(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {

                case 0:
                    FadeOut(120);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 1:
                    game.CurrentChapter.CutsceneState++;
                    (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY = 0;
                    game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "Mongolian Camp"), TheGreatWall.toBehindGreatWall);
                    game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
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
                    if (state == 0)
                        DrawFade(s, 0);
                    s.End();
                    break;
            }
        }
    }
}
