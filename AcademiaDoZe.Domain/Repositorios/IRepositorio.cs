using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe.Domain.Repositorios
{
    public interface IRepositorio<TEntity> where TEntity : Entity
    {
        abstract Task<TEntity> Adicionar(TEntity entidade);
        abstract Task<TEntity> Atualizar(TEntity entidade);
        abstract Task<bool> Remover(int id);
        abstract Task<TEntity> ObterPorId(int id);
        abstract Task<IEnumerable<TEntity>> ObterTodos();
    }
}
