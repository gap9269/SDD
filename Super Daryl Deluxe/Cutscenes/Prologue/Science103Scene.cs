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
    public class Science103Scene : Cutscene
    {
        GameObject camFollow;
        int flowerFrame, doorFrame;
        int flowerDelay = 5;
        int bobDelay = 4;
        int bobFrame = 14;
        Boolean gateGone = false;
        int timesBobbed;
        SoundEffectInstance cutscene_science_plant_guy;

        public Science103Scene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
        }

        public override void LoadContent()
        {
            cutscene_science_plant_guy = content.Load<SoundEffect>("Sound\\Cutscenes\\cutscene_science_plant_guy").CreateInstance();
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        camFollow.YScroll = true;
                        camFollow.PositionX = 1050;// player.VitalRec.Center.X;
                        camFollow.PositionY = -670;// player.VitalRec.Center.Y;
                        game.Camera.center = camFollow.Position;
                        game.Camera.ShakeCamera(40, 4);

                        Sound.PlaySoundInstance(cutscene_science_plant_guy, Game1.GetFileName(() => cutscene_science_plant_guy));

                    }

                    player.CanJump = false;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (player.playerState == Player.PlayerState.standing || player.playerState == Player.PlayerState.running)
                        player.CutsceneStand();
                    else
                        player.Update();

                    if (camFollow.PositionY > -670)
                    {
                        camFollow.PositionY -= 50;
                        camFollow.RecY -= 50;
                    }

                    if (camFollow.PositionX < 1050)
                    {
                        camFollow.PositionX += 15;
                        camFollow.RecX += 15;

                        if (camFollow.PositionX > 1050)
                        {
                            camFollow.PositionX = 1050;
                            camFollow.RecX = 1050;
                        }
                    }
                    else if (camFollow.PositionX > 1050)
                    {
                        camFollow.PositionX -= 15;
                        camFollow.RecX -= 15;

                        if (camFollow.PositionX < 1050)
                        {
                            camFollow.PositionX = 1050;
                            camFollow.RecX = 1050;
                        }
                    }

                    if (timer > 3 && !gateGone)
                    {

                        flowerDelay--;

                        if (flowerDelay <= 0)
                        {
                            flowerDelay = 6;

                            if(flowerFrame < 10)
                                game.Camera.ShakeCamera(6, flowerFrame * 10);

                            if (flowerFrame > 10)
                                flowerDelay = 8;
                            if (flowerFrame == 11)
                                flowerDelay = 20;
                            if (flowerFrame == 14)
                                flowerDelay = 4;
                            flowerFrame++;
                            if (flowerFrame > 14)
                            {
                                gateGone = true;
                            }

                            if (flowerFrame > 5)
                                doorFrame++;
                        }
                    }
                    else if (gateGone)
                    {
                        bobDelay--;

                        if (bobDelay <= 0)
                        {
                            bobDelay = 4;
                            bobFrame++;

                            if (bobFrame > 18)
                            {
                                timesBobbed++;

                                if (timesBobbed == 2)
                                {
                                    game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);
                                    game.Camera.centerTarget += new Vector2(0, player.Position.Y + (player.Rec.Height / 2));

                                    UnloadContent();
                                    player.playerState = Player.PlayerState.standing;
                                    game.CurrentChapter.state = Chapter.GameState.Game;
                                    game.CurrentChapter.CutsceneState++;
                                    player.CanJump = true;
                                }
                                else
                                {
                                    bobFrame = 0;
                                }
                            }
                        }
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

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    //Flower animation
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, Game1.camera.Transform);

                    String textureString = "science 103";
                    if (flowerFrame < 10)
                        textureString += "0" + flowerFrame.ToString();
                    else
                        textureString += flowerFrame.ToString();

                    if (!gateGone)
                        s.Draw(Science103.laserTextures[textureString], new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y), Color.White);
                    else
                        DrawFlower(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    player.Draw(s);

                    if(flowerFrame < 5)
                        s.Draw(Science103.gate, new Vector2(1654, game.CurrentChapter.CurrentMap.mapRec.Y + 858), Color.White);
                    else if(doorFrame < 6)
                        s.Draw(Science103.doorTextures.ElementAt(doorFrame).Value, new Vector2(1228, game.CurrentChapter.CurrentMap.mapRec.Y + 564), Color.White);

                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;
            }
        }

        public void DrawFlower(SpriteBatch s)
        {
            s.Draw(Science103.flowerTextures.ElementAt(bobFrame).Value, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y + 925), Color.White);
        }
    }
}
