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
    class TrenchcoatCutscene : Cutscene
    {
        // ATTRIBUTES \\
        NPC cronie;

        //--Takes in a background and all necessary objects
        public TrenchcoatCutscene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void Play()
        {
            base.Play();

            Chapter.effectsManager.Update();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        dialogue.Clear();
                        dialogue.Add("You looking for a textbook?");
                        cronie = game.CurrentChapter.NPCs["TrenchcoatCrony"];
                        cronie.FacingRight = true;
                        player.CanJump = false;
                        DialogueState = 0;
                        player.Sprinting = false;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    cronie.UpdateRecAndPosition();

                    if (player.playerState == Player.PlayerState.standing || player.playerState == Player.PlayerState.running)
                        player.CutsceneStand();
                    else
                        player.Update();


                    if (timer == 90)
                    {
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(1625, 400, 300, 300), 2);
                        cronie.PositionX = 1600;
                        cronie.PositionY = 620 - 388;
                    }

                    if (timer > 100)
                    {
                        player.FacingRight = false;
                        player.Sprinting = false;
                        player.playerState = Player.PlayerState.relaxedStanding;
                        player.CanJump = true;
                    }

                    //--REACH END OF LOBBY, RESET TIMER AND DIALOGUE
                    if (timer > 145)
                    {
                        cronie.moveState = NPC.MoveState.standing;
                        timer = 0;
                        dialogueState = 0;
                        dialogue.Clear();
                        state++;
                    }
                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        cronie.Dialogue.Add("Well, you're too late. I already grabbed all of the product here for my boss.");
                        cronie.Dialogue.Add("Of course I could sell you one, but keep in mind this is top quality stuff and it'll cost you a pretty penny.");
                        cronie.Talking = true;
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    player.playerState = Player.PlayerState.relaxedStanding;

                    cronie.UpdateInteraction();

                    if (cronie.Talking == false)
                    {
                        cronie.Dialogue.Clear();
                        cronie.Dialogue.Add("That'll be five bucks.");
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        player.playerState = Player.PlayerState.standing;
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
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if(!firstFrameOfTheState && timer < 60)
                        DrawDialogue(s);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    cronie.DrawDialogue(s);
                    s.End();
                    break;
               
            }
        }
    }
}
