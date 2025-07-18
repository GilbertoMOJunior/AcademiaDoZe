namespace AcademiaDoZe.Domain
{
    public abstract class Pessoa
    {
        public Pessoa(string cpf, string nome, DateOnly dataNascimento, string? email,
            string telefone, string senha, string? foto, Logradouro logradouro,
            string numero, string? complemento)
        {
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
            var registro = new Entrada(this, DateTime.Now);
        }

        public virtual void Sair()
        {
            var registro = new Entrada(this, DateTime.Now);
        }
    }
}
