using DataBaseLib;
using System;
using System.Collections.ObjectModel;

namespace App1
{
    internal class ExtendedMagicItem : MagicItem
    {
        public ObservableCollection<Features> Features { get; set; } = new ObservableCollection<Features>();
        public string Description { get; set; }
        public string UnderType { get; set; }
        public string UnderQuality { get; set; }
        public string OptionableText { get; set; }
        //public string ItemSource { get; set; }
        public Table Table { get; set; }

        public ExtendedMagicItem(int id)
        {
            object[] item = DataAccess.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id", null, "*")[0];
            Id = id;
            Name = item[1].ToString();
            Quality = (int)(long)item[2];
            Type = item[3].ToString();
            ItemSource = item[5].ToString();
            Description = item[8].ToString();
            if (item[9].ToString() != "")
                UnderType = item[9].ToString();
            if (item[10].ToString() != "")
                UnderQuality = item[10].ToString();
            if (item[11].ToString() != "")
                Attunement = item[11].ToString();
            else
            {
                Attunement = item[4].ToString() != "0" ? "требует настроенности" : "";
            }
            OptionableText = item[12].ToString();

            LoadTableFromDb(id);
            LoadFeatures(id);
        }

        public ExtendedMagicItem()
        {

        }
        private void LoadTableFromDb(int id)
        {
            try
            {
                var data = DataAccess.GetData("TablesMagicItems", $"ParentId = {id}", null, "*");
                Table = new Table(data[0]);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LoadFeatures(int id)
        {
            foreach (var item in DataAccess.GetData("FeaturesOfMagicItem", $"_id = {id}", null, "Name", "Description"))
            {
                Features.Add(new Features()
                {
                    Name = item[0].ToString(),
                    Description = item[1].ToString()
                });
            }

        }

    }
}