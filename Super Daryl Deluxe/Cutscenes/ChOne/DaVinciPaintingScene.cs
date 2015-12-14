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
    class DaVinciPaintingScene : Cutscene
    {
        NPC daVinci;

        public DaVinciPaintingScene(Game1 g, Camera cam, Player p)
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
                        daVinci = game.SideQuestManager.nPCs["Leonardo Da Vinci"];
                        daVinci.ClearDialogue();
                        daVinci.Dialogue.Add("Oh, well hello there young--*sniff* ...");
                        daVinci.Dialogue.Add("Blegh. You must be another crony of that talentless alcoholic. I can smell his cheap booze a mile away.");
                        daVinci.Dialogue.Add("I can only assume that he sent you here to request money from me. I will tell you this now: I am not associated with that man. In fact I do not even know what business it is that he thinks I am a part of. I never asked for such a partnership!");
                        daVinci.Dialogue.Add("I introduced myself as a true renaissance man, but the drunken, deaf idiot heard \"business\" man. He's been nothing but a pain in the neck since, insisting that we work together.");
                        daVinci.Dialogue.Add("Oh, I tried to refuse, I really did. It's too bad the man can't hear a damned thing. No matter what I try to tell him, he just thinks I'm going to fund his operas.");
                        daVinci.Dialogue.Add("I am sorry that he has roped you into his lunacy as well. I'm beginning to believe that the only way out is to actually give the man what he wants.");
                        daVinci.Dialogue.Add("*sigh* Very well, young man. I will free you of this man's servitude. I do not have any money with me, and even if I did I would be hesitant to release it into the custody of that boozed up cretin. However I do have this painting that may fetch a coin or two on the market.");
                        daVinci.Dialogue.Add("I recommend you end this business as fast as you can and leave. The market is just up ahead. I do hope this is the last I hear of Ludwig Van Beethoven.");
                        daVinci.Talking = true;
                        player.playerState = Player.PlayerState.relaxedStanding;
                    }

                    daVinci.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (daVinci.Talking == false)
                    {
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        player.AddStoryItem("Old Painting", "an old painting", 1);
                        ChapterOne.fundRaising.SpecialConditions.Clear();
                        ChapterOne.fundRaising.SpecialConditions.Add("Sell Da Vinci's painting", false);
                        ChapterOne.fundRaising.SpecialConditions.Add("Return to Beethoven with the money", false);

                        Chapter.effectsManager.notificationQueue.Enqueue(new QuestUpdatedNotification(true));
                        daVinci.ClearDialogue();
                        daVinci.Dialogue.Add("Perhaps I will need to invent a machine to rid myself of Beethoven.");
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
                        daVinci.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
