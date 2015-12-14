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
    class JanitorCutscene : Cutscene
    {
        NPC janitor;
        int specialTimer = 0;

        public JanitorCutscene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        janitor = game.CurrentChapter.NPCs["The Janitor"];
                        janitor.ClearDialogue();
                        janitor.Dialogue.Add("The hell was that about?");
                        janitor.Dialogue.Add("Did that lad have a copy of...? No, no...of course he couldn't.");
                        janitor.CurrentDialogueFace = "Normal";

                        player.PositionX = 451;
                        player.PositionY = 288;
                        player.FacingRight = true;
                        player.UpdatePosition();
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 120)
                    {
                        player.CutsceneStand();
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {

                    }

                    if (timer == 20)
                    {
                        player.AddStoryItem("Book Spray", "Book Spray", 1);
                    }

                    Chapter.effectsManager.Update();

                    if (timer > 60 && player.PositionX > -100)
                    {
                        player.CutsceneRun(new Vector2(-player.MoveSpeed, 0));
                    }
                    else if (timer <= 60)
                        player.CutsceneStand();
                    else if (player.PositionX <= -100)
                    {
                        player.MoveFrame = 0;
                        player.playerState = Player.PlayerState.relaxedStanding;

                        specialTimer++;

                        if (specialTimer > 20)
                        {
                            player.PositionX = -5000;
                            player.UpdatePosition();

                        }
                        if (specialTimer > 75)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        janitor.Talking = true;
                    }

                    //camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    janitor.UpdateInteraction();

                    if (janitor.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 3:
                    FadeOut(180);
                    break;

                case 4:
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    player.Alpha = 1;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.CurrentChapter.CurrentMap.enteringMap = false;

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
                    s.DrawString(game.Font, "JANITOR INTRO SCENE + BOSS FIGHT", new Vector2(400, 340), Color.White);
                    s.End();
                    break;
                case 1:
                case 2:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer > 1 && janitor.Talking)
                        janitor.DrawDialogue(s);

                    Chapter.effectsManager.DrawFoundItem(s);
                    s.End();
                    break;
                case 3:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
