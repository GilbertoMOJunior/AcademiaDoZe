namespace AcademiaDoZe.Domain
{
    public struct Entrada
    {
        public Entrada(Pessoa pessoa, DateTime dataHora)
        {
            // if (!aluno.Matricula.Ativo)
            //throw new InvalidOperationException("Aluno não está matriculado ou matrícula não está ativa.");

            Pessoa = pessoa;
            DataHora = dataHora;
        }

        public Pessoa Pessoa { get; set; }
        public DateTime DataHora { get; set; }
    }

    public struct Saida
    {
        public Saida(Pessoa pessoa, DateTime dataHora)
        {
            Pessoa = pessoa;
            DataHora = dataHora;
        }

        public Pessoa Pessoa { get; set; }
        public DateTime DataHora { get; set; }
    }
}
