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
    class WeddingCrasher : Cutscene
    {
        NPC cleopatra, pastorGoblin, caesar;
        HistoryHologram timeLord;
        GameObject camFollow;
        int talkingState;

        public WeddingCrasher(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Game1.npcFaces["Pastor Goblin"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pastor Goblin Normal");
            
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Game1.npcFaces["Pastor Goblin"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void Play()
        {
            base.Play();

            if (timeLord != null)
                timeLord.Update();

            switch (state)
            {

                case 0:
                    if (firstFrameOfTheState)
                    {

                        player.PositionX = 150;
                        player.FacingRight = true;
                        player.UpdatePosition();
                        LoadContent();
                        cleopatra = game.CurrentChapter.NPCs["Cleopatra"];
                        pastorGoblin = new NPC(Game1.whiteFilter, game, "Pastor Goblin");
                        timeLord = game.CurrentChapter.NPCs["Time Lord"] as HistoryHologram;

                        camFollow = new GameObject();
                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.PositionY = player.VitalRec.Center.Y;
                    }

                    player.CutsceneStand();

                    if(timer > 20 && camFollow.PositionX < 2200)
                        camFollow.PositionX += 10;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (camFollow.PositionX >= 2200)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        pastorGoblin.Dialogue.Add("Blrghk Dearly beloved...brghmmgrb we are gathered here on this beautiful day to witness the union of Cleopatra and brghgmm...");
                        pastorGoblin.Talking = true;
                    }

                    if (pastorGoblin.Talking == true)
                        pastorGoblin.UpdateInteraction();

                    else if (timeLord.Talking == true)
                        timeLord.UpdateInteraction();

                    if (pastorGoblin.Talking == false && timeLord.Talking == false)
                    {
                        timeLord.Dialogue.Clear();
                        pastorGoblin.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            timeLord.Dialogue.Add("TIME LORD");
                            timeLord.Talking = true;
                        }

                        if (talkingState == 2)
                        {
                            pastorGoblin.Dialogue.Add("Time Lord in holy brghmmmatrimony... This is a day of great celebrgh-");
                            pastorGoblin.Dialogue.Add("celebrrgh...");
                            pastorGoblin.Dialogue.Add("grumble blrrgh... CELEbration and reverence, on which we blaugh come together before brargh God-");
                            pastorGoblin.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            timeLord.Talking = true;
                            timeLord.Dialogue.Add("Before ME! As we come together before Time Lord! God is nothing before Time Lord! Continue with the sermon, swine!");
                        }
                        if (talkingState == 4)
                        {
                            pastorGoblin.Dialogue.Add("Blagck...blaugh...uhh...Dearly beloved, we are gathered here tod-");
                            pastorGoblin.Talking = true;
                        }
                        if (talkingState == 5)
                        {
                            timeLord.Talking = true;
                            timeLord.Dialogue.Add("Oh for God's sake, just skip to the vows!");
                        }
                        if (talkingState == 6)
                        {
                            pastorGoblin.Dialogue.Add("mmmblaugh...blaghk...May you all remember and cherish this grumble grumble sacred ceremony, for... on this day, with blrghk love, we will forever bind Time - uh - God and Cleopatra together in holy blaughk matrimony.");
                            pastorGoblin.Dialogue.Add("If there is anyone in attendance who has cause to blaugh brrgh-believe that this couple should not bla-blaugh be joined in marriage, you may speak now or forever hold your peace.");
                            pastorGoblin.Talking = true;
                        }

                        if (talkingState == 7)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);


                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        timeLord.ClearDialogue();
                        timeLord.Dialogue.Add("...");
                        timeLord.Dialogue.Add("No objections? I thought not! Every king needs a queen, and as I am to control all of time and history, who better than the most beautiful woman to ever exist?");
                        timeLord.Dialogue.Add("Now, let us finish this ceremony so we may go consumate our marriage and begin my--our- reign!");
                    }

                    if (timer == 180)
                    {
                        timeLord.Talking = true;
                    }

                    if (timer >= 180)
                    {
                        timeLord.UpdateInteraction();

                        if (timeLord.Talking == false)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        timeLord.ClearDialogue();

                        caesar = game.CurrentChapter.NPCs["Julius"];
                        caesar.MapName = "Ancient Altar";
                        caesar.PositionX = 280;
                        caesar.PositionY = 297;
                        caesar.UpdateRecAndPosition();
                        dialogue.Add("NOOOOOOOOOOOOOOOOOOOOO!!");
                        player.playerState = Player.PlayerState.relaxedStanding;

                    }

                    if (camFollow.PositionX > 650 && timer > 200)
                        camFollow.PositionX -= 15;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (camFollow.PositionX <= 650)
                    {
                        state++;
                        timer = 0;
                        dialogue.Clear();
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 4:

                    if (firstFrameOfTheState)
                    {
                        pastorGoblin.ClearDialogue();
                        timeLord.ClearDialogue();
                        caesar.ClearDialogue();

                        caesar.Dialogue.Add("Cleo! My beloved! Oh, what have these monsters done to you? As soon as I heard you had been taken I fought my way through this forsaken pyramid, slaying hundreds - no - COUNTLESS demons in your name!");

                        talkingState = 0;
                    }


                    if (timer > 60 && talkingState == 0)
                        caesar.Talking = true;
                    if (timer > 60)
                    {
                        if (pastorGoblin.Talking == true)
                        {
                            pastorGoblin.UpdateInteraction();
                        }
                        else if (timeLord.Talking == true)
                            timeLord.UpdateInteraction();
                        else if (cleopatra.Talking == true)
                            cleopatra.UpdateInteraction();
                        else if (caesar.Talking == true)
                            caesar.UpdateInteraction();

                        if (pastorGoblin.Talking == false && timeLord.Talking == false && cleopatra.Talking == false && caesar.Talking == false && talkingState < 3)
                        {
                            timeLord.Dialogue.Clear();
                            pastorGoblin.Dialogue.Clear();
                            cleopatra.Dialogue.Clear();
                            caesar.Dialogue.Clear();

                            talkingState++;

                            if (talkingState == 1)
                            {
                                cleopatra.Talking = true;
                                cleopatra.Dialogue.Add("...");
                            }

                            if (talkingState == 2)
                            {
                                caesar.Dialogue.Add("Unchain this woman, you savage! Or you shall incur the wrath of the entire Roman Imperial Army!");
                                caesar.Talking = true;
                            }
                        }
                        else if (talkingState == 3)
                        {
                            if (camFollow.PositionX < 2200)
                                camFollow.PositionX += 15;

                            if (camFollow.PositionX >= 1600)
                            {
                                caesar.PositionX = 1884;
                                caesar.PositionY = 342;
                                caesar.UpdateRecAndPosition();
                            }

                            if (camFollow.PositionX >= 2200)
                            {
                                state++;
                                timer = 0;
                            }
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 5:

                    if (firstFrameOfTheState)
                    {
                        pastorGoblin.ClearDialogue();
                        timeLord.ClearDialogue();
                        caesar.ClearDialogue();

                        pastorGoblin.Talking = true;
                        pastorGoblin.Dialogue.Add("Blaaghk well I guess that's that.");
                        talkingState = 0;

                    }

                    if (pastorGoblin.Talking == true)
                    {
                        pastorGoblin.UpdateInteraction();
                    }
                    else if (timeLord.Talking == true)
                        timeLord.UpdateInteraction();
                    else if (cleopatra.Talking == true)
                        cleopatra.UpdateInteraction();
                    else if (caesar.Talking == true)
                        caesar.UpdateInteraction();

                    if (pastorGoblin.Talking == false && timeLord.Talking == false && cleopatra.Talking == false && caesar.Talking == false)
                    {
                        timeLord.Dialogue.Clear();
                        pastorGoblin.Dialogue.Clear();
                        cleopatra.Dialogue.Clear();
                        caesar.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            timeLord.Dialogue.Add("What the - \"THAT'S THAT?!\" ARE YOU KIDDING ME? TIME LORD is getting pretty PISSED OFF.");
                            timeLord.Talking = true;
                        }
                        if (talkingState == 2)
                        {
                            caesar.Talking = true;
                            caesar.Dialogue.Add("You are no lord of time, you're a wretched animal. A savage fiend! With the audacity to bring a goddess such as my beloved to harm for some offensive game! You-");
                        }
                        if (talkingState == 3)
                        {
                            timeLord.Dialogue.Add("Oh blow it out your ass!");
                            timeLord.Dialogue.Add("I swear, every single one of you will pay. You think that puny attack on Napoleon was the last of it? You think these weak guard dogs I've brainwashed are all I've got up my sleeve? Not even close.");
                            timeLord.Dialogue.Add("I'll make sure each one of you enjoys a slow, painful death at my hands! Even that weird kid that walked in earlier and never moved!");

                            timeLord.Talking = true;
                        }

                        if (talkingState == 4)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);


                    break;
                case 6:
                    if (firstFrameOfTheState)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        camFollow.PositionX = 650;
                        game.Camera.centerTarget = new Vector2(camFollow.PositionX, 0);
                        game.Camera.center = game.Camera.centerTarget;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 7:
                    if (firstFrameOfTheState)
                    {
                        timeLord.ClearDialogue();
                        timeLord.Dialogue.Add("Aaarrgggh! If I wasn't a hologram I'd kick all your asses!");
                        timeLord.Talking = true;
                        camFollow.PositionX = 2200;

                        game.Camera.centerTarget = new Vector2(camFollow.PositionX, 0);
                        game.Camera.center = game.Camera.centerTarget;
                    }
                    timeLord.UpdateInteraction();

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!timeLord.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 8:
                    if (firstFrameOfTheState)
                    {
                        timeLord.TurnOff();
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timeLord.moveFrame > 19)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 9:

                    if (firstFrameOfTheState)
                    {
                        pastorGoblin.ClearDialogue();
                        caesar.ClearDialogue();
                        cleopatra.ClearDialogue();

                        pastorGoblin.Talking = true;
                        pastorGoblin.Dialogue.Add("Blaaugh that concludes the brraau--wedding.");
                        talkingState = 0;

                    }

                    if (pastorGoblin.Talking == true)
                        pastorGoblin.UpdateInteraction();
                    else if (cleopatra.Talking == true)
                        cleopatra.UpdateInteraction();
                    else if (caesar.Talking == true)
                        caesar.UpdateInteraction();

                    if (pastorGoblin.Talking == false && cleopatra.Talking == false && caesar.Talking == false)
                    {
                        pastorGoblin.Dialogue.Clear();
                        cleopatra.Dialogue.Clear();
                        caesar.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            caesar.FacingRight = false;
                            caesar.Dialogue.Add("My darling! Are you alright? Say something!");
                            caesar.Talking = true;
                        }
                        if (talkingState == 2)
                        {
                            cleopatra.Talking = true;
                            cleopatra.Dialogue.Add(".....");
                        }
                        if (talkingState == 3)
                        {
                            caesar.Dialogue.Add("Excellent! I will get you back to safety right away!");
                            caesar.Talking = true;
                        }

                        if (talkingState == 4)
                        {
                            caesar.Dialogue.Add("You! Young man!");
                            caesar.Talking = true;
                        }
                        if (talkingState == 5)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;
                case 10:
                    if (camFollow.PositionX > 650)
                        camFollow.PositionX -= 15;

                    if (camFollow.PositionX <= 1200)
                    {
                        caesar.PositionX = 320;
                        caesar.PositionY = 297;
                        caesar.UpdateRecAndPosition();
                    }

                    if (camFollow.PositionX <= 650)
                    {
                        state++;
                        timer = 0;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 11:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("Go inform Bonaparte of what happened here, of my great victory over our enemy.");
                        caesar.Dialogue.Add("I'm sure he will be interested to hear about this...Time Lord.");
                        caesar.Dialogue.Add("Don't worry, I will ensure that this beautiful lady is brought to safety! Go!");

                        caesar.Talking = true;
                    }

                    caesar.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 12:
                    UnloadContent();
                    caesar.ClearDialogue();
                    cleopatra.ClearDialogue();
                    timeLord.PositionX = 100000;
                    timeLord.UpdateRecAndPosition();
                    cleopatra.Dialogue.Add("...");
                    caesar.Dialogue.Add("I will handle returning Cleo to safety. You should tell Bonaparte that we have finally met our enemy.");
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
                    game.CurrentChapter.CurrentMap.enteringMap = false;
                    game.ChapterTwo.NPCs["Pharaoh Guard 3"].CompleteQuestSilently(game.ChapterTwo.anubisInvasion);
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
                case 11:
                case 12:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer > 2 && timer < 200 && state == 3)
                        DrawDialogue(s, true);

                    if (pastorGoblin != null)
                    {
                        if (pastorGoblin.Talking)
                            pastorGoblin.DrawDialogue(s);
                        if (cleopatra.Talking)
                            cleopatra.DrawDialogue(s);
                        if (timeLord.Talking)
                            timeLord.DrawDialogue(s);
                        if (caesar != null && caesar.Talking)
                            caesar.DrawDialogue(s);
                    }
                    s.End();
                    break;
            }
        }
    }
}
