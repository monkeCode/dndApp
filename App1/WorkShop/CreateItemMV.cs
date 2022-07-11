using App.Model;
using App1;
using System.Linq;
using System.Threading.Tasks;
using DataBaseLib;

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

        public override async void Save()
        {
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
           if (!IsTable)
           {
               Table.DeleteTable("TablesMagicItems", Item.Id);
           }
           else
               Item.Table.UpdateTable("TablesMagicItems", Item.Id);

           await DataAccess.RawRequestAsync($"DELETE from FeaturesOfMagicItem where _id = {Item.Id}");
           Parallel.ForEach(Item.Features, (Features f) => f.Save("FeaturesOfMagicItem", Item.Id));

        }
    }
}
