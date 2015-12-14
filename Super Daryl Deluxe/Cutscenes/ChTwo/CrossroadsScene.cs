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
    class CrossroadsScene : Cutscene
    {
        // ATTRIBUTES \\
        NPC deerFace;
        NPC crossroadsKidFace;

        GameObject camFollow;
        //--Takes in a background and all necessary objects
        public CrossroadsScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {

            camFollow = new GameObject();
            camFollow.PositionX = 1500;

            List<String> deerDialogue = new List<string>();
            List<String> kidDialogue = new List<string>();

            deerFace = new NPC(game.NPCSprites["Alan"], deerDialogue, new Rectangle(-1000, 0, 0, 0),
                    player, game.Font, game, "Crossroads", "Alan", false);

            crossroadsKidFace = new NPC(game.NPCSprites["Paul"], kidDialogue, new Rectangle(-1000, 0, 0, 0),
        player, game.Font, game, "Crossroads", "Paul", false);



        }

        public override void Play()
        {
            base.Play();

            if (player.PositionX >= 900)
                player.playerState = Player.PlayerState.relaxedStanding;
            else
                player.CutsceneWalk(new Vector2(2, 0));

            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        BennyBeaker ben = new BennyBeaker(new Vector2(1451, 680 - 201), "Benny Beaker", game, ref player, Game1.schoolMaps.maps["Crossroads"]);
                        ben.SpawnWithPoof = false;
                        ben.FacingRight = true;
                        BennyBeaker ben1 = new BennyBeaker(new Vector2(1712, 680 - 201), "Benny Beaker", game, ref player, Game1.schoolMaps.maps["Crossroads"]);
                        ben1.SpawnWithPoof = false;
                        ben1.FacingRight = false;
                        BennyBeaker ben2 = new BennyBeaker(new Vector2(1865, 680 - 201), "Benny Beaker", game, ref player, Game1.schoolMaps.maps["Crossroads"]);
                        ben2.SpawnWithPoof = false;
                        ben2.FacingRight = false;

                        ben.Hostile = true;
                        ben1.Hostile = true;
                        ben2.Hostile = true;

                        ben.Alpha = 1f;
                        ben1.Alpha = 1f;
                        ben2.Alpha = 1f;

                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben1);
                        game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben2);

                        game.Camera.centerTarget = new Vector2(camFollow.PositionX, 0);

                        game.Camera.center = game.Camera.centerTarget;

                        if (timer >= 1)
                        {
                            timer = 0;
                            state++;
                        }
                    }

                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    break;
                case 1:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);
                    FadeIn(120);
                    break;
                case 2:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 3:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (firstFrameOfTheState)
                    {
                        crossroadsKidFace.Dialogue.Add("Oh god please let me go I didn't do anything!");
                        crossroadsKidFace.Talking = true;
                    }

                    crossroadsKidFace.UpdateInteraction();

                    if (!crossroadsKidFace.Talking)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 4:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (firstFrameOfTheState)
                    {
                        deerFace.Dialogue.Add("SILENCE! YOU WILL MAKE AN EXCELLENT OFFERING TO OUR KING!");
                        deerFace.Talking = true;
                    }

                    deerFace.UpdateInteraction();

                    if (!deerFace.Talking)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 5:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (firstFrameOfTheState)
                    {
                        crossroadsKidFace.Dialogue.Clear();
                        crossroadsKidFace.Dialogue.Add("This is crazy! You're just stupid deer how can yo-");
                        crossroadsKidFace.Dialogue.Add("-Wait! HEY YOU! YOU OVER THERE! SAVE ME, PLEASE!");
                        crossroadsKidFace.Talking = true;
                    }

                    crossroadsKidFace.UpdateInteraction();

                    if (!crossroadsKidFace.Talking)
                    {
                        game.CurrentChapter.CurrentMap.EnemiesInMap[0].FacingRight = false;
                        timer = 0;
                        state++;
                    }
                    break;

                case 6:
                    camera.Update(camFollow, game, game.CurrentChapter.CurrentMap);

                    if (firstFrameOfTheState)
                    {
                        deerFace.Dialogue.Clear();
                        deerFace.Dialogue.Add("IT'S OUR LUCKY DAY! GET THAT HUMAN CHILD!");
                        deerFace.Talking = true;
                    }

                    deerFace.UpdateInteraction();

                    if (!deerFace.Talking)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        player.playerState = Player.PlayerState.standing;

                        //--Add the second bridge quest and move the bridge kid to the new area
                        //game.ChapterTwo.NPCs["BridgeKidOne"].AddQuest(game.ChapterTwo.buildBridgeTwo);

                        game.CurrentChapter.NPCs["BridgeKidOne"].MapName = "Woodsy River";
                        game.CurrentChapter.NPCs["BridgeKidOne"].PositionX = 425;

                        game.CurrentChapter.NPCs["BridgeKidOne"].UpdateRecAndPosition();
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
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 2000, 2000), Color.Black);
                    s.End();
                    break;

                case 1:
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
                    DrawFade(s, 1);
                    s.End();
                    break;

                case 2:
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
                    s.End();
                    break;

                case 3:
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
                    crossroadsKidFace.DrawDialogue(s);
                    s.End();
                    break;

                case 4:
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
                    deerFace.DrawDialogue(s);
                    s.End();
                    break;

                case 5:
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
                    crossroadsKidFace.DrawDialogue(s);
                    s.End();
                    break;

                case 6:
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
                    deerFace.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}