//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.IRepositorios;
using AcademiaDoZe.Infraestrutura.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infraestrutura.Repositorios
{
    public class RepositorioAcesso : RepositorioBase<Acesso>, IRepositorioAcesso
    {
        public RepositorioAcesso(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType)
        {
        }

        public override async Task<Acesso> Adicionar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                ? $"INSERT INTO {TableName} (pessoa_tipo, pessoa_id, data_hora) "
                + "OUTPUT INSERTED.id_colaborador "
                + "VALUES (@Pessoa_tipo, @Pessoa_id, @Data_hora);"
                : $"INSERT INTO {TableName} (pessoa_tipo, pessoa_id, data_hora) "
                + "VALUES (@Pessoa_tipo, @Pessoa_id, @Data_hora); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_tipo", entity.TipoPessoa, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_id", entity.Pessoa.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.DateTime2, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar aluno: {ex.Message}", ex); }
        }

        public override async Task<Acesso> Atualizar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} "
                + "SET pessoa_tipo = @Pessoa_tipo, "
                + "pessoa_id = @Pessoa_id, "
                + "data_hora = @Data_hora, "
                + "WHERE id_acesso = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_tipo", entity.TipoPessoa, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_id", entity.Pessoa.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.DateTime2, _databaseType));
                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhum aluno encontrado com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar aluno com ID {entity.Id}: {ex.Message}", ex);
            }
        }

        protected override async Task<Acesso> MapAsync(DbDataReader reader)
        {
            try
            {
                // Obtém o logradouro de forma assíncrona
                var tipoPessoa = (ETipoPessoa)Convert.ToInt32(reader["pessoa_tipo"]);
                var pessoaId = Convert.ToInt32(reader["pessoa_id"]);

                if (tipoPessoa == ETipoPessoa.Aluno)
                {

                    var AlunoRepository = new RepositorioAluno(_connectionString, _databaseType);
                    var pessoa = await AlunoRepository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrado.");
                    // Cria o objeto Colaborador usando o método de fábrica
                    var entity = Acesso.Criar(
                    tipoPessoa: tipoPessoa!,
                    pessoa: pessoa!,
                    dataHora: Convert.ToDateTime(reader["data_hora"])!
                    );
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(reader["id_aluno"]));
                    
                    return entity;
                }
                else if (tipoPessoa == ETipoPessoa.Colaborador)
                {
                    var ColaboradorRepository = new RepositorioColaborador(_connectionString, _databaseType);
                    var pessoa = await ColaboradorRepository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrado.");
                    // Cria o objeto Colaborador usando o método de fábrica
                    var entity = Acesso.Criar(
                    tipoPessoa: tipoPessoa!,
                    pessoa: pessoa!,
                    dataHora: Convert.ToDateTime(reader["data_hora"])!
                    );
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(reader["id_aluno"]));
                    
                    return entity;
                }
                else                 
                {
                    throw new InvalidOperationException($"Tipo de pessoa {tipoPessoa} não suportado.");
                }

            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do aluno: {ex.Message}", ex); }
        }
    }
}
