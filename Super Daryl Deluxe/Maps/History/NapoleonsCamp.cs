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
    class NapoleonsCamp : MapClass
    {
        static Portal toIncaEmpire;
        static Portal toTent;
        static Portal toBattlefield;
        static Portal toTrenchfootField;
        static Portal toBathroom;
        static Portal toMedicalTent;

        public static Portal ToMedicalTent { get { return toMedicalTent; } }
        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToTrenchfootField { get { return toTrenchfootField; } }
        public static Portal ToBattlefield { get { return toBattlefield; } }
        public static Portal ToIncaEmpire { get { return toIncaEmpire; } }
        public static Portal ToTent { get { return toTent; } }

        Texture2D parallax, parallax2, sky, soldier, outhouseTexture, trojanHorse, romanSoldiers, sign;

        public NapoleonsCamp(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 5000;
            mapName = "Napoleon's Camp";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new LivingLocker(game, new Rectangle(3534, 50, 620, 500)));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\background2"));
            parallax = content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\parallax");
            parallax2 = content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\parallax2");
            sky = content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\sky");
            sign = content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\sign");

            if (game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"] && !game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"])
            {
                trojanHorse = content.Load<Texture2D>(@"Maps\History\TrojanHorseNormal\horse00");
                romanSoldiers = content.Load<Texture2D>(@"Maps\History\InsideGreatWall\blockedGate");
            }
            soldier = content.Load<Texture2D>(@"NPC\History\French Soldier");
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");

            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");
            game.NPCSprites["French Soldier"] = content.Load<Texture2D>(@"NPC\History\French Soldier");
            game.NPCSprites["Napoleon"] = content.Load<Texture2D>(@"NPC\History\Napoleon");
            Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");
            game.NPCSprites["Julius Caesar"] = content.Load<Texture2D>(@"NPC\Party\Julius");
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusHelmet");

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["French Soldier"] = Game1.whiteFilter;
            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Napoleon"] = Game1.whiteFilter;
            Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Julius Caesar"] = Game1.whiteFilter;
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (game.chapterState <= Game1.ChapterState.chapterOne || game.ChapterTwo.ChapterTwoBooleans["canEnterBattlefield"] == false )
            {
                if(game.chapterState <= Game1.ChapterState.chapterOne)
                    toMedicalTent.IsUseable = false;

                toTrenchfootField.IsUseable = false;
            }
            else
            {
                toMedicalTent.IsUseable = true;
                toTrenchfootField.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIncaEmpire = new Portal(50, platforms[0], "Napoleon's Camp");
            toTrenchfootField = new Portal(4800, platforms[0], "Napoleon's Camp");
            toTent = new Portal(2320, platforms[0], "Napoleon's Camp");
            toBattlefield = new Portal(4400, platforms[0], "Napoleon's Camp");
            toBathroom = new Portal(2750, platforms[0], "Napoleon's Camp");
            toMedicalTent = new Portal(1310, platforms[0], "Napoleon's Camp");
           
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


            s.Draw(outhouseTexture, new Vector2(2640, platforms[0].RecY - outhouseTexture.Height), null, Color.White);

            //Battlefield
            s.Draw(soldier, new Vector2(4370, 250), Color.White);
            s.Draw(soldier, new Rectangle(4100, 260, soldier.Width, soldier.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            //No man's land
            if(NapoleonsCamp.ToTrenchfootField.IsUseable == false)
                s.Draw(soldier, new Rectangle(4700, 295, soldier.Width, soldier.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            //Tent
            s.Draw(soldier, new Vector2(2300, 260), Color.White);
            s.Draw(soldier, new Rectangle(2050, 250, soldier.Width, soldier.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            if (game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"] && !game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"])
            {
                s.Draw(trojanHorse, new Rectangle(741, -87, trojanHorse.Width, trojanHorse.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
                s.Draw(romanSoldiers, new Rectangle(538, 350, romanSoldiers.Width, romanSoldiers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                s.Draw(romanSoldiers, new Rectangle(447, 340, romanSoldiers.Width, romanSoldiers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                s.Draw(romanSoldiers, new Rectangle(1948, 319, romanSoldiers.Width, romanSoldiers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                s.Draw(sign, new Vector2(2313, 340), Color.White);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIncaEmpire, IncaVillage.ToCamp);
            portals.Add(toTent, NapoleonsTent.ToNapoleonsCamp);
            portals.Add(toBattlefield, Battlefield.ToNapoleonsCamp);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toMedicalTent, MedicalTent.ToNapoleonsCamp);

            portals.Add(toTrenchfootField, TrenchfootField.ToNapoleonsCamp);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"] && !game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"])
            {
                s.Draw(romanSoldiers, new Rectangle(1272, 436, romanSoldiers.Width, romanSoldiers.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));
            s.Draw(parallax, new Rectangle(0, 0, parallax.Width, parallax.Height), Color.White);
            s.Draw(parallax2, new Vector2(parallax.Width, 0), Color.White);

            s.End();
        }
    }
}
