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
    class AfterLumberQuestScene : Cutscene
    {
        NPC genghis;
     

        public AfterLumberQuestScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {

                case 0:
                    FadeOut(60);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        genghis = game.CurrentChapter.NPCs["Genghis"];
                        genghis.MapName = "Mongolian Camp";
                        genghis.RecX = 726;
                        genghis.RecY = 306;
                        genghis.PositionX = 726;
                        genghis.PositionY = 306;
                    }
                    if (timer > 15)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    FadeIn(60);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 3:
                    genghis.ClearDialogue();
                    genghis.Dialogue.Add("Here it is, Kublai!");
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:
                case 1:
                case 2:
                case 3:
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
                    else if (state == 1)
                        s.Draw(Game1.whiteFilter, game.CurrentChapter.CurrentMap.mapRec, Color.Black);
                    else if (state == 2)
                        DrawFade(s, 1);
                    s.End();
                    break;
            }
        }
    }
}
