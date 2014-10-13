using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class GettingQuestOne : Cutscene
    {
        NPC alan;
        NPC paul;
        int talkingState;
        int specialTimer;

        public GettingQuestOne(Game1 g, Camera cam, Player p)
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
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        alan.Dialogue.Clear();
                        alan.QuestDialogue.Clear();
                        paul.Dialogue.Clear();
                        paul.QuestDialogue.Clear();

                        paul.Dialogue.Add("Oh thank god! See Alan? I told you Tim didn't kill him.");
                        paul.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    paul.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (paul.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        alan.ClearDialogue();
                        paul.ClearDialogue();
                        alan.Dialogue.Add("That's a damn lie. You were convinced that Tim did him in for putting those flowers in his locker. ");
                        talkingState = 0;
                        alan.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (alan.Talking == true)
                    {
                        alan.UpdateInteraction();
                    }
                    else if (paul.Talking == true)
                        paul.UpdateInteraction();

                    if (alan.Talking == false && paul.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            paul.Talking = true;
                            paul.Dialogue.Add("Alan, I'd love to argue about what I may or may not have said prior to our friend here reappearing \nvery much alive, but there are important matters to discuss.");
                            paul.Dialogue.Add("Derek did you hear that announcement just now? He knows we stole the key to the janitor's closet. \nNow I don't know about you, but seeing as you are the one currently in possession of that key and as you \nwere the one to use it to break into the janitor's closet, I do believe that you are the \none responsible for it.");
                        }

                        if (talkingState == 2)
                        {
                            alan.Dialogue.Add("It's really only fair.");
                            alan.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            paul.Dialogue.Add("That it is, Alan. I know how much you want to be our new friend, but how could we be if you were to \nallow us to be severely punished by the administration just because you didn't feel like returning that key to the janitor's closet?");
                            paul.Dialogue.Add("I can just see the demerits flowing now. My poor mother would be so upset");
                            paul.Talking = true;
                        }
                        if (talkingState == 4)
                        {
                            state++;
                            timer = 0;
                            specialTimer = 0;
                        }
                    }

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();
                        alan.AddQuest(game.ChapterOne.ReturningKeys);
                        alan.Talking = true;
                    }

                    alan.Choice = 0;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    alan.UpdateInteraction();

                    if (alan.Talking == false)
                    {
                        paul.Dialogue.Clear();
                        paul.Dialogue.Add("Please tell me you've returned the key.");
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
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
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer > 1)
                        paul.DrawDialogue(s);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);


                    if (alan.Talking == true)
                    {
                        alan.DrawDialogue(s);
                    }
                    else if (paul.Talking == true)
                    {
                        paul.DrawDialogue(s);
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

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    alan.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
