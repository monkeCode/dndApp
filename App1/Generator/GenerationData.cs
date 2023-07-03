using System;
using System.Collections.ObjectModel;

namespace App.Generator
{
    public class GenerationData
    {
        public ObservableCollection<Jewel> Gems { get; set; } = new ObservableCollection<Jewel>();
        public ObservableCollection<Jewel> Arts { get; set; } = new ObservableCollection<Jewel>();
        public ObservableCollection<GeneratedItem> Items { get; set; } = new ObservableCollection<GeneratedItem>();
        public IObservable<int> Copper { get; set; }
        public IObservable<int> Silver { get; set; }
        public IObservable<int> Electrum { get; set; }
        public IObservable<int> Gold { get; set; }
        public IObservable<int> Platinum { get; set; }
    }
}
