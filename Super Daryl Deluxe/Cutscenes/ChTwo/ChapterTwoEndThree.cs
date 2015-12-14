using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class ChapterTwoEndThree : Cutscene
    {
        // ATTRIBUTES \\
        NPC alan, paul, balto, chelsea;
        GameObject camFollow;
        TheJanitor janitor;
        int talkingState = 0;
        int specialTimer = 0;
        Texture2D noteTexture;
        public ChapterTwoEndThree(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            game.NPCSprites["Chelsea"] = content.Load<Texture2D>(@"NPC\Main\Chelsea");
            Game1.npcFaces["Chelsea"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Chelsea");

            game.NPCSprites["The Janitor"] = content.Load<Texture2D>(@"NPC\Main\The Janitor");

            noteTexture = content.Load<Texture2D>(@"cutscenes\janitorNote");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            game.NPCSprites["Chelsea"] = Game1.whiteFilter;
            Game1.npcFaces["Chelsea"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["The Janitor"] = Game1.whiteFilter;


        }
        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (camFollow.PositionX != 3226)
                    {
                        camFollow.PositionX = 3226;

                        game.Camera.centerTarget = new Vector2(camFollow.PositionX, 0);
                        game.Camera.center = game.Camera.centerTarget;

                        player.PositionY = 309;
                        player.UpdatePosition();

                        janitor = game.ChapterTwo.NPCs["The Janitor"] as TheJanitor;
                        chelsea = game.ChapterTwo.NPCs["Chelsea"];
                        balto = game.ChapterTwo.NPCs["Balto"];
                        alan = game.ChapterTwo.NPCs["Alan"];
                        paul = game.ChapterTwo.NPCs["Paul"];

                        paul.CompleteQuestSilently(game.ChapterTwo.findBaltosPhone);
                        paul.ClearDialogue();
                        paul.Position = new Vector2(2870, 275);
                        paul.FacingRight = true;
                        paul.moveState = NPC.MoveState.standing;
                        paul.UpdateRecAndPosition();

                        alan.ClearDialogue();
                        alan.Position = new Vector2(2770, 298);
                        alan.FacingRight = true;
                        alan.moveState = NPC.MoveState.standing;
                        alan.UpdateRecAndPosition();

                        balto.ClearDialogue();
                        balto.Position = new Vector2(2848, 309);
                        balto.FacingRight = true;
                        balto.moveState = NPC.MoveState.standing;
                        balto.UpdateRecAndPosition();

                        LoadContent();
                        chelsea.ClearDialogue();
                        chelsea.MapName = "North Hall";
                        chelsea.Dialogue.Add("Are you guys coming tonight? We bought enough beer to get an elephant drunk.");
                        chelsea.Dialogue.Add("The barn is all cleaned up, too. Remember how crazy it got for homecoming last year?");

                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    FadeIn(120);
                    break;
                case 1:

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (talkingState > 5)
                    {
                        if (player.PositionX > 3068)
                        {
                            player.CutsceneWalk(new Vector2(-4, 0));
                        }
                    }

                    if (timer == 20)
                        chelsea.Talking = true;

                    if (timer > 20)
                    {
                        if (chelsea.Talking == true)
                            chelsea.UpdateInteraction();
                        else if (alan.Talking)
                            alan.UpdateInteraction();
                        else if (paul.Talking)
                            paul.UpdateInteraction();
                        else if (balto.Talking)
                            balto.UpdateInteraction();
                        if (chelsea.Talking == false && alan.Talking == false && paul.Talking == false && balto.Talking == false)
                        {
                            balto.Dialogue.Clear();
                            chelsea.Dialogue.Clear();
                            paul.Dialogue.Clear();
                            alan.Dialogue.Clear();

                            talkingState++;

                            if (talkingState == 1)
                            {
                                alan.Talking = true;
                                alan.Dialogue.Add("I'll be there. I'm not missing a chance to see Balto fall off the roof again.");
                            }

                            if (talkingState == 2)
                            {
                                balto.Dialogue.Add("Hey, fuck you. That wasn't my fault. It was because the roof was slippery from rain.");
                                balto.Talking = true;
                            }
                            if (talkingState == 3)
                            {
                                paul.Talking = true;
                                paul.Dialogue.Add("I'll be there if I can catch a ride from someone. It sucks that your house is so far out in the middle of no where.");
                            }
                            if (talkingState == 4)
                            {
                                balto.Talking = true;
                                balto.Dialogue.Add("I bet we could get the new kid to figure it out for you.");
                            }
                            if (talkingState == 5)
                            {
                                paul.Talking = true;
                                paul.Dialogue.Add("Yeah...maybe. I dunno, he's kind of an idiot. I'm definitely not going to let him drive me there, that's for sure. We'd end up in ditch, or more likely he would just sit in the driver's seat and stare at me without moving the car.");
                            }
                            if (talkingState == 6)
                            {
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("Speaking of, where is he? He's been here for what, three days now? And I haven't seen him at all.");
                            }
                            if (talkingState == 7)
                            {
                                state++;
                                timer = 0;
                                talkingState = 0;
                            }
                        }
                    }

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        paul.ClearDialogue();

                        paul.Dialogue.Add("Well it looks like it's your lucky day, Chelsea.");
                        paul.Talking = true;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);


                    if (paul.Talking)
                        paul.UpdateInteraction();
                    if (player.PositionX > 3068)
                    {
                        player.CutsceneWalk(new Vector2(-4, 0));
                    }
                    else
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        if (!paul.Talking)
                        {
                        chelsea.FacingRight = true;
                        state++;
                        timer = 0;
                        }
                    }
                    break;
                case 3:

                    if (firstFrameOfTheState)
                    {
                        balto.ClearDialogue();

                        balto.Dialogue.Add("There you are! Where's my phone?");
                        balto.Talking = true;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    balto.UpdateInteraction();

                    if (!balto.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;
                case 5:

                    if (firstFrameOfTheState)
                    {
                        balto.ClearDialogue();
                        balto.Dialogue.Add("Goddamn it!");
                        balto.Talking = true;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                        if (chelsea.Talking == true)
                            chelsea.UpdateInteraction();
                        else if (alan.Talking)
                            alan.UpdateInteraction();
                        else if (paul.Talking)
                            paul.UpdateInteraction();
                        else if (balto.Talking)
                            balto.UpdateInteraction();
                        if (chelsea.Talking == false && alan.Talking == false && paul.Talking == false && balto.Talking == false)
                        {
                            balto.Dialogue.Clear();
                            chelsea.Dialogue.Clear();
                            paul.Dialogue.Clear();
                            alan.Dialogue.Clear();

                            talkingState++;

                            if (talkingState == 1)
                            {
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("Does...does he talk?");
                            }

                            if (talkingState == 2)
                            {
                                alan.Dialogue.Add("Nope. He's good at taking orders, though. Even if he's just a Junior Baby Intern.");
                                alan.Talking = true;
                            }
                            if (talkingState == 3)
                            {
                                balto.Talking = true;
                                balto.Dialogue.Add("Who cares! He has my fucking phone! I can see it sticking out of his pocket!");
                                balto.Dialogue.Add("Give it back, nerd! Our ledgers are on that and I'm behind on them now!");
                            }
                            if (talkingState == 4)
                            {
                                chelsea.FacingRight = false;
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("Ledgers?");
                            }
                            if (talkingState == 5)
                            {
                                paul.Talking = true;
                                paul.Dialogue.Add("Uh-huh, yeah, whatever Balto. Shut up for a second.");
                                paul.Dialogue.Add("Junior Baby Intern, for completing your task you've been promoted to On-Call Duty. That means you have to do work for us when you're not at school, also.");
                                paul.Dialogue.Add("Your first assignment is to attend Chelsea's Homecoming party tonight. There are bound to be a ton of drunk kids that would love nothing more than to buy some textbooks from us.");
                            }
                            if (talkingState == 6)
                            {
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("What the Hell? Don't just go inviting your weird friend to my house! And I don't want any of you trying to run your stupid business out of my barn.");
                            }
                            if (talkingState == 7)
                            {
                                balto.Talking = true;
                                balto.Dialogue.Add("Yeah! And make him give me my phone back!");
                            }
                            if (talkingState == 8)
                            {
                                alan.Dialogue.Add("Chelsea, relax. Paul and I are master salesmen, you won't even know we're there for business.");
                                alan.Dialogue.Add("As for him, well, he doesn't really say or do much so it'll probably be like having another chair or something around.");
                                alan.Talking = true;
                            }
                            if (talkingState == 9)
                            {
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("Ugh...fine.");
                                chelsea.Dialogue.Add("Just don't creep anyone out, alright? This is a good chance to make some real friends, instead of sticking around with these assholes.");
                                chelsea.Dialogue.Add("I'll text the address to Balto's phone for you. Party starts at nine.");
                            }
                            if (talkingState == 10)
                            {
                                balto.Talking = true;
                                balto.Dialogue.Add("Goddamn it!");
                            }
                            if (talkingState == 11)
                            {
                                state++;
                                timer = 0;
                                talkingState = 0;
                            }
                        }

                        if (talkingState == 9)
                        {
                            if (chelsea.DialogueState > 0)
                                chelsea.FacingRight = true;
                        }

                    break;
                case 6:
                    if (firstFrameOfTheState)
                    {
                        janitor.MapName = "North Hall";
                        janitor.RideZamboni(3.5f);
                    }
                    
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (janitor.PositionX < 3085 || specialTimer > 120)
                    {
                        janitor.RideZamboni(3.5f);
                    }
                    else
                    {
                        chelsea.FacingRight = true;
                        specialTimer++;
                    }
                    if (janitor.PositionX > 3820)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 7:
                    if (firstFrameOfTheState)
                    {
                        balto.ClearDialogue();
                        balto.Dialogue.Add("Who the hell was that?");
                        balto.Talking = true;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                        if (chelsea.Talking == true)
                            chelsea.UpdateInteraction();
                        else if (alan.Talking)
                            alan.UpdateInteraction();
                        else if (paul.Talking)
                            paul.UpdateInteraction();
                        else if (balto.Talking)
                            balto.UpdateInteraction();
                        if (chelsea.Talking == false && alan.Talking == false && paul.Talking == false && balto.Talking == false)
                        {
                            balto.Dialogue.Clear();
                            chelsea.Dialogue.Clear();
                            paul.Dialogue.Clear();
                            alan.Dialogue.Clear();

                            talkingState++;

                            if (talkingState == 1)
                            {
                                paul.Talking = true;
                                paul.Dialogue.Add("I have no idea. He handed a piece of paper to Danny.");
                                paul.Dialogue.Add("What's it say?");
                            }
                            
                            if (talkingState == 2)
                            {
                                state++;
                                timer = 0;
                                talkingState = 0;
                            }
                        }
                    break;
                case 8:
                    if (timer > 350)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 9:
                    if (firstFrameOfTheState)
                    {
                        alan.ClearDialogue();
                        alan.Dialogue.Add("It's probably a love note or something.");
                        alan.Dialogue.Add("Hey, is that your new boyfriend? You know you don't have time for that kind of stuff when you're supposed to be finding textbooks for us.");
                        alan.Talking = true;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                        if (chelsea.Talking == true)
                            chelsea.UpdateInteraction();
                        else if (alan.Talking)
                            alan.UpdateInteraction();
                        else if (paul.Talking)
                            paul.UpdateInteraction();
                        else if (balto.Talking)
                            balto.UpdateInteraction();
                        if (chelsea.Talking == false && alan.Talking == false && paul.Talking == false && balto.Talking == false)
                        {
                            balto.Dialogue.Clear();
                            chelsea.Dialogue.Clear();
                            paul.Dialogue.Clear();
                            alan.Dialogue.Clear();

                            talkingState++;

                            if (talkingState == 1)
                            {
                                chelsea.Talking = true;
                                chelsea.Dialogue.Add("Oh, leave him alone. He's embarrassed, just look at him.");
                                chelsea.Dialogue.Add("Christ, guys...");
                            }
                            
                            if (talkingState == 2)
                            {
                                state++;
                                timer = 0;
                                talkingState = 0;
                            }
                        }
                    break;
                case 10:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    FadeOut(120);
                    break;
                case 11:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60)
                    {
                        state = 0;
                        timer = 0;
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
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if(janitor != null)
                        janitor.Draw(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (balto != null && balto.Talking)
                        balto.DrawDialogue(s);

                    if (paul != null && paul.Talking)
                        paul.DrawDialogue(s);

                    if (alan != null && alan.Talking)
                        alan.DrawDialogue(s);

                    if (chelsea != null && chelsea.Talking)
                        chelsea.DrawDialogue(s);

                    if (state == 0)
                        DrawFade(s, 1);
                    if (state == 10)
                        DrawFade(s, 0);

                    if(state == 0 && timer < 10)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);

                    if (state == 8)
                        s.Draw(noteTexture, new Rectangle(0, 0, 1280, 720), Color.White);

                    s.End();
                    break;

                case 11:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
