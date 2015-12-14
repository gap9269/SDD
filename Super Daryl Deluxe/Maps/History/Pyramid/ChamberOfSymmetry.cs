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
    class ChamberOfSymmetry : MapClass
    {
        static Portal toPharaohsKeep;
        static Portal toInnerChamber;

        public static Portal ToInnerChamber { get { return toInnerChamber; } }
        public static Portal ToPharaohsKeep { get { return toPharaohsKeep; } }

        Texture2D foreground, crumblingFloor;

        Platform brokenFloor;
        public ChamberOfSymmetry(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Chamber of Symmetry";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            brokenFloor = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1451,647, 50, 50), false, false, false, 0);
            platforms.Add(brokenFloor);

            interactiveObjects.Add(new Barrel(game, 1285, 480 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .56f, false, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 1617, 480 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .66f, false, Barrel.BarrelType.pyramidPitcher));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfSymmetry\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfSymmetry\foreground");
            crumblingFloor = content.Load<Texture2D>(@"Maps\History\Pyramid\ChamberOfSymmetry\crumblingFloor");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (player.Landing && player.CurrentPlat == brokenFloor && platforms.Contains(brokenFloor))
            {
                player.Landing = false;
                player.CurrentPlat = null;
                player.PositionY += 20;
                player.VelocityY += 10;
                platforms.Remove(brokenFloor);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(1367, 582, 200, 200), 2);
            }

            if (player.VitalRecY > 800)
            {
                if(player.VitalRecX > 1300)
                    ForceToNewMap(new Portal(0, 0, "Chamber of Symmetry"), CentralHallI.fromSymmetryRight);
                else
                    ForceToNewMap(new Portal(0, 0, "Chamber of Symmetry"), CentralHallI.fromSymmetryLeft);

            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toInnerChamber = new Portal(2100, platforms[0], "Chamber of Symmetry");
            toPharaohsKeep = new Portal(50, platforms[0], "Chamber of Symmetry");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (platforms.Contains(brokenFloor))
                s.Draw(crumblingFloor, new Vector2(1390, 627), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsKeep, PharaohsKeep.ToChamberOfSymmetry);
           // portals.Add(toInnerChamber, OuterChamber.ToEntrance);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
