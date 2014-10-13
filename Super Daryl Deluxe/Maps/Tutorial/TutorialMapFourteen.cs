using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class TutorialMapFourteen : MapClass
    {
        static Portal toMapThirteen;
        static Portal toCredits;

        public static Portal ToMapThirteen { get { return toMapThirteen; } }
        public static Portal ToCredits { get { return toCredits; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public static StoneCold steveAustin;

        static Random randomDialogue;

        List<String> lawyerDialogue;

        int tipTimer, tip2Timer;

        public TutorialMapFourteen(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Tutorial Map Fourteen";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 2;

            lawyerDialogue = new List<string>();
            lawyerDialogue.Add("We will sue you!");
            lawyerDialogue.Add("Consider yourself sued, boy.");
            lawyerDialogue.Add("Yep. This is a classic case of us suing you.");
            lawyerDialogue.Add("We'll see you in court.");
            lawyerDialogue.Add("Ladies and Gentlemen of the jury, we're about to open a can of whoop ass.");
            lawyerDialogue.Add("The court fee for this fight is...death");
            lawyerDialogue.Add("We swear to kick your ass and nothing but your ass");
            lawyerDialogue.Add("We're exercising our 6th amendment rights to give you a speedy ass-kicking!");

            randomDialogue = new Random();

            Barrel b1 = new Barrel(game, 1700, 680, Game1.interactiveObjects["BadBarrel"], true, 8, 3, 0, false, 0);
            Barrel b2 = new Barrel(game, 1470, 680, Game1.interactiveObjects["BadBarrel"], true, 8, 3, 0, false, 0);
            Barrel b3 = new Barrel(game, 300, 680, Game1.interactiveObjects["BadBarrel"], true, 8, 3, 0, false, 0);

            interactiveObjects.Add(b1);
            interactiveObjects.Add(b2);
            interactiveObjects.Add(b3);

            steveAustin = new StoneCold(new Vector2(1200, 655 - 270 -1), "STONE COLD STEVE AUSTIN", game, ref player, this);
            steveAustin.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(0, 0, 50, 720), false, false, false));
            steveAustin.Boundaries.Add(new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1950, 0, 50, 720), false, false, false));
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("AustinName", this.content.Load<Texture2D>(@"Bosses\DemoAustin\austinName"));
            game.EnemySpriteSheets.Add("Garden Beast", content.Load<Texture2D>(@"Tutorial\EnemieSheet"));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map14"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\Map14Fore");
            Game1.npcFaces["Demo Danny"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Tutorial\DemoDanny");
            Game1.npcFaces["Copyright Lawyer"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Tutorial\Lawyer");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Demo Danny"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Copyright Lawyer"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void RespawnGroundEnemies()
        {

            base.RespawnGroundEnemies();

            Lawyer en = new Lawyer(pos, "Garden Beast", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }

        public override void Update()
        {
            base.Update();

            //Spawn lawyers
            if (steveAustin.SpawningLawyers && enemiesInMap.Count < 2)
            {
                Lawyer en = new Lawyer(new Vector2(300, platforms[0].Rec.Y - 228 - 1) , "Garden Beast", game, ref player, this);
                enemiesInMap.Add(en);

                Lawyer en1 = new Lawyer(new Vector2(1700, platforms[0].Rec.Y - 228 - 1), "Garden Beast", game, ref player, this);
                enemiesInMap.Add(en1);

                int dialogueNum = randomDialogue.Next(5);

                Chapter.effectsManager.AddInGameDialogue(lawyerDialogue[dialogueNum], "Copyright Lawyer", "Normal", 120);

                steveAustin.SpawningLawyers = false;
            }

            if (steveAustin.VelocityY < -20)
            {
                if (!game.MapBooleans.tutorialMapBooleans["lawyerTip"])
                {
                    game.MapBooleans.tutorialMapBooleans["lawyerTip"] = true;
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][46], 600, 100, game.ChapterTwo.associateOneTex);
                }
            }

            if (tip2Timer < 181 && game.MapBooleans.tutorialMapBooleans["lawyerTip"])
            {
                tip2Timer++;

                if (tip2Timer == 180)
                {
                    Chapter.effectsManager.RemoveToolTip();
                }
            }


            if (game.CurrentChapter.BossFight == false && game.MapBooleans.tutorialMapBooleans["lawyerTip"])
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][47], 600, 100, game.ChapterTwo.associateOneTex);
                toCredits.IsUseable = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapThirteen = new Portal(160, 630, "TutorialMapFourteen");
            toMapThirteen.IsUseable = false;
            toCredits = new Portal(1775, 630, "TutorialMapFourteen");
            toCredits.IsUseable = false;
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapThirteen, TutorialMapThirteen.ToMapFourteen);
            portals.Add(toCredits, TutorialCredits.ToMapFourteen);
        }
    }
}