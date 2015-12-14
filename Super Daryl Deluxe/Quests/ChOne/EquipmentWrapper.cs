using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    [Serializable]
    public class EquipmentWrapper
    {
        public int strength;
        public int tolerance;
        public int motivation;
        public int upgradeSlots;
        public String passiveName = "";
        public EquipmentWrapper() { }

        public EquipmentWrapper(int s, int t, int m, int u, String p)
        {
            strength = s;
            tolerance = t;
            motivation = m;
            upgradeSlots = u;
            passiveName = p;
        }
    }
}
