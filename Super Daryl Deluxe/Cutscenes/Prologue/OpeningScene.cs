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
        Texture2D outsideSchool, outsideWords;
        GameObject camFollow; //An object for the camera to follow
        Texture2D schedule;
        Texture2D mainOffice;
        NPC paul;
        NPC alan;
        float textAlpha = 1f;

        float clouds1Pos, clouds2Pos;

        int specialTimer;
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
            outsideWords = textures["OutsideSchoolWords"];
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
 state++;
                        timer = 0;
                    break;


                //-- KEEP MOVING CAMERA OUTSIDE OF SCHOOL UNTIL YOU REACH THE END OF THE MAP
                case 2:
                    if (firstFrameOfTheState)
                    {
                        dialogue.Add("Ah yes, you must be Daryl Whitelaw. I am Mr. Robatto, the vice principal here.");
                    }
                    //camera.GetStaticTransform(game);
                    camera.Update(camFollow, game);

                    clouds1Pos += .3f;
                    clouds2Pos += .5f;

                    if (timer > 120)
                    {
                        if (textAlpha > 0)
                            textAlpha -= .01f;
                    }
                    if (timer > 400)
                    {
                       state++;
                       timer = 0;
                    }
                    break;

                //-- FADE OUT FROM OUTSIDE SCHOOL
                case 3:
                    clouds1Pos += .3f;
                    clouds2Pos += .5f;
                    FadeOut(35);
                    break;

                //-- PAN ACROSS MAIN LOBBY SLOWLY, PLAY DIALOGUE WITHOUT KNOWING WHO IS TALKING
                case 4:
                    if (firstFrameOfTheState)
                    {
                        //--SET DIALOGUE AND CAM POSITION
                        camFollow.PositionX = 2;
                        dialogue.Clear();
                        dialogue.Add("Welcome! Water Falls High School is the finest school in the state. We pride ourselves on the beauty and safety that our campus offers.");
                        dialogue.Add("Looking around you, you're sure to see a vast community of friendly, helpful students. If you ever need help, don't be afraid to reach out to any of your peers or faculty.");
                        dialogue.Add("Of course, academics are of the utmost importance to us. There's nothing we care about more than the success of our students, so we have created the perfect environment for learning and growing into an upstanding citizen.");
                        dialogue.Add("Speaking of, here is your class schedule. What do you think of your classes?");
                        DialogueState = 0;
                    }

                    //--CHANGE DIALOGUE STATES BASED ON CAMERA POSITION
                    else if (camFollow.PositionX == 1900)
                        DialogueState = 3;
                    else if (camFollow.PositionX == 1400)
                        DialogueState = 2;
                    else if (camFollow.PositionX == 850)
                        DialogueState = 1;

                    //--REACH END OF LOBBY, RESET TIMER AND DIALOGUE
                    if (camFollow.PositionX >= 3425 - 1280)
                    {
                        specialTimer++;
                    }
                    else
                    {
                        camFollow.PositionX += 1f;
                        camera.Update(camFollow, game);
                    }

                    if (specialTimer >= 100)
                    {
                        timer = 0;
                        state++;
                        dialogueState = 0;
                        dialogue.Clear();
                        specialTimer = 0;
                    }
                    break;


                //-- THIS SCENE DRAWS ONLY DARYL HOLDING HIS SCHEDULE
                case 5:
                    camera.Update(camFollow, game);
                    if (timer > 280)
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
                        robatto.Dialogue.Add("Yes Daryl, that schedule will be your guide to success here at WFHS. You're going to do just fine.");
                        robatto.Dialogue.Add("Classes are about to begin soon. If you would follow me, I'll show you to your locker.");

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
                    FadeOut(120);

                    if (timer == 119)
                    {
                        game.CurrentChapter.CurrentMap.UnloadContent();
                        game.CurrentChapter.CurrentMap.UnloadNPCContent();
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["NorthHall"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.LoadCurrentMapLocks();
                        player.PositionX = game.CurrentChapter.CurrentMap.MapWidth - 600;
                        player.PositionY = game.CurrentChapter.CurrentMap.Platforms[0].Rec.Y - player.VitalRecHeight - 135 - 37;
                        robatto.PositionX = player.PositionX - 150;
                        robatto.PositionY = player.PositionY - 30;

                        camFollow.PositionX = robatto.PositionX - 65;

                        camera.centerTarget = new Vector2(camFollow.PositionX, 0);
                        camera.center = camera.centerTarget;
                    }
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
                        robatto.Dialogue.Add("This locker is yours. You will be accessing it frequently throughout the day. It is a very important part of your high school experience.");
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
                            alan.Dialogue.Add("You made a good choice coming to us first. Anybody else would've tried exploiting you by now.");
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
                            alan.Dialogue.Add("Took the words right outta my mouth, Paul. Damn kids nowadays only think for themselves.");
                            alan.Talking = true;
                        }

                        else if (talkingState == 6)
                        {
                            paul.Dialogue.Add("Right on the ball, Alan. Right on the ball. Not us though.");
                            paul.Talking = true;
                        }
                        else if (talkingState == 7)
                        {
                            alan.Dialogue.Add("Nope. We're on a mission from God to save our doomed generation. We lead by example.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 9)
                        {
                            paul.Dialogue.Add("Of course, our lowly peers aren't always on the same page as us. We face discrimination left and right. Only an hour ago a plain bully named Tim nabbed an important paper from Alan and threw it into The Quad.");
                            paul.Talking = true;
                        }
                        else if (talkingState == 10)
                        {
                            alan.Dialogue.Add("It was completely uncalled for! We can't save any souls without that paper, buddy. Plus, half of The Quad is closed to students. Paul and I could be expelled if we're caught out there again.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 11)
                        {
                            paul.Dialogue.Add("I shan't begin to think what would become of my mother of poor health if she were to hear that her only child had been expelled...for -trespassing-!");
                            paul.Talking = true;
                        }
                        else if (talkingState == 12)
                        {
                            alan.Dialogue.Add("Now you've done it, kid. You made Paul sad. You owe it to him and his mother to go get that paper.");
                            alan.Talking = true;
                        }
                        else if (talkingState == 13)
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
                        alan.Dialogue.Add("You can get to The Quad from the Main Lobby.");
                        alan.Dialogue.Add("You know, that big room you entered the building through? Only an idiot could miss it.");
                        alan.Dialogue.Add("Oh! While you're there pick some dandelions for us. Friends love flowers! Understand?");
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
                        alan.Dialogue.Add("Whatever you do, don't go to the far side of the quad. It's said anyone who does that faces Death.");
                        game.CurrentSideQuests.Add((game.CurrentChapter as Prologue).QuestOne);
                        player.playerState = Player.PlayerState.standing;
                        player.CanJump = false;
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
                    s.Draw(outsideSchool, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.Draw(outsideWords, new Rectangle(0, 720 - outsideWords.Height, 1280, outsideWords.Height), Color.White);
                    s.End();

                    //--This stays static on the screen, draws the fade-in square
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (timer < 100)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    //DrawFade(s, 1);
                    s.End();
                    break;

                case 2: //Draw the outside of the school still based on the camera
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    s.Draw(outsideSchool, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.Draw(outsideWords, new Rectangle(0, 720 - outsideWords.Height, 1280, outsideWords.Height), Color.White * textAlpha);
                    s.Draw(textures["Clouds1"], new Vector2(-200 + clouds1Pos, 0), Color.White);
                    s.Draw(textures["Clouds2"], new Vector2(0 + clouds2Pos, 0), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (timer < 2)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    if (timer > 200)
                    {
                        DrawDialogue(s, false);
                    }
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    s.Draw(outsideSchool, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.Draw(textures["Clouds1"], new Vector2(-200 + clouds1Pos, 0), Color.White);
                    s.Draw(textures["Clouds2"], new Vector2(0 + clouds2Pos, 0), Color.White);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawDialogue(s, false);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 4:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (dialogue.Count > 0)
                    {
                        DrawDialogue(s, false);
                    }

                    if(timer < 20)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
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
                    DrawFade(s, 0);
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
