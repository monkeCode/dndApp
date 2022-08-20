using App.Model;
using App1;
using System.Linq;
using System.Threading.Tasks;
using App1.WorkShop;
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
            IsAttunemended = Item.Attunement != "";
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
            (await DataBaseLib.DataAccess.RawRequestAsync($"UPDATE MagicItems " +
                                                          $"SET Name = \'{Formator.CreateDbValidStr(Item.Name)}\', " +
                                                          $"Quality = {Item.Quality}, " +
                                                          $"Type = \'{Item.Type}\', " +
                                                          $"Attunement = \'{((Item.Attunement != string.Empty)?1:0)}\', " +
                                                          $"Source = \'{Formator.CreateDbValidStr(Item.ItemSource)}\', " +
                                                          $"isHomeBrew = 0 " +
                                                          $"Where _id = {Item.Id}")).Close();
           (await DataBaseLib.DataAccess.RawRequestAsync("UPDATE ExtendedMagicItems SET " +
                $"Description = \'{Formator.CreateDbValidStr(Item.Description)}\', " +
                $"Undertype = \'{Formator.CreateDbValidStr(Item.UnderType)}\'," +
                $"UnderQuality = \'{Formator.CreateDbValidStr(Item.UnderQuality)}\', " +
                $"Attunement = \'{Formator.CreateDbValidStr(Item.Attunement)}\', " +
                $"OptionalText = \'{Formator.CreateDbValidStr(Item.OptionableText)}\' " +
                $"Where _id = {Item.Id}")).Close();
           if (!IsTable)
           {
               Table.DeleteTable("TablesMagicItems", Item.Id);
           }
           else
               Item.Table.UpdateTable("TablesMagicItems", Item.Id);

           await DataAccess.RawRequestAsync($"DELETE from FeaturesOfMagicItem where _id = {Item.Id}"); 
           await Task.Run(() =>
           {
               foreach (var feature in Item.Features)
               {
                   feature.Save("FeaturesOfMagicItem", Item.Id);
               }
           });
        }
    }
}
