﻿using System;
using DataBaseLib;
using System.Collections.ObjectModel;

namespace App1
{
    internal class ExtendedMagicItem : MagicItem
    {
        public ObservableCollection<Feautes> Features { get; set; } = new ObservableCollection<Feautes>();
        public string Description { get; set; }
        public string UnderType { get; set; }
        public string UnderQuality { get; set; }
        public string OptionableText { get; set; }
        public string ItemSource { get; set; }
        public ObservableCollection<Link> Links { get; set; } = new ObservableCollection<Link>();
        public Table Table { get; set; }

        public ExtendedMagicItem(int id)
        {
            object[] item = DataAccess.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id", null, "*")[0];
            Id = id;
            Name = item[1].ToString();
            Quality = (int)(long)item[2];
            Type = item[3].ToString();
            ItemSource = item[5].ToString();

            Description = item[7].ToString();
            if (item[8].ToString() != "")
                UnderType = "(" + item[8] + ")";
            if (item[9].ToString() != "")
                UnderQuality = "(" + item[9] + ")";
            Attunement = item[10].ToString();
            OptionableText = item[11].ToString();

            LoadTableFromDb(id);
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
    }

    internal class Feautes
    {
        public string Name { get; set; }
        public string Desctipt { get; set; }
    }
}