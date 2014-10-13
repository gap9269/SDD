using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class PrologueEnd : Cutscene
    {
        // ATTRIBUTES \\
        NPC tim;
        Texture2D complete;
        GameObject camFollow;

        //--Takes in a background and all necessary objects
        public PrologueEnd(Game1 g, Camera cam, Player player, Texture2D com)
            : base(g, cam, player)
        {
            complete = com;
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        camFollow = new GameObject();
                        tim = game.CurrentChapter.NPCs["Tim"];
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        game.CurrentChapter.CurrentMap.Platforms.RemoveAt(game.CurrentChapter.CurrentMap.Platforms.Count - 1);
                        tim.PositionX = 4100;
                        tim.RecX = 4100;
                        tim.Update();
                        camFollow.PositionX = 4300;
                        player.PositionX = 4300;
                        player.FacingRight = false;
                        player.PositionY = game.CurrentChapter.CurrentMap.Platforms[0].Rec.Y - player.VitalRecHeight - 135;
                        player.UpdatePosition();
                        player.Velocity = Vector2.Zero;
                        player.KnockedBack = false;
                        player.InvincibleTime = 0;
                        player.Alpha = 1f;
                        tim.FacingRight = true;

                        player.playerState = Player.PlayerState.standing;

                        game.Prologue.Synopsis += "\n\nPaul and Alan were unhappy that you only brought back one textbook, but\n sold you a new skill anyway. Using this skill, you were able to defeat\n Tim, who was rather angry and didn't seem to understand that friends love flowers.";
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60)
                    {
                        state++;
                        timer = 0;
                    }

                    break;

                case 1:

                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    break;

                case 2:

                    if (firstFrameOfTheState)
                    {
                        tim.Dialogue.Clear();
                        tim.Dialogue.Add("You're fuckin' weird. Stay away from my locker.");
                        tim.Talking = true;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    tim.UpdateInteraction();

                    if (tim.Talking == false)
                    {
                        tim.Move(new Vector2(-4, 0));
                        tim.FacingRight = false;
                    }

                    if (tim.PositionX <= 3400)
                    {
                        timer = 0;
                        state++;
                    }

                    break;

                case 3:
                    if (firstFrameOfTheState)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 600)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 4:
                    FadeOut(120);
                    break;
                case 5:
                    game.chapterState = Game1.ChapterState.chapterOne;
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.CurrentChapter = game.ChapterOne;
                    player.playerState = Player.PlayerState.standing;
                    timer = 0;
                    player.StopSkills();
                    game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                    game.Camera.center = game.Camera.centerTarget;

                    Chapter.effectsManager.fButtonRecs.Clear();
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
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

                    s.Draw(game.EnemySpriteSheets["TimDead"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    DrawCutsceneBars(s);
                    s.End();
                    break;

                case 1:
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawCutsceneBars(s);
                    if (tim.Talking)
                        tim.DrawDialogue(s);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer > 400)
                        s.Draw(complete, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);


                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();
                    break;
               
            }
        }
    }
}
