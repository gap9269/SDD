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
    class BaltosPhone : Cutscene
    {
        NPC alan;
        NPC paul;
        NPC balto;
        int talkingState;
        int specialTimer;
        GameObject camFollow;
        float fadeAlpha = 1f;

        public BaltosPhone(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.PositionX = 4600;
        }

        public override void SkipCutscene()
        {
            base.SkipCutscene();

            balto.PositionX = 2600;
            balto.moveState = NPC.MoveState.standing;
            balto.UpdateRecAndPosition();
            paul.Dialogue.Clear();
            paul.AddQuest(game.ChapterTwo.findBaltosPhone);
            paul.SkipQuestDialogue();
            alan.ClearDialogue();
            alan.Dialogue.Add("You're our first Junior Baby Intern, you know.");
            balto.ClearDialogue();
            balto.Dialogue.Add("Hey dork. Robatto went into the old History room over there. Go get my phone back, we need those ledgers!");
            state = 0;
            timer = 0;
            player.playerState = Player.PlayerState.standing;
            game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);
            game.Camera.center = game.Camera.centerTarget;
            game.CurrentChapter.CutsceneState++;
            game.CurrentChapter.state = Chapter.GameState.Game;


        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        timer = 1;
                        fadeAlpha = 1;
                        player.PositionY = 293;
                        player.MoveFrame = 0;
                        player.PositionX = 3000;
                        player.FacingRight = false;
                        player.UpdatePosition();
                        talkingState = 1;
                        player.StopSkills();

                        player.playerState = Player.PlayerState.relaxedStanding;
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        balto = game.CurrentChapter.NPCs["Balto"];

                        paul.FacingRight = true;
                        alan.FacingRight = true;


                        balto.PositionX = 200;
                        balto.UpdateRecAndPosition();

                        alan.ClearDialogue();
                        paul.ClearDialogue();
                        balto.ClearDialogue();

                        alan.Dialogue.Add("...and you haven't put in any extra hours, which is cause for another demotion.");

                        alan.DialogueState = 0;
                        paul.Dialogue.Add("Let's see, what else? It says here that you've only met 6% of your textbook quota for the week. Well that's no good.");
                        //alan.Talking = true;
                    }

                    if (timer >80)
                    {
                        if (fadeAlpha > 0)
                            fadeAlpha -= 1f / 90f;
                    }

                    if (balto.PositionX > 2250 && talkingState != 2 )
                    {
                        talkingState = 2;
                        alan.ClearDialogue();
                        alan.Talking = true;
                        alan.Dialogue.Add("Hmmm...helping other students while on the job, and I see that the Employee Task List has barely been touched. Is that perfume I smell on you?");

                    }

                    if (balto.PositionX < 2600)
                    {
                        balto.Move(new Vector2(2, 0));
                        camFollow.PositionX = balto.PositionX + 250;
                    }
                    else
                    {
                        balto.PositionX = 2600;
                        balto.moveState = NPC.MoveState.standing;
                        balto.UpdateRecAndPosition();

                        if (camFollow.PositionX < 3035)
                            camFollow.PositionX += 2;
                        else
                            specialTimer++;
                    }

                    //if (camFollow.PositionX > 3200)
                    //{
                    //    camFollow.PositionX -= 1;
                    //}

                    //alan.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (specialTimer >= 60)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        alan.Talking = false;
                        talkingState = 0;
                        alan.ClearDialogue();
                        paul.ClearDialogue();
                        paul.Dialogue.Add("Did I see you gambling yesterday on the outcome of today's Homecoming game? Gambling on the job when you were supposed to be getting Book Spray, huh?");
                        paul.Dialogue.Add("I'm afraid we're going to have to demote you down to, \"Junior Baby Intern\" status until you get your act together, D--");
                        talkingState = 0;
                        paul.Talking = true;
                        balto.Talking = false;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (talkingState == 0 && paul.DialogueState == 1)
                        paul.CurrentDialogueFace = "Arrogant";
                    else
                        paul.CurrentDialogueFace = "Normal";

                    if (talkingState == 16 && alan.DialogueState == 1)
                        alan.CurrentDialogueFace = "Arrogant";
                    else
                        alan.CurrentDialogueFace = "Normal";

                    if (alan.Talking == true)
                    {
                        alan.UpdateInteraction();
                    }
                    else if (paul.Talking == true)
                        paul.UpdateInteraction();
                    else if (balto.Talking)
                        balto.UpdateInteraction();

                    if (alan.Talking == false && paul.Talking == false && balto.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();
                        balto.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("Welp, I lost the ledgers.");
                            alan.FacingRight = false;
                        }

                        if (talkingState == 2)
                        {
                            paul.Dialogue.Add("What?");
                            paul.FacingRight = false;
                            paul.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("Our ledgers, for the Textbook business.");
                        }

                        if (talkingState == 4)
                        {
                            paul.Dialogue.Add("Our ledgers?");
                            paul.Talking = true;
                        }

                        if (talkingState == 5)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("I lost them.");
                        }

                        if (talkingState == 6)
                        {
                            alan.Talking = true;
                            alan.Dialogue.Add("What do you mean you lost them?");
                        }

                        if (talkingState == 7)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("Well, I keep them all on my phone. Easy mobile access and stuff. You know. We don't want a paper trail.");
                            balto.Dialogue.Add("Mr. Robatto caught me updating them this morning and he told me that phones aren't allowed during school hours. I tried telling him it wasn't a phone, it was a study machine.");
                        }

                        if (talkingState == 8)
                        {
                            paul.Dialogue.Add("And it didn't work?");
                            paul.Talking = true;
                        }

                        if (talkingState == 9)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("I don't know, he asked me if I had a hall pass. I told him my phone was a special hall pass, but he just took it and left.");
                        }

                        if (talkingState == 10)
                        {
                            alan.Talking = true;
                            alan.Dialogue.Add("Well you deserve it for being an idiot. We don't even have ledgers.");
                        }

                        if (talkingState == 11)
                        {
                            paul.Dialogue.Add("Yeah, that's what you get for breaking the rules. Get a new phone, fat ass.");
                            paul.Talking = true;
                        }

                        if (talkingState == 12)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("Hey screw you, my phone was taken because of you two. This is your fault.");
                        }

                        if (talkingState == 13)
                        {
                            paul.Dialogue.Add("Our fault?! We didn't tell you to wave around your phone in front of Robatto.");
                            paul.Talking = true;
                        }
                        if (talkingState == 14)
                        {
                            alan.Talking = true;
                            alan.Dialogue.Add("Or use your phone at all, actually.");
                        }
                        if (talkingState == 15)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("I don't care. I want my phone back or I'm done with you two. Hey, is this that new Dalton kid you guys said works for us now?");
                        }
                        if (talkingState == 16)
                        {
                            alan.Talking = true;
                            alan.Dialogue.Add("Yeah this is him. And so far he's been good for nothing and has brought us hardly any product.");
                            alan.Dialogue.Add("Hey, he could get your phone back. He has to do what we say now that he's just a Junior Baby Intern.");
                        }
                        if (talkingState == 17)
                        {
                            paul.Dialogue.Add("Do you have any idea where Robatto went, Balto?");
                            paul.Talking = true;
                        }
                        if (talkingState == 18)
                        {
                            balto.Talking = true;
                            balto.Dialogue.Add("Hmmm... I think he went into the old History Room after he took my phone.");
                        }
                        if (talkingState == 19)
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
                        balto.Dialogue.Clear();
                        paul.AddQuest(game.ChapterTwo.findBaltosPhone);
                        paul.Talking = true;
                        timer = 1;
                        paul.FacingRight = true;
                    }

                    if (timer == 55)
                        alan.FacingRight = true;

                    paul.Choice = 0;
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    paul.UpdateInteraction();

                    if (paul.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("You're our first Junior Baby Intern, you know.");
                        balto.Dialogue.Clear();
                        balto.Dialogue.Add("Hey dork. Robatto went into the old History room over there. Go get my phone back, we need those ledgers!");
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

                    if (timer > 320)
                    {
                        if (balto.PositionX <= 1525)
                        {
                            alan.Talking = true;
                            alan.DrawDialogue(s);
                        }
                        else if (balto.PositionX <= 2250)
                        {
                            alan.Talking = false;
                            paul.Talking = true;
                            paul.DrawDialogue(s);
                        }
                        else
                        {
                            paul.Talking = false;
                            alan.Talking = true;
                            alan.DrawDialogue(s);
                        }
                    }
                    if (fadeAlpha > 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * fadeAlpha);
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
                    else if (balto.Talking == true)
                    {
                        balto.DrawDialogue(s);
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
                    paul.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
