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
    class ScroogeRoomThree : Cutscene
    {
        NPC scrooge;
        GameObject camFollow;
        NPC death;
        Boolean drawDeath = false;
        Marley marley;

        public ScroogeRoomThree(Game1 g, Camera cam, Player p)
            : base(g, cam, p)
        {
            camFollow = new GameObject();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            game.NPCSprites["Death"] = content.Load<Texture2D>(@"NPC\Main\Death");
            marley = game.CurrentChapter.NPCs["Marley"] as Marley;

            Game1.npcFaces["Death"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\DeathNormal");

        }

        public override void  UnloadContent()
        {
            game.NPCSprites["Death"] = Game1.whiteFilter;

            Game1.npcFaces["Death"].faces["Normal"] = Game1.whiteFilter;
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
                        scrooge = game.CurrentChapter.NPCs["Ebenezer Scrooge"];
                        LoadContent();
                        death = new NPC(game.NPCSprites["Death"], new List<String>(), new Rectangle(1238, 315, 516, 388), player, game.Font, game, "Haunted Bedroom", "Death", false);
                        death.FacingRight = true;
                        camFollow.PositionX = player.VitalRec.Center.X;
                    }
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (camFollow.PositionX < 1400)
                        camFollow.PositionX += 10;
                    else
                    {
                        state++;
                        timer = 0;
                    }

                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        scrooge.ClearDialogue();
                        scrooge.Dialogue.Add("Oh! Make it all stop! Please!");
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
                        marley.Dialogue.Add("HAHAHA! NEVER! I WEAR THE CHAIN I FORGED IN LIFE. I MADE IT, LINK BY LINK AND YARD BY YARD, WHILE ON EARTH, AND NOW I WILL NEVER BE RID OF IT... ALL THE BETTER TO STRANGLE YOU WITH!");
                        marley.Dialogue.Add("Now prepare, Ebenezer Scrooge, for your doom... THE FINAL AND MOST TERRIBLE OF THE THREE SPIRITS ...the ghost of CHRISTMAS YET TO COME!");

                        marley.Talking = true;
                    }

                    marley.UpdateInteraction();
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (!marley.Talking)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    if (firstFrameOfTheState)
                    {
                        Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(1316, 323, 370, 277), 2);
                        death.Dialogue.Add("...?");
                        death.Dialogue.Add("Oh. This again. Right.");
                        death.Dialogue.Add("Uhhh...yeah. Be generous. Give your money to charity or you'll die. Christmas is good.");
                        death.Dialogue.Add("Welp. That about does it. Job well done, I always say.");
                        death.Dialogue.Add("Always a pleasure.");
                        death.Talking = true;
                    }

                    if (timer > 15)
                        drawDeath = true;

                    if (timer > 150)
                    {
                        death.UpdateInteraction();

                        if (!death.Talking)
                        {
                            Chapter.effectsManager.AddSmokePoofSpecifySize(new Rectangle(1316, 323, 370, 277), 2);
                            state++;
                            timer = 0;

                        }
                    }

                    break;
                case 4:
                    if (firstFrameOfTheState)
                    {
                        marley.ClearDialogue();
                        marley.Dialogue.Add("...");
                        marley.Dialogue.Add("No no no NO NO NOO!");
                        marley.Dialogue.Add("It's all ruined! My carefully crafted story! This is NOT HOW IT GOES!!");
                        marley.Dialogue.Add("AAGGGGGHHHHHH! This is YOUR fault! You've interrupted my story and now you're going to suffer for it!");
                        marley.Dialogue.Add("I'm going to rip that stupid headband off your head and make you choke on it, you shrimpy, slack-jawed moron!");
                    }
                    if (timer > 15)
                    {
                        drawDeath = false;
                    }

                    if (marley.DialogueState >= 3)
                    {
                        if (player.VitalRec.Center.X < marley.Rec.Center.X)
                            marley.FacingRight = false;
                    }

                    if(marley.Talking)
                        marley.UpdateInteraction();

                    if (timer == 45)
                    {
                        marley.Talking = true;
                    }
                    else if (timer > 45 && !marley.Talking)
                    {
                        state++;
                        timer = 0;
                        marley.Teleport();
                    }
                    break;
                case 5:

                    if (firstFrameOfTheState)
                    {

                    }

                    if (marley.teleportFrame > 1 && !game.CurrentChapter.BossFight)
                    {
                        LiteratureGuardian guardian = new LiteratureGuardian(new Vector2(930, 46), "Literature Guardian", game, ref player, game.CurrentChapter.CurrentMap, 285);
                        guardian.facingRight = marley.FacingRight;
                        game.CurrentChapter.BossFight = true;
                        game.CurrentChapter.CurrentBoss = guardian;
                        game.ChapterTwo.ChapterTwoBooleans["bedroomThreeCleared"] = true;

                        marley.Alpha = 0f;
                    }

                    if(game.CurrentChapter.BossFight && !marley.teleporting)
                        game.CurrentChapter.CurrentBoss.HealthBarGrow();

                    if (timer > 200 && !marley.teleporting)
                    {
                        marley.MapName = "";
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        UnloadContent();
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

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

                    if(drawDeath)
                        death.Draw(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);

                    if(game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (scrooge != null && scrooge.Talking)
                        scrooge.DrawDialogue(s);

                    if (death != null && death.Talking && timer > 150)
                        death.DrawDialogue(s);


                    if (marley != null && marley.Talking)
                        marley.DrawDialogue(s);

                    if (game.CurrentChapter.CurrentBoss != null)
                        game.CurrentChapter.CurrentBoss.DrawHud(s);
                    s.End();
                    break;
            }
        }
    }
}
