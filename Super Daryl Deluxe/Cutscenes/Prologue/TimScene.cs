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
        MovingPlatform light;
        GameObject camFollow; //An object for the camera to follow
        float nameAlpha = 1f;

        //--Takes in a background and all necessary objects
        public TimScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
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
                        tim = game.CurrentChapter.NPCs["Tim"];
                        alan = game.CurrentChapter.NPCs["Alan"];
                        paul = game.CurrentChapter.NPCs["Paul"];
                        tim.PositionX = player.PositionX - 900;
                        tim.PositionY = 680 - 388;
                        dialogue.Add("Which one of you faggots put the flowers in my locker and stole my lunch money?");
                        DialogueState = 0;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        player.FacingRight = false;
                        camFollow = new GameObject();
                        camFollow.PositionX = camera.center.X;

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

                    if (timer < 90 && timer > 30)
                    camFollow.PositionX -= 5;

                    if (Vector2.Distance(player.Position, tim.Position) > 530)
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
                        tim.Dialogue.Add("I know it was one of you two. I'm sick of your shit.");
                        tim.Dialogue.Add("I don't know how you keep getting into my locker, but this is the last time it's going to happen.");
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
                        tim.Dialogue.Add("You're new, eh? Well I'll give you one chance to say sorry to me, kid, or else you're gonna get it.");
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
                        tim.Dialogue.Add("Too tough to say anything, huh?");
                        tim.Dialogue.Add("That's it!");
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
                    if (timer >2)
                    {
                        timer = 0;
                        state++;
                        player.playerState = Player.PlayerState.standing;
                    }
                    break;

                case 11:
                    if (firstFrameOfTheState)
                    {
                        tim.PositionX -= 120;
                    }

                    player.CutsceneStand();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    if (timer > 61) //&& stayStill)
                    {

                        //--Pillars
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3200, 0, 100, 800), false, false, false));
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5200, 0, 100, 800), false, false, false));

                        //--Steps on Pillars
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3300, 400, 100, 50), true, false, false));
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5050, 350, 100, 50), true, false, false));

                        //--Lights
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3650, 220, 400, 50), true, false, false));
                        List<Vector2> targets = new List<Vector2>();
                        targets.Add(new Vector2(4375, 225));
                        targets.Add(new Vector2(4350, 220));
                        targets.Add(new Vector2(4375, 225));
                        targets.Add(new Vector2(4400, 220));
                        light = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4400, 220, 400, 50), true, false, false, targets, 1, 50);
                        Game1.schoolMaps.maps["NorthHall"].Platforms.Add(light);



                        GorillaTim gorillaTim = new GorillaTim(new Vector2(tim.Position.X, 630 - 300), "GORILLA TIM", game, ref player, Game1.schoolMaps.maps["NorthHall"]);
                        tim.Move(new Vector2(-10000, 0));
                        gorillaTim.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3300, 400, 100, 50), true, false, false));
                        gorillaTim.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5050, 350, 100, 50), true, false, false));
                        game.CurrentChapter.CurrentBoss = gorillaTim;
                        game.CurrentChapter.BossFight = true;
                        timer = 0;
                        state++;
                    }
                    break;
                case 12:
                    if (firstFrameOfTheState)
                    {
                        camFollow.PositionX = 4486;
                        
                    }
                    light.Update();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if(timer > 60)
                        game.CurrentChapter.CurrentBoss.HealthBarGrow();

                    player.CutsceneStand();


                    if (timer > 200)
                    {
                        if (nameAlpha > 0)
                            nameAlpha -= .01f;
                        else
                            game.CurrentChapter.CurrentBoss.DrawHUDName = true;
                    }
                    if (timer > 350)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
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
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                     

                    if(timer > 2)
                        DrawDialogue(s);
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
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(game.EnemySpriteSheets["GorillaTimName"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * nameAlpha);

                     

                    if(timer > 60)
                        game.CurrentChapter.CurrentBoss.DrawHud(s);

                    s.End();
                    break;

            }
        }
    }
}