using System.Data.Common;
using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.IRepositorios;
using AcademiaDoZe.Infraestrutura.Data;

namespace AcademiaDoZe.Infraestrutura.Repositorios
{
    public class RepositorioLogradouro : BaseRepository<Logradouro>, IRepositorioLogradouro
    {
        public RepositorioLogradouro(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }
        public override async Task<Logradouro> Adicionar(Logradouro entity)
        {
            return entity;
        }
        public override async Task<Logradouro> Atualizar(Logradouro entity)
        {
            return entity;
        }
        public async Task<Logradouro?> ObterPorCep(string cep)
        {
            return null;
        }
        public async Task<IEnumerable<Logradouro>> ObterPorCidade(string cidade)
        {
            return Enumerable.Empty<Logradouro>();
        }
        protected override async Task<Logradouro> MapAsync(DbDataReader reader)
        {
            try
            {
                var logradouro = Logradouro.Criar(
                cep: reader["cep"].ToString()!,
                nome: reader["nome"].ToString()!,
                bairro: reader["bairro"].ToString()!,
                cidade: reader["cidade"].ToString()!,
                estado: reader["estado"].ToString()!,
                pais: reader["pais"].ToString()!);
                // Usando reflexão para definir o ID, já que a propriedade Id é herdada e não tem setter público
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(logradouro, Convert.ToInt32(reader["id_logradouro"]));
                return logradouro;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do logradouro: {ex.Message}", ex); }
        }
    }
}
