//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Infraestrutura.Data;

namespace AcademiaDoZe.Infra.Tests
{
    public abstract class TestBase
    {
        protected string ConnectionString { get; private set; }
        protected DatabaseType DatabaseType { get; private set; }
        protected TestBase()
        {
            var config = CreateSqlServerConfig();
            //var config = CreateMySqlConfig();
            ConnectionString = config.ConnectionString;
            DatabaseType = config.DatabaseType;
        }
        private (string ConnectionString, DatabaseType DatabaseType) CreateSqlServerConfig()
        {
            var connectionString = "Server=localhost,1433;Database=db_academia_do_ze;User Id=sa;Password=abcBolinhas12345;TrustServerCertificate=True;Encrypt=True;";

            return (connectionString, DatabaseType.SqlServer);

        }
        private (string ConnectionString, DatabaseType DatabaseType) CreateMySqlConfig()
        {
            var connectionString = "Server=localhost:3306;Database=db_academia_do_ze;User Id=root;Password=abcBolinhas12345;";

            return (connectionString, DatabaseType.MySql);

        }
    }
}