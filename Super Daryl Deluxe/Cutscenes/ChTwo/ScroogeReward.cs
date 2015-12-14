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
    class ScroogeReward : Cutscene
    {
        NPC scrooge;

        public ScroogeReward(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void Play()
        {
            base.Play();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();
            switch (state)
            {

                case 0:
                    if (firstFrameOfTheState)
                    {
                        scrooge = game.CurrentChapter.NPCs["Ebenezer Scrooge"];
                        scrooge.ClearDialogue();

                        scrooge.Dialogue.Add("You've done it! You've rid my life of those terrible spirits!");
                        scrooge.Dialogue.Add("Just in time, too. I was beginning to give consideration to what that final spectre said. But, of course, justice wins in the end.");
                        scrooge.Dialogue.Add("This whole experience has persuaded me to change the way I live my life. From now on I will dedicate all of my time and resources into finding a way to eliminate all ghosts.");
                        scrooge.Dialogue.Add("I will find a way to destroy every last one of them, so I may live in peace! Honest, hard-working men like myself should never have to deal with creatures such as these.");
                        scrooge.Dialogue.Add("Yes, and then I will finally be-- Ah, that's right. Your reward. Of course.");
                        scrooge.Dialogue.Add("Normally I would never dream of giving any of my money away for something like this, but you have saved my life, I'm sure. Here you go, boy. I won't forget what you've done for me.");
                        scrooge.Dialogue.Add("Make sure not to spend it all at once.");


                        scrooge.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    scrooge.UpdateInteraction();
                    if (!scrooge.Talking)
                    {
                        scrooge.ClearDialogue();
                        scrooge.Dialogue.Add("Bah, ghosts! I'll kill them all!");
                        game.ChapterTwo.ChapterTwoBooleans["moneyReceived"] = true;
                        player.AddStoryItem("Ten Bucks", "ten American dollars", 1);

                        state = 0;
                        timer = 0;
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
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (scrooge != null && scrooge.Talking)
                        scrooge.DrawDialogue(s);

                    s.End();
                    break;
            }
        }
    }
}
