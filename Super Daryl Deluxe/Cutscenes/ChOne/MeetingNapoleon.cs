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
    class MeetingNapoleon : Cutscene
    {
        NPC napoleon;
        NPC frenchSoldier;
        int talkingState;
        int specialTimer;

        public MeetingNapoleon(Game1 g, Camera cam, Player p)
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
                        napoleon = game.CurrentChapter.NPCs["Napoleon"];
                        frenchSoldier = game.CurrentChapter.NPCs["French Soldier"];
                        napoleon.ClearDialogue();
                        frenchSoldier.ClearDialogue();

                        napoleon.Dialogue.Add("Ho hoh! Now what is zis? How did you make it through ze guards? Zis area is under permanent curfew, boy! No man, woman, or sniffling child may be outside for any reason!");
                        napoleon.Dialogue.Add("We are chasing a dangerous criminal who may be involved with ze chaos happening in my territory. In fact, you zeem odd...your clothing is not of zis time or place, much like his. Are you another one of zem...?");
                        napoleon.Dialogue.Add("...! Guards! Arrest zis--");
                        napoleon.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    napoleon.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (napoleon.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:
                    if (firstFrameOfTheState)
                    {
                        napoleon.ClearDialogue();
                        frenchSoldier.ClearDialogue();
                        frenchSoldier.Dialogue.Add("Sir, a letter just arrived from camp. Ze enemy has noticed that our forces are split and is preparing an assault.");
                        frenchSoldier.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);


                    frenchSoldier.UpdateInteraction();


                    if (frenchSoldier.Talking == false)
                    {
                        state++;
                        timer = 0;
                        specialTimer = 0;
                    }
                    

                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        napoleon.Dialogue.Clear();
                        frenchSoldier.Dialogue.Clear();
                        napoleon.AddQuest(game.ChapterOne.protectTheCamp);
                        napoleon.Talking = true;
                    }

                    napoleon.Choice = 0;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    napoleon.UpdateInteraction();

                    Chapter.effectsManager.Update();
                    if (napoleon.Talking == false && specialTimer == 0)
                    {
                        specialTimer++;
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(napoleon.RecX + 150, napoleon.RecY + 170, 200, 200), 2);
                    }

                    if (specialTimer < 60 && specialTimer > 0)
                        specialTimer++;

                    if (specialTimer >= 10)
                    {
                        napoleon.MapName = "Napoleon's Tent";
                        napoleon.RecX = 680;
                        napoleon.RecY = 310;
                        napoleon.PositionX = 680;
                        napoleon.PositionY = 310;
                    }
                    if (specialTimer >= 60)
                    {
                        Chapter.effectsManager.foregroundFButtonRecs.Clear();
                        Chapter.effectsManager.fButtonRecs.Clear();

                        NapoleonsCamp.ToTrenchfootField.IsUseable = false;
                        frenchSoldier.Dialogue.Clear();
                        frenchSoldier.Dialogue.Add("I would suggest that you offer your assistance back in ze History Room if you would like ze curfew lifted here.");
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
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
                    if(timer > 1)
                        napoleon.DrawDialogue(s);
                    s.End();
                    break;

                case 1:

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

                    frenchSoldier.DrawDialogue(s);
                    
                    s.End();
                    break;
                case 2:

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
                    napoleon.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
