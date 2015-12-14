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
    class IntroducingTheManager : Cutscene
    {
        NPC theaterManager;
        MaracasHermanos en, en2;
        GameObject camFollow;
        int specialState = 0;

        float fadeAlpha = 1f;
        public IntroducingTheManager(Game1 g, Camera cam, Player p)
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
                        camFollow = new GameObject();
                        camFollow.Position = new Vector2(0, 0);
                        theaterManager = game.CurrentChapter.NPCs["Theater Manager"];
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("Whoa-oh!");
                        theaterManager.Dialogue.Add("Who let this ridiculously dressed dude in? What do I even pay you guys for, huh?");
                        theaterManager.Dialogue.Add("Hah! But really. Listen compadre, I have this joint hooked up head to toe in cameras, don't think I didn't hear you and that old hack Beethoven chatting it up out there on my stage.");
                        theaterManager.Dialogue.Add("Do you know who I am? I'm the guy putting together history's greatest band. That's who I am. I don't need some old timer ruining the vibe around here. Besides, he drinks enough booze for an entire theater of talentless hacks.");
                        theaterManager.Dialogue.Add("I need solid, dirty riffs, man. Frankly I'm jealous that the bag of dust can't hear his own music.");
                        theaterManager.CurrentDialogueFace = "Normal";
                        en = new MaracasHermanos(new Vector2(1053, 467), "Maracas Hermanos", game, ref player, game.CurrentChapter.CurrentMap);
                        en2 = new MaracasHermanos(new Vector2(1190, 467), "Maracas Hermanos", game, ref player, game.CurrentChapter.CurrentMap);
                        en.FacingRight = false;
                        en2.FacingRight = false;

                        en.SpawnWithPoof = false;
                        en2.SpawnWithPoof = false;

                        en.Alpha = 1;
                        en2.Alpha = 1;

                        en.Hostile = true;
                        en2.Hostile = true;

                        en.UpdateRectangles();
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en);
                        en2.UpdateRectangles();
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en2);

                        player.PositionY = 318;
                    }

                    if (timer > 40)
                    {
                        if (fadeAlpha > 0)
                            fadeAlpha -= 1f / 90f;
                    }
                    en.CutsceneStand();
                    en2.CutsceneStand();

                    camFollow.PositionX = player.VitalRec.Center.X + 230;
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (player.RecX <= 403)
                    {
                        player.CutsceneWalk(new Vector2(4, 0));
                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.RecX = player.VitalRec.Center.X;
                        camFollow.PositionY = 360;
                    }

                    else
                    {
                        player.CutsceneStand();
                        camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
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

                    en.CutsceneStand();
                    en2.CutsceneStand();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
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
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("Anywho, Beethoven's out, and so are you. I'm not sure how you found this place, my boss was pretty clear that I'd be the only one in here. I'll do him a favor and get rid of you, too.");
                    }

                    en.CutsceneStand();
                    en2.CutsceneStand();
                    player.CutsceneStand();

                    if (theaterManager.Talking)
                    {
                        theaterManager.UpdateInteraction();
                    }

                    if (timer == 30)
                        theaterManager.Talking = true;

                    if (theaterManager.PositionX > 666)
                        theaterManager.Move(new Vector2(-3, 0), Platform.PlatformType.rock);
                    else
                        theaterManager.moveState = NPC.MoveState.standing;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (theaterManager.PositionX <= 666 && theaterManager.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 3:
                    if (firstFrameOfTheState)
                    {
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("Adios, amigo. Say hi to Death for me. He owes me from that last football match.");
                            theaterManager.CurrentDialogueFace = "Sneer";
                    }

                    en.CutsceneStand();
                    en2.CutsceneStand();
                    player.CutsceneStand();

                    if(theaterManager.Talking)
                        theaterManager.UpdateInteraction();


                    if (timer == 60)
                        theaterManager.Talking = true;

                    //Raise the bookshelf
                    if (specialState == 0 && (game.CurrentChapter.CurrentMap as ManagersOffice).RaiseBookshelf())
                        specialState = 1;

                    //Fade the manager out
                    if (specialState == 1 && theaterManager.Alpha > 0 && theaterManager.Talking == false)
                    {
                        theaterManager.Alpha -= .01f;

                        if (theaterManager.Alpha <= 0)
                            specialState = 2;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    //Lower the bookshelf
                    if (specialState == 2 && (game.CurrentChapter.CurrentMap as ManagersOffice).LowerBookshelf())
                    {
                        theaterManager.MapName = "Axis of Musical Reality";
                        game.ChapterOne.ChapterOneBooleans["chasingTheManager"] = true;
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.enteringMap = false;
                        ManagersOffice.ToBackstage.IsUseable = false;
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
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer > 1 && theaterManager.Talking)
                        theaterManager.DrawDialogue(s);

                    if (fadeAlpha > 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * fadeAlpha);
                    s.End();
                    break;
            }
        }
    }
}
