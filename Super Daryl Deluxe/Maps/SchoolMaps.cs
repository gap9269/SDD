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
            maps.Add("Main Lobby", mainLobby);

            List<Texture2D> northHallBack = new List<Texture2D>();
            NorthHall northHall = new NorthHall(northHallBack, game, ref player);
            maps.Add("North Hall", northHall);

            List<Texture2D> southHallBack = new List<Texture2D>();
            southHallBack.Add(content.Load<Texture2D>(@"Maps\MapTest3"));
            SouthHall southHall = new SouthHall(northHallBack, game, ref player);
            maps.Add("South Hall", southHall);

            List<Texture2D> quadBack = new List<Texture2D>();
            TheQuad theQuad = new TheQuad(quadBack, game, ref player);
            maps.Add("The Quad", theQuad);

            List<Texture2D> farBack = new List<Texture2D>();
            TheFarSide theFarSide = new TheFarSide(farBack, game, ref player);
            maps.Add("The Far Side", theFarSide);

            List<Texture2D> artBack = new List<Texture2D>();
            EastHall eastHall = new EastHall(artBack, game, ref player);
            maps.Add("East Hall", eastHall);

            List<Texture2D> janitorBack = new List<Texture2D>();
            JanitorCloset janitorsCloset = new JanitorCloset(janitorBack, game, ref player);
            maps.Add("Janitor's Closet", janitorsCloset);

            Kitchen kitchen = new Kitchen(janitorBack, game, ref player);
            maps.Add("Kitchen", kitchen);

            GymLobby gymLobby = new GymLobby(janitorBack, game, ref player);
            maps.Add("Gym Lobby", gymLobby);

            DwarvesAndDruidsRoom dwarvesAndDruidsRoom = new DwarvesAndDruidsRoom(quadBack, game, ref player);
            maps.Add("Dwarves & Druids Club", dwarvesAndDruidsRoom);

            TITSRoom titsRoom = new TITSRoom(quadBack, game, ref player);
            maps.Add("Paranormal Club", titsRoom);

            #endregion

            #region Science Dungeon

            List<Texture2D> science101Back = new List<Texture2D>();
            ScienceIntroRoom scienceIntro = new ScienceIntroRoom(science101Back, game, ref player);
            maps.Add("Intro to Science", scienceIntro);

            Science101 science101 = new Science101(science101Back, game, ref player);
            maps.Add("Science 101", science101);

            List<Texture2D> science102Back = new List<Texture2D>();
            Science102 science102 = new Science102(science102Back, game, ref player);
            maps.Add("Science 102", science102);

            Science103 science103 = new Science103(science102Back, game, ref player);
            maps.Add("Science 103", science103);

            Science104 science104 = new Science104(science102Back, game, ref player);
            maps.Add("Science 104", science104);

            Science105 science105 = new Science105(science102Back, game, ref player);
            maps.Add("Science 105", science105);
            #endregion

            #region Music Room
            MusicIntroRoom musicIntro = new MusicIntroRoom(new List<Texture2D>(), game, ref player);
            maps.Add("Intro To Music", musicIntro);

            EntranceHall entranceHall = new EntranceHall(new List<Texture2D>(), game, ref player);
            maps.Add("Entrance Hall", entranceHall);

            TheStage stage = new TheStage(science101Back, game, ref player);
            maps.Add("The Stage", stage);

            SecondFloor secondFloor = new SecondFloor(science101Back, game, ref player);
            maps.Add("Second Floor", secondFloor);

            TenantHallwayWest tenantHallway = new TenantHallwayWest(science101Back, game, ref player);
            maps.Add("Tenant Hallway West", tenantHallway);

            TenantHallwayEast tenantHallwayEast = new TenantHallwayEast(science101Back, game, ref player);
            maps.Add("Tenant Hallway East", tenantHallwayEast);

            TchaikovskysRoom roomTwo = new TchaikovskysRoom(science101Back, game, ref player);
            maps.Add("Mozart's Room", roomTwo);

            MozartsRoom roomOne = new MozartsRoom(science101Back, game, ref player);
            maps.Add("Tchaikovsky's Room", roomOne);

            RoomThree roomThree = new RoomThree(science101Back, game, ref player);
            maps.Add("Tenant Room #3", roomThree);

            VacantRoom vacantRoom = new VacantRoom(science101Back, game, ref player);
            maps.Add("Vacant Room", vacantRoom);

            WarholsRoom warholsRoom = new WarholsRoom(science101Back, game, ref player);
            maps.Add("Warhol's Room", warholsRoom);

            RoomFive roomFive = new RoomFive(science101Back, game, ref player);
            maps.Add("Tenant Room #5", roomFive);

            RoomFour roomFour = new RoomFour(science101Back, game, ref player);
            maps.Add("Tenant Room #4", roomFour);

            StorageRoom storageRoom = new StorageRoom(science101Back, game, ref player);
            maps.Add("Storage Room", storageRoom);

            BeethovensRoom beethovensRoom = new BeethovensRoom(science101Back, game, ref player);
            maps.Add("Beethoven's Room", beethovensRoom);

            Backstage backStage = new Backstage(science101Back, game, ref player);
            maps.Add("Backstage", backStage);

            ManagersOffice managersFloor = new ManagersOffice(science101Back, game, ref player);
            maps.Add("Manager's Office", managersFloor);

            RestrictedHallway restrictedHallway = new RestrictedHallway(science101Back, game, ref player);
            maps.Add("Restricted Hallway", restrictedHallway);

            AxisOfMusicalReality axisOfMusic = new AxisOfMusicalReality(science101Back, game, ref player);
            maps.Add("Axis of Musical Reality", axisOfMusic);

            AxisOfArtisticReality axisOfArt = new AxisOfArtisticReality(science101Back, game, ref player);
            maps.Add("Axis of Artistic Reality", axisOfArt);

            BridgeOfArmanhand bridgeOfArmanhand = new BridgeOfArmanhand(science101Back, game, ref player);
            maps.Add("Bridge of Armanhand", bridgeOfArmanhand);

            BridgeOfArmanhandRift bridgeOfArmanhandRift = new BridgeOfArmanhandRift(science101Back, game, ref player);
            maps.Add("Bridge of Armanhand - Rift", bridgeOfArmanhandRift);

            GrandCanal grandCanal = new GrandCanal(science101Back, game, ref player);
            maps.Add("The Grand Canal", grandCanal);

            ArtistsPlayground artistsPlayground = new ArtistsPlayground(science101Back, game, ref player);
            maps.Add("Artist's Playground", artistsPlayground);

            Market theMarket = new Market(science101Back, game, ref player);
            maps.Add("Art Gallery", theMarket);

            CityStreets cityStreets = new CityStreets(science101Back, game, ref player);
            maps.Add("City Streets", cityStreets);

            TownSquare townSquare = new TownSquare(science101Back, game, ref player);
            maps.Add("Town Square", townSquare);
            #endregion

            #region Upper Vents
            List<Texture2D> ventsBack = new List<Texture2D>();

            UpperVents1 uVents1 = new UpperVents1(ventsBack, game, ref player);
            maps.Add("Upper Vents I", uVents1);

            UpperVentsII uVentsII = new UpperVentsII(ventsBack, game, ref player);
            maps.Add("Upper Vents II", uVentsII);

            List<Texture2D> vents3Back = new List<Texture2D>();
            UpperVentsIII uVentsIII = new UpperVentsIII(vents3Back, game, ref player);
            maps.Add("Upper Vents III", uVentsIII);

            UpperVentsIV uVentsIV = new UpperVentsIV(ventsBack, game, ref player);
            maps.Add("Upper Vents IV", uVentsIV);

            UpperVentsV uVentsV = new UpperVentsV(ventsBack, game, ref player);
            maps.Add("Upper Vents V", uVentsV);

            UpperVentsVI uVentsVI = new UpperVentsVI(ventsBack, game, ref player);
            maps.Add("Upper Vents VI", uVentsVI);

            Furnace furnace = new Furnace(ventsBack, game, ref player);
            maps.Add("Furnace Room", furnace);

            CoalShaft uVentsChall = new CoalShaft(ventsBack, game, ref player);
            maps.Add("Coal Shaft", uVentsChall);

            SideVentsI sVentsI = new SideVentsI(ventsBack, game, ref player);
            maps.Add("Side Vents I", sVentsI);

            SideVentsII sVentsII = new SideVentsII(ventsBack, game, ref player);
            maps.Add("Side Vents II", sVentsII);

            PrincessLockerRoom princessRoom = new PrincessLockerRoom(ventsBack, game, ref player);
            maps.Add("Princess' Room", princessRoom);
            #endregion

            #region Chelsea's
            List<Texture2D> partyBack = new List<Texture2D>();
            TheParty theParty = new TheParty(partyBack, game, ref player);
            maps.Add("The Party", theParty);

            List<Texture2D> outsidePartyBack = new List<Texture2D>();
            OutsideTheParty outsideTheParty = new OutsideTheParty(outsidePartyBack, game, ref player);
            maps.Add("Outside the Party", outsideTheParty);

            List<Texture2D> behindPartyBack = new List<Texture2D>();
            BehindTheParty behindTheParty = new BehindTheParty(behindPartyBack, game, ref player);
            maps.Add("Behind the Party", behindTheParty);

            List<Texture2D> theGoatsBack = new List<Texture2D>();
            TheGoats theGoats = new TheGoats(theGoatsBack, game, ref player);
            maps.Add("The Goats", theGoats);

            List<Texture2D> poolBack = new List<Texture2D>();
            ChelseasPool chelseasPool = new ChelseasPool(poolBack, game, ref player);
            maps.Add("Chelsea's Pool", chelseasPool);

            List<Texture2D> houseBack = new List<Texture2D>();
            TreeHouse treeHouse = new TreeHouse(houseBack, game, ref player);
            maps.Add("Tree House", treeHouse);

            List<Texture2D> chelseasFieldBack = new List<Texture2D>();
            ChelseasField chelseasField = new ChelseasField(chelseasFieldBack, game, ref player);
            maps.Add("Chelsea's Field", chelseasField);

            List<Texture2D> spookyBack = new List<Texture2D>();
            SpookyField spookyField = new SpookyField(spookyBack, game, ref player);
            maps.Add("Spooky Field", spookyField);

            List<Texture2D> anotherSpookyBack = new List<Texture2D>();
            AnotherSpookyField anotherSpookyField = new AnotherSpookyField(anotherSpookyBack, game, ref player);
            maps.Add("Another Spooky Field", anotherSpookyField);

            List<Texture2D> tutBack = new List<Texture2D>();
            InBetweenField inBetweenField = new InBetweenField(tutBack, game, ref player);
            maps.Add("InBetween Field", inBetweenField);

            List<Texture2D> workersBack = new List<Texture2D>();
            WorkersField workersField = new WorkersField(workersBack, game, ref player);
            maps.Add("Worker's Field", workersField);

            IrrigationCanal irrigationCanal = new IrrigationCanal(new List<Texture2D>() { }, game, ref player);
            maps.Add("Irrigation Canal", irrigationCanal);

            TheWoods theWoods = new TheWoods(new List<Texture2D>() { }, game, ref player);
            maps.Add("The Woods", theWoods);

            DeepWoods deepWoods = new DeepWoods(new List<Texture2D>() { }, game, ref player);
            maps.Add("Deep Woods", deepWoods);

            List<Texture2D> barnBack = new List<Texture2D>();
            OokySpookyBarn ookySpookyBarn = new OokySpookyBarn(barnBack, game, ref player);
            maps.Add("Ooky Spooky Barn", ookySpookyBarn);

            List<Texture2D> shedBack = new List<Texture2D>();
            OldShed oldShed = new OldShed(shedBack, game, ref player);
            maps.Add("Old Shed", oldShed);

            List<Texture2D> hutBack = new List<Texture2D>();
            TrollsHut trollsHut = new TrollsHut(shedBack, game, ref player);
            maps.Add("Trolls Hut", trollsHut);

            MysteriouslyPeacefulClearing mysteriouslyPeacefulClearing = new MysteriouslyPeacefulClearing(new List<Texture2D>() { }, game, ref player);
            maps.Add("Mysteriously Peaceful Clearing", mysteriouslyPeacefulClearing);

            Crossroads crossroads = new Crossroads(new List<Texture2D>() { }, game, ref player);
            maps.Add("Crossroads", crossroads);

            WoodsyRiver woodsyRiver = new WoodsyRiver(new List<Texture2D>() { }, game, ref player);
            maps.Add("Woodsy River", woodsyRiver);

            DirtyPath dirtyPath = new DirtyPath(new List<Texture2D>() { }, game, ref player);
            maps.Add("Dirty Path", dirtyPath);

            SuperSecretDeerBaseAlpha superSecretDeerBaseAlpha = new SuperSecretDeerBaseAlpha(new List<Texture2D>() { }, game, ref player);
            maps.Add("Super Secret Deer Base Alpha", superSecretDeerBaseAlpha);

            EmptyField emptyField = new EmptyField(new List<Texture2D>() { }, game, ref player);
            maps.Add("Empty Field", emptyField);

            HiddenPath hiddenPath = new HiddenPath(new List<Texture2D>() { }, game, ref player);
            maps.Add("Hidden Path", hiddenPath);

            DeerShack deerShack = new DeerShack(new List<Texture2D>() { }, game, ref player);
            maps.Add("Deer Shack", deerShack);

            #endregion

            #region History
            OutsideStoneFort outsideEnemyCamp = new OutsideStoneFort(new List<Texture2D>(), game, ref player);
            maps.Add("Stone Fort Gate", outsideEnemyCamp);

            StoneFortCentral stoneFortCentral = new StoneFortCentral(new List<Texture2D>(), game, ref player);
            maps.Add("Stone Fort - Central", stoneFortCentral);

            StoneFortWest stoneFortWest = new StoneFortWest(new List<Texture2D>(), game, ref player);
            maps.Add("Stone Fort - West", stoneFortWest);

            StoneFortEast stoneFortEast = new StoneFortEast(new List<Texture2D>(), game, ref player);
            maps.Add("Stone Fort - East", stoneFortEast);

            StoneFortWasteland stoneWasteland = new StoneFortWasteland(new List<Texture2D>(), game, ref player);
            maps.Add("Stone Fort Wasteland", stoneWasteland);

            HistoryEntrance historyEntrance = new HistoryEntrance(new List<Texture2D>(), game, ref player);
            maps.Add("Intro to History", historyEntrance);

            IncaVillage incaEmpire = new IncaVillage(new List<Texture2D>(), game, ref player);
            maps.Add("Inca Empire", incaEmpire);

            NapoleonsCamp napoleonsCamp = new NapoleonsCamp(science101Back, game, ref player);
            maps.Add("Napoleon's Camp", napoleonsCamp);

            NapoleonsTent napoleonsTent = new NapoleonsTent(science101Back, game, ref player);
            maps.Add("Napoleon's Tent", napoleonsTent);

            MedicalTent medicalTent = new MedicalTent(science101Back, game, ref player);
            maps.Add("Medical Tent", medicalTent);

            MongolCamp mongolCamp = new MongolCamp(science101Back, game, ref player);
            maps.Add("Mongolian Camp", mongolCamp);

            TheGreatWall theGreatWall = new TheGreatWall(science101Back, game, ref player);
            maps.Add("The Great Wall", theGreatWall);

            GenghisYurt genghisYurt = new GenghisYurt(science101Back, game, ref player);
            maps.Add("The Yurt of Khan", genghisYurt);

            Battlefield battlefield = new Battlefield(science101Back, game, ref player);
            maps.Add("Battlefield", battlefield);

            GoblinMedicalCamp enemyMedicalCamp = new GoblinMedicalCamp(science101Back, game, ref player);
            maps.Add("Enemy Medical Camp", enemyMedicalCamp);

            TrenchfootField trenchfootField = new TrenchfootField(science101Back, game, ref player);
            maps.Add("Trenchfoot Field", trenchfootField);

            NoMansValley noMansValley = new NoMansValley(science101Back, game, ref player);
            maps.Add("No Man's Valley", noMansValley);

            BattlefieldOutskirts battlefieldOutskirts = new BattlefieldOutskirts(science101Back, game, ref player);
            maps.Add("Battlefield Outskirts", battlefieldOutskirts);

            DryDesert dryDesert = new DryDesert(science101Back, game, ref player);
            maps.Add("Dry Desert", dryDesert);

            WindyDesert windyDesert = new WindyDesert(science101Back, game, ref player);
            maps.Add("Windy Desert", windyDesert);

            DistantDesert distantDesert = new DistantDesert(science101Back, game, ref player);
            maps.Add("Distant Desert", distantDesert);

            ForgottenTomb forgottenTomb = new ForgottenTomb(science101Back, game, ref player);
            maps.Add("Forgotten Tomb", forgottenTomb);

            CentralSands centralSands = new CentralSands(science101Back, game, ref player);
            maps.Add("Central Sands", centralSands);

            CursedSands cursedSands = new CursedSands(science101Back, game, ref player);
            maps.Add("Cursed Sands", cursedSands);

            Oasis oasis = new Oasis(science101Back, game, ref player);
            maps.Add("Oasis", oasis);

            Egypt egypt = new Egypt(science101Back, game, ref player);
            maps.Add("Egypt", egypt);

            TheGreatPyramid pyramid = new TheGreatPyramid(science101Back, game, ref player);
            maps.Add("The Great Pyramid", pyramid);

            MikrovsHill mikrovsHill = new MikrovsHill(science101Back, game, ref player);
            maps.Add("Mikrov's Hill", mikrovsHill);

            AxisOfHistoricalReality axisOfHistory = new AxisOfHistoricalReality(science101Back, game, ref player);
            maps.Add("Axis of Historical Reality", axisOfHistory);

            maps.Add("Behind the Great Wall", new BehindTheGreatWall(new List<Texture2D>(), game, ref player));

            maps.Add("Central Sands - Rift", new CentralSandsRift(new List<Texture2D>(), game, ref player));



            #endregion

            #region Pyramid
            maps.Add("Pyramid Entrance", new PyramidEntrance(new List<Texture2D>(), game, ref player));
            maps.Add("Outer Chamber", new OuterChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Side Chamber I", new SideChamberI(new List<Texture2D>(), game, ref player));
            maps.Add("Side Chamber II", new SideChamberII(new List<Texture2D>(), game, ref player));
            maps.Add("Side Chamber III", new SideChamberIII(new List<Texture2D>(), game, ref player));
            maps.Add("Side Chamber IV", new SideChamberIV(new List<Texture2D>(), game, ref player));
            maps.Add("Main Chamber", new MainChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Basement Stairs", new BasementStairs(new List<Texture2D>(), game, ref player));
            maps.Add("Pharaoh's Keep", new PharaohsKeep(new List<Texture2D>(), game, ref player));
            maps.Add("Indoor Garden", new IndoorGarden(new List<Texture2D>(), game, ref player));
            maps.Add("Chamber of Symmetry", new ChamberOfSymmetry(new List<Texture2D>(), game, ref player));
            maps.Add("Inner Chamber", new InnerChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Central Hall I", new CentralHallI(new List<Texture2D>(), game, ref player));
            maps.Add("Central Hall II", new CentralHallII(new List<Texture2D>(), game, ref player));
            maps.Add("Central Hall III", new CentralHallIII(new List<Texture2D>(), game, ref player));
            maps.Add("Collapsing Room", new CollapsingRoom(new List<Texture2D>(), game, ref player));
            maps.Add("Forgotten Chamber I", new ForgottenChamberI(new List<Texture2D>(), game, ref player));
            maps.Add("Forgotten Chamber II", new ForgottenChamberII(new List<Texture2D>(), game, ref player));
            maps.Add("Emperor Boom-Boom's Tomb", new EmperorBoomBoomsTomb(new List<Texture2D>(), game, ref player));
            maps.Add("Pharaoh's Gate", new PharaohsGate(new List<Texture2D>(), game, ref player));
            maps.Add("False Room", new FalseRoom(new List<Texture2D>(), game, ref player));
            maps.Add("Flower Sanctuary", new FlowerSanctuary(new List<Texture2D>(), game, ref player));
            maps.Add("The Cliff of Ile", new TheCliffOfIle(new List<Texture2D>(), game, ref player));
            maps.Add("The Pit of Long Falls", new PitOfLongFalls(new List<Texture2D>(), game, ref player));
            maps.Add("Pharaoh's Road", new PharaohsRoad(new List<Texture2D>(), game, ref player));
            maps.Add("Pharaoh's Trap", new PharaohsTrap(new List<Texture2D>(), game, ref player));
            maps.Add("Hidden Passage", new HiddenPassage(new List<Texture2D>(), game, ref player));
            maps.Add("Pyramid Chute", new PyramidChute(new List<Texture2D>(), game, ref player));
            maps.Add("Ancient Altar", new AncientAltar(new List<Texture2D>(), game, ref player));
            maps.Add("Eastern Chamber", new EasternChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Center of the Pyramid", new CenterOfThePyramid(new List<Texture2D>(), game, ref player));
            maps.Add("Secret Passage", new SecretPassage(new List<Texture2D>(), game, ref player));
            //Basement
            maps.Add("Underground Tunnel I", new UndergroundTunnelI(new List<Texture2D>(), game, ref player));
            maps.Add("Underground Tunnel II", new UndergroundTunnelII(new List<Texture2D>(), game, ref player));
            maps.Add("The Pit", new ThePit(new List<Texture2D>(), game, ref player));
            maps.Add("Basement Entrance", new BasementEntrance(new List<Texture2D>(), game, ref player));
            maps.Add("Darkened Chamber", new DarkenedChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Small Treasure Room", new SmallTreasureRoom(new List<Texture2D>(), game, ref player));
            maps.Add("Resting Chamber", new RestingChamber(new List<Texture2D>(), game, ref player));
            maps.Add("The Moaning Hallway", new TheMoaningHallway(new List<Texture2D>(), game, ref player));
            maps.Add("Hall of the Undead", new HalloftheUndead(new List<Texture2D>(), game, ref player));
            maps.Add("The Summoning Crypt", new TheSummoningCrypt(new List<Texture2D>(), game, ref player));
            maps.Add("Hall of Trials", new HallofTrials(new List<Texture2D>(), game, ref player));
            maps.Add("Burial Chamber", new BurialChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Prison Chamber", new PrisonChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Torture Chamber", new TortureChamber(new List<Texture2D>(), game, ref player));
            maps.Add("Organ Storage Room Two", new OrgranStorageRoomTwo(new List<Texture2D>(), game, ref player));
            maps.Add("Organ Storage Room One", new OrgranStorageRoomOne(new List<Texture2D>(), game, ref player));
            maps.Add("Organ Storage Room Three", new OrgranStorageRoomThree(new List<Texture2D>(), game, ref player));
            maps.Add("Chamber 44", new Chamber44(new List<Texture2D>(), game, ref player));
            maps.Add("Room of Offerings", new RoomOfOfferings(new List<Texture2D>(), game, ref player));
            maps.Add("Tunnel of Certain Death", new TunnelOfCertainDeath(new List<Texture2D>(), game, ref player));
            maps.Add("Chamber of Corruption", new ChamberOfCorruption(new List<Texture2D>(), game, ref player));
            maps.Add("Butterfly Chamber", new ButterflyChamber(new List<Texture2D>(), game, ref player));
            #endregion

            #region Literature
            maps.Add("Welcome to Middle Earth", new WelcomeToMiddleEarth(new List<Texture2D>(), game, ref player));
            maps.Add("Forest of Ents", new ForestOfEnts(new List<Texture2D>(), game, ref player));
            maps.Add("Forest of Ents - Rift", new ForestOfEntsRift(new List<Texture2D>(), game, ref player));
            maps.Add("Forest Path", new ForestPath(new List<Texture2D>(), game, ref player));
            maps.Add("Tall Tale Terrace", new TallTaleTerrace(new List<Texture2D>(), game, ref player));

            //A christmas carol
            maps.Add("Snowy Streets", new SnowyStreets(new List<Texture2D>(), game, ref player));
            maps.Add("Ebenezer's Mansion", new EbenezersMansion(new List<Texture2D>(), game, ref player));
            maps.Add("Western Corridor", new WesternCorridor(new List<Texture2D>(), game, ref player));
            maps.Add("Scrooge's Bedroom", new ScroogesBedroom(new List<Texture2D>(), game, ref player));
            maps.Add("Living Area", new LivingArea(new List<Texture2D>(), game, ref player));
            maps.Add("Central Corridor", new CentralCorridor(new List<Texture2D>(), game, ref player));
            maps.Add("Unused Bedroom", new UnusedBedroom(new List<Texture2D>(), game, ref player));
            maps.Add("Walk In Safe", new WalkInSafe(new List<Texture2D>(), game, ref player));
            maps.Add("The Grand Corridor", new TheGrandCorridor(new List<Texture2D>(), game, ref player));
            maps.Add("Under the Mansion", new UnderTheMansion(new List<Texture2D>(), game, ref player));
            maps.Add("Abandoned Safe Room", new AbandonedSafeRoom(new List<Texture2D>(), game, ref player));
            maps.Add("The Haunted Ballroom", new TheHauntedBallroom(new List<Texture2D>(), game, ref player));
            maps.Add("Haunted Bedroom", new HauntedBedroom(new List<Texture2D>(), game, ref player));
            maps.Add("Eastern Corridor", new EasternCorridor(new List<Texture2D>(), game, ref player));
            maps.Add("Dining Hall", new DiningHall(new List<Texture2D>(), game, ref player));

            #endregion

            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}
