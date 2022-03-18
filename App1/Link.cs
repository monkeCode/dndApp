using App1.Directories;
using System;

namespace App1
{
    public class Link
    {
        public Type Page { get; private set; }
        public int Id { get; private set; }
        public string Text { get; private set; }

        public Link(string type, int id, string str)
        {
            Id = id;
            Text = str;
            Page = type switch
            {
                "MI" => typeof(MagicItemExtendedPage),
                "MO" => throw new Exception(),
                "MA" => typeof(MagicItemExtendedPage)
            };
        }

    }
}