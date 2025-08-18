using AcademiaDoZe.Domain.Exeption;

namespace AcademiaDoZe.Domain
{
    public abstract class Pessoa : Entity
    {
        public Pessoa(string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, Arquivo? foto, Logradouro logradouro,
            string numero, string? complemento)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new DomainException("CPF não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome não pode ser vazio.");
            if (dataNascimento == default)
                throw new DomainException("Data de nascimento não pode ser nula.");
            if (string.IsNullOrWhiteSpace(telefone))
                throw new DomainException("Telefone não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(senha))
                throw new DomainException("Senha não pode ser vazia.");
            if (logradouro is null)
                throw new DomainException("Logradouro não pode ser nulo.");
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número não pode ser vazio.");

            Cpf = cpf;
            Nome = nome;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
            Senha = senha;
            Foto = foto;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
        }

        public string Cpf { get; }
        public string Nome { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string? Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public Arquivo? Foto { get; set; }
        public Logradouro Logradouro { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }

        public virtual int Idade()
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            var idade = hoje.Year - DataNascimento.Year;

            if (hoje.Month < DataNascimento.Month || (hoje.Month == DataNascimento.Month && hoje.Day < DataNascimento.Day))
                idade--;

            return idade;
        }

        public virtual Acesso Entrar()
        {
            return Acesso.Criar(this, DateTime.Now, 0);
        }

        public virtual Acesso Sair()
        {
            return Acesso.Criar(this, DateTime.Now, 0);
        }
    }
}
