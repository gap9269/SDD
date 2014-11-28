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

           SocialRank AirJordan = new SocialRank() {socialRank = "Air Jordan", karmaNeeded = 15, passiveGrantedThisRank = PassiveManager.allPassives["Double Jump"] };


           //Add the passives IN ORDER OF INCREASING KARMA REQUIRED
           allSocialRanks.Add(FlowerBoy);
           allSocialRanks.Add(AirJordan);
       }
    }
}
