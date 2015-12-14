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
    class ScroogeRoomTwo : Cutscene
    {
        NPC scrooge;
        GameObject camFollow;
        Marley marley;

        public ScroogeRoomTwo(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
           
        }

        public override void Play()
        {
            base.Play();
            if (marley != null)
                marley.Update();
            if (scrooge != null)
                scrooge.Update();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();
            switch (state)
            {

                case 0:
                    if (firstFrameOfTheState)
                    {
                        camFollow.PositionX = player.VitalRec.Center.X;
                        marley = game.CurrentChapter.NPCs["Marley"] as Marley;
                        scrooge = game.CurrentChapter.NPCs["Ebenezer Scrooge"];
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    (game.CurrentChapter.CurrentMap as UnusedBedroom).christmasPresentAlpha -= .01f;
                    if (camFollow.PositionX > 1000)
                        camFollow.PositionX -= 10;
                    else if ((game.CurrentChapter.CurrentMap as UnusedBedroom).christmasPresentAlpha <= 0)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        scrooge.ClearDialogue();
                        scrooge.Dialogue.Add("Oh! The terror is over! Again!");
                        scrooge.Talking = true;
                    }

                    scrooge.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!scrooge.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        marley.ClearDialogue();
                        marley.Dialogue.Add("ARRRRRRRGGGGGGGHHHHH!!");
                        marley.Dialogue.Add("WHY HAVE YOU RETURNED? THIS IS NOT WHERE YOU BELONG!");
                        marley.Dialogue.Add("YOU ARE DISRUPTING A CAREFUL PROCESS. IF YOU FOLLOW AGAIN YOU WILL KNOW TRUE HORROR!");
                        marley.Talking = true;
                    }

                    marley.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!marley.Talking)
                    {
                        state++;
                        timer = 0;

                        marley.Teleport();
                        scrooge.MapName = "Haunted Bedroom";
                        scrooge.RecX = 987 + 516;
                        scrooge.RecY = 220;
                        scrooge.PositionX = scrooge.RecX;
                        scrooge.PositionY = scrooge.RecY;
                        scrooge.UpdateRecAndPosition();

                        Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(162, 307, 275, 275), 2);
                    }
                    break;
                case 3:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!marley.teleporting)
                    {
                        scrooge.ClearDialogue();
                        game.ChapterTwo.ChapterTwoBooleans["bedroomTwoCleared"] = true;
                        marley.MapName = "Haunted Bedroom";
                        marley.RecX = 1097;
                        marley.RecY = 96;
                        marley.PositionX = marley.RecX;
                        marley.PositionY = marley.RecY;
                        marley.FacingRight = true;
                        state = 0;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
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
                    game.CurrentChapter.DrawNPC(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (scrooge != null && scrooge.Talking)
                        scrooge.DrawDialogue(s);

                    if (marley != null && marley.Talking)
                        marley.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
