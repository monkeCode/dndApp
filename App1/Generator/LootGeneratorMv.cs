using App1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Generator
{
    public class LootGeneratorMv
    {
        public GenerationData Data { get; set; } = new GenerationData();
        public List<GenerationData> TreasureData { get; set; } = new List<GenerationData>();
        private CustomCommand _buttonGem10Click;
       public CustomCommand ButtonGem10Click{ 
            get
            {
              _buttonGem10Click??= new CustomCommand(obj =>
                {
                    foreach(var gem in GemGenerator.GenerateGemsBy10(1))
                    {
                        AddGem(gem);

                    }
                }
                );
                return _buttonGem10Click;
            }
            }
        private void AddGem(Gem g)
        {
            if(Data.Gems.Count > 4)
            {
                Data.Gems.Remove(Data.Gems.Last());
            }
            Data.Gems.Insert(0, g);
        }
    }
}
