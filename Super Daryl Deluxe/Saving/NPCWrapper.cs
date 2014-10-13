using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    [Serializable]
    public class NPCWrapper
    {
        public List<String> dialogue;
        public List<String> questDialogue;
        public int dialogueState;
        public Boolean facingRight;
        public String questName;
        public Boolean acceptedQuest = false;
        public List<Boolean> trenchcoatSoldOut;
        public Boolean trenchCoat = false;
        public String mapName;

        public NPCWrapper(List<String> d, List<String> qd, int ds, Boolean fr, String qn, Boolean aq, String mName)
        {
            dialogue = d;
            questDialogue = qd;
            dialogueState = ds;
            facingRight = fr;
            questName = qn;
            acceptedQuest = aq;
            mapName = mName;
        }

        public NPCWrapper() { }

        public NPCWrapper(List<String> d, int ds, Boolean fr, Boolean aq, String mName)
        {
            dialogue = d;
            dialogueState = ds;
            facingRight = fr;
            acceptedQuest = aq;
            mapName = mName;
        }

        public NPCWrapper(List<String> d, int ds, Boolean fr, Boolean aq, List<Boolean> so, String mName)
        {
            dialogue = d;
            dialogueState = ds;
            facingRight = fr;
            acceptedQuest = aq;
            trenchcoatSoldOut = so;
            trenchCoat = true;
            mapName = mName;
        }
    }
}
