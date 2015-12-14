using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class ChapterTwoEndOne : Cutscene
    {
        // ATTRIBUTES \\
        NPC princess, alan, paul, balto, chelsea;
        SoundEffect popup_prologue_end;
        Boolean playedRobattoEnd = false;

        int flashTimer = 0;

        public ChapterTwoEndOne(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
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
                        flashTimer = -15;
                        LoadContent();
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    flashTimer++;

                    if (flashTimer >= 100)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (timer > 120)
                    {
                        state = 0;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "Princess' Room"), PrincessLockerRoom.ToUpperVentsI);
                        game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
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

                    if(game.CurrentChapter.BossFight)
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

                         if (game.CurrentChapter.BossFight)
                            game.CurrentChapter.CurrentBoss.DrawHud(s);

                        //HUD
                        game.CurrentChapter.HUD.Draw(s);

                        //Screen tint based on Daryl's low health
                        if (player.Health < (player.realMaxHealth / 4))
                        {
                            s.Draw(Game1.lowHealthTint, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * game.CurrentChapter.tintAlpha);
                        }

                    if(flashTimer < -9|| (flashTimer > -5 && flashTimer < 0))
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.White);
                    else if(flashTimer <= 100)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * ((flashTimer / 100f) * 2));
                    s.End();

                    break;
                case 1:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
