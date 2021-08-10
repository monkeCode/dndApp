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
        public string OptionableText { get; set; }
        public string Sourse { get; set; }
        public ObservableCollection<Link> Links { get; set; } = new ObservableCollection<Link>();
        public Table Table { get; set; }
        public ExtendedMIviewModel(int id)
        {
            object[] item =  DataAccess.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id",null, "*")[0];
            Id = id;
            Name = item[1].ToString();
            Quality = ((ItemQuality)(long)item[2]).ToString();
            Type = item[3].ToString();
            Sourse = item[5].ToString();

            Description = item[7].ToString();
            if (item[8].ToString() != "")
                UnderType = "(" + item[8].ToString() + ")";
            if (item[9].ToString() != "")
                UnderQuality = "(" + item[9].ToString() + ")";
            Attunement = item[10].ToString();
            OptionableText = item[11].ToString();
        }
    }
    class MagicItemFeautes
    {
        public string Name { get; set; }
        public string Desctipt { get; set; }
    }
}
