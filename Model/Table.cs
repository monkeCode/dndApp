using System.Collections.Generic;

namespace Model
{
    public class Table
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<string> Fields { get; set; }

        public Table()
        {
            Fields = new List<string>();
        }
    }
}