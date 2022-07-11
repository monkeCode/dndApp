using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Generator
{
    public class GemGenerator
    {
        private static readonly IReadOnlyList<string> GEMS10G = new List<string>
        {
            "Азурит (непрозрачный, пестрый темно-синий)",
            "Агат бастионный (полупрозрачный, синие, белые, красные или бурые полосы)",
            "Голубой кварц (прозрачный бледно-голубой)",
            "Агат глазковый (полупрозрачный, в кружках серого, белого, бурого, синего, или зеленого цвета)",
            "Гематит (непрозрачный темно-синий)",
            "Лазурит (непрозрачный светло-синий или темно-синий с желтыми вкраплениями)",
            "Малахит (непрозрачный, в полосках светло-зеленого или темно-зеленого)",
            "Моховой агат (полупрозрачный розовый или желто-зеленый с серыми или зелеными отметинами)",
            "Обсидиан (непрозрачный черный)",
            "Родохрозит (непрозачный светло-розовый)",
            "Тигровый глаз (полупрозрачный коричневый с золотой серцевиной)",
            "Бирюза (непрозрачная голубовато-зеленая)"
        };
       public static List<Gem> GenerateGemsBy10(int count)
        {
            return Generate(GEMS10G, count, 10);
        }

        private static List<Gem> Generate(IReadOnlyList<string> gems, int count, int price)
        {
            List<string> g = new List<string>();
            for (int i = 0; i < count; i++)
            {
                g.Add(gems[new Random().Next(0, GEMS10G.Count)]);
            }
            return g.ToLookup(x => x, key => g.Count(i => i == key)).Select(it => new Gem { Name = it.Key, Count = it.First(), Price = price }).ToList(); ; ;
        }
    }
}
