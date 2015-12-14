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
    class Market : MapClass
    {
        static Portal toPlayground;

        public static Portal ToPlayground { get { return toPlayground; } }

        Texture2D foreground, foreground2, sky, door, theaterParallax, streetsParallax;

        public Market(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Art Gallery";

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
            background.Add(content.Load<Texture2D>(@"Maps\Music\Art Gallery\background"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Art Gallery\foreground");        

            game.NPCSprites["Percy von Lugsworth"] = content.Load<Texture2D>(@"NPC\Music\Art Dealer");
            Game1.npcFaces["Percy von Lugsworth"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\Art Dealer Normal");

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Percy von Lugsworth"] = Game1.whiteFilter;
            Game1.npcFaces["Percy von Lugsworth"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toPlayground = new Portal(50, platforms[0], "Art Gallery");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPlayground, ArtistsPlayground.ToMarket);
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
