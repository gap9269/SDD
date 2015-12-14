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
    class EmergencyFootballPractice : Cutscene
    {
        NPC chad;
        NPC savingInstructor;
        int talkingState;
        int specialTimer;

        public EmergencyFootballPractice(Game1 g, Camera cam, Player p)
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
                        chad = game.CurrentChapter.NPCs["Chad Champson"];
                        savingInstructor = game.CurrentChapter.NPCs["Saving Instructor"];
                        chad.ClearDialogue();
                        savingInstructor.ClearDialogue();
                        player.playerState = Player.PlayerState.relaxedStanding;

                        dialogue.Add("Attention all members of the Varsity Football Team: There is an emergency practice session to be held in the gymnasium in five minutes. Please report there immediately.");
                        dialogueState = 0;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer >= 340)
                    {
                        state ++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        chad.Dialogue.Add("Woh-oh. Looks like we gotta go, boys and girls. Guess the Coach wants us to run some plays before our inevitable victory tomorrow, eh?");
                        chad.Talking = true;
                        talkingState = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (chad.Talking == true)
                    {
                        chad.UpdateInteraction();
                    }
                    else if (savingInstructor.Talking == true)
                        savingInstructor.UpdateInteraction();

                    if (chad.Talking == false && savingInstructor.Talking == false)
                    {
                        savingInstructor.Dialogue.Clear();
                        chad.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            savingInstructor.Talking = true;
                            savingInstructor.Dialogue.Add("Aww, no! Don't leave us, Chad!");
                        }

                        if (talkingState == 2)
                        {
                            chad.Dialogue.Add("Hahaha, no, I must do my duty as the quarterback of the WFHS Salmon. Who else is going to lead our team to fame? See you guys on the other side...of victory!");
                            chad.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            state++;
                            timer = 0;
                            specialTimer = 0;
                        }
                    }

                    break;
                case 2:
                    FadeOut(50);
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        game.ChapterOne.ChapterOneBooleans["homecomingHypeEnded"] = true;
                        game.ChapterOne.SwitchOutOfHomecomingHypeState();
                    }
                    if(timer > 10)
                        FadeIn(50);
                    break;

                case 4:
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
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
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer > 1)
                        DrawDialogue(s, false);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);


                    if (chad.Talking == true)
                    {
                        chad.DrawDialogue(s);
                    }
                    else if (savingInstructor.Talking == true)
                    {
                        savingInstructor.DrawDialogue(s);
                    }
                    s.End();
                    break;
                case 2:
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
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 1);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    break;
            }
        }
    }
}
