using AcademiaDoZe.Domain.Exeption;
using AcademiaDoZe.Domain.Service;

namespace AcademiaDoZe.Domain
{
    public sealed class Aluno : Pessoa
    {
        private Aluno(int id, string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, Arquivo? foto, Logradouro logradouro, string numero, string? complemento)
            : base(cpf, nome, dataNascimento, email, telefone, senha, foto, logradouro, numero, complemento)
        {

        }

        public static Aluno Criar(int id, string cpf, string nome, DateOnly dataNascimento, string? email, string telefone,
            string senha, Arquivo? foto, Logradouro endereco, string numero, string? complemento)
        {

            nome = NormalizadoService.LimparEspacos(nome);
            if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");
            cpf = NormalizadoService.LimparEDigitos(cpf);
            if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");
            if (dataNascimento == default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIO");
            if (dataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-12))) throw new DomainException("DATA_NASCIMENTO_MINIMA_INVALIDA");
            if (string.IsNullOrWhiteSpace(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
            telefone = NormalizadoService.LimparEDigitos(telefone);
            if (telefone.Length != 11) throw new DomainException("TELEFONE_DIGITOS");
            email = NormalizadoService.LimparEspacos(email);
            if (NormalizadoService.ValidarFormatoEmail(email)) throw new DomainException("EMAIL_FORMATO");
            if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("SENHA_OBRIGATORIO");
            senha = NormalizadoService.LimparEspacos(senha);
            if (NormalizadoService.ValidarFormatoSenha(senha)) throw new DomainException("SENHA_FORMATO");
            if (foto == null) throw new DomainException("FOTO_OBRIGATORIO");
            if (endereco == null) throw new DomainException("LOGRADOURO_OBRIGATORIO");

            if (string.IsNullOrWhiteSpace(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
            numero = NormalizadoService.LimparEspacos(numero);
            complemento = NormalizadoService.LimparEspacos(complemento);

            return new Aluno(id, cpf, nome, dataNascimento, email, telefone, senha, foto, endereco, numero, complemento);
        }
    }
}