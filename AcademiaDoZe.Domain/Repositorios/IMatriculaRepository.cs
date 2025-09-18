using AcademiaDoZe.Domain.Repositorios;
namespace AcademiaDoZe.Domain.IRepositorios
{
    public interface IMatriculaRepository : IRepositorio<Matricula>
    {
        Task<IEnumerable<Matricula>> ObterPorAluno(int alunoId);
        Task<IEnumerable<Matricula>> ObterAtivas(int alunoId = 0);
        Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
    }
}
