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
    class TutorialMapEight : MapClass
    {
        static Portal toMapSeven;
        static Portal toMapNine;

        public static Portal ToMapSeven { get { return toMapSeven; } }
        public static Portal ToMapNine { get { return toMapNine; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapEight(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Tutorial Map Eight";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map3"));
            game.NPCSprites["Your Friend"] = content.Load<Texture2D>(@"Tutorial\FriendOne");
            Game1.npcFaces["Your Friend"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Tutorial\YourFriend");
        }

        
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Your Friend"] = Game1.whiteFilter;
            Game1.npcFaces["Your Friend"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if(game.CurrentChapter.NPCs["YourFriend"].AcceptedQuest == true && player.EquippedSkills.Count > 0)
                toMapNine.IsUseable = true;

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapSeven = new Portal(160, 630, "TutorialMapEight");
            toMapNine = new Portal(1675, 630, "TutorialMapEight");
            toMapNine.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapSeven, TutorialMapSeven.ToMapEight);
            portals.Add(toMapNine, TutorialMapNine.ToMapEight);
        }
    }
}