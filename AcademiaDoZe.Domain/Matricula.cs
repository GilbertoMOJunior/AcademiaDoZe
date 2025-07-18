namespace AcademiaDoZe.Domain
{
    public sealed class Matricula
    {
        public Matricula(Aluno aluno, PlanoMatricula plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, List<Restricao> restricoes)
        {
            if (aluno is null)
                throw new ArgumentNullException(nameof(aluno));

            if (aluno.Idade() < 12)
                throw new InvalidOperationException("Aluno deve ter pelo menos 12 anos para se matricular.");

            if (aluno.Idade() < 17 && (restricoes is null || !restricoes.Any()))
                throw new InvalidOperationException("Aluno menor de 17 anos deve possuir laudo medico para ser cadastrado.");

            if (restricoes.Any(x => x.Laudo == null))
                throw new InvalidOperationException("O Aluno deve apresentar um laudo caso tenha alguma restrição.");

            Aluno = aluno;
            Plano = plano;
            DataInicio = dataInicio;
            DataVencimento = dataFim;
            Objetivo = objetivo;
            Restricoes = restricoes;
        }

        public Aluno Aluno { get; set; }
        public PlanoMatricula Plano { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataVencimento { get; set; }
        public string Objetivo { get; set; }
        public List<Restricao>? Restricoes { get; set; } = new List<Restricao> { };
   
        public bool Ativo => DataVencimento >= DateOnly.FromDateTime(DateTime.Today);
    }

    public class Restricao
    {
        public string Observacao { get; set; }
        public string Laudo { get; set; }
    }
    public enum PlanoMatricula
    {
        Mensal = 1,
        Trimestral = 2,
        Semestral = 3,
        Anual = 4
    }
}

