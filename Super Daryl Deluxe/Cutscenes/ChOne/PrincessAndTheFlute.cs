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
    class PrincessAndTheFlute : Cutscene
    {
        NPC princess;

        float fadeAlpha = 1f;
        public PrincessAndTheFlute(Game1 g, Camera cam, Player p)
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
                        game.ChapterOne.ChapterOneBooleans["chandelierAdded"] = true;
                        princess = game.CurrentChapter.NPCs["The Princess"];
                        princess.RemoveQuest(game.ChapterOne.musicForAPrincess);
                        princess.ClearDialogue();
                        princess.Dialogue.Add("About time! You better have brought me something fit for a princess or I'm going to throw a fit unlike any you've ever seen. And then I'll tell Daddy.");
                        princess.Dialogue.Add("Come on, hand it over!");
                        princess.Dialogue.Add("...");
                        princess.Dialogue.Add("Well, it looks like you're good for something after all. This will have to do for now. To be honest I feel my musical interest starting to fade, anyway. Must have been a phase.");
                        princess.Dialogue.Add("In the off chance that you didn't disappoint me, I replaced the hole that you made in my ceiling with some wood from that old piano that you gave me. Now you can go back to doing whatever it was that you were doing up in those filthy vents. Nothing good, I'm sure.");
                        princess.Dialogue.Add("And watch the chandelier while you're crawling around up there. I just had it put in.");

                        princess.CurrentDialogueFace = "Normal";

                        player.PositionX = 140;
                        player.PositionY = -35;
                        player.UpdatePosition();
                    }


                    camera.Update(player, game, game.CurrentChapter.CurrentMap);


                    if (timer > 40)
                    {
                        if (fadeAlpha > 0)
                            fadeAlpha -= 1f / 90f;

                        if (player.RecX <= 324)
                        {
                            player.CutsceneWalk(new Vector2(4, 0));
                        }

                        else
                        {
                            player.CutsceneStand();
                            camera.Update(player, game, game.CurrentChapter.CurrentMap);
                            state++;
                            timer = 0;
                        }
                    }
                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        princess.Talking = true;
                    }

                    if (fadeAlpha > 0)
                        fadeAlpha -= 1f / 90f;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    princess.UpdateInteraction();

                    if (princess.Talking == false)
                    {
                        princess.ClearDialogue();
                        princess.Dialogue.Add("I fixed that hole you made so you would leave me alone. Why are you still here?");
                        game.ChapterOne.musicForAPrincess.RewardPlayer();
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.enteringMap = false;
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
                case 1:
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
                    if(timer > 1 && princess.Talking)
                        princess.DrawDialogue(s);

                    if (fadeAlpha > 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * fadeAlpha);
                    s.End();
                    break;
            }
        }
    }
}
