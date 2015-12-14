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
    class BeethovenCanStay : Cutscene
    {
        NPC theaterManager;
        int specialState = 0;
        int specialTimer = 0;
        float fadeAlpha = 1f;

        public BeethovenCanStay(Game1 g, Camera cam, Player p)
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
                        theaterManager = game.CurrentChapter.NPCs["Theater Manager"];
                        theaterManager.PositionX = 770;
                        theaterManager.Alpha = 1;
                        theaterManager.FacingRight = true;
                        player.FacingRight = true;
                        theaterManager.PositionY = 310;
                        player.PositionX = 670;
                        player.Alpha = 0f;
                        player.UpdatePosition();
                        theaterManager.UpdateRecAndPosition();
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("What the hell, man? I had some real vintage shit on that shelf. We're talking a Limited Edition gold-plated album signed by Skrillex himself.");
                        theaterManager.Dialogue.Add("What do you want, anyway? Is it really all that important to you that some geezer stays here and finishes his crappy opera? You know it flops, right? Did you even take Musical History?");
                        theaterManager.Dialogue.Add("You know, you royally piss me off. ...But I'll admit that taking down that xylophone is pretty hardcore.");
                        theaterManager.Dialogue.Add("I guess I could kill you myself, but I don't have the time to clean dead idiot out of my carpet. I realize that these \"musicians\" I have here aren't going to cut it for me. Instead I'm going to remix all of them into exactly what I need for history's greatest band.");
                        theaterManager.Dialogue.Add("Tell Beethoven whatever you want. It makes no difference to me now, I doubt he would be useful even as a remix. As for you, if I ever see you again I'll strangle you with that stupid headband of yours.");
                        theaterManager.Dialogue.Add("Now get the hell out of my theater.");

                        theaterManager.CurrentDialogueFace = "Normal";
                    }
                    Chapter.effectsManager.Update();
                    specialTimer++;

                    if (theaterManager.PositionX < 960)
                        theaterManager.Move(new Vector2(3, 0), Platform.PlatformType.rock);
                    else if(specialState == 0)
                    {
                        game.Camera.ShakeCamera(15, 5);
                        specialTimer = 0;
                        specialState = 1;
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X - 165, player.VitalRec.Center.Y - 125, 250, 250), 3);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X - 100, player.VitalRec.Center.Y - 155, 100, 100), 3);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X + 40, player.VitalRec.Center.Y - 105, 80, 80), 3);
                    }
                    else if (specialState == 1)
                    {
                        if (specialTimer == 5)
                        {
                            Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X - 70, player.VitalRec.Center.Y - 115, 80, 80), 3);
                            Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X + 10, player.VitalRec.Center.Y - 145, 80, 80), 3);
                        }
                        if (specialTimer == 10)
                        {
                            Chapter.effectsManager.AddInGameDialogue("!?", "Theater Manager", "Normal", 50);
                            theaterManager.FacingRight = false;
                            theaterManager.moveState = NPC.MoveState.standing;
                        }
                        else if(specialTimer < 10)
                            theaterManager.Move(new Vector2(3, 0), Platform.PlatformType.rock);

                    }

                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (specialState==1 && specialTimer > 120)
                    {
                        player.CutsceneStand();
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        theaterManager.Talking = true;
                    }

                    if (fadeAlpha > 0)
                        fadeAlpha -= 1f / 90f;
                    Chapter.effectsManager.Update();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    theaterManager.UpdateInteraction();
                    if (theaterManager.DialogueState == 2)
                        theaterManager.CurrentDialogueFace = "Sneer";
                    else
                        theaterManager.CurrentDialogueFace = "Normal";

                    if (theaterManager.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(theaterManager.Rec.Center.X - 70, theaterManager.Rec.Center.Y, 150, 150), 2);
                    }

                    Chapter.effectsManager.Update();
                    if (timer == 5)
                    {
                        theaterManager.PositionX = 9000;
                        theaterManager.UpdateRecAndPosition();
                    }

                    player.CutsceneStand();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    //Lower the bookshelf
                    if (timer > 120)
                    {
                        theaterManager.MapName = "None";
                        game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"] = true;
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.enteringMap = false;
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
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    if (specialState == 0)
                        s.Draw((game.CurrentChapter.CurrentMap as ManagersOffice).bookshelf, new Vector2(0, 0), Color.White);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);

                    if(specialState == 1 && specialTimer > 5 || state > 0)
                        player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    Chapter.effectsManager.DrawDialogue(s);

                    if(timer > 1 && theaterManager.Talking)
                        theaterManager.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
