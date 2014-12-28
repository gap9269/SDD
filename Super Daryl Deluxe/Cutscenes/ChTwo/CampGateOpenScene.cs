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
    class CampGateOpenScene : Cutscene
    {
        // ATTRIBUTES \\
        int gateFrame, gateTimer;
        Boolean opening = true;
        GameObject camFollow = new GameObject();
        TrojanHorse horse;

        //--Takes in a background and all necessary objects
        public CampGateOpenScene(Game1 g, Camera cam, Player player)
            : base(g, cam, player)
        {
            gateTimer = 5;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //gateCollapseSound = content.Load<SoundEffect>(@"Sound\Cutscenes\cutscene_goblin_gate");
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
                        player.StopSkills();

                       // camFollow.Rec = new Rectangle((int)camFollow.PositionX, (int)camFollow.PositionY, 1, 1);
                       // gateCollapseSound.CreateInstance().Play();
                    }

                    //if (Sound.ambienceVolume > 0)
                    //    Sound.IncrementAmbienceVolume(-.005f);
                    //else
                    //    Sound.StopAmbience();

                    if(timer == 2)
                        game.Camera.ShakeCamera(10, 4);

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    gateTimer--;

                    if (gateTimer == 0)
                    {
                        gateFrame++;
                        gateTimer = 5;
                    }

                    if(gateFrame == 14)
                        game.Camera.ShakeCamera(20, 7);

                    player.CutsceneStand();

                    if (gateFrame > 12 && timer > 150)
                    {
                        timer = 0;
                        state++;
                        horse = OutsideStoneFort.horse;
                        horse.PositionX = 2600;
                    }
                    break;
                case 1:

                    camera.Update(player, game, game.CurrentChapter.CurrentMap);
                    player.CutsceneStand();

                    horse.Move(6);
                    horse.Update();
                    if (horse.PositionX > 4900)
                    {
                        timer = 0;
                        game.CurrentChapter.state = Chapter.GameState.Game;
                        game.CurrentChapter.CutsceneState++;
                        UnloadContent();
                        Chapter.effectsManager.AddInGameDialogue("Well done, soldier! Now get inside and let's blow this place to smithereens!", "Napoleon", "Normal", 300);
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
                    game.CurrentChapter.CurrentMap.DrawEnemies(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, Game1.camera.Transform);

                    if (OutsideStoneFort.doorFalling != null && gateFrame < 13)
                        s.Draw(OutsideStoneFort.doorFalling.ElementAt(gateFrame).Value, new Vector2(5000 - 1394, game.CurrentChapter.CurrentMap.MapY), Color.White);
                    
                    if(gateFrame < 4)
                        s.Draw(OutsideStoneFort.door, new Vector2(3161, game.CurrentChapter.CurrentMap.MapY + 352), Color.White);

                    s.End();
                    break;

                case 1:

                    game.CurrentChapter.CurrentMap.DrawBackgroundAndParallax(s);

                    s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    null, null, null, null, camera.Transform);

                    game.CurrentChapter.CurrentMap.Draw(s);
                    game.CurrentChapter.DrawNPC(s);
                    game.CurrentChapter.CurrentMap.DrawEnemies(s);
                    Chapter.effectsManager.DrawPoofs(s);
                    player.Draw(s);
                    OutsideStoneFort.horse.Draw(s);
                    s.End();

                    game.CurrentChapter.CurrentMap.DrawParallaxAndForeground(s);
                    break;
            }
        }
    }
}