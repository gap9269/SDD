using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    public class PassiveManager
    {

        public static Dictionary<String, Passive> allPassives;

        public PassiveManager(Game1 g)
        {
            allPassives = new Dictionary<string, Passive>();

            //Social rank passives
            Friends youHaveFriends = new Friends(g);
            allPassives.Add(youHaveFriends.Name, youHaveFriends);

            MagicMagnet magicMagnet = new MagicMagnet(g);
            allPassives.Add(magicMagnet.Name, magicMagnet);

            EnhancedVitamins enhancedVitamins = new EnhancedVitamins(g);
            allPassives.Add(enhancedVitamins.Name, enhancedVitamins);

            //Equipment passives
            SawGhost sawGhost = new SawGhost(g);
            allPassives.Add(sawGhost.Name, sawGhost);

            CurseOfCountRoger curseOfCountRoger = new CurseOfCountRoger(g);
            allPassives.Add(curseOfCountRoger.Name, curseOfCountRoger);

            ABitofLuck aBitofLuck = new ABitofLuck(g);
            allPassives.Add(aBitofLuck.Name, aBitofLuck);

            GoodFortune goodFortune = new GoodFortune(g);
            allPassives.Add(goodFortune.Name, goodFortune);

            PlusFiveExperience plusFiveExp = new PlusFiveExperience(g);
            allPassives.Add(plusFiveExp.Name, plusFiveExp);

            ShieldOfDesperation shieldOfDesperation = new ShieldOfDesperation(g);
            allPassives.Add(shieldOfDesperation.Name, shieldOfDesperation);

            SwordOfDesperation swordOfDesperation = new SwordOfDesperation(g);
            allPassives.Add(swordOfDesperation.Name, swordOfDesperation);

            ImpairedVision impairedVision = new ImpairedVision(g);
            allPassives.Add(impairedVision.Name, impairedVision);

            AutomatedHealing automatedHealing = new AutomatedHealing(g);
            allPassives.Add(automatedHealing.Name, automatedHealing);

            //Fake passives for trenchcoat joke in CH1
            ExperienceBoostMax experienceBoostMax = new ExperienceBoostMax(g);
            allPassives.Add(experienceBoostMax.Name, experienceBoostMax);

            DarylCanTalk darylCanTalk = new DarylCanTalk(g);
            allPassives.Add(darylCanTalk.Name, darylCanTalk);

            SummonBrownies summonBrownies = new SummonBrownies(g);
            allPassives.Add(summonBrownies.Name, summonBrownies);

            UnlockAllSkills unlockAllSkills = new UnlockAllSkills(g);
            allPassives.Add(unlockAllSkills.Name, unlockAllSkills);

            InfiniteLivesPassive infiniteLivesPassive = new InfiniteLivesPassive(g);
            allPassives.Add(infiniteLivesPassive.Name, infiniteLivesPassive);
        }

    }
}
