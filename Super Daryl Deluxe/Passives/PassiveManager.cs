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

            DoubleJump doubleJump = new DoubleJump(g);
            allPassives.Add(doubleJump.Name, doubleJump);

            //Equipment passives
            SawGhost sawGhost = new SawGhost(g);
            allPassives.Add(sawGhost.Name, sawGhost);


        }

    }
}
