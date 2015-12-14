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
    class PrincessSceneOne : Cutscene
    {
        NPC princess;


        public PrincessSceneOne(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            Chapter.effectsManager.Update();

            switch (state)
            {

                case 0:
                    if (firstFrameOfTheState)
                    {
                        princess = game.ChapterTwo.NPCs["The Princess"];
                        princess.CompleteQuestSilently(game.ChapterTwo.tutoringThePrincess);
                        princess.ClearDialogue();
                        princess.Dialogue.Add("If you stand on my bed ONE more time, I swear I'm-- Wait");
                        princess.Dialogue.Add("Are you...are you wearing a TUTORING LICENSE?");
                        princess.Dialogue.Add("Hahahahahahaha!");
                        princess.Dialogue.Add("Oh lord...that's good. What do you teach? Knuckle dragging? Snot licking?");
                        princess.Dialogue.Add("No, no wait! I have it! You give lessons on being a weird vent lurker. How silly of me to not see that.");
                        princess.Dialogue.Add("Look, we both know you're a few steps down the evolutionary escalator from me, and you're not going to be teaching me anything. I could tell Daddy right now that you're here and I'd never have to worry about you dragging dirt across my sheets again.");
                        princess.Dialogue.Add("But I'll tell you what, if you promise to be my personal servant forever I'll keep you out of trouble. I'll even give you my number so you can trick whoever put you up to this into thinking you're actually tutoring me.");
                        princess.Dialogue.Add(".........");
                        princess.Dialogue.Add("Alright, here, take it.");

                        princess.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    princess.UpdateInteraction();

                    if (princess.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        princess.ClearDialogue();
                        princess.Dialogue.Add("What are you doing? Take it. Haven't you ever gotten a girl's number before? Or anyone's? Put it in your phone.");
                        princess.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    princess.UpdateInteraction();

                    if (princess.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 3:

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:

                    if (firstFrameOfTheState)
                    {
                        princess.ClearDialogue();
                        princess.Dialogue.Add("...");
                        princess.Dialogue.Add("You don't have a phone do you?");
                        princess.Dialogue.Add("Oh my GOD. Why does Daddy let idiots like you into this school?? Ugh, how were you supposed to tutor me without a way to contact me? You realize I can't leave this room, right? Daddy would kill me!");
                        princess.Dialogue.Add("What are you still standing around for? Go get a phone!");

                        princess.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    princess.UpdateInteraction();

                    princess.UpdateInteraction();

                    if (princess.Talking == false)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] = true;
                        princess.ClearDialogue();
                        princess.Dialogue.Add("You're here and you don't have a phone. And my pillowcases are covered in vent dust. I might scream.");
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.SideQuestManager.sideQuestScenes = SideQuestManager.SideQuestScenes.none;
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
                case 2:
                case 3:
                case 4:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (princess != null && princess.Talking)
                    {
                        princess.DrawDialogue(s);
                    }
                    s.End();
                    break;
            }
        }
    }
}
