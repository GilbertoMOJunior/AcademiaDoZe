using System.ComponentModel;

namespace AcademiaDoZe.Domain
{
    public abstract class Entity
    {
        public int Id { get; private set; }

        protected Entity(int id)
        {
            Id = id;
        }
    }
}
