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
    class DarylCaught : Cutscene
    {
        // ATTRIBUTES \\
        NPC tim;
        NPC deerFace;
        Rectangle cageRec;
        float cageRecVelocity;

        //--Takes in a background and all necessary objects
        public DarylCaught(Game1 g, Camera cam, Player player)
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
                        cageRec = new Rectangle(player.VitalRecX - 20, -500, player.VitalRecWidth + 40, 500);
                        List<String> deerDialogue = new List<string>();
                        deerDialogue.Add("We got him!");
                        deerDialogue.Add("Let's take them back to the boss. He'll be pleased with this one, for sure.");

                        tim = game.ChapterTwo.NPCs["Tim"];
                        deerFace = new NPC(game.NPCSprites["Alan"], deerDialogue, new Rectangle(-1000, 0, 0, 0),
                                player, game.Font, game, "Crossroads", "Alan", false);

                    }
                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    if (timer > 40)
                    {
                        cageRecVelocity += GameConstants.GRAVITY;
                        cageRec.Y += (int)cageRecVelocity;
                    }

                    if (cageRec.Intersects(player.CurrentPlat.Rec))
                    {
                        game.Camera.ShakeCamera(10, 10);
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(cageRec.Center.X - 25, cageRec.Y + cageRec.Height - 25, 50, 50), 2);
                        timer = 0;
                        state++;
                    }
                    break;
                case 1:
                    player.playerState = Player.PlayerState.relaxedStanding;
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer == 10)
                    {
                        Goblin gob = new Goblin(Vector2.Zero, "Field Goblin", game, ref player, game.CurrentChapter.CurrentMap);

                        gob.Position = new Vector2(cageRec.X + cageRec.Width + 75, 680 - gob.Rec.Height - 1);
                        gob.UpdateRectangles();
                        gob.SpawnWithPoof = false;
                        gob.Respawning = false;
                        gob.Alpha = 1f;
                        gob.FacingRight = false;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(gob);
                        Chapter.effectsManager.AddSmokePoof(gob.VitalRec, 2);
                    }

                    if (timer == 20)
                    {
                        Goblin gob = new Goblin(Vector2.Zero, "Field Goblin", game, ref player, game.CurrentChapter.CurrentMap);

                        gob.Position = new Vector2(cageRec.X - 275, 680 - gob.Rec.Height - 1);
                        gob.UpdateRectangles();
                        gob.SpawnWithPoof = false;
                        gob.Respawning = false;
                        gob.Alpha = 1f;
                        gob.FacingRight = true;
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(gob);
                        Chapter.effectsManager.AddSmokePoof(gob.VitalRec, 2);
                    }

                    if (timer == 35)
                    {
                        deerFace.Talking = true;
                    }

                    if (deerFace.Talking)
                        deerFace.UpdateInteraction();

                    if (timer > 35 && deerFace.Talking == false)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 2:

                    if (firstFrameOfTheState)
                    {
                        tim.Talking = true;
                        tim.Dialogue.Clear();
                        tim.Dialogue.Add("Oh for fuck's sake...");
                    }
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    FadeOut(240);
                    break;
                case 3:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60)
                    {
                        timer = 0;
                        state++;
                        game.CurrentChapter.CutsceneState++;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        player.playerState = Player.PlayerState.standing;
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
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.Draw(Game1.whiteFilter, cageRec, Color.Gray * .5f);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.Draw(Game1.whiteFilter, cageRec, Color.Gray * .5f);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if(deerFace.Talking)
                        deerFace.DrawDialogue(s);
                    s.End();
                    break;

                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.Draw(Game1.whiteFilter, cageRec, Color.Gray * .5f);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if(tim.Talking)
                        tim.DrawDialogue(s);

                    DrawFade(s, 0f);
                    s.End();
                    break;
                case 3:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.Draw(Game1.whiteFilter, cageRec, Color.Gray * .5f);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);

                    if (tim.Talking)
                        tim.DrawDialogue(s);

                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, 720), Color.Black);
                    s.End();
                    break;
            }
        }
    }
}