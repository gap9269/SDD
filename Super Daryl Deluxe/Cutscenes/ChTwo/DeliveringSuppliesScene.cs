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
    class DeliveringSuppliesScene : Cutscene
    {
        NPC privateBrian;

        public DeliveringSuppliesScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();

            game.Camera.Update(player, game, game.CurrentChapter.CurrentMap);
            player.CutsceneStand();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        privateBrian = game.ChapterTwo.NPCs["Private Brian"];
                        privateBrian.ClearDialogue();
                        privateBrian.Dialogue.Add("Zeez must be ze supplies zat we have been waiting for. You can set zem down over here.");
                        privateBrian.Dialogue.Add("I hope zat our leader remembered to send ze toilet paper zis time, ohoh.");
                        privateBrian.Dialogue.Add("Oh yes, I saw ze tall gray man entering ze enemy's fort up ahead. It zeemed like he belonged, by ze way he waltzed in so casually.");
                        privateBrian.Talking = true;
                    }

                    privateBrian.UpdateInteraction();

                    if (!privateBrian.Talking)
                    {
                        privateBrian.ClearDialogue();
                        privateBrian.Dialogue.Add("I wonder what ze gray man is doing in zhere...");
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.ChapterTwo.NPCs["Napoleon"].CompleteQuestSilently(game.ChapterTwo.deliveringSupplies);
                        game.ChapterTwo.NPCs["French Soldier"].ClearDialogue();
                        game.ChapterTwo.NPCs["French Soldier"].Dialogue.Add("I 'ave not pooped in days. Where is ze toilet paper? Ohoho.");

                        game.ChapterTwo.NPCs["Julius"].MapName = "Napoleon's Camp";
                        game.ChapterTwo.NPCs["Julius"].RecX = 1582;
                        game.ChapterTwo.NPCs["Julius"].RecY = 300;
                        game.ChapterTwo.NPCs["Julius"].FacingRight = true;
                        game.ChapterTwo.NPCs["Julius"].PositionX = game.ChapterTwo.NPCs["Julius"].RecX;
                        game.ChapterTwo.NPCs["Julius"].PositionY = game.ChapterTwo.NPCs["Julius"].RecY;
                        game.ChapterTwo.NPCs["Julius"].UpdateRecAndPosition();
                        game.ChapterTwo.NPCs["Julius"].ClearDialogue();
                        game.ChapterTwo.NPCs["Julius"].Dialogue.Add("Cleo, my beloved, where art thou? I have come for you!");

                        game.ChapterTwo.NPCs["Napoleon"].MapName = "Napoleon's Camp";
                        game.ChapterTwo.NPCs["Napoleon"].RecX = 1869;
                        game.ChapterTwo.NPCs["Napoleon"].RecY = 313;
                        game.ChapterTwo.NPCs["Napoleon"].FacingRight = false;
                        game.ChapterTwo.NPCs["Napoleon"].PositionX = game.ChapterTwo.NPCs["Napoleon"].RecX;
                        game.ChapterTwo.NPCs["Napoleon"].PositionY = game.ChapterTwo.NPCs["Napoleon"].RecY;
                        game.ChapterTwo.NPCs["Napoleon"].UpdateRecAndPosition();
                        game.ChapterTwo.NPCs["Napoleon"].ClearDialogue();
                        game.ChapterTwo.NPCs["Napoleon"].Dialogue.Add("Oh, thank the lord, zhere you are!");
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

                    if (privateBrian != null && privateBrian.Talking)
                        privateBrian.DrawDialogue(s);
                    s.End();

                    break;
            }
        }
    }
}
