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
    public static class GenghisCaesarInsults
    {
        public static List<NameDialogue> nameAndDialogueList;
        public static int cooldown;

        static List<int> convosUnused;

        public struct NameDialogue
        {
            public List<String> name;
            public List<String> dialogue;

            public void Add(String n, String d)
            {
                name.Add(n);
                dialogue.Add(d);
            }
        }

        public static void InitializeInsults()
        {
            nameAndDialogueList = new List<NameDialogue>();

            NameDialogue convoOne = new NameDialogue();
            convoOne.name = new List<string>();
            convoOne.dialogue = new List<string>();
            convoOne.Add("Genghis", "Why don't you come down here and face me like a REAL man? You hide behind your wall like a frightened rabbit!");
            convoOne.Add("Julius Caesar", "Why don't you come in and get me if you're so tough, huh?");
            convoOne.Add("Genghis", "Hrrrraauugghh! You will pay, Caesar! YOU WILL PAY!");

            NameDialogue convoTwo = new NameDialogue();
            convoTwo.name = new List<string>();
            convoTwo.dialogue = new List<string>();
            convoTwo.Add("Julius Caesar", "Don't you have anything better to do? Like ruining the lives of millions through your senseless pillaging?");
            convoTwo.Add("Genghis", "Oh no, for now you have my full attention, you spineless cretin.");
            convoTwo.Add("Julius Caesar", "Mmmm, yes. How lucky I am.");

            NameDialogue convoThree = new NameDialogue();
            convoThree.name = new List<string>();
            convoThree.dialogue = new List<string>();
            convoThree.Add("Julius Caesar", "*Yaaaaaaaawwwwnnn* Phew, I sure am tired from all of this safely standing around on my wall.");
            convoThree.Add("Julius Caesar", "I do think I'll retire to my warm, comfy bed soon. I hope you enjoy the cold ground, savage.");
            convoThree.Add("Genghis", "I will stand here through the next three moons if that is what it takes to reclaim what you owe me, Caesar.");

            NameDialogue convoFour = new NameDialogue();
            convoFour.name = new List<string>();
            convoFour.dialogue = new List<string>();
            convoFour.Add("Julius Caesar", "Ghengis Kahn? more like Ghengis Khan't.");
            convoFour.Add("Genghis", "...");

            NameDialogue convoFive = new NameDialogue();
            convoFive.name = new List<string>();
            convoFive.dialogue = new List<string>();
            convoFive.Add("Genghis", "When I get my hands on you, Caesar, I WILL RIP YOUR HEAD FROM YOUR SHOULDERS AND TAKE BACK--");
            convoFive.Add("Julius Caesar", "Oh, what? What's that? I cannot hear you with this large stone wall between us.");

            nameAndDialogueList.Add(convoOne);
            nameAndDialogueList.Add(convoTwo);
            nameAndDialogueList.Add(convoThree);
            nameAndDialogueList.Add(convoFour);
            nameAndDialogueList.Add(convoFive);

            cooldown = Game1.randomNumberGen.Next(180, 500);

            convosUnused = new List<int>();

            for (int i = 0; i < nameAndDialogueList.Count; i++)
                convosUnused.Add(i);
        }

        public static void Update()
        {
            if (cooldown > 0)
                cooldown--;
            else
            {
                //Pick a random conversation
                int convoPicked = Game1.randomNumberGen.Next(0, convosUnused.Count);
                NameDialogue conversation = nameAndDialogueList[convosUnused[convoPicked]];

                //Add all of them to the ingame dialogue list
                for (int i = 0; i < conversation.name.Count; i++)
                {
                    if (conversation.name[i] == "Julius Caesar")
                        Chapter.effectsManager.AddInGameDialogue(conversation.dialogue[i], conversation.name[i], "Helmet", 300);
                    else
                        Chapter.effectsManager.AddInGameDialogue(conversation.dialogue[i], conversation.name[i], "Normal", 300);

                }

                //Remove the picked convo from the list on unused ones and reset the cooldown
                convosUnused.RemoveAt(convoPicked);
                cooldown = Game1.randomNumberGen.Next(180 + (250 * conversation.name.Count), 700 + (180 * conversation.name.Count));

            }

            //Re-add all of the conversations after they run out
            if (convosUnused.Count <= 0)
            {
                for (int i = 0; i < nameAndDialogueList.Count; i++)
                    convosUnused.Add(i);
            }

        }


    }

    class TheGreatWall : MapClass
    {
        static Portal toMongolCamp;
        public static Portal toBehindGreatWall;

        public static Portal ToMongolCamp { get { return toMongolCamp; } }

        Texture2D foreground, foreground2, sky, parallax, clouds, clouds2, gate, gateWall, soldiers, caesar, mongolSoldiers, foregroundSoldier;

        float cloudPosX = 0;

        public int caesarPosY = 0;

        public float gatePosY = 0;

        public TheGreatWall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1400;
            mapWidth = 5000;
            mapName = "The Great Wall";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);

            enemyAmount = 3;
            zoomLevel = .64f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Tree Ent", 0);

            GenghisCaesarInsults.InitializeInsults();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Great Wall\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Great Wall\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\Great Wall\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\Great Wall\parallax");
            clouds = content.Load<Texture2D>(@"Maps\History\Great Wall\clouds");
            foreground = content.Load<Texture2D>(@"Maps\History\Great Wall\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\Great Wall\foreground2");
            gate = content.Load<Texture2D>(@"Maps\History\Great Wall\gate");
            gateWall = content.Load<Texture2D>(@"Maps\History\Great Wall\gateWall");
            soldiers = content.Load<Texture2D>(@"Maps\History\Great Wall\soldiers");
            caesar = content.Load<Texture2D>(@"Maps\History\Great Wall\caesar");
            mongolSoldiers = content.Load<Texture2D>(@"Maps\History\Great Wall\mongolSoldiers");
            foregroundSoldier = content.Load<Texture2D>(@"Maps\History\OutsideFort\genghisSoldier");

            game.NPCSprites["Genghis"] = content.Load<Texture2D>(@"NPC\History\Genghis");
            Game1.npcFaces["Genghis"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\GenghisNormal");
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusHelmet");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.TreeEntEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Tree Ent"] < enemyAmount)
            {
                TreeEnt en = new TreeEnt(pos, "Tree Ent", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(Game1.randomNumberGen.Next(-354, 0), monsterY);

                en.TimeBeforeSpawn = 150;
                en.Hostile = true;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Tree Ent"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Genghis"] = Game1.whiteFilter;

            Game1.npcFaces["Genghis"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Julius Caesar"].faces["Helmet"] = Game1.whiteFilter;

        }


        public override void Update()
        {
            base.Update();

            if (game.Camera.center.Y > 470)
            {
                game.Camera.center.Y = 471;
            }

            cloudPosX += 1f;

            if (cloudPosX >= mapWidth + clouds.Width)
                cloudPosX = -clouds.Width - 100;

            if (!game.ChapterTwo.ChapterTwoBooleans["entsReleasedScenePlayed"])
                GenghisCaesarInsults.Update();

            if (game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] && !game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
                RespawnGroundEnemies();

            if (game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] && enemiesInMap.Count > 0)
            {
                enemiesInMap.Clear();
                ResetEnemyNamesAndNumberInMap();
            }
            if (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] && toBehindGreatWall.IsUseable)
                toBehindGreatWall.IsUseable = false;
            else if(!toBehindGreatWall.IsUseable)
                toBehindGreatWall.IsUseable = true;

        }

        public override void SetPortals()
        {
            base.SetPortals();
            toMongolCamp = new Portal(4800, platforms[0], "The Great Wall");
            toBehindGreatWall = new Portal(400, platforms[0], "The Great Wall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMongolCamp, MongolCamp.ToGreatWall);
            portals.Add(toBehindGreatWall, BehindTheGreatWall.toTheGreatWall);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
                s.Draw(gate, new Vector2(0, mapRec.Y + gatePosY), Color.White);
            s.Draw(gateWall, new Vector2(0, mapRec.Y), Color.White);
            if (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"])
            {
                s.Draw(soldiers, new Vector2(1195, mapRec.Y + 100 + caesarPosY), Color.White);
                s.Draw(caesar, new Vector2(1195, mapRec.Y + 100 + caesarPosY), Color.White);
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] || (game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] && !game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"]))
            {
                s.Draw(mongolSoldiers, new Vector2(1703, mapRec.Y + 1030), Color.White);

            }


        }
        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (!game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] || (game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] && !game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"]))
            {
                s.Draw(foregroundSoldier, new Vector2(4289, mapRec.Y + 20), Color.White);
            }

            s.Draw(foreground, new Vector2(0, mapRec.Y + 100), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);

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
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, sky.Height), Color.White);

            s.Draw(clouds, new Vector2(cloudPosX, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1, this, game));
            s.Draw(parallax, new Vector2(mapWidth - parallax.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}
