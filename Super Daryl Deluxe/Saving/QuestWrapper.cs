using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISurvived
{
    [Serializable]
    public class QuestWrapper
    {
        public String questName;
        public bool completedQuest;
        public List<int> enemiesKilledForQuest;
        public Boolean inQuestHelper;
        public String npcName;

        public QuestWrapper() { }

        public QuestWrapper(String qName, bool completedQ, List<int> enemiesKilled, bool inQHelper, String nameNPC)
        {
            questName = qName;
            completedQuest = completedQ;
            enemiesKilledForQuest = enemiesKilled;
            inQuestHelper = inQHelper;
            npcName = nameNPC;
        }

    }
}
