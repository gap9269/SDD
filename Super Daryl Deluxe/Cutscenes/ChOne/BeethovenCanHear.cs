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
    class BeethovenCanHear : Cutscene
    {
        NPC beethoven;
        int talkingState;
        int specialTimer;

        public BeethovenCanHear(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
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
                        beethoven = game.CurrentChapter.NPCs["Beethoven"];
                        beethoven.RemoveQuest(beethoven.Quest);
                        beethoven.ClearDialogue();
                        beethoven.Dialogue.Add("Now that I can hear whatever it is you are saying, let us discuss your new job as my servant boy.");
                        beethoven.Dialogue.Add("That is why you are here, correct?");
                        beethoven.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    beethoven.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (beethoven.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                        specialTimer = 0;
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        beethoven.Dialogue.Clear();
                        beethoven.AddQuest(game.ChapterOne.dealingWithManagement);
                        beethoven.Talking = true;
                    }

                    beethoven.Choice = 0;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    beethoven.UpdateInteraction();

                    if (beethoven.Talking == false)
                    {
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
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
                    if(timer > 1)
                        beethoven.DrawDialogue(s);
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

                    break;
                case 2:
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
                    beethoven.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
