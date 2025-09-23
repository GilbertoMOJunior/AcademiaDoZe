using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public sealed class Matricula : Entity
    {
        public Aluno Aluno { get; set; }
        public EMatriculaPlano Plano { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public string Objetivo { get; set; }
        public EMatriculaRestricoes? Restricoes { get; set; }
        public string ObservacoesRestricoes { get; private set; }

        public Arquivo? LaudoMedico { get; set; }

        public bool Ativo => DataFim >= DateOnly.FromDateTime(DateTime.Today);
        private Matricula(int id, Aluno aluno, EMatriculaPlano plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, EMatriculaRestricoes? restricoes, string observacoes, Arquivo? laudo) : base(id)
        {
            
            Aluno = aluno;
            Plano = plano;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Objetivo = objetivo;
            Restricoes = restricoes;
            ObservacoesRestricoes = observacoes;
            LaudoMedico = laudo;
        }

        public static Matricula Criar(int id, Aluno aluno, EMatriculaPlano plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, EMatriculaRestricoes? restricoes, string observacoes, Arquivo? laudo)
        {
            if (aluno is null)
                throw new DomainException(nameof(aluno));

            if (dataInicio == default || dataFim == default)
                throw new DomainException("Data de início e fim não podem ser nulas.");

            if (dataInicio > dataFim)
                throw new DomainException("Data de início não pode ser maior que a data de fim.");

            if (string.IsNullOrWhiteSpace(objetivo))
                throw new DomainException("Objetivo não pode ser vazio.");

            if (aluno.Idade() < 12)
                throw new DomainException("Aluno deve ter pelo menos 12 anos para se matricular.");

            if (aluno.Idade() < 17 && laudo is null)
                throw new DomainException("Aluno menor de 17 anos deve possuir laudo medico para ser cadastrado.");

            if ((restricoes.HasValue && restricoes.Value != EMatriculaRestricoes.Nenhuma) && laudo is null)
                throw new DomainException("Aluno com restrições deve possuir um laudo médico.");

            return new Matricula(id, aluno, plano, dataInicio, dataFim, objetivo, restricoes, observacoes,laudo);
        }
    }
}

