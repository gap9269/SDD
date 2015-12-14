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
    public class Bathroom : MapClass
    {
        Rectangle saveSpot;
        Rectangle frec;
        int timerAfterSave = 0;

        static Portal toLastMap;
        static Portal lastMapPortal;

        public static Portal ToLastMap { get { return toLastMap; } set { toLastMap = value; } }
        public static Portal LastMapPortal { get { return lastMapPortal; } set { lastMapPortal = value; } }

        public Bathroom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Bathroom";

            saveSpot = new Rectangle(915, 350, 100, 200);

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            frec = new Rectangle(saveSpot.X + 45 -
   43 / 2, saveSpot.Y - 60, 43,
    65);

            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\Bathroom\Bathroom"));

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Saving Instructor"] = content.Load<Texture2D>(@"NPC\DD\save");
                Game1.npcFaces["Saving Instructor"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Save");
            }

            SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_bathroom");
            SoundEffectInstance amb = am.CreateInstance();
            amb.IsLooped = true;
            Sound.ambience.Add("ambience_bathroom", amb);
        }

        public override void UnloadNPCContent()
        {
            Sound.UnloadAmbience();

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Saving Instructor"] = Game1.whiteFilter;
                Game1.npcFaces["Saving Instructor"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void LeaveMap()
        {
            base.LeaveMap();
        }

        public override void PlayBackgroundMusic()
        {
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_bathroom");
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            //if (!game.MapBooleans.tutorialMapBooleans["TutorialSaved"])
            //{
            //    Chapter.effectsManager.AddToolTipWithImage("Beautiful! What a pleasant area. Press 'F' at \nthat stall over there to save your game!", 300, 30, game.ChapterTwo.associateOneTex);
            //}

            portals.Clear();
            portals.Add(toLastMap, lastMapPortal);

            if (timerAfterSave > 0)
                timerAfterSave--;

            if (player.VitalRec.Intersects(saveSpot) && timerAfterSave == 0 && game.chapterState != Game1.ChapterState.demo) 
            {
                if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                    Chapter.effectsManager.AddFButton(frec);

                if((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                {
                    ////TUTORIAL TOOLTIP
                    //if (!game.MapBooleans.tutorialMapBooleans["TutorialSaved"])
                    //{
                    //    game.MapBooleans.tutorialMapBooleans["TutorialSaved"] = true;
                    //    Chapter.effectsManager.AddToolTipWithImage("Saving is important. If you lose all of your \nhealth you will have to start over from the \nlast time you saved. Keep track of your health \nand experience up here!", 0, 100, game.ChapterTwo.associateOneTex);
                    //}

                    //SAVE ALL MAP DATA
                    Game1.schoolMaps.SaveMapDataToWrapper();

                    Sound.PlaySoundInstance(Sound.SoundNames.popup_save_game);

                    game.SaveLoadManager.InitiateSave();
                    timerAfterSave = 180;
                    player.Health = player.realMaxHealth;
                }
            }

            else
            {
                if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                    Chapter.effectsManager.fButtonRecs.Remove(frec);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toLastMap = new Portal(50, platforms[0], "Bathroom", Portal.DoorType.movement_door_open);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toLastMap, lastMapPortal);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            /*
            if (player.VitalRec.Intersects(saveSpot) && timerAfterSave == 0)
                s.DrawString(game.Font, "To Save", new Vector2(saveSpot.X + 60, saveSpot.Y), Color.Black);*/

            if(timerAfterSave > 130)
                Game1.OutlineFont(Game1.font, s, "Game Saved", 1, saveSpot.X, saveSpot.Y - 30, Color.Black, Color.White);
        }
    }
}
