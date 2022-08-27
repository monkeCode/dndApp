using App.Directories;
using System;
using Model;

namespace App
{
    public class Link
    {
        public Type Page { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public DataItem.DataType Type { get; set; }

        public DataItem Item
        {
            get
            {
                return Type switch
                {
                    DataItem.DataType.MagicItem => App.DataContext.GetExtendedMagicById(Id),
                    DataItem.DataType.Monster => App.DataContext.GetExtendedMonsterById(Id),
                    DataItem.DataType.Spell => App.DataContext.GetExtendedSpellById(Id),
                    _ => throw new Exception()

                };
            }
            set
            {
                Id = value.Id;
                Type = value.ItemType;
            }
        }

        public Link(DataItem.DataType type, int id, string str)
        {
            Id = id;
            Text = str;
            Page = GetPage(type);
            Type = (type);
        }

        public static DataItem.DataType GetDataType(string type)
        {
            return type switch
            {
                "MI" => DataItem.DataType.MagicItem,
                "MO" => DataItem.DataType.Monster,
                "SP" => DataItem.DataType.Spell,
                _ => throw new Exception()
            };
        }

        public static Type GetPage(string type)
        {
            return type switch
            {
                "MI" => typeof(MagicItemExtendedPage),
                "MO" => typeof(MonsterPage),
                "SP" => typeof(MagicItemExtendedPage),
                _ => throw new Exception()
            };
        }
        public static Type GetPage(DataItem.DataType type)
        {
            return type switch
            {
                DataItem.DataType.MagicItem => typeof(MagicItemExtendedPage),
                DataItem.DataType.Monster => typeof(MonsterPage),
                DataItem.DataType.Spell => typeof(MagicItemExtendedPage),
                _ => throw new Exception()
            };
        }
    }
}