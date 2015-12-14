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
    class TrollAppear : Cutscene
    {
        // ATTRIBUTES \\
        GameObject camFollow = new GameObject();
        NPC napoleon;

        int otherTimer = 0;
        SoundEffect cutscene_troll_burst_through_wall;
        //--Takes in a background and all necessary objects
        public TrollAppear(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            cutscene_troll_burst_through_wall = content.Load<SoundEffect>(@"Sound\Cutscenes\cutscene_troll_burst_through_wall");
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
                        game.CurrentChapter.CurrentMap.EnemiesInMap.Clear();
                        game.CurrentChapter.CurrentMap.ResetEnemyNamesAndNumberInMap();
                        napoleon = new NPC(game.NPCSprites["Napoleon"], new List<String>(), new Rectangle(500, 370, 516, 388), player, game.Font, game, "", "Napoleon", false);
                        player.PositionX = 6050;
                        player.PositionY = 308;
                        player.VelocityX = 0;

                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        game.Camera.center = game.Camera.centerTarget;

                        StoneFortWest.horse.PositionX = 6000;
                        StoneFortWest.horse.RecX = 6000;

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
                        napoleon.Dialogue.Add("You 'ave done it! Victory iz at our doorstep!");
                        napoleon.Talking = true;
                    }

                    player.StopSkills();

                    napoleon.UpdateInteraction();

                    if (napoleon.Talking == false && (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll == null)
                    {
                        game.Camera.ShakeCamera(30, 15);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(6650, 340, 250, 250), 2);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(6500, 380, 300, 300), 2);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(6800, 420, 250, 250), 2);
                        game.ChapterTwo.ChapterTwoBooleans["trollSpawnedInWest"] = true;
                        game.ChapterTwoDemo.ChapterTwoBooleans["trollSpawnedInWest"] = true;
                        napoleon.Dialogue.Clear();
                        napoleon.Dialogue.Add("SACREBLEU! Zey have a bald zasquatch!");
                        napoleon.Dialogue.Add("Ze bald zasquatch killed my fazher, kill him before he does ze zame to you!");

                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll = new FieldTroll(new Vector2(6400, 135), "Field Troll", game, ref player, game.CurrentChapter.CurrentMap);
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.Hostile = true;
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.FacingRight = false;
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.SpawnWithPoof = false;
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.TimeBeforeSpawn = 0;
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.UpdateRectangles();
                        (Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll.Alpha = 1;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList((Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll);
                        LoadContent();
                        Sound.PlaySoundInstance(cutscene_troll_burst_through_wall, Game1.GetFileName(() => cutscene_troll_burst_through_wall));
                    }

                    if ((Game1.schoolMaps.maps["Stone Fort - West"] as StoneFortWest).troll != null)
                        otherTimer++;

                    if (otherTimer == 50)
                        napoleon.Talking = true;

                    Chapter.effectsManager.Update();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (napoleon.Talking == false && otherTimer >= 60)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.playerState = Player.PlayerState.standing;
                        UnloadContent();
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

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(napoleon != null && napoleon.Talking)
                        napoleon.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}