using AcademiaDoZe.Domain.Repositorios;
using System.Data.Common;

namespace AcademiaDoZe.Domain.IRepositorios
{
    public interface IRepositorioAcesso : IRepositorio<Acesso>
    {
        Task<Acesso> Adicionar(Acesso entity);
        Task<Acesso> Atualizar(Acesso entity);
    }
}
