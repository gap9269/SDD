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
    class GhostLockGone : Cutscene
    {
        // ATTRIBUTES \\
        GameObject camFollow = new GameObject();
        NPC claire;
        NPC jason, ken, steve;
        int talkingState;

        //--Takes in a background and all necessary objects
        public GhostLockGone(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
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

            Chapter.effectsManager.Update();

            switch (state)
            {
                case 0:

                    FadeOut(40);
                    Chapter.effectsManager.Update();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 1:
                    Chapter.effectsManager.Update();
                    if (timer > 40)
                    {
                        timer = 0;
                        state++;
                    }
                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        player.PositionX = 1780;
                        player.PositionY = 311;
                        player.VelocityX = 0;

                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        game.Camera.center = game.Camera.centerTarget;

                        player.playerState = Player.PlayerState.relaxedStanding;

                        player.UpdatePosition();
                        player.FacingRight = true;
                        player.StopSkills();
                        player.KnockedBack = false;
                    }
                    Chapter.effectsManager.Update();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    game.Camera.center = game.Camera.centerTarget;

                    FadeIn(60);
                    break;
                case 3:

                    if (firstFrameOfTheState)
                    {
                        claire = game.CurrentChapter.NPCs["Claire Voyant"];
                        jason = game.CurrentChapter.NPCs["Jason Mysterio"];
                        claire.RemoveQuest(ChapterOne.protectTITS);
                        claire.ClearDialogue();
                        jason.ClearDialogue();

                        ken = new NPC(game.NPCSprites["Ken Speercy"], new List<String>(), new Rectangle(), player, game.Font, game, "Tenant Hallway West", "Ken Speercy", false);
                        steve = new NPC(game.NPCSprites["Steve Pantski"], new List<String>(), new Rectangle(), player, game.Font, game, "Tenant Hallway West", "Steve Pantski", false);

                        steve.Dialogue.Add("Is..is it over?");
                        ken.Dialogue.Add("We did it!");
                        jason.Dialogue.Add("Good job, Claire. You really saved us there. And the Ghost Sucker actually worked, the Lock Ghost is gone! Ken, did you get it all?");
                        claire.Dialogue.Add("It irks me to ruin this celebration, but judging by his blank stare it would appear that by controlling this boy's brain I may have caused some severe mental damage.");
                        Sound.PlaySoundInstance(Portal.object_lock_open, "object_lock_open");

                        TenantHallwayWest.ToBeethovensRoom.startedToOpen = true;
                    }

                    player.StopSkills();

                    game.CurrentChapter.CurrentMap.Update();

                    Chapter.effectsManager.Update();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (TenantHallwayWest.ToBeethovensRoom.ItemNameToUnlock == "" || TenantHallwayWest.ToBeethovensRoom.ItemNameToUnlock == null)
                    {
                        timer = 0;
                        state++;
                    }
                    break;
                case 4:
                    if (firstFrameOfTheState)
                    {
                        steve.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (steve.Talking == true)
                        steve.UpdateInteraction();
                    else if (claire.Talking == true)
                        claire.UpdateInteraction();
                    else if (ken.Talking)
                        ken.UpdateInteraction();
                    else if (jason.Talking)
                        jason.UpdateInteraction();

                    game.CurrentChapter.CurrentMap.Update();


                    if (steve.Talking == false && claire.Talking == false && ken.Talking == false && jason.Talking == false)
                    {
                        talkingState++;

                        if (talkingState == 1)
                        {
                            ken.Talking = true;
                        }

                        if (talkingState == 2)
                        {
                            jason.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            ken.Dialogue.Clear();
                            ken.Dialogue.Add("Of course I did.");
                            ken.Talking = true;
                        }
                        if (talkingState == 4)
                        {
                            jason.Dialogue.Clear();
                            jason.Dialogue.Add("Ha, we're going to be famous! The most famous kids in Water Falls High! Move over Trenchcoat Kid, the Transparanormal Investigation Team Squad is movin' in.");
                            jason.Talking = true;
                        }
                        if (talkingState == 5)
                        {
                            claire.Talking = true;
                        }
                        if (talkingState == 6)
                        {
                            jason.Dialogue.Clear();
                            jason.Dialogue.Add("He'll be fine.");
                            jason.Talking = true;
                        }
                        if (talkingState == 7)
                        {
                            steve.Dialogue.Clear();
                            steve.Dialogue.Add("Can we get out of here now?? I have a bunch of homework to do! And...and chores!");
                            steve.Talking = true;
                        }
                    }

                    if (talkingState == 8)
                    {
                        claire.canTalk = true;
                        jason.canTalk = true;
                        claire.Dialogue.Clear();
                        jason.Dialogue.Clear();
                        claire.Dialogue.Add("I apologize for taking control of your body back there, but it was necessary to save our lives and the future of the Transparanormal Investigation Team Squad.");
                        jason.Dialogue.Add("We're the Transparanormal Investigation Team Squad. Expect to see us on the news soon, kiddo. We're going to be famous.");
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.playerState = Player.PlayerState.standing;
                        UnloadContent();
                        ChapterOne.protectTITS.RewardPlayer();
                        Game1.questHUD.RemoveQuestFromHelper(ChapterOne.protectTITS);

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
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 0);
                    s.End();

                    break;
                case 1:

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
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
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
                        DrawFade(s, 1);
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
                    break;
                case 4:

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