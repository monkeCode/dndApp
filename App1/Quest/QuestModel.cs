﻿using System.Collections.Generic;

namespace App.Quest
{
    internal class QuestModel
    {
        public enum States
        {
            NonActive,
            Active,
            Completed
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reward { get; set; }
        public States State { get; set; }
        public List<QuestTarget> Targets { get; set; }
    }

    class QuestTarget
    {
        public int QuestId { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
