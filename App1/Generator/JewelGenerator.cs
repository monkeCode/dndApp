using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Generator;

public class JewelGenerator 
{
    protected static List<Jewel> Generate(IReadOnlyList<string> gems, int count, int price)
    {
        List<string> g = new List<string>();
        for (int i = 0; i < count; i++)
        {
            g.Add(gems[new Random().Next(0, gems.Count)]);
        }
        return g.ToLookup(x => x, key => g.Count(i => i == key)).Select(it => new Jewel { Name = it.Key, Count = it.First(), Price = price }).ToList();
    }
}