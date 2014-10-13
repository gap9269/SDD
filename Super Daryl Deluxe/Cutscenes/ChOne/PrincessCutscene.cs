using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class PrincessCutscene : Cutscene
    {
        Texture2D flashback;
        GameObject camFollow;
        NPC princess;

        //--Takes in a background and all necessary objects
        public PrincessCutscene(Game1 g, Camera cam, Player player, Texture2D t)
            : base(g, cam, player)
        {
            flashback = t;
            camFollow = new GameObject();
            camFollow.PositionY = -400;
        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {
                case 0:

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (player.VitalRec.Center.X >= 1400 && player.VitalRec.Center.X <= 1410)
                    {
                        camera.ShakeCamera(10, 10);
                    }
                     
                    if (player.VitalRec.Center.X >= 1500)
                    {
                        player.playerState = Player.PlayerState.relaxedStanding;
                        timer = 0;
                        state++;
                    }
                    else
                    {
                        player.CutsceneWalk(new Vector2(player.MoveSpeed, 0));
                    }
                    break;
                case 1:

                    camFollow.PositionX = player.VitalRec.Center.X;

                    if (timer < 110)
                    {
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        camFollow.PositionY = player.VitalRec.Center.Y;
                    }

                    if (timer >= 80 && timer <= 115)
                    {
                        player.CutsceneWalk(new Vector2(player.MoveSpeed, 0));
                    }

                    if(timer == 115)
                        camera.ShakeCamera(15, 25);

                    if (timer > 115)
                    {
                        camera.Update(player, game, game.CurrentChapter.CurrentMap);
                        player.VelocityY += GameConstants.GRAVITY;
                        player.RecY += (int)player.VelocityY;
                        //player.UpdatePosition();
                        //player.playerState = Player.PlayerState.jumping;
                        princess = game.CurrentChapter.NPCs["Daddy's Little Princess"];
                    }

                    if (timer == 275)
                    {
                        timer = 0; 
                        state++;
                    }
                    break;
                case 2:
                    FadeOut(120);
                    break;

                //--Flashback
                case 3:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 200)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    FadeOut(120);
                    break;
                //Daryl getting up cutscene
                case 5:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                        player.PositionX = 1300;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        player.PositionY = 630 - player.VitalRecHeight - 135;
                        player.UpdatePosition();
                        player.Velocity = Vector2.Zero;
                        princess.FacingRight = false;

                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);
                        game.Camera.centerTarget += new Vector2(0, player.PositionY + (player.Rec.Height / 2));

                        game.Camera.center = game.Camera.centerTarget;
                    }
                    break;
                //Back to game cutscene
                case 6:
                    if (firstFrameOfTheState)
                    {
                        game.CurrentQuests.Remove(game.ChapterOne.ReturningKeys);
                        princess.Dialogue.Clear();
                        princess.Dialogue.Add("Oh my god! Who are you and what are you doing here?!");
                        princess.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    princess.UpdateInteraction();
                    if (princess.Talking == false)
                    {
                        state++; 
                        timer = 0;
                    }
                    break;
                case 7:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 8:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (firstFrameOfTheState)
                    {
                        princess.Dialogue.Clear();
                        princess.Dialogue.Add("Say something!");
                        princess.Talking = true;
                    }
                    princess.UpdateInteraction();
                    if (princess.Talking == false)
                    {
                        state++; 
                        timer = 0;
                    }
                    break;
                case 9:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 10:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (firstFrameOfTheState)
                    {
                        princess.Dialogue.Clear();
                        princess.Dialogue.Add("Oh my god you're so weird! How did you find me anyway? Daddy said that no one knows about \nthis locker room. He made it for me special so I'd be safe.");

                        princess.Dialogue.Add("Wait...are you here to hurt me? You better not, or Daddy will get really mad and hurt you back.");

                        princess.Dialogue.Add("...");

                        princess.Dialogue.Add("...No, you look too stupid to be someone that would know how to find me and hurt me. So \nwhy were you crawling around in the vents, stupid boy? They only go to that smelly Janitor's \nCloset from here. And that smelly Janitor lives there.");

                        princess.Dialogue.Add("Well whatever you were doing up there, Daddy is going to be really mad at you when I tell \nhim that you found me. I'm his Little Princess you know. He'll do whatever I want and he makes sure I'm safe. \nHe says the world is dangerous, especially around here in this school. That's why he made me this \nsecret locker room.");

                        princess.Dialogue.Add("It's really nice here; I just wish I had something to do. It gets boring in here all day long, you know!");
                        princess.Talking = true;
                    }
                    princess.UpdateInteraction();
                    if (princess.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 11:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (firstFrameOfTheState)
                    {
                        princess.Dialogue.Clear();
                        princess.AddQuest(game.ChapterOne.DaddysLittlePrincess);
                        princess.Talking = true;
                    }
                    princess.UpdateInteraction();
                    if (princess.Talking == false)
                    {
                        state++;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
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
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentMap.DrawMapOverlay(s);
                    s.End();

                    break;
                case 2:
                     s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentMap.DrawMapOverlay(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFadeWhite(s, 0);
                    s.End();

                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(flashback, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
                    s.End();
                    break;
                case 4:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(flashback, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    DrawFadeWhite(s, 0);
                    s.End();
                    break;
                case 5:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.DrawString(Game1.font, "Daryl gets up off floor, princess seees him and screams", new Vector2(500, 300), Color.White);
                    s.End();
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (princess.Talking)
                        princess.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
