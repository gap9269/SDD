using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class ChapterTwoEndTwo : Cutscene
    {
        // ATTRIBUTES \\
        NPC princess;
        public ChapterTwoEndTwo(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        princess = game.ChapterTwo.NPCs["The Princess"];
                        LoadContent();
                        princess.ClearDialogue();
                        princess.Dialogue.Add("...");
                        princess.Dialogue.Add("I expected you to at least, you know, go home before you came back with a phone.");
                        princess.Dialogue.Add("Maybe you're not as stupid as I thought. I knew Daddy wouldn't let someone like that into his school.");
                        princess.Dialogue.Add("Put that number in your phone. Now it's your job to keep me happy, or I'll tell Daddy to destroy you.");
                        princess.Dialogue.Add("It's almost the end of the day, so you'll start your job tomorrow. Don't keep me waiting or I'll scream.");
                        princess.FacingRight = false;
                        player.FacingRight = true;
                        player.Position = new Vector2(300, -35);
                        player.UpdatePosition();
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    FadeIn(120);
                    break;
                case 1:

                    if (timer == 60)
                        princess.Talking = true;

                    if(princess.Talking)
                        princess.UpdateInteraction();


                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!princess.Talking && timer > 60)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 2:

                    FadeOut(120);
                    break;
                case 3:
                    state = 0;
                    timer = 0;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "North Hall"), NorthHall.ToBathroom);
                    game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:
                case 1:
                case 2:
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
                    if (princess != null && princess.Talking)
                        princess.DrawDialogue(s);
                    if (state == 0)
                        DrawFade(s, 1);
                    if (state == 2)
                        DrawFade(s, 0);
                    s.End();
                    break;

                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
