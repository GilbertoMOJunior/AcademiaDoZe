namespace AcademiaDoZe.Domain
{
    public abstract class Pessoa : Entity
    {
        public Pessoa(string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, string? foto, Logradouro logradouro,
            string numero, string? complemento)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF não pode ser vazio.", nameof(cpf));
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));
            if (dataNascimento == default)
                throw new ArgumentException("Data de nascimento não pode ser nula.", nameof(dataNascimento));
            if (string.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Telefone não pode ser vazio.", nameof(telefone));
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(senha));
            if (logradouro is null)
                throw new ArgumentNullException(nameof(logradouro), "Logradouro não pode ser nulo.");
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Número não pode ser vazio.", nameof(numero));

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
        protected string Senha { get; set; }
        public string? Foto { get; set; }
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

        public virtual void Entrar()
        {
            var registro = new Catraca(this, DateTime.Now);
        }

        public virtual void Sair()
        {
            var registro = new Catraca(this, DateTime.Now);
        }
    }
}
