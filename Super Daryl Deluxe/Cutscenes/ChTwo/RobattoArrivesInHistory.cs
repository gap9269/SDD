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
    class RobattoArrivesInHistory : Cutscene
    {
        NPC robatto;
        GameObject camfollow;
        public RobattoArrivesInHistory(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camfollow = new GameObject();
            camfollow.Rec = new Rectangle(0, 0, 1, 1);
        }

        public override void SkipCutscene()
        {
            base.SkipCutscene();
            robatto = game.CurrentChapter.NPCs["Mr. Robatto"];
            robatto.ClearDialogue();
            robatto.Dialogue.Add("You are in violation of rule code 2.17.B of the Water Falls High School Student Mandate: Existing in restricted territory without proper documentation.");
            robatto.Dialogue.Add("Uh oh! As is protocol for this circumstance, I must perform discipline. For your protection, please do not resist. There is no reason for this to affect our friendship!");
            robatto.PositionX = player.PositionX - 450;
            robatto.moveState = NPC.MoveState.standing;
            robatto.UpdateRecAndPosition();
            player.FacingRight = false;

            UnloadContent();
            state = 0;
            timer = 0;
            game.CurrentChapter.CutsceneState++;
            game.CurrentChapter.state = Chapter.GameState.Game;

            game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 545);
            game.Camera.center = game.Camera.centerTarget;
        }

        public override void Play()
        {
            base.Play();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        robatto = game.CurrentChapter.NPCs["Mr. Robatto"];
                        robatto.MapName = "Axis of Historical Reality";
                        robatto.UpdateRecAndPosition();
                        robatto.FacingRight = true;
                        robatto.ClearDialogue();
                        robatto.Dialogue.Add("Hello there student Daryl Whitelaw!");
                        robatto.Talking = true;
                    }


                    if (timer == 60)
                        player.FacingRight = false;

                    if(timer == 120)
                        camfollow.Position = new Vector2(player.VitalRecX + (player.VitalRecWidth / 2), 545);

                    if (timer > 120)
                    {
                        if (camfollow.PositionX > 910)
                            camfollow.PositionX -= 4;

                        camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);

                    }
                    else
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if(timer > 180)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        player.FacingRight = false;

                        robatto.ClearDialogue();
                        robatto.Dialogue.Add("You are in violation of rule code 2.17.B of the Water Falls High School Student Mandate: Existing in restricted territory without proper documentation.");
                        robatto.Dialogue.Add("Uh oh! As is protocol for this circumstance, I must perform discipline. For your protection, please do not resist. There is no reason for this to affect our friendship!"); 
                    }
                    if (camfollow.PositionX > 910)
                        camfollow.PositionX -= 4;

                    if (robatto.PositionX <= player.PositionX - 450)
                        robatto.Move(new Vector2(5, 0));
                    else
                    {
                        robatto.moveState = NPC.MoveState.standing;
                        state++;
                        timer = 0;
                    }
                    camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        robatto.Talking = true;
                    }

                    if (camfollow.PositionX > 910)
                        camfollow.PositionX -= 4;

                    camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);
                    robatto.UpdateInteraction();

                    if (!robatto.Talking)
                    {
                        UnloadContent();
                        state = 0;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                    }
                    break;
            }

            if (state > 0 || timer > 119)
            {
                game.Camera.centerTarget = new Vector2(camfollow.PositionX, 545);
                game.Camera.center = game.Camera.centerTarget;
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
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (robatto != null && (robatto.Talking || state == 0))
                        robatto.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
