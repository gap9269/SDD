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
    public class SchoolMaps : MapZone
    {
        Game1 game;
        Player player;


        public SchoolMaps(Game1 g, Player p) : base(g)
        {
            game = g;
            player = p;
        }

        public override void LoadEnemyData()
        {
            game.ResetEnemySpriteList();
        }

        public void LoadSchoolZone()
        {
            LoadEnemyData();

            #region Tutorial Maps
            TutorialMapOne tutOne = new TutorialMapOne(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapOne", tutOne);

            TutorialMapTwo tutTwo = new TutorialMapTwo(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapTwo", tutTwo);

            TutorialMapThree tutThree = new TutorialMapThree(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapThree", tutThree);

            TutorialMapFour tutFour = new TutorialMapFour(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapFour", tutFour);

            TutorialMapFive tutFive = new TutorialMapFive(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapFive", tutFive);

            SquigglesTheClown squiggles = new SquigglesTheClown(new List<Texture2D> { }, game, ref player);
            maps.Add("SquigglesTheClown", squiggles);

            TutorialMapSix tutSix = new TutorialMapSix(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapSix", tutSix);

            TutorialMapSeven tutSeven = new TutorialMapSeven(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapSeven", tutSeven);

            TutorialMapEight tutEight = new TutorialMapEight(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapEight", tutEight);

            TutorialMapNine tutNine = new TutorialMapNine(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapNine", tutNine);

            TutorialMapTen tutTen = new TutorialMapTen(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapTen", tutTen);

            FruitGarden fruitGarden = new FruitGarden(new List<Texture2D> { }, game, ref player);
            maps.Add("FruitGarden", fruitGarden);

            TutorialMapEleven tutEleven = new TutorialMapEleven(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapEleven", tutEleven);

            TutorialMapTwelve tutTwelve = new TutorialMapTwelve(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapTwelve", tutTwelve);

            TutorialMapThirteen tutThirteen = new TutorialMapThirteen(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapThirteen", tutThirteen);

            TutorialMapFourteen tutFourteen = new TutorialMapFourteen(new List<Texture2D> { }, game, ref player);
            maps.Add("TutorialMapFourteen", tutFourteen);

            TutorialCredits theCredits = new TutorialCredits(new List<Texture2D> { }, game, ref player);
            maps.Add("TheCredits", theCredits);
            #endregion

            #region School

            List<Texture2D> bathroomBack = new List<Texture2D>();
            Bathroom bathroom = new Bathroom(bathroomBack, game, ref player);
            maps.Add("Bathroom", bathroom);

            Upstairs upstairs = new Upstairs(new List<Texture2D>(), game, ref player);
            maps.Add("Upstairs", upstairs);

            List<Texture2D> mainLobbyBack = new List<Texture2D>();
            MainLobby mainLobby = new MainLobby(mainLobbyBack, game, ref player);
            maps.Add("MainLobby", mainLobby);

            List<Texture2D> northHallBack = new List<Texture2D>();
            NorthHall northHall = new NorthHall(northHallBack, game, ref player);
            maps.Add("NorthHall", northHall);

            List<Texture2D> southHallBack = new List<Texture2D>();
            southHallBack.Add(content.Load<Texture2D>(@"Maps\MapTest3"));
            SouthHall southHall = new SouthHall(northHallBack, game, ref player);
            maps.Add("SouthHall", southHall);

            List<Texture2D> quadBack = new List<Texture2D>();
            TheQuad theQuad = new TheQuad(quadBack, game, ref player);
            maps.Add("TheQuad", theQuad);

            List<Texture2D> farBack = new List<Texture2D>();
            TheFarSide theFarSide = new TheFarSide(farBack, game, ref player);
            maps.Add("TheFarSide", theFarSide);

            List<Texture2D> artBack = new List<Texture2D>();
            ArtHall eastHall = new ArtHall(artBack, game, ref player);
            maps.Add("EastHall", eastHall);

            List<Texture2D> janitorBack = new List<Texture2D>();
            JanitorCloset janitorsCloset = new JanitorCloset(janitorBack, game, ref player);
            maps.Add("JanitorsCloset", janitorsCloset);

            SideHall sideHall = new SideHall(janitorBack, game, ref player);
            maps.Add("SideHall", sideHall);

            Cafeteria cafeteria = new Cafeteria(janitorBack, game, ref player);
            maps.Add("Cafeteria", cafeteria);

            Freezer freezer = new Freezer(janitorBack, game, ref player);
            maps.Add("Freezer", freezer);

            Basement basement = new Basement(janitorBack, game, ref player);
            maps.Add("Basement", basement);

            DDRoom ddRoom = new DDRoom(janitorBack, game, ref player);
            maps.Add("DwarvesAndDruidsClub", ddRoom);

            GeneratorRoom genRoom = new GeneratorRoom(janitorBack, game, ref player);
            maps.Add("GeneratorRoom", genRoom);

            Dungeon dungeon = new Dungeon(janitorBack, game, ref player);
            maps.Add("Dungeon", dungeon);

            GymLobby gymLobby = new GymLobby(janitorBack, game, ref player);
            maps.Add("GymLobby", gymLobby);

            Roof roof = new Roof(quadBack, game, ref player);
            maps.Add("TheRoof", roof);

            #endregion

            #region Science Dungeon

            List<Texture2D> science101Back = new List<Texture2D>();
            ScienceIntroRoom scienceIntro = new ScienceIntroRoom(science101Back, game, ref player);
            maps.Add("IntroToScience", scienceIntro);

            Science101 science101 = new Science101(science101Back, game, ref player);
            maps.Add("Science101", science101);

            List<Texture2D> science102Back = new List<Texture2D>();
            Science102 science102 = new Science102(science102Back, game, ref player);
            maps.Add("Science102", science102);

            Science103 science103 = new Science103(science102Back, game, ref player);
            maps.Add("Science103", science103);

            Science104 science104 = new Science104(science102Back, game, ref player);
            maps.Add("Science104", science104);

            Science105 science105 = new Science105(science102Back, game, ref player);
            maps.Add("Science105", science105);

            ScienceChallengeRoomI sciChal = new ScienceChallengeRoomI(science102Back, game, ref player);
            maps.Add("ScienceChallengeRoomI", sciChal);
            #endregion

            #region Music Room
            MusicIntroRoom musicIntro = new MusicIntroRoom(new List<Texture2D>(), game, ref player);
            maps.Add("IntroToMusic", musicIntro);
            #endregion

            #region Upper Vents
            List<Texture2D> ventsBack = new List<Texture2D>();
            ventsBack.Add(content.Load<Texture2D>(@"Maps\hall"));

            UpperVents1 uVents1 = new UpperVents1(ventsBack, game, ref player);
            maps.Add("UpperVentsI", uVents1);

            UpperVentsII uVentsII = new UpperVentsII(ventsBack, game, ref player);
            maps.Add("UpperVentsII", uVentsII);

            List<Texture2D> vents3Back = new List<Texture2D>();
            vents3Back.Add(content.Load<Texture2D>(@"Maps\MapTest3"));
            vents3Back.Add(content.Load<Texture2D>(@"Maps\MapTest3"));
            UpperVentsIII uVentsIII = new UpperVentsIII(vents3Back, game, ref player);
            maps.Add("UpperVentsIII", uVentsIII);

            UpperVentsIV uVentsIV = new UpperVentsIV(ventsBack, game, ref player);
            maps.Add("UpperVentsIV", uVentsIV);

            UpperVentsV uVentsV = new UpperVentsV(ventsBack, game, ref player);
            maps.Add("UpperVentsV", uVentsV);

            UpperVentsVI uVentsVI = new UpperVentsVI(ventsBack, game, ref player);
            maps.Add("UpperVentsVI", uVentsVI);

            Furnace furnace = new Furnace(ventsBack, game, ref player);
            maps.Add("FurnaceRoom", furnace);

            UpperVentsChallengeRoom uVentsChall = new UpperVentsChallengeRoom(ventsBack, game, ref player);
            maps.Add("UpperVentsChallengeRoom", uVentsChall);

            SideVentsI sVentsI = new SideVentsI(ventsBack, game, ref player);
            maps.Add("SideVentsI", sVentsI);

            SideVentsII sVentsII = new SideVentsII(ventsBack, game, ref player);
            maps.Add("SideVentsII", sVentsII);

            PrincessLockerRoom princessRoom = new PrincessLockerRoom(ventsBack, game, ref player);
            maps.Add("PrincessLockerRoom", princessRoom);
            #endregion

            #region Chelsea's
            List<Texture2D> partyBack = new List<Texture2D>();
            TheParty theParty = new TheParty(partyBack, game, ref player);
            maps.Add("TheParty", theParty);

            List<Texture2D> outsidePartyBack = new List<Texture2D>();
            OutsideTheParty outsideTheParty = new OutsideTheParty(outsidePartyBack, game, ref player);
            maps.Add("OutsidetheParty", outsideTheParty);

            List<Texture2D> behindPartyBack = new List<Texture2D>();
            BehindTheParty behindTheParty = new BehindTheParty(behindPartyBack, game, ref player);
            maps.Add("BehindtheParty", behindTheParty);

            List<Texture2D> theGoatsBack = new List<Texture2D>();
            TheGoats theGoats = new TheGoats(theGoatsBack, game, ref player);
            maps.Add("TheGoats", theGoats);

            List<Texture2D> poolBack = new List<Texture2D>();
            ChelseasPool chelseasPool = new ChelseasPool(poolBack, game, ref player);
            maps.Add("ChelseasPool", chelseasPool);

            List<Texture2D> houseBack = new List<Texture2D>();
            TreeHouse treeHouse = new TreeHouse(houseBack, game, ref player);
            maps.Add("TreeHouse", treeHouse);

            List<Texture2D> chelseasFieldBack = new List<Texture2D>();
            ChelseasField chelseasField = new ChelseasField(chelseasFieldBack, game, ref player);
            maps.Add("ChelseasField", chelseasField);

            List<Texture2D> spookyBack = new List<Texture2D>();
            SpookyField spookyField = new SpookyField(spookyBack, game, ref player);
            maps.Add("SpookyField", spookyField);

            List<Texture2D> anotherSpookyBack = new List<Texture2D>();
            AnotherSpookyField anotherSpookyField = new AnotherSpookyField(anotherSpookyBack, game, ref player);
            maps.Add("AnotherSpookyField", anotherSpookyField);

            List<Texture2D> tutBack = new List<Texture2D>();
            InBetweenField inBetweenField = new InBetweenField(tutBack, game, ref player);
            maps.Add("InBetweenField", inBetweenField);

            List<Texture2D> workersBack = new List<Texture2D>();
            WorkersField workersField = new WorkersField(workersBack, game, ref player);
            maps.Add("Worker'sField", workersField);

            IrrigationCanal irrigationCanal = new IrrigationCanal(new List<Texture2D>() { }, game, ref player);
            maps.Add("IrrigationCanal", irrigationCanal);

            TheWoods theWoods = new TheWoods(new List<Texture2D>() { }, game, ref player);
            maps.Add("TheWoods", theWoods);

            DeepWoods deepWoods = new DeepWoods(new List<Texture2D>() { }, game, ref player);
            maps.Add("DeepWoods", deepWoods);

            List<Texture2D> barnBack = new List<Texture2D>();
            OokySpookyBarn ookySpookyBarn = new OokySpookyBarn(barnBack, game, ref player);
            maps.Add("OokySpookyBarn", ookySpookyBarn);

            List<Texture2D> shedBack = new List<Texture2D>();
            OldShed oldShed = new OldShed(shedBack, game, ref player);
            maps.Add("OldShed", oldShed);

            List<Texture2D> hutBack = new List<Texture2D>();
            TrollsHut trollsHut = new TrollsHut(shedBack, game, ref player);
            maps.Add("TrollsHut", trollsHut);

            MysteriouslyPeacefulClearing mysteriouslyPeacefulClearing = new MysteriouslyPeacefulClearing(new List<Texture2D>() { }, game, ref player);
            maps.Add("MysteriouslyPeacefulClearing", mysteriouslyPeacefulClearing);

            Crossroads crossroads = new Crossroads(new List<Texture2D>() { }, game, ref player);
            maps.Add("Crossroads", crossroads);

            WoodsyRiver woodsyRiver = new WoodsyRiver(new List<Texture2D>() { }, game, ref player);
            maps.Add("WoodsyRiver", woodsyRiver);

            DirtyPath dirtyPath = new DirtyPath(new List<Texture2D>() { }, game, ref player);
            maps.Add("DirtyPath", dirtyPath);

            SuperSecretDeerBaseAlpha superSecretDeerBaseAlpha = new SuperSecretDeerBaseAlpha(new List<Texture2D>() { }, game, ref player);
            maps.Add("SuperSecretDeerBaseAlpha", superSecretDeerBaseAlpha);

            EmptyField emptyField = new EmptyField(new List<Texture2D>() { }, game, ref player);
            maps.Add("EmptyField", emptyField);

            HiddenPath hiddenPath = new HiddenPath(new List<Texture2D>() { }, game, ref player);
            maps.Add("HiddenPath", hiddenPath);


            DeerShack deerShack = new DeerShack(new List<Texture2D>() { }, game, ref player);
            maps.Add("DeerShack", deerShack);


            List<Texture2D> silo1Back = new List<Texture2D>();
            //silo1Back.Add(whiteFilter);
            Silo1 silo1 = new Silo1(behindPartyBack, game, ref player);
            maps.Add("Silo1", silo1);

            List<Texture2D> silo2Back = new List<Texture2D>();
            //silo2Back.Add(whiteFilter);
            Silo2 silo2 = new Silo2(behindPartyBack, game, ref player);
            maps.Add("Silo2", silo2);

            List<Texture2D> silo3Back = new List<Texture2D>();
            //silo3Back.Add(whiteFilter);
            Silo3 silo3 = new Silo3(behindPartyBack, game, ref player);
            maps.Add("Silo3", silo3);

            List<Texture2D> silo4Back = new List<Texture2D>();
            //silo4Back.Add(whiteFilter);
            Silo4 silo4 = new Silo4(behindPartyBack, game, ref player);
            maps.Add("Silo4", silo4);
            #endregion

            #region History
            OutsideStoneFort outsideEnemyCamp = new OutsideStoneFort(new List<Texture2D>(), game, ref player);
            maps.Add("OutsideEnemyCamp", outsideEnemyCamp);

            StoneFortCentral stoneFortCentral = new StoneFortCentral(new List<Texture2D>(), game, ref player);
            maps.Add("StoneFort-Central", stoneFortCentral);

            StoneFortWest stoneFortWest = new StoneFortWest(new List<Texture2D>(), game, ref player);
            maps.Add("StoneFort-West", stoneFortWest);

            StoneFortEast stoneFortEast = new StoneFortEast(new List<Texture2D>(), game, ref player);
            maps.Add("StoneFort-East", stoneFortEast);
            #endregion

            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}
