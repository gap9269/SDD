﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class ChapterOneOpening : Cutscene
    {
        Texture2D temp;

        Video flashbackAndOpening;
        VideoPlayer videoPlayer;

        //--Takes in a background and all necessary objects
        public ChapterOneOpening(Game1 g, Camera cam, Player player, Texture2D t)
            : base(g, cam, player)
        {
            temp = t;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            videoPlayer = new VideoPlayer();

            flashbackAndOpening = content.Load<Video>(@"Cutscenes\ChapterOneOpeningScene");
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
                        videoPlayer.Play(flashbackAndOpening);
                    }
                    break;
                case 1:
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    game.CurrentChapter.CutsceneState++;
                    timer = 0;
                    UnloadContent();
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
