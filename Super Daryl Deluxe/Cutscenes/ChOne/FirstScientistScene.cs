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
    class FirstScientistScene : Cutscene
    {

        Scientist sci;
        NPC scientist;

        public FirstScientistScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {

            
        }

        public override void Play()
        {
            base.Play();

            switch (state)
            {
                case 0:

                    if (firstFrameOfTheState)
                    {
                        sci = new Scientist(new Vector2(1000, 400), "Scientist", game, ref player, game.CurrentChapter.CurrentMap);
                        
                        sci.FacingRight = true;
                        sci.Alpha = 1f;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(sci);
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(new Scientist(new Vector2(1800, 400), "Scientist", game, ref player, game.CurrentChapter.CurrentMap));
                        scientist = game.CurrentChapter.NPCs["ScientistOne"];
                    }

                    player.CutsceneWalk(new Vector2(player.MoveSpeed, 0));
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (player.PositionX > 300)
                    {
                        state++;
                        timer = 0;
                        sci.FacingRight = false;
                    }

                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        scientist.Dialogue.Add("Wha?!");
                        scientist.Dialogue.Add("Who are you? And what are you doing here? You aren't supposed to be here, kid!");
                        scientist.Dialogue.Add("The boss says that I have to eliminate anyone who isn't supposed to be here. Gosh...I'm not so \ngood at eliminating things, but you look like a wuss. Prepare yourself, kid!");
                        scientist.Talking = true;

                    }

                    scientist.UpdateInteraction();

                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (scientist.Talking == false)
                    {
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        Chapter.effectsManager.AddInGameDialogue("AHHHHHHHHHHH!", "Alan", "Normal", 80);
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
                    if(timer > 1)
                        sci.Draw(s);
                    player.Draw(s);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    sci.Draw(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    scientist.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
