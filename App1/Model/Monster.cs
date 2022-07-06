using App.Model;
using DataBaseLib;

namespace App1
{
    public class Monster : DataItem
    {
        public string Type { set; get; }
        public int Size { get; set; }
        public string Challenge { get; set; }
        public int Ex =>
            Challenge switch
            {
                "0" => 0,
                "1/8" => 25,
                "1/4" => 50,
                "1/2" => 100,
                "1" => 200,
                "2" => 450,
                "3" => 700,
                "4" => 1100,
                "5" => 1800,
                "6" => 2300,
                "7" => 2900,
                "8" => 3900,
                "9" => 5000,
                "10" => 5900,
                "11" => 7200,
                "12" => 8400,
                "13" => 10000,
                "14" => 11500,
                "15" => 13000,
                "16" => 15000,
                "17" => 18000,
                "18" => 20000,
                "19" => 22000,
                "20" => 25000,
                "21" => 33000,
                "22" => 41000,
                "23" => 50000,
                "24" => 62000,
                "25" => 75000,
                "26" => 90000,
                "27" => 105000,
                "28" => 120000,
                "29" => 135000,
                "30" => 155000
            };
        public string Source { get; set; }
        public bool isLegendary { get; set; }
        public string Habitat { get; set; }
        public Monster()
        {
            ItemType = DataType.Monster;
        }

        public Monster(int id) : this()
        {
            var list = DataAccess.GetData("Monsters", $"_id = {id}", null, "*")[0];
            Id = id;
            Name = list[1].ToString();
            Size = (int)(long)list[2];
            Type = list[3].ToString();
            Habitat = list[4].ToString();
            Challenge = list[5].ToString();
            isLegendary = list[6].ToString() == "1";
            Source = list[7].ToString();

        }
    }
}