using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public class Catraca : Entity
    {
        private Catraca(Pessoa pessoa, DateTime dataHora)
        {
            Pessoa = pessoa;
            DataHora = dataHora;
        }

        public static Catraca Criar(Pessoa pessoa, DateTime dataHora)
        {
            if (pessoa is null)
                throw new DomainException("Pessoa não pode ser nula.");

            if (dataHora == default)
                throw new DomainException("Data e hora não podem ser nulas.");

            return new Catraca(pessoa, dataHora);
        }

        public Pessoa Pessoa { get; set; }
        public DateTime DataHora { get; set; }
    }
}
