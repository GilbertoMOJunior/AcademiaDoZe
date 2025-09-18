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
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=AcademiaDoZe;User Id=sa;Password=V@$c0_1234;TrustServerCertificate=True;Encrypt=True;";
            //var connectionString = "Server=localhost\\SQLEXPRESS;Database=AcademiaDoZe;Persist Security Info=True;Integrated Security=SSPI;Trusted_Connection=yes;";
            //"Data Source=BR-OCIH-SPCDB01.dcndd.local;Initial Catalog=NDD_RETORNONFCom;connection timeout=550;Persist Security Info=True;Integrated Security=SSPI; Trusted_Connection=yes;MultipleActiveResultSets=true";
            return (connectionString, DatabaseType.SqlServer);

        }
        private (string ConnectionString, DatabaseType DatabaseType) CreateMySqlConfig()
        {
            var connectionString = "Server=localhost:3306;Database=db_academia_do_ze;User Id=root;Password=abcBolinhas12345;";

            return (connectionString, DatabaseType.MySql);

        }
    }
}