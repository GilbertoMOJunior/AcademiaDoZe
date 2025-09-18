using AcademiaDoZe.Domain.Repositorios;

namespace AcademiaDoZe.Domain.IRepositorios
{
    public interface IColaboradorRepository : IRepositorio<Colaborador>
    {
        Task<Colaborador?> ObterPorCpf(string cpf);
        Task<bool> CpfJaExiste(string cpf, int? id = null);
        Task<bool> TrocarSenha(int id, string novaSenha);
    }
}
