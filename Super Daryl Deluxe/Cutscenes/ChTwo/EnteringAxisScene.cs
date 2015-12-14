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
    class EnteringAxisScene : Cutscene
    {
        GameObject camFollow;
        NPC napoleon;
        float fadeAlpha = 1f;

        public EnteringAxisScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.Rec = new Rectangle(0, 0, 1, 1);
        }

        public override void SkipCutscene()
        {
            base.SkipCutscene();
            napoleon = game.CurrentChapter.NPCs["Napoleon"];

            napoleon.ClearDialogue();
            state = 0;
            timer = 0;
            player.PositionX = 1330;
            player.UpdatePosition();
            player.FacingRight = true;
            player.playerState = Player.PlayerState.standing;
            game.CurrentChapter.state = Chapter.GameState.Game;
            game.CurrentChapter.CutsceneState++;
            UnloadContent();
            game.CurrentChapter.CurrentMap.enteringMap = false;
            game.ChapterTwo.ChapterTwoBooleans["enteringAxisScenePlayed"] = true;
            game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 545);
            game.Camera.center = game.Camera.centerTarget;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;

        }
        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        player.Sprinting = false;
                        player.MoveFrame = 0;
                        napoleon = game.CurrentChapter.NPCs["Napoleon"];
                        napoleon.ClearDialogue();
                        napoleon.CompleteQuestSilently(game.ChapterTwo.joiningForcesPartTwo);
                        camFollow.PositionX = player.PositionX - 45;
                        camFollow.PositionY = 545;
                        game.Camera.centerTarget = new Vector2(camFollow.PositionX, 545);
                        game.Camera.center = game.Camera.centerTarget;
                    }

                    if (timer > 40)
                    {
                        if (fadeAlpha > 0)
                            fadeAlpha -= 1f / 90f;
                    }

                    camFollow.PositionX = player.PositionX + 100;
                    player.UpdatePosition();

                    player.CutsceneWalk(new Vector2(4, 0));
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (player.PositionX > 800)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        napoleon.Dialogue.Add("What ze blasted hell is zat?");
                        napoleon.Talking = true;
                    }

                    if(napoleon.Talking)
                        napoleon.UpdateInteraction();
                    camFollow.PositionX = player.PositionX + 100;

                    player.UpdatePosition();

                    if (player.PositionX < 1330)
                        player.CutsceneWalk(new Vector2(4, 0));
                    else
                        player.playerState = Player.PlayerState.relaxedStanding;

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!napoleon.Talking && player.playerState == Player.PlayerState.relaxedStanding)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();
                        napoleon.Dialogue.Add("I zink you should be getting out of zhere. Zis \"Time Lord\" is not around, and zat machine is giving me ze creeps.");
                        napoleon.Dialogue.Add("You have done well, ze enemy is crippled and the rest of us can prepare to take our classroom back. I am in your debt, ohoh.");
                        napoleon.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    napoleon.UpdateInteraction();

                    if (!napoleon.Talking)
                    {

                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        UnloadContent();
                        game.CurrentChapter.CurrentMap.enteringMap = false;
                        game.ChapterTwo.ChapterTwoBooleans["enteringAxisScenePlayed"] = true;
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
                    if (napoleon != null && napoleon.Talking)
                    {
                        napoleon.DrawDialogue(s);
                    }

                    if (fadeAlpha > 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * fadeAlpha);
                    s.End();
                    break;
            }
        }
    }
}
