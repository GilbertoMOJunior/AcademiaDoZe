namespace AcademiaDoZe.Domain
{
    public sealed class Logradouro : Entity
    {
        public Logradouro(string nomeLogradouro, string cEP, string pais, string cidade, string bairro)
        {
            if (string.IsNullOrWhiteSpace(nomeLogradouro))
                throw new ArgumentException("Nome do logradouro não pode ser vazio.", nameof(nomeLogradouro));
            if (string.IsNullOrWhiteSpace(cEP))
                throw new ArgumentException("CEP não pode ser vazio.", nameof(cEP));
            if (string.IsNullOrWhiteSpace(pais))
                throw new ArgumentException("País não pode ser vazio.", nameof(pais));
            if (string.IsNullOrWhiteSpace(cidade))
                throw new ArgumentException("Cidade não pode ser vazia.", nameof(cidade));
            if (string.IsNullOrWhiteSpace(bairro))
                throw new ArgumentException("Bairro não pode ser vazio.", nameof(bairro));

            NomeLogradouro = nomeLogradouro;
            CEP = cEP;
            Pais = pais;
            Cidade = cidade;
            Bairro = bairro;
        }

        public string NomeLogradouro { get; set; }
        public string CEP { get; set; }
        public string Pais { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
    }
}
