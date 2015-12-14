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
    class SecondMessageScene : Cutscene
    {
        NPC messengerBoy;
        Video hangermanScene;
        VideoPlayer videoPlayer;

        public SecondMessageScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            videoPlayer = new VideoPlayer();
            hangermanScene = content.Load<Video>(@"Cutscenes\DarylMeetsHangerman");
            Game1.npcFaces["Messenger Boy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Messenger Boy Normal");
            game.NPCSprites["Messenger Boy"] = content.Load<Texture2D>(@"NPC\History\Messenger Boy");

        }

        public override void  UnloadContent()
        {
            Game1.npcFaces["Messenger Boy"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Messenger Boy"] = Game1.whiteFilter;

        }

        public override void SkipCutscene()
        {
            base.SkipCutscene();

            messengerBoy.MapName = "Not a real map";
            game.ChapterTwo.ChapterTwoBooleans["hangermanOfficeScenePlayed"] = true;
            player.FacingRight = false;
            game.ChapterTwo.NPCs["Alan"].Dialogue.Clear();
            game.ChapterTwo.NPCs["Alan"].Dialogue.Add("Where have you been? It's almost the end of the day. You need to hurry up and get Balto's phone back.");
            game.ChapterTwo.NPCs["Balto"].Dialogue.Clear();
            game.ChapterTwo.NPCs["Balto"].Dialogue.Add("Have you found Robatto and my phone yet? We need those ledgers.");
            game.ChapterTwo.NPCs["Paul"].ChangeQuestDialogueForAcceptedQuest("We don't pay employees to take forever on the job, Drake. Do you want to be demoted again? Go get Balto's phone!");
            game.ChapterTwo.NPCs["The Princess"].AddQuest(game.ChapterTwo.tutoringThePrincess);
            game.ChapterTwo.NPCs["The Princess"].AcceptQuestRemotely(game.ChapterTwo.tutoringThePrincess);

            videoPlayer.Stop();

            player.playerState = Player.PlayerState.standing;
            UnloadContent();
            state = 0;
            timer = 0;
            game.CurrentChapter.CutsceneState++;
            game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "Main Lobby"), MainLobby.toMainOffice);
            game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
        }

        public override void Play()
        {
            base.Play();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        messengerBoy = game.CurrentChapter.NPCs["Messenger Boy"];
                        messengerBoy.PositionX = player.PositionX + 200;
                        messengerBoy.PositionY = 330;
                        messengerBoy.UpdateRecAndPosition();
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(messengerBoy.RecX + 150, messengerBoy.RecY + 170, 200, 200), 2);
                    }

                    if (timer == 15)
                    {
                        messengerBoy.MapName = "Pyramid Entrance";

                        messengerBoy.FacingRight = false;

                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if(timer > 60)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        player.FacingRight = true;
                        messengerBoy.ClearDialogue();
                        messengerBoy.Dialogue.Add("Message for Daryl Whitelaw!");
                        messengerBoy.Dialogue.Add("Oh, it's you again. You hang around some weird places, ya know?");
                        messengerBoy.Dialogue.Add("*Ahem* Well, Principal Hangerman would like to see you in his office. Better hurry, if you don't mind me sayin' so.");
                        messengerBoy.Dialogue.Add("I'm s'posed to bring you back there myself. Guess he don't trust you to follow instructions.");
                        messengerBoy.Dialogue.Add("Now if you'll just be followin' me...");
                        messengerBoy.Talking = true;
                    }
                    messengerBoy.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!messengerBoy.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    FadeOut(180);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        videoPlayer.Play(hangermanScene);
                    }
                    break;
                case 4:
                    if (firstFrameOfTheState)
                    {
                        messengerBoy.MapName = "Not a real map";
                        game.ChapterTwo.ChapterTwoBooleans["hangermanOfficeScenePlayed"] = true;
                        player.FacingRight = false;
                        game.ChapterTwo.NPCs["Alan"].Dialogue.Clear();
                        game.ChapterTwo.NPCs["Alan"].Dialogue.Add("Where have you been? It's almost the end of the day. You need to hurry up and get Balto's phone back.");
                        game.ChapterTwo.NPCs["Balto"].Dialogue.Clear();
                        game.ChapterTwo.NPCs["Balto"].Dialogue.Add("Have you found Robatto and my phone yet? We need those ledgers.");
                        game.ChapterTwo.NPCs["Paul"].ChangeQuestDialogueForAcceptedQuest("We don't pay employees to take forever on the job, Drake. Do you want to be demoted again? Go get Balto's phone!");
                        game.ChapterTwo.NPCs["The Princess"].AddQuest(game.ChapterTwo.tutoringThePrincess);
                        game.ChapterTwo.NPCs["The Princess"].AcceptQuestRemotely(game.ChapterTwo.tutoringThePrincess);
                    }
                    player.FacingRight = false;

                    if (timer > 10)
                    {
                        UnloadContent();
                        state = 0;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "Main Lobby"), MainLobby.toMainOffice);
                        game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
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
                case 4:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (messengerBoy != null && messengerBoy.Talking)
                        messengerBoy.DrawDialogue(s);

                    if (state == 2)
                        DrawFade(s, 0);
                    if (state == 4)
                    {
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    }
                    s.End();
                    break;
                case 3:
                    if (videoPlayer != null && videoPlayer.State == MediaState.Playing)
                    {
                        Texture2D sceneTex = videoPlayer.GetTexture();

                        if (sceneTex != null)
                        {
                            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                            s.Draw(sceneTex, new Rectangle(0, 0, 1280, 720), Color.White);
                            s.End();
                            sceneTex.Dispose();
                        }
                    }

                    if (videoPlayer.State == MediaState.Stopped && timer > 200)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
            }
        }
    }
}
