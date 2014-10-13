using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{

    //This class keeps track of every boolean that is specific to maps. This includes platforms appearing and disappearing, doors opening, chests appearing,
    //and other shit. 
    public class MapBooleans
    {
        public Dictionary<String, Boolean> prologueMapBooleans;
        public Dictionary<String, Boolean> chapterOneMapBooleans;
        public Dictionary<String, Boolean> tutorialMapBooleans;
        public Dictionary<String, Boolean> chapterTwoMapBooleans;

        public MapBooleans()
        {
            prologueMapBooleans = new Dictionary<string, bool>();
            chapterOneMapBooleans = new Dictionary<string, bool>();
            tutorialMapBooleans = new Dictionary<string, bool>();
            chapterTwoMapBooleans = new Dictionary<string, bool>();
        }
    }
}
