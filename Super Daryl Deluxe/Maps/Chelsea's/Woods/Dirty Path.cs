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
    class DirtyPath : MapClass
    {
        static Portal toCrossroads;
        static Portal toBuilding;

        public static Portal ToBuilding { get { return toBuilding; } }
        public static Portal ToCrossroads { get { return toCrossroads; } }

        public DirtyPath(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Dirty Path";

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
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\Dirty Path"));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCrossroads = new Portal(100, platforms[0], "Dirty Path");
            toBuilding = new Portal(1600, platforms[0], "Dirty Path");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCrossroads, Crossroads.ToPathTwo);
            portals.Add(ToBuilding, SuperSecretDeerBaseAlpha.ToDirtyPath);
        }
    }
}
