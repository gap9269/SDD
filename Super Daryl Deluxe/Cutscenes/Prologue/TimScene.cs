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
    class TimScene : Cutscene
    {
        // ATTRIBUTES \\
        NPC tim;
        NPC alan;
        NPC paul;
        GameObject camFollow; //An object for the camera to follow
        float nameAlpha = 1f;
        int timFrame;
        int timDelay = 5;
        float leftPlatY, rightPlatY, platVelocity;

        int timesTimStandLooped = 0;
        int timTransformTimer = 0;
        Boolean transformed = false;
        Boolean mapTransformed = false;
        Boolean platformsFall = false;
        Boolean drawTitle = false;
        Boolean updateBossHealth = false;
        enum TimState
        {
            stand, snarl, pound
        }
        TimState timState;

        float gorillaWordPositionX, timWordPositionX;
        int timeWordsHangInCenter = 100;

        //--Takes in a background and all necessary objects
        public TimScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public void TimStand()
        {
            timDelay--;

            if (timDelay <= 0)
            {
                timFrame++;
                timDelay = 5;

                //During transformation, only play about half of the standing animation
                if (!transformed)
                {
                    if (timFrame > 3)
                    {
                        timesTimStandLooped++;
                        timFrame = 0;

                        if (timesTimStandLooped == 1)
                        {
                            timesTimStandLooped = 0;
                            timState = TimState.pound;
                            timTransformTimer = 0;
                        }
                    }
                }
                    //Otherwise loop a couple times, then snarl
                else
                {
                    if (timFrame > 5)
                    {
                        timesTimStandLooped++;
                        timFrame = 0;

                        if (timesTimStandLooped == 5)
                        {
                            timesTimStandLooped = 0;
                            timState = TimState.snarl;
                        }
                    }
                }
            }
        }

        public void TimSnarl()
        {
            timDelay--;

            if (timDelay <= 0)
            {
                timFrame++;
                timDelay = 5;

                if (timFrame > 4)
                {
                    timFrame = 0;
                    timState = TimState.stand;
                }
                
            }
        }

        public void TimGroundPound()
        {
            timDelay--;

            if (timDelay <= 0)
            {
                timFrame++;
                timDelay = 5;

                if (timFrame == 7)
                {
                    platformsFall = true;
                    game.Camera.ShakeCamera(30, 15);
                }

                if (timFrame > 12)
                {
                    timFrame = 0;
                    timState = TimState.stand;
                    timTransformTimer = 0;
                    transformed = true;
                }
            }
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        leftPlatY = -150;
                        rightPlatY = -75;
                        tim = game.CurrentChapter.NPCs["Tim"];
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        tim.PositionX = player.PositionX - 1200;
                        tim.PositionY = 670 - 388;
                        dialogue.Add("Which one of you assholes put the flowers in my locker and stole my lunch money?");
                        DialogueState = 0;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        player.FacingRight = false;
                        camFollow = new GameObject();
                        camFollow.PositionX = camera.center.X;
                        player.Sprinting = false;
                        player.PositionX = 3110;
                        player.UpdatePosition();
                        Chapter.effectsManager.ClearDustPoofs();
                        alan.Dialogue.Clear();
                        paul.Dialogue.Clear();

                        if (alan.QuestDialogue != null)
                            alan.QuestDialogue.Clear();

                        if (paul.QuestDialogue != null)
                            paul.QuestDialogue.Clear();
                    }

                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);

                    if (timer > 20)
                        alan.FacingRight = false;
                    if (timer > 40)
                        paul.FacingRight = false;

                    if (camFollow.PositionX > 3075 && timer > 30)
                    camFollow.PositionX -= 6;

                    if (tim.Position.X < 2550)
                    {
                        tim.Move(new Vector2(3, 0));
                    }
                    else
                    {
                        dialogue.Clear();
                        tim.Dialogue.Clear();
                        tim.moveState = NPC.MoveState.standing;
                        timer = 0;
                        state++;
                    }
                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        tim.Dialogue.Add("Yeah, it was hilarious sending me to the hospital the last time.  Life's a fuckin' joke to people with deadly pollen allergies.");
                        tim.Dialogue.Add("I know it was one of you two. I don't know how you keep getting into my locker, but this is the last time it's going to happen. I'm gonna send you both to the hospital!");
                        tim.Talking = true;
                    }

                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    tim.UpdateInteraction();

                    if (tim.Talking == false)
                    {
                        tim.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 2:
                    if (firstFrameOfTheState)
                    {
                        alan.Dialogue.Add("It wasn't us, Tim! Dan did it.");
                        alan.Talking = true;
                    }

                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    alan.UpdateInteraction();

                    if (alan.Talking == false)
                    {
                        alan.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 3:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Add("Yeah! We didn't know anything about it.");
                        paul.Talking = true;
                    }

                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    paul.UpdateInteraction();

                    if (paul.Talking == false)
                    {
                        paul.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 4:
                    if (firstFrameOfTheState)
                    {
                        tim.Dialogue.Add("Who the hell is Dan?");
                        tim.Talking = true;
                    }

                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    tim.UpdateInteraction();

                    if (tim.Talking == false)
                    {
                        tim.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 5:
                    if (firstFrameOfTheState)
                    {
                        paul.Dialogue.Add("He's right here! He's new.");
                        paul.Dialogue.Add("Way to go, you pissed Tim off. We're outta here.");
                        paul.Talking = true;
                    }
                    if (paul.DialogueState > 0)
                        paul.FacingRight = true;

                    paul.UpdateInteraction();
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    if (paul.Talking == false)
                    {
                        paul.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 6:
                    if (firstFrameOfTheState)
                    {
                        alan.FacingRight = true;
                    }
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    if (timer > 2)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 7:
                    if (firstFrameOfTheState)
                    {
                        tim.Dialogue.Add("New, huh? Well I'll give you one chance to say sorry to me, or else you're dead.");
                        tim.Talking = true;
                        paul.Position = new Vector2(paul.RecX, paul.RecY);
                        alan.Position = new Vector2(alan.RecX, alan.RecY);
                    }

                    paul.Move(new Vector2(3, 0));
                    alan.Move(new Vector2(3, 0));

                    tim.UpdateInteraction();
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    if (tim.Talking == false)
                    {
                        tim.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 8:

                    paul.Move(new Vector2(3, 0));
                    alan.Move(new Vector2(3, 0));
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    if(timer > 320)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 9:
                    if (firstFrameOfTheState)
                    {
                        tim.Dialogue.Add("Too tough to say anything, huh? That's it!");
                        tim.Talking = true;
                    }

                    paul.Move(new Vector2(5, 0));
                    alan.Move(new Vector2(5, 0));
                    tim.UpdateInteraction();
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    if (tim.Talking == false)
                    {
                        tim.Dialogue.Clear();
                        timer = 0;
                        state++;
                    }
                    break;

                case 10:

                    //TIM TRANSFORMS
                    camera.Update(camFollow, game, Game1.schoolMaps.maps["NorthHall"]);
                    NorthHall.drawTimMap = true;

                    //Tim transforms
                    if (!transformed)
                    {

                        timTransformTimer++;

                        if (timState == TimState.stand)
                        {

                            if (timTransformTimer < 10 || timTransformTimer > 25 && timTransformTimer < 45 || timTransformTimer > 65 && timTransformTimer < 85 || timTransformTimer > 105 && timTransformTimer < 125 || timTransformTimer > 155 && timTransformTimer < 170 || timTransformTimer > 190)
                            {
                                NorthHall.drawTimMap = true;
                                TimStand();
                            }
                            else
                                NorthHall.drawTimMap = false;

                        }
                        else if (timState == TimState.pound)
                        {
                            if (timTransformTimer < 5 || timTransformTimer > 15 && timTransformTimer < 25 || timTransformTimer > 35 && timTransformTimer < 40 || timTransformTimer > 45 && timTransformTimer < 50 || timTransformTimer > 55)
                            {
                                NorthHall.drawTimMap = true;
                                TimGroundPound();
                            }
                            else
                                NorthHall.drawTimMap = false;
                        }
                    }
                    //After, the map transforms
                    if (!mapTransformed && platformsFall)
                    {
                        player.playerState = Player.PlayerState.standing;
                        player.CutsceneStand();
                        if (timState == TimState.stand)
                        {
                            TimStand();
                        }
                        else if (timState == TimState.snarl)
                        {
                            TimSnarl();
                        }

                        #region Make platforms drop in
                        platVelocity += GameConstants.GRAVITY;
                        if (leftPlatY < 190)
                        {
                            leftPlatY += platVelocity;

                            if (leftPlatY > 175)
                                leftPlatY = 175;
                        }
                        if (rightPlatY < 175)
                        {
                            rightPlatY += platVelocity;

                            if (rightPlatY > 175)
                                rightPlatY = 175;
                        }

                        if (leftPlatY == 175 && rightPlatY == 175)
                            mapTransformed = true;
                        #endregion
                    }

                    if(transformed && mapTransformed)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 11:
                    if (firstFrameOfTheState)
                    {
                        player.CutsceneStand();
                        camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                        //--Pillars
                        NorthHall.leftPillar = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2200, -1000, 100, 3000), false, false, false);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.leftPillar);
                        NorthHall.rightPillar = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4000, -1000, 100, 3000), false, false, false);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.rightPillar);

                        //--Steps on Pillars
                        NorthHall.leftStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2300, 400, 100, 50), false, false, false);
                        NorthHall.rightStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3850, 350, 100, 50), false, false, false);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.leftStep);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.rightStep);

                        //--Platforms
                        NorthHall.leftTimPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2600, 175, 400, 50), true, false, false);
                        NorthHall.rightTimPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3250, 175, 400, 50), true, false, false);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.leftTimPlat);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(NorthHall.rightTimPlat);

                        //Barrels
                        NorthHall.timBar1 = new Barrel(game, 3613, 456 + 155, Game1.interactiveObjects["Barrel"], true, 4, 5, 0f, false, Barrel.BarrelType.TimBarrel);
                        NorthHall.timBar2 = new Barrel(game, 2919, 456 + 155, Game1.interactiveObjects["Barrel"], true, 4, 6, .04f, false, Barrel.BarrelType.TimBarrel);
                        NorthHall.timBar3 = new Barrel(game, 2454, 550 + 155, Game1.interactiveObjects["Barrel"], true, 4, 7, .14f, true, Barrel.BarrelType.TimBarrel);

                        Game1.schoolMaps.maps["NorthHall"].InteractiveObjects.Add(NorthHall.timBar1);
                        Game1.schoolMaps.maps["NorthHall"].InteractiveObjects.Add(NorthHall.timBar2);
                        Game1.schoolMaps.maps["NorthHall"].InteractiveObjects.Add(NorthHall.timBar3);
                        
                        GorillaTim gorillaTim = new GorillaTim(new Vector2((int)tim.PositionX + 55, 118), "GORILLA TIM", game, ref player, Game1.schoolMaps.maps["NorthHall"]);
                        tim.Move(new Vector2(-10000, 0));
                        gorillaTim.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2300, 400, 100, 50), true, false, false));
                        gorillaTim.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3850, 350, 100, 50), true, false, false));
                        game.CurrentChapter.CurrentBoss = gorillaTim;
                        gorillaTim.moveFrame = timFrame;
                        gorillaTim.CutsceneStand();
                        gorillaTim.faceTexture = GorillaTim.animationTextures["GorillaTimFace"];
                        game.CurrentChapter.BossFight = true;
                        timer = 0;
                        state++;
                    }
                    break;
                case 12:
                    if (firstFrameOfTheState)
                    {
                        timWordPositionX = 1400;
                        gorillaWordPositionX = -650;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 45 && drawTitle == false && updateBossHealth == false)
                        drawTitle = true;

                    if (drawTitle && gorillaWordPositionX < 1280 && timWordPositionX > -311)
                    {
                        if (timer > 60)
                        {
                            if (gorillaWordPositionX >= 250 && gorillaWordPositionX < 300)
                            {
                                gorillaWordPositionX += .5f;
                                timWordPositionX -= .5f;
                            }
                            else
                            {
                                gorillaWordPositionX += 40;
                                timWordPositionX -= 40;
                            }
                        }
                    }
                    else if(drawTitle == true)
                    {
                        drawTitle = false;
                        updateBossHealth = true;
                        timer = 2;
                    }

                    if (drawTitle == false)
                    {
                        player.CutsceneStand();
                        (game.CurrentChapter.CurrentBoss as GorillaTim).CutsceneStand();

                        if(updateBossHealth)
                            game.CurrentChapter.CurrentBoss.HealthBarGrow();
                    }

                    if (timer > 180 && drawTitle == false)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                    }
                    break;
            }
        }

        public void DrawTim(SpriteBatch s)
        {
            if (NorthHall.drawTimMap)
            {
                s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(3613, 456, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);
                s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(2919, 456, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);
                s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(2454, 550, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);

                if (timState == TimState.stand)
                {
                    s.Draw(GorillaTim.animationTextures["Stand" + timFrame], new Rectangle((int)tim.PositionX + 55, 118, (int)(940 * .65f), (int)(796 * .65f)), Color.White);
                }
                else if (timState == TimState.pound)
                {
                    s.Draw(GorillaTim.animationTextures["pound" + timFrame], new Rectangle((int)tim.PositionX + 55, 118, (int)(940 * .65f), (int)(796 * .65f)), Color.White);
                }
                else if (timState == TimState.snarl)
                {
                    s.Draw(GorillaTim.animationTextures["Snarl" + timFrame], new Rectangle((int)tim.PositionX + 55, 118, (int)(940 * .65f), (int)(796 * .65f)), Color.White);
                }
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
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     

                    if(timer > 2)
                        DrawDialogue(s, true);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     
                    tim.DrawDialogue(s);
                    s.End();
                    break;

                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    s.Draw(NorthHall.timPlatform, new Rectangle(2590, (int)leftPlatY, 420, 50), Color.White);
                    s.Draw(NorthHall.timPlatform, new Rectangle(3240, (int)rightPlatY, 420, 50), Color.White);

                    if (NorthHall.drawTimMap)
                        DrawTim(s);
                    else
                        game.CurrentChapter.DrawNPC(s);

                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     

                    if (tim.Talking)
                        tim.DrawDialogue(s);
                    if (alan.Talking)
                        alan.DrawDialogue(s);
                    if (paul.Talking)
                        paul.DrawDialogue(s);
                    s.End();
                    break;
                case 12:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);

                    s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(3613, 456, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);
                    s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(2919, 456, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);
                    s.Draw(Game1.interactiveObjects["Barrel"], new Rectangle(2454, 550, 105, 155), new Rectangle(0, 775, 105, 155), Color.White);

                    player.Draw(s);
                    game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (drawTitle)
                    {
                        s.Draw(GorillaTim.animationTextures["BossTitleBar"], new Vector2(0, 0), Color.White * nameAlpha);

                        s.Draw(GorillaTim.animationTextures["GORILLA"], new Vector2((int)gorillaWordPositionX, 191), Color.White);
                        s.Draw(GorillaTim.animationTextures["TIM"], new Vector2((int)timWordPositionX, 359), Color.White);
                    }
                    
                    if(updateBossHealth)
                    {
                        game.CurrentChapter.CurrentBoss.DrawHud(s);
                    }

                    s.End();
                    break;

            }
        }
    }
}