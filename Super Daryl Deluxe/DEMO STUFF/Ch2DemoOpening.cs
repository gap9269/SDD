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
    class Ch2DemoOpening : Cutscene
    {
        // ATTRIBUTES \\
        NPC napoleon;

        //--Takes in a background and all necessary objects
        public Ch2DemoOpening(Game1 g, Camera cam, Player player)
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

            Chapter.effectsManager.Update();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        napoleon = game.CurrentChapter.NPCs["Napoleon"];
                        napoleon.RemoveQuest(game.ChapterTwoDemo.fortRaid);
                        napoleon.Dialogue.Clear();

                        napoleon.Dialogue.Add("Welcome to ze Zuper Daryl Deluxe demo! Make zure you have played ze Prologue first so you know how to play.");
                        napoleon.Dialogue.Add("Zis demo will show you what ze game is all about...you know, after Daryl has acquired more of his skills.");
                        napoleon.Dialogue.Add("Zhere are a few hints I will give that the Prologue does not teach. I call zem, \"Napoleon's Greatest Hints\".");
                        napoleon.Dialogue.Add("First: Ze flying pink locker can be used as your regular locker. Just hit it out of ze sky to use it. Make sure you buy more skills, you have enough textbooks for all of zem! Once we invade ze fort I will tie ze locker to our horse. You can use it by pressing 'F' at ze horse once that happens.");
                        napoleon.Dialogue.Add("Second: I have given you new equipment. Open your inventory to equip zem. You will need it against the mighty foes you are about to face.");
                        napoleon.Dialogue.Add("Third: Make sure you follow ze quest objective at the top right of your screen. When in doubt, destroy everything! Including buildings!");
                        napoleon.Dialogue.Add("Lastly, ze game is all about creating your own unique combat system. Mix and match different skills until you find ze combination you like. You can have up to four skills equipped, so use zem all!");
                        napoleon.Dialogue.Add("Now, let's get started.");

                        napoleon.Talking = true;
                    }

                    napoleon.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();
                    game.CurrentChapter.CurrentMap.Update();

                    if (napoleon.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        napoleon.Dialogue.Clear();
                        napoleon.AddQuest(game.ChapterTwoDemo.fortRaid);
                        napoleon.Talking = true;
                    }
                    napoleon.Choice = 0;
                    napoleon.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();
                    game.CurrentChapter.CurrentMap.Update();
                    if (napoleon.Talking == false)
                    {
                        timer = 0;
                        Chapter.effectsManager.AddFoundItem("Caesar's Toga", Game1.equipmentTextures["Toga"]);
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.playerState = Player.PlayerState.standing;
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
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawLivingLocker(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
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