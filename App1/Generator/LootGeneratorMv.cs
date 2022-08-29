using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Generator
{
    public class LootGeneratorMv
    {
        public GenerationData Data { get; set; } = new GenerationData();
        public List<GenerationData> TreasureData { get; set; } = new List<GenerationData>();
        private CustomCommand _buttonGem10Click;
        public CustomCommand ButtonGem10Click
        {
            get
            {
                _buttonGem10Click ??= new CustomCommand(obj =>
                  {
                      Gem gem = null;
                      switch (obj.ToString())
                      {
                          case "10":
                              gem = GemGenerator.GenerateGemsBy10(1)[0];
                              break;
                          case "50":
                              gem = GemGenerator.GenerateGemsBy50(1)[0];
                              break;
                          case "100":
                              gem = GemGenerator.GenerateGemsBy100(1)[0];
                              break;
                          case "500":
                              gem = GemGenerator.GenerateGemsBy500(1)[0];
                              break;
                          case "1000":
                              gem = GemGenerator.GenerateGemsBy1000(1)[0];
                              break;
                          case "5000":
                              gem = GemGenerator.GenerateGemsBy5000(1)[0];
                              break;
                          default: throw new NotImplementedException();

                      }
                      AddGem(gem);

                  }
                  );
                return _buttonGem10Click;
            }
        }
        private void AddGem(Gem g)
        {
            if (Data.Gems.Count > 4)
            {
                Data.Gems.Remove(Data.Gems.Last());
            }
            Data.Gems.Insert(0, g);
        }
    }
}
