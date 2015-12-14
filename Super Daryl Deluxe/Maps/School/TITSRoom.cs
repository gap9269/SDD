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
    class TITSRoom:MapClass
    {
        static Portal toUpstairs;

        public static Portal ToUpstairs { get { return toUpstairs; } }

        public TITSRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2040;
            mapName = "Paranormal Club";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;


            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

           // TreasureChest janitorsChest = new TreasureChest(Game1.treasureChestSheet, 1800, 624, player, 0, new KeyRing(false), this); //TODO add chest with D&D item in it?
            //treasureChests.Add(janitorsChest);
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());
        }


        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_school_empty");
        }


        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\TITS\background"));

            game.NPCSprites["Jason Mysterio"] = content.Load<Texture2D>(@"NPC\TITS\Jason Mysterio");
            Game1.npcFaces["Jason Mysterio"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\JasonMysterioNormal");

            game.NPCSprites["Claire Voyant"] = content.Load<Texture2D>(@"NPC\TITS\Claire Voyant");
            Game1.npcFaces["Claire Voyant"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\ClaireVoyantNormal");

            game.NPCSprites["Ken Speercy"] = content.Load<Texture2D>(@"NPC\TITS\Ken Speercy");
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\KenSpeercyNormal");

            game.NPCSprites["Steve Pantski"] = content.Load<Texture2D>(@"NPC\TITS\Steve Pantski");
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\StevePantskiNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Jason Mysterio"] = Game1.whiteFilter;
            Game1.npcFaces["Jason Mysterio"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Claire Voyant"] = Game1.whiteFilter;
            Game1.npcFaces["Claire Voyant"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Ken Speercy"] = Game1.whiteFilter;
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Steve Pantski"] = Game1.whiteFilter;
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawMapOverlay(SpriteBatch s)
        {
            base.DrawMapOverlay(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.End();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpstairs = new Portal(0, platforms[0], "Paranormal Club", Portal.DoorType.movement_door_open);
            toUpstairs.FButtonYOffset = -30;
            toUpstairs.PortalNameYOffset = -30;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpstairs, Upstairs.ToTITS);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
