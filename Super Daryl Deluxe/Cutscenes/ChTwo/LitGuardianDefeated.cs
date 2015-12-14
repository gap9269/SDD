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
    class LitGuardianDefeated : Cutscene
    {
        NPC scrooge;
        GameObject camFollow;
        NPC litGuardianFace;
        Rectangle teleportRec;
        LiteratureGuardian litGuardian;
        public LitGuardianDefeated(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Game1.npcFaces["Literature Guardian"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Literature Guardian Normal");
        }

        public override void  UnloadContent()
        {
            Game1.npcFaces["Literature Guardian"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void Play()
        {
            base.Play();
            if (game.CurrentChapter.CurrentBoss != null)
                game.CurrentChapter.CurrentBoss.Update();
            if (scrooge != null)
                scrooge.Update();
            Chapter.effectsManager.Update();
            player.CutsceneUpdate();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        LoadContent();
                        litGuardian = game.CurrentChapter.CurrentBoss as LiteratureGuardian;
                        scrooge = game.CurrentChapter.NPCs["Ebenezer Scrooge"];
                        litGuardianFace = new NPC(Game1.whiteFilter, new List<String>(), new Rectangle(1000238, 30015, 516, 388), player, game.Font, game, "Haunted Bedroom", "Literature Guardian", false);
                        camFollow.PositionX = player.VitalRec.Center.X;

                        if(player.VitalRec.Center.X - 1000 + litGuardian.rectanglePaddingLeftRight >= 6)
                            teleportRec = new Rectangle(player.VitalRec.Center.X - 800, 46, 0, 0);
                        else
                            teleportRec = new Rectangle(player.VitalRec.Center.X + 800, 46, 0, 0);

                    }
                    litGuardian.CutsceneTeleport(teleportRec);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if(litGuardian.movementState == LiteratureGuardian.Movestate.idle)
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        litGuardianFace.ClearDialogue();
                        litGuardianFace.Dialogue.Add("AAAAAUUUUUUUGGGGHHHHHH!");
                        litGuardianFace.Dialogue.Add("Who the hell do you think you are??");
                        litGuardianFace.Dialogue.Add("I am one of the Elite, the Guardian of the Literature Realm, an all-powerful Honor Student!");
                        litGuardianFace.Dialogue.Add("I was chosen for my incredible power and creativity! Who are you to think you can interrupt ME in MY DOMAIN?");
                        litGuardianFace.Dialogue.Add("Rrrrrrgh. I'm supposed to be playing the Queen of Hearts right now, but you've made me late.");
                        litGuardianFace.Dialogue.Add("This isn't over, boy.");
                        litGuardianFace.Talking = true;
                    }
                    litGuardianFace.UpdateInteraction();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (litGuardian.VitalRec.Center.X > player.VitalRec.Center.X)
                        player.FacingRight = true;
                    else
                        player.FacingRight = false;

                    if (!litGuardianFace.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:

                    if (firstFrameOfTheState)
                    {
                        litGuardianFace.ClearDialogue();
                        teleportRec = new Rectangle(50000, 46, 0, 0);
                    }

                    litGuardian.CutsceneTeleport(teleportRec);
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (litGuardian.movementState == LiteratureGuardian.Movestate.idle)
                    {
                        game.ChapterTwo.ChapterTwoBooleans["literatureGuardianDefeated"] = true;
                        game.CurrentChapter.BossFight = false;
                        game.CurrentChapter.CurrentBoss = null;
                        
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(1636, 372, 230, 172), 2);
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer == 12)
                    {
                        scrooge.canTalk = true;
                        scrooge.FacingRight = false;
                        scrooge.RecX = 20200;
                        scrooge.RecY = 343;
                        scrooge.PositionX = scrooge.RecX;
                        scrooge.PositionY = scrooge.RecY;
                        scrooge.UpdateRecAndPosition();
                        (scrooge as Scrooge).isScared = false;
                        Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(1636, 489, 230, 172), 2);
                    }

                    if (timer == 24)
                    {
                        scrooge.canTalk = true;
                        scrooge.FacingRight = false;
                        scrooge.RecX = 1503;
                        scrooge.RecY = 343;
                        scrooge.PositionX = scrooge.RecX;
                        scrooge.PositionY = scrooge.RecY;
                        scrooge.UpdateRecAndPosition();
                        (scrooge as Scrooge).isScared = false;

                        state = 0;
                        timer = 0;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        UnloadContent();
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

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (scrooge != null && scrooge.Talking)
                        scrooge.DrawDialogue(s);
                    if (litGuardianFace != null && litGuardianFace.Talking)
                        litGuardianFace.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}
