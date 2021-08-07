using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Text;
using DataBaseLib;

namespace App1
{
    class ExtendedMIviewModel : MagicItem
    {
        public ObservableCollection<MagicItemFeautes> Features { get; set; } = new ObservableCollection<MagicItemFeautes>();
        public string Description { get; set; }
        public string UnderType { get; set; }
        public string UnderQuality { get; set; }
        public ObservableCollection<Link> Links { get; set; } = new ObservableCollection<Link>();
         public Table Table { get; set; }
        public ExtendedMIviewModel(int id)
        {
           var item =  DataAccess.GetData("MagicItems", $"_id = {id}", "*")[0];
            Id = id;
            Name = item[1].ToString();
            Quality = ((ItemQuality)(long)item[2]).ToString();
            Type = item[3].ToString();
            //Attunement
            Description = "это ~Описание~ |подчеркнуто|";

            Features.Add(new MagicItemFeautes { Name = "типо имя", Desctipt = "я ~люблю~ |козу|, а ~|также|~ `|Корову|`" });
            Features.Add(new MagicItemFeautes { Name = "типо имя", Desctipt = "еще один ^гиперлинк^ fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff" });
            Features.Add(new MagicItemFeautes { Desctipt = "без названия" });
            Links.Add(new Link(typeof(MagicPage), 1, "гиперлинк"));

            Table = new Table()
            {
                Rows = 4,
                Columns = 2,

            };
            Table.Fields.Add("~Название~");
            Table.Fields.Add("~Действие~");
            Table.Fields.Add("название1");
            Table.Fields.Add("действие1");
            Table.Fields.Add("название2");
            Table.Fields.Add("действие2");
            Table.Fields.Add("название3");
            Table.Fields.Add("действие3   fgfffffffffffffff");
        }
    }
    class MagicItemFeautes
    {
        public string Name { get; set; }
        public string Desctipt { get; set; }
    }
}
