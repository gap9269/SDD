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
    class HorseDestroyed : Cutscene
    {
        // ATTRIBUTES \\
        GameObject camFollow = new GameObject();

        SoundEffectInstance cutscene_tsar_bomb_explosion;
        Texture2D explosionTexture;
        //--Takes in a background and all necessary objects
        public HorseDestroyed(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            explosionTexture = content.Load<Texture2D>(@"Maps\\History\\Explosion");
            cutscene_tsar_bomb_explosion = content.Load<SoundEffect>(@"Sound\Cutscenes\cutscene_tsar_bomb_explosion").CreateInstance();


        }
        public override void Play()
        {
            base.Play();

            Chapter.effectsManager.Update();

            switch (state)
            {
                case 0:

                    LoadContent();
                    timer = 0;
                    state++;
                    game.CurrentChapter.CurrentMap.StopSounds();
                    Sound.StopAmbience();
                    Sound.PauseBackgroundMusic();
                    Sound.PlaySoundInstance(cutscene_tsar_bomb_explosion, Game1.GetFileName(() => cutscene_tsar_bomb_explosion));
                    Chapter.effectsManager.Update();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    
                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        game.Camera.ShakeCamera(60, 30);
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    FadeOut(30);

                    break;
                case 2:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    FadeIn(60);
                    break;
                case 3:
                    if (timer > 150)
                    {
                        timer = 0;
                        state++;
                    }
                    break;
                case 4:
                    FadeOut(60);
                    break;
                case 5:
                    game.CurrentChapter.CurrentMap.StopSounds();
                    Sound.StopAmbience();
                    Sound.PauseBackgroundMusic();
                    cutscene_tsar_bomb_explosion.Stop();
                    Game1.deathScreen.LoadContent();
                    game.CurrentChapter.state = Chapter.GameState.dead;
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
                    DrawFadeWhite(s, 0);
                    s.End();

                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    DrawFadeWhite(s, 1);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(explosionTexture, new Vector2(0, 0), Color.White);
                    DrawFade(s, 0);
                    s.End();
                    break;
                case 5:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}