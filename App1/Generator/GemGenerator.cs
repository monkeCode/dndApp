﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        private static readonly IReadOnlyList<string> GEMS50G = new List<string>
        {
            "Гелиотроп (непрозрачный темно-серый с красными вкраплениями)",
            "Сердолик (непрозрачный, от оранжевого до красно-коричневого)",
            "Халцедон (непрозрачный белый)",
            "Хризопаз (полупрозрачный зеленый)",
            "Цитрин (прозрачный бледный желто-коричневый)",
            "Яшма (непрозрачная синяя, черная или коричневая)",
            "Лунный камень (полупрозрачный белый с бледно-голубым оттенком)",
            "Оникс (непрозрачный, полосы черного и белого или полностью черный или белый)",
            "Кварц (прозрачный белый, дымчато-серый или желтый)",
            "Сардоникс (непрозрачный, полосы красного и белого)",
            "Звезчатый розовый кварц (прозрачный розовый с белым центром в форме звезды)",
            "Циркон (прозрачный бледный сине-зеленый)"
        };
        private static readonly IReadOnlyList<string> GEMS100G = new List<string>
        {
            "Янтарь (прозрачный, от бледно-золотого до насыщенного-золотого)",
            "Аметист (прозрачный темно-фиолетовый)",
            "Хризоберилл (прозрачный, от желто-зеленого до бледно зеленого)",
            "Коралл (непрозрачный красный)",
            "Гранат (прозрачный красный, коричнево-зеленый или фиолетовый)",
            "Нефрит (полупрозрачный светло-зеленый, темно-зеленый или белый)",
            "Гагат (непрозрачный черный)",
            "Жемчуг (непрозрачный блестящий белый, желтый или розовый)",
            "Шпинель (прозрачная красная, красно-коричневая или темно-зеленая)",
            "Турмалин (прозрачный бледно-зеленый, синий, коричневый или красный)"
        };
        private static readonly IReadOnlyList<string> GEMS500G = new List<string>
        {
            "Александрит (прозрачный темно-зеленый)",
            "Аквамарин (прозрачный бледный сине-зеленый)",
            "Черный жемчуг (непрозрачный черный)",
            "Сапфировая шпинель (прозрачная темно-синяя)",
            "Хризолит (прозрачный насыщенный оливкого-зеленый)",
            "Топаз (прозрачный золотисто-желтый)"
        };
        private static readonly IReadOnlyList<string> GEMS1000G = new List<string>
        {
            "Черный опал (полупрозрачный темно-зеленый с черными и золотыми вкраплениями)",
            "Синий сапфир (прозрачный, от сине-белого до синего)",
            "Изумред (прозрачный темно зеленый)",
            "Огненный опал (полупрозрачный огненно-красный)",
            "Опал (полупрозрачный бледно-голубой с зелеными и золотистыми пятнышками)",
            "Звезчатый рубин (полупрозрачный рубин с белым центром в форме звезды)",
            "Звезчатый сапфир (полупрозрачный синий сапфир с белым центром в форме звезды)",
            "Желтый сапфир (прозрачный огненно-желтый или желто-зеленый)"
        };
        private static readonly IReadOnlyList<string> GEMS5000G = new List<string>
        {
            "Черный сапфир (полупрозрачный блестящий черный со светлыми вкраплениями)",
            "Алмаз (прозрачный сине-белый, желтый, розоватый, коричневый или синий)",
            "Гиацинт (прозрачный огненно-оранжевый)",
            "Рубин (прозрачный, от светло-красного до темно-красного)"

        };
        public static List<Gem> GenerateGemsBy10(int count)
        {
            return Generate(GEMS10G, count, 10);
        }
        public static List<Gem> GenerateGemsBy50(int count)
        {
            return Generate(GEMS50G, count, 50);
        }
        public static List<Gem> GenerateGemsBy100(int count)
        {
            return Generate(GEMS100G, count, 100);
        }
        public static List<Gem> GenerateGemsBy500(int count)
        {
            return Generate(GEMS500G, count, 500);
        }
        public static List<Gem> GenerateGemsBy1000(int count)
        {
            return Generate(GEMS1000G, count, 1000);
        }
        public static List<Gem> GenerateGemsBy5000(int count)
        {
            return Generate(GEMS5000G, count, 5000);
        }

        private static List<Gem> Generate(IReadOnlyList<string> gems, int count, int price)
        {
            List<string> g = new List<string>();
            for (int i = 0; i < count; i++)
            {
                g.Add(gems[new Random().Next(0, gems.Count)]);
            }
            return g.ToLookup(x => x, key => g.Count(i => i == key)).Select(it => new Gem { Name = it.Key, Count = it.First(), Price = price }).ToList(); ; ;
        }
    }
}
