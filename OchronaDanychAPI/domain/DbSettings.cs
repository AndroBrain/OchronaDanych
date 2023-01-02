using System.Reflection;

namespace ShopManagmentAPI.domain;

public class DbSettings
{
    public static readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OchronaDanych", "database.db");
}
