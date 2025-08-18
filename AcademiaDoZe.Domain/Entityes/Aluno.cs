using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public sealed class Aluno : Pessoa
    {
        private Aluno(string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, Arquivo? foto, Logradouro logradouro, string numero, string? complemento)
            : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {

        }

        public static Aluno Criar(string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, Arquivo? foto, Logradouro logradouro, string numero, string? complemento)
        {
            return new Aluno(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento);
        }

        public override Acesso Entrar()
        {
            try
            {
                var registro = Acesso.Criar(this, DateTime.Now, ETipoPessoa.Aluno);
                return registro;
            }
            catch (DomainException ex)
            {
                throw new DomainException("Erro ao registrar entrada: " + ex.Message);
            }
        }

        public override Acesso Sair()
        {
            try
            {
                var registro = Acesso.Criar(this, DateTime.Now, ETipoPessoa.Aluno);
                return registro;
            }
            catch (DomainException ex)
            {
                throw new DomainException("Erro ao registrar entrada: " + ex.Message);
            }
        }

        public string GetTempoPermanencia(DateTime Inicio, DateTime FIm)
        {
            throw new NotImplementedException("Método GetTempoPermanencia não implementado.");
        }

        public string GetTempoContrato()
        {
            throw new NotImplementedException("Método GetTempoContrato não implementado.");
        }

        public void TrocarSenha(string senhaAtual, string novaSenha)
        {
            if (string.IsNullOrWhiteSpace(senhaAtual))
                throw new DomainException("Senha atual não pode ser vazia.");
            if (string.IsNullOrWhiteSpace(novaSenha))
                throw new DomainException("Nova senha não pode ser vazia.");
            if (senhaAtual != Senha)
                throw new DomainException("Senha atual está incorreta.");
            Senha = novaSenha;
        }
    }
}