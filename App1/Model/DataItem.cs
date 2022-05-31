using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseLib;

namespace App.Model
{
    public class DataItem
    {
        public enum DataType
        {
            MagicItem,Monster,Spell
        }
        public string Name { get; set; }
        public DataType ItemType { get; set; }
        public int Id { get; set; }

        //public static List<DataItem> GetAll()
        //{
        //    DataAccess.GetData()
        //}
    }
}
