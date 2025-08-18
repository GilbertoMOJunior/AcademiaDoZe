using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public partial class Acesso : Entity
    {
        private Acesso(Pessoa pessoa, DateTime dataHora, ETipoPessoa tipo)
        {
            Pessoa = pessoa;
            DataHora = dataHora;
            TipoPessoa = tipo;
        }

        public static Acesso Criar(Pessoa pessoa, DateTime dataHora, ETipoPessoa tipoPessoa)
        {
            if (pessoa is null)
                throw new DomainException("Pessoa não pode ser nula.");

            if (dataHora == default)
                throw new DomainException("Data e hora não podem ser nulas.");

            return new Acesso(pessoa, dataHora, tipoPessoa);
        }

        public Pessoa Pessoa { get; set; }
        public DateTime DataHora { get; set; }
        public ETipoPessoa TipoPessoa { get; set; }
    }
}
