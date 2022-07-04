using App.Model;
using App1;
using System.Linq;

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

        public override async void Save()
        {
            if (!IsTable)
                Item.Table = null;
            if (!IsAttunemended)
                Item.Attunement = "";
            else if (IsAttunemended && Item.Attunement.Trim() == string.Empty)
                Item.Attunement = "требует настроенности";
            if (Item.ItemSource == "Dungeon master\'s guide")
                Item.ItemSource = "Dungeon master\'\'s guide";
            (await DataBaseLib.DataAccess.RawRequestAsync($"UPDATE MagicItems " +
                $"SET Name = \'{Item.Name}\', " +
                $"Quality = {Item.Quality}, " +
                $"Type = \'{Item.Type}\', " +
                $"Attunement = \'{((Item.Attunement != string.Empty)?1:0)}\', " +
                $"Source = \'{Item.ItemSource}\', " +
                $"isHomeBrew = 0 " +
                $"Where _id = {Item.Id}")).Close();
           (await DataBaseLib.DataAccess.RawRequestAsync("UPDATE ExtendedMagicItems SET " +
                $"Description = \'{Item.Description}\', " +
                $"Undertype = \'{Item.UnderType}\'," +
                $"UnderQuality = \'{Item.UnderQuality}\', " +
                $"Attunement = \'{Item.Attunement}\', " +
                $"OptionalText = \'{Item.OptionableText}\' " +
                $"Where _id = {Item.Id}")).Close();
            
        }

        public override void DeleteLink(Link link)
        {
            Item.Links.Remove(link);
        }
    }
}
