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
    class PharaohsRoad : MapClass
    {
        static Portal toInnerChamber;
        static Portal toPharaohsTrap;
        public static Portal toBathroom;

        public static Portal ToPharaohsTrap { get { return toPharaohsTrap; } }
        public static Portal ToInnerChamber { get { return toInnerChamber; } }

        Texture2D foreground, outhouse;

        public PharaohsRoad(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3500;
            mapName = "Pharaoh's Road";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new LivingLocker(game, new Rectangle(1600, 100, 1200, 500)));

            interactiveObjects.Add(new Barrel(game, 3036, 460 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .36f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 2853, 460 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, .36f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 2655, 460 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .06f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 2946, 517 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, true, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 2761, 517 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, true, Barrel.BarrelType.pyramidUrn));
            interactiveObjects.Add(new Barrel(game, 2566, 517 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, true, Barrel.BarrelType.pyramidUrn));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsRoad\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsRoad\foreground");
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

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toInnerChamber = new Portal(50, platforms[0], "Pharaoh's Road");
            toPharaohsTrap = new Portal(3300, platforms[0], "Pharaoh's Road");
            toBathroom = new Portal(1100, platforms[0], "Pharaoh's Road");
            toBathroom.FButtonYOffset = -15;
            toBathroom.PortalNameYOffset = -15;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(outhouse, new Vector2(1000, 355), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsTrap, PharaohsTrap.ToPharaohsRoad);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toInnerChamber, InnerChamber.ToPharaohsRoad);
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

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
