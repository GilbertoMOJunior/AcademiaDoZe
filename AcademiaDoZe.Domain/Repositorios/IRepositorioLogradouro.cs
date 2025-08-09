using System.Data.Common;
using AcademiaDoZe.Domain.Repositorios;

namespace AcademiaDoZe.Domain.IRepositorios
{
    public interface IRepositorioLogradouro : IRepositorio<Logradouro>
    {
        public Task<Logradouro?> ObterPorCep(string cep);
        public Task<IEnumerable<Logradouro>> ObterPorCidade(string cidade);
    }
}
