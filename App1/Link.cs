using System;

namespace App1
{
    public class Link
    {
        public Type Page { get; private set; }
        public int Id { get; private set; }
        public string Text { get; private set; }
        public Link(Type type, int id, string str)
        {
            Page = type;
            Id = id;
            Text = str;
        }
    }
}