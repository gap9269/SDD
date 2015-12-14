using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
   public class SocialRankManager
    {
       public struct SocialRank
       {
           public String socialRank;
           public int karmaNeeded;
           public Passive passiveGrantedThisRank;
       }

       public static List<SocialRank> allSocialRanks;

       public SocialRankManager()
       {
           allSocialRanks = new List<SocialRank>();
           SocialRank FlowerBoy = new SocialRank() {socialRank = "Flower Boy",  karmaNeeded = 1, passiveGrantedThisRank = PassiveManager.allPassives["You Have Friends!"]};

           SocialRank PantsWetter = new SocialRank() {socialRank = "Pants Wetter", karmaNeeded = 20, passiveGrantedThisRank = PassiveManager.allPassives["Magic Magnet"] };

           SocialRank LousyEmployee = new SocialRank() { socialRank = "Lousy Employee", karmaNeeded = 33, passiveGrantedThisRank = PassiveManager.allPassives["Enhanced Vitamins"] };

           //Add the passives IN ORDER OF INCREASING KARMA REQUIRED
           allSocialRanks.Add(FlowerBoy);
           allSocialRanks.Add(PantsWetter);
           allSocialRanks.Add(LousyEmployee);
       }
    }
}
