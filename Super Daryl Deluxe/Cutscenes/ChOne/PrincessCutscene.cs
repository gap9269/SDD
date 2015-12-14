using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class PrincessCutscene : Cutscene
    {
        Texture2D flashback;
        GameObject camFollow;
        NPC princess;
        Boolean floorCollapse = false;
        int collapseFrame;
        int collapseDelay = 5;
        Boolean floorGone = false;
        float platformPosY, platVelocityY;

        int otherTimer;
        float alpha = 1f;

        Video newspaperFlashback;
        VideoPlayer videoPlayer;
        //--Takes in a background and all necessary objects
        public PrincessCutscene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            camFollow = new GameObject();
            camFollow.PositionY = -400;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            videoPlayer = new VideoPlayer();

            newspaperFlashback = content.Load<Video>(@"Cutscenes\NewspaperFlashback");

        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                        LoadContent();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    game.Camera.center.Y = -862;

                    if (player.RecX > 250 && floorCollapse == false && floorGone == false)
                    {
                        camera.ShakeCamera(20, 20);
                        floorCollapse = true;
                    }
                    else if (player.RecX <= 340)
                    {
                        player.CutsceneRun(new Vector2(player.MoveSpeed, 0));
                    }

                    if (floorCollapse && floorGone == false)
                    {
                        player.ImplementGravity();
                        player.UpdatePosition();
                        player.AttackFalling = true;
                       // player.playerState = Player.PlayerState.jumping;
                        collapseDelay--;

                        platVelocityY += GameConstants.GRAVITY;
                        platformPosY += platVelocityY;

                        if (collapseDelay <= 0)
                        {
                            collapseDelay = 5;
                            collapseFrame++;

                            if (collapseFrame > 9)
                            {
                                floorCollapse = false;
                                floorGone = true;
                            }
                        }
                    }

                    if (player.VitalRec.Center.Y > 150)
                    {
                        if(otherTimer == 0)
                            camera.ShakeCamera(20, 20);
                        otherTimer++;

                    }
                    if (otherTimer > 90)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        timer = 0;
                        state++;
                    }

                    break;
                case 1:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    game.Camera.center.Y = -862;

                    FadeOut(120);
                    break;

                //--Flashback
                case 2:
                    if (firstFrameOfTheState)
                    {
                        player.Position = new Vector2(-25, -131);
                        player.Landing = false;
                        player.UpdatePosition();
                        Chapter.effectsManager.ClearDustPoofs();
                        player.StunDaryl(100);
                        player.FrameDelay = 100;
                        player.MoveFrame = 3;
                        player.Alpha = 1f;
                        videoPlayer.Play(newspaperFlashback);
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 3:

                    if (alpha <= 0)
                    {
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        player.Update();

                        if (player.IsStunned == false)
                        {
                            state++;
                            timer = 0;
                        }
                    }
                    else
                        alpha -= .025f;
                    break;

                case 4:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (firstFrameOfTheState)
                    {
                        princess = game.CurrentChapter.NPCs["The Princess"];
                        princess.Dialogue.Clear();
                        princess.Dialogue.Add("Ugh! Now you're standing on it! You think you can just come in here and stand on my bed with your filthy shoes? Get down!");
                        princess.Dialogue.Add("You know, you'll be destroyed once daddy hears about you breaking in. He'll never stand to have the safety of his little princess compromised by goons like you.");
                        princess.Dialogue.Add("I'm the center of his world. Daddy hates mouth breathers, you'll be crushed like a bug.");
                        princess.Dialogue.Add("Are you even listening to me? Say something!");

                        //princess.AddQuest(game.ChapterOne.DaddysLittlePrincess);
                    }
                    if (timer == 20)
                        princess.Talking = true;
                    princess.UpdateInteraction();
                    if (princess.Talking == false && timer > 20)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 5:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 140)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 6:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (firstFrameOfTheState)
                    {
                        princess = game.CurrentChapter.NPCs["The Princess"];
                        princess.Dialogue.Clear();
                        princess.AddQuest(game.ChapterOne.DaddysLittlePrincess);
                        princess.Talking = true;
                    }
                    princess.UpdateInteraction();
                    if (princess.Talking == false)
                    {
                        game.CurrentChapter.CurrentMap.enteringMap = false;
                        state++;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        player.playerState = Player.PlayerState.standing;
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
                case 0:
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);

                    if(!floorGone)
                        s.Draw(PrincessLockerRoom.healthyVent, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y), Color.White);

                    if (floorCollapse && collapseFrame < 10)
                    {
                        s.Draw(PrincessLockerRoom.collapseAnimation.ElementAt(collapseFrame).Value, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y), Color.White);
                        s.Draw(PrincessLockerRoom.platform, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y + platformPosY), Color.White);
                    }

                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer < 2 && state == 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    DrawFadeWhite(s, 0);
                    s.End();
                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.White);
                    s.End();
                    if (videoPlayer != null && timer > 0)
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


                        if (videoPlayer.State == MediaState.Stopped && timer > 600)
                        {
                            state++;
                            timer = 0;
                        }
                    }
                    break;
                case 3:
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer < 100)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * alpha);
                    s.End();
                    break;
                case 4:
                case 5:
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
                    if (princess != null && princess.Talking)
                        princess.DrawDialogue(s);

                    s.End();
                    break;
            }
        }
    }
}
