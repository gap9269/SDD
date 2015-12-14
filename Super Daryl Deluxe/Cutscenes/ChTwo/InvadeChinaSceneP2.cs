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
    class InvadeChinaSceneP2 : Cutscene
    {


        TrojanHorse trojanHorse;
        NPC caesar;
        public InvadeChinaSceneP2(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }
        public override void LoadContent()
        {
            base.LoadContent();
            trojanHorse = new TrojanHorse(2405, game.CurrentChapter.CurrentMap.MapY + 641, player, game.CurrentChapter.CurrentMap);
            trojanHorse.LoadContent(content, false, true);
        }
        public override void Play()
        {
            base.Play();
            Chapter.effectsManager.Update();
            if (game.Camera.center.Y > 470)
            {
                game.Camera.center.Y = 471;
            }

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        caesar = game.CurrentChapter.NPCs["Julius"];
                        caesar.CurrentDialogueFace = "Helmet";
                        game.CurrentChapter.CurrentMap.EnemiesInMap.Clear();
                        game.CurrentChapter.CurrentMap.ResetEnemyNamesAndNumberInMap();

                        Chapter.effectsManager.AddInGameDialogue("What a beautiful day to own a giant wall!", "Julius Caesar", "Helmet", 300);
                      
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 120)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    FadeIn(120);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    break;
                case 2:

                    if (trojanHorse.PositionX > 1185)
                    {
                        trojanHorse.Move(-4);
                        trojanHorse.Update(game.CurrentChapter.CurrentMap.MapWidth);
                    }
                    else
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        caesar.ClearDialogue();
                        caesar.Dialogue.Add("Oh? What this this? A gift?");
                        caesar.Dialogue.Add("Why, it is! It's a gift! How spectacular! It must be from Cleopatra, there is no other explanation!");
                        caesar.Dialogue.Add("I am glad that she has finally accepted my love and has responded in kind.");
                        caesar.Dialogue.Add("Guards! Open the gates! Let my lover's gift through so I may look upon it closely and admire it's beautiful craftsmanship!");
                        caesar.Talking = true;
                    }

                    caesar.UpdateInteraction();

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 4:
                    if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY > -369)
                    {
                        (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY -= 2;
                        Game1.camera.ShakeCamera(1, .5f);
                    }
                        trojanHorse.Move(-3);
                        trojanHorse.Update(game.CurrentChapter.CurrentMap.MapWidth);
                    FadeOut(150);
                    break;
                case 5:
                    game.CurrentChapter.CutsceneState++;
                    (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY = 0;
                    game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "The Great Wall"), BehindTheGreatWall.toTheGreatWall);
                    game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
                    UnloadContent();
                    (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY = 0;
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

                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);


                    if(trojanHorse != null)
                        trojanHorse.DrawHorse(s);

                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    Chapter.effectsManager.DrawDialogue(s);

                    if (caesar!= null && caesar.Talking)
                        caesar.DrawDialogue(s);

                    if (state == 1)
                        DrawFade(s, 1);
                    if (state == 4)
                        DrawFade(s, 0);
                    if (state == 5 || state == 0)
                        s.Draw(Game1.whiteFilter, game.CurrentChapter.CurrentMap.mapRec, Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
