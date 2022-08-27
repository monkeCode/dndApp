using System.Collections.Generic;

namespace Model
{
    public class Player
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public string Class { get; set; }

        public int Lvl => Experience switch
        {
            >= 355000 => 20,
            >= 305000 => 19,
            >= 265000 => 18,
            >= 225000 => 17,
            >= 195000 => 16,
            >= 165000 => 15,
            >= 140000 => 14,
            >= 120000 => 13,
            >= 100000 => 12,
            >= 85000 => 11,
            >= 64000 => 10,
            >= 48000 => 9,
            >= 34000 => 8,
            >= 23000 => 7,
            >= 14000 => 6,
            >= 6500 => 5,
            >= 2700 => 4,
            >= 900 => 3,
            >= 300 => 2,
            _=>1
        };

        public int Experience { get; set; } = 0;
        public int AC { get; set; }
        public int HP { get; set; }
        public int PassWis { get; set; }
        public int Initiative { get; set; }
        public int GroupId { get; set; }
        public string Race { get; set; }
    }
}