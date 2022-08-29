using Model;
using System;
using System.Collections.Generic;

namespace App.Generator
{
    internal class ItemGenerator
    {
        private string _title;
        private int _id;
        public ItemGenerator(int itemId, string title = null)
        {
            _title = title;
            _id = itemId;
        }
        protected ItemGenerator() { }
        public virtual GeneratedItem GetItem()
        {
            MagicItem magicItem = App.DataContext.GetExtendedMagicById(_id);
            _title ??= magicItem.Name;
            return new GeneratedItem(_title, magicItem);
        }
    }
    class ItemRangeGenerator : ItemGenerator
    {
        private IList<ItemGenerator> _items;
        public ItemRangeGenerator(IList<ItemGenerator> items)
        {
            this._items = items;
        }
        public override GeneratedItem GetItem()
        {
            return _items[new Random().Next(0, _items.Count)].GetItem();
        }
    }
}
