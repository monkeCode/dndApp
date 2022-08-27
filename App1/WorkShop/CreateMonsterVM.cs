using App;
using Model;
using System;

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

        public override void Save()
        {
            throw new NotImplementedException();
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
            Item = App.DataContext.GetExtendedMonsterById(1);
        }

    }
}
