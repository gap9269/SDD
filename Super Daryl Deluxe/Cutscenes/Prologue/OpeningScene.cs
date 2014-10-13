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
    class OpeningScene : Cutscene
{
        // ATTRIBUTES \\
        NPC robatto;
        Texture2D outsideSchool;
        GameObject camFollow; //An object for the camera to follow
        Texture2D schedule;
        Texture2D mainOffice;
        NPC paul;
        NPC alan;

        int talkingState;

        // CONSTRUCTOR \\

        public override void LoadContent()
        {
            base.LoadContent();

            game.NPCSprites["Mr. Robatto"] = content.Load<Texture2D>(@"NPC\Main\robatto");

            Game1.npcFaces["Mr. Robatto"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Robatto");
        }

        //--Takes in a background and all necessary objects
        public OpeningScene(Game1 g, Camera cam, Player player, Dictionary<String,Texture2D> texts)
            : base(g, cam, player)
        {
            textures = texts;
            robatto = new NPC(game.NPCSprites["Mr. Robatto"], new List<String>(), new Rectangle(), player, Game1.font, game,"North Hall", "Mr. Robatto", false);

            cutsceneNPCs.Add("Mr. Robatto", robatto);
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
            schedule = textures["Schedule"];
            outsideSchool = textures["OutsideSchool"];
            mainOffice = textures["MainOffice"];
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                //-- FADE OUT FROM MAIN SCREEN
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                    }
                    //camera.GetStaticTransform(game);
                    topBarPos = 0;
                    botBarPos = (int)(Game1.aspectRatio * 1280) - 66;
                    FadeOut(120);
                    break;

                //-- FADE IN AND PAN ACROSS OUTSIDE OF SCHOOL
                case 1:
                    FadeIn(180);
                    camera.Update(camFollow, game);
                    camFollow.PositionX += 2;
                    break;


                //-- KEEP MOVING CAMERA OUTSIDE OF SCHOOL UNTIL YOU REACH THE END OF THE MAP
                case 2:
                    //camera.GetStaticTransform(game);
                    camera.Update(camFollow, game);
                    camFollow.PositionX += 2;
                    if (camera.Center.X > 2000 - 1280)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //-- FADE OUT FROM OUTSIDE SCHOOL
                case 3:
                    FadeOut(60);
                    break;

                //-- PAN ACROSS MAIN LOBBY SLOWLY, PLAY DIALOGUE WITHOUT KNOWING WHO IS TALKING
                case 4:
                    if (firstFrameOfTheState)
                    {
                        //--SET DIALOGUE AND CAM POSITION
                        camFollow.PositionX = 1;
                        dialogue.Clear();
                        dialogue.Add("Greetings! You must be Daryl Whitelaw. My name is Mr. Robatto, it's a pleasure to meet you.");
                        dialogue.Add("I hope you are impressed by everything you have seen so far here at Water Falls High School. We strive for excellence here.");
                        dialogue.Add("This is your schedule. I hope you find it satisfactory to completing your education here at WFHS. Do you like your classes?");
                        DialogueState = 0;
                    }

                    //--CHANGE DIALOGUE STATES BASED ON CAMERA POSITION
                    else if (camFollow.PositionX == 800)
                        DialogueState = 2;
                    else if (camFollow.PositionX == 400)
                        DialogueState = 1;

                    camFollow.PositionX += 1;
                    camera.Update(camFollow, game);

                    //--REACH END OF LOBBY, RESET TIMER AND DIALOGUE
                    if (camFollow.PositionX >= 2300 - 1280)
                    {
                        timer = 0;
                        state++;
                        dialogueState = 0;
                        dialogue.Clear();
                    }
                    break;


                //-- THIS SCENE DRAWS ONLY DARYL HOLDING HIS SCHEDULE
                case 5:
                    camera.Update(camFollow, game);
                    if (timer > 120)
                    {
                        //alpha = 0;
                        timer = 0;
                        state++;
                    }
                    break;
                

                //-- DARYL AND VITALE IN MAIN OFFICE, VITALE MONOLOGUE ABOUT SCHOOL IMPORTANCE
                case 6:
                    camera.Update(camFollow, game);
                    if (firstFrameOfTheState)
                    {
                        //--CREATE VITALE
                        robatto.Rec = new Rectangle(0, 0, 518, 388);

                        dialogue.Clear();
                        robatto.Dialogue.Add("It looks like you do. I should not have to remind you how important it is that you attend all of your classes.");
                        robatto.Dialogue.Add("We strive to provide only the best education a young, handsome man like yourself can obtain. ");
                        robatto.Dialogue.Add("And I am sure you are concerned about meeting new people as well. That will not be an issue! Our students are " +
                            "not only very helpful, but very kind. They will be happy to show you around.");

                        robatto.Dialogue.Add("You will be making new friends in no time, and I am sure you will fit right in. If you will just follow me I will show you to your new locker.");
                        robatto.Dialogue.Add("Right this way Daryl. Classes begin soon!");

                    }
                    if (timer == 21)
                        robatto.Talking = true;
                    if (timer > 20)
                        robatto.UpdateInteraction();

                    if (robatto.Talking == false && timer > 22)
                    {
                        state++;
                        timer = 0;
                    }
                    break;


                //-- FADE OUT OF MAIN OFFICE
                case 7:
                    camera.Update(camFollow, game);
                    if (firstFrameOfTheState)
                    {
                        game.CurrentChapter.CurrentMap.UnloadContent();
                        game.CurrentChapter.CurrentMap.UnloadNPCContent();
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["NorthHall"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        player.PositionX = game.CurrentChapter.CurrentMap.MapWidth - 600;
                        player.PositionY = game.CurrentChapter.CurrentMap.Platforms[0].Rec.Y - player.VitalRecHeight - 135 - 37;
                        robatto.PositionX = player.PositionX - 150;
                        robatto.PositionY = player.PositionY - 30;

                        camFollow.PositionX = robatto.PositionX - 65;

                        camera.centerTarget = new Vector2(camFollow.PositionX, 0);
                        camera.center = camera.centerTarget;
                    }
                    FadeOut(120);
                    break;

                //-- FADE IN TO DARYL FOLLOWING VITALE DOWN NORTH HALL
                case 8:
                    FadeIn(120);

                    //-- SET CURRENT MAP, PLAYER AND VITALE POSITION AND SET THE POSITON FOR DAN AND GARY
                    if (firstFrameOfTheState)
                    {
                        paul = game.CurrentChapter.NPCs["Paul"];
                        alan = game.CurrentChapter.NPCs["Alan"];
                    }
                    camFollow.PositionX -= 5;
                    robatto.Move(new Vector2(-5, 0));
                    player.UpdatePosition();

                    if (timer > 5)
                        player.CutsceneWalk(new Vector2(-5, 0));

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;

                case 9:


                    robatto.Move(new Vector2(-5, 0));
                    player.UpdatePosition();
                    player.CutsceneWalk(new Vector2(-5, 0));

                    if (camFollow.PositionX >= 3230)
                        camFollow.PositionX -= 5;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (player.PositionX <= 3220)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 10:
                    if (firstFrameOfTheState)
                    {
                        robatto.Dialogue.Clear();
                        robatto.moveState = NPC.MoveState.standing;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        robatto.Dialogue.Add("This locker is yours. You will be accessing it frequently throughout the day. It is a very important" +
                            " part of your high school experience.");
                        robatto.Dialogue.Add("Oh look, there are some new friends now!");
                        robatto.Dialogue.Add("Paul, Alan, this is Daryl. Daryl, this is Paul and Alan.");
                        robatto.Dialogue.Add("...");
                        robatto.Dialogue.Add("...   ...");
                        robatto.Dialogue.Add("...   ...   ...");
                        robatto.Dialogue.Add("See? It's easy to make friends!");
                        robatto.Dialogue.Add("Have fun!");
                        camFollow.PositionX = robatto.Rec.Center.X;
                        robatto.Talking = true;
                    }
                    if (robatto.DialogueState == 2 || robatto.DialogueState == 3 || robatto.DialogueState == 4 || robatto.DialogueState == 5)
                        robatto.FacingRight = false;
                    else
                        robatto.FacingRight = true;

                    robatto.UpdateInteraction();

                    if (robatto.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 11:
                    robatto.Move(new Vector2(4, 0));

                    if (robatto.PositionX > camera.Center.X + (1280 / 2) + 400)
                    {
                        state++;
                        timer = 0;
                        dialogue.Clear();
                    }
                    break;

                case 12:
                    if (firstFrameOfTheState)
                    {
                        alan.QuestDialogue = null;
                        paul.QuestDialogue = null;
                        paul.Dialogue.Clear();
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("Uh...who are you?");
                    }

                    if (timer > 60 && timer < 80)
                        player.CutsceneWalk(new Vector2(-3, 0));

                    if (timer == 80)
                        player.playerState = Player.PlayerState.relaxedStanding;

                    if (timer == 90)
                        alan.FacingRight = true;

                    if (timer == 181)
                    {
                        alan.Talking = true;
                        paul.FacingRight = true;
                    }

                    if (timer > 180)
                    {
                        alan.UpdateInteraction();
                        alan.Update();
                    }

                    if (timer > 180 && alan.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        state++;
                        timer = 0;
                    }
                    break;

                //-- DARYL STANDS AND STARES AT THEM
                case 13:
                    if (timer > 180)
                    {
                        timer = 0;
                        state++;
                    }
                    break;


                case 14:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Add("...");
                        paul.Dialogue.Add("Okay, bye.");
                        paul.Talking = true;
                    }

                    paul.UpdateInteraction();

                    if (paul.Talking == false && alan.FacingRight == true)
                    {
                        alan.FacingRight = false;
                        timer = 2;
                    }
                    if (paul.Talking == false && alan.FacingRight == false && timer > 20)
                    {
                        paul.FacingRight = false;
                        state++;
                        timer = 0;
                    }
                    break;

                case 15:
                    if (timer > 90 && timer < 120)
                        player.CutsceneWalk(new Vector2(-3, 0));
                    if (timer == 120)
                    {
                        paul.FacingRight = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        timer = 0;
                        state++;
                    }
                    break;
                case 16:
                    if (firstFrameOfTheState)
                    {
                        paul.DialogueState = 0;
                        paul.Dialogue.Clear();
                        paul.Dialogue.Add("Oh god he got closer.");
                    }

                    if (timer == 20)
                    {
                        paul.Talking = true; 
                        alan.FacingRight = true;
                    }

                    if (timer > 10)
                        paul.UpdateInteraction();

                    if (paul.Talking == false && timer > 20)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 17:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("What do you want?");
                        alan.Talking = true;
                    }

                    alan.UpdateInteraction();

                    if (alan.Talking == false && timer > 20)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 18:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();
                    }
                    if (timer > 180)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 19:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Clear();
                        paul.Dialogue.Add("...");
                        paul.Talking = true;
                    }

                    paul.UpdateInteraction();

                    if (paul.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 20:

                    paul.Update();
                    alan.Update();

                    if (timer < 10)
                    {
                        paul.PositionX -= 3;
                        paul.RecX -= 3;
                        alan.PositionX -= 3;
                        alan.RecX -= 3;
                    }

                    if (timer > 13 && timer < 23)
                        player.CutsceneWalk(new Vector2(-3, 0));

                    if (timer == 23)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        state++;
                        timer = 0;
                    }
                    break;

                case 21:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("What's wrong with him?");
                    }

                    if (timer == 31)
                        alan.Talking = true;

                    if (timer > 30)
                    {
                        alan.UpdateInteraction();
                    }

                    if (timer > 30 && alan.Talking == false)
                    {
                        timer = 0; 
                        state++;
                    }
                    break;
                case 22:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Clear();
                        paul.Dialogue.Add("Maybe he's stupid.");
                        paul.Dialogue.Add("Who is he?");
                        paul.Talking = true;
                    }

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
                            alan.Dialogue.Add("Beats me. I'd wager he wants to be our friend.");
                        }

                        else if (talkingState == 2)
                        {
                            paul.CurrentDialogueFace = "Arrogant";
                            paul.Dialogue.Add("Of course he wants to be our friend. Quite frankly the fact that he came to us first tells me this kid has some fine taste in people.");
                            paul.Talking = true;
                        }

                        else if (talkingState == 3)
                        {
                            alan.CurrentDialogueFace = "Arrogant";
                            alan.Dialogue.Add("Fine taste indeed...");
                            alan.Dialogue.Add("You made a good choice coming to us first. Anybody else would've tried exploiting your ignorance by now.");
                            alan.Talking = true;
                        }

                        else if (talkingState == 4)
                        {
                            paul.CurrentDialogueFace = "Normal";
                            paul.Dialogue.Add("It makes me sick just thinking about the lack of character in this dump.");
                            paul.Talking = true;

                        }
                        else if (talkingState == 5)
                        {
                            alan.CurrentDialogueFace ="Normal";
                            alan.Dialogue.Add("No character at all...");
                            alan.Talking = true;
                        }

                        else if (talkingState == 6)
                        {
                            paul.Dialogue.Add("None! It's no surprise the world is in the poor state it's in.");
                            paul.Talking = true;
                        }
                        else if (talkingState == 7)
                        {
                            alan.Dialogue.Add("Took the words right outta my mouth, Paul. Damn kids nowadays only think for themselves.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 9)
                        {
                            paul.CurrentDialogueFace ="Arrogant";
                            paul.Dialogue.Add("Right on the ball, Alan. Right on the ball. Not us though.");
                            paul.Talking = true;
                        }
                        else if (talkingState == 10)
                        {
                            alan.CurrentDialogueFace = "Arrogant";
                            alan.Dialogue.Add("Nope. We're on a mission from God to save our doomed generation. We lead by example.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 11)
                        {
                            paul.CurrentDialogueFace = "Normal";
                            paul.Dialogue.Add("We're two halves of one Messiah, friend. Unfortunately our peers aren't on the same page as us. As recently as an hour ago we've faced discrimination in our endeavors. A plain bully named Tim nabbed an important piece of paper from Alan and threw it into the quad.");
                            paul.Talking = true;
                        }
                        else if (talkingState == 12)
                        {
                            alan.CurrentDialogueFace = "Normal";
                            alan.Dialogue.Add("It was completely uncalled for! We can't spread the word of God without that paper, buddy. The Far Side of the Quad is closed to students, and our mission has put us in bad favor with the school authorities. Paul and I can't afford another demerit on our records.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 13)
                        {
                            paul.CurrentDialogueFace = "Normal";
                            paul.Dialogue.Add("I shan't begin to think what would become of my mother of poor health if she were to hear that her only child had been caught wandering around the Far Side...");
                            paul.Dialogue.Add("*sob*");
                            paul.Talking = true;
                        }
                        else if (talkingState == 14)
                        {
                            alan.CurrentDialogueFace = "Normal";
                            alan.Dialogue.Add("Now you've done it, kid. You made Paul sad. You owe it to him and his mother to go get that paper.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 15)
                        {
                            state++;
                            timer = 0;
                        }

                    }
                    break;

                case 23:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Clear();
                        alan.Dialogue.Clear();
                        paul.QuestDialogue = paul.Quest.QuestDialogue;
                        paul.Talking = true;
                        paul.CurrentDialogueFace = "Arrogant";
                    }

                    paul.UpdateInteraction();
                    paul.Choice = 0;

                    if (paul.Talking == false)
                    {
                        paul.CurrentDialogueFace = "Normal";
                        state++;
                        timer = 0;
                    }
                    break;

                case 24:
                    if (firstFrameOfTheState)
                    {
                        alan.CurrentDialogueFace = "Normal";
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("Be careful when you're over there. I heard kids get shot on sight if they get caught on the far side.");
                        alan.Dialogue.Add("Oh! While you're there pick some flowers for us. Friends love flowers!");
                        alan.Dialogue.Add("Understand?");
                    }

                    if(alan.DialogueState == 1)
                        alan.CurrentDialogueFace = "Arrogant";
                    else
                        alan.CurrentDialogueFace = "Normal";

                    if (timer == 3)
                        alan.Talking = true;
                    if (timer > 3)
                        alan.UpdateInteraction();

                    if (timer > 3 && alan.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 25:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("Alright...bye bye.");
                    }

                    if (timer == 121)
                        alan.Talking = true;
                    if (timer > 120)
                        alan.UpdateInteraction();

                    if (timer > 120 && alan.Talking == false)
                    {
                        state++;
                        timer = 0;
                        player.UpdatePosition();
                    }

                    break;
                    
                case 26: //Start the game

                    int disX;
                    disX = (int)(camFollow.PositionX - player.VitalRecX);
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    if (disX < 0)
                        camFollow.PositionX++;
                    if (disX > 0)
                        camFollow.PositionX--;

                    if (camFollow.PositionX == player.VitalRecX)
                    {
                        paul.Dialogue.Clear();
                        alan.Dialogue.Clear();
                        alan.Dialogue.Add("Be careful not to get caught.");
                        game.CurrentSideQuests.Add((game.CurrentChapter as Prologue).QuestOne);
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        game.Prologue.PrologueBooleans["ratSpawned"] = true;
                        UnloadContent();
                    }
                    break;

            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0: //Draws the black box over the entire screen and changes the alpha
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;

                case 1: //Draw the background and the fade-in
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    s.Draw(outsideSchool, new Rectangle(0, 0, 2000, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();

                    //--This stays static on the screen, draws the fade-in square
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.DrawString(Game1.twConMedium, "Water Falls High School, Present Day", new Vector2(1280 / 2 - Game1.HUDFont.MeasureString("Water Falls High School, Present Day").X / 2, (int)(Game1.aspectRatio * 1280 * .8)), Color.Black);
                    DrawFade(s, 1);
                    s.End();
                    break;

                case 2: //Draw the outside of the school still based on the camera
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    s.Draw(outsideSchool, new Rectangle(0, 0, 2000, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    s.Draw(outsideSchool, new Rectangle(0, 0, 2000, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (dialogue.Count > 0 && camFollow.PositionX > 100)
                    {
                        DrawDialogue(s);
                    }
                    s.End();
                    break;
                case 5: //Draws a filler for the scene
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(schedule, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;

                case 6:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(mainOffice, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    if (timer > 20)
                        robatto.DrawDialogue(s);
                    s.End();
                    break;

                case 7: //Draws the currentMap, player, and dialogue
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(mainOffice, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    
                    DrawFade(s, 0);
                    s.End();
                    break;

                case 8:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    robatto.Draw(s);
                    player.Draw(s);
                    robatto.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    
                    DrawFade(s, 1);

                    s.End();
                    break;

                case 9:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    robatto.Draw(s);
                    //cutsceneNPCs["Robatto"].Draw(s);
                    
                    player.Draw(s);
                    robatto.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    
                    s.End();

                    break;


                case 10:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    robatto.Draw(s);
                    //cutsceneNPCs["Robatto"].Draw(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    
                    robatto.DrawDialogue(s);
                    s.End();
                    break;

                case 11:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    robatto.Draw(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    
                    s.End();
                    break;
                case 12:
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                     null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    

                    if (timer > 160)
                        alan.DrawDialogue(s);
                    s.End();
                    break;
                case 13:
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
                case 14:
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

                case 15:
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

                case 16:
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
                case 17:
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
                case 18:
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
                case 19:
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
                case 20:
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
                case 21:
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

                case 22:
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                     null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    

                    if(paul.Talking)
                        paul.DrawDialogue(s);
                    if (alan.Talking)
                        alan.DrawDialogue(s);
                    s.End();
                    break;
                case 23:
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

                case 24:
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

                case 25:
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

                case 26:
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
            }
        }
    }
}
