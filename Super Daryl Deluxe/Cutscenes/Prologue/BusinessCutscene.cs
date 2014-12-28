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
    class BusinessCutscene:Cutscene
    {
        NPC alan;
        NPC paul;
        int talkingState;
        int specialTimer;

        public BusinessCutscene(Game1 g, Camera cam, Player p)
            : base(g,cam,p)
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
                        game.CurrentQuests[0].RewardPlayer();
                        Game1.questHUD.RemoveQuestFromHelper(game.CurrentQuests[0]);
                        game.CurrentQuests.Remove(game.AllQuests["Flower Delivery"]);
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        alan.Dialogue.Clear();
                        //alan.QuestDialogue.Clear();
                        paul.DialogueState = 0;
                        paul.ClearDialogue();
                        paul.QuestDialogue.Clear();
                        alan.Dialogue.Add("Tim didn't see you, right?");
                        talkingState = 0;
                        alan.Talking = true;

                        float playerPosX = player.VitalRec.Center.X;

                        if (alan.Rec.Center.X < playerPosX)
                            alan.FacingRight = true;
                        if (paul.Rec.Center.X < playerPosX)
                            paul.FacingRight = true;
                    }
                    if (player.playerState != Player.PlayerState.jumping)
                        player.playerState = Player.PlayerState.relaxedStanding;

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    player.CanJump = false;
                    specialTimer++;
                    player.Update();

                    if (alan.Talking == true)
                    {
                        if ((talkingState == 3 && alan.DialogueState == 1) || (talkingState == 2 && alan.DialogueState == 2) || (talkingState == 8 && alan.DialogueState > 1))
                            alan.CurrentDialogueFace = "Arrogant";
                        else
                            alan.CurrentDialogueFace = "Normal";

                        alan.UpdateInteraction();


                    }
                    else if (paul.Talking == true)
                    {
                        if ((talkingState == 1 && paul.DialogueState == 1) || (talkingState == 2 && paul.DialogueState == 2) || (talkingState == 5 && paul.DialogueState > 0))
                            paul.CurrentDialogueFace = "Arrogant";
                        else if(talkingState == 9)
                            paul.CurrentDialogueFace = "Fonz";
                        else
                            paul.CurrentDialogueFace = "Normal";

                        paul.UpdateInteraction();
                    }



                    if (specialTimer != 0 && talkingState != 4 && talkingState != 6)
                        specialTimer = 0;

                    if (alan.Talking == false && paul.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();

                        if(talkingState != 4 && talkingState != 6)
                            talkingState++;

                        if (talkingState == 1)
                        {
                            paul.Talking = true;

                            paul.Dialogue.Add("I don't like your face. You're judging us...");
                            paul.Dialogue.Add("I'll have you know that we surprise Tim with flowers all the time, and he thanks us -every- time. Friends love flowers!");
                        }

                        else if (talkingState == 2)
                        {
                            paul.CurrentDialogueFace = "Normal";
                            paul.Dialogue.Add("But maybe you're right. Maybe breaking into people's lockers is wrong without their permission.");
                            paul.Dialogue.Add("However, is it so wrong if it encourages good character? If it makes their lockers smell pretty and look nice? Dammit, it would be wrong not to!");
                            paul.Dialogue.Add("I'm glad we could discuss our differences.");
                            paul.Talking = true;
                        }

                        else if (talkingState == 3)
                        {
                            alan.Dialogue.Add("This is going to be a good potential friendship. However, if you want to fit in around here you have to learn how to interact with others besides us.");
                            alan.Dialogue.Add("Luckily we have just the thing for that. And as a first time customer, we'll give it to you for free!");
                            alan.Talking = true;
                        }

                        else if (talkingState == 4 && specialTimer >= 120)
                        {
                            talkingState++;
                            paul.Dialogue.Add("That's right, Alan! You're standing in the presence of Water Falls High School's two newest aspiring entrepreneurs! That book will teach you just what you need to know to make friends.");
                            paul.Dialogue.Add("Now, as good businessmen we went ahead and ripped out all of the pages for you. If you want the good stuff, you'll have to help us nurture our young, fragile business.");
                            paul.Dialogue.Add("Since you obviously know how to discuss differences, we'll just give you that first page for free.");
                            player.LearnedSkills.Add(SkillManager.AllSkills["Discuss Differences"]);
                            paul.Talking = true;

                        }
                        else if (talkingState == 6 && specialTimer >= 120)
                        {
                            talkingState++;
                            paul.Dialogue.Add("You see, right now textbooks are a hot commodity. In fact our rival, the Trenchcoat kid, has cronies all over the school right now pawning off textbooks at a price that we are willing to beat.");
                            paul.Talking = true;
                        }

                        else if (talkingState == 8)
                        {
                            alan.Dialogue.Add("The problem is, Darren, we don't have any textbooks.");
                            alan.Dialogue.Add("To jumpstart our company, you (our employee) needs to get us some product. See, Trenchcoat Kid and his cronies have all of the easy sources on lockdown...");
                            alan.Dialogue.Add("But last year the school was remodeled and all the old classrooms were locked down. It was done so quickly, I bet they didn't even clean the rooms out. The old Science Room should have some textbooks still.");
                            alan.Dialogue.Add("You get us some of those books and we'll give you some pages to that book we gave you, and you'll make a ton of friends. You'll be The Fonz in no time!");
                            alan.Talking = true;
                        }
                        else if (talkingState == 9)
                        {
                            paul.Dialogue.Add("...Eyyyy");
                            paul.Talking = true;
                        }
                        else if (talkingState == 10)
                        {
                            alan.CurrentDialogueFace = "Normal";
                            paul.CurrentDialogueFace = "Normal";
                            state++;
                            timer = 0;
                        }
                    }
                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Clear();
                        alan.Dialogue.Clear();
                        paul.AddQuest((game.CurrentChapter as Prologue).QuestThree);
                        paul.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    paul.UpdateInteraction();

                    if (paul.Talking == false)
                    {
                        game.CurrentChapter.HUD.SkillsHidden = false;
                        player.CanJump = true;
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("The science room is two doors down.");
                        state = 0;
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
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

                     

                    if (alan != null)
                    {
                        if (alan.Talking == true)
                        {
                            alan.DrawDialogue(s);
                        }

                        else if (paul.Talking == true)
                        {
                            paul.DrawDialogue(s);
                        }
                    }

                    switch (talkingState)
                    {
                        case 4:
                            s.Draw(Game1.youFoundItemTexture, new Rectangle(440, 300, 400, Game1.youFoundItemTexture.Height), Color.White);

                            Vector2 meas = Game1.twConRegularSmall.MeasureString("'How to Interact with Others'!");

                            s.DrawString(Game1.twConRegularSmall, "You found a Textbook: ", new Vector2(650 - Game1.twConRegularSmall.MeasureString("You found a Textbook: ").X / 2, 330), Color.Black);
                            s.DrawString(Game1.twConRegularSmall, "'How to Interact with Others'!", new Vector2(650 - (int)(meas.X / 2), 355), Color.Black);
                            break;
                        case 6:
                            s.Draw(Game1.youFoundItemTexture, new Rectangle(485, 300, 310, Game1.youFoundItemTexture.Height), Color.White);

                            Vector2 meas2 = Game1.twConRegularSmall.MeasureString("'Discuss Differences'!");

                            s.DrawString(Game1.twConRegularSmall, "You found a Skill Page:", new Vector2(645 - Game1.twConRegularSmall.MeasureString("You found a Skill Page:").X / 2, 330), Color.Black);
                            s.DrawString(Game1.twConRegularSmall, "'Discuss Differences'!", new Vector2(645 - (int)(meas2.X / 2), 355), Color.Black);
                            break;
                    }
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
                     
                    paul.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
