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
    class EnteringTheMarket : Cutscene
    {
        NPC artDealer;
        GameObject camFollow;
        int specialState = 0;

        float fadeAlpha = 1f;
        public EnteringTheMarket(Game1 g, Camera cam, Player p)
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

                        artDealer = game.ChapterOne.NPCs["Art Merchant"];
                        camFollow = new GameObject();
                        camFollow.Position = new Vector2(300, 0);

                        artDealer.Dialogue.Clear();
                        artDealer.Dialogue.Add("Attention artists and peddlers alike! I am seeking fine paintings to purchase with my enormous wealth...mhmm, oh yes.");
                    }

                    if (timer == 40)
                        artDealer.Talking = true;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    if (camFollow.PositionX <= 2000)
                    {
                        camFollow.PositionX += 9;
                    }
                    else
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 110)
                    {
                        artDealer.ClearDialogue();
                        artDealer.Dialogue.Add("Oh-hoh! Now this here is a splendid work of art, mhmm, yes.");
                        artDealer.Dialogue.Add("You are quite the artist, and I LOVE the orange pants... mmm. I will purchase this piece for 250 Ducats. A most generous offer, you must agree.");
                        artDealer.Dialogue.Add("Mmhmmm... Is 250 Ducats for this fabulous painting satisfactory? Do we have a deal?");
                        artDealer.Talking = false;
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.enteringMap = false;
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
                case 1:
                case 2:
                case 3:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if(artDealer != null && artDealer.Talking)
                        artDealer.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
