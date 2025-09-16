//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.IRepositorios;
using AcademiaDoZe.Infraestrutura.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infraestrutura.Repositorios
{
    public class MatriculaRepository : RepositorioBase<Matricula>, IRepositorioMatricula
    {
        public MatriculaRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType)
        {
        }

        public override async Task<Matricula> Adicionar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                ? $"INSERT INTO {TableName} (aluno, plano, data_inicio, data_fim, objetivo, restricoes_medicas, laudo_medico, observacoes_restricoes) "
                + "OUTPUT INSERTED.id_matricula "
                + "VALUES (@Aluno, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricoes_medicas, @Laudo_medico, @Observacoes_restricoes);"
                : $"INSERT INTO {TableName} (aluno, plano, data_inicio, data_fim, objetivo, restricoes_medicas, laudo_medico, observacoes_restricoes) "
                + "VALUES (@Aluno, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricoes_medicas, @Laudo_medico, @Observacoes_restricoes); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno", entity.Aluno.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", entity.Plano, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataVencimento, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricoes_medicas", entity.Restricoes.Value, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Laudo_medico", entity.Laudo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Observacoes_restricoes", (object)entity.ObservacoesRestricoes ?? DBNull.Value, DbType.String, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar matricula: {ex.Message}", ex); }
        }

        public override async Task<Matricula> Atualizar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} "
                + "SET aluno = @Aluno, "
                + "plano = @Plano, "
                + "data_inicio = @Data_inicio, "
                + "data_fim = @Data_fim, "
                + "objetivo = @Objetivo, "
                + "restricoes_medicas = @Restricoes_medicas, "
                + "laudo_medico = @Laudo_medico, "
                + "observacoes_restricoes = @Observacoes_restricoes "
                + "WHERE id_matrcula = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno", entity.Aluno.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", entity.Plano, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataVencimento, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricoes_medicas", entity.Restricoes, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Laudo_medico", entity.Laudo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Observacoes_restricoes", (object)entity.ObservacoesRestricoes ?? DBNull.Value, DbType.String, _databaseType));
                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhuma matricula encontrado com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar matrcula com ID {entity.Id}: {ex.Message}", ex);
            }
        }

        protected override async Task<Matricula> MapAsync(DbDataReader reader)
        {
            try
            {
                // Obtém o aluno de forma assíncrona
                var alunoId = Convert.ToInt32(reader["aluno_id"]);
                var alunoRepository = new AlunoRepository(_connectionString, _databaseType);
                var aluno = await alunoRepository.ObterPorId(alunoId) ?? throw new InvalidOperationException($"Aluno com ID {alunoId} não encontrado.");
                // Cria o objeto Matricula usando o método de fábrica
                var matricula = Matricula.Criar(
                    aluno: aluno,
                    plano: (EPlanoMatricula)Convert.ToInt32(reader["plano"]),
                    dataInicio: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    dataFim: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    objetivo: reader["objetivo"].ToString(),
                    restricoes: (ERestricaoMatricula)Convert.ToInt32(reader["restricoes_medicas"]),
                    laudo: reader["laudo_medico"] is DBNull ? null : Arquivo.Criar((byte[])reader["laudo_medico"], ".jpg"),
                    observacoes: reader["observacoes_restricoes"]?.ToString() ?? string.Empty
                );
                // Define o ID usando reflection
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(matricula, Convert.ToInt32(reader["id_matricula"]));

                return matricula;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do colaborador: {ex.Message}", ex); }
        }
    }
}
