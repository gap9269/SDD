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
    public class QuadPaperScene : Cutscene
    {
        GameObject camFollow;
        LockerSheet pieceOfPaper;
        SoundEffect cutscene_prologue_bird_steals_note;

        public QuadPaperScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
            pieceOfPaper = new LockerSheet(1350, 550);
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        camFollow.Position = new Vector2(player.VitalRec.Center.X);
                    }

                    player.CanJump = false;

                    if (player.playerState == Player.PlayerState.standing || player.playerState == Player.PlayerState.running)
                        player.CutsceneStand();
                    else
                        player.Update();

                    camFollow.PositionX += 5;
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (camFollow.PositionX >= 1350)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    pieceOfPaper.RecX += 5;

                    if (timer > 150)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    if (topBarPos <= -66)
                    {
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.CanJump = true;
                    }
                    break;


            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:

                case 1: //Draws the black box over the entire screen and changes the alpha
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                     null, null, null, null, camera.Transform);

                     game.CurrentChapter.CurrentMap.Draw(s);
                     player.Draw(s);
                     pieceOfPaper.Draw(s);
                     s.End();

                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     DrawCutsceneBars(s);
                     s.End();
                     break;

                case 2:
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                     null, null, null, null, camera.Transform);
                     game.CurrentChapter.CurrentMap.Draw(s);
                     game.CurrentChapter.DrawNPC(s);
                     player.Draw(s);
                     s.End();

                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
 null, null, null, null, camera.StaticTransform);

                     DrawRemoveCutsceneBars(s);

                     s.End();
                     break;
            }
        }
    }
}
