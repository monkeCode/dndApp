using Model;
namespace App.WorkShop
{
    internal class CreateItemMV : CreateMv<ExtendedMagicItem>
    {

        public bool IsAttunemended { get; set; }
        public bool IsTable { get; set; }
        public CreateItemMV(int id) : base(false)
        {
            Item = App.DataContext.GetExtendedMagicById(id);
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
            Item.Features.Add(new Feature());
        }

        public override async void Save()
        {
            if (!IsAttunemended)
                Item.Attunement = "";
            else if (IsAttunemended && string.IsNullOrEmpty(Item?.Attunement?.Trim()))
                Item.Attunement = "требует настроенности";
            if (!IsTable)
                Item.Table = null;
            if (isNew)
            {
                await App.DataContext.AddItem(Item);
                return;
            }
            await App.DataContext.UpdateItem(Item);
        }
    }
}
