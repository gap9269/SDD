using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class ChapterOneEnd: Cutscene
    {
        Video endScene;
        VideoPlayer videoPlayer;

        //--Takes in a background and all necessary objects
        public ChapterOneEnd(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            videoPlayer = new VideoPlayer();

            endScene = content.Load<Video>(@"Cutscenes\ChapterOneEnd");
        }

        public override void Play()
        {
            base.Play();

            camera.Update(player, game, game.CurrentChapter.CurrentMap);
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        videoPlayer.Play(endScene);
                    }
                    break;
                case 1:
                    //game.CurrentChapter.state = Chapter.GameState.Game;
                    //game.CurrentChapter.CutsceneState++;
                    //timer = 0;
                    UnloadContent();
                    game.ResetGameAndGoToMain();


                        game.chapterState = Game1.ChapterState.chapterTwo;
                        game.CurrentChapter.state = Chapter.GameState.Cutscene;
                        game.CurrentChapter.CutsceneState = 0;
                        game.CurrentChapter = game.ChapterTwo;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.StartingPortal = NorthHall.ToArtHall;
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["North Hall"];
                        Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;
                        Chapter.lastMap = "Janitor's Closet";
                        Sound.StopAmbience();
                        Sound.StopBackgroundMusic();
                        game.CurrentChapter.CurrentMap.LoadContent();
                        timer = 0;
                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);
                        game.Camera.center = game.Camera.centerTarget;

                        player.Health = player.realMaxHealth;
                        player.PositionX = 1420;
                        player.PositionY = 290;
                        player.UpdatePosition();
                        player.VelocityX = 0;
                        player.FacingRight = true;
                        player.StopSkills();
                        player.KnockedBack = false;

                        Chapter.effectsManager.fButtonRecs.Clear();
                        Chapter.effectsManager.foregroundFButtonRecs.Clear();
                        Chapter.effectsManager.spaceButtonRecs.Clear();
                        Chapter.effectsManager.foregroundSpaceButtonRecs.Clear();
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:
                    if (videoPlayer != null)
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
                            state ++;
                            timer = 0;
                        }
                    }
                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
