using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    [Serializable]
    public class InteractiveWrapper
    {
        public int state;
        public Boolean finished;
        public int health;
        public Boolean hidden;
        public InteractiveWrapper() { }

        public InteractiveWrapper(int sta, bool fin, int hel, bool hide) 
        {
            state = sta;
            finished = fin;
            health = hel;
            hidden = hide;
        }
    }
}
