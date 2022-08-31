using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace App.WorkShop
{

    internal class WorkShopModelView
    {
        public ObservableCollection<CompletedDataItem> MagicItems { get; set; }
        public ObservableCollection<CompletedDataItem> Monsters { get; set; }
        public ObservableCollection<CompletedDataItem> Spells { get; set; }

        public WorkShopModelView()
        {
            MagicItems = new ObservableCollection<CompletedDataItem>();
            Monsters = new ObservableCollection<CompletedDataItem>();
            IEnumerable<CompletedDataItem> completedItems;

#if !DEBUG
            
               var list = new List<CompletedDataItem>();
            foreach (var item in App.DataContext.GetCompletedItems())
            {
               list.Add(new CompletedDataItem()
               {
                   Id = item.Id,
                   IsHomebrew = item.IsHomebrew,
                   ItemType = item.ItemType,
                   Name = item.Name,
                   Color = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255))
               });
            }

            completedItems = list.Where(it => it.IsHomebrew);

#else
            completedItems = App.DataContext.GetCompletedItems();
#endif
            foreach (var it in completedItems)
            {
                switch (it.ItemType)
                {
                    case DataItem.DataType.MagicItem:
                        MagicItems.Add(it);
                        break;
                    case DataItem.DataType.Monster:
                        Monsters.Add(it);
                        break;
                    case DataItem.DataType.Spell:
                        break;
                }
                
            }
        }

        public void Delete(DataItem item)
        {
            switch (item.ItemType)
            {
                case DataItem.DataType.MagicItem:
                    MagicItems.Remove(MagicItems.First(i => i.Id == item.Id));
                    App.DataContext.DeleteItem(item.Id);
                    break;
                case DataItem.DataType.Monster:
                    Monsters.Remove(Monsters.First(i => i.Id == item.Id));
                    App.DataContext.DeleteMonster(item.Id);
                    break;
                case DataItem.DataType.Spell:
                    Spells.Remove(Spells.First(it => item.Id == it.Id));
                    App.DataContext.DeleteSpell(item.Id);
                    break;
            }
        }
    }
}
