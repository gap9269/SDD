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
                        alan.RemoveQuest(game.ChapterOne.ReturningKeys);
                        alan.ClearDialogue();
                        paul.ClearDialogue();

                        alan.Dialogue.Add("Finally. I'm glad you decided to show up to your second day of work, albeit late and wearing the same clothes as yesterday.");
                        alan.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    alan.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (alan.Talking == false)
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
                        paul.Dialogue.Add("Your crime of bad fashion and hygiene isn't the only one either. You left us in a real pickle yesterday when you left us alone to argue with Tim. I wouldn't do that to a friend. Would you, Alan?");
                        talkingState = 0;
                        paul.Talking = true;
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
                            alan.Talking = true;
                            alan.Dialogue.Add("I wouldn't dream of it. You definitely owe it to us now to be the best employee that you can be.");
                        }

                        if (talkingState == 2)
                        {
                            paul.Dialogue.Add("Devon, being a part of this business isn't going to be easy. We're a fragile Start-Up in a crowded market that's fit to burst. Trenchcoat Kid isn't going to show us mercy, and we need to offer him the same kindness.");
                            paul.Dialogue.Add("Our only hope is to offer a product that is cheaper, cleaner, and of a higher quality than the junk that he sells.");
                            paul.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            alan.Talking = true;
                            alan.Dialogue.Add("Speaking of clean, I think some of your dirt rubbed off on the textbook that you brought us yesterday. Did you drop it in a mud puddle while you were coming back with it?");
                        }

                        if (talkingState == 4)
                        {
                            paul.Dialogue.Add("What Alan means to say is that the product you brought us yesterday is filthy. Whether you ruined it through some sort of osmosis or not is irrelevant...what's important is that we can't sell it like this. What would my mother think if she saw me selling dirty textbooks?");
                            paul.Dialogue.Add("You're going to need to fix all of these mistakes you made or we simply can't be friends. Why don't you take that key you have to the Janitor's Closet and find us some Book Spray? That should fix this textbook up, good as new.");

                            paul.Talking = true;
                        }

                        if (talkingState == 5)
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
                        paul.Dialogue.Add("If you want to be friends so badly, stop staring at me and find us some book spray.");
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
                        alan.DrawDialogue(s);
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
