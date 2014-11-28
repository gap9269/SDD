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
    class GettingQuestTwoScene : Cutscene
    {
        GameObject camFollow;
        NPC alan;
        NPC paul;
        int talkingState;
        int specialTimer;

        public GettingQuestTwoScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
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
                        game.CurrentQuests.Clear();
                        Chapter.effectsManager.secondNotificationQueue.Clear(); //Clear this queue because a quest is added to it when we reward the player above
                        game.CurrentSideQuests.Remove((game.CurrentChapter as Prologue).QuestOne);
                        player.PositionX = game.CurrentChapter.CurrentMap.MapWidth - 600;
                        player.StoryItems.Remove("Piece of Paper");
                        player.Sprinting = false;
                        player.MoveFrame = 0;
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        paul.RecX = 2880;
                        paul.PositionX = 2880;
                        alan.RecX = 2700;
                        alan.PositionX = 2700;
                        alan.moveState = NPC.MoveState.standing;
                        paul.moveState = NPC.MoveState.standing;
                        paul.FacingRight = false;
                        alan.FacingRight = true;
                        alan.ClearDialogue();
                        paul.ClearDialogue();
                        paul.QuestDialogue.Clear();
                        paul.DialogueState = 0;
                        alan.Dialogue.Add("Don't blame me. I don't know what you did with it.");
                        paul.Dialogue.Add("You had them last!");
                        camFollow.PositionX = player.PositionX + 45;
                    }
                    camFollow.PositionX -= player.MoveSpeed;
                    player.UpdatePosition();
                    player.CutsceneRun(new Vector2(-player.MoveSpeed, 0));
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (player.PositionX <= 3200)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        state++;
                        alan.Talking = false;
                        paul.Talking = false;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        alan.FacingRight = true;
                        alan.ClearDialogue();
                        paul.ClearDialogue();
                        alan.Dialogue.Add("Wow, he actually came back.");
                        talkingState = 0;
                        alan.Talking = true;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    specialTimer++;
                    if (alan.Talking == true)
                    {
                        alan.UpdateInteraction();
                    }
                    else if (paul.Talking == true)
                    {
                        if (talkingState == 5 && paul.DialogueState == 1 || paul.DialogueState == 2)
                        {
                            paul.CurrentDialogueFace = "Arrogant";
                        }
                        else
                        {
                            paul.CurrentDialogueFace = "Normal";
                        }

                        paul.UpdateInteraction();

                    }

                    if (alan.Talking == false && paul.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();

                        if (specialTimer != 0 && talkingState != 2 && talkingState != 4)
                            specialTimer = 0;

                        if(talkingState != 2 && talkingState != 4)
                            talkingState++;

                        if (talkingState == 1)
                        {
                            paul.FacingRight = true;
                            paul.Talking = true;
                            paul.Dialogue.Add("He even got the paper and flowers, like some sort of Flower Boy or something.");
                            player.Karma = 1;
                            player.CheckSocialRankUp();
                        }

                        if (talkingState == 2 && Chapter.effectsManager.notificationQueue.Count == 0)
                        {
                            talkingState++;
                            alan.Dialogue.Add("While you were gone Mr. Robatto stopped by. I guess some dork named Daryl left his notebook in the Main Office.");
                            alan.Dialogue.Add("I don't know who Daryl is, and I don't really care, so I guess you can have it for getting us those flowers.");
                            alan.Talking = true;
                        }

                        if (talkingState == 4 && specialTimer > 120)
                        {
                            talkingState++;
                            paul.Dialogue.Add("I'm sure you're curious about what's so important about that paper.");
                            paul.Dialogue.Add("It's Tim's locker combination. We took it so we could give him fun gifts all the time. That's what friends do.");
                            paul.Dialogue.Add("And those flowers, you ask? Those flowers are exactly what Tim needs to spruce up his locker! Friends love flowers!");
                            paul.Dialogue.Add("Now go ahead and put those flowers in Tim's locker for him. It's over there.");
                            paul.Talking = true;
                        }

                        if (talkingState == 6)
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
                        //camFollow.PositionX = player.Rec.Center.X;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (camFollow.PositionX >= 1550)
                    {
                        camFollow.PositionX -= 12;
                    }
                    else
                    {
                        specialTimer++;
                    }

                    if (specialTimer >= 60)
                    {
                        state++;
                        timer = 0;
                        specialTimer = 0;
                    }
                    
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();
                        paul.AddQuest((game.CurrentChapter as Prologue).QuestTwo);
                        paul.Talking = true;
                    }

                    paul.Choice = 0;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    paul.UpdateInteraction();

                    if(paul.DialogueState == 1)
                        paul.CurrentDialogueFace = "Arrogant";
                    else
                        paul.CurrentDialogueFace = "Normal";

                    if (paul.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                    /*
                case 4:
                    if (topBarPos <= -66)
                    {
                        state++;
                        timer = 0;
                    }
                    break;*/
                case 4:

                    last = current;
                    current = Keyboard.GetState();
                    if (current.IsKeyUp(Keys.I) && last.IsKeyDown(Keys.I))
                    {
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("Tim loves flowers. He'll thank us.");
                        state = 0;
                        timer = 0;
                        game.Notebook.LoadContent();
                        game.CurrentChapter.state = Chapter.GameState.noteBook;
                        game.Notebook.Inventory.ResetInventoryBoxes();
                        game.Notebook.Inventory.ResetStoryBoxes();
                        Chapter.effectsManager.RemoveToolTip();
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


                    if (player.PositionX <= 3900)
                    {
                        paul.Talking = true;
                        paul.DrawDialogue(s);
                    }
                    else if (player.PositionX <= 4400)
                    {
                        alan.Talking = true;
                        alan.DrawDialogue(s);
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

                    if (alan.Talking == true)
                    {
                        alan.DrawDialogue(s);
                    }
                    else if (paul.Talking == true)
                    {
                        paul.DrawDialogue(s);
                    }

                    switch (talkingState)
                    {
                        case 2:
                            Chapter.effectsManager.Update();
                            Chapter.effectsManager.DrawNotification(s);
                            break;
                        case 4:
                            Chapter.effectsManager.DrawCutsceneItem(s,  "Daryl's Notebook");
                            break;
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

                    paul.DrawDialogue(s);

                    s.End();
                    break;

                    /*
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    DrawRemoveCutsceneBars(s);

                    s.End();
                    break;*/

                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(Game1.toolTipTexture, new Vector2(450, 300), Color.White);
                    s.DrawString(Game1.twConQuestHudName, "Press 'i' to open Daryl's Notebook", new Vector2(550, 345), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
