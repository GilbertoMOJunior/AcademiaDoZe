using AcademiaDoZe.Domain.Repositorios;

namespace AcademiaDoZe.Domain.IRepositorios
{
    public interface IAlunoRepository : IRepositorio<Aluno>
    {
        Task<Aluno?> ObterPorCpf(string cpf);
        Task<bool> CpfJaExiste(string cpf, int? id = null);
        Task<bool> TrocarSenha(int id, string novaSenha);
    }
}
