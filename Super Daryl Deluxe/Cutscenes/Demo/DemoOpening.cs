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
    class DemoOpening : Cutscene
    {
        NPC associateOne;

        Texture2D popUpSheet, tutLevel1, tutLevel2, enterButton;
        int textTimer = 0;
        GameObject camFollow;

        public DemoOpening(Game1 g, Camera cam, Player p, Texture2D popUps, Texture2D tut1, Texture2D tut2, Texture2D enter)
            : base(g, cam, p)
        {
            tutLevel1 = tut1;
            tutLevel2 = tut2;
            enterButton = enter;
            popUpSheet = popUps;
            camFollow = new GameObject();
            camFollow.PositionX = 1100;
            camFollow.Rec = new Rectangle((int)camFollow.PositionX, 0, 1, 1);
            List<String> assocOneDialogue = new List<string>();
            assocOneDialogue.Add("Hello there player! My name's Danny. I'm super excited to be here with you today!");
            assocOneDialogue.Add("Dan & Gary Games has hired me to help you through this quick tutorial. I \npromise I'll make it as easy and fun as possible!");

            associateOne = new NPC(Game1.whiteFilter, assocOneDialogue , new Rectangle(-1000, 0, 0, 0),
                    player, game.Font, game, "Tutorial Map One", "Demo Danny", false);
        }

        public override void Play()
        {
            base.Play();


            last = current;
            current = Keyboard.GetState();

            //Increment the TUTORIAL LEVEL text timer
            textTimer++;
            if (textTimer == 60)
            {
                textTimer = 0;
            }

            switch (state)
            {
                //Black screen for a bit, sets the player position
                case 0:
                    if (firstFrameOfTheState)
                    {
                        player.UpdatePosition();
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["OutsidetheParty"];
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 0)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                //Fade in and pan across the outside of the party.
                //Start by the window so you can see the lights flashing inside
                case 1:
                    FadeIn(300);
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    camFollow.PositionX += 1f;
                    break;
                //Continue panning until you hit a certain point
                case 2:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    camFollow.PositionX += 1f;

                    if (camFollow.PositionX > 1500)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                //Fade out
                case 3:
                    FadeOut(120);
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;

                //Remain at a black screen for a second and move the camera back to the left side of the party
                case 4:
                    if (firstFrameOfTheState)
                    {
                        camFollow.PositionX = 1;
                        game.CurrentChapter.CurrentMap.UnloadContent();

                        //Reset the NPC sprites
                        for (int i = 0; i < game.CurrentChapter.NPCs.Count; i++)
                        {
                            if (game.CurrentChapter.NPCs.ElementAt(i).Value.MapName == game.CurrentChapter.CurrentMap.MapName)
                            {
                                game.CurrentChapter.NPCs.ElementAt(i).Value.Spritesheet = Game1.whiteFilter;
                            }
                        }

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["TheParty"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                    }

                    camera.Update(camFollow, game);
                    if (timer > 60)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //SNAPS TO DIFFERENT VIEWS OF THE PARTY OVER SEVERAL SECONDS, FINALLY LANDING ON DARYL DANCING
                case 5:
                    if(timer < 180)
                        camera.Update(camFollow, game);
                    else
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    game.CurrentChapter.CurrentMap.Update();
                    player.CutsceneStand();

                    if (timer == 180)
                    {
                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                        game.Camera.center = game.Camera.centerTarget;
                    }
                    else if (timer == 90)
                    {
                        camFollow.PositionX = 1120;
                        timer = 90;
                    }

                    if (timer > 270)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //FREEZES THE GAME AND WELCOMES THE PLAYER TO THE DEMO
                case 6:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 30 && (last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //ANOTHER POP UP SAYING THE PLAYER MUST DO THE TUTORIAL LEVEL
                case 7:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 30 && (last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //POP UP FOR LOADING THE TUTORIAL LEVE
                case 8:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 1000)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //MAP CHANGES AND POP UP SAYS LOAD IS COMPLETE
                case 9:

                    if (firstFrameOfTheState)
                    {
                        game.CurrentChapter.CurrentMap.UnloadContent();

                        //Reset the NPC sprites
                        for (int i = 0; i < game.CurrentChapter.NPCs.Count; i++)
                        {
                            if (game.CurrentChapter.NPCs.ElementAt(i).Value.MapName == game.CurrentChapter.CurrentMap.MapName)
                            {
                                game.CurrentChapter.NPCs.ElementAt(i).Value.Spritesheet = Game1.whiteFilter;
                            }
                        }

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["TutorialMapOne"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        player.RecX = 200;
                        player.PositionX = 200;

                        player.UpdatePosition();

                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                        game.Camera.center = game.Camera.centerTarget;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60 && (last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //Pop up telling you about the associates
                case 10:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer < 60)
                        player.CutsceneStand();

                    if (timer > 80 && (last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                //Bunch of pop-ups while loading 
                case 11:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 500)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 12:
                    if (firstFrameOfTheState)
                    {
                        associateOne.Talking = true;
                    }

                    associateOne.UpdateInteraction();

                    if (associateOne.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 13:
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    game.CurrentChapter.MakingDecision = true;
                    game.ChapterTwo.decisions = ChapterTwo.Decisions.tutorialResolution;
                    game.CurrentChapter.NPCs["Alan"].PositionX = 213;
                    game.CurrentChapter.NPCs["Alan"].CurrentDialogueFace = "Tutorial";
                    game.CurrentChapter.NPCs["Alan"].MapName = "Tutorial Map Seven";

                    game.CurrentChapter.NPCs["Paul"].PositionX = 650;
                    game.CurrentChapter.NPCs["Paul"].CurrentDialogueFace = "Tutorial";
                    game.CurrentChapter.NPCs["Paul"].MapName = "Tutorial Map Seven";

                    game.CurrentChapter.NPCs["Paul"].UpdateRecAndPosition();
                    game.CurrentChapter.NPCs["Alan"].UpdateRecAndPosition();
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                //Draw black screen
                case 0:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;

                //Draw outside the party and fade in
                case 1:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 1);
                    s.End();
                    break;

                //Keep panning across outside the party
                case 2:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    s.End();

                    break;

                //Fade out
                case 3:

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();
                    break;

                //Draw a black screen for a second
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;

                //Draw the party and snap to a few different positions
                case 5:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    if (timer > 180 || timer < 90)
                        game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    break;

                //Freeze the game and draw a pop up and enter button flashing
                case 6:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 0, 1100, 260), Color.White);

                    if (textTimer < 15 || (textTimer > 30 && textTimer < 45))
                        s.Draw(enterButton, new Rectangle(140, -((int)(Game1.aspectRatio * 1280 * .6) - 12), 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;

                //Pop up again
                case 7:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 1820, 1100, 260), Color.White);

                    if (textTimer < 30)
                    s.Draw(tutLevel1, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

                    if (textTimer < 15 || (textTimer > 30 && textTimer < 45))
                        s.Draw(enterButton, new Rectangle(140, -((int)(Game1.aspectRatio * 1280 * .6) - 12), 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;

                //'Load the tutorial level' pop up
                case 8:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer < 500)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260, 1100, 260), Color.White);
                    else
                    {
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 520, 1100, 260), Color.White);
                        if (textTimer < 30)
                            s.Draw(tutLevel2, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    }

                    if (textTimer > 45)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 6, 1100, 260), Color.White);
                    else if (textTimer > 30)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 5, 1100, 260), Color.White);
                    else if (textTimer > 15)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 4, 1100, 260), Color.White);

                    s.End();
                    break;

                //Changes maps and keeps the pop up for a second, then says LOAD COMPLETE
                case 9:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer > 10)
                    {
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 780, 1100, 260), Color.White);
                        if (textTimer < 15 || (textTimer > 30 && textTimer < 45))

                            s.Draw(enterButton, new Rectangle(140, -((int)(Game1.aspectRatio * 1280 * .6) - 12), 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    }
                    

                    s.End();
                    break;

                //Changes maps and keeps the pop up for a second, then says LOAD COMPLETE
                case 10:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer > 60)
                    {
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(1100, 0, 1100, 260), Color.White);

                        if (timer > 80)
                        {
                            if (textTimer < 15 || (textTimer > 30 && textTimer < 45))
                                s.Draw(enterButton, new Rectangle(140, -((int)(Game1.aspectRatio * 1280 * .6) - 12), 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                        }
                    }

                    s.End();
                    break;

                //Changes maps and keeps the pop up for a second, then says LOAD COMPLETE
                case 11:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer < 400)
                    {
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(1100, 260, 1100, 260), Color.White);
                    }

                    else
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(1100, 1300, 1100, 260), Color.White);

                    //Ellipses
                    if (textTimer > 45)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 6, 1100, 260), Color.White);
                    else if (textTimer > 30)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 5, 1100, 260), Color.White);
                    else if(textTimer > 15)
                        s.Draw(popUpSheet, new Rectangle(91, 37, 1100, 260), new Rectangle(0, 260 * 4, 1100, 260), Color.White);

                    s.End();
                    break;


                //PLACEHOLDER DRAW MAP
                case 12:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    associateOne.DrawDialogue(s);
                    s.End();
                    break;

                case 13:
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
