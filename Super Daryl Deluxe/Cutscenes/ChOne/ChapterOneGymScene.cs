using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class ChapterOneGymScene : Cutscene
    {
        Texture2D temp;
        NPC coach;

        //--Takes in a background and all necessary objects
        public ChapterOneGymScene(Game1 g, Camera cam, Player player, Texture2D t)
            : base(g, cam, player)
        {
            temp = t;
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        dialogue.Clear();

                        dialogue.Add("HEY YOU");
                        dialogue.Add("YEAH YOU, NEW KID.");

                        DialogueState = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer == 180)
                        DialogueState = 1;

                    if (timer >= 240)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        coach = new NPC(game.NPCSprites["Alan"], new List<String>(), new Rectangle(0, 0, 0, 0),
                    player, game.Font, game, "Gym Lobby", "Alan", true);

                        coach.Dialogue.Add("The hell is your name again? Daryl, was it?");
                        coach.Dialogue.Add("Yes, that's right. Daryl Whitelaw. That's it.");
                        coach.Dialogue.Add("Well Daryl, where the hell are your gym clothes? You can't expect to participate dressed like that.");
                        coach.Dialogue.Add("Got anything to say?");

                        coach.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    coach.UpdateInteraction();

                    if (coach.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer >= 200)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        coach.Dialogue.Clear();
                        coach.Dialogue.Add("...  ...  ...");
                        coach.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    coach.DialogueState = 0;

                    if (timer >= 120)
                    {
                        coach.Talking = false;
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:
                    if (firstFrameOfTheState)
                    {
                        dialogue.Clear();
                        dialogue.Add("GET OUT!");
                        DialogueState = 0;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer >= 240)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.PositionX = 1000;
                        player.RecX = 1000;
                        player.PositionY = game.CurrentChapter.CurrentMap.Platforms[0].Rec.Y - player.VitalRecHeight - 135;
                        player.RecY = (int)player.PositionY;

                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                        game.Camera.center = game.Camera.centerTarget;

                        player.UpdatePosition();
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
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    if(timer > 60)
                        DrawDialogue(s);
                    s.End();
                    break;
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(temp, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    if(timer > 1)
                        coach.DrawDialogue(s);
                    s.End();

                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(temp, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(temp, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    coach.DrawDialogue(s);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);

                    if (timer > 1 && timer < 180)
                        DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
