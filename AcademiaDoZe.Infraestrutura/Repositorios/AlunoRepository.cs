//Gilberto Mota de Oliveira Junior
using AcademiaDoZe.Domain;
using AcademiaDoZe.Domain.IRepositorios;
using AcademiaDoZe.Infraestrutura.Data;
using System.Data;
using System.Data.Common;

namespace AcademiaDoZe.Infraestrutura.Repositorios
{
    public class AlunoRepository : RepositorioBase<Aluno>, IAlunoRepository
    {
        public AlunoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType)
        {
        }

        public override async Task<Aluno> Adicionar(Aluno entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                ? $"INSERT INTO {TableName} (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, foto) "
                + "OUTPUT INSERTED.id_colaborador "
                + "VALUES (@Cpf, @Telefone, @Nome, @Nascimento, @Email, @LogradouroId, @Numero, @Complemento, @Senha, @Foto);"
                : $"INSERT INTO {TableName} (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, foto) "
                + "VALUES (@Cpf, @Telefone, @Nome, @Nascimento, @Email, @LogradouroId, @Numero, @Complemento, @Senha, @Foto); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", entity.Cpf, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Telefone", entity.Telefone, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Nascimento", entity.DataNascimento, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Email", entity.Email, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LogradouroId", entity.Endereco.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Numero", entity.Numero, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Complemento", (object)entity.Complemento ?? DBNull.Value, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Senha", entity.Senha, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Foto", (object)entity.Foto.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
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

        public override async Task<Aluno> Atualizar(Aluno entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} "
                + "SET cpf = @Cpf, "
                + "telefone = @Telefone, "
                + "nome = @Nome, "
                + "nascimento = @Nascimento, "
                + "email = @Email, "
                + "logradouro_id = @LogradouroId, "
                + "numero = @Numero, "
                + "complemento = @Complemento, "
                + "senha = @Senha, "
                + "foto = @Foto, "
                + "WHERE id_colaborador = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Cpf", entity.Cpf, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Telefone", entity.Telefone, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Nome", entity.Nome, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Nascimento", entity.DataNascimento, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Email", entity.Email, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@LogradouroId", entity.Endereco.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Numero", entity.Numero, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Complemento", (object)entity.Complemento ?? DBNull.Value, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Senha", entity.Senha, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Foto", (object)entity.Foto.Conteudo ?? DBNull.Value, DbType.Binary, _databaseType));
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

        public Task<bool> CpfJaExiste(string cpf, int? id = null)
        {
            throw new NotImplementedException();
        }

        public Task<Aluno?> ObterPorCpf(string cpf)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TrocarSenha(int id, string novaSenha)
        {
            throw new NotImplementedException();
        }

        protected override async Task<Aluno> MapAsync(DbDataReader reader)
        {
            try
            {
                // Obtém o logradouro de forma assíncrona
                var logradouroId = Convert.ToInt32(reader["logradouro_id"]);
                var logradouroRepository = new LogradouroRepository(_connectionString, _databaseType);
                var logradouro = await logradouroRepository.ObterPorId(logradouroId) ?? throw new InvalidOperationException($"Logradouro com ID {logradouroId} não encontrado.");
                // Cria o objeto Colaborador usando o método de fábrica
                var entity = Aluno.Criar(
                1,  
                cpf: reader["cpf"].ToString()!,
                telefone: reader["telefone"].ToString()!,
                nome: reader["nome"].ToString()!,
                dataNascimento: DateOnly.FromDateTime(Convert.ToDateTime(reader["nascimento"])),
                email: reader["email"].ToString()!,
                endereco: logradouro,
                numero: reader["numero"].ToString()!,
                complemento: reader["complemento"]?.ToString(),
                senha: reader["senha"].ToString()!,
                foto: reader["foto"] is DBNull ? null : Arquivo.Criar((byte[])reader["foto"], ".jpg")
                );
                // Define o ID usando reflection
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(entity, Convert.ToInt32(reader["id_aluno"]));
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do aluno: {ex.Message}", ex); }
        }
    }
}
