using App;
using Model;

namespace App.Generator
{
    public class GeneratedItem
    {
        public string Title { get; set; }
        public MagicItem MagicItem { get; set; }
        public GeneratedItem(string title, MagicItem magicItem)
        {
            Title = title;
            MagicItem = magicItem;
        }
    }
}
