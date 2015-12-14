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
    class TownSquare : MapClass
    {
        static Portal toCityStreets;

        public static Portal ToCityStreets { get { return toCityStreets; } }

        Texture2D foreground, sky, parallax, wall;

        float wallAlpha;

        public TownSquare(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 3350;
            mapName = "Town Square";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;

            Vase bar = new Vase(game, 3235, mapRec.Y + 335 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.clayLarge);
            interactiveObjects.Add(bar);
            Vase bar1 = new Vase(game, 3035, mapRec.Y + 335 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.claySmall);
            interactiveObjects.Add(bar1);
            Vase bar2 = new Vase(game, 2935, mapRec.Y + 335 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.clayMedium);
            interactiveObjects.Add(bar2);

            Vase bar4 = new Vase(game, 2975, mapRec.Y + 915 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.clayLarge);
            interactiveObjects.Add(bar4);
            Vase bar3 = new Vase(game, 2932, mapRec.Y + 928 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.claySmall);
            interactiveObjects.Add(bar3);
            Vase bar5 = new Vase(game, 3236, mapRec.Y + 924 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, false, Vase.VaseType.clayMedium);
            interactiveObjects.Add(bar5);

            Vase bar6 = new Vase(game, 3100, mapRec.Y + 975 + 155, Game1.interactiveObjects["Vase"], true, 2, 3, .04f, true, Vase.VaseType.clayLarge);
            interactiveObjects.Add(bar6);

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Town Square\background"));
            sky = content.Load<Texture2D>(@"Maps\Music\Town Square\sky");
            parallax = content.Load<Texture2D>(@"Maps\Music\Town Square\parallax");
            wall = content.Load<Texture2D>(@"Maps\Music\Town Square\wall");
            foreground = content.Load<Texture2D>(@"Maps\Music\Town Square\foreground");

            game.NPCSprites["Napoleon"] = content.Load<Texture2D>(@"NPC\History\Napoleon");
            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");

            game.NPCSprites["French Soldier"] = content.Load<Texture2D>(@"NPC\History\French Soldier");
            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");

            game.NPCSprites["Town Claysmith"] = content.Load<Texture2D>(@"NPC\Music\Town Claysmith");
            Game1.npcFaces["Town Claysmith"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\Town Claysmith Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Napoleon"] = Game1.whiteFilter;
            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Town Claysmith"] = Game1.whiteFilter;
            Game1.npcFaces["Town Claysmith"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["French Soldier"] = Game1.whiteFilter;
            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCityStreets = new Portal(50, platforms[0], "Town Square");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCityStreets, CityStreets.ToTownSquare);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            //Front door
            if (player.VitalRec.X > 2850 && player.VitalRecY < 100)
            {

                if (wallAlpha > 0)
                    wallAlpha -= .05f;
            }
            else
            {
                if (wallAlpha < 1f)
                    wallAlpha += .05f;
            }

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(wall, new Vector2(0, mapRec.Y), Color.White * wallAlpha);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }
    }
}
