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
    class TutorialEnd : Cutscene
    {
        // ATTRIBUTES \\
        NPC associateOne;

        //--Takes in a background and all necessary objects
        public TutorialEnd(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            List<String> assocOneDialogue = new List<string>();
            assocOneDialogue.Add("What an amazing end to an amazing tutorial level!");
            assocOneDialogue.Add("It has been a pleasure assisting you, player. I'll send you back to the party \nnow. I'm removing all of the equipment that you found, so make sure to \nget some more. Enjoy the rest of this demo!");

            associateOne = new NPC(game.NPCSprites["Paul"], assocOneDialogue, new Rectangle(-1000, 0, 0, 0),
                    player, game.Font, game, "The Credits", "Demo Danny", false);
        }

        public override void Play()
        {
            base.Play();
            switch (state)
            {
                case 0:
                    if (firstFrameOfTheState)
                    {
                        associateOne.Talking = true;
                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    associateOne.UpdateInteraction();

                    player.CanJump = false;

                    if (player.playerState == Player.PlayerState.standing || player.playerState == Player.PlayerState.running)
                    {
                        player.Landing = false;
                        player.CutsceneStand();
                    }
                    else
                        player.Update();

                    if (associateOne.Talking == false)
                    {
                        state++;
                        timer = 0;
                    }
                    break;

                case 1:
                    player.CutsceneStand();
                    FadeOut(120);
                    break;
                case 2:
                    if (firstFrameOfTheState)
                    {

                        //Reset the NPC sprites
                        for (int i = 0; i < game.CurrentChapter.NPCs.Count; i++)
                        {
                            if (game.CurrentChapter.NPCs.ElementAt(i).Value.MapName == game.CurrentChapter.CurrentMap.MapName)
                            {
                                game.CurrentChapter.NPCs.ElementAt(i).Value.Spritesheet = Game1.whiteFilter;
                            }
                        }

                        game.CurrentChapter.CurrentMap = Game1.schoolMaps.maps["TheParty"];
                        game.CurrentChapter.CurrentMap.LoadContent();
                        game.CurrentChapter.CurrentMap.LoadEnemyData();

                        player.Position = new Vector2(1350, 290); //1350 for Party
                        player.UpdatePosition();

                        game.Camera.centerTarget = new Vector2(player.PositionX + (player.Rec.Width / 2), 0);

                        game.Camera.center = game.Camera.centerTarget;

                        game.CurrentChapter.NPCs["Alan"].PositionX = 513;
                        game.CurrentChapter.NPCs["Alan"].CurrentDialogueFace = "Normal";
                        game.CurrentChapter.NPCs["Alan"].MapName ="The Party";
                        game.CurrentChapter.NPCs["Alan"].Spritesheet = game.NPCSprites["Alan"];
                        game.CurrentChapter.NPCs["Alan"].Dialogue.Clear();
                        game.CurrentChapter.NPCs["Alan"].Dialogue.Add("Keep your eye out for any of Trenchcoat Kid's employees. They always follow us \nhere and try to steal our Textbook market.");



                        game.CurrentChapter.NPCs["Paul"].CurrentDialogueFace = "Normal";
                        game.CurrentChapter.NPCs["Paul"].MapName = "The Party";
                        game.CurrentChapter.NPCs["Paul"].Spritesheet = game.NPCSprites["Paul"];
                        game.CurrentChapter.NPCs["Paul"].Dialogue.Clear();
                        game.CurrentChapter.NPCs["Paul"].Dialogue.Add("While I do respect a healthy dose of asset seizure, I don't understand why you \nstill have Balto's cell phone. He's been bitching about it all night, and I can't \nimagine the texts he gets are very interesting.");

                        game.CurrentChapter.NPCs["Paul"].UpdateRecAndPosition();
                        game.CurrentChapter.NPCs["Alan"].UpdateRecAndPosition();

                        player.HasCellPhone = true;

                    }

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);

                    if (timer > 60)
                    {
                        state++;
                        timer = 0;
                    }
                    break;
                case 3:
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    FadeIn(120);
                    player.CutsceneStand();
                    break;
                case 4:
                    player.CutsceneStand();
                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    timer = 0;
                    game.CurrentChapter.state = Chapter.GameState.Game;
                    game.CurrentChapter.CutsceneState++;
                    player.CanJump = true;

                    player.Experience = 0;

                    player.RemoveAllEquipment();

                    player.OwnedAccessories.Clear();
                    player.OwnedHats.Clear();
                    player.OwnedHoodies.Clear();
                    player.OwnedWeapons.Clear();
                    player.StoryItems.Clear();

                    game.CurrentChapter.HUD.SkillsHidden = false;

                    //Add the new skills to the locker shop
                    game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Spin Slash"]);
                    game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Quick Retort"]);
                    game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Shocking Statement"]);
                    game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Mopping Up"]);
                    game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Fowl Mouth"]);

                    player.Money = 0;
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
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    associateOne.DrawDialogue(s);
                    s.End();
                    break;
                case 1:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    DrawFade(s, 1f);
                    s.End();
                    break;
                case 2:
                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, camera.StaticTransform);
                    s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black);
                    s.End();
                    break;
                case 3:

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
                    DrawFade(s, 0f);
                    s.End();
                    break;

                case 4:

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;
            }
        }
    }
}