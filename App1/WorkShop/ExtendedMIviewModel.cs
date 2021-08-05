using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Text;

namespace App1
{
    class ExtendedMIviewModel : MagicItem
    {
        public ObservableCollection<MagicItemFeautes> Features { get; set; } = new ObservableCollection<MagicItemFeautes>();

        public ExtendedMIviewModel(int id)
        {
            Features.Add(new MagicItemFeautes { Name = "Тест", Desctipt = "я ~люблю~ |козу|, а ~|также|~ `|Корову|`" });
            Name = "Имя";
            Type = "чудесный предмет";
            Quality = ItemQuality.legendary.ToString();
        }
    }
    class MagicItemFeautes
    {
        public string Name { get; set; }
        public string Desctipt { get; set; }
    }
}
