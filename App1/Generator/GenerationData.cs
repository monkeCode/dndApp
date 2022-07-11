using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Generator
{
    public class GenerationData
    {
        public ObservableCollection<Gem> Gems { get; set; } = new ObservableCollection<Gem>();
        public ObservableCollection<Art> Arts { get; set; } = new ObservableCollection<Art>();
        public ObservableCollection<GeneratedItem> Items { get; set; } = new ObservableCollection<GeneratedItem>();
        public IObservable<int> Copper { get; set; }
        public IObservable<int> Silver { get; set; }
        public IObservable<int> Electrum { get; set; }
        public IObservable<int> Gold { get; set; }
        public IObservable<int> Platinum { get; set; }
    }
}
