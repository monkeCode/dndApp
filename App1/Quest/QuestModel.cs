using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Quest
{
    internal class QuestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reward { get; set; }
        public List<QuestState> States { get; set; }
    }

    class QuestState
    {
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
