using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1;
using App1.Model;

namespace App.WorkShop
{
    internal class CreateMonsterVM
    {
        public ExtendedMonster Monster { get; set; }
        public bool IsTable { get; set; }

        private CustomCommand _featureCommand;
        private CustomCommand _actionCommand;
        private CustomCommand _reActionCommand;
        private CustomCommand _legendaryActionCommand;
        public CustomCommand FeatureCommand
        {
            get
            {
                return _featureCommand ??= new CustomCommand(
                    (p) => { Monster.Features.Add(new Features()); }
                );
            }
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

        public CreateMonsterVM()
        {
            Monster = new ExtendedMonster(1);
        }
        
    }
}
