using App1.WorkShop;

namespace App1;

internal class Features
{
    public string Name { get; set; }
    public string Description { get; set; }

    public void Save(string table, int parentId)
    {
        DataBaseLib.DataAccess.RawRequest($"INSERT INTO {table} (_id, Name, Description) " +
                                          $"values ({parentId},\'{Formator.CreateDbValidStr(Name)}\', \'{Formator.CreateDbValidStr(Description)}\')");
    }
}