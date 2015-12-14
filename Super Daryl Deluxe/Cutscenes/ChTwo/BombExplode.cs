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
    class BombExplode : Cutscene
    {
        // ATTRIBUTES \\
        GameObject camFollow = new GameObject();
        NPC napoleon;

        int otherTimer = 0;
        SoundEffect cutscene_tsar_bomb_explosion;
        Texture2D explosionTexture;
        //--Takes in a background and all necessary objects
        public BombExplode(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            explosionTexture = content.Load<Texture2D>(@"Maps\\History\\Explosion");
            cutscene_tsar_bomb_explosion = content.Load<SoundEffect>(@"Sound\Cutscenes\cutscene_tsar_bomb_explosion");
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
                        LoadContent();
                        napoleon = new NPC(game.NPCSprites["Napoleon"], new List<String>(), new Rectangle(500, 370, 516, 388), player, game.Font, game, "", "Napoleon", false);
                    }
                    if (game.CurrentChapter.CurrentMap.MapName == "Stone Fort - Central")
                    {
                        if (player.PositionX < 600)
                            player.CutsceneRun(new Vector2(player.MoveSpeed, 0));
                        else
                        {
                            state++;
                            timer = 0;
                        }
                    }
                    else
                    {
                        if(player.PositionX > 6800)
                            player.CutsceneRun(new Vector2(-player.MoveSpeed, 0));
                        else
                        {
                            state++;
                            timer = 0;
                        }
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        napoleon.Dialogue.Clear();
                        napoleon.Dialogue.Add("You really gave it to zem. Ze bomb should be going off any time now...");
                        napoleon.Talking = true;
                        player.MoveFrame = 0;
                        player.playerState = Player.PlayerState.standing;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    napoleon.UpdateInteraction();

                    if (napoleon.Talking == false)
                    {
                        timer = 0;
                        state++;
                        Sound.StopAmbience();
                        Sound.PauseBackgroundMusic();

                        Sound.PlaySoundInstance(cutscene_tsar_bomb_explosion, Game1.GetFileName(() => cutscene_tsar_bomb_explosion));
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        game.Camera.ShakeCamera(60, 30);
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    FadeOut(30);
                    break;
                case 3:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    FadeIn(60);
                    break;
                case 4:
                    if (timer > 150)
                    {
                        timer = 0;
                        state++;
                    }
                    break;
                case 5:
                        FadeOut(60);
                    break;
                case 6:

                    if (firstFrameOfTheState)
                    {
                        game.CurrentChapter.CurrentMap.PlayAmbience();
                        game.Camera.ShakeCamera(45, 3);
                        napoleon.Dialogue.Clear();
                        napoleon.Dialogue.Add("Ah yes, zhere it is. Yes.");
                        napoleon.Dialogue.Add("Uh..why don't you go check ze area? Radiation has not been discovered yet...you will be fine.");
                    }
                    Chapter.effectsManager.Update();
                    napoleon.UpdateInteraction();

                    if (timer == 20)
                    {
                        napoleon.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    game.Camera.center = game.Camera.centerTarget;

                    if (napoleon.Talking == false && timer > 60)
                    {
                        game.CurrentChapter.CurrentMap.enteringMap = false;
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        game.ChapterTwo.ChapterTwoBooleans["horseInWest"] = false;
                        game.ChapterTwoDemo.ChapterTwoBooleans["horseInWest"] = false;

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
                    if (napoleon != null && napoleon.Talking)
                        napoleon.DrawDialogue(s);
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
                    DrawFadeWhite(s, 0);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    DrawFadeWhite(s, 1);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    s.End();
                    break;
                case 5:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 6:

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
                    if (napoleon != null && napoleon.Talking)
                        napoleon.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}