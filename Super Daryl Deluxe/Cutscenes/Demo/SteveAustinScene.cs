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
    class SteveAustinScene : Cutscene
    {
        // ATTRIBUTES \\
        float nameAlpha = 1f;
        NPC associateOne;

        //--Takes in a background and all necessary objects
        public SteveAustinScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            List<String> assocOneDialogue = new List<string>();
            assocOneDialogue.Add("Oh no! It's Stone Cold Steve Austin! And he looks angry!");
            assocOneDialogue.Add("He's the only thing stopping you from completing this tutorial, player! Remember \nto duck his punches and go show him what you've learned!");

            associateOne = new NPC(game.NPCSprites["Paul"], assocOneDialogue, new Rectangle(-1000, 0, 0, 0),
                    player, game.Font, game, "Tutorial Map Fourteen", "Demo Danny", false);
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    player.CutsceneRun(new Vector2(player.MoveSpeed, 0));

                    if(player.PositionX > 500)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        associateOne.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    player.CutsceneStand();
                    associateOne.UpdateInteraction();

                    if (associateOne.Talking == false)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 2:
                    if (firstFrameOfTheState)
                    {
                        game.CurrentChapter.CurrentBoss = TutorialMapFourteen.steveAustin;
                        game.CurrentChapter.BossFight = true;

                        timer = 0;
                        state++;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    break;
                case 3:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);


                    if(timer > 60)
                        game.CurrentChapter.CurrentBoss.HealthBarGrow();

                    player.CutsceneStand();


                    if (timer > 200)
                    {
                        if (nameAlpha > 0)
                            nameAlpha -= .01f;
                        else
                            game.CurrentChapter.CurrentBoss.DrawHUDName = true;
                    }
                    if (timer > 350)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
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
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    TutorialMapFourteen.steveAustin.Draw(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    TutorialMapFourteen.steveAustin.Draw(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    associateOne.DrawDialogue(s);
                    s.End();
                    break;

                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    TutorialMapFourteen.steveAustin.Draw(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    s.Draw(game.EnemySpriteSheets["AustinName"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * nameAlpha);

                    if (timer > 60)
                        game.CurrentChapter.CurrentBoss.DrawHud(s);

                    s.End();
                    break;

            }
        }
    }
}