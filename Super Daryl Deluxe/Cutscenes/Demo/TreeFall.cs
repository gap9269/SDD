using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class TreeFall : Cutscene
    {
        NPC associateOne;

        public TreeFall(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            List<String> assocOneDialogue = new List<string>();
            assocOneDialogue.Add("Player! Look at the beautiful tree! Only a master game developer could ever \nachieve such a brilliant atmosphere.");
            assocOneDialogue.Add("Wait! Oh no! The tree is falling! Quick player, be careful!");

            associateOne = new NPC(game.NPCSprites["Paul"], assocOneDialogue, new Rectangle(-1000, 0, 0, 0),
                    player, game.Font, game, "Tutorial Map One", "Demo Danny", false);
        }

        public override void Play()
        {
            base.Play();


            last = current;
            current = Keyboard.GetState();

            switch (state)
            {
                //Player walks up to the tree
                case 0:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (player.playerState != Player.PlayerState.jumping)
                        player.CutsceneRun(new Vector2(player.MoveSpeed, 0));
                    else
                    {
                        player.VelocityX = 0;
                        player.Update();
                        player.CanJump = false;
                    }

                    if (player.PositionX >= 1950 && player.playerState != Player.PlayerState.jumping)
                    {
                        state++;
                        timer = 0;
                        player.CanJump = true;
                    }
                    break;

                //Associate warns you of tree falling
                case 1:
                    if (firstFrameOfTheState)
                    {
                        associateOne.Talking = true;
                    }
                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    associateOne.UpdateInteraction();

                    if (associateOne.Talking == false)
                    {
                        state++;
                        timer = 0;
                        game.MapBooleans.tutorialMapBooleans["TreeFell"] = true;
                        camera.ShakeCamera(30, 10);
                    }
                    break;

                //Tree falls
                case 2:

                    player.CutsceneStand();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if(timer > 60)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 3:
                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                //Draw the player running to the tree
                case 0:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;

                //Draw the associate talking
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);

                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    associateOne.DrawDialogue(s);
                    s.End();
                    break;

                //tree falls
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    break;

                //Fade out
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;
            }
        }
    }
}
