using AcademiaDoZe.Domain.Enums;

namespace AcademiaDoZe.Domain
{
    public sealed class Matricula : Entity
    {
        public Matricula(Aluno aluno, EPlanoMatricula plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, ERestricaoMatricula? restricoes, Arquivo? laudo)
        {
            if (aluno is null)
                throw new ArgumentNullException(nameof(aluno));

            if (dataInicio == default || dataFim == default)
                throw new ArgumentException("Data de início e fim não podem ser nulas.", nameof(dataInicio));

            if (dataInicio > dataFim)
                throw new ArgumentException("Data de início não pode ser maior que a data de fim.", nameof(dataInicio));

            if (string.IsNullOrWhiteSpace(objetivo))
                throw new ArgumentException("Objetivo não pode ser vazio.", nameof(objetivo));

            if (aluno.Idade() < 12)
                throw new InvalidOperationException("Aluno deve ter pelo menos 12 anos para se matricular.");

            if (aluno.Idade() < 17 && laudo is null)
                throw new InvalidOperationException("Aluno menor de 17 anos deve possuir laudo medico para ser cadastrado.");

            if ((restricoes.HasValue && restricoes.Value != ERestricaoMatricula.Nenhuma) && laudo is null)
                throw new InvalidOperationException("Aluno com restrições deve possuir um laudo médico.");

            Aluno = aluno;
            Plano = plano;
            DataInicio = dataInicio;
            DataVencimento = dataFim;
            Objetivo = objetivo;
            Restricoes = restricoes;
        }

        public Aluno Aluno { get; set; }
        public EPlanoMatricula Plano { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataVencimento { get; set; }
        public string Objetivo { get; set; }
        public ERestricaoMatricula? Restricoes { get; set; }

        public Arquivo Laudo { get; set; }

        public bool Ativo => DataVencimento >= DateOnly.FromDateTime(DateTime.Today);
    }
}

