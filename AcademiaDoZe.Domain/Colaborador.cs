namespace AcademiaDoZe.Domain
{
    public sealed class Colaborador : Pessoa
    {
        public Colaborador(DateOnly dataAdmissao, TipoColaborador tipoColaborador, Vinculo vinculo,
            string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, string foto, Logradouro logradouro,
            string numero, string complemento) : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {
            DataAdmissao = dataAdmissao;
            TipoColaborador = tipoColaborador;
            Vinculo = vinculo;
        }

        public DateOnly DataAdmissao { get; set; }
        public TipoColaborador TipoColaborador { get; set; }
        public Vinculo Vinculo { get; set; }

        public Entrada RegistrarEntradaAluno(Aluno aluno)
        {
            try
            {
                var registro = new Entrada(aluno, DateTime.Now);
                return registro;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Erro ao registrar entrada do aluno: " + ex.Message);
            }
        }

        public Saida RegistrarSaidaAluno(Aluno aluno)
        {
            try
            {
                var registro = new Saida(aluno, DateTime.Now);
                return registro;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Erro ao registrar entrada do aluno: " + ex.Message);
            }

        }

        public Aluno CadastrarAluno(string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, string? foto, Logradouro logradouro, string numero, string? complemento)
        {
            if (this.TipoColaborador == TipoColaborador.Instrutor)
                throw new InvalidOperationException("Somente atendentes e administradores podem cadastrar alunos.");
            try
            {
                var novoAluno = new Aluno(cpf, nome, dataNascimento, email, telefone,
                    senha, foto ?? "", logradouro, numero, complemento ?? "");

                return novoAluno;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Erro ao cadastrar aluno: " + ex.Message);
            }
        }

        public Matricula MatricularAluno(Aluno aluno, PlanoMatricula plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, List<Restricao>? restricoes)
        {
            if (this.TipoColaborador == TipoColaborador.Instrutor)
                throw new InvalidOperationException("Somente atendentes e administradores podem cadastrar alunos.");
            
            try
            {
                var matricula = new Matricula(aluno, plano, dataInicio, dataFim, objetivo, restricoes);
                return matricula;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Erro ao matricular aluno: " + ex.Message);
            }
        }
    }

    public enum TipoColaborador
    {
        Administrador = 1,
        Atendente = 2,
        Instrutor = 3
    }

    public enum Vinculo
    {
        Clt = 1,
        Estagio = 2
    }
}
