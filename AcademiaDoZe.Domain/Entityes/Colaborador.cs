using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public sealed class Colaborador : Pessoa
    {
        private Colaborador(DateOnly dataAdmissao, ETipoColaborador tipoColaborador, EVinculoColaborador vinculo,
            string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, Arquivo? foto, Logradouro logradouro,
            string numero, string complemento) : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {

            DataAdmissao = dataAdmissao;
            TipoColaborador = tipoColaborador;
            Vinculo = vinculo;
        }

        public static Colaborador Criar(DateOnly dataAdmissao, ETipoColaborador tipoColaborador, EVinculoColaborador vinculo,
            string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, Arquivo? foto, Logradouro logradouro,
            string numero, string complemento)
        {

            if (dataAdmissao == default)
                throw new DomainException("Data de admissão não pode ser nula.");

            if (dataAdmissao > DateOnly.FromDateTime(DateTime.Now))
                throw new DomainException("Data de admissão não pode ser no futuro.");

            return new Colaborador(dataAdmissao, tipoColaborador, vinculo, cpf, nome, dataNascimento, email,
                telefone, senha, foto, logradouro, numero, complemento);
        }

        public DateOnly DataAdmissao { get; set; }
        public ETipoColaborador TipoColaborador { get; set; }
        public EVinculoColaborador Vinculo { get; set; }

        public Catraca RegistrarEntradaAluno(Aluno aluno)
        {
            try
            {
                var registro = Catraca.Criar(aluno, DateTime.Now);
                return registro;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Erro ao registrar entrada do aluno: " + ex.Message);
            }
        }

        public Catraca RegistrarSaidaAluno(Aluno aluno)
        {
            try
            {
                var registro = Catraca.Criar(aluno, DateTime.Now);
                return registro;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Erro ao registrar entrada do aluno: " + ex.Message);
            }

        }

        public Aluno CadastrarAluno(string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, Arquivo? foto, Logradouro logradouro, string numero, string? complemento)
        {
            if (this.TipoColaborador == ETipoColaborador.Instrutor)
                throw new InvalidOperationException("Somente atendentes e administradores podem cadastrar alunos.");
            try
            {
                var novoAluno = Aluno.Criar(cpf, nome, dataNascimento, email, telefone,
                    senha, foto, logradouro, numero, complemento ?? "");

                return novoAluno;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Erro ao cadastrar aluno: " + ex.Message);
            }
        }

        public Matricula MatricularAluno(Aluno aluno, EPlanoMatricula plano, DateOnly dataInicio, DateOnly dataFim, string objetivo, ERestricaoMatricula? restricoes, Arquivo? laudo)
        {
            if (this.TipoColaborador == ETipoColaborador.Instrutor)
                throw new InvalidOperationException("Somente atendentes e administradores podem cadastrar alunos.");

            try
            {
                var matricula = Matricula.Criar(aluno, plano, dataInicio, dataFim, objetivo, restricoes, laudo);
                return matricula;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Erro ao matricular aluno: " + ex.Message);
            }
        }
    }
}
