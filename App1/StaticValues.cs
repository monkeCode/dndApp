using System.Collections.Generic;
using System.Linq;

namespace App1
{
    public class StaticValues
    {
        public readonly static Dictionary<string, int> magicItemQality = new Dictionary<string, int>()
        {
                    {"Обычный", 0 },
                    {"Необычный", 1 },
                    {"Редкий", 2 },
                    {"Крайне редкий", 3 },
                    {"Легендарный", 4 },
                    {"Артефакт", 5 },
                    {"Варьируется", 6 }
        };

        public static List<string> MagicItemsQuality { get => magicItemQality.Keys.ToList(); }

        public static List<string> MagicItemsType { get; } = new List<string>()
        {
             "Чудесный предмет",
              "Броня",
              "Оружие",
              "Жезл",
              "Кольцо",
              "Зелье",
              "Посох",
              "Свиток",
              "Скипетр"
        };

        public static List<string> MagicItemsSource { get; } = new List<string>()
        {
            "Dungeon master's guide",
            "Homebrew"
        };

        public StaticValues()
        {
        }
    }
}