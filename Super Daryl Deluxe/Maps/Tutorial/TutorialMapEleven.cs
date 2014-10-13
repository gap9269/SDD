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
    class TutorialMapEleven : MapClass
    {
        static Portal toMapTen;
        static Portal toMapTwelve;

        public static Portal ToMapTen { get { return toMapTen; } }
        public static Portal ToMapTwelve { get { return toMapTwelve; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapEleven(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Tutorial Map Eleven";

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

            if (game.CurrentQuests.Count == 0 && !toMapTwelve.IsUseable)
            {
                toMapTwelve.IsUseable = true;
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapTen = new Portal(160, 630, "TutorialMapEleven");
            toMapTwelve = new Portal(1675, 630, "TutorialMapEleven");
            toMapTwelve.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapTen, TutorialMapTen.ToMapEleven);
            portals.Add(toMapTwelve, TutorialMapTwelve.ToMapEleven);
        }
    }
}