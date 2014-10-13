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
    class GateCollapseScene : Cutscene
    {
        // ATTRIBUTES \\
        Rectangle rock1Rec, rock2Rec;
        float rockVelocity, rock1Velocity;
        int explosionFrame, explosionTimer;
        Boolean exploding = true;
        Boolean playerFalling = false;
        GameObject camFollow = new GameObject();
        int goblinIndex = 0;

        //--Takes in a background and all necessary objects
        public GateCollapseScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            explosionTimer = 5;

            rock1Rec = new Rectangle(1663, -3100 + 720 + 360 + 72, 1295, 1043);
            rock2Rec = new Rectangle(1663, -3100 + 720 + 360 + 72, 1295, 1043);
        }

        public Rectangle GetExplosionSource()
        {
            if (explosionFrame < 4)
                return new Rectangle(998 * explosionFrame, 0, 998, 766);
            else if (explosionFrame < 8)
                return new Rectangle(998 * (explosionFrame - 4), 766, 998, 766);
            else if (explosionFrame < 12)
                return new Rectangle(998 * (explosionFrame - 8), 1532, 998, 766);
            else
                return new Rectangle(998 * (explosionFrame - 12), 2298, 998, 766);
        }

        public override void Play()
        {
            base.Play();

            Chapter.effectsManager.Update();

            switch (state)
            {
                case 0:

                    if (firstFrameOfTheState)
                    {
                        game.Camera.ShakeCamera(30, 20);
                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.YScroll = true;
                        camFollow.RecY = -1450;
                        camFollow.PositionY = -1450;

                        camFollow.Rec = new Rectangle((int)camFollow.PositionX, (int)camFollow.PositionY, 1, 1);
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    explosionTimer--;

                    if (explosionTimer == 0)
                    {
                        explosionFrame++;
                        //Console.WriteLine(explosionFrame);
                        explosionTimer = 4;
                    }
                    if(explosionFrame < 9)
                        player.CutsceneStand();


                    if (explosionFrame == 9 && !game.MapBooleans.chapterTwoMapBooleans["explosionHitFrame10"])
                    {
                        game.MapBooleans.chapterTwoMapBooleans["explosionHitFrame10"] = true;
                        game.Camera.ShakeCamera(15, 25);
                        playerFalling = true;
                        player.Falling = true;
                    }
                    if (explosionFrame >= 9)
                    {
                        rockVelocity += GameConstants.GRAVITY;

                        rock1Rec.Y += (int)rockVelocity;

                        if (playerFalling)
                        {
                            player.PositionY += rockVelocity;
                            player.UpdatePosition();
                        }
                    }

                    if (explosionFrame >= 10 || (explosionFrame == 9 && explosionTimer == 1))
                    {
                        rock1Velocity += GameConstants.GRAVITY;
                        rock2Rec.Y += (int)rock1Velocity;
                    }

                    if (player.VitalRecY > -100)
                    {
                        Sound.PauseBackgroundMusic();
                        Sound.backgroundVolume = 0f;
                    }

                    if (player.VitalRecY > 300)
                    {
                        game.Camera.ShakeCamera(15, 15);
                        playerFalling = false;
                        player.VitalRecY = 200;
                        timer = 10;
                    }

                    if (explosionFrame > 13 && timer > 150)
                    {
                        player.PositionX = 2030;
                        player.PositionY = 320;
                        player.StopSkills();
                        player.UpdatePosition();
                        player.StunDaryl(250);
                        player.MoveFrame = 3;
                        player.Alpha = 1f;
                        exploding = false;
                        timer = 0;
                        state++;

                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.RecX = player.VitalRec.Center.X;

                        for (int i = 0; i < game.CurrentChapter.CurrentMap.EnemiesInMap.Count; i++)
                        {

                            if (game.CurrentChapter.CurrentMap.EnemiesInMap[i].EnemyType == "Goblin")
                            {
                                //Give them full health and make them not hostile, just in case something weird happened to them before the cutscene
                                game.CurrentChapter.CurrentMap.EnemiesInMap[i].Health = game.CurrentChapter.CurrentMap.EnemiesInMap[i].MaxHealth;
                                game.CurrentChapter.CurrentMap.EnemiesInMap[i].Hostile = false;

                                switch (goblinIndex)
                                {
                                    case 0:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 1526;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = true;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 0;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;
                                    case 1:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 1681;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = true;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 0;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;
                                    case 2:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 1845;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = true;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 1;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;
                                    case 3:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 1911;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = true;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 0;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;


                                    case 4:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 2675;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = false;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 1;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;
                                    case 5:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 2806;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = false;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 0;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;
                                    case 6:
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].PositionX = 2865;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].FacingRight = false;
                                        (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).StandState = 1;
                                        game.CurrentChapter.CurrentMap.EnemiesInMap[i].UpdateRectangles();
                                        break;

                                }

                                goblinIndex++;
                            }
                        }
                    }
                    break;
                case 1:

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    for (int i = 0; i < game.CurrentChapter.CurrentMap.EnemiesInMap.Count; i++)
                    {
                        if (game.CurrentChapter.CurrentMap.EnemiesInMap[i].EnemyType == "Goblin")
                        {
                            (game.CurrentChapter.CurrentMap.EnemiesInMap[i] as Goblin).CutsceneStand();
                        }
                    }

                    //KEEP CAMERA LOCKED WHEN PLAYER IS AT BOTTOM OF MAP
                    if (game.Camera.center.Y < 600)
                    {
                        camFollow.PositionY += 10;
                        camFollow.RecY += 10;
                    }
                    else
                    {
                        if (WorkersField.lightRayAlpha < 1f)
                        {
                            Sound.ResumeBackgroundMusic();
                            Sound.IncrementBackgroundVolume(.005f);

                            game.CurrentChapter.CurrentMap.PlayBackgroundMusic();


                            WorkersField.lightRayAlpha += .005f;

                            if (WorkersField.lightRayAlpha > 1f)
                                WorkersField.lightRayAlpha = 1f;
                        }
                    }

                    if (WorkersField.lightRayAlpha == 1f)
                    {

                        for (int i = 0; i < game.CurrentChapter.CurrentMap.EnemiesInMap.Count; i++)
                        {
                            if (game.CurrentChapter.CurrentMap.EnemiesInMap[i].EnemyType != "Goblin" && game.CurrentChapter.CurrentMap.EnemiesInMap[i].EnemyType != "Field Troll")
                            {
                                game.CurrentChapter.CurrentMap.EnemiesInMap.RemoveAt(i);
                                i--;
                                continue;
                            }
                        }
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.CurrentBoss = null;
                        game.CurrentChapter.BossFight = false;
                        player.StunDaryl(80);
                        player.MoveFrame = 3;
                        player.FrameDelay = 80;
                        player.Health = player.MaxHealth;
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

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemies(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if (explosionFrame >= 9)
                    {
                        s.Draw(WorkersField.rock1, rock1Rec, Color.White);
                        s.Draw(WorkersField.rock2, rock2Rec, Color.White);
                    }

                    if (exploding)
                    {
                        s.Draw(WorkersField.explosionSheet, new Rectangle(1901, -game.CurrentChapter.CurrentMap.MapHeight + 720 + 360 + 527 + 200, 998, 776), GetExplosionSource(), Color.White);
                    }
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;

                case 1:

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                        game.CurrentChapter.CurrentMap.Draw(s);
                        game.CurrentChapter.DrawNPC(s);
                        game.CurrentChapter.CurrentMap.DrawEnemies(s);
                        Chapter.effectsManager.DrawPoofs(s);
                        player.Draw(s);

                    s.End();

                        game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    //MAP OVERLAY
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                        game.CurrentChapter.CurrentMap.DrawMapOverlay(s);
                    s.End();
                    break;
            }
        }
    }
}