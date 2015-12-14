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
    public class SkillManager
    {
        static Dictionary<String, Skill> allSkills;
        public static Dictionary<String, List<Texture2D>> skillImpactEffects;

        Dictionary<String, Texture2D> animations;
        Dictionary<String, Texture2D> icons;

        public static Dictionary<String, Skill> AllSkills { get { return allSkills; } set { allSkills = value; } }

        public SkillManager(Dictionary<String, Texture2D> animations, Dictionary<String, Texture2D> icons)
        {
            allSkills = new Dictionary<string, Skill>();
            skillImpactEffects = new Dictionary<string, List<Texture2D>>();

            this.animations = animations;
            this.icons = icons;

            DiscussDifferences discussDifferences = new DiscussDifferences(animations["Discuss Differences"], Game1.Player, icons["Discuss Differences"]);
            allSkills.Add("Discuss Differences", discussDifferences);

            BlindingLogic blindingLogic = new BlindingLogic(animations["Blinding Logic"], Game1.Player, icons["Blinding Logic"]);
            allSkills.Add("Blinding Logic", blindingLogic);

            QuickRetort quickRetort = new QuickRetort(animations["Quick Retort"], Game1.Player, icons["Quick Retort"]);
            allSkills.Add("Quick Retort", quickRetort);
            quickRetort.LoadContent();
            Game1.Player.quickRetort = quickRetort;

            TwistedThinking twistedThinking = new TwistedThinking(animations["Twisted Thinking"], Game1.Player, icons["Twisted Thinking"]);
            //allSkills.Add("Twisted Thinking", twistedThinking);

            ShockingStatement shockingStatement = new ShockingStatement(animations["Shocking Statements CH.1"], Game1.Player, icons["Shocking Statements CH.1"]);
            allSkills.Add("Shocking Statements CH.1", shockingStatement);

            LightningDash lightningDash = new LightningDash(animations["Lightning Dash"], Game1.Player, icons["Lightning Dash"]);
            //allSkills.Add("Lightning Dash", lightningDash);

            DistanceYourself distanceYourself = new DistanceYourself(animations["Distance Yourself"], Game1.Player, icons["Distance Yourself"]);
            //allSkills.Add("Distance Yourself", distanceYourself);

            SharpComment sharpComment = new SharpComment(animations["Sharp Comment"], Game1.Player, icons["Sharp Comment"]);
            //allSkills.Add("Sharp Comment", sharpComment);

            PointedJabs pointedJabs = new PointedJabs(animations["Pointed Jabs"], Game1.Player, icons["Pointed Jabs"]);
            //allSkills.Add("Pointed Jabs", pointedJabs);

            SharpComments spinSlash = new SharpComments(animations["Sharp Comments"], Game1.Player, icons["Sharp Comments"]);
            allSkills.Add("Sharp Comments", spinSlash);

            Shooter shooter = new Shooter(animations["Fowl Mouth"], Game1.Player, icons["Fowl Mouth"]);
            allSkills.Add("Fowl Mouth", shooter);

            MoppingUp launch = new MoppingUp(animations["Mopping Up"], Game1.Player, icons["Mopping Up"]);
            allSkills.Add("Mopping Up", launch);

            ShockingStatementsCh3 lightningArrow = new ShockingStatementsCh3(animations["Shocking Statements CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.3", lightningArrow);

            CrushingRealization crushingRealization = new CrushingRealization(animations["Crushing Realization"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Crushing Realization", crushingRealization);

            CombustibleConfutationCH1 harshBurn = new CombustibleConfutationCH1(animations["Combustible Confutation CH.1"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.1", harshBurn);

            CuttingCorners cuttingCorners = new CuttingCorners(animations["Cutting Corners"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Cutting Corners", cuttingCorners);

            ShockingStatementCh2 shockingStatementCh2 = new ShockingStatementCh2(animations["Shocking Statements CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.2", shockingStatementCh2);

            CombustibleConfutationCH2 combustibleConfutationCH2 = new CombustibleConfutationCH2(animations["Combustible Confutation CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.2", combustibleConfutationCH2);

            CombustibleConfutationCH3 combustibleConfutationCH3 = new CombustibleConfutationCH3(animations["Combustible Confutation CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.3", combustibleConfutationCH3);
        }

        public void Reset()
        {
            allSkills = new Dictionary<string, Skill>();

            DiscussDifferences discussDifferences = new DiscussDifferences(animations["Discuss Differences"], Game1.Player, icons["Discuss Differences"]);
            allSkills.Add("Discuss Differences", discussDifferences);

            BlindingLogic blindingLogic = new BlindingLogic(animations["Blinding Logic"], Game1.Player, icons["Blinding Logic"]);
            allSkills.Add("Blinding Logic", blindingLogic);

            QuickRetort quickRetort = new QuickRetort(animations["Quick Retort"], Game1.Player, icons["Quick Retort"]);
            allSkills.Add("Quick Retort", quickRetort);

            TwistedThinking twistedThinking = new TwistedThinking(animations["Twisted Thinking"], Game1.Player, icons["Twisted Thinking"]);
            //allSkills.Add("Twisted Thinking", twistedThinking);

            ShockingStatement shockingStatement = new ShockingStatement(animations["Shocking Statements CH.1"], Game1.Player, icons["Shocking Statements CH.1"]);
            allSkills.Add("Shocking Statements CH.1", shockingStatement);

            LightningDash lightningDash = new LightningDash(animations["Lightning Dash"], Game1.Player, icons["Lightning Dash"]);
            //allSkills.Add("Lightning Dash", lightningDash);

            DistanceYourself distanceYourself = new DistanceYourself(animations["Distance Yourself"], Game1.Player, icons["Distance Yourself"]);
            //allSkills.Add("Distance Yourself", distanceYourself);

            SharpComment sharpComment = new SharpComment(animations["Sharp Comment"], Game1.Player, icons["Sharp Comment"]);
            //allSkills.Add("Sharp Comment", sharpComment);

            PointedJabs pointedJabs = new PointedJabs(animations["Pointed Jabs"], Game1.Player, icons["Pointed Jabs"]);
            //allSkills.Add("Pointed Jabs", pointedJabs);

            SharpComments spinSlash = new SharpComments(animations["Sharp Comments"], Game1.Player, icons["Sharp Comments"]);
            allSkills.Add("Sharp Comments", spinSlash);

            Shooter shooter = new Shooter(animations["Fowl Mouth"], Game1.Player, icons["Fowl Mouth"]);
            allSkills.Add("Fowl Mouth", shooter);

            MoppingUp launch = new MoppingUp(animations["Mopping Up"], Game1.Player, icons["Mopping Up"]);
            allSkills.Add("Mopping Up", launch);

            ShockingStatementsCh3 lightningArrow = new ShockingStatementsCh3(animations["Shocking Statements CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.3", lightningArrow);

            CrushingRealization crushingRealization = new CrushingRealization(animations["Crushing Realization"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Crushing Realization", crushingRealization);

            CombustibleConfutationCH1 harshBurn = new CombustibleConfutationCH1(animations["Combustible Confutation CH.1"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.1", harshBurn);

            CuttingCorners cuttingCorners = new CuttingCorners(animations["Cutting Corners"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Cutting Corners", cuttingCorners);

            ShockingStatementCh2 shockingStatementCh2 = new ShockingStatementCh2(animations["Shocking Statements CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.2", shockingStatementCh2);

            CombustibleConfutationCH2 combustibleConfutationCH2 = new CombustibleConfutationCH2(animations["Combustible Confutation CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.2", combustibleConfutationCH2);

            CombustibleConfutationCH3 combustibleConfutationCH3 = new CombustibleConfutationCH3(animations["Combustible Confutation CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.3", combustibleConfutationCH3);
        }

        public void AddDemoSkills()
        {
            allSkills = new Dictionary<string, Skill>();

            DiscussDifferencesDemo discussDifferences = new DiscussDifferencesDemo(animations["Discuss Differences"], Game1.Player, icons["Discuss Differences"]);
            allSkills.Add("Discuss Differences", discussDifferences);

            BlindingLogicDemo blindingLogic = new BlindingLogicDemo(animations["Blinding Logic"], Game1.Player, icons["Blinding Logic"]);
            BlindingLogicDemo.flashTextures = BlindingLogic.flashTextures;

            allSkills.Add("Blinding Logic", blindingLogic);

            QuickRetortDemo quickRetort = new QuickRetortDemo(animations["Quick Retort"], Game1.Player, icons["Quick Retort"]);
            allSkills.Add("Quick Retort", quickRetort);

            ShockingStatementDemo shockingStatement = new ShockingStatementDemo(animations["Shocking Statements CH.1"], Game1.Player, icons["Shocking Statements CH.1"]);
            allSkills.Add("Shocking Statements CH.1", shockingStatement);

            SharpCommentsDemo spinSlash = new SharpCommentsDemo(animations["Sharp Comments"], Game1.Player, icons["Sharp Comments"]);
            allSkills.Add("Sharp Comments", spinSlash);

            ShooterDemo shooter = new ShooterDemo(animations["Fowl Mouth"], Game1.Player, icons["Fowl Mouth"]);
            allSkills.Add("Fowl Mouth", shooter);

            MoppingUpDemo launch = new MoppingUpDemo(animations["Mopping Up"], Game1.Player, icons["Mopping Up"]);
            allSkills.Add("Mopping Up", launch);

            ShockingStatementsCh3Demo lightningArrow = new ShockingStatementsCh3Demo(animations["Shocking Statements CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.3", lightningArrow);

            CrushingRealizationDemo crushingRealization = new CrushingRealizationDemo(animations["Crushing Realization"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Crushing Realization", crushingRealization);

            CombustibleConfutationCH1Demo harshBurn = new CombustibleConfutationCH1Demo(animations["Combustible Confutation CH.1"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.1", harshBurn);

            CuttingCornersDemo cuttingCorners = new CuttingCornersDemo(animations["Cutting Corners"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Cutting Corners", cuttingCorners);

            ShockingStatementCh2Demo shockingStatementCh2 = new ShockingStatementCh2Demo(animations["Shocking Statements CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Shocking Statements CH.2", shockingStatementCh2);

            CombustibleConfutationCH2Demo combustibleConfutationCH2 = new CombustibleConfutationCH2Demo(animations["Combustible Confutation CH.2"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.2", combustibleConfutationCH2);

            CombustibleConfutationCH3Demo combustibleConfutationCH3 = new CombustibleConfutationCH3Demo(animations["Combustible Confutation CH.3"], Game1.Player, Game1.whiteFilter);
            allSkills.Add("Combustible Confutation CH.3", combustibleConfutationCH3);
        }
    }
}
