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
    class SavedFirstKidScene : Cutscene
    {
        // ATTRIBUTES \\
        NPC crossroadsKidFace;

        //--Takes in a background and all necessary objects
        public SavedFirstKidScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {

            List<String> kidDialogue = new List<string>();
            kidDialogue.Add("thx 4 saving me bby");
            kidDialogue.Add("These deer have been kidnapping kids all over the woods! You have to help \nme save them!");
            kidDialogue.Add("How about this, you go ahead and free the rest of the kids out there in the scary \nwoods, and I'll stay here and be the look out. We'll be the best \nteam this part of the woods has ever seen!");
            kidDialogue.Add("Does that sound good?");

            crossroadsKidFace = new NPC(game.NPCSprites["Paul"], kidDialogue, new Rectangle(-1000, 0, 0, 0),
        player, game.Font, game, "Crossroads", "Paul", false);
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
                        player.playerState = Player.PlayerState.relaxedStanding;
                        crossroadsKidFace.Talking = true;
                        game.ChapterTwo.NPCs["CrossroadsKid"].RecX = 1558;
                        game.ChapterTwo.NPCs["CrossroadsKid"].PositionX = 1558;
                    }

                    crossroadsKidFace.UpdateInteraction();

                    if (!crossroadsKidFace.Talking)
                    {
                        timer = 0;
                        state++;
                    }
                    break;
                case 1:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 180)
                    {
                        timer = 0;
                        state++;
                    }
                    break;

                case 2:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (firstFrameOfTheState)
                    {
                        crossroadsKidFace.Dialogue.Clear();
                        crossroadsKidFace.Dialogue.Add("Good! Before they caught me I saw them coming down that path \nbehind you. Start there. TEAM BREAK!");
                        crossroadsKidFace.Talking = true;
                    }

                    crossroadsKidFace.UpdateInteraction();

                    if (!crossroadsKidFace.Talking)
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
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    crossroadsKidFace.DrawDialogue(s);
                    s.End();
                    break;

                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemiesAndHazards(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
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
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    crossroadsKidFace.DrawDialogue(s);
                    s.End();
                    break;
            }
        }
    }
}