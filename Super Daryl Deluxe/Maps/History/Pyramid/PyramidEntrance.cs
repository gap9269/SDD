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
    class PyramidEntrance : MapClass
    {
        static Portal toGreatPyramid;
        static Portal toOuterChamber;
        public static Portal toBathroom;

        public static Portal ToOuterChamber { get { return toOuterChamber; } }
        public static Portal ToGreatPyramid { get { return toGreatPyramid; } }

        Texture2D foreground, outhouse;

        public PyramidEntrance(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Pyramid Entrance";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            Barrel bar = new Barrel(game, 1794, 543 + 155, Game1.interactiveObjects["Barrel"], true, 3, 3, .36f, true, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 1071, 490 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .36f, false, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 1590, 543 + 155, Game1.interactiveObjects["Barrel"], true, 3, 1, .16f, true, Barrel.BarrelType.pyramidUrn);
            interactiveObjects.Add(bar1);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PyramidEntrance\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\DryDesert\foreground");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Chained Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toGreatPyramid = new Portal(50, platforms[0], "Pyramid Entrance");
            toOuterChamber = new Portal(2120, platforms[0], "Pyramid Entrance");
            toBathroom = new Portal(680, platforms[0], "Pyramid Entrance");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Vector2(571, 353), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toGreatPyramid, TheGreatPyramid.ToEntrance);
            portals.Add(toOuterChamber, OuterChamber.ToEntrance);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
