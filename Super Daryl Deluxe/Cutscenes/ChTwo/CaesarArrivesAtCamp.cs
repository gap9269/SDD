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
    class CaesarArrivesAtCamp : Cutscene
    {
        NPC caesar;
        NPC napoleon;
       
        int talkingState;
        int specialTimer;

        public CaesarArrivesAtCamp(Game1 g, Camera cam, Player p)
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
                        player.playerState = Player.PlayerState.relaxedStanding;
                        caesar = game.CurrentChapter.NPCs["Julius"];
                        napoleon = game.CurrentChapter.NPCs["Napoleon"];
                        caesar.CurrentDialogueFace = "Helmet";

                        caesar.ClearDialogue();
                        napoleon.ClearDialogue();

                        napoleon.Dialogue.Add("Oh, thank the lord, zhere you are!");
                        napoleon.Dialogue.Add("Julius has finally arrived, and he won't stop as--");

                        napoleon.Talking = true;
                    }
                        napoleon.UpdateInteraction();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!napoleon.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        caesar.Dialogue.Add("Ahem, yes, hello!");
                        caesar.Dialogue.Add("I was assured Cleo would be by my side upon the moment of my arrival to your fly-infested camp, and instead I am left pining for my beloved!");
                        caesar.Dialogue.Add("Each second is yet another death when the heart lives only for one who is absent!");
                        caesar.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    caesar.UpdateInteraction();

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();

                        napoleon.Dialogue.Add("He is going to drive me insane, you know.");

                        napoleon.Talking = true;
                    }
                        napoleon.UpdateInteraction();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!napoleon.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("I have traveled across lands scorched by the heat of a thousand desert suns, and battled countless barbarian hordes to see my true love!");
                        caesar.Dialogue.Add("Have I not earned that which was promised to me? Do I not deserve the warm embrace of my beautiful Cleo, now that I am a part of this ridiculous club?");
                        caesar.Dialogue.Add("Oh, my heart and its woes! Must I search through this entire disease-ridden shanty town? Such a place is not fit for one like her!");
                        caesar.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    caesar.UpdateInteraction();

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 4:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();

                        napoleon.Dialogue.Add("Mon Dieu Caesar, get a life.");

                        napoleon.Talking = true;
                        napoleon.FacingRight = false;
                    }
                        napoleon.UpdateInteraction();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!napoleon.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 5:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("Gasp! Why-- how dare you!");
                        caesar.Dialogue.Add("Is this how you treat new members of your alliance? How is it that you have kept the membership of Empress Cleopatra, I do wonder?");
                        caesar.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (caesar.Talking == true)
                        caesar.UpdateInteraction();
                    else if (napoleon.Talking)
                        napoleon.UpdateInteraction();

                    if (napoleon.Talking == false && caesar.Talking == false)
                    {
                        napoleon.Dialogue.Clear();
                        caesar.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            napoleon.Talking = true;
                            napoleon.Dialogue.Add("For starters, she was no where near you and your sappy monologues.");
                        }

                        if (talkingState == 2)
                        {
                            caesar.Dialogue.Add("Oh please! The only way I can imagine you could have kept her from coming to find me herself was to have chained her to a post somewhere!");
                            caesar.Dialogue.Add("However, I doubt that a tiny man such as yourself could have ever done such a thing to a powerful woman like Cleopatra. I am beginning to think she was never here in the first place.");
                            caesar.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            napoleon.Talking = true;
                            napoleon.Dialogue.Add("Don't be ridiculous. Our boy here was just about to go fetch her from her luxurious tent. I do believe she is enjoying a nap.");
                        }
                        if (talkingState == 4)
                        {
                            if (player.VitalRec.Center.X < napoleon.Rec.Center.X)
                                napoleon.FacingRight = false;
                            else
                                napoleon.FacingRight = true;

                            state++;
                            timer = 0;
                            specialTimer = 0;
                        }
                    }

                    break;
                case 6:
                    if (firstFrameOfTheState)
                    {
                        napoleon.RemoveQuest(game.ChapterTwo.deliveringSupplies);
                        napoleon.AddQuest(game.ChapterTwo.joiningForcesPartTwo);
                        napoleon.Talking = true;
                    }
                    napoleon.UpdateInteraction();

                    if (!napoleon.Talking)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("I have not detected even a hint of her perfume! I'm inclined to believe I'm being swindled.");
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        Chapter.effectsManager.foregroundFButtonRecs.Clear();
                        Chapter.effectsManager.fButtonRecs.Clear();
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
                case 4:
                case 5:
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

                    if (caesar != null)
                    {
                        if (caesar.Talking == true)
                        {
                            caesar.DrawDialogue(s);
                        }
                        else if (napoleon.Talking == true)
                        {
                            napoleon.DrawDialogue(s);
                        }
                    }
                    s.End();
                    break;
            }
        }
    }
}
