using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1;
using App1.Model;

namespace App.WorkShop
{
    internal class CreateMonsterVM:CreateMv<ExtendedMonster>
    {
        public ExtendedMonster Monster => Item;
        public bool IsTable { get; set; }

        private CustomCommand _actionCommand;
        private CustomCommand _reActionCommand;
        private CustomCommand _legendaryActionCommand;

        public override void AddFeature()
        {
            Monster.Features.Add(new Features());
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
                    (p) => { Monster.Actions.Add(new Features()); }
                );
            }
        }
        public CustomCommand ReActionCommand
        {
            get
            {
                return _reActionCommand ??= new CustomCommand(
                    (p) => { Monster.ReciprocalActions.Add(new Features()); }
                );
            }
        }
        public CustomCommand LegendaryActionCommand
        {
            get
            {
                return _legendaryActionCommand ??= new CustomCommand(
                    (p) => { Monster.LegendaryActions.Add(new Features()); }
                );
            }
        }

        public CreateMonsterVM():base(true)
        {
            Item = new ExtendedMonster(1);
        }
        
    }
}
