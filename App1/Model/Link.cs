using App1.Directories;
using System;
using System.Linq;
using App.Model;
using DataBaseLib;

namespace App1
{
    public class Link
    {
        public Type Page { get;  set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public DataItem.DataType Type { get; set; }

        public DataItem Item
        {
            get
            {
                return Type switch
                {
                    DataItem.DataType.MagicItem => new MagicItem(Id),
                    DataItem.DataType.Monster => new Monster(Id),
                    DataItem.DataType.Spell => new Spell(Id),
                    _ => throw new Exception()

                };
            }
            set
            {
                Id = value.Id;
                Type = value.ItemType;
            }
    }

        public Link(string type, int id, string str)
        {
            Id = id;
            Text = str;
            Page = GetPage(type);
            Type = GetDataType(type);
        }

        private static DataItem.DataType GetDataType(string type)
        {
            return type switch
            {
                "MI" => DataItem.DataType.MagicItem,
                "MO" => DataItem.DataType.Monster,
                "SP" => DataItem.DataType.Spell,
                _ => throw new Exception()
            };
        }

        private static Type GetPage(string type)
        {
            return type switch
            {
                "MI" => typeof(MagicItemExtendedPage),
                "MO" => typeof(MonsterPage),
                "SP" => typeof(MagicItemExtendedPage),
                _ => throw new Exception()
            };
        }
    }
}