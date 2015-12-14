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
    class SoldiersLeavingValley : Cutscene
    {

        public SoldiersLeavingValley(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            game.Camera.Update(player, game, game.CurrentChapter.CurrentMap);
            player.CutsceneStand();

            switch (state)
            {
                case 0:
                    FadeOut(60);
                    break;
                case 1:
                    if (timer > 15)
                    {
                        game.ChapterTwo.NPCs["Private Brian"].MapName = "Battlefield Outskirts";
                        game.ChapterTwo.NPCs["Private Brian"].RecX = 1300;
                        game.ChapterTwo.NPCs["Private Brian"].RecY = 260;
                        game.ChapterTwo.NPCs["Private Brian"].PositionX = 1300;
                        game.ChapterTwo.NPCs["Private Brian"].PositionY = 260;
                        game.ChapterTwo.NPCs["Private Brian"].ClearDialogue();
                        game.ChapterTwo.NPCs["Private Brian"].Dialogue.Add("General Bonaparte's orders are for us to remain here until back up arrives. Ze enemy's fortress is up ahead.");
                        game.ChapterTwo.NPCs["Private Brian"].UpdateRecAndPosition();


                        game.ChapterTwo.NPCs["French Soldier"].MapName = "Battlefield Outskirts";
                        game.ChapterTwo.NPCs["French Soldier"].RecX = 1527;
                        game.ChapterTwo.NPCs["French Soldier"].RecY = 252;
                        game.ChapterTwo.NPCs["French Soldier"].PositionX = 1527;
                        game.ChapterTwo.NPCs["French Soldier"].PositionY = 252;
                        game.ChapterTwo.NPCs["French Soldier"].ClearDialogue();
                        game.ChapterTwo.NPCs["French Soldier"].Dialogue.Add("Do not tell me we are going to attack ze enemy alone!");
                        game.ChapterTwo.NPCs["French Soldier"].Dialogue.Add("To think I could be collecting useless coins for zat creepy hill monster instead...");
                        game.ChapterTwo.NPCs["French Soldier"].UpdateRecAndPosition();

                        state++;
                        timer = 0;

                    }
                    break;
                case 2:
                    FadeIn(60);
                    break;
                case 3:
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
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
                    DrawFade(s, 0);
                    s.End();

                    break;

                case 1:
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
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
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
                    if(state == 2)
                        DrawFade(s, 1);
                    s.End();
                    break;
            }
        }
    }
}
