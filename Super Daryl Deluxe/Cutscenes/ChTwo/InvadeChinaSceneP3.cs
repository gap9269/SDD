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
    class InvadeChinaSceneP3 : Cutscene
    {
        NPC caesar;
        TrojanHorse trojanHorse;
        int specialTimer;
        GameObject camfollow;
        public InvadeChinaSceneP3(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camfollow = new GameObject();

            camfollow.PositionX = 780;
            camfollow.PositionY = player.VitalRec.Center.Y;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            trojanHorse = new TrojanHorse(2252, -120, player, game.CurrentChapter.CurrentMap);
            trojanHorse.LoadContent(content, false, true);
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
                        LoadContent();
                        player.PositionX = 638;
                        player.UpdatePosition();

                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);
                        game.Camera.center = game.Camera.centerTarget;

                        player.FacingRight = false;
                        player.playerState = Player.PlayerState.relaxedStanding;

                        caesar = game.CurrentChapter.NPCs["Julius"];
                        caesar.CurrentDialogueFace = "Helmet";

                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("This must mean she has finally accepted my dinner invitation. How like her to be so extravagant.");
                        caesar.Dialogue.Add("What beautiful worksmanship this is! Only someone with her infinite beauty and wisdom could create such a marvelous piece.");
                        caesar.Dialogue.Add("I do wonder how I should respond...");
                    }
                    FadeIn(120);
                    camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);

                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        caesar.Talking = true;
                    }
                    camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);
                    caesar.UpdateInteraction();

                    if (trojanHorse.PositionX > 700)
                    {
                        trojanHorse.Move(-4);
                        trojanHorse.Update(game.CurrentChapter.CurrentMap.MapWidth);

                    }

                    else if (!caesar.Talking && specialTimer <= 0)
                    {
                        specialTimer = 60;
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(player.VitalRec.Center.X - 100, player.VitalRec.Center.Y - 70, 200, 200), 2);
                    }

                    specialTimer--;

                    if (specialTimer == 3)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("Gasp!");
                        caesar.Dialogue.Add("What is this?! A ruse?");
                        caesar.Dialogue.Add("You look a lot like that nephew of his. Kublai, is it? How did you manage to sneak inside the gift that Cleopatra sent me? How despicable!");
                        caesar.Dialogue.Add("You must be wanting to settle this fight between Khan and I as well, am I correct?");
                        caesar.Talking = true;
                    }
                    camera.Update(camfollow, game, game.CurrentChapter.CurrentMap);

                    caesar.UpdateInteraction();

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.AddQuest(game.ChapterTwo.foreignDebt);
                        caesar.Talking = true;
                    }
                        caesar.UpdateInteraction();
                    if (!caesar.Talking)                    
                    {
                        caesar.Dialogue.Add("I am coming, Cleo!");
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        Chapter.effectsManager.foregroundFButtonRecs.Clear();
                        Chapter.effectsManager.fButtonRecs.Clear();
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

                    if (trojanHorse != null)
                        trojanHorse.DrawHorse(s);

                    game.CurrentChapter.DrawNPC(s);

                    if(state > 1 || specialTimer > 0 && specialTimer < 45)
                        player.Draw(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (caesar != null && caesar.Talking)
                        caesar.DrawDialogue(s);

                    if (state == 0)
                        DrawFade(s, 1);

                    s.End();
                    break;
            }
        }
    }
}
