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
    class DwarvesAndDruidsRoom:MapClass
    {
        static Portal toSouthHall;

        public static Portal ToSouthHall { get { return toSouthHall; } }

        public DwarvesAndDruidsRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2040;
            mapName = "Dwarves & Druids Club";

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
            background.Add(content.Load<Texture2D>(@"Maps\School\DD\background"));

            game.NPCSprites["Weapons Master"] = content.Load<Texture2D>(@"NPC\DD\inventory");
            game.NPCSprites["Skill Sorceress"] = content.Load<Texture2D>(@"NPC\DD\skill");
            game.NPCSprites["Keeper of the Quests"] = content.Load<Texture2D>(@"NPC\DD\journal");
            game.NPCSprites["Saving Instructor"] = content.Load<Texture2D>(@"NPC\DD\save");
            game.NPCSprites["Karma Shaman"] = content.Load<Texture2D>(@"NPC\DD\karma");
            game.NPCSprites["The Magister of Maps"] = content.Load<Texture2D>(@"NPC\DD\maps");

            Game1.npcFaces["Karma Shaman"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Karma");
            Game1.npcFaces["The Magister of Maps"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\MapsKid");
            Game1.npcFaces["Saving Instructor"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Save");
            Game1.npcFaces["Keeper of the Quests"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Journal");
            Game1.npcFaces["Skill Sorceress"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Skill");
            Game1.npcFaces["Weapons Master"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Equipment");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Skill Sorceress"] = Game1.whiteFilter;
            game.NPCSprites["Keeper of the Quests"] = Game1.whiteFilter;

            Game1.npcFaces["Skill Sorceress"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Keeper of the Quests"].faces["Arrogant"] = Game1.whiteFilter;

            game.NPCSprites["Weapons Master"] = Game1.whiteFilter;
            Game1.npcFaces["Weapons Master"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Karma Shaman"] = Game1.whiteFilter;
            Game1.npcFaces["Karma Shaman"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["The Magister of Maps"] = Game1.whiteFilter;
            Game1.npcFaces["The Magister of Maps"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Saving Instructor"] = Game1.whiteFilter;
            Game1.npcFaces["Saving Instructor"].faces["Normal"] = Game1.whiteFilter;
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

            toSouthHall = new Portal(0, platforms[0], "Dwarves & Druids Club", Portal.DoorType.movement_door_open);
            toSouthHall.FButtonYOffset = -35;
            toSouthHall.PortalNameYOffset = -35;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSouthHall, SouthHall.ToDwarvesAndDruids);
        }

        public override void AddNPCs()
        {
            base.AddNPCs();
        }
    }
}
