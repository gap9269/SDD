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
    class BombArrives : Cutscene
    {
        NPC napoleon, julius, cleo, genghis;
        int talkingState;

        int cutsceneTimer = 5, cutseneFrame;

        Dictionary<String, Texture2D> cutscene;
        public BombArrives(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            cutscene = ContentLoader.LoadContent(content, @"Maps\History\Outskirts\BombCutscene");
        }

        public override void Play()
        {
            base.Play();

            if (state > 0 && game.Camera.center.Y > 226)
            {
                game.Camera.center.Y = 225;
            }

            switch (state)
            {

                case 0:
                    FadeOut(120);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneUpdate();

                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        player.PositionX = 1100;
                        player.PositionY = game.CurrentChapter.CurrentMap.mapRec.Y + 891;
                        player.FacingRight = true;
                        player.UpdatePosition();
                        player.playerState = Player.PlayerState.relaxedStanding;

                        julius = game.ChapterTwo.NPCs["Julius"];
                        genghis = game.ChapterTwo.NPCs["Genghis"];
                        cleo = game.ChapterTwo.NPCs["Cleopatra"];

                        julius.FacingRight = false;
                        genghis.FacingRight = true;
                        cleo.FacingRight = false;

                        napoleon = game.ChapterTwo.NPCs["Napoleon"];
                        napoleon.PositionX = 1513;
                        napoleon.FacingRight = false;
                        napoleon.UpdateRecAndPosition();
                        napoleon.RemoveQuest(game.ChapterTwo.fortRaid);
                        napoleon.ClearDialogue();

                        game.ChapterTwo.ChapterTwoBooleans["bombArriveScenePlayed"] = true;
                        game.CurrentChapter.CurrentMap.ZoomLevel = .75f;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 200)
                    {
                        LoadContent();
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    FadeIn(180);
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        napoleon.Dialogue.Add("Everyzing is packed and ready to go. Let us g--");
                        napoleon.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (julius.Talking == true)
                        julius.UpdateInteraction();
                    else if (napoleon.Talking)
                        napoleon.UpdateInteraction();
                    else if (cleo.Talking)
                        cleo.UpdateInteraction();
                    else if (genghis.Talking)
                        genghis.UpdateInteraction();

                    if (napoleon.Talking == false && julius.Talking == false && cleo.Talking == false && genghis.Talking == false)
                    {
                        napoleon.Dialogue.Clear();
                        julius.Dialogue.Clear();
                        genghis.Dialogue.Clear();
                        cleo.Dialogue.Clear();

                        talkingState++;

                        if (talkingState == 1)
                        {
                            cleo.Talking = true;
                            cleo.Dialogue.Add("Aren't we going to need a bomb of some sort?");
                        }

                        if (talkingState == 2)
                        {
                            napoleon.Dialogue.Add("..Wh-- huh?");
                            napoleon.Talking = true;
                        }
                        if (talkingState == 3)
                        {
                            cleo.Talking = true;
                            cleo.Dialogue.Add("If we're going to be attacking a large enemy fort of unknown size, with an unknown amount of demons inside, led by an all-powerful warlock that wants to rule over the entire History Room, shouldn't we bring something capable of destroying the whole place?");
                        }
                        if (talkingState == 4)
                        {
                            napoleon.FacingRight = true;
                            julius.Talking = true;
                            julius.Dialogue.Add("The beautiful lady is right, little man. How do you expect to win this fight with only a handful of soldiers, a barbarian with a starved horse, and a child with a shockingly perfect sense of fashion?");
                            julius.Dialogue.Add("The most dangerous-looking thing you have here is the wheeled sculpture that my sweet Cleopatra created for me.");
                        }
                        if (talkingState == 5)
                        {
                            cleo.Talking = true;
                            cleo.Dialogue.Add("I did no such thing.");
                        }
                        if (talkingState == 6)
                        {
                            genghis.Talking = true;
                            genghis.Dialogue.Add("Ho ho, this is true. My men worked long hours to create this masterpiece you see here. A woman's touch had no part in it.");
                        }
                        if (talkingState == 7)
                        {
                            julius.Talking = true;
                            julius.Dialogue.Add("That is absured! A savage could never construct such a beautiful gift!");
                        }
                        if (talkingState == 8)
                        {
                            napoleon.Dialogue.Add("Forget who created it, zat is not important! We must focus on ze task at hand, not your petty squabbles!");
                            napoleon.Talking = true;
                        }
                        if (talkingState == 9)
                        {
                            julius.Talking = true;
                            julius.Dialogue.Add("Mmmm...I am inclined to agree, I suppose. Regardless, this does not change the fact that you are masterfully ill-equipped for an all-out assault on our enemy's base.");
                        }
                        if (talkingState == 10)
                        {
                            napoleon.Dialogue.Add("I...am willing to concede to that. However, what can we do about it now? I do not suppose any of you are capable of constructing a lethal weapon out of thin air.");
                            napoleon.Talking = true;
                        }
                        if (talkingState == 11)
                        {
                            state++;
                            timer = 0;
                        }
                    }
                    break;
                case 4:

                    cutsceneTimer--;

                    if (cutsceneTimer <= 0)
                    {
                        cutseneFrame++;
                        cutsceneTimer = 5;

                        if (cutseneFrame == 4)
                            cutsceneTimer = 10;
                        if (cutsceneTimer < 4)
                            cutsceneTimer = 6;

                        if (cutseneFrame == 32)
                        {
                            state++;
                            timer = 0;
                        }
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (cutseneFrame == 2)
                    {
                        napoleon.FacingRight = false;
                    }
                    break;
                case 5:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();
                        napoleon.Dialogue.Add("...");
                        napoleon.Talking = true;
                    }

                    napoleon.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!napoleon.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 6:
                    if (firstFrameOfTheState)
                    {
                        julius.ClearDialogue();
                        julius.Dialogue.Add("Obviously god is on our side!");
                        julius.Talking = true;
                    }

                    julius.UpdateInteraction();

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!julius.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 7:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();
                        napoleon.Dialogue.Add("I think we have found our bomb.");
                        napoleon.Dialogue.Add("Let's get moving.");
                        napoleon.Talking = true;
                    }

                    napoleon.UpdateInteraction();

                    if (!napoleon.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 8:
                    FadeOut(180);
                    break;
                case 9:
                    julius.MapName = "Stone Fort Gate";
                    julius.PositionX = 2350;
                    julius.PositionY = 370;
                    julius.FacingRight = false;
                    julius.UpdateRecAndPosition();
                    julius.ClearDialogue();
                    julius.Dialogue.Add("Have a gander at Cleopatra over there! I sure would like to conquer -her- lands. I do think it's time I go ask for her numerals.");

                    napoleon.MapName = "Stone Fort Gate";
                    napoleon.PositionX = 1840;
                    napoleon.PositionY = 370;
                    napoleon.FacingRight = true;
                    napoleon.UpdateRecAndPosition();

                    genghis.MapName = "Stone Fort Gate";
                    genghis.PositionX = 1470;
                    genghis.PositionY = 370;
                    genghis.FacingRight = false;
                    genghis.UpdateRecAndPosition();
                    genghis.ClearDialogue();
                    genghis.Dialogue.Add("Am I the only man here who has not seen these devil beasts yet?");

                    cleo.MapName = "Stone Fort Gate";
                    cleo.PositionX = 1690;
                    cleo.PositionY = 370;
                    cleo.FacingRight = true;
                    cleo.UpdateRecAndPosition();
                    cleo.Dialogue.Add("I am beginning to think that my soldiers are underdressed for the occasion.");
                    player.playerState = Player.PlayerState.standing;
                    game.CurrentChapter.CutsceneState++;
                    game.CurrentChapter.CurrentMap.ForceToNewMap(new Portal(0, 0, "Stone Fort Gate"), OutsideStoneFort.toOutskirts);
                    game.CurrentChapter.CurrentMap.leavingMapTimer = 41;
                    break;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            switch (state)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    player.Draw(s);

                    if (state == 4 && cutseneFrame < 23)
                        s.Draw(cutscene.ElementAt(cutseneFrame).Value, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y), Color.White);
                    if(cutseneFrame > 22)
                        s.Draw(cutscene.ElementAt(22).Value, new Vector2(0, game.CurrentChapter.CurrentMap.mapRec.Y), Color.White);

                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (julius != null)
                    {
                        if (julius.Talking == true)
                        {
                            julius.DrawDialogue(s);
                        }
                        else if (napoleon.Talking == true)
                        {
                            napoleon.DrawDialogue(s);
                        }
                        else if (cleo.Talking == true)
                        {
                            cleo.DrawDialogue(s);
                        }
                        else if (genghis.Talking == true)
                        {
                            genghis.DrawDialogue(s);
                        }
                    }

                    if (state == 8 || state == 0)
                        DrawFade(s, 0);
                    if (state == 2)
                        DrawFade(s, 1);
                    s.End();
                    break;
                case 9:
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}
