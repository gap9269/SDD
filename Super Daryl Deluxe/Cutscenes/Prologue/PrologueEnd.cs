using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class PrologueEnd : Cutscene
    {
        // ATTRIBUTES \\
        NPC tim;
        Texture2D complete;
        GameObject camFollow;
        int timeDelay = 5;
        int timFrame;
        int timTransformTimer;
        List<Rectangle> barrelRecs;

        //--Takes in a background and all necessary objects
        public PrologueEnd(Game1 g, Camera cam, Player player, Texture2D com)
            : base(g, cam, player)
        {
            complete = com;
            barrelRecs = new List<Rectangle>();
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
                        tim = game.CurrentChapter.NPCs["Tim"];

                        game.Prologue.Synopsis += "\n\nPaul and Alan were unhappy that you only brought back one textbook, but\n sold you a new skill anyway. Using this skill, you were able to defeat\n Tim, who was rather angry and didn't seem to understand that friends love flowers.";
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 150)
                    {
                        state++;
                        timer = 0;
                    }

                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        tim.PositionX = 2600;
                        tim.RecX = 2600;
                        tim.UpdateRecAndPosition();
                        player.PositionX = 3070;
                        player.FacingRight = false;
                        player.PositionY = game.CurrentChapter.CurrentMap.Platforms[0].Rec.Y - player.VitalRecHeight - 175;
                        player.UpdatePosition();
                        player.Velocity = Vector2.Zero;
                        player.KnockedBack = false;
                        player.InvincibleTime = 0;
                        player.Alpha = 1f;
                        tim.FacingRight = true;
                        game.CurrentChapter.BossFight = false;
                        game.CurrentChapter.CurrentBoss = null;
                        player.playerState = Player.PlayerState.standing;
                        player.StopSkills();
                        player.UpdateInvincible();
                        tim.moveState = NPC.MoveState.standing;
                        camFollow.PositionX = 3120;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        Game1.schoolMaps.maps["NorthHall"].InteractiveObjects.Clear();
                        NorthHall.drawTimMap = true;
                        game.Camera.centerTarget = new Vector2(camFollow.PositionX + (camFollow.Rec.Width / 2), 0);
                        game.Camera.center = game.Camera.centerTarget;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    if (timeDelay <= 0)
                    {
                        timeDelay = 5;
                        timFrame++;

                        if (timFrame > 11)
                            timFrame = 0;
                    }
                    FadeIn(60);
                    break;

                case 2:
                    timeDelay--;

                    if (timeDelay <= 0)
                    {
                        timeDelay = 5;
                        timFrame++;

                        if (timFrame > 11)
                            timFrame = 0;
                    }

                    if(timer > 240)
                        timTransformTimer++;

                    if (timTransformTimer < 10 || timTransformTimer > 25 && timTransformTimer < 40 || timTransformTimer > 50 && timTransformTimer < 60 || timTransformTimer > 65 && timTransformTimer < 70)
                    {
                        NorthHall.drawTimMap = true;
                    }
                    else
                        NorthHall.drawTimMap = false;

                    //if (timTransformTimer < 10 || timTransformTimer > 25 && timTransformTimer < 45 || timTransformTimer > 65 && timTransformTimer < 85 || timTransformTimer > 100 && timTransformTimer < 110 || timTransformTimer > 115 && timTransformTimer < 120 || timTransformTimer > 125 && timTransformTimer < 130)
                    //{
                    //    NorthHall.drawTimMap = true;
                    //}
                    //else
                    //    NorthHall.drawTimMap = false;


                    if (timTransformTimer > 250)
                    {
                        state++;
                        timer = 0;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    //player.CutsceneStand();
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        //TODO
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.leftTimPlat);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.leftStep);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.leftPillar);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.rightPillar);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.rightStep);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Remove(NorthHall.rightTimPlat);

                        tim.Dialogue.Clear();
                        tim.Dialogue.Add("You're fuckin' weird. Stay away from my locker.");
                        tim.Talking = true;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    //player.CutsceneStand();

                    tim.UpdateInteraction();

                    if (tim.Talking == false)
                    {
                        tim.Move(new Vector2(-3, 0));
                        tim.FacingRight = false;
                    }

                    //if (tim.PositionX <= 3400)
                    //{
                    //    timer = 0;
                    //    state++;
                    //}

                    break;

                case 4:
                    if (firstFrameOfTheState)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 600)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 5:
                    FadeOut(120);
                    break;
                case 6:
                    game.chapterState = Game1.ChapterState.chapterOne;
                    game.CurrentChapter.state = Chapter.GameState.Cutscene;
                    game.CurrentChapter = game.ChapterOne;
                    player.playerState = Player.PlayerState.standing;
                    timer = 0;
                    player.StopSkills();
                    game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                    game.Camera.center = game.Camera.centerTarget;

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
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    //s.Draw(game.EnemySpriteSheets["TimDead"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    //DrawCutsceneBars(s);
                    s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
null, null, null, null, camera.Transform);
                        Chapter.effectsManager.DrawSkillEffects(s);
                        game.CurrentChapter.CurrentMap.DrawEnemyDamage(s);
                        player.DrawDamage(s);
                        game.CurrentChapter.CurrentMap.DrawPortalInfo(s);
                    s.End();

                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                        game.CurrentChapter.CurrentBoss.DrawHud(s);

                        //HUD
                        game.CurrentChapter.HUD.Draw(s);

                        //Screen tint based on Daryl's low health
                        if (player.Health < (player.MaxHealth / 4))
                        {
                            s.Draw(Game1.lowHealthTint, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * game.CurrentChapter.tintAlpha);
                        }

                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.White * (timer / 100f * 2));
                    s.End();

                    break;

                case 1:
                case 2:
                case 3:

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);

                    if (NorthHall.drawTimMap)
                    {
                        NorthHall.timBar1.Draw(s);
                        NorthHall.timBar2.Draw(s);
                        NorthHall.timBar3.Draw(s);
                        s.Draw(GorillaTim.animationTextures["down" + timFrame], new Rectangle((int)tim.PositionX + 55, 118, (int)(940 * .65f), (int)(796 * .65f)), Color.White);
                    }
                    else
                        game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (tim.Talking)
                        tim.DrawDialogue(s);

                    if (state == 1)
                        DrawFadeWhite(s, 1);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (timer > 400)
                        s.Draw(complete, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;
                case 5:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();
                    break;
               
            }
        }
    }
}
