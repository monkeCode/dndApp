using App.Model;
using App1;
using System;

namespace App.WorkShop
{
    internal class CreateItemMV : CreateMv<ExtendedMagicItem>
    {

        public bool IsAttunemended { get; set; }
        public bool IsTable { get; set; }
        public CreateItemMV(int id) : base(false)
        {
            Item = new ExtendedMagicItem(id);
            IsTable = Item.Table != null;
        }

        public CreateItemMV() : base(true)
        {
            Item = new ExtendedMagicItem();
            IsAttunemended = Item.Attunement != "";
            IsTable = false;
        }

        public override void AddFeature()
        {
            Item.Features.Add(new Features());
        }

        public override void AddLink(DataItem item)
        {
            Item.Links.Add(new Link(item.ItemType, item.Id, ""));
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
    }
}
