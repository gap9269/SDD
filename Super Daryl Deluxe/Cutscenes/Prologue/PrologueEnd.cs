using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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
        SoundEffect cutscene_gorilla_tim_end, popup_prologue_end;
        SoundEffectInstance TimEnd;
        Boolean playedTimEnd = false;
        //--Takes in a background and all necessary objects
        public PrologueEnd(Game1 g, Camera cam, Player player, Texture2D com)
            : base(g, cam, player)
        {
            complete = com;
            barrelRecs = new List<Rectangle>();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            cutscene_gorilla_tim_end = content.Load<SoundEffect>(@"Sound\Cutscenes\Tim\cutscene_gorilla_tim_end");
            TimEnd = content.Load<SoundEffect>(@"Sound\Cutscenes\Tim\TimEnd").CreateInstance();
            popup_prologue_end = content.Load<SoundEffect>(@"Sound\Pop Ups\popup_prologue_end");
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
                        camFollow = new GameObject();
                        tim = game.CurrentChapter.NPCs["Tim"];

                        Sound.PlaySoundInstance(cutscene_gorilla_tim_end, Game1.GetFileName(()=> cutscene_gorilla_tim_end));

                        game.Prologue.Synopsis += "\n\nUpon your return, Paul and Alan were unimpressed that you had only brought back one textbook. However, they were eager to make a deal and sold you a new skill anyway. This transaction concluded just in time to meet your new friend Tim, who joined in to remind everyone again that he was deathly allergic to dandelion pollen and that he would rather they didn't open his locker and take his money without permission. During this exchange, Paul and Alan kindly introduced you to Tim and selflessly wandered off in order to let you acquaint yourselves in peace. Tim then turned into a towering gorilla and proceeded to attempt disemboweling you. Your new skill came in much use, however, and you were soon able talk Tim back down to his calm, rational human form. Before parting ways, Tim reminded you again to please refrain from using his locker without permission. Finally alone, you stood there in the hall reveling in the events that transpired in your exciting first day, sure that you were in for a thrilling adventure.";
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 150)
                    {
                        state++;
                        timer = 0;

                        if (TimEnd.State != SoundState.Playing && !playedTimEnd)
                        {
                            playedTimEnd = true;
                            GorillaTim.timFightTheme.Stop();
                            Sound.PlaySoundInstance(TimEnd, Game1.GetFileName(() => TimEnd));
                        }

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
                        Game1.schoolMaps.maps["North Hall"].InteractiveObjects.Clear();
                        NorthHall.drawTimMap = true;
                        game.Camera.centerTarget = new Vector2(camFollow.PositionX + (camFollow.Rec.Width / 2), 0);
                        game.Camera.center = game.Camera.centerTarget;
                        Chapter.effectsManager.ClearDustPoofs();

                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    timeDelay--;
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

                    if(timer > 300)
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


                    if (timTransformTimer > 300)
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
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.leftTimPlat);
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.leftStep);
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.leftPillar);
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.rightPillar);
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.rightStep);
                        Game1.schoolMaps.maps["North Hall"].Platforms.Remove(NorthHall.rightTimPlat);

                        tim.Dialogue.Clear();
                        tim.Dialogue.Add("...");
                        tim.Dialogue.Add("You're fuckin' weird.");
                        tim.Dialogue.Add("Stay away from my locker.");
                        tim.Talking = true;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    //player.CutsceneStand();

                    tim.UpdateInteraction();
                    if (tim.Talking == false)
                    {
                        tim.Move(new Vector2(-3, 0), Platform.PlatformType.rock, 1, 5);
                        tim.FacingRight = false;
                    }

                    if (tim.PositionX <= 1500)
                    {
                        timer = 0;
                        state++;
                    }

                    break;

                case 4:
                    if (firstFrameOfTheState)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    if (timer == 300)
                    {
                        Sound.PlaySoundInstance(popup_prologue_end, Game1.GetFileName(() => popup_prologue_end));
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 400)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 5:
                    FadeOut(120);
                    break;
                case 6:
                    if (timer > 120)
                    {
                        UnloadContent();
                        game.chapterState = Game1.ChapterState.chapterOne;
                        game.CurrentChapter.state = Chapter.GameState.Cutscene;
                        game.CurrentChapter.CutsceneState = 1;
                        game.CurrentChapter = game.ChapterOne;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.StartingPortal = GymLobby.ToNorthHall;
                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["Gym Lobby"];
                        Game1.Player.YScroll = game.CurrentChapter.CurrentMap.yScroll;
                        Chapter.lastMap = "North Hall";
                        Sound.StopAmbience();
                        Sound.StopBackgroundMusic();
                        game.CurrentChapter.CurrentMap.LoadContent();
                        timer = 0;
                        tim.PositionX = -1000;
                        tim.UpdateRecAndPosition();
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

                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statements CH.1"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Combustible Confutation CH.1"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);
                        game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Crushing Realization"]);

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
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
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
                        if (player.Health < (player.realMaxHealth / 4))
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

                    if (timer > 300)
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

                    s.Draw(complete, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 6:

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
