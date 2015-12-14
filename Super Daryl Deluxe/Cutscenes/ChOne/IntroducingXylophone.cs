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
    class IntroducingXylophone : Cutscene
    {
        NPC theaterManager;
        GameObject camFollow;
        int specialState = 0;

        LordXylophone xylophone;

        float fadeAlpha = 1f;
        public IntroducingXylophone(Game1 g, Camera cam, Player p)
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
                        camFollow = new GameObject();
                        camFollow.Position = new Vector2(0, 0);
                        theaterManager = game.CurrentChapter.NPCs["Theater Manager"];
                        theaterManager.Position = new Vector2(1033, 320);
                        theaterManager.Alpha = 1;
                        theaterManager.UpdateRecAndPosition();
                        theaterManager.MapName = "Axis of Musical Reality";
                        theaterManager.FacingRight = true;
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("What the?");
                        theaterManager.Dialogue.Add("Oh, for fu--did you destroy all of my guards?");
                        theaterManager.Dialogue.Add("Ha, that's pretty metal, dude. It's a shame I'm royally pissed off now.");
                        theaterManager.Dialogue.Add("You have NO idea how much of a pain in the ass it is to make guards. Especially those goddamn cymbals. Do you think hacked off limbs grow on trees?");
                        theaterManager.Dialogue.Add("I can't do a thing until I get more guards, which means I can't force the nice tenants here to cook me up more platinum records, which frankly, makes me pretty upset. I don't tolerate setbacks in MY business.");
                        theaterManager.Dialogue.Add("And you just set me back a couple of days... it's kind of a problem that any moron like you can waltz in here and tear all my hard work apart.");
                    }

                    if (timer > 40)
                    {
                        if (fadeAlpha > 0)
                            fadeAlpha -= 1f / 90f;
                    }

                    camFollow.PositionX = player.VitalRec.Center.X + 218;
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (player.RecX <= 568)
                    {
                        player.CutsceneWalk(new Vector2(4, 0));
                        camFollow.PositionX = player.VitalRec.Center.X;
                        camFollow.RecX = player.VitalRec.Center.X;
                        camFollow.PositionY = 360;
                    }

                    else
                    {
                        player.CutsceneStand();
                        camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                        state++;
                        timer = 0;
                    }
                    break;
                case 1:

                    if (firstFrameOfTheState)
                    {
                        theaterManager.FacingRight = false;
                    }

                    if(timer > 45)
                        theaterManager.Talking = true;

                    if (fadeAlpha > 0)
                        fadeAlpha -= 1f / 90f;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    theaterManager.UpdateInteraction();

                    if (theaterManager.DialogueState == 2)
                        theaterManager.CurrentDialogueFace = "Sneer";
                    else
                        theaterManager.CurrentDialogueFace = "Normal";

                    if (theaterManager.Talking == false && timer > 60)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {
                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("You have a real way with words, you know that?");
                        theaterManager.Dialogue.Add("Look, I can do this all day. But I won't. I have a music empire to build, and J.P. is gonna be real pissed at me if he finds out you stepped foot in here.");
                    }

                    player.CutsceneStand();

                    if (theaterManager.Talking)
                        theaterManager.UpdateInteraction();

                    if (timer == 30)
                        theaterManager.Talking = true;

                    if (theaterManager.PositionX > 822)
                        theaterManager.Move(new Vector2(-3, 0), Platform.PlatformType.rock);
                    else
                        theaterManager.moveState = NPC.MoveState.standing;

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (theaterManager.PositionX <= 822 && theaterManager.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 3:
                    if (firstFrameOfTheState)
                    {
                        game.CurrentChapter.CurrentMap.EnemiesInMap.Clear();
                        xylophone = new LordXylophone(new Vector2(2000, 338), "Lord Glockenspiel", game, ref player, game.CurrentChapter.CurrentMap);
                        xylophone.UpdateRectangles();
                        xylophone.Alpha = 1;
                        xylophone.Hostile = true;
                        xylophone.SpawnWithPoof = false;
                        xylophone.FacingRight = false;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(xylophone);

                        theaterManager.ClearDialogue();
                        theaterManager.Dialogue.Add("Here, have a shot at this guy. He's my prized creation. And try not to get your blood everywhere.");
                        theaterManager.Dialogue.Add("Sayonara, weirdo.");
                    }

                    if (xylophone.PositionX > 1100)
                    {
                        xylophone.CutsceneMove(-10);
                    }
                    else
                        xylophone.CutsceneStand();
                    
                    player.CutsceneStand();

                    if (theaterManager.Talking)
                    {
                        theaterManager.UpdateInteraction();

                        if (theaterManager.DialogueState == 1)
                            theaterManager.CurrentDialogueFace = "Sneer";
                    }

                    if (timer == 60)
                        theaterManager.Talking = true;

                    //Raise the bookshelf
                    if (specialState == 0 && (game.CurrentChapter.CurrentMap as AxisOfMusicalReality).RaiseBookshelf())
                        specialState = 1;

                    //Fade the manager out
                    if (specialState == 1 && theaterManager.Alpha > 0 && theaterManager.Talking == false)
                    {
                        theaterManager.Alpha -= .01f;

                        if (theaterManager.Alpha <= 0)
                            specialState = 2;
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    //Lower the bookshelf
                    if (specialState == 2 && (game.CurrentChapter.CurrentMap as AxisOfMusicalReality).LowerBookshelf())
                    {
                        theaterManager.MapName = "Manager's Office";

                        game.ChapterOne.ChapterOneBooleans["summonedXylophone"] = true;
                        state = 0;
                        timer = 0;
                        player.playerState = Player.PlayerState.standing;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CurrentMap.enteringMap = false;
                        AxisOfMusicalReality.ToRestrictedHallway.IsUseable = false;
                        AxisOfMusicalReality.ToAxisOfArt.IsUseable = false;
                        xylophone.enemyState = LordXylophone.EnemyState.none;
                        xylophone.attackState = LordXylophone.AttackState.melee;
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
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);

                    player.Draw(s);
                    s.End();
                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    if(timer > 1 && theaterManager.Talking)
                        theaterManager.DrawDialogue(s);

                    if (fadeAlpha > 0)
                        s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black * fadeAlpha);
                    s.End();
                    break;
            }
        }
    }
}
