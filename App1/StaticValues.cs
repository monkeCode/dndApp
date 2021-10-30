using System.Collections.Generic;
using System.Linq;
using Windows.Networking.PushNotifications;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

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

        public static List<string> MonsterType { get; } = new List<string>()
        {
            "Абберация",
            "Бестия",
            "Великан",
            "Гуманоид",
            "Дракон",
            "Жижа",
            "Животное",
            "Конструкция",
            "Монстр",
            "Небожитель",
            "Нежить",
            "Растение",
            "Фея",
            "Элементаль"
        };

        public readonly static Dictionary<string, int> monsterSize = new Dictionary<string, int>()
        {
            {"Маленький", 0},
            {"Небольшой", 1},
            {"Средний", 2},
            {"Крупный", 3},
            {"Огромный", 4},
            {"Исполинский", 5},
        };
        public  static List<string> MonsterSize
        {
            get => monsterSize.Keys.ToList();
        }
        public static List<string> MonsterRate { get; } = new List<string>()
        {
            "0",
            "1/8",
            "1/4",
            "1/2",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"
        };

        public static List<string> MonsterSource { get; } = new List<string>()
        {
            "Monster manual",
            "Homebrew"
        };

        public static List<string> MonsterHabitat { get; } = new List<string>()
        {

        };
        public StaticValues()
        {
        }
    }
}