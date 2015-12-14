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
    class CentralHallIII : MapClass
    {
        static Portal toCentralHallII;
        static Portal toCollapsingRoom;
        public static Portal toSecretPassage;
        public static Portal ToCollapsingRoom { get { return toCollapsingRoom; } }
        public static Portal ToCentralHallII { get { return toCentralHallII; } }

        Texture2D foreground, backgroundOpen;

        Platform door;
        ExplodingFlower flower;
        public CentralHallIII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1230;
            mapWidth = 2000;
            mapName = "Central Hall III";

            mapRec = new Rectangle(0, -300, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1215,200, 350, 50), false, false, false);
            platforms.Add(door);

            Barrel bar2 = new Barrel(game, 714, mapRec.Y + 341 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .06f, false, Barrel.BarrelType.pyramidBirdJar);
            bar2.facingRight = false;
            interactiveObjects.Add(bar2);

            interactiveObjects.Add(new Barrel(game, 1044, mapRec.Y + 341 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .26f, false, Barrel.BarrelType.pyramidBirdJar));
            interactiveObjects.Add(new Barrel(game, 615, mapRec.Y + 341 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .16f, false, Barrel.BarrelType.pyramidUrn));

            flower = new ExplodingFlower(game, 1177, mapRec.Y + 509, false, 300);
            interactiveObjects.Add(flower);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallIII\background"));
            background.Add(Game1.whiteFilter); //Need to keep this to keep the scale of the first image
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallIII\foreground");
            backgroundOpen = content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallIII\backgroundOpen");
            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
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

            if (game.ChapterTwo.ChapterTwoBooleans["secretPassageOpen"] == false)
            {
                if (flower.flowerState == ExplodingFlower.FlowerState.dead)
                {
                    game.ChapterTwo.ChapterTwoBooleans["secretPassageOpen"] = true;
                    game.Camera.ShakeCamera(10, 25);
                    game.ChapterTwo.NPCs["Pyramid Guard 4"].ClearDialogue();
                    game.ChapterTwo.NPCs["Pyramid Guard 4"].Dialogue.Add("My floor! No! What have you done?!");
                }
            }

            else if (platforms.Contains(door))
                platforms.Remove(door);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralHallII = new Portal(90, platforms[0], "Central Hall III");
            toCollapsingRoom = new Portal(1700, platforms[0], "Central Hall III");
            toSecretPassage = new Portal(1700, platforms[1], "Central Hall III");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["secretPassageOpen"] && background[0] != backgroundOpen)
                background[0] = backgroundOpen;
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralHallII, CentralHallII.ToCentralHallIII);
            portals.Add(toCollapsingRoom, CollapsingRoom.ToCentralHallIII);
            portals.Add(toSecretPassage, SecretPassage.toCentralHallIII);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}
