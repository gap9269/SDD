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
    class DominiqueTransform : Cutscene
    {
        DrDominique dominique;


        public DominiqueTransform(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
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
                        dominique = game.SideQuestManager.nPCs["Dr. Dominique Jean Larrey"] as DrDominique;
                        dominique.CompleteQuestSilently(dominique.Quest);
                        dominique.ClearDialogue();
                        dominique.Dialogue.Add("Ooohoho, you have done it zen?");
                        dominique.Dialogue.Add("I suppose you did not see any strange side effects from ze serum? I must know if you did!");
                        dominique.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    dominique.UpdateInteraction();

                    if (dominique.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        dominique.ClearDialogue();
                        dominique.Dialogue.Add("Hmmm...it is just zat I have sustained a slight injury from a patient flailing around during a routine test earlier today, you see. Zis hacksaw was embedded quite deeply in ze flesh of my arm.");
                        dominique.Dialogue.Add("It would be rather unfortunate if ze serum you have been testing does not work as I intend. Zis army needs my surgical expertise, oohoho.");
                        dominique.Dialogue.Add("It would zeem that you have used all of the medicine I have given you, so it must have worked well, yes? No violent screaming or flesh dissolving?");
                        dominique.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    dominique.UpdateInteraction();

                    if (dominique.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 3:

                    if (timer > 180)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:

                    if (firstFrameOfTheState)
                    {
                        dominique.ClearDialogue();
                        dominique.Dialogue.Add("Very well zen!");
                        dominique.Dialogue.Add("I will waste no more time, as I believe zis wound is beginning to fester. I have saved one syringe of ze miracle serum. Zeez half-men monsters know how to make a cure, zat is for sure.");
                        dominique.Dialogue.Add("Here goes nothzing!");
                        dominique.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    dominique.UpdateInteraction();

                    if (dominique.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 5:

                    if (firstFrameOfTheState)
                    {
                        dominique.ClearDialogue();
                        dominique.Dialogue.Add(".......");
                        dominique.Dialogue.Add("EEEAAAGGHHH!");
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(dominique.Rec.Center.X - 90, dominique.Rec.Center.Y, 150, 150), 2);

                    }

                    if (timer == 12)
                        dominique.goblinified = true;

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer >= 100)
                    {
                        if (timer == 100)
                            dominique.Talking = true;

                        dominique.UpdateInteraction();

                        if (dominique.Talking == false)
                        {
                            dominique.ClearDialogue();
                            dominique.Dialogue.Add("Well... zis is most troublesome.");
                            state = 0;
                            timer = 0;
                            player.playerState = Player.PlayerState.standing;
                            game.CurrentChapter.state = Chapter.GameState.Game;
                            game.SideQuestManager.sideQuestScenes = SideQuestManager.SideQuestScenes.none;
                        }
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

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
                case 4:
                case 5:
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

                    if (dominique!= null && dominique.Talking)
                    {
                        dominique.DrawDialogue(s);
                    }
                    s.End();
                    break;
            }
        }
    }
}
