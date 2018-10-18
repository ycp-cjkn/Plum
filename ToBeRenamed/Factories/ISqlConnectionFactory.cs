using System.Data.Common;

namespace ToBeRenamed.Factories
{
    public interface ISqlConnectionFactory
    {
        DbConnection GetSqlConnection();
    }
}