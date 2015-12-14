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
    class SnowyStreets : MapClass
    {
        static Portal toTallTaleTerrace;
        static Portal toEbenezersMansion;

        public static Portal ToTallTaleTerrace { get { return toTallTaleTerrace; } }
        public static Portal ToEbenezersMansion { get { return toEbenezersMansion; } }

        Texture2D foreground;

        public SnowyStreets(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1550;
            mapWidth = 3350;
            mapName = "Snowy Streets";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);

            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\SnowyStreets\background"));
            background.Add(Game1.whiteFilter);
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\SnowyStreets\foreground");
            game.NPCSprites["Bell Man"] = content.Load<Texture2D>(@"NPC\Literature\Bell Man");
            Game1.npcFaces["Bell Man"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Bell Man Normal");

            game.NPCSprites["Jason Mysterio"] = content.Load<Texture2D>(@"NPC\TITS\Jason Mysterio");
            Game1.npcFaces["Jason Mysterio"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\JasonMysterioNormal");

            game.NPCSprites["Claire Voyant"] = content.Load<Texture2D>(@"NPC\TITS\Claire Voyant");
            Game1.npcFaces["Claire Voyant"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\ClaireVoyantNormal");

            game.NPCSprites["Ken Speercy"] = content.Load<Texture2D>(@"NPC\TITS\Ken Speercy");
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\KenSpeercyNormal");

            game.NPCSprites["Steve Pantski"] = content.Load<Texture2D>(@"NPC\TITS\Steve Pantski");
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\StevePantskiNormal");
            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Bell Man"] = Game1.whiteFilter;
            Game1.npcFaces["Bell Man"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Jason Mysterio"] = Game1.whiteFilter;
            Game1.npcFaces["Jason Mysterio"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Claire Voyant"] = Game1.whiteFilter;
            Game1.npcFaces["Claire Voyant"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Ken Speercy"] = Game1.whiteFilter;
            Game1.npcFaces["Ken Speercy"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Steve Pantski"] = Game1.whiteFilter;
            Game1.npcFaces["Steve Pantski"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTallTaleTerrace = new Portal(150, platforms[0], "Snowy Streets");
            toEbenezersMansion = new Portal(3100, platforms[0], "Snowy Streets");

            toEbenezersMansion.PortalNameYOffset = -30;
            toEbenezersMansion.FButtonYOffset = -30;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTallTaleTerrace, TallTaleTerrace.ToSnowyStreets);
            portals.Add(toEbenezersMansion, EbenezersMansion.ToSnowyStreets);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (game.ChapterTwo.NPCs["Jason Mysterio"].MapName == "Snowy Streets" && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                s.Draw(game.NPCSprites["Ken Speercy"], new Vector2(2239, mapRec.Y + 821), Color.White);
            }

                s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
