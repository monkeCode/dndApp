using Model;

namespace App.WorkShop
{
    internal class CreateMonsterVM : CreateMv<ExtendedMonster>
    {
        public ExtendedMonster Monster => Item;
        public bool IsTable { get; set; }

        private CustomCommand _actionCommand;
        private CustomCommand _reActionCommand;
        private CustomCommand _legendaryActionCommand;

        public override void AddFeature()
        {
            Monster.Features.Add(new Feature());
        }

        public override async void Save()
        {
            if (!IsTable)
                Item.Table = null;
            if(!Monster.IsLegendary)
            {
                Monster.LegendaryActions.Clear();
                Monster.LairActions = "";
                Monster.RegionalEf = "";
            }

            if (isNew)
            {
                await App.DataContext.AddMonster(Item);
                return;
            }

            await App.DataContext.UpdateMonster(Item);
        }

        public CustomCommand ActionCommand
        {
            get
            {
                return _actionCommand ??= new CustomCommand(
                    (p) => { Monster.Actions.Add(new Feature()); }
                );
            }
        }
        public CustomCommand ReActionCommand
        {
            get
            {
                return _reActionCommand ??= new CustomCommand(
                    (p) => { Monster.ReciprocalActions.Add(new Feature()); }
                );
            }
        }
        public CustomCommand LegendaryActionCommand
        {
            get
            {
                return _legendaryActionCommand ??= new CustomCommand(
                    (p) => { Monster.LegendaryActions.Add(new Feature()); }
                );
            }
        }

        public CreateMonsterVM() : base(true)
        {
            Item = new ExtendedMonster();
            IsTable = false;
        }

        public CreateMonsterVM(int id, bool isN) : base(isN)
        {
            if(id == -1)
                return;
            Item = App.DataContext.GetExtendedMonsterById(id);
            IsTable = Item.Table != null;
        }

    }
}
