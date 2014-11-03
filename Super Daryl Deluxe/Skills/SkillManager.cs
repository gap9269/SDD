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
    public class SkillManager
    {
        static Dictionary<String, Skill> allSkills;
        public static Dictionary<String, Texture2D> skillImpactEffects;

        Dictionary<String, Texture2D> animations;
        Dictionary<String, Texture2D> icons;

        public static Dictionary<String, Skill> AllSkills { get { return allSkills; } set { allSkills = value; } }

        public SkillManager(Dictionary<String, Texture2D> animations, Dictionary<String, Texture2D> icons, Player player)
        {
            allSkills = new Dictionary<string, Skill>();
            skillImpactEffects = new Dictionary<string, Texture2D>();

            this.animations = animations;
            this.icons = icons;

            DiscussDifferences discussDifferences = new DiscussDifferences(animations["Discuss Differences"], player, icons["Discuss Differences"]);
            allSkills.Add("Discuss Differences", discussDifferences);

            BlindingLogic blindingLogic = new BlindingLogic(animations["Blinding Logic"], player, icons["Blinding Logic"]);
            allSkills.Add("Blinding Logic", blindingLogic);

            QuickRetort quickRetort = new QuickRetort(animations["Quick Retort"], player, icons["Quick Retort"]);
            allSkills.Add("Quick Retort", quickRetort);

            TwistedThinking twistedThinking = new TwistedThinking(animations["Twisted Thinking"], player, icons["Twisted Thinking"]);
            //allSkills.Add("Twisted Thinking", twistedThinking);

            ShockingStatement shockingStatement = new ShockingStatement(animations["Shocking Statement"], player, icons["Shocking Statement"]);
            allSkills.Add("Shocking Statement", shockingStatement);

            LightningDash lightningDash = new LightningDash(animations["Lightning Dash"], player, icons["Lightning Dash"]);
            //allSkills.Add("Lightning Dash", lightningDash);

            DistanceYourself distanceYourself = new DistanceYourself(animations["Distance Yourself"], player, icons["Distance Yourself"]);
            //allSkills.Add("Distance Yourself", distanceYourself);

            SharpComment sharpComment = new SharpComment(animations["Sharp Comment"], player, icons["Sharp Comment"]);
            //allSkills.Add("Sharp Comment", sharpComment);

            PointedJabs pointedJabs = new PointedJabs(animations["Pointed Jabs"], player, icons["Pointed Jabs"]);
            //allSkills.Add("Pointed Jabs", pointedJabs);

            SpinSlash spinSlash = new SpinSlash(animations["Sharp Comments"], player, icons["Sharp Comments"]);
            allSkills.Add("Sharp Comments", spinSlash);

            Shooter shooter = new Shooter(animations["Fowl Mouth"], player, icons["Fowl Mouth"]);
            allSkills.Add("Fowl Mouth", shooter);

            Launch launch = new Launch(animations["Mopping Up"], player, icons["Mopping Up"]);
            allSkills.Add("Mopping Up", launch);
        }

        public void Reset()
        {

            allSkills = new Dictionary<string, Skill>();

            DiscussDifferences discussDifferences = new DiscussDifferences(animations["Discuss Differences"], Game1.Player, icons["Discuss Differences"]);
            allSkills.Add("Discuss Differences", discussDifferences);

            BlindingLogic blindingLogic = new BlindingLogic(animations["Blinding Logic"], Game1.Player, icons["Blinding Logic"]);
           // allSkills.Add("Blinding Logic", blindingLogic);

            QuickRetort quickRetort = new QuickRetort(animations["Quick Retort"], Game1.Player, icons["Quick Retort"]);
            allSkills.Add("Quick Retort", quickRetort);

            TwistedThinking twistedThinking = new TwistedThinking(animations["Twisted Thinking"], Game1.Player, icons["Twisted Thinking"]);
            //allSkills.Add("Twisted Thinking", twistedThinking);

            ShockingStatement shockingStatement = new ShockingStatement(animations["Shocking Statement"], Game1.Player, icons["Shocking Statement"]);
            allSkills.Add("Shocking Statement", shockingStatement);

            LightningDash lightningDash = new LightningDash(animations["Lightning Dash"], Game1.Player, icons["Lightning Dash"]);
            //allSkills.Add("Lightning Dash", lightningDash);

            DistanceYourself distanceYourself = new DistanceYourself(animations["Distance Yourself"], Game1.Player, icons["Distance Yourself"]);
           // allSkills.Add("Distance Yourself", distanceYourself);

            SharpComment sharpComment = new SharpComment(animations["Sharp Comment"], Game1.Player, icons["Sharp Comment"]);
            //allSkills.Add("Sharp Comment", sharpComment);

            PointedJabs pointedJabs = new PointedJabs(animations["Pointed Jabs"], Game1.Player, icons["Pointed Jabs"]);
            //allSkills.Add("Pointed Jabs", pointedJabs);

            SpinSlash spinSlash = new SpinSlash(animations["Sharp Comments"], Game1.Player, icons["Sharp Comments"]);
            allSkills.Add("Sharp Comments", spinSlash);

            Shooter shooter = new Shooter(animations["Fowl Mouth"], Game1.Player, icons["Fowl Mouth"]);
            allSkills.Add("Fowl Mouth", shooter);

            Launch launch = new Launch(animations["Mopping Up"], Game1.Player, icons["Mopping Up"]);
            allSkills.Add("Mopping Up", launch);
        }
    }
}
