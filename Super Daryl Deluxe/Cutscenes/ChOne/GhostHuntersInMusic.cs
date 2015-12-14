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
    class GhostHuntersInMusic : Cutscene
    {
        NPC claire;
        NPC jason, ken, steve;
        int talkingState;

        TubaGhost en, en2;
        public GhostHuntersInMusic(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            game.NPCSprites["Ken Speercy"] = content.Load<Texture2D>(@"NPC\TITS\Ken Speercy");
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\KenSpeercyNormal");

            game.NPCSprites["Steve Pantski"] = content.Load<Texture2D>(@"NPC\TITS\Steve Pantski");
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\StevePantskiNormal");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            game.NPCSprites["Ken Speercy"] = Game1.whiteFilter;
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Steve Pantski"] = Game1.whiteFilter;
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = Game1.whiteFilter;
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
                        claire = game.CurrentChapter.NPCs["Claire Voyant"];
                        jason = game.CurrentChapter.NPCs["Jason Mysterio"];
                        claire.ClearDialogue();
                        jason.ClearDialogue();

                        ken = new NPC(game.NPCSprites["Ken Speercy"], new List<String>(), new Rectangle(), player, game.Font, game, "Tenant Hallway West", "Ken Speercy", false);

                        steve = new NPC(game.NPCSprites["Steve Pantski"], new List<String>(), new Rectangle(), player, game.Font, game, "Tenant Hallway West", "Steve Pantski", false);


                        jason.Dialogue.Add("You gettin' this, Ken? This is it, the big one. I won't have you messing up my series pilot again.");
                        jason.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    jason.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (jason.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        ken.Dialogue.Add("Yeah, yeah, yeah, calm down. I'm getting it.");
                        steve.Dialogue.Add("Gu-guys, I'm detecting high trans-paranormal readings in the vicinity...");
                        claire.Dialogue.Add("Relax, Steven. I would sense the anger and murder in the air if there were any spirits here wanting to harm us.");
                        talkingState = 0;
                        ken.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (steve.Talking == true)
                        steve.UpdateInteraction();
                    else if (claire.Talking == true)
                        claire.UpdateInteraction();
                    else if (ken.Talking)
                        ken.UpdateInteraction();

                    if (steve.Talking == false && claire.Talking == false && ken.Talking == false)
                    {
                        talkingState++;

                        if (talkingState == 1)
                        {
                            steve.Talking = true;
                        }

                        if (talkingState == 2)
                        {
                            claire.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    break;
                case 2:

                    //Add enemy ghosts and poofs
                    if (firstFrameOfTheState)
                    {
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(1481, 420, 200, 200), 2);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(2812, 420, 200, 200), 2);
                        Sound.PlayRandomRegularPoof(1481, 420);
                        Sound.PlayRandomRegularPoof(2812, 420);
                        en = new TubaGhost(new Vector2(1300, 300), "Tuba Ghost", game, ref player, game.CurrentChapter.CurrentMap);
                        en2 = new TubaGhost(new Vector2(2630, 300), "Tuba Ghost", game, ref player, game.CurrentChapter.CurrentMap);
                        en2.FacingRight = false;

                        en.SpawnWithPoof = false;
                        en2.SpawnWithPoof = false;
                        en.objectToAttack = (game.CurrentChapter.CurrentMap as TenantHallwayWest).ghostSucker;
                        en2.objectToAttack = (game.CurrentChapter.CurrentMap as TenantHallwayWest).ghostSucker;

                        en.Alpha = 1;
                        en2.Alpha = 1;

                        en.UpdateRectangles();
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en);
                        en2.UpdateRectangles();
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en2);
                    }

                    en.CutsceneStand();
                    en2.CutsceneStand();
                    Chapter.effectsManager.Update();

                    game.CurrentChapter.CurrentMap.EnemiesInMap[0].RecX = 1300;

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 150)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 3:
                    if (firstFrameOfTheState)
                    {
                        ken.Dialogue.Clear();
                        steve.Dialogue.Clear();
                        jason.Dialogue.Clear();
                        claire.Dialogue.Clear();

                        ken.Dialogue.Add(".......");
                        jason.Dialogue.Add("Hey, shut up! Both of you, just shut the hell up. I'm not going to let some wannabe musician ghost stop me from capturing this Lock Ghost. I've come too far! Ken, keep filming!");
                        steve.Dialogue.Add("AHH! See?! I told you! They don't like us here! They're going to murder all of us! How can you guys just stand there, aren't you scared?!");
                        claire.Dialogue.Add("You think you're scared? Imagine being a lost soul, cursed to wander the world of the living for all eternity...lost...forever...imagine that, Steven.");
                        talkingState = 0;
                        steve.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    en.CutsceneStand();
                    en2.CutsceneStand();

                    if (steve.Talking == true)
                        steve.UpdateInteraction();
                    else if (claire.Talking == true)
                        claire.UpdateInteraction();
                    else if (ken.Talking)
                        ken.UpdateInteraction();
                    else if (jason.Talking)
                        jason.UpdateInteraction();

                    if (steve.Talking == false && claire.Talking == false && ken.Talking == false && jason.Talking == false)
                    {
                        talkingState++;

                        if (talkingState == 1)
                        {
                            claire.Talking = true;
                        }

                        if (talkingState == 2)
                        {
                            jason.Talking = true;
                        }

                        if (talkingState == 3)
                        {
                            ken.Talking = true;
                        }

                        if (talkingState == 4)
                        {
                            claire.Dialogue.Clear();
                            claire.AddQuest(ChapterOne.protectTITS);
                            claire.Talking = true;
                        }
                        if (talkingState == 5)
                        {
                            claire.Dialogue.Clear();
                            jason.Dialogue.Clear();
                            claire.canTalk = false;
                            jason.canTalk = false;
                            claire.Dialogue.Add("I apologize for taking control of your body back there, but it was necessary to save our lives and the future of the Transparanormal Investigation Team Squad.");
                            jason.Dialogue.Add("We're the Transparanormal Investigation Team Squad. Expect to see us on the news soon, kiddo. We're going to be famous.");
                            state = 0;
                            timer = 0;
                            player.playerState = Player.PlayerState.standing;
                            game.CurrentChapter.CutsceneState++;
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            UnloadContent();

                            for (int i = 0; i < game.CurrentChapter.CurrentMap.Portals.Count; i++)
                            {
                                game.CurrentChapter.CurrentMap.Portals.ElementAt(i).Key.IsUseable = false;
                            }

                            game.ChapterOne.BossFight = true;
                            game.ChapterOne.CurrentBoss = (game.CurrentChapter.CurrentMap as TenantHallwayWest).ghostSucker;
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

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (timer > 1)
                    {
                        if (steve.Talking == true)
                            steve.DrawDialogue(s);
                        else if (claire.Talking == true)
                            claire.DrawDialogue(s);
                        else if (ken.Talking)
                            ken.DrawDialogue(s);
                        else if (jason.Talking)
                            jason.DrawDialogue(s);
                    }
                    s.End();
                    break;

                case 1:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (timer > 1)
                    {
                        if (steve.Talking == true)
                            steve.DrawDialogue(s);
                        else if (claire.Talking == true)
                            claire.DrawDialogue(s);
                        else if (ken.Talking)
                            ken.DrawDialogue(s);
                        else if (jason.Talking)
                            jason.DrawDialogue(s);
                    }
                    s.End();
                    break;
                case 2:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    Chapter.effectsManager.DrawPoofs(s);

                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.End();
                    break;
                case 3:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);

                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (timer > 1)
                    {
                        if (steve.Talking == true)
                            steve.DrawDialogue(s);
                        else if (claire.Talking == true)
                            claire.DrawDialogue(s);
                        else if (ken.Talking)
                            ken.DrawDialogue(s);
                        else if (jason.Talking)
                            jason.DrawDialogue(s);
                    }
                    s.End();
                    break;
            }
        }
    }
}
