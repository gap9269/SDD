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
    class TreeEntsAppear : Cutscene
    {
        NPC caesar;
        NPC genghis;
       
        int talkingState;
        int specialTimer;
        GameObject camFollow;
        float fadeAlpha = 1f;

        public TreeEntsAppear(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
            camFollow.PositionX = 4600;
        }

        public override void Play()
        {
            base.Play();

            if (game.Camera.center.Y > 470)
            {
                game.Camera.center.Y = 471;
            }

            Chapter.effectsManager.Update();

            foreach (Enemy e in game.CurrentChapter.CurrentMap.EnemiesInMap)
            {
                e.Respawning = false;
                e.Alpha = 1;
                e.enemyState = Enemy.EnemyState.standing;
                e.moveTimer = 100;
            }

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        timer = 1;
                        talkingState = 1;

                        player.playerState = Player.PlayerState.relaxedStanding;
                        caesar = game.CurrentChapter.NPCs["Julius"];
                        genghis = game.CurrentChapter.NPCs["Genghis"];
                        caesar.CurrentDialogueFace = "Helmet";

                        camFollow = new GameObject();
                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.PositionY = player.VitalRec.Center.Y;
                        camFollow.YScroll = true;

                        caesar.ClearDialogue();
                        genghis.ClearDialogue();

                        genghis.FacingRight = false;
                        player.FacingRight = false;

                        genghis.Dialogue.Add("You will be mine, Caesar! I will have you riddled with my arrows by the time the sun sets!");

                        caesar.Dialogue.Add("Yes, yes. Well I do say, I'm quite famished. Perhaps I'll step away now and have my gourmet chefs whip me up something nice.");
                        caesar.Dialogue.Add("Perhaps we can continue this tomorrow? Oh, that's right, I am most busy most of the day. Next week, then.");
                        caesar.Dialogue.Add("Goodbye, Genghis.");

                        genghis.Talking = true;
                    }
                    genghis.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!genghis.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        caesar.Talking = true;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    caesar.UpdateInteraction();

                    if (!caesar.Talking)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 2:

                    (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY += 3;

                    if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY >= 230)
                        (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY = 230;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);


                    if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).caesarPosY >= 230)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        caesar.Dialogue.Clear();
                        genghis.Dialogue.Clear();

                        genghis.Dialogue.Add("Caesar! Get back here and stand your ground like a man!");

                        genghis.Talking = true;

                        talkingState = 1;
                        specialTimer = 0;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);


                    if (genghis.Talking == false && talkingState != 2)
                    {
                        talkingState++;
                    }

                    if (talkingState == 1 || talkingState == 3)
                    {
                        genghis.UpdateInteraction();
                    }
                    else if (talkingState == 2)
                    {
                        specialTimer++;

                        if (specialTimer == 120)
                        {
                            talkingState++;
                            genghis.ClearDialogue();
                            genghis.Dialogue.Add("Caesar you pathetic, back-stabbing rat! Get out here!");
                            genghis.Talking = true;
                        }
                    }
                    else if (talkingState == 4)
                    {
                        state++;
                        timer = 0;
                        specialTimer = 0;
                    }
                    break;
                case 4:

                    if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY > -369 && game.CurrentChapter.CurrentMap.EnemiesInMap.Count < 3)
                    {
                        (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY -= 2;
                        Game1.camera.ShakeCamera(1, .5f);
                    }
                    else if (game.CurrentChapter.CurrentMap.EnemiesInMap.Count < 3)
                    {
                        specialTimer = 0;
                        TreeEnt en = new TreeEnt(new Vector2(), "Tree Ent", game, ref player, game.CurrentChapter.CurrentMap);
                        en.Position = new Vector2(-41, 480);

                        game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Tree Ent"]++;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en);

                        TreeEnt en2 = new TreeEnt(new Vector2(), "Tree Ent", game, ref player, game.CurrentChapter.CurrentMap);
                        en2.Position = new Vector2(-211, 480);

                        game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Tree Ent"]++;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en2);

                        TreeEnt en3 = new TreeEnt(new Vector2(), "Tree Ent", game, ref player, game.CurrentChapter.CurrentMap);
                        en3.Position = new Vector2(-354, 480);

                        game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Tree Ent"]++;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(en3);

                        en.UpdateRectangles();
                        en2.UpdateRectangles();
                        en3.UpdateRectangles();

                        en.Hostile = true;
                        en2.Hostile = true;
                        en3.Hostile = true;

                        Chapter.effectsManager.AddSmokePoof(en3.VitalRec, 2);
                        Chapter.effectsManager.AddSmokePoof(en2.VitalRec, 2);
                        Chapter.effectsManager.AddSmokePoof(en.VitalRec, 2);
                        
                    }
                    else
                    {
                        specialTimer++;

                        if (specialTimer > 60)
                        {
                            if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY < 0)
                            {
                                (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY += 2;
                                Game1.camera.ShakeCamera(1, .5f);

                                if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY > 0)
                                    (Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY += 0;
                            }

                            if ((Game1.schoolMaps.maps["The Great Wall"] as TheGreatWall).gatePosY == 0)
                            {
                                state++;
                                timer = 0;
                            }
                        }
                    }
                    if (camFollow.PositionX > 1250)
                        camFollow.PositionX -= 8;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;
                case 5:
                    if (firstFrameOfTheState)
                    {
                        genghis.ClearDialogue();
                        genghis.Dialogue.Add("Hrraauugh! Caesar!");
                        genghis.Dialogue.Add("Retreat, men! Retreat!");
                        genghis.Talking = true;
                    }

                    genghis.UpdateInteraction();
                    if (!genghis.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 6:
                    FadeOut(60);
                    break;
                case 7:

                    if (firstFrameOfTheState)
                    {
                        genghis.AddQuest(game.ChapterTwo.livingLumber);
                        genghis.MapName = "The Yurt of Khan";
                        genghis.RecX = 680;
                        genghis.RecY = 310;
                        genghis.PositionX = 680;
                        genghis.PositionY = 310;
                        game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] = true;
                    }
                    if (timer > 15)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 8:
                    FadeIn(60);
                    break;
                case 9:
                    state = 0;
                    timer = 0;
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    Chapter.effectsManager.foregroundFButtonRecs.Clear();
                    Chapter.effectsManager.fButtonRecs.Clear();
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
                case 7:
                case 8:
                case 9:
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
                        else if (genghis.Talking == true)
                        {
                            genghis.DrawDialogue(s);
                        }
                    }

                    if (state == 6)
                        DrawFade(s, 0);
                    else if (state == 7)
                        s.Draw(Game1.whiteFilter, game.CurrentChapter.CurrentMap.mapRec, Color.Black);
                    else if (state == 8)
                        DrawFade(s, 1);
                    s.End();
                    break;
            }
        }
    }
}
