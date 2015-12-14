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
    class EasternChamber : MapClass
    {
        static Portal toPharaohsKeep;
        static Portal toIndoorGarden;

        public static Portal ToIndoorGarden { get { return toIndoorGarden; } }
        public static Portal ToPharaohsKeep { get { return toPharaohsKeep; } }

        Texture2D foreground;

        public EasternChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Eastern Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            Barrel bar1 = new Barrel(game, 1064, 477 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .36f, false, Barrel.BarrelType.pyramidBirdJar);
            bar1.facingRight = false;
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 947, 477 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .36f, false, Barrel.BarrelType.pyramidBirdJar);
            bar2.facingRight = false;
            interactiveObjects.Add(bar2);

            interactiveObjects.Add(new Barrel(game, 461, 477 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .36f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 578, 477 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .36f, false, Barrel.BarrelType.pyramidBirdJar));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\EasternChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\EasternChamber\foreground");

            game.NPCSprites["The Pyramid Gardener"] = content.Load<Texture2D>(@"NPC\History\The Pyramid Gardener");
            Game1.npcFaces["The Pyramid Gardener"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\The Pyramid Gardener Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["The Pyramid Gardener"] = Game1.whiteFilter;
            Game1.npcFaces["The Pyramid Gardener"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toPharaohsKeep = new Portal(1300, platforms[0], "Eastern Chamber");
            toIndoorGarden = new Portal(120, platforms[0], "Eastern Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsKeep, PharaohsKeep.ToEasternChamber);
            portals.Add(toIndoorGarden, IndoorGarden.ToEasternChamber);
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
