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
    class TestScene : Cutscene
    {
        // ATTRIBUTES \\
        NPC cronie;
        Texture2D back, fireBack, fireFore, ruins, kids, daryl, robatto;

        int fireBackPos, fireForePos, ruinsPos, kidsPos, darylPos, robattoPos;

        //--Takes in a background and all necessary objects
        public TestScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            back = content.Load<Texture2D>(@"Cutscenes\Trailer\t1Back");
            fireBack = content.Load<Texture2D>(@"Cutscenes\Trailer\t1BackFire");
            fireFore = content.Load<Texture2D>(@"Cutscenes\Trailer\t1FireFore");
            ruins = content.Load<Texture2D>(@"Cutscenes\Trailer\t1Ruins");
            daryl = content.Load<Texture2D>(@"Cutscenes\Trailer\t1Daryl");
            robatto = content.Load<Texture2D>(@"Cutscenes\Trailer\t1Robatto");
            kids = content.Load<Texture2D>(@"Cutscenes\Trailer\t1Kids");
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
                        LoadContent();
                    }


                    camera.Update(player, game);
                    if (robattoPos > -110)
                    {
                        if (ruinsPos < -110)
                        {
                            ruinsPos += 1;
                        }

                        if (kidsPos < -110)
                        {
                            kidsPos += 3;
                        }

                        if (fireForePos < -110)
                        {
                            fireForePos += 13;
                        }

                        if (robattoPos > -110)
                        {
                            robattoPos -= 10;
                        }

                        if (darylPos < -110)
                        {
                            darylPos += 6;
                        }

                        if (fireBackPos > -110)
                        {
                            fireBackPos -= 1;
                        }
                    }
                    //--REACH END OF LOBBY, RESET TIMER AND DIALOGUE
                    if (timer > 500)
                    {
                        darylPos = -390;
                        kidsPos = -240;
                        fireForePos = -720;
                        robattoPos = 340;
                        ruinsPos = -160;
                        fireBackPos = -60;
                        timer = 0;
                       // cronie.moveState = NPC.MoveState.standing;
                       // timer = 0;
                       // dialogueState = 0;
                        //dialogue.Clear();
                        //state++;
                    }
                    break;

                case 1:
                    if (firstFrameOfTheState)
                    {
                        cronie.Dialogue.Add("Well, you're too late. I already grabbed all of the goods here for my boss.");
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
null, null, null, null, camera.StaticTransform);
                    if (timer > 0)
                    {
                        s.Draw(back, new Vector2(-110, 0), Color.White);
                        s.Draw(ruins, new Vector2(ruinsPos, 0), Color.White);
                        s.Draw(fireBack, new Vector2(fireBackPos, 0), Color.White);
                        s.Draw(kids, new Vector2(kidsPos, 0), Color.White);
                        s.Draw(daryl, new Vector2(darylPos, 0), Color.White);
                        s.Draw(robatto, new Vector2(robattoPos, 0), Color.White);
                        s.Draw(fireFore, new Vector2(fireForePos, 0), Color.White);
                    }
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
