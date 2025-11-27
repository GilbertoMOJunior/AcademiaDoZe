//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.IRepositorios;
using AcademiaDoZe.Infraestrutura.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infraestrutura.Repositorios
{
    public class MatriculaRepository : RepositorioBase<Matricula>, IMatriculaRepository
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
                ? $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, laudo_medico, obs_restricao) "
                + "OUTPUT INSERTED.id_matricula "
                + "VALUES (@Aluno_id, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricao_medica, @Laudo_medico, @Obs_restricao);"
                : $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, laudo_medico, obs_restricao) "
                + "VALUES (@Aluno_id, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricao_medica, @Laudo_medico, @Obs_restricao); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno_id", entity.Aluno.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricao_medica", entity.Restricoes.Value, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Laudo_medico", (object)entity.LaudoMedico?.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs_restricao", entity.ObservacoesRestricoes, DbType.String, _databaseType));
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
                + "SET aluno_id = @Aluno, "
                + "plano = @Plano, "
                + "data_inicio = @Data_inicio, "
                + "data_fim = @Data_fim, "
                + "objetivo = @Objetivo, "
                + "restricao_medica = @Restricao_medica, "
                + "laudo_medico = @Laudo_medico, "
                + "obs_restricao = @Obs_restricao "
                + "WHERE id_matricula = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno", entity.Aluno.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricao_medica", entity.Restricoes.Value, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Laudo_medico", (object)entity.LaudoMedico?.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs_restricao", entity.ObservacoesRestricoes, DbType.String, _databaseType));
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

        public async Task<IEnumerable<Matricula>> ObterAtivas(int alunoId = 0)
        {
            var matriculas = new List<Matricula>();

            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query =
                    $"SELECT * FROM {TableName} " +
                    "WHERE data_fim >= @Hoje " +
                    (alunoId > 0 ? "AND aluno_id = @AlunoId " : "");

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Hoje", DateTime.Today, DbType.Date, _databaseType));

                if (alunoId > 0)
                    command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId, DbType.Int32, _databaseType));

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    matriculas.Add(await MapAsync(reader));

                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matrículas ativas: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId)
        {
            var matriculas = new List<Matricula>();

            try
            {
                await using var connection = await GetOpenConnectionAsync();

                string query =
                    $"SELECT * FROM {TableName} " +
                    "WHERE aluno_id = @AlunoId";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@AlunoId", alunoId, DbType.Int32, _databaseType));

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    matriculas.Add(await MapAsync(reader));

                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matrículas por aluno (ID: {alunoId}): {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias)
        {
            var matriculas = new List<Matricula>();

            try
            {
                await using var connection = await GetOpenConnectionAsync();

                DateTime limite = DateTime.Today.AddDays(dias);

                string query =
                    $"SELECT * FROM {TableName} " +
                    "WHERE data_fim BETWEEN @Hoje AND @Limite";

                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Hoje", DateTime.Today, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Limite", limite, DbType.Date, _databaseType));

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    matriculas.Add(await MapAsync(reader));

                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matrículas vencendo em {dias} dias: {ex.Message}", ex);
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
                    id: Convert.ToInt32(reader["id_matricula"]),
                    aluno: aluno,
                    plano: (EMatriculaPlano)Convert.ToInt32(reader["plano"]),
                    dataInicio: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    dataFim: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    objetivo: reader["objetivo"].ToString(),
                    restricoes: (EMatriculaRestricoes)Convert.ToInt32(reader["restricao_medica"]),
                    laudo: reader["laudo_medico"] is DBNull ? null : Arquivo.Criar((byte[])reader["laudo_medico"], ".jpg"),
                    observacoes: reader["obs_restricao"]?.ToString() ?? string.Empty
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
