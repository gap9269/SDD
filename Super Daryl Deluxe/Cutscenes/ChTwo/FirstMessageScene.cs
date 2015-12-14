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
    class FirstMessageScene : Cutscene
    {
        NPC messengerBoy;
        public FirstMessageScene(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Game1.npcFaces["Messenger Boy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Messenger Boy Normal");
            game.NPCSprites["Messenger Boy"] = content.Load<Texture2D>(@"NPC\History\Messenger Boy");

        }

        public override void  UnloadContent()
        {
            Game1.npcFaces["Messenger Boy"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Messenger Boy"] = Game1.whiteFilter;

        }

        public override void Play()
        {
            base.Play();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        messengerBoy = game.CurrentChapter.NPCs["Messenger Boy"];
                        messengerBoy.PositionX = player.PositionX - 200;
                        messengerBoy.PositionY = 330;
                        messengerBoy.UpdateRecAndPosition();
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(messengerBoy.RecX + 150, messengerBoy.RecY + 170, 200, 200), 2);
                    }

                    if (timer == 15)
                    {
                        messengerBoy.MapName = "Mongolian Camp";

                        messengerBoy.FacingRight = true;

                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if(timer > 60)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        player.FacingRight = false;
                        messengerBoy.ClearDialogue();
                        messengerBoy.Dialogue.Add("Message for Junior Baby Intern!");
                        messengerBoy.Dialogue.Add("That you, kid? You match the description they gave me, that's for sure. Taken me ages to find ya.");
                        messengerBoy.Dialogue.Add("*Ahem* Well, you gots a message from Paul Palte:");
                        messengerBoy.Dialogue.Add("\"Dear Junior Baby Intern, I hope this message finds you well into your search for Balto's phone. He won't shut the hell up about it and I'm getting tired of listening to him. It's been like three hours already.\"");
                        messengerBoy.Dialogue.Add("...\"You better not have gotten distracted or you're demoted.\"");
                        messengerBoy.Dialogue.Add("...\"P.S: Robatto hasn't come out of the History Room yet. How hard is it to find someone inside of a classroom?\"");
                        messengerBoy.Dialogue.Add("...\"Love, Paul and Co.\"");
                        messengerBoy.Dialogue.Add("*Ahem* Yep, that's it. I'm outta here.");
                        messengerBoy.Talking = true;
                    }
                    messengerBoy.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (!messengerBoy.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(messengerBoy.RecX + 150, messengerBoy.RecY + 170, 200, 200), 2);
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer == 15)
                    {
                        messengerBoy.MapName = "Not a real map";
                        messengerBoy.UpdateRecAndPosition();
                    }

                    if (timer == 24)
                    {
                        game.CurrentChapter.NPCs["Alan"].ClearDialogue();
                        game.CurrentChapter.NPCs["Alan"].Dialogue.Add("Where have you been? Tasks are piling up, Balto is bitching about his phone, and you're MIA all morning.");

                        game.CurrentChapter.NPCs["Balto"].ClearDialogue();
                        game.CurrentChapter.NPCs["Balto"].Dialogue.Add("Where is my phone?! My grandma's birthday is coming up soon and I better have my phone back for it!");

                        game.CurrentChapter.NPCs["Paul"].ChangeQuestDialogueForAcceptedQuest("What, are you taking the day off or something? You're not earning any promotions at this rate.");

                        UnloadContent();
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
                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);
                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);

                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if (messengerBoy != null && messengerBoy.Talking)
                        messengerBoy.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
